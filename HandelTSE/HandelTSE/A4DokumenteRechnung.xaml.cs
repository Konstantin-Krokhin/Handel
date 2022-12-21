using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class A4DokumenteRechnung : UserControl
    {

        public A4DokumenteRechnung()
        {
            InitializeComponent();
        }

        private void ObenUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (ObenTextBox.Text == "") ObenTextBox.Text = "0";
            if (ObenTextBox.Text != "29.5")
            {
                double Top = 0;
                Canvas.SetTop(CanvasRectangle, ++Top);
                double Oben = double.Parse(ObenTextBox.Text, System.Globalization.CultureInfo.InvariantCulture);
                Oben += 0.1;
                ObenTextBox.Text = Oben.ToString();
            }
        }

        private void ObenDownButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UntenUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (UntenTextBox.Text == "") UntenTextBox.Text = "0";
            if (UntenTextBox.Text != "29.5")
            {
                CanvasRectangle.Height--;
                double Unten = double.Parse(UntenTextBox.Text, System.Globalization.CultureInfo.InvariantCulture);
                Unten += 0.1;
                UntenTextBox.Text = Unten.ToString();
            }
        }

        private void UntenDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (UntenTextBox.Text == "") UntenTextBox.Text = "0";
            if (UntenTextBox.Text != "0")
            {
                CanvasRectangle.Height++;
                double Unten = double.Parse(UntenTextBox.Text, System.Globalization.CultureInfo.InvariantCulture);
                Unten -= 0.1;
                UntenTextBox.Text = Unten.ToString();
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (double.Parse(UntenTextBox.Text, System.Globalization.CultureInfo.InvariantCulture) < 0 || double.Parse(UntenTextBox.Text, System.Globalization.CultureInfo.InvariantCulture) > 29.5) return;
                if (CanvasRectangle.Height + double.Parse(UntenTextBox.Text, System.Globalization.CultureInfo.InvariantCulture) * 10 > 296) CanvasRectangle.Height -= Math.Abs(296 - (CanvasRectangle.Height + double.Parse(UntenTextBox.Text, System.Globalization.CultureInfo.InvariantCulture) * 10));
                else CanvasRectangle.Height += Math.Abs(296 - (CanvasRectangle.Height + double.Parse(UntenTextBox.Text, System.Globalization.CultureInfo.InvariantCulture) * 10));
            }
        }

        
    }

    
}
