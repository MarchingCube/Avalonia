using System.Collections.Generic;

namespace Avalonia.Collections
{
    public static class ObjectPredicates<T>
    {
        public new static readonly PredicateWithState<T, T> Equals = EqualsImpl;

        private static bool EqualsImpl(T item, in T state)
        {
            return EqualityComparer<T>.Default.Equals(item, state);
        }
    }
}
