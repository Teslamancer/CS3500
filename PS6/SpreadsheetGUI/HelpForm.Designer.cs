namespace SpreadsheetGUI
{
    partial class HelpForm
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
            this.helpItemList = new System.Windows.Forms.ListBox();
            this.HelpContent = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // helpItemList
            // 
            this.helpItemList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.helpItemList.FormattingEnabled = true;
            this.helpItemList.ItemHeight = 16;
            this.helpItemList.Items.AddRange(new object[] {
            "Save",
            "Open",
            "Formula",
            "Shortcuts",
            "Autosave",
            "Editing Cell Contents",
            "Navigating"});
            this.helpItemList.Location = new System.Drawing.Point(16, 15);
            this.helpItemList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.helpItemList.Name = "helpItemList";
            this.helpItemList.Size = new System.Drawing.Size(159, 532);
            this.helpItemList.TabIndex = 0;
            this.helpItemList.SelectedIndexChanged += new System.EventHandler(this.helpItemList_SelectedIndexChanged);
            // 
            // HelpContent
            // 
            this.HelpContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HelpContent.Location = new System.Drawing.Point(184, 15);
            this.HelpContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.HelpContent.Name = "HelpContent";
            this.HelpContent.ReadOnly = true;
            this.HelpContent.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.HelpContent.Size = new System.Drawing.Size(619, 532);
            this.HelpContent.TabIndex = 1;
            this.HelpContent.Text = "";
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 554);
            this.Controls.Add(this.HelpContent);
            this.Controls.Add(this.helpItemList);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "HelpForm";
            this.Text = "Help";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox helpItemList;
        private System.Windows.Forms.RichTextBox HelpContent;
    }
}