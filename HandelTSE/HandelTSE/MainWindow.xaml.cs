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

namespace HandelTSE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<items> it = new List<items>();
        List<items> it2 = new List<items>();
        public MainWindow()
        {
            InitializeComponent();

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
        }


        class items
        {
            public string titel { get; set; }
            public string geld { get; set; }
        }

    }
}
