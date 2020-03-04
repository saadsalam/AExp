namespace AutoExport
{
    partial class frmTimeOutNotice
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
            this.pctHourGlass = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnCancelTimeout = new System.Windows.Forms.Button();
            this.bckShowsecs = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pctHourGlass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pctHourGlass
            // 
            this.pctHourGlass.Image = global::AutoExport.Properties.Resources.hourglass;
            this.pctHourGlass.Location = new System.Drawing.Point(45, 5);
            this.pctHourGlass.Name = "pctHourGlass";
            this.pctHourGlass.Size = new System.Drawing.Size(92, 213);
            this.pctHourGlass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pctHourGlass.TabIndex = 2;
            this.pctHourGlass.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 230);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 74;
            this.pictureBox2.TabStop = false;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Blue;
            this.lblMsg.Location = new System.Drawing.Point(152, 9);
            this.lblMsg.MinimumSize = new System.Drawing.Size(75, 50);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(296, 72);
            this.lblMsg.TabIndex = 75;
            this.lblMsg.Text = "Due to inactivity the program\r\n will close in 10 seconds.\r\n\r\nClick Cancel Timeout" +
    " to continue working.";
            // 
            // btnCancelTimeout
            // 
            this.btnCancelTimeout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnCancelTimeout.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelTimeout.Location = new System.Drawing.Point(155, 167);
            this.btnCancelTimeout.Name = "btnCancelTimeout";
            this.btnCancelTimeout.Size = new System.Drawing.Size(222, 51);
            this.btnCancelTimeout.TabIndex = 76;
            this.btnCancelTimeout.Text = "Cancel Timeout";
            this.btnCancelTimeout.UseVisualStyleBackColor = false;
            this.btnCancelTimeout.Click += new System.EventHandler(this.btnCancelTimeout_Click);
            // 
            // frmTimeOutNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(459, 232);
            this.Controls.Add(this.btnCancelTimeout);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pctHourGlass);
            this.Name = "frmTimeOutNotice";
            this.Text = "DAI Export - Timing Out";
            ((System.ComponentModel.ISupportInitialize)(this.pctHourGlass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctHourGlass;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnCancelTimeout;
        private System.ComponentModel.BackgroundWorker bckShowsecs;
    }
}