using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DataGridRow = System.Windows.Controls.DataGridRow;
using Color = System.Windows.Media.Color;
using System.Windows.Controls.Primitives;
using System.Threading;

namespace HandelTSE.ViewModels
{
    static class Globals
    {
        public static int high = 0;
        public static int opened = 0;
        public static int Training_mode = 0;
        public static int Admin_mode = 0;
        public static string CsvZeitungenFilePath = "";
        public static PresseUndVMP p = new PresseUndVMP();
    }
    public partial class Artikelverwaltung
    {
        public delegate System.Windows.Point GetPosition(IInputElement element);
        public List<items> Data { get; set; }
        public List<items2> Data2 { get; set; }
        public TreeViewItem selectedTVI { get; set; }
        public BrushConverter bc = new BrushConverter();
        List<items> it = new List<items>();
        List<items2> it2 = new List<items2>();
        List<string> articlesToDelete = new List<string>();
        public static string ArtikelName;
        public static string WG_str;
        int eanSuchenTrigger = 0, n_size = 20;

        public ItemsControl parent { get; set; }
        CheckBox checkBox = new CheckBox();
        public Int32 rowIndex;
        int ArtikelCodeLength = 0;
        int codeGeneriert = 7600001;

        public Artikelverwaltung()
        {
            InitializeComponent();
            if (!File.Exists(@"data.csv")) File.Create(@"data.csv").Close();
            dg3.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(dg3_PreviewMouseLeftButtonDown);
            EinheitDataChanged(null, null);
        }

        //Fires when TreeView needs to be loaded
        private void LoadForm(object sender, System.EventArgs e) { LoadTreeViewFromDB(); }

        //Loads Warengruppen and Artikeln from DB into the TreeView (Update the TreeView)
        public void LoadTreeViewFromDB()
        {
            TreeView.Items.Refresh();
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
                                if (i.Header.ToString() == prevNode)
                                {  i.Items.Add(new TreeViewItem() { Header = row.Substring(row.IndexOf(",") + 1) }); }
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
            if (PluEan.SelectedIndex > 1)
                if (ArtikelCodeTemplateValue.Text.Length != ArtikelCodeLength) { MessageBox.Show("Der Artikelcode muss " + ArtikelCodeLength + " Zaichen lang sein."); return; }
            var lines = new List<string>();
            foreach (TextBox cb in Artikelverwaltung.FindVisualChildren<TextBox>(this))
                if (cb.Name != "gruppe" && cb.Name != "PART_EditableTextBox" && cb.Name != "SearchBoxArtikel")
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
                if (cb.Name != "WGComboBox" && cb.Name != "ComboBoxMitPreis" && cb.Name != "ArtikelCodeTemplate") lines.Add(string.Format("{0},{1}", cb.Name, cb.Text)); 
                if (cb.Name == "PluEan" && cb.Text == codeGeneriert.ToString()) File.WriteAllText(@"eancodegenerieren.csv", codeGeneriert.ToString());
                cb.SelectedIndex = 0;
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
            if (PluEan.Text == "") { MessageBox.Show("Bitte geben Sie den PluEan an!"); return; }
            if (PluEan.SelectedIndex > 1)
                if (ArtikelCodeTemplateValue.Text.Length != ArtikelCodeLength) { MessageBox.Show("Der Artikelcode muss " + ArtikelCodeLength + " Zaichen lang sein."); return; }

            if (CheckGroupItems(new TreeViewItem() { Header = Artikel.Text }) == 1)
            {
                SaveWGToDB();

                //Add Artikel to Warengruppe in TreeView
                selectedTVI.Items.Add(new TreeViewItem() { Header = Artikel.Text });
                Artikel.Text = "";
            }
            else
            {
                string csvData = File.ReadAllText(@"data.csv");

                List<string> data = new List<string>(csvData.Split('\n'));
                TreeViewItem item = new TreeViewItem();
                string WG = selectedTVI.Header.ToString();

                int trigger = 0, trigger2 = 0, trigger3 = 0, trigger4 = 0;

                for (int j = 0; j < data.Count(); j++)
                {
                    if (string.IsNullOrEmpty(data[j]) || data[j].Length <= 0) continue;
                    if (data[j] == (string)"[" + WG + "]") { trigger = 1; continue; }
                    if (trigger == 1)
                    {
                        if (data[j].Substring(data[j].IndexOf(",") + 1) == Artikel.Text) { trigger2 = 1; continue; }
                        if (trigger2 == 1)
                        {
                            foreach (TextBox cb in Artikelverwaltung.FindVisualChildren<TextBox>(this))
                            {
                                trigger3 = 1;
                                // Check if in text fields no special character like '[' for enclosing Warengruppe name in database file is used
                                if (cb.Text.Contains("[") || cb.Text.Contains("]")) { MessageBox.Show("Textfelder dürfen keine '[' oder ']' Zeichen enthalten!"); return; }

                                    if (data[j].Substring(0, data[j].IndexOf(",")) == cb.Name)
                                    {
                                        data[j] = data[j].Substring(0, data[j].IndexOf(",")+1) + cb.Text;
                                        trigger3 = 0;
                                        break;
                                    }
                            }

                            if (trigger3 == 1)
                                foreach (ComboBox cb in Artikelverwaltung.FindVisualChildren<ComboBox>(this))
                                {
                                    if (data[j].Substring(0, data[j].IndexOf(",")) == cb.Name)
                                    {
                                        if (data[j].Substring(0, data[j].IndexOf(",")) == "ImHausComboBox2") trigger4 = 1;
                                        data[j] = data[j].Substring(0, data[j].IndexOf(",")+1) + cb.Text;
                                        break;
                                    }
                                }
                        }
                    }
                    if (trigger4 == 1) break;
                }

                File.WriteAllLines("data.csv", new[] { String.Join("\n", data) });
            }

            LoadTVItems();
        }

