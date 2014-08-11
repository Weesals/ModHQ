using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Data.Resources;
using RTS4.Data.Serialization;

namespace RTS4.Data.Actions {
    public class ARegenerate : UnitAction {

        [ElementXml("param", "name=Rate")]
        public UnitRate[] Rate { get; set; }

    }
}
