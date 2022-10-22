using Loggers.CSV;
using Meta.Core;

namespace Loggers
{
    public interface ILogger<TRecord> where TRecord : IRecord
    {
        Task Log(TRecord param);
    }
}
