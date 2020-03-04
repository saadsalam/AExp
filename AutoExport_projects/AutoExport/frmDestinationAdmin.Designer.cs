﻿namespace AutoExport
{
    partial class frmDestinationAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDestinationAdmin));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cboCust = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSheetColor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHandheldAbbrev = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pnlResults = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.CodeID_dest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodeID_color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Destination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HandheldAbbrev = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SheetColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SheetColorDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecordStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDestinationRecords = new System.Windows.Forms.Label();
            this.lblResults = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.lblRecordDetails = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDestination_record = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtHandheldAbbrev_record = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cboStatus_record = new System.Windows.Forms.ComboBox();
            this.label61 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.txtCreatedBy = new System.Windows.Forms.TextBox();
            this.txtCreationDate = new System.Windows.Forms.TextBox();
            this.label60 = new System.Windows.Forms.Label();
            this.txtUpdatedDate = new System.Windows.Forms.TextBox();
            this.label58 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtUpdatedBy = new System.Windows.Forms.TextBox();
            this.txtCodeID_dest = new System.Windows.Forms.TextBox();
            this.btnColor = new System.Windows.Forms.Button();
            this.pnlColor = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.pnlCustColor = new System.Windows.Forms.Panel();
            this.txtSheetColor_record = new System.Windows.Forms.TextBox();
            this.lblRGB = new System.Windows.Forms.Label();
            this.btnDelCustColor = new System.Windows.Forms.Button();
            this.btnCancelCustColor = new System.Windows.Forms.Button();
            this.lblCustColor = new System.Windows.Forms.Label();
            this.btnSaveCustColor = new System.Windows.Forms.Button();
            this.cboCust_record = new System.Windows.Forms.ComboBox();
            this.btnModCustColor = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.btnNewCustColor = new System.Windows.Forms.Button();
            this.txtCodeID_color = new System.Windows.Forms.TextBox();
            this.recbuttons = new AutoExport.RecordButtons();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlCustColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearch.Controls.Add(this.cboCust);
            this.pnlSearch.Controls.Add(this.label4);
            this.pnlSearch.Controls.Add(this.txtSheetColor);
            this.pnlSearch.Controls.Add(this.label3);
            this.pnlSearch.Controls.Add(this.txtHandheldAbbrev);
            this.pnlSearch.Controls.Add(this.label2);
            this.pnlSearch.Controls.Add(this.cboStatus);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Controls.Add(this.txtDestination);
            this.pnlSearch.Controls.Add(this.label5);
            this.pnlSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlSearch.Location = new System.Drawing.Point(45, 10);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(450, 140);
            this.pnlSearch.TabIndex = 1;
            // 
            // cboCust
            // 
            this.cboCust.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCust.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCust.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCust.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCust.FormattingEnabled = true;
            this.cboCust.Location = new System.Drawing.Point(145, 75);
            this.cboCust.Name = "cboCust";
            this.cboCust.Size = new System.Drawing.Size(278, 24);
            this.cboCust.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 15);
            this.label4.TabIndex = 27;
            this.label4.Text = "Customer:";
            // 
            // txtSheetColor
            // 
            this.txtSheetColor.BackColor = System.Drawing.SystemColors.Window;
            this.txtSheetColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSheetColor.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSheetColor.Location = new System.Drawing.Point(300, 42);
            this.txtSheetColor.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtSheetColor.MinimumSize = new System.Drawing.Size(5, 25);
            this.txtSheetColor.Name = "txtSheetColor";
            this.txtSheetColor.Size = new System.Drawing.Size(123, 23);
            this.txtSheetColor.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(220, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 15);
            this.label3.TabIndex = 24;
            this.label3.Text = "Sheet Color:";
            // 
            // txtHandheldAbbrev
            // 
            this.txtHandheldAbbrev.BackColor = System.Drawing.SystemColors.Window;
            this.txtHandheldAbbrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHandheldAbbrev.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHandheldAbbrev.Location = new System.Drawing.Point(145, 42);
            this.txtHandheldAbbrev.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtHandheldAbbrev.MinimumSize = new System.Drawing.Size(5, 25);
            this.txtHandheldAbbrev.Name = "txtHandheldAbbrev";
            this.txtHandheldAbbrev.Size = new System.Drawing.Size(60, 23);
            this.txtHandheldAbbrev.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "Handheld Abbrev.:";
            // 
            // cboStatus
            // 
            this.cboStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(145, 105);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(278, 24);
            this.cboStatus.TabIndex = 20;
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 15);
            this.label1.TabIndex = 21;
            this.label1.Text = "Record Status:";
            // 
            // txtDestination
            // 
            this.txtDestination.BackColor = System.Drawing.SystemColors.Window;
            this.txtDestination.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDestination.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDestination.Location = new System.Drawing.Point(145, 15);
            this.txtDestination.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtDestination.MinimumSize = new System.Drawing.Size(240, 25);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(278, 23);
            this.txtDestination.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 15);
            this.label5.TabIndex = 17;
            this.label5.Text = "Destination:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
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
            this.pnlResults.Controls.Add(this.label11);
            this.pnlResults.Controls.Add(this.dgResults);
            this.pnlResults.Controls.Add(this.lblDestinationRecords);
            this.pnlResults.Location = new System.Drawing.Point(45, 160);
            this.pnlResults.Name = "pnlResults";
            this.pnlResults.Size = new System.Drawing.Size(550, 475);
            this.pnlResults.TabIndex = 74;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(181, 455);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(359, 16);
            this.label11.TabIndex = 3;
            this.label11.Text = "If Customer or Desc. are not listed, WHITE is the default";
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
            this.CodeID_dest,
            this.CodeID_color,
            this.Color,
            this.CustomerID,
            this.Destination,
            this.HandheldAbbrev,
            this.Customer,
            this.SheetColor,
            this.SheetColorDesc,
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
            this.dgResults.Size = new System.Drawing.Size(535, 435);
            this.dgResults.TabIndex = 2;
            this.dgResults.TabStop = false;
            this.dgResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResults_CellClick);
            this.dgResults.Sorted += new System.EventHandler(this.dgResults_Sorted);
            // 
            // CodeID_dest
            // 
            this.CodeID_dest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CodeID_dest.DataPropertyName = "CodeID_dest";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.CodeID_dest.DefaultCellStyle = dataGridViewCellStyle3;
            this.CodeID_dest.FillWeight = 1F;
            this.CodeID_dest.HeaderText = "";
            this.CodeID_dest.MaxInputLength = 5;
            this.CodeID_dest.MinimumWidth = 2;
            this.CodeID_dest.Name = "CodeID_dest";
            this.CodeID_dest.ReadOnly = true;
            this.CodeID_dest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CodeID_dest.Visible = false;
            this.CodeID_dest.Width = 2;
            // 
            // CodeID_color
            // 
            this.CodeID_color.DataPropertyName = "CodeID_color";
            this.CodeID_color.HeaderText = "";
            this.CodeID_color.MinimumWidth = 2;
            this.CodeID_color.Name = "CodeID_color";
            this.CodeID_color.ReadOnly = true;
            this.CodeID_color.Width = 2;
            // 
            // Color
            // 
            this.Color.DataPropertyName = "Color";
            this.Color.HeaderText = "";
            this.Color.Name = "Color";
            this.Color.ReadOnly = true;
            this.Color.Visible = false;
            // 
            // CustomerID
            // 
            this.CustomerID.DataPropertyName = "CustomerID";
            this.CustomerID.HeaderText = "";
            this.CustomerID.MinimumWidth = 2;
            this.CustomerID.Name = "CustomerID";
            this.CustomerID.ReadOnly = true;
            this.CustomerID.Visible = false;
            this.CustomerID.Width = 2;
            // 
            // Destination
            // 
            this.Destination.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Destination.DataPropertyName = "Destination";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Destination.DefaultCellStyle = dataGridViewCellStyle4;
            this.Destination.HeaderText = "Destination";
            this.Destination.Name = "Destination";
            this.Destination.ReadOnly = true;
            this.Destination.Width = 125;
            // 
            // HandheldAbbrev
            // 
            this.HandheldAbbrev.DataPropertyName = "Abbrev";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            this.HandheldAbbrev.DefaultCellStyle = dataGridViewCellStyle5;
            this.HandheldAbbrev.HeaderText = "Abbr.";
            this.HandheldAbbrev.Name = "HandheldAbbrev";
            this.HandheldAbbrev.ReadOnly = true;
            this.HandheldAbbrev.Width = 50;
            // 
            // Customer
            // 
            this.Customer.DataPropertyName = "Customer";
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            this.Customer.DefaultCellStyle = dataGridViewCellStyle6;
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            this.Customer.ReadOnly = true;
            // 
            // SheetColor
            // 
            this.SheetColor.HeaderText = "Color";
            this.SheetColor.Name = "SheetColor";
            this.SheetColor.ReadOnly = true;
            this.SheetColor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SheetColor.Width = 50;
            // 
            // SheetColorDesc
            // 
            this.SheetColorDesc.DataPropertyName = "ColorDesc";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            this.SheetColorDesc.DefaultCellStyle = dataGridViewCellStyle7;
            this.SheetColorDesc.HeaderText = "Desc.";
            this.SheetColorDesc.Name = "SheetColorDesc";
            this.SheetColorDesc.ReadOnly = true;
            // 
            // RecordStatus
            // 
            this.RecordStatus.DataPropertyName = "RecordStatus";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordStatus.DefaultCellStyle = dataGridViewCellStyle8;
            this.RecordStatus.HeaderText = "Status";
            this.RecordStatus.Name = "RecordStatus";
            this.RecordStatus.ReadOnly = true;
            // 
            // lblDestinationRecords
            // 
            this.lblDestinationRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDestinationRecords.AutoSize = true;
            this.lblDestinationRecords.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestinationRecords.Location = new System.Drawing.Point(5, 455);
            this.lblDestinationRecords.Name = "lblDestinationRecords";
            this.lblDestinationRecords.Size = new System.Drawing.Size(28, 15);
            this.lblDestinationRecords.TabIndex = 1;
            this.lblDestinationRecords.Text = "567";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResults.ForeColor = System.Drawing.Color.Black;
            this.lblResults.Location = new System.Drawing.Point(50, 155);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(92, 15);
            this.lblResults.TabIndex = 75;
            this.lblResults.Text = "Search Results";
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(500, 125);
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
            this.btnMenu.Image = ((System.Drawing.Image)(resources.GetObject("btnMenu.Image")));
            this.btnMenu.Location = new System.Drawing.Point(5, 10);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 30);
            this.btnMenu.TabIndex = 79;
            this.btnMenu.UseVisualStyleBackColor = true;
            // 
            // lblRecordDetails
            // 
            this.lblRecordDetails.AutoSize = true;
            this.lblRecordDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordDetails.ForeColor = System.Drawing.Color.Black;
            this.lblRecordDetails.Location = new System.Drawing.Point(610, 5);
            this.lblRecordDetails.Name = "lblRecordDetails";
            this.lblRecordDetails.Size = new System.Drawing.Size(112, 15);
            this.lblRecordDetails.TabIndex = 81;
            this.lblRecordDetails.Text = "Destination Details";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 15);
            this.label6.TabIndex = 18;
            this.label6.Text = "Destination:";
            // 
            // txtDestination_record
            // 
            this.txtDestination_record.BackColor = System.Drawing.SystemColors.Window;
            this.txtDestination_record.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDestination_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDestination_record.Location = new System.Drawing.Point(130, 15);
            this.txtDestination_record.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtDestination_record.MinimumSize = new System.Drawing.Size(200, 25);
            this.txtDestination_record.Name = "txtDestination_record";
            this.txtDestination_record.Size = new System.Drawing.Size(300, 23);
            this.txtDestination_record.TabIndex = 20;
            this.txtDestination_record.TextChanged += new System.EventHandler(this.txtDestination_record_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 15);
            this.label7.TabIndex = 21;
            this.label7.Text = "Handheld Abbev:";
            // 
            // txtHandheldAbbrev_record
            // 
            this.txtHandheldAbbrev_record.BackColor = System.Drawing.SystemColors.Window;
            this.txtHandheldAbbrev_record.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHandheldAbbrev_record.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtHandheldAbbrev_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHandheldAbbrev_record.Location = new System.Drawing.Point(130, 45);
            this.txtHandheldAbbrev_record.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtHandheldAbbrev_record.MaxLength = 10;
            this.txtHandheldAbbrev_record.MinimumSize = new System.Drawing.Size(200, 25);
            this.txtHandheldAbbrev_record.Name = "txtHandheldAbbrev_record";
            this.txtHandheldAbbrev_record.Size = new System.Drawing.Size(300, 23);
            this.txtHandheldAbbrev_record.TabIndex = 22;
            this.txtHandheldAbbrev_record.TextChanged += new System.EventHandler(this.txtHandheldAbbrev_record_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 45);
            this.label8.MaximumSize = new System.Drawing.Size(75, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "Sheet Color (per Cust.)";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(5, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 15);
            this.label9.TabIndex = 26;
            this.label9.Text = "Record Status:";
            // 
            // cboStatus_record
            // 
            this.cboStatus_record.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStatus_record.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStatus_record.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus_record.FormattingEnabled = true;
            this.cboStatus_record.Location = new System.Drawing.Point(130, 75);
            this.cboStatus_record.Name = "cboStatus_record";
            this.cboStatus_record.Size = new System.Drawing.Size(300, 24);
            this.cboStatus_record.TabIndex = 25;
            this.cboStatus_record.SelectedIndexChanged += new System.EventHandler(this.cboStatus_record_SelectedIndexChanged);
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label61.ForeColor = System.Drawing.Color.Blue;
            this.label61.Location = new System.Drawing.Point(5, 105);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(74, 15);
            this.label61.TabIndex = 73;
            this.label61.Text = "Created On:";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label59.ForeColor = System.Drawing.Color.Blue;
            this.label59.Location = new System.Drawing.Point(5, 135);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(72, 15);
            this.label59.TabIndex = 74;
            this.label59.Text = "Created By:";
            // 
            // txtCreatedBy
            // 
            this.txtCreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreatedBy.Enabled = false;
            this.txtCreatedBy.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreatedBy.ForeColor = System.Drawing.Color.Black;
            this.txtCreatedBy.Location = new System.Drawing.Point(130, 135);
            this.txtCreatedBy.Name = "txtCreatedBy";
            this.txtCreatedBy.ReadOnly = true;
            this.txtCreatedBy.Size = new System.Drawing.Size(300, 23);
            this.txtCreatedBy.TabIndex = 75;
            this.txtCreatedBy.TabStop = false;
            // 
            // txtCreationDate
            // 
            this.txtCreationDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreationDate.Enabled = false;
            this.txtCreationDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreationDate.ForeColor = System.Drawing.Color.Black;
            this.txtCreationDate.Location = new System.Drawing.Point(130, 105);
            this.txtCreationDate.Name = "txtCreationDate";
            this.txtCreationDate.ReadOnly = true;
            this.txtCreationDate.Size = new System.Drawing.Size(300, 23);
            this.txtCreationDate.TabIndex = 76;
            this.txtCreationDate.TabStop = false;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label60.ForeColor = System.Drawing.Color.Blue;
            this.label60.Location = new System.Drawing.Point(5, 165);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(76, 15);
            this.label60.TabIndex = 77;
            this.label60.Text = "Updated On:";
            // 
            // txtUpdatedDate
            // 
            this.txtUpdatedDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUpdatedDate.Enabled = false;
            this.txtUpdatedDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdatedDate.ForeColor = System.Drawing.Color.Black;
            this.txtUpdatedDate.Location = new System.Drawing.Point(130, 165);
            this.txtUpdatedDate.Name = "txtUpdatedDate";
            this.txtUpdatedDate.ReadOnly = true;
            this.txtUpdatedDate.Size = new System.Drawing.Size(300, 23);
            this.txtUpdatedDate.TabIndex = 78;
            this.txtUpdatedDate.TabStop = false;
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label58.ForeColor = System.Drawing.Color.Blue;
            this.label58.Location = new System.Drawing.Point(5, 200);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(74, 15);
            this.label58.TabIndex = 79;
            this.label58.Text = "Updated By:";
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
            // txtUpdatedBy
            // 
            this.txtUpdatedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUpdatedBy.Enabled = false;
            this.txtUpdatedBy.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdatedBy.ForeColor = System.Drawing.Color.Black;
            this.txtUpdatedBy.Location = new System.Drawing.Point(130, 200);
            this.txtUpdatedBy.Name = "txtUpdatedBy";
            this.txtUpdatedBy.ReadOnly = true;
            this.txtUpdatedBy.Size = new System.Drawing.Size(300, 23);
            this.txtUpdatedBy.TabIndex = 80;
            this.txtUpdatedBy.TabStop = false;
            // 
            // txtCodeID_dest
            // 
            this.txtCodeID_dest.BackColor = System.Drawing.SystemColors.Window;
            this.txtCodeID_dest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCodeID_dest.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodeID_dest.Location = new System.Drawing.Point(461, 45);
            this.txtCodeID_dest.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodeID_dest.Name = "txtCodeID_dest";
            this.txtCodeID_dest.Size = new System.Drawing.Size(23, 23);
            this.txtCodeID_dest.TabIndex = 84;
            this.txtCodeID_dest.Visible = false;
            // 
            // btnColor
            // 
            this.btnColor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnColor.Location = new System.Drawing.Point(124, 324);
            this.btnColor.Margin = new System.Windows.Forms.Padding(4);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(100, 25);
            this.btnColor.TabIndex = 86;
            this.btnColor.Text = "Select Color";
            this.btnColor.UseVisualStyleBackColor = true;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // pnlColor
            // 
            this.pnlColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlColor.Enabled = false;
            this.pnlColor.Location = new System.Drawing.Point(125, 75);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.Size = new System.Drawing.Size(300, 220);
            this.pnlColor.TabIndex = 87;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.pnlCustColor);
            this.panel1.Controls.Add(this.txtCodeID_color);
            this.panel1.Controls.Add(this.txtCodeID_dest);
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
            this.panel1.Controls.Add(this.cboStatus_record);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtHandheldAbbrev_record);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtDestination_record);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(605, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(575, 625);
            this.panel1.TabIndex = 80;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(10, 245);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 15);
            this.label12.TabIndex = 92;
            this.label12.Text = "Customer/Color";
            // 
            // pnlCustColor
            // 
            this.pnlCustColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCustColor.Controls.Add(this.txtSheetColor_record);
            this.pnlCustColor.Controls.Add(this.lblRGB);
            this.pnlCustColor.Controls.Add(this.btnDelCustColor);
            this.pnlCustColor.Controls.Add(this.btnCancelCustColor);
            this.pnlCustColor.Controls.Add(this.btnColor);
            this.pnlCustColor.Controls.Add(this.lblCustColor);
            this.pnlCustColor.Controls.Add(this.btnSaveCustColor);
            this.pnlCustColor.Controls.Add(this.cboCust_record);
            this.pnlCustColor.Controls.Add(this.btnModCustColor);
            this.pnlCustColor.Controls.Add(this.label10);
            this.pnlCustColor.Controls.Add(this.btnNewCustColor);
            this.pnlCustColor.Controls.Add(this.pnlColor);
            this.pnlCustColor.Controls.Add(this.label8);
            this.pnlCustColor.Location = new System.Drawing.Point(5, 250);
            this.pnlCustColor.Name = "pnlCustColor";
            this.pnlCustColor.Size = new System.Drawing.Size(562, 358);
            this.pnlCustColor.TabIndex = 98;
            // 
            // txtSheetColor_record
            // 
            this.txtSheetColor_record.BackColor = System.Drawing.Color.White;
            this.txtSheetColor_record.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSheetColor_record.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSheetColor_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSheetColor_record.Location = new System.Drawing.Point(125, 45);
            this.txtSheetColor_record.Name = "txtSheetColor_record";
            this.txtSheetColor_record.Size = new System.Drawing.Size(300, 23);
            this.txtSheetColor_record.TabIndex = 100;
            // 
            // lblRGB
            // 
            this.lblRGB.AutoSize = true;
            this.lblRGB.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRGB.ForeColor = System.Drawing.Color.Black;
            this.lblRGB.Location = new System.Drawing.Point(121, 298);
            this.lblRGB.Name = "lblRGB";
            this.lblRGB.Size = new System.Drawing.Size(0, 15);
            this.lblRGB.TabIndex = 99;
            // 
            // btnDelCustColor
            // 
            this.btnDelCustColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnDelCustColor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelCustColor.ForeColor = System.Drawing.Color.Black;
            this.btnDelCustColor.Location = new System.Drawing.Point(430, 195);
            this.btnDelCustColor.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelCustColor.Name = "btnDelCustColor";
            this.btnDelCustColor.Size = new System.Drawing.Size(125, 25);
            this.btnDelCustColor.TabIndex = 98;
            this.btnDelCustColor.Text = "Delete Cust./Color";
            this.btnDelCustColor.UseVisualStyleBackColor = false;
            this.btnDelCustColor.Click += new System.EventHandler(this.btnDelCustColor_Click);
            // 
            // btnCancelCustColor
            // 
            this.btnCancelCustColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnCancelCustColor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelCustColor.ForeColor = System.Drawing.Color.Black;
            this.btnCancelCustColor.Location = new System.Drawing.Point(432, 270);
            this.btnCancelCustColor.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancelCustColor.Name = "btnCancelCustColor";
            this.btnCancelCustColor.Size = new System.Drawing.Size(125, 25);
            this.btnCancelCustColor.TabIndex = 96;
            this.btnCancelCustColor.Text = "Cancel Cust./Color";
            this.btnCancelCustColor.UseVisualStyleBackColor = false;
            this.btnCancelCustColor.Click += new System.EventHandler(this.btnCancelCustColor_Click);
            // 
            // lblCustColor
            // 
            this.lblCustColor.AutoSize = true;
            this.lblCustColor.BackColor = System.Drawing.Color.Blue;
            this.lblCustColor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustColor.ForeColor = System.Drawing.Color.White;
            this.lblCustColor.Location = new System.Drawing.Point(430, 15);
            this.lblCustColor.MinimumSize = new System.Drawing.Size(125, 25);
            this.lblCustColor.Name = "lblCustColor";
            this.lblCustColor.Size = new System.Drawing.Size(125, 25);
            this.lblCustColor.TabIndex = 97;
            this.lblCustColor.Text = "Read Only";
            this.lblCustColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSaveCustColor
            // 
            this.btnSaveCustColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSaveCustColor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveCustColor.ForeColor = System.Drawing.Color.Black;
            this.btnSaveCustColor.Location = new System.Drawing.Point(431, 165);
            this.btnSaveCustColor.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveCustColor.Name = "btnSaveCustColor";
            this.btnSaveCustColor.Size = new System.Drawing.Size(125, 25);
            this.btnSaveCustColor.TabIndex = 95;
            this.btnSaveCustColor.Text = "Save Cust./Color";
            this.btnSaveCustColor.UseVisualStyleBackColor = false;
            this.btnSaveCustColor.Click += new System.EventHandler(this.btnSaveCustColor_Click);
            // 
            // cboCust_record
            // 
            this.cboCust_record.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCust_record.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCust_record.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCust_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCust_record.FormattingEnabled = true;
            this.cboCust_record.Location = new System.Drawing.Point(125, 15);
            this.cboCust_record.Name = "cboCust_record";
            this.cboCust_record.Size = new System.Drawing.Size(300, 24);
            this.cboCust_record.TabIndex = 89;
            this.cboCust_record.SelectedIndexChanged += new System.EventHandler(this.cboCust_record_SelectedIndexChanged);
            // 
            // btnModCustColor
            // 
            this.btnModCustColor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModCustColor.Location = new System.Drawing.Point(430, 105);
            this.btnModCustColor.Margin = new System.Windows.Forms.Padding(4);
            this.btnModCustColor.Name = "btnModCustColor";
            this.btnModCustColor.Size = new System.Drawing.Size(125, 25);
            this.btnModCustColor.TabIndex = 94;
            this.btnModCustColor.Text = "Modify Cust./Color";
            this.btnModCustColor.UseVisualStyleBackColor = true;
            this.btnModCustColor.Click += new System.EventHandler(this.btnModCustColor_Click);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(2, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 15);
            this.label10.TabIndex = 90;
            this.label10.Text = "Customer:";
            // 
            // btnNewCustColor
            // 
            this.btnNewCustColor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewCustColor.Location = new System.Drawing.Point(430, 75);
            this.btnNewCustColor.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewCustColor.Name = "btnNewCustColor";
            this.btnNewCustColor.Size = new System.Drawing.Size(125, 25);
            this.btnNewCustColor.TabIndex = 93;
            this.btnNewCustColor.Text = "New Cust./Color";
            this.btnNewCustColor.UseVisualStyleBackColor = true;
            this.btnNewCustColor.Click += new System.EventHandler(this.btnNewCustColor_Click);
            // 
            // txtCodeID_color
            // 
            this.txtCodeID_color.BackColor = System.Drawing.SystemColors.Window;
            this.txtCodeID_color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCodeID_color.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodeID_color.Location = new System.Drawing.Point(461, 77);
            this.txtCodeID_color.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodeID_color.Name = "txtCodeID_color";
            this.txtCodeID_color.Size = new System.Drawing.Size(23, 23);
            this.txtCodeID_color.TabIndex = 91;
            this.txtCodeID_color.Visible = false;
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
            // frmDestinationAdmin
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
            this.Location = new System.Drawing.Point(10, 65);
            this.Name = "frmDestinationAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DAI Export - Destination Admin";
            this.Activated += new System.EventHandler(this.frmDestinationAdmin_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDestinationAdmin_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmDestinationAdmin_MouseMove);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlResults.ResumeLayout(false);
            this.pnlResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlCustColor.ResumeLayout(false);
            this.pnlCustColor.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlResults;
        protected System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.Label lblDestinationRecords;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Label lblRecordDetails;
        private System.Windows.Forms.TextBox txtSheetColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtHandheldAbbrev;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDestination_record;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtHandheldAbbrev_record;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboStatus_record;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.TextBox txtCreatedBy;
        private System.Windows.Forms.TextBox txtCreationDate;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.TextBox txtUpdatedDate;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label lblStatus;
        private RecordButtons recbuttons;
        private System.Windows.Forms.TextBox txtUpdatedBy;
        private System.Windows.Forms.TextBox txtCodeID_dest;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.Panel pnlColor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboCust;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboCust_record;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCodeID_color;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodeID_dest;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodeID_color;
        private System.Windows.Forms.DataGridViewTextBoxColumn Color;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Destination;
        private System.Windows.Forms.DataGridViewTextBoxColumn HandheldAbbrev;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn SheetColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn SheetColorDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordStatus;
        private System.Windows.Forms.Button btnSaveCustColor;
        private System.Windows.Forms.Button btnModCustColor;
        private System.Windows.Forms.Button btnNewCustColor;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblCustColor;
        private System.Windows.Forms.Button btnCancelCustColor;
        private System.Windows.Forms.Panel pnlCustColor;
        private System.Windows.Forms.Button btnDelCustColor;
        private System.Windows.Forms.Label lblRGB;
        private System.Windows.Forms.TextBox txtSheetColor_record;
    }
}