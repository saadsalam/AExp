using AutoExport.Objects;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmCustomClearedSheets : Form
    {
        //CONSTANTS
        private const string CURRENTMODULE = "frmCustomClearedSheets";
        //private const string SHEETPRINTER = "Xerox Phaser 5500DN";
        private const string SHEETREPORT = "rptCustomsApprovedSheet.rdlc";
        private const string COVERSHEETREPORT = "rptCustomsApprovedCoversheet.rdlc";

        //Variables
        public bool blnCustomsClearedReport = true;
        public PrintInfo objPrintInfo;
        public string strRptType;

        private bool blncboCustFill = true;
        private bool blncboVoyageFill = true;
        private bool blnDisplayingReport = false;
        private string strSheetPrinter;

        public frmCustomClearedSheets(PrintInfo objPInfo)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                List<string> lsExcludes = new List<string>();

                InitializeComponent();
                objPrintInfo = objPInfo;

                //Read appSettings from app.config file for SheetPrinter
                strSheetPrinter = appSettings["SheetPrinter"];

                FillcboCust();
                FillcboVoyage();
                Globalitems.SetControlHeight(this);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmCustomClearedSheets", ex.Message);
            }
        }

        private void frmCustomClearedSheets_Activated(object sender, EventArgs e)
        {
            //Leave form settings unchanged if returning from Displaying Report
            if (blnDisplayingReport)
            {
                blnDisplayingReport = false;
                return;
            }

            //Set rbSelected if blnSetSelected
            if (objPrintInfo.SelectedIDs.Count > 0)
            {
                rbSelected.Text = objPrintInfo.Message;
                rbSelected.Enabled = true;
                rbSelected.Checked = true;
            }
            else
            {
                rbSelected.Text = "No rows selected from another form";
                rbSelected.Enabled = false;
            }

            if (blnCustomsClearedReport)
                this.Text = "Export - Customs Cleared Sheets";
            else
                this.Text = "Export - Customs Cleared Coversheet";

            if (Globalitems.runmode == "TEST") this.Text = "TEST - " + this.Text;

            //Ck if there are SheetNotYetPrinted
            SheetsNotYetPrinted();

            //Display/hide Voyage, selected rows, Print Date depending on blnCoversheetReport
            lblVoyage.Visible = blnCustomsClearedReport;
            cboVoyage.Visible = blnCustomsClearedReport;
            rbSelected.Visible = blnCustomsClearedReport;
            rbVIN.Visible = blnCustomsClearedReport;
            rbVoyage.Visible = blnCustomsClearedReport;
            rbPrintDateCriteria.Visible = blnCustomsClearedReport;
        }

        private void FillcboVoyage()
        {
            ComboboxItem cboitem;
            DataSet ds;
            string strSQL;
            string strval;

            try
            {
                //Don't run when cboCust is being filled w/values
                if (blncboCustFill) return;

                lblNoVoyages.Visible = false;

                cboVoyage.Items.Clear();

                strSQL = @"SELECT DISTINCT 
                    ISNULL(CONVERT(varchar(10),voy.VoyageDate,101),'') + ' - '  +
                    + voy.VoyageNumber + ' - ' + ves.VesselName AS voyinfo, 
                    voy.AEVoyageID,
                    voy.VoyageDate 
                    FROM AEVoyage voy 
                    LEFT JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID 
                    LEFT JOIN AEVoyageCustomer voycust ON voy.AEVoyageID = voycust.AEVoyageID 
                    WHERE 
                    voy.VoyageDate < '1/1/3000' AND
                    voy.VoyageDate >= CONVERT(varchar(10), CURRENT_TIMESTAMP, 101) ";

                    //Add CustomerID if cboCust <> 'All'
                    strval = (cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    if (strval != "All") strSQL += " AND voycust.CustomerID = " + strval;

                strSQL += " ORDER BY voy.VoyageDate DESC";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillCombos",
                        "No tables returned from Voyage info table");
                    return;
                }

                if (ds.Tables[0].Rows.Count == 0) lblNoVoyages.Visible = true;

                // Add All to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "All";
                cboitem.cboValue = "All";
                cboVoyage.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dr["voyinfo"].ToString();
                    cboitem.cboValue = dr["AEVoyageID"].ToString();
                    cboVoyage.Items.Add(cboitem);
                }

                cboVoyage.DisplayMember = "cboText";
                cboVoyage.ValueMember = "cboValue";
                cboVoyage.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillcboVoyage", ex.Message);
            }
        }

        private void FillcboCust()
        {
            ComboboxItem cboitem;
            DataSet ds;
            string strSQL;

            try
            {
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

                blncboCustFill = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillcboCust", ex.Message);
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }

        private void rbPrintDate_CheckedChanged(object sender, EventArgs e)
        {
            //Hide/Display pnlPrintDate
            pnlPrintDate.Visible = rbPrintDate.Checked;
            if (!blnCustomsClearedReport) rbApprovedDate.Checked = true;     
        }

        private void rbVIN_CheckedChanged(object sender, EventArgs e)
        {
            //HideDisplay txtVIN
            txtVIN.Visible = rbVIN.Checked;
        }

        private void KeyPressTextbox(TextBox txtbox, KeyPressEventArgs e)
        {
            if (!Globalitems.ValidDateKeyStroke(e.KeyChar)) e.Handled = true;
        }

        private void SheetsNotYetPrinted()
        {
            DataSet ds;
            int intRecs;
            string strSQL;
            string strval;

            try
            {
                if (blncboCustFill || blncboVoyageFill) return;

                if (blnCustomsClearedReport)
                {
                    strSQL = "SELECT AutoportExportVehiclesID " +
                    "FROM AutoportExportVehicles " +
                    "WHERE CustomsApprovalPrintedInd = 0 ";

                    //Add CustomerID if cboCust <> 'All'
                    strval = (cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    if (strval != "All") strSQL += " AND CustomerID = " + strval;

                    //Add VoyageID if cboVoyage <> 'All'
                    strval = (cboVoyage.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    if (strval != "All") strSQL += " AND VoyageID = " + strval;
                }
                else
                {
                    strSQL = "SELECT AutoportExportVehiclesID " +
                    "FROM AutoportExportVehicles " +
                    "WHERE CustomsApprovedCoverSheetPrintedInd = 0 ";

                    //Add CustomerID if cboCust <> 'All'
                    strval = (cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    if (strval != "All") strSQL += " AND CustomerID = " + strval;
                }

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "SheetsNotYetPrinted", "No data " +
                        "returned from SQL.");
                    return;
                }

                intRecs = ds.Tables[0].Rows.Count;

                if (intRecs == 0) 
                {
                        rbAll.Checked = false;
                        rbAll.Text = "There are currently NO Unprinted Labels";
                        rbAll.Enabled = false;
                    if (!blnCustomsClearedReport) rbPrintDate.Checked = true;
                }
                else
                {
                    rbAll.Text = "Print All Unprinted Sheets";
                    rbAll.Enabled = true;

                    //Place unprinted IDs into objPrintInfo
                    foreach (DataRow drow in ds.Tables[0].Rows)
                        objPrintInfo.UnprintedIDs.Add((int)drow[0]);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SheetsNotYetPrinted", ex.Message);
            }
        }

        private string SQLForCustomsClearedCoversheet()
        {
            try
            {
                DateTime dat;
                string strSQL;
                string strval;

                strSQL = @"SELECT
                0 AS Linenumber, 
                veh.AutoportExportVehiclesID, 
                CASE 
	                WHEN DATALENGTH(ff.FreightForwarderShortName) > 0 THEN 
                        ff.FreightForwarderShortName 
	                ELSE ff.FreightForwarderName 
                END Forwarder,
                ISNULL(veh.DestinationName,'') AS Destination, 
                veh.VIN, 
                veh.Model,
                CASE 
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
	                ELSE cus.CustomerName 
                END Customer,
                CustomsApprovedCoverSheetPrintedInd
                FROM AutoportExportVehicles veh
                LEFT OUTER JOIN Customer cus ON veh.CustomerID = cus.CustomerID
                LEFT OUTER JOIN AEFreightForwarder ff ON 
                    veh.FreightForwarderID = ff.AEFreightForwarderID
                WHERE veh.CustomsApprovedDate IS NOT NULL ";

                //Add params to WHERE clause

                //Ck All Unprinted  
                if (rbAll.Checked)
                    strSQL += "AND veh.CustomsApprovedCoverSheetPrintedInd = 0 ";

                //Ck Customer
                if (cboCust.SelectedIndex != 0)
                {
                    strval = (cboCust.SelectedItem as ComboboxItem).cboValue;
                    strSQL += "AND veh.CustomerID =  " + strval + " ";
                }

                //Ck Print date
                if (rbPrintDate.Checked)
                {
                    if (txtStartDate.Text.Trim().Length > 0)
                        strSQL += "AND veh.CustomsApprovedDate >= '" +
                            txtStartDate.Text.Trim() + "' ";

                    //If End Date specified add 1 day, and set date to less than new date
                    if (txtEndDate.Text.Trim().Length > 0)
                    {
                        dat = Convert.ToDateTime(txtEndDate.Text.Trim());
                        dat = dat.AddDays(1);
                        strSQL += "AND veh.CustomsApprovedDate < '" + dat.ToString() + "' ";
                    }
                }

                strSQL += "ORDER BY Customer,Forwarder,Destination,Model,VIN;";

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForCustomsClearedCoversheet",
                    ex.Message);
                return "";
            }
        }

        private string SQLForCustomsClearedSheets()
            //4/11/18 D.Maibor: change WHERE veh.CustomsApprovedDate IS NOT NULL AND 
            //  veh.DateShipped IS NULL, to WHERE veh.VehicleStatus='ClearedCustoms'
            //3/29/18 D.Maibor: add ISNULL to where clause for printing all unprinted sheets
        {
            try
            {
                DateTime dat;
                string strSQL;
                string strval;

                //Basic SELECT for all Customs Cleared Sheets
                strSQL = @"SELECT veh.CustomerID,
                veh.AutoportExportVehiclesID,
                veh.VIN,
                RIGHT(veh.VIN, 8) AS VIN8,
                veh.VehicleYear,
                veh.Make,
                veh.Model, 
                veh.DestinationName,
                CASE
                    WHEN LEN(RTRIM(ISNULL(cus.ShortName, ''))) > 0 THEN RTRIM(cus.ShortName)
                    ELSE RTRIM(cus.CustomerName)
                END AS Customer,
                    veh.CustomsApprovedDate,
                veh.BayLocation, 
                veh.BookingNumber, 
                ves.VesselName,
                NoStartInd AS NonRunner,
                veh.HasAudioSystemInd,
                veh.HasNavigationSystemInd,
                CASE 
                    WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName
                    ELSE cus.CustomerName 
                END AS custname,
                RTRIM(ISNULL(veh.SpecialInstructions,'')) AS SpecialInstructions, 
                veh.CustomsApprovalPrintedInd,
                ISNULL(Code.Value1,'WHITE') AS Color, 
                ISNULL(Code.Value1Description,'WHITE') ColorDesc
                FROM AutoportExportVehicles veh 
                LEFT JOIN Customer cus ON veh.CustomerID = cus.CustomerID 
                LEFT JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID 
                LEFT JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                LEFT JOIN Code dest ON dest.CodeType='ExportDischargePort'  AND dest.Code=veh.DestinationName
                LEFT JOIN Code ON Code.CodeType='CustomsSheetColor' AND
                    Code.Value2=veh.CustomerID AND Code.Description=dest.CodeID
                WHERE veh.VehicleStatus='ClearedCustoms'   ";

                //Ck cboCust
                strval = (cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strval != "All") strSQL += "AND veh.CustomerID = " + strval;

                //ck cboVoyage
                strval = (cboVoyage.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strval != "All") strSQL += "AND veh.VoyageID = " + strval;

                //Ck Selected rows from veh. locator
                if (rbSelected.Checked)
                {
                    //Set WHERE clause to veh. IDs in lsSelectedIDs
                    strSQL += "AND veh.AutoportExportVehiclesID IN (";

                    foreach (int intID in objPrintInfo.SelectedIDs)
                        strSQL += intID.ToString() + ",";

                    //Replace ',' with ')' in last position of strSQL
                    strSQL = strSQL.Substring(0, strSQL.Length - 1) + ") ";
                }

                //Ck Unprinted
                if (rbAll.Checked)
                    strSQL += " AND ISNULL(veh.CustomsApprovalPrintedInd,0) = 0 ";

                //Ck Print Date
                if (rbPrintDate.Checked)
                {
                    if (rbApprovedDate.Checked)
                    {
                        if (txtStartDate.Text.Trim().Length > 0)
                            strSQL += "AND veh.CustomsApprovedDate >= '" +
                                txtStartDate.Text.Trim() + "' ";

                        //If End Date specified add 1 day, and set date to less than new date
                        if (txtEndDate.Text.Trim().Length > 0)
                        {
                            dat = Convert.ToDateTime(txtEndDate.Text.Trim());
                            dat = dat.AddDays(1);
                            strSQL += "AND veh.CustomsApprovedDate < '" + dat.ToString() + "' ";
                        }
                    }
                    else
                    {
                        if (txtStartDate.Text.Trim().Length > 0)
                            strSQL += "AND veh.CustomsApprovalPrintedDate >= '" +
                                txtStartDate.Text.Trim() + "' ";

                        //If End Date specified add 1 day, and set date to less than new date
                        if (txtEndDate.Text.Trim().Length > 0)
                        {
                            dat = Convert.ToDateTime(txtEndDate.Text.Trim());
                            dat = dat.AddDays(1);
                            strSQL += "AND veh.CustomsApprovalPrintedDate < '" + dat.ToString() + "' ";
                        }
                    }
                }

                //Ck VIN
                if (rbVIN.Checked) strSQL += "AND veh.VIN LIKE '" + txtVIN.Text.Trim() + "' ";

                //Add order by 
                strSQL += "ORDER BY DestinationName,BayLocation,VIN ";

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForCustomsClearedSheets",
                    ex.Message);
                return "";
            }
        }

        private DataTable GetAllData()
        {
           
            DataSet ds;
            string strSQL;

            try
            {
                if (blnCustomsClearedReport)
                    strSQL = SQLForCustomsClearedSheets();
                else
                    strSQL = SQLForCustomsClearedCoversheet();

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("No vehicles meet the criteria.", "NOTHING TO PRINT",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }

                return ds.Tables[0];
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetReportDataSource", ex.Message);
                return null;
            }
        }

        private void OpenReportDisplayForm(ReportDataSource rptSource,string strReportPath,
            List<ReportParameter> lsParams = null)
        {
            frmDisplayreport frm;
            string strMsg;

            try
            {
                blnDisplayingReport = true;
                strMsg = "Customs Cleared Sheets";
                if (!blnCustomsClearedReport) strMsg = "Customs Cleared Coversheet";

                frm = new frmDisplayreport(strMsg, strReportPath , rptSource,
                    900, 1100,lsParams);
                Formops.SetFormBackground(frm);
                frm.ShowDialog();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenReportDisplayForm", ex.Message);
            }
        }

        private void PrintSheetsDirectly(ReportDataSource rptSource,string strReportPath,
            List<ReportParameter>lsParams = null)
        {
            LocalReport rpt;
            PrintReport objPrint;
            string strIDs;
            string strSQL;

            try
            {
                //Instantiate a local report, and set its ReportPath & Datasource
                rpt = new LocalReport();
                rpt.ReportPath = strReportPath;
                rpt.DataSources.Add(rptSource);

                //Use the PrintReport object to print the report 
                objPrint = new PrintReport(rpt, strSheetPrinter,"SHEET");
                objPrint.PrintAReport();

                //Since no exception, assume labels printed. Update BarCodeLabelPrintedDate,BarCodeLabelPrintedInd 
                //  in AutoportExportVehicles table, with no print data
                if (objPrintInfo.SelectedIDs.Count > 0)
                {
                    strIDs = "AND AutoportExportVehiclesID IN (";

                    foreach (int intID in objPrintInfo.SelectedIDs)
                        strIDs += intID.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    strSQL = "UPDATE AutoportExportVehicles SET CustomsApprovalPrintedInd=1," +
                        "CustomsApprovalPrintedDate='" + DateTime.Now.ToString() + "' " +
                        "WHERE CustomsApprovalPrintedInd=0 " + strIDs;
                    DataOps.PerformDBOperation(strSQL);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PrintLabelsDirectly", ex.Message);
            }
        }

        private void StartRequest(string strRQ)
            //6/8/18 D.Maibor. Fix filter to updated print ind; use Isnull
        {
            DataSet ds;
            DataTable dtAllData;
            DataTable dtDistinct;
            DataTable dtReportdata = null;
            DataRow dtRow;
            DataView dv;
            DialogResult dlResult;
            System.Drawing.Color dwColor;
            frmColorSheetsForPrinter frmColor;
            int intCustomerID;
            int intRed;
            int intGreen;
            int intBlue;
            int inttotalrecs = 0;
            int intval;
            List<ReportParameter> lsParams = null;
            List<int> lsUnprintedIDs = new List<int>();
            ReportParameter rptParam;
            ReportDataSource rptSource = null;
            string[] strRGB;
            string strSheetColor;
            string strSheetColorDesc;
            string strCustomer;
            string strCustomer_current = "";
            string strCustomer_next = "";
            string strDAIAddress = "";
            string strDestination = "";
            string strDestination_current = "";
            string strDestination_next = "";
            string strFilter;
            string strForwarder_current = "";
            string strForwarder_next = "";
            string strIDs;
            string strReport = "";

            string strReportTitle = "";
            string strSQL;

            try
            {
                if (!ValidCriteria()) return;

                dtAllData = GetAllData();
                if (dtAllData == null) return;

                //Special processing for CustomsCleared sheets
                if (blnCustomsClearedReport)
                {
                    strReport = Globalitems.SetReportPath(SHEETREPORT);

                    //Get all the distinct Destination/Customer sets in dt
                    strFilter = "CustomerID IS NOT NULL";
                    dv = new DataView(dtAllData, strFilter, "DestinationName",
                        DataViewRowState.CurrentRows);
                    dtDistinct = dv.ToTable(true, "DestinationName", "CustomerID", "custname",
                        "ColorDesc", "Color");

                    foreach (DataRow drow in dtDistinct.Rows)
                    {
                        intCustomerID = (int)drow["CustomerID"];
                        strCustomer = drow["custname"].ToString();
                        strDestination = drow["DestinationName"].ToString();
                        strSheetColorDesc = drow["ColorDesc"].ToString();
                        strSheetColor = drow["Color"].ToString();

                        lsUnprintedIDs.Clear();

                        //Get the number of recs for the Dest/Cust
                        strFilter = "CustomerID = " + intCustomerID.ToString() +
                            " AND DestinationName = '" + strDestination + "'";
                        dv = new DataView(dtAllData, strFilter, "BayLocation",
                            DataViewRowState.CurrentRows);
                        intval = dv.Count;
                        dtReportdata = dv.ToTable();

                        //Set dwColor
                        if (strSheetColor.Contains(","))
                        {
                            strRGB = strSheetColor.Split(',');

                            //Must be 3 values in strRGB
                            if (strRGB.Length != 3)
                            {
                                Globalitems.HandleException(CURRENTMODULE, "StartRequest",
                                    "strRGB did not contain 3 elements");
                                return;
                            }

                            intRed = Convert.ToInt16(strRGB[0]);
                            intGreen = Convert.ToInt16(strRGB[1]);
                            intBlue = Convert.ToInt16(strRGB[2]);

                            dwColor = Color.FromArgb(intRed, intGreen, intBlue);
                        }
                        else
                        {
                            dwColor = Color.FromName(strSheetColor);
                        }

                        //Return table in ds as a ReportDataSource named 'dsCustomsApprovedSheets'
                        //  or dsCustomsApproveCoversheet
                        rptSource = new ReportDataSource("dsCustomsApprovedSheets",
                           dtReportdata);

                        //Open frmSetSelection in modal form, to get new Destination
                        frmColor = new frmColorSheetsForPrinter(strSheetColorDesc, strDestination,
                            strCustomer, intval, dwColor);
                        dlResult = frmColor.ShowDialog();

                        if (dlResult != DialogResult.OK) return;

                        if (strRQ == "DISPLAY")
                            OpenReportDisplayForm(rptSource, strReport);
                        else
                            PrintSheetsDirectly(rptSource, strReport);

                        strFilter = "Isnull(CustomsApprovalPrintedInd,0) = 0";
                        dv = new DataView(dtReportdata, strFilter, "AutoportExportVehiclesID",
                            DataViewRowState.CurrentRows);

                        if (dv.Count > 0)
                        {
                            foreach (DataRowView dvrow in dv)
                                lsUnprintedIDs.Add((int)dvrow["AutoportExportVehiclesID"]);
                        }

                        //Assume user printed labels. Update CustomsApprovedDate,
                        //  CustomsApprovalPrintededInd 
                        //  in AutoportExportVehicles table, with no print data
                        if (lsUnprintedIDs.Count > 0)
                        {
                            strIDs = "AND AutoportExportVehiclesID IN (";

                            foreach (int intID in lsUnprintedIDs)
                                strIDs += intID.ToString() + ",";

                            //Replace last ',' with ')'
                            strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                            strSQL = @"UPDATE AutoportExportVehicles 
                                SET CustomsApprovalPrintedInd=1,
                                CustomsApprovalPrintedDate='" + DateTime.Now.ToString() + "' " +
                                "WHERE ISNULL(CustomsApprovalPrintedInd,0) = 0 " + strIDs;                                

                            DataOps.PerformDBOperation(strSQL);
                        }
                    }   // foreach             
                }   // if blnCustomsClearedReport

                if (!blnCustomsClearedReport)
                {
                    strReport = Globalitems.SetReportPath(COVERSHEETREPORT);

                    //Set strReportTitle
                    strReportTitle = "Export vehicles Approved By Customs ";

                    if (rbPrintDate.Checked)
                    {
                        //Add Date info depending on From/To Dates
                        if (txtStartDate.Text.Trim().Length > 0 ||
                            txtEndDate.Text.Trim().Length > 0)
                        {
                            //4 possibilities:
                            //Only Start Date - 'On Or After [startdate]'
                            //Start Date = End Date - 'On [startdate]'
                            //Start Date < End Date - 'Between [startdate] And [enddate]'
                            //Only End Date - 'On Or Before [enddate]'
                            if (txtStartDate.Text.Trim().Length > 0)
                            {
                                //Only Start Date
                                if (txtEndDate.Text.Trim().Length == 0)
                                    strReportTitle += "On Or After " +
                                        Convert.ToDateTime(txtStartDate.Text.Trim()).ToString("MM/dd/yyyy");

                                //Start Date = End Date
                                else if (txtStartDate.Text.Trim() == txtEndDate.Text.Trim())
                                    strReportTitle += "On " +
                                        Convert.ToDateTime(txtStartDate.Text.Trim()).ToString("MM/dd/yyyy");

                                //Start Date < End Date
                                else
                                    strReportTitle += "Between " +
                                        Convert.ToDateTime(txtStartDate.Text.Trim()).ToString("MM/dd/yyyy") +
                                        " AND " + Convert.ToDateTime(txtEndDate.Text.Trim()).ToString("MM/dd/yyyy");
                            }   // if startdate.length > 0
                            else
                                //Only Start Date
                                if (txtEndDate.Text.Trim().Length == 0)
                                strReportTitle += "On Or Before " +
                                    Convert.ToDateTime(txtEndDate.Text.Trim()).ToString("MM/dd/yyyy");
                        }   // if startdate.length > 0
                    }


                    //Set DAIAddress
                    strSQL = @"SELECT TOP 1 
                    CompanyName,
                    AddressLine1,
                    ISNULL(AddressLine2,'') AS AddressLine2,
                    City + ',  ' + State +'   ' + Zip AS CitySTZip,
                    '(' + SUBSTRING(Phone,1,3) + ') ' + 
                        SUBSTRING(Phone,4,3) + '-' + 
                        SUBSTRING(Phone,7,DATALENGTH(Phone)-6) AS tel
                    FROM ApplicationConstants ";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "StartRequest",
                            "No data returned from ApplicationConstants table");
                        return;
                    }

                    dtRow = ds.Tables[0].Rows[0];
                    strDAIAddress = dtRow["CompanyName"] + "\n" +
                        dtRow["AddressLine1"] + "\n";

                    if (dtRow["AddressLine2"].ToString().Trim().Length > 0)
                        strDAIAddress += dtRow["AddressLine1"] + "\n";

                    strDAIAddress += dtRow["CitySTZip"] + "\n" +
                        dtRow["tel"];

                    inttotalrecs = dtAllData.Rows.Count;

                    //Setup linenumbers in dtAllData for each Customer/Forwarder/Destination
                    intval = 0;
                    foreach (DataRow dtAllrow in dtAllData.Rows)
                    {
                        strCustomer_next = dtAllrow["Customer"].ToString();
                        strForwarder_next = dtAllrow["Forwarder"].ToString();
                        strDestination_next = dtAllrow["Destination"].ToString();

                        if (strCustomer_next != strCustomer_current ||
                            strForwarder_next != strForwarder_current ||
                                strDestination_next != strDestination_current)
                        {
                            //Start new linenumber
                            intval = 1;
                            strCustomer_current = strCustomer_next;
                            strForwarder_current = strForwarder_next;
                            strDestination_current = strDestination_next;
                        }
                        else
                        {
                            //increment linenumber
                            intval += 1;
                        }
                        dtAllrow["linenumber"] = intval;
                    }

                    //Return table in ds as a ReportDataSource named 'dsCustomsApprovedSheets'
                    rptSource = new ReportDataSource("dsCustomsApprovedCoversheet",
                        dtAllData);

                    //Fill lsParams for CustomsApprovedCoversheet report
                    lsParams = new List<ReportParameter>();
                    rptParam = new ReportParameter("title", strReportTitle);
                    lsParams.Add(rptParam);

                    rptParam = new ReportParameter("DAIAddress", strDAIAddress);
                    lsParams.Add(rptParam);

                    rptParam = new ReportParameter("totalrecs", 
                        inttotalrecs.ToString("#,##0"));
                    lsParams.Add(rptParam);
                    

                    //Open Display Form or Print Directly
                    if (strRQ == "DISPLAY")
                        OpenReportDisplayForm(rptSource, strReport, lsParams);
                    else
                        PrintSheetsDirectly(rptSource, strReport, lsParams);

                    strFilter = "CustomsApprovedCoverSheetPrintedInd = 0";
                    dv = new DataView(dtAllData, strFilter, "AutoportExportVehiclesID",
                        DataViewRowState.CurrentRows);

                        if (dv.Count > 0)
                        {
                            foreach (DataRowView dvrow in dv)
                                lsUnprintedIDs.Add((int)dvrow["AutoportExportVehiclesID"]);
                        }

                        //Assume user printed labels. Update BarCodeLabelPrintedDate,BarCodeLabelPrintedInd 
                        //  in AutoportExportVehicles table, with no print data
                        if (lsUnprintedIDs.Count > 0)
                        {
                            strIDs = "AND AutoportExportVehiclesID IN (";

                            foreach (int intID in lsUnprintedIDs)
                                strIDs += intID.ToString() + ",";

                            //Replace last ',' with ')'
                            strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";
                        
                            strSQL = @"UPDATE AutoportExportVehicles 
                            SET CustomsApprovedCoverSheetPrintedInd=1
                            WHERE CustomsApprovedCoverSheetPrintedInd=0 " + strIDs;

                            DataOps.PerformDBOperation(strSQL);
                        }
                    }   // if !blnCleared Report                              
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "StartRequest", ex.Message);
            }
        }

        private bool ValidCriteria()
        {
            try
            {
                //Verify a selection rb is selected
                if (blnCustomsClearedReport && !rbSelected.Checked && 
                    !rbAll.Checked && !rbPrintDate.Checked &&
                    !rbVIN.Checked && !rbVoyage.Checked)
                {
                    MessageBox.Show("Please select the criteria for printing Custom Cleared Sheets",
                        "NO SELECTION CRITERIA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!blnCustomsClearedReport && 
                    !rbAll.Checked && !rbPrintDate.Checked)
                {
                    MessageBox.Show("Please select the criteria for printing the Customs Cleared Coversheet",
                        "NO SELECTION CRITERIA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //Ck ALL Customers for Customs Cleared Coversheet
                if (!blnCustomsClearedReport && 
                    (cboCust.SelectedIndex == 0))
                {
                    if (MessageBox.Show("Are you sure you want to print the " +
                        "Customs Approved Coversheet for ALL customers?",
                        "PRINT FOR ALL CUSTOMERS?", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No) return false;
                }

                //Ck Print Date
                if (rbPrintDate.Checked)
                {
                    //Ck If Approved or Print Date ck'd
                    if (!rbApprovedDate.Checked && !rbPrintDateCriteria.Checked)
                    {
                        MessageBox.Show("Please select Approved Date or Print Date.",
                        "NO DATE SELECTION CRITERIA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    //Ck that Start or End Date is entered
                    if (txtStartDate.Text.Trim().Length == 0 && txtEndDate.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Please enter a Start Date, End Date, or both.",
                        "NO DATES FOR SELECTION CRITERIA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    //Ck that End Date <= Start Date
                    if (txtStartDate.Text.Trim().Length > 0 && txtEndDate.Text.Trim().Length > 0)
                    {
                        if (Convert.ToDateTime(txtStartDate.Text) > Convert.ToDateTime(txtEndDate.Text))
                        {
                            MessageBox.Show("The Start Date cannot be later than the End Date.",
                                "INCORRECT START/END DATES",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtStartDate.Focus();
                            return false;
                        }
                    }
                }

                //Ck VIN
                if (blnCustomsClearedReport && rbVIN.Checked)
                {
                    if (txtVIN.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Please enter the VIN#.",
                               "MISSING VIN#",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtVIN.Focus();
                        return false;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidCriteria", ex.Message);
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

        private void cboCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(blnCustomsClearedReport && !blncboCustFill) FillcboVoyage();
            SheetsNotYetPrinted();
        }

        private void cboVoyage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strval;

            try
            {
                if (blnCustomsClearedReport && !blncboVoyageFill)
                {
                    rbVoyage.Enabled = true;
                    strval = (cboVoyage.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    if (strval == "All")
                    {
                        rbVoyage.Checked = false;
                        rbVoyage.Enabled = false;
                    }
                    SheetsNotYetPrinted();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "cboVoyage+SelectedIndexChanged", ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtStartDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            { KeyPressTextbox(txtStartDate, e); }
        }

        private void txtEndDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            { KeyPressTextbox(txtEndDate, e); }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            StartRequest("DISPLAY");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            StartRequest("PRINT");
        }

        private void txtStartDate_Validating(object sender, CancelEventArgs e)
        {
            { ValidateTextbox(txtStartDate, e); }
        }

        private void txtEndDate_Validating(object sender, CancelEventArgs e)
        {
            { ValidateTextbox(txtEndDate, e); }
        }

        private void frmCustomClearedSheets_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
