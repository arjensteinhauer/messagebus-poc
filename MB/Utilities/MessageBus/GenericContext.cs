using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace MB.Utilities.MessageBus
{
    [Serializable]
    public class GenericContext<T>
    {
#pragma warning disable S2743 // Static fields should not be used in generic types
        private static readonly string _typeName;
#pragma warning restore S2743 // Static fields should not be used in generic types

        public T Value { get; set; }

        public static GenericContext<T> Current
        {
            get
            {
                var context = OperationContext.Current;
                if (context == null)
                {
                    return null;
                }

                if (!context.IncomingHeaders.TryGetValue(_typeName, out object current))
                {
                    return null;
                }

                if (current is Newtonsoft.Json.Linq.JObject)
                {
                    var json = current as Newtonsoft.Json.Linq.JObject;
                    current = JsonConvert.DeserializeObject<GenericContext<T>>(json.ToString());
                }

                return current as GenericContext<T>;
            }
            set
            {
                var context = OperationContext.Current ?? new OperationContext();
                if (context.OutgoingHeaders.ContainsKey(_typeName))
                {
                    throw new InvalidOperationException("A header with name " + _typeName + " already exists in the current call context.");
                }

                context.OutgoingHeaders.Add(_typeName, value);
            }
        }


#pragma warning disable S3963 // "static" fields should be initialized inline
        static GenericContext()
        {
            // verify [Serializable] on T
            Debug.Assert(typeof(T).IsSerializable);

            _typeName = $"{MessageHeaders.HeaderKeyPrefix}{TypeNameHelper.GetTypeName(typeof(GenericContext<T>))}";
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public GenericContext(T value)
        {
            Value = value;
        }

        public GenericContext() : this(default)
        {
        }
    }
}
