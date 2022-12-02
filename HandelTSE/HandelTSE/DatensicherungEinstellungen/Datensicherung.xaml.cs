using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using HandelTSE.DatensicherungEinstellungen;
using System.IO;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.SQLite;
using HandelTSE.ViewModels;

namespace HandelTSE
{
    /// <summary>
    /// Interaction logic for Datensicherung.xaml
    /// </summary>
    public partial class Datensicherung : UserControl
    {
        List<Files> list = new List<Files>();
        public List<Files> Data { get; set; }
        public string dbExceptionMsg = "";
        public static SQLiteConnection con;

        public class Files
        {
            public string Datenbank { get; set; }
            public string Grosse { get; set; }
            public string Datum { get; set; }
        }

        public Datensicherung()
        {
            InitializeComponent();

            if (con == null) con = MainWindow.con;
            DatenbankDirectoryTextBox.Text = Properties.Settings.Default.Datenbank;
            SaveDirectoryTextBox.Text = Properties.Settings.Default.Verzeichnis;

            ListFilesUnderDirectoryOnDG();

            var image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(System.Drawing.SystemIcons.Information.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            icon.Source = image;
        }

        private void RecordSelected(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DatenbankDataGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OpenDirectory_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Offen";
            openFileDialog.Filter = "Alle Dateien *.*|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                DatenbankDirectoryTextBox.Text = openFileDialog.FileName;
            }
        }

