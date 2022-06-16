using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using BCrypt.Net;


namespace MedicomApp.NET
{
    /// <summary>
    /// Логика взаимодействия для AutharizationWindow.xaml
    /// </summary>
    public partial class AutharizationWindow : Window
    {
        public AutharizationWindow()
        {
            InitializeComponent();
        }

        public static string userRole = "";
        private void loginButtin_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG
            if (loginTextBox.Text == "")
            {
                loginTextBox.Text = "admin";
                passwordTextBox.Password = "admin";
            }
#endif
            DBHelper dbHelper = new DBHelper();
            var users = dbHelper.GetUsersList();

            var userToLogin = users.Where(x => x.UserName == loginTextBox.Text).ToList().FirstOrDefault();
            if(userToLogin != null && ValidatePassword(passwordTextBox.Password, userToLogin.Password))
            {
                userRole = userToLogin.RoleName;
                if (userRole != "USER")
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Доступ для пользователей с ролью USER запрещён.", "Авторизация");
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Авторизация");
            }

        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(8));
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
    }
}
