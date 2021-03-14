using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HandelTSE.ViewModels//.Artikelverwaltung
{
    /// <summary>
    /// Interaction logic for ArtikelOptionen.xaml
    /// </summary>
    
    public partial class ArtikelOptionen
    {
        public List<items> Data { get; set; }
        List<items> it = new List<items>();
        public List<items> Data2 { get; set; }

        List<string> artikel = new List<string>();
        public class items
        {
            public string option { get; set; }
            public string preis { get; set; }
        }
        public ArtikelOptionen()
        {
            InitializeComponent();

            // FOR DEBUG ONLY
            //***************
            if (HandelTSE.ViewModels.Artikelverwaltung.WG_str == null) HandelTSE.ViewModels.Artikelverwaltung.WG_str = "Test";
            if (HandelTSE.ViewModels.Artikelverwaltung.ArtikelName == null) HandelTSE.ViewModels.Artikelverwaltung.ArtikelName = "test";
            //***************

            LoadOptionenDataToArtikelArray();

        }

        // *************LOADING PART START****************
        private void LoadOptionenToDG(object sender, RoutedEventArgs e)
        {
            OrderColumns(listOption1);
            if (artikel.Count != 0) LoadDataToDG();
        }

        private void LoadDataToDG()
        {
            if (!string.IsNullOrEmpty(artikel[0]))
            {
                string option_name = "", prev_option = "", option_preis = "";
                foreach (string s in artikel)
                {
                    if (s.StartsWith("Option,"))
                    {
                        option_name = s.Substring(s.IndexOf(",") + 1);

                        for (int i = 0; i < listOption1.Items.Count; i++)
                        {
                            DataGridRow dataGridRow = (DataGridRow)listOption1.ItemContainerGenerator.ContainerFromIndex(i);
                            var data = dataGridRow.Item as items;
                            if (data == null) continue;
                            if (option_name == data.option)
                            {
                                foreach (CheckBox c in Artikelverwaltung.FindVisualChildren<CheckBox>(dataGridRow))
                                {
                                    if (c.Name == "theCheckbox")
                                    {
                                        c.IsChecked = true;
                                        prev_option = option_name; break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else if (s.StartsWith("Preis,"))
                    {
                        option_preis = s.Substring(s.IndexOf(",") + 1);
                        for (int i = 0; i < listOption1.Items.Count; i++)
                        {
                            DataGridRow dataGridRow = (DataGridRow)listOption1.ItemContainerGenerator.ContainerFromIndex(i);
                            var data = dataGridRow.Item as items;
                            if (data == null) continue;
                            if (prev_option == data.option)
                            {
                                foreach (CheckBox c in Artikelverwaltung.FindVisualChildren<CheckBox>(dataGridRow))
                                {
                                    if (c.Name == "theCheckbox2") { c.IsChecked = true; DGCell.GetCell(listOption1, dataGridRow, 3).Content = option_preis; break; }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void LoadOptionenDataToArtikelArray()
        {
            string row_artikel = "";
            int trigger = 0, trigger2 = 0;
            string csvData = File.ReadAllText("artikel_optionen_data.csv");
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row) && row.Contains((string)"[" + Artikelverwaltung.WG_str + "]"))
                {
                    trigger = 1;
                    continue;
                }
                if (trigger == 1)
                {
                    row_artikel = row;
                    if (row.Contains("Name:" + Artikelverwaltung.ArtikelName) && row.Substring(row.IndexOf(":") + 1).Count() > 0)
                    {
                        trigger2 = 1;
                        continue;
                    }
                    if (trigger2 == 1)
                    {
                        if (row.Contains('[') && row.Contains(']')) break;
                        if (row == "Option,S" || row == "Option,M" || (row.Contains("Option,") && row.Contains("L"))) Option1CB.SelectedIndex = 2;
                        else if (row.Contains("Option,") && (row.Any(Char.IsDigit))) Option1CB.SelectedIndex = 3;
                        else if (row.Contains("Option,")) Option1CB.SelectedIndex = 1;
                        artikel.Add(row);
                    }
                }
            }
        }
        // *************LOADING PART END****************

        private void CloseButton_Clicked(object sender, RoutedEventArgs e) { Content = new Artikelverwaltung(); }

        private void Option1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            it = new List<items>();
            if (listOption1 == null) return;
            if (listOption1.Visibility == Visibility.Hidden) listOption1.Visibility = Visibility.Visible;

            if (Option1CB.SelectedIndex == 0) 
            {
                ohnePreisAlleCheckBoxLeft.IsEnabled = false;
                addierenAlleCheckBoxLeft.IsEnabled = false;
                listOption1.Visibility = Visibility.Hidden; 
            }
            else
            {
                ohnePreisAlleCheckBoxLeft.IsEnabled = true;
                addierenAlleCheckBoxLeft.IsEnabled = true;
                if (Option1CB.SelectedIndex == 1) // Farbe
                {
                    Farben();
                    Data = it;
                    listOption1.ItemsSource = Data;
                    OrderColumns(listOption1);
                }
                else if (Option1CB.SelectedIndex == 2) // Textil
                {
                    Grossen();
                    Data = it;
                    listOption1.ItemsSource = Data;
                    OrderColumns(listOption1);
                }
                else if (Option1CB.SelectedIndex == 3) // Schuhe
                {
                    for (int i = 35; i <= 47; i++) it.Add(new items { option = i.ToString(), preis = "0.00" });
                    Data = it;
                    listOption1.ItemsSource = Data;
                    OrderColumns(listOption1);
                }
            }
        }

        private void Option2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            it = new List<items>();
            if (listOption2 == null) return;
            if (listOption2.Visibility == Visibility.Hidden) listOption2.Visibility = Visibility.Visible;

            if (Option2CB.SelectedIndex == 0)
            {
                ohnePreisAlleCheckBoxRight.IsEnabled = false;
                addierenAlleCheckBoxRight.IsEnabled = false;
                listOption2.Visibility = Visibility.Hidden;
            }
            else
            {
                ohnePreisAlleCheckBoxRight.IsEnabled = true;
                addierenAlleCheckBoxRight.IsEnabled = true;
                if (Option2CB.SelectedIndex == 1) // Farbe
                {
                    Farben();
                    Data2 = it;
                    listOption2.ItemsSource = Data2;
                    OrderColumns(listOption2);
                }
                else if (Option2CB.SelectedIndex == 2) // Textil
                {
                    Grossen();
                    Data2 = it;
                    listOption2.ItemsSource = Data2;
                    OrderColumns(listOption2);
                }
                else if (Option2CB.SelectedIndex == 3) // Schuhe
                {
                    for (int i = 35; i <= 47; i++) it.Add(new items { option = i.ToString(), preis = "0.00" });
                    Data2 = it;
                    listOption2.ItemsSource = Data2;
                    OrderColumns(listOption2);
                }
            }

        }

        public void Farben()
        {
            it.Add(new items { option = "weiß", preis = "0.00" });
            it.Add(new items { option = "schwarz", preis = "0.00" });
            it.Add(new items { option = "blau", preis = "0.00" });
            it.Add(new items { option = "gelb", preis = "0.00" });
            it.Add(new items { option = "rot", preis = "0.00" });
            it.Add(new items { option = "grau", preis = "0.00" });
            it.Add(new items { option = "grün", preis = "0.00" });
            it.Add(new items { option = "braun", preis = "0.00" });
        }

        public void Grossen()
        {
            it.Add(new items { option = "S", preis = "0.00" });
            it.Add(new items { option = "M", preis = "0.00" });
            it.Add(new items { option = "L", preis = "0.00" });
            it.Add(new items { option = "XL", preis = "0.00" });
            it.Add(new items { option = "XXL", preis = "0.00" });
            it.Add(new items { option = "XXXL", preis = "0.00" });
        }

        private void CustomizeHeaders(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            var grid = (DataGrid)sender;
            if (e.Column.Header.ToString() == "option") e.Column.Header = "Option";
            if (e.Column.Header.ToString() == "preis") e.Column.Header = "Preis(B)";
            foreach (var item in grid.Columns)
            {
                if (item.Header.ToString() == "ohne Pr.") { item.Visibility = Visibility.Visible; }
                if (item.Header.ToString() == "addieren") { item.Visibility = Visibility.Visible; }
            }
        }

        private void OrderColumns(DataGrid grid)
        {
            foreach (var item in grid.Columns)
            {
                if (item.Header.ToString() == "Option") { item.DisplayIndex = 0; }
                if (item.Header.ToString() == "Preis(B)") { item.DisplayIndex = 1; }
            }
        }

        private void CheckedBox_SelectAll(object sender, RoutedEventArgs e) { CheckAllCheckboxesInColumn(sender, e, true); }

        private void CheckedBox_DeselectAll(object sender, RoutedEventArgs e) { CheckAllCheckboxesInColumn(sender, e, false); }

        private void CheckAllCheckboxesInColumn(object sender, RoutedEventArgs e, bool b)
        {
            int i = 1, j = 0;
            DataGrid dataGrid = null;
            if (((CheckBox)sender).Name == "ohnePreisAlleCheckBoxLeft") { dataGrid = listOption1; }
            else if (((CheckBox)sender).Name == "ohnePreisAlleCheckBoxRight") { j = 1; dataGrid = listOption2; }
            else if (((CheckBox)sender).Name == "addierenAlleCheckBoxLeft") { i = 0; j = 2; dataGrid = listOption1; }
            else if (((CheckBox)sender).Name == "addierenAlleCheckBoxRight") { i = 0; j = 3; dataGrid = listOption2; }

            foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(dataGrid))
            {
                if (i % 2 == 1) { ch.IsChecked = b; i++; }
                else { ch.IsChecked = false; i++; }
            }
            if (j == 0) addierenAlleCheckBoxLeft.IsChecked = false;
            else if (j == 1) addierenAlleCheckBoxRight.IsChecked = false;
            else if (j == 2) ohnePreisAlleCheckBoxLeft.IsChecked = false;
            else ohnePreisAlleCheckBoxRight.IsChecked = false;
        }

        void OnChecked1(object sender, RoutedEventArgs e) { OnCheckedOhnePreis(sender, e, listOption1); }
        void OnChecked2(object sender, RoutedEventArgs e) { OnCheckedAddieren(sender, e, listOption1); }
        void OnChecked3(object sender, RoutedEventArgs e) { OnCheckedOhnePreis(sender, e, listOption2); }
        void OnChecked4(object sender, RoutedEventArgs e) { OnCheckedAddieren(sender, e, listOption2); }

        void OnCheckedOhnePreis(object sender, RoutedEventArgs e, DataGrid listOption)
        {
            int i = 0;
            foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(listOption))
            {
                if (i == 1) { ch.IsChecked = false; break; }
                if (((CheckBox)sender) == ch) { i = 1; continue; }
            }
        }

        void OnCheckedAddieren(object sender, RoutedEventArgs e, DataGrid listOption)
        {
            CheckBox c = new CheckBox();
            foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(listOption))
            {
                if (((CheckBox)sender) == ch) { c.IsChecked = false; break; }
                c = ch;
            }
        }

        // *************SAVING PART START****************
        private void SpeichernLeftGrid_Click(object sender, RoutedEventArgs e) { Speichern(sender, e, listOption1); }

        private void SpeichernRightGrid_Click(object sender, RoutedEventArgs e) { Speichern(sender, e, listOption2); }

        private void Speichern(object sender, RoutedEventArgs e, DataGrid dg)
        {
            int i = 1, trigger = 0, trigger2 = 0, trigger3 = 0, t = 0;
            List<string> optionenToSave = new List<string>();
            List<string> optionenToSubstitute = new List<string>();
            optionenToSave.Add("\n\n[" + Artikelverwaltung.WG_str + "]");
            optionenToSave.Add("\nName:" + Artikelverwaltung.ArtikelName);
            
            // Reading data from DG into the List optionenToSave to then add to DB
            foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(dg))
            {
                DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(ch);

                if (ch.IsChecked == true && !String.IsNullOrEmpty(dataGridRow.ToString()))
                {
                    var data = dataGridRow.Item as items;
                    if (data == null) break;
                    optionenToSave.Add(string.Format("{0},{1}", "Option", data.option));
                    if (i % 2 == 0) optionenToSave.Add(string.Format("{0},{1}", "Preis", data.preis));
                    if (t == 0) t = 1;
                }
                i++;
            }

            // Check if any checkboxes are chosen
            if (t == 0) { MessageBox.Show("Bitte geben Sie die Daten ein!"); return; }

            // Reading from DB file into the List optionenToSubstitute
            string csvData = File.ReadAllText("artikel_optionen_data.csv");
            foreach (string row in csvData.Split('\n'))
            {
                if (trigger2 != 1) optionenToSubstitute.Add(row);
                
                // If the record is already present in the DB file then substitute the part with parameters of the given Artikel
                if (row.Contains("[" + Artikelverwaltung.WG_str + "]")) { trigger = 1; continue; }
                if (trigger == 1) 
                    if (row.Contains("Name:" + Artikelverwaltung.ArtikelName))
                    {
                        trigger = 0;
                        trigger2 = 1;
                        continue;
                    }
                if (trigger2 == 1)
                {
                    if (trigger3 == 0) foreach (string s in optionenToSave) { if (s.Contains("[" + Artikelverwaltung.WG_str + "]") || s.Contains("Name:")) continue; optionenToSubstitute.Add(s); }
                    trigger3 = 1;
                }
                if (row.Contains("[") && row.Contains("]") && !row.Contains("[" + Artikelverwaltung.WG_str + "]")) { trigger2 = 0; optionenToSubstitute.Add(row); }
            }
            if (trigger3 == 0)
                foreach (string s in new[] { String.Join("\n", optionenToSave) }) File.AppendAllText(@"artikel_optionen_data.csv", s);
            else
            {
                File.Delete(@"artikel_optionen_data.csv");
                foreach (string s in new[] { String.Join("\n", optionenToSubstitute) }) File.AppendAllText(@"artikel_optionen_data.csv",  s);
            }
        }

        // *************SAVING PART END****************

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int trigger = 0, trigger2 = 0, trigger3 = 0, trigger4 = 0;
            List<string> data = new List<string>();
            string csvData = File.ReadAllText("artikel_optionen_data.csv");
            foreach (string row in csvData.Split('\n'))
            {
                
                if (row.Contains("[" + Artikelverwaltung.WG_str + "]")) { trigger = 1; trigger2 = 1; continue; }
                if (trigger == 1)
                {
                    if (row.Contains("Name:" + Artikelverwaltung.ArtikelName))
                    {
                        trigger2 = 0;
                        trigger3 = 0;
                        trigger4 = 1;
                        continue;
                    }
                    else if (row == "" && trigger2 == 1)
                    {
                        trigger3 = 1;
                        continue;
                    }
                    else if (trigger2 == 1 && trigger3 == 1)
                    {
                        data.Add("[" + Artikelverwaltung.WG_str + "]");
                        data.Add(row);
                        trigger = 0;
                        trigger2 = 0;
                        continue;
                    }
                    else if (row.Contains("[") && row.Contains("]") && !row.Contains("[" + Artikelverwaltung.WG_str + "]")) { data.Add(row); trigger = 0; trigger2 = 0; continue; };
                    if (row.Contains("Name:") && !row.Contains(Artikelverwaltung.ArtikelName)) trigger4 = 0;
                    if (trigger4 == 1) { continue; }
                }
                data.Add(row);
            }
            File.Delete(@"artikel_optionen_data.csv");
            foreach (string s in new[] { String.Join("\n", data) }) File.AppendAllText(@"artikel_optionen_data.csv", s);
        }
    }

    static class DGCell
    {
        public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
