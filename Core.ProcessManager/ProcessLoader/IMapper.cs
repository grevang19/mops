using System.Collections.Generic;
using Core.ProcessManager.Process;

namespace Core.ProcessManager.ProcessLoader
{
    /// <summary>
    /// Интерфейс, обеспечивающий импорт/экспорт списка процессов
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Производит экспорт процессов в Xml файлы.
        /// </summary>
        /// <param name="processes">Список процессов.</param>
        void Export(IEnumerable<IProcess> processes);

        /// <summary>
        /// Выполняет импорт процессов из файлов.
        /// </summary>
        /// <param name="path">Путь к папке с файлами процессов.</param>
        /// <returns>Возвращает список процессов.</returns>
        List<IProcess> Import(string path);
    }
}