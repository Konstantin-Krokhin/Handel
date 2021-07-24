using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
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

namespace HandelTSE.ViewModels
{
    /// <summary>
    /// Interaction logic for Personalverwaltung.xaml
    /// </summary>
    public partial class Personalverwaltung : UserControl
    {
        OleDbConnection con = new OleDbConnection();

        List<MyData> list = new List<MyData>();
        List<MyData> SubstituteList = new List<MyData>();
        public List<MyData> Data { get; set; }
        public class MyData
        {
            public int Identyfikator { get; set; }
            public string Name { get; set; }
            //not sure what finalconc type would be, so here just using string
            public string Login { get; set; }
        }

        public Personalverwaltung()
        {
            InitializeComponent();

            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

            try
            {
                con.Open();
                LoadGrid();
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Bitte installieren Sie die Microsoft Access Database Engine 2010. Möchten Sie zur Download-Seite weitergeleitet werden?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://www.microsoft.com/en-us/download/confirmation.aspx?id=13255");
                    MessageBox.Show("Nach der Installation des Treibers laden Sie bitte das Menü Personalverwaltung oder den Computer neu, falls erforderlich. ");
                }
                else if (result == MessageBoxResult.No)
                {
                    MessageBox.Show("Sie müssen den Treiber installieren, um die Daten sehen zu können.");
                }

            }
        }

        private void LoadGrid()
        {
            //OleDbCommand cmd = new OleDbCommand("SELECT Identyfikator, Name, Login, Rabatt, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], [21] FROM [TBL_PERSONAL]", con); //  temp_tb ; SELECT* FROM temp_tb

            ///OleDbDataReader rd = cmd.ExecuteReader();

            ///grid.ItemsSource = rd;

            OleDbCommand cmd = new OleDbCommand("SELECT Identyfikator, Name, Login FROM [TBL_PERSONAL];", con);

            OleDbDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            
            MyData data = new MyData();
            while (myReader.Read())
            {
                data = new MyData();
                data.Identyfikator = (int)myReader["Identyfikator"];
                data.Name = (string)myReader["Name"];
                data.Login = (string)myReader["Login"]; // or whatever the type should be
                list.Add(data);
                if (list.Count == 3) break;
            }
            Data = list;
            grid.ItemsSource = Data;

            //grid.ItemsSource = (IEnumerable<string>)data;
        }

        private void Speichern_Click(object sender, RoutedEventArgs e) { SaveToDB(); }

        void SaveToDB()
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = "insert into [TBL_PERSONAL](Name, Login, Passwort, Rabatt, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], [21])Values('" + Name.Text + "','" + Login.Text + "','" + Passwort.Text + "','" + Rabatt.Text + "','" + Storno.Text + "','" + Warenverwaltung.Text + "','" + Artikelrabatt.Text + "','" + Gutschein.Text + "','" + GutscheinStorno.Text + "','" + ZAbschlag.Text + "','" + SofortStorno.Text + "','" + PlusMinusFunk.Text + "','" + LetzterBon.Text + "','" + Office.Text + "','" + EinAusnahme.Text + "','" + Einstellungen.Text + "','" + Buchhaltung.Text + "','" + XAbschlag.Text + "','" + Kassenlade.Text + "','" + AdminStorno.Text + "','" + Warenbestand.Text + "','" + Inventur.Text + "','" + PreisF6.Text + "','" + Berichte.Text + "','" + Wareneingang.Text + "')";
            cmd.Connection = con;

            int result = 0;
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt.");
            }

            LoadGrid();
            if (result > 0) MessageBox.Show("Ihre Änderungen wurden erfolgreich gespeichert!");
        }

        private void Loschen_Click(object sender, RoutedEventArgs e)
        {
            var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(grid.SelectedIndex);
            var item = row.Item as MyData;

            SubstituteList.AddRange(list);
            SubstituteList.Remove(item);
            list = new List<MyData>();
            list.AddRange(SubstituteList);

            Data = list;
            grid.ItemsSource = Data;

            grid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void gridLoaded(object sender, RoutedEventArgs e) { if (grid.Items.Count > 0) grid.Columns[0].Visibility = Visibility.Collapsed; }
    }
}
