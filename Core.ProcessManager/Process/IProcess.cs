using System.Collections.Generic;
using Core.ProcessManager.Messenger;
using Core.ProcessManager.Timer;

namespace Core.ProcessManager.Process
{
    /// <summary>
    /// Процесс.
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// Таймер для выполнения действия.
        /// </summary>
        ITimer Timer { get; }

        /// <summary>
        /// Мессенджер, который выполняет нотирование выполнения действия.
        /// </summary>
        IMessenger Messeger { get; }

        /// <summary>
        /// Название процесса.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Действия процесса.
        /// </summary>
        List<IAction> Actions { get; set; }

        /// <summary>
        /// Поиск начального действия в процессе;
        /// </summary>
        /// <returns>Действие, которое означает старт процесса</returns>
        IAction GetInitialAction();

        /// <summary>
        /// Поиск именованного действия в процессе;
        /// </summary>
        /// <param name="name">Имя действия</param>
        /// <returns>Действие, имя которого задано.</returns>
        IAction GetByName(string name);

        /// <summary>
        /// Запуск процесса
        /// </summary>
        /// <param name="messenger">Оповещатель.</param>
        /// <param name="timer">Таймер, моделирующий процесс выполнения действия.</param>
        void StartProcess(IMessenger messenger, ITimer timer);
    }
}