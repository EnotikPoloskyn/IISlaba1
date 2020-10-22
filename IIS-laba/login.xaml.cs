using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IIS_laba
{
    public partial class login : Page
    {
        public MainWindow mainWindow;
        int clientcode;

        public login(MainWindow _mainWindow,int code)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            clientcode = code;
        }
        public login(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (log.Text.Length > 0)   
            {
                if (password.Password.Length > 0)      
                {               
                    DataTable client = mainWindow.Select("SELECT Код_клієнта FROM [dbo].[Клієнт] WHERE [login] = '" + log.Text + "' AND [password] = '" + password.Password + "'");
                    if (client.Rows.Count > 0)       
                    {
                        int highestcode = Convert.ToInt32("" + client.Rows[0][0]);
                        clientcode = highestcode;
                        mainWindow.frame.Navigate(new UserOffice(mainWindow,clientcode));
                        MessageBox.Show("Авторизовано");     
                    }
                    else MessageBox.Show("Користувача не знайдено"); 
                }
                else MessageBox.Show("Введіть пароль");   
            }
            else MessageBox.Show("Введіть логін");
        }
        private void register_Click(object sender, RoutedEventArgs e)
        {
            //mainWindow.OpenPage(MainWindow.pages.register);
            mainWindow.frame.Navigate(new register(mainWindow));
        }
    }
}
