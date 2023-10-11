using System.Configuration;
using System.Windows;

namespace ZooManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["ZooManager.Properties.Settings.TrainingDBConnectionString"].ConnectionString;
        }
    }
}
