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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //OpenPage(pages.login);
            frame.Navigate(new login(this));
        }

        //public enum pages
        //{
        //    login,
        //    register,
        //    UserOffice
        //}

        //public void OpenPage(pages pages)
        //{
        //    if(pages == pages.login)
        //    {
        //        frame.Navigate(new login(/*this*/));
        //    }
        //    else if (pages == pages.register)
        //    {
        //        frame.Navigate(new register(/*this*/));
        //    }

            //else if (pages == pages.UserOffice)
            //{
            //    frame.Navigate(new UserOffice(this));
            //}
        //}

        public DataTable Select(string selectSQL)
        {
            DataTable dataTable = new DataTable("dataBase");
            SqlConnection sqlConnection = new SqlConnection("server=CHEBUPEL\\SQLEXPRESS;Trusted_Connection=Yes;DataBase=Test;");
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = selectSQL;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
    }
}

//DataTable dt_user = Select("SELECT * FROM [dbo].[Клієнт]");

//            for (int i = 0; i<dt_user.Rows.Count; i++)
//            { 
//                MessageBox.Show(dt_user.Rows[i][0] + "|" + dt_user.Rows[i][1]);
//            }