using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace HandelTSE.ViewModels
{
    public partial class Artikelverwaltung
    {
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
                var DGdata = dataGridRow.Item as items;
                var ArtikelName = DGdata.artikel;
                DragDropEffects dragdropeffects = DragDropEffects.Move;
                if (DragDrop.DoDragDrop(dg3, ArtikelName, dragdropeffects) != DragDropEffects.None)
                {
                    foreach (TreeViewItem i in TreeView.Items)
                    {
                        if (i.Background.ToString() == ((System.Windows.Media.Brush)bc.ConvertFrom("#FF0078D7")).ToString())
                        {
                            if (TreeView.SelectedItem == null || parent.GetType() != typeof(TreeView)) { MessageBox.Show("Bitte wählen Sie die Warengruppe im TreeView-Bereich aus"); return; }
                            string messageBoxText = "Eine Artikel aus Warengruppe " + "'" + selectedTVI.Header.ToString() + "'" + " in die Warengruppe " + "'" + i.Header.ToString() + "'";
                            chosenTVI = i;
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
                            break;
                        }
                    }

                }
                if (chosenTVI != null) chosenTVI.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFFFFF");
            }
        }

        private bool GetMouseTargetRow(Visual theTarget, GetPosition position)
        {
            Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
            System.Windows.Point point = position((IInputElement)theTarget);
            return rect.Contains(point);
        }

        private DataGridRow GetRowItem(int index)
        {
            if (dg3.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) return null;
            return dg3.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
        }

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
}
