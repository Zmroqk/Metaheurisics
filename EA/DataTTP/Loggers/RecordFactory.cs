using EA.Core.Loggers.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DataTTP.Loggers
{
    public class RecordFactory : IRecordFactory<Record>
    {
        private Action<Record> ApplyAdditionalData { get; set; }
        public RecordFactory(Action<Record> applyAdditionalData = null)
        {
            this.ApplyAdditionalData = applyAdditionalData;
        }
        public Record CreateRecord()
        {
            return new Record(this.ApplyAdditionalData);
        }
    }
}
