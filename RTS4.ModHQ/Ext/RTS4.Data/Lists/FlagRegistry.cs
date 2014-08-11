using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.Data.Lists {
    public class FlagRegistry : Registry<UnitFlag> {

        public override UnitFlag Generate(string name) {
            return new UnitFlag(name);
        }

    }
}
