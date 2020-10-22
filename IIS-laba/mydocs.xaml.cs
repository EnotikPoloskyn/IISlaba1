using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для mydocs.xaml
    /// </summary>
    public partial class mydocs : Page
    {
        public MainWindow mainWindow;
        int clientcode;
        int documentcode;
        int[] codes = new int[5];
        public mydocs(MainWindow _mainWindow, int client)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            clientcode = client;
            docs();
        }
        
        public void docs()
        {
            DataTable docs = mainWindow.Select("SELECT [Код_документа] FROM [dbo].[Паспорт] WHERE Код_клієнта =" + clientcode);
            for(int i=0;i < docs.Rows.Count; i++)
            {
                codes[i] = Convert.ToInt32(docs.Rows[i][0]);
            }
            int k = 0;
            foreach(var child in docnumber.Items)
            {
                ComboBoxItem temp = child as ComboBoxItem;
                temp.Content = Convert.ToString(codes[k]);
                k ++;
                if(Convert.ToString(temp.Content) == "0")
                {
                    temp.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.frame.Navigate(new UserOffice(mainWindow, clientcode));
        }

        private void accept_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem ComboItem = (ComboBoxItem)docnumber.SelectedItem;
            string cont = docnumber.SelectionBoxItem.ToString();
            documentcode = Convert.ToInt32(cont);
            mainWindow.frame.Navigate(new pasShow(mainWindow, clientcode, documentcode));
        }
    }
}
