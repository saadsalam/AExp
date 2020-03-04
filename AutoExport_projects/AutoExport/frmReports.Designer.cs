namespace AutoExport
{
    partial class frmReports
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
            this.pnlHandheldFile = new System.Windows.Forms.Panel();
            this.btnNoLabels = new System.Windows.Forms.Button();
            this.btnVehsOnHoldExcel = new System.Windows.Forms.Button();
            this.btnVoyPushCarList = new System.Windows.Forms.Button();
            this.btnGroundedLaneExcel = new System.Windows.Forms.Button();
            this.btnGroundedLane = new System.Windows.Forms.Button();
            this.btnGroundedSummary = new System.Windows.Forms.Button();
            this.btnPrintCustomsSub = new System.Windows.Forms.Button();
            this.btnLabels = new System.Windows.Forms.Button();
            this.btnCusClearedCoversheet = new System.Windows.Forms.Button();
            this.btnCustomsCleared = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnMenu = new System.Windows.Forms.Button();
            this.pnlHandheldFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightGray;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(50, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Export Reports";
            // 
            // pnlHandheldFile
            // 
            this.pnlHandheldFile.BackColor = System.Drawing.Color.White;
            this.pnlHandheldFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHandheldFile.Controls.Add(this.btnNoLabels);
            this.pnlHandheldFile.Controls.Add(this.btnVehsOnHoldExcel);
            this.pnlHandheldFile.Controls.Add(this.btnVoyPushCarList);
            this.pnlHandheldFile.Controls.Add(this.btnGroundedLaneExcel);
            this.pnlHandheldFile.Controls.Add(this.btnGroundedLane);
            this.pnlHandheldFile.Controls.Add(this.btnGroundedSummary);
            this.pnlHandheldFile.Controls.Add(this.btnPrintCustomsSub);
            this.pnlHandheldFile.Controls.Add(this.btnLabels);
            this.pnlHandheldFile.Controls.Add(this.btnCusClearedCoversheet);
            this.pnlHandheldFile.Controls.Add(this.btnCustomsCleared);
            this.pnlHandheldFile.Location = new System.Drawing.Point(45, 10);
            this.pnlHandheldFile.Name = "pnlHandheldFile";
            this.pnlHandheldFile.Size = new System.Drawing.Size(223, 340);
            this.pnlHandheldFile.TabIndex = 1;
            this.pnlHandheldFile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlHandheldFile_MouseMove);
            // 
            // btnNoLabels
            // 
            this.btnNoLabels.Location = new System.Drawing.Point(190, 90);
            this.btnNoLabels.Name = "btnNoLabels";
            this.btnNoLabels.Size = new System.Drawing.Size(25, 25);
            this.btnNoLabels.TabIndex = 74;
            this.btnNoLabels.Text = "?";
            this.btnNoLabels.UseVisualStyleBackColor = true;
            this.btnNoLabels.Visible = false;
            this.btnNoLabels.Click += new System.EventHandler(this.btnNoLabels_Click);
            // 
            // btnVehsOnHoldExcel
            // 
            this.btnVehsOnHoldExcel.Location = new System.Drawing.Point(5, 265);
            this.btnVehsOnHoldExcel.Name = "btnVehsOnHoldExcel";
            this.btnVehsOnHoldExcel.Size = new System.Drawing.Size(210, 25);
            this.btnVehsOnHoldExcel.TabIndex = 12;
            this.btnVehsOnHoldExcel.Text = "Vehicles On Hold (Excel)";
            this.btnVehsOnHoldExcel.UseVisualStyleBackColor = true;
            this.btnVehsOnHoldExcel.Click += new System.EventHandler(this.btnVehsOnHoldExcel_Click);
            // 
            // btnVoyPushCarList
            // 
            this.btnVoyPushCarList.Location = new System.Drawing.Point(5, 300);
            this.btnVoyPushCarList.Name = "btnVoyPushCarList";
            this.btnVoyPushCarList.Size = new System.Drawing.Size(210, 25);
            this.btnVoyPushCarList.TabIndex = 11;
            this.btnVoyPushCarList.Text = "Voyage Push Car List";
            this.btnVoyPushCarList.UseVisualStyleBackColor = true;
            this.btnVoyPushCarList.Click += new System.EventHandler(this.btnVoyPushCarList_Click);
            // 
            // btnGroundedLaneExcel
            // 
            this.btnGroundedLaneExcel.Location = new System.Drawing.Point(5, 195);
            this.btnGroundedLaneExcel.Name = "btnGroundedLaneExcel";
            this.btnGroundedLaneExcel.Size = new System.Drawing.Size(210, 25);
            this.btnGroundedLaneExcel.TabIndex = 7;
            this.btnGroundedLaneExcel.Text = "Grounded Lane Sum. (Excel)";
            this.btnGroundedLaneExcel.UseVisualStyleBackColor = true;
            this.btnGroundedLaneExcel.Click += new System.EventHandler(this.btnGroundedLaneExcel_Click);
            // 
            // btnGroundedLane
            // 
            this.btnGroundedLane.Location = new System.Drawing.Point(5, 160);
            this.btnGroundedLane.Name = "btnGroundedLane";
            this.btnGroundedLane.Size = new System.Drawing.Size(210, 25);
            this.btnGroundedLane.TabIndex = 6;
            this.btnGroundedLane.Text = "Grounded Lane Summary";
            this.btnGroundedLane.UseVisualStyleBackColor = true;
            this.btnGroundedLane.Click += new System.EventHandler(this.btnGroundedLane_Click);
            // 
            // btnGroundedSummary
            // 
            this.btnGroundedSummary.Location = new System.Drawing.Point(5, 125);
            this.btnGroundedSummary.Name = "btnGroundedSummary";
            this.btnGroundedSummary.Size = new System.Drawing.Size(210, 25);
            this.btnGroundedSummary.TabIndex = 5;
            this.btnGroundedSummary.Text = "Grounded Summary";
            this.btnGroundedSummary.UseVisualStyleBackColor = true;
            this.btnGroundedSummary.Click += new System.EventHandler(this.btnGroundedSummary_Click);
            // 
            // btnPrintCustomsSub
            // 
            this.btnPrintCustomsSub.Location = new System.Drawing.Point(5, 230);
            this.btnPrintCustomsSub.Name = "btnPrintCustomsSub";
            this.btnPrintCustomsSub.Size = new System.Drawing.Size(210, 25);
            this.btnPrintCustomsSub.TabIndex = 4;
            this.btnPrintCustomsSub.Text = "Print Customs Submitted";
            this.btnPrintCustomsSub.UseVisualStyleBackColor = true;
            this.btnPrintCustomsSub.Click += new System.EventHandler(this.btnPrintCustomsSub_Click);
            // 
            // btnLabels
            // 
            this.btnLabels.Location = new System.Drawing.Point(5, 90);
            this.btnLabels.Name = "btnLabels";
            this.btnLabels.Size = new System.Drawing.Size(210, 25);
            this.btnLabels.TabIndex = 2;
            this.btnLabels.Text = "Labels";
            this.btnLabels.UseVisualStyleBackColor = true;
            this.btnLabels.Click += new System.EventHandler(this.btnLabels_Click);
            // 
            // btnCusClearedCoversheet
            // 
            this.btnCusClearedCoversheet.Location = new System.Drawing.Point(5, 55);
            this.btnCusClearedCoversheet.Name = "btnCusClearedCoversheet";
            this.btnCusClearedCoversheet.Size = new System.Drawing.Size(210, 25);
            this.btnCusClearedCoversheet.TabIndex = 1;
            this.btnCusClearedCoversheet.Text = "Customs Cleared Coversheet";
            this.btnCusClearedCoversheet.UseVisualStyleBackColor = true;
            this.btnCusClearedCoversheet.Click += new System.EventHandler(this.btnCusClearedCoversheet_Click);
            // 
            // btnCustomsCleared
            // 
            this.btnCustomsCleared.Location = new System.Drawing.Point(5, 20);
            this.btnCustomsCleared.Name = "btnCustomsCleared";
            this.btnCustomsCleared.Size = new System.Drawing.Size(210, 25);
            this.btnCustomsCleared.TabIndex = 0;
            this.btnCustomsCleared.Text = "Customs Cleared";
            this.btnCustomsCleared.UseVisualStyleBackColor = true;
            this.btnCustomsCleared.Click += new System.EventHandler(this.btnCustomsCleared_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 350);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            // 
            // btnMenu
            // 
            this.btnMenu.Image = global::AutoExport.Properties.Resources.Menu1;
            this.btnMenu.Location = new System.Drawing.Point(5, 10);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 30);
            this.btnMenu.TabIndex = 72;
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // frmReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(274, 352);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlHandheldFile);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmReports";
            this.Text = "DAI Export: REPORTS";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmReports_MouseMove);
            this.pnlHandheldFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlHandheldFile;
        private System.Windows.Forms.Button btnLabels;
        private System.Windows.Forms.Button btnCusClearedCoversheet;
        private System.Windows.Forms.Button btnCustomsCleared;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnPrintCustomsSub;
        private System.Windows.Forms.Button btnGroundedSummary;
        private System.Windows.Forms.Button btnGroundedLane;
        private System.Windows.Forms.Button btnGroundedLaneExcel;
        private System.Windows.Forms.Button btnVoyPushCarList;
        private System.Windows.Forms.Button btnVehsOnHoldExcel;
        private System.Windows.Forms.Button btnNoLabels;
    }
}