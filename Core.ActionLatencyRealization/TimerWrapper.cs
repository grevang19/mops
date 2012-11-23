using System;
using System.Threading;
using System.Timers;
using Core.ProcessManager.Messenger;
using Core.ProcessManager.Timer;
using Timer = System.Timers.Timer;

namespace Core.ActionLatencyRealization
{
    public interface ITimerWrapper
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

        /// <summary>
        /// Создает клон текущего объекта.
        /// </summary>
        /// <returns>Клон текущего объекта.</returns>
        object Clone();
    }

    /// <summary>
    /// Класс-обертка для класса Timer. реализующий необходимый функционал.
    /// </summary>
    public class TimerWrapper : ITimer, ITimerWrapper
    {
        private readonly Timer _timer;
        private int _tickTime; // Период таймера

        private Action<IMessenger> _callbackFunction; //function, which will started after last Timer TICK
        private IMessenger _messenger; //messenger instance for callback function

        private int _executionTime;
        /// <summary>
        /// Время выполнения действия.
        /// </summary>
        public int ExecutionTime
        {
            get { return _executionTime; }
            set { _executionTime = value; }
        }

        private int _tickNumber;
        /// <summary>
        /// Время одного тика таймера, после которого будет вызвано событие OnTimerTick.
        /// </summary>
        public int TickNumber
        {
            get { return _tickNumber; }
            set { _tickNumber = value; }
        }

        private int _tickState;
        /// <summary>
        /// Количество тиков, прощедших после запуска таймера.
        /// </summary>
        public int TickState
        {
            get { return _tickState; }
        }

        /// <summary>
        /// Событие которое генерируется после каждого тика таймера.
        /// </summary>
        public event TimerEvent OnTimerTick;

        public TimerWrapper()
        {
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += TimerTickCallback;
        }

        /// <summary>
        /// Остановка таймера при выполнении действия;
        /// </summary>
        /// <param name="state"></param>
        private void TimerTickCallback(object state, ElapsedEventArgs elapsedEventArgs)
        {
            if (_tickState >= _tickNumber)
            {
                _timer.Stop();
                _callbackFunction.Invoke(_messenger);
                return;
            }

            _tickState++;
            OnTimerTick(this);
        }

        /// <summary>
        /// Запуск таймера.
        /// </summary>
        public void Start()
        {
            _tickTime = _executionTime / _tickNumber;

            if(_tickTime == 0)
            {
                _callbackFunction.Invoke(_messenger);
                return;
            }

            _timer.Interval = _tickTime;
            _timer.Start();
            /*for (var i = 0; i <= _tickNumber; i++)
            {
                //Thread.Sleep(_tickTime);
                TimerTickCallback();
            }*/
        }

        /// <summary>
        /// Запускает таймер с указанием функции, которая запустится после остановки таймера.
        /// </summary>
        /// <param name="callbackFunction">Функция, которая запустится после остановки таймера.</param>
        /// <param name="messenger">Мессенджер, который будет нотифицировать процесс.</param>
        public void Start(Action<IMessenger> callbackFunction, IMessenger messenger)
        {
            _callbackFunction = callbackFunction;
            _messenger = messenger;

            Start();
        }

        /// <summary>
        /// Создает клон текущего объекта.
        /// </summary>
        /// <returns>Клон текущего объекта.</returns>
        public object Clone()
        {
            return new TimerWrapper
                       {
                           ExecutionTime = _executionTime,
                           TickNumber = _tickNumber
                       };
        }
    }
}