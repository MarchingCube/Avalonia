using System.Collections.Generic;

namespace Avalonia.Collections
{
    public static class ListExtensions
    {
        public static T FirstOrDefault<T, TState>(this List<T> source, in TState state,
            PredicateWithState<T, TState> predicate)
        {
            return EnumeratorOperations.FirstOrDefault(source.GetEnumerator(), in state, predicate);
        }

        public static bool Any<T>(this List<T> source)
        {
            return EnumeratorOperations.Any<List<T>.Enumerator, T>(source.GetEnumerator());
        }

        public static bool Any<T, TState>(this List<T> source, in TState state,
            PredicateWithState<T, TState> predicate)
        {
            return EnumeratorOperations.Any(source.GetEnumerator(), in state, predicate);
        }
    }
}
