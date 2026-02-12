using System;

namespace Assets._Project.Develop.Runtime.Utilities.Reactive
{
    public class Subscriber : IDisposable
    {
        private readonly Action _action;
        private readonly Action<Subscriber> _onDispose;

        public Subscriber(Action action, Action<Subscriber> onDispose)
        {
            _action = action;
            _onDispose = onDispose;
        }

        public void Dispose() => _onDispose?.Invoke(this);

        public void Invoke() => _action?.Invoke();
    }

    public class Subscriber<T> : IDisposable
    {
        private readonly Action<T> _action;
        private readonly Action<Subscriber<T>> _onDispose;

        public Subscriber(Action<T> action, Action<Subscriber<T>> onDispose)
        {
            _action = action;
            _onDispose = onDispose;
        }

        public void Dispose() => _onDispose?.Invoke(this);

        public void Invoke(T arg1) => _action?.Invoke(arg1);
    }

    public class Subscriber<T, K> : IDisposable
    {
        private readonly Action<T, K> _action;
        private readonly Action<Subscriber<T, K>> _onDispose;

        public Subscriber(Action<T, K> action, Action<Subscriber<T, K>> onDispose)
        {
            _action = action;
            _onDispose = onDispose;
        }

        public void Dispose() => _onDispose?.Invoke(this);

        public void Invoke(T arg1, K arg2) => _action?.Invoke(arg1, arg2);
    }
}