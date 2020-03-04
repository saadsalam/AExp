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
    public partial class frmDamageCodes : Form
    {
        private const string CURRENTMODULE = "frmDamageCodes";

        private bool blnCheckForNote = true;
        private frmVehDetail frmDetail;
        private int intVehicleID;
        private string strVIN;
        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="txtVIN",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboInspectionType",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtInspectionDate",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode1",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode2",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode3",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode4",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode5",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode6",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode7",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode8",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode9",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode10",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtVIN_edit",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboInspectionType_edit",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtInspectionDate_edit",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCurrentCode",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDamageCode_edit",ControlPropetyToBind="Text"}
        };

        public frmDamageCodes(frmVehDetail frm, bool blnPreviousRecords,int intVehID, string strVINFromDetail)
        {
            InitializeComponent();

            frmDetail = frm;

            intVehicleID = intVehID;
            strVIN = strVINFromDetail;

            tipDate.SetToolTip(txtInspectionDate, Globalitems.DATE_TOOLTIP);

            if (!blnPreviousRecords)
            {
                blnCheckForNote = false;
                pnlNote.Visible = false;
                this.Width = 375;
            }

            frm.blnNewDamageCodeInfo = false;
        }

        private void frmDamageCodes_Load(object sender, EventArgs e)
        {
            List<string> lsExcludes = new List<string>
            {
                {"txtNote"}
            };

            try
            {
                this.Text = "Export - Add Damage Code";

                Formops.SetFormBackground(this);
                Globalitems.SetControlHeight(this,lsExcludes);
                FillCombos();

                txtVIN.Text = strVIN;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmDamageCodes_Load", ex.Message);
            }
        }

        private void ClearForm()
        {
            try
            {
                Formops.ClearSetup(this, lsControls);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearForm", ex.Message);
            }
        }

        private void FillCombos()
        {
            ComboboxItem cboitem;
            string strFilter;

            try
            {
                strFilter = "CodeType='InspectionType' AND Code <> '' ";

                Globalitems.FillComboboxFromCodeTable(strFilter, cboInspectionType,
                       true, false);

                //Replace 'All; in 1st item with <select>
                cboitem = (ComboboxItem)cboInspectionType.Items[0];
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboInspectionType.Items[0] = cboitem;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void KeyPressTextbox(TextBox txtbox, KeyPressEventArgs e)
        {
            if (!Globalitems.ValidDateKeyStroke(e.KeyChar)) e.Handled = true;
        }

        private void PerformSave()
        {
            DataRow drow;
            DateTime datCreationDate = DateTime.Now;
            int intVehInspectionID = 1;
            int intDamageDetailID = 1;
            List<string> lsDamageCodes = new List<string>();

            try
            {
                if (ValidRecord(ref lsDamageCodes))
                {
                    //If any recs in frmDetail.blnNewDamageCodeInfo, get the highest InspectionID
                    //  and increment for the current set of damage codes
                    if (frmDetail.dtNewDamageCodes.Rows.Count > 0)
                    {
                        intVehInspectionID = 
                            Convert.ToInt32(frmDetail.dtNewDamageCodes.Compute("max([AEVehicleInspectionID])", string.Empty));
                        intVehInspectionID += 1;
                    }

                    //Loop to add a row for each DamageCode in frmDetail.dtNewDamageCodes
                    for (int iCode = 0; iCode < lsDamageCodes.Count; iCode++)
                    {
                        //Create a new row for frmDetail.dtNewDamageCodes
                        drow = frmDetail.dtNewDamageCodes.NewRow();

                        //Add Inspection row info
                        drow["AutoportExportVehiclesID"] = intVehicleID;
                        drow["AEVehicleInspectionID"] = intVehInspectionID;
                        drow["DamageCodeCount"] = lsDamageCodes.Count;
                        drow["InspectionDate"] = Convert.ToDateTime(txtInspectionDate.Text);
                        drow["InspectionType"] =  Convert.ToInt32((cboInspectionType.SelectedItem as ComboboxItem).cboValue);
                        drow["InspectionType_desc"] = (cboInspectionType.SelectedItem as ComboboxItem).cboText;
                        drow["InspectedBy"] = Globalitems.strUserName;
                        drow["CreationDate"] = datCreationDate;
                        if (txtNote.Text.Trim().Length > 0)
                        {
                            drow["Note"] = "VIEW";
                            drow["FullNote"] = txtNote.Text.Trim();
                        }
                        else
                        {
                            drow["Note"] = "";
                            drow["FullNote"] = "";
                        }

                        //Add the Damage Detail info
                        drow["AEVehicleDamageDetailID"] = intDamageDetailID;
                        drow["DamageCode"] = lsDamageCodes[iCode];
                        drow["DamageDescription"] = Globalitems.GetDamageCodeDescription(lsDamageCodes[iCode]);

                        //Add the row to frmDetail.dtNewDamageCodes
                        frmDetail.dtNewDamageCodes.Rows.Add(drow);

                        intDamageDetailID += 1;
                    }

                    //Let frmVehDetail know there is new DamageCode info added to dtNewDamageCodes
                    frmDetail.blnNewDamageCodeInfo = true;
                    this.Close();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSave", ex.Message);
            }
        }

        private DataTable CreateDamageCodeTable()
        {
            DataColumn col;
            DataTable dt = new DataTable();

            try
            {
                col = new DataColumn("AEVehicleDamageDetailID");
                col.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(col);

                col = new DataColumn("AEDamageClaimID");
                col.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(col);

                col = new DataColumn("AEVehicleInspectionID");
                col.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(col);

                col = new DataColumn("ClaimNumber");
                col.DataType = System.Type.GetType("System.String");
                dt.Columns.Add(col);

                col = new DataColumn("AutoportExportVehiclesID");
                col.DataType = System.Type.GetType("System.Int32");
                dt.Columns.Add(col);

                col = new DataColumn("DamageCode");
                col.DataType = System.Type.GetType("System.String");
                dt.Columns.Add(col);

                col = new DataColumn("DamageDescription");
                col.DataType = System.Type.GetType("System.String");
                dt.Columns.Add(col);

                col = new DataColumn("CreationDate");
                col.DataType = System.Type.GetType("System.DateTime");
                dt.Columns.Add(col);

                col = new DataColumn("CreatedBy");
                col.DataType = System.Type.GetType("System.String");
                dt.Columns.Add(col);

                col = new DataColumn("UpdatedDate");
                col.DataType = System.Type.GetType("System.DateTime");
                dt.Columns.Add(col);

                col = new DataColumn("UpdatedBy");
                col.DataType = System.Type.GetType("System.String");
                dt.Columns.Add(col);

                return dt;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CreateDamageCodeTable", ex.Message);
                return null;
            }
        }

        private bool ValidRecord(ref List<string> lsDamageCodes)
        {
            Control[] ctrls;
            DateTime datval; 
            int intlength;
            int inttotal = 0;
            string strDamageCode;
            string strval;
            TextBox txtDamageCode;
            

            try
            {
                //Ck Inspection type
                if (cboInspectionType.SelectedIndex < 1)
                {
                    MessageBox.Show("Please select the Inspection Type", "MISSING INSPECTION TYPE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboInspectionType.Focus();
                    return false;
                }

                //Ck Inspection Date
                if (txtInspectionDate.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter the Inspection Date", "MISSING INSPECTION DATE",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtInspectionDate.Focus();
                    return false;
                }

                //Ck that Inspection Date is not after today
                datval = Convert.ToDateTime(txtInspectionDate.Text);
                if (datval > DateTime.Today)
                {
                    MessageBox.Show("You cannot enter an Inspection Date later than today",
                        "INVALID INSPECTION DATE", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtInspectionDate.Focus();
                    txtInspectionDate.SelectionStart = 0;
                    txtInspectionDate.SelectionLength = txtInspectionDate.Text.Length;
                    return false;
                }

                //Ck Damage Codes
                //Get the length of TxtDamageCode1-10.
                for (int i=1; i<11; i++)
                {
                    intlength = 0;

                    ctrls = this.Controls.Find("txtDamageCode" + i,true);
                    if (ctrls.Count() > 0)
                    {
                        txtDamageCode = (TextBox) ctrls[0];

                        if (txtDamageCode.Text.Trim().Length != 0)
                        {
                            strDamageCode = txtDamageCode.Text.Trim();

                            //Ck that Damage Code is 5 chars
                            intlength = strDamageCode.Length;
                            if (intlength > 0 && intlength < 5)
                            {
                                MessageBox.Show("All Damage Codes must be 5 numbers.",
                                    "INVALID DAMAGE CODE",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtDamageCode.Focus();
                                txtDamageCode.SelectionStart = 0;
                                txtDamageCode.SelectionLength = txtDamageCode.Text.Length;
                                return false;
                            }

                            //1st 2 digits, Location, cannot be: 
                            //  32 / 41 / 43 / 46 / 47 / 51 / 60 / 62 / 87 / 88
                            strval = strDamageCode;
                            if (strval.StartsWith("00") ||
                                strval.StartsWith("32") || strval.StartsWith("41") ||
                                strval.StartsWith("43") || strval.StartsWith("46") ||
                                strval.StartsWith("47") || strval.StartsWith("51") ||
                                strval.StartsWith("60") || strval.StartsWith("62") ||
                                strval.StartsWith("87") || strval.StartsWith("88"))
                            {
                                MessageBox.Show("Damage Code Location (1st 2 digits) " +
                                    "cannot begin with " +
                                    txtDamageCode.Text.Trim().Substring(0, 2) + ".",
                                   "INVALID DAMAGE CODE",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtDamageCode.Focus();
                                txtDamageCode.SelectionStart = 0;
                                txtDamageCode.SelectionLength = txtDamageCode.Text.Length;
                                return false;
                            }

                            //Digits 3-4, Type, must be 01-14, 18-25, 29-30, 34, 36-38
                            //  Cannot be 15 / 16 / 17 / 26 / 27 / 28 / 31 / 32 / 33 / 35
                            strval = strDamageCode.Substring(2, 2);
                            if (strval.StartsWith("00") || string.Compare(strval, "38") > 0)
                            {
                                MessageBox.Show("Damage Code Type (2nd 2 digits) " +
                                    "cannot be 00 or greater than 38.",
                                   "INVALID DAMAGE CODE",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtDamageCode.Focus();
                                txtDamageCode.SelectionStart = 0;
                                txtDamageCode.SelectionLength = txtDamageCode.Text.Length;
                                return false;
                            }

                            if (strval.StartsWith("15") || strval.StartsWith("16") ||
                                strval.StartsWith("17") || strval.StartsWith("26") ||
                                strval.StartsWith("27") || strval.StartsWith("28") ||
                                strval.StartsWith("31") || strval.StartsWith("32") ||
                                strval.StartsWith("33") || strval.StartsWith("35"))
                            {
                                MessageBox.Show("Damage Code Type (2nd 2 digits) " +
                                    "cannot be " + strval + ".",
                                   "INVALID DAMAGE CODE",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtDamageCode.Focus();
                                txtDamageCode.SelectionStart = 0;
                                txtDamageCode.SelectionLength = txtDamageCode.Text.Length;
                                return false;
                            }

                                //Severity, Digit 5, must be 1-6
                                strval = strDamageCode.Substring(4, 1);
                            if (strval == "0" || string.Compare(strval, "6") > 0)
                            {
                                MessageBox.Show("Damage Code Severity (last digit) " +
                                    "cannot be 0 or greater than 6.",
                                   "INVALID DAMAGE CODE",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtDamageCode.Focus();
                                txtDamageCode.SelectionStart = 0;
                                txtDamageCode.SelectionLength = txtDamageCode.Text.Length;
                                return false;
                            }

                            //Valid Damage Code, add to lsDamageCodes & increment DamageCodeCount
                            lsDamageCodes.Add(strDamageCode);
                        }

                            inttotal += intlength;
                    }
                }

                if (inttotal == 0)
                {
                    MessageBox.Show("Please enter at least one new Damage code.",
                        "NO DAMAGE CODES ENTERED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDamageCode1.Focus();
                    return false;
                }

                if (blnCheckForNote)
                {
                    if (txtNote.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Please enter the Reason for creating new Damage Codes.",
                       "MISSING REASON FOR NEW DAMAGE CODES",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtNote.Focus();
                        return false;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidRecord", ex.Message);
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
            PerformSave();
        }

        private void txtInspectionDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            { KeyPressTextbox(txtInspectionDate, e); }
        }

        private void txtInspectionDate_Validating(object sender, CancelEventArgs e)
        {
            ValidateTextbox(txtInspectionDate, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtDamageCode1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Formops.NumericKeyPress(txtDamageCode1, e);
        }

        private void txtDamageCode10_KeyPress(object sender, KeyPressEventArgs e)
        {
            Formops.NumericKeyPress(txtDamageCode10, e);
        }

        private void txtDamageCode2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Formops.NumericKeyPress(txtDamageCode2, e);
        }

        private void txtDamageCode3_KeyPress(object sender, KeyPressEventArgs e)
        {
            Formops.NumericKeyPress(txtDamageCode3, e);
        }

        private void txtDamageCode4_KeyPress(object sender, KeyPressEventArgs e)
        {
            Formops.NumericKeyPress(txtDamageCode4, e);
        }

        private void txtDamageCode5_KeyPress(object sender, KeyPressEventArgs e)
        {
            Formops.NumericKeyPress(txtDamageCode5, e);
        }

        private void txtDamageCode6_KeyPress(object sender, KeyPressEventArgs e)
        {
            Formops.NumericKeyPress(txtDamageCode6, e);
        }

        private void txtDamageCode7_KeyPress(object sender, KeyPressEventArgs e)
        {
            Formops.NumericKeyPress(txtDamageCode7, e);
        }

        private void txtDamageCode8_KeyPress(object sender, KeyPressEventArgs e)
        {
            Formops.NumericKeyPress(txtDamageCode8, e);
        }

        private void txtDamageCode9_KeyPress(object sender, KeyPressEventArgs e)
        {
            Formops.NumericKeyPress(txtDamageCode9, e);
        }

        private void frmDamageCodes_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
