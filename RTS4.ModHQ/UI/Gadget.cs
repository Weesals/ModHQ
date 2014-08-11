using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;

namespace RTS4.ModHQ.UI {
    public class StateEntry : PropertyModel {
        private string background;
        public string Background { get { return background; } set { ChangeProperty("Background", ref background, value); } }
    }
    public class Gadget : PropertyModel {
        public readonly string Type;

        private string name;
        public string Name { get { return name; } set { ChangeProperty("Name", ref name, value); } }
        private Rect rect;
        public Rect Rectangle1024 { get { return rect; } set { ChangeProperty("Rectangle1024", ref rect, value); } }
        private bool hidden;
        public bool Hidden { get { return hidden; } set { ChangeProperty("Hidden", ref hidden, value); } }
        private int z = 10;
        public int Z { get { return z; } set { ChangeProperty("Z", ref z, value); } }

        private string text;
        public string Text { get { return text; } set { ChangeProperty("Text", ref text, value); } }

        private string command;
        public string Command { get { return command; } set { ChangeProperty("Command", ref command, value); } }

        public Gadget Parent { get; set; }
        public Gadget[] Children { get; set; }
        public StateEntry[] StateEntries { get; set; }

        public Dictionary<string, string> Values { get; private set; }

        public Gadget(string type) {
            Values = new Dictionary<string,string>();;
            Type = type;
        }

    }
}
