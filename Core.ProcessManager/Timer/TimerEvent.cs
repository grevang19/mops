namespace Core.ProcessManager.Timer
{
    /// <summary>
    /// Делегат для события тиков таймера.
    /// </summary>
    /// <param name="sender">Непосредственно сам таймер.</param>
    public delegate void TimerEvent(ITimer sender);
}