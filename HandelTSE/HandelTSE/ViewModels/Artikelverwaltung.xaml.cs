using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Artikelverwaltung
    {
        //public List<items> TreeData { get; set; }
        public List<items> Data { get; set; }
        public TreeViewItem selectedTVI { get; set; }
        List<items> it = new List<items>();
        ItemsControl parent { get; set; }
        public Artikelverwaltung()
        {
            InitializeComponent();
            for (int i = 0; i < 30; i++) it.Add(new items { });
            Data = it;
            if (!File.Exists(@"data.csv"))File.Create(@"data.csv");
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


        public class items
        { }

        //Saves ComboBoxes and TextBoxes into the file and cleans the form
        private void SaveButton(object sender, RoutedEventArgs e)
        {
            if (TreeView.SelectedItem == null)
            {
                MessageBox.Show("Bitte Warengruppe anlagen und auswählen!");
                return;
            }
            if (parent.GetType() == typeof(TreeViewItem))
            {
                MessageBox.Show("Sie können keinen Artikel in einem anderen hinzufügen. Es muss eine Warengruppe sein!");
                return;
            }
            if (Artikel.Text == "")
            {
                MessageBox.Show("Bitte geben Sie den Artikelname an!");
                return;
            }//selectedTVI.Parent.GetType()

            //MessageBox.Show(selectedTVI.Parent.GetType().ToString());
            var lines = new List<string>();
            string ArtikelString = Artikel.Text;
            
            foreach (TextBox cb in Artikelverwaltung.FindVisualChildren<TextBox>(this))
            if (cb.Name != "gruppe" & cb.Name != "artikel")
            {
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
            var str = "["+ tvi.Header.ToString() + "]" ;
            File.AppendAllText(@"data.csv", str);
            foreach (string i in lines) File.AppendAllText(@"data.csv", i + "\n");
        }

        private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
        {
            TreeView.Tag = e.OriginalSource;
            selectedTVI = TreeView.Tag as TreeViewItem;

            TreeViewItem item = e.OriginalSource as TreeViewItem;
            if (item != null) parent = GetSelectedTreeViewItemParent(item);
        }

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
        // !!! Add group to file functionality !!!
        private void WarenGruppeSave(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(gruppe.Text))
            {
                TreeViewItem newChild = new TreeViewItem() { Header = gruppe.Text };
                //newChild.Header = gruppe.Text;
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
