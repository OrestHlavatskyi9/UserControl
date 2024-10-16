using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;
using UserControl.Models;

namespace UserControl.ViewModels;

public class FilterViewModel : INotifyPropertyChanged
{
    private string _filterText;
    private readonly Dictionary<string, string> _filters = new();
    private bool _isAdvancedFilterVisible;

    public string FilterText
    {
        get => _filterText;
        set
        {
            _filterText = value;
            OnPropertyChanged(nameof(FilterText));
            ApplyFilter();
        }
    }

    public bool IsAdvancedFilterVisible
    {
        get => _isAdvancedFilterVisible;
        set
        {
            _isAdvancedFilterVisible = value;
            OnPropertyChanged(nameof(IsAdvancedFilterVisible));
        }
    }

    public ObservableCollection<Product> Products { get; set; }
    public ICollectionView FilteredProducts { get; private set; }

    public FilterViewModel()
    {
        Products = new ObservableCollection<Product>
        {
            new Product(1, "SOF453", "Apple", 10.0m, 2.3m),
            new Product(2, "SVG137", "Banana", 35.0m, 3.5m),
            new Product(3, "SOF454", "Cherry", 20.0m, 1.5m),
            new Product(4, "SOF455", "Date", 25.0m, 2.0m),
            new Product(5, "SOF456", "Elderberry", 30.0m, 3.0m),
            new Product(6, "SOF457", "Fig", 15.0m, 1.8m),
            new Product(7, "SVG138", "Grape", 50.0m, 2.5m),
            new Product(8, "SOF458", "Honeydew", 12.0m, 1.9m),
            new Product(9, "SVG139", "Kiwi", 40.0m, 4.0m),
            new Product(10, "SOF459", "Lemon", 8.0m, 1.2m),
            new Product(11, "SVG140", "Mango", 28.0m, 2.8m),
            new Product(12, "SOF460", "Nectarine", 22.0m, 3.1m),
            new Product(13, "SVG141", "Orange", 55.0m, 2.6m),
            new Product(14, "SOF461", "Papaya", 16.0m, 2.7m),
            new Product(15, "SVG142", "Quince", 10.0m, 2.0m),
            new Product(16, "SOF462", "Raspberry", 13.0m, 5.2m),
            new Product(17, "SVG143", "Strawberry", 45.0m, 4.5m),
            new Product(18, "SOF463", "Tomato", 18.0m, 1.6m),
            new Product(19, "SVG144", "Watermelon", 60.0m, 3.8m),
            new Product(20, "SOF464", "Zucchini", 9.0m, 1.4m)
        };

        FilteredProducts = CollectionViewSource.GetDefaultView(Products);
    }

    public void SetFilter(string propertyName, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            _filters.Remove(propertyName);
        }
        else
        {
            _filters[propertyName] = value;
        }
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        if (FilteredProducts == null) return;

        FilteredProducts.Filter = item =>
        {
            if (item is Product product)
            {
                bool matchesFilter = string.IsNullOrEmpty(FilterText) ||
                                     product.GetType()
                                         .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                         .Any(prop =>
                                         {
                                             var propValue = prop.GetValue(product)?.ToString()?.ToLower();
                                             return propValue != null && propValue.Contains(FilterText.ToLower());
                                         });

                foreach (var filter in _filters)
                {
                    var property = product.GetType().GetProperty(filter.Key, BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        var propValue = property.GetValue(product)?.ToString()?.ToLower();
                        matchesFilter &= propValue != null && propValue.Contains(filter.Value.ToLower());
                    }
                }

                return matchesFilter;
            }
            return false;
        };

        FilteredProducts.Refresh();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
