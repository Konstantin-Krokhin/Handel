﻿using System;
using System.Collections.Generic;
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

namespace HandelTSE
{
    /// <summary>
    /// Interaction logic for Zahlungen.xaml
    /// </summary>
    public partial class Zahlungen : UserControl
    {
        Zahlung data = new Zahlung();
        List<Zahlung> list = new List<Zahlung>();
        public static OleDbConnection con = new OleDbConnection();
        public List<Zahlung> Data { get; set; }
        public class Zahlung
        {
            public Int32 Id { get; set; }
            public Int32 Nr { get; set; }
            public string Zahlungsmethode { get; set; }
            public string Status { get; set; }
            public string ZArt { get; set; }
            public string Bemerkung { get; set; }
            public string BargeldCheck { get; set; }
            public string KassenladeCheck { get; set; }
            public string AbfrageCheck { get; set; }
        }
        public Zahlungen()
        {
            InitializeComponent();
            
            // If the menu Zahlungen is being open multiple times
            if (con.ConnectionString.Length == 0)
            {
                con = MainWindow.con;
                OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_Zahlungen] ([Id] COUNTER, [Nr] INT,[Zahlungsmethode] TEXT(55),[Status] TEXT(55),[ZArt] TEXT(55),[Bemerkung] TEXT(55), [BargeldCheck] TEXT(1), [KassenladeCheck] TEXT(1), [AbfrageCheck] TEXT(1))", con);
                try
                {
                    cmd.ExecuteNonQuery();
                    //Create first empty record with the proper starting ID starting from 1
                    OleDbCommand cmd2 = new OleDbCommand("insert into [TBL_Zahlungen](Id, Nr)Values('" + 1 + "','" + 1 + "')", con);
                    try { cmd2.ExecuteNonQuery(); }
                    catch { }
                }
                catch { }
            }

            if (MainWindow.con.State != System.Data.ConnectionState.Closed) LoadGrid();
        }

