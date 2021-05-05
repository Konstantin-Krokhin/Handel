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
        List<items> it = new List<items>();
        public static List<string> WGs = new List<string>();
        public static TreeView TV = new TreeView();
        int n_size = 20;

        public class items
        {
            public string artikel { get; set; }
            public string preis { get; set; }
        }

        public SetArtikel()
        {
            InitializeComponent();
            foreach (string s in WGs) WGComboBox.Items.Add(s);
        }

        private void Text_Suchen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EAN_Suchen_Click(object sender, RoutedEventArgs e)
        {

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

        }

        private void BackToArticle_Click(object sender, RoutedEventArgs e) { Content = new Artikelverwaltung(); }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e) { Content = new Artikelverwaltung(); }
    }
}
