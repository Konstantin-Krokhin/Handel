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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HandelTSE.ViewModels
{
    /// <summary>
    /// Interaction logic for SetArtikel.xaml
    /// </summary>
    /// 
    public partial class SetArtikel : UserControl
    {
        public List<items> Data { get; set; }
        public List<items2> Data2 { get; set; }
        List<items> it = new List<items>();
        List<items2> it2 = new List<items2>();
        public static List<string> WGs = new List<string>();
        public static TreeView TV = new TreeView();
        List<string> artikel = new List<string>();

        public class items
        {
            public string artikel { get; set; }
            public string preis { get; set; }
        }

        public class items2
        {
            public string Artikel { get; set; }
            public string Menge { get; set; }
        }

        public SetArtikel()
        {
            InitializeComponent();
            foreach (string s in WGs) WGComboBox.Items.Add(s);
            
            if (!File.Exists(@"artikel_set_data.csv")) File.Create(@"artikel_set_data.csv").Close();

            LoadSetDataToArtikelArray();
        }

        private void Text_Suchen_Click(object sender, RoutedEventArgs e)
        {
            //eanSuchenTrigger = 1;
            if (SearchBox.Text == "") return;
            string csvData = File.ReadAllText("data.csv");
            List<string> artikel = new List<string>();

            foreach (TreeViewItem t in TV.Items)
            {
                foreach (TreeViewItem i in t.Items)
                {
                    if (i.Header.ToString().ToUpper().Contains(SearchBox.Text.ToUpper()))
                    {
                        it.Add(new items { artikel = i.Header.ToString(), preis = "0" });
                    }
                    artikel = new List<string>();
                }
            }
            Data = it;
            listArtikeln.ItemsSource = Data;
            it = new List<items>();
        }

        private void EAN_Suchen_Click(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "") return;
            string csvData = File.ReadAllText("data.csv");
            int trigger = 0, trigger2 = 0, n = 0;
            List<string> artikel = new List<string>();
            string WG = "", ArtikelName = "";

            foreach (string s in csvData.Split('\n'))
            {
                if (s.Contains("[") && s.Contains("]")) { WG = s; trigger = 1; continue; }
                if (s.Substring(s.IndexOf("Artikel,") + 1).Length > 0 && trigger == 1)
                { ArtikelName = s; artikel.Add(WG); artikel.Add(ArtikelName); trigger = 0; trigger2 = 1; continue; }
                trigger = 0;
                if (trigger2 == 1)
                {
                    artikel.Add(s);
                }
                if (s.Contains("ImHausComboBox2"))
                {
                    trigger2 = 0;
                    foreach (string i in artikel) if (i.Contains("PluEan,") && i.Substring(i.IndexOf(",") + 1).ToUpper().Contains(SearchBox.Text.ToUpper())) trigger2 = 1;
                    if (trigger2 == 1)
                    {
                        if (artikel.Count <= 0) return;
                        if (!string.IsNullOrEmpty(artikel[0]))
                        {
                            string str_artikel = "";
                            foreach (string t in artikel)
                            {
                                if (t.StartsWith("Artikel,")) str_artikel = t.Substring(t.IndexOf(",") + 1);
                            }
                            it.Add(new items { artikel = str_artikel, preis = "0" });
                        }
                        trigger2 = 0;
                    }
                    artikel = new List<string>();
                }
            }
            Data = it;
            listArtikeln.ItemsSource = Data;
            it = new List<items>();
        }

        private void WGComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string WGName = WGComboBox.SelectedItem.ToString();
            foreach (TreeViewItem i in TV.Items)
                if (i.Header.ToString() == WGName)
                {
                    foreach (TreeViewItem j in i.Items) { 
                        it.Add(new items { artikel = j.Header.ToString(), preis = "0.0" });
                    }
                    Data = it;
                    listArtikeln.ItemsSource = Data;
                    it = new List<items>();
                }
        }

        private void listArtikelnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void SetDelete_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Wollen Sie wirklich ein Set löschen?";
            string caption = "Set-Artikel löschen";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (result)
            {
                case MessageBoxResult.OK:
                    artikel = new List<string>();
                    UpdateDG(sender, e);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void BackToArticle_Click(object sender, RoutedEventArgs e) { Content = new Artikelverwaltung(); }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e) { Content = new Artikelverwaltung(); }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (listArtikeln.SelectedItem == null) { MessageBox.Show("Bitte wählen Sie den Artikel"); return; }
            LoadSetDataToArtikelArray();

            artikel.Add("Artikel,"+((items)listArtikeln.SelectedItem).artikel);
            artikel.Add("Menge,1");

            UpdateDG(sender, e);

            //Save_Click(sender, e);
        }

        private void UpdateDG(object sender, RoutedEventArgs e)
        {
            it2 = new List<items2>();
            Data2 = null;
            LoadDataToDG(sender, e);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (listArtikelnData.SelectedItem == null) { MessageBox.Show("Bitte wählen Sie den Artikel"); return; }
            int n = artikel.FindIndex(x => x.Contains("Artikel," + ((items2)listArtikelnData.SelectedItem).Artikel));
            artikel.RemoveAt(n);
            artikel.RemoveAt(n++);
            UpdateDG(sender, e);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            int trigger = 0, trigger2 = 0, trigger3 = 0;
            List<string> optionenToSave = new List<string>();
            List<string> optionenToSubstitute = new List<string>();
            optionenToSave.Add("\n\n[" + Artikelverwaltung.WG_str + "]");
            optionenToSave.Add("\nName:" + Artikelverwaltung.ArtikelName);

            // Reading data from DG into the List optionenToSave to then add to DB
            foreach (DataGridRow dataGridRow in Artikelverwaltung.FindVisualChildren<DataGridRow>(listArtikelnData))
            {
                //DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(ch);

                if (!String.IsNullOrEmpty(dataGridRow.ToString()))
                {
                    var data = dataGridRow.Item as items2;
                    if (data == null) break;
                    optionenToSave.Add(string.Format("{0},{1}", "Artikel", data.Artikel));
                    optionenToSave.Add(string.Format("{0},{1}", "Menge", data.Menge));
                }
            }

            // Reading from DB file into the List optionenToSubstitute
            string csvData = File.ReadAllText("artikel_set_data.csv");
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
                foreach (string s in new[] { String.Join("\n", optionenToSave) }) File.AppendAllText(@"artikel_set_data.csv", s);
            else
            {
                File.Delete(@"artikel_set_data.csv");
                foreach (string s in new[] { String.Join("\n", optionenToSubstitute) }) File.AppendAllText(@"artikel_set_data.csv", s);
            }
        }

        private void LoadDataToDG(object sender, RoutedEventArgs e)
        {
            if (artikel != null)
            {
                string option_name = "", option_menge = "";
                foreach (string s in artikel)
                {
                    if (s.StartsWith("Artikel,"))
                    {
                        option_name = s.Substring(s.IndexOf(",") + 1);
                    }
                    else if (s.StartsWith("Menge,"))
                    {
                        option_menge = s.Substring(s.IndexOf(",") + 1);
                        it2.Add(new items2 { Artikel = option_name, Menge = option_menge });
                    }
                }
                Data2 = it2;
                listArtikelnData.ItemsSource = Data2;
            }

        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Menge") { e.Column.IsReadOnly = false; }
            if (e.PropertyName == "Artikel") { e.Column.IsReadOnly = true; }
        }

        private void LoadSetDataToArtikelArray()
        {
            artikel = new List<string>();
            string row_artikel = "";
            int trigger = 0, trigger2 = 0;
            string csvData = File.ReadAllText("artikel_set_data.csv");
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
                        /*if (row == "Option,S" || row == "Option,M" || (row.Contains("Option,") && row.Contains("L"))) Option1CB.SelectedIndex = 2;
                        else if (row.Contains("Artikel,") && (row.Any(Char.IsDigit))) Option1CB.SelectedIndex = 3;
                        else if (row.Contains("Menge,")) Option1CB.SelectedIndex = 1;*/
                        if (row != "") artikel.Add(row);
                    }
                }
            }
        }
    }
}
