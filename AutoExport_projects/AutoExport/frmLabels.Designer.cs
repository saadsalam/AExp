namespace AutoExport
{
    partial class frmLabels
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnMenu = new System.Windows.Forms.Button();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtVIN = new System.Windows.Forms.TextBox();
            this.rbVIN = new System.Windows.Forms.RadioButton();
            this.rbSelected = new System.Windows.Forms.RadioButton();
            this.rbPrintDate = new System.Windows.Forms.RadioButton();
            this.rbBatch = new System.Windows.Forms.RadioButton();
            this.dgBatch = new System.Windows.Forms.DataGridView();
            this.BatchID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImportDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceivedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecordStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgPrint = new System.Windows.Forms.DataGridView();
            this.FullDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbDouble = new System.Windows.Forms.RadioButton();
            this.rbSingle = new System.Windows.Forms.RadioButton();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.toolTipPrint = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipDisplay = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgBatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgPrint)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 482);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 73;
            this.pictureBox2.TabStop = false;
            // 
            // btnMenu
            // 
            this.btnMenu.Image = global::AutoExport.Properties.Resources.Menu1;
            this.btnMenu.Location = new System.Drawing.Point(5, 10);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 30);
            this.btnMenu.TabIndex = 75;
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(0, 30);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(168, 19);
            this.rbAll.TabIndex = 76;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "Print All Unprinted Labels";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtVIN);
            this.panel1.Controls.Add(this.rbVIN);
            this.panel1.Controls.Add(this.rbSelected);
            this.panel1.Controls.Add(this.rbPrintDate);
            this.panel1.Controls.Add(this.rbBatch);
            this.panel1.Controls.Add(this.rbAll);
            this.panel1.Location = new System.Drawing.Point(45, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(550, 84);
            this.panel1.TabIndex = 77;
            // 
            // txtVIN
            // 
            this.txtVIN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVIN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtVIN.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVIN.Location = new System.Drawing.Point(225, 30);
            this.txtVIN.MaxLength = 17;
            this.txtVIN.Name = "txtVIN";
            this.txtVIN.Size = new System.Drawing.Size(145, 21);
            this.txtVIN.TabIndex = 81;
            // 
            // rbVIN
            // 
            this.rbVIN.AutoSize = true;
            this.rbVIN.Location = new System.Drawing.Point(175, 30);
            this.rbVIN.Name = "rbVIN";
            this.rbVIN.Size = new System.Drawing.Size(47, 19);
            this.rbVIN.TabIndex = 80;
            this.rbVIN.TabStop = true;
            this.rbVIN.Text = "VIN:";
            this.rbVIN.UseVisualStyleBackColor = true;
            // 
            // rbSelected
            // 
            this.rbSelected.AutoSize = true;
            this.rbSelected.Location = new System.Drawing.Point(0, 0);
            this.rbSelected.Name = "rbSelected";
            this.rbSelected.Size = new System.Drawing.Size(244, 19);
            this.rbSelected.TabIndex = 79;
            this.rbSelected.TabStop = true;
            this.rbSelected.Text = "Selected rows from Veh. Locator form ";
            this.rbSelected.UseVisualStyleBackColor = true;
            // 
            // rbPrintDate
            // 
            this.rbPrintDate.AutoSize = true;
            this.rbPrintDate.Location = new System.Drawing.Point(375, 60);
            this.rbPrintDate.Name = "rbPrintDate";
            this.rbPrintDate.Size = new System.Drawing.Size(120, 19);
            this.rbPrintDate.TabIndex = 78;
            this.rbPrintDate.TabStop = true;
            this.rbPrintDate.Text = "Select Print Date";
            this.rbPrintDate.UseVisualStyleBackColor = true;
            // 
            // rbBatch
            // 
            this.rbBatch.AutoSize = true;
            this.rbBatch.Location = new System.Drawing.Point(0, 60);
            this.rbBatch.Name = "rbBatch";
            this.rbBatch.Size = new System.Drawing.Size(140, 19);
            this.rbBatch.TabIndex = 77;
            this.rbBatch.TabStop = true;
            this.rbBatch.Text = "Select Batch ID/Date";
            this.rbBatch.UseVisualStyleBackColor = true;
            // 
            // dgBatch
            // 
            this.dgBatch.AllowUserToAddRows = false;
            this.dgBatch.AllowUserToDeleteRows = false;
            this.dgBatch.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.dgBatch.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgBatch.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgBatch.BackgroundColor = System.Drawing.Color.White;
            this.dgBatch.CausesValidation = false;
            this.dgBatch.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgBatch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgBatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgBatch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BatchID,
            this.ImportDate,
            this.ReceivedBy,
            this.RecordStatus});
            this.dgBatch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.dgBatch.EnableHeadersVisualStyles = false;
            this.dgBatch.Location = new System.Drawing.Point(45, 90);
            this.dgBatch.MultiSelect = false;
            this.dgBatch.Name = "dgBatch";
            this.dgBatch.ReadOnly = true;
            this.dgBatch.RowHeadersVisible = false;
            this.dgBatch.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgBatch.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgBatch.RowTemplate.ReadOnly = true;
            this.dgBatch.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgBatch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgBatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgBatch.ShowCellErrors = false;
            this.dgBatch.Size = new System.Drawing.Size(355, 310);
            this.dgBatch.TabIndex = 78;
            this.dgBatch.TabStop = false;
            // 
            // BatchID
            // 
            this.BatchID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.BatchID.DataPropertyName = "BatchID";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.BatchID.DefaultCellStyle = dataGridViewCellStyle3;
            this.BatchID.HeaderText = "Batch ID";
            this.BatchID.MaxInputLength = 20;
            this.BatchID.MinimumWidth = 70;
            this.BatchID.Name = "BatchID";
            this.BatchID.ReadOnly = true;
            this.BatchID.Width = 80;
            // 
            // ImportDate
            // 
            this.ImportDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ImportDate.DataPropertyName = "ImportDate";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.ImportDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.ImportDate.FillWeight = 1F;
            this.ImportDate.HeaderText = "Date";
            this.ImportDate.MaxInputLength = 75;
            this.ImportDate.MinimumWidth = 75;
            this.ImportDate.Name = "ImportDate";
            this.ImportDate.ReadOnly = true;
            // 
            // ReceivedBy
            // 
            this.ReceivedBy.DataPropertyName = "ReceivedBy";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            this.ReceivedBy.DefaultCellStyle = dataGridViewCellStyle5;
            this.ReceivedBy.HeaderText = "Rcv\'d By";
            this.ReceivedBy.Name = "ReceivedBy";
            this.ReceivedBy.ReadOnly = true;
            // 
            // RecordStatus
            // 
            this.RecordStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RecordStatus.DataPropertyName = "Recs";
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            this.RecordStatus.DefaultCellStyle = dataGridViewCellStyle6;
            this.RecordStatus.HeaderText = "Records";
            this.RecordStatus.MaxInputLength = 15;
            this.RecordStatus.MinimumWidth = 75;
            this.RecordStatus.Name = "RecordStatus";
            this.RecordStatus.ReadOnly = true;
            this.RecordStatus.Width = 75;
            // 
            // dgPrint
            // 
            this.dgPrint.AllowUserToAddRows = false;
            this.dgPrint.AllowUserToDeleteRows = false;
            this.dgPrint.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.dgPrint.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgPrint.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgPrint.BackgroundColor = System.Drawing.Color.White;
            this.dgPrint.CausesValidation = false;
            this.dgPrint.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgPrint.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgPrint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPrint.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FullDate,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgPrint.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dgPrint.EnableHeadersVisualStyles = false;
            this.dgPrint.Location = new System.Drawing.Point(420, 90);
            this.dgPrint.MultiSelect = false;
            this.dgPrint.Name = "dgPrint";
            this.dgPrint.ReadOnly = true;
            this.dgPrint.RowHeadersVisible = false;
            this.dgPrint.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgPrint.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgPrint.RowTemplate.ReadOnly = true;
            this.dgPrint.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgPrint.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgPrint.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgPrint.Size = new System.Drawing.Size(175, 308);
            this.dgPrint.TabIndex = 80;
            this.dgPrint.TabStop = false;
            // 
            // FullDate
            // 
            this.FullDate.DataPropertyName = "FullDate";
            this.FullDate.HeaderText = "FullDate";
            this.FullDate.Name = "FullDate";
            this.FullDate.ReadOnly = true;
            this.FullDate.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "PrintDate";
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTextBoxColumn1.HeaderText = "Print Date";
            this.dataGridViewTextBoxColumn1.MaxInputLength = 20;
            this.dataGridViewTextBoxColumn1.MinimumWidth = 70;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Recs";
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn2.FillWeight = 1F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Records";
            this.dataGridViewTextBoxColumn2.MaxInputLength = 75;
            this.dataGridViewTextBoxColumn2.MinimumWidth = 75;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 75;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel2.Controls.Add(this.rbDouble);
            this.panel2.Controls.Add(this.rbSingle);
            this.panel2.Location = new System.Drawing.Point(45, 411);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(299, 45);
            this.panel2.TabIndex = 78;
            // 
            // rbDouble
            // 
            this.rbDouble.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbDouble.AutoSize = true;
            this.rbDouble.Checked = true;
            this.rbDouble.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.rbDouble.Location = new System.Drawing.Point(0, 25);
            this.rbDouble.Name = "rbDouble";
            this.rbDouble.Size = new System.Drawing.Size(286, 19);
            this.rbDouble.TabIndex = 1;
            this.rbDouble.TabStop = true;
            this.rbDouble.Text = "Print 2 sets of labels (sorted by Line, Bay Loc.)";
            this.rbDouble.UseVisualStyleBackColor = true;
            // 
            // rbSingle
            // 
            this.rbSingle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbSingle.AutoSize = true;
            this.rbSingle.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.rbSingle.Location = new System.Drawing.Point(0, 0);
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.Size = new System.Drawing.Size(249, 19);
            this.rbSingle.TabIndex = 0;
            this.rbSingle.Text = "Print 1 set of labels (sorted by Bay Loc.)";
            this.rbSingle.UseVisualStyleBackColor = true;
            // 
            // btnDisplay
            // 
            this.btnDisplay.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDisplay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisplay.Location = new System.Drawing.Point(350, 411);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(100, 25);
            this.btnDisplay.TabIndex = 79;
            this.btnDisplay.Text = "Display Labels";
            this.toolTipDisplay.SetToolTip(this.btnDisplay, "View labels on the screen. \r\nThen click the Printer icon to print.");
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrint.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(350, 442);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 25);
            this.btnPrint.TabIndex = 80;
            this.btnPrint.Text = "Print Labels";
            this.toolTipPrint.SetToolTip(this.btnPrint, "Send labels directly to the Label printer.");
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Visible = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(495, 411);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 25);
            this.btnClose.TabIndex = 81;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmLabels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(603, 496);
            this.Controls.Add(this.dgBatch);
            this.Controls.Add(this.dgPrint);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnDisplay);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.pictureBox2);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Name = "frmLabels";
            this.Text = "DAI Export - Print Labels";
            this.Activated += new System.EventHandler(this.frmLabels_Activated);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmLabels_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgBatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgPrint)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbPrintDate;
        private System.Windows.Forms.RadioButton rbBatch;
        protected System.Windows.Forms.DataGridView dgBatch;
        private System.Windows.Forms.RadioButton rbSelected;
        protected System.Windows.Forms.DataGridView dgPrint;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbDouble;
        private System.Windows.Forms.RadioButton rbSingle;
        private System.Windows.Forms.Button btnDisplay;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip toolTipDisplay;
        private System.Windows.Forms.ToolTip toolTipPrint;
        private System.Windows.Forms.TextBox txtVIN;
        private System.Windows.Forms.RadioButton rbVIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImportDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceivedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}