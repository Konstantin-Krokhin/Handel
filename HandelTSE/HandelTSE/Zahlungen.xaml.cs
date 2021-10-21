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
    /// Interaction logic for Zahlungen.xaml
    /// </summary>
    public partial class Zahlungen : UserControl
    {
        Zahlung data = new Zahlung();
        List<Zahlung> list = new List<Zahlung>();
        public static OleDbConnection con = new OleDbConnection();
        public List<Zahlung> Data { get; set; }
        public class Zahlung
        {
            public Int32 Id { get; set; }
            public string Nr { get; set; }
            public string Zahlungsmethode { get; set; }
            public string Status { get; set; }
            public string ZArt { get; set; }
            public string Bemerkung { get; set; }
        }
        public Zahlungen()
        {
            InitializeComponent();
            
            // If the menu ProgramEinstellungen is being open multiple times
            if (con.ConnectionString.Length == 0)
            {
                con = MainWindow.con;
                OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_Zahlungen] ([Id] COUNTER, [Nr] TEXT(55),[Zahlungsmethode] TEXT(55),[Status] TEXT(55),[ZArt] TEXT(55),[Bemerkung] TEXT(55))", con);
                try
                {
                    cmd.ExecuteNonQuery();
                    //Create first empty record with the proper starting ID and Schlussel starting from 1
                    OleDbCommand cmd2 = new OleDbCommand("insert into [TBL_Zahlungen](Id)Values('" + 1 + "')", con);
                    try { cmd2.ExecuteNonQuery(); }
                    catch { }
                }
                catch { }
            }

            if (MainWindow.con.State != System.Data.ConnectionState.Closed) LoadGrid();
            HideColumns();
        }

        private void LoadGrid()
        {
            OleDbCommand cmd2 = new OleDbCommand("SELECT * FROM [TBL_Zahlungen]", con);
            OleDbDataReader myReader = cmd2.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["Id"] != DBNull.Value) data.Id = (Int32)myReader["Id"]; else data.Id = -1;
                if (myReader["Nr"] != DBNull.Value) data.Nr = (string)myReader["Nr"]; else data.Nr = "";
                if (myReader["Zahlungsmethode"] != DBNull.Value) data.Zahlungsmethode = (string)myReader["Zahlungsmethode"]; else data.Zahlungsmethode = "";
                if (myReader["Status"] != DBNull.Value) data.Status = (string)myReader["Status"]; else data.Status = "";
                if (myReader["ZArt"] != DBNull.Value) data.ZArt = (string)myReader["ZArt"]; else data.ZArt = "";
                if (myReader["Bemerkung"] != DBNull.Value) data.Bemerkung = (string)myReader["Bemerkung"]; else data.Bemerkung = "";
                list.Add(new Zahlung { Id = data.Id, Nr = data.Nr, Zahlungsmethode = data.Zahlungsmethode, Status = data.Status, ZArt = data.ZArt, Bemerkung = data.Bemerkung });
            }
            Data = list;
            ZahlungenDataGrid.ItemsSource = Data;
            list = new List<Zahlung>();
        }

        private void CustomizeHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e) { if (e.Column.Header.ToString() == "Nr") e.Column.Header = "Nr."; if (e.Column.Header.ToString() == "ZArt") e.Column.Header = "Z-Art"; }

        private void NeuZahlungsmethode_Click(object sender, RoutedEventArgs e) { NeuZahlungWindow.Visibility = Visibility.Visible; ZahlungenDataGrid.SelectedItem = null; NeuZahlungsmethodeButton.IsEnabled = false; }

        private void ZahlungenDataGrid_Loaded(object sender, RoutedEventArgs e) { }

        private void HideColumns() { if (ZahlungenDataGrid.Items.Count > 0 && ZahlungenDataGrid.Columns.Count > 0) foreach (var item in ZahlungenDataGrid.Columns) if (item.Header.ToString() == "Id") item.Visibility = Visibility.Collapsed; }

        private void RecordSelected(object sender, SelectionChangedEventArgs e) { NeuZahlungsmethodeButton.IsEnabled = false; if (NeuZahlungWindow.Visibility != Visibility.Visible) NeuZahlungWindow.Visibility = Visibility.Visible; ZahlungenDataGrid.IsEnabled = false; }

        private void NeuZahlungWindow_CloseButtonClicked(object sender, RoutedEventArgs e) { NeuZahlungWindow.Visibility = Visibility.Collapsed; NeuZahlungsmethodeButton.IsEnabled = true; ZahlungenDataGrid.IsEnabled = true; }

        private void speichern_Click(object sender, RoutedEventArgs e)
        {
            Int32 ID = 0;
            int result = 0;
            Zahlung item = null;
            if (ZahlungenDataGrid.SelectedItem != null) item = ((Zahlung)ZahlungenDataGrid.SelectedItem);
            OleDbCommand cmd = new OleDbCommand();
            if (item != null)
            {
                ID = item.Id;
                cmd = new OleDbCommand("UPDATE [TBL_Zahlungen] SET Nr = @Nr, Zahlungsmethode = @Zahlungsmethode, Status = @Status, ZArt = @ZArt, Bemerkung = @Bemerkung WHERE Id = @ID", con);

                cmd.Parameters.Add(new OleDbParameter("@Nr", item.Nr));
                cmd.Parameters.Add(new OleDbParameter("@Zahlungsmethode", item.Zahlungsmethode));
                cmd.Parameters.Add(new OleDbParameter("@Status", item.Status));
                cmd.Parameters.Add(new OleDbParameter("@ZArt", item.ZArt));
                cmd.Parameters.Add(new OleDbParameter("@Bemerkung", item.Bemerkung));
                cmd.Parameters.Add(new OleDbParameter("@ID", ID));
            }
            else
            {
                OleDbCommand maxCommand = new OleDbCommand("SELECT max(Id) from TBL_Zahlungen", con);
                try { ID = (Int32)maxCommand.ExecuteScalar(); } catch { }
                cmd = new OleDbCommand("insert into [TBL_Zahlungen](Id, Nr, Zahlungsmethode, Status, ZArt, Bemerkung)Values('" + ++ID + "','" + 0 + "','" + Zahlungsname.Text + "','"  + StatusButton.Content + "','" + Zahlungsart.Text  + "','" + Bemerkung.Text + "')", con);
            }

            try { result = cmd.ExecuteNonQuery(); LoadGrid(); HideColumns(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

            NeuZahlungsmethodeButton.IsEnabled = true;
        }
    }
}
