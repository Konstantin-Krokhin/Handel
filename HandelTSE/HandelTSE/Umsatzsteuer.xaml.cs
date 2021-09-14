using System;
using System.Collections.Generic;
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

namespace HandelTSE
{
    /// <summary>
    /// Interaction logic for Umsatzsteuer.xaml
    /// </summary>
    public partial class Umsatzsteuer : UserControl
    {
        Umsatz data = new Umsatz();
        List<Umsatz> list = new List<Umsatz>();
        public static OleDbConnection con = new OleDbConnection();
        public List<Umsatz> Data { get; set; }

        public class Umsatz
        {
            public Int32 Id { get; set; }
            public string MwSt { get; set; }
            public string Bezeich { get; set; }
            public string Schlussel { get; set; }
            public string Beschreibung { get; set; }
            public string Konto { get; set; }
            public string GKonto { get; set; }
            public string Kennzeich { get; set; }
        }

        public Umsatzsteuer()
        {
            InitializeComponent();

            // If the menu ProgramEinstellungen is being open multiple times
            if (con.ConnectionString.Length == 0)
            {
                con = MainWindow.con;
                OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_Umsatzsteuer] ([Id] COUNTER, [MwSt] TEXT(55),[Bezeich] TEXT(55),[Schlussel] TEXT(55),[Beschreibung] TEXT(55),[Konto] TEXT(55),[GKonto] TEXT(55), [Kennzeich] TEXT(55))", con);
                try 
                { 
                    cmd.ExecuteNonQuery();
                    //Create first empty record with the proper starting ID = 1
                    OleDbCommand cmd2 = new OleDbCommand("insert into [TBL_Umsatzsteuer](Id)Values('" + 1 + "')", con);
                    try { cmd2.ExecuteNonQuery(); }
                    catch { }
                }
                catch { }
            }

            LoadGrid();
        }

        private void LoadGrid()
        {
            OleDbCommand cmd2 = new OleDbCommand("SELECT * FROM [TBL_Umsatzsteuer]", con);
            OleDbDataReader myReader = cmd2.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["Id"] != DBNull.Value) data.Id = (Int32)myReader["Id"]; else data.Id = -1;
                if (myReader["MwSt"] != DBNull.Value) data.MwSt = (string)myReader["MwSt"]; else data.MwSt = "";
                if (myReader["Bezeich"] != DBNull.Value) data.Bezeich = (string)myReader["Bezeich"]; else data.Bezeich = "";
                if (myReader["Schlussel"] != DBNull.Value) data.Schlussel = (string)myReader["Schlussel"]; else data.Schlussel = "";
                if (myReader["Beschreibung"] != DBNull.Value) data.Beschreibung = (string)myReader["Beschreibung"]; else data.Beschreibung = "";
                if (myReader["Konto"] != DBNull.Value) data.Konto = (string)myReader["Konto"]; else data.Konto = "";
                if (myReader["GKonto"] != DBNull.Value) data.GKonto = (string)myReader["GKonto"]; else data.GKonto = "";
                if (myReader["Kennzeich"] != DBNull.Value) data.Kennzeich = (string)myReader["Kennzeich"]; else data.Kennzeich = "";
                list.Add(new Umsatz { Id = data.Id, MwSt = data.MwSt, Bezeich = data.Bezeich, Schlussel = data.Schlussel, Beschreibung = data.Beschreibung, Konto = data.Konto, GKonto = data.GKonto, Kennzeich = data.Kennzeich });
            }
            Data = list;
            UmsatzsteuerDataGrid.ItemsSource = Data;
            list = new List<Umsatz>();
        }

        private void CustomizeHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e) { if (e.Column.Header.ToString() == "MwSt") e.Column.Header = "MwSt%"; if (e.Column.Header.ToString() == "GKonto") e.Column.Header = "G.Konto"; }

        private void RecordSelected(object sender, SelectionChangedEventArgs e) { NeuUmsatzsteuerButton.IsEnabled = true; SpeichernButton.IsEnabled = true; SpeichernButton.Opacity = 1; }

        private void NeuUmsatzsteuer_Click(object sender, RoutedEventArgs e) { SpeichernButton.IsEnabled = true; SpeichernButton.Opacity = 1; UmsatzsteuerDataGrid.SelectedItem = new DataGridRow(); NeuUmsatzsteuerButton.Opacity = 0.7; NeuUmsatzsteuerButton.IsEnabled = false; }

        private void speichern_Click(object sender, RoutedEventArgs e)
        {
            int k = 0;
            Umsatz item = ((Umsatz)UmsatzsteuerDataGrid.SelectedItem);
            if (item == null || item.MwSt == "" || item.Bezeich == "" || !item.MwSt.All(char.IsDigit)) k = 1;
            if (k == 1) { MessageBox.Show("Überprüfen Sie bitte die Steuerndaten (Steuersatz und Bezeichnung)!"); return; }
            SpeichernButton.IsEnabled = false;
            SpeichernButton.Opacity = 0.7;

            Int32 ID = 0;
            int result = 0;
            OleDbCommand cmd = new OleDbCommand();
            if (item.Id != 0) 
            { 
                ID = item.Id;
                cmd = new OleDbCommand("UPDATE [TBL_Umsatzsteuer] SET MwSt = @MwSt, Bezeich = @Bezeich, Schlussel = @Schlussel, Beschreibung = @Beschreibung, Konto = @Konto, GKonto = @GKonto, Kennzeich = @Kennzeich WHERE Id = @ID", con);

                cmd.Parameters.Add(new OleDbParameter("@MwSt", item.MwSt));
                cmd.Parameters.Add(new OleDbParameter("@Bezeich", item.Bezeich));
                cmd.Parameters.Add(new OleDbParameter("@Schlussel", item.Schlussel));
                cmd.Parameters.Add(new OleDbParameter("@Beschreibung", item.Beschreibung));
                cmd.Parameters.Add(new OleDbParameter("@Konto", item.Konto));
                cmd.Parameters.Add(new OleDbParameter("@GKonto", item.GKonto));
                cmd.Parameters.Add(new OleDbParameter("@Kennzeich", item.Kennzeich));
                cmd.Parameters.Add(new OleDbParameter("@ID", ID));
            }
            else
            {
                OleDbCommand maxCommand = new OleDbCommand("SELECT max(Id) from TBL_Umsatzsteuer", con);
                try { ID = (Int32)maxCommand.ExecuteScalar(); } catch { }
                cmd = new OleDbCommand("insert into [TBL_Umsatzsteuer](Id, MwSt, Bezeich, Schlussel, Beschreibung, Konto, GKonto, Kennzeich)Values('" + ++ID + "','" + item.MwSt + "','" + item.Bezeich + "','" + item.Schlussel + "','" + item.Beschreibung + "','" + item.Konto + "','" + item.GKonto + "','" + item.Kennzeich + "')", con);
            }

            try { result = cmd.ExecuteNonQuery(); LoadGrid(); HideColumns(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
        }

        private void HideColumns() { if (UmsatzsteuerDataGrid.Items.Count > 0 && UmsatzsteuerDataGrid.Columns.Count > 0) UmsatzsteuerDataGrid.Columns[0].Visibility = Visibility.Collapsed; }

        private void UmsatzsteuerDataGrid_Loaded(object sender, RoutedEventArgs e) { HideColumns(); }

        private void UmsatzsteuerDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e) { }

        private void UmsatzsteuerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
