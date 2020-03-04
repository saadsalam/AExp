using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmPrintSelection : Form
    {
        //CONSTANTS
        private const string CURRENTMODULE = "frmSetSelection";

        private string strItemToSelect;
        public frmPrintSelection()
        {
            try
            {
                InitializeComponent();

                Formops.SetFormBackground(this);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmSetSelection", ex.Message);
            }
        }

        private void KeyPressTextbox(KeyPressEventArgs e)
        {
            if (!Globalitems.ValidDateKeyStroke(e.KeyChar)) e.Handled = true;
        }

        private bool ValidSelection()
        {
            try
            {
                if (rbPrintDate.Checked)
                {
                    if (txtStartDate.Text.Trim().Length + 
                        txtEndDate.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Please enter at least one Submitted Print Date",
                            "MISSING PRINT DATE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    //Ck that End Date <= Start Date
                    if (txtStartDate.Text.Trim().Length > 0 && txtEndDate.Text.Trim().Length > 0)
                    {
                        if (Convert.ToDateTime(txtStartDate.Text) > 
                            Convert.ToDateTime(txtEndDate.Text))
                        {
                            MessageBox.Show("The Submitted From Date cannot be later than the Submitted To Date.",
                                "INCORRECT FROM/TO DATES",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtStartDate.Focus();
                            return false;
                        }
                    }
                }
                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidSelection", ex.Message);
                return false;
            }
        }

        private void ValidateTextbox(TextBox txtbox, CancelEventArgs e)
        {
            try
            {
                // Use Globalitems ValidDate. If true, strval is in proper date format
                //  If false, don't allow movement from control
                string strval = txtbox.Text.Trim();

                if (Globalitems.ValidDate(ref strval))
                { txtbox.Text = strval; }
                else
                    e.Cancel = true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidateTextbox", ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidSelection())
            {
                if (rbAll.Checked)
                    Globalitems.strSelection = "unprinted";
                else
                {
                    Globalitems.strSelection = "";

                    if (txtStartDate.Text.Trim().Length > 0)
                        Globalitems.strSelection = "DateFrom" +
                                    Globalitems.chrNameValueSeparator +
                                    txtStartDate.Text.Trim() + Globalitems.chrRecordSeparator;

                    if (txtEndDate.Text.Trim().Length > 0)
                        Globalitems.strSelection += "DateTo" +
                                    Globalitems.chrNameValueSeparator +
                                    txtEndDate.Text.Trim();
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbPrintDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPrintDate.Checked)
                pnlPrintDate.Visible = true;
            else
                pnlPrintDate.Visible = false;
        }

        private void txtStartDate_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(e); }

        private void txtEndDate_KeyPress(object sender, KeyPressEventArgs e)
        {KeyPressTextbox(e);}

        private void txtStartDate_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        { ValidateTextbox(txtStartDate, e); }

        private void txtEndDate_Validating(object sender, CancelEventArgs e)
        {ValidateTextbox(txtEndDate, e);}

        private void frmPrintSelection_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
