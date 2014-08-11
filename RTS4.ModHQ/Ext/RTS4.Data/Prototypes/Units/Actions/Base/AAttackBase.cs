using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Resources;
using RTS4.Data.Serialization;
using RTS4.Data.Serialization.Converters;

namespace RTS4.Data.Actions {
    public abstract class AAttackBase : UnitAction {

        //[SerializationName("param", "value1", "name=MaximumRange")]
        [ElementXml("param", "name=MaximumRange")]
        [AttributeXml("value1")]
        public XReal MaximumRange { get; set; }
        //[SerializationName("param", "value1", "name=Rate")]
        [ElementXml("param", "name=Rate")]
        public UnitRate[] Rate { get; set; }

        //[SerializationName("param", null, "name=Damage")]
        [ElementXml("param", "name=Damage")]
        public DamageType[] DamageTypes { get; set; }
        //[SerializationName("param", null, "name=DamageBonus")]
        [ElementXml("param", "name=DamageBonus")]
        public DamageBonus[] DamageBonus { get; set; }

        [ElementXml("param", "name=ChargeAction")]
        public bool ChargeAction { get; set; }
        [ElementXml("param", "name=SingleUse")]
        public bool SingleUse { get; set; }

        [ElementXml("param", "name=AttackAction")]
        public bool AttackAction { get; set; }

        [ElementXml("param", "name=NoWorkOnFrozenUnits")]
        public bool NoWorkOnFrozenUnits { get; set; }
        [ElementXml("param", "name=NoWorkOnFrozenUnits")]
        public bool NoWorkOnStoneUnits { get; set; }

    }
}
