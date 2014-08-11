using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.Data {
    public class Registry<T> where T : class {

        private Dictionary<string, T> items = new Dictionary<string, T>();

        public virtual T Generate(string name) {
            return null;
        }

        public void Add(string name, T item) {
            items.Add(name, item);
        }

        public T Get(string name) {
            if (items.ContainsKey(name)) return items[name];
            else {
                var item = Generate(name);
                items.Add(name, item);
                return item;
            }
        }

    }
}
