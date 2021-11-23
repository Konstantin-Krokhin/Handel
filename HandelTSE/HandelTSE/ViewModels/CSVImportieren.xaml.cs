using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Microsoft;

namespace HandelTSE.ViewModels
{
    /// <summary>
    /// Interaction logic for CSVImportieren.xaml
    /// </summary>
    public partial class CSVImportieren : UserControl
    {
        public static OleDbConnection con = MainWindow.con;
        List<Presse> list = new List<Presse>();
        public List<Presse> Data { get; set; }
        int KopfzeileAdded_flag = 0;

        public class Presse
        {
            public string CEAN { get; set; }
            public string CNAME { get; set; }
        }

        public CSVImportieren()
        {
            InitializeComponent();

            ZeilenTitle.Text = Globals.presseList.Count + " Zeilen";
            ZeilenTitle.Visibility = Visibility.Visible;

            SpaltenTitle.Text = 2 + " Spalten";
            SpaltenTitle.Visibility = Visibility.Visible;

            Data = Globals.presseList;
            ZeitungenDataGrid.ItemsSource = Data;
            Globals.presseList = new List<Presse>();
        }

        private void ImportierenButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.CsvZeitungenFilePath = "";
            OleDbCommand cmd = new OleDbCommand();
            Int32 ID = 0;
            int chosen = 0;

            foreach (CheckBox x in Artikelverwaltung.FindVisualChildren<CheckBox>(ZeitungenDataGrid))
            {
                if (x.IsChecked == true)
                {
                    if (chosen == 0)
                    {
                        chosen = 1;
                        OleDbCommand maxCommand = new OleDbCommand("SELECT max(Id) from TBL_PRESSE", con);
                        try { ID = (Int32)maxCommand.ExecuteScalar(); } catch { }
                    }
                    DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(x);

                    var data = dataGridRow.Item as Presse;
                    if (data == null) break;

                    if (ZeitungenDataGrid.Columns[1].Header.ToString() == "EAN" && ZeitungenDataGrid.Columns[2].Header.ToString() == "Bezeichnung")
                        cmd = new OleDbCommand("insert into [TBL_PRESSE](Id, CEAN, CNAME)Values('" + ++ID + "','" + data.CEAN + "','" + data.CNAME + "')", con);

                    else if (ZeitungenDataGrid.Columns[1].Header.ToString() == "Bezeichnung" && ZeitungenDataGrid.Columns[2].Header.ToString() == "EAN")
                        cmd = new OleDbCommand("insert into [TBL_PRESSE](Id, CEAN, CNAME)Values('" + ++ID + "','" + data.CNAME + "','" + data.CEAN + "')", con);

                    try { cmd.ExecuteNonQuery(); }
                    catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); break; }
                }
            }

            Content = new PresseUndVMP();
        }

        private void CustomizeHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "CEAN") e.Column.Header = "1";
            if (e.Column.Header.ToString() == "CNAME") e.Column.Header = "2";
        }

        private void ZuruckButton_Click(object sender, RoutedEventArgs e) { Content = new PresseUndVMP(); }

        private void ZuordnenButton_Click(object sender, RoutedEventArgs e)
        {
            if (EANCB.Text == "1" || EANCB.Text == ".....") { ZeitungenDataGrid.Columns[1].Header = "EAN"; ZeitungenDataGrid.Columns[2].Header = "Bezeichnung"; }
            else if (EANCB.Text == "2") { ZeitungenDataGrid.Columns[1].Header = "Bezeichnung"; ZeitungenDataGrid.Columns[2].Header = "EAN"; }

            AlleArtikelCheckbox.Visibility = Visibility.Visible; 
            ZeitungenDataGrid.Columns[0].Visibility = Visibility.Visible;
            if (KopfzeileCheckbox.IsChecked == false && KopfzeileAdded_flag == 0)
            {
                list.AddRange(Data);
                list.Insert(0, Globals.KopfzeilItem);
                Data = list;
                ZeitungenDataGrid.ItemsSource = Data;
                list = new List<Presse>();
                KopfzeileAdded_flag = 1;
            }
            else if (KopfzeileCheckbox.IsChecked == true && KopfzeileAdded_flag == 1)
            {
                list.AddRange(Data);
                list.Remove(Globals.KopfzeilItem);
                Data = list;
                ZeitungenDataGrid.ItemsSource = Data;
                list = new List<Presse>();
                KopfzeileAdded_flag = 0;
            }
            importierenButton.IsEnabled = true;
        }

        private void AbbrechenButton_Click(object sender, RoutedEventArgs e) { AlleArtikelCheckbox.Visibility = Visibility.Collapsed; ZeitungenDataGrid.Columns[0].Visibility = Visibility.Collapsed; if (AlleArtikelCheckbox.IsChecked == true) AlleArtikelCheckbox.IsChecked = false; }

        private void UncheckedBox(object sender, RoutedEventArgs e)
        {

        }

        private void CheckedBox(object sender, RoutedEventArgs e)
        {

        }

        private void AlleArtikelCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox x in Artikelverwaltung.FindVisualChildren<CheckBox>(ZeitungenDataGrid)) { x.IsChecked = true; }
        }

        private void AlleArtikelCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox x in Artikelverwaltung.FindVisualChildren<CheckBox>(ZeitungenDataGrid)) { x.IsChecked = false; }
        }
    }
}
