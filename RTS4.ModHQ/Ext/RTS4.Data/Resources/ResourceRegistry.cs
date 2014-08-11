using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.Data.Resources {
    public class ResourceRegistry : Registry<ResourceType> {

        public ResourceRegistry(params string[] resources) {
            foreach (var resource in resources) {
                Add(resource, new ResourceType(resource));
            }
        }

    }
}
