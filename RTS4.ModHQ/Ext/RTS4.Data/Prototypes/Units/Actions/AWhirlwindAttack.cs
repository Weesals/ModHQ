using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Serialization;

namespace RTS4.Data.Actions {
    public class AWhirlwindAttack : AAttackBase {

        [ElementXml("param", "name=MinimumRange")]
        [AttributeXml("value1")]
        public XReal MinimumRange { get; set; }

    }
}
