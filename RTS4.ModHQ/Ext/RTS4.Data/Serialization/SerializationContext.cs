using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Data.Serialization;

namespace RTS4.Data.Serialization {
    public class SerializationContext {

        private Dictionary<Type, ITypeSerializer> serializerCache = new Dictionary<Type,ITypeSerializer>();
        private List<object> services = new List<object>();

        public ITypeSerializer GetSerializer(Type type) {
            if (serializerCache.ContainsKey(type)) return serializerCache[type];
            Type serializerType = null;
            var serializers = type.GetCustomAttributes(typeof(SerializationSerializer), true);
            if (serializers.Length > 0) serializerType = ((SerializationSerializer)serializers.FirstOrDefault()).Serializer;
            if(serializerType == null) serializerType = typeof(TypeSerializer<>);
            if (serializerType.IsGenericTypeDefinition) serializerType = serializerType.MakeGenericType(type);
            var serializer = serializerType.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ITypeSerializer;
            serializerCache.Add(type, serializer);
            return serializer;
        }


        public void AddService<T>(T service) {
            services.Add(service);
        }
        public T GetService<T>() {
            return (T)services.FirstOrDefault(s => s is T);
        }

        public void LogError(string error) {
            var logger = GetService<ILogService>();
            if (logger != null) logger.Error(error);
            else Debug.WriteLine(error);
        }
    }
}
