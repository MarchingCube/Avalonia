using System.Collections;
using System.Collections.Generic;

namespace Avalonia.Collections
{
    public static class ReadOnlyList
    {
        public static ReadOnlyList<T> From<T>(List<T> inner)
        {
            return new ReadOnlyList<T>(inner);
        }
    }

    public readonly struct ReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly List<T> _inner;

        public ReadOnlyList(List<T> inner)
        {
            _inner = inner;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_inner);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _inner.Count;

        public T this[int index] => _inner[index];

        public struct Enumerator : IEnumerator<T>
        {
            private readonly List<T> _inner;
            private List<T>.Enumerator _innerEnumerator;

            public Enumerator(List<T> inner)
            {
                _inner = inner;
                _innerEnumerator = inner.GetEnumerator();
            }

            public bool MoveNext()
            {
                return _innerEnumerator.MoveNext();
            }

            public void Reset()
            {
                _innerEnumerator.Dispose();
                _innerEnumerator = _inner.GetEnumerator();
            }

            public T Current => _innerEnumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _innerEnumerator.Dispose();
            }
        }
    }
}
