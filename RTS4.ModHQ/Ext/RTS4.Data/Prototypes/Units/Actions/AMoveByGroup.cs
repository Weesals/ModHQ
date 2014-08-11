using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Data.Serialization;

namespace RTS4.Data.Actions {
    public class AMoveByGroup : UnitAction {

        [ElementXml("param", "name=AckSound")]
        public bool AckSound { get; set; }

    }
}
