using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Infrastructure.IocContainer
{
    /// <summary>
    /// ���������� IOC ����������.
    /// </summary>
    internal class UnityContainer : IContainer
    {
        private static readonly IUnityContainer Container = new Microsoft.Practices.Unity.UnityContainer();

        /// <summary>
        /// ��������� �� ���������� �������.
        /// </summary>
        /// <typeparam name="T">��� ������������ �������.</typeparam>
        /// <returns>������ ���� �.</returns>
        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// ��������� �� ���������� �������.
        /// </summary>
        /// <typeparam name="T">��� ������������ �������.</typeparam>
        /// <param name="key">����.</param>
        /// <returns>������ ���� �.</returns>
        public T Resolve<T>(string key)
        {
            return Container.Resolve<T>(key);
        }

        /// <summary>
        /// ��������� �� ���������� �������.
        /// </summary>
        /// <typeparam name="T">��� ������������ �������.</typeparam>
        /// <param name="parameters">������ ����������, ������� ����� �������� � ������������.</param>
        /// <returns>������ ���� �.</returns>
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
        /// ������������ ����� ��� � ����������.
        /// </summary>
        /// <typeparam name="T">��� ������������.</typeparam>
        /// <typeparam name="R">��� ������������.</typeparam>
        public void RegisterType<T, R>() where R : T
        {
            Container.RegisterType<T, R>();
        }

        /// <summary>
        /// ������������ ����� ��� � ����������.
        /// </summary>
        /// <typeparam name="T">��� ������������.</typeparam>
        /// <typeparam name="R">��� ������������.</typeparam>
        /// <param name="key">����.</param>
        public void RegisterType<T, R>(string key) where R : T
        {
            Container.RegisterType<T, R>(key);
        }
    }
}