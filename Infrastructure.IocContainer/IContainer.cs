using System.Collections.Generic;

namespace Infrastructure.IocContainer
{
    /// <summary>
    /// Интерфейс длоступа к IOC контейнеру
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Извлекает из контейнера инстанс.
        /// </summary>
        /// <typeparam name="T">Тип извлекаемого объекта.</typeparam>
        /// <returns>Объект типа Т.</returns>
        T Resolve<T>();

        /// <summary>
        /// Извлекает из контейнера инстанс.
        /// </summary>
        /// <typeparam name="T">Тип извлекаемого объекта.</typeparam>
        /// <param name="key">Ключ.</param>
        /// <returns>Объект типа Т.</returns>
        T Resolve<T>(string key);

        /// <summary>
        /// Извлекает из контейнера инстанс.
        /// </summary>
        /// <typeparam name="T">Тип извлекаемого объекта.</typeparam>
        /// <param name="parameters">Список параметров, которые будут переданы в констркуктор.</param>
        /// <returns>Объект типа Т.</returns>
        T Resolve<T>(IDictionary<string, object> parameters);

        /// <summary>
        /// Регистрирует новый тип в контейнере.
        /// </summary>
        /// <typeparam name="T">Тип соответствия.</typeparam>
        /// <typeparam name="R">Тип соответствия.</typeparam>
        void RegisterType<T, R>() where R : T;

        /// <summary>
        /// Регистрирует новый тип в контейнере.
        /// </summary>
        /// <typeparam name="T">Тип соответствия.</typeparam>
        /// <typeparam name="R">Тип соответствия.</typeparam>
        /// <param name="key">Ключ.</param>
        void RegisterType<T, R>(string key) where R : T;
    }
}
