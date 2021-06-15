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
    /// Interaction logic for ArtikelOptionenEinstellungen.xaml
    /// </summary>
    public partial class ArtikelOptionenEinstellungen : UserControl
    {
        public List<items> Data { get; set; }
        public List<items2> Data2 { get; set; }
        List<items> it = new List<items>();
        List<items2> it2 = new List<items2>();
        List<string> artikel = new List<string>();

        public class items
        {
            public string artikel { get; set; }
            public string text { get; set; }
        }

        public class items2 { public string attribute { get; set; } }

        public ArtikelOptionenEinstellungen()
        {
            InitializeComponent();

            if (!File.Exists(@"artikel_option_einstellungen_data.csv")) File.Create(@"artikel_option_einstellungen_data.csv").Close();
            LoadData();
        }

        private void NeuArtikelOption_Click(object sender, RoutedEventArgs e) { ArtikelOption.Text = ""; OptionText.Text = ""; }

        // Deleting the whole option set with its attributes
        private void LoschenArtikelOption_Click(object sender, RoutedEventArgs e)
        {
            LoadDataToArtikelArray();

            int n = artikel.FindIndex(x => x.Contains("[" + ((items)listArtikeln.SelectedItem).artikel + "]"));
            artikel.RemoveAt(n);
            artikel.RemoveAt(n);
            for (int i = n; ; i++) { if (artikel[i].Contains("Artikel,")) artikel.RemoveAt(i--); else break; }
            File.WriteAllLines("artikel_option_einstellungen_data.csv", new[] { String.Join("\n", artikel) });
            it2 = new List<items2>();
            Data2 = it2;
            listArtikelnData.ItemsSource = Data2;

            DeleteOptionSetForEachArtikel();

            LoadData();
        }

        private void DeleteOptionSetForEachArtikel()
        {
            List<string> l = new List<string>();
            string csvData = File.ReadAllText("artikel_optionen_data.csv");
            foreach (string row in csvData.Split('\n')) { if (row != "\n") l.Add(row); }
            for (int i = 0; i < l.Count(); i++)
            {
                if (l[i].Contains("OptionName:" + ((items)listArtikeln.SelectedItem).artikel))
                {
                    int trigger = 0;
                    for (i -= 3; i >= 0 && i < l.Count();)
                    {
                        if (trigger == 1) break;
                        l.RemoveAt(i);
                        if (i < l.Count()) if (l[i].Contains("[") && l[i].Contains("]")) trigger = 1;
                    }
                }
            }

            File.WriteAllLines(@"artikel_optionen_data.csv", new[] { String.Join("\n", l) });
        }

        private void SpeichernArtikelOption_Click(object sender, RoutedEventArgs e)
        {
            if (ArtikelOption.Text == "") { MessageBox.Show("Setzen Sie die Name der Option ein!"); return; }

            File.AppendAllText(@"artikel_option_einstellungen_data.csv", "\n[" + ArtikelOption.Text + "]\n" + "Beschreibung," + OptionText.Text + "\n");
            
            LoadData();
            ArtikelOption.Text = ""; OptionText.Text = "";
        }

        private void NeuOptionAttribute_Click(object sender, RoutedEventArgs e) { OptionAttribute.Text = ""; }

        // Deleting only one attribute inside the option set
        private void LoschenOptionAttribute_Click(object sender, RoutedEventArgs e)
        {
            LoadDataToArtikelArray();

            int n = artikel.FindIndex(x => x.Contains("Artikel," + ((items2)listArtikelnData.SelectedItem).attribute));
            artikel.RemoveAt(n);
            File.WriteAllLines("artikel_option_einstellungen_data.csv", new[] { String.Join("\n", artikel) });
            it2 = new List<items2>();

            DeleteOptionAttributeForSpecificArtikel();

            LoadAttributen();
        }

        private void DeleteOptionAttributeForSpecificArtikel()
        {
            List<string> l = new List<string>();
            int trigger = 0;
            string csvData = File.ReadAllText("artikel_optionen_data.csv");
            foreach (string row in csvData.Split('\n')) { if (row != "\n") l.Add(row); }
            for (int i = 0; i < l.Count(); i++)
            {
                if (l[i].Contains("OptionName:" + ((items)listArtikeln.SelectedItem).artikel) || trigger == 1)
                {
                    if (trigger == 0) { trigger = 1; continue; }
                    if (l[i].Contains("Option," + ((items2)listArtikelnData.SelectedItem).attribute))
                    {
                        l.RemoveAt(i);
                        if (l[i].Contains("Preis,")) l.RemoveAt(i);
                        if ((l[i] == "" || l[i] == "\r" || i == l.Count() - 1 || l[i].Contains("[") && l[i].Contains("]")) && l[i-1].Contains("OptionName:")) for (i -= 5; i >= 0 && i < l.Count();) l.RemoveAt(i);
                        trigger = 0;
                    }
                }
            }

            File.WriteAllLines(@"artikel_optionen_data.csv", new[] { String.Join("\n", l) });
        }

        private void SpeichernOptionAttribute_Click(object sender, RoutedEventArgs e)
        {
            if (listArtikeln.SelectedItem == null) { MessageBox.Show("Bitte wählen Sie die Option"); return; }
            if (OptionAttribute.Text == "") { MessageBox.Show("Setzen Sie die Name der Attribute ein!"); return; }

            LoadDataToArtikelArray();
            int n = artikel.FindIndex(x => x.Contains("[" + ((items)listArtikeln.SelectedItem).artikel + "]"));
            if (!artikel[n + 2].Contains("Artikel,")) artikel.Insert(n + 2, "Artikel," + OptionAttribute.Text);
            else for (int i = n + 3; i < artikel.Count(); i++) if (!artikel[i].Contains("Artikel,")) { artikel.Insert(i, "Artikel," + OptionAttribute.Text); break; }

            File.WriteAllLines("artikel_option_einstellungen_data.csv", new[] { String.Join("\n", artikel) });
            it2 = new List<items2>();
            LoadAttributen();
            OptionAttribute.Text = "";
        }


        private void listArtikelnOptionenLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void listOptionAttributenLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void LoadData()
        {
            it = new List<items>();
            string row_artikel = "";
            int trigger = 0;
            string csvData = File.ReadAllText("artikel_option_einstellungen_data.csv");
            foreach (string row in csvData.Split('\n'))
            {
                if (row.Contains("[") && row.Contains("]"))
                {
                    trigger = 1;
                    row_artikel = row.Substring(row.IndexOf("[") + 1, row.IndexOf("]") - 1);
                    continue;
                }
                if (trigger == 1 && row.Contains("Beschreibung,")) { it.Add(new items { artikel = row_artikel, text = row.Substring(row.IndexOf(",") + 1) }); trigger = 0; }
            }
            Data = it;
            listArtikeln.ItemsSource = Data;
        }

        private void ArtikelOptionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (listArtikeln.SelectedItem == null) it = new List<items>();
            it2 = new List<items2>();

            LoadAttributen();
        }

        private void LoadAttributen()
        {
            var row = (DataGridRow)listArtikeln.ItemContainerGenerator.ContainerFromIndex(listArtikeln.SelectedIndex);
            if (row == null) return;

            int trigger = 0, trigger2 = 0;
            string csvData = File.ReadAllText("artikel_option_einstellungen_data.csv");
            foreach (string r in csvData.Split('\n'))
            {
                if (r.Contains("[" + ((items)row.Item).artikel + "]")) { if (trigger == 0 && trigger2 == 0) trigger = 1; continue; }
                if (r.Contains("[") && r.Contains("]")) { if (trigger2 == 1) break; continue; }
                if (trigger == 1 && r.Contains("Beschreibung,")) { trigger = 0; trigger2 = 1; continue; }
                if (trigger2 == 1 && r.Contains("Artikel,")) { it2.Add(new items2 { attribute = r.Substring(r.IndexOf(",") + 1) }); }
            }
            Data2 = it2;
            listArtikelnData.ItemsSource = Data2;
        }

        // Load all records from DB
        private void LoadDataToArtikelArray()
        {
            artikel = new List<string>();
            string csvData = File.ReadAllText("artikel_option_einstellungen_data.csv");
            foreach (string row in csvData.Split('\n')) { artikel.Add(row); }
        }
    }
}
