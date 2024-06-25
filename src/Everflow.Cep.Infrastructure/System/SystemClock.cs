using Everflow.Cep.Core.Interfaces;

namespace Everflow.Cep.Infrastructure.System;

public class SystemClock : ISystemClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}