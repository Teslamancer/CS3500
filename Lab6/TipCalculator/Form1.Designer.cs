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
            this.CalculateButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.PercentBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(145, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Total Bill";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // BillBox
            // 
            this.BillBox.Location = new System.Drawing.Point(350, 61);
            this.BillBox.Name = "BillBox";
            this.BillBox.Size = new System.Drawing.Size(277, 26);
            this.BillBox.TabIndex = 1;
            // 
            // TipBox
            // 
            this.TipBox.Location = new System.Drawing.Point(350, 152);
            this.TipBox.Name = "TipBox";
            this.TipBox.Size = new System.Drawing.Size(277, 26);
            this.TipBox.TabIndex = 2;
            // 
            // CalculateButton
            // 
            this.CalculateButton.Location = new System.Drawing.Point(138, 152);
            this.CalculateButton.Name = "CalculateButton";
            this.CalculateButton.Size = new System.Drawing.Size(118, 26);
            this.CalculateButton.TabIndex = 3;
            this.CalculateButton.Text = "Calculate Tip";
            this.CalculateButton.UseVisualStyleBackColor = true;
            this.CalculateButton.Click += new System.EventHandler(this.CalculateButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tip Percentage";
            // 
            // PercentBox
            // 
            this.PercentBox.Location = new System.Drawing.Point(350, 106);
            this.PercentBox.Name = "PercentBox";
            this.PercentBox.Size = new System.Drawing.Size(277, 26);
            this.PercentBox.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PercentBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CalculateButton);
            this.Controls.Add(this.TipBox);
            this.Controls.Add(this.BillBox);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox BillBox;
        private System.Windows.Forms.TextBox TipBox;
        private System.Windows.Forms.Button CalculateButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PercentBox;
    }
}

