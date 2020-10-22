using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IIS_laba
{
    /// <summary>
    /// Логика взаимодействия для UserOffice.xaml
    /// </summary>
    public partial class UserOffice : Page
    {
        private int clientcode;
        public MainWindow mainWindow;

        public UserOffice(MainWindow _mainWindow,int client)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            clientcode = client;
            name();
        }

        private void toomuch()
        {
            DataTable docs = mainWindow.Select("SELECT [Код_документа] FROM [dbo].[Паспорт] WHERE Код_клієнта =" + clientcode);
            if (docs.Rows.Count >= 5)
            {
                MessageBox.Show("Ви вже створили 5 документів");
                mainWindow.frame.Navigate(new UserOffice(mainWindow, clientcode));
            }

        }

        private void name()
        {
            DataTable client = mainWindow.Select("SELECT login FROM [dbo].[Клієнт] WHERE [Код_клієнта] = '" + clientcode + "'");
            greet.Content = "Доброго дня, "+ client.Rows[0][0];
        }

        private void documents_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.frame.Navigate(new mydocs(mainWindow, clientcode));
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.frame.Navigate(new login(mainWindow));
        }

        private void passport_Click(object sender, RoutedEventArgs e)
        {
            
            mainWindow.frame.Navigate(new pasReg(mainWindow, clientcode));
            toomuch();
        }

        //private void purchases_Click(object sender, RoutedEventArgs e)
        //{
        //    mainWindow.frame.Navigate(new purchases(mainWindow));
        //}
    }
}
