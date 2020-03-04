namespace AutoExport
{
    partial class frmInvRptParams
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboCust = new System.Windows.Forms.ComboBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.dgResults = new System.Windows.Forms.DataGridView();
            this.BatchID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlBatchDate = new System.Windows.Forms.Panel();
            this.rbDate = new System.Windows.Forms.RadioButton();
            this.rbBatch = new System.Windows.Forms.RadioButton();
            this.lbDest = new System.Windows.Forms.ListBox();
            this.pnlDest = new System.Windows.Forms.Panel();
            this.rbAllDest = new System.Windows.Forms.RadioButton();
            this.rbDest = new System.Windows.Forms.RadioButton();
            this.ckActive = new System.Windows.Forms.CheckBox();
            this.lblVoyage = new System.Windows.Forms.Label();
            this.cboVoyageDate = new System.Windows.Forms.ComboBox();
            this.pnlPhyDate = new System.Windows.Forms.Panel();
            this.pnlShipped = new System.Windows.Forms.Panel();
            this.rbShipped = new System.Windows.Forms.RadioButton();
            this.rbNotShipped = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
            this.pnlBatchDate.SuspendLayout();
            this.pnlDest.SuspendLayout();
            this.pnlPhyDate.SuspendLayout();
            this.pnlShipped.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 505);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 93;
            this.pictureBox1.TabStop = false;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(45, 5);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(363, 15);
            this.lblMsg.TabIndex = 104;
            this.lblMsg.Text = "Select the parameters for the xxx Inventory Comparison report";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(45, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 106;
            this.label2.Text = "Customer:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 15);
            this.label3.TabIndex = 107;
            this.label3.Text = "Phy. Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(76, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 15);
            this.label4.TabIndex = 109;
            this.label4.Text = "From:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(206, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 15);
            this.label5.TabIndex = 110;
            this.label5.Text = "To:";
            // 
            // cboCust
            // 
            this.cboCust.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCust.FormattingEnabled = true;
            this.cboCust.Location = new System.Drawing.Point(130, 35);
            this.cboCust.Name = "cboCust";
            this.cboCust.Size = new System.Drawing.Size(294, 23);
            this.cboCust.TabIndex = 111;
            this.cboCust.SelectedIndexChanged += new System.EventHandler(this.cboCust_SelectedIndexChanged);
            // 
            // txtFrom
            // 
            this.txtFrom.BackColor = System.Drawing.Color.White;
            this.txtFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFrom.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFrom.Location = new System.Drawing.Point(116, 0);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(75, 21);
            this.txtFrom.TabIndex = 113;
            this.txtFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrom_KeyPress);
            this.txtFrom.Validating += new System.ComponentModel.CancelEventHandler(this.txtFrom_Validating);
            // 
            // txtTo
            // 
            this.txtTo.BackColor = System.Drawing.Color.White;
            this.txtTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTo.Location = new System.Drawing.Point(231, 0);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(75, 21);
            this.txtTo.TabIndex = 114;
            this.txtTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTo_KeyPress);
            this.txtTo.Validating += new System.ComponentModel.CancelEventHandler(this.txtTo_Validating);
            // 
            // dgResults
            // 
            this.dgResults.AllowUserToAddRows = false;
            this.dgResults.AllowUserToDeleteRows = false;
            this.dgResults.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.dgResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
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
            this.BatchID,
            this.CreationDate,
            this.count});
            this.dgResults.Cursor = System.Windows.Forms.Cursors.Default;
            this.dgResults.EnableHeadersVisualStyles = false;
            this.dgResults.Location = new System.Drawing.Point(45, 122);
            this.dgResults.Name = "dgResults";
            this.dgResults.ReadOnly = true;
            this.dgResults.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgResults.RowHeadersWidth = 15;
            this.dgResults.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgResults.Size = new System.Drawing.Size(293, 143);
            this.dgResults.TabIndex = 115;
            this.dgResults.TabStop = false;
            // 
            // BatchID
            // 
            this.BatchID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.BatchID.DataPropertyName = "BatchID";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.BatchID.DefaultCellStyle = dataGridViewCellStyle3;
            this.BatchID.HeaderText = "Batch";
            this.BatchID.MaxInputLength = 6;
            this.BatchID.MinimumWidth = 30;
            this.BatchID.Name = "BatchID";
            this.BatchID.ReadOnly = true;
            this.BatchID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.BatchID.Width = 75;
            // 
            // CreationDate
            // 
            this.CreationDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CreationDate.DataPropertyName = "CreationDate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreationDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.CreationDate.HeaderText = "Date";
            this.CreationDate.MaxInputLength = 50;
            this.CreationDate.MinimumWidth = 125;
            this.CreationDate.Name = "CreationDate";
            this.CreationDate.ReadOnly = true;
            this.CreationDate.Width = 125;
            // 
            // count
            // 
            this.count.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.count.DataPropertyName = "totrecs";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.count.DefaultCellStyle = dataGridViewCellStyle5;
            this.count.HeaderText = "Count";
            this.count.MaxInputLength = 200;
            this.count.Name = "count";
            this.count.ReadOnly = true;
            this.count.Width = 75;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Pink;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(609, 465);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 118;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.PaleGreen;
            this.btnSave.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(50, 465);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 27);
            this.btnSave.TabIndex = 117;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlBatchDate
            // 
            this.pnlBatchDate.Controls.Add(this.rbDate);
            this.pnlBatchDate.Controls.Add(this.rbBatch);
            this.pnlBatchDate.Location = new System.Drawing.Point(45, 95);
            this.pnlBatchDate.Name = "pnlBatchDate";
            this.pnlBatchDate.Size = new System.Drawing.Size(423, 20);
            this.pnlBatchDate.TabIndex = 119;
            // 
            // rbDate
            // 
            this.rbDate.AutoSize = true;
            this.rbDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDate.ForeColor = System.Drawing.Color.Black;
            this.rbDate.Location = new System.Drawing.Point(350, 0);
            this.rbDate.Name = "rbDate";
            this.rbDate.Size = new System.Drawing.Size(67, 19);
            this.rbDate.TabIndex = 1;
            this.rbDate.TabStop = true;
            this.rbDate.Text = "By Date";
            this.rbDate.UseVisualStyleBackColor = true;
            // 
            // rbBatch
            // 
            this.rbBatch.AutoSize = true;
            this.rbBatch.Checked = true;
            this.rbBatch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBatch.ForeColor = System.Drawing.Color.Black;
            this.rbBatch.Location = new System.Drawing.Point(5, 0);
            this.rbBatch.Name = "rbBatch";
            this.rbBatch.Size = new System.Drawing.Size(72, 19);
            this.rbBatch.TabIndex = 0;
            this.rbBatch.TabStop = true;
            this.rbBatch.Text = "By Batch";
            this.rbBatch.UseVisualStyleBackColor = true;
            // 
            // lbDest
            // 
            this.lbDest.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDest.FormattingEnabled = true;
            this.lbDest.ItemHeight = 15;
            this.lbDest.Location = new System.Drawing.Point(5, 30);
            this.lbDest.Name = "lbDest";
            this.lbDest.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbDest.Size = new System.Drawing.Size(200, 139);
            this.lbDest.TabIndex = 121;
            // 
            // pnlDest
            // 
            this.pnlDest.Controls.Add(this.rbAllDest);
            this.pnlDest.Controls.Add(this.rbDest);
            this.pnlDest.Controls.Add(this.lbDest);
            this.pnlDest.Location = new System.Drawing.Point(45, 275);
            this.pnlDest.Name = "pnlDest";
            this.pnlDest.Size = new System.Drawing.Size(460, 180);
            this.pnlDest.TabIndex = 120;
            // 
            // rbAllDest
            // 
            this.rbAllDest.AutoSize = true;
            this.rbAllDest.Checked = true;
            this.rbAllDest.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAllDest.ForeColor = System.Drawing.Color.Black;
            this.rbAllDest.Location = new System.Drawing.Point(349, 0);
            this.rbAllDest.Name = "rbAllDest";
            this.rbAllDest.Size = new System.Drawing.Size(111, 19);
            this.rbAllDest.TabIndex = 1;
            this.rbAllDest.TabStop = true;
            this.rbAllDest.Text = "All Destinations";
            this.rbAllDest.UseVisualStyleBackColor = true;
            // 
            // rbDest
            // 
            this.rbDest.AutoSize = true;
            this.rbDest.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDest.ForeColor = System.Drawing.Color.Black;
            this.rbDest.Location = new System.Drawing.Point(5, 0);
            this.rbDest.Name = "rbDest";
            this.rbDest.Size = new System.Drawing.Size(132, 19);
            this.rbDest.TabIndex = 0;
            this.rbDest.TabStop = true;
            this.rbDest.Text = "Select Destinations";
            this.rbDest.UseVisualStyleBackColor = true;
            // 
            // ckActive
            // 
            this.ckActive.AutoSize = true;
            this.ckActive.Checked = true;
            this.ckActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckActive.ForeColor = System.Drawing.Color.Black;
            this.ckActive.Location = new System.Drawing.Point(473, 35);
            this.ckActive.Name = "ckActive";
            this.ckActive.Size = new System.Drawing.Size(242, 19);
            this.ckActive.TabIndex = 122;
            this.ckActive.Text = "Only ACTIVE Customers && Destinations";
            this.ckActive.UseVisualStyleBackColor = true;
            this.ckActive.CheckedChanged += new System.EventHandler(this.ckActive_CheckedChanged);
            // 
            // lblVoyage
            // 
            this.lblVoyage.AutoSize = true;
            this.lblVoyage.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVoyage.ForeColor = System.Drawing.Color.Black;
            this.lblVoyage.Location = new System.Drawing.Point(45, 65);
            this.lblVoyage.Name = "lblVoyage";
            this.lblVoyage.Size = new System.Drawing.Size(78, 15);
            this.lblVoyage.TabIndex = 123;
            this.lblVoyage.Text = "Voyage Date:";
            // 
            // cboVoyageDate
            // 
            this.cboVoyageDate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVoyageDate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboVoyageDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVoyageDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboVoyageDate.FormattingEnabled = true;
            this.cboVoyageDate.Location = new System.Drawing.Point(130, 65);
            this.cboVoyageDate.MaxDropDownItems = 40;
            this.cboVoyageDate.Name = "cboVoyageDate";
            this.cboVoyageDate.Size = new System.Drawing.Size(294, 24);
            this.cboVoyageDate.TabIndex = 124;
            // 
            // pnlPhyDate
            // 
            this.pnlPhyDate.Controls.Add(this.txtTo);
            this.pnlPhyDate.Controls.Add(this.label3);
            this.pnlPhyDate.Controls.Add(this.label4);
            this.pnlPhyDate.Controls.Add(this.label5);
            this.pnlPhyDate.Controls.Add(this.txtFrom);
            this.pnlPhyDate.Location = new System.Drawing.Point(392, 122);
            this.pnlPhyDate.Name = "pnlPhyDate";
            this.pnlPhyDate.Size = new System.Drawing.Size(309, 30);
            this.pnlPhyDate.TabIndex = 125;
            // 
            // pnlShipped
            // 
            this.pnlShipped.Controls.Add(this.rbShipped);
            this.pnlShipped.Controls.Add(this.rbNotShipped);
            this.pnlShipped.Location = new System.Drawing.Point(473, 65);
            this.pnlShipped.Name = "pnlShipped";
            this.pnlShipped.Size = new System.Drawing.Size(180, 25);
            this.pnlShipped.TabIndex = 126;
            // 
            // rbShipped
            // 
            this.rbShipped.AutoSize = true;
            this.rbShipped.Location = new System.Drawing.Point(110, 0);
            this.rbShipped.Name = "rbShipped";
            this.rbShipped.Size = new System.Drawing.Size(71, 19);
            this.rbShipped.TabIndex = 1;
            this.rbShipped.TabStop = true;
            this.rbShipped.Text = "Shipped";
            this.rbShipped.UseVisualStyleBackColor = true;
            // 
            // rbNotShipped
            // 
            this.rbNotShipped.AutoSize = true;
            this.rbNotShipped.Checked = true;
            this.rbNotShipped.Location = new System.Drawing.Point(0, 0);
            this.rbNotShipped.Name = "rbNotShipped";
            this.rbNotShipped.Size = new System.Drawing.Size(93, 19);
            this.rbNotShipped.TabIndex = 0;
            this.rbNotShipped.TabStop = true;
            this.rbNotShipped.Text = "Not Shipped";
            this.rbNotShipped.UseVisualStyleBackColor = true;
            // 
            // frmInvRptParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(716, 502);
            this.Controls.Add(this.pnlShipped);
            this.Controls.Add(this.pnlPhyDate);
            this.Controls.Add(this.cboVoyageDate);
            this.Controls.Add(this.lblVoyage);
            this.Controls.Add(this.ckActive);
            this.Controls.Add(this.pnlDest);
            this.Controls.Add(this.pnlBatchDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgResults);
            this.Controls.Add(this.cboCust);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Name = "frmInvRptParams";
            this.Text = "DAI Export: Inv. Comparison params";
            this.Activated += new System.EventHandler(this.frmInvRptParams_Activated);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmInvRptParams_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
            this.pnlBatchDate.ResumeLayout(false);
            this.pnlBatchDate.PerformLayout();
            this.pnlDest.ResumeLayout(false);
            this.pnlDest.PerformLayout();
            this.pnlPhyDate.ResumeLayout(false);
            this.pnlPhyDate.PerformLayout();
            this.pnlShipped.ResumeLayout(false);
            this.pnlShipped.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboCust;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.TextBox txtTo;
        protected System.Windows.Forms.DataGridView dgResults;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlBatchDate;
        private System.Windows.Forms.RadioButton rbDate;
        private System.Windows.Forms.RadioButton rbBatch;
        private System.Windows.Forms.Panel pnlDest;
        private System.Windows.Forms.RadioButton rbAllDest;
        private System.Windows.Forms.RadioButton rbDest;
        private System.Windows.Forms.ListBox lbDest;
        private System.Windows.Forms.CheckBox ckActive;
        private System.Windows.Forms.Label lblVoyage;
        private System.Windows.Forms.ComboBox cboVoyageDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn count;
        private System.Windows.Forms.Panel pnlPhyDate;
        private System.Windows.Forms.Panel pnlShipped;
        private System.Windows.Forms.RadioButton rbShipped;
        private System.Windows.Forms.RadioButton rbNotShipped;
    }
}