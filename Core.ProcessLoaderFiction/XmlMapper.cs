using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Core.ProcessManager.Process;
using Core.ProcessManager.ProcessLoader;
using Infrastructure.IocContainer;

namespace Core.ProcessLoaderFiction
{
    /// <summary>
    /// Класс, обеспечивающий импорт/экспорт списков процессов в формате XML
    /// </summary>
    public class XmlMapper : IMapper
    {
        private readonly XmlDocument _xmlDocument;
        private readonly IContainer _container;

        public XmlMapper()
        {
            _xmlDocument = new XmlDocument();
            _container = ContainersFactory.GetContainer();
        }

        /// <summary>
        /// Экспорт списка процессов в файл формата XML
        /// </summary>
        /// <param name="processes">Список процессов</param>
        public void Export(IEnumerable<IProcess> processes)
        {
            _xmlDocument.RemoveAll();
            var root = _xmlDocument.CreateElement("Processes");

            // Создание структуры XML, отражающей структуру класса Process
            foreach (var process in processes)
            {
                var procNode = _xmlDocument.CreateElement("Process");
                var nameNode = _xmlDocument.CreateElement("Name");
                var actsNode = _xmlDocument.CreateElement("Actions");

                // Привязка значений полей и свойств процесса соостветствующим полям документа
                nameNode.InnerText = process.Name;
                procNode.AppendChild(nameNode);
                
                Actions2Xml(actsNode, process.Actions);
                procNode.AppendChild(actsNode);

                root.AppendChild(procNode);
                _xmlDocument.AppendChild(root);

                try
                {
                    _xmlDocument.Save(string.Format("{0}.xml", process.Name));
                }
                catch (Exception exception)
                {
                    throw new IOException("Some troubles: " + exception.Message);
                }
            }
        }

        /// <summary>
        /// Добавляет в структуру документа описания действий, содержащихся в процессе
        /// </summary>
        /// <param name="actsNode">Узел-отражение списка действий в процессе</param>
        /// <param name="actions">Список действий в процессе</param>
        private void Actions2Xml(XmlNode actsNode, IEnumerable<IAction> actions)
        {
            foreach (var action in actions)
            {
                var actNode = _xmlDocument.CreateElement("Action");
                var nameNode = _xmlDocument.CreateElement("Name");
                var initNode = _xmlDocument.CreateElement("InitialMessage");
                var finalNode = _xmlDocument.CreateElement("FinalMessage");
                var timeNode = _xmlDocument.CreateElement("ExecutionTime");
                var procNode = _xmlDocument.CreateElement("Process");
                var parActNode = _xmlDocument.CreateElement("ParentActions");
                var childActNode = _xmlDocument.CreateElement("ChildActions");

                nameNode.InnerText = action.Name;
                initNode.InnerText = action.InitialMessage;
                finalNode.InnerText = action.FinalMessage;
                timeNode.InnerText = action.ExecutionTime.ToString();
                procNode.InnerText = action.Process != null ? action.Process.Name : String.Empty;

                if (action.ParentActions.Count != 0)
                    foreach (var parAction in action.ParentActions)
                    {
                        var pActNode = _xmlDocument.CreateElement("ParentAction");
                        pActNode.InnerText = parAction;
                        pActNode.AppendChild(pActNode);
                    }

                if (action.ChildActions.Count != 0)
                    foreach (var chlAction in action.ChildActions)
                    {
                        var cActNode = _xmlDocument.CreateElement("ChildAction");
                        cActNode.InnerText = chlAction;
                        parActNode.AppendChild(cActNode);
                    }

                actNode.AppendChild(nameNode);
                actNode.AppendChild(initNode);
                actNode.AppendChild(finalNode);
                actNode.AppendChild(timeNode);
                actNode.AppendChild(procNode);
                actNode.AppendChild(parActNode);
                actNode.AppendChild(childActNode);
                actsNode.AppendChild(actNode);
            }
            return;
        }

