using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            double bill;
            Double.TryParse(BillBox.Text, out bill);
            double tip;
            Double.TryParse(PercentBox.Text, out tip);
            bill *= tip/100;
            TipBox.Text = bill.ToString();
        }
    }
}
