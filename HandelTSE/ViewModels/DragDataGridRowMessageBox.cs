using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandelTSE.ViewModels
{
    public partial class DragDataGridRowMessageBox : Form
    {
        public DragDataGridRowMessageBox(string message, string buttonText1, string buttonText2, string buttonText3)
        {
            InitializeComponent();

            label1.Text = message;
            button1.Text = buttonText1;
            button2.Text = buttonText2;
            button3.Text = buttonText3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserChoice = "kopieren";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserChoice = "verschieben";
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserChoice = "abbrechen";
            this.Close();
        }
    }
}
