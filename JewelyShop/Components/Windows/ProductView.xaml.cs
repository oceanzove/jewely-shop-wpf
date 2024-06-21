using Database;
using JewelyShop.Components.Frame;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Xml.Linq;

namespace JewelyShop.Components.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductView.xaml
    /// </summary>
    public partial class ProductView : Window
    {
        public static Database.TradeEntities database;
        public ObservableCollection<Product> FilteredProducts { get; set; } 
        private ObservableCollection<Manufacturer> Manufacturers { get; set; }
        public ObservableCollection<Manufacturer> SotrtedManufacturers { get; set; }
        public int ProductsCount;
        public int SelectedProductsCount;


        private Database.User User;
        private string FullName;

        public ProductView(Database.TradeEntities entities)
        {

            InitializeComponent();

           

            database = entities;
            DataContext = this;

            setManufacturers();

            this.FilteredProducts = new ObservableCollection<Product>(database.Products);

            updateProducts();
            setUser();
        }

        public void updateProducts()
        {
            if (lvProducts != null)
            {
                lvProducts.Items.Clear();
            }
            if (this.FilteredProducts != null)
            {
                foreach (var product in this.FilteredProducts)
                {
                    ProductFrame productFrame = new ProductFrame(product, this.Manufacturers, User?.Role, this);
                    lvProducts.Items.Add(productFrame);
                }
            }
        }

        public void saveProduct(string name, string description, Database.Manufacturer manufacturer, decimal cost, int quantityInStock, Database.Product product)
        {
            if (name != product.ProductName)
            {
                product.ProductName = name;
            }
            if (description != product.ProductDescription)
            {
                product.ProductDescription = description;
            }
            if (manufacturer != product.Manufacturer)
            {
                product.Manufacturer = manufacturer;
            }
            if (cost != product.ProductCost)
            {
                product.ProductCost = cost;
            }
            if (quantityInStock != product.ProductQuantityInStock)
            {
                product.ProductQuantityInStock = quantityInStock;
            }
            database.SaveChanges();
            updateProducts();
        }

        public void deleteProduct(Database.Product product)
        {
            try
            {
                var itemOrdered = database.OrderProducts.Where(p => p.ProductArticleNumber == product.ProductArticleNumber).FirstOrDefault();
                if (itemOrdered != null)
                {
                    MessageBox.Show("Удаление товара " + product.ProductArticleNumber + " " + product.ProductName + " не возможно", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                database.Products.Remove(product);
                this.FilteredProducts.Remove(product);
                database.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Удаление товара " + product.ProductArticleNumber + " " + product.ProductName + " не возможно", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            setProductCounts();
            updateProducts();
        }

        private void setUser()
        {
             this.User = ViewManager.SignIn.getUser();
            if (User == null)
            {
                this.FullName = "Гость";
            } else
            {
                this.FullName = User.Role.RoleName + ": " + User.UserName + " " + User.UserSurname + " " + User.UserPatronymic;
            }

            Binding bFullName = new Binding();
            bFullName.Source = this.FullName;
            tbFullName.SetBinding(TextBlock.TextProperty, bFullName);
        }

        private void bLogout_Click(object sender, RoutedEventArgs e)
        {
            var signInWindow = ViewManager.SignIn;
            signInWindow.Show();
            Close();
        }

        private void applyFilter_Changed(object sender, TextChangedEventArgs e)
        {
            applyFilter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            applyFilter();
        }


        private void applyFilter()
        {
            filterProducts();
            sortProductsByCost();
            setProductCounts();
            updateProducts();
        }

        private void setProductCounts()
        {
            setProductsCount();
            setSelectedProductsCount();
        }

        private void filterProducts()
        {
            if (this.FilteredProducts == null)
            {
                return;
            }
            var searchedText = tbSearch.Text.Trim().ToLower();
            var searchedTerms = searchedText.Split(' ');
            this.FilteredProducts.Clear();
            foreach (var product in database.Products)
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
                    if (cbSortManufacturer.SelectedValue == SotrtedManufacturers[2])
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
                    this.FilteredProducts = new ObservableCollection<Product>(this.FilteredProducts);
                    break;
            }
        }

        private void setProductsCount()
        {
            if (this.FilteredProducts == null)
            {
                return;
            }
            Binding bCount = new Binding();
            bCount.Source = database.Products.Count();
            tbProductsCount.SetBinding(TextBlock.TextProperty, bCount);
        }
        private void setSelectedProductsCount()
        {
            if (this.FilteredProducts == null)
            {
                return;
            }
            Binding bCount = new Binding();
            bCount.Source = this.FilteredProducts.Count();
            tbSelectedProductsCount.SetBinding(TextBlock.TextProperty, bCount);
        }

        private void setManufacturers()
        {
            Manufacturers = new ObservableCollection<Manufacturer>(database.Manufacturers);
            SotrtedManufacturers = new ObservableCollection<Manufacturer>(database.Manufacturers)
            {
                new Manufacturer { ManufacturerID = 3, ManufacturerName = "Все производители" }
            };
        }
        
    }
}
