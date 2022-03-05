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
        }

        private void ProgrammBeenden_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void SicherungButton_Click(object sender, RoutedEventArgs e)
        {
            string destPath = VerzeichnisTextBlock.Text + "\\db_source_" + DateTime.Now.ToString("dd.MM.yyyy") + ".zip";
            
            string copyDestPath = VerzeichnisTextBlock.Text + "\\db_handel.db";
            string dbPath = Properties.Settings.Default.Datenbank;//.Replace(@"\db_handel.db", @"\db_handel.db");
            //string copyDestPath2 = copyDestPath.Replace(@"\\", @"\");
            //string curFile = System.IO.Path.GetFileName(dbPath);
            //string newDir = dbPath.Replace(@"\db_handel.db", @"\") + "copy\\";
            //string newPathToFile = System.IO.Path.Combine(copyDestPath, curFile);

            if (File.Exists(dbPath) && dbPath.ToLower() != copyDestPath.ToLower())
            {
                File.Copy(dbPath, copyDestPath, true);
            }
            using (FileStream fs = new FileStream(destPath, FileMode.Create))
            using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                arch.CreateEntryFromFile(copyDestPath, "db_handel.db");
            }

            /*using (FileStream fs = new FileStream(destPath, FileMode.Create))
            using (ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                arch.CreateEntryFromFile(copyDestPath, "db_handel.db");
            }*/

            ////// -------------------

            /*var permissionSet = new PermissionSet(PermissionState.None);
            var writePermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, VerzeichnisTextBlock.Text);
            permissionSet.AddPermission(writePermission);

            if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
            {
                

                //try { ZipFile.CreateFromDirectory(Properties.Settings.Default.Datenbank, destPath); }
                //catch { MessageBox.Show("Directory error!"); }
            }
            else
            {
                // alternative stuff
            }*/

        }

        private void EinstellungenButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                if (dialog.SelectedPath != "") VerzeichnisTextBlock.Text = dialog.SelectedPath;
            }
        }
    }
}
