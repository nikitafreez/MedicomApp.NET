using MedicomApp.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для PatientAddEditWindow.xaml
    /// </summary>
    public partial class PatientAddEditWindow : Window
    {
        public PatientAddEditWindow()
        {
            InitializeComponent();
        }

        DBHelper DBHelper = new DBHelper();

        private void addEditPatientButton_Click(object sender, RoutedEventArgs e)
        {
            Regex regexPhone = new Regex(@"[7-8]\d{10}");
            Regex regexOms = new Regex(@"\d{16}");
            Regex regexPassSeria = new Regex(@"\d{4}");
            Regex regexPassNum = new Regex(@"\d{6}");
            Regex regexEmail = new Regex(@"\S+@\S+\.\S+$");

            bool IsUnique = true;
            var patients = DBHelper.GetPatientsList();
            if (MainWindow.command == 1)
            {
                foreach (Patient patient in patients)
                {
                    if (patient.OMS == omsTextBox.Text || (patient.PassNum == passNumTextBox.Text && patient.PassSeria == passSeriaTextBox.Text))
                        IsUnique = false;
                }
            }
            else
            {
                foreach (Patient patient in patients)
                {
                    if (patient.OMS == omsTextBox.Text && (patient.PassNum == passNumTextBox.Text && patient.PassSeria == passSeriaTextBox.Text)
                        && patient.OMS == MainWindow.patientEdit.OMS && patient.PassNum == MainWindow.patientEdit.PassNum && patient.PassSeria == MainWindow.patientEdit.PassSeria)
                        IsUnique = false;
                }
            }

            if (regexOms.IsMatch(omsTextBox.Text)
               && !string.IsNullOrWhiteSpace(nameTextBox.Text) && nameTextBox.Text.Length > 1 && nameTextBox.Text.Length < 20
               && !string.IsNullOrWhiteSpace(surnameTextBox.Text) && surnameTextBox.Text.Length > 1 && surnameTextBox.Text.Length < 20
               && patronymicTextBox.Text.Length < 20
               && regexPassSeria.IsMatch(passSeriaTextBox.Text)
               && regexPassNum.IsMatch(passNumTextBox.Text)
               && regexPhone.IsMatch(phoneNumTextBox.Text)
               && regexEmail.IsMatch(emailTextBox.Text)
               && IsUnique == true)
            {
                if (MainWindow.command == 1)
                {
                    DateTime date = (DateTime)datePickBox.SelectedDate;
                    DBHelper.AddPatient(omsTextBox.Text, nameTextBox.Text, passNumTextBox.Text, passSeriaTextBox.Text, patronymicTextBox.Text, surnameTextBox.Text, date.ToString("yyyy-MM-dd"), emailTextBox.Text, phoneNumTextBox.Text);
                }
                if (MainWindow.command == 2)
                {
                    DateTime date = (DateTime)datePickBox.SelectedDate;
                    DBHelper.EditPatient(omsTextBox.Text, nameTextBox.Text, passNumTextBox.Text, passSeriaTextBox.Text, patronymicTextBox.Text, surnameTextBox.Text, date.ToString("yyyy-MM-dd"), emailTextBox.Text, phoneNumTextBox.Text, MainWindow.patientEdit.Id);
                }
                this.Close();
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

            if (MainWindow.command == 1)
            {
                datePickBox.SelectedDate = DateTime.UtcNow;
            }
            if (MainWindow.command == 2)
            {
                addEditPatientButton.Content = "Изменить";
                omsTextBox.Text = MainWindow.workerEdit.INN;
                nameTextBox.Text = MainWindow.workerEdit.Name;
                passNumTextBox.Text = MainWindow.workerEdit.PassNum;
                passSeriaTextBox.Text = MainWindow.workerEdit.PassSeria;
                patronymicTextBox.Text = MainWindow.workerEdit.Patronymic;
                surnameTextBox.Text = MainWindow.workerEdit.Surmame;
                emailTextBox.Text = MainWindow.workerEdit.Email;
                phoneNumTextBox.Text = MainWindow.workerEdit.PhoneNumber;
                datePickBox.SelectedDate = MainWindow.workerEdit.BirthDate;
            }
        }
    }
}
