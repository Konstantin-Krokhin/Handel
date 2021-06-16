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

namespace HandelTSE.ViewModels
{
    /// <summary>
    /// Interaction logic for ArtikelOptionen.xaml
    /// </summary>
    
    public partial class ArtikelOptionen
    {
        public List<items> Data { get; set; }
        List<items> it = new List<items>();
        public List<items> Data2 { get; set; }
        List<string> optionen = new List<string>();

        List<string> artikel = new List<string>();
        public class items
        {
            public string option { get; set; }
            public string preis { get; set; }
        }
        public ArtikelOptionen()
        {
            InitializeComponent();

            if (!File.Exists(@"artikel_optionen_data.csv")) File.Create(@"artikel_optionen_data.csv").Close();
            if (!File.Exists(@"artikel_option_einstellungen_data.csv")) File.Create(@"artikel_option_einstellungen_data.csv").Close();

            LoadDataToArtikelArray();
            FillListOption1WithValues();
            LoadOptionenDataToArtikelArray();

            // FOR DEBUG ONLY
            //***************
            //if (HandelTSE.ViewModels.Artikelverwaltung.WG_str == null) HandelTSE.ViewModels.Artikelverwaltung.WG_str = "Test";
            //if (HandelTSE.ViewModels.Artikelverwaltung.ArtikelName == null) HandelTSE.ViewModels.Artikelverwaltung.ArtikelName = "test";
            //***************

        }

        // *************LOADING PART START****************
        private void LoadOptionenToDG(object sender, RoutedEventArgs e)
        {
            OrderColumns(listOption1);
            if (artikel.Count != 0) LoadDataToDG();
        }

