using JewelyShop.Components;
using JewelyShop.Components.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace JewelyShop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Вызов базовой реализации OnStartup
            base.OnStartup(e);

            // Использование ViewManager для отображения окна SignIn
            var signInWindow = ViewManager.SignIn;
            signInWindow.Show();
        }
    }
}
