using MedicomApp.NET.Models;
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
    /// Логика взаимодействия для DiseaseAddEditWindow.xaml
    /// </summary>
    public partial class DiseaseAddEditWindow : Window
    {
        public DiseaseAddEditWindow()
        {
            InitializeComponent();
        }

        DBHelper dbHelper = new DBHelper();
        private void addEditDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            bool IsUnique = true;
            var diseases = dbHelper.GetDiseasesList();
            foreach(Disease disease in diseases)
            {
                if(disease.DiseaseName == nameTextBox.Text)
                    IsUnique = false;
            }

            if (!string.IsNullOrWhiteSpace(nameTextBox.Text) && nameTextBox.Text.Length > 1 && nameTextBox.Text.Length < 50 && IsUnique == true
                && descTextBox.Text.Length < 200)
            {
                if (MainWindow.command == 1)
                {
                    dbHelper.AddDisease(nameTextBox.Text, descTextBox.Text);
                    this.Close();
                }
                else if (MainWindow.command == 2)
                {
                    dbHelper.EditDisease(nameTextBox.Text, descTextBox.Text, MainWindow.diseaseEdit.Id);

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(MainWindow.command == 2)
            {
                addEditDiseaseButton.Content = "Изменить";
                nameTextBox.Text = MainWindow.diseaseEdit.DiseaseName;
                descTextBox.Text = MainWindow.diseaseEdit.DiseaseDescription;
            }
        }
    }
}
