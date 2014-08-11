using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Serialization;

namespace RTS4.Data {
    public class UnitPrototype : Prototype {

        public int Id { get; internal set; }
        public string Name { get; internal set; }

        //[SerializationName("dbid")]
        [ElementXml("dbid")]
        public int DBId { get; set; }
        //[SerializationName("displaynameid")]
        [ElementXml("displaynameid")]
        public int DisplaynameId { get; set; }
        //[SerializationName("icon")]
        [ElementXml("icon")]
        public string Icon { get; set; }
        //[SerializationName("portraiticon")]
        [ElementXml("portraiticon")]
        public string PortraitIcon { get; set; }

        public HitPoints HitPoints { get; set; }

        [ElementXml("los")]
        public XReal LineOfSight { get; set; }
        public XVector2 ObstructionRadius { get; set; }
        public Movement Movement { get; set; }
        public XColor MinimapColor { get; set; }

        public UnitAction[] Actions { get; set; }

        public UnitPrototype() {
        }
        public UnitPrototype(int id, string name) {
            Id = id;
            Name = name;
        }

    }
}
