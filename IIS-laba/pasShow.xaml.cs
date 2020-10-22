using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Ink;

namespace IIS_laba
{
    /// <summary>
    /// Логика взаимодействия для pasShow.xaml
    /// </summary>
    public partial class pasShow : Page
    {
        public MainWindow mainWindow;
        int clientcode;
        int documentcode;
        public pasShow(MainWindow _mainWindow, int client, int pass)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            clientcode = client;
            documentcode = pass;

            fillLabels();
            fillImages();
            fillSign();
            nomer();
            
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

        private void back_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.frame.Navigate(new pasReg(mainWindow, clientcode));
            toomuch();
        }

        private void otherside_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.frame.Navigate(new otherside(mainWindow, clientcode, documentcode));
        }
        public void fillLabels()
        {
            DataTable sortedcodes = mainWindow.Select("SELECT [Прізвище],[Прізвище_тран],[Ім_я],[Ім_я_тран],[По_батькові],[Стать],[Дата_народження],[Дійсний_до] FROM [dbo].[Паспорт] WHERE Код_документа ="+documentcode);
            int i = 0;
            foreach (var child in stack.Children)
            {
                Label temp = child as Label;
                temp.Content = sortedcodes.Rows[0][i];
                i++;
            }
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
        public void fillSign()
        {
            DataTable fil = mainWindow.Select("Select Підпис from Паспорт where Код_документа='" + documentcode + "'");
            string filename = Convert.ToString(fil.Rows[0][0]);

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StrokeCollection strokes = new StrokeCollection(fs);
            pidpys.Strokes = strokes;
        }
        public void nomer()
        {
            DataTable birthday = mainWindow.Select("SELECT [Дата_народження] FROM [dbo].[Паспорт] WHERE Код_документа =" + documentcode);
            string strbirthday = Convert.ToString(birthday.Rows[0][0]);
            string birtdigits="";
            for(int i = 0; i < 10; i++)
            {
                if (char.IsDigit(strbirthday[i]))
                {
                    birtdigits += strbirthday[i];
                }
            }
            int digits = Convert.ToInt32(birtdigits);
            string res = digits + "-" + documentcode.ToString("D5");
            recordnomer.Content = res;
            docnomer.Content = documentcode.ToString("D9");
        }
    }
}
