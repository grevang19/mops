using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Infrastructure.IocContainer
{
    /// <summary>
    /// Реализация IOC контейнера.
    /// </summary>
    internal class UnityContainer : IContainer
    {
        private static readonly IUnityContainer Container = new Microsoft.Practices.Unity.UnityContainer();

        /// <summary>
        /// Извлекает из контейнера инстанс.
        /// </summary>
        /// <typeparam name="T">Тип извлекаемого объекта.</typeparam>
        /// <returns>Объект типа Т.</returns>
        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// Извлекает из контейнера инстанс.
        /// </summary>
        /// <typeparam name="T">Тип извлекаемого объекта.</typeparam>
        /// <param name="key">Ключ.</param>
        /// <returns>Объект типа Т.</returns>
        public T Resolve<T>(string key)
        {
            return Container.Resolve<T>(key);
        }

        /// <summary>
        /// Извлекает из контейнера инстанс.
        /// </summary>
        /// <typeparam name="T">Тип извлекаемого объекта.</typeparam>
        /// <param name="parameters">Список параметров, которые будут переданы в констркуктор.</param>
        /// <returns>Объект типа Т.</returns>
        public T Resolve<T>(IDictionary<string, object> parameters)
        {
            var parameterOverrides = new ParameterOverrides();

            foreach (var parameter in parameters)
            {
                parameterOverrides.Add(parameter.Key, parameter.Value);
            }

            return Container.Resolve<T>(parameterOverrides);
        }

        /// <summary>
        /// Регистрирует новый тип в контейнере.
        /// </summary>
        /// <typeparam name="T">Тип соответствия.</typeparam>
        /// <typeparam name="R">Тип соответствия.</typeparam>
        public void RegisterType<T, R>() where R : T
        {
            Container.RegisterType<T, R>();
        }

        /// <summary>
        /// Регистрирует новый тип в контейнере.
        /// </summary>
        /// <typeparam name="T">Тип соответствия.</typeparam>
        /// <typeparam name="R">Тип соответствия.</typeparam>
        /// <param name="key">Ключ.</param>
        public void RegisterType<T, R>(string key) where R : T
        {
            Container.RegisterType<T, R>(key);
        }
    }
}