        private void LoadDataToDG()
        {
            items data = null;
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
                            if (dataGridRow != null) data = dataGridRow.Item as items;
                            else continue;
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
                            if (dataGridRow != null) data = dataGridRow.Item as items;
                            else continue;
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

        // Add options to array to print on DG and choose appropriate option for selected Artikel on ComboBox
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
                        if (row.Contains("OptionName:")) 
                        { 
                            Option1CB.SelectedValue = row.Substring(row.IndexOf(":") + 1);
                            continue; 
                        }
                        artikel.Add(row);
                    }
                }
            }
        }

        // Load and add option names (template names) available for all articles to ComboBox
        private void LoadDataToArtikelArray()
        {
            optionen = new List<string>();
            string csvData = File.ReadAllText("artikel_option_einstellungen_data.csv");
            foreach (string row in csvData.Split('\n')) { optionen.Add(row); }

            foreach (string s in optionen)
            {
                if (s.Contains("[") && s.Contains("]"))
                {
                    Option1CB.Items.Add(s.Substring(s.IndexOf("[") + 1, s.IndexOf("]") - 1));
                    Option2CB.Items.Add(s.Substring(s.IndexOf("[") + 1, s.IndexOf("]") - 1));
                }
            }
        }

        // *************LOADING PART END****************

        private void CloseButton_Clicked(object sender, RoutedEventArgs e) { Content = new Artikelverwaltung(); }

        private void Option1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listOption1 == null || Option1CB.SelectedValue == null) return;
            it = new List<items>();
            ohnePreisAlleCheckBoxLeft.IsChecked = false;
            addierenAlleCheckBoxLeft.IsChecked = false;

            if (Option1CB.SelectedIndex == 0) 
            {
                ohnePreisAlleCheckBoxLeft.IsEnabled = false;
                addierenAlleCheckBoxLeft.IsEnabled = false;
                it = new List<items>();
                Data = it;
                listOption1.ItemsSource = Data;
                listOption1.HeadersVisibility = DataGridHeadersVisibility.None;
            }
            else
            {
                listOption1.HeadersVisibility = DataGridHeadersVisibility.All;
                ohnePreisAlleCheckBoxLeft.IsEnabled = true;
                addierenAlleCheckBoxLeft.IsEnabled = true;
                FillListOption1WithValues();
            }
        }

        private void FillListOption1WithValues()
        {
            int trigger = 0, trigger2 = 0;
            foreach (string s in optionen)
            {
                if (s.Contains("[" + Option1CB.SelectedValue.ToString() + "]"))
                {
                    trigger = 1;
                    continue;
                }
                if (trigger == 1 && s.Contains("Beschreibung,"))
                {
                    trigger2 = 1;
                    continue;
                }
                if (trigger == 1 && trigger2 == 1 && s.Contains("Artikel,"))
                {
                    it.Add(new items { option = s.Substring(s.IndexOf(",") + 1).ToString(), preis = "0.00" });
                }
                else if (it.Count >= 1) break;
            }
            Data = it;
            listOption1.ItemsSource = Data;
            OrderColumns(listOption1);
        }

        private void Option2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            it = new List<items>();
            if (listOption2 == null) return;
            ohnePreisAlleCheckBoxRight.IsChecked = false;
            addierenAlleCheckBoxRight.IsChecked = false;

            if (Option2CB.SelectedIndex == 0)
            {
                ohnePreisAlleCheckBoxRight.IsEnabled = false;
                addierenAlleCheckBoxRight.IsEnabled = false;
                it = new List<items>();
                Data2 = it;
                listOption2.ItemsSource = Data2;
                listOption2.HeadersVisibility = DataGridHeadersVisibility.None;
            }
            else
            {
                listOption2.HeadersVisibility = DataGridHeadersVisibility.All;
                ohnePreisAlleCheckBoxRight.IsEnabled = true;
                addierenAlleCheckBoxRight.IsEnabled = true;

                int trigger = 0, trigger2 = 0;
                foreach (string s in optionen)
                {
                    if (s.Contains("[" + Option2CB.SelectedItem.ToString() + "]"))
                    {
                        trigger = 1;
                        continue;
                    }
                    if (trigger == 1 && s.Contains("Beschreibung,"))
                    {
                        trigger2 = 1;
                        continue;
                    }
                    if (trigger == 1 && trigger2 == 1 && s.Contains("Artikel,"))
                    {
                        it.Add(new items { option = s.Substring(s.IndexOf(",") + 1).ToString(), preis = "0.00" });
                    }
                    else if (it.Count >= 1) break;
                }
                Data2 = it;
                listOption2.ItemsSource = Data2;
                OrderColumns(listOption2);
            }

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

        // *************SAVING PART START*****************
        private void SpeichernLeftGrid_Click(object sender, RoutedEventArgs e) { Speichern(sender, e, listOption1); }

        private void SpeichernRightGrid_Click(object sender, RoutedEventArgs e) { Speichern(sender, e, listOption2); }

        private void Speichern(object sender, RoutedEventArgs e, DataGrid dg)
        {
            int i = 1, trigger1 = 0, trigger = 0, trigger2 = 0, trigger3 = 0, t = 0;
            List<string> optionenToSave = new List<string>();
            List<string> optionenToSubstitute = new List<string>();
            optionenToSave.Add("\n\n[" + Artikelverwaltung.WG_str + "]");
            optionenToSave.Add("\nName:" + Artikelverwaltung.ArtikelName);
            if (dg.Name == "listOption1") optionenToSave.Add("OptionName:" + Option1CB.Text);
            else if (dg.Name == "listOption2") optionenToSave.Add("OptionName:" + Option2CB.Text);

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
                    if (trigger == 0 && row.Contains("OptionName:")) { optionenToSubstitute.Add("OptionName:" + Option1CB.Text); continue; }
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
            List <string> options = new List<string>();
            string csvData = File.ReadAllText("artikel_optionen_data.csv");
            foreach (string row in csvData.Split('\n')) { options.Add(row); }
            List<string> optionsEdited = new List<string>(options);

            int trigger = 0, trigger2 = 0, n = 0;
            foreach (string s in options)
            {
                if (s.Contains('[' + Artikelverwaltung.WG_str + ']')) { trigger = 1; n = optionsEdited.IndexOf(s); continue; }
                if (trigger == 1 && s.Contains("Name:" + Artikelverwaltung.ArtikelName)) { trigger2 = 1; optionsEdited.RemoveAt(n); optionsEdited.RemoveAt(n); continue; }
                if (trigger2 == 1 && !(s.Contains("[") && s.Contains("]"))) { optionsEdited.RemoveAt(n); continue; }
                if (trigger2 == 1 && ((s.Contains("[") && s.Contains("]")) || s == "")) { optionsEdited.RemoveAt(n); break; }
            }

            File.WriteAllLines(@"artikel_optionen_data.csv", new[] { String.Join("\n", optionsEdited) });
            Option1CB.SelectedIndex = 0;
            Option2CB.SelectedIndex = 0;
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
