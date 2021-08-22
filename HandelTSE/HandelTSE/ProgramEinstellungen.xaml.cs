using HandelTSE.ViewModels;
using System;
using System.Collections.Generic;
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

namespace HandelTSE
{
    /// <summary>
    /// Interaction logic for ProgramEinstellungen.xaml
    /// </summary>
    public partial class ProgramEinstellungen : UserControl
    {

        List<items> it = new List<items>();
        BrushConverter converter = new System.Windows.Media.BrushConverter();
        Brush brush_red = new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)128, (byte)128));

        public class items
        {
            public string Terminal_ID { get; set; }
            public string Marke { get; set; }
        }
        public ProgramEinstellungen()
        {
            InitializeComponent();
        }

        private void cp1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cp1.SelectedColor.HasValue)
            {
                Color C = cp1.SelectedColor.Value;
                long colorVal = Convert.ToInt64(C.B * (Math.Pow(256, 0)) + C.G * (Math.Pow(256, 1)) + C.R * (Math.Pow(256, 2)));

                string colorString = cp1.SelectedColor.Value.ToString();

                Farbauswahl1.Style = (Style)FindResource("BlueButton");
                Farbauswahl1.Background = (Brush)converter.ConvertFromString(colorString);
            }
            cp1.Visibility = Visibility.Collapsed;
        }

        private void cp2_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cp2.SelectedColor.HasValue)
            {
                Color C = cp2.SelectedColor.Value;
                long colorVal = Convert.ToInt64(C.B * (Math.Pow(256, 0)) + C.G * (Math.Pow(256, 1)) + C.R * (Math.Pow(256, 2)));

                string colorString = cp2.SelectedColor.Value.ToString();

                Farbauswahl2.Style = (Style)FindResource("BlueButton");
                Farbauswahl2.Background = (Brush)converter.ConvertFromString(colorString);
            }
            cp2.Visibility = Visibility.Collapsed;
        }

        private void Color1Change(object sender, RoutedEventArgs e) { if (cp1.IsOpen == false) cp1.IsOpen = true; else cp1.IsOpen = false; }
        private void Color2Change(object sender, RoutedEventArgs e) { if (cp2.IsOpen == false) cp2.IsOpen = true; else cp2.IsOpen = false; }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CustomizeHeaders(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e) { if (e.Column.Header.ToString() == "Terminal_ID") e.Column.Header = "Terminal-ID"; }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            int k = 0;
            foreach (TextBox tb in Artikelverwaltung.FindVisualChildren<TextBox>(TerminalIDPanel)) { if (tb.Text == "") { tb.Background = brush_red; k = 1; } }

            if (k == 1) return;
        }

        private void TextChanged(object sender, TextChangedEventArgs e) { if (((TextBox)sender).Background == brush_red) ((TextBox)sender).Background = Brushes.White; }
    }
}
