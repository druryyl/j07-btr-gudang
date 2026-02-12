namespace BtrGudang.Winform
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.packingOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PT1DownloadPackingOrderMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.packingOrderToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(939, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // packingOrderToolStripMenuItem
            // 
            this.packingOrderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PT1DownloadPackingOrderMenu});
            this.packingOrderToolStripMenuItem.Name = "packingOrderToolStripMenuItem";
            this.packingOrderToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.packingOrderToolStripMenuItem.Text = "Packing Order";
            // 
            // PT1DownloadPackingOrderMenu
            // 
            this.PT1DownloadPackingOrderMenu.Name = "PT1DownloadPackingOrderMenu";
            this.PT1DownloadPackingOrderMenu.Size = new System.Drawing.Size(246, 22);
            this.PT1DownloadPackingOrderMenu.Text = "PT1 - Download Packing Order...";
            this.PT1DownloadPackingOrderMenu.Click += new System.EventHandler(this.PT1DownloadPackingOrderMenu_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 626);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem packingOrderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PT1DownloadPackingOrderMenu;
    }
}

