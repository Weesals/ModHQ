using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Data.Serialization;

namespace RTS4.Data.Actions {
    public class AChargedRangedAttack : ARangedAttack {

        [ElementXml("param", "name=MuteDamage")]
        public bool MuteDamage { get; set; }

        [ElementXml("param", "name=Bounces")]
        [AttributeXml("value1")]
        public int Bounces { get; set; }

    }
}
