using Core.ProcessManager.Messenger;
using Core.ProcessManager.Timer;

namespace Core.Messenger
{
    /// <summary>
    /// Выполняет нонифицирование выполнения действия.
    /// </summary>
    public class ClassicMessenger : IMessenger
    {
        /// <summary>
        /// Событие, которое генирируется при изменении свойства MessengerState.
        /// </summary>
        public event MessengerEventHandler OnMessengerStateChange;

        private MessengerEventsArgs _messengerState;

        /// <summary>
        /// Хранит состояние действия.
        /// </summary>
        public MessengerEventsArgs MessengerState
        {
            get { return _messengerState; }
            set
            {
                _messengerState = value;
                OnMessengerStateChange(this, _messengerState);
            }
        }

        /// <summary>
        /// Изменяет прогресс выполнения текущего действия.
        /// </summary>
        /// <param name="sender">Источник изменения прогресса действия.</param>
        public void ChangeProgressStatus(ITimer sender)
        {
            var newMessengerEventsArgs = _messengerState;
            newMessengerEventsArgs.ActionProgress = GetPercent(sender.TickState, sender.TickNumber);
            MessengerState = newMessengerEventsArgs;
        }

        /// <summary>
        /// Изменяет состояние текущего действия.
        /// </summary>
        /// <param name="stateMessage">Новое состояние действия.</param>
        public void ChangeStateMessage(string stateMessage)
        {
            var newMessengerEventsArgs = _messengerState;
            newMessengerEventsArgs.ActionState = stateMessage;
            MessengerState = newMessengerEventsArgs;
        }

        /// <summary>
        /// Считает процент выполнения действия
        /// </summary>
        /// <param name="current">Данное время выполнения</param>
        /// <param name="max">Полное время выполнения</param>
        /// <returns></returns>
        private static int GetPercent(int current, int max)
        {
            var percent = ((double)current / max) * 100;
            return (int)percent;
        }

        /// <summary>
        /// Фабричный метод, возворащающий новый объект состояния действия MessengerEventsArgs.
        /// </summary>
        /// <param name="actionName">Назвение события.</param>
        /// <param name="actionState">Состояние выполнения события.</param>
        /// <param name="actionProgress">Прогресс выполнения события.</param>
        /// <returns>Объект хранящий в себе состояние выполнения действия.</returns>
        public MessengerEventsArgs CreateNewState(string actionName, string actionState, int actionProgress)
        {
            return new MessengerEventsArgs(actionName, actionState, actionProgress);
        }

        public object Clone()
        {
            var newClassicMessenger = new ClassicMessenger();
            newClassicMessenger.OnMessengerStateChange = OnMessengerStateChange;

            return newClassicMessenger;
        }
    }
}
