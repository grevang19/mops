namespace Infrastructure.IocContainer
{
    /// <summary>
    /// Фабрика IOC контейнеров.
    /// </summary>
    public static class ContainersFactory
    {
        /// <summary>
        /// Создает и возвращает объект контейнера.
        /// </summary>
        /// <returns>Объект контейнера</returns>
        public static IContainer GetContainer()
        {
            return new UnityContainer();
        }
    }
}