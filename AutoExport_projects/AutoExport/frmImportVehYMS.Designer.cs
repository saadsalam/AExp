namespace AutoExport
{
    partial class frmImportVehYMS
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblUsers = new System.Windows.Forms.Label();
            this.cboUsers = new System.Windows.Forms.ComboBox();
            this.btnMenu = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.pnlImport = new System.Windows.Forms.Panel();
            this.ckActive = new System.Windows.Forms.CheckBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.btnDest = new System.Windows.Forms.Button();
            this.btnCust = new System.Windows.Forms.Button();
            this.btnResubVoyage = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblBatchID = new System.Windows.Forms.Label();
            this.btnSizeClass = new System.Windows.Forms.Button();
            this.lblBatchDetails = new System.Windows.Forms.Label();
            this.lblBatchRecords = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRunStatus = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.ckExcludeShippedVehs = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblReports = new System.Windows.Forms.Label();
            this.pnlReports = new System.Windows.Forms.Panel();
            this.btnLabels = new System.Windows.Forms.Button();
            this.btnInv = new System.Windows.Forms.Button();
            this.progbar = new System.Windows.Forms.ProgressBar();
            this.bckLoadData = new System.ComponentModel.BackgroundWorker();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnNoLabels = new System.Windows.Forms.Button();
            this.txtImportType = new System.Windows.Forms.TextBox();
            this.RecordID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importedind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BayLoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Destination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Make = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BodyStyle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SizeClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VehicleHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BookingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NonRunner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnlImport.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            this.pnlReports.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUsers
            // 
            this.lblUsers.AutoSize = true;
            this.lblUsers.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsers.Location = new System.Drawing.Point(5, 15);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(148, 15);
            this.lblUsers.TabIndex = 0;
            this.lblUsers.Text = "User To Import Work For:";
            // 
            // cboUsers
            // 
            this.cboUsers.FormattingEnabled = true;
            this.cboUsers.Location = new System.Drawing.Point(5, 75);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.Size = new System.Drawing.Size(145, 24);
            this.cboUsers.TabIndex = 1;
            // 
            // btnMenu
            // 
            this.btnMenu.Image = global::AutoExport.Properties.Resources.Menu1;
            this.btnMenu.Location = new System.Drawing.Point(5, 10);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 30);
            this.btnMenu.TabIndex = 74;
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 592);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 73;
            this.pictureBox2.TabStop = false;
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(5, 105);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(145, 25);
            this.btnImport.TabIndex = 79;
            this.btnImport.Text = "Import File";
            this.btnImport.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnImport.UseCompatibleTextRendering = true;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // pnlImport
            // 
            this.pnlImport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlImport.Controls.Add(this.ckActive);
            this.pnlImport.Controls.Add(this.btnProcess);
            this.pnlImport.Controls.Add(this.cboUsers);
            this.pnlImport.Controls.Add(this.btnImport);
            this.pnlImport.Controls.Add(this.lblUsers);
            this.pnlImport.Location = new System.Drawing.Point(45, 10);
            this.pnlImport.Name = "pnlImport";
            this.pnlImport.Size = new System.Drawing.Size(155, 170);
            this.pnlImport.TabIndex = 80;
            // 
            // ckActive
            // 
            this.ckActive.AutoSize = true;
            this.ckActive.Checked = true;
            this.ckActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckActive.Location = new System.Drawing.Point(5, 45);
            this.ckActive.Name = "ckActive";
            this.ckActive.Size = new System.Drawing.Size(120, 19);
            this.ckActive.TabIndex = 81;
            this.ckActive.Text = "Only Active Users";
            this.ckActive.UseVisualStyleBackColor = true;
            this.ckActive.CheckedChanged += new System.EventHandler(this.ckActive_CheckedChanged);
            // 
            // btnProcess
            // 
            this.btnProcess.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.Location = new System.Drawing.Point(5, 135);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(145, 25);
            this.btnProcess.TabIndex = 80;
            this.btnProcess.Text = "Process File";
            this.btnProcess.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnProcess.UseCompatibleTextRendering = true;
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.Black;
            this.lblSearch.Location = new System.Drawing.Point(50, 5);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(126, 15);
            this.lblSearch.TabIndex = 81;
            this.lblSearch.Text = "Import Scanned Veh\'s";
            // 
            // pnlGrid
            // 
            this.pnlGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.Controls.Add(this.btnDest);
            this.pnlGrid.Controls.Add(this.btnCust);
            this.pnlGrid.Controls.Add(this.btnResubVoyage);
            this.pnlGrid.Controls.Add(this.lblStatus);
            this.pnlGrid.Controls.Add(this.lblBatchID);
            this.pnlGrid.Controls.Add(this.btnSizeClass);
            this.pnlGrid.Controls.Add(this.lblBatchDetails);
            this.pnlGrid.Controls.Add(this.lblBatchRecords);
            this.pnlGrid.Controls.Add(this.btnExport);
            this.pnlGrid.Controls.Add(this.btnClear);
            this.pnlGrid.Controls.Add(this.btnRunStatus);
            this.pnlGrid.Controls.Add(this.btnLoad);
            this.pnlGrid.Controls.Add(this.dgResults);
            this.pnlGrid.Controls.Add(this.ckExcludeShippedVehs);
            this.pnlGrid.Location = new System.Drawing.Point(210, 10);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(1295, 570);
            this.pnlGrid.TabIndex = 82;
            // 
            // btnDest
            // 
            this.btnDest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDest.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDest.Location = new System.Drawing.Point(530, 540);
            this.btnDest.Name = "btnDest";
            this.btnDest.Size = new System.Drawing.Size(100, 25);
            this.btnDest.TabIndex = 92;
            this.btnDest.Text = "Set Dest.";
            this.btnDest.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDest.UseCompatibleTextRendering = true;
            this.btnDest.UseVisualStyleBackColor = true;
            this.btnDest.Click += new System.EventHandler(this.btnDest_Click);
            // 
            // btnCust
            // 
            this.btnCust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCust.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCust.Location = new System.Drawing.Point(425, 540);
            this.btnCust.Name = "btnCust";
            this.btnCust.Size = new System.Drawing.Size(100, 25);
            this.btnCust.TabIndex = 91;
            this.btnCust.Text = "Set Customer";
            this.btnCust.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCust.UseCompatibleTextRendering = true;
            this.btnCust.UseVisualStyleBackColor = true;
            this.btnCust.Click += new System.EventHandler(this.btnCust_Click);
            // 
            // btnResubVoyage
            // 
            this.btnResubVoyage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResubVoyage.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResubVoyage.Location = new System.Drawing.Point(635, 540);
            this.btnResubVoyage.Name = "btnResubVoyage";
            this.btnResubVoyage.Size = new System.Drawing.Size(159, 25);
            this.btnResubVoyage.TabIndex = 90;
            this.btnResubVoyage.Text = "Resubmit Voy. Not Found";
            this.btnResubVoyage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnResubVoyage.UseCompatibleTextRendering = true;
            this.btnResubVoyage.UseVisualStyleBackColor = true;
            this.btnResubVoyage.Click += new System.EventHandler(this.btnResubVoyage_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(120, 490);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(28, 15);
            this.lblStatus.TabIndex = 88;
            this.lblStatus.Text = "567";
            this.lblStatus.Visible = false;
            // 
            // lblBatchID
            // 
            this.lblBatchID.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblBatchID.AutoSize = true;
            this.lblBatchID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatchID.ForeColor = System.Drawing.Color.Black;
            this.lblBatchID.Location = new System.Drawing.Point(635, 490);
            this.lblBatchID.Name = "lblBatchID";
            this.lblBatchID.Size = new System.Drawing.Size(92, 15);
            this.lblBatchID.TabIndex = 87;
            this.lblBatchID.Text = "BatchID: 24466";
            this.lblBatchID.Visible = false;
            // 
            // btnSizeClass
            // 
            this.btnSizeClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSizeClass.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSizeClass.Location = new System.Drawing.Point(320, 540);
            this.btnSizeClass.Name = "btnSizeClass";
            this.btnSizeClass.Size = new System.Drawing.Size(100, 25);
            this.btnSizeClass.TabIndex = 85;
            this.btnSizeClass.Text = "Set Size Class";
            this.btnSizeClass.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSizeClass.UseCompatibleTextRendering = true;
            this.btnSizeClass.UseVisualStyleBackColor = true;
            this.btnSizeClass.Click += new System.EventHandler(this.btnSizeClass_Click);
            // 
            // lblBatchDetails
            // 
            this.lblBatchDetails.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblBatchDetails.AutoSize = true;
            this.lblBatchDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatchDetails.Location = new System.Drawing.Point(635, 515);
            this.lblBatchDetails.Name = "lblBatchDetails";
            this.lblBatchDetails.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblBatchDetails.Size = new System.Drawing.Size(286, 15);
            this.lblBatchDetails.TabIndex = 84;
            this.lblBatchDetails.Text = "Received By: CPeterson  Processed By: CPeterson";
            this.lblBatchDetails.Visible = false;
            // 
            // lblBatchRecords
            // 
            this.lblBatchRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBatchRecords.AutoSize = true;
            this.lblBatchRecords.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatchRecords.Location = new System.Drawing.Point(5, 490);
            this.lblBatchRecords.Name = "lblBatchRecords";
            this.lblBatchRecords.Size = new System.Drawing.Size(28, 15);
            this.lblBatchRecords.TabIndex = 83;
            this.lblBatchRecords.Text = "567";
            this.lblBatchRecords.Visible = false;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(1190, 540);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(95, 25);
            this.btnExport.TabIndex = 82;
            this.btnExport.Text = "Export Results";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExport.UseCompatibleTextRendering = true;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(110, 540);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 25);
            this.btnClear.TabIndex = 81;
            this.btnClear.Text = "Clear Detail";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClear.UseCompatibleTextRendering = true;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRunStatus
            // 
            this.btnRunStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRunStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunStatus.Location = new System.Drawing.Point(215, 540);
            this.btnRunStatus.Name = "btnRunStatus";
            this.btnRunStatus.Size = new System.Drawing.Size(100, 25);
            this.btnRunStatus.TabIndex = 80;
            this.btnRunStatus.Text = "Non Run Status";
            this.btnRunStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRunStatus.UseCompatibleTextRendering = true;
            this.btnRunStatus.UseVisualStyleBackColor = true;
            this.btnRunStatus.Click += new System.EventHandler(this.btnRunStatus_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.Location = new System.Drawing.Point(5, 540);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 25);
            this.btnLoad.TabIndex = 79;
            this.btnLoad.Text = "Load Batch";
            this.btnLoad.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoad.UseCompatibleTextRendering = true;
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dgResults
            // 
            this.dgResults.AllowUserToAddRows = false;
            this.dgResults.AllowUserToDeleteRows = false;
            this.dgResults.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.dgResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgResults.BackgroundColor = System.Drawing.Color.White;
            this.dgResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecordID,
            this.importedind,
            this.VIN,
            this.BayLoc,
            this.Destination,
            this.Make,
            this.Model,
            this.BodyStyle,
            this.SizeClass,
            this.VehicleHeight,
            this.BookingNumber,
            this.NonRunner,
            this.CustomerName,
            this.Status});
            this.dgResults.Cursor = System.Windows.Forms.Cursors.Default;
            this.dgResults.EnableHeadersVisualStyles = false;
            this.dgResults.Location = new System.Drawing.Point(5, 15);
            this.dgResults.MultiSelect = false;
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgResults.RowHeadersWidth = 15;
            this.dgResults.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgResults.Size = new System.Drawing.Size(1280, 460);
            this.dgResults.TabIndex = 3;
            this.dgResults.TabStop = false;
            this.dgResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResults_CellClick);
            // 
            // ckExcludeShippedVehs
            // 
            this.ckExcludeShippedVehs.AutoSize = true;
            this.ckExcludeShippedVehs.Checked = true;
            this.ckExcludeShippedVehs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckExcludeShippedVehs.Location = new System.Drawing.Point(8, 515);
            this.ckExcludeShippedVehs.Name = "ckExcludeShippedVehs";
            this.ckExcludeShippedVehs.Size = new System.Drawing.Size(188, 20);
            this.ckExcludeShippedVehs.TabIndex = 89;
            this.ckExcludeShippedVehs.Text = "Exclude Shipped Vehicles";
            this.ckExcludeShippedVehs.UseVisualStyleBackColor = true;
            this.ckExcludeShippedVehs.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(250, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 83;
            this.label2.Text = "Batch Detail";
            // 
            // lblReports
            // 
            this.lblReports.AutoSize = true;
            this.lblReports.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReports.ForeColor = System.Drawing.Color.Black;
            this.lblReports.Location = new System.Drawing.Point(50, 295);
            this.lblReports.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.lblReports.Name = "lblReports";
            this.lblReports.Size = new System.Drawing.Size(92, 15);
            this.lblReports.TabIndex = 84;
            this.lblReports.Text = "Reports/Labels";
            // 
            // pnlReports
            // 
            this.pnlReports.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlReports.Controls.Add(this.btnLabels);
            this.pnlReports.Controls.Add(this.btnInv);
            this.pnlReports.Location = new System.Drawing.Point(45, 300);
            this.pnlReports.Name = "pnlReports";
            this.pnlReports.Size = new System.Drawing.Size(155, 80);
            this.pnlReports.TabIndex = 0;
            // 
            // btnLabels
            // 
            this.btnLabels.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLabels.Location = new System.Drawing.Point(5, 45);
            this.btnLabels.Name = "btnLabels";
            this.btnLabels.Size = new System.Drawing.Size(145, 25);
            this.btnLabels.TabIndex = 81;
            this.btnLabels.Text = "Print Labels";
            this.btnLabels.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLabels.UseCompatibleTextRendering = true;
            this.btnLabels.UseVisualStyleBackColor = true;
            this.btnLabels.Click += new System.EventHandler(this.btnLabels_Click);
            // 
            // btnInv
            // 
            this.btnInv.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInv.Location = new System.Drawing.Point(5, 15);
            this.btnInv.Name = "btnInv";
            this.btnInv.Size = new System.Drawing.Size(145, 25);
            this.btnInv.TabIndex = 80;
            this.btnInv.Text = "Inv. Comp. Report";
            this.btnInv.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnInv.UseCompatibleTextRendering = true;
            this.btnInv.UseVisualStyleBackColor = true;
            this.btnInv.Click += new System.EventHandler(this.btnInv_Click);
            // 
            // progbar
            // 
            this.progbar.Location = new System.Drawing.Point(45, 185);
            this.progbar.Name = "progbar";
            this.progbar.Size = new System.Drawing.Size(150, 23);
            this.progbar.Step = 1;
            this.progbar.TabIndex = 88;
            // 
            // bckLoadData
            // 
            this.bckLoadData.WorkerReportsProgress = true;
            this.bckLoadData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckLoadData_DoWork);
            this.bckLoadData.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bckLoadData_ProgressChanged);
            this.bckLoadData.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bckLoadData_RunWorkerCompleted);
            // 
            // txtStatus
            // 
            this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatus.Location = new System.Drawing.Point(45, 215);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(150, 21);
            this.txtStatus.TabIndex = 88;
            // 
            // btnNoLabels
            // 
            this.btnNoLabels.Location = new System.Drawing.Point(170, 386);
            this.btnNoLabels.Name = "btnNoLabels";
            this.btnNoLabels.Size = new System.Drawing.Size(25, 25);
            this.btnNoLabels.TabIndex = 89;
            this.btnNoLabels.Text = "?";
            this.btnNoLabels.UseVisualStyleBackColor = true;
            this.btnNoLabels.Visible = false;
            this.btnNoLabels.Click += new System.EventHandler(this.btnNoLabels_Click);
            // 
            // txtImportType
            // 
            this.txtImportType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtImportType.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImportType.ForeColor = System.Drawing.Color.Blue;
            this.txtImportType.Location = new System.Drawing.Point(45, 440);
            this.txtImportType.Multiline = true;
            this.txtImportType.Name = "txtImportType";
            this.txtImportType.Size = new System.Drawing.Size(157, 40);
            this.txtImportType.TabIndex = 90;
            this.txtImportType.Text = "Import Physical Inventory";
            // 
            // RecordID
            // 
            this.RecordID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RecordID.DataPropertyName = "RecordID";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.RecordID.DefaultCellStyle = dataGridViewCellStyle3;
            this.RecordID.FillWeight = 1F;
            this.RecordID.HeaderText = "";
            this.RecordID.MaxInputLength = 5;
            this.RecordID.MinimumWidth = 2;
            this.RecordID.Name = "RecordID";
            this.RecordID.ReadOnly = true;
            this.RecordID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RecordID.Visible = false;
            this.RecordID.Width = 2;
            // 
            // importedind
            // 
            this.importedind.DataPropertyName = "ImportedInd";
            this.importedind.HeaderText = "";
            this.importedind.MinimumWidth = 2;
            this.importedind.Name = "importedind";
            this.importedind.ReadOnly = true;
            this.importedind.Visible = false;
            this.importedind.Width = 2;
            // 
            // VIN
            // 
            this.VIN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.VIN.DataPropertyName = "VIN";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VIN.DefaultCellStyle = dataGridViewCellStyle4;
            this.VIN.HeaderText = "VIN";
            this.VIN.MaxInputLength = 17;
            this.VIN.Name = "VIN";
            this.VIN.ReadOnly = true;
            this.VIN.Width = 160;
            // 
            // BayLoc
            // 
            this.BayLoc.DataPropertyName = "BayLocation";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            this.BayLoc.DefaultCellStyle = dataGridViewCellStyle5;
            this.BayLoc.HeaderText = "Loc.";
            this.BayLoc.MaxInputLength = 6;
            this.BayLoc.Name = "BayLoc";
            this.BayLoc.ReadOnly = true;
            this.BayLoc.Width = 60;
            // 
            // Destination
            // 
            this.Destination.DataPropertyName = "DestinationName";
            this.Destination.HeaderText = "Dest.";
            this.Destination.MaxInputLength = 3;
            this.Destination.Name = "Destination";
            this.Destination.ReadOnly = true;
            this.Destination.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Destination.Width = 65;
            // 
            // Make
            // 
            this.Make.DataPropertyName = "Make";
            this.Make.HeaderText = "Make";
            this.Make.Name = "Make";
            this.Make.ReadOnly = true;
            this.Make.Width = 125;
            // 
            // Model
            // 
            this.Model.DataPropertyName = "Model";
            this.Model.HeaderText = "Model";
            this.Model.Name = "Model";
            this.Model.ReadOnly = true;
            this.Model.Width = 125;
            // 
            // BodyStyle
            // 
            this.BodyStyle.DataPropertyName = "Bodystyle";
            this.BodyStyle.HeaderText = "Body Style";
            this.BodyStyle.Name = "BodyStyle";
            this.BodyStyle.ReadOnly = true;
            this.BodyStyle.Width = 200;
            // 
            // SizeClass
            // 
            this.SizeClass.DataPropertyName = "SizeClass";
            this.SizeClass.HeaderText = "Size";
            this.SizeClass.Name = "SizeClass";
            this.SizeClass.ReadOnly = true;
            this.SizeClass.Width = 40;
            // 
            // VehicleHeight
            // 
            this.VehicleHeight.DataPropertyName = "VehicleHeight";
            this.VehicleHeight.HeaderText = "Hgt.";
            this.VehicleHeight.MaxInputLength = 6;
            this.VehicleHeight.Name = "VehicleHeight";
            this.VehicleHeight.ReadOnly = true;
            this.VehicleHeight.Width = 40;
            // 
            // BookingNumber
            // 
            this.BookingNumber.DataPropertyName = "BookingNumber";
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            this.BookingNumber.DefaultCellStyle = dataGridViewCellStyle6;
            this.BookingNumber.HeaderText = "Book #";
            this.BookingNumber.MaxInputLength = 20;
            this.BookingNumber.Name = "BookingNumber";
            this.BookingNumber.ReadOnly = true;
            this.BookingNumber.Width = 75;
            // 
            // NonRunner
            // 
            this.NonRunner.DataPropertyName = "NonRunner";
            this.NonRunner.HeaderText = "Non Runner";
            this.NonRunner.Name = "NonRunner";
            this.NonRunner.ReadOnly = true;
            this.NonRunner.Width = 75;
            // 
            // CustomerName
            // 
            this.CustomerName.DataPropertyName = "CustomerName";
            this.CustomerName.HeaderText = "Cust.";
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.ReadOnly = true;
            this.CustomerName.Width = 75;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "RecordStatus";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status.DefaultCellStyle = dataGridViewCellStyle7;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 225;
            // 
            // frmImportVehYMS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1514, 592);
            this.Controls.Add(this.txtImportType);
            this.Controls.Add(this.btnNoLabels);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.progbar);
            this.Controls.Add(this.lblReports);
            this.Controls.Add(this.pnlReports);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.pnlImport);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.pictureBox2);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Name = "frmImportVehYMS";
            this.Text = "DAI Export: Vehicles - YMS";
            this.Activated += new System.EventHandler(this.frmImportVehYMS_Activated);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmImportVehYMS_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnlImport.ResumeLayout(false);
            this.pnlImport.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            this.pnlGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            this.pnlReports.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.ComboBox cboUsers;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Panel pnlImport;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblReports;
        private System.Windows.Forms.Panel pnlReports;
        private System.Windows.Forms.Button btnInv;
        protected System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnLabels;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRunStatus;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblBatchDetails;
        private System.Windows.Forms.Label lblBatchRecords;
        private System.Windows.Forms.Button btnSizeClass;
        private System.Windows.Forms.CheckBox ckActive;
        private System.Windows.Forms.Label lblBatchID;
        private System.Windows.Forms.ProgressBar progbar;
        private System.ComponentModel.BackgroundWorker bckLoadData;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnNoLabels;
        private System.Windows.Forms.CheckBox ckExcludeShippedVehs;
        private System.Windows.Forms.TextBox txtImportType;
        private System.Windows.Forms.Button btnResubVoyage;
        private System.Windows.Forms.Button btnCust;
        private System.Windows.Forms.Button btnDest;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordID;
        private System.Windows.Forms.DataGridViewTextBoxColumn importedind;
        private System.Windows.Forms.DataGridViewTextBoxColumn VIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn BayLoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Destination;
        private System.Windows.Forms.DataGridViewTextBoxColumn Make;
        private System.Windows.Forms.DataGridViewTextBoxColumn Model;
        private System.Windows.Forms.DataGridViewTextBoxColumn BodyStyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn SizeClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn VehicleHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn BookingNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn NonRunner;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}