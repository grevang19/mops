using System.Threading;
using Core.ProcessManager.Process;

namespace Core.Process
{
    public class TimerWrapper : ITimer
    {
        private readonly Timer _timer;
        private readonly int _executionTime;
        private readonly int _tickTime;

        private readonly int _tickNumber;
        public int TickNumber
        {
            get { return _tickNumber; }
        }

        private int _tickState;
        public int TickState
        {
            get { return _tickState; }
        }

        public event TimerEvent OnTimerTick;

        public TimerWrapper(int executionTime, int tickNumber)
        {
            _executionTime = executionTime;
            _tickNumber = tickNumber;

            _tickTime = _executionTime/tickNumber;
            _timer = new Timer(TimerTickCallback, null, Timeout.Infinite, _tickTime);
        }

        private void TimerTickCallback(object state)
        {
            if(_tickState >= _tickNumber)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                return;
            }

            OnTimerTick(this);
            _tickState++;
        }

        public void Start()
        {
            _timer.Change(0, _tickTime);
        }
    }
}