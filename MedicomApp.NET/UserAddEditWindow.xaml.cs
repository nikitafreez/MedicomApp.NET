using System;
using System.Collections.Generic;
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

namespace MedicomApp.NET
{
    /// <summary>
    /// Логика взаимодействия для UserAddEditWindow.xaml
    /// </summary>
    public partial class UserAddEditWindow : Window
    {
        public UserAddEditWindow()
        {
            InitializeComponent();
        }

        DBHelper dbHelper = new DBHelper();
        private void addEditUserButton_Click(object sender, RoutedEventArgs e)
        {
            bool IsUnique = true;
            var users = dbHelper.GetUsersList();
            if (MainWindow.command == 1)
            {
                foreach (var user in users)
                {
                    if (user.UserName == loginTextBox.Text)
                        IsUnique = false;
                }
            }
            else
            {
                foreach (var user in users)
                {
                    if (user.UserName == loginTextBox.Text && user.UserName != MainWindow.editUser.UserName)
                        IsUnique = false;
                }
            }

            if (IsUnique == true
               && loginTextBox.Text.Length > 4 && loginTextBox.Text.Length < 13
               && passwordTextBox.Text.Length > 3 && passwordTextBox.Text.Length < 25)
            {
                if (MainWindow.command == 1)
                {
                    string roleName;
                    if (adminItem.IsSelected)
                        roleName = "ADMIN";
                    else if (doctorItem.IsSelected)
                        roleName = "DOCTOR";
                    else if (kadrItem.IsSelected)
                        roleName = "KADR";
                    else
                        roleName = "USER";
                    dbHelper.AddUser(loginTextBox.Text, HashPassword(passwordTextBox.Text), roleName);

                    this.Close();
                }
                if (MainWindow.command == 2)
                {
                    dbHelper.EditUser(loginTextBox.Text, HashPassword(passwordTextBox.Text), MainWindow.editUser.Id);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Вы ввели некорретные данные", "Ошибка ввода");
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(8));
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(MainWindow.command == 2)
            {
                loginTextBox.Text = MainWindow.editUser.UserName;
                addEditUserButton.Content = "Изменить";
                comboLabel.Visibility = Visibility.Collapsed;
                roleComboBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}
