using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Serialization;

namespace RTS4.Data.Actions {
    public class ARangedAttack : AAttackBase {

        [ElementXml("param", "name=MinimumRange")]
        [AttributeXml("value1")]
        public XReal MinimumRange { get; set; }

        [ElementXml("param", "name=TrackRating")]
        [AttributeXml("value1")]
        public XReal TrackRating { get; set; }

        [ElementXml("param", "name=Accuracy")]
        [AttributeXml("value1")]
        public XReal Accuracy { get; set; }

        [ElementXml("param", "name=AccuracyReductionFactor")]
        [AttributeXml("value1")]
        public XReal AccuracyReductionFactor { get; set; }

        [ElementXml("param", "name=AimBonus")]
        [AttributeXml("value1")]
        public XReal AimBonus { get; set; }

        [ElementXml("param", "name=SpreadFactor")]
        [AttributeXml("value1")]
        public XReal SpreadFactor { get; set; }

        [ElementXml("param", "name=MaxSpread")]
        [AttributeXml("value1")]
        public XReal MaxSpread { get; set; }

        [ElementXml("param", "name=UnintentionalDamageMultiplier")]
        [AttributeXml("value1")]
        public XReal UnintentionalDamageMultiplier { get; set; }

        [ElementXml("param", "name=NumberProjectiles")]
        [AttributeXml("value1")]
        public int NumberProjectiles { get; set; }

        [ElementXml("param", "name=VolleyMode")]
        public bool VolleyMode { get; set; }

        [ElementXml("param", "name=HeightBonusMultiplier")]
        [AttributeXml("value1")]
        public XReal HeightBonusMultiplier { get; set; }
        
    }
}
