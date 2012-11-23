using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Core.ProcessManager;
using Core.ProcessManager.Messenger;
using Core.ProcessManager.ProcessLoader;
using Core.ProcessManager.Timer;
using Infrastructure.IocContainer;

namespace View.ConsoleViewer
{
    /// <summary>
    /// Основная форма приложения
    /// </summary>
    public partial class FormMain : Form
    {
        private List<ListViewAction> _listViewActions;
        private ProcessPerformer _processPerformer;

        public FormMain()
        {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);

            this.UpdateStyles();

            InitializeProcessObjects();

            toolStripStatusLabel.Text =
                string.Format("\"Нажмите кнопку {0}\", чтоб загрузить фалы с описанием процессов.",
                              buttonSelect.Text);
        }

        /// <summary>
        /// Инициализация основных объектов, обеспечивающих обработку процессов
        /// </summary>
        private void InitializeProcessObjects()
        {
            //IoC-контейнер
            var container = ContainersFactory.GetContainer();
            //Оповещатель
            var messenger = container.Resolve<IMessenger>();
            //Загрузчик процессов
            var processLoader = container.Resolve<IProcessLoader>();
            var timer = container.Resolve<ITimer>();

            _processPerformer = new ProcessPerformer(messenger, timer, processLoader);

            messenger.OnMessengerStateChange += MessengerStateChanged;
        }

        /// <summary>
        /// Задает последовательность действий, выполняемых при изменении состояния
        /// любого Action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MessengerStateChanged(IMessenger sender, MessengerEventsArgs args)
        {
            if (listView.InvokeRequired)
            {
                listView.Invoke(new MessengerEventHandler(MessengerStateChanged), new object[] {sender, args});
            }
            else
            {
                if (!IsStartOrEnd(args.ActionName))
                    WriteToLog(args.ActionName + " \n");

                foreach (ListViewItem item in listView.Items)
                {
                    if (item.Text != args.ActionName)
                    {
                        continue;
                    }
                    // Изменение статусов и подписей на форме
                    item.SubItems[1].Text = args.ActionState;
                    item.SubItems[2].Text = string.Format("{0}%", args.ActionProgress);

                    if (args.ActionProgress == 100)
                    {
                        item.ImageKey = "isReady"; //ставит новую картинку возде экшена
                    }
                }
            }
        }

        #region ListView preparations
        /// <summary>
        /// Подготовка списка для наполнения элемента ListViewListView
        /// </summary>
        /// <param name="processPerformer">Используемый обработчик процессов</param>
        private void PrepareListView(ProcessPerformer processPerformer)
        {
            _listViewActions = new List<ListViewAction>();

            listView.Clear();
            listView.Groups.Clear();

            CreateGroups(processPerformer);
            CreateListViewColumns();
            FillListView();
        }

        /// <summary>
        /// Группировка действий по процессам
        /// </summary>
        /// <param name="processPerformer"></param>
        private void CreateGroups(ProcessPerformer processPerformer)
        {
            if (!processPerformer.IsValid)
            {
                return;
            }

            foreach (var process in processPerformer.Processes)
            {
                listView.Groups.Add(new ListViewGroup(process.Name, process.Name));
                foreach (var action in process.Actions)
                {
                    if (IsStartOrEnd(action.Name)) continue;
                    _listViewActions.Add(
                        new ListViewAction(action.Name, process.Name));
                }
            }
        }

        /// <summary>
        /// Заполнение элемента ListView на форме списком процессов и действий
        /// </summary>
        private void FillListView()
        {
            for (var i = 0; i < _listViewActions.Count; i++)
            {
                var listViewAction = _listViewActions[i];

                var item = new ListViewItem
                               {
                                   Group = listView.Groups[listViewAction.Group],
                                   Text = listViewAction.Name,
                                   ImageKey = "inProcess" //ставит картинку возде экшена
                               };

                item.SubItems.Add(listViewAction.State);
                item.SubItems.Add(string.Format("{0}%", listViewAction.Progress));

                listView.Items.Add(item);
            }
        }

        /// <summary>
        /// Задание столбцоа в ListView
        /// </summary>
        private void CreateListViewColumns()
        {
            listView.View = System.Windows.Forms.View.Details;
            listView.Columns.Add("Название операции", 180);
            listView.Columns.Add("Состояние", 220);
            listView.Columns.Add("Прогресс", 70);
            listView.Items.Clear();
        }

        #endregion
        /// <summary>
        /// Запись сообщений в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        private void WriteToLog(string message)
        {
            textBoxLog.AppendText(string.Format("Системное время: {0:hh:mm:ss:ff}. Событие: {1}", DateTime.Now, message));
        }

        private static bool IsStartOrEnd(string name)
        {
            return name == "Start" || name == "End";
        }

        private void ToolStripButtonClick(object sender, EventArgs e)
        {
            if (_processPerformer.IsValid)
            {
                _processPerformer.ExecuteProcesses();
                toolStripStatusLabel.Text = "Выполнение процессов...";
            }
        }

        /// <summary>
        /// Выбор папки с файлом - списком процессов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSelectClick(object sender, EventArgs e)
        {
            InitializeProcessObjects();

            var selectedPath = Directory.GetCurrentDirectory();
            var folderBrowserDialog = new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();

           if (result == DialogResult.OK)
           {
               selectedPath = folderBrowserDialog.SelectedPath;
           }

            LoadProcesses(selectedPath);
        }

        /// <summary>
        /// Выгрузка списков процессов из файла
        /// </summary>
        /// <param name="selectedPath"></param>
        private void LoadProcesses(string selectedPath)
        {
            _processPerformer.LoadProcesses(selectedPath);
            PrepareListView(_processPerformer);

            toolStripStatusLabel.Text = "Успешно загружены процессы: ";
            foreach (var process in _processPerformer.Processes)
            {
                toolStripStatusLabel.Text += string.Format("{0} ", process.Name);
            }
        }

        /// <summary>
        /// Вызов отображения окна "О программе"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButtonHelpClick(object sender, EventArgs e)
        {
            var formAbout = new FormAbout();
            formAbout.ShowDialog(this);
        }
    }
}