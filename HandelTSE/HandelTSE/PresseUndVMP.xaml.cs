using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
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
    /// Interaction logic for PresseUndVMP.xaml
    /// </summary>
    public partial class PresseUndVMP : UserControl
    {
        Presse data = new Presse();
        List<Presse> list = new List<Presse>();
        public static OleDbConnection con = new OleDbConnection();
        public List<Presse> Data { get; set; }

        public class Presse
        {
            public string CEAN { get; set; }
            public string CNAME { get; set; }
        }

        public PresseUndVMP()
        {
            InitializeComponent();

            // If the menu PresseUndVMP is being open multiple times
            if (con.ConnectionString.Length == 0)
            {
                string str = "";
                con = MainWindow.con;
                OleDbCommand cmd = new OleDbCommand("CREATE TABLE [TBL_PRESSE] ([CEAN] TEXT(55), [CNAME] TEXT(55))", con);
                try
                {
                    cmd.ExecuteNonQuery();

                    // Create a table with almost all existing German newspapers and their EAN codes from Presse_liste file with SQL commands inside
                    int i = 0;
                    string commands = File.ReadAllText("../Debug/Presse_liste.txt");
                    while (i < commands.Length)
                    {
                        // Delete \r and \n from the string
                        if (commands[i] == '\r' || commands[i] == '\n')
                        {
                            commands = commands.Remove(i, 1);
                            continue;
                        }
                        i++;
                    }
                    str = commands;

                    // Separate SQL commands from each other
                    string[] sqlStatements = str.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    // Run all commands for inserting the records
                    if (con.State == System.Data.ConnectionState.Closed) con.Open();
                    OleDbTransaction transaction = con.BeginTransaction();
                    foreach (string statement in sqlStatements)
                    {
                        using (OleDbCommand cmd2 = new OleDbCommand(statement, con, transaction))
                        {
                            cmd2.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch { }
            }

            if (MainWindow.con.State != System.Data.ConnectionState.Closed) LoadGrid(); 
        }

        private void LoadGrid()
        {
            OleDbCommand cmd2 = new OleDbCommand("SELECT * FROM [TBL_PRESSE]", con);
            OleDbDataReader myReader = cmd2.ExecuteReader();
            while (myReader.Read())
            {
                if (myReader["CEAN"] != DBNull.Value) data.CEAN= (string)myReader["CEAN"]; else data.CEAN = "";
                if (myReader["CNAME"] != DBNull.Value) data.CNAME = (string)myReader["CNAME"]; else data.CNAME = "";
                list.Add(new Presse { CEAN = data.CEAN, CNAME = data.CNAME });
            }
            Data = list;
            EANDataGrid.ItemsSource = Data;
            list = new List<Presse>();
        }

        private void EANPressecodeDataGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void RecordSelected(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NeuPresseCode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoschenPresseCode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void speichernPresseCode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NeuZeitung_Click(object sender, RoutedEventArgs e)
        {
            LoschenButton.IsEnabled = false;
        }
    }
}
