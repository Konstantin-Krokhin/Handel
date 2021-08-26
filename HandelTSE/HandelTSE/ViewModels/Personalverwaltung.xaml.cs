using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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
    /// <summary>
    /// Interaction logic for Personalverwaltung.xaml
    /// </summary>
    public partial class Personalverwaltung
    {
        public static OleDbConnection con = new OleDbConnection();
        public BrushConverter bc = new BrushConverter();
        List<MyData> list = new List<MyData>();
        List<MyData> SubstituteList = new List<MyData>();
        public List<MyData> Data { get; set; }
        Brush brush_red = new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)128, (byte)128));

        OleDbCommand cmd;
        MyData data = new MyData();

        public class MyData
        {
            public int Identyfikator { get; set; }
            public string Name { get; set; }
            public string Login { get; set; }
            public string Passwort { get; set; }
            public string Rabatt { get; set; }
            public string one { get; set; }
            public string two { get; set; }
            public string three { get; set; }
            public string four { get; set; }
            public string five { get; set; }
            public string six { get; set; }
            public string seven { get; set; }
            public string eight { get; set; }
            public string nine { get; set; }
            public string ten { get; set; }
            public string eleven { get; set; }
            public string twelve { get; set; }
            public string thirteen { get; set; }
            public string fourteen { get; set; }
            public string fifteen { get; set; }
            public string sixteen { get; set; }
            public string seventeen { get; set; }
            public string eighteen{ get; set; }
            public string nineteen { get; set; }
            public string twenty { get; set; }
            public string twentyone { get; set; }
        }

        public Personalverwaltung()
        {
            InitializeComponent();

            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

            try { con.Open(); }
            catch
            {
                MessageBoxResult result = MessageBox.Show("Bitte installieren Sie die Microsoft Access Database Engine 2010. Möchten Sie zur Download-Seite weitergeleitet werden?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://www.microsoft.com/en-us/download/confirmation.aspx?id=13255");
                    MessageBox.Show("Nach der Installation des Treibers laden Sie bitte das Menü Personalverwaltung oder den Computer neu, falls erforderlich. ");
                }
                else if (result == MessageBoxResult.No) { MessageBox.Show("Sie müssen den Treiber installieren, um die Daten sehen zu können."); }
            }
            cmd = new OleDbCommand("SELECT Identyfikator, Name, Login, Passwort, Rabatt, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], [21] FROM [TBL_PERSONAL]", con);
            
            LoadGrid();
        }

        private void LoadGrid()
        {
            OleDbDataReader myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["Identyfikator"] != DBNull.Value) data.Identyfikator = (int)myReader["Identyfikator"]; else data.Identyfikator = 0;
                if (myReader["Name"] != DBNull.Value) data.Name = (string)myReader["Name"]; else data.Name = "";
                if (myReader["Login"] != DBNull.Value) data.Login = (string)myReader["Login"]; else data.Login = "";
                if (myReader["Passwort"] != DBNull.Value) data.Passwort = (string)myReader["Passwort"]; else data.Passwort = "";
                if (myReader["Rabatt"] != DBNull.Value) data.Rabatt = (string)myReader["Rabatt"] + "%"; else data.Rabatt = "";
                if (myReader["1"] != DBNull.Value) data.one = (string)myReader["1"] == "ja" ? " X" : " -"; else data.one = "";
                if (myReader["2"] != DBNull.Value) data.two = (string)myReader["2"] == "ja" ? " X" : " -"; else data.two = "";
                if (myReader["3"] != DBNull.Value) data.three = (string)myReader["3"] == "ja" ? " X" : " -"; else data.three = "";
                if (myReader["4"] != DBNull.Value) data.four = (string)myReader["4"] == "ja" ? " X" : " -"; else data.four = "";
                if (myReader["5"] != DBNull.Value) data.five = (string)myReader["5"] == "ja" ? " X" : " -"; else data.five = "";
                if (myReader["6"] != DBNull.Value) data.six = (string)myReader["6"] == "ja" ? " X" : " -"; else data.six = "";
                if (myReader["7"] != DBNull.Value) data.seven = (string)myReader["7"] == "ja" ? " X" : " -"; else data.seven = "";
                if (myReader["8"] != DBNull.Value) data.eight = (string)myReader["8"] == "ja" ? " X" : " -"; else data.eight = "";
                if (myReader["9"] != DBNull.Value) data.nine = (string)myReader["9"] == "ja" ? " X" : " -"; else data.nine = "";
                if (myReader["10"] != DBNull.Value) data.ten = (string)myReader["10"] == "ja" ? " X" : " -"; else data.ten = "";
                if (myReader["11"] != DBNull.Value) data.eleven = (string)myReader["11"] == "ja" ? " X" : " -"; else data.eleven = "";
                if (myReader["12"] != DBNull.Value) data.twelve = (string)myReader["12"] == "ja" ? " X" : " -"; else data.twelve = "";
                if (myReader["13"] != DBNull.Value) data.thirteen = (string)myReader["13"] == "ja" ? " X" : " -"; else data.thirteen = "";
                if (myReader["14"] != DBNull.Value) data.fourteen = (string)myReader["14"] == "ja" ? " X" : " -"; else data.fourteen = "";
                if (myReader["15"] != DBNull.Value) data.fifteen = (string)myReader["15"] == "ja" ? " X" : " -"; else data.fifteen = "";
                if (myReader["16"] != DBNull.Value) data.sixteen = (string)myReader["16"] == "ja" ? " X" : " -"; else data.sixteen = "";
                if (myReader["17"] != DBNull.Value) data.seventeen = (string)myReader["17"] == "ja" ? " X" : " -"; else data.seventeen = "";
                if (myReader["18"] != DBNull.Value) data.eighteen = (string)myReader["18"] == "ja" ? " X" : " -"; else data.eighteen = "";
                if (myReader["19"] != DBNull.Value) data.nineteen = (string)myReader["19"] == "ja" ? " X" : " -"; else data.nineteen = "";
                if (myReader["20"] != DBNull.Value) data.twenty = (string)myReader["20"] == "ja" ? " X" : " -"; else data.twenty = "";
                if (myReader["21"] != DBNull.Value) data.twentyone = (string)myReader["21"] == "ja" ? " X" : " -"; else data.twentyone = "";
                list.Add(new MyData { Identyfikator = data.Identyfikator, Name = data.Name, Login = data.Login, Passwort = data.Passwort, Rabatt = data.Rabatt, one = data.one, two = data.two, three = data.three, four = data.four, five = data.five, six = data.six, seven = data.seven, eight = data.eight, nine = data.nine, ten = data.ten, eleven = data.eleven, twelve = data.twelve, thirteen = data.thirteen, fourteen = data.fourteen, fifteen = data.fifteen, sixteen = data.sixteen, seventeen = data.seventeen, eighteen = data.eighteen, nineteen = data.nineteen, twenty = data.twenty, twentyone = data.twentyone});
            }
            Data = list;
            grid.ItemsSource = Data;
            list = new List<MyData>();
            if (Globals.Training_mode == 1) LimitForTrainingMode();
            if (Globals.Admin_mode == 1) LimitForAdminMode();
        }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            int k = 0;
            if (Name.Text == "") { Name.Background = brush_red; k = 1; }
            if (Login.Text == "") { Login.Background = brush_red; k = 1; }
            if (Passwort.Text == "") { Passwort.Background = brush_red; k = 1; }

            // Checking on whether the password exists in DB
            OleDbCommand cmd = new OleDbCommand("SELECT COUNT(1) FROM TBL_PERSONAL WHERE Passwort = @Passwort", con);
            cmd.Parameters.Add(new OleDbParameter("@Passwort", Passwort.Text));
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            if (result > 0) { PasswordWarning.Visibility = Visibility.Visible; k = 1; }
            else { PasswordWarning.Visibility = Visibility.Hidden; }

            if (k == 1) return;

            if (grid.SelectedItem == null) SaveToDB();
            else UpdateDB();
        }

        void UpdateDB()
        {
            var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(grid.SelectedIndex);
            var item = row.Item as MyData;

            OleDbCommand cmd = new OleDbCommand("UPDATE [TBL_PERSONAL] SET Name = @Name, Login = @Login, Passwort = @Passwort, Rabatt = @Rabatt, 1 = @1, 2 = @2, 3 = @3, 4 = @4, 5 = @5, 6 = @6, 7 = @7, 8 = @8, 9 = @9, 10 = @10, 11 = @11, 12 = @12, 13 = @13, 14 = @14, 15 = @15, 16 = @16, 17 = @17, 18 = @18, 19 = @19, 20 = @20, 21 = @21 WHERE Identyfikator = @ID", con);

            cmd.Parameters.Add(new OleDbParameter("@Name", Name.Text));
            cmd.Parameters.Add(new OleDbParameter("@Login", Login.Text));
            cmd.Parameters.Add(new OleDbParameter("@Passwort", Passwort.Text));
            cmd.Parameters.Add(new OleDbParameter("@Rabatt", Rabatt.Text));
            cmd.Parameters.Add(new OleDbParameter("@1", Storno.Text));
            cmd.Parameters.Add(new OleDbParameter("@2", Warenverwaltung.Text));
            cmd.Parameters.Add(new OleDbParameter("@3", Artikelrabatt.Text));
            cmd.Parameters.Add(new OleDbParameter("@4", Gutschein.Text));
            cmd.Parameters.Add(new OleDbParameter("@5", GutscheinStorno.Text));
            cmd.Parameters.Add(new OleDbParameter("@6", ZAbschlag.Text));
            cmd.Parameters.Add(new OleDbParameter("@7", SofortStorno.Text));
            cmd.Parameters.Add(new OleDbParameter("@8", PlusMinusFunk.Text));
            cmd.Parameters.Add(new OleDbParameter("@9", LetzterBon.Text));
            cmd.Parameters.Add(new OleDbParameter("@10", Office.Text));
            cmd.Parameters.Add(new OleDbParameter("@11", EinAusnahme.Text));
            cmd.Parameters.Add(new OleDbParameter("@12", Einstellungen.Text));
            cmd.Parameters.Add(new OleDbParameter("@13", Buchhaltung.Text));
            cmd.Parameters.Add(new OleDbParameter("@14", XAbschlag.Text));
            cmd.Parameters.Add(new OleDbParameter("@15", Kassenlade.Text));
            cmd.Parameters.Add(new OleDbParameter("@16", AdminStorno.Text));
            cmd.Parameters.Add(new OleDbParameter("@17", Warenbestand.Text));
            cmd.Parameters.Add(new OleDbParameter("@18", Inventur.Text));
            cmd.Parameters.Add(new OleDbParameter("@19", PreisF6.Text));
            cmd.Parameters.Add(new OleDbParameter("@20", Berichte.Text));
            cmd.Parameters.Add(new OleDbParameter("@21", Wareneingang.Text));
            cmd.Parameters.Add(new OleDbParameter("@ID", item.Identyfikator));

            //"UPDATE [TBL_PERSONAL] SET (Name, Login, Passwort, Rabatt, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], [21])Values('" + Name.Text + "','" + Login.Text + "','" + Passwort.Text + "','" + Rabatt.Text + "','" + Storno.Text + "','" + Warenverwaltung.Text + "','" + Artikelrabatt.Text + "','" + Gutschein.Text + "','" + GutscheinStorno.Text + "','" + ZAbschlag.Text + "','" + SofortStorno.Text + "','" + PlusMinusFunk.Text + "','" + LetzterBon.Text + "','" + Office.Text + "','" + EinAusnahme.Text + "','" + Einstellungen.Text + "','" + Buchhaltung.Text + "','" + XAbschlag.Text + "','" + Kassenlade.Text + "','" + AdminStorno.Text + "','" + Warenbestand.Text + "','" + Inventur.Text + "','" + PreisF6.Text + "','" + Berichte.Text + "','" + Wareneingang.Text + "') WHERE Identyfikator = " + item.Identyfikator
            int result = 0;
            try { result = cmd.ExecuteNonQuery(); }
            catch
            { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

            LoadGrid();
            HideColumns();
            
            if (result > 0) MessageBox.Show("Ihre Änderungen wurden erfolgreich aktualisiert!");
        }

        void SaveToDB()
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = "insert into [TBL_PERSONAL](Name, Login, Passwort, Rabatt, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], [21])Values('" + Name.Text + "','" + Login.Text + "','" + Passwort.Text + "','" + Rabatt.Text + "','" + Storno.Text + "','" + Warenverwaltung.Text + "','" + Artikelrabatt.Text + "','" + Gutschein.Text + "','" + GutscheinStorno.Text + "','" + ZAbschlag.Text + "','" + SofortStorno.Text + "','" + PlusMinusFunk.Text + "','" + LetzterBon.Text + "','" + Office.Text + "','" + EinAusnahme.Text + "','" + Einstellungen.Text + "','" + Buchhaltung.Text + "','" + XAbschlag.Text + "','" + Kassenlade.Text + "','" + AdminStorno.Text + "','" + Warenbestand.Text + "','" + Inventur.Text + "','" + PreisF6.Text + "','" + Berichte.Text + "','" + Wareneingang.Text + "')";
            cmd.Connection = con;

            int result = 0;
            try { result = cmd.ExecuteNonQuery(); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

            LoadGrid();
            HideColumns();
            if (result > 0) MessageBox.Show("Ihre Änderungen wurden erfolgreich gespeichert!");
        }

        private void Loschen_Click(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItem == null) { MessageBox.Show("Bitte wählen Sie den Datensatz in der Tabelle aus!"); return; }
            var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(grid.SelectedIndex);
            var item = row.Item as MyData;

            OleDbCommand cmd = new OleDbCommand("DELETE FROM [TBL_PERSONAL] where Identyfikator = @ID", con);
            cmd.Parameters.Add(new OleDbParameter("@ID", item.Identyfikator));

            int result = 0;
            try { result = cmd.ExecuteNonQuery(); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

            LoadGrid();
            HideColumns();
            if (result > 0) MessageBox.Show("Ihre Änderungen wurden erfolgreich erloscht!");
        }

        private void HideColumns() { if (grid.Items.Count > 0) grid.Columns[0].Visibility = Visibility.Collapsed; grid.Columns[3].Visibility = Visibility.Collapsed; }

        private void gridLoaded(object sender, RoutedEventArgs e) { HideColumns(); }

        private void CustomizeHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "one") e.Column.Header = "1.";
            if (e.Column.Header.ToString() == "two") e.Column.Header = "2.";
            if (e.Column.Header.ToString() == "three") e.Column.Header = "3.";
            if (e.Column.Header.ToString() == "four") e.Column.Header = "4.";
            if (e.Column.Header.ToString() == "five") e.Column.Header = "5.";
            if (e.Column.Header.ToString() == "six") e.Column.Header = "6.";
            if (e.Column.Header.ToString() == "seven") e.Column.Header = "7.";
            if (e.Column.Header.ToString() == "eight") e.Column.Header = "8.";
            if (e.Column.Header.ToString() == "nine") e.Column.Header = "9.";
            if (e.Column.Header.ToString() == "ten") e.Column.Header = "10.";
            if (e.Column.Header.ToString() == "eleven") e.Column.Header = "11.";
            if (e.Column.Header.ToString() == "twelve") e.Column.Header = "12.";
            if (e.Column.Header.ToString() == "thirteen") e.Column.Header = "13.";
            if (e.Column.Header.ToString() == "fourteen") e.Column.Header = "14.";
            if (e.Column.Header.ToString() == "fifteen") e.Column.Header = "15.";
            if (e.Column.Header.ToString() == "sixteen") e.Column.Header = "16.";
            if (e.Column.Header.ToString() == "seventeen") e.Column.Header = "17.";
            if (e.Column.Header.ToString() == "eighteen") e.Column.Header = "18.";
            if (e.Column.Header.ToString() == "nineteen") e.Column.Header = "19.";
            if (e.Column.Header.ToString() == "twenty") e.Column.Header = "20.";
            if (e.Column.Header.ToString() == "twentyone") e.Column.Header = "21.";
        }

        private void NameTextChanged(object sender, TextChangedEventArgs e) { if (Name.Background == brush_red) Name.Background = Brushes.White; }

        private void LoginTextChanged(object sender, TextChangedEventArgs e) { if (Login.Background == brush_red) Login.Background = Brushes.White; }

        private void PasswordTextChanged(object sender, TextChangedEventArgs e) { if (Passwort.Background == brush_red) Passwort.Background = Brushes.White; }

        private void RecordSelected(object sender, SelectionChangedEventArgs e)
        {
            if (grid.SelectedItem == null) return;
            var dg = sender as System.Windows.Controls.DataGrid;
            if (dg == null) return;
            var row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
            if (row == null) return;

            Name.Text = ((MyData)row.Item).Name;
            Login.Text = ((MyData)row.Item).Login;
            Passwort.Text = ((MyData)row.Item).Passwort;
            Rabatt.Text = ((MyData)row.Item).Rabatt != "" ? ((MyData)row.Item).Rabatt.Substring(0, ((MyData)row.Item).Rabatt.IndexOf("%")) : "";
            Storno.SelectedIndex = ((MyData)row.Item).one == " X" ?  1 : 0;
            Warenverwaltung.SelectedIndex = ((MyData)row.Item).two == " X" ? 1 : 0;
            Artikelrabatt.SelectedIndex = ((MyData)row.Item).three == " X" ? 1 : 0;
            Gutschein.SelectedIndex = ((MyData)row.Item).four == " X" ? 1 : 0;
            GutscheinStorno.SelectedIndex = ((MyData)row.Item).five == " X" ? 1 : 0;
            ZAbschlag.SelectedIndex = ((MyData)row.Item).six == " X" ? 1 : 0;
            SofortStorno.SelectedIndex = ((MyData)row.Item).seven == " X" ? 1 : 0;
            PlusMinusFunk.SelectedIndex = ((MyData)row.Item).eight == " X" ? 1 : 0;
            LetzterBon.SelectedIndex = ((MyData)row.Item).nine == " X" ? 1 : 0;
            Office.SelectedIndex = ((MyData)row.Item).ten == " X" ? 1 : 0;
            EinAusnahme.SelectedIndex = ((MyData)row.Item).eleven == " X" ? 1 : 0;
            Einstellungen.SelectedIndex = ((MyData)row.Item).twelve == " X" ? 1 : 0;
            Buchhaltung.SelectedIndex = ((MyData)row.Item).thirteen == " X" ? 1 : 0;
            XAbschlag.SelectedIndex = ((MyData)row.Item).fourteen == " X" ? 1 : 0;
            Kassenlade.SelectedIndex = ((MyData)row.Item).fifteen == " X" ? 1 : 0;
            AdminStorno.SelectedIndex = ((MyData)row.Item).sixteen == " X" ? 1 : 0;
            Warenbestand.SelectedIndex = ((MyData)row.Item).seventeen == " X" ? 1 : 0;
            Inventur.SelectedIndex = ((MyData)row.Item).eighteen == " X" ? 1 : 0;
            PreisF6.SelectedIndex = ((MyData)row.Item).nineteen == " X" ? 1 : 0;
            Berichte.SelectedIndex = ((MyData)row.Item).twenty == " X" ? 1 : 0;
            Wareneingang.SelectedIndex = ((MyData)row.Item).twentyone == " X" ? 1 : 0;
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            Login.Text = "";
            Name.Text = "";
            Passwort.Text = "";
            Rabatt.Text = "0";
            if (Name.Background == brush_red) Name.Background = Brushes.White;
            if (Login.Background == brush_red) Login.Background = Brushes.White;
            if (Passwort.Background == brush_red) Passwort.Background = Brushes.White;
            OfficeStartenCheck.IsChecked = false;
            foreach (ComboBox cb in Artikelverwaltung.FindVisualChildren<ComboBox>(this)) { cb.SelectedIndex = 0; }
            grid.SelectedItem = null;
        }

        // Disabling all controls in case user ID = 1 (Training mode)
        private void LimitForTrainingMode()
        {
            foreach (TextBox cb in Artikelverwaltung.FindVisualChildren<TextBox>(MainPanel)) { cb.IsEnabled = false; }
            foreach (ComboBox cb in Artikelverwaltung.FindVisualChildren<ComboBox>(BerechtigungenPanel)) { cb.IsEnabled = false; }
            foreach (Button cb in Artikelverwaltung.FindVisualChildren<Button>(BerechtigungenPanel)) { cb.IsEnabled = false; }
            foreach (CheckBox cb in Artikelverwaltung.FindVisualChildren<CheckBox>(MainPanel)) { cb.IsEnabled = false; }
            Rabatt.IsEnabled = false;
        }

        // Disabling some controls in case user ID = 2 (Admin mode)
        private void LimitForAdminMode()
        {
            foreach (ComboBox cb in Artikelverwaltung.FindVisualChildren<ComboBox>(BerechtigungenPanel)) { cb.IsEnabled = false; }
            DeleteButton.IsEnabled = false;
        }
    }
}
