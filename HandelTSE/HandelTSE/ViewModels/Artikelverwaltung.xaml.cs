using Microsoft.Windows.Controls;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataGridRow = System.Windows.Controls.DataGridRow;

namespace HandelTSE.ViewModels
{
    public partial class Artikelverwaltung
    {
        public List<items> Data { get; set; }
        public List<items2> Data2 { get; set; }
        public TreeViewItem selectedTVI { get; set; }
        List<items> it = new List<items>();
        List<items2> it2 = new List<items2>();
        List<string> articlesToDelete = new List<string>();
        ItemsControl parent { get; set; }
        CheckBox checkBox = new CheckBox();
        public Artikelverwaltung()
        {
            InitializeComponent();
            if (!File.Exists(@"data.csv")) File.Create(@"data.csv").Close();
            
            //dg3.Columns.Add(chk);
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
                        else if (row.Contains("Artikel,") && !string.IsNullOrEmpty(row.Substring(row.IndexOf(",") + 1)))
                        {
                            //Look for currently added Warengruppe for this Artikel
                            foreach (TreeViewItem i in TreeView.Items)
                                if (i.Header.ToString() == prevNode) { i.Items.Add(new TreeViewItem() { Header = row.Substring(row.IndexOf(",") + 1)}); }
                        }
                    }
                }
            }
        }

        //Checks if TreeView contains newChild item
        public int CheckGroup(TreeViewItem newChild) { foreach (TreeViewItem i in TreeView.Items) if (i.Header.ToString() == newChild.Header.ToString()) return 0; return 1; }

        //Checks if group contains newchild item
        public int CheckGroupItems(TreeViewItem newChild) { foreach (TreeViewItem i in selectedTVI.Items) if (i.Header.ToString() == newChild.Header.ToString()) return 0; return 1; }

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

        //DataGrid columns in case Articles searched by EAN or Artikel Name
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
                        if (string.IsNullOrEmpty(cb.Text) && CheckGroupItems(newChild) == 0) { MessageBox.Show("Artikel mit demselben Namen oder leerem Inhalt kann nicht in derselben Warengruppe erstellt werden!"); return; }
                    }
                    // Check if in text fields no special character like '[' for enclosing Warengruppe name in database file is used
                    if (cb.Text.Contains("[") || cb.Text.Contains("]")) { MessageBox.Show("Textfelder dürfen keine '[' oder ']' Zeichen enthalten!"); return; }
                    lines.Add(string.Format("{0},{1}", cb.Name, cb.Text));
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

            SaveWGToDB();

            //Add Artikel to Warengruppe in TreeView
            selectedTVI.Items.Add(new TreeViewItem() { Header = Artikel.Text });
            Artikel.Text = "";

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
                //Clear the list with chosen articles from the previous WG
                articlesToDelete.Clear();

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
                            if (row == "Artikel," + i.Header.ToString() && row.Substring(row.IndexOf(",") + 1).Count() > 0)
                            {
                                trigger2 = 1;
                            }
                            if (trigger2 == 1)
                            {
                                if (!row.Contains("ImHausComboBox"))
                                {
                                    if (n < 20) artikel[n++] = row;
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
                            if (s.StartsWith("Artikel")) str_artikel = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("PluEan")) str_pluean = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("VKPreisBrutto")) str_preis = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("AuserHausComboBox2")) str_mwst = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("Bestand")) str_bestand = s.Substring(s.IndexOf(",") + 1);
                        }
                        it.Add(new items { nr = nr++.ToString(), pluean = str_pluean, artikel = str_artikel, preis = str_preis, mwst = str_mwst, bestand = str_bestand });
                    }
                }//1st foreach
                Data = it;
                
                dg3.ItemsSource = Data;

                it = new List<items>();
            }
        }

        // Used by button "alle Artikel"
        private void CheckedBox_SelectAll(object sender, RoutedEventArgs e)
        { foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(dg3)) ch.IsChecked = true; }

        private void CheckedBox_DeselectAll(object sender, RoutedEventArgs e)
        { foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(dg3)) ch.IsChecked = false; }

        // When checkbox is checked method adds Artikel to articlesToDelete (queue for deleting)
        private void CheckedBox(object sender, RoutedEventArgs e)
        {
            checkBox = (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);

            if (checkBox.IsChecked == true && !String.IsNullOrEmpty(dataGridRow.ToString()))
            {
                var data = dataGridRow.Item as items;
                if (!articlesToDelete.Contains(data.artikel)) articlesToDelete.Add(data.artikel);
            }

            e.Handled = true;
        }

        // When unchecked the checkbox its deleted from the articlesToDelete array used later to delete articles
        private void UncheckedBox(object sender, RoutedEventArgs e)
        {
            checkBox = (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);

            if (checkBox.IsChecked == false && !String.IsNullOrEmpty(dataGridRow.ToString()))
            {
                var data = dataGridRow.Item as items;
                if (articlesToDelete.Contains(data.artikel)) articlesToDelete.Remove(data.artikel);
            }

            e.Handled = true;
        }

        // If TreeView item was selected (warengruppe or artikel)
        private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
        {
            TreeView.Tag = e.OriginalSource;
            selectedTVI = TreeView.Tag as TreeViewItem;
            parent = GetSelectedTreeViewItemParent(selectedTVI);

            if (selectedTVI != null) { LoadTVItems(); }

            if (parent.GetType() == typeof(TreeViewItem)) 
            {
                // Select the row in DataGrid corresponding to the TreeViewItem selected and tick the checkbox to add Artikel to articlesToDelete array
                try
                {
                    // Select row in DataGrid based on the selected TreeViewItem (Artikel) in the Warengruppe in TreeView
                    var emp = (from i in Data
                               where i.artikel == selectedTVI.Header.ToString()
                               select i).FirstOrDefault();
                    if (emp != null) dg3.SelectedItem = emp;

                    // Checks the checkbox on the selected row in DataGrid
                    var row = (DataGridRow)dg3.ItemContainerGenerator.ContainerFromIndex(dg3.SelectedIndex);
                    foreach (CheckBox x in Artikelverwaltung.FindVisualChildren<CheckBox>(row))  x.IsChecked = true;
                }
                catch (Exception){}
            }
        }

        //Change default names inherited from item class variables to human readable
        private void CustomizeHeaders(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
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
            foreach (TextBox cb in Artikelverwaltung.FindVisualChildren<TextBox>(this)) if (cb.Name != "gruppe" & cb.Name != "SearchBoxArtikel") cb.Text = "";
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
            } else { MessageBox.Show("Field Box cannot be empty or contain the name that exists already!"); }
        }

        //Delete the Warengruppe from the TreeView
        private void WarenGruppeDelete_Click(object sender, RoutedEventArgs e)
        {
            if (TreeView.SelectedItem == null || parent.GetType() != typeof(TreeView)) { MessageBox.Show("Bitte wählen Sie die Warengruppe im TreeView-Bereich aus"); return; }
            string messageBoxText = "Achtung!\n\nAlle dazugehörige Artikel werden gelöscht.\nWollen Sie wirklich die Daten löschen?";
            string caption = "Warengruppe löschen";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            string csvData = File.ReadAllText(@"data.csv");
            int trigger = 0;

            // Process the user choice
            switch (result)
            {
                case MessageBoxResult.OK:
                    List <string> data = new List<string>(csvData.Split('\n'));
                    for (int i = 0; i < data.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(data[i]) && data[i] == (string)"[" + selectedTVI.Header.ToString() + "]")
                        {
                            trigger = 1;
                        }
                        if (trigger == 1)
                        {
                            if (string.IsNullOrEmpty(data[i]))
                            { data.RemoveAt(i--); trigger = 0; continue; }
                            data.RemoveAt(i--);
                        }
                    }
                    File.WriteAllLines("data.csv", new [] { String.Join("\n", data) });
                    TreeView.Items.RemoveAt(TreeView.Items.IndexOf(TreeView.SelectedItem));
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        //Delete all the articles from WG
        private void AlleArtikelnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (TreeView.SelectedItem == null || parent.GetType() != typeof(TreeView)) { MessageBox.Show("Bitte wählen Sie die Warengruppe im TreeView-Bereich aus"); return; }
            string messageBoxText = "Achtung!\n\nWollen Sie wirklich alle Artikel löschen?";
            string caption = "Artikel löschen";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            string csvData = File.ReadAllText(@"data.csv");
            int trigger = 0, trigger2 = 0;

            // Process the user choice
            switch (result)
            {
                case MessageBoxResult.OK:
                    List<string> data = new List<string>(csvData.Split('\n'));
                    foreach (TreeViewItem item in selectedTVI.Items)
                    {
                        for (int i = 0; i < data.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(data[i]) && data[i] == (string)"[" + selectedTVI.Header.ToString() + "]")
                            {
                                trigger = 1;
                                continue;
                            }
                            if (trigger == 1)
                            {
                                if (string.IsNullOrEmpty(data[i]))
                                { data.RemoveAt(i--); trigger = 0; trigger2 = 0; continue; }
                                if (data[i].Substring(data[i].IndexOf(",") + 1) == item.Header.ToString())
                                {
                                    data.RemoveAt(--i);
                                    trigger2 = 1;
                                }
                                if (trigger2 == 1)
                                {
                                    data.RemoveAt(i--);
                                }
                            }
                        }
                    }
                    File.WriteAllLines("data.csv", new[] { String.Join("\n", data) });
                    selectedTVI.Items.Clear();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        //Delete one article from WG
        private void ArtikelDelete_Click(object sender, RoutedEventArgs e)
        {
            if (articlesToDelete.Count == 0) { MessageBox.Show("Bitte wählen Sie die Artikel im Tabelle aus"); return; }
            string messageBoxText = "Achtung!\n\nWollen Sie wirklich " + articlesToDelete.Count + " Artikel löschen?";
            string caption = "Artikel löschen";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            string csvData = File.ReadAllText(@"data.csv");
            int trigger = 0, trigger2 = 0;

            // Process the user choice
            switch (result)
            {
                case MessageBoxResult.OK:
                    List<string> data = new List<string>(csvData.Split('\n'));
                    string WG = "";
                    TreeViewItem tv = new TreeViewItem();
                    if (parent.GetType() == TreeView.GetType()) { tv = selectedTVI; WG = selectedTVI.Header.ToString(); }
                    else
                    {
                        tv = (TreeViewItem)GetSelectedTreeViewItemParent(selectedTVI);
                        WG = tv.Header.ToString();
                    }
                    for (int j = 0; j < articlesToDelete.Count; j++)
                    {
                        for (int i = 0; i < data.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(data[i]) && data[i] == (string)"[" + WG + "]")
                            {
                                trigger = 1;
                                continue;
                            }
                            if (trigger == 1)
                            {
                                if (string.IsNullOrEmpty(data[i]) && trigger2 == 1)
                                { data.RemoveAt(i); trigger = 0; trigger2 = 0; continue; }
                                if (data[i].Substring(data[i].IndexOf(",") + 1) == articlesToDelete[j])
                                {
                                    data.RemoveAt(--i);
                                    trigger2 = 1;
                                }
                                if (trigger2 == 1)
                                {
                                    data.RemoveAt(i--);
                                }
                            }
                        }
                    }
                    File.WriteAllLines("data.csv", new[] { String.Join("\n", data) });

                    //Loop through all TreeViewItems to delete ones that were deleted from DB
                    for (int i = 0; i < tv.Items.Count; i++) 
                        if (articlesToDelete.Contains(((TreeViewItem)tv.Items[i]).Header.ToString())) 
                            tv.Items.RemoveAt(tv.Items.IndexOf(((TreeViewItem)tv.Items[i])));
                    LoadTVItems();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
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
                                if (s.StartsWith("Artikel")) str_artikel = s.Substring(s.IndexOf(",") + 1);
                                if (s.StartsWith("PluEan")) str_pluean = s.Substring(s.IndexOf(",") + 1);
                                if (s.StartsWith("VKPreisBrutto")) str_preis = s.Substring(s.IndexOf(",") + 1);
                                if (s.StartsWith("AuserHausComboBox2")) str_mwst = s.Substring(s.IndexOf(",") + 1);
                                if (s.StartsWith("Bestand")) str_bestand = s.Substring(s.IndexOf(",") + 1);
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

        //Searching for Artikel by EAN
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
                                    if (row.Substring(row.IndexOf(",") + 1).ToUpper().Contains(SearchBoxArtikel.Text.ToUpper()))
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
                            if (s.StartsWith("Artikel")) str_artikel = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("PluEan")) str_pluean = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("VKPreisBrutto")) str_preis = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("AuserHausComboBox2")) str_mwst = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("Bestand")) str_bestand = s.Substring(s.IndexOf(",") + 1);
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

        // Looks for children of the element (Control)
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
    }

    // Class with method for looking the Parent of the element (Control)
    public class VisualTreeHelpers
    {
        // Returns the first ancester of specified type
        public static T FindAncestor<T>(DependencyObject current)
        where T : DependencyObject
        {
            current = VisualTreeHelper.GetParent(current);

            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            };
            return null;
        }
    }
}
