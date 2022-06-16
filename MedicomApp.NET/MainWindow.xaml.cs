using MedicomApp.NET.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MedicomApp.NET
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        DBHelper dbHelper = new DBHelper();

        List<User> users = new List<User>();
        List<Role> roles = new List<Role>();
        List<Disease> diseases = new List<Disease>();
        List<Worker> workers = new List<Worker>();
        List<Position> positions = new List<Position>();
        List<Patient> patients = new List<Patient>();
        List<Treatment> treatments = new List<Treatment>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAllTables();

            if (AutharizationWindow.userRole == "ADMIN")
            {

            }
            if (AutharizationWindow.userRole == "DOCTOR")
            {
                PatientTab.IsSelected = true;
                UsersTab.Visibility = Visibility.Collapsed;
                RolesTab.Visibility = Visibility.Collapsed;
                WorkerTab.Visibility = Visibility.Collapsed;
                PositionTab.Visibility = Visibility.Collapsed;
            }
            if (AutharizationWindow.userRole == "KADR")
            {
                WorkerTab.IsSelected = true;
                UsersTab.Visibility = Visibility.Collapsed;
                RolesTab.Visibility = Visibility.Collapsed;
                DiseaseTab.Visibility = Visibility.Collapsed;
                PatientTab.Visibility = Visibility.Collapsed;
                TreatmentTab.Visibility = Visibility.Collapsed;
            }
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
            else
                return;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            AutharizationWindow autharizationWindow = new AutharizationWindow();
            autharizationWindow.Show();
            this.Close();
        }

        private void userSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var users = dbHelper.GetUsersList();

            users = users.Where(x => x.UserName.ToLower().Contains(userSearch.Text.ToLower()) || x.Id.ToString() == userSearch.Text || x.RoleName.ToLower().Contains(userSearch.Text.ToLower())).ToList();

            userDataGrid.ItemsSource = users;
        }
        public static int command = 0;
        public static User editUser;
        private void addUserButton_Click(object sender, RoutedEventArgs e)
        {
            command = 1;
            UserAddEditWindow userAddEditWindow = new UserAddEditWindow();

            userAddEditWindow.Show();
        }

        private void editUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                command = 2;
                editUser = userDataGrid.SelectedItem as User;
                if (editUser != null)
                {
                    if (editUser.UserName != "admin")
                    {
                        UserAddEditWindow userAddEditWindow = new UserAddEditWindow();

                        userAddEditWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Нельзя редактировать пользователя с логином admin", "Ошибка");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        private void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = userDataGrid.SelectedItem as User;
                if (user != null)
                {
                    if (user.UserName != "admin")
                    {
                        dbHelper.DeleteUser(user.Id);
                        userDataGrid.ItemsSource = dbHelper.GetUsersList();
                    }
                    else
                    {
                        MessageBox.Show("Нельзя удалить пользователя с логином admin", "Ошибка");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        private void roleSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var roles = dbHelper.GetRolesList();

            roles = roles.Where(x => x.RoleName.ToLower().Contains(roleSearch.Text.ToLower())).ToList();

            roleDataGrid.ItemsSource = roles;
        }

        private void diseaseSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var disease = dbHelper.GetDiseasesList();

            disease = disease.Where(x => x.DiseaseName.ToLower().Contains(diseaseSearch.Text.ToLower()) || x.DiseaseDescription.ToLower().Contains(diseaseSearch.Text.ToLower())).ToList();

            diseaseDataGrid.ItemsSource = disease;
        }

        private void addDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            command = 1;
            DiseaseAddEditWindow diseaseAddEditWindow = new DiseaseAddEditWindow();

            diseaseAddEditWindow.Show();
        }

        public static Disease diseaseEdit;
        private void editDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                command = 2;
                diseaseEdit = diseaseDataGrid.SelectedItem as Disease;
                DiseaseAddEditWindow diseaseAddEditWindow = new DiseaseAddEditWindow();
                if (diseaseEdit != null)
                    diseaseAddEditWindow.Show();
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        private void deleteDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var disease = diseaseDataGrid.SelectedItem as Disease;
                if (disease != null)
                {
                    dbHelper.DeleteDisease(disease.Id);
                    diseaseDataGrid.ItemsSource = dbHelper.GetDiseasesList();
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllTables();
        }

        private void UpdateAllTables()
        {
            roles = dbHelper.GetRolesList();
            users = dbHelper.GetUsersList();
            diseases = dbHelper.GetDiseasesList();
            workers = dbHelper.GetWorkersList();
            positions = dbHelper.GetPositionsList();
            patients = dbHelper.GetPatientsList();
            treatments = dbHelper.GetTreatmentsList();

            userDataGrid.ItemsSource = users;
            roleDataGrid.ItemsSource = roles;
            diseaseDataGrid.ItemsSource = diseases;
            workerDataGrid.ItemsSource = workers;
            positionDataGrid.ItemsSource = positions;
            patientDataGrid.ItemsSource = patients;
            treatmentDataGrid.ItemsSource = treatments;
        }

        private void deleteTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var treatment = treatmentDataGrid.SelectedItem as Treatment;
                if (treatment != null)
                {
                    dbHelper.DeleteTreatment(treatment.Id);

                    treatmentDataGrid.ItemsSource = dbHelper.GetTreatmentsList();
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        public static Treatment treatmentEdit;
        private void editTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                command = 2;
                treatmentEdit = treatmentDataGrid.SelectedItem as Treatment;
                if (treatmentEdit != null)
                {
                    TreatmentAddEdit treatmentAddEdit = new TreatmentAddEdit();

                    treatmentAddEdit.Show();
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        private void addTreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            command = 1;
            TreatmentAddEdit treatmentAddEdit = new TreatmentAddEdit();

            treatmentAddEdit.Show();
        }

        private void treatmentSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var treatments = dbHelper.GetTreatmentsList();
            treatments = treatments.Where(x => x.TreatmentSum.ToString().Contains(treatmentSearch.Text.ToLower()) || x.DiseaseName.ToLower().Contains(treatmentSearch.Text.ToLower()) ||
            x.Patient.ToLower().Contains(treatmentSearch.Text.ToLower()) || x.Worker.ToLower().Contains(treatmentSearch.Text.ToLower())).ToList();
        }

        private void addPatientButton_Click(object sender, RoutedEventArgs e)
        {
            command = 1;
            PatientAddEditWindow patientAddEditWindow = new PatientAddEditWindow();

            patientAddEditWindow.Show();
        }

        public static Patient patientEdit;
        private void editPatientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                command = 2;
                patientEdit = patientDataGrid.SelectedItem as Patient;
                if (patientEdit != null)
                {
                    PatientAddEditWindow patientAddEditWindow = new PatientAddEditWindow();

                    patientAddEditWindow.Show();
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        private void deletePatientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var patient = patientDataGrid.SelectedItem as Patient;
                if (patient != null)
                {
                    dbHelper.DeletePatient(patient.Id);

                    patientDataGrid.ItemsSource = dbHelper.GetPatientsList();
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        private void patientSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var patients = dbHelper.GetPatientsList();
            patients = patients.Where(x => x.FIO.ToLower().Contains(patientSearch.Text.ToLower()) || x.Email.ToLower().Contains(patientSearch.Text.ToLower()) || x.OMS.ToLower().Contains(patientSearch.Text.ToLower()) ||
            x.PassNum.ToLower().Contains(patientSearch.Text.ToLower()) || x.PassSeria.ToLower().Contains(patientSearch.Text.ToLower()) || x.PhoneNumber.ToLower().Contains(patientSearch.Text.ToLower())).ToList();

            patientDataGrid.ItemsSource = patients;
        }

        private void positionSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var positions = dbHelper.GetPositionsList();
            positions = positions.Where(x => x.PositionName.ToLower().Contains(positionSearch.Text.ToLower())).ToList();

            positionDataGrid.ItemsSource = positions;
        }

        private void addPositionButton_Click(object sender, RoutedEventArgs e)
        {
            command = 1;
            PositionAddEditWindow positionAddEditWindow = new PositionAddEditWindow();

            positionAddEditWindow.Show();

        }
        public static Position positionEdit;
        private void editPositionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                command = 2;
                PositionAddEditWindow positionAddEditWindow = new PositionAddEditWindow();
                positionEdit = positionDataGrid.SelectedItem as Position;
                if (positionEdit != null)
                    positionAddEditWindow.Show();
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        private void deletePositionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var position = positionDataGrid.SelectedItem as Position;
                if (position != null)
                {
                    dbHelper.PositionDelete(position.Id);

                    positionDataGrid.ItemsSource = dbHelper.GetPositionsList();
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        private void workerSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var workers = dbHelper.GetWorkersList();
            workers = workers.Where(x => x.FIO.ToLower().Contains(workerSearch.Text.ToLower()) || x.Email.ToLower().Contains(workerSearch.Text.ToLower()) || x.INN.ToLower().Contains(workerSearch.Text.ToLower()) ||
            x.PassNum.ToLower().Contains(workerSearch.Text.ToLower()) || x.PassSeria.ToLower().Contains(workerSearch.Text.ToLower()) || x.PhoneNumber.ToLower().Contains(workerSearch.Text.ToLower())).ToList();

            workerDataGrid.ItemsSource = workers;
        }

        private void addWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            command = 1;
            WorkerAddEditWindow workerAddEditWindow = new WorkerAddEditWindow();
            workerAddEditWindow.Show();
        }

        public static Worker workerEdit;
        private void editWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                command = 2;
                workerEdit = workerDataGrid.SelectedItem as Worker;
                if (workerEdit != null)
                {
                    WorkerAddEditWindow workerAddEditWindow = new WorkerAddEditWindow();
                    workerAddEditWindow.Show();
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        private void deleteWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var worker = workerDataGrid.SelectedItem as Worker;
                if (worker != null)
                {
                    dbHelper.DeleteWorker(worker.Id);

                    workerDataGrid.ItemsSource = dbHelper.GetWorkersList();
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали данные", "Ошибка");
            }
        }

        #region Excel Export
        ExcelHelper excelHelper = new ExcelHelper();

        private void excelExportPositionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                excelHelper.ExcelExport(ref positionDataGrid);
            }
            catch
            {

            }
        }

        private void excelExportUserButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                excelHelper.ExcelExport(ref userDataGrid);
            }
            catch
            {

            }
        }

        private void excelExportRoleButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                excelHelper.ExcelExport(ref roleDataGrid);
            }
            catch
            {

            }
        }

        private void excelExportDiseaseButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                excelHelper.ExcelExport(ref diseaseDataGrid);
            }
            catch
            {

            }
        }

        private void excelExportWorkerButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                excelHelper.ExcelExport(ref workerDataGrid);
            }
            catch
            {

            }
        }

        private void excelExportPatientButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                excelHelper.ExcelExport(ref patientDataGrid);
            }
            catch
            {

            }
        }

        private void excelExportTreatmentButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                excelHelper.ExcelExport(ref treatmentDataGrid);
            }
            catch
            {

            }
        }

        #endregion

        private void statOpenButton_Click(object sender, RoutedEventArgs e)
        {
            StatWindow statWindow = new StatWindow();
            statWindow.Show();
        }
    }
}