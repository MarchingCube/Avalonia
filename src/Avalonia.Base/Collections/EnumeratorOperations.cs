using System;
using System.Collections.Generic;

namespace Avalonia.Collections
{
    internal static class EnumeratorOperations
    {
        public static T FirstOrDefault<TEnumerator, T, TState>(TEnumerator enumerator, in TState state,
            PredicateWithState<T, TState> predicate)
            where TEnumerator : IEnumerator<T>
        {
            try
            {
                while (enumerator.MoveNext())
                {
                    T current = enumerator.Current;

                    if (predicate(current, in state))
                    {
                        return current;
                    }
                }

                return default;
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        public static bool Any<TEnumerator, T>(TEnumerator enumerator)
            where TEnumerator : IEnumerator<T>
        {
            try
            {
                return enumerator.MoveNext();
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        public static bool Any<TEnumerator, T, TState>(TEnumerator enumerator, in TState state,
            PredicateWithState<T, TState> predicate)
            where TEnumerator : IEnumerator<T>
        {
            try
            {
                while (enumerator.MoveNext())
                {
                    T current = enumerator.Current;

                    if (predicate(current, in state))
                    {
                        return true;
                    }
                }

                return false;
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        public static bool Any<TEnumerator, T>(TEnumerator enumerator, Predicate<T> predicate)
            where TEnumerator : IEnumerator<T>
        {
            return Any(enumerator, predicate, StatelessPredicateAdapter<T>.Instance);
        }

        private static class StatelessPredicateAdapter<T>
        {
            public static readonly PredicateWithState<T, Predicate<T>> Instance = Adapt;

            private static bool Adapt(T instance, in Predicate<T> inner)
            {
                return inner(instance);
            }
        }
    }
}
