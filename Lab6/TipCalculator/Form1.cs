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
        private static double bill;
        private static double tip;
        private static double tipPercent;
        private static double total;
        private static bool validBillAmount;
        private static bool validTipAmount;
        private void calculate()
        {
            if(validBillAmount && validTipAmount)
            {                
                tip = bill * tipPercent/100;
                total = bill + tip;
                TipBox.Text = tip.ToString();
                TotalBox.Text = total.ToString();
            }
            else
            {
                TipBox.Text = "";
                TotalBox.Text = "";
            }
        }
        public Form1()
        {
            InitializeComponent();
            bill = 0;
            tip = 0;
            validBillAmount = false;
            validTipAmount = false;
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }        

        private void BillBox_TextChanged(object sender, EventArgs e)
        {
            double billamount;
            if (Double.TryParse(BillBox.Text, out billamount))
            {
                bill = billamount;
                validBillAmount = true;
                calculate();
            }
            else
            {
                validBillAmount = false;
                calculate();
            }
        }

        private void PercentBox_TextChanged(object sender, EventArgs e)
        {
            double tipamount;
            if(Double.TryParse(PercentBox.Text, out tipamount))
            {
                tipPercent = tipamount;
                validTipAmount = true;
                calculate();
            }
            else
            {
                validTipAmount = false;
                calculate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
