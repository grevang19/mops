using System;
using System.Windows.Forms;
using Core.ActionLatencyRealization;
using Core.Messenger;
using Core.Process;
using Core.ProcessLoaderFiction;
using Core.ProcessManager.Messenger;
using Core.ProcessManager.Process;
using Core.ProcessManager.ProcessLoader;
using Core.ProcessManager.Timer;
using Infrastructure.IocContainer;

namespace View.ConsoleViewer
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        /// <summary>
        /// Регистрация зависимостей.
        /// </summary>
        private static void Initialize()
        {
            var container = ContainersFactory.GetContainer();

            container.RegisterType<IProcessLoader, ProcessLoader>();
            container.RegisterType<IMessenger, ClassicMessenger>();
            container.RegisterType<ITimer, TimerWrapper>();
            container.RegisterType<IProcess, StandartProcess>();
            container.RegisterType<IAction, StandartAction>();
            container.RegisterType<IMapper, XmlMapper>();
        }
    }
}
