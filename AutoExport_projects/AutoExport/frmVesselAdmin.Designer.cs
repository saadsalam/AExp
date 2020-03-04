namespace AutoExport
{
    partial class frmVesselAdmin
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
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVesselName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pnlResults = new System.Windows.Forms.Panel();
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.VoyageID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Vessel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecordStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblVesselRecords = new System.Windows.Forms.Label();
            this.lblResults = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtVesselID = new System.Windows.Forms.TextBox();
            this.txtUpdatedBy = new System.Windows.Forms.TextBox();
            this.recbuttons = new AutoExport.RecordButtons();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.txtUpdatedDate = new System.Windows.Forms.TextBox();
            this.label60 = new System.Windows.Forms.Label();
            this.txtCreationDate = new System.Windows.Forms.TextBox();
            this.txtCreatedBy = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.txtNote_record = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboStatus_record = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLloydsCode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtVesselShortName_record = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtVesselName_record = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblRecordDetails = new System.Windows.Forms.Label();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearch.Controls.Add(this.txtTo);
            this.pnlSearch.Controls.Add(this.txtFrom);
            this.pnlSearch.Controls.Add(this.label4);
            this.pnlSearch.Controls.Add(this.label3);
            this.pnlSearch.Controls.Add(this.label2);
            this.pnlSearch.Controls.Add(this.cboStatus);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Controls.Add(this.txtVesselName);
            this.pnlSearch.Controls.Add(this.label5);
            this.pnlSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlSearch.Location = new System.Drawing.Point(45, 10);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(450, 110);
            this.pnlSearch.TabIndex = 1;
            // 
            // txtTo
            // 
            this.txtTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTo.ForeColor = System.Drawing.Color.Black;
            this.txtTo.Location = new System.Drawing.Point(317, 75);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(121, 23);
            this.txtTo.TabIndex = 26;
            this.txtTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTo_KeyPress);
            this.txtTo.Validating += new System.ComponentModel.CancelEventHandler(this.txtTo_Validating);
            // 
            // txtFrom
            // 
            this.txtFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFrom.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFrom.ForeColor = System.Drawing.Color.Black;
            this.txtFrom.Location = new System.Drawing.Point(160, 75);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(121, 23);
            this.txtFrom.TabIndex = 25;
            this.txtFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrom_KeyPress);
            this.txtFrom.Validating += new System.ComponentModel.CancelEventHandler(this.txtFrom_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(287, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 15);
            this.label4.TabIndex = 24;
            this.label4.Text = "To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 23;
            this.label3.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "Created:";
            // 
            // cboStatus
            // 
            this.cboStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(160, 45);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(121, 24);
            this.cboStatus.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 15);
            this.label1.TabIndex = 21;
            this.label1.Text = "Record Status:";
            // 
            // txtVesselName
            // 
            this.txtVesselName.BackColor = System.Drawing.SystemColors.Window;
            this.txtVesselName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVesselName.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVesselName.Location = new System.Drawing.Point(160, 15);
            this.txtVesselName.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtVesselName.MinimumSize = new System.Drawing.Size(240, 25);
            this.txtVesselName.Name = "txtVesselName";
            this.txtVesselName.Size = new System.Drawing.Size(278, 23);
            this.txtVesselName.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 15);
            this.label5.TabIndex = 17;
            this.label5.Text = "Vessel Name:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 647);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.Black;
            this.lblSearch.Location = new System.Drawing.Point(50, 5);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(121, 15);
            this.lblSearch.TabIndex = 16;
            this.lblSearch.Text = "Enter Search Criteria";
            // 
            // pnlResults
            // 
            this.pnlResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlResults.Controls.Add(this.dgResults);
            this.pnlResults.Controls.Add(this.lblVesselRecords);
            this.pnlResults.Location = new System.Drawing.Point(45, 135);
            this.pnlResults.Name = "pnlResults";
            this.pnlResults.Size = new System.Drawing.Size(450, 505);
            this.pnlResults.TabIndex = 74;
            // 
            // dgResults
            // 
            this.dgResults.AllowUserToAddRows = false;
            this.dgResults.AllowUserToDeleteRows = false;
            this.dgResults.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.dgResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgResults.BackgroundColor = System.Drawing.Color.White;
            this.dgResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VoyageID,
            this.Vessel,
            this.RecordStatus});
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
            this.dgResults.Size = new System.Drawing.Size(435, 460);
            this.dgResults.TabIndex = 2;
            this.dgResults.TabStop = false;
            this.dgResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResults_CellClick);
            // 
            // VoyageID
            // 
            this.VoyageID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.VoyageID.DataPropertyName = "AEVesselID";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.VoyageID.DefaultCellStyle = dataGridViewCellStyle3;
            this.VoyageID.FillWeight = 1F;
            this.VoyageID.HeaderText = "";
            this.VoyageID.MaxInputLength = 5;
            this.VoyageID.MinimumWidth = 2;
            this.VoyageID.Name = "VoyageID";
            this.VoyageID.ReadOnly = true;
            this.VoyageID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.VoyageID.Visible = false;
            this.VoyageID.Width = 2;
            // 
            // Vessel
            // 
            this.Vessel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Vessel.DataPropertyName = "VesselName";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Vessel.DefaultCellStyle = dataGridViewCellStyle4;
            this.Vessel.HeaderText = "Vessel Name";
            this.Vessel.Name = "Vessel";
            this.Vessel.ReadOnly = true;
            this.Vessel.Width = 300;
            // 
            // RecordStatus
            // 
            this.RecordStatus.DataPropertyName = "RecordStatus";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordStatus.DefaultCellStyle = dataGridViewCellStyle5;
            this.RecordStatus.HeaderText = "Status";
            this.RecordStatus.Name = "RecordStatus";
            this.RecordStatus.ReadOnly = true;
            // 
            // lblVesselRecords
            // 
            this.lblVesselRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVesselRecords.AutoSize = true;
            this.lblVesselRecords.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVesselRecords.Location = new System.Drawing.Point(5, 480);
            this.lblVesselRecords.Name = "lblVesselRecords";
            this.lblVesselRecords.Size = new System.Drawing.Size(28, 15);
            this.lblVesselRecords.TabIndex = 1;
            this.lblVesselRecords.Text = "567";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResults.ForeColor = System.Drawing.Color.Black;
            this.lblResults.Location = new System.Drawing.Point(50, 130);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(92, 15);
            this.lblResults.TabIndex = 75;
            this.lblResults.Text = "Search Results";
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(500, 95);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(95, 25);
            this.btnExport.TabIndex = 78;
            this.btnExport.Text = "Export Results";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExport.UseCompatibleTextRendering = true;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(500, 10);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 25);
            this.btnSearch.TabIndex = 76;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(500, 41);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 25);
            this.btnClear.TabIndex = 77;
            this.btnClear.Text = "Clear Results";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.Image = global::AutoExport.Properties.Resources.Menu1;
            this.btnMenu.Location = new System.Drawing.Point(5, 10);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 30);
            this.btnMenu.TabIndex = 79;
            this.btnMenu.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtVesselID);
            this.panel1.Controls.Add(this.txtUpdatedBy);
            this.panel1.Controls.Add(this.recbuttons);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.label58);
            this.panel1.Controls.Add(this.txtUpdatedDate);
            this.panel1.Controls.Add(this.label60);
            this.panel1.Controls.Add(this.txtCreationDate);
            this.panel1.Controls.Add(this.txtCreatedBy);
            this.panel1.Controls.Add(this.label59);
            this.panel1.Controls.Add(this.label61);
            this.panel1.Controls.Add(this.txtNote_record);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.cboStatus_record);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtLloydsCode);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtVesselShortName_record);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtVesselName_record);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(605, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(575, 630);
            this.panel1.TabIndex = 80;
            // 
            // txtVesselID
            // 
            this.txtVesselID.BackColor = System.Drawing.SystemColors.Window;
            this.txtVesselID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVesselID.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVesselID.Location = new System.Drawing.Point(443, 67);
            this.txtVesselID.Margin = new System.Windows.Forms.Padding(4);
            this.txtVesselID.Name = "txtVesselID";
            this.txtVesselID.Size = new System.Drawing.Size(23, 23);
            this.txtVesselID.TabIndex = 84;
            this.txtVesselID.Visible = false;
            // 
            // txtUpdatedBy
            // 
            this.txtUpdatedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUpdatedBy.Enabled = false;
            this.txtUpdatedBy.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdatedBy.ForeColor = System.Drawing.Color.Black;
            this.txtUpdatedBy.Location = new System.Drawing.Point(378, 600);
            this.txtUpdatedBy.Name = "txtUpdatedBy";
            this.txtUpdatedBy.ReadOnly = true;
            this.txtUpdatedBy.Size = new System.Drawing.Size(150, 23);
            this.txtUpdatedBy.TabIndex = 80;
            this.txtUpdatedBy.TabStop = false;
            // 
            // recbuttons
            // 
            this.recbuttons.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recbuttons.ForeColor = System.Drawing.Color.Blue;
            this.recbuttons.Location = new System.Drawing.Point(505, 45);
            this.recbuttons.Name = "recbuttons";
            this.recbuttons.Size = new System.Drawing.Size(65, 160);
            this.recbuttons.TabIndex = 82;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Blue;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(435, 15);
            this.lblStatus.MinimumSize = new System.Drawing.Size(135, 25);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(135, 25);
            this.lblStatus.TabIndex = 83;
            this.lblStatus.Text = "Read Only";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label58.ForeColor = System.Drawing.Color.Blue;
            this.label58.Location = new System.Drawing.Point(291, 600);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(74, 15);
            this.label58.TabIndex = 79;
            this.label58.Text = "Updated By:";
            // 
            // txtUpdatedDate
            // 
            this.txtUpdatedDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUpdatedDate.Enabled = false;
            this.txtUpdatedDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdatedDate.ForeColor = System.Drawing.Color.Black;
            this.txtUpdatedDate.Location = new System.Drawing.Point(378, 570);
            this.txtUpdatedDate.Name = "txtUpdatedDate";
            this.txtUpdatedDate.ReadOnly = true;
            this.txtUpdatedDate.Size = new System.Drawing.Size(150, 23);
            this.txtUpdatedDate.TabIndex = 78;
            this.txtUpdatedDate.TabStop = false;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label60.ForeColor = System.Drawing.Color.Blue;
            this.label60.Location = new System.Drawing.Point(291, 570);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(76, 15);
            this.label60.TabIndex = 77;
            this.label60.Text = "Updated On:";
            // 
            // txtCreationDate
            // 
            this.txtCreationDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreationDate.Enabled = false;
            this.txtCreationDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreationDate.ForeColor = System.Drawing.Color.Black;
            this.txtCreationDate.Location = new System.Drawing.Point(90, 570);
            this.txtCreationDate.Name = "txtCreationDate";
            this.txtCreationDate.ReadOnly = true;
            this.txtCreationDate.Size = new System.Drawing.Size(150, 23);
            this.txtCreationDate.TabIndex = 76;
            this.txtCreationDate.TabStop = false;
            // 
            // txtCreatedBy
            // 
            this.txtCreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreatedBy.Enabled = false;
            this.txtCreatedBy.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreatedBy.ForeColor = System.Drawing.Color.Black;
            this.txtCreatedBy.Location = new System.Drawing.Point(90, 600);
            this.txtCreatedBy.Name = "txtCreatedBy";
            this.txtCreatedBy.ReadOnly = true;
            this.txtCreatedBy.Size = new System.Drawing.Size(150, 23);
            this.txtCreatedBy.TabIndex = 75;
            this.txtCreatedBy.TabStop = false;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label59.ForeColor = System.Drawing.Color.Blue;
            this.label59.Location = new System.Drawing.Point(5, 600);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(72, 15);
            this.label59.TabIndex = 74;
            this.label59.Text = "Created By:";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label61.ForeColor = System.Drawing.Color.Blue;
            this.label61.Location = new System.Drawing.Point(5, 570);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(74, 15);
            this.label61.TabIndex = 73;
            this.label61.Text = "Created On:";
            // 
            // txtNote_record
            // 
            this.txtNote_record.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNote_record.Location = new System.Drawing.Point(130, 135);
            this.txtNote_record.Multiline = true;
            this.txtNote_record.Name = "txtNote_record";
            this.txtNote_record.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNote_record.Size = new System.Drawing.Size(300, 410);
            this.txtNote_record.TabIndex = 28;
            this.txtNote_record.TextChanged += new System.EventHandler(this.txtNote_record_TextChanged);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 135);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 15);
            this.label10.TabIndex = 27;
            this.label10.Text = "Note:";
            // 
            // cboStatus_record
            // 
            this.cboStatus_record.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStatus_record.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStatus_record.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus_record.FormattingEnabled = true;
            this.cboStatus_record.Location = new System.Drawing.Point(130, 105);
            this.cboStatus_record.Name = "cboStatus_record";
            this.cboStatus_record.Size = new System.Drawing.Size(300, 24);
            this.cboStatus_record.TabIndex = 25;
            this.cboStatus_record.SelectedIndexChanged += new System.EventHandler(this.cboStatus_record_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(5, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 15);
            this.label9.TabIndex = 26;
            this.label9.Text = "Record Status:";
            // 
            // txtLloydsCode
            // 
            this.txtLloydsCode.BackColor = System.Drawing.SystemColors.Window;
            this.txtLloydsCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLloydsCode.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLloydsCode.Location = new System.Drawing.Point(130, 75);
            this.txtLloydsCode.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtLloydsCode.MinimumSize = new System.Drawing.Size(200, 25);
            this.txtLloydsCode.Name = "txtLloydsCode";
            this.txtLloydsCode.Size = new System.Drawing.Size(300, 23);
            this.txtLloydsCode.TabIndex = 24;
            this.txtLloydsCode.TextChanged += new System.EventHandler(this.txtLloydsCode_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 15);
            this.label8.TabIndex = 23;
            this.label8.Text = "Lloyd\'s Code";
            // 
            // txtVesselShortName_record
            // 
            this.txtVesselShortName_record.BackColor = System.Drawing.SystemColors.Window;
            this.txtVesselShortName_record.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVesselShortName_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVesselShortName_record.Location = new System.Drawing.Point(130, 45);
            this.txtVesselShortName_record.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtVesselShortName_record.MinimumSize = new System.Drawing.Size(200, 25);
            this.txtVesselShortName_record.Name = "txtVesselShortName_record";
            this.txtVesselShortName_record.Size = new System.Drawing.Size(300, 23);
            this.txtVesselShortName_record.TabIndex = 22;
            this.txtVesselShortName_record.TextChanged += new System.EventHandler(this.txtVesselShortName_record_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 15);
            this.label7.TabIndex = 21;
            this.label7.Text = "Vessel Short Name:";
            // 
            // txtVesselName_record
            // 
            this.txtVesselName_record.BackColor = System.Drawing.SystemColors.Window;
            this.txtVesselName_record.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVesselName_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVesselName_record.Location = new System.Drawing.Point(130, 15);
            this.txtVesselName_record.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtVesselName_record.MinimumSize = new System.Drawing.Size(200, 25);
            this.txtVesselName_record.Name = "txtVesselName_record";
            this.txtVesselName_record.Size = new System.Drawing.Size(300, 23);
            this.txtVesselName_record.TabIndex = 20;
            this.txtVesselName_record.TextChanged += new System.EventHandler(this.txtVesselName_record_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 15);
            this.label6.TabIndex = 18;
            this.label6.Text = "Vessel Name:";
            // 
            // lblRecordDetails
            // 
            this.lblRecordDetails.AutoSize = true;
            this.lblRecordDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordDetails.ForeColor = System.Drawing.Color.Black;
            this.lblRecordDetails.Location = new System.Drawing.Point(610, 5);
            this.lblRecordDetails.Name = "lblRecordDetails";
            this.lblRecordDetails.Size = new System.Drawing.Size(89, 15);
            this.lblRecordDetails.TabIndex = 81;
            this.lblRecordDetails.Text = "Record Details";
            // 
            // frmVesselAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1189, 647);
            this.Controls.Add(this.lblRecordDetails);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.pnlResults);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pnlSearch);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Name = "frmVesselAdmin";
            this.Text = "DAI Export - Vessel Admin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmVesselAdmin_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmVesselAdmin_MouseMove);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlResults.ResumeLayout(false);
            this.pnlResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtVesselName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlResults;
        protected System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.Label lblVesselRecords;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblRecordDetails;
        private RecordButtons recbuttons;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtVesselID;
        private System.Windows.Forms.TextBox txtVesselName_record;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNote_record;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboStatus_record;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLloydsCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtVesselShortName_record;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCreationDate;
        private System.Windows.Forms.TextBox txtCreatedBy;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox txtUpdatedBy;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.TextBox txtUpdatedDate;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.DataGridViewTextBoxColumn VoyageID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Vessel;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordStatus;
    }
}