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
    /// Interaction logic for Kasse.xaml
    /// </summary>
    public partial class Kasse
    {
        public Kasse()
        {
            InitializeComponent();
            if (!File.Exists(@"data.csv")) File.Create(@"data.csv").Close();
        }

        //Load Warengruppen and Artikeln from DB on the Button names in the dgWG Grid
        private void LoadForm(object sender, System.EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            List<Button> buttons = Artikelverwaltung.FindVisualChildren<Button>(dgWG).ToList();

            //Wierd combination for outputing empty buttons 
            Button temp = new Button() { Content = "".Trim() };
            for (int i = 0; i < 40; i++) buttons[i].Content = temp.Content;

            if (File.Exists(@"data.csv"))
            {
                int x = 0;
                string csvData = File.ReadAllText("data.csv");
                string prevNode = "", row_artikel = "";
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        row_artikel = row;
                        if (row.Contains("["))
                        {
                            Button newChild = new Button() { Content = row.Trim('[', ']') };
                            //prevNode = (string)newChild.Content;
                            if (CheckGroup(newChild) == 1) { if (x < 40) buttons[x++].Content = newChild.Content; }
                        }
                    }
                }
            }
            //for(int i = 0; i < 40; i++) if (string.IsNullOrEmpty(buttons[i].Content.ToString())) buttons[i].IsEnabled = false;
        }

        //Checks if Buttons contain newChild item
        public int CheckGroup(Button newChild) { foreach (Button i in Artikelverwaltung.FindVisualChildren<Button>(dgWG)) if (i.Content.ToString() == newChild.Content.ToString()) return 0; return 1; }

        // Button "beenden" Closes the window
        private void Close(object sender, RoutedEventArgs e) { this.Close(); }

        /*
        else if (row.Contains("Artikel,") && !string.IsNullOrEmpty(row.Substring(row.IndexOf(",") + 1)))
                        {
                            //Look for currently added Warengruppe for this Artikel
                            foreach (TreeViewItem i in TreeView.Items)
                                if (i.Header.ToString() == prevNode) { i.Items.Add(new TreeViewItem() { Header = row.Substring(row.IndexOf(",") + 1) }); }
                        }*/
    }
}
