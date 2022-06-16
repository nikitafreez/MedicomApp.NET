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
    /// Логика взаимодействия для PositionAddEditWindow.xaml
    /// </summary>
    public partial class PositionAddEditWindow : Window
    {
        public PositionAddEditWindow()
        {
            InitializeComponent();
        }
        DBHelper dbHelper = new DBHelper();
        private void addEditPositionButton_Click(object sender, RoutedEventArgs e)
        {
            bool IsUnique = true;
            var positions = dbHelper.GetPositionsList();
            if (MainWindow.command == 1)
            {
                foreach (var position in positions)
                {
                    if (position.PositionName == nameTextBox.Text)
                        IsUnique = false;
                }
            }
            else
            {
                foreach (var position in positions)
                {
                    if (position.PositionName == nameTextBox.Text && position.PositionName != MainWindow.positionEdit.PositionName)
                        IsUnique = false;
                }
            }

            if (IsUnique == true
                && !string.IsNullOrWhiteSpace(nameTextBox.Text) && nameTextBox.Text.Length > 2 && nameTextBox.Text.Length < 30)
            {
                if (MainWindow.command == 1)
                {
                    dbHelper.PositionAdd(nameTextBox.Text);

                    this.Close();
                }
                if (MainWindow.command == 2)
                {
                    dbHelper.PositionEdit(nameTextBox.Text, MainWindow.positionEdit.Id);

                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Вы ввели некорретные данные", "Ошибка ввода");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(MainWindow.command == 2)
            {
                addEditPositionButton.Content = "Изменить";
                nameTextBox.Text = MainWindow.positionEdit.PositionName;
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
