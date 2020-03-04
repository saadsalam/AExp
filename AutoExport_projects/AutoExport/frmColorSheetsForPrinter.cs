using AutoExport.Objects;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmColorSheetsForPrinter : Form
    {
        public frmColorSheetsForPrinter(string strColor, string strDestination,string strCust,
            int intQty,System.Drawing.Color drColor)
        {
            InitializeComponent();

            Formops.SetFormBackground(this);
            lblColor.Text = "Color: " + strColor;
            lblDestination.Text = "Destination: " + strDestination;
            lblCust.Text = "Customer: " + strCust;
            lblQty.Text = "Quantity: " + intQty.ToString();
            panel1.BackColor = drColor;
            panel2.BackColor = drColor;
            panel3.BackColor = drColor;
        }

        private void btnContinue_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void frmColorSheetsForPrinter_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
