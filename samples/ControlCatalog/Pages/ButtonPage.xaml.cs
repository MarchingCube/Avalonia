using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DynamicData;
using ReactiveUI;
using System.Linq;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    public class TestViewModel : ReactiveObject
    {
        private readonly ReadOnlyObservableCollection<string> _availableItems;
        private readonly SourceList<string> _allAvailableItems;
        private string _searchText;
        private string _selectedText;

        public ReadOnlyObservableCollection<string> AvailableItems => _availableItems;

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public string SelectedText
        {
            get => _selectedText;
            set => this.RaiseAndSetIfChanged(ref _selectedText, value);
        }

        public TestViewModel()
        {
            _allAvailableItems = new SourceList<string>();

            var filter = this.WhenAnyValue(vm => vm.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Select(BuildFilter);

            _allAvailableItems.Connect()
                .Filter(filter)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Bind(out _availableItems)
                .Subscribe();
        }

        public void PrepareAvailableItems()
        {
            _allAvailableItems.Edit(l =>
            {
                l.Clear();

                for (int i = 1; i < 500; i++)
                {
                    l.Add($"lItem{i}");
                }

                l.Add("Last");
            });

            SelectedText = _availableItems.FirstOrDefault(i => i.Contains("Last"));
        }

        private Func<string, bool> BuildFilter(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return t => true;
            }

            return t => t.ToLower().Contains(searchText.ToLower());
        }
    }

    public class ButtonPage : UserControl
    {
        private TestViewModel _vm;

        public ButtonPage()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = _vm = new TestViewModel();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            _vm.PrepareAvailableItems();
        }
    }
}
