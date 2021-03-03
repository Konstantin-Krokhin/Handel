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
        public class items
        {
            public string option { get; set; }
            public string preis { get; set; }
        }
        public ArtikelOptionen()
        {
            InitializeComponent();
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            Content = new Artikelverwaltung();
        }

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

        void OnChecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(listOption1))
            {
                if (((CheckBox)sender).Name == ch.Name)
                {
                    MessageBox.Show(((CheckBox)sender).Name + ch.Name);
                }
            }
        }

        private void SpeichernLeft_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;
            List<string> optionenToSave = new List<string>();
            optionenToSave.Add("\n[" + Artikelverwaltung.WG_str + "]\n");
            
            foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(listOption1))
            {
                DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(ch);

                if (ch.IsChecked == true && !String.IsNullOrEmpty(dataGridRow.ToString()))
                {
                    var data = dataGridRow.Item as items;
                    optionenToSave.Add(string.Format("{0},{1}", "Option", data.option));
                    if (i % 2 == 1) optionenToSave.Add(string.Format("{0},{1}", "Preis", data.preis));
                    optionenToSave.Add("\n");
                }

            }
            foreach (string s in new[] { String.Join("\n", optionenToSave) }) File.AppendAllText(@"artikel_optionen_data.csv", s);
        }
    }
}
