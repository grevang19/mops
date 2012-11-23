using System.Collections.Generic;
using Core.ProcessManager.Process;

namespace Core.ProcessManager.ProcessLoader
{
    /// <summary>
    /// Выполняет загрузку из хранилища данных о выполняемых процессах.
    /// </summary>
    public interface IProcessLoader
    {
        /// <summary>
        /// Загружает из хранилища данные о выполняемых процессах.
        /// </summary>
        /// <returns>Список процессов.</returns>
        List<IProcess> LoadAll(string path);
    }
}
