namespace AutoExport
{
    partial class frmLoadSeqSortOrder
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.pnlSortby = new System.Windows.Forms.Panel();
            this.rbLowToHigh = new System.Windows.Forms.RadioButton();
            this.rbHighToLow = new System.Windows.Forms.RadioButton();
            this.lblSort = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSequence = new System.Windows.Forms.ComboBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblSequences = new System.Windows.Forms.Label();
            this.dgSeq = new System.Windows.Forms.DataGridView();
            this.Sequence_string = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckCleared = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlSortby.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSeq)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::AutoExport.Properties.Resources.Color_vertical;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 237);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 92;
            this.pictureBox1.TabStop = false;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(40, 5);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(244, 15);
            this.lblMsg.TabIndex = 93;
            this.lblMsg.Text = "Sort the Bay. Loc. rows in each Sequence:";
            // 
            // pnlSortby
            // 
            this.pnlSortby.Controls.Add(this.rbLowToHigh);
            this.pnlSortby.Controls.Add(this.rbHighToLow);
            this.pnlSortby.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlSortby.Location = new System.Drawing.Point(40, 28);
            this.pnlSortby.Name = "pnlSortby";
            this.pnlSortby.Size = new System.Drawing.Size(115, 52);
            this.pnlSortby.TabIndex = 94;
            // 
            // rbLowToHigh
            // 
            this.rbLowToHigh.AutoSize = true;
            this.rbLowToHigh.Location = new System.Drawing.Point(0, 28);
            this.rbLowToHigh.Name = "rbLowToHigh";
            this.rbLowToHigh.Size = new System.Drawing.Size(110, 20);
            this.rbLowToHigh.TabIndex = 1;
            this.rbLowToHigh.TabStop = true;
            this.rbLowToHigh.Text = "Low To High";
            this.rbLowToHigh.UseVisualStyleBackColor = true;
            this.rbLowToHigh.CheckedChanged += new System.EventHandler(this.rbLowToHigh_CheckedChanged);
            // 
            // rbHighToLow
            // 
            this.rbHighToLow.AutoSize = true;
            this.rbHighToLow.Location = new System.Drawing.Point(0, 0);
            this.rbHighToLow.Name = "rbHighToLow";
            this.rbHighToLow.Size = new System.Drawing.Size(110, 20);
            this.rbHighToLow.TabIndex = 0;
            this.rbHighToLow.TabStop = true;
            this.rbHighToLow.Text = "High To Low";
            this.rbHighToLow.UseVisualStyleBackColor = true;
            this.rbHighToLow.CheckedChanged += new System.EventHandler(this.rbHighToLow_CheckedChanged);
            // 
            // lblSort
            // 
            this.lblSort.AutoSize = true;
            this.lblSort.Location = new System.Drawing.Point(40, 98);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(251, 15);
            this.lblSort.TabIndex = 95;
            this.lblSort.Text = "Change the Sort Low To High for Sequence:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(40, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 96;
            this.label2.Text = "Sequence:";
            // 
            // cboSequence
            // 
            this.cboSequence.FormattingEnabled = true;
            this.cboSequence.Location = new System.Drawing.Point(114, 127);
            this.cboSequence.Name = "cboSequence";
            this.cboSequence.Size = new System.Drawing.Size(75, 23);
            this.cboSequence.TabIndex = 97;
            this.cboSequence.SelectedIndexChanged += new System.EventHandler(this.cboSequence_SelectedIndexChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(205, 214);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(65, 23);
            this.btnRemove.TabIndex = 99;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.Color.Pink;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(40, 213);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 101;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.PaleGreen;
            this.btnSave.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(40, 184);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 100;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblSequences
            // 
            this.lblSequences.AutoSize = true;
            this.lblSequences.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSequences.ForeColor = System.Drawing.Color.Black;
            this.lblSequences.Location = new System.Drawing.Point(173, 46);
            this.lblSequences.Name = "lblSequences";
            this.lblSequences.Size = new System.Drawing.Size(87, 16);
            this.lblSequences.TabIndex = 102;
            this.lblSequences.Text = "Sequences: ";
            // 
            // dgSeq
            // 
            this.dgSeq.AllowUserToAddRows = false;
            this.dgSeq.AllowUserToDeleteRows = false;
            this.dgSeq.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.dgSeq.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgSeq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgSeq.BackgroundColor = System.Drawing.Color.White;
            this.dgSeq.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgSeq.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgSeq.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgSeq.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSeq.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sequence_string});
            this.dgSeq.Cursor = System.Windows.Forms.Cursors.Default;
            this.dgSeq.EnableHeadersVisualStyles = false;
            this.dgSeq.Location = new System.Drawing.Point(205, 127);
            this.dgSeq.MultiSelect = false;
            this.dgSeq.Name = "dgSeq";
            this.dgSeq.ReadOnly = true;
            this.dgSeq.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgSeq.RowHeadersWidth = 15;
            this.dgSeq.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgSeq.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgSeq.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSeq.Size = new System.Drawing.Size(70, 80);
            this.dgSeq.TabIndex = 104;
            this.dgSeq.TabStop = false;
            // 
            // Sequence_string
            // 
            this.Sequence_string.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Sequence_string.DataPropertyName = "Sequence_string";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sequence_string.DefaultCellStyle = dataGridViewCellStyle3;
            this.Sequence_string.HeaderText = "Seq.";
            this.Sequence_string.MinimumWidth = 30;
            this.Sequence_string.Name = "Sequence_string";
            this.Sequence_string.ReadOnly = true;
            this.Sequence_string.Width = 55;
            // 
            // ckCleared
            // 
            this.ckCleared.AutoSize = true;
            this.ckCleared.Checked = true;
            this.ckCleared.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckCleared.Location = new System.Drawing.Point(40, 160);
            this.ckCleared.Name = "ckCleared";
            this.ckCleared.Size = new System.Drawing.Size(131, 19);
            this.ckCleared.TabIndex = 105;
            this.ckCleared.Text = "Only Cleared veh\'s";
            this.ckCleared.UseVisualStyleBackColor = true;
            // 
            // frmLoadSeqSortOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(299, 242);
            this.Controls.Add(this.ckCleared);
            this.Controls.Add(this.dgSeq);
            this.Controls.Add(this.lblSequences);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.cboSequence);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSort);
            this.Controls.Add(this.pnlSortby);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Name = "frmLoadSeqSortOrder";
            this.Text = "DAI Export - Load Seq. Sort Order";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmLoadSeqSortOrder_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlSortby.ResumeLayout(false);
            this.pnlSortby.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSeq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Panel pnlSortby;
        private System.Windows.Forms.RadioButton rbLowToHigh;
        private System.Windows.Forms.RadioButton rbHighToLow;
        private System.Windows.Forms.Label lblSort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboSequence;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblSequences;
        protected System.Windows.Forms.DataGridView dgSeq;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sequence_string;
        private System.Windows.Forms.CheckBox ckCleared;
    }
}