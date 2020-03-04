using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Configuration;
using System.ComponentModel;
using System.IO;

namespace AutoExport
{
    public partial class frmPrintInvoices : Form
    {
        //Use for params for Export Billing records as well
     
        //CONSTANTS
        private const string CURRENTMODULE = "frmPrintInvoices";
        private const string INVOICEREPORT = "rptInvoice_1.rdlc";

        //Variables
        public PrintInfo objPrintInfo;
        private string strMode;

        public frmPrintInvoices(string strAction = "PRINT")
        {
            var appSettings = ConfigurationManager.AppSettings;
            strMode = strAction;
            
            InitializeComponent();
            
            Formops.SetFormBackground(this);    
        }

        private void frmPrintInvoices_Activated(object sender, EventArgs e)
        {
            lblPrinting.Visible = false;
            btnPrint.Enabled = true;
            btnClose.Enabled = true;

            if (strMode == "EXPORT")
            {
                this.Text = "Export Billing Records";
                lblPrinting.Text = "Exporting Invoices";
                rbUnprinted.Text = "All New Billing Records";
                rbInvDate.Text = "Export Date";
                rbPrintDate.Visible = false;
                btnPrint.Text = "Export recs";               
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {this.Close();}

        private DataTable GetExportData(ref bool blnNoData)
        {
            try
            {
                DateTime datTo;
                DataSet ds;
                string strSQL;

                strSQL = @"SELECT line.BillingLineItemsID, 
                line.CustomerID, line.BillingID, line.CustomerNumber,
                line.InvoiceNumber, line.InvoiceDate, line.DebitAccountNumber,
                line.DebitProfitCenterNumber, line.DebitCostCenterNumber,
                line.CreditAccountNumber, line.CreditProfitCenterNumber,
                line.CreditCostCenterNumber, line.ARTransactionAmount,
                line.CreditMemoInd, line.Description, line.ExportedInd, line.ExportBatchID,
                line.ExportedDate, line.ExportedBy, line.CreationDate, line.CreatedBy,
                line.UpdatedDate, line.UpdatedBy, 
                DATEPART(year,bill.InvoiceDate) AS InvYear, 
                DATENAME(Month,bill.InvoiceDate) AS InvMonth, 
                cus.CustomerOf
                FROM BillingLineItems line, 
                Billing bill, 
                Customer cus
                WHERE line.BillingID = bill.BillingID
                AND bill.CustomerID = cus.CustomerID ";

                //Add WHERE clause from form
                if (rbUnprinted.Checked)
                    strSQL += "AND ISNULL(line.ExportedInd,0) = 0 AND bill.PrintedInd = 1 ";

                if (rbDate.Checked)
                {
                    if (txtDateFrom.Text.Trim().Length > 0)
                        strSQL += @"AND line.ExportedInd = 1 
                        AND line.ExportedDate >= '" + txtDateFrom.Text.Trim() + "' ";

                    if (txtDateTo.Text.Trim().Length > 0)
                    {
                        datTo = Convert.ToDateTime(txtDateTo.Text.Trim());
                        datTo = datTo.AddDays(1);
                        strSQL += "AND line.ExportedDate < '" +
                            datTo.ToString("M/d/yyyy") + "' ";
                    }
                }

                if (rbVIN.Checked)
                    strSQL += @"AND line.BillingID = 
                    (SELECT TOP 1 BillingID FROM AutoportExportVehicles 
                    WHERE VIN = '" + txtVIN.Text.Trim() + 
                    "' ORDER BY DateShipped DESC) ";

                if (rbInvNumber.Checked)
                {
                    if (txtInvFrom.Text.Trim().Length > 0)
                        strSQL += "AND bill.InvoiceNumber >= '" +
                            txtInvFrom.Text.Trim() + "' ";

                    if (txtInvTo.Text.Trim().Length > 0)
                        strSQL += "AND bill.InvoiceNumber <= '" +
                            txtInvTo.Text.Trim() + "' ";
                }

                strSQL += @"ORDER BY cus.CustomerOf, InvYear, InvMonth, 
                line.InvoiceDate,line.InvoiceNumber, line.BillingLineItemsID";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GetExportData",
                        "No data returned");
                    return null;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    blnNoData = true;
                    MessageBox.Show("There are no Billing records for the criteria you entered.",
                        "NO BILLING RECORDS", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return null;
                }

                return (ds.Tables[0]);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetExportData", ex.Message);
                return null;
            }
        }

        private ReportDataSource GetReportDataSource(ref bool blnNoData)
        {

            DateTime datTo;
            DataSet ds;
            DataTable dtDistinct;
            DataView dv;
            int intval;
            PrintInfo objMissingSizeClass = new PrintInfo();
            string strFilter;
            string strSQL;
            string strval;

            try
            {
                blnNoData = false;

                strSQL = @"SELECT 
                0 AS rownumber,
                bill.BillingID, 
                bill.PrintedInd,
                cus.CustomerName, 
                cus.CustomerCode, 
                loc.AddressLine1, 
                RTRIM(ISNULL(loc.AddressLine2,'')) AS AddressLine2,
                RTRIM(ISNULL(loc.City,'')) AS City, 
                RTRIM(ISNULL(loc.State,'')) AS State, 
                RTRIM(ISNULL(loc.Zip,'')) AS Zip, 
                CASE 
	                WHEN RTRIM(loc.Country) = 'U.S.A.' THEN '' 
	                ELSE RTRIM(loc.Country) 
                END AS Country,
                '' AS CustomerAddress,
                'Invoice For Export Services'  AS InvoiceType, 
                bill.InvoiceNumber, 
                CONVERT(varchar(10),bill.InvoiceDate,101) AS InvoiceDate,
                CASE
					WHEN Len(veh.VIN + ' ' + veh.VehicleYear + ' ' + veh.Model) < 34 THEN
						veh.VIN + ' ' + veh.VehicleYear + ' ' + veh.Model
					ELSE
						veh.VIN + '$' + veh.VehicleYear + ' ' + veh.Model
                END AS vehinfo,
                CONVERT(varchar(10),veh.DateReceived,101) AS DateReceived,
                CONVERT(varchar(10),veh.DateShipped,101) AS DateShipped,
                veh.EntryRate AS ProcessingFee,
                ISNULL(
	                (SELECT SUM(PerDiem) FROM AutoportExportPerDiem  
	                WHERE AutoportExportVehiclesID = veh.AutoportExportVehiclesID),0) AS StorageFee,
                DATEDIFF(day,veh.DateReceived,veh.DateShipped)+1 AS StorageDays,
                CASE 
	                WHEN veh.PerDiemGraceDays > DATEDIFF(day,veh.DateReceived,veh.DateShipped)+1 THEN 0 
	                ELSE DATEDIFF(day,veh.DateReceived,veh.DateShipped)+1 - veh.PerDiemGraceDays 
                END AS BilledDays,
                veh.TotalCharge AS Rate, 
                bill.InvoiceAmount, 
                voy.VoyageNumber, 
                ves.VesselName AS Vessel, 
                ff.FreightForwarderName AS FreightForwarder,
                ex.ExporterName AS exporter
                FROM Billing bill
                LEFT JOIN Customer cus ON bill.CustomerID = cus.CustomerID
                LEFT JOIN AutoportExportVehicles veh ON bill.BillingID = veh.BillingID
                LEFT JOIN Location loc ON cus.BillingAddressID = loc.LocationID
                LEFT JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID
                LEFT JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                LEFT JOIN AEFreightForwarder ff ON veh.FreightForwarderID = ff.AEFreightForwarderID   
                LEFT JOIN AEExporter ex ON veh.ExporterID = ex.AEExporterID   
                WHERE bill.InvoiceType = 'ExportCharge'
                AND cus.DoNotPrintInvoiceInd = 0
                AND cus.BulkBillingInd = 0
                AND bill.CreditedOutInd = 0
                AND bill.CreditMemoInd = 0.0 ";

                //Add User params to WHERE clause
                if (rbUnprinted.Checked)
                    strSQL += "AND ISNULL(bill.PrintedInd,0) = 0 ";

                if (rbInvNumber.Checked)
                {
                    if (txtInvFrom.Text.Trim().Length > 0)
                        strSQL += "AND bill.InvoiceNumber >= '" +
                            txtInvFrom.Text.Trim() + "' ";

                    if (txtInvTo.Text.Trim().Length > 0)
                        strSQL += "AND bill.InvoiceNumber <= '" +
                            txtInvTo.Text.Trim() + "' ";
                }

                if (rbDate.Checked)
                {
                    if (rbInvDate.Checked)
                    {
                        if (txtDateFrom.Text.Trim().Length > 0)
                            strSQL += "AND bill.InvoiceDate >= '" +
                                txtDateFrom.Text.Trim() + "' ";

                        if (txtDateTo.Text.Trim().Length > 0)
                        {
                            datTo = Convert.ToDateTime(txtDateTo.Text.Trim());
                            datTo = datTo.AddDays(1);
                            strSQL += "AND bill.InvoiceDate < '" +
                                datTo.ToString("M/d/yyyy") + "' ";
                        }
                    }

                    if (rbPrintDate.Checked)
                    {
                        if (txtDateFrom.Text.Trim().Length > 0)
                            strSQL += "AND bill.DatePrinted >= '" +
                                txtDateFrom.Text.Trim() + "' ";

                        if (txtDateTo.Text.Trim().Length > 0)
                        {
                            datTo = Convert.ToDateTime(txtDateTo.Text.Trim());
                            datTo = datTo.AddDays(1);
                            strSQL += "AND bill.DatePrinted < '" +
                                datTo.ToString("M/d/yyyy") + "' ";
                        }
                    }
                }

                if (rbVIN.Checked)
                    strSQL += "AND veh.VIN LIKE '%" + txtVIN.Text.Trim() + "%' ";

                strSQL += " ORDER BY bill.InvoiceNumber,ff.FreightForwarderName,ex.ExporterName";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GetReportDataSource",
                        "No data returned");
                    return null;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    blnNoData = true;
                    MessageBox.Show("There is no Invoice data for the criteria you entered.",
                        "NO INVOICE DATA", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return null;
                }

                //Replace new line ('\n) with '$' in vehinfo to start a new line after VIN
                // in DataGridview
                foreach (DataRow dtrow in ds.Tables[0].Rows)
                {
                    strval = dtrow["vehinfo"].ToString();
                    if (strval.Contains("$"))
                    {
                        strval = strval.Replace("$", "\n");
                        dtrow["vehinfo"] = strval;
                    }
                }

                //Update rownumber for each row in each Inv#
                dv = new DataView(ds.Tables[0]);
                dtDistinct = dv.ToTable(true, "InvoiceNumber");

                foreach (DataRow dtrow in dtDistinct.Rows)
                {
                    strval = dtrow["InvoiceNumber"].ToString();
                    intval = 1;

                    //Get all the rows for strval(InvoiceNumber), sort by Forwarder, exporter, vehinfo (VIN)
                    dv = new DataView(ds.Tables[0], "InvoiceNumber = '" + strval + "'",
                        "FreightForwarder,exporter,vehinfo",
                        DataViewRowState.CurrentRows);
                    foreach (DataRowView dvrow in dv)
                    {
                        //Update rownumber
                        dvrow["rownumber"] = intval;
                        intval++;

                        strval = dvrow["CustomerName"].ToString().Trim();

                        //Create CustomerAddress
                        if (dvrow["AddressLine1"].ToString().Trim().Length > 0)
                            strval += "\n" + 
                                dvrow["AddressLine1"].ToString().Trim();
                        
                        if (dvrow["AddressLine2"].ToString().Trim().Length > 0)
                            strval += "\n" +
                                dvrow["AddressLine2"].ToString().Trim();

                        if (dvrow["City"].ToString().Trim().Length > 0)
                            strval += "\n" +
                                dvrow["City"].ToString().Trim();

                        if (dvrow["State"].ToString().Trim().Length > 0)
                            strval += ", " + dvrow["State"].ToString().Trim();

                        if (dvrow["Zip"].ToString().Trim().Length > 0)
                            strval += "  " + dvrow["Zip"].ToString().Trim();

                        if (dvrow["Country"].ToString().Trim().Length > 0)
                            strval += "\n" +
                                dvrow["Country"].ToString().Trim();

                        dvrow["CustomerAddress"] = strval;
                    }
                }

                // //Store DISTINCT vehIDs with no PrintedInd, in UnprintedIDs
                objPrintInfo = new PrintInfo();
                objPrintInfo.UnprintedIDs.Clear();
                strFilter = "PrintedInd = 0";
                dv = new DataView(ds.Tables[0],strFilter, "BillingID",
                     DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    dtDistinct = dv.ToTable(true, "BillingID");
                    foreach (DataRow drow in dtDistinct.Rows)
                        objPrintInfo.UnprintedIDs.Add(
                            Convert.ToInt32(drow["BillingID"].ToString()));
                }

                //Return table in ds as a ReportDataSource named 'dsLabels'
                return new ReportDataSource("dsInvoice_invoice", ds.Tables[0]);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetReportDataSource", ex.Message);
                return null;
            }
        }


        private void StartRequest()
        {
            //3/7/18 D.Maibor: Add ck for blnNoData after GetReportDataSource
            bool blnNoData = false;
            ReportDataSource rptSource;
            string strIDs;
            string strSQL;

            try
            {   
                if (ValidParams())
                {
                    //Use backgrounworker to disable buttons & display lblPrint
                    bckgrdDisplay.ReportProgress(1);

                    if (strMode == "PRINT")
                    {
                        lblPrinting.Text = "Printing Invoices";

                        rptSource = GetReportDataSource(ref blnNoData);

                        if (!blnNoData)
                        {
                            OpenReportDisplayForm(rptSource);

                            //Update PrintedInd in Billing table
                            if (objPrintInfo.UnprintedIDs.Count > 0)
                            {
                                strIDs = "AND BillingID IN (";

                                foreach (int intID in objPrintInfo.UnprintedIDs)
                                    strIDs += intID.ToString() + ",";

                                //Replace last ',' with ')'
                                strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                                strSQL = "UPDATE Billing SET PrintedInd=1," +
                                    "DatePrinted='" + DateTime.Now.ToString() + "' " +
                                    "WHERE PrintedInd=0 " + strIDs;
                                DataOps.PerformDBOperation(strSQL);
                            }
                        }
                    }
                    else
                    {
                        ExportBillingRecords();
                    }
                }                
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "StartRequest", ex.Message);
            }
        }

