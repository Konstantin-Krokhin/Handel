using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    public partial class Artikelverwaltung
    {
        public List<items> Data { get; set; }
        public TreeViewItem selectedTVI { get; set; }
        List<items> it = new List<items>();
        ItemsControl parent { get; set; }
        public Artikelverwaltung()
        {
            InitializeComponent();
            //for (int i = 0; i < 30; i++) it.Add(new items {  });
            //Data = it;
            if (!File.Exists(@"data.csv"))File.Create(@"data.csv");
        }

        private void LoadForm(object sender, System.EventArgs e)
        {
            if (File.Exists(@"data.csv"))
            {
                string csvData = File.ReadAllText("data.csv");
                string prevNode = "";
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        if (row.Contains("["))
                        {
                            TreeViewItem newChild = new TreeViewItem() { Header = row.Trim('[', ']') };
                            prevNode = (string)newChild.Header;
                            if (CheckGroup(newChild) == 1) { TreeView.Items.Add(newChild); }
                        }
                        else if (row.Contains("Artikel"))
                        {
                            //Look for currently added Warengruppe for this Artikel
                            foreach(TreeViewItem i in TreeView.Items)
                                if (i.Header.ToString() == prevNode) i.Items.Add(new TreeViewItem() { Header = row.TrimStart("Artikel,".ToCharArray()) });
                        }
                    }
                }
            }
        }

        //Checks if TreeView contains newChild item
        public int CheckGroup(TreeViewItem newChild) { foreach (TreeViewItem i in TreeView.Items) if (i.Header.ToString() == newChild.Header.ToString()) return 0; return 1; }

        //Checks if group contains newchild item
        public int CheckGroupItems(TreeViewItem newChild) { foreach (TreeViewItem i in selectedTVI.Items) if (i.Header.ToString() == newChild.Header.ToString()) return 0; return 1; }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T) { yield return (T)child; }
                    foreach (T childOfChild in FindVisualChildren<T>(child)) { yield return childOfChild; }
                }
            }
        }

        public class items
        {
            public string nr { get; set; }
            public string pluean { get; set; }
            public string artikel { get; set; }
            public string preis { get; set; }
            public string mwst { get; set; }
            public string bestand { get; set; }
        }

        //Saves ComboBoxes and TextBoxes into the file and cleans the form
        private void SaveButton(object sender, RoutedEventArgs e)
        {
            if (TreeView.SelectedItem == null) { MessageBox.Show("Bitte Warengruppe anlagen und auswählen!"); return; }
            if (parent.GetType() == typeof(TreeViewItem)) { MessageBox.Show("Sie können keinen Artikel in einem anderen hinzufügen. Es muss eine Warengruppe sein!"); return; }
            if (Artikel.Text == "") { MessageBox.Show("Bitte geben Sie den Artikelname an!"); return; }

            var lines = new List<string>();
            string ArtikelString = Artikel.Text;

            foreach (TextBox cb in Artikelverwaltung.FindVisualChildren<TextBox>(this))
            if (cb.Name != "gruppe" & cb.Name != "artikel")
            {
                if (cb.Name == "Artikel")
                {
                    TreeViewItem newChild = new TreeViewItem() { Header = cb.Text };
                    if (CheckGroupItems(newChild) == 0) { MessageBox.Show("Artikel mit demselben Namen kann nicht in derselben Warengruppe erstellt werden!"); return; }
                }
                // Check if in text fields no special character like '[' for enclosing Warengruppe name in database file is used
                if (cb.Text.Contains("[") || cb.Text.Contains("]")) { MessageBox.Show("Textfelder dürfen keine '[' oder ']' Zeichen enthalten!"); return; }
                lines.Add(string.Format("{0},{1}", cb.Name, cb.Text));
                cb.Text = "";
            }
            foreach (ComboBox cb in Artikelverwaltung.FindVisualChildren<ComboBox>(this))
            {
                lines.Add(string.Format("{0},{1}", cb.Name, cb.Text));
                cb.Text = "";
            }

            //Add Artikel to Warengruppe in TreeView
            selectedTVI.Items.Add(ArtikelString);

            //Save to DB file
            TreeViewItem tvi = TreeView.Tag as TreeViewItem;
            var str = "\n["+ tvi.Header.ToString() + "]\n" ;
            File.AppendAllText(@"data.csv", str);
            foreach (string i in lines) File.AppendAllText(@"data.csv", i + "\n");
        }

        // If TreeView item was selected (warengruppe or artikel)
        private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
        {
            TreeView.Tag = e.OriginalSource;
            selectedTVI = TreeView.Tag as TreeViewItem;
            int trigger = 0, trigger2 = 0, n = 0, nr = 0;
            string[] artikel = new string[21];

            TreeViewItem item = e.OriginalSource as TreeViewItem;
            if (item != null) parent = GetSelectedTreeViewItemParent(item);

            if (parent.GetType() == typeof(TreeView) && File.Exists(@"data.csv"))
            {
                string csvData = File.ReadAllText("data.csv");
                //Data = null;
                foreach (TreeViewItem i in selectedTVI.Items)
                {
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row) && row == (string)"[" + selectedTVI.Header.ToString() + "]")
                        {
                            trigger = 1;
                            continue;
                        }
                        if (trigger == 1)
                        {
                            if (row == "Artikel," + i.Header.ToString())
                            {
                                trigger2 = 1;
                            }
                            if (trigger2 == 1)
                            {
                                if (!row.Contains("ImHausComboBox"))
                                {
                                    artikel[n++] = row;
                                }
                                else
                                {
                                    artikel[n] = row;
                                    trigger = 0;
                                    trigger2 = 0;
                                    n = 0;
                                    break;
                                }
                            }
                        }
                    }//2nd foreach

                    if (!string.IsNullOrEmpty(artikel[0]))
                    {
                        string str_artikel = "", str_pluean = "", str_preis = "", str_mwst = "", str_bestand = "";
                        foreach (string s in artikel)
                        {
                            //MessageBox.Show(s);
                            if (s.Contains("Artikel")) str_artikel = s.TrimStart("Artikel,".ToCharArray());
                            if (s.Contains("PluEan")) str_pluean = s.TrimStart("PluEan,".ToCharArray());
                            if (s.Contains("VKPreisBrutto")) str_preis = s.TrimStart("VKPreisBrutto,".ToCharArray());
                            if (s.Contains("AuserHausComboBox2")) str_mwst = s.TrimStart("AuserHausComboBox2,".ToCharArray());
                            if (s.StartsWith("Bestand")) str_bestand = s.TrimStart("Bestand,".ToCharArray());
                        }
                        it.Add(new items { nr = nr++.ToString(), pluean = str_pluean, artikel = str_artikel, preis = str_preis, mwst = str_mwst, bestand = str_bestand});
                    }
                }//1st foreach
                Data = it;
                dg3.ItemsSource = Data;
                it = new List<items>();
            }
        }

        //Change default names inherited from item class variables to human readable
        private void CustomizeHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "nr") e.Column.Header = "Nr.";
            if (e.Column.Header.ToString() == "pluean") e.Column.Header = "PLU / EAN";
            if (e.Column.Header.ToString() == "preis") e.Column.Header = "Preis";
            if (e.Column.Header.ToString() == "artikel") e.Column.Header = "Artikel";
            if (e.Column.Header.ToString() == "mwst") e.Column.Header = "MwSt.";
            if (e.Column.Header.ToString() == "bestand") e.Column.Header = "Bestand";
        }

        //Check if selected item parent is warengruppe, which means that selected item is artikel
        public ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);

            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as ItemsControl;
        }

        //Clean the form
        private void CleanFormButton(object sender, RoutedEventArgs e)
        {
            IEnumerator<TextBox> a = (IEnumerator<TextBox>)FindVisualChildren<TextBox>(this);
            foreach (TextBox cb in Artikelverwaltung.FindVisualChildren<TextBox>(this)) if (cb.Name != "gruppe" & cb.Name != "artikel") cb.Text = "";
            foreach (ComboBox cb in Artikelverwaltung.FindVisualChildren<ComboBox>(this)) cb.Text = "";
        }

        //Cleans the TextBox for Warengruppe
        private void CleanGroupButton(object sender, RoutedEventArgs e) { gruppe.Text = ""; }

        //Adds the group with the name entered in the textfield to the TreeView
        private void WarenGruppeSave(object sender, RoutedEventArgs e)
        {
            // Checks if Warengruppe field is not empty and does not contain same name as existing group in the TreeView
            if (!String.IsNullOrEmpty(gruppe.Text) && !TreeView.Items.Cast<TreeViewItem>().Any(item => item.Header.ToString() == gruppe.Text))
            {
                TreeViewItem newChild = new TreeViewItem() { Header = gruppe.Text };
                TreeView.Items.Add(newChild);
            }
        }

        //Delete the group from the TreeView
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TreeView.SelectedItem != null) TreeView.Items.RemoveAt(TreeView.Items.IndexOf(TreeView.SelectedItem));
        }
    }
}
