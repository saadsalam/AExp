using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmExporterAdmin : Form
    {
        public bool blnNewExporterRQFromOtherForm = false;

        private const string CURRENTMODULE = "frmExporterAdmin";

        private bool blnClearingForm = false;
        private bool blnFillingcbos = false;
        private bool blnModifySetup = false;

        private bool blnIgnoreZipcode = false;
        private BindingSource bs1 = new BindingSource();
        private DataTable dtExporters = new DataTable();
        private List<int> lsForwarderIDs = new List<int>();
        private string strMode;

        //Use to track initial state of Location, and outgoing state, sent to SProc to
        //  update FreightForwarder info
        //States are:
        //"" [no change]
        //NEW - add new loc info for FreightForwarder
        //MODIFY - modify loc info
        //DELETE - delete all loc info
        private string strMode_address = "";

        //Set up List of ControlInfo objects, lsControlInfo, as follows:
        //  Order in list establishes Indexes for tabbing, implemented by SetTabIndex() method
        //  AlwaysReadOnly identifies if control cannot be modified by User
        //  ControlPropertyToBind identifies what controls are initialized 
        //  RecordFieldName identify what controls display record detail
        //  HeaderText sets column header to use for Export to csv file
        //  Updated property establishes what controls User has modified

        //Similar to CustomerAdmin form, since Address info is a separate table,
        // use separate lists for FreightForwarder info & Address info
        private List<ControlInfo> lsControls_primary = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="cboCust", ControlPropetyToBind ="SelectedValue"},
            new ControlInfo {ControlID="ckActiveCust"},
            new ControlInfo {ControlID="cboForwarders",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtExporter",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCity",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtState",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboStatus",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtZip",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="btnSearch" },
            new ControlInfo {ControlID="btnClear" },
            new ControlInfo {ControlID="btnExport" },
            new ControlInfo {ControlID="cboCust_record",
                ControlPropetyToBind ="SelectedValue"},
            new ControlInfo {ControlID="cboForwarders_record",
                RecordFieldName ="AEFreightForwarderID",
                ControlPropetyToBind ="SelectedValue"},
            new ControlInfo {ControlID="txtExporter_record",
                RecordFieldName ="ExporterName",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtShortName_record",
                RecordFieldName ="ExporterShortName",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtCustExporterCode",
                RecordFieldName ="CustomerExporterCode",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="cboStatus_record",
                RecordFieldName ="RecordStatus",
                ControlPropetyToBind ="SelectedValue"},
            new ControlInfo {ControlID="txtNotes",
                RecordFieldName ="Notes",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtLocationID",
                RecordFieldName ="LocationID",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtCreationDate",
                RecordFieldName ="CreationDate",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtCreatedBy",
                RecordFieldName ="CreatedBy",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtUpdatedDate",
                RecordFieldName ="UpdatedDate",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtUpdatedBy",
                RecordFieldName ="UpdatedBy",
                ControlPropetyToBind="Text" },

            // objects needed for csv file  HeaderText="Cust. Name"
            new ControlInfo {RecordFieldName="CustName",HeaderText="Customer"},
            new ControlInfo {RecordFieldName="Forwarder",HeaderText="Forwarder"},
            new ControlInfo {RecordFieldName="Exporter",HeaderText="Exporter"},
            new ControlInfo {RecordFieldName="AddressLine1",HeaderText="Street Addr."},
            new ControlInfo {RecordFieldName="City",HeaderText="City"},
            new ControlInfo {RecordFieldName="State",HeaderText="ST"},
            new ControlInfo {RecordFieldName="Zip",HeaderText="Zip"},
            new ControlInfo {RecordFieldName="RecordStatus",HeaderText="Status"}
        };

        private List<ControlInfo> lsControls_address = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="txtLocName_record",
                RecordFieldName ="LocationName",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtShortLocName_record",
                RecordFieldName ="LocationShortName",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="cboLocType",
                RecordFieldName ="LocationType",
                ControlPropetyToBind ="SelectedValue"},
            new ControlInfo {ControlID="cboLocSubtype",
                RecordFieldName ="LocationSubType",
                ControlPropetyToBind ="SelectedValue"},
            new ControlInfo {ControlID="txtAddr1_record",
                RecordFieldName ="AddressLine1",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtAddr2_record",
                RecordFieldName ="AddressLine2",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtCity_record",
                RecordFieldName ="City",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtState_record",
                RecordFieldName ="State",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtZip_record",
                RecordFieldName ="Zip",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="cboCountry",
                RecordFieldName ="Country",
                ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtMainPhone_record",
                RecordFieldName ="MainPhone",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtMainFax_record",
                RecordFieldName ="FaxNumber",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtPrimaryFName_record",
                RecordFieldName ="PrimaryContactFirstName",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtPrimaryLName_record",
                RecordFieldName ="PrimaryContactLastName",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtPrimaryPhone_record",
                RecordFieldName ="PrimaryContactPhone",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtPrimaryExt_record",
                RecordFieldName ="PrimaryContactExtension",
                ControlPropetyToBind="Text" },
             new ControlInfo {ControlID="txtPrimaryCell_record",
                RecordFieldName ="PrimaryContactCellPhone",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtPrimaryEmail_record",
                RecordFieldName ="PrimaryContactEmail",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtAlternateFName_record",
                RecordFieldName ="AlternateContactFirstName",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtAlternateLName_record",
                RecordFieldName ="AlternateContactLastName",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtAlternatePhone_record",
                RecordFieldName ="AlternateContactPhone",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtAlternateExt_record",
                RecordFieldName ="AlternateContactExtension",
                ControlPropetyToBind="Text" },
             new ControlInfo {ControlID="txtAlternateCell_record",
                RecordFieldName ="AlternateContactCellPhone",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtAlternateEmail_record",
                RecordFieldName ="AlternateContactEmail",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtOtherPhoneDesc1_record",
                RecordFieldName ="OtherPhone1Description",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtOtherPhone1_record",
                RecordFieldName ="OtherPhone1",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtOtherPhoneDesc2_record",
                RecordFieldName ="OtherPhone2Description",
                ControlPropetyToBind="Text" },
            new ControlInfo {ControlID="txtOtherPhone2_record",
                RecordFieldName ="OtherPhone2",
                ControlPropetyToBind="Text" }
        };

        public frmExporterAdmin()
        {
            List<string> lsExcludes = new List<string>
            {
                {"txtNotes"}
            };

            InitializeComponent();
            dgResults.AutoGenerateColumns = false;

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
            Formops.SetTabIndex(this, lsControls_primary);
            Formops.SetTabIndex(this, lsControls_address);

            //Clear record # texts
            lblForwarderRecords.Text = "";

            //Disable btnExport
            btnExport.Enabled = false;

            DisplayMode();
        }

        private void frmExporterAdmin_Activated(object sender, EventArgs e)
        {
            if (blnNewExporterRQFromOtherForm)
            {
                blnNewExporterRQFromOtherForm = false;
                recbuttons.btnNew_Click(null, null);
            }
        }

        private void AdjustReadOnlyStatus(bool blnReadOnly)
        {
           try
            {
                Formops.SetReadOnlyStatus(this, lsControls_primary, blnReadOnly, recbuttons);
                Formops.SetReadOnlyStatus(this, lsControls_address, blnReadOnly, recbuttons);
                btnClearAddr.Enabled = !blnReadOnly;

                cboCust_record.Enabled = false;

                //Only enable cboCust_Record & set to <select> when creating a new Exporter
                if (strMode == "NEW")
                {
                    cboCust_record.Enabled = true;
                    cboCust_record.SelectedIndex = 0;
                }
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "AdjustReadOnlyStatus", ex.Message);
            }
        }

        private bool AddrSectionHasInfo()
        {
            //Checks if any textboxes or ComboBox in the section has a value
            //Doesn't look at checkbox, in Street tab
            try
            {
                ComboBox cboBox;
                Control[] ctrls;
                Control ctrlx;
                List<ControlInfo> lsControls = lsControls_address;

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
                Formops.ClearRecordData(this, lsControls_primary);
                Formops.ClearRecordData(this, lsControls_address);

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
                    FillDetailRecord(lsForwarderIDs[intCurrentBSPosition]);
                }

                blnIgnoreZipcode = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CancelSetup", ex.Message);
            }
        }

        private void ClearForm()
        {
            try
            {
                blnClearingForm = true;

                //1. Clear all items in lsControls
                Formops.ClearSetup(this, lsControls_primary);
                Formops.ClearSetup(this, lsControls_address);

                //2. Clear Form unique grids
                ClearGridView();

                //3. Set Form unique Readonly/enabled status for controls
                AdjustReadOnlyStatus(true);

                blnClearingForm = false;
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearSetup", ex.Message);
            }
        }

        private void FilterBindingSource()
        {
            try
            {
                DataGridViewRow dgRow = dgResults.SelectedRows[0];

                //User has selected a row in the Gridview, dgResults.
                //Change the current binding source, lsForwarders to the selected row
                int intForwarderID;

                //Get CustomerID from Cell[0] in row clicked
                intForwarderID = Convert.ToInt32(dgRow.Cells["ForwarderID"].Value.ToString());

                //Update lsCustomerIDs with new CustomerID
                lsForwarderIDs[0] = intForwarderID;

                //Update Detail Record with new CustomerID details
                FillDetailRecord(intForwarderID);

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

                    if (strMode == "NEW" && bs1 != null)
                    {
                        Formops.ClearRecordData(this, lsControls_primary);
                        Formops.ClearRecordData(this, lsControls_address);
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "DisplayMode", ex.Message);
            }
        }

       

        private void ClearGridView()
        {
            try
            {
                lblForwarderRecords.Text = "";

                dtExporters.Clear();

                // Binding dgResults to lsUsers after the Clear method, can lead to runtime error because
                //  the CurrencyManager pointing to the Current position in lsUsers, doesn't reset to -1
                dgResults.DataSource = dtExporters;

                recbuttons.blnRecordsToDisplay = false;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearGridView", ex.Message);
            }
        }

        private void FillCombos()
        {
            ComboboxItem cboitem;
            ComboboxItem cboitemcopy;
            DataSet ds;
            string strFilter;
            string strSQL;

            try
            {
                blnFillingcbos = true;

                //cboCust
                cboCust.Items.Clear();
                cboCust_record.Items.Clear();

                strSQL = "SELECT CustomerID, " +
                    "CASE WHEN LEN(RTRIM(ISNULL(ShortName,''))) > 0 THEN RTRIM(ShortName) " +
                    "else RTRIM(CustomerName) END AS CustName " +
                    "FROM Customer  WHERE LEN(RTRIM(ISNULL(CustomerName,''))) > 0 ";

                if (ckActiveCust.Checked)
                    strSQL += "AND RecordStatus = 'Active' ";

                strSQL += "ORDER BY CustName ";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillCombos",
                        "No rows returned from Customer table");
                    return;
                }

                // Add All to cboCust
                cboitem = new ComboboxItem();
                cboitem.cboText = "All";
                cboitem.cboValue = "All";
                cboCust.Items.Add(cboitem);

                // Add <select> to cboCust
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboCust_record.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dr["CustName"].ToString();
                    cboitem.cboValue = dr["CustomerID"].ToString();
                    cboCust.Items.Add(cboitem);

                    cboitemcopy = cboitem.MakeCopy(cboitem);
                    cboCust_record.Items.Add(cboitem);
                }

                cboCust.DisplayMember = "cboText";
                cboCust.ValueMember = "cboValue";
                cboCust.SelectedIndex = 0;

                cboCust_record.DisplayMember = "cboText";
                cboCust_record.ValueMember = "cboValue";
                cboCust_record.SelectedIndex = -1;

                //Fill cboForwarders
                cboForwarders.Items.Clear();
                cboForwarders_record.Items.Clear();
                strSQL = @"SELECT ff.AEFreightForwarderID,
                    CASE
                    WHEN LEN(RTRIM(ISNULL(ff.FreightForwarderShortName, ''))) > 0 THEN
                        RTRIM(ff.FreightForwarderShortName)
                    ELSE RTRIM(ff.FreightForwarderName)
                    END AS FreightForwarder 
                    FROM
                    AEFreightForwarder ff
                    LEFT OUTER JOIN Customer cus on cus.CustomerID = ff.AECustomerID
                    WHERE LEN(RTRIM(ISNULL(FreightForwarderName, ''))) > 0 ";

                if (ckActiveCust.Checked)
                    strSQL += "AND cus.RecordStatus = 'Active' ";

                if (ckActiveFF.Checked)
                    strSQL += "AND ff.RecordStatus = 'Active' ";

                //Add ORDER BY
                strSQL += " ORDER BY FreightForwarder";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillCombos",
                        "No rows returned from FreightForwarder table");
                    return;
                }

                // Add All to cboForwarders
                cboitem = new ComboboxItem();
                cboitem.cboText = "All";
                cboitem.cboValue = "All";
                cboForwarders.Items.Add(cboitem);

                // Add <select> to cboForwarders_record
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboForwarders_record.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dr["FreightForwarder"].ToString();
                    cboitem.cboValue = dr["AEFreightForwarderID"].ToString();
                    cboForwarders.Items.Add(cboitem);

                    cboitemcopy = cboitem.MakeCopy(cboitem);
                    cboForwarders_record.Items.Add(cboitemcopy);
                }

                cboForwarders.DisplayMember = "cboText";
                cboForwarders.ValueMember = "cboValue";
                cboForwarders.SelectedIndex = 0;

                cboForwarders_record.DisplayMember = "cboText";
                cboForwarders_record.ValueMember = "cboValue";
                cboForwarders_record.SelectedIndex = -1;

                //Fill cboStatus, cboStatus_record, just first time
                if (cboStatus.Items.Count == 0)
                {
                    strFilter = "CodeType = 'RecordStatus'";
                    Globalitems.FillComboboxFromCodeTable(strFilter, cboStatus, true, false);
                    Globalitems.FillComboboxFromCodeTable(strFilter, cboStatus_record, true, false);

                    //Change 1st item in cboStatus_record from All to <select>
                    cboitem = (ComboboxItem)cboStatus_record.Items[0];
                    cboitem.cboText = "<select>";
                    cboitem.cboValue = "select";

                    cboStatus.DisplayMember = "cboText";
                    cboStatus.ValueMember = "cboValue";
                    cboStatus.SelectedIndex = 0;

                    cboStatus_record.DisplayMember = "cboText";
                    cboStatus_record.ValueMember = "cboValue";
                    cboStatus_record.SelectedIndex = -1;
                }

                if (cboLocType.Items.Count == 0)
                {
                    strFilter = "CodeType = 'LocationType'";
                    Globalitems.FillComboboxFromCodeTable(strFilter, cboLocType, true, false);
                    cboLocType.SelectedIndex = -1;

                    //Change 1st item from All to <select>
                    cboitem = (ComboboxItem)cboLocType.Items[0];
                    cboitem.cboText = "<select>";
                    cboitem.cboValue = "select";
                    cboLocType.SelectedIndex = -1;
                }

                if (cboLocSubtype.Items.Count == 0)
                {
                    strFilter = "CodeType = 'LocationSubType'";
                    Globalitems.FillComboboxFromCodeTable(strFilter, cboLocSubtype, true, false);

                    //Change 1st item from All to <select>
                    cboitem = (ComboboxItem)cboLocSubtype.Items[0];
                    cboitem.cboText = "<select>";
                    cboitem.cboValue = "select";
                    cboLocSubtype.SelectedIndex = -1;
                }
                
                //Fill cboCountry
                if (cboCountry.Items.Count == 0)
                {
                    strFilter = "CodeType = 'CountryCode'";
                    Globalitems.FillComboboxFromCodeTable(strFilter, cboCountry, true, false);

                    //Change 1st item from All to <select>
                    cboitem = (ComboboxItem)cboCountry.Items[0];
                    cboitem.cboText = "<select>";
                    cboitem.cboValue = "select";
                    cboCountry.SelectedIndex = -1;
                }

                blnFillingcbos = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void FillDetailRecord(int intForwarderID)
        {
            try
            {
                DataGridViewRow dgRow = dgResults.SelectedRows[0];
                DataRow dr;
                DataSet ds;

                Int64 intTel;
                SProcParameter Paramobj;
                List<SProcParameter> Paramobjects = new List<SProcParameter>();

                string strCustomerID;
                string strCountry;
                string strExporterID;
                string strForwarderID;
                string strSproc = "spGetForwarderExporterAdminInfo";
                string strval;

                //Set cboCust_record to Customer 
                strCustomerID = dgRow.Cells["CustomerID"].Value.ToString();
                foreach (ComboboxItem cboitem in cboCust_record.Items)
                    if (cboitem.cboValue == strCustomerID) cboCust_record.SelectedItem = cboitem;

                //Store ForwarderID & persist in txtForwarderID_record
                strForwarderID = dgRow.Cells["ForwarderID"].Value.ToString();
                txtForwarderID_record.Text = strForwarderID;

                //Store ExporterID & persist in txtExporterID_record
                strExporterID = dgRow.Cells["ExporterID"].Value.ToString();
                txtExporterID_record.Text = strExporterID;

                Paramobj = new SProcParameter();
                Paramobj.Paramname = "@type";
                Paramobj.Paramvalue = "EXPORTER";
                Paramobjects.Add(Paramobj);

                Paramobj = new SProcParameter();
                Paramobj.Paramname = "@ID";
                Paramobj.Paramvalue = Convert.ToInt32(strExporterID);
                Paramobjects.Add(Paramobj);

                ds = DataOps.GetDataset_with_SProc(strSproc, Paramobjects);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillDetailRecord",
                        "No data returned from SProc");
                    return;
                }

                dr = ds.Tables[0].Rows[0];

                //loop through lsControls to set record controls
                Formops.ClearRecordData(this, lsControls_primary);
                Formops.ClearRecordData(this, lsControls_address);
                Formops.SetDetailRecord(dr, this, lsControls_primary);
                Formops.SetDetailRecord(dr, this, lsControls_address);

                //Set LocID to "0", if ""
                if (txtLocationID.Text.Length == 0) txtLocationID.Text = "0";

                //Set Address mode to NEW or MODIFY based on whether a LocID exists
                strMode_address = "NEW";
                if (txtLocationID.Text != "0")
                    strMode_address = "MODIFY";

                //Set date time formats
                txtCreationDate.Text = Globalitems.FormatDatetime(txtCreationDate.Text);
                txtUpdatedDate.Text = Globalitems.FormatDatetime(txtUpdatedDate.Text);

                //Set phone formats, if lenght is 10 and country is US
                strCountry = "";
                if ((cboCountry.SelectedItem as ComboboxItem).cboValue == "U.S.A.")
                    strCountry = "US";

                strval = txtMainPhone_record.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 &&
                    Int64.TryParse(strval, out intTel))
                    txtMainPhone_record.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtMainFax_record.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 &&
                    Int64.TryParse(strval, out intTel))
                    txtMainFax_record.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtPrimaryPhone_record.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 &&
                    Int64.TryParse(strval, out intTel))
                    txtPrimaryPhone_record.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtPrimaryCell_record.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 &&
                    Int64.TryParse(strval, out intTel))
                    txtPrimaryCell_record.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtAlternatePhone_record.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 &&
                    Int64.TryParse(strval, out intTel))
                    txtAlternatePhone_record.Text = String.Format("{0:(###) ###-####}", intTel);

                strval = txtAlternateCell_record.Text.Trim();
                if (strCountry == "US" && strval.Length == 10 &&
                    Int64.TryParse(strval, out intTel))
                    txtAlternateCell_record.Text = String.Format("{0:(###) ###-####}", intTel);

            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillDetailRecord", ex.Message);
            }
        }

        private bool InfoChangeValid(List<ControlInfo> lsChangedItems)
        {
            //Make sure changes to Addr are valid.
            //strMode_address has initial mode set to 
            //  NEW/MODIFY in FillDetailRecord
            //Address Change may be:
            //  Forwarder Mode: NEW
            //  Addr:[no value]/NEW
            
            //  Forwarder Mode: MODIFY
            //  Addr:[no change]/NEW/MODIFY/DELETE

            try
            {
                DataSet ds;
                int intLocationID = 0;
                List<ControlInfo> lsControls = lsControls_address;
                string strModeLoc = strMode_address;
                string strSQL;
                TextBox txtLocName = txtLocName_record;
                TextBox txtLocShortname = txtShortLocName_record;
                TextBox txtLocID = txtLocationID;

                if (txtLocID.Text.Trim().Length > 0)
                    intLocationID = Convert.ToInt32(txtLocID.Text);

                //Make sure the required Loc. info is present
                //If txtLocname has a value, must be unique,
                //If txtLocShortname has a value, must be unique 
                //SubType must be selected.
                if (txtLocName.Text.Trim().Length > 0)
                {
                    //Make sure Loc. Name is unique among other locations
                    strSQL = "SELECT LocationName " +
                        "FROM Location " +
                        "WHERE ParentRecordTable = 'AEFreightForwarder' AND " +
                        "LocationName = '" +
                        Globalitems.HandleSingleQuoteForSQL(txtLocName.Text.Trim()) +
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
                        MessageBox.Show("The Location " +
                            "name must be unique.\n\n" +
                            "One or more other Locations have the same name",
                            "DUPLICATE LOCATION NAMES",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtLocName.Focus();
                        return false;
                    }

                    //Make sure Loc. Short Name is unique
                    if (txtLocShortname.Text.Trim().Length > 0)
                    {
                        strSQL = "SELECT LocationShortName " +
                        "FROM Location " +
                        "WHERE ParentRecordTable = 'AEFreightForwarder' AND " +
                        "LocationShortName = '" +
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
                            MessageBox.Show("The Location " +
                                "Short name must be unique.\n\n" +
                                "One or more other Locations have the same short name",
                                "DUPLICATE LOCATION SHORT NAMES",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtLocShortname.Focus();
                            txtLocShortname.Select(0, txtLocShortname.Text.Length);
                            return false;
                        }
                    }

                    //Make sure Loc. Type is selected
                    if ((cboLocType.SelectedItem as ComboboxItem).cboValue == "select")
                    {
                        MessageBox.Show("You must select " +
                            "the Location Type", "MISSING LOCATION TYPE",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cboLocType.Focus();
                        return false;
                    }

                    //Make sure Loc. Sub Type is selected
                    if ((cboLocSubtype.SelectedItem as ComboboxItem).cboValue == "select")
                    {
                        MessageBox.Show("You must select " +
                            "the Location Sub Type", "MISSING LOCATION SUB TYPE",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cboLocSubtype.Focus();
                        return false;
                    }

                }   // if txtLocname.Length > 0

                //Check if Locname is blank, but other values entered
                if (txtLocName.Text.Trim().Length == 0 && AddrSectionHasInfo())
                {
                    MessageBox.Show("You must enter " +
                        "the Location Name", "MISSING LOCATION NAME",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLocName.Focus();
                    return false;
                }

                // NEW Forwarder, strModLoc was already set to NEW in NewRecordSetup
                // Location action may need to change to:  ""[do nothing], SAME
                if (strMode == "NEW")
                {
                    //If no Address Loc. name, do nothing
                    if (txtLocName.Text.Trim().Length == 0) strMode_address = "";

                    return true;
                }

                //MODIFY Forwarder. strMod_address was pre-set in FillDetailRecord
                //If strModeLoc is NEW: may need to change to ""[do nothing], SAME (for Street)
                //If strModLoc is MODIFY: may need to change to:  ""[do nothing],SAME, DELETE
                //If strModLoc is SAME: may need to change to:  ""[do nothing],NEW, DELETE
                if (strMode == "MODIFY")
                {
                    //If Loc change is NEW
                    //Check if no change to Section or SAME for Street Addr
                    if (strModeLoc == "NEW")
                    {
                        if (!AddrSectionHasInfo()) strMode_address = ""; //no Address loc info to add

                        return true;
                    }

                    if (strModeLoc == "MODIFY")
                    {
                        //strModeLoc is MODIFY
                        //Check for "" [no change]
                        if (lsChangedItems.Count == 0)
                        {
                            strMode_address = "";
                            return true;
                        }

                        //strModeLoc is MODIFY
                        //Check for DELETE
                        if (!AddrSectionHasInfo() && lsChangedItems.Count > 0)
                        {
                            strMode_address = "DELETE";
                            return true;
                        }
                    }   // If strModeLoc = MODIFY

                }  // If strMode = MODIFY   


                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "InfoChangeValid", ex.Message);
                return false;
            }
        }

        private void ModifyRecordSetup()
        {
            try
            {
                string strForwarderID;

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
                Formops.ResetControls(this, lsControls_address);

                //7. Set recbuttons to Modify
                recbuttons.SetButtons(RecordButtons.ACTION_MODIFYRECORD);

                //8. Set Updated By/Date to new value
                txtUpdatedBy.Text = Globalitems.strUserName;
                txtUpdatedDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

                //9. Set focus on first control
                txtExporter_record.Focus();

                //10. Handle Form unique controls

                //Set so cboForwarders_record will not register a changed value
                blnModifySetup = true;

                //Save cboForwarders_record.Value
                strForwarderID = (cboForwarders_record.SelectedItem as ComboboxItem).cboValue;

                //Refill cboForwarders_record so it only contains FF's for the current Customer
                RefillcboForwarders(false);

                //Set cboForwarders to prev. value
                foreach (ComboboxItem cboitem in cboForwarders_record.Items)
                    if (cboitem.cboValue == strForwarderID)
                        cboForwarders_record.SelectedItem = cboitem;

                //Reset to false, so if User changes Forwarder, cboForwarder_record will register
                //  a changed value
                blnModifySetup = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ModifyRecordSetup", ex.Message);
            }
        }

        private void NewRecordSetup()
        {
            //1. Set Mode
            strMode = "NEW";
            strMode_address = "NEW";

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
            Formops.ClearRecordData(this, lsControls_address);

            //7. Set recbuttons to New
            recbuttons.SetButtons(RecordButtons.ACTION_NEWRECORD);

            //8. Set Created By/Date 
            txtCreatedBy.Text = Globalitems.strUserName;
            txtCreationDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

            //9. Set focus on first contol
            cboCust_record.Focus();

            //10. Handle Form unique controls
            foreach (ComboboxItem cboitem in cboStatus_record.Items)
                if (cboitem.cboValue == "Active") cboStatus_record.SelectedItem = cboitem;

            txtCustomerID.Text = "0";
            txtForwarderID_record.Text = "0";

            //Set cbo's to <select>
            cboCust_record.SelectedIndex = 0;
            cboForwarders_record.SelectedIndex = 0;
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
                    strSQL = @"SELECT ValueKey,ValueDescription FROM SettingTable 
                        WHERE ValueKey IN ('ExportDirectory','ExporterExportFileName') 
                        AND RecordStatus='Active' ORDER BY ValueKey";
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
                    dt = dtExporters.Copy();

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
                    ControlInfo objctrlinfo_Customer = lsControls_primary.First(obj => obj.HeaderText == "Customer");
                    lsCSVcols.Add(objctrlinfo_Customer);

                    ControlInfo objctrlinfo_Fwd = lsControls_primary.First(obj => obj.HeaderText == "Forwarder");
                    lsCSVcols.Add(objctrlinfo_Fwd);

                    ControlInfo objctrlinfo_Exp = lsControls_primary.First(obj => obj.HeaderText == "Exporter");
                    lsCSVcols.Add(objctrlinfo_Exp);

                    ControlInfo objctrlinfo_Street = lsControls_primary.First(obj => obj.HeaderText == "Street Addr.");
                    lsCSVcols.Add(objctrlinfo_Street);

                    var objctrlinfo_City = lsControls_primary.First(obj => obj.HeaderText == "City");
                    lsCSVcols.Add(objctrlinfo_City);

                    ControlInfo objctrlinfo_ST = lsControls_primary.First(obj => obj.HeaderText == "ST");
                    lsCSVcols.Add(objctrlinfo_ST);

                    ControlInfo objctrlinfo_Zip = lsControls_primary.First(obj => obj.HeaderText == "Zip");
                    lsCSVcols.Add(objctrlinfo_Zip);

                    ControlInfo objctrlinfo_Status = lsControls_primary.First(obj => obj.HeaderText == "Status");
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

        private void PerformDeleteRecord()
        {
            try
            {
                DialogResult dlResult;
                DataSet ds;
                frmAreYouSure frmConfirm;
                int inttotrecs;
                string strMessage;
                string strSQL;

                //Cannot delete FreightForwarder is any vehicle references it
                strSQL = @"SELECT COUNT(AutoportExportVehiclesID) AS totrec 
                    FROM AutoportExportVehicles
                    WHERE ExporterID = " + txtExporterID_record.Text;
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord",
                        "No data returned from query");
                    return;
                }

                inttotrecs = (int)ds.Tables[0].Rows[0]["totrec"];
                if (inttotrecs > 0)
                {
                    strMessage = "The Exporter cannot be deleted because\n";
                    if (inttotrecs == 1)
                        strMessage += "1 vehicle references it.";
                    else
                        strMessage += inttotrecs.ToString("N0") +
                            " vehicles reference it.";

                    MessageBox.Show(strMessage,"CANNOT DELETE FREIGHT FORWARDER",
                        MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }

                strMessage = "You are about to Delete an Exporter!\n\n" +
                  "You could instead change the Status to Inactive.\n\n" +
                  "Are you sure you want to delete the Exporter:\n" +
                  txtExporter_record.Text + "?";

                frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    //Delete Loc ID, if any
                    if (txtLocationID.Text != "0")
                    {
                        strSQL = "DELETE Location WHERE LocationID = " + txtLocationID.Text;
                        DataOps.PerformDBOperation(strSQL);
                    }

                    //Delete Exporter
                    strSQL = "DELETE AEExporter WHERE AEExporterID = " +
                        txtExporterID_record.Text;
                    DataOps.PerformDBOperation(strSQL);

                    PerformSearch();

                    MessageBox.Show("The Exporter has been removed from the DB", 
                        "EXPORTER DELETED",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord", ex.Message);
            }
        }

        private void PerformMoveNext()
        {
            try
            {
                bs1.MoveNext();
                FillDetailRecord(lsForwarderIDs[bs1.Position]);
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

        private void PerformMovePrevious()
        {
            try
            {
                bs1.MovePrevious();
                FillDetailRecord(lsForwarderIDs[bs1.Position]);
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMovePrevious", ex.Message);
            }

        }

        private void PerformSaveRecord()
        {
            try
            {
                // Use linq to get a list of updated Customer,Billing Addr, Street Addr controls, 
                //  For this form, textboxes, comboboxes, checkboxes
                var changedlist_primary = lsControls_primary.Where(ctrlinfo => (ctrlinfo.Updated == true) &&
                            ctrlinfo.RecordFieldName != null &&
                            (ctrlinfo.ControlPropetyToBind == "Text" ||
                            ctrlinfo.ControlPropetyToBind == "SelectedValue" ||
                            ctrlinfo.ControlPropetyToBind == "Checked")).ToList();

                var changedlist_addr = lsControls_address.Where(ctrlinfo => (ctrlinfo.Updated == true) &&
                            (ctrlinfo.ControlPropetyToBind == "Text" ||
                            ctrlinfo.ControlPropetyToBind == "SelectedValue" ||
                            ctrlinfo.ControlPropetyToBind == "Checked")).ToList();

                DataSet ds;
                int intCurrentBSPosition = 0;
                SProcParameter objParam;
                List<SProcParameter> lsParams = new List<SProcParameter>();
                string strCreationDate = DateTime.Now.ToString();
                string strForwarder_action = "";
                string strLoc_action = "";
                string strSProc = "spUpdateForwarderExporter";
                string strSQL_ForwarderExporter = "";
                string strSQL_loc = "";
                string strResult;

                if (ValidRecord())
                {
                    //1. Perform the DB action
                    if (strMode == "NEW")
                    {
                        // Need SQL to insert new customer info
                        strSQL_ForwarderExporter = SQLForNewExporter(strCreationDate);

                        strForwarder_action = "NEW";

                        //Check if Billing info added
                        if (strMode_address == "NEW")
                        {
                            strSQL_loc = SQLForNewLocation(strCreationDate);
                            strLoc_action = "NEW";
                        }
                    }

                    if (strMode == "MODIFY")
                    {
                        intCurrentBSPosition = bs1.Position;

                        if (changedlist_primary.Count > 0)
                        {
                            strForwarder_action = "MODIFY";
                            strSQL_ForwarderExporter = SQLForModifiedExporter(changedlist_primary);
                        }

                        strLoc_action = strMode_address;
                        switch (strLoc_action)
                        {
                            case "NEW":
                                strSQL_loc = SQLForNewLocation(strCreationDate);
                                break;
                            case "MODIFY":
                                strSQL_loc =
                                        SQLForModifiedLocation(changedlist_addr,
                                            txtLocationID.Text);
                                break;
                            case "DELETE":
                                strSQL_loc = "DELETE Location WHERE LocationID = " +
                                    txtLocationID.Text;
                                break;
                        }
                    }

                    objParam = new SProcParameter();
                    objParam.Paramname = "@type";
                    objParam.Paramvalue = "EXPORTER";
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@action";
                    objParam.Paramvalue = strForwarder_action;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@loc_action";
                    objParam.Paramvalue = strLoc_action;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@ID";
                    objParam.Paramvalue = txtExporterID_record.Text;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@primarySQL";
                    objParam.Paramvalue = strSQL_ForwarderExporter;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@locSQL";
                    objParam.Paramvalue = strSQL_loc;
                    lsParams.Add(objParam);

                    objParam = new SProcParameter();
                    objParam.Paramname = "@locID";
                    objParam.Paramvalue = txtLocationID.Text;
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
                    MessageBox.Show("The Exporter information has been modified in the DB.",
                    "CUSTOMER INFO MODIFIED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //3. Display other forms
                    Globalitems.DisplayOtherForms(this, true);

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

                    //7. Perform new search
                    PerformSearch();

                    blnIgnoreZipcode = false;

                }
         }

        catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord", ex.Message);
            }
    }

        private void PerformSearch()
        {
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
                Formops.ClearRecordData(this, lsControls_address);

                //4. Set recbuttons to display = false
                recbuttons.blnRecordsToDisplay = false;

                //5. Retrieve data as datatable
                strSQL = @"SELECT
                    AEExporterID,
                    ff.AEFreightForwarderID AS ForwarderID,
                    ff.AECustomerID AS CustomerID,
                    CASE
                    WHEN LEN(RTRIM(ISNULL(cus.ShortName, ''))) > 0 THEN cus.ShortName
                    ELSE RTRIM(ISNULL(cus.CustomerName, ''))
                    END AS CustName,
                    CASE
                        WHEN LEN(RTRIM(ISNULL(ff.FreightForwarderShortName, ''))) > 0 THEN RTRIM(ff.FreightForwarderShortName)
                        ELSE RTRIM(ff.FreightForwarderName)
                    END AS Forwarder,
                    CASE
	                    WHEN LEN(RTRIM(ISNULL(ex.ExporterShortName,''))) > 0 THEN RTRIM(ex.ExporterShortName)
	                    ELSE RTRIM(ex.ExporterName)
                    END AS Exporter,
                    CASE
                        WHEN LocationType = 'Street Address' THEN RTRIM(ISNULL(loc.AddressLine1, ''))
                        ELSE ''
                    END AS AddressLine1,
                    CASE
                        WHEN LocationType = 'Street Address' THEN RTRIM(ISNULL(loc.City, ''))
                        ELSE ''
                    END AS City,
                    CASE
                        WHEN LocationType = 'Street Address' THEN RTRIM(ISNULL(loc.State, ''))
                        ELSE ''
                    END AS State,
                    CASE
                        WHEN LocationType = 'Street Address' THEN RTRIM(ISNULL(loc.Zip, ''))
                        ELSE ''
                    END AS Zip,
                    ff.RecordStatus
                    FROM 
                    AEExporter ex
                    LEFT OUTER JOIN AEFreightForwarder ff on 
                        ff.AEFreightForwarderID = EX.AEFreightForwarderID
                    LEFT OUTER JOIN Customer cus on cus.CustomerID = ff.AECustomerID
                    LEFT OUTER JOIN Location loc ON loc.LocationID = ex.ExporterAddressID
                    WHERE LEN(RTRIM(ISNULL(EX.ExporterName, ''))) > 0 ";

                //Add CustomerID to WHERE clause if specified
                if ((cboCust.SelectedItem as ComboboxItem).cboValue != "All")
                {
                    strSQL += "AND ff.AECustomerID = " +
                        (cboCust.SelectedItem as ComboboxItem).cboValue.ToString() + " ";
                }

                //Restrict to Active Customers if checked
                if (ckActiveCust.Checked)
                    strSQL += "AND cus.RecordStatus='Active' ";

                //Restrict to Active Forwarders if checked
                if (ckActiveFF.Checked)
                    strSQL += "AND ff.RecordStatus='Active' ";

                //Add Forwarder if specified
                if ((cboForwarders.SelectedItem as ComboboxItem).cboValue != "All")
                {
                    strSQL += "AND ex.AEFreightForwarderID = " +
                        (cboForwarders.SelectedItem as ComboboxItem).cboValue.ToString() + " ";
                }

                //Add Exporter to WHERE clause if specified
                if (txtExporter.Text.Trim().Length > 0)
                {
                    strval = Globalitems.HandleSingleQuoteForSQL(txtExporter.Text.Trim());
                    strSQL += "AND (ex.ExporterName LIKE '" + strval + "%' " +
                        "OR ex.ExporterShortName LIKE '" + strval + "%') ";
                }

                if ((cboStatus.SelectedItem as ComboboxItem).cboValue != "All")
                {
                    strval = (cboStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    strSQL += "AND ff.RecordStatus ='" + strval + "' ";
                }

                //Add City to WHERE clause 
                if (txtCity.Text.Trim().Length > 0)
                {
                    strval = Globalitems.HandleSingleQuoteForSQL(txtCity.Text.Trim());
                    strSQL += "AND loc.City LIKE '" + strval + "%' ";
                }

                //Add State to WHERE clause 
                if (txtState.Text.Trim().Length > 0)
                {
                    strval = Globalitems.HandleSingleQuoteForSQL(txtState.Text.Trim());
                    strSQL += "AND loc.State LIKE '" + strval + "%' ";
                }

                //Add Zip to WHERE clause 
                if (txtZip.Text.Trim().Length > 0)
                {
                    strval = Globalitems.HandleSingleQuoteForSQL(txtZip.Text.Trim());
                    strSQL += "AND loc.Zip LIKE '" + strval + "%' ";
                }

                strSQL += " ORDER BY Exporter";

                ds = DataOps.GetDataset_with_SQL(strSQL);

                // Use a DataTable as the DataSource for the DataGridView to make sorting by Col Header
                //  clicks, automatic
                dtExporters = ds.Tables[0].Copy();
                if (dtExporters.Rows.Count == 0) return;

                //6. If data found:
                //6a. Enable Export button
                btnExport.Enabled = true;

                //6b. Assign Datatable to gridvirew
                dgResults.DataSource = dtExporters;

                //6c. Update # records label
                lblForwarderRecords.Text = "Records: " + dtExporters.Rows.Count;

                //6d. Because dgResults is not multiselect, 
                //create a list of 1 CustomerID for the Form's binding source, 
                //for use by the nav buttons
                lsForwarderIDs.Clear();
                lsForwarderIDs.Add(int.Parse(dtExporters.Rows[0]["ForwarderID"].ToString()));

                bs1.DataSource = lsForwarderIDs;

                //6e. Fill detail record with first row CustomerID
                FillDetailRecord(lsForwarderIDs[0]);

                //6f. Update recbuttons
                recbuttons.blnRecordsToDisplay = true;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                Globalitems.SetNavButtons(recbuttons, bs1);

                //6g. Set Form unique controls
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSearch", ex.Message);
            }
        }

        private string SQLForModifiedExporter(List<ControlInfo> lsChangedItems)
        {
            try
            {
                ComboBox cboBox;
                Control[] ctrls;
                string strSQL = "";
                string strval;

                strSQL = "UPDATE AEExporter SET ";

                foreach (ControlInfo ctrlinfo in lsChangedItems)
                {
                    strSQL += ctrlinfo.RecordFieldName + " = ";

                    //Place the control into the array ctrls, s/b only one
                    ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                    //Omit CustomerID, not in AEExporter table
                    if (ctrlinfo.ControlID != "cboCust_record")
                    {
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
                }

                //Add UpdatedBy, UpdatedDate
                strSQL += "UpdatedBy = '" + Globalitems.strUserName + "',";
                strSQL += "UpdatedDate = CURRENT_TIMESTAMP";

                // Add WHERE clause
                strSQL += " WHERE AEExporterID = " + txtExporterID_record.Text;

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForModifiedForwarder", ex.Message);
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

        private string SQLForNewExporter(string strCreationDate)
        {
            try
            {
                string strSQL = @"INSERT INTO AEExporter (ExporterName, 
                    ExporterShortName, Notes, 
                    RecordStatus, CreationDate, CreatedBy, 
                    AEFreightForwarderID,CustomerExporterCode) VALUES (";

                string strval;

                // ExporterName
                strval = "";
                if (txtExporter_record.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtExporter_record.Text.Trim());
                strSQL += "'" + strval + "',";

                // ShortName
                strval = "";
                if (txtShortName_record.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtShortName_record.Text.Trim());
                strSQL += "'" + strval + "',";

                // Notes
                strval = "";
                if (txtNotes.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtNotes.Text.Trim());
                strSQL += "'" + strval + "',";

                // Record Status
                strval = (cboStatus_record.SelectedItem as ComboboxItem).cboValue;
                if (strval == "select") strval = "";
                strSQL += "'" + strval + "',";

                // CreationDate
                strSQL += "'" + strCreationDate + "',";

                //CreatedBy
                strSQL += "'" +
                    Globalitems.HandleSingleQuoteForSQL(Globalitems.strUserName) +
                    "',";

                //AEFreightForwarderID
                strSQL += (cboForwarders_record.SelectedItem as ComboboxItem).cboValue +
                    ",";

                //CustomerExporterCode
                strval = "";
                if (txtCustExporterCode.Text.Trim().Length > 0)
                    strval = Globalitems.HandleSingleQuoteForSQL(txtCustExporterCode.Text.Trim());
                strSQL += "'" + strval + "');";

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForNewForwarder", ex.Message);
                return "";
            }
        }

        private string SQLForNewLocation(string strCreationDate)
        {
            //1/12/18 Set ParentRecordID to ~ if no txtExporterID_record

            try
            {
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

                //ParentRecordID. Use ~ if new Exporter so SProc can replace
                if (txtExporterID_record.Text.Length > 0)
                    strSQL += txtExporterID_record.Text + ",";
                else
                    strSQL += "~,";

                //ParentRecordTable
                strSQL += "'AEExporter',";

                //LocationType
                strval = (cboLocType.SelectedItem as ComboboxItem).cboValue;
                strSQL += "'" + strval + "',";

                //LocationSubType
                strSQL += "'" + (cboLocSubtype.SelectedItem as ComboboxItem).cboValue + "',";

                //LocationName
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(txtLocName_record.Text) + "',";

                //LocationShortName
                strval = "";
                if (txtShortLocName_record.Text.Trim().Length > 0)
                    strval = txtShortLocName_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AddressLine1
                strval = "";
                if (txtAddr1_record.Text.Trim().Length > 0) strval = txtAddr1_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AddressLine2
                strval = "";
                if (txtAddr2_record.Text.Trim().Length > 0) strval = txtAddr2_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //City
                strval = "";
                if (txtCity_record.Text.Trim().Length > 0) strval = txtCity_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //State
                strval = "";
                if (txtState_record.Text.Trim().Length > 0) strval = txtState_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //Zip
                strval = "";
                if (txtZip_record.Text.Trim().Length > 0) strval = txtZip_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //Country
                strval = "";
                if ((cboCountry.SelectedItem as ComboboxItem).cboValue != "select")
                    strval = (cboCountry.SelectedItem as ComboboxItem).cboValue;
                strSQL += "'" + strval + "',";

                //MainPhone
                strval = "";
                if (txtMainPhone_record.Text.Trim().Length > 0)
                    strval = txtMainPhone_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //FaxNumber
                strval = "";
                if (txtMainFax_record.Text.Trim().Length > 0)
                    strval = txtMainFax_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactFirstName
                strval = "";
                if (txtPrimaryFName_record.Text.Trim().Length > 0)
                    strval = txtPrimaryFName_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactLastName
                strval = "";
                if (txtPrimaryLName_record.Text.Trim().Length > 0)
                    strval = txtPrimaryLName_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactPhone
                strval = "";
                if (txtPrimaryPhone_record.Text.Trim().Length > 0)
                    strval = txtPrimaryPhone_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactExtension
                strval = "";
                if (txtPrimaryExt_record.Text.Trim().Length > 0)
                    strval = txtPrimaryExt_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactCellPhone
                strval = "";
                if (txtPrimaryCell_record.Text.Trim().Length > 0)
                    strval = txtPrimaryCell_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //PrimaryContactEmail
                strval = "";
                if (txtPrimaryEmail_record.Text.Trim().Length > 0)
                    strval = txtPrimaryEmail_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactFirstName
                strval = "";
                if (txtAlternateFName_record.Text.Trim().Length > 0)
                    strval = txtAlternateFName_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactLastName
                strval = "";
                if (txtAlternateLName_record.Text.Trim().Length > 0)
                    strval = txtAlternateLName_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactPhone
                strval = "";
                if (txtAlternatePhone_record.Text.Trim().Length > 0)
                    strval = txtAlternatePhone_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactExtension
                strval = "";
                if (txtAlternateExt_record.Text.Trim().Length > 0)
                    strval = txtAlternateExt_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactCellPhone
                strval = "";
                if (txtAlternateCell_record.Text.Trim().Length > 0)
                    strval = txtAlternateCell_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //AlternateContactEmail
                strval = "";
                if (txtAlternateEmail_record.Text.Trim().Length > 0)
                    strval = txtAlternateEmail_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //OtherPhone1Description
                strval = "";
                if (txtOtherPhoneDesc1_record.Text.Trim().Length > 0)
                    strval = txtOtherPhoneDesc1_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //OtherPhone1
                strval = "";
                if (txtOtherPhone1_record.Text.Trim().Length > 0)
                    strval = txtOtherPhone1_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //OtherPhone2Description
                strval = "";
                if (txtOtherPhoneDesc2_record.Text.Trim().Length > 0)
                    strval = txtOtherPhoneDesc2_record.Text.Trim();
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //OtherPhone2
                strval = "";
                if (txtOtherPhone2_record.Text.Trim().Length > 0)
                    strval = txtOtherPhone2_record.Text.Trim();
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

        private bool ValidRecord()
        {
            try
            {
                DataSet ds;
                string strSQL;
                string strval;

                //Use linq to see what controls changed
                var changedlist_primary = lsControls_primary.Where(ctrlinfo => ctrlinfo.Updated == true).ToList();
                var changedlist_address = lsControls_address.Where(ctrlinfo => ctrlinfo.Updated == true).ToList();

                //Check Customer
                if ((cboCust_record.SelectedItem as ComboboxItem).cboValue == "select")
                {
                    MessageBox.Show("Please select the Customer.",
                        "MISSING CUSTOMER",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboCust_record.Focus();
                    return false;
                }

                //Check Forwarder
                if ((cboForwarders_record.SelectedItem as ComboboxItem).cboValue == "select")
                {
                    MessageBox.Show("Please select the Forwarder.", 
                        "MISSING FREIGHT FORWARDER",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboForwarders_record.Focus();
                    return false;
                }

                //Ck Exporter Name
                if (txtExporter_record.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Forwarder Name cannot be blank.", 
                        "MISSING FORWARDER NAME",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtExporter_record.Focus();
                    return false;
                }

                //Make sure Exporter Name is unique
                strSQL = "SELECT AEExporterID FROM AEExporter " +
                    "WHERE AEExporterID <> " + txtExporterID_record.Text +
                    " AND RTRIM(ISNULL(ExporterName,'')) = " +
                    "'" + Globalitems.HandleSingleQuoteForSQL(txtExporter_record.Text.Trim()) + 
                    "'";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ValidRecord",
                        "Query to check unique Exporter Name " +
                        "returned no table.");
                    return false;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("The Exporter Name must be unique.\n\n" +
                        "Another Exporter already has this Name.", 
                        "DUPLICATE EXPORTER NAME",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtExporter_record.Focus();
                    txtExporter_record.Select(0, txtExporter_record.Text.Length);
                    return false;
                }

                //If entered, make sure Exporter Short Name is unique
                if (txtShortName_record.Text.Trim().Length > 0)
                {
                    strSQL = "SELECT AEExporterID FROM AEExporter " +
                    "WHERE AEExporterID <> " + txtExporterID_record.Text +
                    " AND RTRIM(ISNULL(ExporterShortName,'')) = " +
                    "'" + Globalitems.HandleSingleQuoteForSQL(txtShortName_record.Text.Trim()) 
                    + "'";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "ValidRecord",
                            "Query to check unique Exporter Short Name " +
                            "returned no table.");
                        return false;
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("The Exporter Short Name must be unique.\n\n" +
                            "Another Exporter already has this Short Name.",
                            "DUPLICATE EXPORTER SHORT NAME",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtShortName_record.Focus();
                        txtShortName_record.Select(0, txtExporter_record.Text.Length);
                        return false;
                    }
                }

                //Make sure Record Status is not blank
                if ((cboStatus_record.SelectedItem as ComboboxItem).cboValue == "select")
                {
                    MessageBox.Show("Record Status cannot be blank.", "MISSING RECORD STATUS",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboStatus_record.Focus();
                    return false;
                }

                //If cboCountry is US or Canada, State & Zip must be entered
                if ((cboCountry.SelectedItem as ComboboxItem).cboValue == "U.S.A." ||
                    (cboCountry.SelectedItem as ComboboxItem).cboValue == "Canada")
                {
                    if (txtState_record.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("The State " +
                            "must be entered for the U.S.A and Canada.",
                            "MISSING STATE",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtState_record.Focus();
                        return false;
                    }

                    if (txtZip_record.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("The Zip code " +
                            "must be entered for the U.S.A and Canada.",
                            "MISSING ZIP CODE",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtZip_record.Focus();
                        return false;
                    }
                }

                //Ck any emails entered
                if (txtPrimaryEmail_record.Text.Trim().Length > 0)
                {
                    strval = txtPrimaryEmail_record.Text.Trim();
                    if (!Globalitems.validemailaddress(strval))
                    {
                        MessageBox.Show("The Primary Contact Email is not valid",
                            "INVALID EMAIL ADDRESS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPrimaryEmail_record.Focus();
                        txtPrimaryEmail_record.Select(0, strval.Length);
                        return false;
                    }
                }

                if (txtAlternateEmail_record.Text.Trim().Length > 0)
                {
                    strval = txtAlternateEmail_record.Text.Trim();
                    if (!Globalitems.validemailaddress(strval))
                    {
                        MessageBox.Show("The Alternate Contact Email is not valid",
                            "INVALID EMAIL ADDRESS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtAlternateEmail_record.Focus();
                        txtAlternateEmail_record.Select(0, strval.Length);
                        return false;
                    }
                }

                //Check if nothing's changed
                if (strMode == "MODIFY")
                {
                    if (changedlist_primary.Count == 0 &&
                       strMode_address == "MODIFY" &&
                       changedlist_address.Count == 0)
                    {
                        MessageBox.Show("You have not changed anything for this Exporter.\r\n" +
                           "There is nothing to update", "NO CHANGES MADE",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                //Validate Addr info 
                if (!InfoChangeValid(changedlist_address)) return false;

                //Recheck if nothing's changed for MODIFY after Addr check  
                if (strMode == "MODIFY")
                {
                    if (changedlist_primary.Count == 0 &&
                       strMode_address == "" &&
                       changedlist_address.Count == 0)
                    {
                        MessageBox.Show("You have not changed anything for this Exporter.\r\n" +
                           "There is nothing to update", "NO CHANGES MADE",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void ZipCodeCheck()
        {
            try
            {
                AddressInfo objAddress = null;

                if (strMode == "READ") return;
                if (blnIgnoreZipcode) return;
                if (cboCountry.SelectedItem == null) return;

                //Check zip code, if Country is US
                if ((cboCountry.SelectedItem as ComboboxItem).cboValue == "U.S.A." &&
                    txtZip_record.Text.Trim().Length > 0)
                {
                    objAddress = Formops.CheckZipCode(this, txtZip_record, ref blnIgnoreZipcode);
                    if (objAddress == null) return;
                    if (objAddress.error.Length > 0) return;

                    //OK Zip code, set City, State
                    if (objAddress.city.Length > 0)
                    {
                        txtCity_record.Text = objAddress.city;
                        txtState_record.Text = objAddress.state;
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ZipCodeCheck", ex.Message);
            }
        }

        private void btnCancel_Clicked()
        { CancelSetup(); }

        private void btnDelete_Clicked()
        { PerformDeleteRecord(); }

        private void btnModify_Clicked()
        { ModifyRecordSetup(); }

        private void btnNext_Clicked()
        { PerformMoveNext(); }

        private void btnNew_Clicked()
        { NewRecordSetup(); }

        private void btnPrev_Clicked()
        {
            try { PerformMovePrevious(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnPrev_Clicked", ex.Message); }
        }

        private void btnSave_Clicked()
        { PerformSaveRecord(); }

        private void ckActive_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ClearForm();
                FillCombos();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ckActive_CheckedChanged", ex.Message);
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            { ClearForm(); }
        }

        private void dgResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //As long as row clicked is not the Column Header row, index = -1, change the binding source
            if (e.RowIndex > -1) FilterBindingSource();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            //Make sure Main form displays and has the focus
            Globalitems.MainForm.Visible = true;
            Globalitems.MainForm.Focus();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {OpenCSVFile();}

        private void cboCust_record_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefillcboForwarders(false);
            cboForwarders_record.SelectedIndex = 0;
        }

        private void txtExporter_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtExporter_record",
                    lsControls_primary);
        }

        private void txtShortName_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtShortName_record",
                    lsControls_primary);
        }

        private void txtCustForwarderCode_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void cboStatus_record_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY")
                Formops.ChangeControlUpdatedStatus("cboStatus_record",
                    lsControls_primary);
        }

        private void txtNotes_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtNotes",
                    lsControls_primary);
        }

        private void txtLocName_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtLocName_record",
                    lsControls_address);
        }

        private void txtShortLocName_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtShortLocName_record",
                    lsControls_address);
        }

        private void cboLocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("cboLocType",
                    lsControls_address);
        }

        private void cboLocSubtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("cboLocSubtype",
                    lsControls_address);
        }

        private void txtAddr1_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAddr1_record",
                    lsControls_address);
        }

        private void txtAddr2_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAddr2_record",
                    lsControls_address);
        }

        private void txtCity_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtCity_record",
                    lsControls_address);
        }

        private void txtState_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtState_record",
                    lsControls_address);
        }

        private void txtZip_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtZip_record",
                    lsControls_address);
        }

        private void cboCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZipCodeCheck();

            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("cboCountry",
                    lsControls_address);
        }

        private void txtMainPhone_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtMainPhone_record",
                    lsControls_address);
        }

        private void txtMainFax_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtMainFax_record",
                    lsControls_address);
        }

        private void txtPrimaryFName_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryFName_record",
                    lsControls_address);
        }

        private void txtPrimaryLName_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryLName_record",
                    lsControls_address);
        }

        private void txtPrimaryPhone_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryPhone_record",
                    lsControls_address);
        }

        private void txtPrimaryExt_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryExt_record",
                    lsControls_address);
        }

        private void txtPrimaryCell_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryCell_record",
                    lsControls_address);
        }

        private void txtPrimaryEmail_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtPrimaryEmail_record",
                    lsControls_address);
        }

        private void txtAlternateFName_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateFName_record",
                    lsControls_address);
        }

        private void txtAlternateLName_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateLName_record",
                    lsControls_address);
        }

        private void txtAlternatePhone_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternatePhone_record",
                    lsControls_address);
        }

        private void txtAlternateExt_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateExt_record",
                    lsControls_address);
        }

        private void txtAlternateCell_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateCell_record",
                    lsControls_address);
        }

        private void txtAlternateEmail_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtAlternateEmail_record",
                    lsControls_address);
        }

        private void txtOtherPhoneDesc1_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtOtherPhoneDesc1_record",
                    lsControls_address);
        }

        private void txtOtherPhone1_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtOtherPhone1_record",
                    lsControls_address);
        }

        private void txtOtherPhoneDesc2_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtOtherPhoneDesc2_record",
                    lsControls_address);
        }

        private void txtOtherPhone2_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && strMode_address == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtOtherPhone2_record",
                    lsControls_address);
        }

        private void txtZip_record_Leave(object sender, EventArgs e)
        {ZipCodeCheck();}

        private void btnClearAddr_Click(object sender, EventArgs e)
        {
            Formops.ClearRecordData(this, lsControls_address, false);
        }

        private void RefillcboForwarders(bool blnBoth = true)
        {
            //If blnBoth = true, fill cboForwarders_record with same items as cboForwarders
            //If blnBoth = false, fill cboForwarders_record with items for cboCust_record
            //  SelectedItem, if not <select> 
            try
            {
                ComboboxItem cboitem;
                ComboboxItem cboitemcopy;
                DataSet ds;
                string strSELECT;
                string strSQL;

                //Fill cboForwarders
                cboForwarders_record.Items.Clear();

                //Start SQL for both cboForwarders, cboForwarders_record
                strSELECT = @"SELECT ff.AEFreightForwarderID,
                    CASE
                    WHEN LEN(RTRIM(ISNULL(ff.FreightForwarderShortName, ''))) > 0 THEN
                        RTRIM(ff.FreightForwarderShortName)
                    ELSE RTRIM(ff.FreightForwarderName)
                    END AS FreightForwarder 
                    FROM
                    AEFreightForwarder ff
                    LEFT OUTER JOIN Customer cus on cus.CustomerID = ff.AECustomerID
                    WHERE LEN(RTRIM(ISNULL(FreightForwarderName, ''))) > 0 ";

                if (ckActiveCust.Checked)
                    strSELECT += "AND cus.RecordStatus = 'Active' ";

                if (ckActiveFF.Checked)
                    strSELECT += "AND ff.RecordStatus = 'Active' ";

                //If Refill cboCust, refill from the items in the above query
                if (blnBoth)
                {
                    cboForwarders.Items.Clear();

                    strSQL = strSELECT;

                    //Add Cust if not All
                    if ((cboCust.SelectedItem as ComboboxItem).cboValue != "All")
                    {
                        strSQL += "AND ff.AECustomerID = " +
                            (cboCust.SelectedItem as ComboboxItem).cboValue;
                    }

                    strSQL += " ORDER BY FreightForwarder";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "RefillcboForwarders",
                            "No rows returned from FreightForwarder table");
                        return;
                    }

                    //Add All as 1st item
                    cboitem = new ComboboxItem();
                    cboitem.cboText = "All";
                    cboitem.cboValue = "All";
                    cboForwarders.Items.Add(cboitem);

                    //Add retrieved items to cboForwarders
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        cboitem = new ComboboxItem();
                        cboitem.cboText = dr["FreightForwarder"].ToString();
                        cboitem.cboValue = dr["AEFreightForwarderID"].ToString();

                        cboForwarders.Items.Add(cboitem);
                    }

                    cboForwarders.DisplayMember = "cboText";
                    cboForwarders.ValueMember = "cboValue";
                    cboForwarders.SelectedIndex = 0;
                }

                //Fill cboForwarders_record. If strMode = READ,
                //  fill same as cboForwarders. If strMode = NEW restrict to
                //  cboCust_record, if Customer is selected
                //Add <select> as 1st item
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboForwarders_record.Items.Add(cboitem);

                cboForwarders_record.DisplayMember = "cboText";
                cboForwarders_record.ValueMember = "cboValue";
                cboForwarders_record.SelectedIndex = -1;

                if (strMode == "READ")
                {
                    foreach (ComboboxItem cbofwditem in cboForwarders.Items)
                    {
                        if (cbofwditem.cboValue != "All")
                        {
                            cboitemcopy = cbofwditem.MakeCopy(cbofwditem);
                            cboForwarders_record.Items.Add(cboitemcopy);
                        }
                    }
                }
                else
                {
                    //Mode is NEW/MODIFY, retrieve Forwarders based on cboCust_record
                    if ((cboCust_record.SelectedItem as ComboboxItem).cboValue != "select")
                    {
                        strSQL = strSELECT + "AND ff.AECustomerID = " +
                        (cboCust_record.SelectedItem as ComboboxItem).cboValue;
                        strSQL += " ORDER BY FreightForwarder";

                        ds = DataOps.GetDataset_with_SQL(strSQL);
                        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "RefillcboForwarders",
                                "No rows returned from FreightForwarder table");
                            return;
                        }

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            cboitem = new ComboboxItem();
                            cboitem.cboText = dr["FreightForwarder"].ToString();
                            cboitem.cboValue = dr["AEFreightForwarderID"].ToString();
                            cboForwarders_record.Items.Add(cboitem);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "RefillcboForwarders", ex.Message);
            }
        }

        private void cboCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!blnClearingForm && !blnFillingcbos) RefillcboForwarders(); 
        }

        private void txtExporterCode_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY")
                Formops.ChangeControlUpdatedStatus("txtCustExporterCode",
                    lsControls_primary);
        }

        private void cboForwarders_record_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY" && !blnModifySetup)
                Formops.ChangeControlUpdatedStatus("cboForwarders_record",
                    lsControls_primary);
        }

        private void frmExporterAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (strMode != "READ" && !Globalitems.blnException)
            {
                MessageBox.Show("You must SAVE or Cancel the current changes to close this form",
                   "CANNOT CLOSE THIS FORM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void frmExporterAdmin_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
