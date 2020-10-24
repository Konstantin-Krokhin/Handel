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

namespace HandelTSE.ViewModels
{
    public partial class Artikelverwaltung
    {
        public List<items> Data { get; set; }
        List<items> it = new List<items>();
        public Artikelverwaltung()
        {
            InitializeComponent();
            for (int i = 0; i < 30; i++) it.Add(new items { });
            Data = it;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var lines = new List<string>();

            IEnumerator<TextBox> a = (IEnumerator<TextBox>)FindVisualChildren<TextBox>(this);
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
                
            System.IO.File.WriteAllLines(@"data.csv", lines);
        }
    }
}
