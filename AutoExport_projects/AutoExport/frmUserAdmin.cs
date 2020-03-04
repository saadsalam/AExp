using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmUserAdmin : Form
    {
        private const string CURRENTMODULE = "frmUserAdmin";

        private BindingSource bs1 = new BindingSource();
        private List<User> lsUsers = new List<User>();
        private DataTable dtUsers = new DataTable();
        private string strMode;

        //Set up List of ControlInfo objects, lsControlInfo, as follows:
        //  Order in list establishes Indexes for tabbing, implemented by SetTabIndex() method
        //  ControlPropertyToBind identifies what controls are initialized 
        //  RecordFieldName identify what controls display record detail,
        //  HeaderText sets column header to use for Export to csv file
        //  Updated property establishes what controls User has modified

        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="txtUname",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtFname",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtLname",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboRole",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="cboStatus",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="btnSearch"},
            new ControlInfo {ControlID="btnClear"},
            new ControlInfo {ControlID="btnMenu"},
            new ControlInfo {RecordFieldName="UserFullName",HeaderText="Name"},
            new ControlInfo {ControlID="txtUserID", ControlPropetyToBind="Text",
                RecordFieldName ="UserID",HeaderText="User ID" },
            new ControlInfo {ControlID="txtUname_record", ControlPropetyToBind="Text",
                RecordFieldName ="UserCode",HeaderText="User Name"},
            new ControlInfo {ControlID="txtFname_record", ControlPropetyToBind="Text",
                RecordFieldName ="FirstName",HeaderText = "First Name"},
            new ControlInfo {ControlID="txtLname_record", ControlPropetyToBind="Text",
                RecordFieldName ="LastName", HeaderText = "Last Name"},
            new ControlInfo {ControlID="txtPassword", ControlPropetyToBind="Text",
                RecordFieldName ="Password", HeaderText = "Password"},
            new ControlInfo {ControlID="txtPassword_confirm", ControlPropetyToBind="Text",
                RecordFieldName ="Password"},
            new ControlInfo {ControlID="cboStatus_record", ControlPropetyToBind="SelectedValue",
                RecordFieldName ="RecordStatus", HeaderText = "Status"},
            new ControlInfo {ControlID="ckAdmin", ControlPropetyToBind="Checked",
                RecordFieldName ="Admin" },
            new ControlInfo {ControlID="ckYard", ControlPropetyToBind="Checked",
                RecordFieldName ="Yard" },
            new ControlInfo {ControlID="ckBilling", ControlPropetyToBind="Checked",
                RecordFieldName ="Billing" },
            new ControlInfo {ControlID="ckHide", ControlPropetyToBind="Checked",
                RecordFieldName ="Hide" },
            new ControlInfo {ControlID="txtPhone", ControlPropetyToBind="Text",
                RecordFieldName ="Phone",},
            new ControlInfo {ControlID="txtExtension", ControlPropetyToBind="Text",
                RecordFieldName ="PhoneExtension"},
            new ControlInfo {ControlID="txtCell", ControlPropetyToBind="Text",
                RecordFieldName ="CellPhone"},
            new ControlInfo {ControlID="txtFax", ControlPropetyToBind="Text",
                RecordFieldName ="FaxNumber"},
            new ControlInfo {ControlID="txtEmpl", ControlPropetyToBind="Text",
                RecordFieldName ="EmployeeNumber"},
            new ControlInfo {ControlID="txtPortPassID", ControlPropetyToBind="Text",
                RecordFieldName ="PortPassIDNumber"},
            new ControlInfo {ControlID="txtEmail", ControlPropetyToBind="Text",
                RecordFieldName ="EmailAddress"},
            new ControlInfo {ControlID="txtCreationDate", ControlPropetyToBind="Text",
                RecordFieldName ="CreationDate", ReadOnly=true},
            new ControlInfo {ControlID="txtUpdatedDate", ControlPropetyToBind="Text",
                RecordFieldName ="UpdatedDate", ReadOnly=true},
            new ControlInfo {ControlID="txtCreatedBy", ControlPropetyToBind="Text",
                RecordFieldName ="CreatedBy", ReadOnly=true},
            new ControlInfo {ControlID="txtUpdatedBy", ControlPropetyToBind="Text",
                RecordFieldName ="UpdatedBy", ReadOnly=true}
        };

        public frmUserAdmin()
        {
            try
            {
                InitializeComponent();
                strMode = "READ";
                lblRecords.Text = "";
                btnExport.Enabled = false;

                //Hide txtUserID from User
                txtUserID.BorderStyle = BorderStyle.None;

                txtUserID.AutoSize = false;

                Globalitems.SetControlHeight(this);
                FillCombos();

                // Assign methods to the recbuttons public event variables
                recbuttons.CancelRecord += btnCancel_Clicked;
                recbuttons.MovePrev += btnPrev_Clicked;
                recbuttons.MoveNext += btnNext_Clicked;
                recbuttons.DeleteRecord += btnDelete_Clicked;
                recbuttons.NewRecord += btnNew_Clicked;
                recbuttons.ModifyRecord += btnModify_Clicked;
                recbuttons.SaveRecord += btnSave_Clicked;

                dgResults.AutoGenerateColumns = false;
                DisplayMode();

                txtUname.Focus();
                Formops.SetTabIndex(this,lsControls);

                lblPwd_confirm.Visible = false;
                txtPassword_confirm.Visible = false;
                lblRecords.Text = "";
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmUserAdmin (Initializer)", ex.Message);
            }
        }

        private void CancelSetup()
        {
            try
            {
                DataRow dr;
                int intBSPosition = 0;

                //1. Set Status label
                lblStatus.Text = "Read only";

                //2. Enable Search/Results panels
                pnlSearch.Enabled = true;
                pnlResults.Enabled = true;
               
                //3. Display other open forms
                Globalitems.DisplayOtherForms(this, true);

                //4. Turn on ReadOnly on form's controls
                AdjustReadOnlyStatus(true);

                //5. Clear Record Controls
                Formops.ClearRecordData(this, lsControls);

                //6. Set Mode to READ, recbuttons with no records to display, & recbuttons to READONLY
                strMode = "READ";
                recbuttons.blnRecordsToDisplay = false;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);

                //7. Enable Menu, Search, Clear buttons
                btnMenu.Enabled = true;
                btnSearch.Enabled = true;
                btnClear.Enabled = true;

                //8. If dgResults has 1 or more rows: enable btnExport,reset navbuttons, reset record details 
                if (dgResults.RowCount > 0)
                {
                    btnExport.Enabled = true;
                    recbuttons.blnRecordsToDisplay = true;
                    recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                    intBSPosition = bs1.Position;

                    //Retrieve record details for bs1.position
                    dr = GetDetailRow(lsUsers[intBSPosition]);

                    Formops.SetDetailRecord(dr, this, lsControls);
                    Globalitems.SetNavButtons(recbuttons, bs1);
                }

                //9. Handle Form unique controls
                lblPwd_confirm.Visible = false;
                txtPassword_confirm.Visible = false;

                //Set date time formats
                txtCreationDate.Text = Globalitems.FormatDatetime(txtCreationDate.Text);
                txtUpdatedDate.Text = Globalitems.FormatDatetime(txtUpdatedDate.Text);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CancelSetup", ex.Message);
            }
        }

        private void btnCancel_Clicked()
        {
            try { CancelSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnCancel_Clicked", ex.Message); }
        }

        private void btnSave_Clicked()
        {
            try { PerformSaveRecord(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnSave_Clicked", ex.Message); }
        }

        private string SQLForModifiedRecord()
        {
            ComboBox cboBox;
            Control[] ctrls;
            string strSQL;
            string strval;

            try
            {
                //1. Retrieve a list of updated controls
                //Use linq to get a list of updated controls, requiring an update to the Users table
                //  For this form, textboxes and comboboxes
                var changedlist = lsControls.Where(ctrlinfo => (ctrlinfo.Updated == true) &&
                    (ctrlinfo.ControlPropetyToBind == "Text" ||
                    ctrlinfo.ControlPropetyToBind == "SelectedValue")).ToList();

                if (changedlist.Count == 0) return "";
                    
                strSQL = "Update Users SET ";

                    foreach (ControlInfo ctrlinfo in changedlist)
                    {
                        strSQL += ctrlinfo.RecordFieldName + " = ";

                        //Place the control into the array ctrls, s/b only one
                        ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                        switch (ctrlinfo.ControlPropetyToBind)
                        {
                            case "Text":
                                // Use HandleQuote to replace ' in text to '' for SQL
                                strval = Globalitems.HandleSingleQuoteForSQL(ctrls[0].Text);

                                //Check if MaskedTextBox. On this form, it means it's a tel#
                                //  Use CompressedTelNumber to save only digits
                                if (ctrls[0].GetType().Name == "MaskedTextBox")
                                {
                                    strval = CompressedTelNumber(strval);
                                }
                                if (strval.Length == 0)
                                { strSQL += "NULL"; }
                                else
                                { strSQL += "'" + strval + "'"; }
                                break;
                            case "SelectedValue":
                                //Cast control to ComboBox
                                cboBox = (ComboBox)ctrls[0];
                                strSQL += "'" + cboBox.SelectedValue + "'";
                                break;
                        }
                        strSQL += ",";

                        // Update PasswordUpdatedDate if txtPassword is modified
                        if (ctrlinfo.ControlID == "txtPassword")
                        {
                            strSQL += "PasswordUpdatedDate = '" + txtUpdatedDate.Text + "',";
                        }
                    }

                    //Remove extra ',' at end of strSQL
                    strSQL = strSQL.Substring(0, strSQL.Length - 1);

                    // Add WHERE clause
                    strSQL += " WHERE userID = " + txtUserID.Text;
                    return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForModifiedRecord", ex.Message);
                return "";
            }
        }

        private string SQLForNewRecord()
        {
            string strSQL;

            try
            {
                //1a. Construct SQL INSERT statement for Users table
                strSQL = "INSERT INTO Users (UserCode,FirstName,LastName," +
                    "Password,PIN,Phone,PhoneExtension,CellPhone," +
                    "FaxNumber,EmailAddress,LabelXOffset,LabelYOffset,IMEI," +
                    "LastConnection,datsVersion,RecordStatus,CreationDate,CreatedBy," +
                    "UpdatedDate,UpdatedBy,EmployeeNumber,PortPassIDNumber,Department," +
                    "StraightTimeRate,PieceRateRate,PDIRate,FlatBenefitPayRate," +
                    "AlternateEmailAddress,PasswordUpdatedDate) " +
                    "VALUES ('" + Globalitems.HandleSingleQuoteForSQL(txtUname_record.Text) + "'," +
                    "'" + Globalitems.HandleSingleQuoteForSQL(txtFname_record.Text) + "'," +
                    "'" + Globalitems.HandleSingleQuoteForSQL(txtLname_record.Text) + "'," +
                    "'" + Globalitems.HandleSingleQuoteForSQL(txtPassword.Text) + "'," +
                    "NULL,";

                //Add optional fields or NULL
                if (txtPhone.MaskCompleted)
                {
                    strSQL += "'" + CompressedTelNumber(txtPhone.Text) + "',";
                }
                else
                {
                    strSQL += "NULL,";
                }

                if (!string.IsNullOrWhiteSpace(txtExtension.Text))
                {
                    strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(txtExtension.Text) + "',";
                }
                else
                {
                    strSQL += "NULL,";
                }

                if (txtCell.MaskCompleted)
                {
                    strSQL += "'" + CompressedTelNumber(txtCell.Text) + "',";
                }
                else
                {
                    strSQL += "NULL,";
                }

                if (txtFax.MaskCompleted)
                {
                    strSQL += "'" + CompressedTelNumber(txtFax.Text) + "',";
                }
                else
                {
                    strSQL += "NULL,";
                }

                if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(txtEmail.Text) + "',";
                }
                else
                {
                    strSQL += "NULL,";
                }

                // NULL values for LabelXOffset, LabelYOffset, IMEI, LastConnection, datsVersion
                strSQL += "NULL,NULL,NULL,NULL,NULL,";

                strSQL += "'" + cboStatus_record.SelectedValue + "',";   //RecordStatus

                strSQL += "'" + txtCreationDate.Text + "',";   //CreationDate 
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(txtCreatedBy.Text) + "',";    //Createdby
                strSQL += "NULL,NULL,";  //UpdatedDate, UpdatedBy

                //EmployeeNumber
                if (!string.IsNullOrWhiteSpace(txtEmpl.Text))
                {
                    strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(txtEmpl.Text) + "',";
                }
                else
                {
                    strSQL += "NULL,";
                }

                //PortPassIDNumber
                if (!string.IsNullOrWhiteSpace(txtPortPassID.Text))
                {
                    strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(txtPortPassID.Text) + "',";
                }
                else
                {
                    strSQL += "NULL,";
                }

                strSQL += "NULL,NULL,NULL,NULL,";   //Department, StraightTimeRate,PieceRateRate,PDIRate
                strSQL += "NULL,NULL,";             //FlatBenefitPayRate, AlternateEmailAddress
                strSQL += "'" + txtCreationDate.Text + "')";

                return strSQL;
                
            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForNewRecord", ex.Message);
                return "";
            }
        }

        private void PerformSaveRecord()
        // For new User, insert a new record into Users table, and new record(s) into UserRole table.
        // For modified User, update Users table aand Insert/Delete recs from UserRole table.
        // Since User changes are infrequent and only impact a single user, 
        //  don't need to treat updates to the Users & UserRole tables
        //  as an atomic transaction
        {
            CheckBox ckBox;
            Control[] ctrls;

            try
            {
                DataSet ds;
                int intUserID;
                string strSQL;

                if (ValidRecord())
                {
                    if (strMode == "NEW")
                    {
                        strSQL = SQLForNewRecord();

                        DataOps.PerformDBOperation(strSQL);

                        //Retrieve UserID created in the Users table
                        strSQL = "SELECT UserID from Users WHERE UserCode = '" + txtUname_record.Text + "'";
                        ds = DataOps.GetDataset_with_SQL(strSQL);
                        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord",
                                "No UserID returned after new record inserted into Users table.");
                            return;
                        }
                        intUserID = Convert.ToInt32(ds.Tables[0].Rows[0]["UserID"]);

                        //1b. Insert role records into UserRole table
                        var changedlist = lsControls.Where(ctrlinfo => (ctrlinfo.Updated == true) &&
                           (ctrlinfo.ControlPropetyToBind == "Checked")).ToList();
                        if (changedlist.Count > 0)
                        {
                            foreach (ControlInfo ctrlinfo in changedlist)
                            {
                                //Place the control into the array ctrls, s/b only one
                                ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                                //Cast control to CheckBox
                                ckBox = (CheckBox)ctrls[0];

                                //Only Insert a record if ckBox is checked
                                if (ckBox.Checked)
                                {
                                    strSQL = "INSERT INTO UserRole (UserID,RoleName,CreationDate,CreatedBy) " +
                                        "VALUES (" + intUserID + ",";

                                    //Determine RoleName based on RecordField
                                    switch (ctrlinfo.RecordFieldName)
                                    {
                                        case "Admin":
                                            strSQL += "'Administrator',";
                                            break;

                                        case "Yard":
                                            strSQL += "'YardOperations',";
                                            break;

                                        case "Billing":
                                            {
                                                strSQL += "'Billing',";
                                                break;
                                            }
                                        case "Hide":
                                            {
                                                strSQL += "'HideRates',";
                                                break;
                                            }
                                    }

                                    strSQL += "'" + txtCreationDate.Text + "'," +
                                        "'" + txtCreatedBy.Text + "')";
                                    DataOps.PerformDBOperation(strSQL);
                                }   // if ckBox.Checked
                            }   // end for each
                        }   //if ChangedList.count > 0

                        //2. Inform User of success
                        MessageBox.Show("The new user has been added to the DB.\nIf the new user " +
                            "does't appear in the table, check your search criteria.", "NEW USER CREATED",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    //strMode = MODIFY
                    if (strMode == "MODIFY")
                    {
                        strSQL = SQLForModifiedRecord();

                        if (strSQL.Length > 0) DataOps.PerformDBOperation(strSQL);

                        //Update UserRole table if any controls updated
                        //Use linq to get a list of updated controls, requiring an update to the UserRole table
                        //  For this form, checkboxes that are updated
                        var changedlist = lsControls.Where(ctrlinfo => (ctrlinfo.Updated == true) &&
                        (ctrlinfo.ControlPropetyToBind == "Checked")).ToList();
                        if (changedlist.Count > 0)
                        {
                            foreach (ControlInfo ctrlinfo in changedlist)
                            {
                                //Place the control into the array ctrls, s/b only one
                                ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                                //Cast control to CheckBox
                                ckBox = (CheckBox)ctrls[0];

                                //If ckBox is checked, need to Insert a Record
                                if (ckBox.Checked)
                                {
                                    strSQL = "INSERT INTO UserRole (UserID,RoleName,CreationDate,CreatedBy) " +
                                        "VALUES ('" + txtUserID.Text + "',";

                                    //Determine RoleName based on RecordField
                                        switch (ctrlinfo.RecordFieldName)
                                    {
                                        case "Admin":
                                            strSQL += "'Administrator',";
                                            break;

                                        case "Yard":
                                            strSQL += "'YardOperations',";
                                            break;

                                        case "Billing":
                                            {
                                                strSQL += "'Billing',";
                                                break;
                                            }
                                        case "Hide":
                                            {
                                                strSQL += "'HideRates',";
                                                break;
                                            }
                                    }

                                    strSQL += "GetDate(),"  + 
                                        "'" + Globalitems.strUserName + "')";
                                }
                                // ckBox was unchecked need to Delete the record
                                else
                                {
                                    strSQL = "DELETE UserRole WHERE UserID = " +
                                        txtUserID.Text + " AND RoleName = ";

                                    //Determine RoleName based on RecordField
                                    switch (ctrlinfo.RecordFieldName)
                                    {
                                        case "Admin":
                                            strSQL += "'Administrator'";
                                            break;

                                        case "Yard":
                                            strSQL += "'YardOperations'";
                                            break;

                                        case "Billing":
                                            {
                                                strSQL += "'Billing'";
                                                break;
                                            }

                                        case "Hide":
                                            strSQL += "'HideRates'";
                                            break;
                                    }
                                }

                                DataOps.PerformDBOperation(strSQL);
                            }   // end foreach
                        }   //if changedlist.count > 0

                        MessageBox.Show("The User's information has been modified in the DB.",
                         "USER INFO MODIFIED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }   // end if strMode =  MODIFY

                    //3. Display other forms
                    Globalitems.DisplayOtherForms(this, true);

                    //4. Set Mode to READ
                    strMode = "READ";

                    //5. Enable Search/Results panels
                    pnlSearch.Enabled = true;
                    pnlResults.Enabled = true;

                    //6. Set Status label to Read only
                    lblStatus.Text = "Read only";

                    //6. Perform new 
                    PerformSearch();

                    btnSearch.Enabled = true;
                    btnClear.Enabled = true;
                                    
                }   // end if ValidRecord
            }   // end try

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord", ex.Message);
            }   
        }

        private bool ValidRecord()
        {
            try
            {
                DataSet ds;
                string strSQL;
                string strval ;

                // Ck for UserName if New record
                if (strMode == "NEW")
                {
                    if (string.IsNullOrWhiteSpace(txtUname_record.Text))
                    {
                        MessageBox.Show("Please enter a User Name.",
                            "Missing User Name", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtUname_record.Focus();
                        return false;
                    }

                    // Ck UserName is at least 3 chars
                    if (txtUname_record.Text.Trim().Length < 3)
                    {
                        MessageBox.Show("The User Name must be at least three characters.",
                            "User Name Too Short", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtUname_record.Focus();
                        return false;
                    }

                    txtUname_record.Text = txtUname_record.Text.Trim();

                    //Ck that UserName is not already in the Users table
                    // **** NEED TO CK ALL DB'S ONCE IMPLEMENTED  ****
                    strSQL = "SELECT COUNT(UserID) AS totrecs FROM Users WHERE RTRIM(UserCode) = " +
                        "'" + txtUname_record.Text + "';";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)  //Per the SQL s/b one row returned
                    {
                        Globalitems.HandleException("frmUserAdmin", "frmValidNewUser",
                            "No table or row returned in ds");
                        return false;
                    }

                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["totrecs"]) != 0)
                    {
                        MessageBox.Show("The User Name already exists.\r\n Please enter a new User Name.",
                            "User Name Exists", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtUname_record.Focus();
                        return false;
                    }
                }

                //Ck that at least one control has changed if mode is MODIFY
                if (strMode == "MODIFY")
                {
                    //Use linq to find all updated controls
                    var changedlist = lsControls.Where(ctrlinfo => ctrlinfo.Updated == true).ToList();
                    if (changedlist.Count == 0)
                    {
                        MessageBox.Show("You have not changed anything for this User.\r\n" +
                           "There is nothing to update", "NO CHANGES MADE",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                //Ck for First Name
                if (string.IsNullOrWhiteSpace(txtFname_record.Text))
                {
                    MessageBox.Show("Please enter a First Name.",
                        "Missing First Name", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtFname_record.Focus();
                    return false;
                }

                //Ck for Last Name
                if (string.IsNullOrWhiteSpace(txtLname_record.Text))
                {
                    MessageBox.Show("Please enter a Last Name.",
                        "Missing Last Name", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtLname_record.Focus();
                    return false;
                }

                //Ck for Password

                // Password entered?
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please enter a Password.",
                        "Missing Password", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtPassword.Focus();
                    return false;
                }

                txtPassword.Text = txtPassword.Text.Trim();

                //Confirm Password entered?
                if (string.IsNullOrWhiteSpace(txtPassword_confirm.Text))
                {
                    MessageBox.Show("Please enter the Confirm Password", "MISSING CONFIRM PASSWORD", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtPassword_confirm.Focus();
                    return false;
                }
                txtPassword_confirm.Text = txtPassword_confirm.Text.Trim();

                //Ck that Password & Confirm Password match
                if (txtPassword.Text != txtPassword_confirm.Text)
                {
                    MessageBox.Show("The Password and the Confirm Password do not match",
                        "PASSWORD MISMATCH", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtPassword_confirm.Focus();
                    return false;
                }

                //Use validpassword method to check agains password rules
                strval = Globalitems.validpassword(txtPassword.Text);

                if (strval != "OK")
                {
                    switch (strval)
                    {
                        case "SHORT":
                            MessageBox.Show("The new Password must be 4 or more characters.",
                                "PASSWORD TOO SHORT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPassword.Focus();
                            break;

                        case "DAI":
                            MessageBox.Show("The new Password cannot begin with DAI.",
                                "PASSWORD STARTS WITH DAI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPassword.Focus();
                            break;

                        case "REPEAT":
                            MessageBox.Show("The new Password cannot repeat the same character more than 3 times.",
                                "PASSWORD REPEATS CHARACTERS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPassword.Focus();
                            break;

                        case "SEQ":
                            MessageBox.Show("The new Password cannot use a numerical sequence (e.g. 1234).",
                                "PASSWORD USES NUMERICAL SEQUENCE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtPassword.Focus();
                            break;

                    }
                    return false;
                }

                //Make sure at least one role is checked
                if (!ckAdmin.Checked && !ckYard.Checked && !ckBilling.Checked && !ckHide.Checked)
                {
                    MessageBox.Show("You must assign at least one Role to the user.",
                                "NO ROLE ASSIGNED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //PortPassID#, can be blank. If not blank, min. 4 chars, and cannot already exist for other user
                if (!string.IsNullOrWhiteSpace(txtPortPassID.Text))
                {
                    if (txtPortPassID.Text.Trim().Length < 4)
                    {
                        MessageBox.Show("The Port Pass ID# must be at least 4 characters.",
                                "PORT PASS ID# TOO SHORT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    txtPortPassID.Text = txtPortPassID.Text.Trim();

                    //Make sure Port Pass ID# is not already used if new user. Must be
                    //  unique across all DB's
                    // ****** NEED TO UPDATE WHEN 4 DB's ARE IN PLACE ******
                    if (strMode == "NEW")
                    {
                        strSQL = "SELECT COUNT(UserID) AS totrecs from Users WHERE PortPassIDNumber='" +
                            txtPortPassID.Text + "'; ";
                        ds = DataOps.GetDataset_with_SQL(strSQL);
                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "ValidRecord",
                                "No table or no rows were returned from query on Users table.");
                            return false;
                        }

                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["totrecs"].ToString()) > 0)
                        {
                            MessageBox.Show("The Port Pass ID# is already in use. Please create a new ID#.",
                                "DUPLICATE PORT PASS ID#",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }

                //Ck that Phone is completed if not blank after removing masked items
                strval = CompressedTelNumber(txtPhone.Text);
                if (strval.Length > 0)
                {
                    if (!txtPhone.MaskCompleted)
                    {
                        MessageBox.Show("The Phone number is incomplete.",
                            "INCOMPLETE PHONE NO.", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtPhone.Focus();
                        return false;
                    }
                }

                //Ck that Cell is completed if not blank after removing masked items
                strval = CompressedTelNumber(txtCell.Text);
                if (strval.Length > 0)
                {
                    if (!txtCell.MaskCompleted)
                    {
                        MessageBox.Show("The Cell Phone number is incomplete.",
                            "INCOMPLETE CELL PHONE NO.", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtCell.Focus();
                        return false;
                    }
                }

                //Ck that Fax is completed if not blank after removing masked items
                strval = CompressedTelNumber(txtFax.Text);
                if (strval.Length > 0)
                {
                    if (!txtFax.MaskCompleted)
                    {
                        MessageBox.Show("The Fax number is incomplete.",
                            "INCOMPLETE FAX NO.", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtFax.Focus();
                        return false;
                    }
                }

                //Email address can be blank. If not blank, check if valid email address
                if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    if (!Globalitems.validemailaddress(txtEmail.Text.Trim()))
                    {
                        MessageBox.Show("The Email address is not valid.",
                                "INVALID EMAIL ADDRESS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidNewUser", ex.Message);
                return false;
            }
        }

        private string CompressedTelNumber(string strTel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strTel)) return "";

                //Remove the masked items for tel#'s
                strTel = strTel.Replace("(", "");
                strTel = strTel.Replace(")", "");
                strTel = strTel.Replace("-", "");
                strTel = strTel.Replace(" ", "");
                strTel = strTel.Trim();
                return strTel;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CompressedTelNumber", ex.Message);
                return "";
            }
        }

        private void ValidPassword()
        {
            /*
               "1) Must be at least 4 characters.\n" +
               "2) Cannot begin with DAI.\n" +
               "3) Cannot be a sequence of numbers, e.g. 1234.\n" +
               "4) Password is case sensitive.\n" +
               "5) Cannot be the same character repeat 4 times.";  */

            try
            {

            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidPassword", ex.Message);
            }
        }

        private void PerformMovePrevious()
        {
            try
            {
                bs1.MovePrevious();
                GetRecordDetail(lsUsers[bs1.Position].UserID);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMovePrevious", ex.Message);
            }
        }

        private void PerformDeleteRecord()
        {
            try
            {
                string strMessage = "Are you sure you want to delete this User?\n" + 
                    "You can also change the status to Inactive.\n" +
                    "Once deleted this User will be removed from the DB.";

                frmAreYouSure frmConfirm = new frmAreYouSure(strMessage);
                var result = frmConfirm.ShowDialog();
                string strSQL;

                if (result == DialogResult.OK)
                {
                    strSQL = "DELETE Users WHERE UserID = " + txtUserID.Text;
                    DataOps.PerformDBOperation(strSQL);

                    strSQL = "DELETE UserRole WHERE UserID = " + txtUserID.Text;
                    DataOps.PerformDBOperation(strSQL);

                    PerformSearch();

                    pnlSearch.Enabled = true;
                    pnlResults.Enabled = true;
                    lblStatus.Text = "Read only";

                    MessageBox.Show("The User is deleted.", "USER DELETED", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord", ex.Message);
            }
        }

        private void GetRecordDetail(int intUserID)
        {
            try
            {
                DataSet ds;
                DataRow dtRow;
                string strSQL;

                strSQL = Globalitems.strSavedSQL + " WHERE us.UserID = " + intUserID;
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GetRecorddetail",
                        "No record returned in Dataset");
                    return;
                }

                dtRow = ds.Tables[0].Rows[0];
                FillDetailRecord(dtRow);
                Globalitems.SetNavButtons(recbuttons,bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetRecordDetail", ex.Message);
                return;
            }
        }

        private void PerformMoveNext()
        {
            try
            {               
                bs1.MoveNext();
                GetRecordDetail(lsUsers[bs1.Position].UserID);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMoveNext", ex.Message);
            }
        }

        private void btnPrev_Clicked()
        {
            try { PerformMovePrevious(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnPrev_Clicked", ex.Message); }
        }

        private void btnNext_Clicked()
        {
            try { PerformMoveNext(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnNext_Clicked", ex.Message); }
        }

        private void ModifyRecordSetup()
        {
            try
            {
                //1. Set Mode
                strMode = "MODIFY";

                //2. Set Status label
                lblStatus.Text = "Modify current record";

                //3. Disable Search/Results panels & Menu btn
                pnlSearch.Enabled = false;
                pnlResults.Enabled = false;
                btnMenu.Enabled = false;
                
                //4. Hide other open forms
                Globalitems.DisplayOtherForms(this, false);
                
                //5. Turn off ReadOnly on form's controls
                AdjustReadOnlyStatus(false);

                //6. Reset form's controls to Updated = false
                Formops.ResetControls(this, lsControls);

                //7. Set recbuttons to Modify
                recbuttons.SetButtons(RecordButtons.ACTION_MODIFYRECORD);

                //8. Set Updated By/Date to new value
                txtUpdatedBy.Text = Globalitems.strUserName;
                txtUpdatedDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

                //9. Set focus on first control
                txtFname_record.Focus();

                //10. Handle Form unique controls
                lblPwd_confirm.Visible = true;
                txtPassword_confirm.Visible = true;
                txtPassword_confirm.Text = txtPassword.Text;
            }

            catch (Exception ex)
            {
               Globalitems.HandleException(CURRENTMODULE, "ModifyRecordSetup", ex.Message);
            }
        }

        private void NewRecordSetup()
        {
            try
            {
                //1. Set Mode
                strMode = "NEW";

                //2. Set Status label
                lblStatus.Text = "Add new record";

                //3.Disable Search/Results panels, & related buttons
                pnlSearch.Enabled = false;
                pnlResults.Enabled = false;
                btnMenu.Enabled = false;
                btnSearch.Enabled = false;
                btnClear.Enabled = false;
                btnExport.Enabled = false;
                
                //4. Hide other open forms
                Globalitems.DisplayOtherForms(this, false);

                //5. Turn off ReadOnly on form's controls
                AdjustReadOnlyStatus(false);

                //6. clear Record Controls
                Formops.ClearRecordData(this,lsControls);

                //7. Set recbuttons to New
                recbuttons.SetButtons(RecordButtons.ACTION_NEWRECORD);

                //8. Set Created By/Date 
                txtCreatedBy.Text = Globalitems.strUserName;
                txtCreationDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

                //9. Set focus
                txtUname_record.Focus();

                //10. Handle Form unique controls
                lblPwd_confirm.Visible = true;
                txtPassword_confirm.Visible = true;
                
                txtPortPassID.Text = DateTime.Now.ToString("MMddyyHHmmss");
                ckYard.Checked = true;
                cboStatus_record.SelectedValue = "Active";
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "NewRecordSetup", ex.Message);
            }
        }

        private void btnNew_Clicked()
        {
            try { NewRecordSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnNew_Clicked", ex.Message); }
        }

        private void btnDelete_Clicked()
        {
            try {PerformDeleteRecord();}
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnDelete_Clicked", ex.Message); }   
        }

        private void btnModify_Clicked()
        {
            try { ModifyRecordSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnModify_Clicked", ex.Message); }
        }

        private void FillCombos()
        {
            //Use FillComboboxFromCodeTable in Globalitems to fill comboboxes from dtCode
            try
            {
                string strFilter;

                // To retrieve Selected value from a combobox need to assign a datasource to the combobox

                strFilter = "CodeType = 'RecordStatus'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboStatus, true, false);
                Globalitems.FillComboboxFromCodeTable(strFilter, cboStatus_record, false, true);

                strFilter = "CodeType = 'UserRole'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboRole, true, false);
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void ClearGridView()
        {
            try
            {
                lblRecords.Text = "";
                btnExport.Enabled = false;

                lsUsers.Clear();
                dtUsers.Clear();
                bs1.Clear();

                // Binding dgResults to lsUsers after the Clear method, can lead to runtime error because
                //  the CurrencyManager pointing to the Current position in lsUsers, doesn't reset to -1
                dgResults.DataSource = dtUsers;

                recbuttons.blnRecordsToDisplay = false;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearGridView", ex.Message);
            }
        }

        private void ClearForm()
        {
            try
            {
                //1. Clear all items in lsControls
                Formops.ClearSetup(this, lsControls);

                //2. Clear Form unique grids
                ClearGridView();
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearSetup", ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try { ClearForm(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnClear_Click", ex.Message); }
        }

        private DataRow GetDetailRow(User objUser)
        {
            //Create a dtUsers DataRow based on the property values in objUser 
            try
            {
                DataRow dr = dtUsers.NewRow();
                PropertyInfo prop;

                //Use linq to get a list of controls where RecordFieldName != null and ControlID != null
                var recordlist = lsControls.Where(ctrlinfo =>
                    ctrlinfo.RecordFieldName != null && ctrlinfo.ControlID != null).ToList();
                foreach (ControlInfo ctrlinfo in recordlist)
                {
                    prop = typeof(User).GetProperty(ctrlinfo.RecordFieldName);
                    if (prop.GetValue(objUser) != null) dr[ctrlinfo.RecordFieldName] = prop.GetValue(objUser);
                }

                return dr;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetDetailRecord", ex.Message);
                return null;
            }
        }

        private void FillDetailRecord(DataRow dr)
        {
            try
            {
                //loop through lsControls to set record controls
                Formops.ClearRecordData(this,lsControls);
                Formops.SetDetailRecord(dr, this, lsControls);

                //Set date time formats
                txtCreationDate.Text = Globalitems.FormatDatetime(txtCreationDate.Text);
                txtUpdatedDate.Text = Globalitems.FormatDatetime(txtUpdatedDate.Text);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetDetailRecord", ex.Message);
            }
        }

        private void SetUpBindingSource()
        {
            try
            {

                //Use lsUsers so the binding source can be filtered without affecting the dg
                bs1.DataSource = lsUsers;
                bs1.Position = 0;

                AdjustReadOnlyStatus(true);
                Globalitems.SetNavButtons(recbuttons,bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetUpBindingSource", ex.Message);
            }
        }

        private void BindTextboxes_0(bool blnBind)
        {
            try
            {
                txtUserID.DataBindings.Clear();
                txtLname_record.DataBindings.Clear();
                txtFname_record.DataBindings.Clear();
                txtUname_record.DataBindings.Clear();
                txtPhone.DataBindings.Clear();
                txtExtension.DataBindings.Clear();
                txtCell.DataBindings.Clear();
                txtFax.DataBindings.Clear();
                txtEmail.DataBindings.Clear();
                txtPassword.DataBindings.Clear();
                txtEmpl.DataBindings.Clear();
                cboStatus_record.DataBindings.Clear();
                txtPortPassID.DataBindings.Clear();
                txtCreationDate.DataBindings.Clear();
                txtUpdatedDate.DataBindings.Clear();
                txtCreatedBy.DataBindings.Clear();
                txtUpdatedBy.DataBindings.Clear();
                ckAdmin.DataBindings.Clear();
                ckYard.DataBindings.Clear();
                ckBilling.DataBindings.Clear();

                if (blnBind)
                {
                    //Bind Controls to BindingSource
                    txtUserID.DataBindings.Add(new Binding("Text", bs1, "UserID", true));
                    txtLname_record.DataBindings.Add(new Binding("Text", bs1, "LastName", true));
                    txtFname_record.DataBindings.Add(new Binding("Text", bs1, "FirstName", true));
                    txtUname_record.DataBindings.Add(new Binding("Text", bs1, "UserCode", true));
                    txtPhone.DataBindings.Add(new Binding("Text", bs1, "Phone", true));
                    txtExtension.DataBindings.Add(new Binding("Text", bs1, "PhoneExtension", true));
                    txtCell.DataBindings.Add(new Binding("Text", bs1, "CellPhone", true));
                    txtFax.DataBindings.Add(new Binding("Text", bs1, "FaxNumber", true));
                    txtEmail.DataBindings.Add(new Binding("Text", bs1, "EmailAddress", true));
                    txtPassword.DataBindings.Add(new Binding("Text", bs1, "Password", true));
                    txtEmpl.DataBindings.Add(new Binding("Text", bs1, "EmployeeNumber", true));

                    cboStatus_record.DataBindings.Add(new Binding("SelectedValue", bs1, "RecordStatus", true));

                    txtPortPassID.DataBindings.Add(new Binding("Text", bs1, "PortPassIDNumber", true));
                    txtCreationDate.DataBindings.Add(new Binding("Text", bs1, "CreationDate", true));
                    txtUpdatedDate.DataBindings.Add(new Binding("Text", bs1, "UpdatedDate", true));
                    txtCreatedBy.DataBindings.Add(new Binding("Text", bs1, "CreatedBy", true));
                    txtUpdatedBy.DataBindings.Add(new Binding("Text", bs1, "UpdatedBy", true));
                    ckAdmin.DataBindings.Add(new Binding("Checked", bs1, "Admin", true));
                    ckYard.DataBindings.Add(new Binding("Checked", bs1, "AE", true));
                    ckBilling.DataBindings.Add(new Binding("Checked", bs1, "Billing", true));
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "BindTextboxes", ex.Message);
            }
        }       

        private void AdjustReadOnlyStatus(bool blnReadOnly)
        {
            try
            {
                Formops.SetReadOnlyStatus(this, lsControls, blnReadOnly, recbuttons);

                //Only display Required/Optional labels if not in READONLY mode
                lblOptional.Visible = !blnReadOnly;
                lblRequired.Visible = !blnReadOnly;

                //Cannot change Usernames already assigned
                if (strMode != "NEW") txtUname_record.ReadOnly = true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetReadOnlyStatus", ex.Message);
            }
        }

        private void PerformSearch()
        {
            DataSet ds;
            string strSQL;
            string strval;

            try
            {
                //1. Disable btnExport
                btnExport.Enabled = false;

                //2. Clear Results Gridview
                ClearGridView();

                string strWHERE = "";

                //3. Clear record data
                Formops.ClearRecordData(this,lsControls);

                //4. Set recbuttons to display = false
                recbuttons.blnRecordsToDisplay = false;

                //5. Retrieve data

                strSQL = "select us.UserID,us.UserCode," +
                    "Rtrim(us.FirstName) AS FirstName," +
                    "Rtrim(us.LastName)AS LastName," +
                    "us.Password, us.PIN," +
                    "us.Phone, us.PhoneExtension, us.CellPhone," +
                    "us.FaxNumber, us.EmailAddress," +
                    "us.LabelXOffset,us.LabelYOffset,us.IMEI," +
                    "us.LastConnection,us.datsVersion," +
                    "us.RecordStatus," +
                    "us.CreationDate, us.CreatedBy," +
                    "us.UpdatedDate, us.UpdatedBy," +
                    "us.EmployeeNumber,us.PortPassIDNumber, " +
                    "us.Department,us.StraightTimeRate," +
                    "us.PieceRateRate,us.PDIRate,us.FlatBenefitPayRate," +
                    "us.AlternateEmailAddress,us.PasswordUpdatedDate," +

                    // Additional columns not in Users table
                    "RTRIM(us.FirstName) + ' ' + RTRIM(us.LastName) AS UserFullName, " +
                    "CASE " +
                        "WHEN ur_Ad.UserID IS NULL THEN 0 " +
                        "ELSE 1 " +
                    "END AS Admin," +
                    "CASE " +
                        "WHEN ur_Yard.UserID IS NULL THEN 0 " +
                        "ELSE 1 " +
                    "END AS Yard," +
                    "CASE " +
                        "WHEN ur_B.UserID IS NULL THEN 0 " +
                        "ELSE 1 " +
                    "END AS Billing, " +
                    "CASE " +
                        "WHEN ur_Hide.UserID IS NULL THEN 0 " +
                        "ELSE 1 " +
                    "END AS Hide, " +
                    "NULL AS DBAction " +
                    "from Users us " +
                    "LEFT OUTER JOIN UserRole ur_Ad on us.UserID = ur_Ad.UserID AND " +
                        "ur_Ad.RoleName = 'Administrator' " +
                    "LEFT OUTER JOIN UserRole ur_Yard on us.UserID = ur_Yard.UserID AND " +
                        "ur_Yard.RoleName = 'YardOperations' " +
                    "LEFT OUTER JOIN UserRole ur_B on us.UserID = ur_B.UserID AND " +
                        "ur_B.RoleName = 'Billing' " +
                    "LEFT OUTER JOIN UserRole ur_Hide on us.UserID = ur_Hide.UserID AND " +
                        "ur_Hide.RoleName = 'HideRates' ";

                //Save basic query for nav buttons
                Globalitems.strSavedSQL = strSQL;

                //Add to WHERE clause if any names in text boxes
                if ((txtLname.Text.Trim() + txtFname.Text.Trim() + txtUname.Text.Trim()).Length != 0)
                {
                    if (txtLname.Text.Trim().Length > 0)
                    {
                        //Trim name and handle single quote ('). Use Lname & Fname entries in text box, with
                        // LIKE search
                        txtLname.Text = txtLname.Text.Trim();
                        strval = txtLname.Text;
                        strval = strval.Replace("'", "''");
                        strWHERE = "WHERE us.LastName LIKE '" + strval + "%' ";
                    }

                    if (txtFname.Text.Trim().Length > 0)
                    {
                        txtFname.Text = txtFname.Text.Trim();
                        strval = txtFname.Text;
                        strval = strval.Replace("'", "''");

                        if (strWHERE.Length > 0)
                        {
                            strWHERE += "AND ";
                        }
                        else
                        {
                            strWHERE = "WHERE ";
                        }
                        strWHERE += "us.FirstName LIKE '" + strval + "%' ";
                    }

                    // Like DATS original, User Code must be exact match
                    if (txtUname.Text.Trim().Length > 0)
                    {
                        txtUname.Text = txtUname.Text.Trim();
                        strval = txtUname.Text;
                        strval = strval.Replace("'", "''");

                        if (strWHERE.Length > 0)
                        {
                            strWHERE += "AND ";
                        }
                        else
                        {
                            strWHERE = "WHERE ";
                        }
                        strWHERE += "us.UserCode = '" + strval + "' ";
                    }
                }  // end if any values in text boxes

                //Add combo box selections, if selection is not all
                //Cannot use combox.SelectedValue if combobox does not have a DataSource for its contents
                if ((cboRole.SelectedItem as ComboboxItem).cboValue != "All")
                {
                    strval = (cboRole.SelectedItem as ComboboxItem).cboValue;
                    strval = strval.Replace("'", "''");
                    if (strWHERE.Length > 0)
                    {
                        strWHERE += "AND ";
                    }
                    else
                    {
                        strWHERE = "WHERE ";
                    }
                    strWHERE += "us.UserID IN (SELECT UserID from UserRole WHERE RoleName='" + strval + "') ";
                }

                if ((cboStatus.SelectedItem as ComboboxItem).cboValue != "All")
                {
                    strval = (cboStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    strval = strval.Replace("'", "''");
                    if (strWHERE.Length > 0)
                    {
                        strWHERE += "AND ";
                    }
                    else
                    {
                        strWHERE = "WHERE ";
                    }
                    strWHERE += "us.RecordStatus ='" + strval + "' ";
                }

                strSQL += strWHERE + "Order by UserCode";

                ds = DataOps.GetDataset_with_SQL(strSQL);

                //6. If data found
                if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    //6a. Enable btnExport
                    btnExport.Enabled = true;

                    //6b. Assign datable as gridview datasource
                    // Use a DataTable as the DataSource for the DataGridView to provide sorting by Col Header
                    //  clicks, automatic
                    dtUsers = ds.Tables[0].Copy();
                    dgResults.DataSource = dtUsers;

                    //6c. Update records label with # records
                    lblRecords.Text = "Records: " + dtUsers.Rows.Count;

                    //6d. Create a list from the table's data as the binding source
                    // Create a separate List of Users from the Datatable, for the Form's BindingSource
                    lsUsers = Globalitems.CreateListFromDataTable<User>(dtUsers);
                    bs1.DataSource = lsUsers;

                    //6e. Fill record details with 1st row of returned datatable
                    FillDetailRecord(dtUsers.Rows[0]);

                    //6f. Update recbuttons: Set records to display = true, Action to READONLY, and Set Nav btns
                    recbuttons.blnRecordsToDisplay = true;
                    recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                    Globalitems.SetNavButtons(recbuttons,bs1);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSearch", ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try { PerformSearch(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnSearch_Click", ex.Message); }
        }

        private void DisplayMode()
        {
            try
            {
                if (strMode == "READ")
                {
                    //Show panels, Action & Nav btns
                    lblStatus.Text = "Read Only";
                    AdjustReadOnlyStatus(true);

                    pnlSearch.Enabled = true;
                    pnlResults.Enabled = true;
                }
                else
                {
                    pnlSearch.Enabled = false;
                    pnlResults.Enabled = false;

                    if (strMode == "NEW" && bs1 != null) Formops.ClearRecordData(this,lsControls); 
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "DisplayMode", ex.Message);
            }
        }

        private void FilterBindingSource()
        //User has selected rows in the Gridview, dgResults.
        //Filter the current binding source, lsUsers_copy with Linq to only contrain the selected rows
        {
            try
            {
                DataTable dtFiltered;
                DataView dv;
                List<int> lsIDs = new List<int>();
                string strFilter;
                string strSort = "UserCode";

                //Change bs1 to selected rows
                if (dgResults.SelectedRows.Count == 0)
                {
                    btnClear_Click(null, null);
                    return;
                }
                else
                {
                    //Place all the Selected UserIDs, cell 0, of all selected rows into strFilter
                    strFilter = "UserID IN (";
                    for (int row = 0; row < dgResults.SelectedRows.Count; row++)
                    {
                        strFilter += dgResults.SelectedRows[row].Cells[0].EditedFormattedValue.ToString();
                        if (row < dgResults.SelectedRows.Count - 1) strFilter += ",";
                    }
                    strFilter += ")";

                    // Change Col. to sort by if selected, change sort order if desc
                    if (dgResults.SortedColumn != null) strSort = dgResults.SortedColumn.DataPropertyName;
                    if (dgResults.SortOrder == SortOrder.Descending) strSort += " DESC";

                    // Create a DataView from dtUsers based on Filter & Sort
                    dv = new DataView(dtUsers, strFilter, strSort,DataViewRowState.CurrentRows);

                    // Create dtFiltered from dv
                    dtFiltered = dv.ToTable();
                    
                    // Create lsUsers from dtFiltered & set as bs1 Datasource
                    lsUsers = Globalitems.CreateListFromDataTable<User>(dtFiltered);
                    bs1.DataSource = lsUsers;

                    FillDetailRecord(dtFiltered.Rows[0]);

                    recbuttons.blnRecordsToDisplay = true;
                    recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                    Globalitems.SetNavButtons(recbuttons,bs1);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FilterBindingSource", ex.Message);
            }
        }

        private void dgResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //As long as row clicked is not the Column Header row, index = -1, change the binding source
            if (e.RowIndex > -1) FilterBindingSource();
        }

        private void PerformPasswordHelp()
        {
            try
            {
                string strPwdRules;

                strPwdRules = "1) Must be at least 4 chars.\r" +
                "2) Cannot begin with DAI.\r" +
                "3) Cannot be a seq. of #'s.\r" +
                "4) Pwd is case sensitive.\r" +
                "5) Cannot repeat same char 4 times.\r";

                MessageBox.Show(strPwdRules, "PASSWORD RULES",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformPasswordHelp", ex.Message);
            }
        }

        private void btnPwdHelp_Click(object sender, EventArgs e)
        {
            try { PerformPasswordHelp(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnPwdHelp", ex.Message); }
        }

        private void txtFname_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtFname_record",lsControls);
        }

        private void txtFname_KeyDown(object sender, KeyEventArgs e)
        {
            //Set Enter press to mean user clicked Search button
            if (e.KeyData == Keys.Enter) PerformSearch();
        }

        private void cboRole_KeyDown(object sender, KeyEventArgs e)
        {
            //Set Enter press to mean user clicked Search button
            if (e.KeyData == Keys.Enter) PerformSearch();
        }

        private void cboStatus_KeyDown(object sender, KeyEventArgs e)
        {
            //Set Enter press to mean user clicked Search button
            if (e.KeyData == Keys.Enter) PerformSearch();
        }

        private void txtLname_KeyDown(object sender, KeyEventArgs e)
        {
            //Set Enter press to mean user clicked Search button
            if (e.KeyData == Keys.Enter) PerformSearch();
        }

        private void txtUname_record_KeyDown(object sender, KeyEventArgs e)
        {
            //if strMode = NEW, Set Enter press to mean user clicked Save button 
            if (strMode == "NEW" && e.KeyData == Keys.Enter) btnSave_Clicked();
        }

        private void txtFname_record_KeyDown(object sender, KeyEventArgs e)
        {
            //if strMode != READ, Set Enter press to mean user clicked Save button 
            if (strMode != "READ" && e.KeyData == Keys.Enter) btnSave_Clicked();
        }

        private void txtLname_record_KeyDown(object sender, KeyEventArgs e)
        {
            //if strMode != READ, Set Enter press to mean user clicked Save button 
            if (strMode != "READ" && e.KeyData == Keys.Enter) btnSave_Clicked();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            //if strMode != READ, Set Enter press to mean user clicked Save button 
            if (strMode != "READ" && e.KeyData == Keys.Enter) btnSave_Clicked();
        }

        private void txtPassword_confirm_KeyDown(object sender, KeyEventArgs e)
        {
            //if strMode != READ, Set Enter press to mean user clicked Save button 
            if (strMode != "READ" && e.KeyData == Keys.Enter) btnSave_Clicked();
        }

        private void cboStatus_record_KeyDown(object sender, KeyEventArgs e)
        {
            //if strMode != READ, Set Enter press to mean user clicked Save button 
            if (strMode != "READ" && e.KeyData == Keys.Enter) btnSave_Clicked();
        }

        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            //if strMode != READ, Set Enter press to mean user clicked Save button 
            if (strMode != "READ" && e.KeyData == Keys.Enter) btnSave_Clicked();
        }

        private void txtExtension_KeyDown(object sender, KeyEventArgs e)
        {
            //if strMode != READ, Set Enter press to mean user clicked Save button 
            if (strMode != "READ" && e.KeyData == Keys.Enter) btnSave_Clicked();
        }

        private void txtUname_KeyDown(object sender, KeyEventArgs e)
        {
            //Set Enter press to mean user clicked Search button
            if (e.KeyData == Keys.Enter) PerformSearch();
        }

        private void txtLname_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtLname_record",lsControls);
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtPassword",lsControls);
        }

        private void ckAdmin_CheckedChanged(object sender, EventArgs e)
        {
            if (strMode != "READ") Formops.ChangeControlUpdatedStatus("ckAdmin",lsControls);
        }

        private void ckHide_CheckedChanged(object sender, EventArgs e)
        {
            if (strMode != "READ") Formops.ChangeControlUpdatedStatus("ckHide",lsControls);
        }

        private void ckBilling_CheckedChanged(object sender, EventArgs e)
        {
            if (strMode != "READ") Formops.ChangeControlUpdatedStatus("ckBilling",lsControls);
        }

        private void ckYard_CheckedChanged(object sender, EventArgs e)
        {
            if (strMode != "READ") Formops.ChangeControlUpdatedStatus("ckYard", lsControls);
        }
        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtPhone",lsControls);
        }

        private void txtExtension_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtExtension",lsControls);
        }

        private void txtFax_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtFax",lsControls);
        }

        private void txtEmpl_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtEmpl",lsControls);
        }

        private void txtPortPassID_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtPortPassID",lsControls);
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtEmail",lsControls);
        }

        private void txtCell_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtCell",lsControls);
        }

        private void cboStatus_record_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboStatus_record",lsControls);
        }

        private void OpenCSVFile()
        {
            try
            {
                DataSet ds;
                DataTable dt;
                DataView dv;
                string strFilename;
                string strSort = "";
                string strSQL;
                List<ControlInfo> lsCSVcols = new List<ControlInfo>();

                //1. Get the file Directory & Filename from the SettingTable
                strSQL = "SELECT ValueKey,ValueDescription FROM SettingTable " + 
                    "WHERE ValueKey IN ('ExportDirectory','UserExportFileName') " +
                    "AND RecordStatus='Active' ORDER BY ValueKey";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                // S/B just two active rows, row 1 ExportDirectory, row 2 UserExportFileName
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 2)
                {
                    Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile",
                        "No rows returned from SettingTable");
                    return;
                }
                // 1st Record s/b ExportDirectory, 2nd Record s/b UserExportFileName
                strFilename = ds.Tables[0].Rows[0]["ValueDescription"].ToString();
                strFilename += @"\" + ds.Tables[0].Rows[1]["ValueDescription"].ToString();

                //2. Create a copy of the datatable used for the datagridview datasource
                dt = dtUsers.Copy();

                //3. If the gridview is sorted, use a dv to sort the table copy the same way
                if (dgResults.SortedColumn != null)
                {
                    strSort = dgResults.SortedColumn.DataPropertyName;
                    if (dgResults.SortOrder == SortOrder.Descending) strSort += " DESC";
                    dv = new DataView(dt,"", strSort, DataViewRowState.CurrentRows);
                    dt = dv.ToTable();
                }

                //4. Create a list, lsCSVcols with ControlInfo objects in the order to appear in the csv file 
                //Get ctrlinfo object from lsControls for UserCode & add to lsCSVcols
                var objctrlinfo = lsControls.Where(obj => obj.RecordFieldName == "UserCode").ToList();
                lsCSVcols.Add(objctrlinfo[0]);

                //Get ctrlinfo object from lsControls for UserFullName & add to lsCSVcols
                objctrlinfo = lsControls.Where(obj => obj.RecordFieldName == "UserFullName").ToList();
                lsCSVcols.Add(objctrlinfo[0]);

                //Get ctrlinfo object from lsControls for RecordStatus & add to lsCSVcols
                objctrlinfo = lsControls.Where(obj => obj.RecordFieldName == "RecordStatus").ToList();
                lsCSVcols.Add(objctrlinfo[0]);

                //5. Invoke CreateSCVFile to create, save, & open the csv file
                Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile", ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            OpenCSVFile();   
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            //Make sure Main form displays and has the focus
            Globalitems.MainForm.Visible = true;
            Globalitems.MainForm.Focus();
        }

        private void frmUserAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (strMode != "READ" && !Globalitems.blnException)
            {
                MessageBox.Show("You must SAVE or Cancel the current changes to close this form",
                    "CANNOT CLOSE THIS FORM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void frmUserAdmin_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
