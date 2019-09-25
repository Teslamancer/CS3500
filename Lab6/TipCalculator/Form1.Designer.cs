namespace TipCalculator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.BillBox = new System.Windows.Forms.TextBox();
            this.TipBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PercentBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TotalBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label1.Location = new System.Drawing.Point(24, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Bill";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // BillBox
            // 
            this.BillBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.BillBox.Location = new System.Drawing.Point(335, 28);
            this.BillBox.Name = "BillBox";
            this.BillBox.Size = new System.Drawing.Size(292, 53);
            this.BillBox.TabIndex = 1;
            this.BillBox.TextChanged += new System.EventHandler(this.BillBox_TextChanged);
            // 
            // TipBox
            // 
            this.TipBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.TipBox.Location = new System.Drawing.Point(335, 163);
            this.TipBox.Name = "TipBox";
            this.TipBox.ReadOnly = true;
            this.TipBox.Size = new System.Drawing.Size(292, 53);
            this.TipBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label2.Location = new System.Drawing.Point(24, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(290, 46);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tip Percentage";
            // 
            // PercentBox
            // 
            this.PercentBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.PercentBox.Location = new System.Drawing.Point(335, 93);
            this.PercentBox.Name = "PercentBox";
            this.PercentBox.Size = new System.Drawing.Size(292, 53);
            this.PercentBox.TabIndex = 5;
            this.PercentBox.TextChanged += new System.EventHandler(this.PercentBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label3.Location = new System.Drawing.Point(24, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 46);
            this.label3.TabIndex = 6;
            this.label3.Text = "Total";
            // 
            // TotalBox
            // 
            this.TotalBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.TotalBox.Location = new System.Drawing.Point(335, 228);
            this.TotalBox.Name = "TotalBox";
            this.TotalBox.ReadOnly = true;
            this.TotalBox.Size = new System.Drawing.Size(292, 53);
            this.TotalBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.label4.Location = new System.Drawing.Point(24, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(224, 46);
            this.label4.TabIndex = 8;
            this.label4.Text = "Tip Amount";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TotalBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PercentBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TipBox);
            this.Controls.Add(this.BillBox);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Tip Calculator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox BillBox;
        private System.Windows.Forms.TextBox TipBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PercentBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TotalBox;
        private System.Windows.Forms.Label label4;
    }
}

