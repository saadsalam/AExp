namespace AutoExport
{
    partial class frmCustomClearedSheets
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnMenu = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbVoyage = new System.Windows.Forms.RadioButton();
            this.txtVIN = new System.Windows.Forms.TextBox();
            this.pnlPrintDate = new System.Windows.Forms.Panel();
            this.txtEndDate = new System.Windows.Forms.TextBox();
            this.txtStartDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbPrintDateCriteria = new System.Windows.Forms.RadioButton();
            this.rbApprovedDate = new System.Windows.Forms.RadioButton();
            this.rbSelected = new System.Windows.Forms.RadioButton();
            this.rbPrintDate = new System.Windows.Forms.RadioButton();
            this.rbVIN = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.cboCust = new System.Windows.Forms.ComboBox();
            this.ckActive = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVoyage = new System.Windows.Forms.Label();
            this.cboVoyage = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.lblNoVoyages = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlPrintDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox2.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 368);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 74;
            this.pictureBox2.TabStop = false;
            // 
            // btnMenu
            // 
            this.btnMenu.Image = global::AutoExport.Properties.Resources.Menu1;
            this.btnMenu.Location = new System.Drawing.Point(5, 12);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 30);
            this.btnMenu.TabIndex = 76;
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbVoyage);
            this.panel1.Controls.Add(this.txtVIN);
            this.panel1.Controls.Add(this.pnlPrintDate);
            this.panel1.Controls.Add(this.rbSelected);
            this.panel1.Controls.Add(this.rbPrintDate);
            this.panel1.Controls.Add(this.rbVIN);
            this.panel1.Controls.Add(this.rbAll);
            this.panel1.Location = new System.Drawing.Point(45, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 200);
            this.panel1.TabIndex = 78;
            // 
            // rbVoyage
            // 
            this.rbVoyage.AutoSize = true;
            this.rbVoyage.Location = new System.Drawing.Point(0, 180);
            this.rbVoyage.Name = "rbVoyage";
            this.rbVoyage.Size = new System.Drawing.Size(132, 19);
            this.rbVoyage.TabIndex = 82;
            this.rbVoyage.TabStop = true;
            this.rbVoyage.Text = "Print Entire Voyage";
            this.rbVoyage.UseVisualStyleBackColor = true;
            // 
            // txtVIN
            // 
            this.txtVIN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVIN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtVIN.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVIN.Location = new System.Drawing.Point(135, 150);
            this.txtVIN.Margin = new System.Windows.Forms.Padding(4);
            this.txtVIN.MaxLength = 17;
            this.txtVIN.MinimumSize = new System.Drawing.Size(45, 25);
            this.txtVIN.Name = "txtVIN";
            this.txtVIN.Size = new System.Drawing.Size(135, 23);
            this.txtVIN.TabIndex = 81;
            this.txtVIN.Visible = false;
            // 
            // pnlPrintDate
            // 
            this.pnlPrintDate.Controls.Add(this.txtEndDate);
            this.pnlPrintDate.Controls.Add(this.txtStartDate);
            this.pnlPrintDate.Controls.Add(this.label4);
            this.pnlPrintDate.Controls.Add(this.label3);
            this.pnlPrintDate.Controls.Add(this.rbPrintDateCriteria);
            this.pnlPrintDate.Controls.Add(this.rbApprovedDate);
            this.pnlPrintDate.Location = new System.Drawing.Point(135, 60);
            this.pnlPrintDate.Name = "pnlPrintDate";
            this.pnlPrintDate.Size = new System.Drawing.Size(205, 89);
            this.pnlPrintDate.TabIndex = 80;
            this.pnlPrintDate.Visible = false;
            // 
            // txtEndDate
            // 
            this.txtEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndDate.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEndDate.ForeColor = System.Drawing.Color.Black;
            this.txtEndDate.Location = new System.Drawing.Point(70, 60);
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
            this.txtStartDate.Location = new System.Drawing.Point(70, 30);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(100, 23);
            this.txtStartDate.TabIndex = 83;
            this.txtStartDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStartDate_KeyPress);
            this.txtStartDate.Validating += new System.ComponentModel.CancelEventHandler(this.txtStartDate_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 15);
            this.label4.TabIndex = 82;
            this.label4.Text = "End Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 15);
            this.label3.TabIndex = 81;
            this.label3.Text = "Start Date";
            // 
            // rbPrintDateCriteria
            // 
            this.rbPrintDateCriteria.AutoSize = true;
            this.rbPrintDateCriteria.Location = new System.Drawing.Point(125, 0);
            this.rbPrintDateCriteria.Name = "rbPrintDateCriteria";
            this.rbPrintDateCriteria.Size = new System.Drawing.Size(81, 19);
            this.rbPrintDateCriteria.TabIndex = 80;
            this.rbPrintDateCriteria.TabStop = true;
            this.rbPrintDateCriteria.Text = "Print Date";
            this.rbPrintDateCriteria.UseVisualStyleBackColor = true;
            // 
            // rbApprovedDate
            // 
            this.rbApprovedDate.AutoSize = true;
            this.rbApprovedDate.Location = new System.Drawing.Point(0, 0);
            this.rbApprovedDate.Name = "rbApprovedDate";
            this.rbApprovedDate.Size = new System.Drawing.Size(108, 19);
            this.rbApprovedDate.TabIndex = 79;
            this.rbApprovedDate.TabStop = true;
            this.rbApprovedDate.Text = "Approved Date";
            this.rbApprovedDate.UseVisualStyleBackColor = true;
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
            this.rbPrintDate.Location = new System.Drawing.Point(0, 60);
            this.rbPrintDate.Name = "rbPrintDate";
            this.rbPrintDate.Size = new System.Drawing.Size(90, 19);
            this.rbPrintDate.TabIndex = 78;
            this.rbPrintDate.TabStop = true;
            this.rbPrintDate.Text = "Select Date";
            this.rbPrintDate.UseVisualStyleBackColor = true;
            this.rbPrintDate.CheckedChanged += new System.EventHandler(this.rbPrintDate_CheckedChanged);
            // 
            // rbVIN
            // 
            this.rbVIN.AutoSize = true;
            this.rbVIN.Location = new System.Drawing.Point(0, 150);
            this.rbVIN.Name = "rbVIN";
            this.rbVIN.Size = new System.Drawing.Size(91, 19);
            this.rbVIN.TabIndex = 77;
            this.rbVIN.TabStop = true;
            this.rbVIN.Text = "Print By VIN";
            this.rbVIN.UseVisualStyleBackColor = true;
            this.rbVIN.CheckedChanged += new System.EventHandler(this.rbVIN_CheckedChanged);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(0, 30);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(170, 19);
            this.rbAll.TabIndex = 76;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "Print All Unprinted Sheets";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // cboCust
            // 
            this.cboCust.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCust.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCust.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCust.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCust.FormattingEnabled = true;
            this.cboCust.Location = new System.Drawing.Point(115, 5);
            this.cboCust.MaxDropDownItems = 40;
            this.cboCust.Name = "cboCust";
            this.cboCust.Size = new System.Drawing.Size(180, 24);
            this.cboCust.TabIndex = 79;
            this.cboCust.SelectedIndexChanged += new System.EventHandler(this.cboCust_SelectedIndexChanged);
            // 
            // ckActive
            // 
            this.ckActive.AutoSize = true;
            this.ckActive.Checked = true;
            this.ckActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckActive.Location = new System.Drawing.Point(310, 5);
            this.ckActive.Name = "ckActive";
            this.ckActive.Size = new System.Drawing.Size(194, 19);
            this.ckActive.TabIndex = 80;
            this.ckActive.Text = "Only show ACTIVE customers";
            this.ckActive.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 81;
            this.label1.Text = "Customer";
            // 
            // lblVoyage
            // 
            this.lblVoyage.AutoSize = true;
            this.lblVoyage.Location = new System.Drawing.Point(45, 40);
            this.lblVoyage.Name = "lblVoyage";
            this.lblVoyage.Size = new System.Drawing.Size(48, 15);
            this.lblVoyage.TabIndex = 82;
            this.lblVoyage.Text = "Voyage";
            // 
            // cboVoyage
            // 
            this.cboVoyage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVoyage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboVoyage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVoyage.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboVoyage.FormattingEnabled = true;
            this.cboVoyage.Location = new System.Drawing.Point(115, 40);
            this.cboVoyage.MaxDropDownItems = 40;
            this.cboVoyage.Name = "cboVoyage";
            this.cboVoyage.Size = new System.Drawing.Size(377, 24);
            this.cboVoyage.TabIndex = 83;
            this.cboVoyage.SelectedIndexChanged += new System.EventHandler(this.cboVoyage_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(392, 299);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 25);
            this.btnClose.TabIndex = 86;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(399, 249);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 25);
            this.btnPrint.TabIndex = 85;
            this.btnPrint.Text = "Print Sheets";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Visible = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnDisplay
            // 
            this.btnDisplay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisplay.Location = new System.Drawing.Point(170, 299);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(100, 25);
            this.btnDisplay.TabIndex = 84;
            this.btnDisplay.Text = "Display Sheets";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // lblNoVoyages
            // 
            this.lblNoVoyages.AutoSize = true;
            this.lblNoVoyages.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoVoyages.ForeColor = System.Drawing.Color.Red;
            this.lblNoVoyages.Location = new System.Drawing.Point(360, 67);
            this.lblNoVoyages.Name = "lblNoVoyages";
            this.lblNoVoyages.Size = new System.Drawing.Size(132, 15);
            this.lblNoVoyages.TabIndex = 87;
            this.lblNoVoyages.Text = "No voyages scheduled";
            this.lblNoVoyages.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 335);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(232, 15);
            this.label5.TabIndex = 88;
            this.label5.Text = "Sorted by Destination, Bay Location, VIN";
            // 
            // frmCustomClearedSheets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(511, 362);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblNoVoyages);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnDisplay);
            this.Controls.Add(this.cboVoyage);
            this.Controls.Add(this.lblVoyage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ckActive);
            this.Controls.Add(this.cboCust);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.pictureBox2);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Name = "frmCustomClearedSheets";
            this.Text = "DAI Export - Customs Cleared Sheets";
            this.Activated += new System.EventHandler(this.frmCustomClearedSheets_Activated);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmCustomClearedSheets_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlPrintDate.ResumeLayout(false);
            this.pnlPrintDate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbSelected;
        private System.Windows.Forms.RadioButton rbPrintDate;
        private System.Windows.Forms.RadioButton rbVIN;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.ComboBox cboCust;
        private System.Windows.Forms.CheckBox ckActive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblVoyage;
        private System.Windows.Forms.ComboBox cboVoyage;
        private System.Windows.Forms.Panel pnlPrintDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbPrintDateCriteria;
        private System.Windows.Forms.RadioButton rbApprovedDate;
        private System.Windows.Forms.TextBox txtEndDate;
        private System.Windows.Forms.TextBox txtStartDate;
        private System.Windows.Forms.RadioButton rbVoyage;
        public System.Windows.Forms.TextBox txtVIN;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnDisplay;
        private System.Windows.Forms.Label lblNoVoyages;
        private System.Windows.Forms.Label label5;
    }
}