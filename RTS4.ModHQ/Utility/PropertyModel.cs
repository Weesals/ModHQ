using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.ModHQ {
    public class PropertyModel : INotifyPropertyChanged {

        private class Listener {
            public string[] Names;
            public Action<object> Callback;
        }
        private List<Listener> listeners = new List<Listener>();

        protected void ChangeProperty<T>(string name, ref T variable, T value) {
            if (variable == null) {
                if (value == null) return;
            } else if (variable.Equals(value)) return;
            variable = value;
            NotifyPropertyChanged(name);
        }
        public void OnPropertyChanged(Action<object> callback, params string[] names) {
            listeners.Add(new Listener() { Names = names, Callback = callback, });
        }
        public void RemoveOnPropertyChanged(Action<object> callback, params string[] names) {
            var entry = listeners.FirstOrDefault(l => l.Callback == callback && l.Names == names);
            if (entry != null) listeners.Remove(entry);
        }

        public void NotifyPropertyChanged(params string[] names) {
            for (int n = 0; n < names.Length; ++n) _NotifyPropertyChanged(names[n]);
            for (int l = 0; l < listeners.Count; ++l) {
                bool match = false;
                for (int n = 0; n < names.Length; ++n) if (listeners[l].Names.Contains(names[n])) match = true;
                if (match) listeners[l].Callback(this);
            }
        }
        public void NotifyPropertyChanged(string name) {
            _NotifyPropertyChanged(name);
            for (int l = 0; l < listeners.Count; ++l) {
                if (listeners[l].Names.Contains(name)) listeners[l].Callback(this);
            }
        }

        private void _NotifyPropertyChanged(string name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