        private void SearchDirectory_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                if (dialog.SelectedPath != "") SaveDirectoryTextBox.Text = dialog.SelectedPath;
            }
        }

        private void ODBC_Clicked(object sender, RoutedEventArgs e) 
        {
            string caption = "Information";
            string messageBoxText = "Wollen Sie wirklich die Daten-Einstellungen ändern?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.None;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    Content = new ODBCDatensicherung();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void DatenbankSpeichernButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Datenbank = DatenbankDirectoryTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void CustomizeHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Grosse") e.Column.Header = "Gr. KB";
        }

        private void VerzeichnisSpeichernButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Verzeichnis = SaveDirectoryTextBox.Text;
            Properties.Settings.Default.Save();

            ListFilesUnderDirectoryOnDG();
        }

        public void ListFilesUnderDirectoryOnDG()
        {
            string path = SaveDirectoryTextBox.Text;
            if (!Directory.Exists(path)) { Data = list; DatenbankDataGrid.ItemsSource = Data; DatenbankDataGrid.Items.Refresh(); return; }
            
            string[] files = GetFileNames(path, "*.mdb|*.zip|*.db", SearchOption.TopDirectoryOnly);
            string[] date = new string[files.Length];
            string[] size = GetFileSize(path, "*.mdb|*.zip|*.db", SearchOption.TopDirectoryOnly, date);

            if (files != null)
            {
                for (int i = 0; i < files.Length; i++) list.Add(new Files { Datenbank = files[i], Grosse = size[i], Datum = date[i] });
                Data = list;
                DatenbankDataGrid.ItemsSource = Data;
                list = new List<Files>();
            }
        }

        private static string[] GetFileNames(string sourceFolder, string filters, System.IO.SearchOption searchOption)
        {
            return filters.Split('|').SelectMany(filter => Directory.EnumerateFiles(sourceFolder, filter, searchOption)).Select(System.IO.Path.GetFileName).ToArray();
        }

        private string[] GetFileSize(string sourceFolder, string filters, System.IO.SearchOption searchOption, string[] date)
        {
            string[] info = filters.Split('|').SelectMany(filter => Directory.EnumerateFiles(sourceFolder, filter, searchOption)).ToArray();

            long holder;
            for (int i = 0; i < info.Length; i++) { date[i] = new FileInfo(info[i]).CreationTime.ToString(); holder = new FileInfo(info[i]).Length; holder /= 1000; info[i] = holder.ToString(); }
            return info;
        }

        private void VerzeichnisLeeren_Clicked(object sender, RoutedEventArgs e)
        {
            string caption = "Datei entfernen...";
            string messageBoxText = "Wollen Sie wirklich alle Daten entfernen?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    var files = Directory.GetFiles(Properties.Settings.Default.Verzeichnis, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".mdb") || s.EndsWith(".zip"));
                    foreach (string file in files) { File.Delete(file); }
                    ListFilesUnderDirectoryOnDG();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void DatenbankSicherungskopieClicked(object sender, RoutedEventArgs e)
        {
            Datenbank_Sicherungskopie window = new Datenbank_Sicherungskopie();
            window.Show();
        }

        private void DatenbankKomprimieren_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Properties.Settings.Default.Verzeichnis + "\\old_db_handel.db")) File.Delete(Properties.Settings.Default.Verzeichnis + "\\old_db_handel.db");
            System.IO.File.Move(Properties.Settings.Default.Verzeichnis + "\\db_handel.db", Properties.Settings.Default.Verzeichnis + "\\old_db_handel.db");
            File.Copy(Properties.Settings.Default.Datenbank, Properties.Settings.Default.Verzeichnis + "\\db_handel.db", true);
            
            string caption = "Datenbank komprimieren";
            string messageBoxText = Properties.Settings.Default.Verzeichnis + "\\db_handel.db" + "\n\nwurde erfolgreich komprimiert.";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);
        }

        private void VerbindungTesten_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(DatenbankDirectoryTextBox.Text)) { MessageBox.Show("Keine Verbindung mit der Datenbank.\nCould not find file\n'" + DatenbankDirectoryTextBox.Text + "'."); return; }

            if (IsServerConnected() == true) MessageBox.Show("Die Verbindung mit der Datenbank ist erfolgreich.");
            else MessageBox.Show("Keine Verbindung mit der Datenbank.\n" + dbExceptionMsg + "\n'" + DatenbankDirectoryTextBox.Text + "'.");
        }

        public bool IsServerConnected()
        {
            using (SQLiteConnection db = new SQLiteConnection(@"Data Source=" + DatenbankDirectoryTextBox.Text + ";FailIfMissing=True;"))
            {
                try
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction()) { transaction.Rollback(); }
                }
                catch (SQLiteException ex)
                {
                    dbExceptionMsg = ex.Message;
                    return false;
                }
            }
            return true;
        }

        private void DatenbankErstbetriebVorbereiten_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Daten löschen...";
            string messageBoxText = "Wollen Sie wirklich die Daten löschen?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    messageBoxText = "WARNUNG!\n\nIhre Daten werden unwiderruflich gelöscht.\n\nSind Sie ganz sicher?";
                    messageResult = MessageBox.Show(messageBoxText, caption, button, icon);
                    switch (messageResult)
                    {
                        case MessageBoxResult.OK:

                            string[] tables = { "TBL_ProgramEinstellungenKassennetz", "TBL_ProgramEinstellungenDarstellung", "TBL_ProgramEinstellungenFirmendaten", "TBL_ProgramEinstellungenKassendaten", "TBL_PRESSE", "TBL_EANCode", "TBL_PERSONAL", "TBL_Stornogrunde", "TBL_Umsatzsteuer", "TBL_FunktionsEinstellungenProgram", "TBL_FunktionsEinstellungenFunktionen", "TBL_FunktionsEinstellungenAbschlag", "TBL_FunktionsEinstellungenBondrucker", "TBL_FunktionsEinstellungenGutscheine", "TBL_Zahlungen" };
                            string[] sqlStatements = new string[tables.Count()];
                            for (int i = 0; i < tables.Count(); i++) { sqlStatements[i] = "DROP TABLE IF EXISTS " + tables[i]; }

                            // Run all commands for dropping (emptying) the records
                            if (con.State == System.Data.ConnectionState.Closed) con.Open();
                            SQLiteTransaction transaction = con.BeginTransaction();
                            foreach (string statement in sqlStatements)
                                using (SQLiteCommand cmd0 = new SQLiteCommand(statement, con, transaction)) { cmd0.ExecuteNonQuery(); }
                            transaction.Commit();

                            messageBoxText = "Die Kasse wird für den ersten Betrieb vorbereitet.";
                            button = MessageBoxButton.OK;
                            icon = MessageBoxImage.None;
                            MessageBox.Show(messageBoxText, caption, button, icon);
                            ProgramEinstellungen.con = null;
                            FunktionsEinstellungen.con = null;
                            Stornogrunde.con = null;
                            Umsatzsteuer.con = null;
                            Zahlungen.con = null;
                            con = null;
                            Personalverwaltung.con = null;
                            PresseUndVMP.con = null;
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void WerkeinstellungenWiederherstellen_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Information";
            string messageBoxText = "Wollen Sie wirklich Werkeinstellungen wiederherstellen?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    // ?
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void AlsWerkeinstellungenSpeichern_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Information";
            string messageBoxText = "Wollen Sie wirklich die Einstellungen sichern?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    // ?
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
    }
}
