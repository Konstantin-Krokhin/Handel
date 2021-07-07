using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
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
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = "select * from [TBL_PERSONAL]";
            cmd.Connection = con;
            OleDbDataReader rd = cmd.ExecuteReader();
            grid.ItemsSource = rd;
        }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = "insert into [TBL_PERSONAL](Name)Values(@nm)";
            cmd.Parameters.AddWithValue("@nm", Name.Text);
            cmd.Connection = con;

            cmd.CommandText = "insert into [TBL_PERSONAL](Login)Values(@nm)";
            cmd.Parameters.AddWithValue("@nm", Login.Text);
            cmd.Connection = con;

            cmd.CommandText = "insert into [TBL_PERSONAL](Rabatt)Values(@nm)";
            cmd.Parameters.AddWithValue("@nm", Rabatt.Text);
            cmd.Connection = con;

            /*cmd.CommandText = "insert into [TBL_PERSONAL](1)Values(@nm)";
            cmd.Parameters.AddWithValue("@nm", Login.Text);
            cmd.Connection = con;*/

            int result = 0;
            try
            {
                result = cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung mit der Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist.");
            }
            
            if (result > 0) MessageBox.Show("Ihre Änderungen wurden erfolgreich gespeichert!");
        }
    }
}
