using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmLockout : Form
    {
        public frmLockout()
        {
            InitializeComponent();
            lblMsg.Text = Globalitems.strLockoutMsg;
            SoundPlayer simplesound = new SoundPlayer(Properties.Resources.explosion);
            simplesound.Play();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLockout_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
