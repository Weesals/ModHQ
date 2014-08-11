using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.Data.Serialization {
    public interface ILogService {
        void Error(string error);
        void Warning(string warning);
        void Info(string info);
    }
}
