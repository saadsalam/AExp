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
    public partial class frmAddEditAddlCriteria : Form
    {
        private const string CURRENTMODULE = "frmAddEditAddlCriteria";

        private bool blnResettingContols = false;
        private AdditionalCriteriaItem objAdditionalCriteria;
        private ComboBox cboCust;
        private frmVehSearch frm;

        //Use the HeaderText property of each ControlInfo object to identify 
        //  the corresponding AddlCriteriaItem property to store the info, 
        //RecordFieldName is set so the Formops.ResetControls method can be used
        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="txtYear",ControlPropetyToBind="Text",HeaderText="Year",RecordFieldName="none"},
            new ControlInfo {ControlID="txtMake",ControlPropetyToBind="Text",HeaderText="Make",RecordFieldName="none"},
            new ControlInfo {ControlID="txtModel",ControlPropetyToBind="Text",HeaderText="Model",RecordFieldName="none"},
            new ControlInfo {ControlID="txtInvFrom",ControlPropetyToBind="Text",HeaderText="InvDateFrom",RecordFieldName="none"},
            new ControlInfo {ControlID="txtInvTo",ControlPropetyToBind="Text",HeaderText="InvDateTo",RecordFieldName="none"},
            new ControlInfo {ControlID="ckInvBlank",ControlPropetyToBind="Checked",HeaderText="InvIsBlank",RecordFieldName="none"},
            new ControlInfo {ControlID="txtRcvdFrom",ControlPropetyToBind="Text",HeaderText="RcvdDateFrom",RecordFieldName="none"},
            new ControlInfo {ControlID="txtRcvdTo",ControlPropetyToBind="Text",HeaderText="RcvdDateTo",RecordFieldName="none"},
            new ControlInfo {ControlID="ckRcvdBlank",ControlPropetyToBind="Checked",HeaderText="RcvdIsBlank",RecordFieldName="none"},
            new ControlInfo {ControlID="txtRcvdExFrom",ControlPropetyToBind="Text",HeaderText="RcvdExDateFrom",RecordFieldName="none"},
            new ControlInfo {ControlID="txtRcvdExTo",ControlPropetyToBind="Text",HeaderText="RcvdExDateTo",RecordFieldName="none"},
            new ControlInfo {ControlID="ckRcvdExBlank",ControlPropetyToBind="Checked",HeaderText="RcvdExIsBlank",RecordFieldName="none"},
            new ControlInfo {ControlID="txtSubFrom",ControlPropetyToBind="Text",HeaderText="SubDateFrom",RecordFieldName="none"},
            new ControlInfo {ControlID="txtSubTo",ControlPropetyToBind="Text",HeaderText="SubDateTo",RecordFieldName="none"},
            new ControlInfo {ControlID="ckSubBlank",ControlPropetyToBind="Checked",HeaderText="SubIsBlank",RecordFieldName="none"},
            new ControlInfo {ControlID="txtCustomsExFrom",ControlPropetyToBind="Text",HeaderText="CustomsExDateFrom",RecordFieldName="none"},
            new ControlInfo {ControlID="txtCustomsExTo",ControlPropetyToBind="Text",HeaderText="CustomsExDateTo",RecordFieldName="none"},
            new ControlInfo {ControlID="ckCustomsExBlank",ControlPropetyToBind="Checked",HeaderText="CustomsExIsBlank",RecordFieldName="none"},
            new ControlInfo {ControlID="txtCustApprovedFrom",ControlPropetyToBind="Text",HeaderText="CustomsApprovedDateFrom",RecordFieldName="none"},
            new ControlInfo {ControlID="txtCustApprovedTo",ControlPropetyToBind="Text",HeaderText="CustomsApprovedDateTo",RecordFieldName="none"},
            new ControlInfo {ControlID="ckCustomsApprovedBlank",ControlPropetyToBind="Checked",HeaderText="CustomsApprovedIsBlank",RecordFieldName="none"},
            new ControlInfo {ControlID="txtShippedFrom",ControlPropetyToBind="Text",HeaderText="ShipDateFrom",RecordFieldName="none"},
            new ControlInfo {ControlID="txtShippedTo",ControlPropetyToBind="Text",HeaderText="ShipDateTo",RecordFieldName="none"},
            new ControlInfo {ControlID="ckShippedBlank",ControlPropetyToBind="Checked",HeaderText="ShipDateIsBlank",RecordFieldName="none"},
            new ControlInfo {ControlID="txtPhysicalFrom",ControlPropetyToBind="Text",HeaderText="PhysicalDateFrom",RecordFieldName="none"},
            new ControlInfo {ControlID="txtPhysicalTo",ControlPropetyToBind="Text",HeaderText="PhysicalDateTo",RecordFieldName="none"},
            new ControlInfo {ControlID="ckPhysicalBlank",ControlPropetyToBind="Checked",HeaderText="PhysicalIsBlank",RecordFieldName="none"},
            new ControlInfo {ControlID="txtInvNumber",ControlPropetyToBind="Text",HeaderText="InvNumber",RecordFieldName="none"},
            new ControlInfo {ControlID="ckSizeClass",ControlPropetyToBind="Checked",HeaderText="SizeClass",RecordFieldName="none"},
            new ControlInfo {ControlID="ckNonRunners",ControlPropetyToBind="Checked",HeaderText="NonRunners",RecordFieldName="none"},
            new ControlInfo {ControlID="ckMechExceptions",ControlPropetyToBind="Checked",HeaderText="MechExceptions",RecordFieldName="none"},
            new ControlInfo {ControlID="txtVINs",ControlPropetyToBind="Text",HeaderText="MultiVins",RecordFieldName="none"},
            new ControlInfo {ControlID="cboBillTo",ControlPropetyToBind="SelectedValue",HeaderText="BillTo",RecordFieldName="none"},
            new ControlInfo {ControlID="btnSave"},
            new ControlInfo {ControlID="btnCancel"},
            new ControlInfo {ControlID="btnClear"}
        };

        public frmAddEditAddlCriteria(AdditionalCriteriaItem objAddlCriteria,frmVehSearch frmveh,
            ComboBox cbo)
        {

            InitializeComponent();

            objAdditionalCriteria = objAddlCriteria;
            frm = frmveh;
            cboCust = cbo;

            List<string> lsExcludes = new List<string>() { "txtVINs" };

            Formops.SetFormBackground(this);
            Globalitems.SetControlHeight(this, lsExcludes);
            Formops.SetTabIndex(this, lsControls);

            //Use Global DATE_TOOLTIP for Start/End date controls, turn off per John
            tipDate.SetToolTip(txtInvFrom, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtInvTo, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtRcvdFrom, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtRcvdTo, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtSubFrom, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtSubTo, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtCustomsExFrom, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtCustomsExTo, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtShippedFrom, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtShippedTo, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtPhysicalFrom, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtPhysicalTo, Globalitems.DATE_TOOLTIP);

            FillCombos();
        }

        private void CancelSetup()
        {
            //Can close the form if no previous add'l criteia
            objAdditionalCriteria.RerunSearch = false;
            if (!objAdditionalCriteria.blnAddlCriteria)
            {
                this.Close();
                return;
            }

            //Reset control values
            RestoreFromVehSearch();
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {CancelSetup();}

        private void FillCombos()
        {
            ComboboxItem cboitem;

            cboBillTo.Items.Clear();

            //Add <select>, (Is Blank), (Not Blank) items
            cboitem = new ComboboxItem();
            cboitem.cboText = "<select>";
            cboitem.cboValue = "select";
            cboBillTo.Items.Add(cboitem);

            cboitem = new ComboboxItem();
            cboitem.cboText = "(Is Blank)";
            cboitem.cboValue = "blank";
            cboBillTo.Items.Add(cboitem);

            cboitem = new ComboboxItem();
            cboitem.cboText = "(Not Blank)";
            cboitem.cboValue = "notblank";
            cboBillTo.Items.Add(cboitem);

            //Add the remaining items from cboCust that was passed in
            foreach (ComboboxItem cboit in cboCust.Items)
                if (cboit.cboValue != "All") cboBillTo.Items.Add(cboit);

            cboBillTo.SelectedIndex = 0;
            cboBillTo.DisplayMember = "cboText";
            cboBillTo.ValueMember = "cboValue";
        }

        private void RestoreFromVehSearch()
        {
            CheckBox ckBox;
            Control[] ctrls;
            string strProperty;
            string strval;

            blnResettingContols = true;

            //  For this form, all textboxes & ckboxes
            var changedlist = lsControls.Where(ctrlinfo => (ctrlinfo.Updated == true)).ToList();

            //Loop through each ctrlinfo object in changedlist and get the corresponding property, 
            // HeaderText in ctrlinfo item, in frmVeh.objAddlCriteria
            foreach (ControlInfo ctrlinfo in changedlist)
            {
                //Get the corresponding property in objAddlCriteria
                strProperty = ctrlinfo.HeaderText;
                
                    //Place the control into the array ctrls, s/b only one
                    ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                    //Set control to blank/unchecked. Update the control if frmVeh.objAddlCriteria has a value 
                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            ctrls[0].Text = "";

                        //Restore value if property is not null
                        if (typeof(AdditionalCriteriaItem).GetProperty(strProperty).GetValue(objAdditionalCriteria) != null)
                        {
                            strval = typeof(AdditionalCriteriaItem).GetProperty(strProperty).GetValue(objAdditionalCriteria).ToString();
                            ctrls[0].Text = strval;
                        }

                            break;
                        case "Checked":
                            //Cast control to CheckBox
                            ckBox = (CheckBox)ctrls[0];
                            ckBox.Checked = false;

                            //Set ckbox to checked if Property is true
                            if (Convert.ToBoolean(typeof(AdditionalCriteriaItem).GetProperty(strProperty).GetValue(objAdditionalCriteria)))
                                ckBox.Checked = true;
                            break;
                    }
                
            }   //foreach ctrlinfo

            //Set Updated -> false for all controls
            Formops.ResetControls(this, lsControls);

            blnResettingContols = false;
        }

        private bool ValidCriteria()
        {
            //Use linq to find all updated controls
            var changedlist = lsControls.Where(ctrlinfo => ctrlinfo.Updated == true).ToList();
            if (changedlist.Count == 0)
            {
                MessageBox.Show("You have not entered or modified any Additional Criteria.\r\n" +
                   "Please enter/modify the Additional Criteria or click Cancel", "NO ADDITIONAL CRITERIA",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //Ck that no From Dates are after To Date

            //Inv. Dates
            if (txtInvFrom.Text.Trim().Length > 0 && txtInvTo.Text.Trim().Length > 0)
            {
                if (Convert.ToDateTime(txtInvFrom.Text) > Convert.ToDateTime(txtInvTo.Text))
                {
                    MessageBox.Show("The Invoice From Date cannot be later than the Invoice To Date.", "INCORRECT INVOICE DATES",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtInvFrom.Focus();
                    return false;
                }
            }

            //Rcvd Dates
            if (txtRcvdFrom.Text.Trim().Length > 0 && txtRcvdTo.Text.Trim().Length > 0)
            {
                if (Convert.ToDateTime(txtRcvdFrom.Text) > Convert.ToDateTime(txtRcvdTo.Text))
                {
                    MessageBox.Show("The Received From Date cannot be later than the Received To Date.", "INCORRECT RECEIVED DATES",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRcvdFrom.Focus();
                    return false;
                }
            }

            //Rcvd Ex Dates
            if (txtRcvdExFrom.Text.Trim().Length > 0 && txtRcvdExTo.Text.Trim().Length > 0)
            {
                if (Convert.ToDateTime(txtRcvdExFrom.Text) > Convert.ToDateTime(txtRcvdExTo.Text))
                {
                    MessageBox.Show("The Received Exception From Date cannot be later than the Received Exception To Date.", "INCORRECT RECEIVED EXCEPTION DATES",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRcvdExFrom.Focus();
                    return false;
                }
            }

            //Sub'd To Customs Dates
            if (txtSubFrom.Text.Trim().Length > 0 && txtSubTo.Text.Trim().Length > 0)
            {
                if (Convert.ToDateTime(txtSubFrom.Text) > Convert.ToDateTime(txtSubTo.Text))
                {
                    MessageBox.Show("The Submitted To Customs From Date cannot be later than the Submitted To Customs To Date.", 
                        "INCORRECT SUBMITTED TO CUSTOMS DATES",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSubFrom.Focus();
                    return false;
                }
            }

            //Customs Ex Dates
            if (txtCustomsExFrom.Text.Trim().Length > 0 && txtCustomsExTo.Text.Trim().Length > 0)
            {
                if (Convert.ToDateTime(txtCustomsExFrom.Text) > Convert.ToDateTime(txtCustomsExTo.Text))
                {
                    MessageBox.Show("The Customs Exception From Date cannot be later than the Customs Exception To Date.", "INCORRECT CUSTOMS EXCEPTION DATES",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRcvdFrom.Focus();
                    return false;
                }
            }

            //Customs Approved Dates
            if (txtCustApprovedFrom.Text.Trim().Length > 0 && txtCustApprovedTo.Text.Trim().Length > 0)
            {
                if (Convert.ToDateTime(txtCustApprovedFrom.Text) > 
                    Convert.ToDateTime(txtCustApprovedTo.Text))
                {
                    MessageBox.Show("The Customs Approved From Date cannot be later than the Customs Approved To Date.", "INCORRECT CUSTOMS APPROVED DATES",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRcvdFrom.Focus();
                    return false;
                }
            }

            //Shipped Dates
            if (txtShippedFrom.Text.Trim().Length > 0 && txtShippedTo.Text.Trim().Length > 0)
            {
                if (Convert.ToDateTime(txtShippedFrom.Text) > Convert.ToDateTime(txtShippedTo.Text))
                {
                    MessageBox.Show("The Shipped From Date cannot be later than the Shipped To Date.", "INCORRECT SHIPPED DATES",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtShippedFrom.Focus();
                    return false;
                }
            }

            //Last Physical Dates
            if (txtPhysicalFrom.Text.Trim().Length > 0 && txtPhysicalTo.Text.Trim().Length > 0)
            {
                if (Convert.ToDateTime(txtPhysicalFrom.Text) > Convert.ToDateTime(txtPhysicalTo.Text))
                {
                    MessageBox.Show("The Last Physical From Date cannot be later than the Last Physical To Date.", "INCORRECT LAST PHYSICAL DATES",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPhysicalFrom.Focus();
                    return false;
                }
            }

            return true;
        }

        public void ClearForm()
        {Formops.ClearSetup(this, lsControls);}

        private void KeyPressTextbox(TextBox txtbox, KeyPressEventArgs e)
        {
            if (!Globalitems.ValidDateKeyStroke(e.KeyChar)) e.Handled = true;
        }

        private void PerformSaveCriteria()
        {
            String strID;

            try
            {
                CheckBox ckBox;
                ComboBox cbo;
                Control[] ctrls;
                DateTime datval;
                string strProperty;
                string strval;

                if (ValidCriteria())
                {

                    objAdditionalCriteria.blnAddlCriteria = false;
                    objAdditionalCriteria.Initialize();

                    //1. Retrieve a list of updated controls
                    //Use linq to get a list of updated controls, requiring an update to 
                    //the AutoportExporRates table

                    //  For this form, textboxes, ckboxes
                    //var changedlist = lsControls.Where(ctrlinfo => (ctrlinfo.Updated == true)).ToList();

                    //Loop through each ctrlinfo object in changedlist and store in corresponding property, HeaderText in ctrlinfo item, 
                    //  in frmVeh.objAddlCriteria
                    foreach (ControlInfo ctrlinfo in lsControls)
                    {
                        strID = ctrlinfo.ControlID;

                        //Get the corresponding property in objAddlCriteria
                        strProperty = ctrlinfo.HeaderText;

                        //Place the control into the array ctrls, s/b only one
                        ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                        //Update the property in frmVeh.objAddlCriteria, 
                        switch (ctrlinfo.ControlPropetyToBind)
                        {
                            case "Text":
                                if (ctrls[0].Text.Trim().Length > 0)
                                {
                                    strval = ctrls[0].Text.Trim();
                                    
                                    //Add 1 Day if Control ID ends in To, because WHERE clause 
                                    // is date < To value
                                    if (ctrlinfo.ControlID.EndsWith("To"))
                                    {
                                        datval = Convert.ToDateTime(strval).AddDays(1);
                                        strval = datval.ToString("M/d/yyyy");
                                    }
                                    typeof(AdditionalCriteriaItem).GetProperty(strProperty).SetValue(objAdditionalCriteria,strval);
                                    objAdditionalCriteria.blnAddlCriteria= true;
                                }
                                break;
                            case "Checked":
                                //Cast control to CheckBox
                                ckBox = (CheckBox)ctrls[0];

                                if (ckBox.Checked)
                                {
                                    typeof(AdditionalCriteriaItem).GetProperty(strProperty).SetValue(objAdditionalCriteria, true);
                                    objAdditionalCriteria.blnAddlCriteria = true;
                                }
                                break;
                            case "SelectedValue":
                                //Cast control to ComboBox
                                cbo = (ComboBox)ctrls[0];
                                strval = (cbo.SelectedItem as ComboboxItem).cboValue;
                                if (strval != "select")
                                {
                                    typeof(AdditionalCriteriaItem).GetProperty(strProperty).SetValue(objAdditionalCriteria, strval);
                                    objAdditionalCriteria.blnAddlCriteria = true;
                                }
                                break;
                        }
                    }   //foreach ctrlinfo

                    //Replace new line (\r\n) in MultiVINS with comma (',') separator
                    if (txtVINs.Text.Trim().Length > 0)
                    {
                        strval = txtVINs.Text.Replace("\r\n", ",");
                        objAdditionalCriteria.MultiVins = strval;
                    }

                    blnResettingContols = false;

                    //Reset updated -> false
                    Formops.ResetControls(this, lsControls);
                    
                    //Make sure frmVehSearch is still open
                    if (Application.OpenForms.OfType<frmVehSearch>().Count() == 0)
                        MessageBox.Show("The Vehicle Locator form does not show as open. Please make sure it is displayed.");
                    else
                    {
                        this.Hide();
                        frm.Show();
                    }
                        
                }
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSaveCriteria", ex.Message);
            }
        }

        private void txtInvFrom_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtInvFrom, e); }

        private void txtInvTo_Validating(object sender, CancelEventArgs e)
        {Globalitems.ValidateDate(txtInvTo, e); }

        private void txtRcvdFrom_Validating(object sender, CancelEventArgs e)
        {Globalitems.ValidateDate(txtRcvdFrom, e); }

        private void txtRcvdTo_Validating(object sender, CancelEventArgs e)
        {Globalitems.ValidateDate(txtRcvdTo, e); }

        private void txtSubFrom_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtSubFrom, e); }

        private void txtSubTo_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtSubTo, e); }        

        private void txtShippedFrom_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtShippedFrom, e); }

        private void txtShippedTo_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtShippedTo, e); }

        private void txtPhysicalFrom_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtPhysicalFrom, e); }

        private void txtPhysicalTo_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtPhysicalTo, e); }

        private void txtRcvdExFrom_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtRcvdExFrom, e); }

        private void txtRcvdExTo_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtRcvdExTo, e); }

        private void txtCustApprovedFrom_Validating(object sender, CancelEventArgs e)
        {Globalitems.ValidateDate(txtCustApprovedFrom, e);}

        private void txtCustApprovedTo_Validating(object sender, CancelEventArgs e)
        {Globalitems.ValidateDate(txtCustApprovedTo, e);}

        private void txtCustomsExTo_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtCustomsExTo, e); }

        private void txtInvFrom_KeyPress(object sender, KeyPressEventArgs e)
        {KeyPressTextbox(txtInvFrom, e);}

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtYear, e); }

        private void txtInvTo_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtInvTo, e); }

        private void txtRcvdFrom_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtRcvdFrom, e); }

        private void txtRcvdTo_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtRcvdTo, e); }

        private void txtSubFrom_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtSubFrom, e); }

        private void txtSubTo_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtSubTo, e); }        

        private void txtShippedFrom_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtShippedFrom, e); }

        private void txtShippedTo_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtShippedTo, e); }

        private void txtPhysicalFrom_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtPhysicalFrom, e); }

        private void txtPhysicalTo_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtPhysicalTo, e); }

        private void txtRcvdExFrom_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtRcvdExFrom, e); }

        private void txtRcvdExTo_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtRcvdExTo, e); }

        private void txtCustApprovedFrom_KeyPress(object sender, KeyPressEventArgs e)
        {KeyPressTextbox(txtCustApprovedFrom, e);}

        private void txtCustApprovedTo_KeyPress(object sender, KeyPressEventArgs e)
        {KeyPressTextbox(txtCustApprovedTo, e);}

        private void txtCustomsExFrom_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtCustomsExFrom, e); }

        private void txtInvFrom_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols)
            {
                ckInvBlank.Checked = false;
                Formops.ChangeControlUpdatedStatus("txtInvFrom", lsControls);
            }
        }
                
        private void txtYear_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtYear", lsControls);}

        private void txtMake_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtMake", lsControls);}

        private void txtModel_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtModel", lsControls);}

        private void txtInvTo_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols)
            {
                ckInvBlank.Checked = false;
                Formops.ChangeControlUpdatedStatus("txtInvTo", lsControls);
            }
        }
                
        private void txtRcvdFrom_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols)
            {
                ckRcvdBlank.Checked = false;
                Formops.ChangeControlUpdatedStatus("txtRcvdFrom", lsControls);
            }
        }
                
        private void txtRcvdTo_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols)
            {
                ckRcvdBlank.Checked = false;
                Formops.ChangeControlUpdatedStatus("txtRcvdTo", lsControls);
            }
        }

        private void txtSubFrom_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols)
            {
                ckSubBlank.Checked = false;
                Formops.ChangeControlUpdatedStatus("txtSubFrom", lsControls);
            }
        }

        private void txtSubTo_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols)
            {
                ckSubBlank.Checked = false;
                Formops.ChangeControlUpdatedStatus("txtSubTo", lsControls);
            }
        }


        private void txtShippedFrom_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtShippedFrom", lsControls);
        }

        private void txtShippedTo_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtShippedTo", lsControls);}

        private void txtPhysicalFrom_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols)
            {
                ckPhysicalBlank.Checked = false;
                Formops.ChangeControlUpdatedStatus("txtPhysicalFrom", lsControls);
            }
        }

        private void txtRcvdExFrom_TextChanged(object sender, EventArgs e)
        {
            {
                if (!blnResettingContols)
                {
                    ckRcvdExBlank.Checked = false;
                    Formops.ChangeControlUpdatedStatus("txtRcvdExFrom", lsControls);
                }
            }
        }

        private void txtPhysicalTo_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols)
            {
                ckPhysicalBlank.Checked = false;
                Formops.ChangeControlUpdatedStatus("txtPhysicalTo", lsControls);
            }
        }

        private void txtInvNumber_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtPhysicalTo", lsControls);}

        private void txtCustApprovedFrom_TextChanged(object sender, EventArgs e)
        {if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtCustApprovedFrom", lsControls);}

        private void ckSizeClass_CheckedChanged(object sender, EventArgs e)
        { if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckSizeClass", lsControls); }

        private void ckNonRunners_CheckedChanged(object sender, EventArgs e)
        { if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckNonRunners", lsControls); }

        private void ckMechExceptions_CheckedChanged(object sender, EventArgs e)
        { if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckMechExceptions", lsControls); }

        private void txtVINs_TextChanged(object sender, EventArgs e)
        { if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtVINs", lsControls); }

        private void txtRcvdExTo_TextChanged(object sender, EventArgs e)
        {if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtRcvdExTo", lsControls);}

        private void txtCustApprovedTo_TextChanged(object sender, EventArgs e)
        {if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtCustApprovedTo", lsControls);}

        private void txtCustomsExFrom_TextChanged(object sender, EventArgs e)
        {if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtCustomsExFrom", lsControls);}

        private void txtCustomsExTo_TextChanged(object sender, EventArgs e)
        {if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("txtCustomsExTo", lsControls);}
        private void ckInvBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckInvBlank", lsControls);
            if (ckInvBlank.Checked)
            {
                txtInvFrom.Text = "";
                txtInvTo.Text = "";
            }
        }

        

        private void ckRcvdBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckRcvdBlank", lsControls);
            if (ckRcvdBlank.Checked)
            {
                txtRcvdFrom.Text = "";
                txtRcvdTo.Text = "";
            }
        }

        private void ckSubBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckSubBlank", lsControls);
            if (ckSubBlank.Checked)
            {
                txtSubFrom.Text = "";
                txtSubTo.Text = "";
            }
        }

        private void ckCustomsExBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckCustomsExBlank", lsControls);
            if (ckCustomsExBlank.Checked)
            {
                txtCustomsExFrom.Text = "";
                txtCustomsExTo.Text = "";
            }
        }

        private void ckPhysicalBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckPhysicalBlank", lsControls);
            if (ckPhysicalBlank.Checked)
            {
                txtPhysicalFrom.Text = "";
                txtPhysicalTo.Text = "";
            }
        }

        private void ckRcvdExBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckRcvdExBlank", lsControls);
            if (ckRcvdExBlank.Checked)
            {
                txtRcvdExFrom.Text = "";
                txtRcvdExTo.Text = "";
            }
        }

        private void ckCustomsApprovedBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckCustomsApprovedBlank",
                lsControls);
            if (ckCustomsApprovedBlank.Checked)
            {
                txtCustApprovedFrom.Text = "";
                txtCustApprovedTo.Text = "";
            }
        }

        private void ckShippedBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("ckShippedBlank",
                lsControls);
            if (ckShippedBlank.Checked)
            {
                txtShippedFrom.Text = "";
                txtShippedTo.Text = "";
            }
        }

        private void txtYear_Validating(object sender, CancelEventArgs e)
        {
            //if value is < 1000, add to 2000. E.g. 17 -> 2017
            int intval;

            if (txtYear.Text.Trim().Length > 0)
            {
                intval = Convert.ToInt32(txtYear.Text);
                if (intval < 1000)
                {
                    intval += 2000;
                    txtYear.Text = intval.ToString();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            objAdditionalCriteria.RerunSearch = false;
            PerformSaveCriteria();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {ClearForm();}

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }

        private void txtCustomsExFrom_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtCustomsExFrom, e); }

        private void txtCustomsExTo_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtCustomsExTo, e); }

        private void cboBillTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!blnResettingContols) Formops.ChangeControlUpdatedStatus("cboBillTo", lsControls);
        }

        private void frmAddEditAddlCriteria_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            objAdditionalCriteria.RerunSearch = true;
            PerformSaveCriteria();}        
    }
}
