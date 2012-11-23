using System;
using Core.ProcessManager.Timer;

namespace Core.ProcessManager.Messenger
{
    /// <summary>
    /// Выполняет нонифицирование выполнения действия.
    /// </summary>
    public interface IMessenger : ICloneable
    {
        /// <summary>
        /// Событие, которое генирируется при изменении свойства MessengerState.
        /// </summary>
        event MessengerEventHandler OnMessengerStateChange;

        /// <summary>
        /// Хранит состояние действия.
        /// </summary>
        MessengerEventsArgs MessengerState { set; get; }

        /// <summary>
        /// Фабричный метод, возворащающий новый объект состояния действия MessengerEventsArgs.
        /// </summary>
        /// <param name="actionName">Назвение события.</param>
        /// <param name="actionState">Состояние выполнения события.</param>
        /// <param name="actionProgress">Прогресс выполнения события.</param>
        /// <returns>Объект хранящий в себе состояние выполнения действия.</returns>
        MessengerEventsArgs CreateNewState(string actionName, string actionState, int actionProgress);

        /// <summary>
        /// Изменяет прогресс выполнения текущего действия.
        /// </summary>
        /// <param name="sender">Источник изменения прогресса действия.</param>
        void ChangeProgressStatus(ITimer sender);

        /// <summary>
        /// Изменяет состояние текущего действия.
        /// </summary>
        /// <param name="stateMessage">Новое состояние действия.</param>
        void ChangeStateMessage(string stateMessage);
    }

    /// <summary>
    /// Делегат для события OnMessengerStateChange, интерфейса IMessenger.
    /// </summary>
    /// <param name="sender">Объект-нотификатор выполнения действия.</param>
    /// <param name="args">Параметры состояния действия.</param>
    public delegate void MessengerEventHandler(IMessenger sender, MessengerEventsArgs args);
}
