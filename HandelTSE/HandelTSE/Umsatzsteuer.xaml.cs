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
        int maxSchlussel = 0;

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

            // If the menu Umsatzsteuer is being open multiple times
            if (con.ConnectionString.Length == 0)
            {
                con = MainWindow.con;
                OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_Umsatzsteuer] ([Id] COUNTER, [MwSt] TEXT(55),[Bezeich] TEXT(55),[Schlussel] TEXT(55),[Beschreibung] TEXT(55),[Konto] TEXT(55),[GKonto] TEXT(55), [Kennzeich] TEXT(55))", con);
                try 
                { 
                    cmd.ExecuteNonQuery();
                    //Create first empty record with the proper starting ID and Schlussel starting from 1
                    OleDbCommand cmd2 = new OleDbCommand("insert into [TBL_Umsatzsteuer](Id, Schlussel)Values('" + 1 + "','" + 1 + "')", con);
                    try { cmd2.ExecuteNonQuery(); }
                    catch { }
                }
                catch { }
            }

            if (MainWindow.con.State != System.Data.ConnectionState.Closed) LoadGrid();
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

        private void RecordSelected(object sender, SelectionChangedEventArgs e) { if (UmsatzsteuerDataGrid.SelectedItem != null && UmsatzsteuerDataGrid.SelectedIndex != UmsatzsteuerDataGrid.Items.Count - 1 && ((Umsatz)UmsatzsteuerDataGrid.Items[UmsatzsteuerDataGrid.Items.Count - 1]).Schlussel == null) {Data.RemoveAt(UmsatzsteuerDataGrid.Items.Count-1); UmsatzsteuerDataGrid.ItemsSource = Data; UmsatzsteuerDataGrid.Items.Refresh(); NeuUmsatzsteuerButton.IsEnabled = true; } }

        private void NeuUmsatzsteuer_Click(object sender, RoutedEventArgs e) { if(Data != null) Data.Add(new Umsatz { }); UmsatzsteuerDataGrid.ItemsSource = Data; UmsatzsteuerDataGrid.Items.Refresh(); UmsatzsteuerDataGrid.SelectedIndex = UmsatzsteuerDataGrid.Items.Count - 1; NeuUmsatzsteuerButton.IsEnabled = false; }

        private void speichern_Click(object sender, RoutedEventArgs e)
        {
            int k = 0;
            Umsatz item = ((Umsatz)UmsatzsteuerDataGrid.SelectedItem);
            if (item == null || item.MwSt == null || item.MwSt == "" || (!item.MwSt.All(char.IsDigit) && !Double.TryParse(item.MwSt, out _))) k = 1;
            if (k == 1) { MessageBox.Show("Überprüfen Sie bitte die Steuerndaten (Steuersatz und Bezeichnung)!"); return; }
            NeuUmsatzsteuerButton.IsEnabled = true;

            Int32 ID = 0;
            int result = 0;
            string Bezeich = "";
            if (item.MwSt.Contains(".") || item.MwSt.Contains(",")) Bezeich = item.MwSt + "%"; else Bezeich = item.MwSt + ".00" + "%";
            OleDbCommand cmd = new OleDbCommand();
            if (item.Id != 0) 
            { 
                ID = item.Id;
                cmd = new OleDbCommand("UPDATE [TBL_Umsatzsteuer] SET MwSt = @MwSt, Bezeich = @Bezeich, Schlussel = @Schlussel, Beschreibung = @Beschreibung, Konto = @Konto, GKonto = @GKonto, Kennzeich = @Kennzeich WHERE Id = @ID", con);

                cmd.Parameters.Add(new OleDbParameter("@MwSt", item.MwSt));
                cmd.Parameters.Add(new OleDbParameter("@Bezeich", Bezeich));
                cmd.Parameters.Add(new OleDbParameter("@Schlussel", item.Schlussel));
                cmd.Parameters.Add(new OleDbParameter("@Beschreibung", item.Beschreibung));
                cmd.Parameters.Add(new OleDbParameter("@Konto", item.Konto));
                cmd.Parameters.Add(new OleDbParameter("@GKonto", item.GKonto));
                cmd.Parameters.Add(new OleDbParameter("@Kennzeich", item.Kennzeich));
                cmd.Parameters.Add(new OleDbParameter("@ID", ID));
            }
            else
            {
                int currentSchlussel = 0;
                OleDbCommand maxCommand = new OleDbCommand("SELECT max(Id) from TBL_Umsatzsteuer", con);
                try { ID = (Int32)maxCommand.ExecuteScalar(); } catch { }
                foreach (var i in UmsatzsteuerDataGrid.Items) { if (((Umsatz)i).Schlussel == null) break; currentSchlussel = int.Parse(((Umsatz)i).Schlussel); if (currentSchlussel > maxSchlussel) maxSchlussel = currentSchlussel; }
                cmd = new OleDbCommand("insert into [TBL_Umsatzsteuer](Id, MwSt, Bezeich, Schlussel, Beschreibung, Konto, GKonto, Kennzeich)Values('" + ++ID + "','" + item.MwSt + "','" + Bezeich + "','" + ++maxSchlussel + "','" + item.Beschreibung + "','" + item.Konto + "','" + item.GKonto + "','" + item.Kennzeich + "')", con);
            }

            try { result = cmd.ExecuteNonQuery(); LoadGrid(); HideColumns(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
        }

        private void HideColumns() { if (UmsatzsteuerDataGrid.Items.Count > 0 && UmsatzsteuerDataGrid.Columns.Count > 0) foreach (var item in UmsatzsteuerDataGrid.Columns) { if (item.Header.ToString() == "Id") item.Visibility = Visibility.Collapsed; if (item.Header.ToString() == "Schlussel") item.IsReadOnly = true; if (item.Header.ToString() == "Bezeich") item.IsReadOnly = true; } }

        private void UmsatzsteuerDataGrid_Loaded(object sender, RoutedEventArgs e) { HideColumns(); }

        private void UmsatzsteuerDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e) { }

        private void Loschen_Click(object sender, RoutedEventArgs e)
        {
            if (UmsatzsteuerDataGrid.SelectedItem == null) { MessageBox.Show("Bitte wählen Sie den Datensatz in der Tabelle aus!"); return; }

            string caption = "Datensatz entfernen...";
            string messageBoxText = "Wollen Sie wirklich entfernen?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    OleDbCommand cmd = new OleDbCommand("DELETE FROM [TBL_Umsatzsteuer] where Id = @ID", con);
                    cmd.Parameters.Add(new OleDbParameter("@ID", ((Umsatz)UmsatzsteuerDataGrid.SelectedItem).Id));
                    int result = 0;
                    try { result = cmd.ExecuteNonQuery(); }
                    catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

                    LoadGrid();
                    HideColumns();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
    }
}
