using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmRateAdmin : Form
    {
        //Receive frmCustomerAdmin & a datarow for dtNewRates as params
        //Update the datarow if User clicks SAVE
        private const string CURRENTMODULE = "frmRateAdmin";

        private DataRow drowtomodify;
        private frmCustomerAdmin frmCustAdmin;

        private int intCustomerID;
        private string strMode = "";

        //Set up List of ControlInfo objects, lsControlInfo, as follows:
        //  Order in list establishes Indexes for tabbing, implemented by SetTabIndex() method
        //  ControlPropertyToBind: determine type of control, and property to set for value
        //  RecordFieldName identify what controls display record detail,
        //  HeaderText: not used
        //  Updated property establishes what controls User has modified
        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="txtEntryFee",ControlPropetyToBind="Text",
                RecordFieldName ="EntryFee"},
            new ControlInfo {ControlID="txtPerDiem",ControlPropetyToBind="Text",
                RecordFieldName ="PerDiem"},
            new ControlInfo {ControlID="txtGraceDays",ControlPropetyToBind="Text",
                RecordFieldName ="PerDiemGraceDays"},
            new ControlInfo {ControlID="cboRateType",ControlPropetyToBind="SelectedValue",
                RecordFieldName ="RateType"},
            new ControlInfo {ControlID="txtStartDate",ControlPropetyToBind="Text",
                RecordFieldName ="StartDate"},
            new ControlInfo {ControlID="txtEndDate",ControlPropetyToBind="Text",
                RecordFieldName ="EndDate"},
             new ControlInfo {ControlID="txtCreationDate", RecordFieldName="CreationDate",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCreatedBy", RecordFieldName="CreatedBy",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtUpdatedDate", RecordFieldName="UpdatedDate",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtUpdatedBy", RecordFieldName="UpdatedBy",
                ControlPropetyToBind="Text"}
        };

        public frmRateAdmin(string strStatus,int intCustID, string strCustName, DataRow drow,
            frmCustomerAdmin frm)
        {
            try
            {
                InitializeComponent();

                Formops.SetFormBackground(this);
                Globalitems.SetControlHeight(this);
                Formops.SetTabIndex(this, lsControls);
                FillCombos();

                lblStatus.Text = strStatus;
                intCustomerID = intCustID;
                txtCustomer.Text = strCustName;
                drowtomodify = drow;
                frmCustAdmin = frm;

                if (strStatus.Contains("Modify"))
                {
                    FillDetail();

                    txtStartDate.Text = Globalitems.FormatDatetime(txtStartDate.Text);
                    txtEndDate.Text = Globalitems.FormatDatetime(txtEndDate.Text);
                    txtCreationDate.Text = Globalitems.FormatDatetime(txtCreationDate.Text);
                    txtUpdatedBy.Text = Globalitems.strUserName;
                    txtUpdatedDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");
                    strMode = "MODIFY";
                }
                else
                {
                    txtCreatedBy.Text = Globalitems.strUserName;
                    txtCreationDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");
                    strMode = "NEW";
                    txtEntryFee.Text = "0.00";
                    txtPerDiem.Text = "0.00";
                    txtGraceDays.Text = "0";
                }

                //Use Global DATE_TOOLTIP for Start/End date controls
                tipDate.SetToolTip(txtStartDate, Globalitems.DATE_TOOLTIP);
                tipDate.SetToolTip(txtEndDate, Globalitems.DATE_TOOLTIP);

                txtEntryFee.Focus();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmRateAdmin", ex.Message);
            }
        }

        private void FillCombos()
        {
            string strFilter;

            // Fill cboRatetype
            strFilter = "CodeType = 'RateType'";
            Globalitems.FillComboboxFromCodeTable(strFilter, cboRateType, false, true);
        }

        private void FillDetail()
        {
            try
            {Formops.SetDetailRecord(drowtomodify,this, lsControls);}

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillDetail", ex.Message);
            }
        }

        private void PerformSaveRecord()
        {
            try
            {
                DateTime datNow = DateTime.Now;
                DateTime datPrevDay;
                DataView dv;
                string strDatNow = datNow.ToString();
                string strFilter;

                if (ValidRecord())
                {
                    //Update drowtomodify
                    drowtomodify["CustomerID"] = intCustomerID;
                    drowtomodify["EntryFee"] = txtEntryFee.Text;
                    drowtomodify["PerDiem"] = txtPerDiem.Text;
                    drowtomodify["PerDiemGraceDays"] = txtGraceDays.Text;
                    drowtomodify["StartDate"] = txtStartDate.Text;
                    drowtomodify["RateType"] =
                         (cboRateType.SelectedItem as ComboboxItem).cboValue;

                    if (txtEndDate.Text.Trim().Length > 0)
                        drowtomodify["EndDate"] = Convert.ToDateTime(txtEndDate.Text);

                    // If strMode is NEW need to updated any previous record
                    if (strMode == "NEW")
                    {
                        drowtomodify["AutoportExportRatesID"] = 0;
                        drowtomodify["CreationDate"] = datNow;
                        drowtomodify["CreatedBy"] = Globalitems.strUserName;

                        strFilter = "StartDate <> '" + txtStartDate.Text +
                            "' AND EndDate IS NULL AND RateType = '" +
                            (cboRateType.SelectedItem as ComboboxItem).cboValue +
                            "'";
                        dv = new DataView(frmCustAdmin.dtNewRates, strFilter, "CustomerID",
                            DataViewRowState.CurrentRows);
                        if (dv.Count > 0)
                        {
                            datPrevDay = Convert.ToDateTime(txtStartDate.Text);
                            datPrevDay = datPrevDay.AddDays(-1);

                            dv[0]["UpdatedDate"] = datNow;
                            dv[0]["UpdatedBy"] = Globalitems.strUserName;
                            dv[0]["EndDate"] = datPrevDay;
                        }
                        
                    }
                    else
                    {
                        drowtomodify["UpdatedDate"] = DateTime.Now;
                        drowtomodify["UpdatedBy"] = Globalitems.strUserName;
                    }

                    //Let code in frmCustAdmin know rates changed
                    frmCustAdmin.blnRatesChanged = true;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }   
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord", ex.Message);
            }
        }

        private bool ValidRecord()
        {
            if (strMode == "MODIFY")
            {
                //Use linq to find all updated controls
                var changedlist = lsControls.Where(ctrlinfo => ctrlinfo.Updated == true).ToList();
                if (changedlist.Count == 0)
                {
                    MessageBox.Show("You have not changed anything for this Rate.\r\n" +
                       "There is nothing to update", "NO CHANGES MADE",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            
            //Check for Rate Type
            if (cboRateType.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a Rate Type.", "MISSING RATE TYPE",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboRateType.Focus();
                return false;
            }

            //Check that Start Date is not blank
            if (txtStartDate.Text.Trim().Length == 0)
            {
                MessageBox.Show("You must enter a Start Date.", "MISSING START DATE",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStartDate.Focus();
                return false;
            }

            //Check that Start Date is not after End Date, if End Date != blank
            if (txtEndDate.Text.Trim().Length > 0 && 
                Convert.ToDateTime(txtStartDate.Text) > Convert.ToDateTime(txtEndDate.Text))
            {
                MessageBox.Show("The Start Date cannot be later than the End Date.", "INCORRECT START DATE",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStartDate.Focus();
                return false;
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PerformSaveRecord();
        }

        private void txtGraceDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only allow digits & backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void txtEntryFee_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only allow digits, backspace, and decimal point, '.' 
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) &&
                e.KeyChar != '.') e.Handled = true;

            // Only allow 1 decimal point ('.')
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1) e.Handled = true;
        }

        private void txtPerDiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only allow digits, backspace, and decimal point, '.' 
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) &&
                e.KeyChar != '.') e.Handled = true;

            // Only allow 1 decimal point ('.')
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1) e.Handled = true;
        }

        private void txtEntryFee_Leave(object sender, EventArgs e)
        {
            string strval = txtEntryFee.Text.Trim();

            //User cannot leave blank, set to default of 0.00
            if (strval.Length > 0)
            {
                Globalitems.FormatTwoDecimal(ref strval);
                txtEntryFee.Text = strval;
            }
            else
            {
                txtEntryFee.Text = "0.00";
            }
        }

        private void txtPerDiem_Leave(object sender, EventArgs e)
        {
            string strval = txtPerDiem.Text.Trim();

            //User cannot leave blank, set to default of 0.00
            if (strval.Length > 0)
            {
                Globalitems.FormatTwoDecimal(ref strval);
                txtPerDiem.Text = strval;
            }
            else
            {
                txtPerDiem.Text = "0.00";
            }
        }

        private void txtStartDate_KeyPress(object sender, KeyPressEventArgs e)
        {Globalitems.CheckDateKeyPress(e);}

        private void txtStartDate_Validating(object sender, CancelEventArgs e)
        {Globalitems.ValidateDate(txtStartDate, e);}

        private void txtGraceDays_Leave(object sender, EventArgs e)
        {
            //User cannot leave blank, set to default of 0
            if (txtGraceDays.Text.Trim().Length == 0) txtGraceDays.Text = "0";
        }

        private void txtEntryFee_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtEntryFee", lsControls);
        }

        private void txtPerDiem_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtPerDiem", lsControls);
        }

        private void txtGraceDays_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtGraceDays", lsControls);
        }

        private void cboRateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboRateType", lsControls);
        }

        private void txtStartDate_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtStartDate", lsControls);
        }

        private void txtEndDate_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtEndDate", lsControls);
        }

        private void frmRateAdmin_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
