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
using System.Data.SQLite;

namespace HandelTSE.ViewModels
{
    /// <summary>
    /// Interaction logic for BonRechnung.xaml
    /// </summary>
    public partial class BonRechnung : UserControl
    {
        public static SQLiteConnection con;

        public BonRechnung()
        {
            InitializeComponent();

            // If the menu Stornogrunde is being open multiple times
            if (con == null)
            {
                con = MainWindow.con;
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE [TBL_BonRechnung] ([Id] COUNTER, [KopfzeileTextBox] TEXT(55), [BoninhaltTextBox] TEXT(55), [FusszeileTextBox] TEXT(55))", con);
                try
                {
                    cmd.ExecuteNonQuery();
                    //Create first empty record with the proper starting ID starting from 1
                    SQLiteCommand cmd2 = new SQLiteCommand("insert into [TBL_BonRechnung](Id, KopfzeileTextBox, BoninhaltTextBox, FusszeileTextBox)Values('" + 1 + "','" + KopfzeileTextBox.Text + "','" + BoninhaltTextBox.Text + "','" + FusszeileTextBox.Text + "')", con);
                    try { cmd2.ExecuteNonQuery(); }
                    catch { }
                }
                catch { }
            }

            if (MainWindow.con.State != System.Data.ConnectionState.Closed) LoadData();

        }

        void LoadData()
        {
            SQLiteCommand cmd2 = new SQLiteCommand("SELECT * FROM [TBL_BonRechnung]", con);
            SQLiteDataReader myReader = cmd2.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["KopfzeileTextBox"] != DBNull.Value) KopfzeileTextBox.Text = (string)myReader["KopfzeileTextBox"]; else KopfzeileTextBox.Text = "";
                if (myReader["BoninhaltTextBox"] != DBNull.Value) BoninhaltTextBox.Text = (string)myReader["BoninhaltTextBox"]; else BoninhaltTextBox.Text = "";
                if (myReader["FusszeileTextBox"] != DBNull.Value) FusszeileTextBox.Text = (string)myReader["FusszeileTextBox"]; else FusszeileTextBox.Text = "";
            }
        }

        private void LeftAlignment1Button_Click(object sender, RoutedEventArgs e)
        {
            KopfzeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Left;
        }

        private void CenterAlignment1Button_Click(object sender, RoutedEventArgs e)
        {
            KopfzeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;
        }

        private void LeftAlignment2Button_Click(object sender, RoutedEventArgs e)
        {
            FusszeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Left;
        }

        private void CenterAlignment2Button_Click(object sender, RoutedEventArgs e)
        {
            FusszeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;
        }

        private void SpeichernButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteCommand cmd = new SQLiteCommand("UPDATE [TBL_BonRechnung] SET KopfzeileTextBox = @KopfzeileTextBox, BoninhaltTextBox = @BoninhaltTextBox, FusszeileTextBox = @FusszeileTextBox WHERE Id = @ID", con);

            cmd.Parameters.Add(new SQLiteParameter("@KopfzeileTextBox", KopfzeileTextBox.Text));
            cmd.Parameters.Add(new SQLiteParameter("@BoninhaltTextBox", BoninhaltTextBox.Text));
            cmd.Parameters.Add(new SQLiteParameter("@FusszeileTextBox", FusszeileTextBox.Text));
            cmd.Parameters.Add(new SQLiteParameter("@ID", 1));

            try { int result = cmd.ExecuteNonQuery(); LoadData(); }
            catch { MessageBox.Show("DB error!"); }
        }
    }
}
