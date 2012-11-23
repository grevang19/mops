namespace View.ConsoleViewer
{
    internal class ListViewAction
    {
        /// <summary>
        /// Нзвание группы, к которой принадлежит данное действие.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Название действия.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Процентное представление прогресса выполнения действия.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Состояние выполнения действия.
        /// </summary>
        private string _state;
        public string State
        {
            get { return string.IsNullOrEmpty(_state) ? "Не задано" : _state; }
            set { _state = value; }
        }

        public ListViewAction(string name, string group)
        {
            Group = group;
            Name = name;
        }
    }
}