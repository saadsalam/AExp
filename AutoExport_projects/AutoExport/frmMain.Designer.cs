namespace AutoExport
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sheetPrinterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vehLocatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customerAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.destinationAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exporterAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.freightForwarderAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateInvoicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vesselAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.voyageAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventProcessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.billingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printInvoicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportBillingRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblFullName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Font = new System.Drawing.Font("Arial", 9F);
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.vehLocatorToolStripMenuItem,
            this.addNewRecordToolStripMenuItem,
            this.importExportToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.eventProcessingToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.billingToolStripMenuItem,
            this.logOutToolStripMenuItem,
            this.refreshDataToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.ShowItemToolTips = true;
            this.mnuMain.Size = new System.Drawing.Size(984, 24);
            this.mnuMain.TabIndex = 1;
            this.mnuMain.Text = "menuStrip1";
            this.mnuMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mnuMain_MouseMove);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.sheetPrinterToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // sheetPrinterToolStripMenuItem
            // 
            this.sheetPrinterToolStripMenuItem.Name = "sheetPrinterToolStripMenuItem";
            this.sheetPrinterToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.sheetPrinterToolStripMenuItem.Text = "Sheet Printer";
            this.sheetPrinterToolStripMenuItem.Click += new System.EventHandler(this.sheetPrinterToolStripMenuItem_Click);
            // 
            // vehLocatorToolStripMenuItem
            // 
            this.vehLocatorToolStripMenuItem.Name = "vehLocatorToolStripMenuItem";
            this.vehLocatorToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.vehLocatorToolStripMenuItem.Text = "&Veh. Locator";
            this.vehLocatorToolStripMenuItem.Click += new System.EventHandler(this.vehLocatorToolStripMenuItem_Click);
            // 
            // addNewRecordToolStripMenuItem
            // 
            this.addNewRecordToolStripMenuItem.Name = "addNewRecordToolStripMenuItem";
            this.addNewRecordToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.addNewRecordToolStripMenuItem.Text = "&Add Veh.";
            this.addNewRecordToolStripMenuItem.Click += new System.EventHandler(this.addNewRecordToolStripMenuItem_Click);
            // 
            // importExportToolStripMenuItem
            // 
            this.importExportToolStripMenuItem.Name = "importExportToolStripMenuItem";
            this.importExportToolStripMenuItem.Size = new System.Drawing.Size(91, 20);
            this.importExportToolStripMenuItem.Text = "&Import/Export";
            this.importExportToolStripMenuItem.Click += new System.EventHandler(this.importExportToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customerAdminToolStripMenuItem,
            this.destinationAdminToolStripMenuItem,
            this.exporterAdminToolStripMenuItem,
            this.freightForwarderAdminToolStripMenuItem,
            this.generateInvoicesToolStripMenuItem,
            this.userAdminToolStripMenuItem,
            this.vesselAdminToolStripMenuItem,
            this.voyageAdminToolStripMenuItem});
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.adminToolStripMenuItem.Text = "Ad&min";
            // 
            // customerAdminToolStripMenuItem
            // 
            this.customerAdminToolStripMenuItem.Name = "customerAdminToolStripMenuItem";
            this.customerAdminToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.customerAdminToolStripMenuItem.Text = "&Customer Admin";
            this.customerAdminToolStripMenuItem.Click += new System.EventHandler(this.customerAdminToolStripMenuItem_Click);
            // 
            // destinationAdminToolStripMenuItem
            // 
            this.destinationAdminToolStripMenuItem.Name = "destinationAdminToolStripMenuItem";
            this.destinationAdminToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.destinationAdminToolStripMenuItem.Text = "Destination Admin";
            this.destinationAdminToolStripMenuItem.Click += new System.EventHandler(this.destinationAdminToolStripMenuItem_Click);
            // 
            // exporterAdminToolStripMenuItem
            // 
            this.exporterAdminToolStripMenuItem.Name = "exporterAdminToolStripMenuItem";
            this.exporterAdminToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.exporterAdminToolStripMenuItem.Text = "Exporter Admin";
            this.exporterAdminToolStripMenuItem.Click += new System.EventHandler(this.exporterAdminToolStripMenuItem_Click);
            // 
            // freightForwarderAdminToolStripMenuItem
            // 
            this.freightForwarderAdminToolStripMenuItem.Name = "freightForwarderAdminToolStripMenuItem";
            this.freightForwarderAdminToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.freightForwarderAdminToolStripMenuItem.Text = "Freight Forwarder Admin";
            this.freightForwarderAdminToolStripMenuItem.Click += new System.EventHandler(this.freightForwarderAdminToolStripMenuItem_Click);
            // 
            // generateInvoicesToolStripMenuItem
            // 
            this.generateInvoicesToolStripMenuItem.Name = "generateInvoicesToolStripMenuItem";
            this.generateInvoicesToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.generateInvoicesToolStripMenuItem.Text = "Generate Invoices";
            this.generateInvoicesToolStripMenuItem.Click += new System.EventHandler(this.generateInvoicesToolStripMenuItem_Click);
            // 
            // userAdminToolStripMenuItem
            // 
            this.userAdminToolStripMenuItem.Name = "userAdminToolStripMenuItem";
            this.userAdminToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.userAdminToolStripMenuItem.Text = "User Admin";
            this.userAdminToolStripMenuItem.Click += new System.EventHandler(this.userAdminToolStripMenuItem_Click);
            // 
            // vesselAdminToolStripMenuItem
            // 
            this.vesselAdminToolStripMenuItem.Name = "vesselAdminToolStripMenuItem";
            this.vesselAdminToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.vesselAdminToolStripMenuItem.Text = "Vessel Admin";
            this.vesselAdminToolStripMenuItem.Click += new System.EventHandler(this.vesselAdminToolStripMenuItem_Click);
            // 
            // voyageAdminToolStripMenuItem
            // 
            this.voyageAdminToolStripMenuItem.Name = "voyageAdminToolStripMenuItem";
            this.voyageAdminToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.voyageAdminToolStripMenuItem.Text = "Voyage Admin";
            this.voyageAdminToolStripMenuItem.Click += new System.EventHandler(this.voyageAdminToolStripMenuItem_Click);
            // 
            // eventProcessingToolStripMenuItem
            // 
            this.eventProcessingToolStripMenuItem.Name = "eventProcessingToolStripMenuItem";
            this.eventProcessingToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.eventProcessingToolStripMenuItem.Text = "&Event Processing";
            this.eventProcessingToolStripMenuItem.Click += new System.EventHandler(this.eventProcessingToolStripMenuItem_Click);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.reportsToolStripMenuItem.Text = "&Reports";
            this.reportsToolStripMenuItem.Click += new System.EventHandler(this.reportsToolStripMenuItem_Click);
            // 
            // billingToolStripMenuItem
            // 
            this.billingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printInvoicesToolStripMenuItem,
            this.exportBillingRecordsToolStripMenuItem});
            this.billingToolStripMenuItem.Name = "billingToolStripMenuItem";
            this.billingToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.billingToolStripMenuItem.Text = "&Billing";
            // 
            // printInvoicesToolStripMenuItem
            // 
            this.printInvoicesToolStripMenuItem.Name = "printInvoicesToolStripMenuItem";
            this.printInvoicesToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.printInvoicesToolStripMenuItem.Text = "Print Customer Invoices";
            this.printInvoicesToolStripMenuItem.Click += new System.EventHandler(this.printInvoicesToolStripMenuItem_Click);
            // 
            // exportBillingRecordsToolStripMenuItem
            // 
            this.exportBillingRecordsToolStripMenuItem.Name = "exportBillingRecordsToolStripMenuItem";
            this.exportBillingRecordsToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.exportBillingRecordsToolStripMenuItem.Text = "Export Billing Records";
            this.exportBillingRecordsToolStripMenuItem.Click += new System.EventHandler(this.exportBillingRecordsToolStripMenuItem_Click);
            // 
            // logOutToolStripMenuItem
            // 
            this.logOutToolStripMenuItem.Name = "logOutToolStripMenuItem";
            this.logOutToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.logOutToolStripMenuItem.Text = "&Log Out";
            this.logOutToolStripMenuItem.Visible = false;
            this.logOutToolStripMenuItem.Click += new System.EventHandler(this.logOutToolStripMenuItem_Click);
            // 
            // refreshDataToolStripMenuItem
            // 
            this.refreshDataToolStripMenuItem.Name = "refreshDataToolStripMenuItem";
            this.refreshDataToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.refreshDataToolStripMenuItem.Text = "Refresh Data";
            this.refreshDataToolStripMenuItem.ToolTipText = "Overwrite all TEST data with current DATS PROD data";
            this.refreshDataToolStripMenuItem.Visible = false;
            this.refreshDataToolStripMenuItem.Click += new System.EventHandler(this.refreshDataToolStripMenuItem_Click);
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.BackColor = System.Drawing.Color.White;
            this.lblFullName.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullName.ForeColor = System.Drawing.Color.Gray;
            this.lblFullName.Location = new System.Drawing.Point(780, 0);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(203, 16);
            this.lblFullName.TabIndex = 3;
            this.lblFullName.Text = "Edward Palmer Betancourth";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AutoExport.Properties.Resources.person;
            this.pictureBox1.Location = new System.Drawing.Point(750, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 19);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(984, 22);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.mnuMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DAI Export";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vehLocatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customerAdminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exporterAdminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem freightForwarderAdminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userAdminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vesselAdminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem voyageAdminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eventProcessingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem billingToolStripMenuItem;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem logOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printInvoicesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem destinationAdminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateInvoicesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportBillingRecordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sheetPrinterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshDataToolStripMenuItem;
    }
}