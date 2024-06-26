﻿using Database;
using JewelyShop.Components.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                if (signIn == null || !signIn.IsVisible)
                {
                    signIn = new SignIn(Database);
                }
                return signIn;
            }
        }
        public static ProductView ProductView
        {
            get
            {
                if (productView == null || !productView.IsVisible)
                {
                    productView = new ProductView(Database);
                }
                return productView;
            }
        }
    }
}
