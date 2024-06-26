﻿using Database;
using JewelyShop.Components.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JewelyShop.Components.Frame
{
    /// <summary>
    /// Логика взаимодействия для ProductFrame.xaml
    /// </summary>
    public partial class ProductFrame : UserControl
    {
        private const int ADMIN_ROLE = 1;
        private const int MANAGER_ROLE = 2;
        private const int CUSTOMER_ROLE = 3;

        private readonly Database.Product Product;
        private readonly Database.Role Role;
        private readonly ProductView ProductView;

        public string ProductName { get; set; }
        public string ProductPhoto { get; set; }
        public Database.Manufacturer Manufacturer { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductCost { get; set; }
        public int ProductQuantityInStock { get; set; }

        public ObservableCollection<Manufacturer> Manufacturers { get; set; }


        public ProductFrame(Database.Product product, ObservableCollection<Manufacturer> manufacturers, Database.Role role, ProductView productView)
        {
            InitializeComponent();

            this.Product = product;
            this.Manufacturers = manufacturers;
            this.Role = role;
            this.ProductView = productView;

            

            DataContext = this;

            setProductContext();
            setVisability();

        }

        private void setVisability()
        {
            if (Role == null)
            {
                return;
            }
            switch (Role.RoleID)
            {
                case ADMIN_ROLE:
                    // Админ
                    setAdminVisability();
                    break;
                case MANAGER_ROLE:
                    // Менеджер
                    break;
                default:
                    // Покупатель
                    break;
            }
        }

        private void setProductContext()
        {
            if (Product == null) return;

            ProductQuantityInStock = Product.ProductQuantityInStock;
            ProductName = Product.ProductName;
            ProductDescription = Product.ProductDescription;
            if (Product.Manufacturer != null)
            {
                Manufacturer = Product.Manufacturer;
            }
            ProductCost = Product.ProductCost;

            if (Product.ProductPhoto.Length < 1)
            {
                ProductPhoto = "/Media/Product/picture.png";
            }
            else
            {
                ProductPhoto = "/Media/Product/" + Product.ProductPhoto;

            }
        }
        private void setAdminVisability()
        {
            bAdminDeleteButton.Visibility = Visibility.Visible;
            bAdminEditButton.Visibility = Visibility.Visible;
        }

        private void deleteProduct_Click(object sender, RoutedEventArgs e)
        {
            this.ProductView.deleteProduct(this.Product);
        }

        private void ToogleEditMode_Click(object sender, RoutedEventArgs e)
        {
            if (Role.RoleID != ADMIN_ROLE)
            {
                return;
            }
            toggleEditMode();
        }
        
        private void toggleEditMode()
        {
            if (gProductEdit.Visibility == Visibility.Visible)
            {
                bAdminEditButton.RenderTransform = new RotateTransform(0, bAdminEditButton.ActualWidth / 2, bAdminEditButton.ActualHeight / 2);
                gProductEdit.Visibility = Visibility.Collapsed;
            }
            else
            {
                bAdminEditButton.RenderTransform = new RotateTransform(180, bAdminEditButton.ActualWidth / 2, bAdminEditButton.ActualHeight / 2);
                gProductEdit.Visibility = Visibility.Visible;
            }
        }

        private void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            var name = tbName.Text.Trim();
            var description = tbDescription.Text.Trim();
            var manufacturer = cbManufacturer.SelectedItem as Database.Manufacturer;
            decimal cost;
            if (!decimal.TryParse(tbCost.Text.Trim().Replace('.', ','), out cost))
            {
                // Обработка ошибки: вывод сообщения или выполнение другого действия
                MessageBox.Show("Введите корректное значение для стоимости.");
                return;
            }
            var quantityInStock = int.Parse(tbQuantityInStock.Text.Trim());

            this.ProductView.saveProduct(name, description, manufacturer, cost, quantityInStock, this.Product);
            toggleEditMode();
        }
    }
}