        private void KeyPressTextbox(TextBox txtbox, KeyPressEventArgs e)
        {
            if (!Globalitems.ValidDateKeyStroke(e.KeyChar)) e.Handled = true;
        }

        private void OpenReportDisplayForm(ReportDataSource rptSource)
        {
            DataSet ds;
            DataRow dtRow;
            frmDisplayreport frm;
            List<ReportParameter> lsParams;
            ReportParameter rptParam;
            string strIDs;
            string strDAIAddress;
            string strMsg;
            string strSQL;
            string strReportPath;

            try
            {

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

                strMsg = "Invoices";
                strReportPath = Globalitems.SetReportPath(INVOICEREPORT);

                //Fill lsParams for CustomsApprovedCoversheet report
                lsParams = new List<ReportParameter>();
                rptParam = new ReportParameter("DAIAddress", strDAIAddress);
                lsParams.Add(rptParam);

                frm = new frmDisplayreport(strMsg, strReportPath, rptSource,
                    265,700,lsParams);
                Formops.SetFormBackground(frm);
                frm.ShowDialog();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenReportDisplayForm", ex.Message);
            }
        }

        private void ExportBillingRecords()
        {
            //12/26/17 D.Maibor: return if blnNoData = true after GetExportData
            try
            {
                bool blnNoData = false;
                DataRow drow;
                DataSet ds;
                DataTable dt;
                DataTable dtDistinct;
                DataView dv;
                int intNextBatchID = -1;
                List<SProcParameter> Paramobjects = new List<SProcParameter>();
                SProcParameter objParam;
                string strFilter;
                string strIDs = "(";
                string strDirectoryPath;
                string strResult;
                string strSProc;
                string strSQL;

                //Get Directory path from SettingTable for txt file to be created
                strSQL = @"SELECT ValueDescription AS filepath 
                FROM SettingTable 
                WHERE ValueKey='BillingLineItemsExportDirectory'";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ExportBillingRecords",
                        "No data returned from SettingTable for BillingLineItemsExportDirectory");
                    return;
                }

