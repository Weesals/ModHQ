﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Data.Resources;
using RTS4.Data.Serialization;

namespace RTS4.Data.Actions {
    public class APickUp : UnitAction {

        [ElementXml("param", "name=TypedRange")]
        public DamageBonus[] TypedRange { get; set; }

    }
}
