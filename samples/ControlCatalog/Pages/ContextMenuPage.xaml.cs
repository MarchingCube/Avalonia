using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    public class ContextMenuPage : UserControl
    {
        public ContextMenuPage()
        {
            this.InitializeComponent();

            var vm = new MenuPageViewModel();

            vm.MenuItems = new[]
            {
                new MenuItemViewModel
                {
                    Header = "_File",
                    Items = new[]
                    {
                        new MenuItemViewModel { Header = "_Open...", Command = vm.OpenCommand },
                        new MenuItemViewModel { Header = "Save", Command = vm.SaveCommand },
                        new MenuItemViewModel { Header = "-" },
                        new MenuItemViewModel
                        {
                            Header = "Recent",
                            Items = new[]
                            {
                                new MenuItemViewModel
                                {
                                    Header = "File1.txt",
                                    Command = vm.OpenRecentCommand,
                                    CommandParameter = @"c:\foo\File1.txt"
                                },
                                new MenuItemViewModel
                                {
                                    Header = "File2.txt",
                                    Command = vm.OpenRecentCommand,
                                    CommandParameter = @"c:\foo\File2.txt"
                                },
                            }
                        },
                    }
                },
                new MenuItemViewModel
                {
                    Header = "_Edit",
                    Items = new[]
                    {
                        new MenuItemViewModel { Header = "_Copy" },
                        new MenuItemViewModel { Header = "_Paste" },
                    }
                }
            };

            DataContext = vm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
