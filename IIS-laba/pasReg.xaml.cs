using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Ink;

namespace IIS_laba
{
    /// <summary>
    /// Логика взаимодействия для pasReg.xaml
    /// </summary>
    public partial class pasReg : Page
    {
        public MainWindow mainWindow;
        int clientcode;
        byte[] byteImg;
        byte[] strokes;
        string str = "strokes.png";
        public pasReg(MainWindow _mainWindow, int client)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            clientcode = client;
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.frame.Navigate(new UserOffice(mainWindow, clientcode));
        }

        private void Donwnlimg_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.ShowDialog();

            FileStream fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read);

            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, System.Convert.ToInt32(fs.Length));
            byteImg = new byte[System.Convert.ToInt32(fs.Length)];
            Array.Copy(data, 0, byteImg, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            ImageSourceConverter imgs = new ImageSourceConverter();
            Firstimage.SetValue(Image.SourceProperty, imgs.ConvertFromString(dlg.FileName.ToString()));
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            pidpys.Strokes.Clear();
        }

        private double date()
        {
            DateTime chosen = birth.SelectedDate.Value;
            DateTime today = DateTime.Today;
            TimeSpan interval = today - chosen;
            int intInterval = Convert.ToInt32(interval.Days);
            double years = Math.Round(intInterval / 365.0, 3);
            return years;
        }
        private void accept_Click(object sender, RoutedEventArgs e)
        {
            if (surname.Text.Length > 0)
            {
                if (tranSurname.Text.Length > 0)
                {
                    if (name.Text.Length > 0)
                    {
                        if (tranName.Text.Length > 0)
                        {
                            if (father.Text.Length > 0)
                            {
                                if (birth.SelectedDate != null)
                                {
                                    if (place.Text.Length > 0)
                                    {
                                        if (Firstimage.Source != null)
                                        {
                                            if (pidpys.Strokes.Count > 0)
                                            {
                                                bool SURNAME = true;
                                                bool TRANSURNAME = true;
                                                bool NAME = true;
                                                bool TRANNAME = true;
                                                bool FATHER = true;
                                                bool PLACE = true;
                                                bool FOURTEEN = true;
                                                bool AFTERDATE = true;
                                                for (int i = 0; i < surname.Text.Length; i++)
                                                {
                                                    if (surname.Text[i] < 'І' || surname.Text[i] > 'І' && surname.Text[i] < 'А' || surname.Text[i] > 'Я') { SURNAME = false; }
                                                }
                                                for (int i = 0; i < tranSurname.Text.Length; i++)
                                                {
                                                    if (tranSurname.Text[i] < 'A' || tranSurname.Text[i] > 'Z') TRANSURNAME = false;
                                                }
                                                for (int i = 0; i < name.Text.Length; i++)
                                                {
                                                    if (name.Text[i] < 'І' || name.Text[i] > 'І' && name.Text[i] < 'А' || name.Text[i] > 'Я') NAME = false;
                                                }
                                                for (int i = 0; i < tranName.Text.Length; i++)
                                                {
                                                    if (tranName.Text[i] < 'A' || tranName.Text[i] > 'Z') TRANNAME = false;
                                                }
                                                for (int i = 0; i < father.Text.Length; i++)
                                                {
                                                    if (father.Text[i] < 'І' || father.Text[i] > 'І' && father.Text[i] < 'А' || father.Text[i] > 'Я') FATHER = false;
                                                }
                                                for (int i = 0; i < place.Text.Length; i++)
                                                {
                                                    if (place.Text[i] < 'І' || place.Text[i] > 'І' && place.Text[i] < 'А' || place.Text[i] > 'Я') PLACE = false;
                                                }
                                                if (date() < 14) FOURTEEN = false;
                                                if (birth.SelectedDate >= DateTime.Today) AFTERDATE = false;
                                                if (!SURNAME)
                                                    MessageBox.Show("В прізвищі доступні лише великі літери українського алфавіту");
                                                else if (!TRANSURNAME)
                                                    MessageBox.Show("В транслітерованому прізвищі доступні лише великі літери латиниці");
                                                else if (!NAME)
                                                    MessageBox.Show("В імені доступні лише великі літери українського алфавіту");
                                                else if (!TRANNAME)
                                                    MessageBox.Show("В транслітерованому імені доступні лише великі літери латиниці");
                                                else if (!FATHER)
                                                    MessageBox.Show("В імені по батькові доступні лише великі літери українського алфавіту");
                                                else if (!PLACE)
                                                    MessageBox.Show("В місці народження доступні лише великі літери українського алфавіту");
                                                else if (!FOURTEEN)
                                                    MessageBox.Show("Паспорт можна оформити лише коли вам виповниться 14 років");
                                                else if (!AFTERDATE)
                                                    MessageBox.Show("Вкажіть вірну дату");
                                                if (SURNAME && TRANSURNAME && NAME && TRANNAME && FATHER && FOURTEEN && AFTERDATE)
                                                {
                                                    DataTable sortedcodes = mainWindow.Select("SELECT [Код_документа] FROM [dbo].[Паспорт] ORDER BY [Код_документа] DESC");
                                                    int highestcode = Convert.ToInt32("" + sortedcodes.Rows[0][0]);
                                                    int nextcode = highestcode + 1;

                                                    ComboBoxItem ComboItem = (ComboBoxItem)sex.SelectedItem;
                                                    string cont = sex.SelectionBoxItem.ToString();

                                                    var birthdate = birth.SelectedDate;

                                                    MemoryStream ms = new MemoryStream();
                                                    pidpys.Strokes.Save(ms);
                                                    strokes = ms.ToArray();
                                                    ms.Dispose();

                                                    FileStream fs = null;
                                                    fs = new FileStream(str, FileMode.Create);
                                                    pidpys.Strokes.Save(fs);
                                                    fs.Close();

                                                    DateTime strok;
                                                    if (date() > 18)
                                                    {
                                                        strok = DateTime.Today.AddDays(365 * 10);
                                                    }
                                                    else
                                                    {
                                                        strok = DateTime.Today.AddDays(365 * 4);
                                                    }

                                                    SqlConnection sqlCon = new SqlConnection("server=CHEBUPEL\\SQLEXPRESS;Trusted_Connection=Yes;DataBase=Test;");
                                                    sqlCon.Open();
                                                    SqlCommand sc = new SqlCommand("insert into [dbo].[Паспорт](Код_документа, Код_клієнта, Прізвище, Прізвище_тран, Ім_я, Ім_я_тран, По_батькові, Стать, Дата_народження, Місце, Дійсний_до, Фото, Підпис) values(" + nextcode + "," + clientcode + ",'" + surname.Text + "','" + tranSurname.Text + "','" + name.Text + "','" + tranName.Text + "','" + father.Text + "','" + cont + "'," + "CAST('" + birthdate.Value + "' as datetime),'" + place.Text + "'," + "CAST('" + strok + "' as datetime), @p, @s);", sqlCon);
                                                    //SqlCommand sc = new SqlCommand("insert into Паспорт(Код_документа, Код_клієнта, Прізвище, Прізвище_тран, Ім_я, Ім_я_тран, По_батькові, Стать, Дата_народження, Дійсний_до, Фото) values(" + 2 + "," + 2 + ",'" + "ФІВФІВ" + "','" + "ASDASD" + "','" + "ФІВФІВ" + "','" + "ASDASD" + "','" + "ФІВФІВ" + "','" + "Чоловік" + "'," + "CAST(" + "2001-04-15" + " as datetime)," + "CAST(" + "2001-04-15" + " as datetime), @p);", sqlCon);
                                                    //mainWindow.Select("insert into Паспорт(Код_документа, Код_клієнта, Прізвище, Прізвище_тран, Ім_я, Ім_я_тран, По_батькові, Стать, Дата_народження, Дійсний_до, Фото) values(" + 2 + "," + 2 + ",'" + "ФІВФІВ" + "','" + "ASDASD" + "','" + "ФІВФІВ" + "','" + "ASDASD" + "','" + "ФІВФІВ" + "','" + "Чоловік" + "'," + "CAST('" + "2001-04-15" + "' as datetime)," + "CAST('" + "2001-04-15" + "' as datetime),"+ byteImg +");");
                                                    sc.Parameters.AddWithValue("@p", byteImg);
                                                    sc.Parameters.AddWithValue("@s", str);
                                                    sc.ExecuteNonQuery();
                                                    sqlCon.Close();
                                                    mainWindow.frame.Navigate(new pasShow(mainWindow, clientcode, nextcode));
                                                }
                                            }
                                            else MessageBox.Show("Вкажіть підпис");
                                        }
                                        else MessageBox.Show("Додайте фотографію");
                                    }
                                    else MessageBox.Show("Введіть місце народження");
                                }
                                else MessageBox.Show("Вкажіть дату народження");
                            }
                            else MessageBox.Show("Вкажіть по батькові");
                        }
                        else MessageBox.Show("Вкажіть транслітероване ім'я");
                    }
                    else MessageBox.Show("Вкажіть ім'я");
                }
                else MessageBox.Show("Вкажіть транслітероване прізвище");
            }
            else MessageBox.Show("Вкажіть прізвище");
        }
    }
}




