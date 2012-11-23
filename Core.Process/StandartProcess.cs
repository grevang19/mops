using System;
using System.Collections.Generic;
using System.Linq;
using Core.ProcessManager.Messenger;
using Core.ProcessManager.Process;
using Core.ProcessManager.Timer;

namespace Core.Process
{
    /// <summary>
    /// Процесс.
    /// </summary>
    public class StandartProcess : IProcess
    {
        private ITimer _timer;

        /// <summary>
        /// Таймер для выполнения действия.
        /// </summary>
        public ITimer Timer
        {
            get { return _timer; }
        }

        private IMessenger _messeger;

        /// <summary>
        /// Мессенджер, который выполняет нотирование выполнения действия.
        /// </summary>
        public IMessenger Messeger
        {
            get { return _messeger; }
        }

        /// <summary>
        /// Название процесса.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список действий внутри процесса;
        /// </summary>
        private List<IAction> _actions = new List<IAction>();

        /// <summary>
        /// Действия процесса.
        /// </summary>
        public List<IAction> Actions
        {
            get { return _actions; }
            set { _actions = value; }
        }

        /// <summary>
        /// Поиск начального действия в процессе;
        /// </summary>
        /// <returns>Действие, которое означает старт процесса</returns>
        public IAction GetInitialAction()
        {
            var initialAction = Actions.Where(a => a.Name == "Start").FirstOrDefault();

            if (initialAction == null) throw new MissingMemberException("Can't find a start Action.");

            return initialAction;
        }

        /// <summary>
        /// Поиск именованного действия в процессе;
        /// </summary>
        /// <param name="name">Имя действия</param>
        /// <returns>Действие, имя которого задано.</returns>
        public IAction GetByName(string name)
        {
            return Actions
                .Where(a => a.Name == name)
                .FirstOrDefault();
        }

        /// <summary>
        /// Запуск процесса
        /// </summary>
        /// <param name="messenger">Оповещатель.</param>
        public void StartProcess(IMessenger messenger, ITimer timer)
        {
            if (messenger == null || timer == null)
                throw new ArgumentNullException(string.Format("messenger or timer is null"));

            _messeger = messenger;
            _timer = timer;

            foreach (var action in Actions)
            {
                action.Process = this;
            }

            GetInitialAction().Execute();
        }
    }
}