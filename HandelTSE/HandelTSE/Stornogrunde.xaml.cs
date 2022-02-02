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
    /// <summary>
    /// Interaction logic for Stornogrunde.xaml
    /// </summary>
    public partial class Stornogrunde : UserControl
    {
        Storno data = new Storno();
        List<Storno> list = new List<Storno>();
        public static SQLiteConnection con;
        public List<Storno> Data { get; set; }

        public class Storno
        {
            public int Id { get; set; }
            public string Stornogrund { get; set; }
        }
        public Stornogrunde()
        {
            InitializeComponent();

            // If the menu Stornogrunde is being open multiple times
            if (con == null)
            {
                con = MainWindow.con;
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE [TBL_Stornogrunde] ([Id] COUNTER, [Stornogrund] TEXT(55))", con);
                try
                {
                    cmd.ExecuteNonQuery();
                    //Create first empty record with the proper starting ID starting from 1
                    SQLiteCommand cmd2 = new SQLiteCommand("insert into [TBL_Stornogrunde](Id)Values('" + 1 + "')", con);
                    try { cmd2.ExecuteNonQuery(); }
                    catch { }
                }
                catch { }
            }

            if (MainWindow.con.State != System.Data.ConnectionState.Closed) LoadGrid();
        }

        private void LoadGrid()
        {
            SQLiteCommand cmd2 = new SQLiteCommand("SELECT * FROM [TBL_Stornogrunde]", con);
            SQLiteDataReader myReader = cmd2.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["Id"] != DBNull.Value) data.Id = (Int32)myReader["Id"]; else data.Id = -1;
                if (myReader["Stornogrund"] != DBNull.Value) data.Stornogrund = (string)myReader["Stornogrund"]; else data.Stornogrund = "";
                list.Add(new Storno { Id = data.Id, Stornogrund = data.Stornogrund });
            }
            Data = list;
            StornogrundeDataGrid.ItemsSource = Data;
            list = new List<Storno>();
        }

        private void HideColumn() { if (StornogrundeDataGrid.Items.Count > 0 && StornogrundeDataGrid.Columns.Count > 0) { foreach (var item in StornogrundeDataGrid.Columns) { if (item.Header.ToString() == "Id") item.Visibility = Visibility.Collapsed; } StornogrundeDataGrid.Columns[1].Width = 400; } }

        private void Neu_Click(object sender, RoutedEventArgs e) { if (Data != null) { Data.Add(new Storno { }); StornogrundeDataGrid.ItemsSource = Data; StornogrundeDataGrid.Items.Refresh(); StornogrundeDataGrid.SelectedIndex = StornogrundeDataGrid.Items.Count - 1; } NeuButton.IsEnabled = false; EntfernenButton.IsEnabled = false; }

        private void Loschen_Click(object sender, RoutedEventArgs e)
        {
            if (StornogrundeDataGrid.SelectedItem == null) { MessageBox.Show("Bitte wählen Sie den Datensatz in der Tabelle aus!"); return; }

            string caption = "Datensatz entfernen...";
            string messageBoxText = "Wollen Sie Stornogrund wirklich entfernen?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM [TBL_Stornogrunde] where Id = @ID", con);
                    cmd.Parameters.Add(new SQLiteParameter("@ID", ((Storno)StornogrundeDataGrid.SelectedItem).Id));
                    int result = 0;
                    try { result = cmd.ExecuteNonQuery(); }
                    catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

                    LoadGrid();
                    HideColumn();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void speichern_Click(object sender, RoutedEventArgs e)
        {
            Storno item = ((Storno)StornogrundeDataGrid.SelectedItem);
            if (item == null) { MessageBox.Show("Bitte wählen Sie den Datensatz in der Tabelle aus oder erstellen Sie einen neuen Datensatz!"); return; }
            if (item.Stornogrund == null || item.Stornogrund == "") { MessageBox.Show("Überprüfen Sie bitte die Stornogrund-Bezeichnung!"); return; }
            NeuButton.IsEnabled = true;

            Int32 ID = 0;
            int result = 0;
            SQLiteCommand cmd = new SQLiteCommand();
            if (item.Id != 0)
            {
                ID = item.Id;
                cmd = new SQLiteCommand("UPDATE [TBL_Stornogrunde] SET Stornogrund = @Stornogrund WHERE Id = @ID", con);

                cmd.Parameters.Add(new SQLiteParameter("@Stornogrund", item.Stornogrund));
                cmd.Parameters.Add(new SQLiteParameter("@ID", ID));
            }
            else
            {
                SQLiteCommand maxCommand = new SQLiteCommand("SELECT max(Id) from TBL_Stornogrunde", con);
                try { ID = (Int32)maxCommand.ExecuteScalar(); } catch { }
                cmd = new SQLiteCommand("insert into [TBL_Stornogrunde](Id, Stornogrund)Values('" + ++ID + "','" + item.Stornogrund + "')", con);
            }

            try { result = cmd.ExecuteNonQuery(); LoadGrid(); HideColumn(); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
        }

        private void RecordSelected(object sender, SelectionChangedEventArgs e) { if (StornogrundeDataGrid.SelectedItem != null && StornogrundeDataGrid.SelectedIndex != StornogrundeDataGrid.Items.Count - 1 && ((Storno)StornogrundeDataGrid.Items[StornogrundeDataGrid.Items.Count - 1]).Id == 0) { Data.RemoveAt(StornogrundeDataGrid.Items.Count - 1); StornogrundeDataGrid.ItemsSource = Data; StornogrundeDataGrid.Items.Refresh(); NeuButton.IsEnabled = true; } EntfernenButton.IsEnabled = true; }

        private void StornogrundeDataGrid_Loaded(object sender, RoutedEventArgs e) { HideColumn(); }
    }
}
