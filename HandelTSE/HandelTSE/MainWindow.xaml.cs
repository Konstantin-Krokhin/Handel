using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace HandelTSE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        List<items> it = new List<items>();
        List<items> it2 = new List<items>();
        ProgressBarWindow pb = null;
        public static OleDbConnection con = new OleDbConnection();
        public MainWindow()
        {
            InitializeComponent();

            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

            try { con.Open(); }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Bitte installieren Sie die Microsoft Access Database Engine 2010. Möchten Sie zur Download-Seite weitergeleitet werden?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://www.microsoft.com/en-us/download/confirmation.aspx?id=13255");
                    MessageBox.Show("Nach der Installation des Treibers öffnen Sie bitte das Programm erneut, falls erforderlich. ");
                }
                else if (result == MessageBoxResult.No)
                { MessageBox.Show("Sie müssen den Treiber installieren, um die Daten sehen zu können."); }
            }

            Globals.PresseCon = con;

            // FOR DEV/TEST Purposes ONLY********************
            ContentWindow.SetValue(Grid.RowProperty, 1);
            ContentWindow.SetValue(Grid.ColumnProperty, 0);
            ContentWindow.SetValue(Grid.ColumnSpanProperty, 7);
            ContentWindow.SetValue(Grid.RowSpanProperty, 5);
            DataContext = new Artikelverwaltung();

            // FOR Login Screen window
            /*if (ViewModels.Globals.opened == 0)
            {
                LoginScreen e = new LoginScreen();
                e.Show();
            }*/
            //*******************************************

            // FOR SHOWING MAIN WINDOW FIRST (COMMENT IN ORDER to WORK ON other pages for convenience)
            /*
            dateBlock.Text = "Heute ist der " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            it.Add(new items { titel = "Umsatz gesamt:", geld = "0 EUR" });
            it.Add(new items { titel = "Gesamt ohne MwSt.:", geld = "0 EUR" });
            it.Add(new items { titel = "MwSt. ingesamt:", geld = "0 EUR" });
            for (int i=0; i < 30; i++) it.Add(new items { titel = "", geld = "" });

            dg1.ItemsSource = it;

            it2.Add(new items { titel = "Gesamt Brutto:", geld = "0 EUR" });
            it2.Add(new items { titel = "Gesamt Netto:", geld = "0 EUR" });
            it2.Add(new items { titel = "davon MwSt.:", geld = "0 EUR" });
            for (int i = 0; i < 30; i++) it2.Add(new items { titel = "", geld = "" });

            dg2.ItemsSource = it2;
            */
        }


        class items
        {
            public string titel { get; set; }
            public string geld { get; set; }
        }

        private void StatTable_Clicked(object sender, RoutedEventArgs e) { DataContext = new StatTableModel(); }

        private void StatGraph_Clicked(object sender, RoutedEventArgs e) { DataContext = new StatisticsGraphViewModel(); }

        private void TransParking_Clicked(object sender, RoutedEventArgs e) { DataContext = new TransactionParkingViewModel(); }

        private void MwStTool_Clicked(object sender, RoutedEventArgs e) { DataContext = new MwStTool(); }

        private void MainWindow_Clicked(object sender, RoutedEventArgs e) { DataContext = new Main(); }

        private void Artikelverwaltung_Click(object sender, RoutedEventArgs e)
        {
            ContentWindow.SetValue(Grid.RowProperty, 1);
            ContentWindow.SetValue(Grid.ColumnProperty, 0);
            ContentWindow.SetValue(Grid.ColumnSpanProperty, 7);
            ContentWindow.SetValue(Grid.RowSpanProperty, 5);
            DataContext = new HandelTSE.ViewModels.Artikelverwaltung();
        }

        private void Kasse_Click(object sender, RoutedEventArgs e) { Kasse kasse = new Kasse(); kasse.Show(); }

        private void EanEinstellungen_Click(object sender, RoutedEventArgs e) { DataContext = new EanEinstellungen(); }

        private void ArtikelOptionen_Click(object sender, RoutedEventArgs e) { DataContext = new ArtikelOptionenEinstellungen(); }

        private void Personalverwaltung_Click(object sender, RoutedEventArgs e) { DataContext = new Personalverwaltung(); }

        // Close all the connections made in User Control Interfaces
        private void Close_Clicked(object sender, EventArgs e) { con.Close(); }

        private void ProgramEinstellungen_Click(object sender, RoutedEventArgs e) { DataContext = new ProgramEinstellungen(); }

        private void FunktionsEnstellungen_Click(object sender, RoutedEventArgs e) { DataContext = new FunktionsEinstellungen(); }

        private void Umsatzsteuer_Click(object sender, RoutedEventArgs e) { DataContext = new Umsatzsteuer(); }

        private void Zahlungen_Click(object sender, RoutedEventArgs e) { DataContext = new Zahlungen(); }

        private void ProgramBeenden_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void Stornogrunde_Click(object sender, RoutedEventArgs e) { DataContext = new Stornogrunde(); }

        private async void PresseUndVMP_Click(object sender, RoutedEventArgs e) 
        {
            if (pb == null) pb = new ProgressBarWindow();
            pb.Owner = this;
            pb.Visibility = Visibility.Visible;
            Globals.PresseCon = con;

            await Task.Run(() => LoadData());

            pb.Visibility = Visibility.Collapsed;

            DataContext = new PresseUndVMP(); 
        }

        private void Datensicherung_Click(object sender, RoutedEventArgs e) { DataContext = new Datensicherung(); }

        // Loading all newspapers from the list and creating tables in DB
        public async Task LoadData()
        {
            string str = "";
            OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_PRESSE] ([Id] COUNTER, [CEAN] TEXT(55), [CNAME] TEXT(55))", con);
            OleDbCommand cmd2 = new OleDbCommand("CREATE TABLE [TBL_EANCode] ([Id] COUNTER, [Landprafix] TEXT(55), [PresseKZ] TEXT(55), [MwSt] TEXT(55), [VDZ] TEXT(55), [Verkaufspreis] TEXT(55), [Bezeichnung] TEXT(55))", con);
            try
            {
                cmd.ExecuteNonQuery();

                // Create a table with almost all existing German newspapers and their EAN codes from Presse_liste file with SQL commands inside
                int i = 0;
                string commands = File.ReadAllText("../Debug/Presse_liste.txt");
                while (i < commands.Length)
                {
                    // Delete \r and \n from the string
                    if (commands[i] == '\r' || commands[i] == '\n')
                    {
                        commands = commands.Remove(i, 1);
                        continue;
                    }
                    i++;
                }
                str = commands;

                // Separate SQL commands from each other
                string[] sqlStatements = str.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                // Run all commands for inserting the records
                if (con.State == System.Data.ConnectionState.Closed) con.Open();
                OleDbTransaction transaction = con.BeginTransaction();
                foreach (string statement in sqlStatements)
                {
                    using (OleDbCommand cmd0 = new OleDbCommand(statement, con, transaction))
                    {
                        cmd0.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
            catch { }
            try { cmd2.ExecuteNonQuery(); }
            catch { }
        }

        // Use If constant positioning of the loading ProgressBar needs to be centered inside the Ownder (add LocationChanged="Window_LocationChanged")
        /*private void Window_LocationChanged(object sender, EventArgs e)
        {
            if (pb != null)
            {
                pb.Left = (this.Left + this.Width)/2;
                pb.Top = (this.Top + this.Height)/2;
            }
        }*/
    }
}
