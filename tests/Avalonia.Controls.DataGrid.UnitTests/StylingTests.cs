using System;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Default;
using Avalonia.UnitTests;
using Xunit;

namespace Avalonia.Controls.UnitTests
{
    public class StylingTests
    {
        [Fact]
        public void Can_Style_Column_Header()
        {
            using var app = UnitTestApplication.Start(TestServices.StyledWindow.With(theme: CreateDefaultTheme()));

            var target = new DataGrid
            {
                HeadersVisibility = DataGridHeadersVisibility.All
            };
            
            target.Styles.Add(new Style(selector => selector.OfType<DataGridColumnHeader>())
            {
                Setters =
                {
                    new Setter(DataGridColumnHeader.AreSeparatorsVisibleProperty, true)
                }
            });
            
            target.Columns.Add(new DataGridCheckBoxColumn
            {
                Header = "Broken"
            });
            
            var root = new TestRoot(true, target);
            
            root.LayoutManager.ExecuteInitialLayoutPass();

            root.Child = null;
        }

        private Func<Styles> CreateDefaultTheme()
        {
            return () =>
            {
                var result = new Styles
                {
                    new DefaultTheme(),
                    
                };

                var baseLight = (IStyle)AvaloniaXamlLoader.Load(
                    new Uri("resm:Avalonia.Themes.Default.Accents.BaseLight.xaml?assembly=Avalonia.Themes.Default"));
                result.Add(baseLight);
                
                var defaultDataGrid = (IStyle)AvaloniaXamlLoader.Load(
                    new Uri("resm:Avalonia.Controls.DataGrid.Themes.Default.xaml?assembly=Avalonia.Controls.DataGrid"));
                result.Add(defaultDataGrid);

                return result;
            };
        }
    }
}
