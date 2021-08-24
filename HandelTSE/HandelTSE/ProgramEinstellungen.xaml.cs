using HandelTSE.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for ProgramEinstellungen.xaml
    /// </summary>
    public partial class ProgramEinstellungen : UserControl
    {
        public static OleDbConnection con = new OleDbConnection();
        List<items> it = new List<items>();
        BrushConverter converter = new System.Windows.Media.BrushConverter();
        Brush brush_red = new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)128, (byte)128));

        public class items
        {
            public string Terminal_ID { get; set; }
            public string Marke { get; set; }
        }
        public ProgramEinstellungen()
        {
            InitializeComponent();

            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

            try
            {
                con.Open();
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Bitte installieren Sie die Microsoft Access Database Engine 2010. Möchten Sie zur Download-Seite weitergeleitet werden?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://www.microsoft.com/en-us/download/confirmation.aspx?id=13255");
                    MessageBox.Show("Nach der Installation des Treibers laden Sie bitte das Menü Personalverwaltung oder den Computer neu, falls erforderlich. ");
                }
                else if (result == MessageBoxResult.No)
                { MessageBox.Show("Sie müssen den Treiber installieren, um die Daten sehen zu können."); }

            }

            OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_ProgramEinstellungenKassennetz] ([Id] COUNTER, [TerminalID] TEXT(55),[MarkeDesTerminals] TEXT(55),[Modellbezeichnung] TEXT(55),[Seriennummer] TEXT(55),[Kassensoftware] TEXT(55),[VersionDerSoftware] TEXT(55), [NetzwerkDatenbank] YESNO)", con);
            try { cmd.ExecuteNonQuery(); }
            catch { }
        }

        private void cp1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cp1.SelectedColor.HasValue)
            {
                Color C = cp1.SelectedColor.Value;
                long colorVal = Convert.ToInt64(C.B * (Math.Pow(256, 0)) + C.G * (Math.Pow(256, 1)) + C.R * (Math.Pow(256, 2)));

                string colorString = cp1.SelectedColor.Value.ToString();

                Farbauswahl1.Style = (Style)FindResource("BlueButton");
                Farbauswahl1.Background = (Brush)converter.ConvertFromString(colorString);
            }
            cp1.Visibility = Visibility.Collapsed;
        }

        private void cp2_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cp2.SelectedColor.HasValue)
            {
                Color C = cp2.SelectedColor.Value;
                long colorVal = Convert.ToInt64(C.B * (Math.Pow(256, 0)) + C.G * (Math.Pow(256, 1)) + C.R * (Math.Pow(256, 2)));

                string colorString = cp2.SelectedColor.Value.ToString();

                Farbauswahl2.Style = (Style)FindResource("BlueButton");
                Farbauswahl2.Background = (Brush)converter.ConvertFromString(colorString);
            }
            cp2.Visibility = Visibility.Collapsed;
        }

        private void Color1Change(object sender, RoutedEventArgs e) { if (cp1.IsOpen == false) cp1.IsOpen = true; else cp1.IsOpen = false; }
        private void Color2Change(object sender, RoutedEventArgs e) { if (cp2.IsOpen == false) cp2.IsOpen = true; else cp2.IsOpen = false; }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void CustomizeHeaders(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e) { if (e.Column.Header.ToString() == "Terminal_ID") e.Column.Header = "Terminal-ID"; }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            int k = 0;
            foreach (TextBox tb in Artikelverwaltung.FindVisualChildren<TextBox>(TerminalIDPanel)) { if (tb.Text == "") { tb.Background = brush_red; k = 1; } }
            if (k == 1) { MessageBox.Show("mit * gekennzeichnete Felder sind Pflichtfelder"); return; };

            Int32 maxID = 0;
            OleDbCommand maxCommand = new OleDbCommand("SELECT max(Id) from TBL_ProgramEinstellungenKassennetz", con);
            try { maxID = (Int32)maxCommand.ExecuteScalar(); }
            catch { }

            int result = 0;
            int NetzwerkDatenbankValue = 0;
            if (NetzwerkDatenbank.IsChecked == true) NetzwerkDatenbankValue = -1; else NetzwerkDatenbankValue = 0;
            OleDbCommand cmd = new OleDbCommand("insert into [TBL_ProgramEinstellungenKassennetz](Id, TerminalID, MarkeDesTerminals, Modellbezeichnung, Seriennummer, Kassensoftware, VersionDerSoftware, NetzwerkDatenbank)Values('"+ ++maxID + "','" + TerminalID.Text + "','" + MarkeDesTermin.Text + "','" + Modellbezeichnung.Text + "','" + Seriennummer.Text + "','" + KassenSoftware.Text + "','" + VersionDerSoftware.Text + "','" + NetzwerkDatenbankValue + "')", con);
            try { result = cmd.ExecuteNonQuery(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
        }

        private void TextChanged(object sender, TextChangedEventArgs e) { if (((TextBox)sender).Background == brush_red) ((TextBox)sender).Background = Brushes.White; }

        private void NeuesTerminal_Click(object sender, RoutedEventArgs e)
        {
            TerminalID.Text = "";
            MarkeDesTermin.Text = "";
            Modellbezeichnung.Text = "";
            Seriennummer.Text = "";
            KassenSoftware.Text = "POSprom Handel Plus TSE";
            VersionDerSoftware.Text = "4.5";
        }
    }
}
