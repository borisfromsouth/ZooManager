using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace ZooManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection; // подключение к БД

        public MainWindow()
        {
            InitializeComponent();

            // ConfigurationManager предоставляет доступ к файлам конфигурации для клиентских приложений
            // Программа берет нужный источник данных из добавленных "Источников данных"
            string connectionString = ConfigurationManager.ConnectionStrings["ZooManager.Properties.Settings.TrainingDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);


            ShowZooData();
            listZoo.SelectedValue = 1;
            ShowAllAnimals();
        }

        private void ShowAllAnimals()
        {
            try
            {
                string query = "SELECT * FROM Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);

                    listAllAnimals.DisplayMemberPath = "Name";      // Что из таблицы выводится в интерфейс
                    listAllAnimals.SelectedValuePath = "Id";            // Что передвется когда выбран предмет из сипска
                    listAllAnimals.ItemsSource = dataTable.DefaultView; // Указываем источник данных 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            listAllAnimals.SelectedValue = 1;
        }


        private void ShowZooData()
        {
            // Для работы с БД рекомендуется испрользовать try-catch
            try
            {
                string query = "SELECT * FROM Zoo"; // запрос в БД

                // SqlDataAdapter представляет своеобразный интерфейс для работы с таблицами БД посредством объектов C#
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);

                    listZoo.DisplayMemberPath = "Location";      // Что из таблицы выводится в интерфейс
                    listZoo.SelectedValuePath = "Id";            // Что передвется когда выбран предмет из сипска
                    listZoo.ItemsSource = dataTable.DefaultView; // Указываем источник данных 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowAnimalData()
        {
            // Для работы с БД рекомендуется испрользовать try-catch
            try
            {
                string query = "SELECT * FROM Animal a inner join ZooAnimal za ON a.Id = za.AnimalId WHERE za.ZooId = @ZooId"; // запрос в БД

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                // SqlDataAdapter представляет своеобразный интерфейс для работы с таблицами БД посредством объектов C#
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoo.SelectedValue);

                    DataTable animalDataTable = new DataTable();
                    sqlDataAdapter.Fill(animalDataTable);

                    listAnimals.DisplayMemberPath = "Name";      // Что из таблицы выводится в интерфейс
                    listAnimals.SelectedValuePath = "Id";            // Что передвется когда выбран предмет из сипска
                    listAnimals.ItemsSource = animalDataTable.DefaultView; // Указываем источник данных 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listZoo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAnimalData();
            SelectZooValueToTextbox();
        }
        
        private void listAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //inputTextBox.Text = listAnimals.DisplayMemberPath;
            SelectAnimalValueToTextbox();
        }

        private void SelectZooValueToTextbox()
        {
            try
            {
                string query = "SELECT Location FROM Zoo WHERE id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoo.SelectedValue);

                    DataTable animalDataTable = new DataTable();
                    sqlDataAdapter.Fill(animalDataTable);

                    inputTextBox.Text = animalDataTable.Rows[0]["Location"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectAnimalValueToTextbox()
        {
            try
            {
                string query = "SELECT Name FROM Animal WHERE id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);

                    DataTable animalDataTable = new DataTable();
                    sqlDataAdapter.Fill(animalDataTable);

                    inputTextBox.Text = animalDataTable.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectAllAnimalValueToTextbox()
        {
            try
            {
                string query = "SELECT * FROM Animal Where id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);

                    DataTable animalDataTable = new DataTable();
                    sqlDataAdapter.Fill(animalDataTable);

                    inputTextBox.Text = animalDataTable.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            if (inputTextBox.Text == string.Empty) return;
            try
            {
                string query = "INSERT INTO Zoo VALUES (@Location)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                this.sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", inputTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                this.sqlConnection.Close();
                this.ShowZooData();
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (inputTextBox.Text == string.Empty) return;
            try
            {
                string query = "INSERT INTO Animal VALUES (@Animal)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                this.sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Animal", inputTextBox.Text); // SelectedValue это Id
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                this.sqlConnection.Close();
                this.ShowAllAnimals();
            }
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            if (listZoo.SelectedValue == null) return;
            try
            {
                string query = "UPDATE Zoo SET Location = @NewLocation";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                this.sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@NewLocation", inputTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                this.sqlConnection.Close();
                this.ShowZooData();
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (listZoo.SelectedValue == null) return;
            try
            {
                string query = "UPDATE Animal SET Name = @NewAnimalName";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                this.sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@NewAnimalName", inputTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                this.sqlConnection.Close();
                this.ShowAnimalData();
            }
        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            if (listZoo.SelectedValue == null || listAllAnimals.SelectedValue == null) return;
            try
            {
                string query = "INSERT INTO ZooAnimal VALUES (@ZooId, @AnimalId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                this.sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoo.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue); // SelectedValue это Id
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                this.sqlConnection.Close();
                this.ShowAnimalData();
            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e) // из общего списка
        {
            if (listAllAnimals.SelectedValue == null) return;
            try
            {
                string query = "DELETE FROM Animal WHERE id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                this.sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                this.sqlConnection.Close();
                this.ShowAllAnimals();
            }
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            if (listZoo.SelectedValue == null) return;
            try
            {
                string query = "DELETE FROM Zoo WHERE id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                this.sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoo.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                this.sqlConnection.Close();
                this.ShowZooData();
            }
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e) // из конкретной таблицы
        {
            if (listZoo.SelectedValue == null && listAnimals.SelectedValue == null) return;
            try
            {
                string query = "DELETE FROM ZooAnimal WHERE ZooId = @temp1 AND AnimalId = @temp2";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                this.sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@temp1", listZoo.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@temp2", listAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                this.sqlConnection.Close();
                //this.ShowZooData();
                this.ShowAnimalData();
            }
        }

        private void listAllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectAllAnimalValueToTextbox();
        }
    }
}
