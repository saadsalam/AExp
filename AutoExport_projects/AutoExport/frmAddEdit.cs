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
    public partial class frmAddEdit : Form
    {
        public bool blnAdditionalCriteria = false;  //Set by frmAddEditAddlCriteria
        public AddlCriteriaItem objAddlCriteria = new AddlCriteriaItem();

        //Object for code in frmAddEditAddlCriteria to set
        public class AddlCriteriaItem
        {
            private string mCustomsExDateFrom;
            private string mCustomsExDateTo;
            private string mInvDateFrom;
            private string mInvDateTo;
            private string mInvNumber;
            private string mMake;
            private bool mMechExceptions = false;
            private string mModel;
            private string mMultiVins;
            private bool mNonRunners = false;
            private string mPhysicalDateFrom;
            private string mPhysicalDateTo;
            private string mRcvdDateFrom;
            private string mRcvdDateTo;
            private string mShipDateFrom;
            private string mShipDateTo;
            private bool mSizeClass = false;
            private string mSubDateFrom;
            private string mSubDateTo;
            private string mYear;


            public string CustomsEXDateFrom
            {
                get { return mCustomsExDateFrom; }
                set { mCustomsExDateFrom = value; }
            }

            public string CustomsExDateTo
            {
                get { return mCustomsExDateTo; }
                set { mCustomsExDateTo = value; }
            }
            public string InvDateFrom
            {
                get { return mInvDateFrom; }
                set { mInvDateFrom = value; }
            }

            public string InvDateTo
            {
                get { return mInvDateTo; }
                set { mInvDateTo = value; }
            }

            public string InvNumber
            {
                get { return mInvNumber; }
                set { mInvNumber = value; }
            }

            public string Make
            {
                get { return mMake; }
                set { mMake = value; }
            }

            public bool MechExceptions
            {
                get { return mMechExceptions; }
                set { mMechExceptions = value; }
            }

            public string Model
            {
                get { return mModel; }
                set { mModel = value; }
            }

            public string MultiVins
            {
                get { return mMultiVins; }
                set { mMultiVins = value; }
            }

            public bool NonRunners
            {
                get { return mNonRunners; }
                set { mNonRunners = value; }
            }

            public string PhysicalDateFrom
            {
                get { return mPhysicalDateFrom; }
                set { mPhysicalDateFrom = value; }
            }

            public string PhysicalDateTo
            {
                get { return mPhysicalDateTo; }
                set { mPhysicalDateTo = value; }
            }

            public string RcvdDateFrom
            {
                get { return mRcvdDateFrom; }
                set { mRcvdDateFrom = value; }
            }

            public string RcvdDateTo
            {
                get { return mRcvdDateTo; }
                set { mRcvdDateTo = value; }
            }

            public string ShipDateFrom
            {
                get { return mShipDateFrom; }
                set { mShipDateFrom = value; }
            }

            public string ShipDateTo
            {
                get { return mShipDateTo; }
                set { mShipDateTo = value; }
            }

            public bool SizeClass
            {
                get { return mSizeClass; }
                set { mSizeClass = value; }
            }

            public string SubDateFrom
            {
                get { return mSubDateFrom; }
                set { mSubDateFrom = value; }
            }

            public string SubDateTo
            {
                get { return mSubDateTo; }
                set { mSubDateTo = value; }
            }

            public string Year
            { get { return mYear; }
            set { mYear = value; }
            }
        }

        private const string CURRENTMODULE = "frmAddEdit";

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
            new ControlInfo {ControlID="ckActive", ControlPropetyToBind="Checked"},
            new ControlInfo {ControlID="btnAddlCriteria"}
        };

        private BindingSource bs1 = new BindingSource();
        private DataTable dtVehicles = new DataTable();
        private List<int> lsCustomerIDs = new List<int>();
        private string strMode = "";

        public frmAddEdit()
        {
            InitializeComponent();
            dgResults.AutoGenerateColumns = false;
            strMode = "READ";

            FillCombos();
            if (Globalitems.blnException) return;

            Globalitems.SetControlHeight(this);
            if (Globalitems.blnException) return;

            Formops.SetTabIndex(this, lsControls);
            if (Globalitems.blnException) return;

            lblAddlCriteria.Visible = false;
        }

        private void FillCombos()

        {
            ComboboxItem cboitem;
            DataSet ds;
            string strFilter;
            string strSQL;

            try
            {
                //cboCust
                cboCust.Items.Clear();

                strSQL = "SELECT CustomerID, " +
                    "CASE WHEN LEN(RTRIM(ISNULL(ShortName,''))) > 0 THEN RTRIM(ShortName) " +
                    "else RTRIM(CustomerName) END AS CustName " +
                    "FROM Customer ";
                if (ckActive.Checked) strSQL += "WHERE RecordStatus='Active' ";
                strSQL += "ORDER BY CustName";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (Globalitems.blnException) return;

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
                    "FROM AEVessel ";
                if (ckActive.Checked) strSQL += "WHERE RecordStatus='Active' ";
                strSQL += "ORDER BY VesselName";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (Globalitems.blnException) return;

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
                strFilter = "CodeType='ExportDischargePort' AND Code <> '' ";
                if (ckActive.Checked) strFilter += "AND RecordStatus='Active'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboDest, true, false);

                //cboVoyageDate
                cboVoyageDate.Items.Clear();

                strSQL = "SELECT AEVoyageID, VoyageDate " +
                    "FROM AEVoyage WHERE VoyageDate IS NOT NULL ORDER BY VoyageDate DESC";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (Globalitems.blnException) return;

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
                strFilter = "CodeType='AutoportExportVehicleStatus' ";
                if (ckActive.Checked) strFilter += "AND RecordStatus='Active'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboVehStatus, true, false);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void ckActive_CheckedChanged(object sender, EventArgs e)
        {
            FillCombos();
        }

        private void cboCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem cboitem;
            DataSet ds;
            string strSQL;

            try
            {
                cboForwarder.Items.Clear();

                strSQL = "SELECT  AEFreightForwarderID, " +
                    "CASE WHEN LEN(RTRIM(ISNULL(FreightForwarderShortName,''))) > 0 THEN " +
                        "FreightForwarderShortName " +
                        "ELSE FreightForwarderName " +
                    "END AS Forwarder " +
                    "FROM AEFreightForwarder " +
                    "WHERE AECustomerID IS NOT NULL ";

                //Add CustomerID if cboCust != All
                if ((cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim() != "All")
                    strSQL += "AND AECustomerID = " + 
                        (cboCust.SelectedItem as ComboboxItem).cboValue.ToString() + " ";

                //Add RecordStatus if ckActiv is checked
                if (ckActive.Checked)
                    strSQL += "AND RecordStatus='Active' ";

                strSQL += "ORDER BY Forwarder";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (Globalitems.blnException) return;

                // Add All to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "All";
                cboitem.cboValue = "All";
                cboForwarder.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)                {
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
                Globalitems.HandleException(CURRENTMODULE, "cboCust_SelectedIndexChanged", ex.Message);
            }
        }

        private void cboForwarder_SelectedIndexChanged(object sender, EventArgs e)
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
                    "WHERE ExporterName IS NOT NULL ";

                //Add AEFreightForwarderID if cboForwarder != All
                if ((cboForwarder.SelectedItem as ComboboxItem).cboValue.ToString().Trim() != "All")
                    strSQL += "AND AEFreightForwarderID  = " +
                        (cboForwarder.SelectedItem as ComboboxItem).cboValue.ToString() + " ";

                //Add RecordStatus if ckActiv is checked
                if (ckActive.Checked)
                    strSQL += "AND RecordStatus='Active' ";

                strSQL += "ORDER BY Exporter";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (Globalitems.blnException) return;

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
                Globalitems.HandleException(CURRENTMODULE, "cboCust_SelectedIndexChanged", ex.Message);
            }
        }

        private void btnAddlCriteria_Click(object sender, EventArgs e)
        {
            frmAddEditAddlCriteria frm;

            //If frmAdd is already open, Activate it
            if (Application.OpenForms.OfType<frmAddEditAddlCriteria>().Count() == 1)
            {
                Formops.SetActiveForm("frmAddEditAddlCriteria");
            }
            else
            {
                //frm = new frmAddEditAddlCriteria(this);
                //frm.Show();
            }
        }

        private void PerformSearch()
        {
            DataSet ds;
            string strSQL;

            //1. Disable Export button
            btnExport.Enabled = false;

            //2. Clear Results gridview
            ClearGridView();
            if (Globalitems.blnException) return;

            //3. Rerieve data as datatable
            strSQL = "SELECT veh.AutoportExportVehiclesID," +
                "RIGHT(veh.VIN, 8)," +
                "veh.VIN," +
                "CASE " +
                    "WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName " +
                    "ELSE cus.CustomerName " +
                "END AS Customer," +
                "veh.DestinationName," +
                "veh.VehicleYear + ' ' + veh.Model," +
                "veh.Color," +
                "veh.BayLocation," +
                "veh.DateReceived," +
                "veh.DateSubmittedCustoms," +
                "veh.CustomsApprovedDate," +
                "veh.DateShipped," +
                "CASE " +
                    "WHEN DATALENGTH(ves.VesselShortName) > 0 THEN ves.VesselShortName " +
                    "ELSE ves.VesselName " +
                "END AS Vessel," +
                "veh.VehicleStatus," +
                "CASE " +
                    "WHEN DATALENGTH(ex.ExporterShortName) > 0 THEN ex.ExporterShortName " +
                    "ELSE ex.ExporterName " +
                "END AS Exporter," +
                "CASE " +
                    "WHEN DATALENGTH(ff.FreightForwarderShortName) > 0 THEN ff.FreightForwarderShortName " +
                    "ELSE ff.FreightForwarderName " +
                "END AS Forwarder," +
                "veh.BookingNumber," +
                "voy.VoyageDate," +
                "veh.SizeClass," +
                "veh.VIVTagNumber," +
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

           

        }

        private void ClearGridView()
        {

            try
            {
                lblVehRecords.Text = "";

                dtVehicles.Clear();
                dgResults.DataSource = dtVehicles;

                recbuttons.blnRecordsToDisplay = false;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearGridView", ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }
    }
}
