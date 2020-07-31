using System;
using System.Collections;
using System.Collections.Generic;

namespace MB.Utilities.MessageBus
{
    public class MessageHeaders : IDictionary<string, object>
    {
        private readonly IDictionary<string, object> _messageHeaders;

        public const string HeaderKeyPrefix = "GenericContext.MB.";

        public object this[string key]
        {
            get => _messageHeaders[key];
            set => _messageHeaders[key] = value;
        }

        public ICollection<string> Keys => _messageHeaders.Keys;

        public ICollection<object> Values => _messageHeaders.Values;

        public int Count => _messageHeaders.Count;

        public bool IsReadOnly => _messageHeaders.IsReadOnly;

        public MessageHeaders() : this(new Dictionary<string, object>())
        {
        }

        public MessageHeaders(IDictionary<string, object> messageHeaders)
        {
            _messageHeaders = messageHeaders;
        }

        public void Add(string key, object value)
        {
            _messageHeaders.Add(key, value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _messageHeaders.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _messageHeaders.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _messageHeaders.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _messageHeaders.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _messageHeaders.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _messageHeaders.Remove(key);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _messageHeaders.Remove(item);
        }

        public bool TryGetValue(string key, out object value)
        {
            bool exists = _messageHeaders.TryGetValue(key, out object messageHeader);
            value = messageHeader;
            return exists;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _messageHeaders.GetEnumerator();
        }
    }
}
