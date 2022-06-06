using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO.Compression;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace HandelTSE.DatensicherungEinstellungen
{
    /// <summary>
    /// Interaction logic for Datenbank_Sicherungskopie.xaml
    /// </summary>
    public partial class Datenbank_Sicherungskopie : Window
    {
        public Datenbank_Sicherungskopie()
        {
            InitializeComponent();

            VerzeichnisTextBlock.Text = Properties.Settings.Default.Verzeichnis;
            if (Properties.Settings.Default.LetzteDatensicherung.Any()) Datum.Text = Properties.Settings.Default.LetzteDatensicherung;
        }

        private void ProgrammBeenden_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void SicherungButton_Click(object sender, RoutedEventArgs e)
        {
            // Choose the name and path for backup file
            string destPath = VerzeichnisTextBlock.Text + "\\db_source_" + DateTime.Now.ToString("dd.MM.yyyy") + ".zip";
            
            // Get all files and paths in the given folder (to later count which prefix to add to the backup made in the same day)
            string[] files = Directory.GetFiles(VerzeichnisTextBlock.Text, "db_source_" + DateTime.Now.ToString("dd.MM.yyyy") + "_*.zip", SearchOption.TopDirectoryOnly);
            
            // Numbering the prefixes of the instances of backup copies made in the same day
            if (File.Exists(destPath) && files.Length == 0) { destPath = destPath.Replace(".zip", "_1.zip");  }
            else if (File.Exists(destPath) && files.Length > 0)
            {
                int startIndex = 0;
                int endIndex = 0;
                List<int> numbs = new List<int>();

                // Go through all files and extract each prefix adding them to the integer array to then loop and find the unocupied prefix name
                foreach (string s in files)
                {
                    startIndex = s.LastIndexOf(Regex.Match(s, "db_source_" + DateTime.Now.ToString("dd.MM.yyyy") + "_").Value) + 21;
                    endIndex = s.IndexOf(".zip");
                    try { numbs.Add(Int32.Parse(s.Substring(startIndex, endIndex - startIndex))); } catch { };
                }
                int i = 1;

                // Sort the array by the ascending order and loop until the first free prefix name is found
                numbs.Sort();
                for (; i <= numbs.Count + 1; i++) if (i > numbs.Count || i != numbs[i - 1]) break;
                destPath = destPath.Replace(".zip", "_" + i.ToString() + ".zip");
            }

            string copyDestPath = VerzeichnisTextBlock.Text + "\\db_handel.db";
            string dbPath = Properties.Settings.Default.Datenbank;

            // Checking if admin permissions are used
            var permissionSet = new PermissionSet(PermissionState.None);
            var writePermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, VerzeichnisTextBlock.Text);
            permissionSet.AddPermission(writePermission);

            if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
            {
                // Copy the db file to zip it as the original file might be in use and might give "access denied"error
                //File.Copy(copyDestPath, VerzeichnisTextBlock.Text + "\\copy_db_handel.db", true);

                // Compress a single DB file .db by first creating .zip and then adding to it .db
                using (FileStream fs = new FileStream(destPath, FileMode.Create))
                using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create)) { arch.CreateEntryFromFile(copyDestPath, "db_handel.db"); }
            }
            else { MessageBox.Show("Bitte stellen Sie sicher, dass das Programm mit Administratorrechten geöffnet ist!"); }

            // Saving the Last Backup date in Property Setting to retrieve it next time the window is open for Datum TextBlock
            Properties.Settings.Default.LetzteDatensicherung = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void EinstellungenButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = VerzeichnisTextBlock.Text;
                dialog.ShowDialog();
                if (dialog.SelectedPath != "") VerzeichnisTextBlock.Text = dialog.SelectedPath;
            }
        }

    }
}
