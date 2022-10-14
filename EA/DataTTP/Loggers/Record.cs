using EA.Core.Loggers.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP.Loggers
{
    public class Record : RecordBase<Record>
    {
        public int CurrentRun { get; set; }
        public Record(Action<Record> applyAdditionalOperation)
        {
            this.ApplyAdditionalData = applyAdditionalOperation;
        }
    }
}
