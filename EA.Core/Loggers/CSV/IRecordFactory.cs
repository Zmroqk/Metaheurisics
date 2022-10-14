using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Core.Loggers.CSV
{
    public interface IRecordFactory<Record> where Record : IRecord<Record>
    {
        Record CreateRecord();
    }
}
