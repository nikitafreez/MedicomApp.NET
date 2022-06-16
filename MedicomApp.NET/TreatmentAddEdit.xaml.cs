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
    /// Логика взаимодействия для TreatmentAddEdit.xaml
    /// </summary>
    public partial class TreatmentAddEdit : Window
    {
        public TreatmentAddEdit()
        {
            InitializeComponent();
        }
        DBHelper DBHelper = new DBHelper();
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addEditTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (workerComboBox.SelectedItem != null && patientComboBox.SelectedItem != null && diseaseComboBox.SelectedItem != null)
            {
                if (MainWindow.command == 1)
                {
                    DBHelper.AddTreatment(datePickBox.SelectedDate?.ToString("yyyy-MM-dd"), sumTextBox.Text, diseaseComboBox.SelectedValue.ToString(), patientComboBox.SelectedValue.ToString(), workerComboBox.SelectedValue.ToString());
                    this.Close();
                }
                if (MainWindow.command == 2)
                {
                    DBHelper.EditTreatment(datePickBox.SelectedDate?.ToString("yyyy-MM-dd"), sumTextBox.Text, diseaseComboBox.SelectedValue.ToString(), patientComboBox.SelectedValue.ToString(), workerComboBox.SelectedValue.ToString(), MainWindow.treatmentEdit.Id);
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
            patientComboBox.ItemsSource = DBHelper.GetPatientsList();
            patientComboBox.DisplayMemberPath = "FIO";
            patientComboBox.SelectedValuePath = "Id";

            workerComboBox.ItemsSource = DBHelper.GetWorkersList();
            workerComboBox.DisplayMemberPath = "FIO";
            workerComboBox.SelectedValuePath = "Id";

            diseaseComboBox.ItemsSource = DBHelper.GetDiseasesList();
            diseaseComboBox.DisplayMemberPath = "DiseaseName";
            diseaseComboBox.SelectedValuePath = "Id";


            if(MainWindow.command == 1)
            {
                datePickBox.SelectedDate = DateTime.UtcNow;
            }
            if (MainWindow.command == 2)
            {
                addEditTreatmentButton.Content = "Изменить";
                sumTextBox.Text = MainWindow.treatmentEdit.TreatmentSum.ToString();
                datePickBox.SelectedDate = MainWindow.treatmentEdit.TreatmentDate;
                patientComboBox.SelectedValue = DBHelper.GetPatientsList().Where(x => x.FIO == MainWindow.treatmentEdit.Patient).FirstOrDefault().Id;
                workerComboBox.SelectedValue = DBHelper.GetWorkersList().Where(x => x.FIO == MainWindow.treatmentEdit.Worker).FirstOrDefault().Id;
                diseaseComboBox.SelectedValue = DBHelper.GetDiseasesList().Where(x => x.DiseaseName == MainWindow.treatmentEdit.DiseaseName).FirstOrDefault().Id;
            }
        }
    }
}
