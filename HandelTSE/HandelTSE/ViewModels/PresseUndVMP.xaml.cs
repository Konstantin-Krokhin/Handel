using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
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
    /// Interaction logic for PresseUndVMP.xaml
    /// </summary>
    public partial class PresseUndVMP : UserControl
    {
        Presse data = new Presse();
        List<Presse> list = new List<Presse>();
        public static OleDbConnection con = new OleDbConnection();
        public List<Presse> Data { get; set; }
        Brush brush_red = new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)128, (byte)128));


        public class Presse
        {
            public Int32 Id { get; set; }
            public string CEAN { get; set; }
            public string CNAME { get; set; }
        }

        public PresseUndVMP()
        {
            InitializeComponent();

            CSVDateiPath.Text = Globals.CsvZeitungenFilePath;

            // If the menu PresseUndVMP is being open multiple times
            if (con.ConnectionString.Length == 0)
            {
                string str = "";
                con = MainWindow.con;
                OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_PRESSE] ([Id] COUNTER, [CEAN] TEXT(55), [CNAME] TEXT(55))", con);
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
                        using (OleDbCommand cmd2 = new OleDbCommand(statement, con, transaction))
                        {
                            cmd2.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch { }
            }

            if (MainWindow.con.State != System.Data.ConnectionState.Closed) LoadGrid();
        }

        private void LoadGrid()
        {
            OleDbCommand cmd2 = new OleDbCommand("SELECT * FROM [TBL_PRESSE]", con);
            OleDbDataReader myReader = cmd2.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["Id"] != DBNull.Value) data.Id = (Int32)myReader["Id"]; else data.Id = -1;
                if (myReader["CEAN"] != DBNull.Value) data.CEAN = (string)myReader["CEAN"]; else data.CEAN = "";
                if (myReader["CNAME"] != DBNull.Value) data.CNAME = (string)myReader["CNAME"]; else data.CNAME = "";
                list.Add(new Presse { Id = data.Id, CEAN = data.CEAN, CNAME = data.CNAME });
            }
            Data = list;
            EANDataGrid.ItemsSource = Data;
            list = new List<Presse>();
        }

        private void EANPressecodeDataGrid_Loaded(object sender, RoutedEventArgs e) { HideColumn(); }

        private void RecordSelected(object sender, SelectionChangedEventArgs e)
        {
            if (EANDataGrid.SelectedItem == null) return;
            var dg = sender as System.Windows.Controls.DataGrid;
            if (dg == null) return;
            var row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
            if (row == null) return;

            EANZeitungTextBox.Text = ((Presse)row.Item).CEAN;
            BezeichnungZeitungTextBox.Text = ((Presse)row.Item).CNAME;

            EANZeitungTextBox.IsEnabled = true;
            BezeichnungZeitungTextBox.IsEnabled = true;

            if (LoschenZeitungButton.IsEnabled == false) LoschenZeitungButton.IsEnabled = true;
            if (SpeichernZeitungButton.IsEnabled == false) SpeichernZeitungButton.IsEnabled = true;
        }

        private void NeuPresseCode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoschenPresseCode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void speichernPresseCode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NeuZeitung_Click(object sender, RoutedEventArgs e)
        {
            EANDataGrid.SelectedItem = null;
            if (LoschenZeitungButton.IsEnabled == true) LoschenZeitungButton.IsEnabled = false;
            SpeichernZeitungButton.IsEnabled = true;
            EANZeitungTextBox.Text = "";
            BezeichnungZeitungTextBox.Text = "";
            EANZeitungTextBox.IsEnabled = true;
            BezeichnungZeitungTextBox.IsEnabled = true;
        }

        // ------------------------- SAVING PART --------------------------------------------
        private void SpeichernZeitungButton_Click(object sender, RoutedEventArgs e)
        {
            int k = 0;
            if (EANZeitungTextBox.Text == "") { EANZeitungTextBox.Background = brush_red; k = 1; }
            if (BezeichnungZeitungTextBox.Text == "") { BezeichnungZeitungTextBox.Background = brush_red; k = 1; }
            if (k == 1) return;

            Int32 ID = 0;
            int result = 0;
            Presse item = (Presse)EANDataGrid.SelectedItem;
            OleDbCommand cmd = new OleDbCommand();
            if (item != null)
            {
                ID = item.Id;
                cmd = new OleDbCommand("UPDATE [TBL_PRESSE] SET CEAN = @CEAN, CNAME = @CNAME WHERE Id = @ID", con);

                cmd.Parameters.Add(new OleDbParameter("@CEAN", EANZeitungTextBox.Text));
                cmd.Parameters.Add(new OleDbParameter("@CNAME", BezeichnungZeitungTextBox.Text));
                cmd.Parameters.Add(new OleDbParameter("@ID", ID));
            }
            else
            {
                OleDbCommand maxCommand = new OleDbCommand("SELECT max(Id) from TBL_PRESSE", con);
                try { ID = (Int32)maxCommand.ExecuteScalar(); } catch { }
                cmd = new OleDbCommand("insert into [TBL_PRESSE](Id, CEAN, CNAME)Values('" + ++ID + "','" + EANZeitungTextBox.Text + "','" + BezeichnungZeitungTextBox.Text + "')", con);
            }

            try { result = cmd.ExecuteNonQuery(); LoadGrid(); HideColumn(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

            EANDataGrid.SelectedItem = null;
            LoschenZeitungButton.IsEnabled = false;
            SpeichernZeitungButton.IsEnabled = false;
            EANZeitungTextBox.Text = "";
            BezeichnungZeitungTextBox.Text = "";
            EANZeitungTextBox.IsEnabled = false;
            BezeichnungZeitungTextBox.IsEnabled = false;
        }

        // -------------------------------------------------------------------------------------

        private void LoschenZeitungButton_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Datensatz löschen...";
            string messageBoxText = "Wollen Sie wirklich löschen?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    OleDbCommand cmd = new OleDbCommand("DELETE FROM [TBL_PRESSE] where Id = @Id", con);
                    cmd.Parameters.Add(new OleDbParameter("@Id", ((Presse)EANDataGrid.SelectedItem).Id));

                    try { cmd.ExecuteNonQuery(); }
                    catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

                    LoadGrid();
                    HideColumn();

                    EANDataGrid.SelectedItem = null;
                    EANZeitungTextBox.Text = "";
                    EANZeitungTextBox.IsEnabled = false;
                    BezeichnungZeitungTextBox.Text = "";
                    BezeichnungZeitungTextBox.IsEnabled = false;
                    LoschenZeitungButton.IsEnabled = false;
                    SpeichernZeitungButton.IsEnabled = false;
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void EANZeitungTextBox_TextChanged(object sender, TextChangedEventArgs e) { if (EANZeitungTextBox.Background == brush_red) EANZeitungTextBox.Background = Brushes.White; }

        private void BezeichnungZeitungTextBox_TextChanged(object sender, TextChangedEventArgs e) { if (BezeichnungZeitungTextBox.Background == brush_red) BezeichnungZeitungTextBox.Background = Brushes.White; }
        private void HideColumn() { if (EANDataGrid.Items.Count > 0 && EANDataGrid.Columns.Count > 0) { foreach (var item in EANDataGrid.Columns) { if (item.Header.ToString() == "Id") item.Visibility = Visibility.Collapsed; } EANDataGrid.Columns[1].Width = 120; EANDataGrid.Columns[2].Width = 260; } }

        private void CSVExportieren_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Ausgabedatei wählen";
            saveFileDialog.FileName = "Presse";
            saveFileDialog.Filter = "csv (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, "EAN;Bezeichnung;\n");
                for (int i = 0; i < Data.Count; i++)
                {
                    File.AppendAllText(saveFileDialog.InitialDirectory + saveFileDialog.FileName, Data[i].CEAN + ";" + Data[i].CNAME + ";\n", Encoding.Unicode);
                }
                MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!");
            }
        }

        private void CustomizeHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "CEAN") e.Column.Header = "EAN";
            if (e.Column.Header.ToString() == "CNAME") e.Column.Header = "Bezeichnung";
        }

        private void CSVImportieren_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Offen";
            openFileDialog.Filter = "CSV (*.csv)|*.csv|Excell-Dateien (*.xls)|*.xls|Alle Dateien (*.csv; *.xls)|*.csv;*.xls";
            if (openFileDialog.ShowDialog() == true)
            {
                CSVDateiPath.Text = openFileDialog.FileName;
                Globals.CsvZeitungenFilePath = CSVDateiPath.Text;
                Content = new CSVImportieren();
            }
        }
    }
}
