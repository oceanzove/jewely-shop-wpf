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

            this.Products = Products;
            
            DataContext = this;
        }

        private void filter()
        {
            var view = CollectionViewSource.GetDefaultView(lvProducts.ItemsSource);
            if (view == null)
            {
                return;
            }
        }

        private void bLogout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = ViewManager.MainWindow;
            mainWindow.Close();

            var signInWindow = ViewManager.SignIn;
            signInWindow.Show();
        }

    }
}
