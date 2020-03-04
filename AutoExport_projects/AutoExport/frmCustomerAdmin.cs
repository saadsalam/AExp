using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmCustomerAdmin : Form
    {
        public bool blnNewCustomerRQFromOtherForm = false;
        public bool blnRatesChanged = false;
        public DataTable dtNewRates = new DataTable();

        private const string CURRENTMODULE = "frmCustomerAdmin";

        private bool blnIgnoreZipcode = false;
        private BindingSource bs1 = new BindingSource();
        private DataTable dtCustomers = new DataTable();
        private DataTable dtOriginalRates;
        private List<int> lsCustomerIDs = new List<int>();
        private string strMode;

        //Used to track if Billing/Street IDs exist in Customer record,when user clicks MODIFY
        private string strMode_Billing;
        private string strMode_Street;

        //Set up List of ControlInfo objects, lsControlInfo, as follows:
        //  Order in list establishes Indexes for tabbing, implemented by SetTabIndex() method
        //  AlwaysReadOnly identifies if control cannot be modified by User
        //  ControlPropertyToBind identifies what controls are initialized 
        //  RecordFieldName identify what controls display record detail
        //  HeaderText sets column header to use for Export to csv file
        //  Updated property establishes what controls User has modified

        //Since the Billing & Street Address tabs, refer to separate recs in the Location
        //  table, use separate lists, to facilitate inserting/updating new recs in the
        //  Location table, if User clicks Save

        private List<ControlInfo> lsControls_primary = new List<ControlInfo>()
        {
            //Controls in Search Panel, and Detail Panel associated with the Customer table
            new ControlInfo {ControlID="txtCustName",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCustCode",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCity",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtState",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtZip",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboStatus",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="btnSearch" },
            new ControlInfo {ControlID="btnClear" },
            new ControlInfo {ControlID="btnExport" },
            new ControlInfo { ControlID="txtCustName_record", RecordFieldName="CustomerName",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtShortName", RecordFieldName="ShortName",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtCustCode_record", RecordFieldName="CustomerCode",
                ControlPropetyToBind="Text",HeaderText="Cust. Code"},
            new ControlInfo {ControlID="txtDBAName", RecordFieldName="DBAName",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtLocationID_Billing",
                 RecordFieldName ="BillingAddressID",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtLocationID_Street",
                 RecordFieldName ="MainAddressID",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="cboPaymentMethod", RecordFieldName="DefaultBillingMethod",
                ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="cboStatus_record", RecordFieldName="RecordStatus",
                ControlPropetyToBind="SelectedValue",HeaderText="Status"},
            new ControlInfo {ControlID="ckEmail", RecordFieldName="SendEmailConfirmationsInd",
                ControlPropetyToBind="Checked"},
            new ControlInfo {ControlID="txtBookingPrefix", RecordFieldName="BookingNumberPrefix",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtHandheldCustID", RecordFieldName="HandheldScannerCustomerCode",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtInternalComment", RecordFieldName="InternalComment",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCreationDate", RecordFieldName="CreationDate",
                ControlPropetyToBind="Text",ReadOnly=true },
            new ControlInfo {ControlID="txtCreatedBy", RecordFieldName="CreatedBy",
                ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="txtUpdatedDate", RecordFieldName="UpdatedDate",
                ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="txtUpdatedBy", RecordFieldName="UpdatedBy",
                ControlPropetyToBind="Text",ReadOnly=true},
            // objects needed for csv file  HeaderText="Cust. Name"
            new ControlInfo {RecordFieldName="CustName",HeaderText="Cust. Name"},
            new ControlInfo {RecordFieldName="AddressLine1",HeaderText="Street Addr."},
            new ControlInfo {RecordFieldName="City",HeaderText="City"},
            new ControlInfo {RecordFieldName="State",HeaderText="ST"},
            new ControlInfo {RecordFieldName="Zip",HeaderText="Zip"}
        };

        private List<ControlInfo> lsControls_BillingAddr = new List<ControlInfo>()
        {
            //Controls in Detail Panel associated with the Location table, Billing Address
            new ControlInfo {ControlID="txtLocName_Billing",
                RecordFieldName ="LocationName_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtShortLocName_Billing",
                RecordFieldName ="LocationShortName_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboLocSubtype_Billing",
                RecordFieldName ="LocationSubType_b",
                ControlPropetyToBind ="SelectedValue"},
            new ControlInfo {ControlID="txtAddr1_Billing",
                RecordFieldName ="AddressLine1_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAddr2_Billing",
                RecordFieldName ="AddressLine2_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCity_record_Billing",
                RecordFieldName ="City_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtState_record_Billing",
                RecordFieldName ="State_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtZip_record_Billing",
                RecordFieldName ="Zip_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboCountry_Billing",
                RecordFieldName ="Country_b",
                ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtMainPhone_Billing",
                RecordFieldName ="MainPhone_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtMainFax_Billing",
                RecordFieldName ="FaxNumber_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryFName_Billing",
                RecordFieldName ="PrimaryContactFirstName_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryLName_Billing",
                RecordFieldName ="PrimaryContactFirstName_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryPhone_Billing",
                RecordFieldName ="PrimaryContactPhone_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryExt_Billing",
                RecordFieldName ="PrimaryContactExtension_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryCell_Billing",
                RecordFieldName ="PrimaryContactCellPhone_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryEmail_Billing",
                RecordFieldName ="PrimaryContactEmail_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternateFName_Billing",
                RecordFieldName ="AlternateContactFirstName_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternateLName_Billing",
                RecordFieldName ="AlternateContactLastName_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternatePhone_Billing",
                RecordFieldName ="AlternateContactPhone_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternateExt_Billing",
                RecordFieldName ="AlternateContactExtension_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternateCell_Billing",
                RecordFieldName ="AlternateContactCellPhone_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternateEmail_Billing",
                RecordFieldName ="AlternateContactEmail_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtOtherPhoneDesc1_Billing",
                RecordFieldName ="OtherPhone1Description_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtOtherPhone1_Billing",
                RecordFieldName ="OtherPhone1_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtOtherPhoneDesc2_Billing",
                RecordFieldName ="OtherPhone2Description_b",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtOtherPhone2_Billing",
                RecordFieldName ="OtherPhone2_b",
                ControlPropetyToBind="Text"}
        };

        private List<ControlInfo> lsControls_StreetAddr = new List<ControlInfo>()
        {
            //Controls in Detail Panel associated with the Location table, Street Address
            new ControlInfo {ControlID="ckSameAsBilling"},
            new ControlInfo {ControlID="txtLocName_Street",
                 RecordFieldName ="LocationName_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtShortLocName_Street",
                RecordFieldName ="LocationShortName_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboLocSubtype_Street",
                RecordFieldName ="LocationSubType_s",
                ControlPropetyToBind ="SelectedValue"},
            new ControlInfo {ControlID="txtAddr1_Street",
                RecordFieldName ="AddressLine1_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAddr2_Street",
                RecordFieldName ="AddressLine2_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCity_record_Street",
                RecordFieldName ="City_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtState_record_Street",
                RecordFieldName ="State_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtZip_record_Street",
                RecordFieldName ="Zip_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboCountry_Street",
                RecordFieldName ="Country_s",
                ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtMainPhone_Street",
                RecordFieldName ="MainPhone_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtMainFax_Street",
                RecordFieldName ="FaxNumber_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryFName_Street",
                RecordFieldName ="PrimaryFName_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryLName_Street",
                RecordFieldName ="PrimaryLName_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryPhone_Street",
                RecordFieldName ="PrimaryContactPhone_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryExt_Street",
                RecordFieldName ="PrimaryContactExt_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryCell_Street",
                RecordFieldName ="PrimaryContactCell_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtPrimaryEmail_Street",
                RecordFieldName ="PrimaryContactEmail_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternateFName_Street",
                RecordFieldName ="AlternateFName_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternateLName_Street",
                RecordFieldName ="AlternateLName_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternatePhone_Street",
                RecordFieldName ="AlternateContactPhone_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternateExt_Street",
                RecordFieldName ="AlternateContactExt_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternateCell_Street",
                RecordFieldName ="AlternateContactCell_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtAlternateEmail_Street",
                RecordFieldName ="AlternateContactEmail_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtOtherPhoneDesc1_Street",
                RecordFieldName ="OtherPhone1Description_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtOtherPhone1_Street",
                RecordFieldName ="OtherPhone1_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtOtherPhoneDesc2_Street",
                RecordFieldName ="OtherPhone2Description_s",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtOtherPhone2_Street",
                RecordFieldName ="OtherPhone2_s",
                ControlPropetyToBind="Text"}
        };

        public frmCustomerAdmin()
        {
            List<string> lsExcludes = new List<string>
            {
                {"txtInternalComment"}
            };

            InitializeComponent();
            dgResults.AutoGenerateColumns = false;
            dgRates.AutoGenerateColumns = false;

            strMode = "READ";
            FillCombos();

            // Assign methods to the recbuttons public event variables
            recbuttons.CancelRecord += btnCancel_Clicked;
            recbuttons.MovePrev += btnPrev_Clicked;
            recbuttons.MoveNext += btnNext_Clicked;
            recbuttons.DeleteRecord += btnDelete_Clicked;
            recbuttons.NewRecord += btnNew_Clicked;
            recbuttons.ModifyRecord += btnModify_Clicked;
            recbuttons.SaveRecord += btnSave_Clicked;

            Globalitems.SetControlHeight(this,lsExcludes);
            Formops.SetTabIndex(this,lsControls_primary);
            Formops.SetTabIndex(this, lsControls_BillingAddr);
            Formops.SetTabIndex(this, lsControls_StreetAddr);

            //Clear record # texts
            lblCustRecords.Text = "";
            lblRateRecords.Text = "";

            //Disable btnExport
            btnExport.Enabled = false;

            DisplayMode();

            if (Globalitems.strRoleNames.Contains("HideRates"))
            {
                Globalitems.blnHideRates = true;
                tbDetails.TabPages.Remove(tabRates);
            }
        }

        private void frmCustomerAdmin_Activated(object sender, EventArgs e)
        {
            if (blnNewCustomerRQFromOtherForm)
            {
                blnNewCustomerRQFromOtherForm = false;
                recbuttons.btnNew_Click(null, null);
            }
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

                    if (strMode == "NEW" && bs1 != null) Formops.ClearRecordData(this, lsControls_primary);
                    if (strMode == "NEW" && bs1 != null) Formops.ClearRecordData(this, lsControls_BillingAddr);
                    if (strMode == "NEW" && bs1 != null) Formops.ClearRecordData(this, lsControls_StreetAddr);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "DisplayMode", ex.Message);
            }
        }

        private void btnCancel_Clicked()
        {
            try { CancelSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnCancel_Clicked", ex.Message); }
        }

        private void btnDelete_Clicked()
        {
            try { PerformDeleteRecord(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnDelete_Clicked", ex.Message); }
        }

        private void btnModify_Clicked()
        {
            try { ModifyRecordSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnModify_Clicked", ex.Message); }
        }

        private void btnNext_Clicked()
        {
            try { PerformMoveNext(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnPrev_Clicked", ex.Message); }
        }

        private void btnNew_Clicked()
        {
            try { NewRecordSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnNew_Clicked", ex.Message); }
        }

        private void btnPrev_Clicked()
        {
            try { PerformMovePrevious(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnPrev_Clicked", ex.Message); }
        }

        private void btnSave_Clicked()
        {
            try { PerformSaveRecord(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnSave_Clicked", ex.Message); }
        }

        private string SQLForNewLocation(string strCreationDate,string strLocType)
        {
            try
            {
                ComboBox cboBox;
                string strSQL = "INSERT INTO Location (ParentRecordID,ParentRecordTable," +
                    "LocationType, LocationSubType,LocationName,LocationShortName,AddressLine1," +
                    "AddressLine2,City,[State],Zip,Country,MainPhone,FaxNumber," +
                    "PrimaryContactFirstName, PrimaryContactLastName," +
                    "PrimaryContactPhone, PrimaryContactExtension," +
                    "PrimaryContactCellPhone,PrimaryContactEmail," +
                    "AlternateContactFirstName, AlternateContactLastName," +
                    "AlternateContactPhone, AlternateContactExtension," +
                    "AlternateContactCellPhone, AlternateContactEmail," +
                    "OtherPhone1Description, OtherPhone1," +
                    "OtherPhone2Description, OtherPhone2," +
                    "RecordStatus, CreationDate,CreatedBy) VALUES (";
                string strval;
                TextBox txtBox;

                //ParentRecordID
                strSQL += txtCustID_record.Text + ",";

                //ParentRecordTable
                strSQL += "'Customer',";

                //LocationType
                txtBox = txtLocType_Billing;
                if (strLocType == "Street") txtBox = txtLocType_Street;
                strSQL += "'" + txtBox.Text + "',";

                //LocationSubType
                cboBox = cboLocSubtype_Billing;
                if (strLocType == "Street") cboBox = cboLocSubtype_Street;
                strSQL += "'" + (cboBox.SelectedItem as ComboboxItem).cboValue + "',";

                //LocationName
                txtBox = txtLocName_Billing;
                if (strLocType == "Street") txtBox = txtLocName_Street;
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(txtBox.Text) + "',";

                //LocationShortName
                txtBox = txtShortLocName_Billing;
                if (strLocType == "Street") txtBox = txtShortLocName_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AddressLine1
                txtBox = txtAddr1_Billing;
                if (strLocType == "Street") txtBox = txtAddr1_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AddressLine2
                txtBox = txtAddr2_Billing;
                if (strLocType == "Street") txtBox = txtAddr2_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //City
                txtBox = txtCity_record_Billing;
                if (strLocType == "Street") txtBox = txtCity_record_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //State
                txtBox = txtState_record_Billing;
                if (strLocType == "Street") txtBox = txtState_record_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //Zip
                txtBox = txtZip_record_Billing;
                if (strLocType == "Street") txtBox = txtZip_record_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //Country
                cboBox = cboCountry_Billing;
                if (strLocType == "Street") cboBox = cboCountry_Street;
                strval = "";
                if ((cboBox.SelectedItem as ComboboxItem).cboValue != "select")
                    strval = (cboBox.SelectedItem as ComboboxItem).cboValue;
                strSQL += "'" + strval + "',";

                //MainPhone
                txtBox = txtMainPhone_Billing;
                if (strLocType == "Street") txtBox = txtMainPhone_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //FaxNumber
                txtBox = txtMainFax_Billing;
                if (strLocType == "Street") txtBox = txtMainFax_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactFirstName
                txtBox = txtPrimaryFName_Billing;
                if (strLocType == "Street") txtBox = txtPrimaryFName_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactLastName
                txtBox = txtPrimaryLName_Billing;
                if (strLocType == "Street") txtBox = txtPrimaryLName_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactPhone
                txtBox = txtPrimaryPhone_Billing;
                if (strLocType == "Street") txtBox = txtPrimaryPhone_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactExtension
                txtBox = txtPrimaryExt_Billing;
                if (strLocType == "Street") txtBox = txtPrimaryExt_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactCellPhone
                txtBox = txtPrimaryCell_Billing;
                if (strLocType == "Street") txtBox = txtPrimaryCell_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactEmail
                txtBox = txtPrimaryEmail_Billing;
                if (strLocType == "Street") txtBox = txtPrimaryEmail_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactFirstName
                txtBox = txtAlternateFName_Billing;
                if (strLocType == "Street") txtBox = txtAlternateFName_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactLastName
                txtBox = txtAlternateLName_Billing;
                if (strLocType == "Street") txtBox = txtAlternateLName_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactPhone
                txtBox = txtAlternatePhone_Billing;
                if (strLocType == "Street") txtBox = txtAlternatePhone_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactExtension
                txtBox = txtAlternateExt_Billing;
                if (strLocType == "Street") txtBox = txtAlternateExt_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactCellPhone
                txtBox = txtAlternateCell_Billing;
                if (strLocType == "Street") txtBox = txtAlternateCell_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactEmail
                txtBox = txtAlternateEmail_Billing;
                if (strLocType == "Street") txtBox = txtAlternateEmail_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //OtherPhone1Description
                txtBox = txtOtherPhoneDesc1_Billing;
                if (strLocType == "Street") txtBox = txtOtherPhoneDesc1_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //OtherPhone1
                txtBox = txtOtherPhone1_Billing;
                if (strLocType == "Street") txtBox = txtOtherPhone1_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //OtherPhone2Description
                txtBox = txtOtherPhoneDesc2_Billing;
                if (strLocType == "Street") txtBox = txtOtherPhoneDesc2_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //OtherPhone2
                txtBox = txtOtherPhone2_Billing;
                if (strLocType == "Street") txtBox = txtOtherPhone2_Street;
                strval = "";
                if (txtBox.Text.Trim().Length > 0) strval = txtBox.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //RecordStatus
                strSQL += "'" + (cboStatus_record.SelectedItem as ComboboxItem).cboValue +
                    "',";

                //CreationDate
                strSQL += "'" + strCreationDate.ToString() + "',";

                //CreatedBy
                strSQL += "'" + Globalitems.strUserName + "');";

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForNewLocation", ex.Message);
                return "";
            }
        }

        private string SQLForModifiedLocation(List<ControlInfo> lsChangedItems,
            string strLocID)
        {
            try
            {
                ComboBox cboBox;
                Control[] ctrls;
                string strSQL = "";
                string strval;

                strSQL = "UPDATE Location SET ";

                foreach (ControlInfo ctrlinfo in lsChangedItems)
                {
                    strSQL += ctrlinfo.RecordFieldName + " = ";

                    //Place the control into the array ctrls, s/b only one
                    ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            // Use HandleSingleQuoteForSQL to replace ' in text to '' for SQL
                            strval = Globalitems.HandleSingleQuoteForSQL(ctrls[0].Text);

                            if (strval.Length == 0)
                            { strSQL += "NULL"; }
                            else
                            { strSQL += "'" + strval + "'"; }
                            break;
                        case "SelectedValue":
                            //Cast control to ComboBox
                            cboBox = (ComboBox)ctrls[0];
                            strSQL += "'" + (cboBox.SelectedItem as ComboboxItem).cboValue + "'";
                            break;
                    }
                    strSQL += ",";
                }

                //Add UpdatedBy, UpdatedDate
                strSQL += "UpdatedBy = '" + Globalitems.strUserName + "',";
                strSQL += "UpdatedDate = CURRENT_TIMESTAMP ";

                // Add WHERE clause
                strSQL += " WHERE LocationID = " + strLocID;

                //Remove _s/_b in recordfield names (used in FillDetailRecord for both Billing
                //  & Street Addr info

                strSQL = strSQL.Replace("_s", "");
                strSQL = strSQL.Replace("_b", "");
                return strSQL;
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForModifiedLocation", ex.Message);
                return "";
            }
        }

        private string SQLForModifiedCustomer(List<ControlInfo> lsChangedItems)
        {
            try
            {
                CheckBox ckBox;
                ComboBox cboBox;
                Control[] ctrls;
                string strSQL = "";
                string strval;

                strSQL = "UPDATE Customer SET ";

                foreach (ControlInfo ctrlinfo in lsChangedItems)
                {
                    strSQL += ctrlinfo.RecordFieldName + " = ";

                    //Place the control into the array ctrls, s/b only one
                    ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            // Use HandleSingleQuoteForSQL to replace ' in text to '' for SQL
                            strval = Globalitems.HandleSingleQuoteForSQL(ctrls[0].Text);

                            if (strval.Length == 0)
                            { strSQL += "NULL"; }
                            else
                            { strSQL += "'" + strval + "'"; }
                            break;
                        case "SelectedValue":
                            //Cast control to ComboBox
                            cboBox = (ComboBox)ctrls[0];
                            strSQL += "'" + (cboBox.SelectedItem as ComboboxItem).cboValue + "'";
                            break;
                        case "Checked":
                            //Cast control to Checkbox
                            ckBox = (CheckBox)ctrls[0];
                            if (ckBox.Checked)
                                strSQL += "1";
                            else
                                strSQL += "0";
                            break;
                    }
                    strSQL += ",";
                }

                //Add UpdatedBy, UpdatedDate
                strSQL += "UpdatedBy = '" + Globalitems.strUserName + "',";
                strSQL += "UpdatedDate = CURRENT_TIMESTAMP";

                // Add WHERE clause
                strSQL += " WHERE CustomerID = " + txtCustID_record.Text;

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForModifiedCustomer", ex.Message);
                return "";
            }
        }
        
        private string SQLForNewCustomer(string strCreationDate)
        {
            try
            {
                string strSQL = "INSERT INTO Customer (CustomerCode,CustomerName,DBAName," +
                    "ShortName,CustomerType,DefaultBillingMethod,RecordStatus," +
                    "SendEmailConfirmationsInd,BookingNumberPrefix,HandheldScannerCustomerCode," +
                    "InternalComment,CreationDate,CreatedBy,AutoportExportCustomerInd) VALUES (";

                string strval;

                // CustomerCode
                strval = "";
                if (txtCustCode_record.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtCustCode_record.Text.Trim());
                strSQL += "'" + strval + "',";

                // CustomerName
                strval = "";
                if (txtCustName_record.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtCustName_record.Text.Trim());
                strSQL += "'" + strval + "',";

                // DBAName
                strval = "";
                if (txtDBAName.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtDBAName.Text.Trim());
                strSQL += "'" + strval + "',";

                // ShortName
                strval = "";
                if (txtShortName.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtShortName.Text.Trim());
                strSQL += "'" + strval + "',";

                // CustomerType
                strSQL += "'ExportCompany',";

                // DefaultBillingMethod
                strval = (cboPaymentMethod.SelectedItem as ComboboxItem).cboValue;
                if (strval == "select") strval = "";
                strSQL += "'" + strval + "',";

                // Record Status
                strval = (cboStatus_record.SelectedItem as ComboboxItem).cboValue;
                if (strval == "select") strval = "";
                strSQL += "'" + strval + "',";

                //SendEmailConfirmationsInd
                strval = "0";
                if (ckEmail.Checked) strval = "1";
                strSQL += strval + ",";

                //BookingNumberPrefix
                strval = "";
                if (txtBookingPrefix.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtBookingPrefix.Text.Trim());
                strSQL += "'" + strval + "',";

                //HandheldScannerCustomerCode
                strval = "";
                if (txtHandheldCustID.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtHandheldCustID.Text.Trim());
                strSQL += "'" + strval + "',";

                // InternalComment
                strval = "";
                if (txtInternalComment.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtInternalComment.Text.Trim());
                strSQL += "'" + strval + "',";

                // CreationDate
                strSQL += "'" + strCreationDate + "',";

                //CreatedBy
                strSQL += "'" +
                    Globalitems.HandleSingleQuoteForSQL(Globalitems.strUserName) +
                    "',";

                //AutoportExportCustomerInd
                strSQL += "1);";

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForNewCustomer", ex.Message);
                return "";
            }
        }

        private void PerformSaveRecord()
        {
            try
            {
                // Use linq to get a list of updated Customer,Billing Addr, Street Addr controls, 
                //  For this form, textboxes, comboboxes, checkboxes
                var changedlist_primary = lsControls_primary.Where(ctrlinfo => (ctrlinfo.Updated == true) &&
                            (ctrlinfo.ControlPropetyToBind == "Text" ||
                            ctrlinfo.ControlPropetyToBind == "SelectedValue" ||
                            ctrlinfo.ControlPropetyToBind == "Checked")).ToList();

                var changedlist_billingaddr = lsControls_BillingAddr.Where(ctrlinfo => (ctrlinfo.Updated == true) &&
                            (ctrlinfo.ControlPropetyToBind == "Text" ||
                            ctrlinfo.ControlPropetyToBind == "SelectedValue" ||
                            ctrlinfo.ControlPropetyToBind == "Checked")).ToList();

                var changedlist_streetaddr = lsControls_StreetAddr.Where(ctrlinfo => (ctrlinfo.Updated == true) &&
                           (ctrlinfo.ControlPropetyToBind == "Text" ||
                           ctrlinfo.ControlPropetyToBind == "SelectedValue" ||
                           ctrlinfo.ControlPropetyToBind == "Checked")).ToList();
                
                DataSet ds;
                DataTable dtBulk = new DataTable();
                DataView dv;
                int intCurrentBSPosition = 0;
                SProcParameter objParam;
                List<SProcParameter> lsParams = new List<SProcParameter>();
                string strCreationDate = DateTime.Now.ToString();
                string strCustomer_action = "";
                string strLocBilling_action = "";
                string strLocStreet_action = "";
                string strSProc = "spUpdateCustomerInfo";
                string strRate_action = "";
                string strSQL;
                string strSQL_customer = "";
                string strSQL_loc_billing = "";
                string strSQL_loc_street = "";
                string strtmpRateTable = "tmpCustRates_" + 
                    DateTime.Now.ToString("yyyyMMddHHmmss");
                string strResult;
                
                if (ValidRecord())
                {
                    //1. Perform the DB action
                    if (strMode == "NEW")
                    {
                        // Need SQL to insert new customer info
                        strSQL_customer = SQLForNewCustomer(strCreationDate);

                        strCustomer_action = "NEW";  

                        //Check if Billing info added
                        if (strMode_Billing == "NEW")
                        {
                            strSQL_loc_billing = SQLForNewLocation(strCreationDate, "Billing");
                            strLocBilling_action = "NEW";
                            if (ckSameAsBilling.Checked) strLocStreet_action = "SAME";
                        }

                        if (strMode_Street == "NEW")
                        {
                            strSQL_loc_street = SQLForNewLocation(strCreationDate, "Street");
                            strLocStreet_action = "NEW";
                        }

                        //Check if any Rates added
                        if (dtNewRates.Rows.Count == 0) strtmpRateTable = ""; 
                    }

                    if (strMode == "MODIFY")
                    {
                        intCurrentBSPosition = bs1.Position;

                        if (changedlist_primary.Count > 0)
                        {
                            strCustomer_action = "MODIFY";
                            strSQL_customer = SQLForModifiedCustomer(changedlist_primary);
                        }

                        strLocBilling_action = strMode_Billing;
                        switch (strLocBilling_action)
                        {
                            case "NEW":
                                strSQL_loc_billing = SQLForNewLocation(strCreationDate, "Billing");
                                break;
                            case "MODIFY":
                                strSQL_loc_billing = 
                                    strSQL_loc_billing = 
                                        SQLForModifiedLocation(changedlist_billingaddr,
                                            txtLocationID_Billing.Text);
                                break;
                            case "DELETE":
                                strSQL_loc_billing = "DELETE Location WHERE LocationID = " +
                                    txtLocationID_Billing.Text;
                                break;
                        }

                        strLocStreet_action = strMode_Street;
                        switch (strLocStreet_action)
                        {
                            case "NEW":
                                strSQL_loc_street = SQLForNewLocation(strCreationDate, "Street");
                                break;
                            case "MODIFY":
                                strSQL_loc_street =
                                    strSQL_loc_street = 
                                    SQLForModifiedLocation(changedlist_streetaddr,
                                        txtLocationID_Street.Text);
                                break;
                            case "DELETE":
                                strSQL_loc_billing = "DELETE Location WHERE LocationID = " +
                                    txtLocationID_Street.Text;
                                break;
                            case "RESET":
                                strSQL_loc_billing = "UPDATE Customer SET MainAddressID = " +
                                    "NULL WHERE LocationID = " + txtLocationID_Street.Text;
                                break;
                        }

                        //Check if any Rates changed
                        if (blnRatesChanged)
                        {
                            if (dtNewRates.Rows.Count == 0)
                                strRate_action = "DELETE";
                            else
                            {
                                strRate_action = "ADD";

                                //Copy undeleted rows from dtNewRates -> dtBulk & 
                                //  remove AutoportExportRatesID col for bulk copy
                                dv = new DataView(dtNewRates);

                                //Exclude Deleted Rows from dv
                                dv.RowStateFilter = DataViewRowState.CurrentRows;
                                dtBulk = dv.ToTable();
                                dtBulk.Columns.Remove("AutoportExportRatesID");

                                //Create tmpTable in DB
                                strSQL = "CREATE TABLE " + strtmpRateTable +
                                " (CustomerID int," +
                                "EntryFee decimal(19,2)," +
                                "PerDiem decimal(19,2)," +
                                "PerDiemGraceDays int," +
                                "StartDate datetime," +
                                "EndDate datetime," +
                                "CreationDate datetime," +
                                "CreatedBy varchar(20)," +
                                "UpdatedDate datetime," +
                                "UpdatedBy varchar(20)," +
                                "RateType varchar(20));";

                                DataOps.PerformDBOperation(strSQL);

                                //Bulkcopy dtBulk into strtmpRateTable if new rows
                                if (dtBulk.Rows.Count > 0)
                                    DataOps.PerformBulkCopy(strtmpRateTable, dtBulk);
                            }
                        }
                        else
                            strtmpRateTable = "";              
                    }

                    objParam = new SProcParameter();
                    objParam.Paramname = "@customeraction";
                    objParam.Paramvalue = strCustomer_action;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@customerSQL";
                    objParam.Paramvalue = strSQL_customer;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@loc_billing_action";
                    objParam.Paramvalue = strLocBilling_action;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@loc_billingSQL";
                    objParam.Paramvalue = strSQL_loc_billing;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@loc_street_action";
                    objParam.Paramvalue = strLocStreet_action;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@loc_streetSQL";
                    objParam.Paramvalue = strSQL_loc_street;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@Rate_action";
                    objParam.Paramvalue = strRate_action;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@tmpRateTable";
                    objParam.Paramvalue = strtmpRateTable;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@CustomerID";
                    objParam.Paramvalue = txtCustID_record.Text;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@CreationDate";
                    objParam.Paramvalue = strCreationDate;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@CreatedBy";
                    objParam.Paramvalue = Globalitems.strUserName;
                    lsParams.Add(objParam);

                    ds = DataOps.GetDataset_with_SProc(strSProc, lsParams);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "PerforSaveRecord",
                            "No data returned after executing SProc");
                        return;
                    }

                    strResult = ds.Tables[0].Rows[0]["result"].ToString();
                    if (strResult != "OK")
                    {
                        Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord",
                            "Error from SProc: " + ds.Tables[0].Rows[0]["ErrorMessage"]);
                        return;
                    }

                    //2. Inform User of success
                    MessageBox.Show("The Customer information has been modified in the DB.",
                    "CUSTOMER INFO MODIFIED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //3. Display other forms
                    Globalitems.DisplayOtherForms(this,true);

                    //4. Set Mode to READ
                    strMode = "READ";

                    //5. Enable Search/Results panels
                    pnlSearch.Enabled = true;
                    pnlResults.Enabled = true;

                    btnSearch.Enabled = true;
                    btnClear.Enabled = true;

                    //6. Set Status label to Read only
                    lblStatus.Text = "Read only";
                    AdjustReadOnlyStatus(true);

                    blnIgnoreZipcode = false;            

                    //7. Perform new search
                    PerformSearch();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord", ex.Message);
            }
        }

        private bool AddrSectionHasInfo(string strAddrType)
        {
            //Checks if any textboxes or ComboBox in the section has a value
            //Doesn't look at checkbox, in Street tab
            try
            {
                ComboBox cboBox;
                Control[] ctrls;
                Control ctrlx;
                List<ControlInfo> lsControls = lsControls_BillingAddr;

                if (strAddrType == "Street") lsControls = lsControls_StreetAddr;

                //Loop through each control. Once a value is found, return true
                foreach (ControlInfo ctrlinfo in lsControls)
                {
                    //Get the Control associated with ctrlinfo.ControlID
                    ctrls = this.Controls.Find(ctrlinfo.ControlID, true);
                    ctrlx = ctrls[0];

                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            if (ctrlx.Text.Trim().Length > 0)
                                return true;
                            break;
                        case "SelectedValue":
                            cboBox = (ComboBox)ctrls[0];
                            if (cboBox.SelectedIndex != -1 && 
                                (cboBox.SelectedItem as ComboboxItem).cboValue != "select")
                                return true;
                            break;
                    }
                }

                return false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "AddrSectionHasInfo", ex.Message);
                return false;
            }
        }

        private bool ValidRecord()
        {
            try
            {
                DataSet ds;
                frmAreYouSure frmConfirm;
                string strMessage;
                string strSQL;
                string strval;

                //Use linq to see what controls changed
                var changedlist_primary = lsControls_primary.Where(ctrlinfo => ctrlinfo.Updated == true).ToList();
                var changedlist_billinginfo = lsControls_BillingAddr.Where(ctrlinfo => ctrlinfo.Updated == true).ToList();
                var changedlist_streetinfo = lsControls_StreetAddr.Where(ctrlinfo => ctrlinfo.Updated == true).ToList();

                //Make sure Cust. Name is not blank
                if (txtCustName_record.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Customer Name cannot be blank.", "MISSING CUSTOMER NAME",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCustName_record.Focus();
                    return false;
                }

                //Make sure Cust. Name is unique 
                strSQL = "SELECT CustomerID FROM Customer " +
                    "WHERE CustomerID <> " + txtCustID_record.Text +
                    " AND RTRIM(ISNULL(CustomerName,'')) = " +
                    "'" + Globalitems.HandleSingleQuoteForSQL(txtCustName_record.Text.Trim()) + "'";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ValidRecord",
                        "Query to check unique Cust. Name " +
                        "returned no table.");
                    return false;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Customer Name must be unique.\n\n" +
                        "Another customer already has this Name.", "DUPLICATE CUSTOMER NAME",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCustName_record.Focus();
                    txtCustName_record.Select(0, txtCustName_record.Text.Length);
                    return false;
                }

                //If ShortName entered, make sure it's unique
                if (txtShortName.Text.Trim().Length > 0)
                {
                    strSQL = "SELECT CustomerID FROM Customer " +
                  "WHERE CustomerID <> " + txtCustID_record.Text +
                  " AND RTRIM(ISNULL(ShortName,'')) = " +
                  "'" + txtShortName.Text.Trim() + "'";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "ValidRecord",
                            "Query to check unique Cust. Short Name " +
                            "returned no table.");
                        return false;
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Customer Short Name must be unique.\n\n" +
                            "Another customer already has this Name.", 
                            "DUPLICATE CUSTOMER SHORT NAME",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtShortName.Focus();
                        txtShortName.Select(0, txtShortName.Text.Length);
                        return false;
                    }
                }

                //Make sure Cust. Code is not blank
                if (txtCustCode_record.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Customer Code cannot be blank.", "MISSING CUSTOMER CODE",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCustCode_record.Focus();
                    return false;
                }

                //Make sure Record Status is not blank
                if ((cboStatus_record.SelectedItem as ComboboxItem).cboValue == "select")
                {
                    MessageBox.Show("Record Status cannot be blank.", "MISSING RECORD STATUS",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboStatus_record.Focus();
                    return false;
                }

                //Make sure Handheld Cust. ID is not blank
                if (txtHandheldCustID.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Handheld Customer ID cannot be blank.", "MISSING HANDHELD ID",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtHandheldCustID.Focus();
                    return false;
                }

                //Make sure Handheld Cust. ID is unique
                strSQL = "SELECT CustomerID FROM Customer " +
                    "WHERE CustomerID <> " + txtCustID_record.Text +
                    " AND RTRIM(ISNULL(HandheldScannerCustomerCode,'')) = " +
                    "'" + txtHandheldCustID.Text.Trim() + "'";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ValidRecord",
                        "Query to check unique HandHeldScannerCustomerCode " +
                        "returned no table.");
                    return false;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Handheld Customer ID must be unique.\n\n" +
                        "Another customer already has this ID.", "DUPLICATE HANDHELD ID",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtHandheldCustID.Focus();
                    txtHandheldCustID.Select(0, txtHandheldCustID.Text.Length);
                    return false;
                }

                //If cboCountry is US or Canada, State & Zip must be entered
                //Ck Billing Address tab
                if (txtLocName_Billing.Text.Trim().Length > 0)
                {

                    if ((cboCountry_Billing.SelectedItem as ComboboxItem).cboValue == "U.S.A." ||
                        (cboCountry_Billing.SelectedItem as ComboboxItem).cboValue == "Canada")
                    {
                        if (txtState_record_Billing.Text.Trim().Length == 0)
                        {
                            MessageBox.Show("On the Billing Address Tab, the State " +
                              "must be entered for the U.S.A and Canada.",
                              "MISSING STATE",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtState_record_Billing.Focus();
                            return false;
                        }

                        if (txtZip_record_Billing.Text.Trim().Length == 0)
                        {
                            MessageBox.Show("On the Billing Address Tab, the Zip code " +
                              "must be entered for the U.S.A and Canada.",
                              "MISSING ZIP CODE",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtZip_record_Billing.Focus();
                            return false;
                        }
                    }
                }

                //Ck Street Address tab
                if (txtLocName_Street.Text.Trim().Length > 0 && !ckSameAsBilling.Checked)
                {

                    if ((cboCountry_Street.SelectedItem as ComboboxItem).cboValue == "U.S.A." ||
                        (cboCountry_Street.SelectedItem as ComboboxItem).cboValue == "Canada")
                    {
                        if (txtState_record_Street.Text.Trim().Length == 0)
                        {
                            MessageBox.Show("On the Street Address Tab, the State " +
                              "must be entered for the U.S.A and Canada.",
                              "MISSING STATE",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtState_record_Street.Focus();
                            return false;
                        }

                        if (txtZip_record_Street.Text.Trim().Length == 0)
                        {
                            MessageBox.Show("On the Street Address Tab, the Zip code " +
                              "must be entered for the U.S.A and Canada.",
                              "MISSING ZIP CODE",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtZip_record_Street.Focus();
                            return false;
                        }
                    }
                }

                        //Ck any emails entered
                        if (txtPrimaryEmail_Billing.Text.Trim().Length > 0)
                {
                    strval = txtPrimaryEmail_Billing.Text.Trim();
                    if (!Globalitems.validemailaddress(strval))
                    {
                        MessageBox.Show("On the Billing Addr. tab, the Primary Contact Email is not valid", 
                            "INVALID EMAIL ADDRESS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPrimaryEmail_Billing.Focus();
                        txtPrimaryEmail_Billing.Select(0,strval.Length);
                        return false;
                    }
                }

                if (txtAlternateEmail_Billing.Text.Trim().Length > 0)
                {
                    strval = txtAlternateEmail_Billing.Text.Trim();
                    if (!Globalitems.validemailaddress(strval))
                    {
                        MessageBox.Show("On the Billing Addr. tab, the Alternate Contact Email is not valid",
                            "INVALID EMAIL ADDRESS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtAlternateEmail_Billing.Focus();
                        txtAlternateEmail_Billing.Select(0, strval.Length);
                        return false;
                    }
                }

                if (txtPrimaryEmail_Street.Text.Trim().Length > 0)
                {
                    strval = txtPrimaryEmail_Street.Text.Trim();
                    if (!Globalitems.validemailaddress(strval))
                    {
                        MessageBox.Show("On the Street Addr. tab, the Primary Contact Email is not valid",
                            "INVALID EMAIL ADDRESS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPrimaryEmail_Street.Focus();
                        txtPrimaryEmail_Street.Select(0, strval.Length);
                        return false;
                    }
                }

                if (txtAlternateEmail_Street.Text.Trim().Length > 0)
                {
                    strval = txtAlternateEmail_Street.Text.Trim();
                    if (!Globalitems.validemailaddress(strval))
                    {
                        MessageBox.Show("On the Street Addr. tab, the Alternate Contact Email is not valid",
                            "INVALID EMAIL ADDRESS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtAlternateEmail_Street.Focus();
                        txtAlternateEmail_Billing.Select(0, strval.Length);
                        return false;
                    }
                }

                if (strMode == "NEW")
                {
                    //Check if Cust. Code is in use by another Customer ID
                    strSQL = @"SELECT CustomerID from Customer 
                        WHERE CustomerCode = '" + txtCustCode_record.Text + "'";
                    ds = DataOps.GetDataset_with_SQL(strSQL);                    
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "ValidRecord", "Checking Customer table " +
                            " for new Customer, Cust. code returned no table.");
                        return false;
                    }

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        strMessage = "The Customer Code is already used by another customer.\n" +
                            "It should be unique for each Customer.\n\n Are you sure you want to " +
                            "create \n" + 
                            "dupicate Customer Codes in the system?";
                        frmConfirm = new frmAreYouSure(strMessage);
                        var result = frmConfirm.ShowDialog();

                        if (result == DialogResult.Cancel)
                        {
                            txtCustCode_record.Text = "";
                            txtCustCode_record.Focus();
                            return false;
                        }
                    }
                }

                if (strMode == "MODIFY")
                {
                    //Check if nothing's changed   
                    if (changedlist_primary.Count == 0 && 
                        strMode_Billing == "MODIFY" &&
                        changedlist_billinginfo.Count == 0 &&
                        strMode_Street == "MODIFY" &&
                        changedlist_streetinfo.Count == 0)
                    {
                        MessageBox.Show("You have not changed anything for this Customer.\r\n" +
                           "There is nothing to update", "NO CHANGES MADE",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    //Check if Cust. Code is used by another Customer ID
                    strSQL = "SELECT CustomerID from Customer " +
                        "WHERE CustomerID <> " + txtCustID_record.Text +
                        " AND CustomerCode = '" + txtCustCode_record.Text.Trim() + "'";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "ValidRecord", 
                            "Checking Customer table " +
                            " for duplicate Cust. code returned no table.");
                        return false;
                    }

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        strMessage = "The Customer Code is already used by another customer.\n" +
                            "It should be unique for each Customer.\n\n Are you sure you want to " +
                            "create \n" +
                            "dupicate Customer Codes in the system?";
                        frmConfirm = new frmAreYouSure(strMessage);
                        var result = frmConfirm.ShowDialog();

                        if (result == DialogResult.Cancel)
                        {
                            txtCustCode_record.Text = "";
                            txtCustCode_record.Focus();
                            return false;
                        }
                    }
                }   // IF strMode = MODIFY 

                //Validate Billing Addr info 
                if (!InfoChangeValid("Billing", changedlist_billinginfo)) return false;

                //Validate Street Addr info
                if (!InfoChangeValid("Street", changedlist_streetinfo)) return false;

                return true;
            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidRecord", ex.Message);
                return false;
            }
        }

        private bool InfoChangeValid(string strInfotype,List<ControlInfo> lsChangedItems)
        {
            //Make sure changes to Billing & Street Addr are valid.
            //strMode_Billing & strMode_Street have initial mode set to 
            //  NEW/MODIFY in FillDetailRecord
            //Address Change may be:
            //  Customer Mode: NEW
            //  Billing Addr:[no value]/NEW
            //  Street Addr: [no value]/NEW/SAME
            //
            //  Customer Mode: MODIFY
            //  Billing Addr:[no change]/NEW/MODIFY/DELETE
            //  Street Addr: [no change]/NEW/MODIFY/SAME/DELETE

            try
            {
                DataSet ds;
                ComboBox cboBox = cboLocSubtype_Billing;
                ComboBox cboCountry = cboCountry_Billing;
                int intLocationID = 0;
                List<ControlInfo> lsControls = lsControls_BillingAddr;
                string strModeLoc = strMode_Billing;
                string strSQL;
                TextBox txtLocname = txtLocName_Billing;
                TextBox txtLocShortname = txtShortLocName_Billing;
                TextBox txtLocID = txtLocationID_Billing;

                if (strInfotype == "Street")
                {
                    lsControls = lsControls_StreetAddr;
                    txtLocname = txtLocName_Street;
                    txtLocShortname = txtShortLocName_Street;
                    txtLocID = txtLocationID_Street;
                    cboBox = cboLocSubtype_Street;
                    cboCountry = cboCountry_Street;
                    strModeLoc = strMode_Street;
                }


                if (txtLocID.Text.Trim().Length > 0)
                    intLocationID = Convert.ToInt32(txtLocID.Text);


                //Make sure the required Loc. info is present
                //If txtLocname has a value, must be unique,
                //If txtLocShortname has a value, must be unique 
                //SubType must be selected.
                if (txtLocname.Text.Trim().Length > 0)
                {
                    //Billing & Addr can't have same LocName
                    if (txtLocName_Street.Text.Trim().Length > 0)
                    {
                        if (txtLocName_Street.Text.Trim() ==
                            txtLocName_Billing.Text.Trim())
                        {
                            MessageBox.Show("On the Street Tab, the Location " +
                           "name must be different from the Billing Loc. name.",
                           "DUPLICATE LOCATION NAMES",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                           txtLocname.Focus();
                           txtLocname.Select(0, txtLocname.Text.Length);
                           return false;
                        }
                    }

                    //Make sure Loc. Name is unique among other locations
                    strSQL = "SELECT LocationName " +
                        "FROM Location " +
                        "WHERE LocationName = '" + 
                        Globalitems.HandleSingleQuoteForSQL(txtLocname.Text.Trim()) +
                        "' and LocationID <> " + intLocationID.ToString();
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "InfoChangeValid",
                            "No table returned from querying Location table for Loc. Name");
                        return false;
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("On the " + strInfotype + " Tab, the Location " +
                            "name must be unique.\n\n" +
                            "One or more other Locations have the same name",
                            "DUPLICATE LOCATION NAMES",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtLocname.Focus();
                        return false;
                    }

                    //Make sure Loc. Short Name is unique
                    if (txtLocShortname.Text.Trim().Length > 0)
                    {
                        //Billing & Addr can't have same LocShortName
                        if (txtShortLocName_Street.Text.Trim().Length > 0)
                        {
                            if (txtShortLocName_Street.Text.Trim() ==
                                txtShortLocName_Billing.Text.Trim())
                            {
                                MessageBox.Show("On the Street Tab, the Location " +
                               "Short name must be different from the Billing Loc. Short name.",
                               "DUPLICATE LOCATION SHORT NAMES",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                               txtLocShortname.Focus();
                               txtLocShortname.Select(0, txtLocShortname.Text.Length);
                               return false;
                            }
                        }

                        strSQL = "SELECT LocationShortName " +
                        "FROM Location " +
                        "WHERE LocationShortName = '" + 
                        Globalitems.HandleSingleQuoteForSQL(txtLocShortname.Text.Trim()) +
                        "' and LocationID <> " + intLocationID.ToString();
                        ds = DataOps.GetDataset_with_SQL(strSQL);
                        if (ds.Tables.Count == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "InfoChangeValid",
                                "No table returned from querying Location table " +
                                "for Loc. Short Name");
                            return false;
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show("On the " + strInfotype + " Tab, the Location " +
                                "Short name must be unique.\n\n" +
                                "One or more other Locations have the same short name",
                                "DUPLICATE LOCATION SHORT NAMES",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtShortName.Focus();
                            txtShortName.Select(0, txtShortName.Text.Length);
                            return false;
                        }
                    }

                    //Make sure Loc. Sub Type is selected
                    if ((cboBox.SelectedItem as ComboboxItem).cboValue == "select")
                    {
                        MessageBox.Show("On the " + strInfotype + " Tab, you must select " +
                            "the Location Sub Type", "MISSING LOCATION SUB TYPE",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cboBox.Focus();
                        return false;
                    }

                }   // if txtLocname.Length > 0

                //Check if Locname is blank, but other values entered
                if (txtLocname.Text.Trim().Length == 0 && AddrSectionHasInfo(strInfotype))
                { 
                    MessageBox.Show("On the " + strInfotype + " Tab, you must enter " +
                        "the Location Name", "MISSING LOCATION NAME",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLocname.Focus();
                    return false;
                }
                
                // NEW Customer, strModLoc was already set to NEW in NewRecordSetup
                // Location action may need to change to:  ""[do nothing], SAME
                if (strMode == "NEW")
                {
                    //If no Billing Loc name, do nothing
                    if (txtLocname.Text.Trim().Length == 0)
                    {
                        if (strInfotype == "Billing")
                            strMode_Billing = "";
                        else
                        {
                            //If no Street Loc name, could be do nothing or SAME
                            if (ckSameAsBilling.Checked)
                                strMode_Street = "SAME";
                            else
                                strMode_Street = "";
                        }
                    }

                    return true;
                }

                //MODIFY Customer. strMod_Billing/Street was pre-set in FillDetailRecord
                //If strModeLoc is NEW: may need to change to ""[do nothing], SAME (for Street)
                //If strModLoc is MODIFY: may need to change to:  ""[do nothing],SAME, DELETE
                //If strModLoc is SAME: may need to change to:  ""[do nothing],NEW, DELETE
                if (strMode == "MODIFY")
                {
                    //If Loc change is NEW
                    //Check if no change to Section or SAME for Street Addr
                    if (strModeLoc == "NEW")
                    {
                        if (!AddrSectionHasInfo(strInfotype))
                        {
                            if (strInfotype == "Billing")
                                strMode_Billing = ""; //no Billing loc info to add
                            else
                            {
                                if (ckSameAsBilling.Checked)
                                    strMode_Street = "SAME"; //Set same as Billing info
                                else
                                    strMode_Street = ""; // no Street loc info to add
                            }
                        }

                        return true;
                    }

                    if (strModeLoc == "MODIFY")
                    {
                        //strModeLoc is MODIFY
                        //Check for "" [no change]
                        if (lsChangedItems.Count == 0)
                        {
                            if (strInfotype == "Billing")
                                strMode_Billing = "";
                            else
                                strMode_Street = "";
                            return true;
                        }

                        //strModeLoc is MODIFY
                        //Check for SAME
                        if (strInfotype == "Street" && ckSameAsBilling.Checked)
                        {
                            strMode_Street = "SAME";
                            return true;
                        }

                        //strModeLoc is MODIFY
                        //Check for DELETE
                        if (!AddrSectionHasInfo(strInfotype) && lsChangedItems.Count > 0)
                        {
                            if (strInfotype == "Billing")
                                strMode_Billing = "DELETE";
                            else
                                strMode_Street = "DELETE";

                            return true;
                        }
                    }   // If strModeLoc = MODIFY

                    if (strInfotype == "Street" && strModeLoc == "SAME")
                    {
                        //strModeLoc is SAME
                        //Check for "" [no change]
                        if (ckSameAsBilling.Checked)
                        {
                            strMode_Street = "";
                            return true;
                        }

                        //strModeLoc is SAME
                        //Check for NEW, RESET (don't use DELETE because Loc ID must remain for
                        //  BillindAddress
                        if (txtLocname.Text.Trim().Length > 0)
                        {
                            strMode_Street = "NEW";
                            return true;
                        }
                        else
                            strMode_Street = "RESET";
                        return true;
                    }   //If strModeLoc = SAME


                }  // If strMode = MODIFY   
                             
                  
                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "InfoChangeValid", ex.Message);
                return false;
            }
        }

        private void CancelSetup()
        {
            try
            {
                int intCurrentBSPosition = -1;
                strMode = "READ";

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
                Formops.ClearRecordData(this, lsControls_primary);
                Formops.ClearRecordData(this, lsControls_BillingAddr);
                Formops.ClearRecordData(this, lsControls_StreetAddr);

                //6. Set recbuttons with no records to display, & recbuttons to READONLY
                recbuttons.blnRecordsToDisplay = false;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);

                //7. Enable Menu, Search, Clear buttons
                btnMenu.Enabled = true;
                btnSearch.Enabled = true;
                btnClear.Enabled = true;

                //7. Store Binding Source position if there are values, set record details, set Nav Buttons
                if (bs1.Count > 0) intCurrentBSPosition = bs1.Position;
                if (strMode == "MODIFY") intCurrentBSPosition = bs1.Position;

                //8. If dgResults has 1 or more rows: enable btnExport,reset navbuttons, reset record details 
                if (dgResults.RowCount > 0)
                {
                    btnExport.Enabled = true;
                    recbuttons.blnRecordsToDisplay = true;
                    recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                    Globalitems.SetNavButtons(recbuttons, bs1);
                    FillDetailRecord(lsCustomerIDs[intCurrentBSPosition]);
                }

                blnIgnoreZipcode = false;
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CancelSetup", ex.Message);
            }
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
                btnSearch.Enabled = false;
                btnClear.Enabled = false;
                btnExport.Enabled = false;

                //4. Hide other open forms
                Globalitems.DisplayOtherForms(this, false);

                //5. Turn off ReadOnly on form's controls
                AdjustReadOnlyStatus(false);

                //6. Reset form's controls to Updated = false
                Formops.ResetControls(this, lsControls_primary);

                Formops.ResetControls(this, lsControls_BillingAddr);

                Formops.ResetControls(this, lsControls_StreetAddr);

                //7. Set recbuttons to Modify - place at end so SetReadOnlyStatus of 
                //  Street controls doesn't set the recbuttons to READ ONLY
                

                //8. Set Updated By/Date to new value
                txtUpdatedBy.Text = Globalitems.strUserName;
                txtUpdatedDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

                //9. Set focus on first control
                txtCustName_record.Focus();

                //10. Handle Form unique controls
                //If ckSameAsBilling is checked, disable Street controls
                if (ckSameAsBilling.Checked)
                    Formops.SetReadOnlyStatus(this, lsControls_StreetAddr, true, recbuttons);

                //Set here for Modify mode 
                recbuttons.SetButtons(RecordButtons.ACTION_MODIFYRECORD);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ModifyRecordSetup", ex.Message);
            }
        }

        private void AdjustReadOnlyStatus(bool blnReadOnly)
        {
            Formops.SetReadOnlyStatus(this, lsControls_primary, blnReadOnly, recbuttons);
            Formops.SetReadOnlyStatus(this, lsControls_BillingAddr, blnReadOnly, recbuttons);
            Formops.SetReadOnlyStatus(this, lsControls_StreetAddr, blnReadOnly, recbuttons);

            ckSameAsBilling.Enabled = !blnReadOnly;
            btnClearBillingAddr.Enabled = !blnReadOnly;
            btnClearStreetAddr.Enabled = !blnReadOnly;

            btnAddRate.Enabled = !blnReadOnly;
            btnModifyRate.Enabled = !blnReadOnly;
            btnDeleteRate.Enabled = !blnReadOnly;

            //Disable Mod/Del Rate btns, if no rates for the customer
            if (!blnReadOnly && dgRates.RowCount == 0)
            {
                btnDeleteRate.Enabled = false;
                btnModifyRate.Enabled = false;
            }
        }

        private void NewRecordSetup()
        {
            //Set before mode change, so ckSameAsBilling_CheckedChanged does nothing
            ckSameAsBilling.Checked = false;

            //1. Set Mode
            strMode = "NEW";
            strMode_Billing = "NEW";
            strMode_Street = "NEW";

            //2. Set Status label
            lblStatus.Text = "Add new record";

            //3. Disable Search/Results panels, & related buttons
            pnlSearch.Enabled = false;
            pnlResults.Enabled = false;
            btnSearch.Enabled = false;
            btnClear.Enabled = false;
            btnExport.Enabled = false;

            //4. Hide Other Forms
            Globalitems.DisplayOtherForms(this, false);

            //5. Turn off ReadOnly on form's controls
            AdjustReadOnlyStatus(false);

            //6. Clear Record controls
            Formops.ClearRecordData(this, lsControls_primary);
            Formops.ClearRecordData(this, lsControls_BillingAddr);
            Formops.ClearRecordData(this, lsControls_StreetAddr);

            //7. Set recbuttons to New
            recbuttons.SetButtons(RecordButtons.ACTION_NEWRECORD);

            //8. Set Created By/Date 
            txtCreatedBy.Text = Globalitems.strUserName;
            txtCreationDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

            //9. Set focus on first contol
            txtCustName_record.Focus();

            //10. Handle Form unique controls
            cboStatus_record.SelectedValue = "Active";
            dgRates.DataSource = "";

            //Set up blank dtNewRates table with FillRatesGrid method
            txtCustID_record.Text = "0";
            FillRatesGrid();
        }

        private void PerformDeleteRecord()
        {
            try
            {
                DataSet ds;
                frmAreYouSure frmConfirm;
                string strMessage;
                string strSQL;

                //Check that the AutoportExportVehicles table has no records for the CustomerID
                strSQL = @"SELECT COUNT(CustomerID) AS totrecs FROM AutoportExportVehicles 
                    WHERE CustomerID = " + txtCustID_record.Text;
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord",
                        "No data returned from the AutoportExportVehicles table when checking " +
                        "Customer IDs");
                    return;
                }

                if (Convert.ToInt32(ds.Tables[0].Rows[0]["totrecs"]) > 0)
                {
                    MessageBox.Show("The customer cannot be deleted because there are " +
                        "associated Export vehicles in the DB.", "CUSTOMER CANNOT BE DELETED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Check that the Billing table has no records for the CustomerID
                strSQL = @"SELECT COUNT(CustomerID) AS totrecs FROM Billing 
                    WHERE CustomerID = " + txtCustID_record.Text;
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord",
                        "No data returned from the Billing table when checking " +
                        "Customer IDs");
                    return;
                }

                if (Convert.ToInt32(ds.Tables[0].Rows[0]["totrecs"]) > 0)
                {
                    MessageBox.Show("The customer cannot be deleted because there are " +
                        "associated Invoice records in the DB.", "CUSTOMER CANNOT BE DELETED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strMessage = "You are about to Delete a Customer!\n\n" +
                    "You could instead change the Status to Inactive.\n\n" +
                    "Are you sure you want to delete the customer:\n" +
                    txtCustName_record.Text + "?";

                frmConfirm = new frmAreYouSure(strMessage);
                var result = frmConfirm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    //Delete the Rates
                    strSQL = "DELETE AutoportExportRates WHERE CustomerID = " + 
                        txtCustID_record.Text;
                    DataOps.PerformDBOperation(strSQL);

                    //Delete the Locations
                    strSQL = "DELETE Location WHERE ParentRecordID = " +
                        txtCustID_record.Text;
                    DataOps.PerformDBOperation(strSQL);

                    //Delete the Customer
                    strSQL = "DELETE Customer WHERE CustomerID = " + 
                        txtCustID_record.Text;
                    DataOps.PerformDBOperation(strSQL);

                    PerformSearch();

                    MessageBox.Show("The customer has been removed from the DB", "CUSTOMER DELETED",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord", ex.Message);
            }
        }

        private void PerformMovePrevious()
        {
            try
            {
                bs1.MovePrevious();
                FillDetailRecord(lsCustomerIDs[bs1.Position]);
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMovePrevious", ex.Message);
            }
            
        }

        private void PerformMoveNext()
        {
            try
            {
                bs1.MoveNext();
                FillDetailRecord(lsCustomerIDs[bs1.Position]);
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMoveNext", ex.Message);
            }
        }
     
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try { PerformSearch(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnSearch_Click", ex.Message); }
        }

        private void ClearGridView()
        {
            try
            {
                lblCustRecords.Text = "";
                lblRateRecords.Text = "";

                dtCustomers.Clear();

                // Binding dgResults to lsUsers after the Clear method, can lead to runtime error because
                //  the CurrencyManager pointing to the Current position in lsUsers, doesn't reset to -1
                dgResults.DataSource = dtCustomers;
                
                //Clear dgRates if they're visible
                if (!Globalitems.blnHideRates) dgRates.DataSource = "";
               
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
                Formops.ClearSetup(this, lsControls_primary);
                Formops.ClearSetup(this, lsControls_BillingAddr);
                Formops.ClearSetup(this, lsControls_StreetAddr);

                //2. Clear Form unique grids
                ClearGridView();

                //3. Set Form unique Readonly/enabled status for controls
                AdjustReadOnlyStatus(true);
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearSetup", ex.Message);
            }
        }

        private void FillCombos()
        {
            try
            {
                ComboboxItem cboItem;
                string strFilter;

                // Fill cboStatus, cboStatus_record 
                strFilter = "CodeType = 'RecordStatus'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboStatus, true, false);
                Globalitems.FillComboboxFromCodeTable(strFilter, cboStatus_record, false, true);

                // Fill cboPaymentMethod
                strFilter = "CodeType='PaymentMethod'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboPaymentMethod, true, false);

                //Change 1st item from All to <select>
                cboItem = (ComboboxItem)cboPaymentMethod.Items[0];
                cboItem.cboText = "<select>";
                cboItem.cboValue = "select";
                cboCountry_Billing.SelectedIndex = -1;

                //Fill cboCountryCode
                strFilter = "CodeType = 'CountryCode'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboCountry_Billing, true, false);

                //Change 1st item from All to <select>
                cboItem = (ComboboxItem) cboCountry_Billing.Items[0];
                cboItem.cboText = "<select>";
                cboItem.cboValue = "select";
                cboCountry_Billing.SelectedIndex = -1;

                Globalitems.FillComboboxFromCodeTable(strFilter, cboCountry_Street, true, false);

                //Change 1st item from All to <select>
                cboItem = (ComboboxItem)cboCountry_Street.Items[0];
                cboItem.cboText = "<select>";
                cboItem.cboValue = "select";
                cboCountry_Street.SelectedIndex = -1;

                //Fill cboLocSubType
                strFilter = "CodeType = 'LocationSubType'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboLocSubtype_Billing, true, false);

                //Change 1st item from All to <select>
                cboItem = (ComboboxItem)cboLocSubtype_Billing.Items[0];
                cboItem.cboText = "<select>";
                cboItem.cboValue = "select";
                cboLocSubtype_Billing.SelectedIndex = -1;

                Globalitems.FillComboboxFromCodeTable(strFilter, cboLocSubtype_Street, true, false);

                //Change 1st item from All to <select>
                cboItem = (ComboboxItem)cboLocSubtype_Street.Items[0];
                cboItem.cboText = "<select>";
                cboItem.cboValue = "select";
                cboLocSubtype_Street.SelectedIndex = -1;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void PerformSearch()
        {
            //Per K. Collins, Handheld Customer ID doesn't need to be exported as a text file.
            //  Updated Handheld program, retrieves info needed from DATS Customer table

            DataSet ds;
            string strSQL;
            string strval;

            try
            {
                //1. Disable Export button
                btnExport.Enabled = false;

                //2. Clear Results gridview
                ClearGridView();

                //3. Clear Record data
                Formops.ClearRecordData(this, lsControls_primary);
                Formops.ClearRecordData(this, lsControls_BillingAddr);
                Formops.ClearRecordData(this, lsControls_StreetAddr);

                //4. Set recbuttons to display = false
                recbuttons.blnRecordsToDisplay = false;

                //5. Retrieve data as datatable
                strSQL = @"SELECT c.CustomerID,c.CustomerCode," +
                "CASE " +
                "WHEN LEN(RTRIM(ISNULL(C.ShortName, ''))) > 0 THEN RTRIM(C.ShortName) " +
                "ELSE RTRIM(ISNULL(c.CustomerName,'')) " +
                "END AS CustName," +
                 "RTRIM(ISNULL(l.AddressLine1,'')) AS AddressLine1," +
                 "RTRIM(ISNULL(l.City,'')) AS City," +
                 "RTRIM(ISNULL(l.State,'')) AS State," +
                 "RTRIM(ISNULL(l.Zip,'')) AS Zip," +
                 "RTRIM(ISNULL(c.CustomerName,'')) AS CustomerName," +
                 "RTRIM(ISNULL(c.ShortName,'')) AS ShortName," +
                 "RTRIM(ISNULL(c.DBAName,'')) AS DBAName," +
                 "RTRIM(ISNULL(c.CustomerType,'')) AS CustomerType," +
                 "RTRIM(ISNULL(c.CustomerOf,'')) AS CustomerOf," +
                 "RTRIM(ISNULL(l.AddressLine2,'')) AS AddressLine2," +
                 "RTRIM(ISNULL(l.Country,'')) AS Country," +
                 "RTRIM(ISNULL(l.MainPhone,'')) AS MainPhone," +
                 "RTRIM(ISNULL(l.FaxNumber,'')) AS FaxNumber," +
                 "c.RecordStatus," +
                 "CASE " +
                 "WHEN l.PrimaryContactFirstName IS NOT NULL " +
                 "  OR PrimaryContactLastName IS NOT NULL THEN " +
                 "  RTRIM(ISNULL(PrimaryContactFirstName,'')) + ' ' + " +
                 "RTRIM(ISNULL(PrimaryContactLastName,'')) " +
                 "ELSE '' " +
                 "END AS BillingPrimaryContact," +
                 "RTRIM(ISNULL(PrimaryContactPhone,'')) AS BillingPrimaryPhone," +
                 "RTRIM(ISNULL(PrimaryContactEmail,'')) AS PrimaryContactEmail " +
                "FROM Customer c " +
                "LEFT OUTER JOIN Location l ON c.MainAddressID = l.LocationID " +
                "WHERE LEN(RTRIM(ISNULL(c.CustomerName,''))) > 0  ";

                //Add Customer Name to WHERE clause 
                if (txtCustName.Text.Trim().Length > 0)
                {
                    strval = Globalitems.HandleSingleQuoteForSQL(txtCustName.Text.Trim());
                    strSQL += "AND (c.CustomerName LIKE '" + strval + "%' " +
                        "OR c.ShortName LIKE '" + strval + "%') ";
                }

                //Add Customer Code to WHERE clause 
                if (txtCustCode.Text.Trim().Length > 0)
                {
                    strval = Globalitems.HandleSingleQuoteForSQL(txtCustCode.Text.Trim());
                    strSQL += "AND c.CustomerCode LIKE '" + strval + "%' ";
                }

                if ((cboStatus.SelectedItem as ComboboxItem).cboValue != "All")
                {
                    strval = (cboStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    strval = Globalitems.HandleSingleQuoteForSQL(strval);
                    strSQL += "AND c.RecordStatus ='" + strval + "' ";
                }

                //Add City to WHERE clause 
                if (txtCity.Text.Trim().Length > 0)
                {
                    strval = Globalitems.HandleSingleQuoteForSQL(txtCity.Text.Trim());
                    strSQL += "AND l.City LIKE '" + strval + "%' ";
                }

                //Add State to WHERE clause 
                if (txtState.Text.Trim().Length > 0)
                {
                    strval = Globalitems.HandleSingleQuoteForSQL(txtState.Text.Trim());
                    strSQL += "l.State LIKE '" + strval + "%' ";
                }

                //Add Zip to WHERE clause 
                if (txtZip.Text.Trim().Length > 0)
                {
                    strval = Globalitems.HandleSingleQuoteForSQL(txtZip.Text.Trim());
                    strSQL += "l.Zip LIKE '" + strval + "%' ";
                }

                strSQL += " ORDER BY c.CustomerCode";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                
                // Use a DataTable as the DataSource for the DataGridView to make sorting by Col Header
                //  clicks, automatic
                dtCustomers = ds.Tables[0].Copy();
                if (dtCustomers.Rows.Count == 0) return;

                //6. If data found:
                //6a. Enable Export button
                btnExport.Enabled = true;

                //6b. Assign Datatable to gridvirew
                dgResults.DataSource = dtCustomers;

                //6c. Update # records label
                lblCustRecords.Text =  "Records: " + dtCustomers.Rows.Count;

                //6d. Because dgResults is not multiselect, 
                //create a list of 1 CustomerID for the Form's binding source, 
                //for use by the nav buttons
                lsCustomerIDs.Clear();
                lsCustomerIDs.Add(int.Parse(dtCustomers.Rows[0]["CustomerID"].ToString())); 

                bs1.DataSource = lsCustomerIDs;

                //6e. Fill detail record with first row CustomerID
                FillDetailRecord(lsCustomerIDs[0]);

                //6f. Update recbuttons
                recbuttons.blnRecordsToDisplay = true;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                Globalitems.SetNavButtons(recbuttons,bs1);            
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSearch", ex.Message);
            }
        }

        private void ProcessStreetSameAsBillingInfo()
        {
            try
            {
                if (strMode == "READ") return;

                //If User checks ckSameAsBilling, clear Street Addr info, and disable Street controls
                if (ckSameAsBilling.Checked)
                {
                    //Verify that Billing Addr has info
                    if (txtLocName_Billing.Text.Trim().Length == 0)
                    {
                        ckSameAsBilling.Checked = false;
                        MessageBox.Show("There is no Location Name on the Billing Address Tab.\n\n" +
                            "To make the Street Address the same as the Billing Address," +
                            "there must be sufficient info on the Billing Address Tab",
                            "INSUFFICIENT BILLING ADDRESS INFO",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Formops.ClearRecordData(this, lsControls_StreetAddr);
                    Formops.SetReadOnlyStatus(this, lsControls_StreetAddr, true, recbuttons);

                    btnClearStreetAddr.Enabled = false;
                }
                else
                {
                    //User unchecked ckSameAsBilling, enable Street controls
                    Formops.SetReadOnlyStatus(this, lsControls_StreetAddr, false, recbuttons);

                    btnClearStreetAddr.Enabled = true;
                }

                //Record the ckSameAsBilling has changed
                Formops.ChangeControlUpdatedStatus("ckSameAsBilling", lsControls_StreetAddr);

                //Reset recbuttons
                if (strMode == "NEW")
                    recbuttons.SetButtons(RecordButtons.ACTION_NEWRECORD);
                else
                    recbuttons.SetButtons(RecordButtons.ACTION_MODIFYRECORD);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ProcessStreetSameAsBilling",
                    ex.Message);
            }
        }

        private void OpenCSVFile()
        {
            try
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
                        "WHERE ValueKey IN ('ExportDirectory','CustomerExportFileName') " +
                        "AND RecordStatus='Active' ORDER BY ValueKey DESC";
                    ds = DataOps.GetDataset_with_SQL(strSQL);

                    // S/B just two active rows, row 1 ExportDirectory, row 2 CustomerExportFileName
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 2)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile",
                            "No rows returned from SettingTable");
                        return;
                    }
                    // 1st Record s/b ExportDirectory, 2nd Record s/b CustomerExportFileName
                    strFilename = ds.Tables[0].Rows[0]["ValueDescription"].ToString();
                    strFilename += @"\" + ds.Tables[0].Rows[1]["ValueDescription"].ToString();

                    //2. Create a copy of the datatable used for the datagridview datasource
                    dt = dtCustomers.Copy();

                    //3. If the gridview is sorted, use a dv to sort the table copy the same way
                    if (dgResults.SortedColumn != null)
                    {
                        strSort = dgResults.SortedColumn.DataPropertyName;
                        if (dgResults.SortOrder == SortOrder.Descending) strSort += " DESC";
                        dv = new DataView(dt, "", strSort, DataViewRowState.CurrentRows);
                        dt = dv.ToTable();
                    }

                    //4. Create a list, lsCSVcols with ControlInfo objects in the order to appear in the csv file 
                    //Get ctrlinfo object from lsControls for UserCode & add to lsCSVcols. Use HeaderText to ID objects
                    var objctrlinfo_CustCode = lsControls_primary.First(obj => obj.HeaderText == "Cust. Code");
                    lsCSVcols.Add(objctrlinfo_CustCode);

                    var objctrlinfo_CustName = lsControls_primary.First(obj => obj.HeaderText == "Cust. Name");
                    lsCSVcols.Add(objctrlinfo_CustName);

                    var objctrlinfo_Street = lsControls_primary.First(obj => obj.HeaderText == "Street Addr.");
                    lsCSVcols.Add(objctrlinfo_Street);

                    var objctrlinfo_City = lsControls_primary.First(obj => obj.HeaderText == "City");
                    lsCSVcols.Add(objctrlinfo_City);

                    var objctrlinfo_ST = lsControls_primary.First(obj => obj.HeaderText == "ST");
                    lsCSVcols.Add(objctrlinfo_ST);

                    var objctrlinfo_Zip = lsControls_primary.First(obj => obj.HeaderText == "Zip");
                    lsCSVcols.Add(objctrlinfo_Zip);

                    var objctrlinfo_Status = lsControls_primary.First(obj => obj.HeaderText == "Status");
                    lsCSVcols.Add(objctrlinfo_Status);

                    //5. Invoke CreateSCVFile to create, save, & open the csv file
                    Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
                }

                catch (Exception ex)
                {
                    Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile", ex.Message);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile", ex.Message);
            }
        }

        private void FillDetailRecord(int intCustomerID)
        {
            try
            {
                DataRow dr;
                DataSet ds;

                Int64 intTel;

                SProcParameter Paramobj;
                List<SProcParameter> Paramobjects = new List<SProcParameter>();

                Paramobj = new SProcParameter();
                Paramobj.Paramname = "@CustomerID";
                Paramobj.Paramvalue = intCustomerID;
                Paramobjects.Add(Paramobj);
                string strCountry;
                string strSproc = "spGetCustomerAdminInfo";
                string strval;

                txtCustID_record.Text = intCustomerID.ToString();

                //Get the Record details for intCustomerID
                ds = DataOps.GetDataset_with_SProc(strSproc, Paramobjects);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                dr = ds.Tables[0].Rows[0];
                
                //loop through lsControls to set record controls
                Formops.ClearRecordData(this,lsControls_primary);
                Formops.ClearRecordData(this, lsControls_BillingAddr);
                Formops.ClearRecordData(this, lsControls_StreetAddr);
                Formops.SetDetailRecord(dr, this, lsControls_primary);
                Formops.SetDetailRecord(dr, this, lsControls_BillingAddr);

                //Set strMode_Billing, strMode_Street based on LocationIDs found
                strMode_Billing = "NEW";
                if (txtLocationID_Billing.Text.Length > 0) strMode_Billing = "MODIFY";

                //If Street Addr is same As Billing Addr, set ckSameAsBilling, strMode_Street
                ckSameAsBilling.Checked = false;
                if (dr["MainAddressID"] != null && dr["BillingAddressID"] != null &&
                   dr["MainAddressID"].ToString().Length > 0 &&
                   dr["MainAddressID"].ToString() == dr["BillingAddressID"].ToString())
                {
                    ckSameAsBilling.Checked = true;
                    strMode_Street = "SAME";
                }
                    
                else
                {
                    //Fill Street Addr info
                    Formops.SetDetailRecord(dr, this, lsControls_StreetAddr);

                    strMode_Street = "NEW";
                    if (txtLocationID_Street.Text.Length > 0) strMode_Street = "MODIFY";
                }

                //Set date time formats
                txtCreationDate.Text = Globalitems.FormatDatetime(txtCreationDate.Text);
                txtUpdatedDate.Text = Globalitems.FormatDatetime(txtUpdatedDate.Text);

                //Set phone formats, if lenght is 10 and country is US
                strCountry = "";
                if ((cboCountry_Billing.SelectedItem as ComboboxItem).cboValue == "U.S.A.")
                    strCountry = "US";

                strval = txtMainPhone_Billing.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval,out intTel))
                    txtMainPhone_Billing.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtMainFax_Billing.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtMainFax_Billing.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtPrimaryPhone_Billing.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 
                    && Int64.TryParse(strval, out intTel))
                    txtPrimaryPhone_Billing.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtPrimaryCell_Billing.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtPrimaryCell_Billing.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtAlternatePhone_Billing.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtAlternatePhone_Billing.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtAlternateCell_Billing.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtAlternateCell_Billing.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtOtherPhone1_Billing.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 
                    && Int64.TryParse(strval, out intTel))
                    txtOtherPhone1_Billing.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtOtherPhone2_Billing.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtOtherPhone2_Billing.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtMainPhone_Street.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtMainPhone_Street.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtMainFax_Street.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtMainFax_Street.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtPrimaryPhone_Street.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtPrimaryPhone_Street.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtPrimaryCell_Street.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtPrimaryCell_Street.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtAlternatePhone_Street.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtAlternatePhone_Street.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtAlternateCell_Street.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtAlternateCell_Street.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtOtherPhone1_Street.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtOtherPhone1_Street.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtOtherPhone2_Street.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 && 
                    Int64.TryParse(strval, out intTel))
                    txtOtherPhone2_Street.Text = String.Format("{0:(###) ###-####}", intTel);

                if (!Globalitems.blnHideRates) FillRatesGrid();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillDetailRecord", ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            { ClearForm(); }
        }

        private void FillRatesGrid()
        {
            try
            {
                DataSet ds;
                string strSQL;

                strSQL = "SELECT AutoportExportRatesID, CustomerID," + 
                    "EntryFee, PerDiem, PerDiemGraceDays," +
                    "StartDate, EndDate,CreationDate,CreatedBy," +
                    "UpdatedDate,UpdatedBy,RateType " +
                    "FROM AutoportExportRates " +
                    "WHERE CustomerID = " + txtCustID_record.Text + 
                    " ORDER BY RateType, StartDate DESC," +
                    "EndDate DESC";

                ds = DataOps.GetDataset_with_SQL(strSQL);

                lblRateRecords.Text =  "Records: " + ds.Tables[0].Rows.Count.ToString();

                dtOriginalRates = ds.Tables[0];
                dgRates.DataSource = dtOriginalRates;

                //Make copy of original rates to handle User changes
                dtNewRates = dtOriginalRates.Copy();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillRatesGrid", ex.Message);
            }
        }

        private void DeleteRate()
        {
            try
            {
                frmAreYouSure frmConfirm;
                DataView dv;
                string strFilter;
                string strMessage;
                string strRateID;

                if (dgRates.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a rate to delete", "NO ROW SELECTED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strMessage = "Are you sure you want to delete this Rate?";
                frmConfirm = new frmAreYouSure(strMessage);
                var result = frmConfirm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    blnRatesChanged = true;

                    //Get the RateID, Cell[0], from the selected row
                    strRateID = dgRates.SelectedRows[0].Cells[0].EditedFormattedValue.ToString();

                    //Remove the row from dtNewRates
                    strFilter = "AutoportExportRatesID = " + strRateID;
                    dv = new DataView(dtNewRates, strFilter, "AutoportExportRatesID",
                        DataViewRowState.CurrentRows);
                    dv[0].Delete();

                    dgRates.DataSource = dtNewRates;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "DeleteRate", ex.Message);
            }
        }

        private void btnDeleteRate_Click(object sender, EventArgs e)
        {
            try
            {DeleteRate();  }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "btnDeleteRate_Click", ex.Message);
            }
        }

        private void txtCustName_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtCustName_record",lsControls_primary);
        }

        private void txtCustCode_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtCustCode_record",lsControls_primary);
        }

        private void txtShortName_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtShortName",lsControls_primary);
        }

        private void txtDBAName_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtDBAName",lsControls_primary);
        }

        private void cboPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboPaymentMethod", lsControls_primary);
        }

        private void cboStatus_record_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboStatus_record", lsControls_primary);
        }

        private void ckEmail_CheckedChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("ckEmail", lsControls_primary);
        }

        private void txtBookingPrefix_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtBookingPrefix", lsControls_primary);
        }

        private void txtHandheldCustID_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtHandheldCustID", lsControls_primary);
        }

        private void txtInternalComment_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtInternalComment", lsControls_primary);
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }

        private void FormatResultsGrid()
        {
            try
            {
                DataGridViewButtonCell dgButtonCell;

                foreach (DataGridViewRow dgRow in dgResults.Rows)
                {
                    dgRow.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    string strCellToCheck = dgRow.Cells[5].Value.ToString();

                    dgButtonCell = (DataGridViewButtonCell)dgRow.Cells[1];
                    

                    // Set default text for button to Delivered
                    dgRow.Cells[1].Value = "Delivered";

                    if (strCellToCheck == "Beirut")
                    {
                        dgRow.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                        dgRow.Cells[1].Value = "EnRoute";
                        
                    }

                    if (strCellToCheck == "Iselin")
                    {
                        dgRow.DefaultCellStyle.ForeColor = System.Drawing.Color.Purple;
                        dgRow.Cells[1].Value = "";
                    }
                }
            }

            catch (Exception ex)
            {
                string strex = ex.Message;
            }
        }

        private void AddRate()
        {
            //Pass to frmRTAdmin a new Datarow from dtNewRates, & this form.
            //If frmRTAdmin returns the DialogResult OK, the datarow has been filled in,
            //  & blnRatesChanged set to true
            try
            {
                DataView dv;
                DialogResult dlResult;
                DataRow drow = dtNewRates.NewRow();
                DataTable dtTmp;
                frmRateAdmin frmRTAdmin;

                frmRTAdmin = new frmRateAdmin("Add New Rate",
                    Convert.ToInt32(txtCustID_record.Text), 
                    txtCustName_record.Text,drow,this);
                dlResult = frmRTAdmin.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    //Add the new row to dtNewRates
                    dtNewRates.Rows.Add(drow);

                    //Sort dtNewRates BY RateType, StartDate DESC,EndDate DESC as dtTmp
                    dv = new DataView(dtNewRates, "CustomerID IS NOT NULL",
                        "RateType, StartDate DESC,EndDate DESC",
                        DataViewRowState.CurrentRows);
                    dtTmp = dv.ToTable();
                    dtNewRates.Clear();
                    dtNewRates = dtTmp.Copy();
                    dgRates.DataSource = dtNewRates;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "AddLocation", ex.Message);
            }
        }

        private void ModifyRate()
        {
            //Pass to frmRTAdmin the Datarow to modify from dtNewRates, & this form.
            //If frmRTAdmin returns the DialogResult OK, the datarow has been updated,
            //  & blnRatesChanged set to true
            try
            {
                DateTime datCreationDate;
                DialogResult dlResult;
                DataRow drowselected = null;
                frmRateAdmin frmRTAdmin;
                int intRateID;
                string strRateType;
                
                if (dgRates.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a rate to modify", "NO ROW SELECTED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Get the RateID, Cell[ExportRatesID], from the selected row
                intRateID = Convert.ToInt32(dgRates.SelectedRows[0].Cells["ExportRatesID"].EditedFormattedValue.ToString());

                //Find the Datarow in dtNewRates
                foreach (DataRow drow in dtNewRates.Rows)
                {
                    //Bypass Deleted rows
                    if (drow.RowState != DataRowState.Deleted)
                    {
                        //Prev. existing rate, has RateID
                        if (intRateID != 0)
                        {
                            if (Convert.ToInt32(drow["AutoportExportRatesID"].ToString()) ==
                            intRateID)
                                drowselected = drow;
                        }
                        else
                        {
                            //New Rate, created with RateID = 0, ID by RateType, CreationDate
                            strRateType = dgRates.SelectedRows[0].Cells["RateType"].EditedFormattedValue.ToString();
                            datCreationDate = (DateTime) dgRates.SelectedRows[0].Cells["CreationDate"].Value;
                                if ((string) drow["RateType"] == strRateType &&
                                       (DateTime) drow["CreationDate"] == datCreationDate)
                                    drowselected = drow;
                        }
                    }
                }

                frmRTAdmin = new frmRateAdmin("Modify Rate",
                    Convert.ToInt32(txtCustID_record.Text),
                    txtCustName_record.Text, drowselected, this);
                    
                dlResult = frmRTAdmin.ShowDialog();
                if (dlResult == DialogResult.OK) dgRates.DataSource = dtNewRates;                
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ModifyRate", ex.Message);
            }
        }

        private void btnMenu_Click_1(object sender, EventArgs e)
        {
            //Make sure Main form displays and has the focus
            Globalitems.MainForm.Visible = true;
            Globalitems.MainForm.Focus();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {OpenCSVFile();}

        private void dgResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //As long as row clicked is not the Column Header row, index = -1, change the binding source
            if (e.RowIndex > -1) FilterBindingSource();
        }

        private void FilterBindingSource()
        {
            try
            {
                //User has selected a row in the Gridview, dgResults.
                //Change the current binding source, lsCustomers to the selected row
                int intCustomerID;

                //Get CustomerID from Cell[0] in row clicked
                intCustomerID = Convert.ToInt16(dgResults.SelectedRows[0].Cells[0].Value.ToString());

                //Update lsCustomerIDs with new CustomerID
                lsCustomerIDs[0] = intCustomerID;

                //Update Detail Record with new CustomerID details
                FillDetailRecord(lsCustomerIDs[0]);

                //6f. Update recbuttons
                recbuttons.blnRecordsToDisplay = true;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "filterBindingSource", ex.Message);
            }
        }

        private void ZipCodeCheck(ComboBox cboBox, TextBox txtbox_City, TextBox txtbox_State,
            TextBox txtbox_Zip)
        {
            try
            {
                AddressInfo objAddress = null;

                if (strMode == "READ") return;
                if (blnIgnoreZipcode) return;
                if (cboBox.SelectedItem == null) return;

                //Check zip code, if Country is US
                if ((cboBox.SelectedItem as ComboboxItem).cboValue == "U.S.A." &&
                    txtbox_Zip.Text.Trim().Length > 0)
                {
                    objAddress = Formops.CheckZipCode(this, txtbox_Zip, ref blnIgnoreZipcode);
                    if (objAddress == null) return;
                    if (objAddress.error.Length > 0) return;

                    //OK Zip code, set City, State
                    if (objAddress.city.Length > 0)
                    {
                        txtbox_City.Text = objAddress.city;
                        txtbox_State.Text = objAddress.state;
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ZipCodeCheck", ex.Message);
            }
        }

        private void btnAddRate_Click(object sender, EventArgs e)
        {AddRate();}

        private void btnModifyRate_Click(object sender, EventArgs e)
        {ModifyRate();}

        private void frmCustomerAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (strMode != "READ" && !Globalitems.blnException)
            {
                MessageBox.Show("You must SAVE or Cancel the current changes to close this form",
                   "CANNOT CLOSE THIS FORM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void txtLocName_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtLocName_Billing", lsControls_BillingAddr);
        }

        private void txtShortLocName_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtShortLocName_Billing", lsControls_BillingAddr);
        }

        private void cboLocSubtype_Billing_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("cboLocSubtype_Billing", 
                    lsControls_BillingAddr);
        }

        private void txtAddr1_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAddr1_Billing", lsControls_BillingAddr);
        }

        private void txtAddr2_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAddr2_Billing", lsControls_BillingAddr);
        }

        private void txtCity_record_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtCity_record_Billing", lsControls_BillingAddr);
        }

        private void txtState_record_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtState_record_Billing", lsControls_BillingAddr);
        }

        private void txtZip_record_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtZip_record_Billing", lsControls_BillingAddr);
        }

        private void cboCountry_Billing_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZipCodeCheck(cboCountry_Billing, txtCity_record_Billing, txtState_record_Billing,
                txtZip_record_Billing);

            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("cboCountry_Billing", lsControls_BillingAddr);
        }

        private void txtMainPhone_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtMainPhone_Billing", lsControls_BillingAddr);
        }

        private void txtMainFax_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtMainFax_Billing", lsControls_BillingAddr);
        }

        private void txtPrimaryFName_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryFName_Billing", lsControls_BillingAddr);
        }

        private void txtPrimaryLName_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryLName_Billing", lsControls_BillingAddr);
        }

        private void txtPrimaryPhone_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryPhone_Billing", lsControls_BillingAddr);
        }

        private void txtPrimaryExt_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryExt_Billing", lsControls_BillingAddr);
        }

        private void txtPrimaryCell_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryCell_Billing", lsControls_BillingAddr);
        }

        private void txtPrimaryEmail_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryEmail_Billing", lsControls_BillingAddr);
        }

        private void txtAlternateFName_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateFName_Billing", lsControls_BillingAddr);
        }

        private void txtAlternateLName_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateLName_Billing", lsControls_BillingAddr);
        }

        private void txtAlternatePhone_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternatePhone_Billing", lsControls_BillingAddr);
        }

        private void txtAlternateExt_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateExt_Billing", lsControls_BillingAddr);
        }

        private void txtAlternateCell_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateCell_Billing", lsControls_BillingAddr);
        }

        private void txtAlternateEmail_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateEmail_Billing", lsControls_BillingAddr);
        }

        private void txtOtherPhoneDesc1_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtOtherPhoneDesc1_Billing", lsControls_BillingAddr);
        }

        private void txtOtherPhone1_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtOtherPhone1_Billing", lsControls_BillingAddr);
        }

        private void txtOtherPhoneDesc2_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtOtherPhoneDesc2_Billing", lsControls_BillingAddr);
        }

        private void txtOtherPhone2_Billing_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Billing == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtOtherPhone2_Billing", lsControls_BillingAddr);
        }

        private void txtLocName_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtLocName_Street", lsControls_StreetAddr);
        }

        private void cboLocSubtype_Street_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("cboLocSubtype_Street", lsControls_StreetAddr);
        }

        private void txtShortLocName_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtShortLocName_Street", lsControls_StreetAddr);
        }

        private void txtAddr1_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAddr1_Street", lsControls_StreetAddr);
        }

        private void txtAddr2_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAddr2_Street", lsControls_StreetAddr);
        }

        private void txtCity_record_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtCity_record_Street", lsControls_StreetAddr);
        }

        private void txtState_record_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtState_record_Street", lsControls_StreetAddr);
        }

        private void txtZip_record_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtZip_record_Street", lsControls_StreetAddr);
        }

        private void cboCountry_Street_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZipCodeCheck(cboCountry_Street, txtCity_record_Street, txtState_record_Street,
                txtZip_record_Street);

            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("cboCountry_Street", lsControls_StreetAddr);
        }

        private void txtMainPhone_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtMainPhone_Street", lsControls_StreetAddr);
        }

        private void txtMainFax_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtMainFax_Street", lsControls_StreetAddr);
        }

        private void txtPrimaryFName_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryFName_Street", lsControls_StreetAddr);
        }

        private void txtPrimaryLName_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryLName_Street", lsControls_StreetAddr);
        }

        private void txtPrimaryPhone_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryPhone_Street", lsControls_StreetAddr);
        }

        private void txtPrimaryExt_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryExt_Street", lsControls_StreetAddr);
        }

        private void txtPrimaryCell_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryCell_Street", lsControls_StreetAddr);
        }

        private void txtPrimaryEmail_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryEmail_Street", lsControls_StreetAddr);
        }

        private void txtAlternateFName_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateFName_Street", lsControls_StreetAddr);
        }

        private void txtAlternateLName_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateLName_Street", lsControls_StreetAddr);
        }

        private void txtAlternatePhone_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternatePhone_Street", lsControls_StreetAddr);
        }

        private void txtAlternateExt_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateExt_Street", lsControls_StreetAddr);
        }

        private void txtAlternateCell_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateCell_Street", lsControls_StreetAddr);
        }

        private void txtAlternateEmail_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateEmail_Street", lsControls_StreetAddr);
        }

        private void txtOtherPhoneDesc1_Street_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_Street == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtOtherPhoneDesc1_Street", lsControls_StreetAddr);
        }

        private void txtOtherPhone1_Street_TextChanged(object sender, EventArgs e)
        {
            Formops.ChangeControlUpdatedStatus("txtOtherPhone1_Street", lsControls_StreetAddr);
        }

        private void txtOtherPhoneDesc2_Street_TextChanged(object sender, EventArgs e)
        {
            Formops.ChangeControlUpdatedStatus("txtOtherPhoneDesc2_Street", lsControls_StreetAddr);
        }
        
        private void txtOtherPhone2_Street_TextChanged(object sender, EventArgs e)
        {
            Formops.ChangeControlUpdatedStatus("txtOtherPhoneDesc2_Street", lsControls_StreetAddr);
        }        

        private void ckSameAsBilling_CheckedChanged(object sender, EventArgs e)
        {ProcessStreetSameAsBillingInfo();}

        private void btnClearBillingAddr_Click(object sender, EventArgs e)
        {
            Formops.ClearRecordData(this, lsControls_BillingAddr,false);
            ckSameAsBilling.Checked = false;
        }

        private void btnClearStreetAddr_Click(object sender, EventArgs e)
        { Formops.ClearRecordData(this, lsControls_StreetAddr,false); }

        private void txtZip_record_Billing_Leave(object sender, EventArgs e)
        {ZipCodeCheck(cboCountry_Billing, txtCity_record_Billing, txtState_record_Billing,
               txtZip_record_Billing);}

        private void txtZip_record_Street_Leave(object sender, EventArgs e)
        {
            ZipCodeCheck(cboCountry_Street, txtCity_record_Street, txtState_record_Street,
                txtZip_record_Street);
        }

        private void frmCustomerAdmin_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