        //Load Artikeln to DataGrid from DB depending on the Warengruppe selected in the TreeView
        public void LoadTVItems()
        {
            parent = GetSelectedTreeViewItemParent(selectedTVI);
            int trigger = 0, trigger2 = 0, n = 0, nr = 0;
            string row_artikel = "";
            string[] artikel = new string[n_size];
            var WG = new TreeViewItem();

            if (parent.GetType() == TreeView.GetType()) { WG = selectedTVI; }
            else { WG = (TreeViewItem)GetSelectedTreeViewItemParent(selectedTVI); }

            // If selected item is Warengruppe then retrieve data (Artikeln) for this group from DB and output in the DataGrid
            if (File.Exists(@"data.csv"))
            {
                //Clear the list with chosen articles from the previous WG
                articlesToDelete.Clear();

                string csvData = File.ReadAllText("data.csv");
                foreach (TreeViewItem i in WG.Items)
                {
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row) && row == (string)"[" + WG.Header.ToString() + "]")
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
                                    if (n < 19) artikel[n++] = row;
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
                            if (s.StartsWith("Artikel,")) str_artikel = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("PluEan")) str_pluean = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("VKPreisBrutto")) str_preis = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("AusserHaus")) str_mwst = s.Substring(s.IndexOf(",") + 1);
                            if (s.StartsWith("Bestand")) str_bestand = s.Substring(s.IndexOf(",") + 1);
                        }
                        it.Add(new items { nr = nr++.ToString(), pluean = str_pluean, artikel = str_artikel, preis = str_preis, mwst = str_mwst, bestand = str_bestand });
                    }
                }//1st foreach
                Data = it;
                
                dg3.ItemsSource = Data;

                it = new List<items>();
            }

            //Hide DataGridRow transfering options to other WG buttons
            {
                EtikettDruckenButton.Visibility = Visibility.Hidden;
                KopierenInWGButton.Visibility = Visibility.Hidden;
                VerschiebenInWGButton.Visibility = Visibility.Hidden;
                WGComboBox.Visibility = Visibility.Hidden;
                KopieSpeichernButton.Visibility = Visibility.Hidden;
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
                if (eanSuchenTrigger == 0) ArtikelName = ((items)dataGridRow.Item).artikel;
                else ArtikelName = ((items2)dataGridRow.Item).artikel;
                /* ERROR FIX WHEN DELETED ONE ARTICLE SELECTED FROM TV and DG */
                if (!articlesToDelete.Contains(ArtikelName)) articlesToDelete.Add(ArtikelName);
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
                if (eanSuchenTrigger == 0) ArtikelName = ((items)dataGridRow.Item).artikel;
                else ArtikelName = ((items2)dataGridRow.Item).artikel;
                if (articlesToDelete.Contains(ArtikelName)) articlesToDelete.Remove(ArtikelName);
            }
            e.Handled = true;
        }

        // If TreeView item was selected (warengruppe or artikel)
        private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
        {
            eanSuchenTrigger = 0;
            TreeView.Tag = e.OriginalSource;
            selectedTVI = TreeView.Tag as TreeViewItem;

            if (selectedTVI != null) { LoadTVItems(); }

            // FOR clicking on the Artikel in TreeView and selecting the row in DG (BUGGY !!! when dg3.selectedItem = emp;)
            {
                /*if (parent.GetType() == typeof(TreeViewItem)) 
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
                    catch (Exception){ MessageBox.Show("CHECKBOX ERROR !"); }
                }*/
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
        private void CleanGroupButton(object sender, RoutedEventArgs e) { gruppe.Text = ""; if(selectedTVI != null) selectedTVI.IsSelected = false; }

        //Adds the group with the name and color chosen in the textfield to the TreeView
        private void WarenGruppeSave(object sender, RoutedEventArgs e)
        {
            string line = null;
            // Checks if Warengruppe field is not empty and does not contain same name as existing group in the TreeView
            if (String.IsNullOrEmpty(gruppe.Text) || TreeView.Items.Cast<TreeViewItem>().Any(item => item.Header.ToString() == gruppe.Text))
            { MessageBox.Show("Feld Box darf nicht leer sein oder den bereits vorhandenen Namen enthalten!"); } 
            else if(selectedTVI == null || selectedTVI.IsSelected == false) // IF item on the TreeView was not selected then create new
            { 
                TreeViewItem newChild = new TreeViewItem() { Header = gruppe.Text };
                TreeView.Items.Add(newChild);
                selectedTVI = newChild;
                SaveWGToDB();
                if (cp.SelectedColor != null) line = string.Format("{0},{1}", gruppe.Text, cp.SelectedColor.Value);
            }
            else // Otherwise if item is selected change WG name in the DB saving the articles
            {
                string csvData = File.ReadAllText(@"data.csv");
                List<string> data = new List<string>(csvData.Split('\n'));
                
                for (int i = 0; i < data.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(data[i]) && data[i] == (string)"[" + selectedTVI.Header.ToString() + "]")
                    {
                        data[i] = "[" + gruppe.Text + "]";
                    }
                }
                if (cp.SelectedColor != null)
                {
                    string csvData2 = File.ReadAllText(@"data_colors.csv");
                    List<string> data2 = new List<string>(csvData2.Split('\n'));
                    int trigger = 0;

                    for (int i = 0; i < data2.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(data2[i]) && data2[i].Contains(selectedTVI.Header.ToString() + ","))
                        {
                            data2[i] = gruppe.Text + "," + cp.SelectedColor.Value.ToString();
                            trigger = 1;
                        }
                    }
                    if (trigger == 0) File.AppendAllText("data_colors.csv", string.Format("{0},{1}", gruppe.Text, cp.SelectedColor.Value));
                    if (trigger == 1) File.WriteAllLines("data_colors.csv", new[] { String.Join("\n", data2) });
                }
                else
                {
                    string csvData2 = File.ReadAllText(@"data_colors.csv");
                    List<string> data2 = new List<string>(csvData2.Split('\n'));

                    for (int i = 0; i < data2.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(data2[i]) && data2[i].Contains(selectedTVI.Header.ToString() + ","))
                        {
                            data2[i] = gruppe.Text + data2[i].Substring(data2[i].IndexOf(","));
                        }
                        File.WriteAllLines("data_colors.csv", new[] { String.Join("\n", data2) });
                    }
                }
                File.WriteAllLines("data.csv", new[] { String.Join("\n", data) });
                TreeView.Items.RemoveAt(TreeView.Items.IndexOf(TreeView.SelectedItem));
                LoadForm(this, e);
            }
            if (line != null) File.AppendAllText("data_colors.csv", line+"\n");
            Artikel.Text = "";
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

        //Delete chosen articles from WG
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
                    else { tv = (TreeViewItem)GetSelectedTreeViewItemParent(selectedTVI); WG = tv.Header.ToString(); }

                    for (int j = 0; j < articlesToDelete.Count; j++)
                    {
                        for (int i = 0; i < data.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(data[i]) && data[i] == (string)"[" + WG + "]")
                            {
                                trigger = 1;
                                continue;
                            }
                            else if (data[i].Contains("[") && data[i].Contains("]")) trigger = 0;
                            if (trigger == 1)
                            {
                                if (string.IsNullOrEmpty(data[i]) && trigger2 == 1)
                                { data.RemoveAt(i); trigger = 0; trigger2 = 0; continue; }
                                if (data[i].Substring(data[i].IndexOf(",") + 1) == articlesToDelete[j])
                                {
                                    if (i == 0) break;
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
            eanSuchenTrigger = 1;
            if (SearchBoxArtikel.Text == "") return;
            string csvData = File.ReadAllText("data.csv");
            int trigger = 0, trigger2 = 0;
            List<string> artikel = new List<string>();

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
                                        artikel.Add(row);
                                    }
                                    else
                                    {
                                        artikel.Add(row);
                                        trigger = 0;
                                        trigger2 = 0;
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
                    artikel = new List<string>();
                }//2nd foreach
            }//1st foreach
            Data2 = it2;
            dg3.ItemsSource = Data2;
            it2 = new List<items2>();
        }

        //Searching for Artikel by EAN
        private void EAN_SearchButton_Click(object sender, RoutedEventArgs e)
        {
            eanSuchenTrigger = 1;
            if (SearchBoxArtikel.Text == "") return;
            string csvData = File.ReadAllText("data.csv");
            int trigger = 0, trigger2 = 0;
            List<string> artikel = new List<string>();
            string WG = "", ArtikelName = "";

            foreach (string s in csvData.Split('\n'))
            {
                if (s.Contains("[") && s.Contains("]")) { WG = s; trigger = 1; continue; }
                if (s.Substring(s.IndexOf("Artikel,") + 1).Length > 0 && trigger == 1) { ArtikelName = s; artikel.Add(WG); artikel.Add(ArtikelName); trigger = 0; trigger2 = 1; continue; }
                trigger = 0;
                if (trigger2 == 1)
                {
                    artikel.Add(s);
                }
                if (s.Contains("ImHausComboBox2"))
                {
                    trigger2 = 0;
                    foreach (string i in artikel) if (i.Contains("PluEan,") && i.Substring(i.IndexOf(",") + 1).ToUpper().Contains(SearchBoxArtikel.Text.ToUpper())) trigger2 = 1;
                    if (trigger2 == 1)
                    {
                        if (artikel.Count <= 0) return;
                        if (!string.IsNullOrEmpty(artikel[0]))
                        {
                            string str_artikel = "", str_waren = "", str_pluean = "", str_preis = "", str_mwst = "", str_bestand = "";
                            foreach (string t in artikel)
                            {
                                if (t.StartsWith("[") && t.Contains("]")) str_waren = t.Substring(t.IndexOf("[") + 1, t.IndexOf("]") - 1);
                                if (t.StartsWith("Artikel")) str_artikel = t.Substring(t.IndexOf(",") + 1);
                                if (t.StartsWith("PluEan")) str_pluean = t.Substring(t.IndexOf(",") + 1);
                                if (t.StartsWith("VKPreisBrutto")) str_preis = t.Substring(t.IndexOf(",") + 1);
                                if (t.StartsWith("AuserHausComboBox2")) str_mwst = t.Substring(t.IndexOf(",") + 1);
                                if (t.StartsWith("Bestand")) str_bestand = t.Substring(t.IndexOf(",") + 1);
                            }
                            it2.Add(new items2 { warengruppe = str_waren, pluean = str_pluean, artikel = str_artikel, preis = str_preis, mwst = str_mwst, bestand = str_bestand });
                        }
                        trigger2 = 0;
                    }
                    artikel = new List<string>();
                }
            }
            Data2 = it2;
            dg3.ItemsSource = Data2;
            it2 = new List<items2>();
        }

        //Change color of the output of WG
        private void ColorOfWGChange(object sender, RoutedEventArgs e) { if (cp.IsOpen == false) cp.IsOpen = true; else cp.IsOpen = false; }

        //For Color Picker
        private void cp_SelectedColorChanged_1(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cp.SelectedColor.HasValue)
            {
                Color C = cp.SelectedColor.Value;
                long colorVal = Convert.ToInt64(C.B * (Math.Pow(256, 0)) + C.G * (Math.Pow(256, 1)) + C.R * (Math.Pow(256, 2)));
            }
            gruppe.Background = new SolidColorBrush(cp.SelectedColor.Value);
            if (!File.Exists(@"data_colors.csv")) File.Create(@"data_colors.csv").Close();
            cp.Visibility = Visibility.Collapsed;
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

        // When row is chosen check the corresponding checkbox too
        private void dg3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as System.Windows.Controls.DataGrid;
            if (dg == null) return;
            var row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
            if (row == null) return;

            var WG = new TreeViewItem();
            if (eanSuchenTrigger == 0)
            {
                parent = GetSelectedTreeViewItemParent(selectedTVI);
                if (parent.GetType() == TreeView.GetType()) { WG = selectedTVI; }
                else { WG = (TreeViewItem)GetSelectedTreeViewItemParent(selectedTVI); }
                WG_str = WG.Header.ToString();
                ArtikelName = ((items)row.Item).artikel;
            }
            else { WG_str = ((items2)row.Item).warengruppe; ArtikelName = ((items2)row.Item).artikel; }

            foreach (CheckBox x in Artikelverwaltung.FindVisualChildren<CheckBox>(row)) { if (x.IsChecked == true) x.IsChecked = false; else x.IsChecked = true; }
            
            int trigger = 0, trigger2 = 0, n = 0;
            string row_artikel = "";
            string[] artikel = new string[n_size];
            
            ArtikelOptionenButton.IsEnabled = true;
            SetArtikelButton.IsEnabled = true;
            TreeViewItem chosenTVI = new TreeViewItem();

            string csvData = File.ReadAllText("data.csv");
            foreach (string s in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(s) && s == (string)"[" + WG_str + "]")
                {
                    trigger = 1;
                    continue;
                }
                if (trigger == 1)
                {
                    row_artikel = s;
                    if (s == "Artikel," + ArtikelName && s.Substring(s.IndexOf(",") + 1).Count() > 0)
                    {
                        trigger2 = 1;
                    }
                    if (trigger2 == 1)
                    {
                        if (!s.Contains("ImHausComboBox"))
                        {
                            if (n < 19) artikel[n++] = s;
                        }
                        else
                        {
                            artikel[n] = s;
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
                int trigger3 = 0;
                foreach (string s in artikel)
                {
                    if (s == null) continue;
                    trigger3 = 1;
                    foreach (TextBox cb in Artikelverwaltung.FindVisualChildren<TextBox>(this))
                    {
                        if (cb.Name == "gruppe" && cb.Name == "PART_EditableTextBox" && cb.Name == "SearchBoxArtikel" && cb.Name == "ArtikelCodeTemplateValue") continue;
                        if (cb.Name == s.Substring(0, s.IndexOf(",")) || cb.Name == s.Replace(",", ""))
                        {
                            cb.Text = s.Substring(s.IndexOf(",") + 1);
                            trigger3 = 0;
                            break;
                        }
                    }
                    if (trigger3 == 1)
                        foreach (ComboBox cb2 in Artikelverwaltung.FindVisualChildren<ComboBox>(this))
                            if (cb2.Name == s.Substring(0, s.IndexOf(",")))
                            {
                                cb2.Text = s.Substring(s.IndexOf(",") + 1);
                                break; 
                            }
                }
            }

            EtikettDruckenButton.Visibility = Visibility.Visible;
            KopierenInWGButton.Visibility = Visibility.Visible;
            VerschiebenInWGButton.Visibility = Visibility.Visible;
            WGComboBox.Visibility = Visibility.Visible;
            KopieSpeichernButton.Visibility = Visibility.Visible;
        } // END -- dg3_SelectionChanged

        // Depending on Template chosen from all available for each type of EAN templates, apply, 'preis' length limits
        private void ArtikelCodeTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = ArtikelCodeTemplate.SelectedIndex;
            if (i < 0) return;
            string csvData = "", line = "", template = "";
            ArtikelCodeTemplateValue.Text = "";
            if (PluEan.SelectedIndex == 2) { csvData = File.ReadAllText(@"eancodes_preis.csv"); }
            else if (PluEan.SelectedIndex == 3) { csvData = File.ReadAllText(@"eancodes_gewicht.csv"); }
            else if (PluEan.SelectedIndex == 4) { csvData = File.ReadAllText(@"eancodes_menge.csv"); }
            List<string> data = new List<string>(csvData.Split('\n'));
            if (data.Count < 1) return;

            if (data[i].Contains("["))
            {
                int j = 1, f = 0;
                for (; j < data[i].Length; j++)
                {
                    line = data[i];
                    if (line[j] == ',') { f++; continue; }
                    if (f == 2)
                    {
                        template += line[j];
                    }
                    if (f == 3) break;
                }
                ArtikelCodeLength = template.Length;
            }
        }

        // When PLU-EAN ComboBox value is "mit Preis" then make all VK and EK Preis fields read-only and show MitPreis related ComboBox and Field
        private void PluEan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PluEan.SelectedItem == null) {
                ArtikelOptionenButton.Visibility = Visibility.Visible;
                SetArtikelButton.Visibility = Visibility.Visible;
                ArtikelCodeTemplate.Visibility = Visibility.Hidden;
                ArtikelCodeTemplateValue.Visibility = Visibility.Hidden;
                return;
            }; // Safeguard for the case if the form with control of interest didn't load yet
            
            //Block/unblock VK and EK Preis text fields when "EAN mit Preis" is selected/deselected
            if (PluEan.SelectedIndex == 2)
            {
                VKPreisBrutto.Text = ""; VKPreisBrutto.IsReadOnly = true; VKPreisNetto.Text = ""; VKPreisNetto.IsReadOnly = true;
                EKPreisBrutto.Text = ""; EKPreisBrutto.IsReadOnly = true; EKPreisNetto.Text = ""; EKPreisNetto.IsReadOnly = true;
                AlsNegativCheckbox.Visibility = Visibility.Visible; AlsNegativTitel.Visibility = Visibility.Visible;
            }
            else if (PluEan.SelectedIndex >= 0 && VKPreisBrutto != null && PluEan.SelectedIndex != 5)
            {
                VKPreisBrutto.IsReadOnly = false; VKPreisNetto.IsReadOnly = false;
                EKPreisBrutto.IsReadOnly = false; EKPreisNetto.IsReadOnly = false;
                AlsNegativCheckbox.Visibility = Visibility.Hidden;
                AlsNegativTitel.Visibility = Visibility.Hidden;
            }

            if (PluEan.SelectedIndex > 1 && PluEan.SelectedIndex != 5)
            {
                ArtikelOptionenButton.Visibility = Visibility.Hidden;
                SetArtikelButton.Visibility = Visibility.Hidden;
                ArtikelCodeTemplate.Visibility = Visibility.Visible;
                ArtikelCodeTemplateValue.Visibility = Visibility.Visible;

                // Based on which PluEan option user chooses open appropriate file with records (EAN Templates)
                string csvData = "";
                ArtikelCodeTemplate.Items.Clear();
                if (PluEan.SelectedIndex == 2) { csvData = File.ReadAllText(@"eancodes_preis.csv"); }
                else if (PluEan.SelectedIndex == 3) { csvData = File.ReadAllText(@"eancodes_gewicht.csv"); }
                else if (PluEan.SelectedIndex == 4) { csvData = File.ReadAllText(@"eancodes_menge.csv"); }

                string line = "", template = "", templateName = "";
                List<string> data = new List<string>(csvData.Split('\n'));

                // Cycle through those records to acquire the templates for appropriate option
                for (int i = 0; i < data.Count(); i++)
                {
                    if (data[i].Contains("[")) // Beginnning of the record
                    {
                        int j = 1, f = 0;
                        for (; j < data[i].Length; j++)
                        {
                            line = data[i];
                            if (line[j] == ',') { f++; continue; }
                            if (f == 2 && i == 0)
                            {
                                template += line[j];
                            }
                            if (f == 3)
                            {
                                if (line[j] != ']') templateName += line[j];
                                else break;
                            }
                        }
                        if (i == 0) ArtikelCodeLength = template.Length;
                        ArtikelCodeTemplate.Items.Add(templateName);
                        templateName = "";
                    }
                }
                ArtikelCodeTemplate.SelectedIndex = 0;
            }
            else if (ArtikelOptionenButton != null) // In case PluEan einscannen or generieren is chosen return buttons
            {
                ArtikelOptionenButton.Visibility = Visibility.Visible;
                SetArtikelButton.Visibility = Visibility.Visible;
                ArtikelCodeTemplate.Visibility = Visibility.Hidden;
                ArtikelCodeTemplateValue.Visibility = Visibility.Hidden;
            }
            if (PluEan.SelectedIndex == 1 && PluEan.Items.Count <= 5) // if EAN-Codegenerieren option was chosen
            {
                if (!File.Exists("eancodegenerieren.csv")) 
                {
                    PluEan.Items.Add(codeGeneriert.ToString());
                    PluEan.SelectedIndex = 5;
                }
                else
                {
                    string csvData = File.ReadAllText("eancodegenerieren.csv");
                    List<string> data = new List<string>(csvData.Split('\n'));
                    PluEan.Items.Add((codeGeneriert = Int32.Parse(data[0])+1).ToString());
                    PluEan.SelectedIndex = 5;
                }
            }
            else if(PluEan.SelectedIndex != 5 && PluEan.SelectedIndex != 1) { PluEan.Items.Remove(codeGeneriert.ToString()); }
        }

        //Load Warengruppen to WGComboBox when it is set to visible
        public ObservableCollection<string> list = new ObservableCollection<string>();
        private void WGComboLoadElements(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (WGComboBox.IsVisible == true)
            {
                foreach(TreeViewItem t in TreeView.Items)
                {
                    list.Add(t.Header.ToString());
                }
                WGComboBox.ItemsSource = list;
            }
        }

        //Relocate DG Row to WG using ComboBox and Buttons
        private void VerschiebenInWGButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (WGComboBox.SelectedItem == null) { MessageBox.Show("Wählen Sie bitte eine Warengruppe aus"); return; };

            string csvData = File.ReadAllText(@"data.csv");
            List<string> data = new List<string>(csvData.Split('\n'));
            TreeViewItem item = new TreeViewItem();
            string WG = selectedTVI.Header.ToString();

            DataGridRow dataGridRow = (DataGridRow)dg3.ItemContainerGenerator.ContainerFromIndex(dg3.SelectedIndex);
            TreeViewItem chosenTVI = new TreeViewItem();
            if (eanSuchenTrigger == 0) ArtikelName = ((items)dataGridRow.Item).artikel;
            else ArtikelName = ((items2)dataGridRow.Item).artikel;

            int trigger = 0;

            for (int j = 0; j < data.Count(); j++)
            {
                if (!string.IsNullOrEmpty(data[j]) && data[j] == (string)"[" + WG + "]")
                {
                    trigger = 1;
                    continue;
                }
                if (trigger == 1)
                {
                    if (data[j].Substring(data[j].IndexOf(",") + 1) == ArtikelName)
                    {
                        foreach (TreeViewItem i in TreeView.Items) if (i.Header.ToString() == WGComboBox.SelectedItem.ToString()) item = i; 
                        data[--j] = "[" + item.Header.ToString() + "]";
                        break;
                    }
                }
            }

            File.WriteAllLines("data.csv", new[] { String.Join("\n", data) });
            //Adding relocated TreeViewItem to new WG
            item.Items.Add(new TreeViewItem() { Header = ArtikelName });
            //Deleting TVI from old WG
            foreach (TreeViewItem y in selectedTVI.Items) if (y.Header.ToString() == ArtikelName) { selectedTVI.Items.Remove(y); break; }
            LoadTVItems();
        }

        //Copy & Paste selected Artikel DG Row to WG using ComboBox and Buttons
        private void KopierenInWGButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (WGComboBox.SelectedItem == null) { MessageBox.Show("Wählen Sie bitte eine Warengruppe aus"); return; };

            string csvData = File.ReadAllText(@"data.csv");
            List<string> data = new List<string>(csvData.Split('\n'));
            TreeViewItem item = new TreeViewItem();
            string WG = selectedTVI.Header.ToString();

            DataGridRow dataGridRow = (DataGridRow)dg3.ItemContainerGenerator.ContainerFromIndex(dg3.SelectedIndex);
            TreeViewItem chosenTVI = new TreeViewItem();
            if (eanSuchenTrigger == 0) ArtikelName = ((items)dataGridRow.Item).artikel;
            else ArtikelName = ((items2)dataGridRow.Item).artikel;
            List<string> Artikel = new List<string>();
            int trigger = 0, trigger2 = 0;

            for (int j = 0; j < data.Count(); j++)
            {
                if (!string.IsNullOrEmpty(data[j]) && data[j] == (string)"[" + WG + "]")
                {
                    trigger = 1;
                    continue;
                }
                if (trigger == 1)
                {
                    if (data[j].Substring(data[j].IndexOf(",") + 1) == ArtikelName)
                    {
                        foreach (TreeViewItem i in TreeView.Items) if (i.Header.ToString() == WGComboBox.SelectedItem.ToString()) item = i;
                        Artikel.Add("[" + item.Header.ToString() + "]");
                        trigger2 = 1;
                    }
                    else if (trigger2 == 1)
                    {
                        Artikel.Add(data[j - 1]);
                        if (string.IsNullOrEmpty(data[j])) break;
                    }
                }
            }

            File.AppendAllLines("data.csv", new[] { String.Join("\n", Artikel) });
            //Adding copied TreeViewItem to new WG
            item.Items.Add(new TreeViewItem() { Header = ArtikelName });
            LoadTVItems();
        }

        private void ArtikelOptionenButton_Click(object sender, RoutedEventArgs e) { this.Content = new ArtikelOptionen(); }

        ///////// DataGridRow Drap&Drop START
        public void dg3_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rowIndex = GetCurrentRowIndex(e.GetPosition);
            if (rowIndex < 0) return;
            dg3.SelectedIndex = rowIndex;

            DataGridRow dataGridRow = (DataGridRow)dg3.ItemContainerGenerator.ContainerFromIndex(rowIndex);

            if (!String.IsNullOrEmpty(dataGridRow.ToString()))
            {
                TreeViewItem chosenTVI = new TreeViewItem();
                if (eanSuchenTrigger == 0) ArtikelName = ((items)dataGridRow.Item).artikel;
                else ArtikelName = ((items2)dataGridRow.Item).artikel;
                DragDropEffects dragdropeffects = DragDropEffects.Move;
                if (DragDrop.DoDragDrop(dg3, ArtikelName, dragdropeffects) != DragDropEffects.None)
                {
                    foreach (TreeViewItem i in TreeView.Items)
                    {
                        if (i.Background.ToString() == ((System.Windows.Media.Brush)bc.ConvertFrom("#FF0078D7")).ToString())
                        {
                            TreeViewDropHighlighter.OnDragLeave(sender, null);
                            if (TreeView.SelectedItem == null || parent.GetType() != typeof(TreeView)) { MessageBox.Show("Bitte wählen Sie die Warengruppe im TreeView-Bereich aus"); return; }
                            string messageBoxText = "Eine Artikel aus Warengruppe " + "'" + selectedTVI.Header.ToString() + "'" + " in die Warengruppe " + "'" + i.Header.ToString() + "'";
                            chosenTVI = i;
                            foreach (TreeViewItem t in i.Items) if(t.Header.ToString() == ArtikelName) { /*chosenTVI.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFFFFFFF");*/ MessageBox.Show("Artikel mit dem gleichen Namen existiert bereits!"); return; } 
                            DragDataGridRowMessageBox frm = new DragDataGridRowMessageBox(messageBoxText, "kopieren", "verschieben", "abbrechen");
                            frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                            frm.ShowDialog();

                            string csvData = File.ReadAllText(@"data.csv");
                            int trigger = 0;
                            // Process the user choice
                            switch (frm.UserChoice)
                            {
                                case "kopieren":
                                    List<string> data1 = new List<string>(csvData.Split('\n'));
                                    List<string> Artikel = new List<string>();
                                    string WG1 = "";
                                    int trigger2 = 0;

                                    if (parent.GetType() == TreeView.GetType()) { WG1 = selectedTVI.Header.ToString(); }
                                    else { WG1 = ((TreeViewItem)GetSelectedTreeViewItemParent(selectedTVI)).Header.ToString(); }

                                    for (int j = 0; j < data1.Count(); j++)
                                    {
                                        if (!string.IsNullOrEmpty(data1[j]) && data1[j] == (string)"[" + WG1 + "]")
                                        {
                                            trigger = 1;
                                            continue;
                                        }
                                        if (trigger == 1)
                                        {
                                            if (data1[j].Substring(data1[j].IndexOf(",") + 1) == ArtikelName)
                                            {
                                                Artikel.Add("[" + i.Header.ToString() + "]");
                                                trigger2 = 1;
                                            }
                                            else if (trigger2 == 1)
                                            {
                                                Artikel.Add(data1[j - 1]);
                                                if (string.IsNullOrEmpty(data1[j]))
                                                    break;
                                            }
                                        }
                                    }

                                    File.AppendAllLines("data.csv", new[] { String.Join("\n", Artikel) });
                                    //Adding copied TreeViewItem to new WG
                                    i.Items.Add(new TreeViewItem() { Header = ArtikelName });
                                    LoadTVItems();
                                    break;

                                // When DataGridRow is put in another WG
                                case "verschieben":
                                    List<string> data = new List<string>(csvData.Split('\n'));
                                    string WG = "";

                                    if (parent.GetType() == TreeView.GetType()) { WG = selectedTVI.Header.ToString(); }
                                    else { WG = ((TreeViewItem)GetSelectedTreeViewItemParent(selectedTVI)).Header.ToString(); }

                                    for (int j = 0; j < data.Count(); j++)
                                    {
                                        if (!string.IsNullOrEmpty(data[j]) && data[j] == (string)"[" + WG + "]")
                                        {
                                            trigger = 1;
                                            continue;
                                        }
                                        if (trigger == 1)
                                        {
                                            if (data[j].Substring(data[j].IndexOf(",") + 1) == ArtikelName)
                                            {
                                                data[--j] = "[" + i.Header.ToString() + "]";
                                                break;
                                            }
                                        }
                                    }

                                    File.WriteAllLines("data.csv", new[] { String.Join("\n", data) });
                                    //Adding relocated TreeViewItem to new WG
                                    i.Items.Add(new TreeViewItem() { Header = ArtikelName });
                                    //Deleting TVI from old WG
                                    foreach (TreeViewItem y in selectedTVI.Items) if (y.Header.ToString() == ArtikelName) { selectedTVI.Items.Remove(y); break; }
                                    LoadTVItems();
                                    break;
                                case "abbrechen":
                                    break;
                            }
                            Globals.high = 0;
                            break;
                        }
                    }

                }
                
                //if (chosenTVI != null) chosenTVI.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFFFFFFF");
            }
        }

        private bool GetMouseTargetRow(Visual theTarget, GetPosition position)
        {
            Rect rect;
            if (theTarget != null)
            {
                rect = VisualTreeHelper.GetDescendantBounds(theTarget);
                System.Windows.Point point = position((IInputElement)theTarget);
                return rect.Contains(point);
            }
            else return false;
        }

        private DataGridRow GetRowItem(int index)
        {
            if (dg3.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) return null;
            return dg3.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
        }

        private void EinheitDataChanged(object sender, SelectionChangedEventArgs e)
        {
            if (einheitType != null && bestandTitel != null)
            {
                Thickness m = Bestand.Margin;
                if (Einheit.SelectedIndex == 0) { einheitType.Text = "St."; bestandTitel.Text = "Bestand (St.)"; m.Left = 0; }
                else if (Einheit.SelectedIndex == 1) { einheitType.Text = "Pac."; bestandTitel.Text = "Bestand (Pac.)"; m.Left = -1; }
                else if (Einheit.SelectedIndex == 2) { einheitType.Text = "Kg."; bestandTitel.Text = "Bestand (Kg.)"; m.Left = 0; }
                else if (Einheit.SelectedIndex == 3) { einheitType.Text = "Gr."; bestandTitel.Text = "Bestand (Gr.)"; m.Left = 0; }
                else if (Einheit.SelectedIndex == 4) { einheitType.Text = "Fl."; bestandTitel.Text = "Bestand (Fl.)"; m.Left = 0; }
                else if (Einheit.SelectedIndex == 5) { einheitType.Text = "Kart."; bestandTitel.Text = "Bestand (Kart.)"; }
                else if (Einheit.SelectedIndex == 6) { einheitType.Text = "m"; bestandTitel.Text = "Bestand (m)"; }
                else if (Einheit.SelectedIndex == 7) { einheitType.Text = "zent."; bestandTitel.Text = "Bestand (zent.)"; }
                else if (Einheit.SelectedIndex == 8) { einheitType.Text = "m2"; bestandTitel.Text = "Bestand (m2)"; m.Left = 0; }
                else if (Einheit.SelectedIndex == 9) { einheitType.Text = "l"; bestandTitel.Text = "Bestand (l)"; m.Left = 3; }
                else { einheitType.Text = ""; bestandTitel.Text = "Bestand"; m.Left = 17; }
                Bestand.Margin = m;
            }
        }

        private void SetArtikel_Clicked(object sender, RoutedEventArgs e) { foreach(TreeViewItem t in TreeView.Items) SetArtikel.WGs.Add(t.Header.ToString()); SetArtikel.TV = TreeView; this.Content = new SetArtikel(); }

        private void LoadForm(object sender, RoutedEventArgs e) { LoadTreeViewFromDB(); }

        private int GetCurrentRowIndex(GetPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < dg3.Items.Count; i++)
            {
                DataGridRow itm = GetRowItem(i);
                if (GetMouseTargetRow(itm, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
        }
        //////// DataGridRow Drap&Drop END
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

    // Class responsible for highlighting the TreeViewItem when DG row is dragged over it
    public class TreeViewDropHighlighter
    {
        /// the TreeViewItem that is the current drop target
        private static TreeViewItem _currentItem = null;

        /// Indicates whether the current TreeViewItem is a possible drop target
        public static bool _dropPossible;

        /// Property key (since this is a read-only DP) for the IsPossibleDropTarget property.
        public static readonly DependencyPropertyKey IsPossibleDropTargetKey =
                                    DependencyProperty.RegisterAttachedReadOnly(
                                        "IsPossibleDropTarget",
                                        typeof(bool),
                                        typeof(TreeViewDropHighlighter),
                                        new FrameworkPropertyMetadata(null,
                                            new CoerceValueCallback(CalculateIsPossibleDropTarget)));


        /// Dependency Property IsPossibleDropTarget. Is true if the TreeViewItem is a possible drop target 
        /// (i.e., if it would receive the OnDrop event if the mouse button is released right now).
        public static readonly DependencyProperty IsPossibleDropTargetProperty = IsPossibleDropTargetKey.DependencyProperty;

        /// Getter for IsPossibleDropTarget
        public static bool GetIsPossibleDropTarget(DependencyObject obj) { return (bool)obj.GetValue(IsPossibleDropTargetProperty); }

        /// Coercion method which calculates the IsPossibleDropTarget property.
        private static object CalculateIsPossibleDropTarget(DependencyObject item, object value)
        {
            if ((item == _currentItem) && (_dropPossible))
                return true;
            else
                return false;
        }

        /// Initializes the <see cref="TreeViewDropHighlighter"/> class.
        static TreeViewDropHighlighter()
        {
            // Get all drag enter/leave events for TreeViewItem.
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewDragEnterEvent, new DragEventHandler(OnDragEvent), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewDragLeaveEvent, new DragEventHandler(OnDragLeave), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewDragOverEvent, new DragEventHandler(OnDragEvent), true);
        }

        /// Called when an item is dragged over the TreeViewItem.
        static void OnDragEvent(object sender, DragEventArgs args)
        {
            lock (IsPossibleDropTargetProperty)
            {
                _dropPossible = false;

                if (_currentItem != null)
                {
                    // Tell the item that previously had the mouse that it no longer does.
                    DependencyObject oldItem = _currentItem;
                    _currentItem = null;
                    oldItem.InvalidateProperty(IsPossibleDropTargetProperty);
                }

                if (args.Effects != DragDropEffects.None)
                {
                    _dropPossible = true;
                }

                TreeViewItem tvi = sender as TreeViewItem;
                if (tvi != null)
                {
                    _currentItem = tvi;
                    // Tell that item to re-calculate the IsPossibleDropTarget property
                    _currentItem.InvalidateProperty(IsPossibleDropTargetProperty);
                }
            }
        }

        /// Called when the drag cursor leaves the TreeViewItem
        public static void OnDragLeave(object sender, DragEventArgs args)
        {
            lock (IsPossibleDropTargetProperty)
            {
                _dropPossible = false;

                if (_currentItem != null)
                {
                    // Tell the item that previously had the mouse that it no longer does.
                    DependencyObject oldItem = _currentItem;
                    _currentItem = null;
                    oldItem.InvalidateProperty(IsPossibleDropTargetProperty);
                }

                TreeViewItem tvi = sender as TreeViewItem;
                if (tvi != null)
                {
                    _currentItem = tvi;
                    tvi.InvalidateProperty(IsPossibleDropTargetProperty);
                }
            }
        }

    }
}
