namespace Infrastructure.IocContainer
{
    /// <summary>
    /// ������� IOC �����������.
    /// </summary>
    public static class ContainersFactory
    {
        /// <summary>
        /// ������� � ���������� ������ ����������.
        /// </summary>
        /// <returns>������ ����������</returns>
        public static IContainer GetContainer()
        {
            return new UnityContainer();
        }
    }
}