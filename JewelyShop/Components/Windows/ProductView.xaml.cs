using Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JewelyShop.Components.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductView.xaml
    /// </summary>
    public partial class ProductView : Window
    {
        public static Database.TradeEntities database;
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> FilteredProducts { get; set; } 
        public ObservableCollection<Manufacturer> Manufacturers { get; set; }
        private string FullName;
        public ProductView(Database.TradeEntities entities, ObservableCollection<Product> Products)
        {

            InitializeComponent();

            // Биндинг с установкой ФИО для окна
            this.FullName = ViewManager.SignIn.getUserFullName();
            Binding bFullName = new Binding();
            bFullName.Source = this.FullName;
            tbFullName.SetBinding(TextBlock.TextProperty, bFullName);

            database = entities;

            // Бинлдинг для комбо-бокса с производителями
            Manufacturers = new ObservableCollection<Manufacturer>(database.Manufacturers)
            {
                new Manufacturer { ManufacturerID = 3, ManufacturerName = "Все производители" }
            };
           
            this.Products = Products;
            this.FilteredProducts = new ObservableCollection<Product>(this.Products);

            DataContext = this;
        }

        private void bLogout_Click(object sender, RoutedEventArgs e)
        {
            var signInWindow = ViewManager.SignIn;
            signInWindow.Show();
            Close();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterProducts();
            sortProductsByCost();
        }
        private void cbCostSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sortProductsByCost();
        }

        private void cbSortManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filterProducts();
            sortProductsByCost();
        }

        private void filterProducts()
        {
            var searchedText = tbSearch.Text.Trim().ToLower();
            var searchedTerms = searchedText.Split(' ');
            this.FilteredProducts.Clear();
            foreach (var product in this.Products)
            {
                bool match = searchedTerms.All(term =>
                    product.ProductName.ToLower().Contains(term) ||
                    product.ProductDescription.ToLower().Contains(term) ||
                    product.Manufacturer.ManufacturerName.ToLower().Contains(term) ||
                    product.ProductCost.ToString().Contains(term) ||
                    product.ProductQuantityInStock.ToString().Contains(term)
                );
                if (match)
                {
                    if (cbSortManufacturer.SelectedValue == Manufacturers[2])
                    {
                        this.FilteredProducts.Add(product);
                        continue;
                    }
                    if (cbSortManufacturer.SelectedItem == product.Manufacturer)
                    {
                        this.FilteredProducts.Add(product);
                    }
                }
            }
        }

        private void sortProductsByCost()
        {
            if (this.FilteredProducts == null)
            {
                return;
            }
            var selectedItem = cbSortCost.SelectedIndex;

            // Сортировка по выбранному критерию
            switch (selectedItem)
            {
                case 1: // По возрастанию
                    this.FilteredProducts = new ObservableCollection<Product>(this.FilteredProducts.OrderBy(p => p.ProductCost));
                    break;
                case 2: // По убыванию
                    this.FilteredProducts = new ObservableCollection<Product>(this.FilteredProducts.OrderByDescending(p => p.ProductCost));
                    break;
                default: // Без сортировки
                    filterProducts();
                    this.FilteredProducts = new ObservableCollection<Product>(this.FilteredProducts);
                    break;
            }
            lvProducts.ItemsSource = this.FilteredProducts;
        }
    }
}
