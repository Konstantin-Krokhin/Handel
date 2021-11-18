using System;
using System.Collections.Generic;
using System.Data;
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
        Presse KopfzeilItem = new Presse();
        int KopfzeileAdded_flag = 0;

        public class Presse
        {
            public string CEAN { get; set; }
            public string CNAME { get; set; }
        }
        public CSVImportieren()
        {
            InitializeComponent();

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(Globals.CsvZeitungenFilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
            Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

            string strCellData = "";
            int rowCnt = 0;
            int colCnt = 0;

            DataTable dt = new DataTable();

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

                if (rowCnt == 1) { KopfzeilItem = new Presse { CEAN = cean, CNAME = cname }; continue; }
                list.Add(new Presse { CEAN = cean, CNAME = cname});
            }
            ZeilenTitle.Text = list.Count + " Zeilen";
            ZeilenTitle.Visibility = Visibility.Visible;

            SpaltenTitle.Text = 2 + " Spalten";
            SpaltenTitle.Visibility = Visibility.Visible;

            Data = list;
            ZeitungenDataGrid.ItemsSource = Data;
            list = new List<Presse>();

            excelBook.Close(true, null, null);
            excelApp.Quit();
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
                    catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
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
                list.Insert(0, KopfzeilItem);
                Data = list;
                ZeitungenDataGrid.ItemsSource = Data;
                list = new List<Presse>();
                KopfzeileAdded_flag = 1;
            }
            else if (KopfzeileCheckbox.IsChecked == true && KopfzeileAdded_flag == 1)
            {
                list.AddRange(Data);
                list.Remove(KopfzeilItem);
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
