using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for EanEinstellungen.xaml
    /// </summary>
    public partial class EanEinstellungen : UserControl
    {
        List<Code> codes = new List<Code>();
        List<Code> preisCodes = new List<Code>();
        List<Code> gewichtCodes = new List<Code>();
        List<Code> mengeCodes = new List<Code>();
        public EanEinstellungen()
        {
            InitializeComponent();

            // Create all required db files for each EAN code grid
            if (!File.Exists(@"eancodes_preis.csv")) File.Create(@"eancodes_preis.csv").Close();
            if (!File.Exists(@"eancodes_gewicht.csv")) File.Create(@"eancodes_gewicht.csv").Close();
            if (!File.Exists(@"eancodes_menge.csv")) File.Create(@"eancodes_menge.csv").Close();

            string csvData = File.ReadAllText(@"eancodes_preis.csv");
            string line = "";

            for (int o = 0; o < 3; o++)
            {
                if (o == 1) csvData = File.ReadAllText(@"eancodes_gewicht.csv");
                else if (o == 2) csvData = File.ReadAllText(@"eancodes_menge.csv");
                List<string> data = new List<string>(csvData.Split('\n'));
                for (int i = 0; i < data.Count(); i++)
                {
                    string[] codeExamples = new string[4];
                    if (data[i].Contains("["))
                    {
                        int j = 1;
                        for (int k = 0; k < 4; k++)
                            for (; j < data[i].Length; j++)
                            {
                                line = data[i];
                                if (line[j] != ',' && line[j] != ']')
                                    codeExamples[k] += line[j];
                                else
                                {
                                    j++;
                                    break;
                                }
                            }
                        codes.Add(new Code { Prefix = Int32.Parse(codeExamples[0]), Produktcode = codeExamples[1], Preis = codeExamples[2], Bezeichnung = codeExamples[3] });
                    }
                }
                if (o == 0) { preisCodes.AddRange(codes); listOfPreisCodes.ItemsSource = preisCodes; }
                else if (o == 1) { gewichtCodes.AddRange(codes); listOfGewichtCodes.ItemsSource = gewichtCodes; }
                else if (o == 2) { mengeCodes.AddRange(codes); listOfMengeCodes.ItemsSource = mengeCodes; }
                codes.Clear();
            }
        }

        public class Code
        {
            public int Prefix { get; set; }
            public string Produktcode { get; set; }
            public string Preis { get; set; }
            public string Bezeichnung { get; set; }
        }

        // Universal click event for saving currently editable grid into the file
        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            string l = "", savePath = "";
            TabItem ti = EanTabs.SelectedItem as TabItem;
            DataGrid dg = new DataGrid();
            if (ti.Name == "PreisTabItem") { dg = listOfPreisCodes; savePath = "eancodes_preis.csv"; EntfernenPreis.IsEnabled = false; }
            else if (ti.Name == "GewichtTabItem") { dg = listOfGewichtCodes; savePath = "eancodes_gewicht.csv"; EntfernenGewicht.IsEnabled = false; }
            else if (ti.Name == "MengeTabItem") { dg = listOfMengeCodes; savePath = "eancodes_menge.csv"; EntfernenMenge.IsEnabled = false; }

            for (int i = 0; i < dg.Items.Count-1; i++)
            {
                l = "";
                DataGridRow dataGridRow = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(i);
                string prefix = (dataGridRow.Item as Code).Prefix.ToString(), produktcode = (dataGridRow.Item as Code).Produktcode,
                preis = (dataGridRow.Item as Code).Preis, bezeichnung = (dataGridRow.Item as Code).Bezeichnung;

                // Check if EAN Code is in correct format
                if (prefix == null || produktcode == null || preis == null || bezeichnung == null) { MessageBox.Show("Prüfen Sie Ihre Daten!\n----------------\nPrefix: 1-2 Zeichen\nProduktcode: 5-6 Zeichen\nPreis: 4-6 Zeichen\n----------------\nGesamt: 12 Zeichen"); list = null; break; }
                if ((prefix.Length == 1 || prefix.Length == 2) && (produktcode.Length == 5 || produktcode.Length == 6) && (preis.Length >= 4 && preis.Length <= 6) && (prefix.Length + produktcode.Length + preis.Length == 12))
                {
                    l += "[" + prefix + ",";
                    l += produktcode + ",";
                    l += preis + ",";
                    l += bezeichnung + "]";
                    list.Add(l);
                }
                else
                {
                    MessageBox.Show("Prüfen Sie Ihre Daten!\n----------------\nPrefix: 1-2 Zeichen\nProduktcode: 5-6 Zeichen\nPreis: 4-6 Zeichen\n----------------\nGesamt: 12 Zeichen");
                    list = null;
                    break;
                }
            }

            if (list != null) File.WriteAllLines(savePath, new[] { String.Join("\n", list) });
        }

        // Universal event. Deletes element from the grid. It is required to click "Speichern" button (to call Speichern event) afterwards to save into the file.
        private void Entfernen_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Wollen Sie wirklich entfernen?";
            string caption = "Datensatz entfernen";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (result)
            {
                case MessageBoxResult.OK:
                    TabItem ti = EanTabs.SelectedItem as TabItem;
                    DataGrid dg = new DataGrid();
                    if (ti.Name == "PreisTabItem") 
                    { 
                        dg = listOfPreisCodes;
                        preisCodes.Remove((Code)dg.SelectedItem);
                        dg.ItemsSource = preisCodes;
                        EntfernenPreis.IsEnabled = false;
                    }
                    else if (ti.Name == "GewichtTabItem") 
                    { 
                        dg = listOfGewichtCodes;
                        gewichtCodes.Remove((Code)dg.SelectedItem);
                        dg.ItemsSource = gewichtCodes;
                        EntfernenGewicht.IsEnabled = false;
                    }
                    else if (ti.Name == "MengeTabItem") 
                    { 
                        dg = listOfMengeCodes;
                        mengeCodes.Remove((Code)dg.SelectedItem);
                        dg.ItemsSource = mengeCodes;
                        EntfernenMenge.IsEnabled = false;
                    }
                    
                    dg.Items.Refresh();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
            // FIX ! TO SAVE RIGHT AFTER DELETED
            //Speichern.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void Neu_Click(object sender, RoutedEventArgs e) 
        {
            if (EanTabs.SelectedIndex == 0)
            {
                preisCodes.Add(new Code { });
                listOfPreisCodes.ItemsSource = preisCodes;
                EntfernenPreis.IsEnabled = false;
                listOfPreisCodes.Items.Refresh();
            }
            else if (EanTabs.SelectedIndex == 1)
            {
                gewichtCodes.Add(new Code { });
                listOfGewichtCodes.ItemsSource = gewichtCodes;
                EntfernenGewicht.IsEnabled = false;
                listOfGewichtCodes.Items.Refresh();
            }
            else if (EanTabs.SelectedIndex == 2)
            {
                mengeCodes.Add(new Code { });
                listOfMengeCodes.ItemsSource = mengeCodes;
                EntfernenMenge.IsEnabled = false;
                listOfMengeCodes.Items.Refresh();
            }
        }

        private void listOfMengeCodes_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (EntfernenMenge.IsEnabled == false) EntfernenMenge.IsEnabled = true; }

        private void listOfGewichtCodes_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (EntfernenGewicht.IsEnabled == false) EntfernenGewicht.IsEnabled = true; }

        private void listOfPreisCodes_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (EntfernenPreis.IsEnabled == false) EntfernenPreis.IsEnabled = true; }
    }
}
