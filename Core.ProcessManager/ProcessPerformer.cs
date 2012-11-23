using System.Collections.Generic;
using Core.ProcessManager.Messenger;
using Core.ProcessManager.Process;
using Core.ProcessManager.ProcessLoader;
using Core.ProcessManager.Timer;

namespace Core.ProcessManager
{
    /// <summary>
    /// Выполняет управление загрузкой и запуском процессов.
    /// </summary>
    public class ProcessPerformer
    {
        private readonly IMessenger _messenger;
        private readonly ITimer _timer;
        private readonly IProcessLoader _processLoader;
        private readonly List<IProcess> _processes;

        /// <summary>
        /// Список процессов.
        /// </summary>
        public IEnumerable<IProcess> Processes
        {
            get { return _processes; }
        }

        public ProcessPerformer(IMessenger messenger, ITimer timer, IProcessLoader processLoader)
        {
            _messenger = messenger;
            _timer = timer;
            _processLoader = processLoader;

            _processes = new List<IProcess>();
        }

        /// <summary>
        /// Возвращает true, если коллекция Processes содержит хоть один процесс.
        /// </summary>
        public bool IsValid
        {
            get { return _processes.Count > 0; }
        }

        /// <summary>
        /// ЗАпускает поочередное выполнение процессов.
        /// </summary>
        public void ExecuteProcesses()
        {
            foreach (var process in _processes)
            {
                process.StartProcess(_messenger, _timer);
            }
        }

        /// <summary>
        /// Выполняет загрузку данных о процессах из фалов.
        /// </summary>
        /// <param name="path">ПУть к папке с файлами.</param>
        public void LoadProcesses(string path)
        {
            _processes.AddRange(_processLoader.LoadAll(path));
        }
    }
}