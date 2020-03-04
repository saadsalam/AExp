using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmDestinationAdmin : Form
    {
        public bool blnNewDestinationRQFromOtherForm = false;

        private const string CURRENTMODULE = "frmDestinationAdmin";

        private bool blnInitialFill = true;
        private BindingSource bs1 = new BindingSource();
        private DataTable dtDestinations = new DataTable();
        private List<string> lsDestinationIDs = new List<string>();
        private string strCurrentCustomerID;
        private string strCurrentColorDesc;
        private string strCustColorMode;
        private string strMode;
        private Color colSelected;
        private Color colCurrent;

        //Set up List of ControlInfo objects, lsControlInfo, as follows:
        //  Order in list establishes Indexes for tabbing, implemented by SetTabIndex() method
        //  AlwaysReadOnly identifies if control cannot be modified by User
        //  ControlPropertyToBind identifies what controls are initialized 
        //  RecordFieldName identify what controls display record detail
        //  HeaderText sets column header to use for Export to csv file
        //  Updated property establishes what controls User has modified

        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="txtDestination",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtHandheldAbbrev",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtSheetColor",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboCust",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="cboStatus",ControlPropetyToBind="SelectedValue"}, 
            new ControlInfo {ControlID="txtDestination_record", RecordFieldName="Destination",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtHandheldAbbrev_record",
                RecordFieldName ="Abbrev",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboCust_record",ControlPropetyToBind="SelectedValue",
             RecordFieldName="CustomerID"},
            new ControlInfo {ControlID="txtSheetColor_record",
                RecordFieldName ="ColorDesc",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="btnColor"},
            new ControlInfo {ControlID="cboStatus_record",ControlPropetyToBind="SelectedValue",
             RecordFieldName="RecordStatus"},
            new ControlInfo {ControlID="txtCreationDate", RecordFieldName="CreationDate",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCreatedBy", RecordFieldName="CreatedBy",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtUpdatedDate", RecordFieldName="UpdatedDate",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtUpdatedBy", RecordFieldName="UpdatedBy",
                ControlPropetyToBind="Text"},
            
            // objects needed for csv file  HeaderText="Cust. Name"
            new ControlInfo {RecordFieldName="Destination",HeaderText="Destination"},
            new ControlInfo {RecordFieldName="Abbrev",HeaderText="Handheld Abbrev."},
            new ControlInfo {RecordFieldName="ColorDesc",HeaderText="Color Desc."},
            new ControlInfo {RecordFieldName="RecordStatus",HeaderText="Status"}
        };

        public frmDestinationAdmin()
        {
            InitializeComponent();
            dgResults.AutoGenerateColumns = false;
            btnExport.Enabled = false;
            lblDestinationRecords.Text = "";

            strMode = "READ";
            strCustColorMode = "READ";
            FillCombos();

            Globalitems.SetControlHeight(this);
            Formops.SetTabIndex(this, lsControls);

            // Assign methods to the recbuttons public event variables
            recbuttons.CancelRecord += btnCancel_Clicked;
            recbuttons.MovePrev += btnPrev_Clicked;
            recbuttons.MoveNext += btnNext_Clicked;
            recbuttons.DeleteRecord += btnDelete_Clicked;
            recbuttons.NewRecord += btnNew_Clicked;
            recbuttons.ModifyRecord += btnModify_Clicked;
            recbuttons.SaveRecord += btnSave_Clicked;

            DisplayMode();

            txtSheetColor_record.ReadOnly = false;
        }

        private void frmDestinationAdmin_Activated(object sender, EventArgs e)
        {
            if (blnNewDestinationRQFromOtherForm)
            {
                blnNewDestinationRQFromOtherForm = false;
                recbuttons.btnNew_Click(null, null);
            }
        }

        private void AdjustReadOnlyCustColorStatus(bool blnReadOnly)
        {
            try
            {
                cboCust_record.Enabled = !blnReadOnly;
                txtSheetColor_record.Enabled = !blnReadOnly;
                btnColor.Enabled = !blnReadOnly;
                btnSaveCustColor.Enabled = !blnReadOnly;
                btnCancelCustColor.Enabled = !blnReadOnly;

                //Enable New if Destination
                if (txtDestination_record.Text.Trim().Length > 0)
                    btnNewCustColor.Enabled = true;
                else
                    btnNewCustColor.Enabled = false;

                //Enable Modify if Customer 
                if (cboCust_record.SelectedIndex == -1 || 
                    (cboCust_record.SelectedItem as ComboboxItem).cboValue == "select")
                {
                    btnModCustColor.Enabled = false;
                    btnDelCustColor.Enabled = false;
                }
                else
                {
                    btnModCustColor.Enabled = true;
                    btnDelCustColor.Enabled = true;
                }
                    
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "AdjustReadOnlyCustColorStatus",
                    ex.Message);
            }
        }

        private void AdjustReadOnlyStatus(bool blnReadOnly)
        {
            Formops.SetReadOnlyStatus(this, lsControls, blnReadOnly, recbuttons);
            AdjustReadOnlyCustColorStatus(true);
        }

        private void CancelSetup()
        {
            int intCurrentBSPosition = -1;

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
            btnMenu.Enabled = true;

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
                FillDetailRecord();
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

                //3. Set Form unique Readonly/enabled status for controls
                AdjustReadOnlyStatus(true);

                //4. Clear Record Data
                Formops.ClearRecordData(this, lsControls);
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearForm", ex.Message);
            }
        }

        private void ClearGridView()
        {
            try
            {
                lblRecordDetails.Text = "";

                dtDestinations.Clear();

                // Binding dgResults to lsUsers after the Clear method, can lead to runtime error because
                //  the CurrencyManager pointing to the Current position in lsUsers, doesn't reset to -1
                dgResults.DataSource = dtDestinations;

                recbuttons.blnRecordsToDisplay = false;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearGridView", ex.Message);
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

                    if (strMode == "NEW" && bs1 != null) Formops.ClearRecordData(this, lsControls);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "DisplayMode", ex.Message);
            }
        }

        private void FillCombos()
        {
            //Use FillComboboxFromCodeTable in Globalitems to fill comboboxes from dtCode
            try
            {
                ComboboxItem cboitem;
                ComboboxItem cboitem_copy;
                DataSet ds;
                string strFilter;
                string strSQL;

                // To retrieve Selected value from a combobox need to assign a 
                //datasource to the combobox
                if (blnInitialFill)
                {
                    strFilter = "CodeType = 'RecordStatus'";
                    Globalitems.FillComboboxFromCodeTable(strFilter, cboStatus, true, false);
                    Globalitems.FillComboboxFromCodeTable(strFilter, cboStatus_record, false, true);

                    //Set to Active
                    foreach (ComboboxItem cbitem in cboStatus.Items)
                        if (cbitem.cboValue == "Active") cboStatus.SelectedItem = cbitem;

                    blnInitialFill = false;
                }

                //cboCust
                cboCust.Items.Clear();
                strSQL = @"SELECT CustomerID, 
                    CASE 
                        WHEN LEN(RTRIM(ISNULL(ShortName,''))) > 0 THEN RTRIM(ShortName) 
                        ELSE RTRIM(CustomerName) 
                    END AS CustName
                    FROM Customer ";

                strFilter = (cboStatus.SelectedItem as ComboboxItem).cboValue;
                if (strFilter != "All") strSQL += "WHERE RecordStatus = '" +
                        strFilter + "' ";                    
                strSQL += " ORDER BY CustName";

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

                // Add Select to cboCust_record
                cboCust_record.Items.Clear();
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

                    cboitem_copy = cboitem.MakeCopy(cboitem);
                    cboCust_record.Items.Add(cboitem_copy);
                }

                cboCust.DisplayMember = "cboText";
                cboCust.ValueMember = "cboValue";
                cboCust.SelectedIndex = 0;

                cboCust_record.DisplayMember = "cboText";
                cboCust_record.ValueMember = "cboValue";
                cboCust_record.SelectedIndex = -1;

               
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void FillDetailRecord()
        {
            DataRow drow;
            DataSet ds;
            DataTable dtDetail;
            string strSQL;

            try
            {
                Formops.ClearRecordData(this, lsControls);

                //Qry returns a row for each CustomerID/DestinationName combination
                strSQL = @"SELECT 
                dest.CodeID AS CodeID_dest,
                color.CodeID AS CodeID_color,
                ISNULL(color.Value1,'WHITE') AS Color,
                cus.CustomerID,
                dest.Code AS Destination,
                dest.Value2 AS Abbrev,
                CASE
	                WHEN LEN(RTRIM(ISNULL(cus.ShortName,''))) > 0 THEN RTRIM(cus.ShortName)
	                WHEN LEN(RTRIM(ISNULL(cus.CustomerName,''))) > 0 THEN RTRIM(cus.CustomerName)
	                ELSE ''
                END AS Customer,
                ISNULL(color.Value1Description,'') AS ColorDesc,
                dest.RecordStatus,
                dest.CreationDate,
                dest.CreatedBy,
                dest.UpdatedDate,
                dest.UpdatedBy
                FROM
                Code dest
                LEFT OUTER JOIN Code color on color.CodeType='CustomsSheetColor' AND 
                    color.Description=dest.CodeID
                LEFT OUTER JOIN Customer cus on cus.CustomerID=color.Value2
                WHERE dest.CodeType='ExportDischargePort' 
                AND dest.CodeID =  " + txtCodeID_dest.Text;

                //Add ColorID if not 0
                if (txtCodeID_color.Text != "0")
                    strSQL += " AND color.CodeID = " + txtCodeID_color.Text;
                
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillDetailRecord",
                        "No data returned from query");
                    return;
                }

                dtDetail = ds.Tables[0];


                //Set Destination info from 1st Row
                //  (same info on each row)
                drow = dtDetail.Rows[0];
                Formops.SetDetailRecord(drow, this, lsControls);

                //Set pnlColor
                //Sheet Color Desc: Value1Description
                //Sheet Color: Value1
                pnlColor.BackColor = GetColorValue(drow["Color"].ToString());

                //Format Date textboxes
                txtCreationDate.Text = Globalitems.FormatDatetime(txtCreationDate.Text);
                txtUpdatedDate.Text = Globalitems.FormatDatetime(txtUpdatedDate.Text);

                AdjustReadOnlyCustColorStatus(true);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillDetailRecord", ex.Message);
            }
        }

        private void FilterBindingSource()
        {
            try
            {
                //User has selected a row in the Gridview, dgResults.
                //Change the current binding source to the selected row

                //Because dgResults is not multiselect, 
                //create a list of 1 VoyageID for the Form's binding source, 
                //for use by the nav buttons
                lsDestinationIDs.Clear();
                lsDestinationIDs.Add(dgResults.SelectedRows[0].Cells["CodeID_dest"].Value.ToString());

                bs1.DataSource = lsDestinationIDs;

                //6e. Fill detail record with first row AEVoyageID
                txtCodeID_dest.Text = lsDestinationIDs[0];
                txtCodeID_color.Text =
                    dgResults.SelectedRows[0].Cells["CodeID_color"].Value.ToString();
                FillDetailRecord();

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

        private void NewRecordSetup()
        {
            try
            {
                //1. Set Mode
                strMode = "NEW";

                //2. Set Status label
                lblStatus.Text = "Add new record";

                //3. Disable Search/Results panels, & related buttons
                pnlSearch.Enabled = false;
                pnlResults.Enabled = false;
                btnSearch.Enabled = false;
                btnClear.Enabled = false;
                btnExport.Enabled = false;
                btnMenu.Enabled = false;
                txtSheetColor_record.Enabled = false;

                //4. Hide Other Forms
                Globalitems.DisplayOtherForms(this, false);

                //5. Turn off ReadOnly on form's controls
                AdjustReadOnlyStatus(false);

                //6. Clear Record controls
                Formops.ClearRecordData(this, lsControls);

                //7. Set recbuttons to New
                recbuttons.SetButtons(RecordButtons.ACTION_NEWRECORD);

                //8. Set Created By/Date 
                txtCreatedBy.Text = Globalitems.strUserName;
                txtCreationDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

                //9. Set focus on first contol
                txtDestination_record.Focus();

                //10. Handle Form unique controls
                cboStatus_record.SelectedValue = "Active";
                pnlColor.BackColor = System.Drawing.Color.White;
                btnNewCustColor.Enabled = false;
                btnModCustColor.Enabled = false;
                btnDelCustColor.Enabled = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "NewRecordSetup", ex.Message);
            }
        }

        private void CancelCustomerColor()
        {
            try
            {
                AdjustReadOnlyCustColorStatus(true);
                lblCustColor.Text = "Read Only";
                strCustColorMode = "READ";
                recbuttons.Enabled = true;

                pnlSearch.Enabled = true;
                pnlResults.Enabled = true;
                btnSearch.Enabled = true;
                btnClear.Enabled = true;
                btnExport.Enabled = true;
                btnMenu.Enabled = true;

                FillDetailRecord();

                Globalitems.DisplayOtherForms(this, true);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CancelCustomerColor", ex.Message);
            }
        }


        private void NewCustomerColorRecordSetup()
        {
            try
            {
                strCustColorMode = "NEW";
                lblCustColor.Text = "New Cust./Color";
                lblRGB.Text = "";
                txtSheetColor_record.Enabled = true;

                pnlSearch.Enabled = false;
                pnlResults.Enabled = false;
                btnSearch.Enabled = false;
                btnClear.Enabled = false;
                btnExport.Enabled = false;
                btnMenu.Enabled = false;

                cboCust_record.SelectedIndex = 0;
                cboCust_record.Enabled = true;
                txtSheetColor_record.Text = "";
                pnlColor.BackColor = System.Drawing.Color.White;
                btnColor.Enabled = true;
                btnModCustColor.Enabled = false;
                btnDelCustColor.Enabled = false;
                btnNewCustColor.Enabled = false;
                btnSaveCustColor.Enabled = true;
                btnCancelCustColor.Enabled = true;
                recbuttons.Enabled = false;

                Globalitems.DisplayOtherForms(this, false);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "NewCustomerColor", ex.Message);
            }
        }

        private void ModifyCustColorRecordSetup()
        {
            try
            {
                strCustColorMode = "MODIFY";
                lblCustColor.Text = "Modify Cust./Color";
                txtSheetColor_record.Enabled = true;

                //Disable other controls on form
                pnlSearch.Enabled = false;
                pnlResults.Enabled = false;
                btnMenu.Enabled = false;
                btnSearch.Enabled = false;
                btnClear.Enabled = false;
                btnExport.Enabled = false;

                //Enable/Disable Detail rec controls
                cboCust_record.Enabled = true;
                btnColor.Enabled = true;
                btnModCustColor.Enabled = false;
                btnDelCustColor.Enabled = false;
                btnNewCustColor.Enabled = false;
                btnSaveCustColor.Enabled = true;
                btnCancelCustColor.Enabled = true;
                recbuttons.Enabled = false;

                //Store current CustomerID, Color, ColorDesc
                strCurrentCustomerID = (cboCust_record.SelectedItem as ComboboxItem).cboValue;
                strCurrentColorDesc = txtSheetColor_record.Text;
                colCurrent = pnlColor.BackColor;

                //Hide other open forms
                Globalitems.DisplayOtherForms(this, false);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ModifyCustColorRecordSetup",
                    ex.Message);
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
                Formops.ResetControls(this, lsControls);

                //7. Set recbuttons to Modify
                recbuttons.SetButtons(RecordButtons.ACTION_MODIFYRECORD);

                //8. Set Updated By/Date to new value
                txtUpdatedBy.Text = Globalitems.strUserName;
                txtUpdatedDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

                //9. Set focus on first control
                txtDestination_record.Focus();

                //10. Handle Form unique controls
                btnNewCustColor.Enabled = false;
                btnModCustColor.Enabled = false;
                btnDelCustColor.Enabled = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ModifyRecordSetup", ex.Message);
            }
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
                strSQL = @"SELECT ValueKey,ValueDescription FROM SettingTable 
                    WHERE ValueKey IN ('ExportDirectory','DestinationExportFileName')
                    AND RecordStatus='Active' ORDER BY ValueKey DESC";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                // S/B just two active rows, row 1 ExportDirectory, row 2 VesselExportFileName
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 2)
                {
                    Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile",
                        "No rows returned from SettingTable");
                    return;
                }
                // 1st Record s/b ExportDirectory, 2nd Record s/b DestinationExportFileName
                strFilename = ds.Tables[0].Rows[0]["ValueDescription"].ToString();
                strFilename += @"\" + ds.Tables[0].Rows[1]["ValueDescription"].ToString();

                //2. Create a copy of the datatable used for the datagridview datasource
                dt = dtDestinations.Copy();

                //3. If the gridview is sorted, use a dv to sort the table copy the same way
                if (dgResults.SortedColumn != null)
                {
                    strSort = dgResults.SortedColumn.DataPropertyName;
                    if (dgResults.SortOrder == SortOrder.Descending) strSort += " DESC";
                    dv = new DataView(dt, "", strSort, DataViewRowState.CurrentRows);
                    dt = dv.ToTable();
                }

                //4. Create a list, lsCSVcols with ControlInfo objects in the order to appear in the csv file 
                //Get ctrlinfo object from lsControls for Vessel & add to lsCSVcols. Use HeaderText to ID objects
                var objctrlinfo_Dest = lsControls.First(obj => obj.HeaderText == "Destination");
                lsCSVcols.Add(objctrlinfo_Dest);

                //Handheld Abbrev.
                var objctrlinfo_Status = lsControls.First(obj => obj.HeaderText == "Status");
                lsCSVcols.Add(objctrlinfo_Status);

                var objctrlinfo_Abbrev = lsControls.First(obj => obj.HeaderText == "Handheld Abbrev.");
                lsCSVcols.Add(objctrlinfo_Abbrev);

                var objctrlinfo_Color = lsControls.First(obj => obj.HeaderText == "Color Desc.");
                lsCSVcols.Add(objctrlinfo_Color);

                //5. Invoke CreateSCVFile to create, save, & open the csv file
                Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile", ex.Message);
            }
        }

        private void PerformDeleteCustColorRecord()
        {
            try
            {
                frmAreYouSure frmConfirm;
                string strMessage;
                string strSQL;

                strMessage = "You are about to Delete a Customer/Color for this Destination!\n\n" +
                   "This will display the default color (White) for Shag Sheets.\n\n" +
                   "Are you sure you want to delete the Customer/Color?";

                frmConfirm = new frmAreYouSure(strMessage);
                var result = frmConfirm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    strSQL = "DELETE Code WHERE CodeID = " + txtCodeID_color.Text;
                    DataOps.PerformDBOperation(strSQL);

                    //Reload Code table into memory
                    Globalitems.SetUpGlobalVariables();

                    PerformSearch();

                    MessageBox.Show("The Customer/Color for this Destination has been removed from the DB", 
                        "CUSTOMER/COLOR DELETED",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformDeleteCustColorRecord",
                    ex.Message);
            }
        }

        private void PerformDeleteRecord()
        {
            try
            {
                DataSet ds;
                frmAreYouSure frmConfirm;
                string strMessage;
                string strSQL;

                //Check if AutoportExportVehicles table has veh's 
                //  for the Destination to delete
                strSQL = @"SELECT COUNT(AutoportExportVehiclesID) AS totrec " +
                    "FROM AutoportExportVehicles where DestinationName = '" + 
                    txtDestination_record.Text.Trim() + "'";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count==0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord",
                        "No data returned from the query when checking " +
                        "Veh. table");
                    return;
                }

                //There are veh's associated with the Vessel to delete 
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["totrec"]) > 0)
                {
                    MessageBox.Show("The Destination cannot be deleted because there are " +
                            "vehicles associated with the Destination in the DB.",
                            "DESTINATION CANNOT BE DELETED",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                //There are no veh's associated with the Destination.
                //Ck if there are voyages associated with the Destination
                strSQL = "SELECT COUNT(AEVoyageDestinationID) AS totrec " +
                    "FROM AEVoyageDestination where DestinationName = '" +
                    txtDestination_record.Text.Trim() + "'";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord",
                        "No data returned from the query when checking " +
                        "Voyage table");
                    return;
                }

                //There are veh's associated with the Vessel to delete 
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["totrec"]) > 0)
                {
                    MessageBox.Show("The Destination cannot be deleted because there are " +
                            "Voyages associated with the Destination in the DB.",
                            "DESTINATION CANNOT BE DELETED",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strMessage = "You are about to Delete a Destination!\n\n" +
                    "You could instead change the Status to Inactive.\n\n" +
                    "Are you sure you want to delete the Destination?";

                frmConfirm = new frmAreYouSure(strMessage);
                var result = frmConfirm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    //Delete the Destination
                    strSQL = "DELETE Code WHERE CodeID = " +
                        txtCodeID_dest.Text;
                    DataOps.PerformDBOperation(strSQL);

                    //Delete the Color Codes if any
                    strSQL = @"DELETE Code
                    where CodeType='CustomsSheetColor'
                    AND Description='" + txtCodeID_dest.Text + "'";
                    DataOps.PerformDBOperation(strSQL);

                    //Reload Code table into memory
                    Globalitems.SetUpGlobalVariables();

                    PerformSearch();

                    MessageBox.Show("The Destination has been removed from the DB", "DESTINATION DELETED",
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
                FillDetailRecord();
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMoveNext", ex.Message);
            }
        }

        private void PerformMovePrevious()
        {
            try
            {
                bs1.MovePrevious();
                FillDetailRecord();
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMovePrevious", ex.Message);
            }
        }

        private void PerformSaveCustColorRecord()
        {
            string strSQL_Color = "";

            try
            {
                if (ValidColorRecord())
                {
                    if (strCustColorMode == "NEW")
                        SQLForNewCustColorRecord(ref strSQL_Color);
                    else
                        SQLForModifiedCustColorRecord(ref strSQL_Color);

                    if (strSQL_Color.Length > 0)
                        DataOps.PerformDBOperation(strSQL_Color);

                    //Rerun SetUpGlobalVariables to reload dtCode into memory
                    Globalitems.SetUpGlobalVariables();

                    MessageBox.Show("The Customer/Color info has been updated in the DB.",
                       "CUSTOMER/COLOR INFO UPDATED", MessageBoxButtons.OK,
                       MessageBoxIcon.Information);

                    //Display other forms
                    Globalitems.DisplayOtherForms(this, true);

                    //Set Mode to READ
                    strCustColorMode = "READ";
                    recbuttons.Enabled = true;
                    AdjustReadOnlyCustColorStatus(true);

                    //Enable Search/Results panels
                    pnlSearch.Enabled = true;
                    pnlResults.Enabled = true;

                    btnSearch.Enabled = true;
                    btnClear.Enabled = true;

                    //Set Status label to Read only
                    lblCustColor.Text = "Read only";

                    //Perform new search
                    PerformSearch();

                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSaveCustColorRecord", 
                    ex.Message);
            }
        }

        private void PerformSaveRecord()
        {
            string strSQL_Dest = "";

            try
            {
                if (ValidRecord())
                {
                    if (strMode == "NEW")
                        SQLForNewRecord(ref strSQL_Dest);
                    else
                        SQLForModifiedRecord(ref strSQL_Dest);
                    
                    if (strSQL_Dest.Length > 0)
                        DataOps.PerformDBOperation(strSQL_Dest);

                    //Rerun SetUpGlobalVariables to reload dtCode into memory
                    Globalitems.SetUpGlobalVariables();

                    MessageBox.Show("The Destination info has been updated in the DB.",
                       "DESTINATION INFO UPDATED", MessageBoxButtons.OK, 
                       MessageBoxIcon.Information);

                    //Display other forms
                    Globalitems.DisplayOtherForms(this, true);

                    //Set Mode to READ
                    strMode = "READ";
                    recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                    AdjustReadOnlyStatus(true);

                    //Enable Search/Results panels
                    pnlSearch.Enabled = true;
                    pnlResults.Enabled = true;

                    btnSearch.Enabled = true;
                    btnClear.Enabled = true;

                    //Set Status label to Read only
                    lblStatus.Text = "Read only";

                    //Perform new search
                    PerformSearch();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord", ex.Message);
            }
        }

        private Color GetColorValue (string strColorCode)
        {
            try
            {
                int intRed;
                int intGreen;
                int intBlue;
                string[] strRGBs;
                Color dwColor;

                lblRGB.Text = "";

                //Color may be Name (e.g. ORANGE) or RGB value (e.g. 128,255,0)
                if (strColorCode.Contains(","))
                    {
                        strRGBs = strColorCode.Split(',');
                        if (strRGBs.Length != 3)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "GetColorValue",
                                "strColorCode did not split into 3 values");
                        }
                        intRed = Convert.ToInt16(strRGBs[0]);
                        intGreen = Convert.ToInt16(strRGBs[1]);
                        intBlue = Convert.ToInt16(strRGBs[2]);

                    //Set lblRGB
                    lblRGB.Text = "Red: " + strRGBs[0] + ", " +
                        "Green: " + strRGBs[1] + ", " +
                        "Blue: " + strRGBs[2];

                        dwColor = System.Drawing.Color.FromArgb(intRed, intGreen, intBlue);
                    }
                    else
                        dwColor = System.Drawing.Color.FromName(strColorCode);
                    

                    return dwColor;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetColorValue",ex.Message);
                return System.Drawing.Color.Transparent;
            }
        }

        private void SetgridViewColor()
        {
            //Set SheetColor image for each row, based on ColorCode value
            string strColorCode;
            Color dwColor;
            foreach (DataGridViewRow dgRow in dgResults.Rows)
            {
                //Get the ColorCode in the row
                strColorCode = dgRow.Cells["Color"].Value.ToString();
                dwColor = GetColorValue(strColorCode);
                dgRow.Cells["SheetColor"].Style.BackColor = dwColor;
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
                Formops.ClearRecordData(this, lsControls);

                //4. Set recbuttons to display = false
                recbuttons.blnRecordsToDisplay = false;

                //5. Retrieve data as datatable
                strSQL = @"SELECT 
                dest.CodeID AS CodeID_dest,
                ISNULL(color.CodeID,'0') AS CodeID_color,
                ISNULL(color.Value1,'WHITE') AS Color,
                cus.CustomerID,
                dest.Code AS Destination,
                dest.Value2 AS Abbrev,
                CASE
	                WHEN LEN(RTRIM(ISNULL(cus.ShortName,''))) > 0 THEN RTRIM(cus.ShortName)
	                WHEN LEN(RTRIM(ISNULL(cus.CustomerName,''))) > 0 THEN RTRIM(cus.CustomerName)
	                ELSE ''
                END AS Customer,
                ISNULL(color.Value1Description,'') AS ColorDesc,
                dest.RecordStatus,
                dest.CreationDate,
                dest.CreatedBy,
                dest.UpdatedDate,
                dest.UpdatedBy
                FROM
                Code dest
                LEFT OUTER JOIN Code color on color.CodeType='CustomsSheetColor' 
                    AND color.Description=dest.CodeID
                LEFT OUTER JOIN Customer cus on cus.CustomerID=color.Value2
                WHERE dest.CodeType='ExportDischargePort' ";

                //Add Destination
                if (txtDestination.Text.Trim().Length > 0)
                    strSQL += "AND dest.Code LIKE '%" + txtDestination.Text.Trim() + "%' ";

                //Add Handheld Abbrev
                if (txtHandheldAbbrev.Text.Trim().Length > 0)
                    strSQL += "AND destValue2 LIKE '" + txtHandheldAbbrev.Text.Trim() + "%' ";

                //Add Sheet Color
                if (txtSheetColor.Text.Trim().Length > 0)
                    strSQL += "AND color.Value1Description LIKE '%" + txtSheetColor.Text.Trim() + 
                        "%' ";

                //Add CustomerID
                if ((cboCust.SelectedItem as ComboboxItem).cboValue != "All")
                    strSQL += "AND cus.CustomerID = '" +
                        (cboCust.SelectedItem as ComboboxItem).cboValue + "' ";

                //Add Status
                if ((cboStatus.SelectedItem as ComboboxItem).cboValue != "All")
                {
                    strval = (cboStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    Globalitems.HandleSingleQuoteForSQL(strval);
                    //strSQL += "AND dest.RecordStatus ='" + strval + 
                    //    "' AND ISNULL(cus.RecordStatus,'Active') ='" + strval + "' ";
                    strSQL += "AND dest.RecordStatus ='" + strval + "'";
                }
               
                strSQL += "ORDER BY Destination,Customer";

                ds = DataOps.GetDataset_with_SQL(strSQL);

                // Use a DataTable as the DataSource for the DataGridView to make sorting by Col Header
                //  clicks, automatic
                dtDestinations = ds.Tables[0].Copy();
                if (dtDestinations.Rows.Count == 0) return;

                //6. If data found:
                //6a. Enable Export button
                btnExport.Enabled = true;

                //6b. Assign Datatable to gridvirew
                dgResults.DataSource = dtDestinations;

                SetgridViewColor();

                //6c. Update # records label
                lblDestinationRecords.Text = "Records: " + dtDestinations.Rows.Count;

                //6d. Because dgResults is not multiselect, 
                //create a list of 1 CustomerID for the Form's binding source, 
                //for use by the nav buttons
                lsDestinationIDs.Clear();
                lsDestinationIDs.Add(dtDestinations.Rows[0]["CodeID_dest"].ToString());

                bs1.DataSource = lsDestinationIDs;

                //6e. Fill detail record with first row AEVoyageID
                txtCodeID_dest.Text = lsDestinationIDs[0] ;
                txtCodeID_color.Text = dtDestinations.Rows[0]["CodeID_color"].ToString();
                FillDetailRecord();

                //6f. Update recbuttons
                recbuttons.blnRecordsToDisplay = true;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                btnColor.Enabled = false;
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSearch", ex.Message);
            }
        }

        private void SQLForModifiedCustColorRecord(ref string strSQL_Color)
        {
            try
            {
                string strCustomerID = (cboCust_record.SelectedItem as ComboboxItem).cboValue;
                string strColor;
                string strColorDesc = txtSheetColor_record.Text.Trim();

                if (pnlColor.BackColor.IsNamedColor)
                    strColor = strColorDesc;
                else
                {
                    strColor = pnlColor.BackColor.R.ToString() + "," +
                        pnlColor.BackColor.G.ToString() + "," +
                        pnlColor.BackColor.B.ToString();
                }

                //Update Code (Dest.ID,Customer ID)
                //  Value1: Color
                //  Value1Description: Color desc
                //  Value2: CustomerID
                strSQL_Color = @"UPDATE CODE SET
                Code = '" + txtCodeID_dest.Text + "_" + strCustomerID + "'," +
                "Value1 = '" + strColor + "'," +
                "Value1Description = '" + strColorDesc + "'," +
                "Value2 = '" + strCustomerID + "' " +
                "WHERE CodeID = " + txtCodeID_color.Text;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForModifiedCustColorRecord",
                    ex.Message);
            }

        }

        private void SQLForModifiedRecord(ref string strSQL_Dest)
        {
            //May need to change the Destination record in the Code table (strSQL_Dest)
            ComboBox cboBox;
            Control[] ctrls;
            string strField;
            string strval;

            try
            {
                // Use linq to get a list of updated controls, 
                //  For this form, textboxes, combobox, checkbox; exclude listboxes
                var changedlist = lsControls.Where(ctrlinfo =>ctrlinfo.Updated == true);

                foreach (ControlInfo ctrlinfo in changedlist)
                {
                    strField = ctrlinfo.RecordFieldName + " = ";

                    //Place the control into the array ctrls, s/b only one
                    ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            strval = ctrls[0].Text.Trim();

                            // Use HandleSingleQuoteForSQL 
                            //to replace ' in text to '' for SQL
                            strval = Globalitems.HandleSingleQuoteForSQL(strval);

                            //Special handling if color changed:
                            //Existing color is removed
                            //Existing color changed
                            //New color added to existing destination
                            {
                                if (strSQL_Dest.Length == 0) strSQL_Dest = "UPDATE CODE SET ";
                                //Need to set RecordFieldName manually
                                switch (ctrlinfo.ControlID)
                                {
                                    case "txtDestination_record":
                                        strSQL_Dest += "Code = '" + strval + "', CodeDescription = '" + strval + "'";
                                        break;
                                    case "txtHandheldAbbrev_record":
                                        strSQL_Dest += "Value2 = '" + strval + "'";
                                        break;
                                }  
                            }
                            
                            break;
                        case "SelectedValue":
                            //Cast control to ComboBox. 
                            //Only change cboRecordStatus 

                            if (ctrlinfo.ControlID.Contains("Status"))
                            {
                                if (strSQL_Dest.Length == 0) strSQL_Dest = "UPDATE CODE SET ";
                                strSQL_Dest += ctrlinfo.RecordFieldName + " = ";

                                cboBox = (ComboBox)ctrls[0];
                                strval = (cboBox.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                                strSQL_Dest += "'" + strval + "'";
                            }
                            break;
                    }

                    if (strSQL_Dest.Length > 0) strSQL_Dest += ",";
                }

                //Add UpdatedDate & UpdatedBy
                if (strSQL_Dest.Length > 0)
                {
                    strSQL_Dest += "UpdatedDate = '" + txtUpdatedDate.Text + "',";
                    strSQL_Dest += "UpdatedBy = '" + txtUpdatedBy.Text + "' ";

                    // Add WHERE clause
                    strSQL_Dest += " WHERE CodeID = " + txtCodeID_dest.Text;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForModifiedRecord", ex.Message);
            }
        }

        private void SQLForNewCustColorRecord(ref string strSQL_Color)
        {
            try
            {
                //Create strSQL_Dest, new rec for Code table with Codetype: CustomsSheetColor
                strSQL_Color = "INSERT INTO Code (Codetype,Code,CodeDescription," +
                   "Value1,Value1Description,Value2,Value2Description," +
                   "RecordStatus,CreationDate,CreatedBy,Description) VALUES (";

                //Codetype
                strSQL_Color += "'CustomsSheetColor',";

                //Code DestinationID (CodeID for Dest.)_CustomerID
                strSQL_Color += "'" + txtCodeID_dest.Text + "_" + 
                    (cboCust_record.SelectedItem as ComboboxItem).cboValue + "',";

                //CodeDescription
                strSQL_Color += "'DestinationID, CustomerID',";

                //Value1 (Color)
                if (pnlColor.BackColor.IsNamedColor)
                    strSQL_Color += "'" + txtSheetColor_record.Text + "',";
                else
                {
                    //Create R,G,B value
                    strSQL_Color += "'" + pnlColor.BackColor.R.ToString() + ",";
                    strSQL_Color += pnlColor.BackColor.G.ToString() + ",";
                    strSQL_Color += pnlColor.BackColor.B.ToString() + "',";
                }

                //Value1Description (Sheet Color)
                strSQL_Color += "'" + txtSheetColor_record.Text.Trim() + "',";

                //Value2 (CustomerID)
                strSQL_Color += "'" + (cboCust_record.SelectedItem as ComboboxItem).cboValue +
                    "',";

                //Value2Description
                strSQL_Color += "'CustomerID',";

                //RecordStatus
                strSQL_Color += "'Active',";

                //CreationDate
                strSQL_Color += "CURRENT_TIMESTAMP,";

                //CreatedBy
                strSQL_Color += "'" + Globalitems.strUserName + "',";

                //Description (DestinationID [CodeID])
                strSQL_Color += "'" + txtCodeID_dest.Text + "');";

            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForNewCustColorRecord", ex.Message);
            }
        }

        private void SQLForNewRecord(ref string strSQL_Dest)
        {
            string strval;

            try
            {
                //Create strSQL_Dest, new rec for Code table with Codetype: ExportDischargePort
                strSQL_Dest = "INSERT INTO Code (Codetype,Code,CodeDescription," +
                   "Value1,Value1Description,Value2,Value2Description," +
                   "RecordStatus,SortOrder,CreationDate,CreatedBy) VALUES (";

                //CodeType
                strSQL_Dest += "'ExportDischargePort',";

                //Code (Destination)
                strval = Globalitems.HandleSingleQuoteForSQL(txtDestination_record.Text.Trim());
                strSQL_Dest += "'" + strval + "',";

                //CodeDescription (Destination)
                strSQL_Dest += "'" + strval + "',";

                //Value1, Value1Description, no longer used, previously SheetColor for Dest.
                strSQL_Dest += "NULL,NULL,";

                //Value2 (Handheld Abbrev), Value2Description
                strSQL_Dest += "'" + txtHandheldAbbrev_record.Text.Trim() + "',";
                strSQL_Dest += "'HandheldAbbreviation',";

                //RecordStatus
                strSQL_Dest += "'" + (cboStatus_record.SelectedItem as ComboboxItem).cboValue +
                    "',";

                //SortOrder, all Destinations use SortOrder of 1
                strSQL_Dest += "1,";

                //CreationDate
                strSQL_Dest += "CURRENT_TIMESTAMP,";

                //CreatedBy
                strSQL_Dest += "'" + Globalitems.strUserName + "');";
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForNewRecord", ex.Message);
            }
        }

        private void SelectColor()
        {
            DialogResult dlResult;
            ColorDialog frmColor = new ColorDialog();

            dlResult = frmColor.ShowDialog();
            if (dlResult == DialogResult.OK)
            {
                lblRGB.Text = "";

                colSelected = frmColor.Color;
                pnlColor.BackColor = colSelected;
                if (colSelected.IsNamedColor)
                {
                    txtSheetColor_record.Text = colSelected.Name;
                }
                else
                {
                    txtSheetColor_record.Text = "<DESCRIBE>";
                    txtSheetColor_record.Focus();
                    txtSheetColor_record.Select(0, txtSheetColor_record.Text.Length);
                    lblRGB.Text = "Red: " + colSelected.R.ToString() + ", " +
                        "Green: " + colSelected.G.ToString() + ", " +
                        "Blue: " + colSelected.B.ToString();
                }
            }
        }

        private bool ValidRecord()
        {
            DataView dv;
            string strMsg;
            string strFilter;
            string strval;

            try
            {
                //Ck Destination Name
                if (txtDestination_record.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter the Destination",
                        "MISSING DESTINATION", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtDestination_record.Focus();
                    return false;
                }

                //Ck Abbrev Name
                if (txtHandheldAbbrev_record.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter the Handheld Abbrev.",
                        "MISSING HANDHELD ABBREV.", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtHandheldAbbrev_record.Focus();
                    return false;
                }

               
                //Ck if Dest already exists
                strval =
                Globalitems.HandleSingleQuoteForSQL(txtDestination_record.Text.Trim());
                strFilter = "CodeType = 'ExportDischargePort'" +
                    " AND Code = '" + strval + "'";
                if (strMode != "NEW") strFilter += " AND CodeID <> " + txtCodeID_dest.Text;

                dv = new DataView(Globalitems.dtCode, strFilter, "CodeID",
                    DataViewRowState.CurrentRows);
                if (dv.Count > 0) 
                {

                    strMsg = "You cannot ";
                    if (strMode == "NEW")
                        strMsg += "add a new record with this destination, because " +
                            "the destination already exists.";
                    else
                        strMsg += "update this destination with this name because " +
                            "the destination already exists.";
                    MessageBox.Show(strMsg,
                       "DESTINATION ALREADY EXISTS", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return false;
                }

                //Ck if Abbrev (Value2) already exists
                strval =
                Globalitems.HandleSingleQuoteForSQL(txtHandheldAbbrev_record.Text.Trim());
                strFilter = "CodeType = 'ExportDischargePort'" +
                    " AND Value2 = '" + strval + "'";
                if (strMode != "NEW") strFilter += " AND CodeID <> " + txtCodeID_dest.Text;
                dv = new DataView(Globalitems.dtCode, strFilter, "CodeID",
                    DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    strMsg = "You cannot ";
                    if (strMode == "NEW")
                        strMsg += "add a new record with this Handheld Abbrev. because " +
                            "the Handheld Abbrev. already exists.";
                    else
                        strMsg += "update this destination with this Handheld Abbrev. because " +
                            "the Handheld Abbrev. already exists.";
                    MessageBox.Show(strMsg,
                        "HANDHELD ABBREV. ALREADY EXISTS", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidRecord", ex.Message);
                return false;
            }
        }

        private bool ValidColorRecord()
        {
            try
            {
                DataSet ds;
                string strMsg;
                string strSQL;

                //Ck for Customer
                if ((cboCust_record.SelectedItem as ComboboxItem).cboValue == "select")
                {
                    MessageBox.Show("Please select the Customer for the Color.",
                       "MISSING CUSTOMER", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return false;
                }

                //Check Sheet Color
                if (txtSheetColor_record.Text.Trim().Contains("DESCRIBE"))
                {
                    MessageBox.Show("Please describe the Sheet Color you selected.",
                        "MISSING SHEET COLOR DESCRIPTION", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtSheetColor_record.Focus();
                    txtSheetColor_record.SelectionStart = 0;
                    txtSheetColor_record.SelectionLength = txtSheetColor_record.Text.Length;
                    return false;
                }

                if (txtSheetColor_record.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please describe the Sheet Color for the Customer.",
                       "MISSING SHEET COLOR DESCRIPTION", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return false;
                }

                if (txtSheetColor_record.Text.Trim().Length == 0 &&
                    pnlColor.BackColor == System.Drawing.Color.White)
                {
                    MessageBox.Show("Please select the Sheet Color for the Customer.",
                       "MISSING SHEET COLOR", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return false;
                }

                //ck if different Customer, Color, Color Desc
                if (strCustColorMode == "MODIFY" && 
                    (cboCust_record.SelectedItem as ComboboxItem).cboValue == strCurrentCustomerID &&
                    txtSheetColor_record.Text == strCurrentColorDesc &&
                    pnlColor.BackColor == colCurrent)
                {
                    MessageBox.Show("You have not changed the Customer or Color.",
                        "NO CHANGE IN CUSTOMER OR COLOR", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }

                //Ck if Dest/Cust already exists in the code table
                strSQL = @"SELECT * FROM Code 
                WHERE CodeType = 'CustomsSheetColor' 
                AND code = '" + txtCodeID_dest.Text +
                @"_" + (cboCust_record.SelectedItem as ComboboxItem).cboValue + "' ";

                if (strCustColorMode == "MODIFY")
                    strSQL += "AND CodeID <> " + txtCodeID_color.Text;

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strMsg = "You cannot ";
                    if (strCustColorMode == "NEW")
                        strMsg += "add a new record with this Customer/Color because " +
                            "a Customer/Color record already exists for this destination.";
                    else
                        strMsg += "update this destination with this Customer/Color because " +
                            "a Customer/Color record already exists for this destination.";
                    MessageBox.Show(strMsg,
                        "CUSTOMER/COLOR RECORD ALREADY EXISTS", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }

                return true;

            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidColorRecord", ex.Message);
                return false;
            }
        }

        private void btnCancel_Clicked()
        {CancelSetup();}

        private void btnPrev_Clicked()
        {PerformMovePrevious();}

        private void btnNext_Clicked()
        {PerformMoveNext();}

        private void btnDelete_Clicked()
        {PerformDeleteRecord();}

        private void btnNew_Clicked()
        {NewRecordSetup();}

        private void btnModify_Clicked()
        { ModifyRecordSetup(); }

        private void btnSave_Clicked()
        {PerformSaveRecord();}

        private void btnSearch_Click(object sender, EventArgs e)
        {PerformSearch();}

        private void dgResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            //As long as row clicked is not the Column Header row, index = -1, 
            //change the binding source
            if (e.RowIndex > -1) FilterBindingSource();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {OpenCSVFile();}

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }


        private void txtDestination_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtDestination_record", lsControls);
        }

        private void txtHandheldAbbrev_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtHandheldAbbrev_record", lsControls);
        }

        private void cboStatus_record_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboStatus_record", lsControls);
        }

        private void dgResults_Sorted(object sender, EventArgs e)
        {
            SetgridViewColor();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {SelectColor();}

        private void cboCust_record_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "READ") return;

            if ((cboCust_record.SelectedItem as ComboboxItem).cboValue == "select")
            {
                txtSheetColor_record.Text = "";
                txtSheetColor_record.Enabled = false;
                pnlColor.BackColor = System.Drawing.Color.White;
                btnColor.Enabled = false;
            }
            else
            {
                txtSheetColor_record.Enabled = true;
                btnColor.Enabled = true;
                if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboCust_record", lsControls);
            }
        }

        private void frmDestinationAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((strMode != "READ" || strCustColorMode != "READ") && !Globalitems.blnException)
            {
                MessageBox.Show("You must SAVE or Cancel the current changes to close this form",
                   "CANNOT CLOSE THIS FORM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {if (!blnInitialFill) FillCombos();}

        private void btnSaveCustColor_Click(object sender, EventArgs e)
        {PerformSaveCustColorRecord();}

        private void btnNewCustColor_Click(object sender, EventArgs e)
        {NewCustomerColorRecordSetup();}

        private void btnModCustColor_Click(object sender, EventArgs e)
        {ModifyCustColorRecordSetup();}

        private void btnCancelCustColor_Click(object sender, EventArgs e)
        {CancelCustomerColor();}

        private void btnDelCustColor_Click(object sender, EventArgs e)
        {PerformDeleteCustColorRecord();}

        private void frmDestinationAdmin_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
