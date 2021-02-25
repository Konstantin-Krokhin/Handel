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
            public CheckBox ohne_preis { get; set; }
            public CheckBox addieren { get; set; }
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

            if (Option1CB.SelectedIndex == 0) { listOption1.Visibility = Visibility.Hidden;} 
            else if (Option1CB.SelectedIndex == 1)
            {
                Farben();
                Data = it;
                listOption1.ItemsSource = Data;
            } else if (Option1CB.SelectedIndex == 2)
            {
                Grossen();
                Data = it;
                listOption1.ItemsSource = Data;
            } else if (Option1CB.SelectedIndex == 3)
            {
                for (int i = 35; i <= 47; i++) it.Add(new items { option = i.ToString(), preis = "0.00" });
                Data = it;
                listOption1.ItemsSource = Data;
            }
        }

        private void Option2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            it = new List<items>();
            if (listOption2 == null) return;
            if (listOption2.Visibility == Visibility.Hidden) listOption2.Visibility = Visibility.Visible;

            if (Option2CB.SelectedIndex == 0) { listOption2.Visibility = Visibility.Hidden; }
            else if (Option2CB.SelectedIndex == 1)
            {
                Farben();
                Data2 = it;
                listOption2.ItemsSource = Data2;
            }
            else if (Option2CB.SelectedIndex == 2)
            {
                Grossen();
                Data2 = it;
                listOption2.ItemsSource = Data2;
            }
            else if (Option2CB.SelectedIndex == 3)
            {
                for (int i = 35; i <= 47; i++) it.Add(new items { option = i.ToString(), preis = "0.00" });
                Data2 = it;
                listOption2.ItemsSource = Data2;
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
            if (e.Column.Header.ToString() == "option") e.Column.Header = "Option";
            if (e.Column.Header.ToString() == "preis") e.Column.Header = "Preis(B)";
            if (e.Column.Header.ToString() == "ohne_preis") e.Column.Header = "ohne Pr.";
        }
    }
}
