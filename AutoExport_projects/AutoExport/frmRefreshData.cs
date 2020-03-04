using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmRefreshData : Form
    {
        private bool blnDoneRefresh = false;

        public frmRefreshData()
        {
            InitializeComponent();
            Formops.SetFormBackground(this);
        }

        private void RefreshData()
        {
            DateTime datStart = DateTime.Now;
            TimeSpan duration;

            try
            {
                //Get Refresh data as background task
                bckLoadData.RunWorkerAsync();

                //Need to update controls to display while
                //background task is running
                pctHourGlass.Update();
                lblTime.Update();
                lblMin.Update();
                lblSec.Update();
                txtMsg.Update();

                while (!blnDoneRefresh)
                {
                    duration = DateTime.Now - datStart;
                    txtMin.Text = duration.Minutes.ToString();

                    //Use mod 60 to get just secs
                    txtSec.Text = (duration.Seconds % 60).ToString();

                    txtMin.Update();
                    txtSec.Update();
                }

                this.Close();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException("frmRefreshData", "RefreshData", ex.Message);
            }
        }

        private void bckLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet ds;
            string strSProc = "spRefreshFromAutoExport";
            string strResult;

            ds = DataOps.GetDataset_with_SProc(strSProc);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                Globalitems.HandleException("frmRefreshData",
                    "dataRefreshToolStripMenuItem_Click",
                    "No data returned from SProc");
                blnDoneRefresh = true;
                return;
            }

            strResult = ds.Tables[0].Rows[0]["result"].ToString();
            if (strResult != "OK")
            {
                Globalitems.HandleException("frmRefreshData",
                    "dataRefreshToolStripMenuItem_Click",
                    "Error from SProc: " + ds.Tables[0].Rows[0]["ErrorMessage"]);
                blnDoneRefresh = true;
                return;
            }

            blnDoneRefresh = true;

            if (MessageBox.Show("The Export TEST data is now the same as Export",
               "TEST DATA UPDATED", MessageBoxButtons.OK, MessageBoxIcon.None)==DialogResult.OK)
                this.Close();           
        }

        private void btnStart_Click(object sender, EventArgs e)
        {RefreshData();}
    }
}
