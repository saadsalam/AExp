namespace AutoExport
{
    partial class frmPrintInvoices
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnMenu = new System.Windows.Forms.Button();
            this.rbUnprinted = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtVIN = new System.Windows.Forms.TextBox();
            this.rbVIN = new System.Windows.Forms.RadioButton();
            this.rbInvNumber = new System.Windows.Forms.RadioButton();
            this.rbDate = new System.Windows.Forms.RadioButton();
            this.btnPrint = new System.Windows.Forms.Button();
            this.toolTipPrint = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipDisplay = new System.Windows.Forms.ToolTip(this.components);
            this.pnlDate = new System.Windows.Forms.Panel();
            this.txtDateTo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDateFrom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbPrintDate = new System.Windows.Forms.RadioButton();
            this.rbInvDate = new System.Windows.Forms.RadioButton();
            this.pnlInvoice = new System.Windows.Forms.Panel();
            this.txtInvTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInvFrom = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblPrinting = new System.Windows.Forms.Label();
            this.bckgrdDisplay = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlDate.SuspendLayout();
            this.pnlInvoice.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 195);
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
            // rbUnprinted
            // 
            this.rbUnprinted.AutoSize = true;
            this.rbUnprinted.Location = new System.Drawing.Point(0, 0);
            this.rbUnprinted.Name = "rbUnprinted";
            this.rbUnprinted.Size = new System.Drawing.Size(177, 19);
            this.rbUnprinted.TabIndex = 76;
            this.rbUnprinted.TabStop = true;
            this.rbUnprinted.Text = "Print All Unprinted Invoices";
            this.rbUnprinted.UseVisualStyleBackColor = true;
            this.rbUnprinted.CheckedChanged += new System.EventHandler(this.rbUnprinted_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtVIN);
            this.panel1.Controls.Add(this.rbVIN);
            this.panel1.Controls.Add(this.rbInvNumber);
            this.panel1.Controls.Add(this.rbDate);
            this.panel1.Controls.Add(this.rbUnprinted);
            this.panel1.Location = new System.Drawing.Point(45, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(454, 55);
            this.panel1.TabIndex = 77;
            // 
            // txtVIN
            // 
            this.txtVIN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVIN.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVIN.Location = new System.Drawing.Point(280, 5);
            this.txtVIN.Name = "txtVIN";
            this.txtVIN.Size = new System.Drawing.Size(144, 21);
            this.txtVIN.TabIndex = 80;
            this.txtVIN.Visible = false;
            // 
            // rbVIN
            // 
            this.rbVIN.AutoSize = true;
            this.rbVIN.Location = new System.Drawing.Point(200, 5);
            this.rbVIN.Name = "rbVIN";
            this.rbVIN.Size = new System.Drawing.Size(72, 19);
            this.rbVIN.TabIndex = 79;
            this.rbVIN.TabStop = true;
            this.rbVIN.Text = "VIN (full)";
            this.rbVIN.UseVisualStyleBackColor = true;
            this.rbVIN.CheckedChanged += new System.EventHandler(this.rbVIN_CheckedChanged);
            // 
            // rbInvNumber
            // 
            this.rbInvNumber.AutoSize = true;
            this.rbInvNumber.Location = new System.Drawing.Point(200, 33);
            this.rbInvNumber.Name = "rbInvNumber";
            this.rbInvNumber.Size = new System.Drawing.Size(152, 19);
            this.rbInvNumber.TabIndex = 78;
            this.rbInvNumber.TabStop = true;
            this.rbInvNumber.Text = "Select Invoice Number";
            this.rbInvNumber.UseVisualStyleBackColor = true;
            this.rbInvNumber.CheckedChanged += new System.EventHandler(this.rbInvNumber_CheckedChanged);
            // 
            // rbDate
            // 
            this.rbDate.AutoSize = true;
            this.rbDate.Location = new System.Drawing.Point(0, 30);
            this.rbDate.Name = "rbDate";
            this.rbDate.Size = new System.Drawing.Size(90, 19);
            this.rbDate.TabIndex = 77;
            this.rbDate.TabStop = true;
            this.rbDate.Text = "Select Date";
            this.rbDate.UseVisualStyleBackColor = true;
            this.rbDate.CheckedChanged += new System.EventHandler(this.rbDate_CheckedChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(45, 160);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 25);
            this.btnPrint.TabIndex = 80;
            this.btnPrint.Text = "Print Invoices";
            this.toolTipPrint.SetToolTip(this.btnPrint, "Send labels directly to the Label printer.");
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // pnlDate
            // 
            this.pnlDate.Controls.Add(this.txtDateTo);
            this.pnlDate.Controls.Add(this.label4);
            this.pnlDate.Controls.Add(this.txtDateFrom);
            this.pnlDate.Controls.Add(this.label3);
            this.pnlDate.Controls.Add(this.rbPrintDate);
            this.pnlDate.Controls.Add(this.rbInvDate);
            this.pnlDate.Location = new System.Drawing.Point(45, 65);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(187, 90);
            this.pnlDate.TabIndex = 82;
            this.pnlDate.Visible = false;
            // 
            // txtDateTo
            // 
            this.txtDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDateTo.Location = new System.Drawing.Point(45, 60);
            this.txtDateTo.Name = "txtDateTo";
            this.txtDateTo.Size = new System.Drawing.Size(80, 21);
            this.txtDateTo.TabIndex = 83;
            this.txtDateTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDateTo_KeyPress);
            this.txtDateTo.Validating += new System.ComponentModel.CancelEventHandler(this.txtDateTo_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 15);
            this.label4.TabIndex = 82;
            this.label4.Text = "To:";
            // 
            // txtDateFrom
            // 
            this.txtDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateFrom.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDateFrom.Location = new System.Drawing.Point(45, 30);
            this.txtDateFrom.Name = "txtDateFrom";
            this.txtDateFrom.Size = new System.Drawing.Size(80, 21);
            this.txtDateFrom.TabIndex = 81;
            this.txtDateFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDateFrom_KeyPress);
            this.txtDateFrom.Validating += new System.ComponentModel.CancelEventHandler(this.txtDateFrom_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 80;
            this.label3.Text = "From:";
            // 
            // rbPrintDate
            // 
            this.rbPrintDate.AutoSize = true;
            this.rbPrintDate.Location = new System.Drawing.Point(105, 0);
            this.rbPrintDate.Name = "rbPrintDate";
            this.rbPrintDate.Size = new System.Drawing.Size(81, 19);
            this.rbPrintDate.TabIndex = 79;
            this.rbPrintDate.TabStop = true;
            this.rbPrintDate.Text = "Print Date";
            this.rbPrintDate.UseVisualStyleBackColor = true;
            // 
            // rbInvDate
            // 
            this.rbInvDate.AutoSize = true;
            this.rbInvDate.Location = new System.Drawing.Point(0, 0);
            this.rbInvDate.Name = "rbInvDate";
            this.rbInvDate.Size = new System.Drawing.Size(94, 19);
            this.rbInvDate.TabIndex = 78;
            this.rbInvDate.TabStop = true;
            this.rbInvDate.Text = "Invoice Date";
            this.rbInvDate.UseVisualStyleBackColor = true;
            // 
            // pnlInvoice
            // 
            this.pnlInvoice.Controls.Add(this.txtInvTo);
            this.pnlInvoice.Controls.Add(this.label2);
            this.pnlInvoice.Controls.Add(this.txtInvFrom);
            this.pnlInvoice.Controls.Add(this.label1);
            this.pnlInvoice.Location = new System.Drawing.Point(250, 65);
            this.pnlInvoice.Name = "pnlInvoice";
            this.pnlInvoice.Size = new System.Drawing.Size(177, 60);
            this.pnlInvoice.TabIndex = 83;
            this.pnlInvoice.Visible = false;
            // 
            // txtInvTo
            // 
            this.txtInvTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInvTo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvTo.Location = new System.Drawing.Point(70, 30);
            this.txtInvTo.Name = "txtInvTo";
            this.txtInvTo.Size = new System.Drawing.Size(100, 21);
            this.txtInvTo.TabIndex = 3;
            this.txtInvTo.Text = "EX00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Inv. To:";
            // 
            // txtInvFrom
            // 
            this.txtInvFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInvFrom.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvFrom.Location = new System.Drawing.Point(70, 0);
            this.txtInvFrom.Name = "txtInvFrom";
            this.txtInvFrom.Size = new System.Drawing.Size(100, 21);
            this.txtInvFrom.TabIndex = 1;
            this.txtInvFrom.Text = "EX00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Inv. From:";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(250, 160);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 25);
            this.btnClose.TabIndex = 81;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblPrinting
            // 
            this.lblPrinting.AutoSize = true;
            this.lblPrinting.BackColor = System.Drawing.Color.Blue;
            this.lblPrinting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblPrinting.Location = new System.Drawing.Point(250, 136);
            this.lblPrinting.Name = "lblPrinting";
            this.lblPrinting.Size = new System.Drawing.Size(113, 15);
            this.lblPrinting.TabIndex = 84;
            this.lblPrinting.Text = "Preparing Invoices";
            // 
            // bckgrdDisplay
            // 
            this.bckgrdDisplay.WorkerReportsProgress = true;
            this.bckgrdDisplay.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bckgrdDisplay_ProgressChanged);
            // 
            // frmPrintInvoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(509, 192);
            this.Controls.Add(this.lblPrinting);
            this.Controls.Add(this.pnlInvoice);
            this.Controls.Add(this.pnlDate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.pictureBox2);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Name = "frmPrintInvoices";
            this.Text = "DAI Export - Print Invoices";
            this.Activated += new System.EventHandler(this.frmPrintInvoices_Activated);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmPrintInvoices_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlDate.ResumeLayout(false);
            this.pnlDate.PerformLayout();
            this.pnlInvoice.ResumeLayout(false);
            this.pnlInvoice.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.RadioButton rbUnprinted;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbInvNumber;
        private System.Windows.Forms.RadioButton rbDate;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.ToolTip toolTipDisplay;
        private System.Windows.Forms.ToolTip toolTipPrint;
        private System.Windows.Forms.Panel pnlDate;
        private System.Windows.Forms.TextBox txtDateTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDateFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbPrintDate;
        private System.Windows.Forms.RadioButton rbInvDate;
        private System.Windows.Forms.Panel pnlInvoice;
        private System.Windows.Forms.TextBox txtInvTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInvFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtVIN;
        private System.Windows.Forms.RadioButton rbVIN;
        private System.Windows.Forms.Label lblPrinting;
        private System.ComponentModel.BackgroundWorker bckgrdDisplay;
    }
}