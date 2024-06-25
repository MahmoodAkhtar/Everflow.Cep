namespace Everflow.Cep.Core.Interfaces;

public interface ISystemClock
{
    public DateTime UtcNow { get; }
}