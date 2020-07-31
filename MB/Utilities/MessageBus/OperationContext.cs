using System;
using System.Collections.Immutable;
using System.Linq;

namespace MB.Utilities.MessageBus
{
    public sealed class OperationContext : IDisposable
    {
        private static readonly string _name = Guid.NewGuid().ToString("N");

        private bool _isDisposed;

        public static OperationContext Current => CurrentContext.FirstOrDefault();

        public MessageHeaders IncomingHeaders { get; } = new MessageHeaders();

        public MessageHeaders OutgoingHeaders { get; } = new MessageHeaders();

        private static ImmutableStack<OperationContext> CurrentContext
        {
            get => StateContext.Get(_name) as ImmutableStack<OperationContext> ?? ImmutableStack.Create<OperationContext>();

            set => StateContext.Set(_name, value);
        }

        public OperationContext(MessageHeaders incomingHeaders) : this()
        {
            IncomingHeaders = incomingHeaders;
            OutgoingHeaders = incomingHeaders;
        }

        public OperationContext()
        {
            CurrentContext = CurrentContext.Push(this);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            CurrentContext = CurrentContext.Pop();

            _isDisposed = true;
        }
    }
}