                strDirectoryPath = ds.Tables[0].Rows[0]["filepath"].ToString();

                //Make sure the directory exists
                if (!Directory.Exists(strDirectoryPath))
                {
                    Globalitems.HandleException(CURRENTMODULE, "ExportBillingRecords",
                        "The Billing Line Items Export Directory does not exist");
                    return;
                }

                //If getting new Billing recs (rbUnprinted.Checked), 
                //get NextBillingExportBatchID
                if (rbUnprinted.Checked)
                {
                    //Get the next BatchID & increment it w/SProc
                    strSProc = "spGetBatchIDAndIncrement";

                    objParam = new SProcParameter();
                    objParam.Paramname = "@ImportType";
                    objParam.Paramvalue = "EXPORT";
                    Paramobjects.Add(objParam);
                    ds = DataOps.GetDataset_with_SProc(strSProc, Paramobjects);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "ExportBillingRecords",
                            "No data returned after invoking SProc for BatchID");
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
                        Globalitems.HandleException(CURRENTMODULE, "ExportBillingRecords",
                            strResult);
                        return;
                    }

                    intNextBatchID = (int)drow["batchID"];
                }

                //Get all the Export data
                dt = GetExportData(ref blnNoData);

                if (blnNoData) return;

                //  A new file is created for each set of rows with the same 
                // InvYear/InvMonth/CustomerOf 
                // Place the distinct InvYear/Inv/Month/CustomerOf tuples into dtDistinct
                // then call CreateBillingRecordFile for each tuple
                dv = new DataView(dt);
                dtDistinct = dv.ToTable(true, "InvYear", "InvMonth", "CustomerOf");
                foreach (DataRow dtrow in dtDistinct.Rows)
                {
                    //Create a DataView from dt with the recs meeting dtDistinct fields
                    strFilter = "InvYear = " + dtrow["InvYear"] +
                        " AND InvMonth = '" + dtrow["InvMonth"] +
                        "' AND CustomerOf = '" + dtrow["CustomerOf"] + "'";
                    
                    dv = new DataView(dt, strFilter, 
                        "InvoiceDate,InvoiceNumber,BillingLineItemsID", 
                        DataViewRowState.CurrentRows);

                    //Create the file for the current dv
                    CreateBillingRecordFile(strDirectoryPath, dv,intNextBatchID);
                }

                //Update BillingLineItems table if New Billing recs selected (rbUnprinted.Checked)
                if (rbUnprinted.Checked)
                {
                    //Put all the BillingLineItemsIDs in strIDs 
                    foreach (DataRow dtrow in dt.Rows)
                        strIDs += dtrow["BillingLineItemsID"].ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ")";

                    //Update BillingLineItems table
                    strSQL = @"UPDATE BillingLineItems 
                    SET ExportedInd = 1, ExportedDate = CURRENT_TIMESTAMP, 
                    ExportedBy = '" + Globalitems.strUserName + "'," +
                    "ExportBatchID = " + intNextBatchID + "," +
                    @"UpdatedDate = CURRENT_TIMESTAMP,
                    UpdatedBy = '" + Globalitems.strUserName + "' " +
                    "WHERE BillingLineItemsID IN " + strIDs;

                    DataOps.PerformDBOperation(strSQL);
                }

                //Let user know everything worked
                MessageBox.Show("Export Complete - " + dt.Rows.Count.ToString() +
                    " billing records exported successfully!");

                //Open Billing Export Backup report
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ExportBillingRecords", ex.Message);
            }
        }

        private void CreateBillingRecordFile(string strDirPath, DataView dv, 
            int intNextBatchID)
        {
            bool blnOKFileName = true;
            int intFileCounter = 1;
            string strFilename;
            StreamWriter strmWriter;

            try
            {
                //Create the filename, up to the last digit (filecounter)
                strFilename = strDirPath + @"\I" + dv[0]["CustomerOf"].ToString() +
                    dv[0]["InvMonth"].ToString().Substring(0, 3);

                if (File.Exists(strFilename + intFileCounter.ToString() + ".txt"))
                {
                    blnOKFileName = false;
                    while (!blnOKFileName)
                    {
                        intFileCounter++;

                        if (!File.Exists(strFilename + intFileCounter.ToString() + ".txt"))
                            blnOKFileName = true;

                        if (intFileCounter >= 1000)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "ExportBillingRecords",
                                "intFileCounter is >= 1000");
                            return;
                        }
                    }
                }

                //Create the file with StreamWriter
                strmWriter = new StreamWriter(strFilename + intFileCounter.ToString() + ".txt");

                //Write each line into file from dv with the format
                //Cols: 1-6,total col. size: 6, CustomerNumber (PadRight) 
                //Cols: 7-16,total col. size: 10, InvoiceNumber (PadRight) 
                //Cols: 17-24,total col. size: 8, InvoiceDate (MM/DD/YYYY)
                //Cols: 25-29, total col. size: 5, DebitAccountNumber (PadRight)
                //Cols: 30-31, total col. size: 2, DebitProfitCenterNumber (PadRight)
                //Cols: 32-34, total col. size: 3, DebitCostCenterNumber (PadRight)
                //Cols: 35-39, total col. size: 5, CreditAccountNumber (PadRight)
                //Cols: 40-41, total col. size: 2, CreditProfitCenterNumber (PadRight)
                //Cols: 42-44, total col. size: 3, CreditCostCenterNumber (PadRight)
                //Cols: 45-52, total col. size: 8, ARTransactionAmount (PadRight)
                //Cols: 53-53, total col. size: 1, CreditMemoInd (PadRight)
                //Cols: 54-83, total col. size: 30, Description (PadRight)
                //Cols: 84-84, total col. size: 1, 'Z'
                foreach (DataRowView dvrow in dv)
                {
                    strmWriter.Write(dvrow["CustomerNumber"].ToString().PadRight(6));
                    strmWriter.Write(dvrow["InvoiceNumber"].ToString().PadRight(10));

                    //Write InvoiceDate in MMddyyyy format
                    strmWriter.Write(Convert.ToDateTime(dvrow["InvoiceDate"]).ToString("MMddyyyy"));
                    strmWriter.Write(dvrow["DebitAccountNumber"].ToString().PadRight(5));
                    strmWriter.Write(dvrow["DebitProfitCenterNumber"].ToString().PadRight(2));
                    strmWriter.Write(dvrow["DebitCostCenterNumber"].ToString().PadRight(3));
                    strmWriter.Write(dvrow["CreditAccountNumber"].ToString().PadRight(5));
                    strmWriter.Write(dvrow["CreditProfitCenterNumber"].ToString().PadRight(2));
                    strmWriter.Write(dvrow["CreditCostCenterNumber"].ToString().PadRight(3));
                    strmWriter.Write(dvrow["ARTransactionAmount"].ToString().PadRight(8));

                    //NOTE: As of 9/13/17, all the 253,335 records in the BillingLineItems table
                    //  have a 0 in the CreditMemoInd field. Doesn't look like it was ever used.
                    //If CreditMemoInd = 0 write space
                    if ((int)dvrow["CreditMemoInd"] == 0)
                        strmWriter.Write(" ");
                    else
                        strmWriter.Write("1");

                    strmWriter.Write(dvrow["Description"].ToString().PadRight(30));
                    strmWriter.WriteLine("Z");
                }

                strmWriter.Flush();
                strmWriter.Close();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CreateBillingRecordFile", ex.Message);
            }
        }

        private ReportDataSource CreateReportDataSource()
        {
            DataColumn col;
            DataRow drow;
            DataTable tbl;
            ReportDataSource rptDataSource;

            try
            {
                tbl = new DataTable("dsLabels_Label");

                col = new DataColumn("Destination");
                col.DataType = typeof(string);
                tbl.Columns.Add(col);

                col = new DataColumn("Customer");
                col.DataType = typeof(string);
                tbl.Columns.Add(col);

                col = new DataColumn("MakeModel");
                col.DataType = typeof(string);
                tbl.Columns.Add(col);

                col = new DataColumn("VIN_alpha");
                col.DataType = typeof(string);
                tbl.Columns.Add(col);

                col = new DataColumn("SizeClass");
                col.DataType = typeof(string);
                tbl.Columns.Add(col);

                col = new DataColumn("VIN_barcode");
                col.DataType = typeof(string);
                tbl.Columns.Add(col);

                col = new DataColumn("Baylocation");
                col.DataType = typeof(string);
                tbl.Columns.Add(col);

                col = new DataColumn("DateReceived");
                col.DataType = typeof(string);
                tbl.Columns.Add(col);

                drow = tbl.NewRow();
                drow["Destination"] = "Lagos";
                drow["Customer"] = "Glovis";
                drow["MakeModel"] = "Toyota Highlander";
                drow["VIN_alpha"] = "JTEHF21A310035245";
                drow["SizeClass"] = "Size: B";
                drow["VIN_barcode"] = "*JTEHF21A310035245*";
                drow["Baylocation"] = "Bay: XX08 03";
                drow["DateReceived"] = "Rec: 11/08/2016";
                tbl.Rows.Add(drow);

                drow = tbl.NewRow();
                drow["Destination"] = "Lagos";
                drow["Customer"] = "Sallaum";
                drow["MakeModel"] = "Toyota Highlander";
                drow["VIN_alpha"] = "JTEHF21A310035245";
                drow["SizeClass"] = "Size: B";
                drow["VIN_barcode"] = "*JTEHF21A310035245*";
                drow["Baylocation"] = "Bay: XX08 03";
                drow["DateReceived"] = "Rec: 11/08/2016";
                tbl.Rows.Add(drow);

                drow = tbl.NewRow();
                drow["Destination"] = "Lagos";
                drow["Customer"] = "LGL";
                drow["MakeModel"] = "Toyota Highlander";
                drow["VIN_alpha"] = "JTEHF21A310035245";
                drow["SizeClass"] = "Size: B";
                drow["VIN_barcode"] = "*JTEHF21A310035245*";
                drow["Baylocation"] = "Bay: XX08 03";
                drow["DateReceived"] = "Rec: 11/08/2016";
                tbl.Rows.Add(drow);

                drow = tbl.NewRow();
                drow["Destination"] = "Lagos";
                drow["Customer"] = "Glovis";
                drow["MakeModel"] = "Toyota Highlander";
                drow["VIN_alpha"] = "JTEHF21A310035245";
                drow["SizeClass"] = "Size: B";
                drow["VIN_barcode"] = "*JTEHF21A310035245*";
                drow["Baylocation"] = "Bay: XX08 03";
                drow["DateReceived"] = "Rec: 11/08/2016";
                tbl.Rows.Add(drow);


                drow = tbl.NewRow();
                drow["Destination"] = "Lagos";
                drow["Customer"] = "Sallaum";
                drow["MakeModel"] = "Toyota Highlander";
                drow["VIN_alpha"] = "JTEHF21A310035245";
                drow["SizeClass"] = "Size: B";
                drow["VIN_barcode"] = "*JTEHF21A310035245*";
                drow["Baylocation"] = "Bay: XX08 03";
                drow["DateReceived"] = "Rec: 11/08/2016";
                tbl.Rows.Add(drow);

                drow = tbl.NewRow();
                drow["Destination"] = "Lagos";
                drow["Customer"] = "LGL";
                drow["MakeModel"] = "Toyota Highlander";
                drow["VIN_alpha"] = "JTEHF21A310035245";
                drow["SizeClass"] = "Size: B";
                drow["VIN_barcode"] = "*JTEHF21A310035245*";
                drow["Baylocation"] = "Bay: XX08 03";
                drow["DateReceived"] = "Rec: 11/08/2016";
                tbl.Rows.Add(drow);

                rptDataSource = new ReportDataSource("dsLabels", tbl);
                return rptDataSource;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CreateReport", ex.Message);
                return null;
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

        private bool ValidParams()
        {
            try
            {
                DateTime datFrom;
                DateTime datTo;

                //Make sure rb is selected
                if (!rbUnprinted.Checked && !rbInvNumber.Checked && !rbDate.Checked &&
                    !rbVIN.Checked)
                {
                    if (strMode == "PRINT")
                        MessageBox.Show("Please select the criteria: All Unprinted/Invoice Number/Invoice Date/VIN ",
                        "MISSING SELECTION CRITERIA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Please select the criteria: All New Billing Recs/Invoice Number/Invoice Date/VIN ",
                        "MISSING SELECTION CRITERIA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //Make sure at least one Inv# provided
                if (rbInvNumber.Checked)
                {
                    if (txtInvFrom.Text.Trim().Length +
                        txtInvTo.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Please enter Inv# From, Inv# To, or both",
                        "MISSING INVOICE NUMBER INFO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }    
                }

                //Make sure Date info provided
                if (rbDate.Checked)
                {
                    if (strMode=="PRINT" && !rbInvDate.Checked && !rbPrintDate.Checked)
                    {
                        MessageBox.Show("Please select Invoice Date or Print Date",
                       "MISSING INVOICE DATE CRITERIA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    //Make sure at least one date is entered
                    if (txtDateFrom.Text.Trim().Length +
                        txtDateTo.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Please enter Date From, Date To, or both",
                        "MISSING DATE INFO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    //Make sure To Date is not Before From Date, if both entered
                    if (txtDateFrom.Text.Trim().Length > 0 && 
                        txtDateTo.Text.Trim().Length > 0)
                    {
                        datFrom = Convert.ToDateTime(txtDateFrom.Text.Trim());
                        datTo = Convert.ToDateTime(txtDateTo.Text.Trim());
                        if (datTo < datFrom)
                        {
                            MessageBox.Show("The Date To cannot be BEFORE the Date From.",
                        "INCORRECT DATE INFO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }

                //Ck VIN
                if (rbVIN.Checked && txtVIN.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter the VIN",
                       "MISSING VIN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtVIN.Focus();
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidParams", ex.Message);
                return false;
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {StartRequest();}

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }

        private void rbUnprinted_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUnprinted.Checked)
            {
                pnlDate.Visible = false;
                pnlInvoice.Visible = false;
                txtVIN.Visible = false;
            }
        }

        private void rbDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDate.Checked)
            {
                pnlDate.Visible = true;
                pnlInvoice.Visible = false;
                txtVIN.Visible = false;

                if (strMode != "PRINT") rbInvDate.Checked = true;
            }
        }

        private void rbInvNumber_CheckedChanged(object sender, EventArgs e)
        {
            if (rbInvNumber.Checked)
            {
                pnlInvoice.Visible = true;
                pnlDate.Visible = false;
                txtVIN.Visible = false;
            }
        }

        private void txtDateFrom_KeyPress(object sender, KeyPressEventArgs e)
            { KeyPressTextbox(txtDateFrom, e); }

        private void txtDateTo_KeyPress(object sender, KeyPressEventArgs e)
            { KeyPressTextbox(txtDateTo, e); }

        private void txtDateFrom_Validating(object sender, System.ComponentModel.CancelEventArgs e)
            { ValidateTextbox(txtDateFrom, e); }

        private void txtDateTo_Validating(object sender, CancelEventArgs e)
            {ValidateTextbox(txtDateTo, e);}

        private void rbVIN_CheckedChanged(object sender, EventArgs e)
        {
            if (rbVIN.Checked)
            {
                pnlInvoice.Visible = false;
                pnlDate.Visible = false;
                txtVIN.Visible = true;
            }
        }

        private void bckgrdDisplay_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Disable buttons and show lblPrint
            btnPrint.Enabled = false;
            btnClose.Enabled = false;
            lblPrinting.Visible = true;
        }

        private void frmPrintInvoices_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