        private void LoadGrid()
        {
            OleDbCommand cmd2 = new OleDbCommand("SELECT * FROM [TBL_Zahlungen]", con);
            OleDbDataReader myReader = cmd2.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["Id"] != DBNull.Value) data.Id = (Int32)myReader["Id"]; else data.Id = -1;
                if (myReader["Nr"] != DBNull.Value) data.Nr = (Int32)myReader["Nr"]; else data.Nr = 0;
                if (myReader["Zahlungsmethode"] != DBNull.Value) data.Zahlungsmethode = (string)myReader["Zahlungsmethode"]; else data.Zahlungsmethode = "";
                if (myReader["Status"] != DBNull.Value) data.Status = (string)myReader["Status"]; else data.Status = "";
                if (myReader["ZArt"] != DBNull.Value) data.ZArt = (string)myReader["ZArt"]; else data.ZArt = "";
                if (myReader["Bemerkung"] != DBNull.Value) data.Bemerkung = (string)myReader["Bemerkung"]; else data.Bemerkung = "";
                if (myReader["BargeldCheck"] != DBNull.Value) data.BargeldCheck = (string)myReader["BargeldCheck"]; else data.BargeldCheck = "";
                if (myReader["KassenladeCheck"] != DBNull.Value) data.KassenladeCheck = (string)myReader["KassenladeCheck"]; else data.KassenladeCheck = "";
                if (myReader["AbfrageCheck"] != DBNull.Value) data.AbfrageCheck = (string)myReader["AbfrageCheck"]; else data.AbfrageCheck = "";
                list.Add(new Zahlung { Id = data.Id, Nr = data.Nr, Zahlungsmethode = data.Zahlungsmethode, Status = data.Status, ZArt = data.ZArt, Bemerkung = data.Bemerkung, BargeldCheck = data.BargeldCheck, KassenladeCheck = data.KassenladeCheck, AbfrageCheck = data.AbfrageCheck });
            }
            Data = list;
            ZahlungenDataGrid.ItemsSource = Data;
            list = new List<Zahlung>();
        }

        private void CustomizeHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e) { if (e.Column.Header.ToString() == "Nr") e.Column.Header = "Nr."; if (e.Column.Header.ToString() == "ZArt") e.Column.Header = "Z-Art"; }

        private void NeuZahlungsmethode_Click(object sender, RoutedEventArgs e) { NeuZahlungWindow.Visibility = Visibility.Visible; ZahlungenDataGrid.SelectedItem = null; NeuZahlungsmethodeButton.IsEnabled = false; Zahlungsname.Text = ""; Zahlungsart.SelectedIndex = 0; Bemerkung.Text = ""; BargeldCheckbox.IsChecked = false; KassenladeCheckbox.IsChecked = false; AbfrageCheckbox.IsChecked = false; StatusButton.Content = "aktivieren"; }

        private void ZahlungenDataGrid_Loaded(object sender, RoutedEventArgs e) { HideColumns(); }

        private void HideColumns() { if (ZahlungenDataGrid.Items.Count > 0 && ZahlungenDataGrid.Columns.Count > 0) foreach (var item in ZahlungenDataGrid.Columns) { if (item.Header.ToString() == "Id" || item.Header.ToString() == "BargeldCheck" || item.Header.ToString() == "KassenladeCheck" || item.Header.ToString() == "AbfrageCheck") item.Visibility = Visibility.Collapsed; } }

        private void RecordSelected(object sender, SelectionChangedEventArgs e) 
        {
            NeuZahlungsmethodeButton.IsEnabled = false; 
            if (NeuZahlungWindow.Visibility != Visibility.Visible) NeuZahlungWindow.Visibility = Visibility.Visible; 
            ZahlungenDataGrid.IsEnabled = false;

            if (ZahlungenDataGrid.SelectedItem != null)
            {
                Zahlung item = ((Zahlung)ZahlungenDataGrid.SelectedItem);
                Zahlungsname.Text = item.Zahlungsmethode;
                Zahlungsart.Text = item.ZArt;
                Bemerkung.Text = item.Bemerkung;
                if (item.Status == "inaktiv") StatusButton.Content = "aktivieren";
                else StatusButton.Content = "deaktivieren";
                if (item.BargeldCheck == "1") BargeldCheckbox.IsChecked = true;
                if (item.KassenladeCheck == "1") KassenladeCheckbox.IsChecked = true;
                if (item.AbfrageCheck == "1") AbfrageCheckbox.IsChecked = true;
            }
        }

        private void NeuZahlungWindow_CloseButtonClicked(object sender, RoutedEventArgs e) { NeuZahlungWindow.Visibility = Visibility.Collapsed; NeuZahlungsmethodeButton.IsEnabled = true; ZahlungenDataGrid.IsEnabled = true; StatusButton.Content = "aktivieren"; }

        private void speichern_Click(object sender, RoutedEventArgs e)
        {
            Int32 ID = 0;
            Int32 Nr = 0;
            int result = 0;
            string bargeldcheck = "0", kassenladecheck = "0", abfragecheck = "0", buttonstatus = "inaktiv";
            Zahlung item = null;

            if (BargeldCheckbox.IsChecked == true) bargeldcheck = "1";
            if (KassenladeCheckbox.IsChecked == true) kassenladecheck = "1";
            if (AbfrageCheckbox.IsChecked == true) abfragecheck = "1";
            if (StatusButton.Content.ToString() == "deaktivieren") buttonstatus = "aktiv";
            if (ZahlungenDataGrid.SelectedItem != null) item = ((Zahlung)ZahlungenDataGrid.SelectedItem);
            OleDbCommand cmd = new OleDbCommand();
            if (item != null)
            {
                ID = item.Id;
                cmd = new OleDbCommand("UPDATE [TBL_Zahlungen] SET Nr = @Nr, Zahlungsmethode = @Zahlungsmethode, Status = @Status, ZArt = @ZArt, Bemerkung = @Bemerkung, BargeldCheck = @BargeldCheck, KassenladeCheck = @KassenladeCheck, AbfrageCheck = @AbfrageCheck WHERE Id = @ID", con);

                cmd.Parameters.Add(new OleDbParameter("@Nr", item.Nr));
                cmd.Parameters.Add(new OleDbParameter("@Zahlungsmethode", Zahlungsname.Text));
                cmd.Parameters.Add(new OleDbParameter("@Status", buttonstatus));
                cmd.Parameters.Add(new OleDbParameter("@ZArt", Zahlungsart.Text));
                cmd.Parameters.Add(new OleDbParameter("@Bemerkung", Bemerkung.Text));
                cmd.Parameters.Add(new OleDbParameter("@BargeldCheck", bargeldcheck));
                cmd.Parameters.Add(new OleDbParameter("@KassenladeCheck", kassenladecheck));
                cmd.Parameters.Add(new OleDbParameter("@AbfrageCheck", abfragecheck));
                
                cmd.Parameters.Add(new OleDbParameter("@ID", ID));
            }
            else
            {
                OleDbCommand maxCommand = new OleDbCommand("SELECT max(Id) from TBL_Zahlungen", con);
                try { ID = (Int32)maxCommand.ExecuteScalar(); } catch { }
                OleDbCommand maxCommand2 = new OleDbCommand("SELECT max(Nr) from TBL_Zahlungen", con);
                try { Nr = (Int32)maxCommand2.ExecuteScalar(); } catch { }
                cmd = new OleDbCommand("insert into [TBL_Zahlungen](Id, Nr, Zahlungsmethode, Status, ZArt, Bemerkung, BargeldCheck, KassenladeCheck, AbfrageCheck)Values('" + ++ID + "','" + ++Nr + "','" + Zahlungsname.Text + "','"  + buttonstatus + "','" + Zahlungsart.Text  + "','" + Bemerkung.Text + "','" + bargeldcheck + "','" + kassenladecheck + "','" + abfragecheck + "')", con);
            }

            try { result = cmd.ExecuteNonQuery(); LoadGrid(); HideColumns(); }
            catch { MessageBox.Show("Bitte stellen Sie sicher, dass die Verbindung zur Datenbank hergestellt ist und der erforderliche Treiber für Microsoft Access 2010 installiert ist oder der Datentyp der Datenbankspalte mit den Daten im Formular übereinstimmt."); }

            if (NeuZahlungWindow.Visibility == Visibility.Visible) NeuZahlungWindow.Visibility = Visibility.Collapsed;
            ZahlungenDataGrid.IsEnabled = true;
            NeuZahlungsmethodeButton.IsEnabled = true;
            StatusButton.Content = "aktivieren";
        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusButton.Content.ToString() == "deaktivieren") StatusButton.Content = "aktivieren";
            else StatusButton.Content = "deaktivieren";
        }
    }
}