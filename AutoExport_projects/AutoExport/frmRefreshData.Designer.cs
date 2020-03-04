namespace AutoExport
{
    partial class frmRefreshData
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
            this.txtSec = new System.Windows.Forms.TextBox();
            this.lblSec = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.bckLoadData = new System.ComponentModel.BackgroundWorker();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pctHourGlass)).BeginInit();
            this.SuspendLayout();
            // 
            // pctHourGlass
            // 
            this.pctHourGlass.Image = global::AutoExport.Properties.Resources.hourglass;
            this.pctHourGlass.Location = new System.Drawing.Point(12, 12);
            this.pctHourGlass.Name = "pctHourGlass";
            this.pctHourGlass.Size = new System.Drawing.Size(92, 213);
            this.pctHourGlass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pctHourGlass.TabIndex = 1;
            this.pctHourGlass.TabStop = false;
            // 
            // txtSec
            // 
            this.txtSec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSec.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSec.ForeColor = System.Drawing.Color.Blue;
            this.txtSec.Location = new System.Drawing.Point(173, 85);
            this.txtSec.Name = "txtSec";
            this.txtSec.Size = new System.Drawing.Size(35, 26);
            this.txtSec.TabIndex = 10;
            this.txtSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSec
            // 
            this.lblSec.AutoSize = true;
            this.lblSec.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSec.ForeColor = System.Drawing.Color.Blue;
            this.lblSec.Location = new System.Drawing.Point(128, 85);
            this.lblSec.Name = "lblSec";
            this.lblSec.Size = new System.Drawing.Size(40, 18);
            this.lblSec.TabIndex = 9;
            this.lblSec.Text = "Sec:";
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMin.ForeColor = System.Drawing.Color.Blue;
            this.lblMin.Location = new System.Drawing.Point(128, 50);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(37, 18);
            this.lblMin.TabIndex = 8;
            this.lblMin.Text = "Min:";
            // 
            // txtMin
            // 
            this.txtMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMin.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMin.ForeColor = System.Drawing.Color.Blue;
            this.txtMin.Location = new System.Drawing.Point(173, 50);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(35, 26);
            this.txtMin.TabIndex = 7;
            this.txtMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(130, 12);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(108, 18);
            this.lblTime.TabIndex = 6;
            this.lblTime.Text = "Elapsed Time:";
            // 
            // bckLoadData
            // 
            this.bckLoadData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckLoadData_DoWork);
            // 
            // txtMsg
            // 
            this.txtMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMsg.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMsg.Location = new System.Drawing.Point(220, 50);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(104, 42);
            this.txtMsg.TabIndex = 11;
            this.txtMsg.Text = "This can take up to 2 min.";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnStart.Location = new System.Drawing.Point(133, 198);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 25);
            this.btnStart.TabIndex = 13;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // frmRefreshData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(363, 233);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.txtSec);
            this.Controls.Add(this.lblSec);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.txtMin);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.pctHourGlass);
            this.Name = "frmRefreshData";
            this.Text = "Refresh Test data";
            ((System.ComponentModel.ISupportInitialize)(this.pctHourGlass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctHourGlass;
        private System.Windows.Forms.TextBox txtSec;
        private System.Windows.Forms.Label lblSec;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.Label lblTime;
        private System.ComponentModel.BackgroundWorker bckLoadData;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button btnStart;
    }
}