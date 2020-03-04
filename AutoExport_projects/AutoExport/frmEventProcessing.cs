using AutoExport.Objects;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmEventProcessing : Form
        //1/24/18 D.Maibor: add RestoreVehiclesToGrid method, and DataTable param to constructor
    {
        //CONSTANTS
        private const string CURRENTMODULE = "frmEventProcessing";
        private const string DESTBARCODEREPORT = "rptDestBarcodes.rdlc";


        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="cboVehStatus", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtStatusDate"},
            new ControlInfo {ControlID="cboCust", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="cboForwarder", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="cboExporter", ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtVIN", ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDestination"}
        };


        private DataTable dtResults = new DataTable();

        public frmEventProcessing(DataTable dtRestoreEvents = null)
        {
            //1/24/18 D.Maibor: Add optional dtRestoreEvents
            InitializeComponent();

            Formops.SetTabIndex(this, lsControls);
            FillCombos();
            ClearForm();

            if (dtRestoreEvents != null) RestoreVehiclesToGrid(dtRestoreEvents);
        }

        private string SQLToGetDataFromVehTable(string strVINs)
            //1/18/18 D.Maibor: return DateReceived, DateShipped, DateSubmittedCustomes, CustomsApprovedDate as just dates 
            //  to avoid time conflicts on same day comparisons
        {
            try
            {
                string strSQL = "";

                strSQL = @"SELECT
                veh.AutoportExportVehiclesID,
                veh.VIN, 
                veh.CustomerID,
                ISNULL(veh.Note,'') AS Note,
                CASE
                    WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName
                    ELSE cus.CustomerName
                END AS customer,
                veh.DestinationName,
                veh.VehicleStatus,
                veh.VehicleWeight, 
                veh.VehicleCubicFeet, 
                veh.SizeClass,
                CAST('1/1/1800' AS datetime) AS statusdate,
                CONVERT(date,ISNULL(DateReceived,'1/1/1800')) AS DateReceived,
                CONVERT(date,ISNULL(DateShipped,'1/1/1800')) AS DateShipped,
                CONVERT(date,ISNULL(DateSubmittedCustoms,'1/1/1800')) AS DateSubmittedCustoms,
                CONVERT(date,ISNULL(CustomsApprovedDate,'1/1/1800')) AS CustomsApprovedDate,
                ISNULL(veh.FreightForwarderID, -1) AS FreightForwarderID,
                 CASE
                    WHEN DATALENGTH(ff.FreightForwarderShortName) > 0 THEN
                        ff.FreightForwarderShortName
                    ELSE ISNULL(ff.FreightForwarderName, '')
                END AS forwarder,
                ISNULL(veh.ExporterID, -1) AS ExporterID,
                CASE
                    WHEN DATALENGTH(ex.ExporterShortName) > 0 THEN ex.ExporterShortName
                    ELSE ISNULL(ex.ExporterName, '')
                END AS exporter,
                veh.BookingNumber,
                'Update Pending' AS RecordStatus,
                CURRENT_TIMESTAMP AS statusdate,
                0 AS newForwarder,
                0 AS newExporter,
                CASE
                    WHEN LEN(RTRIM(ISNULL(veh.Note, ''))) = 0 THEN '' 
                    ELSE 'VIEW'
                END AS NoteToView
                FROM AutoportExportVehicles veh
                LEFT OUTER JOIN Customer cus ON veh.CustomerID = cus.CustomerID
                LEFT OUTER JOIN AEFreightForwarder ff on veh.FreightForwarderID = ff.AEFreightForwarderID
                LEFT OUTER JOIN AEExporter ex on veh.ExporterID = ex.AEExporterID 
                WHERE (veh.DateShipped IS NULL OR veh.VehicleStatus='ShippedByTruck') 
                    AND " + strVINs;

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLToGetDataFromVehTable", ex.Message);
                return "";
            }
        }

        public bool SaveVehiclesFromGrid()
        {
            try
            {
                DataTable dtEvents = CreateTableToSaveEvents();
                DateTime dtNow = DateTime.Now;
                DataRow dtRow;

                //Return if nothing to save
                if (dgResults.Rows.Count == 0) return false;

                //Save each Gridrow in dtEvents if RecordStatus is UpdatePending
                foreach (DataGridViewRow dgRow in dgResults.Rows)
                {
                    if (dgRow.Cells["RecordStatus"].Value.ToString().Contains("Pending"))
                    {
                            dtRow = dtEvents.NewRow();
                            dtRow["UserCode"] = Globalitems.strUserName;
                            dtRow["VIN"] = dgRow.Cells["VIN"].Value.ToString();
                            dtRow["StatusDate"] = dgRow.Cells["Received"].Value.ToString();
                            dtRow["CustomerID"] = dgRow.Cells["CustomerID"].Value.ToString();
                            dtRow["ForwarderID"] = dgRow.Cells["FreightForwarderID"].Value.ToString();

                            if (dgRow.Cells["ExporterID"].Value.ToString().Length > 0)
                                dtRow["ExporterID"] = dgRow.Cells["ExporterID"].Value.ToString();

                            if (dgRow.Cells["Destination"].Value.ToString().Length > 0)
                                dtRow["Destination"] = dgRow.Cells["Destination"].Value.ToString();

                            dtRow["VehicleStatus"] = dgRow.Cells["VehicleStatus"].Value.ToString();
                            dtRow["RecordStatus"] = dgRow.Cells["RecordStatus"].Value.ToString();

                            if (dgRow.Cells["FullNote"].Value.ToString().Length > 0)
                                dtRow["Note"] = dgRow.Cells["FullNote"].Value.ToString();

                            dtRow["CreationDate"] = dtNow;

                            dtEvents.Rows.Add(dtRow);
                        }  
                }

                //Peformbulkcopy to insert rows into EventProcessingRecoveryData, if any rows to save
                if (dtEvents.Rows.Count > 0)
                {
                    DataOps.PerformBulkCopy("EventProcessingRecoveryData", dtEvents);
                    return true;
                }

                return false;

            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SaveVehiclesFromGrid", ex.Message);
                return false;
            }
        }

        private DataTable CreateTableToSaveEvents()
        {
            DataColumn dtcol;
            DataTable dtTable;
            try
            {
                dtTable = new DataTable();

                dtcol = new DataColumn("UserCode");
                dtcol.DataType = System.Type.GetType("System.String");
                dtTable.Columns.Add(dtcol);

                dtcol = new DataColumn("VIN");
                dtcol.DataType = System.Type.GetType("System.String");
                dtTable.Columns.Add(dtcol);

                dtcol = new DataColumn("StatusDate");
                dtcol.DataType = System.Type.GetType("System.String");
                dtTable.Columns.Add(dtcol);

                dtcol = new DataColumn("CustomerID");
                dtcol.DataType = System.Type.GetType("System.Int32");
                dtTable.Columns.Add(dtcol);

                dtcol = new DataColumn("ForwarderID");
                dtcol.DataType = System.Type.GetType("System.Int32");
                dtTable.Columns.Add(dtcol);

                dtcol = new DataColumn("ExporterID");
                dtcol.DataType = System.Type.GetType("System.Int32");
                dtTable.Columns.Add(dtcol);

                dtcol = new DataColumn("Destination");
                dtcol.DataType = System.Type.GetType("System.String");
                dtTable.Columns.Add(dtcol);

                dtcol = new DataColumn("VehicleStatus");
                dtcol.DataType = System.Type.GetType("System.String");
                dtTable.Columns.Add(dtcol);

                dtcol = new DataColumn("RecordStatus");
                dtcol.DataType = System.Type.GetType("System.String");
                dtTable.Columns.Add(dtcol);

                dtcol = new DataColumn("Note");
                dtcol.DataType = System.Type.GetType("System.String");
                dtTable.Columns.Add(dtcol);

                dtcol = new DataColumn("CreationDate");
                dtcol.DataType = System.Type.GetType("System.String");
                dtTable.Columns.Add(dtcol);

                return dtTable;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CreateEventTable", ex.Message);
                return null;
            }

        }

        private void RestoreVehiclesToGrid(DataTable dtEventsToRestore)
        {
            try
            {
                DateTime dat;
                string strNote;

               //Process each row in dtEventsToRestore as though manually entered, to ensure data is still valid
               foreach (DataRow dtRow in dtEventsToRestore.Rows)
                {
                    //Set VIN
                    txtVIN.Text = dtRow["VIN"].ToString();

                    //Set Veh. Status
                    foreach (ComboboxItem cboItem in cboVehStatus.Items)
                        if (cboItem.cboValue == dtRow["VehicleStatus"].ToString()) cboVehStatus.SelectedItem = cboItem;

                    //Set Status date
                    dat = (DateTime) dtRow["StatusDate"];
                    txtStatusDate.Text = dat.ToString("M/d/yyyy");

                    //Set cboCust
                    foreach (ComboboxItem cboItem in cboCust.Items)
                        if (cboItem.cboValue == dtRow["CustomerID"].ToString()) cboCust.SelectedItem = cboItem;


                    //Set Freight Forwarder
                    foreach (ComboboxItem cboItem in cboForwarder.Items)
                        if (cboItem.cboValue == dtRow["ForwarderID"].ToString()) cboForwarder.SelectedItem = cboItem;

                    //Set Freight Exporter, if not null
                    cboExporter.SelectedIndex = 0;
                    if (dtRow["ExporterID"] != null && (int) dtRow["ExporterID"] != -1)
                    {
                        foreach (ComboboxItem cboItem in cboExporter.Items)
                            if (cboItem.cboValue == dtRow["ExporterID"].ToString()) cboExporter.SelectedItem = cboItem;
                    }

                    //Store Note
                    strNote = "";
                    if (dtRow["Note"] != null) strNote = dtRow["Note"].ToString();


                    //Set Destination
                    txtDestination.Text = "";
                    if (dtRow["VehicleStatus"].ToString().Contains("Cleared"))
                        txtDestination.Text = dtRow["Destination"].ToString();

                    AddVehicleToGrid(strNote);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "RestoreVehiclesToGrid",ex.Message);
            }
        }

        private void AddVehicleToGrid(string strFullnote = "")
        //1/24/18 D.Maibor: add optional parm to indicate if Restoring saved veh
        //1/18/18 D.Maibor: replace 11:59 PM on Status date as Status date + current time. 11:59 PM makes DateSubmitted, CustomsApprovedDate look strange and 
        // throws off DateShipped times due to SProc spUpdateAutoportExportDateShipped
        {
            try
            {
                DialogResult dlResult;
                DataSet ds;
                DataTable dtVINdatainVehTable;
                DataTable dtVINdataToAdd;
                DataRow dtRow;
                frmSetSelection frm;
                string strCurrentTime = DateTime.Now.ToString("hh:mm tt");
                string strMsg;
                string strSQL;
                string strVIN;
                string strval;

                if (txtVIN.Text.Trim().Length == 17)
                    strVIN = " veh.VIN = '" + txtVIN.Text.Trim() + "' ";
                else
                    strVIN = "veh.VIN LIKE '%" + txtVIN.Text.Trim() + "' ";

                strSQL = SQLToGetDataFromVehTable(strVIN);
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "AddVehicleToGrid",
                        "No table returned from query");
                    return;
                }

                dtVINdatainVehTable = ds.Tables[0];

                //Create dtVINdataToAdd from form data & veh data
                dtVINdataToAdd = dtVINdatainVehTable.Clone();
                dtRow = dtVINdataToAdd.NewRow();

                //Form data to use
                dtRow["VehicleStatus"] = (cboVehStatus.SelectedItem as ComboboxItem).cboValue;

                //D.Maibor:Replace 11:59 PM of status date to current time; 11:59 PM throws off DateShipped time on Shipday
                //Use 11:59 PM of day so no conflict rcv'ng & submitting to customs on same day
                //dtRow["statusdate"] = txtStatusDate.Text + " 11:59 PM";
                dtRow["statusdate"] = txtStatusDate.Text + " " + strCurrentTime;

                dtRow["CustomerID"] = (cboCust.SelectedItem as ComboboxItem).cboValue;
                dtRow["customer"] = (cboCust.SelectedItem as ComboboxItem).cboText;
                dtRow["FreightForwarderID"] = (cboForwarder.SelectedItem as ComboboxItem).cboValue;
                dtRow["forwarder"] = (cboForwarder.SelectedItem as ComboboxItem).cboText;

                dtRow["RecordStatus"] = "UpdatePending";

                if (dtRow["VehicleStatus"].ToString().Contains("Cleared"))
                    dtRow["DestinationName"] = txtDestination.Text.Trim();
                

                if ((cboExporter.SelectedItem as ComboboxItem).cboValue != "select")
                {
                    dtRow["ExporterID"] =
                        Convert.ToInt32((cboExporter.SelectedItem as ComboboxItem).cboValue);
                    dtRow["exporter"] = (cboExporter.SelectedItem as ComboboxItem).cboText;
                }
                else
                {
                    dtRow["ExporterID"] = -1;
                    dtRow["exporter"] = "";
                }

                dtRow["VIN"] = txtVIN.Text.Trim();
                if (dtRow["VehicleStatus"].ToString().Contains("Cleared"))
                    dtRow["DestinationName"] = txtDestination.Text.Trim();

                //Veh data to use (Booking#, Weight, Cubic Ft,Note,View)
                if (dtVINdatainVehTable.Rows.Count > 0)
                {
                    dtRow["AutoportExportVehiclesID"] =
                        dtVINdatainVehTable.Rows[0]["AutoportExportVehiclesID"];
                    dtRow["BookingNumber"] = dtVINdatainVehTable.Rows[0]["BookingNumber"];
                    dtRow["VehicleWeight"] = dtVINdatainVehTable.Rows[0]["VehicleWeight"];
                    dtRow["Note"] = dtVINdatainVehTable.Rows[0]["Note"];
                    dtRow["NoteToView"] = dtVINdatainVehTable.Rows[0]["NoteToView"];
                    
                    //Remove Veh Cub Feet decimal point &  trailing 0's
                    strval = dtVINdatainVehTable.Rows[0]["VehicleCubicFeet"].ToString();
                    if (strval.Contains(".")) strval = strval.Remove(strval.IndexOf("."));
                    dtRow["VehicleCubicFeet"] = strval;
                    dtRow["SizeClass"] = dtVINdatainVehTable.Rows[0]["SizeClass"];

                    //Add Destination if Status <> Cleared
                    if (!dtRow["VehicleStatus"].ToString().Contains("Cleared"))
                        dtRow["DestinationName"] = dtVINdatainVehTable.Rows[0]["DestinationName"];
                }

                dtRow["newForwarder"] = 0;
                dtRow["newExporter"] = 0;

                dtVINdataToAdd.Rows.Add(dtRow);

                if (ValidVINInfo(dtVINdataToAdd, dtVINdatainVehTable))
                {
                    //Create dtResults if it doesn't exist
                    if (dtResults.Columns.Count == 0) CreateTableFordgResults();

                    //Open frmSetSelection to display any existing Note info &
                    // ensure User has Note info
                    if ((cboVehStatus.SelectedItem as ComboboxItem).cboValue.Contains("Excep"))
                    {
                        //Ck if strFullNote already has a value, becuase it is being restored
                        if (strFullnote.Length > 0)
                            dtRow["Note"] = strFullnote;
                        else
                        {
                            if ((cboVehStatus.SelectedItem as ComboboxItem).cboValue.Contains("Customs"))
                                strMsg = "Customs Exception Note";
                            else
                                strMsg = "Received Exception Note";

                            frm = new frmSetSelection(strMsg, null, "", false, true);
                            frm.strNoteInDB = dtVINdatainVehTable.Rows[0]["Note"].ToString();
                            dlResult = frm.ShowDialog();
                            if (dlResult == DialogResult.Cancel) return;

                            //Set Note to returned text in strSelection
                            dtRow["Note"] = Globalitems.strSelection;
                        }

                        dtRow["NoteToView"] = "VIEW";
                    }

                    //Create a new row for dtResults & add field values from dtVINToAdd
                    dtRow = dtResults.NewRow();
                    foreach (DataColumn dtCol in dtResults.Columns)
                        dtRow[dtCol.ColumnName] = dtVINdataToAdd.Rows[0][dtCol.ColumnName];

                    dtResults.Rows.Add(dtRow);
                    dgResults.DataSource = dtResults;

                    txtVIN.Text = "";
                    txtVIN.Focus();
                    txtDestination.Text = "";

                    btnRemove.Enabled = true;
                    btnExport.Enabled = true;
                    btnClear.Enabled = true;
                    btnSetBooking.Enabled = true;
                    btnSetForwarder.Enabled = true;
                    btnSetExporter.Enabled = true;
                    btnProcess.Enabled = true;
                    ckAllrows.Enabled = true;

                    lblVehRecords.Visible = true;
                    lblVehRecords.Text = "Records: " + dgResults.Rows.Count.ToString("#,##0");

                    //Scroll to bottom of dgResults and unselect all rows
                    dgResults.Rows[dgResults.RowCount - 1].Selected = false;
                    dgResults.FirstDisplayedScrollingRowIndex = dgResults.RowCount - 1;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "AddVehicletoGrid", ex.Message);
            }
        }

        private bool AllDataRQdToCheckVIN()
        {
            try
            {
                //Ck Veh. Status
                if ((cboVehStatus.SelectedItem as ComboboxItem).cboValue == "select") return false;

                //Ck Status Date
                if (txtStatusDate.Text.Trim().Length == 0) return false;

                //Ck Customer
                if ((cboCust.SelectedItem as ComboboxItem).cboValue == "select") return false;

                //Ck Forwarder
                if ((cboForwarder.SelectedItem as ComboboxItem).cboValue == "select") return false;

                //Ck VIN
                if (txtVIN.Text.Trim().Length == 0) return false;

                //Ck Destination if 'ClearedCustoms'
                if ((cboVehStatus.SelectedItem as ComboboxItem).cboValue == "ClearedCustoms")
                {
                    if (txtDestination.Text.Trim().Length == 0) return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "AllDataRQdToCheckVIN", ex.Message);
                return false;
            }
        }

        private class VINitem
        {
            private string mVIN;
            private int mCustomerID;
            private int mForwarderID;
            private int mExporterID;
            private string mCustomer;
            private string mCustomer_dg;
            private string mCustomer_veh;
            private DateTime mDateReceived;
            private DateTime mDateSubCustoms;
            private string mDestination;
            private string mDestination_dg;
            private string mDestination_veh;
            private string mForwarder;
            private string mForwarder_dg;
            private string mForwarder_veh;
            private string mExporter;
            private string mExporter_dg;
            private string mExporter_veh;
            private int mnewExporter = 0;
            private int mnewForwarder = 0;
            private string mVehicleStatus;
            private DateTime mStatusDate;

            public string Customer
            {
                get { return mCustomer; }
                set { mCustomer = value; }
            }

            public string Customer_dg
            {
                get { return mCustomer_dg; }
                set { mCustomer_dg = value; }
            }

            public string Customer_veh
            {
                get { return mCustomer_veh; }
                set { mCustomer_veh = value; }
            }

            public int CustomerID
            {
                get { return mCustomerID; }
                set { mCustomerID = value; }
            }

            public DateTime DateReceived
            {
                get { return mDateReceived; }
                set { mDateReceived = value; }
            }

            public DateTime DateSubCustoms
            {
                get { return mDateSubCustoms; }
                set { mDateSubCustoms = value; }
            }

            public string Destination
            {
                get { return mDestination; }
                set { mDestination = value; }
            }

            public string Destination_dg
            {
                get { return mDestination_dg; }
                set { mDestination_dg = value; }
            }

            public string Destination_veh
            {
                get { return mDestination_veh; }
                set { mDestination_veh = value; }
            }

            public string Forwarder
            {
                get { return mForwarder; }
                set { mForwarder = value; }
            }

            public string Forwarder_dg
            {
                get { return mForwarder_dg; }
                set { mForwarder_dg = value; }
            }

            public string Forwarder_veh
            {
                get { return mForwarder_veh; }
                set { mForwarder_veh = value; }
            }

            public int ForwarderID
            {
                get { return mForwarderID; }
                set { mForwarderID = value; }
            }

            public string Exporter
            {
                get { return mExporter; }
                set { mExporter = value; }
            }

            public string Exporter_dg
            {
                get { return mExporter_dg; }
                set { mExporter_dg = value; }
            }

            public string Exporter_veh
            {
                get { return mExporter_veh; }
                set { mExporter_veh = value; }
            }

            public int ExporterID
            {
                get { return mExporterID; }
                set { mExporterID = value; }
            }

            public int newExporter
            {
                get { return mnewExporter; }
                set { mnewExporter = value; }
            }

            public int newForwarder
            {
                get { return mnewForwarder; }
                set { mnewForwarder = value; }
            }

            public DateTime StatusDate
            {
                get { return mStatusDate; }
                set { mStatusDate = value; }
            }

            public string VehicleStatus
            {
                get { return mVehicleStatus; }
                set { mVehicleStatus = value; }
            }

            public string VIN
            {
                get { return mVIN; }
                set { mVIN = value; }
            }
        }

        private bool ValidVINInfo(DataTable dtGridtable, DataTable dtVehtable)
        {
            //Peform checks both when Adding a single VIN to dgResults, or Processing rows
            //  in dgResults to update the Veh table
            //dtGridtable contains the rec to be added or already in dgResults
            //dtVehtable contains the corresponding recs in the Veh table where DateShipped IS NULL
            try
            {
                DialogResult dlResult;
                frmAreYouSure frm;
                List<VINitem> lsVINs_in_gridtable;
                List<VINitem> lsVINs_in_vehtable;
                List<VINitem> lsProblemVINs;
                string strMsg;
                string strVINs = "";

                //To make Linq queries easier load each input table into lists 
                //Load lsVINs_in_gridtable
                lsVINs_in_gridtable = dtGridtable.AsEnumerable()
                    .Select(row => new VINitem
                    {
                        VIN = row.Field<string>("VIN"),
                        Customer = row.Field<string>("customer"),
                        Customer_dg = row.Field<string>("customer"),
                        CustomerID = row.Field<int>("CustomerID"),
                        Forwarder = row.Field<string>("forwarder"),
                        Forwarder_dg = row.Field<string>("forwarder"),
                        ForwarderID = row.Field<int>("FreightForwarderID"),
                        Exporter = row.Field<string>("exporter"),
                        Exporter_dg = row.Field<string>("exporter"),
                        ExporterID = row.Field<int>("ExporterID"),
                        VehicleStatus = row.Field<string>("VehicleStatus"),
                        StatusDate = row.Field<DateTime>("StatusDate"),
                        Destination = row.Field<string>("DestinationName"),
                        Destination_dg = row.Field<string>("DestinationName"),
                        newExporter = row.Field<int>("newExporter"),
                        newForwarder = row.Field<int>("newForwarder")
                    }).ToList();

                //Load lsVINs_in_vehtable
                lsVINs_in_vehtable = dtVehtable.AsEnumerable()
                     .Select(row => new VINitem
                     {
                         VIN = row.Field<string>("VIN"),
                         Customer = row.Field<string>("customer"),
                         Customer_veh = row.Field<string>("customer"),
                         CustomerID = row.Field<int>("CustomerID"),
                         Forwarder = row.Field<string>("forwarder"),
                         Forwarder_veh = row.Field<string>("forwarder"),
                         ForwarderID = row.Field<int>("FreightForwarderID"),
                         Exporter = row.Field<string>("exporter"),
                         Exporter_veh = row.Field<string>("exporter"),
                         ExporterID = row.Field<int>("ExporterID"),
                         VehicleStatus = row.Field<string>("VehicleStatus"),
                         DateReceived = row.Field<DateTime>("DateReceived"),
                         DateSubCustoms = row.Field<DateTime>("DateSubmittedCustoms"),
                         Destination = row.Field<string>("DestinationName"),
                         Destination_veh = row.Field<string>("DestinationName"),
                     }).ToList();

                //Ck if VIN is NOT IN Veh table
                lsProblemVINs = lsVINs_in_gridtable.Where(vin_dg =>
                  lsVINs_in_vehtable.All(vin_veh => vin_veh.VIN != vin_dg.VIN)).ToList<VINitem>();

                if (lsProblemVINs.Count > 0)
                {
                    strMsg = "There are no unshipped VINs for:";
                    foreach (VINitem objVINitem in lsProblemVINs)
                        strMsg += "\n" + objVINitem.VIN;
                    strMsg += "\nPlease correct.";

                    MessageBox.Show(strMsg, "OPEN VINS NOT FOUND", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    txtVIN.Text = "";
                    txtDestination.Text = "";
                    txtVIN.Focus();

                    return false;
                }

                //Ck if Multiple recs w/same VIN in Veh table
                var qry = from vin in lsVINs_in_vehtable
                          group vin by vin.VIN into grp
                          let count = grp.Count()
                          where grp.Count() > 1
                          select new { Value = grp.Key, Count = count };
                strVINs = "";
                foreach (var vin in qry)
                    strVINs += "\n" + vin.Value;

                if (strVINs.Length > 0)
                {
                    strMsg = "The following open VINs have multiple matches:" + strVINs;
                    strMsg += "\n\nPlease correct.";
                    MessageBox.Show(strMsg, "MULTIPLE VINS FOUND", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    txtVIN.Text = "";
                    txtDestination.Text = "";
                    return false;
                }

                //There is a one-to-one match for each VIN in the two tables

                //Ck for Customer Mismatch
                var mismatch_cus = from vindg in lsVINs_in_gridtable
                                   join vinveh in lsVINs_in_vehtable
                                   on vindg.VIN equals vinveh.VIN
                                   where vindg.CustomerID != vinveh.CustomerID
                                   select new VINitem
                                   {
                                       VIN = vindg.VIN,
                                       Customer_dg = vindg.Customer_dg,
                                       Customer_veh = vinveh.Customer_veh
                                   };

                foreach (VINitem vitem in mismatch_cus)
                    lsProblemVINs.Add(vitem);
                if (lsProblemVINs.Count > 0)
                {
                    foreach (VINitem vitem in lsProblemVINs)
                        strVINs += "\n" + vitem.VIN + ", Cust (selected): " +
                            vitem.Customer_dg + ", Cust (recorded): " +
                            vitem.Customer_veh;

                    strMsg = "The following VINs have Customer mismatches " + strVINs;
                    strMsg += "\n\nPlease correct.";
                    MessageBox.Show(strMsg, "VINS WITH CUSTOMER MISMATCHES", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    txtVIN.Text = "";
                    txtDestination.Text = "";
                    return false;
                }

                //Ck for Forwarder Mismatch
                var mismatch_ff = from vindg in lsVINs_in_gridtable
                                  join vinveh in lsVINs_in_vehtable
                                  on vindg.VIN equals vinveh.VIN
                                  where vindg.newForwarder == 0 &&
                                  vindg.ForwarderID != vinveh.ForwarderID &&
                                  vinveh.ForwarderID > 0
                                  select new VINitem
                                  {
                                      VIN = vindg.VIN,
                                      Forwarder_dg = vindg.Forwarder_dg,
                                      Forwarder_veh = vinveh.Forwarder_veh
                                  };

                foreach (VINitem vitem in mismatch_ff)
                    lsProblemVINs.Add(vitem);
                if (lsProblemVINs.Count > 0)
                {
                    foreach (VINitem vitem in lsProblemVINs)
                        strVINs += "\n" + vitem.VIN + ", Fwdr: (selected): " +
                            vitem.Forwarder_dg + ", Fwdr (recorded): " +
                            vitem.Forwarder_veh;

                    strMsg = "The following VINs have Forwarder mismatches " + strVINs;
                    strMsg += "\n\nPlease correct.";
                    MessageBox.Show(strMsg, "VINS WITH FORWARDER MISMATCHES", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    txtVIN.Text = "";
                    txtDestination.Text = "";
                    return false;
                }

                //Ck for Exporter Mismatch
                var mismatch_ex = from vindg in lsVINs_in_gridtable
                                  join vinveh in lsVINs_in_vehtable
                                  on vindg.VIN equals vinveh.VIN
                                  where vindg.newExporter == 0 &&
                                  vindg.ExporterID != vinveh.ExporterID &&
                                  vinveh.ExporterID > 0
                                  select new VINitem
                                  {
                                      VIN = vindg.VIN,
                                      Exporter_dg = vindg.Exporter_dg,
                                      Exporter_veh = vinveh.Exporter_veh
                                  };

                foreach (VINitem vitem in mismatch_ex)
                    lsProblemVINs.Add(vitem);
                if (lsProblemVINs.Count > 0)
                {
                    foreach (VINitem vitem in lsProblemVINs)
                        strVINs += "\n" + vitem.VIN + ", Exp (selected): " +
                            vitem.Exporter_dg + ", Exp (recorded): " +
                            vitem.Exporter_veh;

                    strMsg = "The following VINs have Exporter mismatches " + strVINs;
                    strMsg += "\n\nPlease correct.";
                    MessageBox.Show(strMsg, "VINS WITH EXPORTER MISMATCHES", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    txtVIN.Text = "";
                    txtDestination.Text = "";
                    return false;
                }

                //Ck for Destination Mismatch if ClearedCustoms status
                var mismatch_dest = from vindg in lsVINs_in_gridtable
                                    join vinveh in lsVINs_in_vehtable
                                    on vindg.VIN equals vinveh.VIN
                                    where vindg.VehicleStatus == "ClearedCustoms" &&
                                    vindg.Destination.ToUpper() != vinveh.Destination.ToUpper()
                                    select new VINitem
                                    {
                                        VIN = vindg.VIN,
                                        Destination_dg = vindg.Destination_dg,
                                        Destination_veh = vinveh.Destination_veh
                                    };

                foreach (VINitem vitem in mismatch_dest)
                    lsProblemVINs.Add(vitem);
                if (lsProblemVINs.Count > 0)
                {
                    foreach (VINitem vitem in lsProblemVINs)
                        strVINs += "\n" + vitem.VIN + ", Dest. (entered): " +
                            vitem.Destination_dg + ", Dest. (recorded): " +
                            vitem.Destination_veh;

                    strMsg = "The following VINs have Destination mismatches " + strVINs;
                    strMsg += "\n\nPlease correct.";
                    MessageBox.Show(strMsg, "VINS WITH DESTINATION MISMATCHES", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    txtDestination.Focus();
                    txtDestination.SelectAll();
                    return false;
                }

                //Ck if Veh table missing Date Received
                var missingDateReceived = from vinveh in lsVINs_in_vehtable
                    where vinveh.DateReceived == Globalitems.NULLDATE
                    select new VINitem
                    { VIN = vinveh.VIN };

                foreach (VINitem vitem in missingDateReceived)
                    lsProblemVINs.Add(vitem);
                if (lsProblemVINs.Count > 0)
                {
                    foreach (VINitem vitem in lsProblemVINs)
                        strVINs += "\n" + vitem.VIN;

                    strMsg = "The following VINs have no Date Received " + strVINs;
                    strMsg += "\n\nPlease correct.";
                    MessageBox.Show(strMsg, "VINS MISSING DATE RECEIVED", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    txtVIN.Text = "";
                    txtDestination.Text = "";
                    return false;
                }

                //Ck if Veh. table Date Received is BEFORE status date 
                var incorrectStatusDate = from vindg in lsVINs_in_gridtable
                                          join vinveh in lsVINs_in_vehtable
                                          on vindg.VIN equals vinveh.VIN
                                          where vindg.StatusDate < vinveh.DateReceived
                                          select new VINitem
                                          {
                                              VIN = vindg.VIN,
                                              StatusDate = vindg.StatusDate,
                                              DateReceived = vinveh.DateReceived
                                          };

                foreach (VINitem vitem in incorrectStatusDate)
                    lsProblemVINs.Add(vitem);
                if (lsProblemVINs.Count > 0)
                {
                    foreach (VINitem vitem in lsProblemVINs)
                        strVINs += "\n" + vitem.VIN + ", Status Date: " +
                            vitem.StatusDate.ToString("M/d/yy H:mm tt") + ", Date Rcvd: " +
                            vitem.DateReceived.ToString("M/d/yy H:mm tt");

                    strMsg = "The following VINs have Status Dates BEFORE Date Received " + strVINs;
                    strMsg += "\n\nPlease correct.";
                    MessageBox.Show(strMsg, "VINS WITH INCORRECT STATUS DATES", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    txtVIN.Text = "";
                    txtDestination.Text = "";
                    return false;
                }

                //Ck if veh status is ClearedCustoms or CustomsException and 
                // DateSubCustoms is NULL
                var incorrectStatusChange = from vindg in lsVINs_in_gridtable
                                            join vinveh in lsVINs_in_vehtable
                                            on vindg.VIN equals vinveh.VIN
                                            where (vindg.VehicleStatus == "ClearedCustoms" ||
                                            vindg.VehicleStatus == "CustomsException") &&
                                            vinveh.DateSubCustoms == Globalitems.NULLDATE
                                            select new VINitem
                                            { VIN = vindg.VIN };

                foreach (VINitem vitem in incorrectStatusChange)
                    lsProblemVINs.Add(vitem);
                if (lsProblemVINs.Count > 0)
                {
                    foreach (VINitem vitem in lsProblemVINs)
                        strVINs += "\n" + vitem.VIN;

                    strMsg = "The following VINs cannot change the status to " +
                        "'Cleared Customs' or 'Customs Exception' " +
                        "because they have no Date Submitted to Customs" + strVINs;
                    strMsg += "\n\nPlease correct.";
                    MessageBox.Show(strMsg, "VINS MISSING DATE SUBMITTED TO CUSTOMS", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    txtVIN.Text = "";
                    txtDestination.Text = "";
                    return false;
                }

                //Ck if veh status is HoldCustomerRequest,HoldMechanical, or ShippedByTruck
                var verifyStatusChange = from  vinveh in lsVINs_in_vehtable
                                            where (vinveh.VehicleStatus == "HoldCustomerRequest" ||
                                            vinveh.VehicleStatus == "HoldMechanical" || 
                                            vinveh.VehicleStatus == "ShippedByTruck") 
                                            select new VINitem
                                            { VIN = vinveh.VIN,
                                            VehicleStatus = vinveh.VehicleStatus};

                foreach (VINitem vitem in verifyStatusChange)
                    lsProblemVINs.Add(vitem);

                if (lsProblemVINs.Count > 0)
                {
                    foreach (VINitem vitem in lsProblemVINs)
                        strVINs += "\n" + vitem.VIN + " - " + vitem.VehicleStatus;

                    strMsg = "The following VINs have a \n'HOLD' or 'SHIPPED BY TRUCK' status:" +
                       strVINs;
                    strMsg += "\n\nAre you sure you want to change the status?";
                    frm = new frmAreYouSure(strMsg);
                    dlResult = frm.ShowDialog();
                    if (dlResult != DialogResult.OK) return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidVINInfo", ex.Message);
                return false;
            }
        }

        private void CheckVIN()
        {
            try
            {
                string strStatus;

                if (txtVIN.Text.Trim().Length == 0) return;

                //Ck for Veh. Status
                strStatus = (cboVehStatus.SelectedItem as ComboboxItem).cboValue;
                if (strStatus == "select")
                {
                    MessageBox.Show("You must select a Vehicle Status", "MISSING VEHICLE STATUS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Return to txtDestination if status is ClearedCustoms
                if (strStatus == "ClearedCustoms" && txtDestination.Text.Trim().Length == 0)
                {
                    MessageBox.Show("You must enter the Destination", "MISSING DESTINATION",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDestination.Focus();
                    return;
                }

                //Ck for Status Date
                if (txtStatusDate.Text.Trim().Length == 0)
                {
                    MessageBox.Show("You must enter the Status Date", "MISSING VEHICLE STATUS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtStatusDate.Focus();
                    return;
                }

                //Ck for Customer
                if ((cboCust.SelectedItem as ComboboxItem).cboValue == "select")
                {
                    MessageBox.Show("You must select the Customer", "MISSING CUSTOMER",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Ck for Forwarder
                if ((cboForwarder.SelectedItem as ComboboxItem).cboValue == "select")
                {
                    MessageBox.Show("You must select the Forwarder", "MISSING FORWARDER",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Ck for Dest, if ClearedCustoms
                if (strStatus == "ClearedCustoms" && txtDestination.Text.Trim().Length == 0)
                {
                    MessageBox.Show("You must enter or scan the Destination",
                        "MISSING DESTINATION",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDestination.FindForm();
                    return;
                }

                //Ck if VIN already in dgResults
                foreach (DataGridViewRow dgRow in dgResults.Rows)
                {
                    if (dgRow.Cells["VIN"].Value.ToString() == txtVIN.Text.Trim())
                    {
                        MessageBox.Show("This vehicle is already in the list",
                            "VEHICLE ALREADY ADDED", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtVIN.Text = "";
                        txtVIN.Focus();
                        return;
                    }
                }

                AddVehicleToGrid();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CheckVIN", ex.Message);
            }
        }

        private void ClearForm()
        {
            try
            {
                DataView dv;
                string strFilter;
                string strMsg = "Are you sure you want to Clear the form";

                //Ck if rows displayed
                if (dgResults.RowCount > 0)
                {
                    //Get rows with RecordStatus of 'UpdatePending'
                    strFilter = "RecordStatus = 'UpdatePending'";
                    dv = new DataView(dtResults, strFilter, "VIN", DataViewRowState.CurrentRows);
                    if (dv.Count > 0)
                        strMsg += " without updating the listed vehicles?";
                    else
                        strMsg += "?";

                    if (MessageBox.Show(strMsg, "CLEAR LISTED VEHICLES?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.No)
                        return;
                }

                //Clear Gridview first to avoid showing Are You Sure message 2x
                ClearGridView();

                //Clear all items in lsControls
                Formops.ClearSetup(this, lsControls);

                //Set controls
                txtDestination.Enabled = false;
                cboExporter.Items.Clear();
                cboForwarder.Items.Clear();
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
                lblVehRecords.Text = "";
                dtResults.Rows.Clear();
                dgResults.DataSource = dtResults;

                btnRemove.Enabled = false;
                btnExport.Enabled = false;
                btnClear.Enabled = false;
                btnSetBooking.Enabled = false;
                btnSetExporter.Enabled = false;
                btnSetForwarder.Enabled = false;
                btnProcess.Enabled = false;
                ckAllrows.Enabled = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearGridView", ex.Message);
            }
        }

        private void CreateTableFordgResults()
        {
            try
            {
                DataColumn dtcol;

                dtcol = new DataColumn("AutoportExportVehiclesID");
                dtcol.DataType = System.Type.GetType("System.Int32");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("CustomerID");
                dtcol.DataType = System.Type.GetType("System.Int32");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("FreightForwarderID");
                dtcol.DataType = System.Type.GetType("System.Int32");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("ExporterID");
                dtcol.DataType = System.Type.GetType("System.Int32");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("Note");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("VIN");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("customer");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("DestinationName");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("BookingNumber");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("VehicleWeight");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("VehicleCubicFeet");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("SizeClass");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("VehicleStatus");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("statusdate");
                dtcol.DataType = System.Type.GetType("System.DateTime");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("forwarder");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("exporter");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("RecordStatus");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("newForwarder");
                dtcol.DataType = System.Type.GetType("System.Int32");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("newExporter");
                dtcol.DataType = System.Type.GetType("System.Int32");
                dtResults.Columns.Add(dtcol);

                dtcol = new DataColumn("NoteToView");
                dtcol.DataType = System.Type.GetType("System.String");
                dtResults.Columns.Add(dtcol);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CreateTableFordgResults", ex.Message);
            }
        }

        private void DestinationBarCodesReport()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                DateTime dat;
                DataSet ds;
                DataColumn dtcol;
                DataTable dtData;
                frmDisplayreport frm;
                int intField = 1;
                List<string> lsTopFour = new List<string>();
                List<string> lsRemaining = new List<string>();
                DataRow row;
                ReportDataSource rptSource;
                string strReport;
                string strSheetPrinter;
                string strSQL;

                //Set dat as two years from today
                dat = DateTime.Now;
                dat = dat.AddYears(-2);

                //Get the Active Destinations with Counts of use for the past two years
                strSQL = @"SELECT
                DestinationName AS dest,
                COUNT(*) as totvehs
                FROM AutoportExportVehicles
                WHERE CreationDate > '" +
                dat.ToString("M/d/yyyy") +
                @"' GROUP BY DestinationName
                ORDER BY Count(*) DESC";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "DestinationBarCodesReport",
                        "No data returned from query");
                    return;
                }

                //Place the top four into lsTopFour & remaining into lsRemaining
                for (int intRow = 0; intRow < ds.Tables[0].Rows.Count; intRow++)
                {
                    if (intRow < 4)
                        lsTopFour.Add(ds.Tables[0].Rows[intRow]["dest"].ToString());
                    else
                        lsRemaining.Add(ds.Tables[0].Rows[intRow]["dest"].ToString());
                }

                //Set up dtData for the report
                dtData = new DataTable();

                for (int intcol = 1; intcol < 21; intcol++)
                {
                    dtcol = new DataColumn("Dest_" + intcol.ToString());
                    dtcol.DataType = System.Type.GetType("System.String");
                    dtData.Columns.Add(dtcol);
                }

                row = dtData.NewRow();

                //Sort lsTopFour and lsRemaining
                lsTopFour = lsTopFour.OrderBy(dest => dest).ToList();
                lsRemaining = lsRemaining.OrderBy(dest => dest).ToList();

                //Add strings in lsTopFour to 1st 4 fields in row
                intField = 1;
                foreach (string strDest in lsTopFour)
                {
                    row["Dest_" + intField] = strDest;
                    intField++;
                }

                //Add strings in lsRemaining to add'l fields in row
                foreach (string strDest in lsRemaining)
                {
                    row["Dest_" + intField] = strDest;
                    intField++;
                }

                dtData.Rows.Add(row);

                //Set dtData as a ReportDataSource named 'dsDestBarcodes'
                rptSource = new ReportDataSource("dsDestBarcodes",
                    dtData);

                strReport = Globalitems.SetReportPath(DESTBARCODEREPORT);
                strSheetPrinter = appSettings["SheetPrinter"];

                frm = new frmDisplayreport("Destination BarCodes", strReport, rptSource,
                     900, 1100);
                Formops.SetFormBackground(frm);
                frm.ShowDialog();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "DestinationBarCodesReport",
                    ex.Message);
            }
        }

        private void FillCombos()

        {
            ComboboxItem cboitem;
            DataSet ds;
            string strSQL;

            try
            {
                //cboCust
                cboCust.Items.Clear();
                cboVehStatus.Items.Clear();

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

                // Add <select> to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
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

                //cboStatus only use Submitted Customs / Cleared Customs / Customs Exception

                //Change 1st item from All to <select>
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboVehStatus.Items.Add(cboitem);

                cboitem = new ComboboxItem();
                cboitem.cboText = "Submitted Customs";
                cboitem.cboValue = "SubmittedCustoms";
                cboVehStatus.Items.Add(cboitem);

                cboitem = new ComboboxItem();
                cboitem.cboText = "Cleared Customs";
                cboitem.cboValue = "ClearedCustoms";
                cboVehStatus.Items.Add(cboitem);

                cboitem = new ComboboxItem();
                cboitem.cboText = "Received Exception";
                cboitem.cboValue = "ReceivedException";
                cboVehStatus.Items.Add(cboitem);

                cboitem = new ComboboxItem();
                cboitem.cboText = "Customs Exception";
                cboitem.cboValue = "CustomsException";
                cboVehStatus.Items.Add(cboitem);

                cboVehStatus.DisplayMember = "cboText";
                cboVehStatus.ValueMember = "cboValue";
                cboVehStatus.SelectedIndex = 0;
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

        private void ProcessBatch()
        {
            try
            {
                DataSet ds;
                DataTable dtVINInfoToUpdate;
                DataTable dtVinInfoInVehtable;
                DataView dv;
                string strDateNow;
                string strFilter;
                string strSQL;
                string strtmptable;
                string strVINs = "";

                //Verify User wants to update veh table
                if (MessageBox.Show("Are you sure you want to process the vehicles now?",
                    "PROCESS VEHICLES NOW?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Hand) == DialogResult.Yes)
                {
                    //Get rows with RecordStatus of 'UpdatePending'
                    strFilter = "RecordStatus = 'UpdatePending'";
                    dv = new DataView(dtResults, strFilter, "VIN", DataViewRowState.CurrentRows);
                    if (dv.Count == 0)
                    {
                        MessageBox.Show("There are no rows with UpdatePending status to process.",
                            "NO ROWS TO PROCESS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    dtVINInfoToUpdate = dv.ToTable();

                    //Get rows from veh table
                    foreach (DataRowView dvRow in dv)
                        strVINs += "'" + dvRow["VIN"] + "',";

                    //Remove last ','
                    strVINs = strVINs.Substring(0, strVINs.Length - 1);

                    //Set up strVINs for WHERE clause
                    strVINs = " VIN IN (" + strVINs + ") ";

                    strSQL = SQLToGetDataFromVehTable(strVINs);
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "ProcessBatch",
                            "No tables returned from Veh. query");
                        return;
                    }

                    dtVinInfoInVehtable = ds.Tables[0];

                    if (ValidVINInfo(dtVINInfoToUpdate, dtVinInfoInVehtable))
                    {
                        //Remove unnecessary cols from dtVehicles
                        dtVINInfoToUpdate.Columns.Remove("VIN");
                        dtVINInfoToUpdate.Columns.Remove("CustomerID");
                        dtVINInfoToUpdate.Columns.Remove("customer");
                        dtVINInfoToUpdate.Columns.Remove("DestinationName");
                        dtVINInfoToUpdate.Columns.Remove("VehicleWeight");
                        dtVINInfoToUpdate.Columns.Remove("VehicleCubicFeet");
                        dtVINInfoToUpdate.Columns.Remove("SizeClass");
                        dtVINInfoToUpdate.Columns.Remove("forwarder");
                        dtVINInfoToUpdate.Columns.Remove("exporter");
                        dtVINInfoToUpdate.Columns.Remove("RecordStatus");
                        dtVINInfoToUpdate.Columns.Remove("newExporter");
                        dtVINInfoToUpdate.Columns.Remove("newForwarder");
                        dtVINInfoToUpdate.Columns.Remove("NoteToView");

                        //Create a unique string based on datetime for tmp table name in SQL DB
                        strDateNow = DateTime.Now.ToString("yyyyMMddHHmmss");
                        strtmptable = "tmpVehicleInfoUpdate_" + strDateNow;

                        //Create the table tmpVehicleInfoUpdate_[strDateNow] in SQL DB
                        //  with the columns to update in the veh table
                        strSQL = "CREATE TABLE " + strtmptable +
                            @" (AutoportExportVehiclesID int,
                            FreightForwarderID int,
                            ExporterID int,
                            Note varchar(1000),                            
                            BookingNumber varchar(20),
                            VehicleStatus varchar(20),
                            statusdate datetime
                            )";

                        DataOps.PerformDBOperation(strSQL);

                        //Bulk copy dtVehicles into tmpVehicleInfoUpdate
                        DataOps.PerformBulkCopy("tmpVehicleInfoUpdate_" + strDateNow,
                            dtVINInfoToUpdate);

                        //Update veh table from tmp table, 
                        //Update either DateSubmittedCustoms or CustomsApprovedDate with 
                        //  statusdate depending on Veh. Status to update

                        //NOTE: don't need to insert new recs into AEVehicleStatusHistory table
                        //  because of Triggers on AutoportExportVehicles table
                        strSQL = @"UPDATE veh
                        SET veh.VehicleStatus = tmp.VehicleStatus,
                        veh.BookingNumber = tmp.BookingNumber,
                        veh.FreightForwarderID = 
                            (CASE
                                WHEN tmp.FreightForwarderID > 0 THEN tmp.FreightForwarderID
                                ELSE NULL
                            END), 
                        veh.ExporterID = 
                             (CASE
                                WHEN tmp.ExporterID > 0 THEN tmp.ExporterID
                                ELSE NULL
                            END), 
                        veh.UpdatedBy = '" + Globalitems.strUserName + @"',
                        veh.UpdatedDate = CURRENT_TIMESTAMP,
                        veh.DateSubmittedCustoms =
                            (CASE
                                WHEN tmp.VehicleStatus = 'SubmittedCustoms' THEN tmp.statusdate
                                WHEN tmp.VehicleStatus = 'ReceivedException' THEN NULL
                                ELSE veh.DateSubmittedCustoms
                            END), 
                       veh.CustomsApprovedDate =
                            (CASE
                                WHEN tmp.VehicleStatus = 'ClearedCustoms' THEN tmp.statusdate
                                ELSE NULL
                            END),
                        veh.CustomsExceptionDate =
                            (CASE
                                WHEN tmp.VehicleStatus = 'CustomsException' THEN tmp.statusdate
                                ELSE NULL
                            END),
                        veh.ReceivedExceptionDate =
                            (CASE
                                WHEN tmp.VehicleStatus = 'ReceivedException' THEN tmp.statusdate
                                ELSE NULL
                            END),
                        veh.Note = tmp.Note,
                        veh.DateShipped = NULL
                        FROM " + strtmptable + @" tmp
                        INNER JOIN AutoportExportVehicles veh ON 
                            veh.AutoportExportVehiclesID = tmp.AutoportExportVehiclesID";

                        DataOps.PerformDBOperation(strSQL);

                        strSQL = "DROP TABLE " + strtmptable;
                        DataOps.PerformDBOperation(strSQL);

                        //Update dtResults & dgResults
                        foreach (DataRowView dvRow in dv)
                        {
                            dvRow["RecordStatus"] = "Updated Successfully";
                        }

                        dgResults.DataSource = dtResults;

                        //Disable btns that don't apply when displaying Updated Successfully
                        btnProcess.Enabled = false;
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ProcessBatch", ex.Message);
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
                    ControlInfo objctrlinfo;
                    string strFilename;
                    string strSort = "";
                    string strSQL;
                    List<ControlInfo> lsCSVcols = new List<ControlInfo>();

                    //1. Get the file Directory & Filename from the SettingTable
                    strSQL = @"SELECT ValueKey,ValueDescription FROM SettingTable 
                        WHERE ValueKey IN ('ExportDirectory','EventProcessingExportFileName') 
                        AND RecordStatus='Active' ORDER BY ValueKey DESC";
                    ds = DataOps.GetDataset_with_SQL(strSQL);

                    // S/B just two active rows, row 1 ExportDirectory, 
                    //  row 2 EventProcessingExportFileName
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
                    dt = dtResults.Copy();

                    //3. If the gridview is sorted, use a dv to sort the table copy the same way
                    if (dgResults.SortedColumn != null)
                    {
                        strSort = dgResults.SortedColumn.DataPropertyName;
                        if (dgResults.SortOrder ==
                            System.Windows.Forms.SortOrder.Descending) strSort += " DESC";
                        dv = new DataView(dt, "", strSort, DataViewRowState.CurrentRows);
                        dt = dv.ToTable();
                    }

                    //4. Create a list, lsCSVcols with ControlInfo objects in the order to appear in the csv file 
                    //Get ctrlinfo object from lsControls for UserCode & add to lsCSVcols. Use HeaderText to ID objects

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "VIN";
                    objctrlinfo.RecordFieldName = "VIN";
                    lsCSVcols.Add(objctrlinfo);

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "Customer";
                    objctrlinfo.RecordFieldName = "customer";
                    lsCSVcols.Add(objctrlinfo);

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "Destination";
                    objctrlinfo.RecordFieldName = "DestinationName";
                    lsCSVcols.Add(objctrlinfo);

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "Booking #";
                    objctrlinfo.RecordFieldName = "BookingNumber";
                    lsCSVcols.Add(objctrlinfo);

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "Weight";
                    objctrlinfo.RecordFieldName = "VehicleWeight";
                    lsCSVcols.Add(objctrlinfo);

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "Cubic Ft.";
                    objctrlinfo.RecordFieldName = "VehicleCubicFeet";
                    lsCSVcols.Add(objctrlinfo);

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "Size";
                    objctrlinfo.RecordFieldName = "SizeClass";
                    lsCSVcols.Add(objctrlinfo);

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "Vehicle Status";
                    objctrlinfo.RecordFieldName = "VehicleStatus";
                    lsCSVcols.Add(objctrlinfo);

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "Forwarder";
                    objctrlinfo.RecordFieldName = "forwarder";
                    lsCSVcols.Add(objctrlinfo);

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "Exporter";
                    objctrlinfo.RecordFieldName = "exporter";
                    lsCSVcols.Add(objctrlinfo);

                    objctrlinfo = new ControlInfo();
                    objctrlinfo.HeaderText = "Record Status";
                    objctrlinfo.RecordFieldName = "RecordStatus";
                    lsCSVcols.Add(objctrlinfo);

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

        private void RemoveRow()
        {
            try
            {
                DataTable dtTmp;
                DataView dv;
                string strFilter;
                string strIDs = "";

                //Ck that at least one row is selected
                if (dgResults.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select at least one row to remove",
                        "NO ROWS SELECTED TO REMOVE", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                //Verify User wants to update veh table
                if (MessageBox.Show("Are you sure you want to remove the selected rows?",
                    "REMOVE ROWS?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Hand) == DialogResult.Yes)
                {
                    //Ck if ALL rows in dgResults are selected
                    if (dgResults.SelectedRows.Count == dgResults.RowCount)
                    {
                        ClearGridView();
                        return;
                    }

                    //Store the VehIDs of all selected rows in strIDs
                    foreach (DataGridViewRow dgRow in dgResults.SelectedRows)
                        strIDs += dgRow.Cells["VehID"].Value.ToString() + ",";

                    //Remove last '
                    strIDs = "(" + strIDs.Substring(0, strIDs.Length - 1) + ")";

                    strFilter = "AutoportExportVehiclesID NOT IN " + strIDs;
                    dv = new DataView(dtResults, strFilter, "VIN", DataViewRowState.CurrentRows);
                    dtTmp = dv.ToTable();
                    dtResults = dtTmp.Copy();
                    dgResults.DataSource = dtResults;

                    if (dtResults.Rows.Count == 0)
                    {
                        btnSetBooking.Enabled = false;
                        btnSetExporter.Enabled = false;
                        btnSetForwarder.Enabled = false;
                        btnRemove.Enabled = false;
                        btnExport.Enabled = false;
                        btnProcess.Enabled = false;
                        lblVehRecords.Text = "";
                    }
                    else
                        lblVehRecords.Text = "Records: " + dtResults.Rows.Count.ToString();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "RemoveRow", ex.Message);
            }
        }

        private void RefillcboExporter(string strForwarderID = "",
            ComboBox cboForSelect = null)
        {
            {
                ComboBox cboToFill;
                ComboboxItem cboitem;
                DataSet ds;
                string strFwdIDForExporters = "";
                string strSQL;

                try
                {
                    //If no params, refill cboForwarder from cboCust, as long as cboCust is not select
                    if (strForwarderID.Length == 0)
                    {
                        //GetCustID from cboCust
                        strFwdIDForExporters =
                            (cboForwarder.SelectedItem as ComboboxItem).cboValue.ToString().Trim();

                        if (strFwdIDForExporters == "select") return;

                        //Set cboForwarder as cboToFill
                        cboToFill = cboExporter;
                    }
                    else
                    {
                        //Use params passed in
                        cboToFill = cboForSelect;
                        strFwdIDForExporters = strForwarderID;
                    }

                    cboToFill.Items.Clear();


                    strSQL = @"SELECT  AEExporterID, 
                    CASE WHEN LEN(RTRIM(ISNULL(ExporterShortName,''))) > 0 THEN 
                        ExporterShortName 
                        ELSE ExporterName  
                    END AS Exporter 
                    FROM AEExporter 
                    WHERE LEN(RTRIM(ISNULL(ExporterName,''))) > 0 
                    AND AEFreightForwarderID  = " + strFwdIDForExporters + " ";

                    //Add RecordStatus if ckActiv is checked
                    if (ckActive.Checked)
                        strSQL += "AND RecordStatus='Active' ";

                    strSQL += "ORDER BY Exporter";

                    ds = DataOps.GetDataset_with_SQL(strSQL);

                    // Add <select> to cbo
                    cboitem = new ComboboxItem();
                    cboitem.cboText = "<select>";
                    cboitem.cboValue = "select";
                    cboToFill.Items.Add(cboitem);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        cboitem = new ComboboxItem();
                        cboitem.cboText = dr["Exporter"].ToString();
                        cboitem.cboValue = dr["AEExporterID"].ToString();
                        cboToFill.Items.Add(cboitem);
                    }

                    cboToFill.DisplayMember = "cboText";
                    cboToFill.ValueMember = "cboValue";
                    cboToFill.SelectedIndex = 0;
                }

                catch (Exception ex)
                {
                    Globalitems.HandleException(CURRENTMODULE, "RefillcboExporter", ex.Message);
                }
            }
        }

        private void RefillcboForwarder(string strCustomerID = "", ComboBox cboForSelect = null)
        {
            ComboBox cboToFill;
            ComboboxItem cboitem;
            DataSet ds;
            string strCustIDForForwarders = "";
            string strSQL;

            try
            {
                //If no params, refill cboForwarder from cboCust, as long as cboCust is not select
                if (strCustomerID.Length == 0)
                {
                    cboForwarder.Items.Clear();
                    cboExporter.Items.Clear();

                    //GetCustID from cboCust
                    strCustIDForForwarders =
                        (cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim();

                    if (strCustIDForForwarders == "select") return;

                    //Set cboForwarder as cboToFill
                    cboToFill = cboForwarder;
                }
                else
                {
                    //Use params passed in
                    cboToFill = cboForSelect;
                    strCustIDForForwarders = strCustomerID;
                }

                cboToFill.Items.Clear();

                strSQL = @"SELECT  AEFreightForwarderID, 
                CASE WHEN LEN(RTRIM(ISNULL(FreightForwarderShortName,''))) > 0 THEN 
                    FreightForwarderShortName 
                    ELSE FreightForwarderName
                END AS Forwarder
                FROM AEFreightForwarder 
                WHERE AECustomerID IS NOT NULL AND 
                LEN(RTRIM(ISNULL(FreightForwarderName,''))) > 0 AND
                AECustomerID = " + strCustIDForForwarders + " ";

                //Add RecordStatus if ckActiv is checked
                if (ckActive.Checked)
                    strSQL += "AND RecordStatus='Active' ";

                strSQL += "ORDER BY Forwarder";

                ds = DataOps.GetDataset_with_SQL(strSQL);

                // Add <select> to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboToFill.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dr["Forwarder"].ToString();
                    cboitem.cboValue = dr["AEFreightForwarderID"].ToString();
                    cboToFill.Items.Add(cboitem);
                }

                cboToFill.DisplayMember = "cboText";
                cboToFill.ValueMember = "cboValue";
                cboToFill.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "RefillcboForwarder", ex.Message);
            }
        }

        private void RowSelection()
        {
            try
            {
                if (ckAllrows.Checked)
                {
                    dgResults.SelectAll();
                    btnClear.Enabled = true;
                    btnSetForwarder.Enabled = true;
                    btnSetExporter.Enabled = true;
                    btnSetBooking.Enabled = true;
                }
                else
                {
                    dgResults.ClearSelection();
                    btnSetForwarder.Enabled = false;
                    btnSetExporter.Enabled = false;
                    btnSetBooking.Enabled = false;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "RowSelection", ex.Message);
            }
        }

        private void SetBookingNumber()
        {
            DialogResult dlResult;
            DataView dv;
            frmSetSelection frm;
            string strIDs = "";
            string strBookingNumber;

            try
            {
                //Make sure at least one row is selected
                if (dgResults.SelectedRows.Count == 0)
                {
                    MessageBox.Show("You must select at least one row to modify the Booking #",
                        "NO ROWS SELECTED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Open frmSetSelection in modal form, to get new Booking Number
                frm = new frmSetSelection("Booking Number");
                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                {
                    strBookingNumber = Globalitems.strSelection.Trim();

                    //Store the VehIDs of all selected rows in strIDs
                    foreach (DataGridViewRow dgRow in dgResults.SelectedRows)
                        strIDs += dgRow.Cells["VehID"].Value.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    //Set up strIDs as filter for dv
                    strIDs = "AutoportExportVehiclesID IN (" + strIDs;

                    dv = new DataView(dtResults, strIDs, "VIN", DataViewRowState.CurrentRows);
                    foreach (DataRowView dvRow in dv)
                    {
                        dvRow["BookingNumber"] = strBookingNumber;
                        dvRow["RecordStatus"] = "UpdatePending";
                    }

                    dgResults.DataSource = dtResults;
                    btnProcess.Enabled = true;
                }

                btnProcess.Enabled = true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetBookingNumber", ex.Message);
            }
        }


        private void SetForwarder()
        {
            ComboBox cbo;
            DialogResult dlResult;
            DataView dv;
            frmSetSelection frmSelection;
            frmAreYouSure frmConfirm;
            string strCustomer;
            string strCustomerID;
            string strForwarder = "";
            string strForwarderID = "";
            string strIDs = "";

            try
            {
                if (dgResults.SelectedRows.Count == 0)
                {
                    MessageBox.Show("You must select at least one row to modify the Forwarder",
                        "NO ROWS SELECTED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Get 1st CustomerID
                strCustomerID = dgResults.SelectedRows[0].Cells["CustomerID"].Value.ToString();
                strCustomer = dgResults.SelectedRows[0].Cells["Customer"].Value.ToString();

                //Ck that all selected rows have the same Customer
                foreach (DataGridViewRow dgRow in dgResults.SelectedRows)
                {
                    if (dgRow.Cells["CustomerID"].Value.ToString() != strCustomerID)
                    {
                        MessageBox.Show("To change the Forwarder, all selected rows must " +
                            "have the same Customer", "DIFFERENT CUSTOMERS IN SELECTED ROWS",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    strIDs += dgRow.Cells["VehID"].Value.ToString() + ",";
                }

                //Remove last ',' in strIDs
                strIDs = strIDs.Substring(0, strIDs.Length - 1);

                //Invoke Are You Sure form
                frmConfirm = new frmAreYouSure("Are you sure you want to change the Forwarder?" +
                    "\n\nAny recorded Exporters will have to be re-entered.");

                dlResult = frmConfirm.ShowDialog();

                if (dlResult != DialogResult.OK) return;

                //Use cboForwarder if cboCust is already set to the correct CustomerID
                if ((cboCust.SelectedItem as ComboboxItem).cboValue == strCustomerID)
                    cbo = cboForwarder;
                else
                {
                    cbo = new ComboBox();
                    RefillcboForwarder(strCustomerID, cbo);
                }

                //Open frmSetSelection in modal form, to get new Booking Number
                frmSelection = new frmSetSelection("Forwarder", cbo,
                    "Please select the Forwarder for " + strCustomer);
                dlResult = frmSelection.ShowDialog();

                if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                {
                    strForwarderID = Globalitems.strSelection;

                    //Get forwarder for FreightForwarderID
                    foreach (ComboboxItem cboitem in cbo.Items)
                    {
                        if (cboitem.cboValue == strForwarderID)
                        {
                            strForwarder = cboitem.cboText;
                            break;
                        }
                    }

                    //Set up strIDs as filter for dv
                    strIDs = "AutoportExportVehiclesID IN (" + strIDs + ")";

                    dv = new DataView(dtResults, strIDs, "VIN", DataViewRowState.CurrentRows);
                    foreach (DataRowView dvRow in dv)
                    {
                        dvRow["FreightForwarderID"] = strForwarderID;
                        dvRow["forwarder"] = strForwarder;
                        dvRow["ExporterID"] = -1;
                        dvRow["exporter"] = "";
                        dvRow["RecordStatus"] = "UpdatePending";
                        dvRow["newForwarder"] = 1;
                        dvRow["newExporter"] = 1;
                    }

                    dgResults.DataSource = dtResults;
                    btnProcess.Enabled = true;
                    btnRemove.Enabled = true;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetBookingNumber", ex.Message);
            }
        }


        private void SetExporter()
        {
            ComboBox cbo;
            DialogResult dlResult;
            DataView dv;
            frmSetSelection frmSelection;
            frmAreYouSure frmConfirm;
            string strCustomer;
            string strCustomerID;
            string strForwarder = "";
            string strForwarderID = "";
            string strExporterID;
            string strExporter = "";
            string strIDs = "";

            try
            {
                if (dgResults.SelectedRows.Count == 0)
                {
                    MessageBox.Show("You must select at least one row to modify the Exporter",
                        "NO ROWS SELECTED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Get 1st CustomerID/ForwarderID
                strCustomerID = dgResults.SelectedRows[0].Cells["CustomerID"].Value.ToString();
                strCustomer = dgResults.SelectedRows[0].Cells["Customer"].Value.ToString();
                strForwarderID = dgResults.SelectedRows[0].Cells["FreightForwarderID"].Value.ToString();
                strForwarder = dgResults.SelectedRows[0].Cells["Forwarder"].Value.ToString();

                //Ck that all selected rows have the same Customer/Forwarder
                foreach (DataGridViewRow dgRow in dgResults.SelectedRows)
                {
                    if (dgRow.Cells["CustomerID"].Value.ToString() != strCustomerID)
                    {
                        MessageBox.Show("To change the Exporter, all selected rows must " +
                            "have the same Customer", "DIFFERENT CUSTOMERS IN SELECTED ROWS",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (dgRow.Cells["FreightForwarderID"].Value.ToString() != strForwarderID)
                    {
                        MessageBox.Show("To change the Exporter, all selected rows must " +
                            "have the same Forwarder", "DIFFERENT FORWRDERS IN SELECTED ROWS",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    strIDs += dgRow.Cells["VehID"].Value.ToString() + ",";
                }

                //Remove last ',' in strIDs
                strIDs = strIDs.Substring(0, strIDs.Length - 1);

                //Invoke Are You Sure form
                frmConfirm = new frmAreYouSure("Are you sure you want to change the Exporter?");

                dlResult = frmConfirm.ShowDialog();

                if (dlResult != DialogResult.OK) return;

                //Use cboExporter is already set to the correct CustomerID
                if ((cboForwarder.SelectedItem as ComboboxItem).cboValue == strForwarderID)
                    cbo = cboExporter;
                else
                {
                    cbo = new ComboBox();
                    RefillcboExporter(strForwarderID, cbo);
                }

                //Open frmSetSelection in modal form, to get new Booking Number
                frmSelection = new frmSetSelection("Exporter", cbo,
                    "Please select the Exporter for " + strForwarder);
                dlResult = frmSelection.ShowDialog();

                if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                {
                    strExporterID = Globalitems.strSelection;

                    //Get Exporter for ExporterID
                    foreach (ComboboxItem cboitem in cbo.Items)
                    {
                        if (cboitem.cboValue == strExporterID)
                        {
                            strExporter = cboitem.cboText;
                            break;
                        }
                    }

                    //Set up strIDs as filter for dv
                    strIDs = "AutoportExportVehiclesID IN (" + strIDs + ")";

                    dv = new DataView(dtResults, strIDs, "VIN", DataViewRowState.CurrentRows);
                    foreach (DataRowView dvRow in dv)
                    {
                        dvRow["ExporterID"] = strExporterID;
                        dvRow["exporter"] = strExporter;
                        dvRow["RecordStatus"] = "UpdatePending";
                        dvRow["newExporter"] = 1;
                    }

                    dgResults.DataSource = dtResults;
                    btnProcess.Enabled = true;
                    btnRemove.Enabled = true;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetBookingNumber", ex.Message);
            }
        }

        private void VehicleStatusChange()
        {
            try
            {
                string strStatus;

                if (cboVehStatus.Items.Count == 0) return;

                strStatus = (cboVehStatus.SelectedItem as ComboboxItem).cboValue;

                if (strStatus == "select")
                {
                    ClearForm();
                    return;
                }

                //Set Status Date to today
                txtStatusDate.Text = DateTime.Now.ToString("M/d/yyyy");

                if (strStatus == "SubmittedCustoms" || strStatus == "CustomsException")
                {
                    txtDestination.Enabled = false;
                    txtDestination.Text = "";
                }

                if (strStatus == "ClearedCustoms")
                {
                    txtDestination.Enabled = true;
                    txtDestination.Text = "";
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "VehicleStatusChange", ex.Message);
            }
        }

        private void ckActive_CheckedChanged(object sender, EventArgs e)
        {
            FillCombos();
            ClearForm();
        }

        private void cboVehStatus_SelectedIndexChanged(object sender, EventArgs e)
        { VehicleStatusChange(); }

        private void cboCust_SelectedIndexChanged(object sender, EventArgs e)
        { RefillcboForwarder(); }

        private void cboForwarder_SelectedIndexChanged(object sender, EventArgs e)
        { RefillcboExporter(); }

        private void btnClear_Click(object sender, EventArgs e)
        { ClearForm(); }

        private void txtStatusDate_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(txtStatusDate, e); }

        private void txtStatusDate_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtStatusDate, e); }

        private void frmEventProcessing_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Ck if items in dgResults
            if (dgResults.RowCount > 0 &&  !Globalitems.blnTimeout)
            {
                if (MessageBox.Show("Are you sure you want to exit without updating " +
                    "the listed vehicles?", "IGNORE UPDATING VEHICLES?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        { this.Close(); }

        private void ckAllrows_CheckedChanged(object sender, EventArgs e)
        { RowSelection(); }

        private void btnAddRecord_Click(object sender, EventArgs e)
        { CheckVIN(); }

        private void btnSetBooking_Click(object sender, EventArgs e)
        { SetBookingNumber(); }

        private void dgResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //As long as row clicked is not the Column Header row, index = -1, enable btnContinue
            if (e.RowIndex > -1)
            {
                btnRemove.Enabled = true;
                btnSetBooking.Enabled = true;
                btnSetForwarder.Enabled = true;
                btnSetExporter.Enabled = true;
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        { ProcessBatch(); }

        private void btnSetExporter_Click(object sender, EventArgs e)
        { SetExporter(); }

        private void btnSetForwarder_Click(object sender, EventArgs e)
        { SetForwarder(); }

        private void btnRemove_Click(object sender, EventArgs e)
        { RemoveRow(); }

        private void btnExport_Click(object sender, EventArgs e)
        { OpenCSVFile(); }

        private void txtVIN_KeyUp(object sender, KeyEventArgs e)
        {
            if ((cboVehStatus.SelectedItem as ComboboxItem).cboValue.Contains("Submitted") &&
                txtVIN.Text.Trim().Length > 16) CheckVIN();
            if ((cboVehStatus.SelectedItem as ComboboxItem).cboValue.Contains("Excep") &&
                txtVIN.Text.Trim().Length > 16) CheckVIN();
            if ((cboVehStatus.SelectedItem as ComboboxItem).cboValue.Contains("Cleared") &&
                txtVIN.Text.Trim().Length > 16) txtDestination.Focus();
        }

        private void txtDestination_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Click Search btn when User hits Enter button
            if (e.KeyChar == (char)Keys.Return || e.KeyChar == (char)Keys.Tab)
                btnAddRecord_Click(null, null);
        }

        private void txtDestination_Leave(object sender, EventArgs e)
        {
            if (txtDestination.Text.Trim().Length > 0) btnAddRecord_Click(null, null);
        }

        private void btnDestBarcodes_Click(object sender, EventArgs e)
        { DestinationBarCodesReport(); }

        private void txtDestination_KeyUp(object sender, KeyEventArgs e)
        {
            string strval;

            if (txtDestination.Text.Trim().Length > 0)
            {
                //If txtDestination ends w/'$', remove '$' and click btnAddRecord
                strval = txtDestination.Text.Trim();
                if (strval.Substring(strval.Length - 1, 1) == "$")
                {
                    txtDestination.Text = strval.Substring(0, strval.Length - 1);
                    btnAddRecord_Click(null, null);
                }

            }

        }

        private void dgResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                string strFullNote;
                string strStatus;

                if ((e.ColumnIndex == this.dgResults.Columns["View"].Index)
                    && e.Value != null && e.Value.ToString() != "")
                {
                    //Get the Veh. Status from col. VehicleStatus
                    strStatus = this.dgResults.Rows[e.RowIndex].Cells["VehicleStatus"].Value.ToString();
                    
                    //Get the full note from col. FullNote
                    strFullNote = this.dgResults.Rows[e.RowIndex].Cells["FullNote"].Value.ToString();


                    DataGridViewCell cell =
                        this.dgResults.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    cell.ToolTipText =  strFullNote;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "dgResults_CellFormatting", ex.Message);
            }
        }

        private void OpenDamageCodeNoteForm(string strVIN,string strCustomer,string strExDate,
            string strNote)
        {
            frmDamageCodeNote frm;

            frm = new frmDamageCodeNote(strVIN,strCustomer,strExDate,"",strNote);
            frm.ShowDialog();
        }

        private void dgResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Cast generic sender as a DataGridView
            var senderGrid = (DataGridView)sender;
            DateTime dat;
            string strCustomer;
            string strExceptionDate;
            string strNote;
            string strVIN;

            //If Note column was clicked and value==VIEW open DamageCodeNote form
            if (senderGrid.Columns[e.ColumnIndex].Name.ToString() == "View" &&
                senderGrid.Rows[e.RowIndex].Cells["View"].Value.ToString() == "VIEW")
            {
                //Get InspectionID & DamageCode
                strCustomer = senderGrid.Rows[e.RowIndex].Cells["Customer"].Value.ToString();
                strVIN = senderGrid.Rows[e.RowIndex].Cells["VIN"].Value.ToString();
                dat = Convert.ToDateTime(senderGrid.Rows[e.RowIndex].Cells["Received"].Value.ToString());
                strExceptionDate = dat.ToString("M/d/yyyy");
                strNote = senderGrid.Rows[e.RowIndex].Cells["FullNote"].Value.ToString();

                OpenDamageCodeNoteForm(strVIN,strCustomer,strExceptionDate,strNote);
            }

        }

        private void frmEventProcessing_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }
    }
}
