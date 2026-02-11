namespace PackingOrderDownloaderApp
{
    partial class DownloadPackingOrderForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.lblClock = new System.Windows.Forms.Label();
            this.btnDownloadNow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbLogs
            // 
            this.rtbLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbLogs.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLogs.Location = new System.Drawing.Point(5, 8);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.Size = new System.Drawing.Size(613, 380);
            this.rtbLogs.TabIndex = 0;
            this.rtbLogs.Text = "";
            // 
            // lblClock
            // 
            this.lblClock.AutoSize = true;
            this.lblClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(231)))), ((int)(((byte)(188)))));
            this.lblClock.Location = new System.Drawing.Point(12, 410);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(79, 20);
            this.lblClock.TabIndex = 1;
            this.lblClock.Text = "00:00:00";
            // 
            // btnDownloadNow
            // 
            this.btnDownloadNow.Location = new System.Drawing.Point(498, 400);
            this.btnDownloadNow.Name = "btnDownloadNow";
            this.btnDownloadNow.Size = new System.Drawing.Size(120, 30);
            this.btnDownloadNow.TabIndex = 2;
            this.btnDownloadNow.Text = "Download Now";
            this.btnDownloadNow.UseVisualStyleBackColor = true;
            this.btnDownloadNow.Click += new System.EventHandler(this.btnDownloadNow_Click);
            // 
            // DownloadPackingOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(45)))), ((int)(((byte)(114)))));
            this.ClientSize = new System.Drawing.Size(628, 444);
            this.Controls.Add(this.btnDownloadNow);
            this.Controls.Add(this.lblClock);
            this.Controls.Add(this.rtbLogs);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DownloadPackingOrderForm";
            this.Text = "Packing Order Downloader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.Button btnDownloadNow;
    }
}