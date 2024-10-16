using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using UserControl.Models;
using UserControl.ViewModels;

namespace UserControl
{
    public partial class MainWindow : Window
    {
        private FilterViewModel _viewModel;
        private DispatcherTimer _popupTimer;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new FilterViewModel();
            DataContext = _viewModel;

            _popupTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2) 
            };
            _popupTimer.Tick += (s, e) =>
            {
                ErrorPopup.IsOpen = false;
                _popupTimer.Stop();
            };
        }

        private void ShowPopup(string message)
        {
            PopupMessage.Text = message;
            ErrorPopup.IsOpen = true;
            _popupTimer.Start();  
        }

        private void ArticleFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                _viewModel.SetFilter(nameof(Product.Article), textBox.Text);
            }
        }

        private void NameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                _viewModel.SetFilter(nameof(Product.Name), textBox.Text);
            }
        }

        private void QuantityFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (decimal.TryParse(textBox.Text, out decimal quantity) && quantity >= 0)
                {
                    _viewModel.SetFilter(nameof(Product.Quantity), textBox.Text);
                }
                else
                {
                    ShowPopup("Please enter a valid non-negative number for Quantity.");
                    _viewModel.SetFilter(nameof(Product.Quantity), string.Empty);
                }
            }
        }

        private void UnitPriceFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (decimal.TryParse(textBox.Text, out decimal unitPrice) && unitPrice >= 0)
                {
                    _viewModel.SetFilter(nameof(Product.UnitPrice), textBox.Text);
                }
                else
                {
                    ShowPopup("Please enter a valid non-negative number for Unit Price.");
                    _viewModel.SetFilter(nameof(Product.UnitPrice), string.Empty);

                }
            }
        }

        private void ToggleAdvancedFilter_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.IsAdvancedFilterVisible = !_viewModel.IsAdvancedFilterVisible;
        }
    }
}
