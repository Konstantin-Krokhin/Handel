using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Xml;
using System.Configuration;
using System.Windows.Threading;
using System.Data;

namespace HandelTSE.ViewModels
{
    /// <summary>
    /// Interaction logic for PresseUndVMP.xaml
    /// </summary>
    public partial class PresseUndVMP : UserControl
    {
        Presse data = new Presse();
        EANCode data2 = new EANCode();
        List<Presse> list = new List<Presse>();
        List<EANCode> list2 = new List<EANCode>();
        public static SQLiteConnection con = new SQLiteConnection();
        public List<Presse> Data { get; set; }
        public List<EANCode> Data2 { get; set; }
        Brush brush_red = new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)128, (byte)128));

        public class Presse
        {
            public Int32 Id { get; set; }
            public string CEAN { get; set; }
            public string CNAME { get; set; }
        }

        public class EANCode
        {
            public Int32 Id { get; set; }
            public string Landprafix { get; set; }
            public string PresseKZ { get; set; }
            public string MwSt { get; set; }
            public string VDZ { get; set; }
            public string Verkaufspreis { get; set; }
            public string Bezeichnung { get; set; }
        }

        public PresseUndVMP()
        {
            InitializeComponent();

            con = Globals.PresseCon;
            CSVDateiPath.Text = Globals.CsvZeitungenFilePath;
            EhastraKundennummerTextBox.Text = Properties.Settings.Default.EhastraKundennummer;

            if (MainWindow.con.State != System.Data.ConnectionState.Closed) LoadGrid();
        }

        private void LoadGrid()
        {
            SQLiteCommand cmd2 = new SQLiteCommand("SELECT * FROM [TBL_PRESSE]", con);
            SQLiteDataReader myReader = cmd2.ExecuteReader();
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

            SQLiteCommand cmd3 = new SQLiteCommand("SELECT * FROM [TBL_EANCode]", con);
            SQLiteDataReader myReader3 = cmd3.ExecuteReader();
            while (myReader3.Read())
            {
                if (myReader3["Id"] != DBNull.Value) data2.Id = (Int32)myReader3["Id"]; else data2.Id = -1;
                if (myReader3["Landprafix"] != DBNull.Value) data2.Landprafix = (string)myReader3["Landprafix"]; else data2.Landprafix = "";
                if (myReader3["PresseKZ"] != DBNull.Value) data2.PresseKZ = (string)myReader3["PresseKZ"]; else data2.PresseKZ = "";
                if (myReader3["MwSt"] != DBNull.Value) data2.MwSt = (string)myReader3["MwSt"]; else data2.MwSt = "";
                if (myReader3["VDZ"] != DBNull.Value) data2.VDZ = (string)myReader3["VDZ"]; else data2.VDZ = "";
                if (myReader3["Verkaufspreis"] != DBNull.Value) data2.Verkaufspreis = (string)myReader3["Verkaufspreis"]; else data2.Verkaufspreis = "";
                if (myReader3["Bezeichnung"] != DBNull.Value) data2.Bezeichnung = (string)myReader3["Bezeichnung"]; else data2.Bezeichnung = "";
                list2.Add(new EANCode { Id = data2.Id, Landprafix = data2.Landprafix, PresseKZ = data2.PresseKZ, MwSt = data2.MwSt, VDZ = data2.VDZ, Verkaufspreis = data2.Verkaufspreis, Bezeichnung = data2.Bezeichnung });
            }
            Data2 = list2;
            EANPressecodeDataGrid.ItemsSource = Data2;
            
            list2 = new List<EANCode>();
        }

        private void EANPressecodeDataGrid_Loaded(object sender, RoutedEventArgs e) { HideColumn2(); }
        private void EANDataGrid_Loaded(object sender, RoutedEventArgs e) { HideColumn(); }

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
            SQLiteCommand cmd = new SQLiteCommand();
            if (item != null)
            {
                ID = item.Id;
                cmd = new SQLiteCommand("UPDATE [TBL_PRESSE] SET CEAN = @CEAN, CNAME = @CNAME WHERE Id = @ID", con);

                cmd.Parameters.Add(new SQLiteParameter("@CEAN", EANZeitungTextBox.Text));
                cmd.Parameters.Add(new SQLiteParameter("@CNAME", BezeichnungZeitungTextBox.Text));
                cmd.Parameters.Add(new SQLiteParameter("@ID", ID));
            }
            else
            {
                SQLiteCommand maxCommand = new SQLiteCommand("SELECT max(Id) from TBL_PRESSE", con);
                try { ID = (Int32)maxCommand.ExecuteScalar(); } catch { }
                cmd = new SQLiteCommand("insert into [TBL_PRESSE](Id, CEAN, CNAME)Values('" + ++ID + "','" + EANZeitungTextBox.Text + "','" + BezeichnungZeitungTextBox.Text + "')", con);
            }

            try { result = cmd.ExecuteNonQuery(); LoadGrid(); HideColumn(); HideColumn2(); }
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
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM [TBL_PRESSE] where Id = @Id", con);
                    cmd.Parameters.Add(new SQLiteParameter("@Id", ((Presse)EANDataGrid.SelectedItem).Id));

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
        private void HideColumn2() { EANPressecodeDataGrid.Columns[0].Visibility = Visibility.Collapsed; }
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
                    File.AppendAllText(saveFileDialog.FileName, Data[i].CEAN + ";" + Data[i].CNAME + ";\n", Encoding.Unicode);
                }
                MessageBox.Show("Ihre Daten wurden erfolgreich in '" + saveFileDialog.FileName + "' exportiert.");
            }
        }

        private void CustomizeHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "CEAN") e.Column.Header = "EAN";
            if (e.Column.Header.ToString() == "CNAME") e.Column.Header = "Bezeichnung";
        }
        private void CustomizeEANHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Landprafix") e.Column.Header = "     Landpräfix\nDeutschland - 41";
            if (e.Column.Header.ToString() == "PresseKZ") e.Column.Header = "Presse-KZ\n      (4)";
            if (e.Column.Header.ToString() == "MwSt") e.Column.Header = "MwSt.\n(19%)";
            if (e.Column.Header.ToString() == "VDZ") e.Column.Header = "VDZ - Nr.\n  XXXXX";
            if (e.Column.Header.ToString() == "Verkaufspreis") e.Column.Header = "Verkaufspreis\n       XXXX";
        }

        private async void CSVImportieren_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Offen";
            openFileDialog.Filter = "CSV (*.csv)|*.csv|Excell-Dateien (*.xls)|*.xls|Alle Dateien (*.csv; *.xls)|*.csv;*.xls";
            if (openFileDialog.ShowDialog() == true)
            {
                CSVDateiPath.Text = openFileDialog.FileName;
                Globals.CsvZeitungenFilePath = CSVDateiPath.Text;
                
                progressBar.Visibility = Visibility.Visible;
                CSVImportieren.Visibility = Visibility.Collapsed;
                await Task.Run(() => LoadImportData());
                CSVImportieren.Visibility = Visibility.Visible;
                progressBar.Visibility = Visibility.Collapsed;

                Content = new CSVImportieren();
            }
        }

        // Loads the data to pass to the CSVImportieren window DataGrid
        public async Task LoadImportData()
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(Globals.CsvZeitungenFilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
            Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

            string strCellData = "";
            int rowCnt = 0;
            int colCnt = 0;

            colCnt = 1;
            string cean = "", cname = "";
            for (rowCnt = 1; rowCnt <= excelRange.Rows.Count; rowCnt++)
            {
                strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;

                cean = strCellData.Substring(0, strCellData.IndexOf(";"));
                cname = strCellData.Substring(strCellData.IndexOf(";") + 1);

                if (cname.Contains(";"))
                {
                    int i = cname.Length - cname.IndexOf(";");
                    if (i < 1) i = 1;
                    cname = cname.Remove(cname.IndexOf(";"), i);
                }

                if (rowCnt == 1) { Globals.KopfzeilItem = new CSVImportieren.Presse { CEAN = cean, CNAME = cname }; continue; }
                Globals.presseList.Add(new CSVImportieren.Presse { CEAN = cean, CNAME = cname });
            }

            excelBook.Close(true, null, null);
            excelApp.Quit();
        }

        private void EhastraKundennummerButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.EhastraKundennummer = EhastraKundennummerTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void RecordSelected2(object sender, SelectionChangedEventArgs e)
        {
            SpeichernButton.IsEnabled = true;
            EntfernenButton.IsEnabled = true;
            Int32 id = ((EANCode)EANPressecodeDataGrid.Items[EANPressecodeDataGrid.Items.Count - 1]).Id;
            if (EANPressecodeDataGrid.SelectedItem != null && EANPressecodeDataGrid.SelectedIndex != EANPressecodeDataGrid.Items.Count - 1 && !(id > 0 && id <= int.MaxValue)) { Data2.RemoveAt(EANPressecodeDataGrid.Items.Count - 1); EANPressecodeDataGrid.ItemsSource = Data2; EANPressecodeDataGrid.Items.Refresh(); NeuButton.IsEnabled = true; }
        }

        private void NeuPresseCode_Click(object sender, RoutedEventArgs e)
        {
            if (Data2 != null) Data2.Add(new EANCode { }); EANPressecodeDataGrid.ItemsSource = Data2; EANPressecodeDataGrid.Items.Refresh(); EANPressecodeDataGrid.SelectedIndex = EANPressecodeDataGrid.Items.Count - 1;
            SpeichernButton.IsEnabled = true;
            NeuButton.IsEnabled = false;
            EntfernenButton.IsEnabled = false;
        }

        private void EntfernenPresseCode_Click(object sender, RoutedEventArgs e)
        {
            if (EANPressecodeDataGrid.SelectedItem == null) { MessageBox.Show("Bitte wählen Sie den Datensatz in der Tabelle aus!"); return; }

            string caption = "Datensatz entfernen...";
            string messageBoxText = "Wollen Sie wirklich entfernen?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM [TBL_EANCode] where Id = @ID", con);
                    cmd.Parameters.Add(new SQLiteParameter("@ID", ((EANCode)EANPressecodeDataGrid.SelectedItem).Id));
                    try { cmd.ExecuteNonQuery(); }
                    catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

                    LoadGrid();
                    HideColumn();
                    HideColumn2();
                    //EANPressecodeDataGrid.SelectedItem = null;
                    EntfernenButton.IsEnabled = false;
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void SpeichernPresseCode_Click(object sender, RoutedEventArgs e)
        {
            EANCode item = ((EANCode)EANPressecodeDataGrid.SelectedItem);
            if (item == null || item.Landprafix == null || item.PresseKZ == "" || item.MwSt == "" || item.VDZ == "" || item.Verkaufspreis == "" || item.Bezeichnung == "" || (!item.Landprafix.All(char.IsDigit) && !Double.TryParse(item.Landprafix, out _)) || (!item.PresseKZ.All(char.IsDigit) && !Double.TryParse(item.PresseKZ, out _)) || (!item.MwSt.All(char.IsDigit) && !Double.TryParse(item.MwSt, out _)) || item.VDZ.Length != 5 || item.Verkaufspreis.Length != 4) { MessageBox.Show("Überprüfen Sie bitte Ihre Daten!"); return; }
            NeuButton.IsEnabled = true;

            Int32 ID = 0;
            SQLiteCommand cmd = new SQLiteCommand();
            if (item.Id != 0)
            {
                ID = item.Id;
                cmd = new SQLiteCommand("UPDATE [TBL_EANCode] SET Landprafix = @Landprafix, PresseKZ = @PresseKZ, MwSt = @MwSt, VDZ = @VDZ, Verkaufspreis = @Verkaufspreis, Bezeichnung = @Bezeichnung WHERE Id = @ID", con);

                cmd.Parameters.Add(new SQLiteParameter("@Landprafix", item.Landprafix));
                cmd.Parameters.Add(new SQLiteParameter("@PresseKZ", item.PresseKZ));
                cmd.Parameters.Add(new SQLiteParameter("@MwSt", item.MwSt));
                cmd.Parameters.Add(new SQLiteParameter("@VDZ", item.VDZ));
                cmd.Parameters.Add(new SQLiteParameter("@Verkaufspreis", item.Verkaufspreis));
                cmd.Parameters.Add(new SQLiteParameter("@Bezeichnung", item.Bezeichnung));
                
                cmd.Parameters.Add(new SQLiteParameter("@ID", item.Id));
            }
            else
            {
                SQLiteCommand maxCommand = new SQLiteCommand("SELECT max(Id) from TBL_EANCode", con);
                try { ID = (Int32)maxCommand.ExecuteScalar(); } catch { }
                
                cmd = new SQLiteCommand("insert into [TBL_EANCode](Id, Landprafix, PresseKZ, MwSt, VDZ, Verkaufspreis, Bezeichnung)Values('" + ++ID + "','" + item.Landprafix + "','" + item.PresseKZ + "','" + item.MwSt + "','" + item.VDZ + "','" + item.Verkaufspreis + "','" + item.Bezeichnung + "')", con);
            }

            try { cmd.ExecuteNonQuery(); LoadGrid(); HideColumn2(); HideColumn(); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

        }
    }
}
