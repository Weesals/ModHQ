using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;

namespace RTS4.Data {
    public struct HeightBob {
        public XReal Period;
        public XReal Magnitude;
    }
    public class Movement {
        public enum MovementTypeE { Air, Land, Water };

        public MovementTypeE Type;
        public XReal MaxVelocity;
        public XReal TurnRate;

        public HeightBob HeightBob;

    }
}
