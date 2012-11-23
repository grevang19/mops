namespace Core.ProcessManager.Messenger
{
    /// <summary>
    /// Хранит состояние выполнения действия.
    /// </summary>
    public class MessengerEventsArgs
    {
        /// <summary>
        /// Название действия.
        /// </summary>
        public string ActionName { set; get; }

        /// <summary>
        /// Состояние действия.
        /// </summary>
        public string ActionState { set; get; }

        /// <summary>
        /// Прогресс выполнения действия.
        /// </summary>
        public int ActionProgress { set; get; }

        public MessengerEventsArgs(string actionName, string actionState, int actionProgress)
        {
            ActionName = actionName;
            ActionState = actionState;
            ActionProgress = actionProgress;
        }
    }
}