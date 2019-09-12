using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    public class ButtonPage : UserControl
    {
        private Decorator _first;
        private Decorator _second;
        private IControl _control;
        private bool _isToggled = true;

        public ButtonPage()
        {
            AvaloniaXamlLoader.Load(this);

            _first = this.FindControl<Decorator>("First");
            _second = this.FindControl<Decorator>("Second");
            _control = this.FindControl<IControl>("Ctrl");
        }

        public void OnToggle(object sender, RoutedEventArgs e)
        {
            if (_isToggled)
            {
                _first.Child = null;
                _second.Child = _control;
            }
            else
            {
                _second.Child = null;
                _first.Child = _control;
            }

            _isToggled = !_isToggled;
        }
    }
}
