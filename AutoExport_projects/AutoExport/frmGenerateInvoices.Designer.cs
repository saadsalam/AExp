namespace AutoExport
{
    partial class frmGenerateInvoices
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
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.rowID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BillToCustomerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BillingAddressID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VoyageID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateShipped = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BillToCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voyinfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecordStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnPrintInvoices = new System.Windows.Forms.Button();
            this.btnClearProcessed = new System.Windows.Forms.Button();
            this.btnGenInvoices = new System.Windows.Forms.Button();
            this.lblInvRecords = new System.Windows.Forms.Label();
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
            this.rowID,
            this.CustomerID,
            this.BillToCustomerID,
            this.BillingAddressID,
            this.CustomerCode,
            this.VoyageID,
            this.DateShipped,
            this.Customer,
            this.BillToCustomer,
            this.Units,
            this.voyinfo,
            this.RecordStatus});
            this.dgResults.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dgResults.EnableHeadersVisualStyles = false;
            this.dgResults.Location = new System.Drawing.Point(45, 5);
            this.dgResults.Margin = new System.Windows.Forms.Padding(4);
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgResults.RowHeadersWidth = 15;
            this.dgResults.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgResults.Size = new System.Drawing.Size(643, 421);
            this.dgResults.TabIndex = 5;
            this.dgResults.TabStop = false;
            // 
            // rowID
            // 
            this.rowID.DataPropertyName = "rowID";
            this.rowID.HeaderText = "rowID";
            this.rowID.Name = "rowID";
            this.rowID.ReadOnly = true;
            this.rowID.Visible = false;
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
            // BillToCustomerID
            // 
            this.BillToCustomerID.DataPropertyName = "BillToCustomerID";
            this.BillToCustomerID.HeaderText = "BillToCustomerID";
            this.BillToCustomerID.Name = "BillToCustomerID";
            this.BillToCustomerID.ReadOnly = true;
            this.BillToCustomerID.Visible = false;
            // 
            // BillingAddressID
            // 
            this.BillingAddressID.DataPropertyName = "BillingAddressID";
            this.BillingAddressID.HeaderText = "BillingAddressID";
            this.BillingAddressID.MinimumWidth = 2;
            this.BillingAddressID.Name = "BillingAddressID";
            this.BillingAddressID.ReadOnly = true;
            this.BillingAddressID.Visible = false;
            this.BillingAddressID.Width = 2;
            // 
            // CustomerCode
            // 
            this.CustomerCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CustomerCode.DataPropertyName = "CustomerCode";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.CustomerCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.CustomerCode.FillWeight = 1F;
            this.CustomerCode.HeaderText = "CustomerCode";
            this.CustomerCode.MaxInputLength = 5;
            this.CustomerCode.MinimumWidth = 2;
            this.CustomerCode.Name = "CustomerCode";
            this.CustomerCode.ReadOnly = true;
            this.CustomerCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CustomerCode.Visible = false;
            this.CustomerCode.Width = 2;
            // 
            // VoyageID
            // 
            this.VoyageID.DataPropertyName = "VoyageID";
            this.VoyageID.HeaderText = "VoyageID";
            this.VoyageID.MinimumWidth = 2;
            this.VoyageID.Name = "VoyageID";
            this.VoyageID.ReadOnly = true;
            this.VoyageID.Visible = false;
            this.VoyageID.Width = 2;
            // 
            // DateShipped
            // 
            this.DateShipped.DataPropertyName = "DateShipped";
            this.DateShipped.HeaderText = "DateShipped";
            this.DateShipped.MinimumWidth = 2;
            this.DateShipped.Name = "DateShipped";
            this.DateShipped.ReadOnly = true;
            this.DateShipped.Visible = false;
            this.DateShipped.Width = 2;
            // 
            // Customer
            // 
            this.Customer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Customer.DataPropertyName = "customer";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Customer.DefaultCellStyle = dataGridViewCellStyle4;
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            this.Customer.ReadOnly = true;
            // 
            // BillToCustomer
            // 
            this.BillToCustomer.DataPropertyName = "billtocustomer";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BillToCustomer.DefaultCellStyle = dataGridViewCellStyle5;
            this.BillToCustomer.HeaderText = "Bill to";
            this.BillToCustomer.Name = "BillToCustomer";
            this.BillToCustomer.ReadOnly = true;
            // 
            // Units
            // 
            this.Units.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Units.DataPropertyName = "Units";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            this.Units.DefaultCellStyle = dataGridViewCellStyle6;
            this.Units.HeaderText = "Units";
            this.Units.MaxInputLength = 8;
            this.Units.MinimumWidth = 65;
            this.Units.Name = "Units";
            this.Units.ReadOnly = true;
            this.Units.Width = 75;
            // 
            // voyinfo
            // 
            this.voyinfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.voyinfo.DataPropertyName = "voyinfo";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.voyinfo.DefaultCellStyle = dataGridViewCellStyle7;
            this.voyinfo.HeaderText = "Voyage";
            this.voyinfo.MaxInputLength = 2;
            this.voyinfo.MinimumWidth = 25;
            this.voyinfo.Name = "voyinfo";
            this.voyinfo.ReadOnly = true;
            this.voyinfo.Width = 250;
            // 
            // RecordStatus
            // 
            this.RecordStatus.DataPropertyName = "RecordStatus";
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordStatus.DefaultCellStyle = dataGridViewCellStyle8;
            this.RecordStatus.HeaderText = "Record Status";
            this.RecordStatus.Name = "RecordStatus";
            this.RecordStatus.ReadOnly = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox2.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 485);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 74;
            this.pictureBox2.TabStop = false;
            // 
            // btnMenu
            // 
            this.btnMenu.Image = global::AutoExport.Properties.Resources.Menu1;
            this.btnMenu.Location = new System.Drawing.Point(5, 10);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 28);
            this.btnMenu.TabIndex = 95;
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnPrintInvoices
            // 
            this.btnPrintInvoices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrintInvoices.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintInvoices.Location = new System.Drawing.Point(435, 457);
            this.btnPrintInvoices.Name = "btnPrintInvoices";
            this.btnPrintInvoices.Size = new System.Drawing.Size(150, 25);
            this.btnPrintInvoices.TabIndex = 98;
            this.btnPrintInvoices.Text = "Print Invoices";
            this.btnPrintInvoices.UseVisualStyleBackColor = true;
            this.btnPrintInvoices.Click += new System.EventHandler(this.btnPrintInvoices_Click);
            // 
            // btnClearProcessed
            // 
            this.btnClearProcessed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearProcessed.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearProcessed.Location = new System.Drawing.Point(240, 457);
            this.btnClearProcessed.Name = "btnClearProcessed";
            this.btnClearProcessed.Size = new System.Drawing.Size(150, 25);
            this.btnClearProcessed.TabIndex = 99;
            this.btnClearProcessed.Text = "Clear Processed";
            this.btnClearProcessed.UseVisualStyleBackColor = true;
            // 
            // btnGenInvoices
            // 
            this.btnGenInvoices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGenInvoices.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenInvoices.Location = new System.Drawing.Point(45, 457);
            this.btnGenInvoices.Name = "btnGenInvoices";
            this.btnGenInvoices.Size = new System.Drawing.Size(150, 25);
            this.btnGenInvoices.TabIndex = 100;
            this.btnGenInvoices.Text = "Generate Invoices";
            this.btnGenInvoices.UseVisualStyleBackColor = true;
            this.btnGenInvoices.Click += new System.EventHandler(this.btnGenInvoices_Click);
            // 
            // lblInvRecords
            // 
            this.lblInvRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInvRecords.AutoSize = true;
            this.lblInvRecords.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvRecords.Location = new System.Drawing.Point(45, 430);
            this.lblInvRecords.Name = "lblInvRecords";
            this.lblInvRecords.Size = new System.Drawing.Size(67, 15);
            this.lblInvRecords.TabIndex = 101;
            this.lblInvRecords.Text = "Records: 0";
            // 
            // frmGenerateInvoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(698, 487);
            this.Controls.Add(this.lblInvRecords);
            this.Controls.Add(this.btnGenInvoices);
            this.Controls.Add(this.btnClearProcessed);
            this.Controls.Add(this.btnPrintInvoices);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.dgResults);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Name = "frmGenerateInvoices";
            this.Text = "DAI Export - Generate Invoices";
            this.Activated += new System.EventHandler(this.frmGenerateInvoices_Activated);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmGenerateInvoices_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnPrintInvoices;
        private System.Windows.Forms.Button btnClearProcessed;
        private System.Windows.Forms.Button btnGenInvoices;
        private System.Windows.Forms.Label lblInvRecords;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn BillToCustomerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn BillingAddressID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn VoyageID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateShipped;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn BillToCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Units;
        private System.Windows.Forms.DataGridViewTextBoxColumn voyinfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordStatus;
    }
}