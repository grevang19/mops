using System.Collections.Generic;
using System.Threading;
using Core.ProcessManager.Messenger;
using Core.ProcessManager.Process;
using Core.ProcessManager.Timer;

namespace Core.Process
{
    /// <summary>
    /// Действияе процесса.
    /// </summary>
    public class StandartAction : IAction
    {
        /// <summary>
        /// Название действия.
        /// </summary>
        public string Name { get; set; } // Название действия

        /// <summary>
        /// Сообщение, которое выводится при начале выполнения действия.
        /// </summary>
        public string InitialMessage { get; set; }

        /// <summary>
        /// Сообщение, которое выводится после завершения выполнения действия.
        /// </summary>
        public string FinalMessage { get; set; }

        /// <summary>
        /// Время выполнения действия.
        /// </summary>
        public int ExecutionTime { get; set; }

        /// <summary>
        /// Ссылка на родительский процесс.
        /// </summary>
        public IProcess Process { get; set; }

        private List<string> _parentActions = new List<string>();

        /// <summary>
        /// Список названий родительских действий.
        /// </summary>
        public List<string> ParentActions
        {
            get { return _parentActions; }
            set { _parentActions = value; }
        }

        private List<string> _childActions = new List<string>();

        /// <summary>
        /// Список названий дочерних действий.
        /// </summary>
        public List<string> ChildActions
        {
            get { return _childActions; }
            set { _childActions = value; }
        }

        /// <summary>
        /// Запускает выполнение данного действия, а после его завершения пытается зарустить дочерние.
        /// </summary>
        public void Execute()
        {
            var messenger = (IMessenger) Process.Messeger.Clone();
            var timer = (ITimer) Process.Timer.Clone();

            timer.ExecutionTime = ExecutionTime;
            timer.TickNumber = 20;

            //Выполнение действия
            messenger.MessengerState = messenger.CreateNewState(Name, InitialMessage, 0);
            
            timer.OnTimerTick += messenger.ChangeProgressStatus;
            timer.Start(TryExecuteChildActions, messenger);
        }

        private void TryExecuteChildActions(IMessenger messenger)
        {
            messenger.ChangeStateMessage(FinalMessage);

            //Определение потока для дочерних действий, 
            var threads = new List<Thread>();

            foreach (var childActionName in ChildActions)
            {
                // Удаление ссылки на себя из их списков;
                var childAction = Process.GetByName(childActionName);
                if (childAction == null) continue;
                childAction.ParentActions.Remove(Name);

                //Определение стартовых методов для выполнения;
                if (childAction.ParentActions.Count == 0)
                    threads.Add(new Thread(childAction.Execute));
            }
            // Запуск дочерних действий в процессе;
            threads.ForEach(t => t.Start()); 
        }
    }
}