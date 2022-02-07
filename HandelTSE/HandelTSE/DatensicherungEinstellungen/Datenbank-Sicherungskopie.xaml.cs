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
            try { ZipFile.CreateFromDirectory(Properties.Settings.Default.Datenbank, VerzeichnisTextBlock.Text); }
            catch { MessageBox.Show("Directory error!"); }
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
