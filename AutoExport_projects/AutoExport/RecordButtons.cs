using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoExport
{

    public partial class RecordButtons : UserControl
    {
        public const string ACTION_READONLY = "READONLY";
        public const string ACTION_NEWRECORD = "NEW";
        public const string ACTION_MODIFYRECORD = "MODIFY";

        //Create public event variables so using form can assign methods to each event
        public event CancelRecordDelegate CancelRecord;
        public event DeleteRecordDelegate DeleteRecord;
        public event ModifyRecordDelegate ModifyRecord;
        public event MovePrevDelegate MovePrev;
        public event MoveNextDelegate MoveNext;
        public event NewRecordDelegate NewRecord;
        public event SaveRecordDelegate SaveRecord;

        public bool blnRecordsToDisplay = false;

        public RecordButtons()
        {
            InitializeComponent();
            SetButtons("READONLY");
        }

        public void EnablebtnMovePrev(bool blnEnable)
        {
            if (blnEnable)
            {
                btnMovePrev.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.PrevMove);
            }
            else
            {   
                btnMovePrev.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.PrevMove_grey);
            }
            btnMovePrev.Enabled = blnEnable;
        }

        public void EnablebtnMoveNext(bool blnEnable)
        {
            if (blnEnable)
            {
                btnMoveNext.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.NextMove);
            }
            else
            {
                btnMoveNext.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.NextMove_grey);
            }
            btnMoveNext.Enabled = blnEnable;
        }

        public void EnableModifyButton(bool blnEnable)
        {
            btnModify.Enabled = blnEnable;
        }

        public void EnableDeleteButton(bool blnEnable)
        {
            btnDelete.Enabled = blnEnable;
        }

        public void SetButtons(string strAction)
        {
            if (strAction == ACTION_READONLY)
            {
                // Enabled buttons show Forecolor:Blue, Disabled buttons show Forecolor/Grey.
                // Only need to Enable/Disable New/Modify/Delete buttons
                btnNew.Enabled = true;

                //Only enable Modify/Delete/Prev/Next if there are records
                btnModify.Enabled = blnRecordsToDisplay;
                btnDelete.Enabled = blnRecordsToDisplay;

                //Nav buttons need to swap images to emphasize Enable/Disable state
                btnMovePrev.Enabled = blnRecordsToDisplay;
                btnMoveNext.Enabled = blnRecordsToDisplay;
                if (blnRecordsToDisplay)
                {
                    btnMoveNext.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.NextMove);
                    btnMovePrev.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.PrevMove);
                }
                else
                {
                    btnMoveNext.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.NextMove_grey);
                    btnMovePrev.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.PrevMove_grey);
                }

                //Change background color
                btnSave.Enabled = false;
                btnSave.BackColor = Color.DarkGray;

                btnCancel.Enabled = false;
                btnCancel.BackColor = Color.DarkGray;
            }
            else
            {
                //New or Modify action. Disable record buttons
                btnNew.Enabled = false;
                btnModify.Enabled = false;
                btnDelete.Enabled = false;
                btnMovePrev.Enabled = false;
                btnMoveNext.Enabled = false;
                btnMoveNext.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.NextMove_grey);
                btnMovePrev.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.PrevMove_grey);

                //Enable & Change background, Forecolor for btnSave, btnCancel
                btnSave.Enabled = true;
                btnSave.UseVisualStyleBackColor = false;
                btnSave.BackColor = Color.PaleGreen;
                btnSave.ForeColor = Color.Black;

                btnCancel.Enabled = true;
                btnCancel.UseVisualStyleBackColor = false;
                btnCancel.BackColor = Color.Pink;
                btnCancel.ForeColor = Color.Red;
            }
        }

        public void btnNew_Click(object sender, EventArgs e)
        {
            SetButtons(ACTION_NEWRECORD);

            //Invoke the method in the using form that NewRecord points to
            NewRecord();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            SetButtons(ACTION_MODIFYRECORD);

            //Invoke the method in the using form that ModifyRecord points to
            ModifyRecord();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Invoke the method in the using form that DeleteRecord points to
            DeleteRecord();
        }

        private void btnMovePrev_Click(object sender, EventArgs e)
        {
            //Invoke the method in the using form that MovePrev points to
            MovePrev();
        }

        private void btnMoveNext_Click(object sender, EventArgs e)
        {
            //Invoke the method in the using form that MoveNext points to
            MoveNext();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetButtons(ACTION_READONLY);

            //Invoke the method in the using form that CancelRecord points to
            CancelRecord();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Invoke the method in the using form that CancelRecord points to
            SaveRecord();
        }
    }
}
