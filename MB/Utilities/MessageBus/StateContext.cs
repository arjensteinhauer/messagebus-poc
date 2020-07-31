using System.Collections.Concurrent;
using System.Threading;

namespace MB.Utilities.MessageBus
{
    internal static class StateContext
    {
        private static readonly ConcurrentDictionary<string, AsyncLocal<object>> _state = new ConcurrentDictionary<string, AsyncLocal<object>>();

        public static void Set(string name, object data) => _state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;

        public static object Get(string name) => _state.TryGetValue(name, out var data) ? data.Value : null;
    }
}
