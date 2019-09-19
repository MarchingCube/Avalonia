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
    }
}
