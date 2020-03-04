namespace AutoExport
{
    partial class frmEventProcessing
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboVehStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStatusDate = new System.Windows.Forms.TextBox();
            this.cboCust = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboExporter = new System.Windows.Forms.ComboBox();
            this.cboForwarder = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVIN = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.btnAddRecord = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblVehRecords = new System.Windows.Forms.Label();
            this.btnSetBooking = new System.Windows.Forms.Button();
            this.ckActive = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ckAllrows = new System.Windows.Forms.CheckBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnSetForwarder = new System.Windows.Forms.Button();
            this.btnSetExporter = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnDestBarcodes = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.VehID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FreightForwarderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExporterID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Destination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BookingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CubicFeet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VehicleStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Received = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Forwarder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Exporter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecordStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.View = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
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
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VehID,
            this.CustomerID,
            this.FreightForwarderID,
            this.ExporterID,
            this.FullNote,
            this.VIN,
            this.Customer,
            this.Destination,
            this.BookingNumber,
            this.Weight,
            this.CubicFeet,
            this.Size,
            this.VehicleStatus,
            this.Received,
            this.Forwarder,
            this.Exporter,
            this.RecordStatus,
            this.View});
            this.dgResults.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dgResults.EnableHeadersVisualStyles = false;
            this.dgResults.Location = new System.Drawing.Point(45, 65);
            this.dgResults.Margin = new System.Windows.Forms.Padding(4);
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgResults.RowHeadersWidth = 15;
            this.dgResults.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgResults.Size = new System.Drawing.Size(1450, 563);
            this.dgResults.TabIndex = 4;
            this.dgResults.TabStop = false;
            this.dgResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResults_CellClick);
            this.dgResults.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResults_CellContentClick);
            this.dgResults.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgResults_CellFormatting);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 715);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 73;
            this.pictureBox2.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(45, 5);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 15);
            this.label10.TabIndex = 75;
            this.label10.Text = "Veh. Status:";
            // 
            // cboVehStatus
            // 
            this.cboVehStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVehStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboVehStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVehStatus.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboVehStatus.FormattingEnabled = true;
            this.cboVehStatus.Location = new System.Drawing.Point(45, 30);
            this.cboVehStatus.MaxDropDownItems = 40;
            this.cboVehStatus.Name = "cboVehStatus";
            this.cboVehStatus.Size = new System.Drawing.Size(195, 24);
            this.cboVehStatus.TabIndex = 76;
            this.cboVehStatus.SelectedIndexChanged += new System.EventHandler(this.cboVehStatus_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(250, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 15);
            this.label1.TabIndex = 77;
            this.label1.Text = "Status Date:";
            // 
            // txtStatusDate
            // 
            this.txtStatusDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatusDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusDate.ForeColor = System.Drawing.Color.Black;
            this.txtStatusDate.Location = new System.Drawing.Point(250, 30);
            this.txtStatusDate.Name = "txtStatusDate";
            this.txtStatusDate.Size = new System.Drawing.Size(100, 23);
            this.txtStatusDate.TabIndex = 78;
            this.txtStatusDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStatusDate_KeyPress);
            this.txtStatusDate.Validating += new System.ComponentModel.CancelEventHandler(this.txtStatusDate_Validating);
            // 
            // cboCust
            // 
            this.cboCust.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCust.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCust.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCust.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCust.FormattingEnabled = true;
            this.cboCust.Location = new System.Drawing.Point(360, 30);
            this.cboCust.MaxDropDownItems = 40;
            this.cboCust.Name = "cboCust";
            this.cboCust.Size = new System.Drawing.Size(145, 24);
            this.cboCust.TabIndex = 80;
            this.cboCust.SelectedIndexChanged += new System.EventHandler(this.cboCust_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(360, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 79;
            this.label2.Text = "Customer:";
            // 
            // cboExporter
            // 
            this.cboExporter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboExporter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboExporter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExporter.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboExporter.FormattingEnabled = true;
            this.cboExporter.Location = new System.Drawing.Point(720, 30);
            this.cboExporter.MaxDropDownItems = 40;
            this.cboExporter.Name = "cboExporter";
            this.cboExporter.Size = new System.Drawing.Size(195, 24);
            this.cboExporter.TabIndex = 84;
            // 
            // cboForwarder
            // 
            this.cboForwarder.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboForwarder.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboForwarder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboForwarder.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboForwarder.FormattingEnabled = true;
            this.cboForwarder.Location = new System.Drawing.Point(515, 30);
            this.cboForwarder.MaxDropDownItems = 40;
            this.cboForwarder.Name = "cboForwarder";
            this.cboForwarder.Size = new System.Drawing.Size(195, 24);
            this.cboForwarder.TabIndex = 83;
            this.cboForwarder.SelectedIndexChanged += new System.EventHandler(this.cboForwarder_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(720, 5);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 15);
            this.label5.TabIndex = 82;
            this.label5.Text = "Exporter:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(515, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 81;
            this.label4.Text = "Forwarder:";
            // 
            // txtVIN
            // 
            this.txtVIN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVIN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtVIN.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVIN.Location = new System.Drawing.Point(925, 30);
            this.txtVIN.Margin = new System.Windows.Forms.Padding(4);
            this.txtVIN.MaxLength = 17;
            this.txtVIN.MinimumSize = new System.Drawing.Size(45, 25);
            this.txtVIN.Name = "txtVIN";
            this.txtVIN.Size = new System.Drawing.Size(145, 23);
            this.txtVIN.TabIndex = 86;
            this.txtVIN.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtVIN_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(925, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 15);
            this.label3.TabIndex = 85;
            this.label3.Text = "VIN:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(1080, 5);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 15);
            this.label6.TabIndex = 87;
            this.label6.Text = "Destination:";
            // 
            // txtDestination
            // 
            this.txtDestination.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDestination.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDestination.Location = new System.Drawing.Point(1080, 30);
            this.txtDestination.Margin = new System.Windows.Forms.Padding(4);
            this.txtDestination.MaxLength = 17;
            this.txtDestination.MinimumSize = new System.Drawing.Size(45, 25);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(145, 23);
            this.txtDestination.TabIndex = 88;
            this.txtDestination.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDestination_KeyPress);
            this.txtDestination.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDestination_KeyUp);
            this.txtDestination.Leave += new System.EventHandler(this.txtDestination_Leave);
            // 
            // btnAddRecord
            // 
            this.btnAddRecord.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRecord.Location = new System.Drawing.Point(1367, 31);
            this.btnAddRecord.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddRecord.Name = "btnAddRecord";
            this.btnAddRecord.Size = new System.Drawing.Size(100, 25);
            this.btnAddRecord.TabIndex = 90;
            this.btnAddRecord.Text = "Add Record";
            this.btnAddRecord.UseVisualStyleBackColor = true;
            this.btnAddRecord.Click += new System.EventHandler(this.btnAddRecord_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(1237, 30);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 25);
            this.btnClear.TabIndex = 89;
            this.btnClear.Text = "Clear Results";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblVehRecords
            // 
            this.lblVehRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVehRecords.AutoSize = true;
            this.lblVehRecords.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVehRecords.Location = new System.Drawing.Point(45, 638);
            this.lblVehRecords.Name = "lblVehRecords";
            this.lblVehRecords.Size = new System.Drawing.Size(67, 15);
            this.lblVehRecords.TabIndex = 91;
            this.lblVehRecords.Text = "Records: 0";
            // 
            // btnSetBooking
            // 
            this.btnSetBooking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetBooking.Enabled = false;
            this.btnSetBooking.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetBooking.Location = new System.Drawing.Point(45, 665);
            this.btnSetBooking.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetBooking.Name = "btnSetBooking";
            this.btnSetBooking.Size = new System.Drawing.Size(100, 25);
            this.btnSetBooking.TabIndex = 92;
            this.btnSetBooking.Text = "Set Booking #";
            this.btnSetBooking.UseVisualStyleBackColor = true;
            this.btnSetBooking.Click += new System.EventHandler(this.btnSetBooking_Click);
            // 
            // ckActive
            // 
            this.ckActive.AutoSize = true;
            this.ckActive.Checked = true;
            this.ckActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckActive.Location = new System.Drawing.Point(1252, 5);
            this.ckActive.Name = "ckActive";
            this.ckActive.Size = new System.Drawing.Size(221, 19);
            this.ckActive.TabIndex = 93;
            this.ckActive.Text = "Only show ACTIVE customers, etc.";
            this.ckActive.UseVisualStyleBackColor = true;
            this.ckActive.CheckedChanged += new System.EventHandler(this.ckActive_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Pink;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(1430, 665);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 25);
            this.btnCancel.TabIndex = 94;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ckAllrows
            // 
            this.ckAllrows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckAllrows.AutoSize = true;
            this.ckAllrows.Enabled = false;
            this.ckAllrows.Location = new System.Drawing.Point(1280, 665);
            this.ckAllrows.Name = "ckAllrows";
            this.ckAllrows.Size = new System.Drawing.Size(110, 19);
            this.ckAllrows.TabIndex = 95;
            this.ckAllrows.Text = "Select all rows";
            this.ckAllrows.UseVisualStyleBackColor = true;
            this.ckAllrows.CheckedChanged += new System.EventHandler(this.ckAllrows_CheckedChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(1159, 665);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 25);
            this.btnRemove.TabIndex = 96;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnSetForwarder
            // 
            this.btnSetForwarder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetForwarder.Enabled = false;
            this.btnSetForwarder.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetForwarder.Location = new System.Drawing.Point(160, 665);
            this.btnSetForwarder.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetForwarder.Name = "btnSetForwarder";
            this.btnSetForwarder.Size = new System.Drawing.Size(100, 25);
            this.btnSetForwarder.TabIndex = 97;
            this.btnSetForwarder.Text = "Set Forwarder";
            this.btnSetForwarder.UseVisualStyleBackColor = true;
            this.btnSetForwarder.Click += new System.EventHandler(this.btnSetForwarder_Click);
            // 
            // btnSetExporter
            // 
            this.btnSetExporter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetExporter.Enabled = false;
            this.btnSetExporter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetExporter.Location = new System.Drawing.Point(275, 665);
            this.btnSetExporter.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetExporter.Name = "btnSetExporter";
            this.btnSetExporter.Size = new System.Drawing.Size(100, 25);
            this.btnSetExporter.TabIndex = 98;
            this.btnSetExporter.Text = "Set Exporter";
            this.btnSetExporter.UseVisualStyleBackColor = true;
            this.btnSetExporter.Click += new System.EventHandler(this.btnSetExporter_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnProcess.Enabled = false;
            this.btnProcess.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.Location = new System.Drawing.Point(390, 665);
            this.btnProcess.Margin = new System.Windows.Forms.Padding(4);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(100, 25);
            this.btnProcess.TabIndex = 99;
            this.btnProcess.Text = "Process Batch";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(975, 665);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 25);
            this.btnExport.TabIndex = 100;
            this.btnExport.Text = "Export Results";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExport.UseCompatibleTextRendering = true;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnDestBarcodes
            // 
            this.btnDestBarcodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDestBarcodes.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDestBarcodes.Location = new System.Drawing.Point(802, 665);
            this.btnDestBarcodes.Name = "btnDestBarcodes";
            this.btnDestBarcodes.Size = new System.Drawing.Size(100, 25);
            this.btnDestBarcodes.TabIndex = 101;
            this.btnDestBarcodes.Text = "Dest. Barcodes";
            this.btnDestBarcodes.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDestBarcodes.UseCompatibleTextRendering = true;
            this.btnDestBarcodes.UseVisualStyleBackColor = true;
            this.btnDestBarcodes.Click += new System.EventHandler(this.btnDestBarcodes_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.Image = global::AutoExport.Properties.Resources.Menu1;
            this.btnMenu.Location = new System.Drawing.Point(5, 10);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 30);
            this.btnMenu.TabIndex = 102;
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // VehID
            // 
            this.VehID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.VehID.DataPropertyName = "AutoportExportVehiclesID";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.VehID.DefaultCellStyle = dataGridViewCellStyle3;
            this.VehID.FillWeight = 1F;
            this.VehID.HeaderText = "VehID";
            this.VehID.MaxInputLength = 5;
            this.VehID.MinimumWidth = 2;
            this.VehID.Name = "VehID";
            this.VehID.ReadOnly = true;
            this.VehID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.VehID.Visible = false;
            this.VehID.Width = 2;
            // 
            // CustomerID
            // 
            this.CustomerID.DataPropertyName = "CustomerID";
            this.CustomerID.HeaderText = "CustomerID";
            this.CustomerID.MinimumWidth = 2;
            this.CustomerID.Name = "CustomerID";
            this.CustomerID.ReadOnly = true;
            this.CustomerID.Visible = false;
            this.CustomerID.Width = 2;
            // 
            // FreightForwarderID
            // 
            this.FreightForwarderID.DataPropertyName = "FreightForwarderID";
            this.FreightForwarderID.HeaderText = "FreightForwarderID";
            this.FreightForwarderID.MinimumWidth = 2;
            this.FreightForwarderID.Name = "FreightForwarderID";
            this.FreightForwarderID.ReadOnly = true;
            this.FreightForwarderID.Visible = false;
            this.FreightForwarderID.Width = 2;
            // 
            // ExporterID
            // 
            this.ExporterID.DataPropertyName = "ExporterID";
            this.ExporterID.HeaderText = "ExporterID";
            this.ExporterID.MinimumWidth = 2;
            this.ExporterID.Name = "ExporterID";
            this.ExporterID.ReadOnly = true;
            this.ExporterID.Visible = false;
            this.ExporterID.Width = 2;
            // 
            // FullNote
            // 
            this.FullNote.DataPropertyName = "Note";
            this.FullNote.HeaderText = "FullNote";
            this.FullNote.MinimumWidth = 2;
            this.FullNote.Name = "FullNote";
            this.FullNote.ReadOnly = true;
            this.FullNote.Visible = false;
            this.FullNote.Width = 2;
            // 
            // VIN
            // 
            this.VIN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.VIN.DataPropertyName = "VIN";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.VIN.DefaultCellStyle = dataGridViewCellStyle4;
            this.VIN.HeaderText = "VIN";
            this.VIN.MaxInputLength = 8;
            this.VIN.MinimumWidth = 65;
            this.VIN.Name = "VIN";
            this.VIN.ReadOnly = true;
            this.VIN.Width = 150;
            // 
            // Customer
            // 
            this.Customer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Customer.DataPropertyName = "customer";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Customer.DefaultCellStyle = dataGridViewCellStyle5;
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            this.Customer.ReadOnly = true;
            this.Customer.Width = 75;
            // 
            // Destination
            // 
            this.Destination.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Destination.DataPropertyName = "DestinationName";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Destination.DefaultCellStyle = dataGridViewCellStyle6;
            this.Destination.HeaderText = "Destination";
            this.Destination.MaxInputLength = 2;
            this.Destination.MinimumWidth = 25;
            this.Destination.Name = "Destination";
            this.Destination.ReadOnly = true;
            // 
            // BookingNumber
            // 
            this.BookingNumber.DataPropertyName = "BookingNumber";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BookingNumber.DefaultCellStyle = dataGridViewCellStyle7;
            this.BookingNumber.HeaderText = "Booking #";
            this.BookingNumber.Name = "BookingNumber";
            this.BookingNumber.ReadOnly = true;
            // 
            // Weight
            // 
            this.Weight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Weight.DataPropertyName = "vehicleweight";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Weight.DefaultCellStyle = dataGridViewCellStyle8;
            this.Weight.HeaderText = "Weight";
            this.Weight.Name = "Weight";
            this.Weight.ReadOnly = true;
            this.Weight.Width = 65;
            // 
            // CubicFeet
            // 
            this.CubicFeet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CubicFeet.DataPropertyName = "Vehiclecubicfeet";
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.Format = "d";
            dataGridViewCellStyle9.NullValue = null;
            this.CubicFeet.DefaultCellStyle = dataGridViewCellStyle9;
            this.CubicFeet.HeaderText = "Cubic Ft.";
            this.CubicFeet.Name = "CubicFeet";
            this.CubicFeet.ReadOnly = true;
            this.CubicFeet.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CubicFeet.Width = 85;
            // 
            // Size
            // 
            this.Size.DataPropertyName = "SizeClass";
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size.DefaultCellStyle = dataGridViewCellStyle10;
            this.Size.HeaderText = "Size";
            this.Size.Name = "Size";
            this.Size.ReadOnly = true;
            this.Size.Width = 50;
            // 
            // VehicleStatus
            // 
            this.VehicleStatus.DataPropertyName = "VehicleStatus";
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VehicleStatus.DefaultCellStyle = dataGridViewCellStyle11;
            this.VehicleStatus.HeaderText = "Vehicle Status";
            this.VehicleStatus.Name = "VehicleStatus";
            this.VehicleStatus.ReadOnly = true;
            this.VehicleStatus.Width = 165;
            // 
            // Received
            // 
            this.Received.DataPropertyName = "statusdate";
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.Format = "d";
            dataGridViewCellStyle12.NullValue = null;
            this.Received.DefaultCellStyle = dataGridViewCellStyle12;
            this.Received.HeaderText = "Status Date";
            this.Received.Name = "Received";
            this.Received.ReadOnly = true;
            // 
            // Forwarder
            // 
            this.Forwarder.DataPropertyName = "Forwarder";
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Forwarder.DefaultCellStyle = dataGridViewCellStyle13;
            this.Forwarder.HeaderText = "Forwarder";
            this.Forwarder.Name = "Forwarder";
            this.Forwarder.ReadOnly = true;
            this.Forwarder.Width = 175;
            // 
            // Exporter
            // 
            this.Exporter.DataPropertyName = "Exporter";
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Exporter.DefaultCellStyle = dataGridViewCellStyle14;
            this.Exporter.HeaderText = "Exporter";
            this.Exporter.Name = "Exporter";
            this.Exporter.ReadOnly = true;
            this.Exporter.Width = 175;
            // 
            // RecordStatus
            // 
            this.RecordStatus.DataPropertyName = "RecordStatus";
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordStatus.DefaultCellStyle = dataGridViewCellStyle15;
            this.RecordStatus.HeaderText = "Record Status";
            this.RecordStatus.Name = "RecordStatus";
            this.RecordStatus.ReadOnly = true;
            this.RecordStatus.Width = 150;
            // 
            // View
            // 
            this.View.DataPropertyName = "NoteToView";
            this.View.HeaderText = "Note";
            this.View.Name = "View";
            this.View.ReadOnly = true;
            this.View.Width = 40;
            // 
            // frmEventProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1509, 702);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.btnDestBarcodes);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnSetExporter);
            this.Controls.Add(this.btnSetForwarder);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.ckAllrows);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.ckActive);
            this.Controls.Add(this.btnSetBooking);
            this.Controls.Add(this.lblVehRecords);
            this.Controls.Add(this.btnAddRecord);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtVIN);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboExporter);
            this.Controls.Add(this.cboForwarder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboCust);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtStatusDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboVehStatus);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.dgResults);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Location = new System.Drawing.Point(10, 65);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmEventProcessing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DAI Export - Event Processing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEventProcessing_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmEventProcessing_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboVehStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStatusDate;
        private System.Windows.Forms.ComboBox cboCust;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboExporter;
        private System.Windows.Forms.ComboBox cboForwarder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtVIN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Button btnAddRecord;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblVehRecords;
        private System.Windows.Forms.Button btnSetBooking;
        private System.Windows.Forms.CheckBox ckActive;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox ckAllrows;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnSetForwarder;
        private System.Windows.Forms.Button btnSetExporter;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnDestBarcodes;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.DataGridViewTextBoxColumn VehID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FreightForwarderID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExporterID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn VIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Destination;
        private System.Windows.Forms.DataGridViewTextBoxColumn BookingNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn CubicFeet;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        private System.Windows.Forms.DataGridViewTextBoxColumn VehicleStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Received;
        private System.Windows.Forms.DataGridViewTextBoxColumn Forwarder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Exporter;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn View;
    }
}