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
        public List<items2> Data2 { get; set; }
        public TreeViewItem selectedTVI { get; set; }
        List<items> it = new List<items>();
        List<items2> it2 = new List<items2>();
        ItemsControl parent { get; set; }
        public Artikelverwaltung()
        {
            InitializeComponent();
            if (!File.Exists(@"data.csv")) File.Create(@"data.csv").Close();
        }

        //Load Warengruppen and Artikeln from DB into the TreeView
        private void LoadForm(object sender, System.EventArgs e)
        {
            if (File.Exists(@"data.csv"))
            {
                string csvData = File.ReadAllText("data.csv");
                string prevNode = "", row_artikel = "";
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        row_artikel = row;
                        if (row.Contains("["))
                        {
                            TreeViewItem newChild = new TreeViewItem() { Header = row.Trim('[', ']') };
                            prevNode = (string)newChild.Header;
                            if (CheckGroup(newChild) == 1) { TreeView.Items.Add(newChild); }
                        }
                        else if (row.Contains("Artikel,") && !(row_artikel.TrimStart("Artikel,".ToCharArray()) == ""))
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

        //DataGrid columns
        public class items
        {
            public string nr { get; set; }
            public string pluean { get; set; }
            public string artikel { get; set; }
            public string preis { get; set; }
            public string mwst { get; set; }
            public string bestand { get; set; }
        }

        public class items2
        {
            public string warengruppe { get; set; }
            public string pluean { get; set; }
            public string artikel { get; set; }
            public string preis { get; set; }
            public string mwst { get; set; }
            public string bestand { get; set; }
        }

        //Saves WG with articles or empty to the DB
        public void SaveWGToDB()
        {
            var lines = new List<string>();
            foreach (TextBox cb in Artikelverwaltung.FindVisualChildren<TextBox>(this))
                if (cb.Name != "gruppe" & cb.Name != "SearchBoxArtikel")
                {
                    if (cb.Name == "Artikel")
                    {
                        TreeViewItem newChild = new TreeViewItem() { Header = cb.Text };
                        if (!string.IsNullOrEmpty(cb.Text)) if (CheckGroupItems(newChild) == 0) { MessageBox.Show("Artikel mit demselben Namen kann nicht in derselben Warengruppe erstellt werden!"); return; }
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

            //Save to DB file
            string str = "";
            if (selectedTVI != null) str = "\n[" + selectedTVI.Header.ToString() + "]\n";
            else str = "\n[" + gruppe.Text + "]\n";
            File.AppendAllText(@"data.csv", str);
            foreach (string i in lines) File.AppendAllText(@"data.csv", i + "\n");
        }

        //Saves ComboBoxes and TextBoxes into the file and cleans the form
        private void SaveButton(object sender, RoutedEventArgs e)
        {
            if (TreeView.SelectedItem == null) { MessageBox.Show("Bitte Warengruppe anlagen und auswählen!"); return; }
            if (parent.GetType() == typeof(TreeViewItem)) { MessageBox.Show("Sie können keinen Artikel in einem anderen hinzufügen. Es muss eine Warengruppe sein!"); return; }
            if (Artikel.Text == "") { MessageBox.Show("Bitte geben Sie den Artikelname an!"); return; }

            //Add Artikel to Warengruppe in TreeView
            selectedTVI.Items.Add(new TreeViewItem() { Header = Artikel.Text });

            SaveWGToDB();

            LoadTVItems();
        }

        //Load Artikeln from DB depending on the Warengruppe selected
        void LoadTVItems()
        {
            parent = GetSelectedTreeViewItemParent(selectedTVI);
            int trigger = 0, trigger2 = 0, n = 0, nr = 0;
            string row_artikel = "";
            string[] artikel = new string[21];

            // If selected item is Warengruppe then retrieve data (Artikeln) for this group from DB and output in the DataGrid
            if (parent.GetType() == typeof(TreeView) && File.Exists(@"data.csv"))
            {
                string csvData = File.ReadAllText("data.csv");
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
                            row_artikel = row;
                            if (row == "Artikel," + i.Header.ToString() && row_artikel.TrimStart("Artikel,".ToCharArray()).Count() > 0)
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
                            if (s.StartsWith("Artikel")) str_artikel = s.TrimStart("Artikel,".ToCharArray());
                            if (s.StartsWith("PluEan")) str_pluean = s.TrimStart("PluEan,".ToCharArray());
                            if (s.StartsWith("VKPreisBrutto")) str_preis = s.TrimStart("VKPreisBrutto,".ToCharArray());
                            if (s.StartsWith("AuserHausComboBox2")) str_mwst = s.TrimStart("AuserHausComboBox2,".ToCharArray());
                            if (s.StartsWith("Bestand")) str_bestand = s.TrimStart("Bestand,".ToCharArray());
                        }
                        it.Add(new items { nr = nr++.ToString(), pluean = str_pluean, artikel = str_artikel, preis = str_preis, mwst = str_mwst, bestand = str_bestand });
                    }
                }//1st foreach
                Data = it;
                dg3.ItemsSource = Data;
                it = new List<items>();
            }
        }

        // If TreeView item was selected (warengruppe or artikel)
        private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
        {
            TreeView.Tag = e.OriginalSource;
            selectedTVI = TreeView.Tag as TreeViewItem;
            
            if (selectedTVI != null)
            {
                parent = GetSelectedTreeViewItemParent(selectedTVI);
                LoadTVItems();
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
            if (e.Column.Header.ToString() == "warengruppe") e.Column.Header = "Warengruppe";
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
                selectedTVI = newChild;
                SaveWGToDB();
            }
            else
            {
                MessageBox.Show("Field Box cannot be empty or contain the name that exists already!");
            }
        }

        //Delete the group from the TreeView
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TreeView.SelectedItem != null && parent.GetType() == typeof(TreeView)) TreeView.Items.RemoveAt(TreeView.Items.IndexOf(TreeView.SelectedItem));
        }

        //Searching for Artikel by name
        private void TextSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string csvData = File.ReadAllText("data.csv");
            int trigger = 0, trigger2 = 0, n = 0;
            string[] artikel = new string[21];

            foreach (TreeViewItem t in TreeView.Items)
            {
                foreach (TreeViewItem i in t.Items)
                {
                    if (i.Header.ToString().ToUpper().Contains(SearchBoxArtikel.Text.ToUpper()))
                    {
                        foreach (string row in csvData.Split('\n'))
                        {
                            if (!string.IsNullOrEmpty(row) && row == (string)"[" + t.Header.ToString() + "]")
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
                        }
                        if (!string.IsNullOrEmpty(artikel[0]))
                        {
                            string str_artikel = "", str_waren = "", str_pluean = "", str_preis = "", str_mwst = "", str_bestand = "";
                            foreach (string s in artikel)
                            {
                                if (s.StartsWith("Artikel")) str_artikel = s.TrimStart("Artikel,".ToCharArray());
                                if (s.StartsWith("PluEan")) str_pluean = s.TrimStart("PluEan,".ToCharArray());
                                if (s.StartsWith("VKPreisBrutto")) str_preis = s.TrimStart("VKPreisBrutto,".ToCharArray());
                                if (s.StartsWith("AuserHausComboBox2")) str_mwst = s.TrimStart("AuserHausComboBox2,".ToCharArray());
                                if (s.StartsWith("Bestand")) str_bestand = s.TrimStart("Bestand,".ToCharArray());
                            }
                            str_waren = t.Header.ToString();
                            it2.Add(new items2 { warengruppe = str_waren, pluean = str_pluean, artikel = str_artikel, preis = str_preis, mwst = str_mwst, bestand = str_bestand });
                        }
                    }
                    
                }//2nd foreach
            }//1st foreach
            Data2 = it2;
            dg3.ItemsSource = Data2;
            it2 = new List<items2>();
        }

        private void EAN_SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string csvData = File.ReadAllText("data.csv");
            int trigger = 0, trigger2 = 0, n = 0;
            string[] artikel = new string[15];

            foreach (TreeViewItem t in TreeView.Items)
            {
                foreach (TreeViewItem i in t.Items)
                {
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row) && row == (string)"[" + t.Header.ToString() + "]")
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
                                if (!row.Contains("PluEan,"))
                                {
                                    artikel[n++] = row;
                                }
                                else
                                {
                                    artikel[n] = row;
                                    if (row.TrimStart("PluEan,".ToCharArray()).ToUpper().Contains(SearchBoxArtikel.Text.ToUpper()))
                                    {
                                        trigger = 0;
                                        trigger2 = 0;
                                        n = 0;
                                        break;
                                    }
                                    else
                                    {
                                        trigger = 0;
                                        trigger2 = 0;
                                        n = 0;
                                        artikel = new string[15];
                                    }
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(artikel[0]))
                    {
                        string str_artikel = "", str_waren = "", str_pluean = "", str_preis = "", str_mwst = "", str_bestand = "";
                        foreach (string s in artikel)
                        {
                            //MessageBox.Show(s);
                            if (s.StartsWith("Artikel")) str_artikel = s.TrimStart("Artikel,".ToCharArray());
                            if (s.StartsWith("PluEan")) str_pluean = s.TrimStart("PluEan,".ToCharArray());
                            if (s.StartsWith("VKPreisBrutto")) str_preis = s.TrimStart("VKPreisBrutto,".ToCharArray());
                            if (s.StartsWith("AuserHausComboBox2")) str_mwst = s.TrimStart("AuserHausComboBox2,".ToCharArray());
                            if (s.StartsWith("Bestand")) str_bestand = s.TrimStart("Bestand,".ToCharArray());
                        }
                        str_waren = t.Header.ToString();
                        it2.Add(new items2 { warengruppe = str_waren, pluean = str_pluean, artikel = str_artikel, preis = str_preis, mwst = str_mwst, bestand = str_bestand });
                    }

                }//2nd foreach
            }//1st foreach
            Data2 = it2;
            dg3.ItemsSource = Data2;
            it2 = new List<items2>();
        }
    }
}
