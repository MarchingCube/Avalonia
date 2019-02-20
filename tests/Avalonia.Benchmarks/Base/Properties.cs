using System;
using System.Reactive.Subjects;
using BenchmarkDotNet.Attributes;

namespace Avalonia.Benchmarks.Base
{
    [MemoryDiagnoser]
    public class AvaloniaPropertyRegistryBenchmark
    {
        [Benchmark]
        public Class1 InitializeProperty()
        {
            return new Class1();
        }

        public class Class1 : AvaloniaObject
        {
            public static readonly AvaloniaProperty<int> IntProperty =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty2 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty3 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty4 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty5 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty6 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty7 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty8 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty9 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty10 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty11 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);

            public static readonly AvaloniaProperty<int> IntProperty12 =
                AvaloniaProperty.Register<Class1, int>("Int", defaultValue: 5);
        }
    }

    [MemoryDiagnoser]
    public class AvaloniaObjectBenchmark
    {
        private Class1 target = new Class1();
        private Subject<int> intBinding = new Subject<int>();

        public AvaloniaObjectBenchmark()
        {
            target.SetValue(Class1.IntProperty, 123);
        }

        [Benchmark]
        public void ClearAndSetIntProperty()
        {
            target.ClearValue(Class1.IntProperty);
            target.SetValue(Class1.IntProperty, 123);
        }

        [Benchmark]
        public void BindIntProperty()
        {
            using (target.Bind(Class1.IntProperty, intBinding))
            {
                for (var i = 0; i < 100; ++i)
                {
                    intBinding.OnNext(i);
                }
            }
        }

        class Class1 : AvaloniaObject
        {
            public static readonly AvaloniaProperty<int> IntProperty =
                AvaloniaProperty.Register<Class1, int>("Int");
        }
    }
}
