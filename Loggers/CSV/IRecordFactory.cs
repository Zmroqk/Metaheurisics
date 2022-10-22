
namespace Loggers.CSV
{
    public interface IRecordFactory<TRecord, TParam> where TRecord : IRecord
    {
        TRecord CreateRecord();
    }
}
