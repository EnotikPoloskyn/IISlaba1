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
    public partial class register : Page
    {
        public MainWindow mainWindow;

        public register(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.frame.Navigate(new login(mainWindow));
            //mainWindow.OpenPage(MainWindow.pages.login);
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text.Length > 0)
            {
                if (password.Password.Length > 0)
	            {
                    if (confirm.Password.Length > 0)
		            {
                        if (password.Password.Length >= 6)
                        {
                            bool en = true;
                            bool number = false; 

                            for (int i = 0; i < password.Password.Length; i++)
                            {
                                if (password.Password[i] >= 'А' && password.Password[i] <= 'я') en = false;
                                //if (password.Password[i] <0 || password.Password[i] >9 && password.Password[i] < 'A' || password.Password[i] > 'Z' && password.Password[i] < 'a' || password.Password[i] > 'z') en = false;
                                if (password.Password[i] >= '0' && password.Password[i] <= '9') number = true;
                            }

                            if (!en)
                                MessageBox.Show("В паролі можлива лише латиниця");
                            else if (!number)
                                MessageBox.Show("Додайте принаймні одну цифру");
                            if (en && number)
	                        {
                                if (password.Password == confirm.Password)
                                {
                                    DataTable sortedcodes = mainWindow.Select("SELECT [Код_клієнта] FROM [dbo].[Клієнт] ORDER BY [Код_клієнта] DESC");
                                    int highestcode = Convert.ToInt32("" + sortedcodes.Rows[0][0]);
                                    int nextcode = highestcode + 1;
                                    DataTable client = mainWindow.Select("INSERT INTO [dbo].[Клієнт] (Код_клієнта, login, password) VALUES ('"+ nextcode + "','" + login.Text+"','"+password.Password+"');");
                                    MessageBox.Show("Користувача зареєстровано");
                                    mainWindow.frame.Navigate(new login(mainWindow,nextcode));
                                }
                                else MessageBox.Show("Паролі не співпадають");
                            }
                        }
                    else MessageBox.Show("Пароль повинен мати мінімум 6 символів");

                }
                    else MessageBox.Show("Підтвердіть пароль");
                }
                else MessageBox.Show("Вкажіть пароль");
            }
            else MessageBox.Show("Вкажіть логін");
        }
    }
}
