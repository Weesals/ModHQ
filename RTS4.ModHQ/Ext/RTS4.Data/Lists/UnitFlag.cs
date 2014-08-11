using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.Data {
    public class UnitFlag {

        public static readonly string UnitTypePrefix = "UnitType.";

        public string Name { get; private set; }

        public Prototype[] Prototypes;

        public UnitFlag(string name) {
            Name = name;
        }

        internal void AddPrototype(Prototype proto) {
            if (Prototypes == null) Prototypes = new[] { proto };
            else Prototypes = Prototypes.Concat(new[] { proto }).ToArray();
        }

    }
}
