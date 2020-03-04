using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;


namespace AutoExport
{
    public partial class frmTimeOutNotice : Form
    {

        public frmTimeOutNotice()
        {
            InitializeComponent();
            Formops.SetFormBackground(this);

            lblMsg.Text = "Due to inactivity the program will close in \n" +
                Globalitems.SECONDSFORCLOSINGPROGRAM.ToString() + " seconds.\n\n" +
                "Click Cancel Timeout to continue working.";
        }

        private void btnCancelTimeout_Click(object sender, EventArgs e)
        {
            Globalitems.ResetActivityTimer();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
