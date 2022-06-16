using MedicomApp.NET.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MedicomApp.NET
{
    public class DBHelper
    {
        //string connectionString = " datasource=localhost;port=3307;username=mysql;password=mysql;database=medicom";
        string connectionString = "Server=MYSQL8001.site4now.net;Database=db_a87516_medicom;Uid=a87516_medicom;Pwd=med1comDB";

        public List<User> GetUsersList()
        {
            List<User> users = new List<User>();
            var roles = GetRolesList();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT * FROM user", connection);
                    command.CommandTimeout = 0;
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            User user = new User();
                            user.Id = reader.GetInt32(0);
                            user.LastEnter = reader.GetDateTime(2);
                            user.Password = reader.GetString(3);
                            user.UserName = reader.GetString(4);
                            var role = roles.Where(x => x.User_Id == user.Id).ToList();
                            user.RoleName = role[0].RoleName;
                            users.Add(user);
                        }
                    }
                    return users;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            return null;
        }

        public List<User> GetUsersIDList()
        {
            List<User> users = new List<User>();
            var roles = GetRolesList();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT * FROM user", connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            User user = new User();
                            user.Id = reader.GetInt32(0);
                            users.Add(user);
                        }
                    }
                    return users;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            return null;
        }

        public void AddUser(string login, string password, string role)
        {
            DateTime dateTime = DateTime.Now;
            string dateNow = dateTime.ToString("yyyy-MM-dd hh:mm:ss");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command1 = new MySqlCommand($"INSERT INTO user(active, password, last_enter, username) VALUES(1, '{password}', '{dateTime.ToString("yyyy-MM-dd")}', '{login}')", connection);
                    connection.Open();
                    command1.ExecuteNonQuery();
                    var user = GetUsersIDList().Last();

                    MySqlCommand command2 = new MySqlCommand($"INSERT INTO user_role(user_id, roles) VALUES({user.Id}, '{role}')", connection);
                    command2.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void EditUser(string login, string password, int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command1 = new MySqlCommand($"UPDATE user SET password = '{password}', username= '{login}' WHERE id = {id};", connection);
                    connection.Open();
                    command1.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command1 = new MySqlCommand($"DELETE FROM user_role WHERE user_id={id}", connection);
                    MySqlCommand command2 = new MySqlCommand($"DELETE FROM user WHERE id={id}", connection);
                    connection.Open();
                    command1.ExecuteNonQuery();
                    command2.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public List<Role> GetRolesList()
        {
            List<Role> roles = new List<Role>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT * FROM user_role", connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Role role = new Role();
                            role.User_Id = reader.GetInt32(0);
                            role.RoleName = reader.GetString(1);
                            roles.Add(role);
                        }
                    }
                }
                return roles;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            return null;
        }

        public List<Disease> GetDiseasesList()
        {
            List<Disease> diseases = new List<Disease>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT * FROM disease", connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Disease disease = new Disease();
                            disease.Id = reader.GetInt32(0);
                            disease.DiseaseName = reader.GetString(2);
                            disease.DiseaseDescription = reader.GetString(1);
                            diseases.Add(disease);
                        }
                    }
                }
                return diseases;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            return null;
        }

        public void AddDisease(string name, string description)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"INSERT INTO disease(disease_name, disease_description)VALUES('{name}', '{description}')", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void EditDisease(string name, string description, int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"UPDATE disease SET disease_name = '{name}', disease_description = '{description}' WHERE id = {id}", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void DeleteDisease(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"DELETE FROM disease WHERE id = {id};", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
        public List<Worker> GetWorkersList()
        {
            List<Worker> workers = new List<Worker>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT * FROM worker", connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Worker worker = new Worker();
                            worker.Id = reader.GetInt32(0);
                            worker.INN = reader.GetString(1);
                            worker.Name = reader.GetString(2);
                            worker.PassNum = reader.GetString(7);
                            worker.PassSeria = reader.GetString(8);
                            worker.Patronymic = reader.GetString(3);
                            worker.Surmame = reader.GetString(4);
                            worker.BirthDate = reader.GetDateTime(5);
                            worker.Position = (GetPositionsList().Where(x => x.Id == reader.GetInt32(10))).FirstOrDefault().PositionName;
                            worker.Email = reader.GetString(6);
                            worker.PhoneNumber = reader.GetString(9);

                            workers.Add(worker);
                        }
                    }
                }
                return workers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            return null;
        }

        public void AddWorker(string inn, string name, string passNum, string passSeria, string patronymic, string surname, string birthdate, string position, string email, string phoneNum)
        {
            //string birth = birthdate.ToString("yyyy-MM-dd");
            
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("INSERT INTO worker(inn, name, pass_num, pass_seria, patronymic, surname, birth_date, position_id, email, phone_num) " +
                        $"VALUES('{inn}', '{name}', '{passNum}', '{passSeria}', '{patronymic}', '{surname}', '{birthdate}', {position}, '{email}', '{phoneNum}')", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void EditWorker(string inn, string name, string passNum, string passSeria, string patronymic, string surname, string birthdate, string position, string email, string phoneNum, int id)
        {
            //string birth = birthdate.ToString("yyyy-MM-dd");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"UPDATE worker SET inn = '{inn}', name = '{name}', pass_num = '{passNum}', pass_seria = '{passSeria}', patronymic = '{patronymic}', surname = '{surname}', birth_date = '{birthdate}', position_id = {position}, email = '{email}', phone_num = '{phoneNum}' WHERE id= {id};", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void DeleteWorker(int id)
        {
            //string birth = birthdate.ToString("yyyy-MM-dd");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"DELETE FROM worker WHERE id= {id};", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public List<Position> GetPositionsList()
        {
            List<Position> positions = new List<Position>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT * FROM position", connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Position position = new Position();
                            position.Id = reader.GetInt32(0);
                            position.PositionName = reader.GetString(1);
                            positions.Add(position);
                        }
                    }
                }
                return positions;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            return null;
        }

        public void PositionAdd(string name)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"INSERT INTO `db_a87516_medicom`.`position` (`position_name`) VALUES ('{name}');", connection);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void PositionEdit(string name, int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"UPDATE position SET position_name = '{name}' WHERE id = {id}", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void PositionDelete(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"DELETE FROM position WHERE id = {id}", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public List<Patient> GetPatientsList()
        {
            List<Patient> patients = new List<Patient>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT * FROM patient", connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Patient patient = new Patient();
                            patient.Id = reader.GetInt32(0);
                            patient.Surmame = reader.GetString(4);
                            patient.OMS = reader.GetString(2);
                            patient.PassNum = reader.GetString(7);
                            patient.PassSeria = reader.GetString(8);
                            patient.Patronymic = reader.GetString(3);
                            patient.Name = reader.GetString(1);
                            patient.BirthDate = reader.GetDateTime(5);
                            patient.Email = reader.GetString(6);
                            patient.PhoneNumber = reader.GetString(9);

                            patients.Add(patient);
                        }
                    }
                }
                return patients;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            return null;
        }

        public void AddPatient(string oms, string name, string passNum, string passSeria, string patronymic, string surname, string birthdate, string email, string phoneNum)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("INSERT INTO patient(oms, name, pass_num, pass_seria, patronymic, surname, birth_date, email, phone_num) " +
                        $"VALUES('{oms}', '{name}', '{passNum}', '{passSeria}', '{patronymic}', '{surname}', '{birthdate}', '{email}', '{phoneNum}')", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void EditPatient(string oms, string name, string passNum, string passSeria, string patronymic, string surname, string birthdate, string email, string phoneNum, int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"UPDATE patient SET oms = '{oms}', name = '{name}', pass_num = '{passNum}', pass_seria = '{passSeria}', patronymic = '{patronymic}', surname = '{surname}', birth_date = '{birthdate}', email = '{email}', phone_num = '{phoneNum}' WHERE id= {id};", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void DeletePatient(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"DELETE FROM patient WHERE id= {id};", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public List<Treatment> GetTreatmentsList()
        {
            List<Treatment> treatments = new List<Treatment>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("SELECT * FROM treatment", connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Treatment treatment = new Treatment();
                            treatment.Id = reader.GetInt32(0);
                            treatment.TreatmentDate = reader.GetDateTime(1);
                            treatment.TreatmentSum = reader.GetFloat(2);
                            treatment.DiseaseName = GetDiseasesList().Where(x => x.Id == reader.GetInt32(3)).FirstOrDefault().DiseaseName;
                            treatment.Worker = GetWorkersList().Where(x => x.Id == reader.GetInt32(5)).FirstOrDefault().FIO;
                            treatment.Patient = GetPatientsList().Where(x => x.Id == reader.GetInt32(4)).FirstOrDefault().FIO;
                            treatments.Add(treatment);
                        }
                    }
                }
                return treatments;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            return null;
        }

        public void AddTreatment(string treatmentDate, string treatmentSum, string disease, string patient, string worker)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand("INSERT INTO treatment(treatment_date, treatment_sum, disease_id, patient_id, worker_id) " +
                        $"VALUES('{treatmentDate}', {treatmentSum}, {disease}, {patient}, {worker});", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void EditTreatment(string treatmentDate, string treatmentSum, string disease, string patient, string worker, int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"UPDATE treatment SET treatment_date = '{treatmentDate}', treatment_sum = {treatmentSum}, disease_id = {disease}, patient_id = {patient}, worker_id = {worker}  WHERE id = {id} ", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public void DeleteTreatment(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand($"DELETE FROM `treatment` WHERE id = {id} ", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}
