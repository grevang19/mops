using System.Collections.Generic;

namespace Core.ProcessManager.Process
{
    /// <summary>
    /// Действияе процесса.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Название действия.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Сообщение, которое выводится при начале выполнения действия.
        /// </summary>
        string InitialMessage { get; set; }
        
        /// <summary>
        /// Сообщение, которое выводится после завершения выполнения действия.
        /// </summary>
        string FinalMessage { get; set; }

        /// <summary>
        /// Время выполнения действия.
        /// </summary>
        int ExecutionTime { get; set; }

        /// <summary>
        /// Список названий родительских действий.
        /// </summary>
        List<string> ParentActions { get; set; }

        /// <summary>
        /// Список названий дочерних действий.
        /// </summary>
        List<string> ChildActions { get; set; }

        /// <summary>
        /// Ссылка на родительский процесс.
        /// </summary>
        IProcess Process { get; set; }

        /// <summary>
        /// Запускает выполнение данного действия, а после его завершения пытается зарустить дочерние.
        /// </summary>
        void Execute();
    }
}