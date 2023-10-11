using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace ZooManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["ZooManager.Properties.Settings.TrainingDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            ShowZooData();
        }
        
        private void ShowZooData()
        {
            try
            {
                string query = "SELECT * FROM Zoo";

                // SqlDataAdapter представляет своеобразный интерфейс для работы с таблицами БД посредством объектов C#
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);

                    listZoo.DisplayMemberPath = "Location"; // Какая инфа должна отображаться
                    listZoo.SelectedValuePath = "Id"; // Что выбирается под копотом
                    listZoo.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
