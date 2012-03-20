using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestSharp.Deserializers
{
    public class CustomDeserializerCollection
    {
        private IDictionary<Type, Func<string, object>> _dict;

        public CustomDeserializerCollection()
        {
            _dict = new Dictionary<Type, Func<string, object>>();
        }

        public bool InsertDeserializer<T>(Func<string, T> deserializeMethod)
        {
            var overridden = false;
            var type = typeof(T);

            Func<string, object> wrappedDeserialization = value =>
                {
                    return (object)deserializeMethod(value);
                };

            if (_dict.ContainsKey(type))
            {
                _dict[type] = wrappedDeserialization;
                overridden = true;
            }
            else
            {
                _dict.Add(type, wrappedDeserialization);
            }

            return overridden;
        }

        public bool Contains(Type type)
        {
            return _dict.ContainsKey(type);
        }

        public object Deserialize(Type type, string input)
        {
            return _dict[type](input);
        }
    }
}
