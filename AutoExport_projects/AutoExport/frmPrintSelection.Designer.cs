namespace AutoExport
{
    partial class frmPrintSelection
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
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlPrintDate = new System.Windows.Forms.Panel();
            this.txtEndDate = new System.Windows.Forms.TextBox();
            this.txtStartDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbPrintDate = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlPrintDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(45, 5);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(215, 16);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "Please select the Print option";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Pink;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(129, 171);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 25);
            this.btnCancel.TabIndex = 90;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.PaleGreen;
            this.btnSave.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(48, 171);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 25);
            this.btnSave.TabIndex = 89;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 203);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 91;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlPrintDate);
            this.panel1.Controls.Add(this.rbPrintDate);
            this.panel1.Controls.Add(this.rbAll);
            this.panel1.Location = new System.Drawing.Point(48, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(275, 130);
            this.panel1.TabIndex = 92;
            // 
            // pnlPrintDate
            // 
            this.pnlPrintDate.Controls.Add(this.txtEndDate);
            this.pnlPrintDate.Controls.Add(this.txtStartDate);
            this.pnlPrintDate.Controls.Add(this.label4);
            this.pnlPrintDate.Controls.Add(this.label3);
            this.pnlPrintDate.Location = new System.Drawing.Point(23, 60);
            this.pnlPrintDate.Name = "pnlPrintDate";
            this.pnlPrintDate.Size = new System.Drawing.Size(243, 62);
            this.pnlPrintDate.TabIndex = 80;
            this.pnlPrintDate.Visible = false;
            // 
            // txtEndDate
            // 
            this.txtEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEndDate.ForeColor = System.Drawing.Color.Black;
            this.txtEndDate.Location = new System.Drawing.Point(135, 35);
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Size = new System.Drawing.Size(100, 23);
            this.txtEndDate.TabIndex = 84;
            this.txtEndDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEndDate_KeyPress);
            this.txtEndDate.Validating += new System.ComponentModel.CancelEventHandler(this.txtEndDate_Validating);
            // 
            // txtStartDate
            // 
            this.txtStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStartDate.ForeColor = System.Drawing.Color.Black;
            this.txtStartDate.Location = new System.Drawing.Point(135, 0);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(100, 23);
            this.txtStartDate.TabIndex = 83;
            this.txtStartDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStartDate_KeyPress);
            this.txtStartDate.Validating += new System.ComponentModel.CancelEventHandler(this.txtStartDate_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 16);
            this.label4.TabIndex = 82;
            this.label4.Text = "Submitted To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 16);
            this.label3.TabIndex = 81;
            this.label3.Text = "Submitted From:";
            // 
            // rbPrintDate
            // 
            this.rbPrintDate.AutoSize = true;
            this.rbPrintDate.Location = new System.Drawing.Point(0, 30);
            this.rbPrintDate.Name = "rbPrintDate";
            this.rbPrintDate.Size = new System.Drawing.Size(181, 20);
            this.rbPrintDate.TabIndex = 78;
            this.rbPrintDate.TabStop = true;
            this.rbPrintDate.Text = "Select Submitted Date";
            this.rbPrintDate.UseVisualStyleBackColor = true;
            this.rbPrintDate.CheckedChanged += new System.EventHandler(this.rbPrintDate_CheckedChanged);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(0, 0);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(206, 20);
            this.rbAll.TabIndex = 76;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "Print All Unprinted Sheets";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // frmPrintSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(349, 201);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblMsg);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmPrintSelection";
            this.Text = "DAI Export - Customs Submitted";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmPrintSelection_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlPrintDate.ResumeLayout(false);
            this.pnlPrintDate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlPrintDate;
        private System.Windows.Forms.TextBox txtEndDate;
        private System.Windows.Forms.TextBox txtStartDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbPrintDate;
        private System.Windows.Forms.RadioButton rbAll;
    }
}