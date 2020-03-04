namespace AutoExport
{
    partial class frmImportExportMenu
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
            this.btnExpCustApproved = new System.Windows.Forms.Button();
            this.btnImportShp = new System.Windows.Forms.Button();
            this.btnImportPhyClone = new System.Windows.Forms.Button();
            this.btnImportRcvd = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnMenu = new System.Windows.Forms.Button();
            this.pnlInvFile = new System.Windows.Forms.Panel();
            this.btnPostShipInv = new System.Windows.Forms.Button();
            this.btnPreShipInv = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlVoyageFile = new System.Windows.Forms.Panel();
            this.btnExpVoyExceptions = new System.Windows.Forms.Button();
            this.btnExpBookSummary = new System.Windows.Forms.Button();
            this.btnExpBookRecs = new System.Windows.Forms.Button();
            this.btnShipManifest = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlHandheldFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnlInvFile.SuspendLayout();
            this.pnlVoyageFile.SuspendLayout();
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
            this.label1.Text = "Handheld Files";
            // 
            // pnlHandheldFile
            // 
            this.pnlHandheldFile.BackColor = System.Drawing.Color.White;
            this.pnlHandheldFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHandheldFile.Controls.Add(this.btnExpCustApproved);
            this.pnlHandheldFile.Controls.Add(this.btnImportShp);
            this.pnlHandheldFile.Controls.Add(this.btnImportPhyClone);
            this.pnlHandheldFile.Controls.Add(this.btnImportRcvd);
            this.pnlHandheldFile.Location = new System.Drawing.Point(45, 10);
            this.pnlHandheldFile.Name = "pnlHandheldFile";
            this.pnlHandheldFile.Size = new System.Drawing.Size(189, 160);
            this.pnlHandheldFile.TabIndex = 1;
            // 
            // btnExpCustApproved
            // 
            this.btnExpCustApproved.Location = new System.Drawing.Point(5, 125);
            this.btnExpCustApproved.Name = "btnExpCustApproved";
            this.btnExpCustApproved.Size = new System.Drawing.Size(175, 23);
            this.btnExpCustApproved.TabIndex = 4;
            this.btnExpCustApproved.Text = "Export Customs Appv\'d";
            this.btnExpCustApproved.UseVisualStyleBackColor = true;
            this.btnExpCustApproved.Click += new System.EventHandler(this.btnExpCustApproved_Click);
            // 
            // btnImportShp
            // 
            this.btnImportShp.Location = new System.Drawing.Point(5, 90);
            this.btnImportShp.Name = "btnImportShp";
            this.btnImportShp.Size = new System.Drawing.Size(175, 23);
            this.btnImportShp.TabIndex = 2;
            this.btnImportShp.Text = "Import Shipped";
            this.btnImportShp.UseVisualStyleBackColor = true;
            this.btnImportShp.Click += new System.EventHandler(this.btnImportShp_Click);
            // 
            // btnImportPhyClone
            // 
            this.btnImportPhyClone.Location = new System.Drawing.Point(5, 55);
            this.btnImportPhyClone.Name = "btnImportPhyClone";
            this.btnImportPhyClone.Size = new System.Drawing.Size(175, 25);
            this.btnImportPhyClone.TabIndex = 1;
            this.btnImportPhyClone.Text = "Import Phy. Clone";
            this.btnImportPhyClone.UseVisualStyleBackColor = true;
            this.btnImportPhyClone.Click += new System.EventHandler(this.btnImportPhyClone_Click);
            // 
            // btnImportRcvd
            // 
            this.btnImportRcvd.Location = new System.Drawing.Point(5, 20);
            this.btnImportRcvd.Name = "btnImportRcvd";
            this.btnImportRcvd.Size = new System.Drawing.Size(175, 25);
            this.btnImportRcvd.TabIndex = 0;
            this.btnImportRcvd.Text = "Import Rcvd Vehicles";
            this.btnImportRcvd.UseVisualStyleBackColor = true;
            this.btnImportRcvd.Click += new System.EventHandler(this.btnImportRcvd_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 185);
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
            // pnlInvFile
            // 
            this.pnlInvFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInvFile.Controls.Add(this.btnPostShipInv);
            this.pnlInvFile.Controls.Add(this.btnPreShipInv);
            this.pnlInvFile.Location = new System.Drawing.Point(455, 10);
            this.pnlInvFile.Name = "pnlInvFile";
            this.pnlInvFile.Size = new System.Drawing.Size(189, 90);
            this.pnlInvFile.TabIndex = 73;
            // 
            // btnPostShipInv
            // 
            this.btnPostShipInv.Location = new System.Drawing.Point(5, 55);
            this.btnPostShipInv.Name = "btnPostShipInv";
            this.btnPostShipInv.Size = new System.Drawing.Size(175, 25);
            this.btnPostShipInv.TabIndex = 2;
            this.btnPostShipInv.Text = "Post-Ship Inventory";
            this.btnPostShipInv.UseVisualStyleBackColor = true;
            this.btnPostShipInv.Click += new System.EventHandler(this.btnPostShipInv_Click);
            // 
            // btnPreShipInv
            // 
            this.btnPreShipInv.Location = new System.Drawing.Point(5, 20);
            this.btnPreShipInv.Name = "btnPreShipInv";
            this.btnPreShipInv.Size = new System.Drawing.Size(175, 25);
            this.btnPreShipInv.TabIndex = 1;
            this.btnPreShipInv.Text = "Pre-Ship Inventory";
            this.btnPreShipInv.UseVisualStyleBackColor = true;
            this.btnPreShipInv.Click += new System.EventHandler(this.btnPreShipInv_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.LightGray;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(460, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 74;
            this.label2.Text = "Inventory Files";
            // 
            // pnlVoyageFile
            // 
            this.pnlVoyageFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVoyageFile.Controls.Add(this.btnExpVoyExceptions);
            this.pnlVoyageFile.Controls.Add(this.btnExpBookSummary);
            this.pnlVoyageFile.Controls.Add(this.btnExpBookRecs);
            this.pnlVoyageFile.Controls.Add(this.btnShipManifest);
            this.pnlVoyageFile.Location = new System.Drawing.Point(250, 10);
            this.pnlVoyageFile.Name = "pnlVoyageFile";
            this.pnlVoyageFile.Size = new System.Drawing.Size(189, 160);
            this.pnlVoyageFile.TabIndex = 75;
            // 
            // btnExpVoyExceptions
            // 
            this.btnExpVoyExceptions.Location = new System.Drawing.Point(5, 125);
            this.btnExpVoyExceptions.Name = "btnExpVoyExceptions";
            this.btnExpVoyExceptions.Size = new System.Drawing.Size(175, 25);
            this.btnExpVoyExceptions.TabIndex = 5;
            this.btnExpVoyExceptions.Text = "Exp. Voy. Exceptions";
            this.btnExpVoyExceptions.UseVisualStyleBackColor = true;
            this.btnExpVoyExceptions.Click += new System.EventHandler(this.btnExpVoyExceptions_Click);
            // 
            // btnExpBookSummary
            // 
            this.btnExpBookSummary.Location = new System.Drawing.Point(5, 90);
            this.btnExpBookSummary.Name = "btnExpBookSummary";
            this.btnExpBookSummary.Size = new System.Drawing.Size(175, 25);
            this.btnExpBookSummary.TabIndex = 4;
            this.btnExpBookSummary.Text = "Exp. Booking Summary";
            this.btnExpBookSummary.UseVisualStyleBackColor = true;
            this.btnExpBookSummary.Click += new System.EventHandler(this.btnExpBookSummary_Click);
            // 
            // btnExpBookRecs
            // 
            this.btnExpBookRecs.Location = new System.Drawing.Point(5, 55);
            this.btnExpBookRecs.Name = "btnExpBookRecs";
            this.btnExpBookRecs.Size = new System.Drawing.Size(175, 25);
            this.btnExpBookRecs.TabIndex = 3;
            this.btnExpBookRecs.Text = "Exp. Booking Records";
            this.btnExpBookRecs.UseVisualStyleBackColor = true;
            this.btnExpBookRecs.Click += new System.EventHandler(this.btnExpBookRecs_Click);
            // 
            // btnShipManifest
            // 
            this.btnShipManifest.Location = new System.Drawing.Point(5, 20);
            this.btnShipManifest.Name = "btnShipManifest";
            this.btnShipManifest.Size = new System.Drawing.Size(175, 25);
            this.btnShipManifest.TabIndex = 2;
            this.btnShipManifest.Text = "Ship Manifest";
            this.btnShipManifest.UseVisualStyleBackColor = true;
            this.btnShipManifest.Click += new System.EventHandler(this.btnShipManifest_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.LightGray;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(255, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 16);
            this.label3.TabIndex = 76;
            this.label3.Text = "Voyage Files";
            // 
            // frmImportExportMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(657, 180);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pnlVoyageFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnlInvFile);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlHandheldFile);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Location = new System.Drawing.Point(100, 55);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmImportExportMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DAI Export: IMPORT/EXPORT MENU";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmImportExportMenu_MouseMove);
            this.pnlHandheldFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnlInvFile.ResumeLayout(false);
            this.pnlVoyageFile.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlHandheldFile;
        private System.Windows.Forms.Button btnImportShp;
        private System.Windows.Forms.Button btnImportPhyClone;
        private System.Windows.Forms.Button btnImportRcvd;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Panel pnlInvFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPostShipInv;
        private System.Windows.Forms.Button btnPreShipInv;
        private System.Windows.Forms.Button btnExpCustApproved;
        private System.Windows.Forms.Panel pnlVoyageFile;
        private System.Windows.Forms.Button btnShipManifest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnExpBookRecs;
        private System.Windows.Forms.Button btnExpBookSummary;
        private System.Windows.Forms.Button btnExpVoyExceptions;
    }
}