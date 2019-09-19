namespace Avalonia.Collections
{
    public static class ReadOnlyListExtensions
    {
        public static T FirstOrDefault<T, TState>(this ReadOnlyList<T> source, in TState state,
            PredicateWithState<T, TState> predicate)
        {
            return EnumeratorOperations.FirstOrDefault(source.GetEnumerator(), in state, predicate);
        }

        public static bool Any<T>(this ReadOnlyList<T> source)
        {
            return EnumeratorOperations.Any<ReadOnlyList<T>.Enumerator, T>(source.GetEnumerator());
        }

        public static bool Any<T, TState>(this ReadOnlyList<T> source, in TState state,
            PredicateWithState<T, TState> predicate)
        {
            return EnumeratorOperations.Any(source.GetEnumerator(), in state, predicate);
        }
    }
}
