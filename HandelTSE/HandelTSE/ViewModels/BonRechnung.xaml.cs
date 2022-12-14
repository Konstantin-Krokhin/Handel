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
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE [TBL_BonRechnung] ([Id] COUNTER, [TextAlignment1] TEXT(55), [TextAlignment2] TEXT(55), [TitleTextBox] TEXT(60), [KopfzeileTextBox] TEXT(555), [BoninhaltTextBox] TEXT(555), [FusszeileTextBox] TEXT(555))", con);
                try
                {
                    cmd.ExecuteNonQuery();
                    //Create first empty record with the proper starting ID starting from 1
                    SQLiteCommand cmd2 = new SQLiteCommand("insert into [TBL_BonRechnung](Id, TextAlignment1, TextAlignment2, TitleTextBox, KopfzeileTextBox, BoninhaltTextBox, FusszeileTextBox)Values('" + 1 + "','" + "C" + "','" + "C" + "','" + TitleTextBox.Text + "','" + KopfzeileTextBox.Text + "','" + BoninhaltTextBox.Text + "','" + FusszeileTextBox.Text + "')", con);
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
                if (myReader["TextAlignment1"] != DBNull.Value) if ((string)myReader["TextAlignment1"] == "L") KopfzeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Left; else if ((string)myReader["TextAlignment1"] == "C") KopfzeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                if (myReader["TextAlignment2"] != DBNull.Value) if ((string)myReader["TextAlignment2"] == "L") FusszeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Left; else if ((string)myReader["TextAlignment2"] == "C") FusszeileTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                if (myReader["TitleTextBox"] != DBNull.Value) TitleTextBox.Text = (string)myReader["TitleTextBox"]; else TitleTextBox.Text = "";
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
            SQLiteCommand cmd = new SQLiteCommand("UPDATE [TBL_BonRechnung] SET TextAlignment1 = @TextAlignment1, TextAlignment2 = @TextAlignment2, TitleTextBox = @TitleTextBox, KopfzeileTextBox = @KopfzeileTextBox, BoninhaltTextBox = @BoninhaltTextBox, FusszeileTextBox = @FusszeileTextBox WHERE Id = @ID", con);

            if (KopfzeileTextBox.HorizontalContentAlignment == HorizontalAlignment.Left) cmd.Parameters.Add(new SQLiteParameter("@TextAlignment1", "L"));
            if (KopfzeileTextBox.HorizontalContentAlignment == HorizontalAlignment.Center) cmd.Parameters.Add(new SQLiteParameter("@TextAlignment1", "C"));
            if (FusszeileTextBox.HorizontalContentAlignment == HorizontalAlignment.Left) cmd.Parameters.Add(new SQLiteParameter("@TextAlignment2", "L"));
            if (FusszeileTextBox.HorizontalContentAlignment == HorizontalAlignment.Center) cmd.Parameters.Add(new SQLiteParameter("@TextAlignment2", "C"));
            cmd.Parameters.Add(new SQLiteParameter("@TitleTextBox", TitleTextBox.Text));
            cmd.Parameters.Add(new SQLiteParameter("@KopfzeileTextBox", KopfzeileTextBox.Text));
            cmd.Parameters.Add(new SQLiteParameter("@BoninhaltTextBox", BoninhaltTextBox.Text));
            cmd.Parameters.Add(new SQLiteParameter("@FusszeileTextBox", FusszeileTextBox.Text));
            cmd.Parameters.Add(new SQLiteParameter("@ID", 1));

            try { int result = cmd.ExecuteNonQuery(); LoadData(); }
            catch { MessageBox.Show("DB error!"); }
        }
    }
}
