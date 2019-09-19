using System.Collections.Generic;
using Avalonia.Controls;
using BenchmarkDotNet.Attributes;

namespace Avalonia.Benchmarks.Base
{
    [MemoryDiagnoser]
    public class AvaloniaObjectInitializationBenchmark
    {
        private const int Count = 1000;
        private static readonly List<Button> s_retain = new List<Button>(Count);

        [Benchmark(OperationsPerInvoke = Count)]
        public void InitializeButton()
        {
            s_retain.Clear();

            for (int i = 0; i < Count; ++i)
            {
                s_retain.Add(new Button());
            }
        }
    }
}
