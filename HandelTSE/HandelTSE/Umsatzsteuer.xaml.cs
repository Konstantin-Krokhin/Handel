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
                try { cmd.ExecuteNonQuery(); }
                catch { }
            }

            LoadGrid();
        }

        private void LoadGrid()
        {
            OleDbCommand cmd2 = new OleDbCommand("SELECT Id, MwSt FROM [TBL_Umsatzsteuer]", con);
            OleDbDataReader myReader = cmd2.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["MwSt"] != DBNull.Value) data.MwSt = (string)myReader["MwSt"]; else data.MwSt = "";
                if (myReader["Bezeich"] != DBNull.Value) data.Bezeich = (string)myReader["Bezeich"]; else data.Bezeich = "";
                if (myReader["Schlussel"] != DBNull.Value) data.Schlussel = (string)myReader["Schlussel"]; else data.Schlussel = "";
                if (myReader["Beschreibung"] != DBNull.Value) data.Beschreibung = (string)myReader["Beschreibung"]; else data.Beschreibung = "";
                if (myReader["Konto"] != DBNull.Value) data.Konto = (string)myReader["Konto"]; else data.Konto = "";
                if (myReader["GKonto"] != DBNull.Value) data.GKonto = (string)myReader["GKonto"]; else data.GKonto = "";
                if (myReader["Kennzeich"] != DBNull.Value) data.Kennzeich = (string)myReader["Kennzeich"]; else data.Kennzeich = "";
                list.Add(new Umsatz { MwSt = data.MwSt, Bezeich = data.Bezeich, Schlussel = data.Schlussel, Beschreibung = data.Beschreibung, Konto = data.Konto, GKonto = data.GKonto, Kennzeich = data.Kennzeich });
            }
            Data = list;
            UmsatzsteuerDataGrid.ItemsSource = Data;
            list = new List<Umsatz>();
        }

        private void CustomizeHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e) { if (e.Column.Header.ToString() == "MwSt") e.Column.Header = "MwSt%"; if (e.Column.Header.ToString() == "GKonto") e.Column.Header = "G.Konto"; }

        private void RecordSelected(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NeuUmsatzsteuer_Click(object sender, RoutedEventArgs e) { UmsatzsteuerDataGrid.SelectedItem = new DataGridRow(); }

        private void speichern_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
