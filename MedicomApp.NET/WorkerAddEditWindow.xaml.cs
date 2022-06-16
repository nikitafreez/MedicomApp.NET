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
    /// Логика взаимодействия c окном WorkerAddEditWindow.xaml
    /// </summary>
    public partial class WorkerAddEditWindow : Window
    {
        public WorkerAddEditWindow()
        {
            InitializeComponent();
        }
        DBHelper DBHelper = new DBHelper();
        private void addEditWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            Regex regexPhone = new Regex(@"[7-8]\d{10}");
            Regex regexInn = new Regex(@"\d{12}");
            Regex regexPassSeria = new Regex(@"\d{4}");
            Regex regexPassNum = new Regex(@"\d{6}");
            Regex regexEmail = new Regex(@"\S+@\S+\.\S+$");

            bool IsUnique = true;
            var workers = DBHelper.GetWorkersList();
            if (MainWindow.command == 1)
            {
                foreach (Worker worker in workers)
                {
                    if (worker.INN == innTextBox.Text || (worker.PassNum == passNumTextBox.Text && worker.PassSeria == passSeriaTextBox.Text))
                        IsUnique = false;
                }
            }
            else
            {
                foreach (Worker worker in workers)
                {
                    if (worker.INN == innTextBox.Text && (worker.PassNum == passNumTextBox.Text && worker.PassSeria == passSeriaTextBox.Text)
                        && worker.INN != MainWindow.workerEdit.INN && worker.PassNum == MainWindow.workerEdit.PassNum && worker.PassSeria == MainWindow.workerEdit.PassSeria)
                        IsUnique = false;
                }
            }

            if (regexInn.IsMatch(innTextBox.Text)
               && !string.IsNullOrWhiteSpace(nameTextBox.Text) && nameTextBox.Text.Length > 1 && nameTextBox.Text.Length < 20
               && !string.IsNullOrWhiteSpace(surnameTextBox.Text) && surnameTextBox.Text.Length > 1 && surnameTextBox.Text.Length < 20
               && patronymicTextBox.Text.Length < 20
               && regexPassSeria.IsMatch(passSeriaTextBox.Text)
               && regexPassNum.IsMatch(passNumTextBox.Text)
               && regexPhone.IsMatch(phoneNumTextBox.Text)
               && regexEmail.IsMatch(emailTextBox.Text)
               && IsUnique == true
               && positionComboBox.SelectedItem != null)
            {
                if (MainWindow.command == 1)
                {
                    DateTime date = (DateTime)datePickBox.SelectedDate;
                    DBHelper.AddWorker(innTextBox.Text, nameTextBox.Text, passNumTextBox.Text, passSeriaTextBox.Text, patronymicTextBox.Text, surnameTextBox.Text, date.ToString("yyyy-MM-dd"), positionComboBox.SelectedValue.ToString(), emailTextBox.Text, phoneNumTextBox.Text);
                }
                if (MainWindow.command == 2)
                {
                    DateTime date = (DateTime)datePickBox.SelectedDate;
                    DBHelper.EditWorker(innTextBox.Text, nameTextBox.Text, passNumTextBox.Text, passSeriaTextBox.Text, patronymicTextBox.Text, surnameTextBox.Text, date.ToString("yyyy-MM-dd"), positionComboBox.SelectedValue.ToString(), emailTextBox.Text, phoneNumTextBox.Text, MainWindow.workerEdit.Id);
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
            var positions = DBHelper.GetPositionsList();
            positionComboBox.ItemsSource = positions;
            positionComboBox.DisplayMemberPath = "PositionName";
            positionComboBox.SelectedValuePath = "Id";
            if (MainWindow.command == 2)
            {
                addEditWorkerButton.Content = "Изменить";
                innTextBox.Text = MainWindow.workerEdit.INN;
                nameTextBox.Text = MainWindow.workerEdit.Name;
                passNumTextBox.Text = MainWindow.workerEdit.PassNum;
                passSeriaTextBox.Text = MainWindow.workerEdit.PassSeria;
                patronymicTextBox.Text = MainWindow.workerEdit.Patronymic;
                surnameTextBox.Text = MainWindow.workerEdit.Surmame;
                emailTextBox.Text = MainWindow.workerEdit.Email;
                phoneNumTextBox.Text = MainWindow.workerEdit.PhoneNumber;
                var positionss = DBHelper.GetPositionsList();
                positionComboBox.SelectedValue = positionss.Where(x => x.PositionName == MainWindow.workerEdit.Position).FirstOrDefault().Id;
                datePickBox.SelectedDate = MainWindow.workerEdit.BirthDate;
            }
        }
    }
}
