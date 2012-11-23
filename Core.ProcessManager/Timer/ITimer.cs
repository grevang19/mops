using System;
using Core.ProcessManager.Messenger;

namespace Core.ProcessManager.Timer
{
    /// <summary>
    /// Выполняет отсчет времени выполнения действия, таким образом моделирует его выполнение.
    /// </summary>
    public interface ITimer : ICloneable
    {
        /// <summary>
        /// Время выполнения действия.
        /// </summary>
        int ExecutionTime { get; set; }

        /// <summary>
        /// Время одного тика таймера, после которого будет вызвано событие OnTimerTick.
        /// </summary>
        int TickNumber { get; set; }

        /// <summary>
        /// Количество тиков, прощедших после запуска таймера.
        /// </summary>
        int TickState { get; }

        /// <summary>
        /// Событие которое генерируется после каждого тика таймера.
        /// </summary>
        event TimerEvent OnTimerTick;

        /// <summary>
        /// Запуск таймера.
        /// </summary>
        void Start();

        /// <summary>
        /// Запускает таймер с указанием функции, которая запустится после остановки таймера.
        /// </summary>
        /// <param name="callbackFunction">Функция, которая запустится после остановки таймера.</param>
        /// <param name="messenger">Мессенджер, который будет нотифицировать процесс.</param>
        void Start(Action<IMessenger> callbackFunction, IMessenger messenger);
    }
}