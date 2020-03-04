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
    public partial class frmVoyageAdmin : Form
    {
        //Public Variables
        public bool blnNewVoyageRQFromOtherForm = false;
        public bool blnLoadSeqOnlyClearedVehs = true;
        public string strSortOrder = "HL";
        public List<string> lsSortExceptions = new List<string>();

        private const string CURRENTMODULE = "frmVoyageAdmin";

        private BindingSource bs1 = new BindingSource();
        private bool blnUpdateVoyageCustomer;
        private bool blnUpdateVoyageDestination;
        private bool blnUpdateLoadSeq;
        private DataTable dtVoyages = new DataTable();
        private List<string> lsVoyageIDs = new List<string>();
        private string strMode;

        //Set up List of ControlInfo objects, lsControlInfo, as follows:
        //  Order in list establishes Indexes for tabbing, implemented by SetTabIndex() method
        //  AlwaysReadOnly identifies if control cannot be modified by User
        //  ControlPropertyToBind identifies what controls are initialized
        //  RecordFieldName identify what controls display record detail
        //  HeaderText sets column header to use for Export to csv file
        //  Updated property establishes what controls User has modified

        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="txtVoyageNumber",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtFrom",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtTo",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtVesselName",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="ckClosed", ControlPropetyToBind="Checked"},
            new ControlInfo {ControlID="txtVoyageNumber_record", RecordFieldName="VoyageNumber",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtVoyageDate_record", RecordFieldName="VoyageDate",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboVessel_record", RecordFieldName="AEVesselID",
                ControlPropetyToBind="SelectedValue"},
            //new ControlInfo {ControlID="ckActive",ControlPropetyToBind ="Checked"},
            new ControlInfo {ControlID="ckClosed_record", RecordFieldName="VoyageClosedInd",
                ControlPropetyToBind ="Checked"},
            new ControlInfo {ControlID="lbAvailCustomers", RecordFieldName="custom",
                ControlPropetyToBind ="Listbox"},
            new ControlInfo {ControlID="lbVoyageCustomers", RecordFieldName="custom",
                ControlPropetyToBind ="Listbox"},
             new ControlInfo {ControlID="lbAvailDestinations", RecordFieldName="custom",
                ControlPropetyToBind ="Listbox"},
            new ControlInfo {ControlID="lbVoyageDestinations", RecordFieldName="custom",
                ControlPropetyToBind ="Listbox"},
            new ControlInfo {ControlID="txtCreationDate", RecordFieldName="CreationDate",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCreatedBy", RecordFieldName="CreatedBy",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtUpdatedDate", RecordFieldName="UpdatedDate",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtUpdatedBy", RecordFieldName="UpdatedBy",
                ControlPropetyToBind="Text"},
             // objects needed for csv file  HeaderText="Cust. Name"
            new ControlInfo {RecordFieldName="VoyageNumber",HeaderText="Voyage #"},
            new ControlInfo {RecordFieldName="VoyageDate",HeaderText="Voyage Date"},
            new ControlInfo {RecordFieldName="Vessel",HeaderText="Vessel"}
        };

        private List<ComboboxItem> lsCustomers = new List<ComboboxItem>();
        private List<string> lsDestinations = new List<string>();

        public frmVoyageAdmin()
        {
            InitializeComponent();
            dgResults.AutoGenerateColumns = false;
            dgAvailLoadSeq.AutoGenerateColumns = false;
            dgVoyageLoadSeq.AutoGenerateColumns = false;
            btnExport.Enabled = false;
            lblVoyageRecords.Text = "";

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

            Formops.SetTabIndex(this, lsControls);

            DisplayMode();
        }

        private void ActiveConditionChange()
        {
            try
            {
                FillCombos();

                if (strMode != "READ") cboVessel_record.SelectedIndex = 0;
                if (bs1.Count > 0) FillDetailRecord(txtVoyageID.Text);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ActiveConditionChange", ex.Message);
            }
        }

        private void AdjustReadOnlyStatus(bool blnReadOnly)
        {
            Formops.SetReadOnlyStatus(this, lsControls, blnReadOnly, recbuttons);

            //ckActive.Enabled = !blnReadOnly;

            //Form unique controls not handled by SetReadOnlyStatus
            dgAvailLoadSeq.Enabled = !blnReadOnly;
            dgVoyageLoadSeq.Enabled = !blnReadOnly;

            //Always set Move buttons to false
            //If in Modify/New mode, enable when User selects items in ListBoxes or DataGridViews
            btnMoveToAvailCust.Enabled = false;
            btnMoveToVoyageCust.Enabled = false;
            btnMoveToAvailDest.Enabled = false;
            btnMoveToVoyageDest.Enabled = false;
            btnMoveToAvailLoadSeq.Enabled = false;
            btnMoveToVoyageLoadSeq.Enabled = false;
        }

        private void UpdateMoveButtons(bool blnUpDownAction = false)
        {
            int intLastSeq = 0;
            List<DataGridViewRow> lsSelectedRows;
            try
            {
                //Ck lbAvailCustomers
                if (lbAvailCustomers.Items.Count == 0 || lbAvailCustomers.SelectedItems.Count == 0)
                {
                    btnMoveToVoyageCust.Enabled = false;
                    btnMoveToVoyageCust.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.NextMove_grey);
                }
                else
                {
                    btnMoveToVoyageCust.Enabled = true;
                    btnMoveToVoyageCust.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.NextMove);
                }

                //Ck lbVoyageCustomers
                if (lbVoyageCustomers.Items.Count == 0 || lbVoyageCustomers.SelectedItems.Count == 0)
                {
                    btnMoveToAvailCust.Enabled = false;
                    btnMoveToAvailCust.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.PrevMove_grey);
                }
                else
                {
                    btnMoveToAvailCust.Enabled = true;
                    btnMoveToAvailCust.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.PrevMove);
                }

                //Ck lbAvailDestinations
                if (lbAvailDestinations.Items.Count == 0 || lbAvailDestinations.SelectedItems.Count == 0)
                {
                    btnMoveToVoyageDest.Enabled = false;
                    btnMoveToVoyageDest.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.NextMove_grey);
                }
                else
                {
                    btnMoveToVoyageDest.Enabled = true;
                    btnMoveToVoyageDest.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.NextMove);
                }

                //Ck lbVoyageDestinations
                if (lbVoyageDestinations.Items.Count == 0 || lbVoyageDestinations.SelectedItems.Count == 0)
                {
                    btnMoveToAvailDest.Enabled = false;
                    btnMoveToAvailDest.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.PrevMove_grey);
                }
                else
                {
                    btnMoveToAvailDest.Enabled = true;
                    btnMoveToAvailDest.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.PrevMove);
                }

                //Ck dgAvailLoadSeq
                //Disable Avail->Voyage/Voyage->Avail/Up/Down/Merge buttons
                btnMoveToVoyageLoadSeq.Enabled = false;
                btnMoveToVoyageLoadSeq.BackgroundImage =
                    new Bitmap(AutoExport.Properties.Resources.NextMove_grey);
                btnMoveToAvailLoadSeq.Enabled = false;
                btnMoveToAvailLoadSeq.BackgroundImage =
                    new Bitmap(AutoExport.Properties.Resources.PrevMove_grey);
                btnUp.Enabled = false;
                btnUp.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.UpMove_grey);
                btnDown.Enabled = false;
                btnDown.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.DownMove_grey);
                btnMerge.Enabled = false;

                btnMoveInfo.Visible = false;

                //Enable Avail->Voyage if rows selected
                if (dgAvailLoadSeq.SelectedRows.Count > 0)
                {
                    btnMoveToVoyageLoadSeq.Enabled = true;
                    btnMoveToVoyageLoadSeq.BackgroundImage =
                        new Bitmap(AutoExport.Properties.Resources.NextMove);
                }

                //Don't need to enable Up/Down/Merge buttons, if no Voyage rows selected
                if (dgVoyageLoadSeq.SelectedRows.Count == 0) return;

                //One or more voyage rows selected
                btnMoveToAvailLoadSeq.Enabled = true;
                btnMoveToAvailLoadSeq.BackgroundImage =
                    new Bitmap(AutoExport.Properties.Resources.PrevMove);

                //Get intLastSeq in dgVoyageLoadSeq
                intLastSeq =
                    Convert.ToInt16(dgVoyageLoadSeq.Rows[dgVoyageLoadSeq.Rows.Count - 1].
                    Cells["Sequence_voyage"].Value.ToString());

                //If one row selected Set Up/Down buttons
                if (dgVoyageLoadSeq.SelectedRows.Count == 1)
                {
                    //Enable both Up/Down buttons
                    btnUp.Enabled = true;
                    btnUp.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.UpMove);

                    btnDown.Enabled = true;
                    btnDown.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.DownMove);

                    //If Seq #1 selected disable Up button
                    if (Convert.ToInt16(dgVoyageLoadSeq.SelectedRows[0].Cells["Sequence_voyage"].
                        Value.ToString()) == 1)
                    {
                        btnUp.Enabled = false;
                        btnUp.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.UpMove_grey);
                    }

                    //If last Seq# selected disable Down button
                    if (Convert.ToInt16(dgVoyageLoadSeq.SelectedRows[0].Cells["Sequence_voyage"].
                        Value.ToString()) == intLastSeq)
                    {
                        btnDown.Enabled = false;
                        btnDown.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.DownMove_grey);
                    }
                    return;
                }

                //Multiple rows are selected

                //Note DataGridView SelectedRows put 1st selected index at bottom of list,
                //  last selected index at top of list
                //  Enable Merge button, don't enable Up/Down buttons if selected rows have different Seq #'s
                //  Enable Up/Down buttons, don't enable Merge button if selected rows are all the same Seq#

                //Create a list of Selected DataGridViewRows
                lsSelectedRows = new List<DataGridViewRow>();
                foreach (DataGridViewRow dgrow in dgVoyageLoadSeq.SelectedRows)
                    lsSelectedRows.Add(dgrow);

                //Use Linq to determine how many distinct Seq's in lsSelectedRows
                var colDistinct = lsSelectedRows.Select(seq =>
                    seq.Cells["Sequence_voyage"]).Distinct();

                //Display btnMoveInfo
                btnMoveInfo.Visible = true;

                if (colDistinct.Count() == 1 || blnUpDownAction)
                //All selected rows are the same Seq, enable Up/Down button
                {
                    //Enable both Up/Down buttons
                    btnUp.Enabled = true;
                    btnUp.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.UpMove);

                    btnDown.Enabled = true;
                    btnDown.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.DownMove);

                    //Disable Up button if Seq#1
                    if (Convert.ToInt16(dgVoyageLoadSeq.SelectedRows[0].Cells["Sequence_voyage"].
                        Value.ToString()) == 1)
                    {
                        btnUp.Enabled = false;
                        btnUp.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.UpMove_grey);
                        return;
                    }

                    //Disable Down button if Seq# is last
                    if (Convert.ToInt16(dgVoyageLoadSeq.SelectedRows[0].Cells["Sequence_voyage"].
                        Value.ToString()) == intLastSeq)
                    {
                        btnDown.Enabled = false;
                        btnDown.BackgroundImage = new Bitmap(AutoExport.Properties.Resources.DownMove_grey);
                        return;
                    }
                }
                else
                //Selected rows have different Seq #'s, enable Merge button
                {
                    btnMerge.Enabled = true;
                }

                if (dgVoyageLoadSeq.RowCount == 0) btnExport.Enabled = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "UpdateMoveButtons", ex.Message);
            }
        }

        private void CancelSetup()
        {
            int intCurrentBSPosition = -1;
            blnUpdateLoadSeq = false;

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
                FillDetailRecord(lsVoyageIDs[intCurrentBSPosition]);
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

                dtVoyages.Clear();

                // Binding dgResults to lsUsers after the Clear method, can lead to runtime error because
                //  the CurrencyManager pointing to the Current position in lsUsers, doesn't reset to -1
                dgResults.DataSource = dtVoyages;
                dgAvailLoadSeq.DataSource = null;
                dgVoyageLoadSeq.DataSource = null;

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
            ComboBox cbo = new ComboBox();
            ComboboxItem cboitem;
            DataSet ds;
            string strFilter;
            string strSQL;

            try
            {
                //cboVessel
                cboVessel_record.Items.Clear();

                strSQL = "SELECT AEVesselID, " +
                    "CASE WHEN LEN(RTRIM(ISNULL(VesselShortName,''))) > 0 THEN RTRIM(VesselShortName) " +
                    "ELSE RTRIM(VesselName) END AS VesselName " +
                    "FROM AEVessel " +
                    "WHERE LEN(RTRIM(ISNULL(VesselName,''))) > 0 ";
                if (ckActive.Checked) strSQL += "AND RecordStatus='Active' ";
                strSQL += "ORDER BY VesselName";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillCombos",
                        "No rows returned from Vessel table");
                    return;
                }

                // Add Select to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboVessel_record.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dr["VesselName"].ToString();
                    cboitem.cboValue = dr["AEVesselID"].ToString();
                    cboVessel_record.Items.Add(cboitem);
                }

                cboVessel_record.DisplayMember = "cboText";
                cboVessel_record.ValueMember = "cboValue";
                cboVessel_record.SelectedIndex = -1;

                //Store the Customers in lsCustomers
                lsCustomers.Clear();

                strSQL = "SELECT CustomerID, " +
                    "CASE WHEN LEN(RTRIM(ISNULL(ShortName,''))) > 0 THEN RTRIM(ShortName) " +
                    "else RTRIM(CustomerName) END AS CustName " +
                    "FROM Customer ";
                if (ckActive.Checked) strSQL += "WHERE RecordStatus='Active' ";
                strSQL += "ORDER BY CustName";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillCombos",
                        "No rows returned from Customer table");
                    return;
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dr["CustName"].ToString().Trim();
                    cboitem.cboValue = dr["CustomerID"].ToString().Trim();
                    lsCustomers.Add(cboitem);
                }

                //Store the Destinations in lsDestinations
                //Use FillComboboxFromCodeTable method to put items into
                //  a Combobox
                strFilter = "CodeType='ExportDischargePort' AND Code <> '' ";
                if (ckActive.Checked) strFilter += "AND RecordStatus='Active'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cbo, false, false);

                //Load the values from the cbo into lsDestinations
                lsDestinations.Clear();
                foreach (ComboboxItem cbDest in cbo.Items)
                    lsDestinations.Add(cbDest.cboValue);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void FillDetailRecord(string strAEVoyageID)
        {
            DataRow drow;
            DataSet ds;
            DataTable dtDetail;
            string strSQL;

            try
            {
                blnUpdateVoyageCustomer = false;
                blnUpdateVoyageDestination = false;
                blnUpdateLoadSeq = false;

                Formops.ClearRecordData(this, lsControls);

                dgAvailLoadSeq.DataSource = null;
                dgVoyageLoadSeq.DataSource = null;

                //Use List<ComboboxItem> as Datasource for two Customer Listboxes
                //  (need to reset after clearing record data)
                lbAvailCustomers.DisplayMember = "cboText";
                lbAvailCustomers.ValueMember = "cboValue";
                lbVoyageCustomers.DisplayMember = "cboText";
                lbVoyageCustomers.ValueMember = "cboValue";

                //Qry returns a row for each CustomerID/DestinationName combination
                //  E.g., voyage w/two customers & 4 destinations returns 8 rows
                strSQL = "SELECT " +
                    "voycust.CustomerID," +
                    "voy.AEVesselID,voy.VoyageNumber,voy.VoyageDate," +
                    "voy.VoyageClosedInd,dest.DestinationName," +
                    "voy.CreatedBy,voy.CreationDate," +
                    "voy.UpdatedBy,voy.UpdatedDate, " +
                    "cus.RecordStatus " +
                    "FROM " +
                    "AEVoyage voy " +
                    "LEFT OUTER JOIN AEVoyageCustomer voycust on " +
                        "voy.AEVoyageID = voycust.AEVoyageID " +
                    "LEFT OUTER JOIN Customer cus on cus.CustomerID = voycust.CustomerID " +
                    "LEFT OUTER JOIN AEVoyageDestination dest on " +
                        "dest.AEVoyageID = voy.AEVoyageID " +
                    "WHERE voy.AEVoyageID = " + strAEVoyageID;
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillDetailRecord",
                        "No data returned from query");
                    return;
                }

                dtDetail = ds.Tables[0];

                //Set Voyage#, VoyageDate,Created info, & Updated info from 1st Row
                //  (same info on each row)
                drow = dtDetail.Rows[0];

                Formops.SetDetailRecord(drow, this, lsControls);

                //Format Date textboxes
                txtVoyageDate_record.Text = Globalitems.FormatDatetime(txtVoyageDate_record.Text);
                txtCreationDate.Text = Globalitems.FormatDatetime(txtCreationDate.Text);
                txtUpdatedDate.Text = Globalitems.FormatDatetime(txtUpdatedDate.Text);

                SetupCustomers(dtDetail);
                lbAvailCustomers.SelectedIndex = -1;
                lbVoyageCustomers.SelectedIndex = -1;

                SetupDestinations(dtDetail);
                lbAvailDestinations.SelectedIndex = -1;
                lbVoyageDestinations.SelectedIndex = -1;

                SetupLoadSeq();

                UpdateMoveButtons();

                btnExportLoadSeq.Enabled = false;
                if (dgVoyageLoadSeq.RowCount > 0) btnExportLoadSeq.Enabled = true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillDetailRecord", ex.Message);
            }
        }

        private void SetupLoadSeq()
        {
            BindingSource bsAvailable = new BindingSource();
            DataSet ds;
            DataTable dt;
            DataView dv;
            LoadSeqItem objLoadSeq;
            LoadSeqItem objLoadSeqCopy;
            List<LoadSeqItem> lsAvailLoadSeq = new List<LoadSeqItem>();
            List<LoadSeqItem> lsVoyageLoadSeq = new List<LoadSeqItem>();

            string strFilter;
            string strSQL;
            string strval;

            try
            {
                //Get the available DISTINCT CustomerID/DestinationName/SizeClass from veh table
                strSQL = "SELECT DISTINCT veh.CustomerID," +
                    "ISNULL(veh.DestinationName,'') AS DestinationName," +
                    "ISNULL(veh.SizeClass,'') AS SizeClass " +
                    "FROM AutoportExportVehicles veh " +
                    "LEFT OUTER JOIN Customer cus ON cus.CustomerID=veh.CustomerID " +
                    "WHERE VoyageID = " + txtVoyageID.Text;
                if (ckActive.Checked) strSQL += " AND veh.RecordStatus='Active' AND " +
                        "cus.RecordStatus='Active' ";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "SetupLoadSeq",
                        "No data returned from 1st query");
                    return;
                }

                dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("There are no vehicles assigned to this voyage.",
                        "NO VEHICLES FOR THIS VOYAGE", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                //Ck if any veh's are missing SizeClass
                dt = ds.Tables[0];
                strFilter = "SizeClass = ''";
                dv = new DataView(dt, strFilter, "SizeClass", DataViewRowState.CurrentRows);

                if (dv.Count > 0)
                {
                    if (dv.Count == 1)
                        strval = "There is 1 vehicle ";
                    else
                        strval = "There are " + dv.Count.ToString() + " vehicles ";
                    MessageBox.Show(strval + "assigned to this voyage " +
                        "with no SizeClass.\n\n All vehicles should have an associated SizeClass.",
                        "VEHICLES WITH NO SIZECLASS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //Load lsAvailLoadSeq with info in dt
                foreach (DataRow drow in dt.Rows)
                {
                    objLoadSeq = new LoadSeqItem();
                    objLoadSeq.CustomerID_string = drow["CustomerID"].ToString();

                    //Get CustomerName from lsCustomers using LINQ
                    var item = lsCustomers.Find(it => it.cboValue == objLoadSeq.CustomerID_string);
                    objLoadSeq.CustomerName = item.cboText;

                    objLoadSeq.DestinationName = drow["DestinationName"].ToString();
                    objLoadSeq.SizeClass = drow["SizeClass"].ToString();

                    lsAvailLoadSeq.Add(objLoadSeq);
                }

                //Retrieve LoadSeq recs from the AEVoyageLoadSequence table
                strSQL = "select Sequence,CustomerID,DestinationName,SizeClass " +
                   "from AEVoyageLoadSequence WHERE VoyageID = " + txtVoyageID.Text;
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "SetupLoadSeq",
                        "No data returned from 2nd query");
                    return;
                }

                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    //Move Seqitems returned in qry from lsAvailLoadSeq to lsVoyageLoadSeq
                    foreach (DataRow drow in dt.Rows)
                    {
                        var item = lsAvailLoadSeq.Find(it =>
                            (it.CustomerID_string == drow["CustomerID"].ToString()) &&
                            (it.DestinationName == drow["DestinationName"].ToString()) &&
                            (it.SizeClass == drow["SizeClass"].ToString())
                            );

                        if (item != null)
                        {
                            objLoadSeqCopy = item.MakeCopy(item);
                            //Add the Sequence from the AEVoyageLoadSequence table
                            objLoadSeqCopy.Sequence_string = drow["Sequence"].ToString();

                            lsAvailLoadSeq.Remove(item);
                            lsVoyageLoadSeq.Add(objLoadSeqCopy);
                        }
                    }
                }

                //Sort Avail by CustomerName, Destination, SizeClass & assign to dgAvailLoadSeq
                lsAvailLoadSeq = lsAvailLoadSeq.OrderBy(it0 =>
                    it0.CustomerName).ThenBy(it1 =>
                    it1.DestinationName).ThenBy(it2 =>
                    it2.SizeClass).ToList();
                dgAvailLoadSeq.DataSource = lsAvailLoadSeq;

                //Sort Voyage by Seq,SizeClass,Destination & assign to dgVoyageLoadSeq
                lsVoyageLoadSeq = lsVoyageLoadSeq.OrderBy(it0 =>
                   it0.Sequence_int).ThenBy(it1 =>
                   it1.SizeClass).ThenBy(it2 =>
                   it2.DestinationName).ToList();
                dgVoyageLoadSeq.DataSource = lsVoyageLoadSeq;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SeupLoadSeq", ex.Message);
            }
        }

        private void SetupCustomers(DataTable dtDetail)
        {
            ComboboxItem cboCopy;
            DataTable dtDistinct;
            DataView dv;
            List<ComboboxItem> lsCustAvail;
            List<ComboboxItem> lsCustVoyage;
            string strCustomerID;
            string strFilter;

            try
            {
                //Get the Distinct CustomerIDs and setup Customer Available/Voyage Listboxes
                strFilter = "CustomerID IS NOT NULL";
                if (ckActive.Checked) strFilter += " AND RecordStatus='Active'";

                dv = new DataView(dtDetail, strFilter, "CustomerID", DataViewRowState.CurrentRows);

                if (dv.Count == 0)
                {
                    lbAvailCustomers.DataSource = lsCustomers;
                }
                else
                {
                    //Place all customers into lsCustAvail
                    lsCustAvail = lsCustomers.ToList();

                    //Put the DISTINCT CustomerIDs in the dv into dtDistinct
                    dtDistinct = dv.ToTable(true, "CustomerID");

                    lsCustVoyage = new List<ComboboxItem>();

                    //Add to lsCustVoyage & remove from lsCustAvail
                    foreach (DataRow drowCustID in dtDistinct.Rows)
                    {
                        strCustomerID = drowCustID["CustomerID"].ToString();
                        var item = lsCustAvail.Find(it => it.cboValue == strCustomerID);
                        cboCopy = item.MakeCopy(item);
                        lsCustAvail.Remove(item);
                        lsCustVoyage.Add(cboCopy);
                    }

                    //Sort lsCustAvail & lsCustVoyage by cboText value (Customer Name)
                    //  & assign to the Listboxes
                    List<ComboboxItem> CustAvailSorted = lsCustAvail.OrderBy(it => it.cboText).ToList();
                    lbAvailCustomers.DataSource = CustAvailSorted;

                    List<ComboboxItem> CustVoyageSorted = lsCustVoyage.OrderBy(it => it.cboText).ToList();
                    lbVoyageCustomers.DataSource = CustVoyageSorted;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetupCustomers", ex.Message);
            }

        }

        private void SetupDestinations(DataTable dtDetail)
        {
            DataTable dtDistinct;
            DataView dv;
            List<string> lsDestAvail;
            List<string> lsDestVoyage;
            string strDestination;
            string strFilter;

            try
            {
                //Ck if any veh's are missing DestinationName
                strFilter = "DestinationName = ''";
                dv = new DataView(dtDetail, strFilter, "DestinationName", DataViewRowState.CurrentRows);
                //if (dv.Count == 0)
                //{
                //    blnMissingVehInfo = true;
                //    MessageBox.Show("There ARE vehicles assigned to this voyage " +
                //        "with no destination.\n\n All vehicles must have an associated destination.",
                //        "VEHICLES WITH NO DESTINATION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                //Get the Distinct DestinationNames and setup
                //Destination Available/Voyage Listboxes
                strFilter = "DestinationName IS NOT NULL";
                dv = new DataView(dtDetail, strFilter, "DestinationName", DataViewRowState.CurrentRows);

                if (dv.Count == 0)
                {
                    lbAvailDestinations.DataSource = lsDestinations;
                }
                else
                {
                    //Place all DestinationNames into lsDestAvail
                    lsDestAvail = lsDestinations.ToList();

                    //Put the DISTINCT DestinationNames in the dv into dtDistinct
                    dtDistinct = dv.ToTable(true, "DestinationName");

                    lsDestVoyage = new List<string>();

                    //Add to lsDestVoyage & remove from lsDestAvail
                    foreach (DataRow drowDest in dtDistinct.Rows)
                    {
                        strDestination = drowDest["DestinationName"].ToString();
                        lsDestAvail.Remove(strDestination);
                        lsDestVoyage.Add(strDestination);
                    }

                    //Sort lsDestAvail & lsDestVoyage
                    //  & assign to the Listboxes
                    lsDestAvail.Sort();
                    lsDestVoyage.Sort();
                    lbAvailDestinations.DataSource = lsDestAvail;
                    lbVoyageDestinations.DataSource = lsDestVoyage;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetupDestinations", ex.Message);

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
                lsVoyageIDs.Clear();
                lsVoyageIDs.Add(dgResults.SelectedRows[0].Cells[0].Value.ToString());

                bs1.DataSource = lsVoyageIDs;

                //6e. Fill detail record with first row AEVoyageID
                txtVoyageID.Text = lsVoyageIDs[0];
                FillDetailRecord(txtVoyageID.Text);

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
                txtVoyageNumber_record.Focus();

                //10. Handle Form unique controls

                //Enable Move buttons based on contents of Listboxes & DataGridViews
                if (lbAvailCustomers.Items.Count == 0)
                {
                    btnMoveToVoyageCust.Enabled = false;
                    btnMoveToVoyageCust.BackgroundImage =
                        new Bitmap(AutoExport.Properties.Resources.NextMove_grey);
                }

                if (lbVoyageCustomers.Items.Count == 0)
                {
                    btnMoveToAvailCust.Enabled = false;
                    btnMoveToAvailCust.BackgroundImage =
                        new Bitmap(AutoExport.Properties.Resources.PrevMove_grey);
                }

                if (lbAvailDestinations.Items.Count == 0)
                {
                    btnMoveToVoyageDest.Enabled = false;
                    btnMoveToVoyageDest.BackgroundImage =
                        new Bitmap(AutoExport.Properties.Resources.NextMove_grey);
                }

                if (lbVoyageDestinations.Items.Count == 0)
                {
                    btnMoveToAvailDest.Enabled = false;
                    btnMoveToAvailDest.BackgroundImage =
                        new Bitmap(AutoExport.Properties.Resources.PrevMove_grey);
                }

                if (dgAvailLoadSeq.Rows.Count == 0)
                {
                    btnMoveToVoyageLoadSeq.Enabled = false;
                    btnMoveToVoyageLoadSeq.BackgroundImage =
                        new Bitmap(AutoExport.Properties.Resources.NextMove_grey);
                }

                if (dgVoyageLoadSeq.Rows.Count == 0)
                {
                    btnMoveToAvailLoadSeq.Enabled = false;
                    btnMoveToAvailLoadSeq.BackgroundImage =
                        new Bitmap(AutoExport.Properties.Resources.PrevMove_grey);
                }
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
                Formops.ClearRecordData(this, lsControls);

                //7. Set recbuttons to New
                recbuttons.SetButtons(RecordButtons.ACTION_NEWRECORD);

                //8. Set Created By/Date
                txtCreatedBy.Text = Globalitems.strUserName;
                txtCreationDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

                //9. Set focus on first contol
                txtVoyageNumber.Focus();

                // Refill available listboxes
                lbAvailCustomers.DisplayMember = "cboText";
                lbAvailCustomers.ValueMember = "cboValue";
                lbVoyageCustomers.DisplayMember = "cboText";
                lbVoyageCustomers.ValueMember = "cboValue";
                lbAvailCustomers.DataSource = lsCustomers;
                lbAvailDestinations.DataSource = lsDestinations;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "NewRecordSetup", ex.Message);
            }
        }

        private void GridViewMove(DataGridView dgFrom, DataGridView dgTo)
        {
            bool blnDone = false;
            bool blnMovingToDGVoyage = true;
            int intHighestSeq = 0;
            int intItem;
            int intSeqBeingChecked;
            int intCurrentSeqShouldBe = 1;
            int intSeqToModify = 0;
            List<LoadSeqItem> lsItemsFrom;
            List<LoadSeqItem> lsItemsTo = new List<LoadSeqItem>();
            List<LoadSeqItem> lsItemsToRemove = new List<LoadSeqItem>();
            LoadSeqItem objLoadSeq;
            LoadSeqItem objCopy;

            try
            {
                //Since each dgCol needs a unique ID, need to ID col. name when accessing
                //  selected rows in dgFrom. Assume dgFrom is dgAvailLoadSeq, dgTo is dgVoyageLoadSeq
                string strDGColumntag_from = "_avail";
                //lsItemWithSeq = lsItemsTo;

                if (dgFrom.Name.Contains("Voy"))
                {
                    strDGColumntag_from = "_voyage";
                    blnMovingToDGVoyage = false;
                    //lsItemWithSeq = lsItemsFrom;
                }

                //Store the initial datasources of the two DataGridViews
                //To perform a move, the from dg must have a datasource with at least one row
                lsItemsFrom = (List<LoadSeqItem>)dgFrom.DataSource;

                if (dgTo.RowCount > 0)
                {
                    lsItemsTo = (List<LoadSeqItem>)dgTo.DataSource;

                    //If moving items to dgVoyage, get intHighestSeq in dgVoyage
                    if (blnMovingToDGVoyage)
                    {
                        intHighestSeq = Convert.ToInt16(dgVoyageLoadSeq.Rows[dgVoyageLoadSeq.RowCount - 1].
                            Cells["Sequence_voyage"].Value.ToString());
                    }
                }

                //Copy the corresponding LoadSeqItem for each SelectedRow to lsItemsTo
                foreach (DataGridViewRow dgRow in dgFrom.SelectedRows)
                {
                    objLoadSeq = new LoadSeqItem();
                    objLoadSeq.CustomerID_string =
                        dgRow.Cells["CustomerID" + strDGColumntag_from].Value.ToString();
                    objLoadSeq.CustomerName = dgRow.Cells["CustomerName" + strDGColumntag_from].Value.ToString();
                    objLoadSeq.DestinationName = dgRow.Cells["DestinationName" + strDGColumntag_from].Value.ToString();
                    objLoadSeq.SizeClass = dgRow.Cells["SizeClass" + strDGColumntag_from].Value.ToString();

                    //If moving items from dgAvail to dgVoy, assign each item the next Seq#
                    if (blnMovingToDGVoyage)
                    {
                        intHighestSeq += 1;
                        objLoadSeq.Sequence_int = intHighestSeq;
                    }

                    lsItemsTo.Add(objLoadSeq);
                    objCopy = objLoadSeq.MakeCopy(objLoadSeq);
                    lsItemsToRemove.Add(objCopy);
                }

                //Remove items in lsItemsToRemove from lsItemsFrom
                foreach (LoadSeqItem ldSeqItem in lsItemsToRemove)
                    lsItemsFrom.RemoveAll(item => item.CustomerID_string ==
                        ldSeqItem.CustomerID_string &&
                        item.DestinationName == ldSeqItem.DestinationName &&
                        item.SizeClass == ldSeqItem.SizeClass);

                //Update Seq#'s in dgVoyage if not empty, and moved rows to Avail
                if (!blnMovingToDGVoyage && lsItemsFrom.Count > 0)
                {
                    intItem = 0;
                    intSeqToModify = lsItemsFrom[0].Sequence_int;

                    while (!blnDone)
                    {
                        objLoadSeq = lsItemsFrom[intItem];

                        //Current item is part of remaining Seq's in dgVoyageLoadSeq, after the move
                        intSeqBeingChecked = objLoadSeq.Sequence_int;

                        if (intSeqBeingChecked != intCurrentSeqShouldBe)
                        {
                            if (intSeqBeingChecked == intSeqToModify)
                                objLoadSeq.Sequence_int = intCurrentSeqShouldBe;
                            else
                            {
                                //Don't increment CurrentSeqShouldBe if 1st entry (Seq > 1)
                                if (intItem != 0) intCurrentSeqShouldBe += 1;

                                intSeqToModify = intSeqBeingChecked;
                                objLoadSeq.Sequence_int = intCurrentSeqShouldBe;
                            }
                        }

                        intItem += 1;
                        if (intItem == lsItemsFrom.Count) blnDone = true;
                    }
                }

                //sort lsFrom, lsTo
                //dgAvailLoadSeq is sorted by CustomerName, DestinationName,SizeClas
                //dgVoyageLoadSeq is sorted by Sequence, SizeClass, then Seq applied 1..n
                if (dgFrom.Name.Contains("Avail"))
                    SortLoadSeq(ref lsItemsFrom, ref lsItemsTo);
                else
                    SortLoadSeq(ref lsItemsTo, ref lsItemsFrom);

                dgAvailLoadSeq.DataSource = null;
                dgVoyageLoadSeq.DataSource = null;

                if (lsItemsFrom.Count > 0) dgFrom.DataSource = lsItemsFrom;
                if (lsItemsTo.Count > 0) dgTo.DataSource = lsItemsTo;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GridViewMove", ex.Message);
            }
        }

        private void ListBoxMove_String(ListBox lbFrom, ListBox lbTo)
        {
            //The Listbox items are simple strings
            string strval;
            List<string> lsFrom = new List<string>();
            List<string> lsTo = new List<string>();

            try
            {
                //Copy all the ComboboxItems in lbFrom into lsFrom
                foreach (string stritem in lbFrom.Items)
                {
                    strval = stritem;
                    lsFrom.Add(stritem);
                }

                //Copy all the strings in lbTo into lsTo
                if (lbTo.Items.Count > 0)
                    foreach (string stritem in lbTo.Items)
                    {
                        strval = stritem;
                        lsTo.Add(strval);
                    }

                //Copy the Selected items in lbFrom into lsTo
                //  &  remove from lsFrom
                foreach (string stritem in lbFrom.SelectedItems)
                {
                    strval = stritem;
                    lsTo.Add(stritem);
                    lsFrom.Remove(stritem);
                }

                //Update Listboxes with sorted lists
                lsFrom.Sort();
                lbFrom.DataSource = lsFrom;

                lsTo.Sort();
                lbTo.DataSource = lsTo;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ListBoxMove_String", ex.Message);
            }

        }

        private void ListBoxMove_ComboboxItem(ListBox lbFrom, ListBox lbTo)
        {
            //The Listbox items are ComboboxItems
            ComboboxItem cboitemcopy;
            List<ComboboxItem> lsFrom = new List<ComboboxItem>();
            List<ComboboxItem> lsTo = new List<ComboboxItem>();

            try
            {
                //Copy all the ComboboxItems in lbFrom into lsFrom
                foreach (ComboboxItem cboitem in lbFrom.Items)
                {
                    cboitemcopy = cboitem.MakeCopy(cboitem);
                    lsFrom.Add(cboitemcopy);
                }

                //Copy all the ComboboxItems in lbTo into lsTo
                if (lbTo.Items.Count > 0)
                    foreach (ComboboxItem cboitem in lbTo.Items)
                    {
                        cboitemcopy = cboitem.MakeCopy(cboitem);
                        lsTo.Add(cboitemcopy);
                    }

                //Copy the Selected items in lbFrom into lsTo
                //  & use LINQ to remove from lsFrom
                foreach (ComboboxItem cboitem in lbFrom.SelectedItems)
                {
                    cboitemcopy = cboitem.MakeCopy(cboitem);
                    lsTo.Add(cboitemcopy);
                    lsFrom.RemoveAll(item => item.cboValue == cboitem.cboValue);
                }

                //Sort lsFrom & lsTo by cboText value
                //  & assign to the Listboxes
                List<ComboboxItem> lsFromSorted = lsFrom.OrderBy(it => it.cboText).ToList();
                lbFrom.DataSource = lsFromSorted;
                List<ComboboxItem> lsToSorted = lsTo.OrderBy(it => it.cboText).ToList();
                lbTo.DataSource = lsToSorted;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ListBoxMove", ex.Message);
            }
        }
        private void PerformVoyageLoadSeqMoveUpdown(string strDirection)
        {
            int intFirstSeq;
            int intLastSeq;
            int intNewSeq = 0;
            int intSeq;
            LoadSeqItem objLdSeqItem;
            List<LoadSeqItem> lsCurrentdgDataSource;
            List<LoadSeqItem> lsNewdgDataSource = new List<LoadSeqItem>();
            List<LoadSeqItem> lsNull = null;

            try
            {
                intFirstSeq = Convert.ToInt16(dgVoyageLoadSeq.SelectedRows[0].
                    Cells["Sequence_voyage"].Value.ToString());

                //Cannot move more than one seq. Check that all selected rows are for the same seq
                if (dgVoyageLoadSeq.SelectedRows.Count > 1)
                {
                    //Create a list of Selected DataGridViewRows
                    List<DataGridViewRow> dgSelectedRows = new List<DataGridViewRow>();
                    foreach (DataGridViewRow dgrow in dgVoyageLoadSeq.SelectedRows)
                        dgSelectedRows.Add(dgrow);

                    //Sort the list using Linq by comparing Seq
                    dgSelectedRows.Sort((dgrow1, dgrow2) =>
                    Convert.ToInt16(dgrow1.Cells["Sequence_voyage"].Value.ToString()).
                    CompareTo(Convert.ToInt16(dgrow2.Cells["Sequence_voyage"].Value.ToString())));

                    intFirstSeq = Convert.ToInt16(dgSelectedRows[0].
                        Cells["Sequence_voyage"].Value.ToString());

                    intLastSeq = Convert.ToInt16(dgSelectedRows[dgSelectedRows.Count - 1].
                        Cells["Sequence_voyage"].Value.ToString());

                    if (intFirstSeq != intLastSeq)
                    {
                        MessageBox.Show("You can only move ONE Sequence at a time up or down.",
                            "CANNOT MOVE MULTIPLE SEQUENCES", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }

                //Get the current datagridview datasource
                lsCurrentdgDataSource = (List<LoadSeqItem>)dgVoyageLoadSeq.DataSource;

                //Move Seq up/down based on strDirection
                if (strDirection == "UP")
                //Swap Selected Seq with the Seq-1 rows above it
                //1. Copy existing rows up to Selected Seq -1 to lsNewdgDataSource
                //2. Add selected rows to lsNewdgDataSource as Selected Seq -1
                //3. Copy existing rows for Seq-1 to lsNewdgDataSource as Seq
                //4. Copy remaining existing rows from Selected Seq+1 to end to lsNewdgDataSource
                {

                    for (int intItem = 0; intItem < lsCurrentdgDataSource.Count; intItem++)
                    {
                        objLdSeqItem = lsCurrentdgDataSource[intItem];
                        intSeq = objLdSeqItem.Sequence_int;

                        //1. Copy existing rows up to Selected Seq-1
                        if (intSeq < intFirstSeq - 1) lsNewdgDataSource.Add(objLdSeqItem);

                        //2. Copy existing rows for Seq-1 to lsNewdgDataSource as Seq
                        if (intSeq == intFirstSeq - 1)
                        {
                            objLdSeqItem.Sequence_int = intFirstSeq;
                            lsNewdgDataSource.Add(objLdSeqItem);
                        }

                        //3. Add selected rows to lsNewdgDataSource as Selected Seq -1
                        if (intSeq == intFirstSeq)
                        {
                            intNewSeq = intFirstSeq - 1;
                            objLdSeqItem.Sequence_int = intNewSeq;
                            lsNewdgDataSource.Add(objLdSeqItem);
                        }

                        //4. Copy remaining existing rows
                        //from Selected Seq+1 to end to lsNewdgDataSource
                        if (intSeq > intFirstSeq) lsNewdgDataSource.Add(objLdSeqItem);
                    }
                }
                else
                {
                    //Down movement
                    ////Swap Selected Seq with the Seq+1 rows below it
                    //1. Copy existing rows up to Selected Seq to lsNewdgDataSource
                    //2. Copy existing rows for Seq+1 to lsNewdgDataSource as Seq
                    //3. Add selected rows to lsNewdgDataSource as Selected Seq + 1
                    //4. Copy remaining existing rows from Selected Seq+2 to end to lsNewdgDataSource

                    for (int intItem = 0; intItem < lsCurrentdgDataSource.Count; intItem++)
                    {
                        objLdSeqItem = lsCurrentdgDataSource[intItem];
                        intSeq = objLdSeqItem.Sequence_int;

                        //1. Copy existing rows up to Selected Seq
                        if (intSeq < intFirstSeq) lsNewdgDataSource.Add(objLdSeqItem);

                        //2. Copy existing rows for Seq+1 to lsNewdgDataSource as Seq
                        if (intSeq == intFirstSeq + 1)
                        {
                            objLdSeqItem.Sequence_int = intFirstSeq;
                            lsNewdgDataSource.Add(objLdSeqItem);
                        }

                        //3. Add selected rows to lsNewdgDataSource as Selected Seq + 1
                        if (intSeq == intFirstSeq)
                        {
                            intNewSeq = intFirstSeq + 1;
                            objLdSeqItem.Sequence_int = intNewSeq;
                            lsNewdgDataSource.Add(objLdSeqItem);
                        }

                        //4. Copy remaining existing rows from Selected Seq+2 to end to lsNewdgDataSource
                        if (intSeq > intFirstSeq + 1) lsNewdgDataSource.Add(objLdSeqItem);
                    }
                }

                //Sort lsNewDataSource & assign as Datasource
                SortLoadSeq(ref lsNull, ref lsNewdgDataSource);
                dgVoyageLoadSeq.DataSource = lsNewdgDataSource;

                //Reselect intFirstSeq
                foreach (DataGridViewRow dgrow in dgVoyageLoadSeq.Rows)
                {
                    intSeq = Convert.ToInt16(dgrow.Cells["Sequence_voyage"].Value.ToString());
                    if (intSeq == intNewSeq) dgrow.Selected = true;
                }

                UpdateMoveButtons(true);

                blnUpdateLoadSeq = true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformVoyageLoadSeqMoveUpDown",
                    ex.Message);
            }
        }

        private void PerformVoyageMerge()
        {
            bool blnDone;
            int intItem;
            int intMergedSeq;
            int intNextSeqToAssign;
            int intSeqBeingChecked;
            List<LoadSeqItem> lsItemsToRemove = new List<LoadSeqItem>();
            List<LoadSeqItem> lsOriginalDataSource;
            List<LoadSeqItem> lsMerged = new List<LoadSeqItem>();
            List<LoadSeqItem> lsNull = null;
            LoadSeqItem objLoadSeqItem;
            LoadSeqItem objCopy;

            try
            {
                //1. Put the Selected rows into lsMerged, & remove from lsOriginalDataSource
                //2. Sort lsMerged by Seq., & store 1st Seq as intMergedSeq
                //3. Update items 1..n in lsMerged with intMergedSeq
                //4. Move non-selected rows with Seq < intMergedSeq from lsOriginalDataSource to
                //  lsMerged
                //5. Increment each Seq in lsOriginalDataSource (if any)
                //6. Copy items in lsOriginalDataSource (if any) to lsMerged
                //7. Sort lsMerged by Seq,Size,Dest
                //8. Set dgVoyageLoadSeq Datasource to lsMerged

                //1. Put the Selected rows into lsMerged, & remove from lsOriginalDataSource
                lsOriginalDataSource = (List<LoadSeqItem>)dgVoyageLoadSeq.DataSource;

                var dgSelectedRows = dgVoyageLoadSeq.SelectedRows;
                foreach (DataGridViewRow dgrow in dgSelectedRows)
                {
                    //Create a new LoadSeqItem object from selected row & add to lsMerged
                    objLoadSeqItem = new LoadSeqItem();
                    objLoadSeqItem.CustomerID_string = dgrow.Cells["CustomerID_voyage"].Value.ToString();
                    objLoadSeqItem.CustomerName =
                        dgrow.Cells["CustomerName_voyage"].Value.ToString();
                    objLoadSeqItem.DestinationName =
                        dgrow.Cells["DestinationName_voyage"].Value.ToString();
                    objLoadSeqItem.Sequence_string =
                        dgrow.Cells["Sequence_voyage"].Value.ToString();
                    objLoadSeqItem.SizeClass =
                        dgrow.Cells["SizeClass_voyage"].Value.ToString();
                    lsMerged.Add(objLoadSeqItem);
                }

                //Remove corresponding selected items from lsOriginalDataSource
                foreach (LoadSeqItem ldSeqItem in lsMerged)
                    lsOriginalDataSource.RemoveAll(obj =>
                              obj.CustomerID_int == ldSeqItem.CustomerID_int &&
                              obj.DestinationName == ldSeqItem.DestinationName &&
                              obj.SizeClass == ldSeqItem.SizeClass);

                //2. Sort lsMerged by Seq., & store 1st Seq as intFirstSeq
                lsMerged.Sort((ldseqitem1, ldseqitem2) =>
                      ldseqitem1.Sequence_int.
                      CompareTo(ldseqitem2.Sequence_int));
                intMergedSeq = lsMerged[0].Sequence_int;

                //3. Update items 1..n in lsMerged with intMergedSeq
                for (int intMergedItem = 1; intMergedItem < lsMerged.Count; intMergedItem++)
                    lsMerged[intMergedItem].Sequence_int = intMergedSeq;

                //4. Move non-selected rows (if any) with
                //  Seq < intMergedSeq from lsOriginalDataSource to lsMerged
                if (lsOriginalDataSource.Count > 0)
                {
                    blnDone = false;
                    intItem = 0;
                    while (!blnDone)
                    {
                        objLoadSeqItem = lsOriginalDataSource[intItem];
                        if (objLoadSeqItem.Sequence_int < intMergedSeq)
                        {
                            objCopy = objLoadSeqItem.MakeCopy(objLoadSeqItem);
                            lsMerged.Add(objCopy);
                            lsItemsToRemove.Add(objCopy);

                            //Set blnDone if last item in lsOriginalDataSource
                            intItem += 1;
                            if (intItem == lsOriginalDataSource.Count) blnDone = true;
                        }
                        else
                            blnDone = true;
                    }
                }

                //Remove items in lsItemsToRemove from lsOriginalDataSource
                foreach (LoadSeqItem ldSeqItem in lsItemsToRemove)
                    lsOriginalDataSource.RemoveAll(obj =>
                             obj.CustomerID_int == ldSeqItem.CustomerID_int &&
                             obj.DestinationName == ldSeqItem.DestinationName &&
                             obj.SizeClass == ldSeqItem.SizeClass);

                //5. Increment each Seq in lsOriginalDataSource (if any)
                if (lsOriginalDataSource.Count > 0)
                {
                    intNextSeqToAssign = intMergedSeq + 1;
                    intSeqBeingChecked = lsOriginalDataSource[0].Sequence_int;

                    //Loop through each item left in lsOrginalDataSource and reassign Seq
                    for (int intOrigItem = 0; intOrigItem < lsOriginalDataSource.Count; intOrigItem++)
                    {
                        objLoadSeqItem = lsOriginalDataSource[intOrigItem];
                        if (objLoadSeqItem.Sequence_int == intSeqBeingChecked)
                            objLoadSeqItem.Sequence_int = intNextSeqToAssign;
                        else
                        {
                            intNextSeqToAssign += 1;
                            intSeqBeingChecked = objLoadSeqItem.Sequence_int;
                            objLoadSeqItem.Sequence_int = intNextSeqToAssign;
                        }
                    }

                    //6. Copy items in lsOriginalDataSource (if any) to lsMerged
                    foreach (LoadSeqItem ldSeqItem in lsOriginalDataSource)
                        lsMerged.Add(ldSeqItem);
                }

                //7. Sort lsMerged
                SortLoadSeq(ref lsNull, ref lsMerged);

                //8. Set dgVoyageLoadSeq Datasource to lsMerged
                dgVoyageLoadSeq.DataSource = lsMerged;

                btnMerge.Enabled = false;
                blnUpdateLoadSeq = true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformVoyageMerge", ex.Message);
            }
        }

        private void SortLoadSeq(ref List<LoadSeqItem> lsAvail, ref List<LoadSeqItem> lsVoyage)
        {
            try
            {
                //Sort Avail by CustomerName,Destination,SizeClass
                if (lsAvail != null)
                    lsAvail = lsAvail.OrderBy(it0 =>
                      it0.CustomerName).ThenBy(it1 =>
                      it1.DestinationName).ThenBy(it2 =>
                      it2.SizeClass).ToList();

                //Sort Voyage by Seq,SizeClass,Destination
                if (lsVoyage != null)
                    lsVoyage = lsVoyage.OrderBy(it0 =>
                       it0.Sequence_int).ThenBy(it1 =>
                       it1.SizeClass).ThenBy(it2 =>
                       it2.DestinationName).ToList();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SortLoadSeq", ex.Message);
            }
        }

        private void PerformGridViewMove(DataGridView dgFrom, DataGridView dgTo)
        {
            try
            {
                GridViewMove(dgFrom, dgTo);

                UpdateMoveButtons();

                blnUpdateLoadSeq = true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformGridViewMove", ex.Message);
            }
        }

        private void PerformListboxMove(string strItemType,
            ListBox lbFrom, ListBox lbTo)
        {
            try
            {
                if (strItemType == "cboitem")
                    ListBoxMove_ComboboxItem(lbFrom, lbTo);
                else
                    ListBoxMove_String(lbFrom, lbTo);

                UpdateMoveButtons();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMove", ex.Message);
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
                strSQL = "SELECT ValueKey,ValueDescription FROM SettingTable " +
                    "WHERE ValueKey IN ('ExportDirectory','VoyageExportFileName') " +
                    "AND RecordStatus='Active' ORDER BY ValueKey";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                // S/B just two active rows, row 1 ExportDirectory, row 2 VoyageExportFileName
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 2)
                {
                    Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile",
                        "No rows returned from SettingTable");
                    return;
                }
                // 1st Record s/b ExportDirectory, 2nd Record s/b VoyageExportFileName
                strFilename = ds.Tables[0].Rows[0]["ValueDescription"].ToString();
                strFilename += @"\" + ds.Tables[0].Rows[1]["ValueDescription"].ToString();

                //2. Create a copy of the datatable used for the datagridview datasource
                dt = dtVoyages.Copy();

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
                ControlInfo controlInfo = lsControls.First(obj => obj.HeaderText == "Voyage #");
                lsCSVcols.Add(controlInfo);

                controlInfo = lsControls.First(obj => obj.HeaderText == "Voyage Date");
                lsCSVcols.Add(controlInfo);

                controlInfo = lsControls.First(obj => obj.HeaderText == "Vessel");
                lsCSVcols.Add(controlInfo);

                //5. Invoke CreateSCVFile to create, save, & open the csv file
                Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile", ex.Message);
            }
        }

        private bool MissingBayLoc()
        {
            try
            {
                DataSet ds;
                string strMsg;
                string strSQL;
                string strVoyageID = txtVoyageID.Text;

                strSQL = @"SELECT VIN FROM AutoportExportVehicles
                WHERE VoyageID = " + strVoyageID +
                @" AND DateShipped IS NULL 
                AND LEN(RTRIM(ISNULL(BayLocation,''))) = 0";

                if (blnLoadSeqOnlyClearedVehs)
                    strSQL += " AND VehicleStatus='ClearedCustoms'";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                        strMsg = "The VIN: " + ds.Tables[0].Rows[0]["VIN"] +
                            " is missing a Bay Loc.\n";
                    else
                    {
                        strMsg = "The following VINs are missing Bay Locations:\n";
                        foreach (DataRow drow in ds.Tables[0].Rows) strMsg += drow["VIN"] + "\n";
                    }

                    strMsg += "Please correct.";

                    MessageBox.Show(strMsg, "MISSING BAY LOCATION", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return true;
                }

                return false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "MissingBayLoc", ex.Message);
                return false;
            }
        }

        private void OpenLoadSeqCSVFile() {
            // 1/22/19 D.Maibor: add MissingBayLoc check.
            try {
                LoadSeqItem lastSequenceItem = ((IEnumerable<LoadSeqItem>)dgVoyageLoadSeq.DataSource).Last();
                int highestSequenceNumber = lastSequenceItem.Sequence_int;

                // Use highest sequence number to query the user for the sort order.
                var frmSort = new frmLoadSeqSortOrder(highestSequenceNumber, this);
                DialogResult sortFormDialogResult = frmSort.ShowDialog();

                if (sortFormDialogResult == DialogResult.Cancel) return;

                if (MissingBayLoc()) return;

                DataSet loadSequenceDataSet = ImportLoadSequencefromDataBase();

                // Verify that stored procedure returned good data. If not, exit.
                bool noDataForCsvFile = loadSequenceDataSet.Tables.Count == 0 || loadSequenceDataSet.Tables[0].Rows.Count == 0;
                if (noDataForCsvFile) {
                    MessageBox.Show("There are no vehicles with the correct status & no ship date for this voyage",
                        "NO VEHICLES TO EXPORT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<DataRow> formattedLoadSequence = GetFormattedLoadSequence(loadSequenceDataSet.Tables[0]);

                // Create columns for the csv file.
                bool anyVeryImportantVehicles =
                    formattedLoadSequence.Any(row => row.Field<int?>("VIV").HasValue && row.Field<int?>("VIV").Value != 0);
                bool anyUnclearedVehicles =
                    formattedLoadSequence.Any(row => row.Field<int?>("CustomsNotClearedCount").HasValue && row.Field<int?>("CustomsNotClearedCount").Value != 0);

                List<ControlInfo> csvColumns = GetLoadSequenceCSVColumnsList(anyVeryImportantVehicles, anyUnclearedVehicles);

                string csvFileName = GetLoadSequenceCSVFileName();

                Formops.CreateCSVFile(formattedLoadSequence.CopyToDataTable(), csvColumns, csvFileName);
            }

            catch (Exception ex) {
                Globalitems.HandleException(CURRENTMODULE, "OpenLoadSeqCSVFile", ex.Message);
            }

        }

        DataSet ImportLoadSequencefromDataBase() {
            string strSProc = "spExportLoadSeqInfo_Modify";
            var Paramobjects = new List<SProcParameter> {
                new SProcParameter { Paramname = "@VoyageID", Paramvalue = int.Parse(txtVoyageID.Text) },
                new SProcParameter { Paramname = "@OnlyClearedVehs", Paramvalue = blnLoadSeqOnlyClearedVehs },
            };
            return DataOps.GetDataset_with_SProc(strSProc, Paramobjects);
        }

        List<ControlInfo> GetLoadSequenceCSVColumnsList(bool anyVeryImportantVehicles, bool anyUnclearedVehicles) {
            var csvColumns = new List<ControlInfo> {
                new ControlInfo { HeaderText = "Seq.", RecordFieldName = "Sequence" },
                new ControlInfo { HeaderText = "Dest.", RecordFieldName = "DestinationName" },
                new ControlInfo { HeaderText = "Row", RecordFieldName = "BayLocRow" },
                new ControlInfo { HeaderText = "Size", RecordFieldName = "Size" },
                new ControlInfo { HeaderText = "Units", RecordFieldName = "Units" },
            };

            // Include VIV column?
            if (anyVeryImportantVehicles) {
                csvColumns.Add(new ControlInfo { HeaderText = "VIV", RecordFieldName = "VIV" });
            }

            // The all clear column.
            if (anyUnclearedVehicles) {
                csvColumns.Add(new ControlInfo { HeaderText = "Not Cleared", RecordFieldName = "CustomsNotClearedCount" });
                csvColumns.Add(new ControlInfo { HeaderText = "Total", RecordFieldName = "Total" });
            }
            else {
                csvColumns.Add(new ControlInfo { HeaderText = "All Cleared", RecordFieldName = "CustomsNotClearedCount" });
            }

            return csvColumns;
        }

        string GetLoadSequenceCSVFileName() {
            // Get the file Directory & Filename from the SettingTable.
            string fileNameQuery = "SELECT ValueKey,ValueDescription FROM SettingTable " +
                "WHERE ValueKey IN ('ExportDirectory','AEVoyageLoadSeqeunceExportFileName') " +
                "AND RecordStatus='Active' ORDER BY ValueKey DESC";
            DataSet fileNameDataSet = DataOps.GetDataset_with_SQL(fileNameQuery);

            // S/B just two active rows, row 1 ExportDirectory, row 2 VoyageExportFileName
            bool fileNameDataMalformed = fileNameDataSet.Tables.Count == 0 || fileNameDataSet.Tables[0].Rows.Count != 2;
            if (fileNameDataMalformed) {
                Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile", "No rows returned from SettingTable");
                return "";
            }

            // Build CSV file name.
            string directory = fileNameDataSet.Tables[0].Rows[0]["ValueDescription"].ToString();
            string fileName = fileNameDataSet.Tables[0].Rows[1]["ValueDescription"].ToString();
            return $@"{directory}\{fileName}";
        }

        List<DataRow> GetFormattedLoadSequence(DataTable inputTable) {
            // Added a new column to table.
            inputTable.Columns.Add(new DataColumn("Total", typeof(int)));

            // Get all the rows from the table to work on.
            List<DataRow> loadSequenceRows = inputTable.AsEnumerable().ToList();

            // Add '/' between SizeClasses, when multi SizeClasses in row
            // Grab all rows where the Size is longer than a single character.
            // Make sure there are no spaces, split into array of characters, and then rejoin with '/'
            loadSequenceRows
                .Where(row => row.Field<string>("Size").Length > 1)
                .ToList()
                .ForEach(row =>
                    row["Size"] = string.Join("/", row.Field<string>("Size").Replace(" ", "").ToCharArray())
                );

            var sequenceGroups = loadSequenceRows.GroupBy(row => row.Field<int>("Sequence"));

            var formattedLoadSequence = new List<DataRow>();
            foreach(var sequence in sequenceGroups) {
                // Sort.
                // Separate the rows into alphaNumeric, and numeric.
                int n;
                var numericBayRows = sequence.Where(row => int.TryParse(row.Field<string>("BayLocRow"), out n));
                var alphaNumericBayRows = sequence.Where(row => int.TryParse(row.Field<string>("BayLocRow"), out n) == false);

                // Sort them according to user choice.
                IEnumerable<DataRow> precedingSet;
                IEnumerable<DataRow> succeedingSet;
                bool usingHighToLow = (strSortOrder == "HL") != (lsSortExceptions.Contains(sequence.Key.ToString()));

                // First they are sorted, and then they are added to the formattedLoadSequence collection.
                if (usingHighToLow) {
                    precedingSet = alphaNumericBayRows.OrderByDescending(row => row.Field<string>("BayLocRow"));
                    succeedingSet = numericBayRows.OrderByDescending(row => int.Parse(row.Field<string>("BayLocRow")));
                }
                else {
                    precedingSet = numericBayRows.OrderBy(row => int.Parse(row.Field<string>("BayLocRow")));
                    succeedingSet = alphaNumericBayRows.OrderBy(row => row.Field<string>("BayLocRow"));
                }

                formattedLoadSequence.AddRange(precedingSet);
                formattedLoadSequence.AddRange(succeedingSet);

                //Add row for totals
                DataRow groupTotalsRow = inputTable.NewRow();
                groupTotalsRow["Size"] = "Total (Seq)";
                groupTotalsRow["Units"] = sequence.Sum(row => row.Field<int>("Units"));
                groupTotalsRow["Total"] = sequence.Sum(row => row.Field<int>("CustomsNotClearedCount"));
                formattedLoadSequence.Add(groupTotalsRow);
            }

            // Empty row to separate from data, and final total.
            formattedLoadSequence.Add(inputTable.NewRow());

            //Add last row with total under Units
            DataRow totalsRow = inputTable.NewRow();
            totalsRow["Size"] = "Total (Units)";
            totalsRow["Units"] = loadSequenceRows.Sum(row => row.Field<int?>("Units"));
            totalsRow["Total"] = loadSequenceRows.Sum(row => row.Field<int?>("CustomsNotClearedCount"));
            formattedLoadSequence.Add(totalsRow);

            // Any place, except totals, where you have a zero(0) for CustomsNotClearedCount, replace with a null;
            // This will make it blank on the csv file.
            formattedLoadSequence
                .Where(row =>
                    row["Sequence"] != null
                    && row.Field<int?>("CustomsNotClearedCount") == 0)
                .ToList()
                .ForEach(row => row["CustomsNotClearedCount"] = DBNull.Value);

            return formattedLoadSequence;
        }

        private void PerformMovePrevious()
        {
            try
            {
                bs1.MovePrevious();
                FillDetailRecord(lsVoyageIDs[bs1.Position]);

                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMovePrevious", ex.Message);
            }

        }

        private bool VehiclesShippedOnVoyage()
        {
            string strSQL;
            DataSet ds;

            try
            {
                strSQL = "SELECT COUNT(AutoportExportVehiclesID) AS totrec " +
                    "FROM AutoportExportVehicles " +
                    "WHERE VoyageID = " + txtVoyageID.Text + " AND DateShipped IS NOT NULL ";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "VehiclesShippedOnVoyage",
                        "No data returned from query");
                    return false;
                }

                if (Convert.ToInt16(ds.Tables[0].Rows[0]["totrec"]) == 0)
                    return false;
                else
                    return true;

            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "VehiclesShippedOnVoyage", ex.Message);
                return false;
            }
        }

        private void PerformDeleteRecord()
        {
            try
            {
                DataRow drow;
                DataSet ds;
                DialogResult dlResult;
                frmAreYouSure frmConfirm;
                SProcParameter objParam;
                List<SProcParameter> Paramobjects = new List<SProcParameter>();
                string strMessage;
                string strResult;
                string strSProc = "spDeleteVoyage";

                //Cannot delete voyage if associated vehicles have Shipdate
                if (VehiclesShippedOnVoyage())
                {
                    MessageBox.Show("Vehicles have already been shipped on this voyage.\n\n" +
                        "It cannot be deleted.", "CANNOT DELETE VOYAGE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strMessage = "Are you sure you want to delete this Voyage?\n\n" +
                    "Any vehicles assigned to this voyage will be unassigned.";
                frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    objParam = new SProcParameter();
                    objParam.Paramname = "@VoyageID";
                    objParam.Paramvalue = Convert.ToInt16(txtVoyageID.Text);
                    Paramobjects.Add(objParam);
                    ds = DataOps.GetDataset_with_SProc(strSProc, Paramobjects);

                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord",
                            "No data returned after invoking SProc");
                        return;
                    }

                    //Ck for error in returned table
                    drow = ds.Tables[0].Rows[0];
                    if (drow["result"].ToString() == "ERROR")
                    {
                        strResult = "DamageSProc ERROR:<br>" +
                            "ERROR NUMBER: " + drow["ErrorNumber"] + "<br>" +
                            "ERROR SEVERITY: " + drow["ErrorSeverity"] + "<br>" +
                            "ERROR STATE: " + drow["ErrorState"] + "<br>" +
                            "ERROR PROCEDURE: " + drow["ErrorProcedure"] + "<br>" +
                            "ERROR LINE: " + drow["ErrorLine"] + "<br>" +
                            "ERROR MESSAGE: " + drow["ErrorMessage"];
                        Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord",
                            strResult);
                        return;
                    }

                    //Set Mode to READ
                    strMode = "READ";
                    btnMenu.Enabled = true;

                    //Refresh dgResults
                    PerformSearch();

                    MessageBox.Show("The Voyage has been deleted", "VOYAGE DELETED",
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
                FillDetailRecord(lsVoyageIDs[bs1.Position]);

                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMoveNext", ex.Message);
            }
        }

        private string SQLForModifiedRecord()
        {
            CheckBox ckBox;
            ComboBox cboBox;
            Control[] ctrls;
            string strSQL = "";
            string strval;

            try
            {
                strSQL = "UPDATE AEVoyage SET ";

                // Use linq to get a list of updated controls,
                //  For this form, textboxes, combobox, checkbox; exclude listboxes
                var changedlist = lsControls.Where(ctrlinfo =>
                (ctrlinfo.Updated == true && !ctrlinfo.ControlID.StartsWith("lb"))).ToList();

                //If no change to AEVoyage table, return empty string for SQL
                if (changedlist.Count == 0) return "";

                foreach (ControlInfo ctrlinfo in changedlist)
                {
                    strSQL += ctrlinfo.RecordFieldName + " = ";

                    //Place the control into the array ctrls, s/b only one
                    ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            strval = ctrls[0].Text.Trim();

                            // Use HandleSingleQuoteForSQL
                            //to replace ' in text to '' for SQL
                            strval = Globalitems.HandleSingleQuoteForSQL(strval);

                            if (strval.Length == 0)
                                strSQL += "NULL";
                            else
                                strSQL += "'" + strval + "'";

                            break;
                        case "SelectedValue":
                            //Cast control to ComboBox. All cbo's except cboVehStatus,
                            //cboRecordStatus, cboDest have an int value in the
                            //AutoportExportVehicles table
                            cboBox = (ComboBox)ctrls[0];
                            strval = (cboBox.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                            strval = Globalitems.HandleSingleQuoteForSQL(strval);
                            if (strval == "select")
                                strSQL += "NULL";
                            else
                                strSQL += strval;
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

                //Add UpdatedDate & UpdatedBy
                strSQL += "UpdatedDate = GetDate(),";
                strSQL += "UpdatedBy = '" + Globalitems.strUserName + "' ";

                // Add WHERE clause
                strSQL += " WHERE AEVoyageID = " + txtVoyageID.Text;
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
            string strval;

            try
            {
                strSQL = "INSERT INTO AEVoyage (AEVesselID, VoyageNumber," +
                    "VoyageDate, VoyageClosedInd, CreationDate, CreatedBy)" +
                     " VALUES (";

                //Ck for VesselID

                strval = (cboVessel_record.SelectedItem as ComboboxItem).cboValue.ToString();

                if (strval == "select")
                    strSQL += "NULL,";
                else
                    strSQL += Globalitems.HandleSingleQuoteForSQL(strval) + ",";

                //Add VoyageNumber
                strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(txtVoyageNumber_record.Text.Trim()) +
                    "',";

                //Add VoyageDate
                strSQL += "'" + txtVoyageDate_record.Text + "',";

                //Ck VoyageClosedInd
                if (ckClosed_record.Checked)
                    strSQL += "1,";
                else
                    strSQL += "0,";

                //Add CreationDate, CreatedBy
                strSQL += "GetDate(),'" + Globalitems.strUserName + "')";

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForNewRecord", ex.Message);
                return "";
            }
        }

        private string CreateTempLoadSeqTable()
        {
            DataColumn col;
            DateTime datNow = DateTime.Now;
            DataTable dtLoadSeqs;
            DataRow dtRow;
            string strDateNow;
            string strSQL;
            string strTempTableName = "tmpVoyageLoadSeqUpdate_";

            try
            {
                //Create dtLoadSeqs table with the needed columns to pass in LoadSeq info
                //NOTE: VoyageID sent in to SProc as parameter
                dtLoadSeqs = new DataTable("dtLoadSeqa");

                col = new DataColumn("Sequence");
                col.DataType = typeof(System.Int32);
                dtLoadSeqs.Columns.Add(col);

                col = new DataColumn("CustomerID");
                col.DataType = typeof(System.Int32);
                dtLoadSeqs.Columns.Add(col);

                col = new DataColumn("DestinationName");
                col.DataType = typeof(System.String);
                dtLoadSeqs.Columns.Add(col);

                col = new DataColumn("SizeClass");
                col.DataType = typeof(System.String);
                dtLoadSeqs.Columns.Add(col);

                //Fill dtLoadSeq with the info from each row in dgVoyageLoadSeq
                foreach (DataGridViewRow dgRow in dgVoyageLoadSeq.Rows)
                {
                    dtRow = dtLoadSeqs.NewRow();
                    dtRow["Sequence"] = Convert.ToInt32(dgRow.Cells["Sequence_voyage"].Value.ToString());
                    dtRow["CustomerID"] = Convert.ToInt32(dgRow.Cells["CustomerID_voyage"].Value.ToString());
                    dtRow["DestinationName"] = dgRow.Cells["DestinationName_voyage"].Value;
                    dtRow["SizeClass"] = dgRow.Cells["SizeClass_voyage"].Value;
                    dtLoadSeqs.Rows.Add(dtRow);
                }

                //Create a unique string based on datetime for tmp table name in SQL DB
                strDateNow = datNow.ToString("yyyyMMddHHMMss");

                //Create the table tmpVoyageLoadSeqUpdate_[strDateNow] in SQL DB
                //  with the same columns as dtLoadSeqs
                strTempTableName += strDateNow;
                strSQL = "CREATE TABLE " + strTempTableName +
                    " (Sequence int," +
                    "CustomerID int," +
                    "DestinationName varchar(20)," +
                    "SizeClass varchar(10))";
                DataOps.PerformDBOperation(strSQL);

                //Use bulk copy to load tmp table
                DataOps.PerformBulkCopy(strTempTableName, dtLoadSeqs);

                return strTempTableName;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CreateTempLoadSeqTable", ex.Message);
                return "";
            }
        }

        private void PerformSaveRecord()
            // 2/7/18 D.Maibor: After insert/save record, check if txtVoyageID is null or empty 
        {
            try
            {
                DataRow drow;
                DataSet ds;

                List<SProcParameter> Paramobjects = new List<SProcParameter>();
                SProcParameter objParam;

                string strSProc = "spUpdateVoyageInfo";
                string strResult;
                string strTempLoadSeqTableName = "";
                string strUpdateVoyageSQL = "";
                string strVoyageCustomers = "";
                string strVoyageDestinations = "";

                if (ValidRecord())
                {
                    if (strMode == "NEW")
                        strUpdateVoyageSQL = SQLForNewRecord();
                    else
                        strUpdateVoyageSQL = SQLForModifiedRecord();

                    //If no listboxes were modified, just execute SQL to update AEVoyage table
                    if (!blnUpdateVoyageCustomer && !blnUpdateVoyageDestination &&
                        !blnUpdateLoadSeq)
                    {
                        DataOps.PerformDBOperation(strUpdateVoyageSQL);
                    }
                    else
                    {
                        //Need to update Customer/Destination/LoadSeq (at least one)
                        //Use the SProc, spUpdateVoyageInfo

                        //Create a string of CustomerIDs separated by ',' if changed
                        if (blnUpdateVoyageCustomer)
                        {
                            if (lbVoyageCustomers.Items.Count > 0)
                            {
                                foreach (ComboboxItem cboitem in lbVoyageCustomers.Items)
                                    strVoyageCustomers += cboitem.cboValue.ToString() + ",";

                                //Remove last ','
                                strVoyageCustomers = strVoyageCustomers.Substring(0,
                                    strVoyageCustomers.Length - 1);
                            }
                        }

                        //Create a string of Destinations separated by ','
                        if (blnUpdateVoyageDestination)
                        {
                            foreach (string strDestination in lbVoyageDestinations.Items)
                                strVoyageDestinations += strDestination + ",";

                            //Remove last ','
                            strVoyageDestinations = strVoyageDestinations.Substring(0,
                                strVoyageDestinations.Length - 1);
                        }

                        if (blnUpdateLoadSeq)
                        {

                            if (dgVoyageLoadSeq.RowCount > 0)
                                strTempLoadSeqTableName = CreateTempLoadSeqTable();
                        }

                        //Create Paramobjects for SProc
                        objParam = new SProcParameter();
                        objParam.Paramname = "@Mode";
                        objParam.Paramvalue = strMode;
                        Paramobjects.Add(objParam);

                        objParam = new SProcParameter();
                        objParam.Paramname = "@VoyageID";

                        // User VoyageID = 0, if NEW rec
                        if (strMode == "MODIFY")
                            objParam.Paramvalue = lsVoyageIDs[0];
                        else
                            objParam.Paramvalue = "0";

                        Paramobjects.Add(objParam);

                        objParam = new SProcParameter();
                        objParam.Paramname = "@VoyageSQL";
                        objParam.Paramvalue = strUpdateVoyageSQL;
                        Paramobjects.Add(objParam);

                        //Only include changed Customer/Destination/LoadSeq params if changed
                        if (blnUpdateVoyageCustomer)
                        {
                            objParam = new SProcParameter();
                            objParam.Paramname = "@CustomerIDs";
                            objParam.Paramvalue = strVoyageCustomers;
                            Paramobjects.Add(objParam);
                        }

                        if (blnUpdateVoyageDestination)
                        {
                            objParam = new SProcParameter();
                            objParam.Paramname = "@Destinations";
                            objParam.Paramvalue = strVoyageDestinations;
                            Paramobjects.Add(objParam);
                        }

                        if (blnUpdateLoadSeq)
                        {
                            //If dgVoyageLoadSeq has rows

                            objParam = new SProcParameter();
                            objParam.Paramname = "@LoadSeqstable";
                            objParam.Paramvalue = strTempLoadSeqTableName;
                            Paramobjects.Add(objParam);
                        }

                        objParam = new SProcParameter();
                        objParam.Paramname = "@Createdby";
                        objParam.Paramvalue = Globalitems.strUserName;
                        Paramobjects.Add(objParam);

                        ds = DataOps.GetDataset_with_SProc(strSProc, Paramobjects);

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord",
                                "No data returned after invoking SProc");
                            return;
                        }

                        //Ck for error in returned table
                        drow = ds.Tables[0].Rows[0];
                        if (drow["result"].ToString() == "ERROR")
                        {
                            strResult = "DamageSProc ERROR:<br>" +
                                "ERROR NUMBER: " + drow["ErrorNumber"] + "<br>" +
                                "ERROR SEVERITY: " + drow["ErrorSeverity"] + "<br>" +
                                "ERROR STATE: " + drow["ErrorState"] + "<br>" +
                                "ERROR PROCEDURE: " + drow["ErrorProcedure"] + "<br>" +
                                "ERROR LINE: " + drow["ErrorLine"] + "<br>" +
                                "ERROR MESSAGE: " + drow["ErrorMessage"];
                            Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord",
                                strResult);
                            return;
                        }
                    }

                    // On successful DB update, alert the user.
                    // If there's rows in the load sequence box,
                    // ask the user if they would like to perform an export.
                    AlertUserOfDatabaseUpdateSuccessAndAllowExportViewIfEligible();

                    //Display other forms
                    Globalitems.DisplayOtherForms(this, true);

                    //Set Mode to READ
                    strMode = "READ";
                    recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                    AdjustReadOnlyStatus(true);

                    //Enable Search/Results panels
                    pnlSearch.Enabled = true;
                    pnlResults.Enabled = true;

                    btnMenu.Enabled = true;
                    btnSearch.Enabled = true;
                    btnClear.Enabled = true;

                    //Set Status label to Read only
                    lblStatus.Text = "Read only";

                    // Capture id of current record, if any;
                    int oldVoyageId = 0;
                    if (!string.IsNullOrEmpty(txtVoyageID.Text)) oldVoyageId = int.Parse(txtVoyageID.Text);

                    //Perform new search
                    PerformSearch();

                    // Get record that matches record before search.
                    // Takes the DataGridViewRowCollection, and casts it to IEnumerable<DataGridViewRow>.
                    // This allows Linq to be used. Find the first record where the value in the first cell,
                    // which is the VoyageId, matches the voyage id of the record the user was updating. If
                    // no row fits this criteria, FirstOrDefault will return null.
                    DataGridViewRow record =
                        dgResults
                            .Rows
                            .Cast<DataGridViewRow>()
                            .FirstOrDefault(row => ((int)row.Cells[0].Value) == oldVoyageId);

                    // If record isn't null, user's record is among search results. Select it, and display it.
                    if (record != null) {
                        dgResults.ClearSelection();
                        record.Selected = true;
                        FilterBindingSource();
                    }

                } // If ValidRecord
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord", ex.Message);
            }
        }

        private bool ValidRecord()
        {
            try
            {
                //Ck Voyage Number
                if (txtVoyageNumber_record.Text.Trim().Length == 0)
                {
                    MessageBox.Show("The Voyage Number cannot be blank.",
                        "MISSING VOYAGE NUMBER", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtVoyageNumber_record.Focus();
                    return false;
                }

                //Ck Voyage Date
                if (txtVoyageDate_record.Text.Trim().Length == 0)
                {
                    MessageBox.Show("The Voyage Date cannot be blank.",
                        "MISSING VOYAGE DATE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtVoyageDate_record.Focus();
                    return false;
                }

                //Use linq to find all updated controls
                var changedlist = lsControls.Where(ctrlinfo => ctrlinfo.Updated == true).ToList();
                if (strMode == "MODIFY" && changedlist.Count == 0 &&
                    !blnUpdateVoyageCustomer && !blnUpdateVoyageDestination && !blnUpdateLoadSeq)
                {
                    MessageBox.Show("You have not changed anything for this Voyage.\r\n" +
                       "There is nothing to update", "NO CHANGES MADE",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                strSQL = "SELECT voy.AEVoyageID, voy.VoyageNumber, " +
                    "CASE " +
                    "WHEN LEN(RTRIM(ISNULL(ves.VesselShortName, ''))) > 0 THEN ves.VesselShortName " +
                     "ELSE ves.VesselName " +
                    "END AS Vessel," +
                     "voy.VoyageDate " +
                     "FROM AEVoyage voy " +
                    "LEFT JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID " +
                    "WHERE VoyageDate IS NOT NULL ";

                //Add VoyageNumber
                if (txtVoyageNumber.Text.Trim().Length > 0)
                    strSQL += "AND voy.VoyageNumber LIKE '%" + txtVoyageNumber.Text.Trim() + "%' ";

                //Add VoyageDate From
                if (txtFrom.Text.Trim().Length > 0)
                    strSQL += " AND voy.VoyageDate >= '" + txtFrom.Text + "' ";

                //Add VoyageDate To
                if (txtTo.Text.Trim().Length > 0)
                    strSQL += " AND voy.VoyageDate <= '" + txtTo.Text + "' ";

                //Add Vessel Name
                if (txtVesselName.Text.Trim().Length > 0)
                {
                    strval = txtVesselName.Text.Trim();
                    strval = Globalitems.HandleSingleQuoteForSQL(strval);
                    strSQL += "AND (ves.VesselShortName LIKE '%" + strval + "%' OR ves.VesselName LIKE '%" +
                        txtVesselName.Text.Trim() + "%') ";
                    if (ckActive.Checked) strSQL += "AND ves.RecordStatus='Active' ";
                }

                //Add Voyage Is Closed
                if (ckClosed.Checked)
                {
                    strSQL += "AND voy.VoyageClosedInd = 1 ";
                }

                strSQL += "ORDER BY voy.VoyageDate DESC";

                ds = DataOps.GetDataset_with_SQL(strSQL);

                // Use a DataTable as the DataSource for the DataGridView to make sorting by Col Header
                //  clicks, automatic
                dtVoyages = ds.Tables[0].Copy();
                if (dtVoyages.Rows.Count == 0) return;

                //6. If data found:
                //6a. Enable Export button
                btnExport.Enabled = true;

                //6b. Assign Datatable to gridvirew
                dgResults.DataSource = dtVoyages;

                //6c. Update # records label
                lblVoyageRecords.Text = "Records: " + dtVoyages.Rows.Count;

                //6d. Because dgResults is not multiselect,
                //create a list of 1 CustomerID for the Form's binding source,
                //for use by the nav buttons
                lsVoyageIDs.Clear();
                lsVoyageIDs.Add(dtVoyages.Rows[0]["AEVoyageID"].ToString());

                bs1.DataSource = lsVoyageIDs;

                //6e. Fill detail record with first row AEVoyageID
                txtVoyageID.Text = dtVoyages.Rows[0]["AEVoyageID"].ToString();
                FillDetailRecord(txtVoyageID.Text);

                //6f. Update recbuttons
                recbuttons.blnRecordsToDisplay = true;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                Globalitems.SetNavButtons(recbuttons, bs1);
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

        private void btnMenu_Click(object sender, EventArgs e)
        {
            //Make sure Main form displays and has the focus
            Globalitems.MainForm.Visible = true;
            Globalitems.MainForm.Focus();
        }

        private void txtFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            { Globalitems.CheckDateKeyPress(e); }
        }

        private void txtTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            { Globalitems.CheckDateKeyPress(e); }
        }

        private void txtFrom_Validating(object sender, CancelEventArgs e)
        {
            { Globalitems.ValidateDate(txtFrom, e); }
        }

        private void txtTo_Validating(object sender, CancelEventArgs e)
        {
            { Globalitems.ValidateDate(txtTo, e); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            OpenCSVFile();
        }

        private void ckActive_CheckedChanged(object sender, EventArgs e)
        {
            ActiveConditionChange();
        }

        private void btnCancel_Clicked()
        {
            try { CancelSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnCancel_Clicked", ex.Message); }
        }

        private void btnDelete_Clicked()
        {
            try { PerformDeleteRecord(); }
            catch (Exception ex)
            { Globalitems.HandleException(CURRENTMODULE, "btnDelete_Clicked", ex.Message); }
        }

        private void btnModify_Clicked()
        {
            try { ModifyRecordSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnModify_Clicked", ex.Message); }
        }

        private void btnNext_Clicked()
        {
            try { PerformMoveNext(); }
            catch (Exception ex)
            { Globalitems.HandleException(CURRENTMODULE, "btnPrev_Clicked", ex.Message); }
        }

        private void btnNew_Clicked()
        {
            try { NewRecordSetup(); }
            catch (Exception ex)
            { Globalitems.HandleException(CURRENTMODULE, "btnNew_Clicked", ex.Message); }
        }

        private void btnPrev_Clicked()
        {
            try { PerformMovePrevious(); }
            catch (Exception ex)
            { Globalitems.HandleException(CURRENTMODULE, "btnPrev_Clicked", ex.Message); }
        }

        private void btnSave_Clicked()
        {
            try { PerformSaveRecord(); }
            catch (Exception ex)
            { Globalitems.HandleException(CURRENTMODULE, "btnSave_Clicked", ex.Message); }
        }

        private void dgResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //As long as row clicked is not the Column Header row, index = -1,
            //change the binding source
            if (e.RowIndex > -1) FilterBindingSource();
        }

        private void dgAvailLoadSeq_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Unselect first row, which is the default action
            dgAvailLoadSeq.ClearSelection();
        }

        private void dgVoyageLoadSeq_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Unselect first row, which is the default action
            dgVoyageLoadSeq.ClearSelection();
        }

        private void MoveToVoyageLoadSeq()
        {
            try
            {
                int intMissingSizeClass = 0;

                //Ck if any selected rows are missing SizeClass
                foreach (DataGridViewRow dgRow in dgAvailLoadSeq.SelectedRows)
                    if (dgRow.Cells["SizeClass_avail"].Value.ToString().Length == 0)
                    {
                        dgRow.Selected = false;
                        intMissingSizeClass += 1;
                    }

                //Warn User there are missing Size Classes
                if (intMissingSizeClass > 0)
                {
                    if (intMissingSizeClass == dgAvailLoadSeq.SelectedRows.Count)
                    {
                        MessageBox.Show("All the selected rows are missing the Size and " +
                            "cannot be added to the Load Sequence",
                            "MISSING SIZE CLASS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show(intMissingSizeClass + " of the selected rows are missing the Size and " +
                            "cannot be added to the Load Sequence",
                            "MISSING SIZE CLASS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                PerformGridViewMove(dgAvailLoadSeq, dgVoyageLoadSeq);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "MoveToVoyageLoadSeq", ex.Message);
            }
        }

        private void btnMoveToVoyageLoadSeq_Click(object sender, EventArgs e)
        { MoveToVoyageLoadSeq(); }

        private void dgAvailLoadSeq_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //As long as row clicked is not the Column Header row, index = -1,
            //change the binding source
            if (e.RowIndex > -1) UpdateMoveButtons();
        }

        private void dgVoyageLoadSeq_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //As long as row clicked is not the Column Header row, index = -1,
            //change the binding source
            if (e.RowIndex > -1) UpdateMoveButtons();
        }

        private void btnMoveToVoyageCust_Click(object sender, EventArgs e)
        {
            PerformListboxMove("cboitem", lbAvailCustomers, lbVoyageCustomers);
            blnUpdateVoyageCustomer = true;
        }

        private void lbAvailCustomers_SelectedIndexChanged(object sender, EventArgs e)
        { UpdateMoveButtons(); }

        private void btnMoveToAvailCust_Click(object sender, EventArgs e)
        {
            PerformListboxMove("cboitem", lbVoyageCustomers, lbAvailCustomers);
            blnUpdateVoyageCustomer = true;
        }

        private void btnMoveToVoyageDest_Click(object sender, EventArgs e)
        {
            PerformListboxMove("string", lbAvailDestinations, lbVoyageDestinations);
            blnUpdateVoyageDestination = true;
        }

        private void btnMoveToAvailDest_Click(object sender, EventArgs e)
        {
            PerformListboxMove("string", lbVoyageDestinations, lbAvailDestinations);
            blnUpdateVoyageDestination = true;
        }

        private void lbVoyageCustomers_SelectedIndexChanged(object sender, EventArgs e)
        { UpdateMoveButtons(); }

        private void lbAvailDestinations_SelectedIndexChanged(object sender, EventArgs e)
        { UpdateMoveButtons(); }

        private void lbVoyageDestinations_SelectedIndexChanged(object sender, EventArgs e)
        { UpdateMoveButtons(); }

        private void btnMoveToAvailLoadSeq_Click(object sender, EventArgs e)
        {
            PerformGridViewMove(dgVoyageLoadSeq, dgAvailLoadSeq);
            blnUpdateLoadSeq = true;
        }

        private void txtVoyageNumber_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtVoyageNumber_record", lsControls);
        }

        private void cboVessel_record_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboVessel_record", lsControls);
        }

        private void txtVoyageDate_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtVoyageDate_record", lsControls);
        }

        private void txtVoyageDate_record_KeyPress(object sender, KeyPressEventArgs e)
        {
            { Globalitems.CheckDateKeyPress(e); }
        }

        private void txtVoyageDate_record_Validating(object sender, CancelEventArgs e)
        {
            { Globalitems.ValidateDate(txtVoyageDate_record, e); }
        }

        private void ckClosed_record_CheckedChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("ckClosed_record", lsControls);
        }

        private void frmVoyageAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (strMode != "READ")
            {
                MessageBox.Show("You must SAVE or Cancel the current changes to close this form",
                    "CANNOT CLOSE THIS FORM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void frmVoyageAdmin_Activated(object sender, EventArgs e)
        {
            if (blnNewVoyageRQFromOtherForm)
            {
                blnNewVoyageRQFromOtherForm = false;
                recbuttons.btnNew_Click(null, null);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        { PerformVoyageLoadSeqMoveUpdown("UP"); }

        private void btnDown_Click(object sender, EventArgs e)
        { PerformVoyageLoadSeqMoveUpdown("DOWN"); }

        private void btnMerge_Click(object sender, EventArgs e)
        { PerformVoyageMerge(); }

        private void btnMoveInfo_Click(object sender, EventArgs e)
        {
            //Inform User why Merge or Up/Down buttons are disabled
            if (btnMerge.Enabled)
                MessageBox.Show("Up/Down buttons are disabled because" +
                "\nall selected rows must be the same Sequence to move up/down.",
                "WHY UP/DOWN ARROWS ARE DISABLED",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Merge button is disabled because, " +
                "\nmerging requires the selected Sequences to be different.",
                "WHY MERGE BUTTON IS DISABLED",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportLoadSeq_Click(object sender, EventArgs e) {
            OpenLoadSeqCSVFile();
        }

        private void AlertUserOfDatabaseUpdateSuccessAndAllowExportViewIfEligible() {
            string message = "The Voyage info has been updated in the DB.";
            string title = "VOYAGE INFO UPDATE";
            var buttons = MessageBoxButtons.OK;
            var icon = MessageBoxIcon.Information;

            if (dgVoyageLoadSeq.Rows.Count != 0) {
                message += "\n\nWould you like to perform an Export?";
                buttons = MessageBoxButtons.YesNo;
                icon = MessageBoxIcon.Question;
            }

            DialogResult result = MessageBox.Show(message, title, buttons, icon);
            if (result == DialogResult.Yes)
                OpenLoadSeqCSVFile();
        }

        private void frmVoyageAdmin_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}