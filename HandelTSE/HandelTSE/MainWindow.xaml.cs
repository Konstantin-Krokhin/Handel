using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using HandelTSE.ViewModels;

namespace HandelTSE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<items> it = new List<items>();
        List<items> it2 = new List<items>();
        //List<items> ArtikelData = new List<items>();
        public MainWindow()
        {
            InitializeComponent();

            // FOR DEV Purposes ONLY********************
            ContentWindow.SetValue(Grid.RowProperty, 1);
            ContentWindow.SetValue(Grid.ColumnProperty, 0);
            ContentWindow.SetValue(Grid.ColumnSpanProperty, 7);
            ContentWindow.SetValue(Grid.RowSpanProperty, 5);
            DataContext = new HandelTSE.ViewModels.Artikelverwaltung();
            //*******************************************

            // FOR SHOWING MAIN WINDOW FIRST (COMMENT IN ORDER to WORK ON other pages for convenience)
            /*
            dateBlock.Text = "Heute ist der " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            it.Add(new items { titel = "Umsatz gesamt:", geld = "0 EUR" });
            it.Add(new items { titel = "Gesamt ohne MwSt.:", geld = "0 EUR" });
            it.Add(new items { titel = "MwSt. ingesamt:", geld = "0 EUR" });
            for (int i=0; i < 30; i++) it.Add(new items { titel = "", geld = "" });

            dg1.ItemsSource = it;

            it2.Add(new items { titel = "Gesamt Brutto:", geld = "0 EUR" });
            it2.Add(new items { titel = "Gesamt Netto:", geld = "0 EUR" });
            it2.Add(new items { titel = "davon MwSt.:", geld = "0 EUR" });
            for (int i = 0; i < 30; i++) it2.Add(new items { titel = "", geld = "" });

            dg2.ItemsSource = it2;
            */
        }


        class items
        {
            public string titel { get; set; }
            public string geld { get; set; }
        }

        private void StatTable_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new StatTableModel();
        }

        private void StatGraph_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new StatisticsGraphViewModel();
        }

        private void TransParking_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new TransactionParkingViewModel();
        }

        private void MwStTool_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new MwStTool();
        }

        private void MainWindow_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new Main();
        }

        private void Artikelverwaltung_Click(object sender, RoutedEventArgs e)
        {
            ContentWindow.SetValue(Grid.RowProperty, 1);
            ContentWindow.SetValue(Grid.ColumnProperty, 0);
            ContentWindow.SetValue(Grid.ColumnSpanProperty, 7);
            ContentWindow.SetValue(Grid.RowSpanProperty, 5);
            DataContext = new HandelTSE.ViewModels.Artikelverwaltung();
        }

        private void Kasse_Click(object sender, RoutedEventArgs e)
        {
            Kasse kasse = new Kasse();
            kasse.Show();
        }

        private void EanEinstellungen_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new EanEinstellungen();
        }
    }
}