        /// <summary>
        ///     Импорт списка процессов из файла формата XML
        /// </summary>
        /// <param name="path">
        ///     Путь к папке с файлами, содержащими списки процессов
        /// </param>
        public List<IProcess> Import(string path)
        {
            var processes = new List<IProcess>();
            var files = Directory.EnumerateFiles(path, "*.xml");

            if (files.Count() == 0) return processes;

            foreach (var file in files)
            {
                try
                {
                    _xmlDocument.RemoveAll();
                    _xmlDocument.Load(file);
                }
                catch (Exception exception)
                {
                    throw new IOException("Some troubles: " + exception.Message);
                }

                if (_xmlDocument.DocumentElement == null) continue;
                
                var roots = _xmlDocument.DocumentElement.ChildNodes;
               if (roots.Count == 0) break;

                foreach(var procNode in from XmlNode procNode in roots where procNode.Name == "Process" select  procNode)
                {
                    var process = _container.Resolve<IProcess>();
                    var nameNodes  = from XmlNode xName in procNode.ChildNodes
                                     where xName.Name == "Name"
                                     select xName.InnerText;

                    // Привязка значений полей документа соостветствующим  полям и свойствам процесса
                    process.Name = nameNodes.FirstOrDefault();

                    process = Xml2Actions(procNode, process);
                    processes.Add(process);
                }
            }            
            return processes;
        }

        /// <summary>
        /// Импорт списка действий из файла формата XML в процесс
        /// </summary>
        /// <param name="procNode"> XML-узел, содержащий в себе процесс</param>
        /// <param name="process">Процесс, в который будут помещены извлеченные действия</param>
        private IProcess Xml2Actions(XmlNode procNode, IProcess process)
        {
            //Выбор узла-списка действий
            var actNodes = from XmlNode xActs in procNode.ChildNodes
                           where xActs.Name == "Actions"
                           from XmlNode xAct in xActs.ChildNodes
                           where xAct.Name == "Action"
                           select xAct;

            //Поиск и присвоение значений действиям из значений в файле
            foreach (var actNode in actNodes)
            {
                ParseAction(actNode);

                // Добавление действия в список в процессе
                process.Actions.Add(ParseAction(actNode));
            }
            return process;
        }

        /// <summary>
        /// Конвертация XML-текста в объект типа IAction
        /// </summary>
        /// <param name="actNode"></param>
        /// <returns></returns>
        private IAction ParseAction(XmlNode actNode)
        {
            var action = _container.Resolve<IAction>();

            action.Name = (from XmlNode xName in actNode.ChildNodes
                           where xName.Name == "Name"
                           select xName.InnerText).FirstOrDefault();
            action.InitialMessage = (from XmlNode xInMsg in actNode.ChildNodes
                                     where xInMsg.Name == "InitialMessage"
                                     select xInMsg.InnerText).FirstOrDefault();
            action.FinalMessage = (from XmlNode xFinMsg in actNode.ChildNodes
                                   where xFinMsg.Name == "FinalMessage"
                                   select xFinMsg.InnerText).FirstOrDefault();
            action.ExecutionTime = Convert.ToInt32((from XmlNode xTime in actNode.ChildNodes
                                                    where xTime.Name == "ExecutionTime"
                                                    select xTime.InnerText).FirstOrDefault());
            action.ParentActions.AddRange(from XmlNode xParActs in actNode.ChildNodes
                                          where xParActs.Name == "ParentActions"
                                          from XmlNode xParAct in xParActs
                                          where xParAct.Name == "ParentAction"
                                          select xParAct.InnerText);
            action.ChildActions.AddRange(from XmlNode xChildActs in actNode.ChildNodes
                                         where xChildActs.Name == "ChildActions"
                                         from XmlNode xChildAct in xChildActs
                                         where xChildAct.Name == "ChildAction"
                                         select xChildAct.InnerText);
            return action;
        }
    }
}