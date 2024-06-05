using Database;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace JewelyShop.Components.Pages
{
    /// <summary>
    /// Логика взаимодействия для SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        private static Database.TradeEntities database;
        private bool isRequireCaptcha;
        private readonly Random random;
        private string captchaCode;
        private readonly string captchaSymbols = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890";

        private DispatcherTimer timer;
        private bool isBlocked;

        private string fullName;


        public SignIn(Database.TradeEntities entities)
        {
            InitializeComponent();

            database = entities;
            random = new Random();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            isBlocked = false;
        }

        public string getUserFullName()
        {
            return fullName;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            isBlocked = false;
            timer.Stop();
        }

        private void OnInputChange(object sender, RoutedEventArgs e)
        {
            bSignIn.IsEnabled = tbLogin.Text.Trim().Length > 0 && pbPassword.Password.Trim().Length > 0;
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            if (isBlocked)
            {
                MessageBox.Show("Попробуйте еще раз через 10 секунд.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (isRequireCaptcha && captchaCode.ToLower() != tbCaptcha.Text.Trim().ToLower())
            {
                MessageBox.Show("Капча введена не правильно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                tbCaptcha.Clear();
                isBlocked = true;
                timer.Start();
                GenerateCapthca();
                return;
            }

            var login = tbLogin.Text.Trim();
            var password = pbPassword.Password.Trim();

            tbLogin.Clear();
            pbPassword.Clear();
            isBlocked = false;

            var user = database.Users.Where(u => u.UserLogin == login && u.UserPassword == password).FirstOrDefault();

            if (user != null)
            {
                if (isRequireCaptcha)
                {
                    spCaptcha.Visibility = Visibility.Collapsed;
                    isRequireCaptcha = false;
                    tbCaptcha.Clear();
                }
                fullName = user.UserName + " " + user.UserSurname + " " + user.UserPatronymic;
                switch (user.UserRole)
                {
                    case 2:
                        {
                            GoToProductView();
                        }
                        break;
                    case 3:
                        {
                            GoToProductView();
                        }
                        break;

                    default: break;
                }
            }
            else
            {
                MessageBox.Show("Логин или пароль введены неверно. Попробуйте еще раз..", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                GenerateCapthca();
                spCaptcha.Visibility = Visibility.Visible;
                isRequireCaptcha = true;
                tbCaptcha.Clear();
                return;
            }
        }
        private void GuestSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (isRequireCaptcha)
            {
                spCaptcha.Visibility = Visibility.Collapsed;
                isRequireCaptcha = false;
                tbCaptcha.Clear();
            }
            fullName = "Гость";
            MessageBox.Show("Ты вошел как гость.", "Победа", MessageBoxButton.OK, MessageBoxImage.Information);
            GoToProductView();
        }

        private void GoToProductView()
        {
            var mainWindow = ViewManager.MainWindow;
            mainWindow.Show();
            mainWindow.AppFrame.Navigate(Components.ViewManager.ProductView);
            Window.GetWindow(this).Hide();
        }

        private void GenerateCapthca()
        {
            canvas.Children.Clear();
            captchaCode = GenerateCapthcaCode();

            for (int i = 0; i < captchaCode.Length; i++)
            {
                AddLetterToCanvas(i, captchaCode[i]);
            }

            DrawNoise();
        }

        private string GenerateCapthcaCode()
        {

            string code = "";

            for (int i = 0; i < 4; i++)
            {
                code += captchaSymbols[random.Next(captchaSymbols.Length)];
            }

            return code;
        }

        private void AddLetterToCanvas(int index, char ch)
        {
            Label label = new Label();
            label.FontSize = random.Next(24, 32);
            label.Width = 40;
            label.Height = 60;
            label.Foreground = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
            label.RenderTransformOrigin = new Point(0.5, 0.5);
            label.RenderTransform = new RotateTransform(random.Next(-20, 16));
            label.Content = ch.ToString();
            canvas.Children.Add(label);

            Canvas.SetLeft(label, 41 + index * 40);
        }

        private void DrawNoise()
        {
            for (int i = 0; i < 10; i++)
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
                line.X1 = random.Next(250);
                line.Y1 = random.Next(60);
                line.X2 = random.Next(250);
                line.Y2 = random.Next(60);
                line.StrokeThickness = random.Next(1, 3);

                canvas.Children.Add(line);
            }
        }

        private void RegenerateCapthcaClick(object sender, RoutedEventArgs e)
        {
            GenerateCapthca();
        }
    }
}
