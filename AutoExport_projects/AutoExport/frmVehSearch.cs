using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

//6/12/18 D.Maibor. Add CloseAdditionalCriteriaForm method and invoke upon Opening & Closing this form.

namespace AutoExport
{
    public partial class frmVehSearch : Form
    {
        //CONSTANTS
        private const string CURRENTMODULE = "frmVehSearch";

        //Variables
        public AdditionalCriteriaItem objAddlCriteria = new AdditionalCriteriaItem();    //Set by frmAddEditAddlCriteria
        private bool blnFillingCBOs = false;
        private BindingSource bs1 = new BindingSource();
        private DataTable dtVehicles = new DataTable();
        private List<string> lsHideSelectedColumns = new List<string>();
        private PrintInfo objPrintInfo = new PrintInfo();

        //Set up List of ControlInfo objects, lsControlInfo, as follows:
        //  Order in list establishes Indexes for tabbing, implemented by SetTabIndex() method
        //  AlwaysReadOnly identifies if control cannot be modified by User
        //  ControlPropertyToBind identifies what controls are initialized 
        //  RecordFieldName identify what controls display record detail
        //  HeaderText sets column header to use for Export to csv file
        //  Updated property establishes what controls User has modified

        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="txtVIN", ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtBayLoc", ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboCust", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="cboForwarder", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="cboExporter", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="cboDest", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="cboVessel", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="cboVoyageDate", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtBookingNumber", ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboVehStatus", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="btnAddlCriteria"},

