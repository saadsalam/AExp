namespace AutoExport
{
    partial class RecordButtons
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordButtons));
            this.tipMoveFirst = new System.Windows.Forms.ToolTip(this.components);
            this.tipMovePrev = new System.Windows.Forms.ToolTip(this.components);
            this.btnMovePrev = new System.Windows.Forms.Button();
            this.tipMoveNext = new System.Windows.Forms.ToolTip(this.components);
            this.btnMoveNext = new System.Windows.Forms.Button();
            this.tipMoveLast = new System.Windows.Forms.ToolTip(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tipMoveFirst
            // 
            this.tipMoveFirst.ToolTipTitle = "Move To First";
            // 
            // tipMovePrev
            // 
            this.tipMovePrev.ToolTipTitle = "Move To Prev";
            // 
            // btnMovePrev
            // 
            this.btnMovePrev.BackColor = System.Drawing.Color.White;
            this.btnMovePrev.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMovePrev.BackgroundImage")));
            this.btnMovePrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMovePrev.Location = new System.Drawing.Point(0, 75);
            this.btnMovePrev.Name = "btnMovePrev";
            this.btnMovePrev.Size = new System.Drawing.Size(30, 25);
            this.btnMovePrev.TabIndex = 21;
            this.tipMovePrev.SetToolTip(this.btnMovePrev, "Move to previous record");
            this.btnMovePrev.UseVisualStyleBackColor = false;
            this.btnMovePrev.Click += new System.EventHandler(this.btnMovePrev_Click);
            // 
            // tipMoveNext
            // 
            this.tipMoveNext.ToolTipTitle = "Move To Next";
            // 
            // btnMoveNext
            // 
            this.btnMoveNext.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMoveNext.BackgroundImage")));
            this.btnMoveNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMoveNext.Location = new System.Drawing.Point(36, 75);
            this.btnMoveNext.Name = "btnMoveNext";
            this.btnMoveNext.Size = new System.Drawing.Size(30, 25);
            this.btnMoveNext.TabIndex = 22;
            this.tipMoveNext.SetToolTip(this.btnMoveNext, "Move to next record");
            this.btnMoveNext.UseVisualStyleBackColor = true;
            this.btnMoveNext.Click += new System.EventHandler(this.btnMoveNext_Click);
            // 
            // tipMoveLast
            // 
            this.tipMoveLast.ToolTipTitle = "Move To Last";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Pink;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(0, 125);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 25);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackgroundImage = global::AutoExport.Properties.Resources.gradient_white;
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(0, 50);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(65, 25);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.BackgroundImage = global::AutoExport.Properties.Resources.gradient_white;
            this.btnModify.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnModify.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModify.Location = new System.Drawing.Point(0, 25);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(65, 25);
            this.btnModify.TabIndex = 16;
            this.btnModify.Text = "Modify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnNew
            // 
            this.btnNew.BackgroundImage = global::AutoExport.Properties.Resources.gradient_white;
            this.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNew.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Location = new System.Drawing.Point(0, 0);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(65, 25);
            this.btnNew.TabIndex = 15;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(0, 100);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 25);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // RecordButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnMoveNext);
            this.Controls.Add(this.btnMovePrev);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.btnNew);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;
            this.Name = "RecordButtons";
            this.Size = new System.Drawing.Size(65, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip tipMoveFirst;
        private System.Windows.Forms.Button btnMovePrev;
        private System.Windows.Forms.ToolTip tipMovePrev;
        private System.Windows.Forms.Button btnMoveNext;
        private System.Windows.Forms.ToolTip tipMoveNext;
        private System.Windows.Forms.ToolTip tipMoveLast;
        private System.Windows.Forms.Button btnSave;
    }
}