//       if (password.Password.Length >= 6)
//                        {
//                            bool en = true;
//                            bool number = false;

//                            for (int i = 0; i<password.Password.Length; i++)
//                            {
//                                if (password.Password[i] >= 'А' && password.Password[i] <= 'я') en = false;
//                                if (password.Password[i] >= '0' && password.Password[i] <= '9') number = true;
//                            }

//                            if (!en)
//                                MessageBox.Show("В паролі можлива лише латиниця");
//                            else if (!number)
//                                MessageBox.Show("Додайте принаймні одну цифру");
//                            if (en && number)
//                            {
//                                if (password.Password == confirm.Password)
//                                {
//                                    DataTable sortedcodes = mainWindow.Select("SELECT [Код_клієнта] FROM [dbo].[Клієнт] ORDER BY [Код_клієнта] DESC");
//int highestcode = Convert.ToInt32("" + sortedcodes.Rows[0][0]);
//int nextcode = highestcode + 1;
//DataTable client = mainWindow.Select("INSERT INTO [dbo].[Клієнт] (Код_клієнта, login, password) VALUES ('" + nextcode + "','" + login.Text + "','" + password.Password + "');");
//MessageBox.Show("Користувача зареєстровано");
//                                    mainWindow.frame.Navigate(new login(mainWindow));
//                                    //mainWindow.OpenPage(MainWindow.pages.login);
//                                }
//                                else MessageBox.Show("Паролі не співпадають");
//                            }
//                        }