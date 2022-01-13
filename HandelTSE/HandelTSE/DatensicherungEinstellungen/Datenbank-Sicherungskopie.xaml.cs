using System;
using System.Collections.Generic;
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
    }
}
