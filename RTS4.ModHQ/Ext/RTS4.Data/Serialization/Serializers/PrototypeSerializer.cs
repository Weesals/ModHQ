using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RTS4.Common;
using RTS4.Data.Lists;

namespace RTS4.Data.Serialization {
    public class PrototypeSerializer : TypeSerializer<UnitPrototype> {

        public override UnitPrototype Deserialize(SerializationContext context, XElement root) {
            var proto = base.Deserialize(context, root);
            proto.Id = root.AttributeOrDefault("id", -1);
            proto.Name = root.AttributeOrDefault("name", "");
            proto.HitPoints = new HitPoints() {
                Initial = (int)root.ElementOrDefault("initialhitpoints", 0.0f),
                Maximum = (int)root.ElementOrDefault("maxhitpoints", 0.0f),
            };
            proto.ObstructionRadius = new XVector2() {
                X = new XReal(root.ElementOrDefault("obstructionradiusx", 0.0f)),
                Y = new XReal(root.ElementOrDefault("obstructionradiusy", 0.0f)),
            };
            var heightbobXml = root.Element("heightbob");
            proto.Movement = new Movement() {
                HeightBob = new HeightBob() {
                    Period = heightbobXml.ElementOrDefault("period", XReal.Zero),
                    Magnitude = heightbobXml.ElementOrDefault("magnitude", XReal.Zero),
                },
                MaxVelocity = root.ElementOrDefault("maxvelocity", XReal.Zero),
                TurnRate = root.ElementOrDefault("turnrate", XReal.Zero),
                Type = (Movement.MovementTypeE)Enum.Parse(typeof(Movement.MovementTypeE), root.ElementOrDefault("movementtype", "land"), true),
            };
            var miniColXml = root.Element("minimapcolor");
            proto.MinimapColor = new XColor(
                (byte)(miniColXml.AttributeOrDefault("red", 1.0f) * 255),
                (byte)(miniColXml.AttributeOrDefault("green", 1.0f) * 255),
                (byte)(miniColXml.AttributeOrDefault("blue", 1.0f) * 255)
            );
            var flagRegistry = context.GetService<FlagRegistry>();
            if (flagRegistry != null) {
                foreach (var unitTypeXml in root.Elements("unittype")) {
                    var flag = flagRegistry.Get(UnitFlag.UnitTypePrefix + unitTypeXml.Value);
                    proto.RegisterToFlag(flag);
                }
                foreach (var flagXml in root.Elements("flag")) {
                    var flag = flagRegistry.Get(flagXml.Value);
                    proto.RegisterToFlag(flag);
                }
            }
            List<UnitAction> actions = new List<UnitAction>();
            foreach (var actionXml in root.ElementsCase("action")) {
                string name = actionXml.AttributeOrDefault("name", "");
                var actionType = Type.GetType("RTS4.Data.Actions.A" + name);
                if (actionType == null) context.LogError("Unable to find action " + name);
                else {
                    var action = context.GetSerializer(actionType).Deserialize(context, actionXml) as UnitAction;
                    if (action == null) context.LogError("Unable to create action " + name);
                    else actions.Add(action);
                }
            }
            proto.Actions = actions.ToArray();
            return proto;
        }

    }

}
