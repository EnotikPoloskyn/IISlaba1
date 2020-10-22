using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace IIS_laba
{
    /// <summary>
    /// Логика взаимодействия для otherside.xaml
    /// </summary>
    public partial class otherside : Page
    {
        public MainWindow mainWindow;
        int clientcode;
        int documentcode;
        public otherside(MainWindow _mainWindow, int client, int pass)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            clientcode = client;
            documentcode = pass;
            fill();
            fillImages();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.frame.Navigate(new pasShow(mainWindow, clientcode, documentcode));
        }

        public void fill()
        {
            date.Content = DateTime.Today;

            authority.Content = "2283";

            DataTable placetable = mainWindow.Select("SELECT [Місце] FROM [dbo].[Паспорт] WHERE Код_документа =" + documentcode);
            place.Content = Convert.ToString(placetable.Rows[0][0]);

            DataTable birthdate = mainWindow.Select("SELECT [Дата_народження] FROM [dbo].[Паспорт] WHERE Код_документа =" + documentcode);
            string bd = string.Format("{0:yyyy-MM-dd}", birthdate.Rows[0][0]);
            //Convert.ToString(birthdate.Rows[0][0]);


            DataTable sex = mainWindow.Select("SELECT [Стать] FROM [dbo].[Паспорт] WHERE Код_документа =" + documentcode);
            string sexstr = Convert.ToString(sex.Rows[0][0]);
            string rightsex="";
            if (sexstr == "Чоловік")
            {
                rightsex = "M";
            }
            else rightsex = "F";

            DataTable strok = mainWindow.Select("SELECT [Дійсний_до] FROM [dbo].[Паспорт] WHERE Код_документа =" + documentcode);
            string strstrok = string.Format("{0:yyyy-MM-dd}", strok.Rows[0][0]);
            //string strstrok = Convert.ToString(strok.Rows[0][0]);

            DataTable name = mainWindow.Select("SELECT [Ім_я_тран] FROM [dbo].[Паспорт] WHERE Код_документа =" + documentcode);
            string strname = Convert.ToString(name.Rows[0][0]);

            DataTable surname = mainWindow.Select("SELECT [Прізвище_тран] FROM [dbo].[Паспорт] WHERE Код_документа =" + documentcode);
            string strsurname = Convert.ToString(name.Rows[0][0]);

            omegacode.Content = "IDUKR" + documentcode.ToString("D10") + documentcode.ToString("D13");
            omegacode1.Content = bd + rightsex + strstrok + "UKR";
            omegacode2.Content = strname + "<<" + strsurname;
        }
        public void fillImages()
        {
            SqlConnection sqlCon = new SqlConnection("server=CHEBUPEL\\SQLEXPRESS;Trusted_Connection=Yes;DataBase=Test;");
            sqlCon.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter sqa = new SqlDataAdapter("Select Фото from Паспорт where Код_документа='" + documentcode + "'", sqlCon);
            sqa.Fill(ds);
            sqlCon.Close();

            byte[] data = (byte[])ds.Tables[0].Rows[0][0];
            MemoryStream strm = new MemoryStream();
            strm.Write(data, 0, data.Length);
            strm.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            photo.Source = bi;
        }
    }
}
