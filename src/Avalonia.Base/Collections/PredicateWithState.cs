namespace Avalonia.Collections
{
    public delegate bool PredicateWithState<in T, TState>(T item, in TState state);
}