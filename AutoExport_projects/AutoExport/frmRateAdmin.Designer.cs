namespace AutoExport
{
    partial class frmRateAdmin
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEntryFee = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPerDiem = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGraceDays = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboRateType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtStartDate = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtEndDate = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCreationDate = new System.Windows.Forms.TextBox();
            this.txtCreatedBy = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtUpdatedBy = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtUpdatedDate = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tipDate = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 410);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(45, 40);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "Customer:";
            // 
            // txtCustomer
            // 
            this.txtCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomer.Enabled = false;
            this.txtCustomer.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomer.ForeColor = System.Drawing.Color.Black;
            this.txtCustomer.Location = new System.Drawing.Point(155, 40);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.ReadOnly = true;
            this.txtCustomer.Size = new System.Drawing.Size(150, 23);
            this.txtCustomer.TabIndex = 65;
            this.txtCustomer.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(45, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 66;
            this.label1.Text = "Entry Fee:";
            // 
            // txtEntryFee
            // 
            this.txtEntryFee.AcceptsTab = true;
            this.txtEntryFee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEntryFee.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEntryFee.ForeColor = System.Drawing.Color.Black;
            this.txtEntryFee.Location = new System.Drawing.Point(155, 70);
            this.txtEntryFee.Name = "txtEntryFee";
            this.txtEntryFee.Size = new System.Drawing.Size(150, 23);
            this.txtEntryFee.TabIndex = 67;
            this.txtEntryFee.TextChanged += new System.EventHandler(this.txtEntryFee_TextChanged);
            this.txtEntryFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEntryFee_KeyPress);
            this.txtEntryFee.Leave += new System.EventHandler(this.txtEntryFee_Leave);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Blue;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(155, 10);
            this.lblStatus.MinimumSize = new System.Drawing.Size(150, 19);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(150, 19);
            this.lblStatus.TabIndex = 68;
            this.lblStatus.Text = "Modify Current Rate";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(45, 100);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 15);
            this.label2.TabIndex = 69;
            this.label2.Text = "Per Diem:";
            // 
            // txtPerDiem
            // 
            this.txtPerDiem.AcceptsTab = true;
            this.txtPerDiem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPerDiem.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPerDiem.ForeColor = System.Drawing.Color.Black;
            this.txtPerDiem.Location = new System.Drawing.Point(155, 100);
            this.txtPerDiem.Name = "txtPerDiem";
            this.txtPerDiem.Size = new System.Drawing.Size(150, 23);
            this.txtPerDiem.TabIndex = 70;
            this.txtPerDiem.TextChanged += new System.EventHandler(this.txtPerDiem_TextChanged);
            this.txtPerDiem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPerDiem_KeyPress);
            this.txtPerDiem.Leave += new System.EventHandler(this.txtPerDiem_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(45, 130);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 15);
            this.label4.TabIndex = 71;
            this.label4.Text = "Grace Days:";
            // 
            // txtGraceDays
            // 
            this.txtGraceDays.AcceptsTab = true;
            this.txtGraceDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGraceDays.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGraceDays.ForeColor = System.Drawing.Color.Black;
            this.txtGraceDays.Location = new System.Drawing.Point(155, 130);
            this.txtGraceDays.Name = "txtGraceDays";
            this.txtGraceDays.Size = new System.Drawing.Size(150, 23);
            this.txtGraceDays.TabIndex = 72;
            this.txtGraceDays.TextChanged += new System.EventHandler(this.txtGraceDays_TextChanged);
            this.txtGraceDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGraceDays_KeyPress);
            this.txtGraceDays.Leave += new System.EventHandler(this.txtGraceDays_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(45, 160);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 15);
            this.label5.TabIndex = 73;
            this.label5.Text = "Rate Type:";
            // 
            // cboRateType
            // 
            this.cboRateType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboRateType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboRateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRateType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRateType.FormattingEnabled = true;
            this.cboRateType.Location = new System.Drawing.Point(155, 160);
            this.cboRateType.Name = "cboRateType";
            this.cboRateType.Size = new System.Drawing.Size(150, 23);
            this.cboRateType.TabIndex = 74;
            this.cboRateType.SelectedIndexChanged += new System.EventHandler(this.cboRateType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(45, 190);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 75;
            this.label6.Text = "Start Date:";
            // 
            // txtStartDate
            // 
            this.txtStartDate.AcceptsTab = true;
            this.txtStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStartDate.ForeColor = System.Drawing.Color.Black;
            this.txtStartDate.Location = new System.Drawing.Point(155, 190);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(150, 23);
            this.txtStartDate.TabIndex = 76;
            this.txtStartDate.TextChanged += new System.EventHandler(this.txtStartDate_TextChanged);
            this.txtStartDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStartDate_KeyPress);
            this.txtStartDate.Validating += new System.ComponentModel.CancelEventHandler(this.txtStartDate_Validating);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(45, 220);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 15);
            this.label7.TabIndex = 77;
            this.label7.Text = "End Date:";
            // 
            // txtEndDate
            // 
            this.txtEndDate.AcceptsTab = true;
            this.txtEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEndDate.ForeColor = System.Drawing.Color.Black;
            this.txtEndDate.Location = new System.Drawing.Point(155, 220);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Size = new System.Drawing.Size(150, 23);
            this.txtEndDate.TabIndex = 78;
            this.txtEndDate.TextChanged += new System.EventHandler(this.txtEndDate_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(45, 250);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 15);
            this.label8.TabIndex = 79;
            this.label8.Text = "Creation Date:";
            // 
            // txtCreationDate
            // 
            this.txtCreationDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreationDate.Enabled = false;
            this.txtCreationDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreationDate.ForeColor = System.Drawing.Color.Black;
            this.txtCreationDate.Location = new System.Drawing.Point(155, 250);
            this.txtCreationDate.Name = "txtCreationDate";
            this.txtCreationDate.ReadOnly = true;
            this.txtCreationDate.Size = new System.Drawing.Size(150, 23);
            this.txtCreationDate.TabIndex = 80;
            this.txtCreationDate.TabStop = false;
            // 
            // txtCreatedBy
            // 
            this.txtCreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreatedBy.Enabled = false;
            this.txtCreatedBy.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreatedBy.ForeColor = System.Drawing.Color.Black;
            this.txtCreatedBy.Location = new System.Drawing.Point(155, 280);
            this.txtCreatedBy.Name = "txtCreatedBy";
            this.txtCreatedBy.ReadOnly = true;
            this.txtCreatedBy.Size = new System.Drawing.Size(150, 23);
            this.txtCreatedBy.TabIndex = 82;
            this.txtCreatedBy.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(45, 280);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 15);
            this.label9.TabIndex = 81;
            this.label9.Text = "Created By:";
            // 
            // txtUpdatedBy
            // 
            this.txtUpdatedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUpdatedBy.Enabled = false;
            this.txtUpdatedBy.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdatedBy.ForeColor = System.Drawing.Color.Black;
            this.txtUpdatedBy.Location = new System.Drawing.Point(155, 340);
            this.txtUpdatedBy.Name = "txtUpdatedBy";
            this.txtUpdatedBy.ReadOnly = true;
            this.txtUpdatedBy.Size = new System.Drawing.Size(150, 23);
            this.txtUpdatedBy.TabIndex = 86;
            this.txtUpdatedBy.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(45, 340);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 15);
            this.label10.TabIndex = 85;
            this.label10.Text = "Updated By:";
            // 
            // txtUpdatedDate
            // 
            this.txtUpdatedDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUpdatedDate.Enabled = false;
            this.txtUpdatedDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdatedDate.ForeColor = System.Drawing.Color.Black;
            this.txtUpdatedDate.Location = new System.Drawing.Point(155, 310);
            this.txtUpdatedDate.Name = "txtUpdatedDate";
            this.txtUpdatedDate.ReadOnly = true;
            this.txtUpdatedDate.Size = new System.Drawing.Size(150, 23);
            this.txtUpdatedDate.TabIndex = 84;
            this.txtUpdatedDate.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Blue;
            this.label11.Location = new System.Drawing.Point(45, 310);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 15);
            this.label11.TabIndex = 83;
            this.label11.Text = "Updated Date:";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Pink;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(240, 370);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 25);
            this.btnCancel.TabIndex = 88;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.PaleGreen;
            this.btnSave.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(155, 370);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 25);
            this.btnSave.TabIndex = 87;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tipDate
            // 
            this.tipDate.ToolTipTitle = "Date Entry";
            // 
            // frmRateAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(319, 407);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtUpdatedBy);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtUpdatedDate);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtCreatedBy);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtCreationDate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtEndDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtStartDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboRateType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtGraceDays);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPerDiem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtEntryFee);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCustomer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Name = "frmRateAdmin";
            this.Text = "DAI Export - Rate Admin";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmRateAdmin_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEntryFee;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPerDiem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGraceDays;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboRateType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtStartDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtEndDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCreationDate;
        private System.Windows.Forms.TextBox txtCreatedBy;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtUpdatedBy;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtUpdatedDate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolTip tipDate;
    }
}