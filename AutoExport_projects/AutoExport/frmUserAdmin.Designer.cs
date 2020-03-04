namespace AutoExport
{
    partial class frmUserAdmin
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pnlResults = new System.Windows.Forms.Panel();
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecordStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRecords = new System.Windows.Forms.Label();
            this.lblResults = new System.Windows.Forms.Label();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.recbuttons = new AutoExport.RecordButtons();
            this.ckHide = new System.Windows.Forms.CheckBox();
            this.btnPwdHelp = new System.Windows.Forms.Button();
            this.txtFax = new System.Windows.Forms.MaskedTextBox();
            this.txtCell = new System.Windows.Forms.MaskedTextBox();
            this.txtPhone = new System.Windows.Forms.MaskedTextBox();
            this.txtPassword_confirm = new System.Windows.Forms.TextBox();
            this.lblPwd_confirm = new System.Windows.Forms.Label();
            this.ckBilling = new System.Windows.Forms.CheckBox();
            this.lblOptional = new System.Windows.Forms.Label();
            this.lblRequired = new System.Windows.Forms.Label();
            this.cboStatus_record = new System.Windows.Forms.ComboBox();
            this.ckYard = new System.Windows.Forms.CheckBox();
            this.ckAdmin = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUpdatedBy = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtCreatedBy = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtUpdatedDate = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtCreationDate = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtPortPassID = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtEmpl = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtExtension = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.txtLname_record = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFname_record = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUname_record = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblRecordDetails = new System.Windows.Forms.Label();
            this.tipBtnMoveFirst = new System.Windows.Forms.ToolTip(this.components);
            this.tipBtnMovePrev = new System.Windows.Forms.ToolTip(this.components);
            this.tipBtnMoveNext = new System.Windows.Forms.ToolTip(this.components);
            this.tipBtnLast = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tipPassword = new System.Windows.Forms.ToolTip(this.components);
            this.tipBtnMenu = new System.Windows.Forms.ToolTip(this.components);
            this.pnlSearch.SuspendLayout();
            this.pnlResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            this.pnlUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.Transparent;
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearch.Controls.Add(this.cboStatus);
            this.pnlSearch.Controls.Add(this.label5);
            this.pnlSearch.Controls.Add(this.cboRole);
            this.pnlSearch.Controls.Add(this.label4);
            this.pnlSearch.Controls.Add(this.txtUname);
            this.pnlSearch.Controls.Add(this.label3);
            this.pnlSearch.Controls.Add(this.txtFname);
            this.pnlSearch.Controls.Add(this.label2);
            this.pnlSearch.Controls.Add(this.txtLname);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Location = new System.Drawing.Point(45, 10);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(475, 105);
            this.pnlSearch.TabIndex = 0;
            // 
            // cboStatus
            // 
            this.cboStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(343, 45);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(121, 24);
            this.cboStatus.TabIndex = 4;
            this.cboStatus.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboStatus_KeyDown);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(230, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Record Status:";
            // 
            // cboRole
            // 
            this.cboRole.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboRole.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRole.FormattingEnabled = true;
            this.cboRole.IntegralHeight = false;
            this.cboRole.ItemHeight = 16;
            this.cboRole.Location = new System.Drawing.Point(344, 15);
            this.cboRole.MinimumSize = new System.Drawing.Size(120, 0);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(120, 24);
            this.cboRole.TabIndex = 2;
            this.cboRole.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboRole_KeyDown);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(233, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Role:";
            // 
            // txtUname
            // 
            this.txtUname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUname.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUname.Location = new System.Drawing.Point(90, 15);
            this.txtUname.MinimumSize = new System.Drawing.Size(100, 25);
            this.txtUname.Name = "txtUname";
            this.txtUname.Size = new System.Drawing.Size(120, 23);
            this.txtUname.TabIndex = 5;
            this.txtUname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUname_KeyDown);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(5, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "User Name:";
            // 
            // txtFname
            // 
            this.txtFname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFname.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFname.Location = new System.Drawing.Point(90, 45);
            this.txtFname.MinimumSize = new System.Drawing.Size(100, 25);
            this.txtFname.Name = "txtFname";
            this.txtFname.Size = new System.Drawing.Size(120, 23);
            this.txtFname.TabIndex = 3;
            this.txtFname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFname_KeyDown);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "First Name:";
            // 
            // txtLname
            // 
            this.txtLname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLname.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtLname.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLname.Location = new System.Drawing.Point(90, 75);
            this.txtLname.MinimumSize = new System.Drawing.Size(100, 25);
            this.txtLname.Name = "txtLname";
            this.txtLname.Size = new System.Drawing.Size(120, 23);
            this.txtLname.TabIndex = 1;
            this.txtLname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLname_KeyDown);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Last Name:";
            // 
            // btnMenu
            // 
            this.btnMenu.Image = global::AutoExport.Properties.Resources.Menu1;
            this.btnMenu.Location = new System.Drawing.Point(5, 10);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 28);
            this.btnMenu.TabIndex = 70;
            this.tipBtnMenu.SetToolTip(this.btnMenu, "Move to Menu with other functions");
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(525, 90);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 25);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "Export Results";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(524, 40);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 25);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear Results";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(525, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 25);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.ForeColor = System.Drawing.Color.Black;
            this.lblSearch.Location = new System.Drawing.Point(55, 5);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(126, 15);
            this.lblSearch.TabIndex = 6;
            this.lblSearch.Text = "Enter Search Criteria";
            // 
            // pnlResults
            // 
            this.pnlResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlResults.Controls.Add(this.dgResults);
            this.pnlResults.Controls.Add(this.lblRecords);
            this.pnlResults.Location = new System.Drawing.Point(45, 127);
            this.pnlResults.Name = "pnlResults";
            this.pnlResults.Size = new System.Drawing.Size(475, 516);
            this.pnlResults.TabIndex = 7;
            // 
            // dgResults
            // 
            this.dgResults.AllowUserToAddRows = false;
            this.dgResults.AllowUserToDeleteRows = false;
            this.dgResults.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.dgResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgResults.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgResults.BackgroundColor = System.Drawing.Color.White;
            this.dgResults.CausesValidation = false;
            this.dgResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
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
            this.UserID,
            this.UserName,
            this.UserFullName,
            this.RecordStatus});
            this.dgResults.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dgResults.EnableHeadersVisualStyles = false;
            this.dgResults.Location = new System.Drawing.Point(5, 10);
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgResults.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgResults.RowTemplate.ReadOnly = true;
            this.dgResults.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgResults.Size = new System.Drawing.Size(460, 476);
            this.dgResults.TabIndex = 2;
            this.dgResults.TabStop = false;
            this.dgResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgResults_CellClick);
            // 
            // UserID
            // 
            this.UserID.DataPropertyName = "UserID";
            this.UserID.HeaderText = "";
            this.UserID.Name = "UserID";
            this.UserID.ReadOnly = true;
            this.UserID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UserID.Visible = false;
            // 
            // UserName
            // 
            this.UserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.UserName.DataPropertyName = "UserCode";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.UserName.DefaultCellStyle = dataGridViewCellStyle3;
            this.UserName.HeaderText = "User Name";
            this.UserName.MaxInputLength = 20;
            this.UserName.MinimumWidth = 125;
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            this.UserName.Width = 125;
            // 
            // UserFullName
            // 
            this.UserFullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.UserFullName.DataPropertyName = "UserFullName";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.UserFullName.DefaultCellStyle = dataGridViewCellStyle4;
            this.UserFullName.FillWeight = 1F;
            this.UserFullName.HeaderText = "Name";
            this.UserFullName.MaxInputLength = 75;
            this.UserFullName.MinimumWidth = 150;
            this.UserFullName.Name = "UserFullName";
            this.UserFullName.ReadOnly = true;
            this.UserFullName.Width = 190;
            // 
            // RecordStatus
            // 
            this.RecordStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RecordStatus.DataPropertyName = "RecordStatus";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            this.RecordStatus.DefaultCellStyle = dataGridViewCellStyle5;
            this.RecordStatus.HeaderText = "Status";
            this.RecordStatus.MaxInputLength = 15;
            this.RecordStatus.MinimumWidth = 75;
            this.RecordStatus.Name = "RecordStatus";
            this.RecordStatus.ReadOnly = true;
            // 
            // lblRecords
            // 
            this.lblRecords.AutoSize = true;
            this.lblRecords.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecords.Location = new System.Drawing.Point(5, 494);
            this.lblRecords.Name = "lblRecords";
            this.lblRecords.Size = new System.Drawing.Size(21, 15);
            this.lblRecords.TabIndex = 1;
            this.lblRecords.Text = "ss";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResults.ForeColor = System.Drawing.Color.Black;
            this.lblResults.Location = new System.Drawing.Point(55, 123);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(92, 15);
            this.lblResults.TabIndex = 8;
            this.lblResults.Text = "Search Results";
            // 
            // pnlUser
            // 
            this.pnlUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUser.Controls.Add(this.recbuttons);
            this.pnlUser.Controls.Add(this.ckHide);
            this.pnlUser.Controls.Add(this.btnPwdHelp);
            this.pnlUser.Controls.Add(this.txtFax);
            this.pnlUser.Controls.Add(this.txtCell);
            this.pnlUser.Controls.Add(this.txtPhone);
            this.pnlUser.Controls.Add(this.txtPassword_confirm);
            this.pnlUser.Controls.Add(this.lblPwd_confirm);
            this.pnlUser.Controls.Add(this.ckBilling);
            this.pnlUser.Controls.Add(this.lblOptional);
            this.pnlUser.Controls.Add(this.lblRequired);
            this.pnlUser.Controls.Add(this.cboStatus_record);
            this.pnlUser.Controls.Add(this.ckYard);
            this.pnlUser.Controls.Add(this.ckAdmin);
            this.pnlUser.Controls.Add(this.label6);
            this.pnlUser.Controls.Add(this.txtUpdatedBy);
            this.pnlUser.Controls.Add(this.label22);
            this.pnlUser.Controls.Add(this.txtCreatedBy);
            this.pnlUser.Controls.Add(this.label23);
            this.pnlUser.Controls.Add(this.txtUpdatedDate);
            this.pnlUser.Controls.Add(this.label19);
            this.pnlUser.Controls.Add(this.txtCreationDate);
            this.pnlUser.Controls.Add(this.label21);
            this.pnlUser.Controls.Add(this.txtPortPassID);
            this.pnlUser.Controls.Add(this.label20);
            this.pnlUser.Controls.Add(this.label15);
            this.pnlUser.Controls.Add(this.txtEmpl);
            this.pnlUser.Controls.Add(this.label18);
            this.pnlUser.Controls.Add(this.txtPassword);
            this.pnlUser.Controls.Add(this.label17);
            this.pnlUser.Controls.Add(this.txtEmail);
            this.pnlUser.Controls.Add(this.label16);
            this.pnlUser.Controls.Add(this.label13);
            this.pnlUser.Controls.Add(this.label14);
            this.pnlUser.Controls.Add(this.txtExtension);
            this.pnlUser.Controls.Add(this.label12);
            this.pnlUser.Controls.Add(this.label11);
            this.pnlUser.Controls.Add(this.lblStatus);
            this.pnlUser.Controls.Add(this.txtUserID);
            this.pnlUser.Controls.Add(this.txtLname_record);
            this.pnlUser.Controls.Add(this.label9);
            this.pnlUser.Controls.Add(this.txtFname_record);
            this.pnlUser.Controls.Add(this.label8);
            this.pnlUser.Controls.Add(this.txtUname_record);
            this.pnlUser.Controls.Add(this.label7);
            this.pnlUser.Location = new System.Drawing.Point(630, 10);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(545, 633);
            this.pnlUser.TabIndex = 0;
            // 
            // recbuttons
            // 
            this.recbuttons.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recbuttons.ForeColor = System.Drawing.Color.Blue;
            this.recbuttons.Location = new System.Drawing.Point(465, 45);
            this.recbuttons.Name = "recbuttons";
            this.recbuttons.Size = new System.Drawing.Size(65, 150);
            this.recbuttons.TabIndex = 71;
            // 
            // ckHide
            // 
            this.ckHide.AutoSize = true;
            this.ckHide.Location = new System.Drawing.Point(232, 255);
            this.ckHide.Name = "ckHide";
            this.ckHide.Size = new System.Drawing.Size(87, 19);
            this.ckHide.TabIndex = 69;
            this.ckHide.Text = "Hide Rates";
            this.ckHide.UseVisualStyleBackColor = true;
            this.ckHide.CheckedChanged += new System.EventHandler(this.ckHide_CheckedChanged);
            // 
            // btnPwdHelp
            // 
            this.btnPwdHelp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPwdHelp.Location = new System.Drawing.Point(288, 135);
            this.btnPwdHelp.Name = "btnPwdHelp";
            this.btnPwdHelp.Size = new System.Drawing.Size(25, 25);
            this.btnPwdHelp.TabIndex = 68;
            this.btnPwdHelp.TabStop = false;
            this.btnPwdHelp.Text = "?";
            this.btnPwdHelp.UseVisualStyleBackColor = true;
            this.btnPwdHelp.Click += new System.EventHandler(this.btnPwdHelp_Click);
            // 
            // txtFax
            // 
            this.txtFax.BackColor = System.Drawing.Color.White;
            this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFax.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFax.Location = new System.Drawing.Point(395, 375);
            this.txtFax.Mask = "(999) 000-0000";
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(135, 23);
            this.txtFax.TabIndex = 67;
            this.txtFax.TextChanged += new System.EventHandler(this.txtFax_TextChanged);
            // 
            // txtCell
            // 
            this.txtCell.BackColor = System.Drawing.Color.White;
            this.txtCell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCell.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCell.Location = new System.Drawing.Point(120, 375);
            this.txtCell.Mask = "(999) 000-0000";
            this.txtCell.Name = "txtCell";
            this.txtCell.Size = new System.Drawing.Size(150, 23);
            this.txtCell.TabIndex = 66;
            this.txtCell.TextChanged += new System.EventHandler(this.txtCell_TextChanged);
            // 
            // txtPhone
            // 
            this.txtPhone.BackColor = System.Drawing.Color.White;
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhone.Location = new System.Drawing.Point(120, 345);
            this.txtPhone.Mask = "(999) 000-0000";
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(150, 23);
            this.txtPhone.TabIndex = 65;
            this.txtPhone.TextChanged += new System.EventHandler(this.txtPhone_TextChanged);
            this.txtPhone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPhone_KeyDown);
            // 
            // txtPassword_confirm
            // 
            this.txtPassword_confirm.BackColor = System.Drawing.Color.White;
            this.txtPassword_confirm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword_confirm.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword_confirm.ForeColor = System.Drawing.Color.Black;
            this.txtPassword_confirm.Location = new System.Drawing.Point(120, 165);
            this.txtPassword_confirm.MaxLength = 20;
            this.txtPassword_confirm.MinimumSize = new System.Drawing.Size(150, 25);
            this.txtPassword_confirm.Name = "txtPassword_confirm";
            this.txtPassword_confirm.PasswordChar = '*';
            this.txtPassword_confirm.Size = new System.Drawing.Size(150, 23);
            this.txtPassword_confirm.TabIndex = 62;
            this.txtPassword_confirm.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_confirm_KeyDown);
            // 
            // lblPwd_confirm
            // 
            this.lblPwd_confirm.AutoSize = true;
            this.lblPwd_confirm.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPwd_confirm.Location = new System.Drawing.Point(11, 165);
            this.lblPwd_confirm.Name = "lblPwd_confirm";
            this.lblPwd_confirm.Size = new System.Drawing.Size(90, 15);
            this.lblPwd_confirm.TabIndex = 61;
            this.lblPwd_confirm.Text = "Pwd (confirm):";
            // 
            // ckBilling
            // 
            this.ckBilling.AutoSize = true;
            this.ckBilling.Location = new System.Drawing.Point(120, 255);
            this.ckBilling.Name = "ckBilling";
            this.ckBilling.Size = new System.Drawing.Size(60, 19);
            this.ckBilling.TabIndex = 60;
            this.ckBilling.Text = "Billing";
            this.ckBilling.UseVisualStyleBackColor = true;
            this.ckBilling.CheckedChanged += new System.EventHandler(this.ckBilling_CheckedChanged);
            // 
            // lblOptional
            // 
            this.lblOptional.AutoSize = true;
            this.lblOptional.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptional.ForeColor = System.Drawing.Color.Blue;
            this.lblOptional.Location = new System.Drawing.Point(117, 315);
            this.lblOptional.Name = "lblOptional";
            this.lblOptional.Size = new System.Drawing.Size(61, 16);
            this.lblOptional.TabIndex = 59;
            this.lblOptional.Text = "Optional";
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequired.ForeColor = System.Drawing.Color.Red;
            this.lblRequired.Location = new System.Drawing.Point(120, 15);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(66, 16);
            this.lblRequired.TabIndex = 58;
            this.lblRequired.Text = "Required";
            // 
            // cboStatus_record
            // 
            this.cboStatus_record.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStatus_record.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStatus_record.BackColor = System.Drawing.Color.White;
            this.cboStatus_record.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus_record.IntegralHeight = false;
            this.cboStatus_record.ItemHeight = 16;
            this.cboStatus_record.Location = new System.Drawing.Point(120, 195);
            this.cboStatus_record.MinimumSize = new System.Drawing.Size(150, 0);
            this.cboStatus_record.Name = "cboStatus_record";
            this.cboStatus_record.Size = new System.Drawing.Size(150, 24);
            this.cboStatus_record.TabIndex = 5;
            this.cboStatus_record.SelectedIndexChanged += new System.EventHandler(this.cboStatus_record_SelectedIndexChanged);
            this.cboStatus_record.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboStatus_record_KeyDown);
            // 
            // ckYard
            // 
            this.ckYard.AutoSize = true;
            this.ckYard.Location = new System.Drawing.Point(232, 225);
            this.ckYard.Name = "ckYard";
            this.ckYard.Size = new System.Drawing.Size(117, 19);
            this.ckYard.TabIndex = 7;
            this.ckYard.Text = "Yard Operations";
            this.ckYard.UseVisualStyleBackColor = true;
            this.ckYard.CheckedChanged += new System.EventHandler(this.ckYard_CheckedChanged);
            // 
            // ckAdmin
            // 
            this.ckAdmin.AutoSize = true;
            this.ckAdmin.Location = new System.Drawing.Point(120, 225);
            this.ckAdmin.Name = "ckAdmin";
            this.ckAdmin.Size = new System.Drawing.Size(104, 19);
            this.ckAdmin.TabIndex = 6;
            this.ckAdmin.Text = "Administrator";
            this.ckAdmin.UseVisualStyleBackColor = true;
            this.ckAdmin.CheckedChanged += new System.EventHandler(this.ckAdmin_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(10, 225);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 15);
            this.label6.TabIndex = 56;
            this.label6.Text = "Roles:";
            // 
            // txtUpdatedBy
            // 
            this.txtUpdatedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUpdatedBy.Enabled = false;
            this.txtUpdatedBy.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdatedBy.ForeColor = System.Drawing.Color.Black;
            this.txtUpdatedBy.Location = new System.Drawing.Point(395, 495);
            this.txtUpdatedBy.Name = "txtUpdatedBy";
            this.txtUpdatedBy.ReadOnly = true;
            this.txtUpdatedBy.Size = new System.Drawing.Size(135, 23);
            this.txtUpdatedBy.TabIndex = 55;
            this.txtUpdatedBy.TabStop = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(285, 495);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(74, 15);
            this.label22.TabIndex = 54;
            this.label22.Text = "Updated By:";
            // 
            // txtCreatedBy
            // 
            this.txtCreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreatedBy.Enabled = false;
            this.txtCreatedBy.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreatedBy.ForeColor = System.Drawing.Color.Black;
            this.txtCreatedBy.Location = new System.Drawing.Point(120, 495);
            this.txtCreatedBy.Name = "txtCreatedBy";
            this.txtCreatedBy.ReadOnly = true;
            this.txtCreatedBy.Size = new System.Drawing.Size(150, 23);
            this.txtCreatedBy.TabIndex = 53;
            this.txtCreatedBy.TabStop = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(10, 495);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(72, 15);
            this.label23.TabIndex = 52;
            this.label23.Text = "Created By:";
            // 
            // txtUpdatedDate
            // 
            this.txtUpdatedDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUpdatedDate.Enabled = false;
            this.txtUpdatedDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdatedDate.ForeColor = System.Drawing.Color.Black;
            this.txtUpdatedDate.Location = new System.Drawing.Point(395, 465);
            this.txtUpdatedDate.Name = "txtUpdatedDate";
            this.txtUpdatedDate.ReadOnly = true;
            this.txtUpdatedDate.Size = new System.Drawing.Size(135, 23);
            this.txtUpdatedDate.TabIndex = 51;
            this.txtUpdatedDate.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(285, 465);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(86, 15);
            this.label19.TabIndex = 50;
            this.label19.Text = "Updated Date:";
            // 
            // txtCreationDate
            // 
            this.txtCreationDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreationDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreationDate.Enabled = false;
            this.txtCreationDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreationDate.ForeColor = System.Drawing.Color.Black;
            this.txtCreationDate.Location = new System.Drawing.Point(120, 465);
            this.txtCreationDate.Name = "txtCreationDate";
            this.txtCreationDate.ReadOnly = true;
            this.txtCreationDate.Size = new System.Drawing.Size(150, 23);
            this.txtCreationDate.TabIndex = 49;
            this.txtCreationDate.TabStop = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(10, 465);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(74, 15);
            this.label21.TabIndex = 48;
            this.label21.Text = "Created On:";
            // 
            // txtPortPassID
            // 
            this.txtPortPassID.BackColor = System.Drawing.Color.White;
            this.txtPortPassID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPortPassID.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPortPassID.ForeColor = System.Drawing.Color.Black;
            this.txtPortPassID.Location = new System.Drawing.Point(395, 405);
            this.txtPortPassID.MaxLength = 12;
            this.txtPortPassID.Name = "txtPortPassID";
            this.txtPortPassID.ReadOnly = true;
            this.txtPortPassID.Size = new System.Drawing.Size(135, 23);
            this.txtPortPassID.TabIndex = 13;
            this.txtPortPassID.TextChanged += new System.EventHandler(this.txtPortPassID_TextChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(285, 405);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(84, 15);
            this.label20.TabIndex = 46;
            this.label20.Text = "Port Pass ID#";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(10, 195);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(91, 15);
            this.label15.TabIndex = 44;
            this.label15.Text = "Record Status:";
            // 
            // txtEmpl
            // 
            this.txtEmpl.BackColor = System.Drawing.Color.White;
            this.txtEmpl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmpl.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpl.ForeColor = System.Drawing.Color.Black;
            this.txtEmpl.Location = new System.Drawing.Point(120, 405);
            this.txtEmpl.MaxLength = 20;
            this.txtEmpl.Name = "txtEmpl";
            this.txtEmpl.Size = new System.Drawing.Size(150, 23);
            this.txtEmpl.TabIndex = 12;
            this.txtEmpl.TextChanged += new System.EventHandler(this.txtEmpl_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(10, 405);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(72, 15);
            this.label18.TabIndex = 42;
            this.label18.Text = "Employee #";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.Black;
            this.txtPassword.Location = new System.Drawing.Point(120, 135);
            this.txtPassword.MaxLength = 20;
            this.txtPassword.MinimumSize = new System.Drawing.Size(150, 25);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(150, 23);
            this.txtPassword.TabIndex = 4;
            this.tipPassword.SetToolTip(this.txtPassword, "1) Must be at least 4 chars.\r\n2) Cannot begin with DAI.\r\n3) Cannot be a seq. of #" +
        "\'s.\r\n4) Pwd is case sensitive.\r\n5) Cannot repeat same char 4 times.");
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(10, 135);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(68, 15);
            this.label17.TabIndex = 38;
            this.label17.Text = "Password:";
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.White;
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.ForeColor = System.Drawing.Color.Black;
            this.txtEmail.Location = new System.Drawing.Point(120, 435);
            this.txtEmail.MaxLength = 50;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(410, 23);
            this.txtEmail.TabIndex = 14;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(10, 435);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 15);
            this.label16.TabIndex = 36;
            this.label16.Text = "Email:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(285, 375);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 15);
            this.label13.TabIndex = 34;
            this.label13.Text = "Fax Number:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(10, 375);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 15);
            this.label14.TabIndex = 32;
            this.label14.Text = "Cell Phone:";
            // 
            // txtExtension
            // 
            this.txtExtension.BackColor = System.Drawing.Color.White;
            this.txtExtension.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExtension.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExtension.ForeColor = System.Drawing.Color.Black;
            this.txtExtension.Location = new System.Drawing.Point(395, 345);
            this.txtExtension.MaxLength = 5;
            this.txtExtension.Name = "txtExtension";
            this.txtExtension.Size = new System.Drawing.Size(135, 23);
            this.txtExtension.TabIndex = 9;
            this.txtExtension.TextChanged += new System.EventHandler(this.txtExtension_TextChanged);
            this.txtExtension.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExtension_KeyDown);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(285, 345);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 15);
            this.label12.TabIndex = 30;
            this.label12.Text = "Extension:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(10, 345);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 15);
            this.label11.TabIndex = 28;
            this.label11.Text = "Phone:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Blue;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(355, 5);
            this.lblStatus.MinimumSize = new System.Drawing.Size(165, 25);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(165, 25);
            this.lblStatus.TabIndex = 20;
            this.lblStatus.Text = "Read only";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtUserID
            // 
            this.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserID.ForeColor = System.Drawing.Color.White;
            this.txtUserID.Location = new System.Drawing.Point(280, 12);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(20, 21);
            this.txtUserID.TabIndex = 19;
            this.txtUserID.TabStop = false;
            this.txtUserID.Visible = false;
            // 
            // txtLname_record
            // 
            this.txtLname_record.BackColor = System.Drawing.Color.White;
            this.txtLname_record.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLname_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLname_record.ForeColor = System.Drawing.Color.Black;
            this.txtLname_record.Location = new System.Drawing.Point(120, 105);
            this.txtLname_record.MaxLength = 30;
            this.txtLname_record.MinimumSize = new System.Drawing.Size(150, 25);
            this.txtLname_record.Name = "txtLname_record";
            this.txtLname_record.Size = new System.Drawing.Size(150, 23);
            this.txtLname_record.TabIndex = 3;
            this.txtLname_record.TextChanged += new System.EventHandler(this.txtLname_record_TextChanged);
            this.txtLname_record.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLname_record_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(10, 105);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 15);
            this.label9.TabIndex = 10;
            this.label9.Text = "Last Name:";
            // 
            // txtFname_record
            // 
            this.txtFname_record.BackColor = System.Drawing.Color.White;
            this.txtFname_record.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFname_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFname_record.ForeColor = System.Drawing.Color.Black;
            this.txtFname_record.Location = new System.Drawing.Point(120, 75);
            this.txtFname_record.MaxLength = 30;
            this.txtFname_record.MinimumSize = new System.Drawing.Size(150, 25);
            this.txtFname_record.Name = "txtFname_record";
            this.txtFname_record.Size = new System.Drawing.Size(150, 23);
            this.txtFname_record.TabIndex = 2;
            this.txtFname_record.TextChanged += new System.EventHandler(this.txtFname_record_TextChanged);
            this.txtFname_record.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFname_record_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(10, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 15);
            this.label8.TabIndex = 8;
            this.label8.Text = "First Name:";
            // 
            // txtUname_record
            // 
            this.txtUname_record.BackColor = System.Drawing.Color.White;
            this.txtUname_record.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUname_record.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUname_record.ForeColor = System.Drawing.Color.Black;
            this.txtUname_record.Location = new System.Drawing.Point(120, 45);
            this.txtUname_record.MaxLength = 20;
            this.txtUname_record.MinimumSize = new System.Drawing.Size(150, 25);
            this.txtUname_record.Name = "txtUname_record";
            this.txtUname_record.Size = new System.Drawing.Size(150, 23);
            this.txtUname_record.TabIndex = 1;
            this.txtUname_record.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUname_record_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(10, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 15);
            this.label7.TabIndex = 6;
            this.label7.Text = "User Name:";
            // 
            // lblRecordDetails
            // 
            this.lblRecordDetails.AutoSize = true;
            this.lblRecordDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordDetails.ForeColor = System.Drawing.Color.Black;
            this.lblRecordDetails.Location = new System.Drawing.Point(640, 5);
            this.lblRecordDetails.Name = "lblRecordDetails";
            this.lblRecordDetails.Size = new System.Drawing.Size(89, 15);
            this.lblRecordDetails.TabIndex = 10;
            this.lblRecordDetails.Text = "Record Details";
            // 
            // tipBtnMoveFirst
            // 
            this.tipBtnMoveFirst.ToolTipTitle = "Move To First Record";
            // 
            // tipBtnMovePrev
            // 
            this.tipBtnMovePrev.ToolTipTitle = "Move To Previous Record";
            // 
            // tipBtnMoveNext
            // 
            this.tipBtnMoveNext.ToolTipTitle = "Move To Next Record";
            // 
            // tipBtnLast
            // 
            this.tipBtnLast.ToolTipTitle = "Move To Last Record";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 656);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // tipPassword
            // 
            this.tipPassword.ToolTipTitle = "Password Rules";
            // 
            // tipBtnMenu
            // 
            this.tipBtnMenu.ToolTipTitle = "Menu button";
            // 
            // frmUserAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1182, 653);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlResults);
            this.Controls.Add(this.lblRecordDetails);
            this.Controls.Add(this.pnlUser);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.KeyPreview = true;
            this.Name = "frmUserAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DAI Export - User Admin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUserAdmin_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmUserAdmin_MouseMove);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlResults.ResumeLayout(false);
            this.pnlResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUname;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel pnlResults;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.Panel pnlUser;
        private System.Windows.Forms.Label lblRecords;
        private System.Windows.Forms.TextBox txtLname_record;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFname_record;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUname_record;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblRecordDetails;
        private System.Windows.Forms.ToolTip tipBtnMoveFirst;
        private System.Windows.Forms.ToolTip tipBtnMovePrev;
        private System.Windows.Forms.ToolTip tipBtnMoveNext;
        private System.Windows.Forms.ToolTip tipBtnLast;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox ckYard;
        private System.Windows.Forms.CheckBox ckAdmin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUpdatedBy;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtCreatedBy;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtUpdatedDate;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtCreationDate;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtPortPassID;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtEmpl;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtExtension;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboStatus_record;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblOptional;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.CheckBox ckBilling;
        private System.Windows.Forms.TextBox txtPassword_confirm;
        private System.Windows.Forms.Label lblPwd_confirm;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MaskedTextBox txtFax;
        private System.Windows.Forms.MaskedTextBox txtCell;
        private System.Windows.Forms.MaskedTextBox txtPhone;
        private System.Windows.Forms.ToolTip tipPassword;
        private System.Windows.Forms.Button btnPwdHelp;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox ckHide;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.ToolTip tipBtnMenu;
        private System.Windows.Forms.ComboBox cboRole;
        private RecordButtons recbuttons;
        protected System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordStatus;
    }
}