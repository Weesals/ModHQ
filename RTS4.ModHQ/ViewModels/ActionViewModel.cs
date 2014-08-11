using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Data;

namespace RTS4.ModHQ.ViewModels {
    public class ActionViewModel : PropertyModel {

        public class Property : PropertyModel {
            public string Name { get; private set; }
            public string Value { get; private set; }
            public Property(string name, string value) {
                Name = name;
                Value = value;
            }
        }

        public string Name { get { return Source.GetType().Name.Substring(1); } }

        public ObservableCollection<Property> Properties { get; private set; }

        public ActionViewModel() {
            Properties = new ObservableCollection<Property>();
        }

        private UnitAction source;
        public UnitAction Source {
            get { return source; }
            set {
                if (source == value) return;
                source = value;
                Properties.Clear();
                if (source != null) {
                    var properties = source.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(RTS4.Data.Serialization.ElementXml), true) != null);
                    foreach (var property in properties) {
                        var val = property.GetValue(source, null);
                        var valStr = (val != null ? val.ToString() : "null");
                        if (val != null && property.PropertyType.IsArray) {
                            var len = ((Array)val).GetLength(0);
                            valStr = len.ToString() + (len == 1 ? " item" : " items");
                        }
                        Properties.Add(new Property(property.Name, valStr));
                    }
                }
                NotifyPropertyChanged("Name");
                NotifyPropertyChanged("Properties");
            }
        }

    }
}
