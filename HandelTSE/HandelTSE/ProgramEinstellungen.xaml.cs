using HandelTSE.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
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
using System.Windows.Threading;

namespace HandelTSE
{
    /// <summary>
    /// Interaction logic for ProgramEinstellungen.xaml
    /// </summary>
    public partial class ProgramEinstellungen : UserControl
    {
        public static OleDbConnection con = new OleDbConnection();
        List<items> it = new List<items>();
        BrushConverter converter = new System.Windows.Media.BrushConverter();
        Brush brush_red = new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)128, (byte)128));

        public List<Terminalen> Data { get; set; }
        Terminalen data = new Terminalen();
        List<Terminalen> list = new List<Terminalen>();
        OleDbCommand cmd2;
        bool started = true;
        bool darstellung_loaded = false;

        public class Terminalen
        {
            public Int32 Id { get; set; }
            public string Terminal_ID { get; set; }
            public string Marke { get; set; }
            public string Modell { get; set; }
            public string Serien { get; set; }
            public string Kassen { get; set; }
            public string Version { get; set; }
            public bool  Netzwerk { get; set; }
        }

        public ProgramEinstellungen()
        {
            InitializeComponent();

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
                else if (result == MessageBoxResult.No)
                { MessageBox.Show("Sie müssen den Treiber installieren, um die Daten sehen zu können."); }
            }

            OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_ProgramEinstellungenKassennetz] ([Id] COUNTER, [TerminalID] TEXT(55),[MarkeDesTerminals] TEXT(55),[Modellbezeichnung] TEXT(55),[Seriennummer] TEXT(55),[Kassensoftware] TEXT(55),[VersionDerSoftware] TEXT(55), [NetzwerkDatenbank] YESNO)", con);
            try { cmd.ExecuteNonQuery(); }
            catch { }

            cmd2 = new OleDbCommand("SELECT Id, TerminalID, MarkeDesTerminals, Modellbezeichnung, Seriennummer, Kassensoftware, VersionDerSoftware, NetzwerkDatenbank FROM [TBL_ProgramEinstellungenKassennetz]", con);

            LoadGrid();
            Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() => { EinstellungenTabs.SelectedIndex = 0; HideColumns(); }));
        }

        private void LoadGrid()
        {
            OleDbDataReader myReader = cmd2.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["Id"] != DBNull.Value) data.Id = (Int32)myReader["Id"]; else data.Id = -1;
                if (myReader["TerminalID"] != DBNull.Value) data.Terminal_ID = (string)myReader["TerminalID"]; else data.Terminal_ID = "";
                if (myReader["MarkeDesTerminals"] != DBNull.Value) data.Marke = (string)myReader["MarkeDesTerminals"]; else data.Marke = "";
                if (myReader["Modellbezeichnung"] != DBNull.Value) data.Modell = (string)myReader["Modellbezeichnung"]; else data.Modell = "";
                if (myReader["Seriennummer"] != DBNull.Value) data.Serien = (string)myReader["Seriennummer"]; else data.Serien = "";
                if (myReader["Kassensoftware"] != DBNull.Value) data.Kassen = (string)myReader["Kassensoftware"]; else data.Kassen = "";
                if (myReader["VersionDerSoftware"] != DBNull.Value) data.Version = (string)myReader["VersionDerSoftware"]; else data.Version = "";
                if (myReader["NetzwerkDatenbank"] != DBNull.Value) data.Netzwerk = (bool)(Boolean)myReader["NetzwerkDatenbank"];// == "0"? false : true; else data.Netzwerk = false;
                list.Add(new Terminalen { Id = data.Id, Terminal_ID = data.Terminal_ID, Marke = data.Serien, Modell = data.Modell, Serien = data.Serien, Kassen = data.Kassen, Version = data.Version, Netzwerk = data.Netzwerk });
            }
            Data = list;
            TerminalenDataGrid.ItemsSource = Data;
            list = new List<Terminalen>();
        }

        private void cp1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cp1.SelectedColor.HasValue)
            {
                started = false;
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
                started = false;
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (darstellung_loaded == true) started = false; }

        private void CustomizeHeaders(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e) { if (e.Column.Header.ToString() == "Terminal_ID") e.Column.Header = "Terminal-ID"; }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            int k = 0;
            foreach (TextBox tb in Artikelverwaltung.FindVisualChildren<TextBox>(TerminalIDPanel)) { if (tb.Text == "") { tb.Background = brush_red; k = 1; } }
            if (k == 1) { MessageBox.Show("mit * gekennzeichnete Felder sind Pflichtfelder"); return; };

            Int32 maxID = 0;
            OleDbCommand maxCommand = new OleDbCommand("SELECT max(Id) from TBL_ProgramEinstellungenKassennetz", con);
            try { maxID = (Int32)maxCommand.ExecuteScalar(); }
            catch { }

            int result = 0;
            int NetzwerkDatenbankValue = 0;
            if (NetzwerkDatenbank.IsChecked == true) NetzwerkDatenbankValue = -1;
            OleDbCommand cmd = new OleDbCommand("insert into [TBL_ProgramEinstellungenKassennetz](Id, TerminalID, MarkeDesTerminals, Modellbezeichnung, Seriennummer, Kassensoftware, VersionDerSoftware, NetzwerkDatenbank)Values('"+ ++maxID + "','" + TerminalID.Text + "','" + MarkeDesTermin.Text + "','" + Modellbezeichnung.Text + "','" + Seriennummer.Text + "','" + KassenSoftware.Text + "','" + VersionDerSoftware.Text + "','" + NetzwerkDatenbankValue + "')", con);
            try { result = cmd.ExecuteNonQuery(); LoadGrid(); HideColumns(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
        }

        private void TextChanged(object sender, TextChangedEventArgs e) { if (((TextBox)sender).Background == brush_red) ((TextBox)sender).Background = Brushes.White; }

        private void NeuesTerminal_Click(object sender, RoutedEventArgs e)
        {
            TerminalID.Text = "";
            MarkeDesTermin.Text = "";
            Modellbezeichnung.Text = "";
            Seriennummer.Text = "";
            KassenSoftware.Text = "POSprom Handel Plus TSE";
            VersionDerSoftware.Text = "4.5";

            if (TerminalID.Background == brush_red) TerminalID.Background = Brushes.White;
            if (MarkeDesTermin.Background == brush_red) MarkeDesTermin.Background = Brushes.White;
            if (Modellbezeichnung.Background == brush_red) Modellbezeichnung.Background = Brushes.White;
            if (Seriennummer.Background == brush_red) Seriennummer.Background = Brushes.White;
            if (KassenSoftware.Background == brush_red) KassenSoftware.Background = Brushes.White;
            if (VersionDerSoftware.Background == brush_red) VersionDerSoftware.Background = Brushes.White;

            TerminalenDataGrid.SelectedItem = null;
        }

        private void RecordSelected(object sender, SelectionChangedEventArgs e)
        {
            if (TerminalenDataGrid.SelectedItem == null) return;
            var dg = sender as System.Windows.Controls.DataGrid;
            if (dg == null) return;
            var row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
            if (row == null) return;

            TerminalID.Text = ((Terminalen)row.Item).Terminal_ID;
            MarkeDesTermin.Text = ((Terminalen)row.Item).Marke;
            Modellbezeichnung.Text = ((Terminalen)row.Item).Modell;
            Seriennummer.Text = ((Terminalen)row.Item).Serien;
            KassenSoftware.Text = ((Terminalen)row.Item).Kassen;
            VersionDerSoftware.Text = ((Terminalen)row.Item).Version;
            NetzwerkDatenbank.IsChecked = ((Terminalen)row.Item).Netzwerk;
        }

        private void HideColumns() { if (TerminalenDataGrid.Items.Count > 0 && TerminalenDataGrid.Columns.Count > 0) for (int i = 0; i < TerminalenDataGrid.Columns.Count; i++) if (i != 1 && i != 2) TerminalenDataGrid.Columns[i].Visibility = Visibility.Collapsed; }

        private void Loschen_Click(object sender, RoutedEventArgs e)
        {
            if (TerminalenDataGrid.SelectedItem == null) { MessageBox.Show("Bitte wählen Sie den Datensatz in der Tabelle aus!"); return; }

            string messageBoxText = "Achtung!\nWollen Sie wirklich das Terminal löschen?";
            string caption = "Terminal löschen...";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process the user choice
            switch (messageResult)
            {
                case MessageBoxResult.OK:
                    var row = (DataGridRow)TerminalenDataGrid.ItemContainerGenerator.ContainerFromIndex(TerminalenDataGrid.SelectedIndex);
                    var item = row.Item as Terminalen;

                    OleDbCommand cmd = new OleDbCommand("DELETE FROM [TBL_ProgramEinstellungenKassennetz] where Id = @ID", con);
                    cmd.Parameters.Add(new OleDbParameter("@ID", item.Id));

                    int result = 0;
                    try { result = cmd.ExecuteNonQuery(); }
                    catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

                    LoadGrid();
                    HideColumns();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EinstellungenTabs.SelectedIndex == 4) speichernCommonButton.IsEnabled = false;
            else if (EinstellungenTabs.IsLoaded && speichernCommonButton.IsEnabled == false) speichernCommonButton.IsEnabled = true;
            if (EinstellungenTabs.SelectedIndex == 0 && started == true)
            {
                OleDbCommand cmd3 = new OleDbCommand("SELECT Hintergrundfarbe, ProgrammOberflache, ProgrammGrosse, Spaltenzahl1, Zeilenzahl1, Spaltenzahl2, Zeilenzahl2, SchriftGross, MenuFunktionen, SchnellDruck FROM [TBL_ProgramEinstellungenDarstellung]", con);
                OleDbDataReader myReader = cmd3.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader["Hintergrundfarbe"] != DBNull.Value) { Farbauswahl1.Style = (Style)FindResource("BlueButton"); Farbauswahl1.Background = (Brush)converter.ConvertFromString((string)myReader["Hintergrundfarbe"]); }
                    if (myReader["ProgrammOberflache"] != DBNull.Value) { Farbauswahl2.Style = (Style)FindResource("BlueButton"); Farbauswahl2.Background = (Brush)converter.ConvertFromString((string)myReader["ProgrammOberflache"]); }
                    if (myReader["ProgrammGrosse"] != DBNull.Value) ProgramGrosseBox.Text = (string)myReader["ProgrammGrosse"];
                    if (myReader["Spaltenzahl1"] != DBNull.Value) Spaltenzahl1.Text = (string)myReader["Spaltenzahl1"];
                    if (myReader["Zeilenzahl1"] != DBNull.Value) Zeilenzahl1.Text = (string)myReader["Zeilenzahl1"];
                    if (myReader["Spaltenzahl2"] != DBNull.Value) Spaltenzahl2.Text = (string)myReader["Spaltenzahl2"];
                    if (myReader["Zeilenzahl2"] != DBNull.Value) Zeilenzahl2.Text = (string)myReader["Zeilenzahl2"];
                    if (myReader["SchriftGross"] != DBNull.Value) SchriftGrossTasten.Text = (string)myReader["SchriftGross"];
                    if (myReader["MenuFunktionen"] != DBNull.Value) MenuFunktionenCheckbox.IsChecked = (bool)(Boolean)myReader["MenuFunktionen"];
                    if (myReader["SchnellDruck"] != DBNull.Value) SchnellDruckBox.Text = (string)myReader["SchnellDruck"];
                }
                darstellung_loaded = true;
            }
            if (EinstellungenTabs.SelectedIndex == 1)
            {
                OleDbCommand cmd3 = new OleDbCommand("SELECT Firma, Inhaber, Strasse, PLZ, Ort, Land, Telefon, Fax, [E-mail], Steuernummer, USTID, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11] FROM [TBL_ProgramEinstellungenFirmendaten]", con);
                OleDbDataReader myReader = cmd3.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader["Firma"] != DBNull.Value) { FirmaField.Text = (string)myReader["Firma"]; }
                    if (myReader["Inhaber"] != DBNull.Value) { InhaberField.Text = (string)myReader["Inhaber"]; }
                    if (myReader["Strasse"] != DBNull.Value) StrasseField.Text = (string)myReader["Strasse"];
                    if (myReader["PLZ"] != DBNull.Value) PLZField.Text = (string)myReader["PLZ"];
                    if (myReader["Ort"] != DBNull.Value) OrtField.Text = (string)myReader["Ort"];
                    if (myReader["Land"] != DBNull.Value) LandField.Text = (string)myReader["Land"];
                    if (myReader["Telefon"] != DBNull.Value) TelefonField.Text = (string)myReader["Telefon"];
                    if (myReader["Fax"] != DBNull.Value) FaxField.Text = (string)myReader["Fax"];
                    if (myReader["E-mail"] != DBNull.Value) EmailField.Text = (string)myReader["E-mail"];
                    if (myReader["Steuernummer"] != DBNull.Value) SteuerField.Text = (string)myReader["Steuernummer"];
                    if (myReader["USTID"] != DBNull.Value) USTIDField.Text = (string)myReader["USTID"];
                    int i = 1;
                    foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(FirmendatenPanel)) { ch.IsChecked = (bool)(Boolean)myReader[i.ToString()]; i++; }
                }
            }
        }

        private void speichernCommonButton_Click(object sender, RoutedEventArgs e)
        {
            if (EinstellungenTabs.SelectedIndex == 0)
            {
                if (SchriftGrossTasten.Text == "" || Spaltenzahl1.Text == "" || Spaltenzahl2.Text == "" || Zeilenzahl1.Text == "" || Zeilenzahl2.Text == "") { MessageBox.Show("Korrigieren Sie Ihre Daten!"); return; }
                OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_ProgramEinstellungenDarstellung] ([Id] COUNTER, [Hintergrundfarbe] TEXT(55),[ProgrammOberflache] TEXT(55),[ProgrammGrosse] TEXT(55),[Spaltenzahl1] TEXT(55),[Zeilenzahl1] TEXT(55),[Spaltenzahl2] TEXT(55),[Zeilenzahl2] TEXT(55),[SchriftGross] TEXT(55), [MenuFunktionen] YESNO, [SchnellDruck] TEXT(55))", con);
                try { cmd.ExecuteNonQuery(); } catch { }

                int result = 0, MenuFunktionenValue = 0;
                Int32 Id = -1;
                if (MenuFunktionenCheckbox.IsChecked == true) MenuFunktionenValue = -1;
                OleDbCommand cmd2;

                OleDbCommand IdCommand = new OleDbCommand("SELECT max(Id) from TBL_ProgramEinstellungenDarstellung", con);
                try { Id = (Int32)IdCommand.ExecuteScalar(); } catch { }

                if (Id == 0) 
                { 
                    cmd2 = new OleDbCommand("UPDATE [TBL_ProgramEinstellungenDarstellung] SET Hintergrundfarbe = @Hintergrundfarbe, ProgrammOberflache = @ProgrammOberflache, ProgrammGrosse = @ProgrammGrosse, Spaltenzahl1 = @Spaltenzahl1, Zeilenzahl1 = @Zeilenzahl1, Spaltenzahl2 = @Spaltenzahl2, Zeilenzahl2 = @Zeilenzahl2, SchriftGross = @SchriftGross, MenuFunktionen = @MenuFunktionen, SchnellDruck = @SchnellDruck WHERE Id = @ID", con);

                    cmd2.Parameters.Add(new OleDbParameter("@Hintergrundfarbe", Farbauswahl1.Background.ToString()));
                    cmd2.Parameters.Add(new OleDbParameter("@ProgrammOberflache", Farbauswahl2.Background.ToString()));
                    cmd2.Parameters.Add(new OleDbParameter("@ProgrammGrosse", ProgramGrosseBox.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@Spaltenzahl1", Spaltenzahl1.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@Zeilenzahl1", Zeilenzahl1.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@Spaltenzahl2", Spaltenzahl2.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@Zeilenzahl2", Zeilenzahl2.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@SchriftGross", SchriftGrossTasten.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@MenuFunktionen", MenuFunktionenValue));
                    cmd2.Parameters.Add(new OleDbParameter("@SchnellDruck", SchnellDruckBox.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@ID", Id));
                }
                else cmd2 = new OleDbCommand("insert into [TBL_ProgramEinstellungenDarstellung](Id, Hintergrundfarbe, ProgrammOberflache, ProgrammGrosse, Spaltenzahl1, Zeilenzahl1, Spaltenzahl2, Zeilenzahl2, SchriftGross, MenuFunktionen, SchnellDruck)Values('" + 0 + "','" + Farbauswahl1.Background.ToString() + "','" + Farbauswahl2.Background.ToString() + "','" + ProgramGrosseBox.Text + "','" + Spaltenzahl1.Text + "','" + Zeilenzahl1.Text + "','" + Spaltenzahl2.Text + "','" + Zeilenzahl2.Text + "','" + SchriftGrossTasten.Text + "','" + MenuFunktionenValue + "','" + SchnellDruckBox.Text + "')", con);
                try { result = cmd2.ExecuteNonQuery(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
                catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
            }
            else if (EinstellungenTabs.SelectedIndex == 1)
            {
                int k = 0;
                foreach (TextBox tb in Artikelverwaltung.FindVisualChildren<TextBox>(FirmendatenPanel)) { if ((tb.Name == "FirmaField" || tb.Name == "StrasseField" || tb.Name == "PLZField" || tb.Name == "OrtField" || tb.Name == "LandField" || tb.Name == "SteuerField" || tb.Name == "USTIDField") && tb.Text == "") { tb.Background = brush_red; k = 1; } }
                if (k == 1) { MessageBox.Show("mit * gekennzeichnete Felder sind Pflichtfelder"); return; };

                OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_ProgramEinstellungenFirmendaten] ([Id] COUNTER, [Firma] TEXT(55),[Inhaber] TEXT(55),[Strasse] TEXT(55),[PLZ] TEXT(55),[Ort] TEXT(55),[Land] TEXT(55),[Telefon] TEXT(55),[Fax] TEXT(55), [E-mail] TEXT(55), [Steuernummer] TEXT(55), [USTID] TEXT(55), [1] YESNO, [2] YESNO, [3] YESNO, [4] YESNO, [5] YESNO, [6] YESNO, [7] YESNO, [8] YESNO, [9] YESNO, [10] YESNO, [11] YESNO)", con);
                try { cmd.ExecuteNonQuery(); } catch { }

                int result = 0, counter = 0;
                int[] Checkboxes = new int[11];
                Int32 Id = -1; 
                foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(FirmendatenPanel)) { if (ch.IsChecked == true) Checkboxes[counter] = -1; else Checkboxes[counter] = 0; counter++; }

                OleDbCommand cmd4;

                OleDbCommand IdCommand = new OleDbCommand("SELECT max(Id) from TBL_ProgramEinstellungenFirmendaten", con);
                try { Id = (Int32)IdCommand.ExecuteScalar(); } catch { }

                if (Id == 0)
                {
                    cmd4 = new OleDbCommand("UPDATE [TBL_ProgramEinstellungenFirmendaten] SET [Firma] = @Firma, [Inhaber] = @Inhaber, [Strasse] = @Strasse, [PLZ] = @PLZ, [Ort] = @Ort, [Land] = @Land, [Telefon] = @Telefon, [Fax] = @Fax, [E-mail] = @Email, [Steuernummer] = @Steuernummer, [USTID] = @USTID, [1] = @1, [2] = @2, [3] = @3, [4] = @4, [5] = @5, [6] = @6, [7] = @7, [8] = @8, [9] = @9, [10] = @10, [11] = @11 WHERE [Id] = @ID", con);

                    cmd4.Parameters.Add(new OleDbParameter("@Firma", FirmaField.Text));
                    cmd4.Parameters.Add(new OleDbParameter("@Inhaber", InhaberField.Text));
                    cmd4.Parameters.Add(new OleDbParameter("@Strasse", StrasseField.Text));
                    cmd4.Parameters.Add(new OleDbParameter("@PLZ", PLZField.Text));
                    cmd4.Parameters.Add(new OleDbParameter("@Ort", OrtField.Text));
                    cmd4.Parameters.Add(new OleDbParameter("@Land", LandField.Text));
                    cmd4.Parameters.Add(new OleDbParameter("@Telefon", TelefonField.Text));
                    cmd4.Parameters.Add(new OleDbParameter("@Fax", FaxField.Text));
                    cmd4.Parameters.Add(new OleDbParameter("@Email", EmailField.Text));
                    cmd4.Parameters.Add(new OleDbParameter("@Steuernummer", SteuerField.Text));
                    cmd4.Parameters.Add(new OleDbParameter("@USTID", USTIDField.Text));
                    for (int i = 1; i < 12; i++) { cmd4.Parameters.Add(new OleDbParameter("@" + i.ToString(), Checkboxes[i - 1])); }
                    cmd4.Parameters.Add(new OleDbParameter("@ID", Id));
                }
                else
                {
                    string checkboxes = "";
                    for (int i = 1; i < 12; i++) { checkboxes += "','" + Checkboxes[i - 1].ToString(); }
                    cmd4 = new OleDbCommand("insert into [TBL_ProgramEinstellungenFirmendaten](Id, Firma, Inhaber, Strasse, PLZ, Ort, Land, Telefon, Fax, [E-mail], Steuernummer, USTID, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11])Values('" + 0 + "','" + FirmaField.Text + "','" + InhaberField.Text + "','" + StrasseField.Text + "','" + PLZField.Text + "','" + OrtField.Text + "','" + LandField.Text + "','" + TelefonField.Text + "','" + FaxField.Text + "','" + EmailField.Text + "','" + SteuerField.Text + "','" + USTIDField.Text + checkboxes + "')", con);
                }
                try { result = cmd4.ExecuteNonQuery(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
                catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

            }
            else if (EinstellungenTabs.SelectedIndex == 2)
            {
                int k = 0;
                foreach (TextBox tb in Artikelverwaltung.FindVisualChildren<TextBox>(KassendatenPanel)) { if (tb.Text == "" && tb.Name != "DSFinField") { tb.Background = brush_red; k = 1; } }
                if (k == 1) { MessageBox.Show("mit * gekennzeichnete Felder sind Pflichtfelder"); return; }; 
                OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_ProgramEinstellungenKassendaten] ([Id] COUNTER, [Kassennummer] TEXT(55),[MarkederKasse] TEXT(55),[Modellbezeichnung] TEXT(55),[Seriennummer] TEXT(55),[Kassensoftware] TEXT(55),[VersionDerSoftware] TEXT(55),[WahrungDerKasse] TEXT(55),[BasiswahrungCode] TEXT(55), [Dsfinv-k] TEXT(55))", con);
                try { cmd.ExecuteNonQuery(); } catch { }

                int result = 0;
                Int32 Id = -1;
                OleDbCommand cmd2;

                OleDbCommand IdCommand = new OleDbCommand("SELECT max(Id) from TBL_ProgramEinstellungenKassendaten", con);
                try { Id = (Int32)IdCommand.ExecuteScalar(); } catch { }

                if (Id == 0)
                {
                    cmd2 = new OleDbCommand("UPDATE [TBL_ProgramEinstellungenKassendaten] SET Kassennummer = @Kassennummer, MarkederKasse = @MarkederKasse, Modellbezeichnung = @Modellbezeichnung, Seriennummer = @Seriennummer, Kassensoftware = @Kassensoftware, VersionDerSoftware = @VersionDerSoftware, WahrungDerKasse = @WahrungDerKasse, BasiswahrungCode = @BasiswahrungCode, [Dsfinv-k] = @Dsfinvk WHERE Id = @ID", con);

                    cmd2.Parameters.Add(new OleDbParameter("@Kassennummer", Kassennummer.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@MarkederKasse", MarkederKasse.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@Modellbezeichnung", Modellbezeichnung_Kassendaten.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@Seriennummer", Seriennummer_Kassendaten.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@Kassensoftware", KassenSoftware_Kassendaten.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@VersionDerSoftware", VersionDerSoftware_Kassendaten.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@WahrungDerKasse", WahrungDerKasse.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@BasiswahrungCode", BasiswahrungCode.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@Dsfinvk", DSFinField.Text));
                    cmd2.Parameters.Add(new OleDbParameter("@ID", Id));
                }
                else cmd2 = new OleDbCommand("insert into [TBL_ProgramEinstellungenKassendaten](Id, Kassennummer, MarkederKasse, Modellbezeichnung, Seriennummer, Kassensoftware, VersionDerSoftware, WahrungDerKasse, BasiswahrungCode, [Dsfinv-k])Values('" + 0 + "','" + Kassennummer.Text + "','" + MarkederKasse.Text + "','" + Modellbezeichnung_Kassendaten.Text + "','" + Seriennummer_Kassendaten.Text + "','" + KassenSoftware_Kassendaten.Text + "','" + VersionDerSoftware_Kassendaten.Text + "','" + WahrungDerKasse.Text + "','" + BasiswahrungCode.Text + "','" + DSFinField.Text + "')", con);
                try { result = cmd2.ExecuteNonQuery(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
                catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
            }
        }

        private void SchnellDruckBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (darstellung_loaded == true) started = false; }

        private void AlleCheckBox_Checked(object sender, RoutedEventArgs e) { foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(FirmendatenPanel)) ch.IsChecked = true; }

        private void AlleCheckBox_Unchecked(object sender, RoutedEventArgs e) { foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(FirmendatenPanel)) ch.IsChecked = false; }
    }
}
