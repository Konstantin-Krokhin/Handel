using System;
using System.Collections.Generic;
using System.Data;
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
        public CSVImportieren()
        {
            InitializeComponent();

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            //Static File From Base Path...........
            //Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            //Dynamic File Using Uploader...........
            Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(Globals.CsvZeitungenFilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
            Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

            string strCellData = "";
            double douCellData;
            int rowCnt = 0;
            int colCnt = 0;

            DataTable dt = new DataTable();
            for (colCnt = 1; colCnt <= 3; colCnt++) { dt.Columns.Add(colCnt.ToString(), typeof(string)); }

                /*string strColumn = "";
                strColumn = (string)(excelRange.Cells[1, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                dt.Columns.Add(strColumn, typeof(string));*/

            for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
            {
                string strData = "";
                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    try
                    {
                        strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                        strData += strCellData + "|";
                    }
                    catch (Exception ex)
                    {
                        douCellData = (excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                        strData += douCellData.ToString() + "|";
                    }
                }
                strData = strData.Remove(strData.Length - 1, 1);
                dt.Rows.Add(strData.Split('|'));
            }

            ZeitungenDataGrid.ItemsSource = dt.DefaultView;

            excelBook.Close(true, null, null);
            excelApp.Quit();
        }

        private void ImportierenButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.CsvZeitungenFilePath = "";
            Content = new PresseUndVMP();
        }

        private void ZuruckButton_Click(object sender, RoutedEventArgs e)
        {
            Content = new PresseUndVMP();
        }

        private void ZuordnenButton_Click(object sender, RoutedEventArgs e) { AlleArtikelCheckbox.Visibility = Visibility.Visible; ZeitungenDataGrid.Columns[0].Visibility = Visibility.Visible; }

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
