using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
    public partial class Sondernzeichen : UserControl
    {
        Sondern data = new Sondern();
        List<Sondern> list = new List<Sondern>();
        public static SQLiteConnection con;
        public List<Sondern> Data { get; set; }
        public class Sondern
        {
            public int Id { get; set; }
            public string Sondernzeichen { get; set; }
            public string Code { get; set; }
        }
        public Sondernzeichen()
        {
            InitializeComponent();

            // If the menu Sondern is being open multiple times
            if (con == null)
            {
                con = MainWindow.con;
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE [TBL_Sondernzeichen] ([Id] COUNTER, [Sondernzeichen] TEXT(55), [Code] TEXT(55))", con);
                try
                {
                    cmd.ExecuteNonQuery();
                    //Create first empty record with the proper starting ID starting from 1
                    SQLiteCommand cmd2 = new SQLiteCommand("insert into [TBL_Sondernzeichen](Id, Sondernzeichen, Code)Values('" + 1 + "','" + "" + "','" + "" + "')", con);
                    try { cmd2.ExecuteNonQuery(); }
                    catch { }
                }
                catch { }
            }

            if (MainWindow.con.State != System.Data.ConnectionState.Closed) LoadGrid();
        }

        private void SondernzeichenDataGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SondernzeichenDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SignTextBox.Visibility = Visibility.Visible;
            CodeTextBox.Visibility = Visibility.Visible;
            speichernButton.Visibility = Visibility.Visible;

            Sondern item = null;
            if (SondernzeichenDataGrid.SelectedItem != null) item = ((Sondern)SondernzeichenDataGrid.SelectedItem);
            if (item != null)
            {
                SignTextBox.Text = item.Sondernzeichen;
                CodeTextBox.Text = item.Code;
                loschenButton.Visibility = Visibility.Visible;
            }
        }

        private void neuButton_Click(object sender, RoutedEventArgs e)
        {
            SignTextBox.Visibility = Visibility.Visible;
            CodeTextBox.Visibility = Visibility.Visible;
            loschenButton.Visibility = Visibility.Hidden;
            speichernButton.Visibility = Visibility.Visible;
            SondernzeichenDataGrid.SelectedItem = null;
            SignTextBox.Text = "";
            CodeTextBox.Text = "";
        }

        private void speichernButton_Clicked(object sender, RoutedEventArgs e)
        {
            Int32 ID = 1;
            Sondern item = null;
            if (SondernzeichenDataGrid.SelectedItem != null) item = ((Sondern)SondernzeichenDataGrid.SelectedItem);
            SQLiteCommand cmd = new SQLiteCommand();
            if (item != null)
            {
                ID = item.Id;
                cmd = new SQLiteCommand("UPDATE [TBL_Sondernzeichen] SET Sondernzeichen = @Sondernzeichen, Code = @Code WHERE Id = @ID", con);

                cmd.Parameters.Add(new SQLiteParameter("@Code", CodeTextBox.Text));
                cmd.Parameters.Add(new SQLiteParameter("@Sondernzeichen", SignTextBox.Text));
                cmd.Parameters.Add(new SQLiteParameter("@ID", ID));
            }
            else
            {
                SQLiteCommand maxCommand = new SQLiteCommand("SELECT max(Id) from TBL_Sondernzeichen", con);
                try { object val = maxCommand.ExecuteScalar(); ID = int.Parse(val.ToString()) + 1; } catch { }
                cmd = new SQLiteCommand("insert into [TBL_Sondernzeichen](Id, Sondernzeichen, Code)Values('" + ID + "','" + SignTextBox.Text + "','" + CodeTextBox.Text + "')", con);
            }

            try { int result = cmd.ExecuteNonQuery(); LoadGrid(); }
            catch { MessageBox.Show("DB error!"); }

            SignTextBox.Visibility = Visibility.Hidden;
            CodeTextBox.Visibility = Visibility.Hidden;
            loschenButton.Visibility = Visibility.Hidden;
            speichernButton.Visibility = Visibility.Hidden;
        }

        void LoadGrid()
        {
            SQLiteCommand cmd2 = new SQLiteCommand("SELECT * FROM [TBL_Sondernzeichen]", con);
            SQLiteDataReader myReader = cmd2.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["Id"] != DBNull.Value) data.Id = int.Parse(myReader["Id"].ToString()); else data.Id = -1;
                if (myReader["Sondernzeichen"] != DBNull.Value) data.Sondernzeichen = (string)myReader["Sondernzeichen"]; else data.Sondernzeichen = "";
                if (myReader["Code"] != DBNull.Value) data.Code = (string)myReader["Code"]; else data.Code = "";
                list.Add(new Sondern { Id = data.Id, Sondernzeichen = data.Sondernzeichen, Code = data.Code });
            }
            Data = list;
            SondernzeichenDataGrid.ItemsSource = Data;
            list = new List<Sondern>();
        }

        private void loschenButton_Click(object sender, RoutedEventArgs e)
        {
            if (SondernzeichenDataGrid.SelectedItem == null) { MessageBox.Show("Bitte wählen Sie den Datensatz in der Tabelle aus!"); return; }

            string caption = "Datensatz entfernen...";
            string messageBoxText = "Wollen Sie Stornogrund wirklich entfernen?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM [TBL_Sondernzeichen] where Id = @ID", con);
                    cmd.Parameters.Add(new SQLiteParameter("@ID", ((Sondern)SondernzeichenDataGrid.SelectedItem).Id));
                    int result = 0;
                    try { result = cmd.ExecuteNonQuery(); }
                    catch { MessageBox.Show("DB error!"); }

                    LoadGrid();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
            loschenButton.Visibility = Visibility.Hidden;
        }
    }
}
