using System.Collections.Generic;
using Core.ProcessManager.Process;
using Core.ProcessManager.ProcessLoader;
using Infrastructure.IocContainer;

namespace Core.ProcessLoaderFiction
{
    /// <summary>
    /// Выполняет загрузку из хранилища данных о выполняемых процессах.
    /// </summary>
    public class ProcessLoader : IProcessLoader
    {
        /// <summary>
        /// Загружает из хранилища данные о выполняемых процессах.
        /// </summary>
        /// <returns>Список процессов.</returns>
        public List<IProcess> LoadAll(string path)
        {
            var container = ContainersFactory.GetContainer();
            var mapper = container.Resolve<IMapper>();
            return mapper.Import(path);
        }
    }
}