using JewelyShop.Components.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JewelyShop.Components
{
    public class ViewManager
    {
        private static Database.TradeEntities database;

        // Окна
        private static SignIn signIn;

        // Страницы
        private static ProductView productView;

        private static Database.TradeEntities Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database.TradeEntities();
                    if (database.Database.Exists() == false)
                    {
                        MessageBox.Show("Подключения к базе данных не было выполнено. Приложения будет завершено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                return database;
            }
        }

        // Окна
        public static SignIn SignIn
        {
            get
            {
                if (signIn == null)
                {
                    signIn = new SignIn(Database);
                }
                return signIn;
            }
        }
        // Старницы
        public static ProductView ProductView
        {
            get
            {
                if (productView == null)
                {
                    productView = new ProductView(Database);
                }
                return productView;
            }
        }

        
    }
}
