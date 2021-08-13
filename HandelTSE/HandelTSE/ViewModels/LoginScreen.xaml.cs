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
using HandelTSE.ViewModels;

namespace HandelTSE.ViewModels
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        OleDbConnection con = new OleDbConnection();
        public LoginScreen()
        {
            InitializeComponent();

            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            
            try
            {
                con.Open();
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM TBL_PERSONAL WHERE Name = @Name", con);
            cmd.Parameters.Add(new OleDbParameter("@Name", LoginField.Text));

            OleDbDataReader d = cmd.ExecuteReader();
            
            if (d.Read()) { Globals.opened++; }

            if (Globals.opened > 0)
            {
                MainWindow m = new MainWindow();
                m.DataContext = new Personalverwaltung();
                m.Show();
                Close();
            }
            else FelschePass.Visibility = Visibility.Visible;
        }

        private void Exit_Click(object sender, RoutedEventArgs e) { Close(); }

        public void Close_Clicked(object sender, EventArgs e) { con.Close(); }
    }
}
