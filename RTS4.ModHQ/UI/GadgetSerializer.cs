using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.Windows.Shapes;
using System.Windows;

namespace RTS4.ModHQ.UI {
    public class GadgetSerializer {

        public static Layout Parse(XElement rootXml) {
            return LayoutFromXml(rootXml);
        }

        private static Layout LayoutFromXml(XElement layoutXml, Gadget parent = null) {
            var layout = new Layout();
            /*foreach (var gadgetXml in layoutXml.Elements()) {
                layout.Gadgets.Add(GadgetFromXml(gadgetXml));
            }*/
            layout.Gadgets.AddRange(GadgetsFromXml(layoutXml, parent));
            return layout;
        }

        private static Gadget[] GadgetsFromXml(XElement gadgetsXml, Gadget parent = null) {
            var children = new List<Gadget>();
            foreach (var childXml in gadgetsXml.Elements()) {
                switch (childXml.Name.LocalName) {
                    case "gadget": children.Add(GadgetFromXml(childXml, parent)); break;
                }
            }
            return children.OrderBy(c => c.Z).ToArray();
        }

        private static Gadget GadgetFromXml(XElement gadgetXml, Gadget parent = null) {
            Debug.Assert(gadgetXml.Name.LocalName == "gadget");
            var type = gadgetXml.AttributeOrNull("type");
            Gadget gadget = null;
            gadget = new Gadget(type);
            foreach (var attr in gadgetXml.Attributes()) {
                switch (attr.Name.LocalName) {
                    case "name": gadget.Name = attr.Value; break;
                    case "size1024": gadget.Rectangle1024 = ToRectangle(attr.Value); break;
                    case "sizeRel1024": {
                        var rect = ToRectangle(attr.Value);
                        rect.X = rect.X * parent.Rectangle1024.Width / 1024;
                        rect.Y = rect.Y * parent.Rectangle1024.Height / 768;
                        rect.Width = rect.Width * parent.Rectangle1024.Width / 1024;
                        rect.Height = rect.Height * parent.Rectangle1024.Height / 768;
                        rect.X += parent.Rectangle1024.X;
                        rect.Y += parent.Rectangle1024.Y;
                        gadget.Rectangle1024 = rect;
                    } break;
                    case "hidden": gadget.Hidden = true; break;
                    case "z": gadget.Z = int.Parse(attr.Value); break;
                    default: gadget.Values.Add(attr.Name.LocalName, attr.Value); break;
                }
            }
            gadget.Text = gadgetXml.GetImmediateValue();
            gadget.Command = gadgetXml.ElementOrNull("command");
            gadget.Children = GadgetsFromXml(gadgetXml, gadget);
            gadget.StateEntries = gadgetXml.Elements("stateentry").Select(e =>
                new StateEntry() {
                    Background = e.AttributeOrNull("background"),
                }
            ).ToArray();
            foreach (var child in gadget.Children) child.Parent = gadget;
            return gadget;
        }

        private static Rect ToRectangle(string str) {
            if (str == null) return Rect.Empty;
            Rect rect = new Rect();
            var nums = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            rect.X = int.Parse(nums[0]);
            rect.Y = int.Parse(nums[1]);
            rect.Width = Math.Max(int.Parse(nums[2]) - rect.X, 0);
            rect.Height = Math.Max(int.Parse(nums[3]) - rect.Y, 0);
            return rect;
        }

        private static Rect ToRectangleF(string str) {
            if (str == null) return Rect.Empty;
            Rect rect = new Rect();
            var nums = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            rect.X = float.Parse(nums[0]);
            rect.Y = float.Parse(nums[1]);
            rect.Width = float.Parse(nums[2]) - rect.X;
            rect.Height = float.Parse(nums[3]) - rect.Y;
            return rect;
        }


    }
}