            // objects needed for csv file  HeaderText="Cust. Name"
            new ControlInfo {RecordFieldName="VIN",HeaderText="VIN"},
            new ControlInfo {RecordFieldName="YearModel",HeaderText="Year/Model"},
            new ControlInfo {RecordFieldName="NonRunner",HeaderText="N/R"},
            new ControlInfo {RecordFieldName="Color",HeaderText="Color"},
            new ControlInfo {RecordFieldName="BayLocation",HeaderText="Bay Location"},
            new ControlInfo {RecordFieldName="Customer",HeaderText="Customer"},
            new ControlInfo {RecordFieldName="DestinationName",HeaderText="Destination"},
            new ControlInfo {RecordFieldName="Vessel",HeaderText="Vessel"},
            new ControlInfo {RecordFieldName="VoyageDate",HeaderText="Voyage Date"},
            new ControlInfo {RecordFieldName="Forwarder",HeaderText="Forwarder"},
            new ControlInfo {RecordFieldName="BookingNumber",HeaderText="Booking Number"},
            new ControlInfo {RecordFieldName="VIVTagNumber",HeaderText="VIV Tag #"},
            new ControlInfo {RecordFieldName="SizeClass",HeaderText="Size Class"},
            new ControlInfo {RecordFieldName="DateReceived",HeaderText="Date Received"},
            new ControlInfo {RecordFieldName="DateSubmittedCustoms",HeaderText="Date Submitted"},
            new ControlInfo {RecordFieldName="CustomsApprovedDate",HeaderText="Date Approved"},
            new ControlInfo {RecordFieldName="DateShipped",HeaderText="Date Shipped"},
            new ControlInfo {RecordFieldName="VehicleStatus",HeaderText="Vehicle Status"}
        };

        public frmVehSearch()
        {
            InitializeComponent();

            dgResults.AutoGenerateColumns = false;
            
            FillCombos();
            Globalitems.SetControlHeight(this);
            Formops.SetTabIndex(this, lsControls);

            if (Globalitems.blnCannotPrintLabels)
            {
                btnPrintLabels.Enabled = false;
                btnNoLabels.Visible = true;
            }

            lblHideCols.Visible = false;
            pnlHiddenCols.Visible = false;
            btnExport.Enabled = false;

            CloseAdditionalCriteriaForm();

            //Hide New btns if User not an Administrator
            if (!Globalitems.strRoleNames.Contains("Administrator"))
            {
                btnNewCustomer.Visible = false;
                btnNewDestination.Visible = false;
                btnNewExporter.Visible = false;
                btnNewForwarder.Visible = false;
                btnNewVoyage.Visible = false;
            }
        }

        private void CloseAdditionalCriteriaForm()
        {
            if (Application.OpenForms.OfType<frmAddEditAddlCriteria>().Count() == 1)
                Application.OpenForms.OfType<frmAddEditAddlCriteria>().First().Close();
        }

        private void frmVehSearch_Activated(object sender, EventArgs e)
        {
            //Ck Add'l Criteria
            lblAddlCriteria.Visible = false;
            if (objAddlCriteria.blnAddlCriteria)
            {
                lblAddlCriteria.Visible = true;

                //Unck ckBlankShippedDate, if User entered Ship From/To date
                if (objAddlCriteria.ShipDateFrom != null || objAddlCriteria.ShipDateTo != null)
                    ckBlankShippedDate.Checked = false;
            }

            btnContinue.Enabled = false;
            if (dgResults.SelectedRows.Count > 0) btnContinue.Enabled = true;

            if (objAddlCriteria.RerunSearch) PerformSearch();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {PerformSearch();}

        private void ClearGridView()
        {
            try
            {
                lblVehRecords.Text = "";

                dtVehicles.Clear();
                dgResults.DataSource = dtVehicles;
                btnExport.Enabled = false;
                btnSelectAll.Enabled = false;
                btnDeselectAll.Enabled = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearGridView", ex.Message);
            }
        }

        private void FillCombos()

        {
            ComboboxItem cboitem;
            DataSet ds;
            string strFilter;
            string strSQL;

            try
            {
                blnFillingCBOs = true;

                //cboCust
                cboCust.Items.Clear();

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
                
                // Add All to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "All";
                cboitem.cboValue = "All";
                cboCust.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dr["CustName"].ToString();
                    cboitem.cboValue = dr["CustomerID"].ToString();
                    cboCust.Items.Add(cboitem);
                }

                cboCust.DisplayMember = "cboText";
                cboCust.ValueMember = "cboValue";
                cboCust.SelectedIndex = 0;

                //cboVessel
                cboVessel.Items.Clear();

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

                // Add All to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "All";
                cboitem.cboValue = "All";
                cboVessel.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dr["VesselName"].ToString();
                    cboitem.cboValue = dr["AEVesselID"].ToString();
                    cboVessel.Items.Add(cboitem);
                }

                cboVessel.DisplayMember = "cboText";
                cboVessel.ValueMember = "cboValue";
                cboVessel.SelectedIndex = 0;

                //cboDest
                cboDest.Items.Clear();
                strFilter = "CodeType='ExportDischargePort' AND Code <> '' ";
                if (ckActive.Checked) strFilter += "AND RecordStatus='Active'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboDest, true, false);

                //cboVoyageDate
                cboVoyageDate.Items.Clear();

                strSQL = "SELECT AEVoyageID, VoyageDate " +
                    "FROM AEVoyage WHERE VoyageDate IS NOT NULL ORDER BY VoyageDate DESC";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillCombos",
                        "No rows returned from Voyage table");
                    return;
                }

                // Add All to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "All";
                cboitem.cboValue = "All";
                cboVoyageDate.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = Convert.ToDateTime(dr["VoyageDate"]).ToString("M/d/yyyy");
                    cboitem.cboValue = dr["AEVoyageID"].ToString();
                    cboVoyageDate.Items.Add(cboitem);
                }

                cboVoyageDate.DisplayMember = "cboText";
                cboVoyageDate.ValueMember = "cboValue";
                cboVoyageDate.SelectedIndex = 0;

                //cboStatus
                cboVehStatus.Items.Clear();
                strFilter = "CodeType='AutoportExportVehicleStatus' ";
                if (ckActive.Checked) strFilter += "AND RecordStatus='Active'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboVehStatus, true, false);

                //lbCols
                string strHeader;
                string strColname;
                foreach (DataGridViewColumn dgCol in dgResults.Columns)
                {
                    if (dgCol.Visible)
                    {
                        strHeader = dgCol.HeaderText;
                        strColname = dgCol.Name;
                        cboitem = new ComboboxItem();
                        cboitem.cboText = strHeader;
                        cboitem.cboValue = strColname;
                        lbCols.Items.Add(cboitem);
                    }
                }

                lbCols.DisplayMember = "cboText";
                lbCols.ValueMember = "cboValue";
                lbCols.SelectedIndex = -1;

                blnFillingCBOs = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
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
                        "WHERE ValueKey IN ('ExportDirectory','VehicleExportFileName') " +
                        "AND RecordStatus='Active' ORDER BY ValueKey";
                    ds = DataOps.GetDataset_with_SQL(strSQL);

                    // S/B just two active rows, row 1 ExportDirectory, row 2 VehicleExportFileName
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 2)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile",
                            "No rows returned from SettingTable");
                        return;
                    }
                    // 1st Record s/b ExportDirectory, 2nd Record s/b VehicleExportFileName
                    strFilename = ds.Tables[0].Rows[0]["ValueDescription"].ToString();
                    strFilename += @"\" + ds.Tables[0].Rows[1]["ValueDescription"].ToString();

                    //2. Create a copy of the datatable used for the datagridview datasource
                    dt = dtVehicles.Copy();

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

                    //Use .First so Linq performs faster
                    var objctrlinfo_VIN = lsControls.First(obj => obj.HeaderText == "VIN");
                    lsCSVcols.Add(objctrlinfo_VIN);

                    var objctrlinfo_YrModel = lsControls.First(obj => obj.HeaderText == "Year/Model");
                    lsCSVcols.Add(objctrlinfo_YrModel);

                    var objctrlinfo_NonRunner = lsControls.First(obj => obj.HeaderText == "N/R");
                    lsCSVcols.Add(objctrlinfo_NonRunner);

                    var objctrlinfo_Color = lsControls.First(obj => obj.HeaderText == "Color");
                    lsCSVcols.Add(objctrlinfo_Color);

                    var objctrlinfo_Bayloc = lsControls.First(obj => obj.HeaderText == "Bay Location");
                    lsCSVcols.Add(objctrlinfo_Bayloc);

                    var objctrlinfo_Customer = lsControls.First(obj => obj.HeaderText == "Customer");
                    lsCSVcols.Add(objctrlinfo_Customer);

                    var objctrlinfo_Destination = lsControls.First(obj => obj.HeaderText == "Destination");
                    lsCSVcols.Add(objctrlinfo_Destination);

                    var objctrlinfo_Vessel = lsControls.First(obj => obj.HeaderText == "Vessel");
                    lsCSVcols.Add(objctrlinfo_Vessel);

                    var objctrlinfo_Voydate = lsControls.First(obj => obj.HeaderText == "Voyage Date");
                    lsCSVcols.Add(objctrlinfo_Voydate);

                    var objctrlinfo_Forwarder = lsControls.First(obj => obj.HeaderText == "Forwarder");
                    lsCSVcols.Add(objctrlinfo_Forwarder);

                    var objctrlinfo_Booknum = lsControls.First(obj => obj.HeaderText == "Booking Number");
                    lsCSVcols.Add(objctrlinfo_Booknum);

                    var objctrlinfo_VIV = lsControls.First(obj => obj.HeaderText == "VIV Tag #");
                    lsCSVcols.Add(objctrlinfo_VIV);

                    var objctrlinfo_SizeClass = lsControls.First(obj => obj.HeaderText == "Size Class");
                    lsCSVcols.Add(objctrlinfo_SizeClass);

                    var objctrlinfo_DateRcvd = lsControls.First(obj => obj.HeaderText == "Date Received");
                    lsCSVcols.Add(objctrlinfo_DateRcvd);

                    var objctrlinfo_DateSub = lsControls.First(obj => obj.HeaderText == "Date Submitted");
                    lsCSVcols.Add(objctrlinfo_DateSub);

                    var objctrlinfo_DateApprov = lsControls.First(obj => obj.HeaderText == "Date Approved");
                    lsCSVcols.Add(objctrlinfo_DateApprov);

                    var objctrlinfo_DateShip = lsControls.First(obj => obj.HeaderText == "Date Shipped");
                    lsCSVcols.Add(objctrlinfo_DateShip);

                    var objctrlinfo_VehStatus = lsControls.First(obj => obj.HeaderText == "Vehicle Status");
                    lsCSVcols.Add(objctrlinfo_VehStatus);

                //5. Invoke CreateSCVFile to create, save, & open the csv file
                Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile", ex.Message);
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

                //3. Rerieve data as datatable
                strSQL = "SELECT veh.AutoportExportVehiclesID AS VehID,veh.CustomerID," +
                    "RIGHT(veh.VIN, 8) AS VIN8," +
                    "veh.VIN," +
                    "CASE " +
                        "WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName " +
                        "ELSE cus.CustomerName " +
                    "END AS Customer," +
                    "veh.DestinationName," +
                    "veh.VehicleYear," +
                    "veh.Make AS Make," +
                    "veh.Model AS Model," +
                    "veh.VehicleYear + ' ' + veh.Model AS YearModel," +
                    "CASE " + 
                        "WHEN ISNULL(veh.NoStartInd,0) = 0 THEN '' " + 
                        "ELSE 'X' " +
                    "END AS NonRunner," +
                    "veh.Color," +
                    "veh.BayLocation," +
                    "veh.DateReceived," +
                    "veh.DateSubmittedCustoms," +
                    "veh.CustomsApprovedDate," +
                    "veh.CustomsApprovalPrintedInd," +
                    "veh.DateShipped," +
                    "CASE " +
                        "WHEN DATALENGTH(ves.VesselShortName) > 0 THEN ves.VesselShortName " +
                        "ELSE ves.VesselName " +
                    "END AS Vessel," +
                    "veh.VehicleStatus," +
                    "ExporterID," +
                    "CASE " +
                        "WHEN DATALENGTH(ex.ExporterShortName) > 0 THEN ex.ExporterShortName " +
                        "ELSE ex.ExporterName " +
                    "END AS Exporter," +
                    "veh.FreightForwarderID," + 
                    "CASE " +
                        "WHEN DATALENGTH(ff.FreightForwarderShortName) > 0 THEN ff.FreightForwarderShortName " +
                        "ELSE ff.FreightForwarderName " +
                    "END AS Forwarder," +
                    "veh.BookingNumber," +
                    "voy.VoyageDate," +
                    "veh.SizeClass," +
                    "veh.VIVTagNumber " +
                    "FROM AutoportExportVehicles veh " +
                    "LEFT JOIN AEVoyage voy ON voy.AEVoyageID = veh.VoyageID " +
                    "LEFT JOIN AEVessel ves ON  ves.AEVesselID = voy.AEVesselID " +
                    "LEFT JOIN AEExporter ex ON  ex.AEExporterID = veh.ExporterID " +
                    "LEFT JOIN AEFreightForwarder ff ON  ff.AEFreightForwarderID = veh.FreightForwarderID " +
                    "LEFT JOIN Customer cus ON  cus.CustomerID = veh.CustomerID " +
                    "LEFT JOIN Billing bill ON  bill.BillingID = veh.BillingID " +
                    "WHERE " +
                    "cus.CustomerID IS NOT NULL ";

                //Get Add'l WHERE criteria

                //Add active to Customer, Forwarder, Exporter, Destination  if ckActive checked
                if (ckActive.Checked)
                {
                    strSQL += " AND cus.RecordStatus='Active'";
                    strSQL += " AND ISNULL(ff.RecordStatus,'Active') = 'Active'";
                    strSQL += " AND ISNULL(ex.RecordStatus,'Active') = 'Active'";
                    strSQL += " AND veh.DestinationName IN (SELECT Code FROM Code " +
                        "WHERE CodeType = 'ExportDischargePort' AND Code <> '' AND RecordStatus = 'Active')";
                    strSQL += " AND ISNULL(ves.RecordStatus,'Active') = 'Active'";
                }

                //Ck VIN
                if (txtVIN.Text.Trim().Length > 0) strSQL += " AND veh.VIN LIKE '%" + txtVIN.Text.Trim() + "%'";

                //Ck Bay Loc.
                if (txtBayLoc.Text.Trim().Length > 0) strSQL += " AND veh.BayLocation  = '" + txtBayLoc.Text.Trim() + "'";

                //Ck Customer
                strval = (cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strval != "All") strSQL += " AND veh.CustomerID = " + strval;

                //Ck Forwarder
                if (cboForwarder.SelectedIndex > -1)
                {
                    strval = (cboForwarder.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    if (strval != "All") strSQL += " AND veh.FreightForwarderID = " + strval;
                }
                
                //Ck Exporter
                if (cboExporter.SelectedIndex > -1)
                {
                    strval = (cboExporter.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    if (strval != "All") strSQL += " AND veh.ExporterID = " + strval;
                }

                //Ck Destination
                strval = (cboDest.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strval != "All") strSQL += " AND veh.DestinationName = '" + Globalitems.HandleSingleQuoteForSQL(strval) + "'";

                //Ck Vessel
                strval = (cboVessel.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strval != "All") strSQL += " AND ves.AEVesselID = " + strval;

                //Ck Voyage Date
                strval = (cboVoyageDate.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strval != "All") strSQL += " AND voy.AEVoyageID = " + strval;

                //Ck Booking No.
                if (txtBookingNumber.Text.Trim().Length > 0) strSQL += " AND veh.BookingNumber  = '" + txtBookingNumber.Text.Trim() + "'";

                //Ck Veh. Status
                strval = (cboVehStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strval != "All") strSQL += " AND veh.VehicleStatus = '" + strval + "'";

                //Ck ShippedDateIsBlank
                if (ckBlankShippedDate.Checked) strSQL += " AND (veh.DateShipped IS NULL OR veh.DateShipped='') ";

                //Add Add'l criteria, if there is any
                if (objAddlCriteria.blnAddlCriteria)
                    strSQL += objAddlCriteria.CreateWhereClauseForAdditionalCriteria();
                
                //Add order by
                strSQL += " ORDER BY RIGHT(veh.VIN, 8)";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Add comma separators to Record count
                    lblVehRecords.Text = "Records: " + ds.Tables[0].Rows.Count.ToString("#,##0");

                    dtVehicles = ds.Tables[0];
                    dgResults.DataSource = dtVehicles; ;
                    dgResults.ClearSelection();
                    btnSelectAll.Enabled = true;
                    btnExport.Enabled = true;
                    btnContinue.Enabled = false;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSearch", ex.Message);
            }
        }

        public void DisplayAddlCriteriaLabel(bool blnDisplay)
        {
            lblAddlCriteria.Visible = blnDisplay;
        }

        private void RefillcboExporter()
        {
            {
                ComboboxItem cboitem;
                DataSet ds;
                string strSQL;

                try
                {
                    cboExporter.Items.Clear();

                    strSQL = "SELECT  AEExporterID, " +
                        "CASE WHEN LEN(RTRIM(ISNULL(ExporterShortName,''))) > 0 THEN " +
                            "ExporterShortName " +
                            "ELSE ExporterName  " +
                        "END AS Exporter " +
                        "FROM AEExporter " +
                        "WHERE LEN(RTRIM(ISNULL(ExporterName,''))) > 0 ";

                    //Add AEFreightForwarderID if cboForwarder != All
                    if ((cboForwarder.SelectedItem as ComboboxItem).cboValue.ToString().Trim() != "All")
                        strSQL += "AND AEFreightForwarderID  = " +
                            (cboForwarder.SelectedItem as ComboboxItem).cboValue.ToString() + " ";

                    //Add RecordStatus if ckActiv is checked
                    if (ckActive.Checked)
                        strSQL += "AND RecordStatus='Active' ";

                    strSQL += "ORDER BY Exporter";

                    ds = DataOps.GetDataset_with_SQL(strSQL);

                    // Add All to cbo
                    cboitem = new ComboboxItem();
                    cboitem.cboText = "All";
                    cboitem.cboValue = "All";
                    cboExporter.Items.Add(cboitem);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        cboitem = new ComboboxItem();
                        cboitem.cboText = dr["Exporter"].ToString();
                        cboitem.cboValue = dr["AEExporterID"].ToString();
                        cboExporter.Items.Add(cboitem);
                    }

                    cboExporter.DisplayMember = "cboText";
                    cboExporter.ValueMember = "cboValue";
                    cboExporter.SelectedIndex = 0;
                }

                catch (Exception ex)
                {
                    Globalitems.HandleException(CURRENTMODULE, "RefillcboExporter", ex.Message);
                }
            }
        }

        private void cboForwarder_SelectedIndexChanged(object sender, EventArgs e)
        {RefillcboExporter(); }

        private void RefillcboForwarder()
        {
            ComboboxItem cboitem;
            DataSet ds;
            string strSQL;

            try
            {
                cboForwarder.Items.Clear();

                //Only fill if a specific customer is selected
                if (cboCust.SelectedIndex < 1) return;

                cboForwarder.Items.Clear();

                strSQL = "SELECT  AEFreightForwarderID, " +
                    "CASE WHEN LEN(RTRIM(ISNULL(FreightForwarderShortName,''))) > 0 THEN " +
                        "FreightForwarderShortName " +
                        "ELSE FreightForwarderName " +
                    "END AS Forwarder " +
                    "FROM AEFreightForwarder " +
                    "WHERE AECustomerID IS NOT NULL AND " +
                    "LEN(RTRIM(ISNULL(FreightForwarderName,''))) > 0 ";

                //Add CustomerID if cboCust != All
                if ((cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim() != "All")
                    strSQL += "AND AECustomerID = " +
                        (cboCust.SelectedItem as ComboboxItem).cboValue.ToString() + " ";

                //Add RecordStatus if ckActiv is checked
                if (ckActive.Checked)
                    strSQL += "AND RecordStatus='Active' ";

                strSQL += "ORDER BY Forwarder";

                ds = DataOps.GetDataset_with_SQL(strSQL);

                // Add All to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "All";
                cboitem.cboValue = "All";
                cboForwarder.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dr["Forwarder"].ToString();
                    cboitem.cboValue = dr["AEFreightForwarderID"].ToString();
                    cboForwarder.Items.Add(cboitem);
                }

                cboForwarder.DisplayMember = "cboText";
                cboForwarder.ValueMember = "cboValue";
                cboForwarder.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "RefillcboForwarder", ex.Message);
            }
        }

        private void cboCust_SelectedIndexChanged(object sender, EventArgs e)
        { if (!blnFillingCBOs) RefillcboForwarder(); }

        private void ClearForm()
        {
            try
            {
                frmAddEditAddlCriteria frm;

                //1. Clear all items in lsControls
                Formops.ClearSetup(this, lsControls);

                //Clear cboForwarder, cboExporter
                cboForwarder.Items.Clear();
                cboExporter.Items.Clear();

                //2. Clear Form unique grids
                ClearGridView();

                //3. Clear 
                objAddlCriteria.Initialize();
                lblAddlCriteria.Visible = false;

                //Clear frmAddEditAddlCriteria if opern
                if (Application.OpenForms.OfType<frmAddEditAddlCriteria>().Count() > 0)
                {
                    frm = (frmAddEditAddlCriteria)Application.OpenForms["frmAddEditAddlCriteria"];
                    frm.ClearForm();
                }

            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearSetup", ex.Message);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void OpenAdditionalCriteria()
        {
            frmAddEditAddlCriteria frm;

            //If frmAddEditAddlCriteria is already open set frm to it
            if (Application.OpenForms.OfType<frmAddEditAddlCriteria>().Count() == 0)
            {
                frm = new frmAddEditAddlCriteria(objAddlCriteria,this,cboCust);
                Formops.OpenNewForm(frm);
            }
            else
            {
                frm = (frmAddEditAddlCriteria)Application.OpenForms["frmAddEditAddlCriteria"];
                frm.Show();
            }           
        }

        private void btnAddlCriteria_Click(object sender, EventArgs e)
        {OpenAdditionalCriteria();}

        private void btnHideCols_Click(object sender, EventArgs e)
        {
            // Store the current selected indices 
            pnlHiddenCols.Visible = true;  
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string strColname;

            try
            {
                //Clear lbCols 
                lbCols.ClearSelected();

                //Set selected in lbCols for each item in lsHideSelectedColumns. Index in lbCols is 1 less than
                //  dgResults
                for (int intIndex = 1; intIndex < dgResults.Columns.Count; intIndex++)
                {
                    strColname = dgResults.Columns[intIndex].Name;

                    if (lsHideSelectedColumns.Contains(strColname))
                        lbCols.SetSelected(intIndex-1,true);
                }

                pnlHiddenCols.Visible = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "btnCancel_Click", ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strColname;

            lblHideCols.Visible = false;
            lsHideSelectedColumns.Clear();

            //Store selected items in lbCols in lsHideSelectedColumns, Hide/Display columns in dgResults
            if (lbCols.SelectedIndices.Count > 0)
            {
                //Store selected items in lsHideSelectedColumns
                foreach (int intIndex in lbCols.SelectedIndices)
                    lsHideSelectedColumns.Add((lbCols.Items[intIndex] as ComboboxItem).cboValue.ToString());

                //Hide/Display dgResults cols based on contents of lsHideSelectedColumns
                for (int intIndex = 0; intIndex < dgResults.Columns.Count; intIndex++)
                {
                    strColname = dgResults.Columns[intIndex].Name;

                    if (lsHideSelectedColumns.Contains(strColname))
                        dgResults.Columns[intIndex].Visible = false;
                    else
                        dgResults.Columns[intIndex].Visible = true;
                }

                lblHideCols.Visible = true;
            }

            pnlHiddenCols.Visible = false;
        }

        private void btnClearCols_Click(object sender, EventArgs e)
        {
            lbCols.ClearSelected();
            lsHideSelectedColumns.Clear();
            lblHideCols.Visible = false;
            pnlHiddenCols.Visible = false;

            for (int intIndex = 1; intIndex < dgResults.Columns.Count; intIndex++)
                dgResults.Columns[intIndex].Visible = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {OpenCSVFile();}

        private void btnSelectAll_Click(object sender, EventArgs e)
        {dgResults.SelectAll();}

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {dgResults.ClearSelection();}

        private void dgResults_SelectionChanged(object sender, EventArgs e)
        {
            //Enable/Disable Continue & Set buttons depending on rows selected

            bool blnEnabled = true;

            if (dgResults.SelectedRows.Count == 0) blnEnabled = false;

            btnDeselectAll.Enabled = blnEnabled;
            btnContinue.Enabled = blnEnabled;
            btnSetDest.Enabled = blnEnabled;
            btnSetVoyage.Enabled = blnEnabled;
            btnSetForwarder.Enabled = blnEnabled;
            btnSetExporter.Enabled = blnEnabled;
            btnSetBooking.Enabled = blnEnabled;
            btnSetCust.Enabled = blnEnabled;
            if (Globalitems.blnAdmin) btnSetBillTo.Enabled = blnEnabled;
        }

        private List<int> GetListOfSelectedIDs()
        {
            int intID;
           
            List<int> lsIDs = new List<int>();

            //Place the VehID from all selected rows into ldIDs
            foreach (DataGridViewRow dgRow in dgResults.SelectedRows)
            {
                intID = Convert.ToInt32(dgRow.Cells["VehID"].Value.ToString());
                lsIDs.Add(intID);
            }
                
            return lsIDs;
        }

        private void OpenPrintCustomClearedSheetsForm()
        //4/11/18 D.Maibor: change criteria to Veh. Status = ClearedCustoms
        {
            DataView dv;
            int intID;
            string strFilter;
            string strIDs;
           
            try
            {
                frmCustomClearedSheets frm;

                objPrintInfo.Message = "";
                objPrintInfo.SelectedIDs = GetListOfSelectedIDs();

                //Ck if any selected rows are not status ClearedCustoms
                if (objPrintInfo.SelectedIDs.Count > 0)
                {
                    //Place all the SelectedIDs into strIDs 
                    strIDs = "VehID IN (";

                    foreach (int intSelectedID in objPrintInfo.SelectedIDs)
                        strIDs += intSelectedID.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    //Use ClearedCustoms status to print CustomsCleared sheets. 
                    //strFilter = "(CustomsApprovedDate IS NULL OR DateShipped IS NOT NULL) AND " + 
                    //    strIDs;
                    strFilter = "(VehicleStatus<>'ClearedCustoms') AND " +
                        strIDs;
                    dv = new DataView(dtVehicles, strFilter, "VIN8", DataViewRowState.CurrentRows);
                    if (dv.Count > 0)
                    {
                        if (dv.Count == objPrintInfo.SelectedIDs.Count)
                        {
                            MessageBox.Show("None of the selected rows have a ClearedCustoms Status for printing " +
                                "Customs Cleared Sheets", "NO VEHICLES TO PRINT", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            objPrintInfo.SelectedIDs.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Some of the selected Rows do not have a ClearedCustoms Status " +
                            "and will not be printed", "SOME VEHICLES ARE NOT CLEARED", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                            //Remove the IDs SelectedIDs
                            foreach (DataRowView dvrow in dv)
                            {
                                intID = (int) dvrow["VehID"];
                                objPrintInfo.SelectedIDs.Remove(intID);
                            }
                        }
                    }

                    if (objPrintInfo.SelectedIDs.Count > 0 )
                    {
                        //Ck if any selected rows are not printed
                        //Place all the SelectedIDs into strIDs 
                        strIDs = "VehID IN (";

                        foreach (int intSelectedID in objPrintInfo.SelectedIDs)
                            strIDs += intSelectedID.ToString() + ",";

                        //Replace last ',' with ')'
                        strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                        //Use DATS critera to print CustomsCleared sheets. 
                        strFilter = "(CustomsApprovalPrintedInd=0) AND " +
                            strIDs;
                        dv = new DataView(dtVehicles, strFilter, "VIN8", DataViewRowState.CurrentRows);
                        if (dv.Count > 0)
                        {
                            //Add the IDs to UnPrintedIDs
                            foreach (DataRowView dvrow in dv)
                            {
                                intID = (int)dvrow["VehID"];
                                objPrintInfo.UnprintedIDs.Add(intID);
                            }
                        }
                    }
                }

                if (objPrintInfo.SelectedIDs.Count > 0)
                {
                    //Set message for rbSelected
                    objPrintInfo.Message =
                        "Selected rows (" + objPrintInfo.SelectedIDs.Count +
                        ") from Veh. Locator form ";
                }
                
                //Use Show method if not currently open
                if (Application.OpenForms.OfType<frmCustomClearedSheets>().Count() == 0)
                {
                    frm = new frmCustomClearedSheets(objPrintInfo);
                    Formops.SetFormBackground(frm);
                    frm.blnCustomsClearedReport = true;
                    frm.Show();
                }
                else //Already open, set as Active form
                {
                    frm = (frmCustomClearedSheets)Application.OpenForms["frmCustomClearedSheets"];
                    frm.objPrintInfo = objPrintInfo;
                    frm.blnCustomsClearedReport = true;
                    frm.Activate();
                }  
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenPrintCustomClearedSheetsForm", ex.Message);
            }
        }

        private void OpenPrintLabelsForm()
        {
            DataView dv;
            frmLabels frm;
            string strIDs;
            string strFilter;

            try
            {
                objPrintInfo.Message = "";
                objPrintInfo.UnprintedIDs.Clear();
                objPrintInfo.SelectedIDs = GetListOfSelectedIDs();

                //Set public variables in frmLabels
                if (objPrintInfo.SelectedIDs.Count > 0)
                {
                    objPrintInfo.Message = "Selected rows (" + 
                        objPrintInfo.SelectedIDs.Count + ") from Veh. Locator form ";

                    //Update objPrintinfo.UnprintedIDs
                    strIDs = "VehID IN (";

                    foreach (int intSelectedID in objPrintInfo.SelectedIDs)
                        strIDs += intSelectedID.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    //Use DATS critera to print CustomsCleared sheets. 
                    strFilter = "(CustomsApprovalPrintedInd=0) AND " +
                        strIDs;
                    dv = new DataView(dtVehicles, strFilter, "VIN8", DataViewRowState.CurrentRows);
                    if (dv.Count > 0)
                    {
                        foreach (DataRowView dvRow in dv)
                            objPrintInfo.UnprintedIDs.Add((int) dvRow["VehID"]);
                    }
                }

                //If frmLabels is already open set frm to it
                if (Application.OpenForms.OfType<frmLabels>().Count() == 0)
                {
                    frm = new frmLabels(objPrintInfo);
                    Formops.SetFormBackground(frm);
                }
                else
                {
                    frm = (frmLabels)Application.OpenForms["frmLabels"];
                }

                //Use Show method if not currently open
                if (Application.OpenForms.OfType<frmLabels>().Count() == 0)
                    frm.Show();
                else //Already open, set as Active form
                {
                    frm.objPrintInfo = objPrintInfo;
                    frm.Activate();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenPrintLabelsForm", ex.Message);
            }
        }

        private void GetNewDestination()
        {
            try
            {
               
            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetNewDestination", ex.Message);
            }
        }

        private void SetCustomer()
        {
            DialogResult dlResult;
            DataView dv;
            frmSetSelection frm;
            List<int> lsSelectedIDs = GetListOfSelectedIDs();
            string strCustomer = "";
            string strFilter;
            string strIDs;
            string strMessage;
            string strSQL;

            try
            {
                // Use frmAreYouSure to confirm changing Customer
                strMessage = "Are you sure you want to update the Customer?\n\n" + 
                    "You will also have to reset the Voyage, Forwarder," + 
                    "\nBillToCustomer, and  Exporter for the vehicles; " +
                    "\nand reprint any previously printed labels.";
                frmAreYouSure frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    //Place all the IDs in lsSelectedIDs into strIDs 
                    strIDs = "VehID IN (";

                    foreach (int intID in lsSelectedIDs) strIDs += intID.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    //Place the selected rows into dv
                    dv = new DataView(dtVehicles, strIDs, "VIN8", DataViewRowState.CurrentRows);

                    //Ck if any selected rows in dtVehicles have a DateShipped value
                    strFilter = "DateShipped IS NOT NULL AND " + strIDs;
                    dv = new DataView(dtVehicles, strFilter, "VIN8", DataViewRowState.CurrentRows);

                    if (dv.Count > 0)
                    {
                        if (dv.Count == lsSelectedIDs.Count)
                        {
                            MessageBox.Show("All the rows you selected " +
                            " have Shipped dates.\n\nThe Customer for these rows cannot be changed.",
                            "ALL SELECTED ROWS HAVE SHIPPED DATES", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        else
                        {
                            dlResult = MessageBox.Show(dv.Count.ToString() + " of the rows you selected " +
                            " have Shipped dates.\n\nThe Customer for these rows cannot be changed.\n\n" +
                            "Do you want to ignore these rows and change the Customer on the remaining rows?",
                            "SOME ROWS SELECTED HAVE SHIPPED DATES", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);

                            if (dlResult == DialogResult.No) return;
                        }
                    }

                    //Open frmSetSelection in modal form, to get new Destination
                    frm = new frmSetSelection("Customer", cboCust);
                    dlResult = frm.ShowDialog();

                    if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                    {
                        //Replace VehID with AutoportExportVehiclesID for table UPDATE 
                        strIDs = strIDs.Replace("VehID", "AutoportExportVehiclesID");

                        //Update AutoportExportVehicles with new VoyageID
                        strSQL = "UPDATE AutoportExportVehicles SET CustomerID = " + 
                            Globalitems.strSelection + "," +
                            "VesselID = NULL," +
                            "VoyageID = 0," +
                            "FreightForwarderID = NULL," +
                            "ExporterID = NULL," +
                            "BillToInd=0," +
                            "BillToCustomerID = NULL," +
                            "BillToNote = NULL," +
                            "UpdatedBy='" + Globalitems.strUserName + "'," +
                            "UpdatedDate = '" + DateTime.Now + "' " +
                            "WHERE DateShipped IS NULL AND " + strIDs;
                        DataOps.PerformDBOperation(strSQL);

                        //Get the selected customer
                        foreach (ComboboxItem cbItem in cboCust.Items)
                            if (cbItem.cboValue == Globalitems.strSelection) strCustomer = cbItem.cboText;

                        MessageBox.Show("The Customer is changed to " + strCustomer +
                            "\n\nYou must reset the Voyage, Forwarder, and Exporter (if appl.)",
                           "CUSTOMER CHANGED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        PerformSearch();
                    }
                }      
             }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetCustomer", ex.Message);
            }
        }

        private void SetBillTo()
        {
            System.Windows.Forms.ComboBox cbo = new System.Windows.Forms.ComboBox();
            ComboboxItem cboitem;
            DataView dv;
            DialogResult dlResult;
            frmSetSelection frm;
            int intpos;
            List<int> lsSelectedIDs = GetListOfSelectedIDs();
            string strBillToCustID;
            string strBillToNote;
            string strCustomerID;
            string strIDs;
            string strMessage;
            string strSQL;

            try
            {
                if (dgResults.SelectedRows.Count == 0)
                {
                    MessageBox.Show("You must select the rows to change", "NO ROWS SELECTED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Place all the IDs in lsSelectedIDs into strIDs 
                strIDs = "VehID IN (";

                foreach (int intID in lsSelectedIDs) strIDs += intID.ToString() + ",";

                //Replace last ',' with ')'
                strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                //Place the selected rows into dv
                dv = new DataView(dtVehicles, strIDs, "VIN8", DataViewRowState.CurrentRows);

                //Get the Distinct cutomers, to see if there are more than one
                DataTable DistinctCustomers = dv.ToTable(true, "CustomerID");

                //Only allow changing the BillTo for one customer at a time
                if (DistinctCustomers.Rows.Count > 1)
                {
                    MessageBox.Show("The vehicles you selected are for more than one customer.\n\n" +
                        "Please select rows for only one customer to change the Bill To.",
                        "CANNOT SET BILL TO FOR MULTIPLE CUSTOMERS", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }



                ////Only allow Sallaum/Glovis to set BillTo, per John.
                //if (dv[0]["Customer"].ToString() == "LGL")
                //{
                //    MessageBox.Show("You cannot set a Bill To Customer for LGL.",
                //        "CANNOT SET BILL TO FOR THE CUSTOMER LGL", MessageBoxButtons.OK,
                //        MessageBoxIcon.Error);
                //    return;
                //}

                //Store the single CustomerID
                strCustomerID = DistinctCustomers.Rows[0]["CustomerID"].ToString();

                //Retrieve BillTo Customers for selected CustomerID
                Globalitems.SetBillToCbo(ref cbo, strCustomerID);

                if (cbo.Items.Count == 0)
                {
                    MessageBox.Show("The selected customer doesn't have any Bill To customers.",
                        "CANNOT SET BILL TO FOR THE SELECTED CUSTOMER", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                strMessage = "Are you sure you want to change the Bill To Customer?";
                frmAreYouSure frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK) 
                {
                    //Get BillTo & Note
                    frm = new frmSetSelection("Bill To Cust.", cbo, 
                        "Please select the Bill To Cust. && enter a Note",
                        false, true);
                    dlResult = frm.ShowDialog();

                    if (dlResult == DialogResult.OK)
                    {
                        //Retrieve BillTo Cust ID & Note with ~ as delimiter
                        intpos = Globalitems.strSelection.IndexOf("~");
                        if (intpos > 0)
                        {
                            strBillToCustID = Globalitems.strSelection.Substring(0, intpos);
                            strBillToNote = Globalitems.strSelection.Substring(intpos+1);
                            strBillToNote = Globalitems.HandleSingleQuoteForSQL(strBillToNote);

                            //Replace VehID with AutoportExportVehiclesID for table UPDATE 
                            strIDs = strIDs.Replace("VehID", "AutoportExportVehiclesID");

                            //Update AutoportExportVehicles based on whether strBillToCustID is
                            //  'none' or a CustomerID. Add note to beginning of existing note
                            if (strBillToCustID == "none")
                                strSQL = @"UPDATE AutoportExportVehicles SET BillToInd = 0,
                                 BillToCustomerID = NULL,
                                 BillToNote = ' --- " + strBillToNote + @"' + ISNULL(BilltoNote,''),
                                 UpdatedBy='" + Globalitems.strUserName + "'," +
                                 "UpdatedDate = '" + DateTime.Now + "' " +
                                 "WHERE " + strIDs;
                            else
                                strSQL = @"UPDATE AutoportExportVehicles SET BillToInd = 1,
                                BillToCustomerID = " + strBillToCustID + @",
                                BillToNote = ' --- " + strBillToNote + @"' + ISNULL(BilltoNote, ''),
                                UpdatedBy='" + Globalitems.strUserName + "'," +
                                "UpdatedDate = '" + DateTime.Now + "' " +
                                "WHERE " + strIDs;

                            DataOps.PerformDBOperation(strSQL);

                            MessageBox.Show("The Bill To Customer for selected VINs " +
                                "has been changed.\n\n" +
                                "Change the search parameters in Veh. Loc., or " +
                                "view Veh. Detail to confirm", "BILL TO CHANGED",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(nameof(frmVehSearch), nameof(SetBillTo), ex.Message);
            }
        }

        private void SetBookingNumber()
        {
            DialogResult dlResult;
            DataView dv;
            frmSetSelection frm;
            List<int> lsSelectedIDs = GetListOfSelectedIDs();
            string strFilter;
            string strIDs;
            string strMessage;
            string strSQL;

            try
            {
                // Use frmAreYouSure to confirm changing Booking Number
                strMessage = "Are you sure you want to update the Booking Number?";
                frmAreYouSure frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    //Place all the IDs in lsSelectedIDs into strIDs 
                    strIDs = "VehID IN (";

                    foreach (int intID in lsSelectedIDs) strIDs += intID.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    //Place the selected rows into dv
                    dv = new DataView(dtVehicles, strIDs, "VIN8", DataViewRowState.CurrentRows);

                    //Ck if any selected rows in dtVehicles have a DateShipped value
                    strFilter = "DateShipped IS NOT NULL AND " + strIDs;
                    dv = new DataView(dtVehicles, strFilter, "VIN8", DataViewRowState.CurrentRows);

                    if (dv.Count > 0)
                    {
                        if (dv.Count == lsSelectedIDs.Count)
                        {
                            MessageBox.Show("All the rows you selected " +
                            " have Shipped dates.\n\nThe Booking Number for these rows cannot be changed.",
                            "ALL SELECTED ROWS HAVE SHIPPED DATES", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        else
                        {
                            dlResult = MessageBox.Show(dv.Count.ToString() + " of the rows you selected " +
                            " have Shipped dates.\n\nThe Exporter for these rows cannot be changed.\n\n" +
                            "Do you want to ignore these rows and change the Booking Number on the remaining rows?",
                            "SOME ROWS SELECTED HAVE SHIPPED DATES", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);

                            if (dlResult == DialogResult.No) return;
                        }
                    }

                    //Open frmSetSelection in modal form, to get new Booking Number
                    frm = new frmSetSelection("Booking Number");
                    dlResult = frm.ShowDialog();

                    if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                    {
                        //Replace VehID with AutoportExportVehiclesID for table UPDATE 
                        strIDs = strIDs.Replace("VehID", "AutoportExportVehiclesID");

                        strSQL = "UPDATE AutoportExportVehicles SET BookingNumber = '" +
                             Globalitems.strSelection + "', UpdatedBy='" + Globalitems.strUserName + "'," +
                             "UpdatedDate = '" + DateTime.Now + "' " +
                             "WHERE DateShipped IS NULL AND " + strIDs;
                        DataOps.PerformDBOperation(strSQL);

                        MessageBox.Show("The Booking Number is changed to " + Globalitems.strSelection,
                          "BOOKING NUMBER CHANGED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        PerformSearch();
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetBookingNumber", ex.Message);
            }
        }

        private void SetExporter()
        {
            System.Windows.Forms.ComboBox cbo;
            ComboboxItem cboitem;
            DialogResult dlResult;
            DataSet ds;
            DataTable dtExporters;
            DataView dv;
            frmSetSelection frm;
            int intCustomerID;
            int intForwarderID;
            List<int> lsSelectedIDs = GetListOfSelectedIDs();
            string strFilter;
            string strIDs;
            string strMessage;
            string strSQL;

            try
            {
                // Use frmAreYouSure to confirm changing Voyage
                strMessage = "Are you sure you want to update the Exporter?";
                frmAreYouSure frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    //Place all the IDs in lsSelectedIDs into strIDs 
                    strIDs = "VehID IN (";

                    foreach (int intID in lsSelectedIDs) strIDs += intID.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    //Place the selected rows into dv
                    dv = new DataView(dtVehicles, strIDs, "VIN8", DataViewRowState.CurrentRows);

                    //Get the Distinct customers, to see if there are more than one
                    DataTable DistinctCustomers = dv.ToTable(true, "CustomerID");

                    //Only allow changing the Exporter for one customer at a time
                    if (DistinctCustomers.Rows.Count > 1)
                    {
                        MessageBox.Show("The vehicles you selected are for more than one customer.\n\n" +
                            "Please select rows for only one customer to change the Exporter.",
                            "CANNOT SET EXPORTER FOR MULTIPLE CUSTOMERS", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    //Store the single CustomerID
                    intCustomerID = (int)DistinctCustomers.Rows[0]["CustomerID"];


                    //Check if the selected rows are missing Forwarder
                    //  cannot select an Exporter if no Forwarder is present 
                    dv = new DataView(dtVehicles, strIDs + " AND FreightForwarderID IS NULL", 
                        "VIN8", DataViewRowState.CurrentRows);
                    if (dv.Count > 0)
                    {
                        if (dv.Count == lsSelectedIDs.Count)
                        {
                            MessageBox.Show("All the rows you selected " +
                            " have no Forwarder.\n\nThe Exporter for these rows cannot be changed.",
                            "ALL SELECTED ROWS HAVE NO FORWARDER", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        else
                        {
                            dlResult = MessageBox.Show(dv.Count.ToString() + " of the rows you selected " +
                            " have no Forwarder.\n\nThe Exporter for these rows cannot be changed.\n\n" +
                            "Do you want to ignore these rows and change the Exporter on the remaining rows?",
                            "SOME ROWS SELECTED HAVE NO FORWARDER", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);

                            if (dlResult == DialogResult.No) return;
                        }
                    }

                    //Get the Distinct Forwarders, to see if there are more than one
                    dv = new DataView(dtVehicles, strIDs + " AND FreightForwarderID IS NOT NULL",
                       "VIN8", DataViewRowState.CurrentRows);
                    DataTable DistinctForwarders = dv.ToTable(true, "FreightForwarderID");

                    //Only allow changing the Exporter for one Forwarder at a time
                    if (DistinctForwarders.Rows.Count > 1)
                    {
                        MessageBox.Show("The vehicles you selected are for more than one Forwarder.\n\n" +
                            "Please select rows for only one one Forwarder to change the Exporter.",
                            "CANNOT SET EXPORTER FOR MULTIPLE FORWARDERS", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    intForwarderID = (int)DistinctForwarders.Rows[0]["FreightForwarderID"];

                    //Check if there is at least one Exporter for the Forwarder
                    strSQL = "SELECT  AEExporterID, " +
                         "CASE WHEN LEN(RTRIM(ISNULL(ExporterShortName,''))) > 0 THEN " +
                             "ExporterShortName " +
                             "ELSE ExporterName  " +
                         "END AS Exporter " +
                         "FROM AEExporter " +
                         "WHERE ExporterName IS NOT NULL AND AEFreightForwarderID  = " +
                         intForwarderID + " ";

                    //Add RecordStatus if ckActiv is checked
                    if (ckActive.Checked)
                        strSQL += "AND RecordStatus='Active' ";

                    strSQL += "ORDER BY Exporter";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "SetExporter", "No table returned for Exporters");
                        return;
                    }

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There are no Exporters for this Forwarder.\n\n" +
                            "You must first create a new Exporter for this Forwarder.",
                            "NO EXPORTERS FOR THIS FORWARDER", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    dtExporters = ds.Tables[0];

                    //Ck if any selected rows in dtVehicles have a DateShipped value
                    strFilter = "DateShipped IS NOT NULL AND " + strIDs;
                    dv = new DataView(dtVehicles, strFilter, "VIN8", DataViewRowState.CurrentRows);

                    if (dv.Count > 0)
                    {
                        if (dv.Count == lsSelectedIDs.Count)
                        {
                            MessageBox.Show("All the rows you selected " +
                            " have Shipped dates.\n\nThe Exporter for these rows cannot be changed.",
                            "ALL SELECTED ROWS HAVE SHIPPED DATES", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        else
                        {
                            dlResult = MessageBox.Show(dv.Count.ToString() + " of the rows you selected " +
                            " have Shipped dates.\n\nThe Exporter for these rows cannot be changed.\n\n" +
                            "Do you want to ignore these rows and change the Forwarder on the remaining rows?",
                            "SOME ROWS SELECTED HAVE SHIPPED DATES", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);

                            if (dlResult == DialogResult.No) return;
                        }
                    }

                    //Create a Combobox to pass to frmSetSelection
                    cbo = new System.Windows.Forms.ComboBox();
                    foreach (DataRow dr in dtExporters.Rows)
                    {
                        cboitem = new ComboboxItem();
                        cboitem.cboText = dr["Exporter"].ToString();
                        cboitem.cboValue = dr["AEExporterID"].ToString();
                        cbo.Items.Add(cboitem);
                    }

                    //Open frmSetSelection in modal form, to get new Destination
                    frm = new frmSetSelection("Exporter", cbo);
                    dlResult = frm.ShowDialog();

                    if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                    {
                        //Replace VehID with AutoportExportVehiclesID for table UPDATE 
                        strIDs = strIDs.Replace("VehID", "AutoportExportVehiclesID");

                        //Update AutoportExportVehicles with new Forwarder
                        strSQL = "UPDATE AutoportExportVehicles SET ExporterID = " +
                             Globalitems.strSelection + ", UpdatedBy='" + Globalitems.strUserName + "'," +
                             "UpdatedDate = '" + DateTime.Now + "' " +
                             "WHERE DateShipped IS NULL AND " + strIDs + " AND CustomerID = " 
                             + intCustomerID + " AND FreightForwarderID = " + intForwarderID;
                        DataOps.PerformDBOperation(strSQL);

                        //Get the selected text
                        strFilter = "AEExporterID = " + Globalitems.strSelection;
                        dv = new DataView(dtExporters, strFilter, "AEExporterID", DataViewRowState.CurrentRows);

                        MessageBox.Show("The Exporter is changed to " + dv[0]["Exporter"],
                           "EXPORTER CHANGED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        PerformSearch();
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetForwarder", ex.Message);
            }
        }

        private void SetForwarder()
        {
            System.Windows.Forms.ComboBox cbo;
            ComboboxItem cboitem;
            DialogResult dlResult;
            DataSet ds;
            DataTable dtForwarders;
            DataView dv;
            frmSetSelection frm;
            int intCustomerID;
            List<int> lsSelectedIDs = GetListOfSelectedIDs();
            string strFilter;
            string strIDs;
            string strMessage;
            string strSQL;

            try
            {
                // Use frmAreYouSure to confirm changing Voyage
                strMessage = "Are you sure you want to update the Forwarder?\n\n" +
                    "The associated Exporter, if any, will be removed.";
                frmAreYouSure frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    //Place all the IDs in lsSelectedIDs into strIDs 
                    strIDs = "VehID IN (";

                    foreach (int intID in lsSelectedIDs) strIDs += intID.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    //Place the selected rows into dv
                    dv = new DataView(dtVehicles, strIDs, "VIN8", DataViewRowState.CurrentRows);

                    //Get the Distinct cutomers, to see if there are more than one
                    DataTable DistinctCustomers = dv.ToTable(true, "CustomerID");

                    //Only allow changing the Forwarder for one customer at a time
                    if (DistinctCustomers.Rows.Count > 1)
                    {
                        MessageBox.Show("The vehicles you selected are for more than one customer.\n\n" +
                            "Please select rows for only one customer to change the Forwarder.",
                            "CANNOT SET FORWARDER FOR MULTIPLE CUSTOMERS", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    //Store the single CustomerID
                    intCustomerID = (int)DistinctCustomers.Rows[0]["CustomerID"];

                    //Check if there is at least one Forwarder for the customer 
                    strSQL = "SELECT AEFreightForwarderID, " +
                    "CASE WHEN LEN(RTRIM(ISNULL(FreightForwarderShortName,''))) > 0 THEN " +
                        "FreightForwarderShortName " +
                        "ELSE FreightForwarderName " +
                    "END AS Forwarder " +
                    "FROM AEFreightForwarder " +
                    "WHERE AECustomerID = " + intCustomerID + " ";

                    //Add RecordStatus if ckActiv is checked
                    if (ckActive.Checked) strSQL += "AND RecordStatus='Active' ";
                    strSQL += "ORDER BY Forwarder";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "SetForwarder", "No table returned for Forwarders");
                        return;
                    }

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There are no Forwarders for this customer.\n\n" +
                            "You must first create a new Forwarder for this customer.",
                            "NO FORWARDERS FOR THIS CUSTOMER", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    dtForwarders = ds.Tables[0];

                    //Ck if any selected rows in dtVehicles have a DateShipped value
                    strFilter = "DateShipped IS NOT NULL AND " + strIDs;
                    dv = new DataView(dtVehicles, strFilter, "VIN8", DataViewRowState.CurrentRows);

                    if (dv.Count > 0)
                    {
                        if (dv.Count == lsSelectedIDs.Count)
                        {
                            MessageBox.Show("All the rows you selected " +
                            " have Shipped dates.\n\nThe Forwarder for these rows cannot be changed.",
                            "ALL SELECTED ROWS HAVE SHIPPED DATES", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        else
                        {
                            dlResult = MessageBox.Show(dv.Count.ToString() + " of the rows you selected " +
                            " have Shipped dates.\n\nThe Forwarder for these rows cannot be changed.\n\n" +
                            "Do you want to ignore these rows and change the Forwarder on the remaining rows?",
                            "SOME ROWS SELECTED HAVE SHIPPED DATES", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);

                            if (dlResult == DialogResult.No) return;
                        }
                    }

                    //Create a Combobox to pass to frmSetSelection
                    cbo = new System.Windows.Forms.ComboBox();
                    foreach (DataRow dr in dtForwarders.Rows)
                    {
                        cboitem = new ComboboxItem();
                        cboitem.cboText = dr["Forwarder"].ToString();
                        cboitem.cboValue = dr["AEFreightForwarderID"].ToString();
                        cbo.Items.Add(cboitem);
                    }

                    //Open frmSetSelection in modal form, to get new Destination
                    frm = new frmSetSelection("Forwarder", cbo);
                    dlResult = frm.ShowDialog();

                    if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                    {
                        //Replace VehID with AutoportExportVehiclesID for table UPDATE 
                        strIDs = strIDs.Replace("VehID", "AutoportExportVehiclesID");

                        //Update AutoportExportVehicles with new Forwarder
                        strSQL = "UPDATE AutoportExportVehicles SET FreightForwarderID = " +
                             Globalitems.strSelection + "," +
                             "ExporterID = NULL," +
                             "UpdatedBy='" + Globalitems.strUserName + "'," +
                             "UpdatedDate = '" + DateTime.Now + "' " +
                             "WHERE DateShipped IS NULL AND " + 
                             strIDs + " AND CustomerID = " + intCustomerID;
                        DataOps.PerformDBOperation(strSQL);

                        //Get the selected text
                        strFilter = "AEFreightForwarderID = " + Globalitems.strSelection;
                        dv = new DataView(dtForwarders, strFilter, "AEFreightForwarderID", DataViewRowState.CurrentRows);

                        MessageBox.Show("The Forwarder is changed to " + dv[0]["Forwarder"],
                           "FORWARDER CHANGED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        PerformSearch();
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetForwarder", ex.Message);
            }
        }

        private void SetVoyage()
        {

            try
            {
                System.Windows.Forms.ComboBox cbo;
                ComboboxItem cboitem;
                DialogResult dlResult;
                DataSet ds;
                DataTable dtVoyages;
                DataView dv;
                frmSetSelection frm;
                int intCustomerID;
                List<int> lsSelectedIDs = GetListOfSelectedIDs();
                string strFilter;
                string strIDs;
                string strMessage;
                string strSQL;

                // Use frmAreYouSure to confirm changing Voyage
                strMessage = "Are you sure you want to update the Voyage?";
                frmAreYouSure frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    //Place all the IDs in lsSelectedIDs into strIDs 
                    strIDs = "VehID IN (";

                    foreach (int intID in lsSelectedIDs) strIDs += intID.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    //Place the selected rows into dv
                    dv = new DataView(dtVehicles, strIDs, "VIN8", DataViewRowState.CurrentRows);

                    //Get the Distinct cutomers, to see if there are more than one
                    DataTable DistinctCustomers = dv.ToTable(true, "CustomerID");

                    //Only allow changing the voyage for one customer at a time
                    if (DistinctCustomers.Rows.Count > 1)
                    {
                        MessageBox.Show("The vehicles you selected are for more than one customer.\n\n" +
                            "Please select rows for only one customer to change the Voyage.",
                            "CANNOT SET VOYAGE FOR MULTIPLE CUSTOMERS",MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    //Store the single CustomerID
                    intCustomerID = (int) DistinctCustomers.Rows[0]["CustomerID"];

                    //Check if there is at least one new voyage for the customer >= Today
                    strSQL = "SELECT ISNULL(CONVERT(varchar(10),voy.VoyageDate,101),'') + ' - ' " +
                        "+ voy.VoyageNumber + ' - ' + ves.VesselName AS voyinfo, voy.AEVoyageID " +
                        "FROM AEVoyage voy " +
                        "LEFT JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID " +
                        "LEFT JOIN AEVoyageCustomer voycust ON voy.AEVoyageID = voycust.AEVoyageID " +
                        "WHERE voy.VoyageDate >= CONVERT(varchar(10), CURRENT_TIMESTAMP, 101) " +
                        "AND voycust.CustomerID = " + intCustomerID + " " +
                        "ORDER BY voy.VoyageDate DESC";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "SetVoyage", "No table returned for voyage dates");
                        return;
                    }

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There are no voyages scheduled today or later for this customer.\n\n" +
                            "You must first create a new voyage for this customer scheduled for today or later.",
                            "NO VOYAGES TODAY OR LATER FOR THIS CUSTOMER", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    dtVoyages = ds.Tables[0];

                    //Ck if any selected rows in dtVehicles have a DateShipped value
                    strFilter = "DateShipped IS NOT NULL AND " + strIDs;
                    dv = new DataView(dtVehicles, strFilter, "VIN8", DataViewRowState.CurrentRows);

                    if (dv.Count > 0)
                    {
                        if (dv.Count == lsSelectedIDs.Count)
                        {
                            MessageBox.Show("All the rows you selected " +
                            " have Shipped dates.\n\nThe Voyage for these rows cannot be changed.",
                            "ALL SELECTED ROWS HAVE SHIPPED DATES", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        else
                        {
                            dlResult = MessageBox.Show(dv.Count.ToString() + " of the rows you selected " +
                            " have Shipped dates.\n\nThe Voyage for these rows cannot be changed.\n\n" +
                            "Do you want to ignore these rows and change the Voyage on the remaining rows?",
                            "SOME ROWS SELECTED HAVE SHIPPED DATES", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);

                            if (dlResult == DialogResult.No) return;
                        }
                    }

                    //Create a Combobox to pass to frmSetSelection
                    cbo = new System.Windows.Forms.ComboBox();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            cboitem = new ComboboxItem();
                            cboitem.cboText = dr["voyinfo"].ToString();
                            cboitem.cboValue = dr["AEVoyageID"].ToString();
                            cbo.Items.Add(cboitem);
                        }

                    //Open frmSetSelection in modal form, to get new Destination
                    frm = new frmSetSelection("Voyage", cbo);
                    dlResult = frm.ShowDialog();

                    if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                    {
                        //Replace VehID with AutoportExportVehiclesID for table UPDATE 
                        strIDs = strIDs.Replace("VehID", "AutoportExportVehiclesID");

                        //Update AutoportExportVehicles with new VoyageID
                        strSQL = "UPDATE AutoportExportVehicles SET VoyageID = " +
                            Globalitems.strSelection + ", UpdatedBy='" + Globalitems.strUserName + "'," +
                            "UpdatedDate = '" + DateTime.Now + "' " +
                            "WHERE DateShipped IS NULL AND " + strIDs + " AND CustomerID = " + intCustomerID;
                        DataOps.PerformDBOperation(strSQL);

                        //Get the selected text
                        strFilter = "AEVoyageID = " + Globalitems.strSelection;
                        dv = new DataView(dtVoyages, strFilter, "AEVoyageID", DataViewRowState.CurrentRows);

                        MessageBox.Show("The Voyage is changed to " + dv[0]["voyinfo"],
                           "VOYAGE CHANGED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        PerformSearch();
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetVoyage", ex.Message);
            }
    }

        private void SetDestination()
        {
            try
            {
                bool blnResubmit = false;
                DataView dv;
                DialogResult dlResult;
                frmSetSelection frm;
                List<int> lsSelectedIDs = GetListOfSelectedIDs();
                string strFilter;
                string strIDs;
                string strMessage;
                string strRemaining = "";
                string strSQL;

                // Use frmAreYouSure to confirm changing Destination
                strMessage = "Are you sure you want to update the Destination?";
                frmAreYouSure frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    //Place all the IDs in lsSelectedIDs into strIDs 
                    strIDs = "VehID IN (";

                    foreach (int intID in lsSelectedIDs) strIDs += intID.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    //Ck if any selected rows in dtVehicles have a DateShipped value
                    strFilter = "DateShipped IS NOT NULL AND " + strIDs;
                    dv = new DataView(dtVehicles, strFilter, "VIN8", DataViewRowState.CurrentRows);

                    if (dv.Count > 0)
                    {
                        if (dv.Count == lsSelectedIDs.Count)
                        {
                            MessageBox.Show("All the rows you selected " +
                            " have Shipped dates.\n\nThe Destination for these rows cannot be changed.",
                            "ALL SELECTED ROWS HAVE SHIPPED DATES", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        else
                        {
                            dlResult = MessageBox.Show(dv.Count.ToString() + " of the rows you selected " +
                            " have Shipped dates.\n\nThe Destination for these rows cannot be changed.\n\n" +
                            "Do you want to ignore these rows and change the Destination on the remaining rows?",
                            "SOME ROWS SELECTED HAVE SHIPPED DATES", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);

                            if (dlResult == DialogResult.No) return;
                            strRemaining = "remaining";
                        }
                    }

                    //Ck if any rows with no DateShipped have DateApproved
                    strFilter = "DateShipped IS NULL AND CustomsApprovedDate IS NOT NULL AND " + strIDs;
                    dv = new DataView(dtVehicles, strFilter, "VIN8", DataViewRowState.CurrentRows);

                    if (dv.Count > 0)
                    {
                        if (dv.Count == lsSelectedIDs.Count)
                        {
                            dlResult = MessageBox.Show("All the " + strRemaining + " rows you selected " +
                             " have Cleared dates.\n\nPlease click:\nYes: to change the Destination, resubmit to Customs,\n" +
                             "  print new Labels & Clear sheets\n\n" +
                             "No: to just change the Destination (paperwork is correct)",
                             "ALL " + strRemaining + " ROWS HAVE CLEARED DATES", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Hand);
                        }
                        else
                        {
                            dlResult = MessageBox.Show("Some of the " + strRemaining + " rows you selected " +
                            " have Cleared dates.\n\nPlease click:\n\nYes: to change the Destination, resubmit to Customs,\n  print new Labels & Clear sheets for these rows.\n\n" +
                            "No: to just change the Destination for these rows (paperwork is correct)",
                            "SOME OF THE " + strRemaining + " ROWS HAVE CLEARED DATES", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Hand);
                        }

                        if (dlResult == DialogResult.Yes) blnResubmit = true;
                        if (dlResult == DialogResult.Cancel) return;
                    }

                    //Open frmSetSelection in modal form, to get new Destination
                    frm = new frmSetSelection("Destination", cboDest);
                    dlResult = frm.ShowDialog();

                    if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                    {
                        //Replace VehID with AutoportExportVehiclesID for table UPDATE 
                        strIDs = strIDs.Replace("VehID", "AutoportExportVehiclesID");

                        strSQL = "UPDATE AutoportExportVehicles SET DestinationName = '" +
                            Globalitems.strSelection + "', UpdatedBy='" + Globalitems.strUserName + "'," +
                            "UpdatedDate = '" + DateTime.Now + "' " +
                            "WHERE " + strIDs;
                        DataOps.PerformDBOperation(strSQL);

                        //If blnResubmit, update DateSubmittedCustoms to Now, CustomsApprovedDate to null,
                        //  VehicleStatus to SubmittedCustoms, CustomsCoverSheetPrintedInd to 0
                        if (blnResubmit)
                        {
                            strSQL = "UPDATE AutoportExportVehicles SET DateSubmittedCustoms = '" +
                                DateTime.Now + "', CustomsApprovedDate = null," +
                                "VehicleStatus='SubmittedCustoms',CustomsCoverSheetPrintedInd = 0, " +
                                "UpdatedBy='" + Globalitems.strUserName + "'," +
                                "UpdatedDate = '" + DateTime.Now + "' " +
                                "WHERE DateShipped IS NULL AND CustomsApprovedDate IS NOT NULL AND " + strIDs;
                            DataOps.PerformDBOperation(strSQL);

                            MessageBox.Show("The Destination is changed to " + Globalitems.strSelection +
                                "\n and the Status changed to SubmittedCustoms for those vehicles that " +
                                "had Cleared dates.\n\nPlease resubmit the paperwork to Customs\n" +
                                "and reprint labels & Cleared sheets.",
                           "DESTINATION CHANGED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("The Destination is changed to " + Globalitems.strSelection,
                             "DESTINATION CHANGED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        PerformSearch();
                    }
                }               
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetDestination", ex.Message);
            }
        }

        private void ContinueToDetailForm()
        {
            try
            {

            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ContinueToDetailForm", ex.Message);
            }
        }

        private void OpenVehicleDetailForm(string strFormMode)
        {
            frmVehDetail frm;

            try
            {
                //If frmVehDetail is already open set frm to it
                if (Application.OpenForms.OfType<frmVehDetail>().Count() == 0)
                {
                    frm = new frmVehDetail();
                    frm.strMode = strFormMode;
                    frm.frmSearch = this;
                    if (strFormMode == "READ" && bs1 != null) frm.bs1 = bs1;
                    Formops.OpenNewForm(frm);
                }
                else
                {
                    frm = (frmVehDetail)Application.OpenForms["frmVehDetail"];
                    frm.strMode = strFormMode;
                    if (strFormMode=="READ" && bs1 != null) frm.bs1 = bs1;
                    frm.Focus();
                }                
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenVehicleDetailForm", ex.Message);
            }
        }

        private void OpenFormToAddItem(string strForm)
        {
            frmCustomerAdmin frmCus;
            frmDestinationAdmin frmDest;
            frmExporterAdmin frmEx;
            frmFreightForwarderAdmin frmFF;
            frmVoyageAdmin frmVoy;

            try
            {               
                switch (strForm)
                {
                    case ("frmCustomerAdmin"):
                        //Open new form if not already open
                        if (Application.OpenForms.OfType<frmCustomerAdmin>().Count() == 0)
                        {
                            frmCus = new frmCustomerAdmin();
                            frmCus.blnNewCustomerRQFromOtherForm = true;
                            Formops.OpenNewForm(frmCus);
                        }
                        else
                        {
                            frmCus = (frmCustomerAdmin)Application.OpenForms["frmCustomerAdmin"];
                            frmCus.blnNewCustomerRQFromOtherForm = true;
                            frmCus.Focus();
                        }
                        
                        break;
                    case ("frmDestinationAdmin"):
                        //Open new form if not already open
                        if (Application.OpenForms.OfType<frmDestinationAdmin>().Count() == 0)
                        {
                            frmDest = new frmDestinationAdmin();
                            frmDest.blnNewDestinationRQFromOtherForm = true;
                            Formops.OpenNewForm(frmDest);
                        }
                        else
                        {
                            frmDest = (frmDestinationAdmin)Application.OpenForms["frmDestinationAdmin"];
                            frmDest.blnNewDestinationRQFromOtherForm = true;
                            frmDest.Focus();
                        }

                        break;
                    case ("frmExporterAdmin"):
                        //Open new form if not already open
                        if (Application.OpenForms.OfType<frmExporterAdmin>().Count() == 0)
                        {
                            frmEx = new frmExporterAdmin();
                            frmEx.blnNewExporterRQFromOtherForm = true;
                            Formops.OpenNewForm(frmEx);
                        }
                        else
                        {
                            frmEx = (frmExporterAdmin)Application.OpenForms["frmExporterAdmin"];
                            frmEx.blnNewExporterRQFromOtherForm = true;
                            frmEx.Focus();
                        }

                        break;
                    case ("frmFreightForwarderAdmin"):
                        //Open new form if not already open
                        if (Application.OpenForms.OfType<frmFreightForwarderAdmin>().Count() == 0)
                        {
                            frmFF = new frmFreightForwarderAdmin();
                            frmFF.blnNewForwarderRQFromOtherForm = true;
                            Formops.OpenNewForm(frmFF);
                        }
                        else
                        {
                            frmFF = (frmFreightForwarderAdmin)Application.OpenForms["frmFreightForwarderAdmin"];
                            frmFF.blnNewForwarderRQFromOtherForm = true;
                            frmFF.Focus();
                        }
                        
                        break;
                    case ("frmVoyageAdmin"):
                        //Open new form if not already open
                        if (Application.OpenForms.OfType<frmVoyageAdmin>().Count() == 0)
                        {
                            frmVoy = new frmVoyageAdmin();
                            frmVoy.blnNewVoyageRQFromOtherForm = true;
                            Formops.OpenNewForm(frmVoy);
                        }
                        else
                        {
                            frmVoy = (frmVoyageAdmin)Application.OpenForms["frmVoyageAdmin"];
                            frmVoy.blnNewVoyageRQFromOtherForm = true;
                            frmVoy.Focus();
                        }
                        break;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenFormtoAddItem", ex.Message);
            }
        }

        private void PerformContinue()
        {
            try
            {
                SetUpDetailBindingSource();
                OpenVehicleDetailForm("READ");
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformContinue", ex.Message);
            }
        }

        private void SetUpDetailBindingSource()
        {
            int intval;
            List<int> lsVehIDs = new List<int>();
            string strval;
            
            try
            {
                //Place all the Selected VehIDs, cell 0, of all selected rows into lsVehIDs
                //  Note: VS stores Selected rows in reverse order from how user selects them
                for (int row = dgResults.SelectedRows.Count-1; row > -1; row--)
                {
                    strval = dgResults.SelectedRows[row].Cells[0].EditedFormattedValue.ToString();
                    intval = Convert.ToInt32(strval);
                    lsVehIDs.Add(intval);
                }

                bs1.DataSource = lsVehIDs;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetUpDetailBindingSource", ex.Message);
            }
        }

        private void btnNewCustomer_Click(object sender, EventArgs e)
        {
            OpenFormToAddItem("frmCustomerAdmin");
        }

        private void btnCustCleared_Click(object sender, EventArgs e)
        {OpenPrintCustomClearedSheetsForm();}

        private void btnPrintLabels_Click(object sender, EventArgs e)
        {OpenPrintLabelsForm();}

        private void btnSetDest_Click(object sender, EventArgs e)
        {SetDestination();}

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }

        private void btnSetVoyage_Click(object sender, EventArgs e)
        {SetVoyage();}

        private void btnSetForwarder_Click(object sender, EventArgs e)
        {SetForwarder();}

        private void btnSetExporter_Click(object sender, EventArgs e)
        {SetExporter();}

        private void btnSetBooking_Click(object sender, EventArgs e)
        {SetBookingNumber();}

        private void btnSetCust_Click(object sender, EventArgs e)
        {SetCustomer();}

        private void btnNewDestination_Click(object sender, EventArgs e)
        { OpenFormToAddItem("frmDestinationAdmin"); }

        private void btnNewVoyage_Click(object sender, EventArgs e)
        {OpenFormToAddItem("frmVoyageAdmin");}

        private void btnNewForwarder_Click(object sender, EventArgs e)
        {OpenFormToAddItem("frmFreightForwarderAdmin");}

        private void btnNewExporter_Click(object sender, EventArgs e)
        { OpenFormToAddItem("frmExporterAdmin"); }

        private void cboVehStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strval;
            strval = (cboVehStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
            if (strval == "Shipped") ckBlankShippedDate.Checked = false;
        }

        private void ckBlankShippedDate_CheckedChanged(object sender, EventArgs e)
        {
            string strval;
            strval = (cboVehStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
            if (ckBlankShippedDate.Checked && strval == "Shipped")cboVehStatus.SelectedIndex = 0 ;
        }

        private void btnNoLabels_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To print labels, a label printer " +
                "\nmust be set as the default printer" + 
                "\n (E.g. Wasp or Zebra printer)", "LABEL PRINTER NOT THE DEFAULT",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNewrecord_Click(object sender, EventArgs e)
        {OpenVehicleDetailForm("NEW");}

        private void btnContinue_Click(object sender, EventArgs e)
        {PerformContinue();}

        private void dgResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //As long as row clicked is not the Column Header row, index = -1, enable btnContinue
            if (e.RowIndex > -1) btnContinue.Enabled = true;
        }

        private void txtVIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Click Search btn when User hits Enter button
            if (e.KeyChar == (char)13) btnSearch_Click(null, null);
        }

        private void dgResults_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // If User double clicks a row, assume Continue button pushed
            if (e.RowIndex > -1)
            {
                btnContinue_Click(null, null);
            }
        }

        private void ckActive_CheckedChanged(object sender, EventArgs e)
        {
            FillCombos();
            ClearForm();
        }

        private void btnSetBillTo_Click(object sender, EventArgs e)
        {SetBillTo();}

        private void frmVehSearch_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}

        private void frmVehSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseAdditionalCriteriaForm();
        }
    }
}
