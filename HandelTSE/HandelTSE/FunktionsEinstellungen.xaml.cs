using HandelTSE.ViewModels;
using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for Funktionseinstellungen.xaml
    /// </summary>
    public partial class FunktionsEinstellungen : UserControl
    {
        public static SQLiteConnection con;
        SQLiteCommand cmd;

        public FunktionsEinstellungen()
        {
            InitializeComponent();

            // If the menu ProgramEinstellungen is being first time opened, - create SQLite db Tables
            if (con == null) InitializeAllTables();
            if (MainWindow.con.State != System.Data.ConnectionState.Closed) LoadData();
            if (EinmannbetriebCheckbox.IsChecked == false && AnmeldungMitPasswortCheckbox.IsChecked == false) AnmeldungMitPasswortCheckbox.IsChecked = true;
        }

        void InitializeAllTables()
        {
            con = MainWindow.con;

            cmd = new SQLiteCommand("CREATE TABLE [TBL_FunktionsEinstellungenProgram] ([Id] COUNTER, [1] YESNO, [2] YESNO, [3] YESNO, [4] YESNO, [5] YESNO, [6] YESNO, [7] YESNO, [8] YESNO, [9] YESNO, [10] YESNO, [11] YESNO, [12] YESNO, [13] YESNO, [14] YESNO, [15] YESNO, [16] YESNO, [SonstigeArtikel] TEXT(55), [Kassenbestand] TEXT(55))", con);
            try
            {
                cmd.ExecuteNonQuery();
                //Create first empty record
                string checkboxes = "";
                for (int i = 1; i < 17; i++) { checkboxes += "','" + "0"; }

                SQLiteCommand cmd2 = new SQLiteCommand("insert into [TBL_FunktionsEinstellungenProgram](Id, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [SonstigeArtikel], [Kassenbestand])Values('" + 0 + checkboxes + "','" + "Diverse Artikel" + "','" + "0.00" + "')", con);
                try { cmd2.ExecuteNonQuery(); }
                catch { }
            }
            catch { }

            cmd = new SQLiteCommand("CREATE TABLE [TBL_FunktionsEinstellungenFunktionen] ([Id] COUNTER, [1] YESNO, [2] YESNO, [3] YESNO, [4] YESNO, [5] YESNO, [6] YESNO, [7] YESNO, [8] YESNO, [9] YESNO, [10] YESNO, [11] YESNO, [BenachrichtigenStuck] TEXT(55))", con);
            try
            {
                cmd.ExecuteNonQuery();
                //Create first empty record
                string checkboxes = "";
                for (int i = 1; i < 12; i++) { checkboxes += "','" + "0"; }

                SQLiteCommand cmd2 = new SQLiteCommand("insert into [TBL_FunktionsEinstellungenFunktionen](Id, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [BenachrichtigenStuck])Values('" + 0 + checkboxes + "','" + "" + "')", con);
                try { cmd2.ExecuteNonQuery(); }
                catch { }
            }
            catch { }

            cmd = new SQLiteCommand("CREATE TABLE [TBL_FunktionsEinstellungenAbschlag] ([Id] COUNTER, [1] YESNO, [2] YESNO, [3] YESNO, [4] YESNO, [5] YESNO, [6] YESNO, [7] YESNO, [8] YESNO, [9] YESNO, [10] YESNO, [11] YESNO, [12] YESNO, [13] YESNO, [AbschlagSendenTextBox] TEXT(55))", con);
            try
            {
                cmd.ExecuteNonQuery();
                //Create first empty record
                string checkboxes = "";
                for (int i = 1; i < 14; i++) { checkboxes += "','" + "0"; }

                SQLiteCommand cmd2 = new SQLiteCommand("insert into [TBL_FunktionsEinstellungenAbschlag](Id, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [AbschlagSendenTextBox])Values('" + 0 + checkboxes + "','" + "" + "')", con);
                try { cmd2.ExecuteNonQuery(); }
                catch { }
            }
            catch { }

            cmd = new SQLiteCommand("CREATE TABLE [TBL_FunktionsEinstellungenBondrucker] ([Id] COUNTER, [1] YESNO, [2] YESNO, [3] YESNO, [4] YESNO, [5] YESNO, [6] YESNO, [7] YESNO, [8] YESNO, [9] YESNO, [10] YESNO, [11] YESNO, [12] YESNO, [13] YESNO, [14] YESNO, [15] YESNO, [16] YESNO, [17] YESNO, [18] YESNO)", con);
            try
            {
                cmd.ExecuteNonQuery();
                //Create first empty record
                string checkboxes = "";
                for (int i = 1; i < 19; i++) { checkboxes += "','" + "0"; }

                SQLiteCommand cmd2 = new SQLiteCommand("insert into [TBL_FunktionsEinstellungenBondrucker](Id, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18])Values('" + 0 + checkboxes + "')", con);
                try { cmd2.ExecuteNonQuery(); }
                catch { }
            }
            catch { }

            cmd = new SQLiteCommand("CREATE TABLE [TBL_FunktionsEinstellungenGutscheine] ([Id] COUNTER, [GutscheinSystemAktivCheckbox] YESNO, [MehrzweckRadioButton] YESNO, [EinzweckComboBox] TEXT(50), [GutscheinTextBox] TEXT(50))", con);
            try
            {
                cmd.ExecuteNonQuery();
                //Create first empty record
                string checkboxes = "";
                for (int i = 1; i < 19; i++) { checkboxes += "','" + "0"; }

                SQLiteCommand cmd2 = new SQLiteCommand("insert into [TBL_FunktionsEinstellungenGutscheine](Id, [GutscheinSystemAktivCheckbox], [MehrzweckRadioButton], [EinzweckComboBox], [GutscheinTextBox])Values('" + 0 + "','" + 0 + "','" + 1 + "','" + "0" + "','" + "25" + "')", con);
                try { cmd2.ExecuteNonQuery(); }
                catch { }
            }
            catch { }
        }

        void LoadData()
        {
            SQLiteDataReader myReader;
            SQLiteCommand cmd;
            cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM TBL_FunktionsEinstellungenProgram WHERE Id = 0";

            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                int i = 1;
                foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(ProgrammPanel)) { ch.IsChecked = (bool)(Boolean)myReader[i.ToString()]; i++; }

                if (myReader["SonstigeArtikel"] != DBNull.Value) SonstigeArtikel.Text = (string)myReader["SonstigeArtikel"]; else SonstigeArtikel.Text = "";
                if (myReader["Kassenbestand"] != DBNull.Value) Kassenbestand.Text = (string)myReader["Kassenbestand"]; else Kassenbestand.Text = "";
            }
        }

        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainWindow.con.State == System.Data.ConnectionState.Closed) return;
            if (EinstellungenTabs.SelectedIndex == 1)
            {
                SQLiteCommand cmd2 = new SQLiteCommand("SELECT [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], BenachrichtigenStuck  FROM [TBL_FunktionsEinstellungenFunktionen]", con);
                SQLiteDataReader myReader = cmd2.ExecuteReader();
                while (myReader.Read())
                {
                    int i = 1;
                    foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(FunktionenPanel)) { ch.IsChecked = (bool)(Boolean)myReader[i.ToString()]; i++; }

                    if (myReader["BenachrichtigenStuck"] != DBNull.Value) BenachrichtigenStuck.Text = (string)myReader["BenachrichtigenStuck"]; else BenachrichtigenStuck.Text = "";
                }
            } 
            else if (EinstellungenTabs.SelectedIndex == 2)
            {
                SQLiteCommand cmd2 = new SQLiteCommand("SELECT [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], AbschlagSendenTextBox  FROM [TBL_FunktionsEinstellungenAbschlag]", con);
                SQLiteDataReader myReader = cmd2.ExecuteReader();
                while (myReader.Read())
                {
                    int i = 1;
                    foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(ZAbschlagPanel)) { ch.IsChecked = (bool)(Boolean)myReader[i.ToString()]; i++; }

                    if (myReader["AbschlagSendenTextBox"] != DBNull.Value) AbschlagSendenTextBox.Text = (string)myReader["AbschlagSendenTextBox"]; else AbschlagSendenTextBox.Text = "";
                }

                if (ZAbschlagDrucken.IsChecked == false) { BondruckerCheckbox.IsEnabled = false; A4DruckerCheckbox.IsEnabled = false; BondruckerCheckbox.IsChecked = false; A4DruckerCheckbox.IsChecked = false; BondruckerTextBlock.Opacity = 0.7; A4DruckerTextBlock.Opacity = 0.7; }
            }
            else if (EinstellungenTabs.SelectedIndex == 3)
            {
                SQLiteCommand cmd2 = new SQLiteCommand("SELECT [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18] FROM [TBL_FunktionsEinstellungenBondrucker]", con);
                SQLiteDataReader myReader = cmd2.ExecuteReader();
                while (myReader.Read()) 
                { 
                    int i = 1; 
                    foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(BondruckerKasseStackPanel)) { ch.IsChecked = (bool)(Boolean)myReader[i.ToString()]; i++; }
                    foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(BondruckerOfficeStackPanel)) { ch.IsChecked = (bool)(Boolean)myReader[i.ToString()]; i++; }
                }
            }
            else if(EinstellungenTabs.SelectedIndex == 4 && EinzweckComboBox.IsEnabled == false)
            {
                SQLiteCommand cmd2 = new SQLiteCommand("SELECT [GutscheinSystemAktivCheckbox], [MehrzweckRadioButton], [EinzweckComboBox], [GutscheinTextBox] FROM [TBL_FunktionsEinstellungenGutscheine]", con);
                SQLiteDataReader myReader = cmd2.ExecuteReader();
                while (myReader.Read())
                {
                    GutscheinSystemAktivCheckbox.IsChecked = (bool)(Boolean)myReader["GutscheinSystemAktivCheckbox"];
                    if ((bool)(Boolean)myReader["MehrzweckRadioButton"] == true) MehrzweckRadioButton.IsChecked = true; else EinzweckRadioButton.IsChecked = true; 
                    if (myReader["EinzweckComboBox"] != DBNull.Value) EinzweckComboBox.Text = (string)myReader["EinzweckComboBox"]; else EinzweckComboBox.SelectedIndex = EinzweckComboBox.Items.Count-1;
                    if (myReader["GutscheinTextBox"] != DBNull.Value) GutscheinTextBox.Text = (string)myReader["GutscheinTextBox"]; else GutscheinTextBox.Text = "25";
                }
            }
        }

        private void speichernCommonButton_Click(object sender, RoutedEventArgs e)
        {
            if (EinstellungenTabs.SelectedIndex == 0)
            {
                int result = 0, counter = 0;
                int[] Checkboxes = new int[16];
                Int32 Id = 0;
                foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(ProgrammPanel)) { if (ch.IsChecked == true) Checkboxes[counter] = 1; else Checkboxes[counter] = 0; counter++; }

                SQLiteCommand cmd4;
                cmd4 = new SQLiteCommand("UPDATE [TBL_FunktionsEinstellungenProgram] SET [1] = @1, [2] = @2, [3] = @3, [4] = @4, [5] = @5, [6] = @6, [7] = @7, [8] = @8, [9] = @9, [10] = @10, [11] = @11, [12] = @12, [13] = @13, [14] = @14, [15] = @15, [16] = @16, [SonstigeArtikel] = @SonstigeArtikel, [Kassenbestand] = @Kassenbestand WHERE [Id] = @ID", con);

                for (int i = 1; i < 17; i++) { cmd4.Parameters.Add(new SQLiteParameter("@" + i.ToString(), Checkboxes[i - 1])); }
                cmd4.Parameters.Add(new SQLiteParameter("@SonstigeArtikel", SonstigeArtikel.Text));
                cmd4.Parameters.Add(new SQLiteParameter("@Kassenbestand", Kassenbestand.Text));
                cmd4.Parameters.Add(new SQLiteParameter("@ID", Id));
                
                try { result = cmd4.ExecuteNonQuery(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
                catch {}
            }
            else if (EinstellungenTabs.SelectedIndex == 1)
            {
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE [TBL_FunktionsEinstellungenFunktionen] ([Id] COUNTER, [1] YESNO, [2] YESNO, [3] YESNO, [4] YESNO, [5] YESNO, [6] YESNO, [7] YESNO, [8] YESNO, [9] YESNO, [10] YESNO, [11] YESNO, [BenachrichtigenStuck] TEXT(55))", con);
                try { cmd.ExecuteNonQuery(); } catch { }

                int result = 0, counter = 0;
                int[] Checkboxes = new int[11];
                Int32 Id = -1;
                foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(FunktionenPanel)) { if (ch.IsChecked == true) Checkboxes[counter] = -1; else Checkboxes[counter] = 0; counter++; }

                SQLiteCommand cmd4;

                SQLiteCommand IdCommand = new SQLiteCommand("SELECT max(Id) from TBL_FunktionsEinstellungenFunktionen", con);
                try { Id = (int)(long)IdCommand.ExecuteScalar(); } catch { }

                if (Id == 0)
                {
                    cmd4 = new SQLiteCommand("UPDATE [TBL_FunktionsEinstellungenFunktionen] SET [1] = @1, [2] = @2, [3] = @3, [4] = @4, [5] = @5, [6] = @6, [7] = @7, [8] = @8, [9] = @9, [10] = @10, [11] = @11, [BenachrichtigenStuck] = @BenachrichtigenStuck WHERE [Id] = @ID", con);

                    for (int i = 1; i < 12; i++) { cmd4.Parameters.Add(new SQLiteParameter("@" + i.ToString(), Checkboxes[i - 1])); }
                    cmd4.Parameters.Add(new SQLiteParameter("@BenachrichtigenStuck", BenachrichtigenStuck.Text));
                    cmd4.Parameters.Add(new SQLiteParameter("@ID", Id));
                }
                else
                {
                    string checkboxes = "";
                    for (int i = 1; i < 12; i++) { checkboxes += "','" + Checkboxes[i - 1].ToString(); }
                    cmd4 = new SQLiteCommand("insert into [TBL_FunktionsEinstellungenFunktionen](Id, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [BenachrichtigenStuck])Values('" + 0 + checkboxes + "','" + BenachrichtigenStuck.Text + "')", con);
                }
                try { result = cmd4.ExecuteNonQuery(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
                catch { MessageBox.Show(""); }
            }
            else if (EinstellungenTabs.SelectedIndex == 2)
            {
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE [TBL_FunktionsEinstellungenAbschlag] ([Id] COUNTER, [1] YESNO, [2] YESNO, [3] YESNO, [4] YESNO, [5] YESNO, [6] YESNO, [7] YESNO, [8] YESNO, [9] YESNO, [10] YESNO, [11] YESNO, [12] YESNO, [13] YESNO, [AbschlagSendenTextBox] TEXT(55))", con);
                try { cmd.ExecuteNonQuery(); } catch { }

                int result = 0, counter = 0;
                int[] Checkboxes = new int[13];
                Int32 Id = -1;
                foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(ZAbschlagPanel)) { if (ch.IsChecked == true) Checkboxes[counter] = -1; else Checkboxes[counter] = 0; counter++; }

                SQLiteCommand cmd4;

                SQLiteCommand IdCommand = new SQLiteCommand("SELECT max(Id) from TBL_FunktionsEinstellungenAbschlag", con);
                try { Id = (int)(long)IdCommand.ExecuteScalar(); } catch { }

                if (Id == 0)
                {
                    cmd4 = new SQLiteCommand("UPDATE [TBL_FunktionsEinstellungenAbschlag] SET [1] = @1, [2] = @2, [3] = @3, [4] = @4, [5] = @5, [6] = @6, [7] = @7, [8] = @8, [9] = @9, [10] = @10, [11] = @11, [12] = @12, [13] = @13, [AbschlagSendenTextBox] = @AbschlagSendenTextBox WHERE [Id] = @ID", con);

                    for (int i = 1; i < 14; i++) { cmd4.Parameters.Add(new SQLiteParameter("@" + i.ToString(), Checkboxes[i - 1])); }
                    cmd4.Parameters.Add(new SQLiteParameter("@AbschlagSendenTextBox", AbschlagSendenTextBox.Text));
                    cmd4.Parameters.Add(new SQLiteParameter("@ID", Id));
                }
                else
                {
                    string checkboxes = "";
                    for (int i = 1; i < 14; i++) { checkboxes += "','" + Checkboxes[i - 1].ToString(); }
                    cmd4 = new SQLiteCommand("insert into [TBL_FunktionsEinstellungenAbschlag](Id, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [AbschlagSendenTextBox])Values('" + 0 + checkboxes + "','" + AbschlagSendenTextBox.Text + "')", con);
                }
                try { result = cmd4.ExecuteNonQuery(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
                catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
            }
            else if (EinstellungenTabs.SelectedIndex == 3)
            {
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE [TBL_FunktionsEinstellungenBondrucker] ([Id] COUNTER, [1] YESNO, [2] YESNO, [3] YESNO, [4] YESNO, [5] YESNO, [6] YESNO, [7] YESNO, [8] YESNO, [9] YESNO, [10] YESNO, [11] YESNO, [12] YESNO, [13] YESNO, [14] YESNO, [15] YESNO, [16] YESNO, [17] YESNO, [18] YESNO)", con);
                try { cmd.ExecuteNonQuery(); } catch { }

                int result = 0, counter = 0;
                int[] Checkboxes = new int[18];
                Int32 Id = -1;
                foreach (CheckBox ch in Artikelverwaltung.FindVisualChildren<CheckBox>(BondruckerPanel)) { if (ch.IsChecked == true) Checkboxes[counter] = -1; else Checkboxes[counter] = 0; counter++; }

                SQLiteCommand cmd4;

                SQLiteCommand IdCommand = new SQLiteCommand("SELECT max(Id) from TBL_FunktionsEinstellungenBondrucker", con);
                try { Id = (int)(long)IdCommand.ExecuteScalar(); } catch { }

                if (Id == 0)
                {
                    cmd4 = new SQLiteCommand("UPDATE [TBL_FunktionsEinstellungenBondrucker] SET [1] = @1, [2] = @2, [3] = @3, [4] = @4, [5] = @5, [6] = @6, [7] = @7, [8] = @8, [9] = @9, [10] = @10, [11] = @11, [12] = @12, [13] = @13, [14] = @14, [15] = @15, [16] = @16, [17] = @17, [18] = @18 WHERE [Id] = @ID", con);

                    for (int i = 1; i < 19; i++) { cmd4.Parameters.Add(new SQLiteParameter("@" + i.ToString(), Checkboxes[i - 1])); }
                    cmd4.Parameters.Add(new SQLiteParameter("@ID", Id));
                }
                else
                {
                    string checkboxes = "";
                    for (int i = 1; i < 19; i++) { checkboxes += "','" + Checkboxes[i - 1].ToString(); }
                    cmd4 = new SQLiteCommand("insert into [TBL_FunktionsEinstellungenBondrucker](Id, [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18])Values('" + 0 + checkboxes + "')", con);
                }
                try { result = cmd4.ExecuteNonQuery(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
                catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
            }
            else if (EinstellungenTabs.SelectedIndex == 4)
            {
                SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE [TBL_FunktionsEinstellungenGutscheine] ([Id] COUNTER, [GutscheinSystemAktivCheckbox] YESNO, [MehrzweckRadioButton] YESNO, [EinzweckComboBox] TEXT(55), [GutscheinTextBox] TEXT(55))", con);
                try { cmd.ExecuteNonQuery(); } catch { }

                int result = 0;
                int GutscheinCheckbox = GutscheinSystemAktivCheckbox.IsChecked == true? GutscheinCheckbox = -1 : GutscheinCheckbox = 0;
                int MehrzweckCheckbox = MehrzweckRadioButton.IsChecked == true ? MehrzweckCheckbox = -1 : MehrzweckCheckbox = 0;
                Int32 Id = -1;

                SQLiteCommand cmd4;

                SQLiteCommand IdCommand = new SQLiteCommand("SELECT max(Id) from TBL_FunktionsEinstellungenGutscheine", con);
                try { Id = (int)(long)IdCommand.ExecuteScalar(); } catch { }
                string EinzweckString = "0";
                if (MehrzweckRadioButton.IsChecked == false) EinzweckString = EinzweckComboBox.Text;

                if (Id == 0)
                {
                    cmd4 = new SQLiteCommand("UPDATE [TBL_FunktionsEinstellungenGutscheine] SET [GutscheinSystemAktivCheckbox] = @GutscheinSystemAktivCheckbox, [MehrzweckRadioButton] = @MehrzweckRadioButton, [EinzweckComboBox] = @EinzweckComboBox, [GutscheinTextBox] = @GutscheinTextBox WHERE [Id] = @ID", con);
                    cmd4.Parameters.Add(new SQLiteParameter("@GutscheinSystemAktivCheckbox", GutscheinCheckbox));
                    cmd4.Parameters.Add(new SQLiteParameter("@MehrzweckRadioButton", MehrzweckCheckbox));
                    cmd4.Parameters.Add(new SQLiteParameter("@EinzweckComboBox", EinzweckString));
                    cmd4.Parameters.Add(new SQLiteParameter("@GutscheinTextBox", GutscheinTextBox.Text));
                    cmd4.Parameters.Add(new SQLiteParameter("@ID", Id));
                }
                else { cmd4 = new SQLiteCommand("insert into [TBL_FunktionsEinstellungenGutscheine](Id, [GutscheinSystemAktivCheckbox], [MehrzweckRadioButton], [EinzweckComboBox], [GutscheinTextBox] )Values('" + 0 + "','" + GutscheinCheckbox + "','" + MehrzweckCheckbox + "','" + EinzweckString + "','" + GutscheinTextBox.Text + "')", con); }
                try { result = cmd4.ExecuteNonQuery(); MessageBox.Show("Ihre Daten wurden erfolgreich gespeichert!"); }
                catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }
            }
        } // Save Common Button --- END

        private void EinmannbetriebCheckBox_Unchecked(object sender, RoutedEventArgs e) { AnmeldungMitPasswortCheckbox.IsEnabled = false; if (AnmeldungMitPasswortCheckbox.IsChecked == false) AnmeldungMitPasswortCheckbox.IsChecked = true; AnmeldungMitPasswortTextBlock.Opacity = 0.5; }

        private void EinmannbetriebCheckBox_Checked(object sender, RoutedEventArgs e) { AnmeldungMitPasswortCheckbox.IsEnabled = true; AnmeldungMitPasswortTextBlock.Opacity = 1; }

        private void AbholscheineCheckbox_Checked(object sender, RoutedEventArgs e) { AbholdatumCheckbox.IsEnabled = true; AbholdatumTextBlock.Opacity = 1; VollstandigCheckbox.IsEnabled = true; KleinerZettelCheckbox.IsEnabled = true; }

        private void AbholscheineCheckbox_Unchecked(object sender, RoutedEventArgs e) { AbholdatumCheckbox.IsEnabled = false; AbholdatumCheckbox.IsChecked = false; AbholdatumTextBlock.Opacity = 0.5; VollstandigCheckbox.IsEnabled = false; KleinerZettelCheckbox.IsEnabled = false; if (VollstandigCheckbox.IsChecked == true) KleinerZettelCheckbox.IsChecked = true; }

        private void VollstandigCheckbox_Checked(object sender, RoutedEventArgs e) { if (KleinerZettelCheckbox.IsChecked == true) KleinerZettelCheckbox.IsChecked = false; }

        private void KleinerZettelCheckbox_Checked(object sender, RoutedEventArgs e) { if (VollstandigCheckbox.IsChecked == true) VollstandigCheckbox.IsChecked = false; }

        private void Bondrucker_Checked(object sender, RoutedEventArgs e) { if (A4DruckerCheckbox.IsChecked == true) A4DruckerCheckbox.IsChecked = false; }
        private void A4Drucker_Checked(object sender, RoutedEventArgs e) { if (BondruckerCheckbox.IsChecked == true) BondruckerCheckbox.IsChecked = false; }

        private void ZAbschlagSenden_Checked(object sender, RoutedEventArgs e) { AbschlagSendenTextBox.IsEnabled = true; }
        private void ZAbschlagSenden_Unchecked(object sender, RoutedEventArgs e) { AbschlagSendenTextBox.Text = ""; AbschlagSendenTextBox.IsEnabled = false; }

        private void ZAbschlagDrucken_Checked(object sender, RoutedEventArgs e)
        {
            BondruckerCheckbox.IsChecked = true;
            A4DruckerCheckbox.IsChecked = false;
            BondruckerCheckbox.IsEnabled = true;
            A4DruckerCheckbox.IsEnabled = true;
            BondruckerTextBlock.Opacity = 1;
            A4DruckerTextBlock.Opacity = 1;
        }

        private void ZAbschlagDrucken_Unchecked(object sender, RoutedEventArgs e)
        {
            BondruckerCheckbox.IsChecked = false;
            A4DruckerCheckbox.IsChecked = false;
            BondruckerCheckbox.IsEnabled = false;
            A4DruckerCheckbox.IsEnabled = false;
            BondruckerTextBlock.Opacity = 0.7;
            A4DruckerTextBlock.Opacity = 0.7;
        }

        private void A4Drucker_Unchecked(object sender, RoutedEventArgs e) { if (BondruckerCheckbox.IsChecked == false && ZAbschlagDrucken.IsChecked == true) A4DruckerCheckbox.IsChecked = true; }
        private void Bondrucker_Unchecked(object sender, RoutedEventArgs e) { if (A4DruckerCheckbox.IsChecked == false && ZAbschlagDrucken.IsChecked == true) BondruckerCheckbox.IsChecked = true; }

        private void VollstandigCheckbox_Unchecked(object sender, RoutedEventArgs e) { if (KleinerZettelCheckbox.IsChecked == false && AbholscheineCheckbox.IsChecked == true) VollstandigCheckbox.IsChecked = true; }
        private void KleinerZettelCheckbox_Unchecked(object sender, RoutedEventArgs e) { if (VollstandigCheckbox.IsChecked == false && AbholscheineCheckbox.IsChecked == true) KleinerZettelCheckbox.IsChecked = true; }

        private void EinzweckRadioButton_Checked(object sender, RoutedEventArgs e) { EinzweckComboBox.IsEnabled = true; EinzweckComboBox.Opacity = 1; }

        private void MehrzweckRadioButton_Checked(object sender, RoutedEventArgs e) { EinzweckComboBox.IsEnabled = false; EinzweckComboBox.Opacity = 0.7; }

        private void GutscheinSystem_Unchecked(object sender, RoutedEventArgs e) { GutscheinTextBox.IsEnabled = false; GutscheinTextBox.Text = "25"; MehrzweckRadioButton.IsChecked = true; MehrzweckRadioButton.IsEnabled = false; EinzweckRadioButton.IsEnabled = false; EinzweckComboBox.SelectedIndex = EinzweckComboBox.Items.Count-1;  }

        private void GutscheinSystem_Checked(object sender, RoutedEventArgs e) { GutscheinTextBox.IsEnabled = true; MehrzweckRadioButton.IsEnabled = true; EinzweckRadioButton.IsEnabled = true; }

        private void InfoButton_Clicked(object sender, RoutedEventArgs e) { if (InfoWindow.Visibility == Visibility.Collapsed) { InfoWindow.Visibility = Visibility.Visible; return; } else if (InfoWindow.Visibility == Visibility.Visible) InfoWindow.Visibility = Visibility.Collapsed; }

        private void InfoWindow_CloseButtonClicked(object sender, RoutedEventArgs e) { InfoWindow.Visibility = Visibility.Collapsed; }

        private void BenachrichtigenCheckbox_Unchecked(object sender, RoutedEventArgs e) { BenachrichtigenStuck.IsEnabled = false; BenachrichtigenStuck.Text = ""; }

        private void BenachrichtigenCheckbox_Checked(object sender, RoutedEventArgs e) { BenachrichtigenStuck.IsEnabled = true; }
    }
}
