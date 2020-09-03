﻿using System.Threading;
using Framework;

namespace InMemoryAdapter
{
    internal sealed class CachedAggregate
    {
        private readonly object _value;
        private int _isTaken = 0;
            
        public CachedAggregate(object value)
        {
            _value = value;
        }

        public Optional<T> TryTakeValue<T>() =>
            Interlocked.CompareExchange(ref _isTaken, 1, 0) == 0
                ? Optional<T>.From((T)_value)
                : Optional<T>.None;

        public bool ReleaseValue() => Interlocked.CompareExchange(ref _isTaken, 0, 1) == 1;
    }
}