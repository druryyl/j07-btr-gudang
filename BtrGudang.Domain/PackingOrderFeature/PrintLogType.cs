using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtrGudang.Domain.PackingOrderFeature
{
    public class PrintLogType : IPrintLogKey
    {
        public PrintLogType(string printLogId, DateTime printLogTimestamp, string docType   )
        {
            PrintLogId = printLogId;
            PrintLogTimestamp = printLogTimestamp;
            DocType = docType;
        }

        public static PrintLogType Create(string docType)
        {
            
            var newId = Ulid.NewUlid().ToString();
            var dateTime = DateTime.Now;
            var result = new PrintLogType(newId, dateTime, docType);
            return result;
        }

        public string PrintLogId { get; private set; }
        public DateTime PrintLogTimestamp { get; private set;  }
        public string DocType { get; private set;  }
    }

    public interface IPrintLogKey
    {
        string PrintLogId { get; }
    }
}
