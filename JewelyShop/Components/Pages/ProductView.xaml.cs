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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JewelyShop.Components.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductView.xaml
    /// </summary>
    public partial class ProductView : Page
    {
        public static Database.TradeEntities database;
        public ObservableCollection<Product> Products { get; set; }
        public ProductView(Database.TradeEntities entities)
        {
            InitializeComponent();
            database = entities;
            Products = new ObservableCollection<Product>(database.Products);
            foreach (var elem in Products)
            {
                if (elem.ProductPhoto == "")
                {
                    elem.ProductPhoto = "/Media/Product/picture.png";
                } else
                {
                    elem.ProductPhoto = "/Media/Product/" + elem.ProductPhoto;
                }

            }
            DataContext = this;
        }

        private void bLogout_Click(object sender, RoutedEventArgs e)
        {
            var signInWindow = ViewManager.SignIn;
            signInWindow.Show();
            Window.GetWindow(this).Close();

           

        }
    }
}
