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
        public EanEinstellungen()
        {
            InitializeComponent();

            if (!File.Exists(@"eancodes.csv")) File.Create(@"eancodes.csv").Close();

            string csvData = File.ReadAllText(@"eancodes.csv");
            string line = "";

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
                            //if (line[j] == '[') continue;
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
            
            listOfPreisCodes.ItemsSource = codes;
        }

        public class Code
        {
            public int Prefix { get; set; }
            public string Produktcode { get; set; }
            public string Preis { get; set; }
            public string Bezeichnung { get; set; }
        }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            string l = "";
            
            for (int i = 0; i < listOfPreisCodes.Items.Count-1; i++)
            {
                l = "";
                DataGridRow dataGridRow = (DataGridRow)listOfPreisCodes.ItemContainerGenerator.ContainerFromIndex(i);
                string prefix = (dataGridRow.Item as Code).Prefix.ToString(), produktcode = (dataGridRow.Item as Code).Produktcode,
                preis = (dataGridRow.Item as Code).Preis, bezeichnung = (dataGridRow.Item as Code).Bezeichnung;

                // Check if EAN Code is in correct format
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
            
            if (list != null) File.WriteAllLines("eancodes.csv", new[] { String.Join("\n", list)});
        }

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
                    codes.Remove((Code)listOfPreisCodes.SelectedItem);
                    listOfPreisCodes.ItemsSource = codes;
                    listOfPreisCodes.Items.Refresh();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
            Speichern.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}
