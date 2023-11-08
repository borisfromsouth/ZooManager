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
            //MessageBox.Show(listZoo.SelectedValue.ToString());
            ShowAnimalData();
        }
    }
}
