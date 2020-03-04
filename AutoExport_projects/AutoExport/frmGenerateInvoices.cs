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
    public partial class frmGenerateInvoices: Form
    {
        private const string CURRENTMODULE = "frmGenerateInvoices";
        private const string INVOICESUMMARYREPORT = "rptInvoiceSummary.rdlc";

        private bool blnFillGrid = true;
        private string strSheetPrinter;

        public frmGenerateInvoices()
        {
            var appSettings = ConfigurationManager.AppSettings;

            InitializeComponent();

            dgResults.AutoGenerateColumns = false;

            //Read appSettings from app.config file for SheetPrinter
            strSheetPrinter = appSettings["SheetPrinter"];
        }

        private void frmGenerateInvoices_Activated(object sender, EventArgs e)
        {
            if (blnFillGrid) FillGrid();
        }

        private void ClearProcessed()
        {
            try
            {
                DataTable dt = (DataTable) dgResults.DataSource;
                DataView dv;
                string strFilter = "";

                //Get rowIDs of all rows without Status of 'Processed'
                foreach (DataGridViewRow dgRow in dgResults.Rows)
                {
                    if (dgRow.Cells["RecordStatus"].Value.ToString() != "Processed")
                        strFilter += dgRow.Cells["rowID"].Value.ToString() + ",";
                }

                if (strFilter.Length == 0)
                {
                    dgResults.DataSource = null;
                    btnGenInvoices.Enabled = false;
                    btnClearProcessed.Enabled = false;
                    return;
                }

                //Remove last ','
                strFilter = strFilter.Substring(0, strFilter.Length - 1);

                //Place strFilter in correct format
                strFilter = "rowID IN (" + strFilter + ")";

                //Create a dataview of remaining rows
                dv = new DataView(dt, strFilter, "rowID", DataViewRowState.CurrentRows);

                //Change dgResults to table from dv
                dgResults.DataSource = dv.ToTable();

                btnClearProcessed.Enabled = false;
                               
            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearProcessed", ex.Message);
            }
        }

        private void FillGrid()
        {
            //4/15/19 D.Maibor. Add alias DateShipped to field so it is included in datagrid
            //4/10/19 D.Maibor. Change DateShipped to CONVERT(varchar(10),DateShipped) so it groups all vehs on voyage
            //  together
            //3/21/19 D.Maibor. Add back DateShipped to query; needed for ShippedbyTruck invoices   
            //6/19/18 D.Maibor. Remove DateShipped in query
            //2/21/18 D.Maibor. Add code to ensure BillToInd = 0 WHEN BillToCustomerID is null or 0
            try
            {
                DataSet ds;
                string strSQL;

                strSQL = @"UPDATE AutoportExportVehicles SET BillToInd = 0 
                         WHERE DateShipped IS NOT NULL 
                         AND ISNULL(CreditHoldInd,0) = 0
                         AND ISNULL(BillToCustomerID,0) = 0";
                DataOps.PerformDBOperation(strSQL);


                strSQL = @"
                SELECT 
                -- Shipped to Customer
                COUNT(*) AS units,
                veh.VoyageID,
                veh.CustomerID,
                ISNULL(veh.BillToInd,0) AS BillToInd,
                veh.BillToCustomerID,
                CASE
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName
	                ELSE cus.CustomerName
                END AS customer,
                CASE
	                WHEN DATALENGTH(ISNULL(billtocus.ShortName,'')) > 0 THEN billtocus.ShortName
	                ELSE ISNULL(billtocus.CustomerName,'') 
                END AS billtocustomer,
                CONVERT(varchar(10),voy.VoyageDate,101) + ' ' + ves.VesselName AS voyinfo,
                'Uninvoiced' AS RecordStatus,
                CONVERT(varchar(10),DateShipped,101) AS DateShipped
                FROM
                AutoportExportVehicles veh
                LEFT OUTER JOIN Customer cus on cus.CustomerID=veh.CustomerID
                LEFT OUTER JOIN Customer billtocus on billtocus.CustomerID=veh.BilltoCustomerID
                LEFT OUTER JOIN AEVoyage voy on voy.AEVoyageID=veh.VoyageID
                LEFT OUTER JOIN AEVessel ves on ves.AEVesselID=voy.AEVesselID
                WHERE 
                veh.DateShipped IS NOT NULL
                AND ISNULL(BilledInd,0) = 0
                AND ISNULL(CreditHoldInd,0) = 0
                AND veh.VehicleStatus <> 'ShippedByTruck'
                AND ISNULL(BillToInd,0) = 0
                GROUP BY
                veh.VoyageID,
                veh.CustomerID,
                ISNULL(BillToInd,0),
                veh.BillToCustomerID,
                CASE
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName
	                ELSE cus.CustomerName
                END,
                CASE
	                WHEN DATALENGTH(ISNULL(billtocus.ShortName,'')) > 0 THEN billtocus.ShortName
	                ELSE ISNULL(billtocus.CustomerName,'') 
                END,
                CONVERT(varchar(10),voy.VoyageDate,101) + ' ' + ves.VesselName,
               CONVERT(varchar(10),DateShipped,101)
                UNION
                -- Shipped, BilledToCustomer
                SELECT 
                COUNT(*) AS units,
                veh.VoyageID,
                veh.CustomerID,
                ISNULL(veh.BillToInd,0) AS BillToInd,
                veh.BillToCustomerID,
                CASE
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName
	                ELSE cus.CustomerName
                END AS customer,
                CASE
	                WHEN DATALENGTH(ISNULL(billtocus.ShortName,'')) > 0 THEN billtocus.ShortName
	                ELSE ISNULL(billtocus.CustomerName,'') 
                END AS billtocustomer,
                CONVERT(varchar(10),voy.VoyageDate,101) + ' ' + ves.VesselName AS voyinfo,
                'Uninvoiced' AS RecordStatus,
                CONVERT(varchar(10),DateShipped,101) AS DateShipped
                FROM
                AutoportExportVehicles veh
                LEFT OUTER JOIN Customer cus on cus.CustomerID=veh.CustomerID
                LEFT OUTER JOIN Customer billtocus on billtocus.CustomerID=veh.BilltoCustomerID
                LEFT OUTER JOIN AEVoyage voy on voy.AEVoyageID=veh.VoyageID
                LEFT OUTER JOIN AEVessel ves on ves.AEVesselID=voy.AEVesselID
                WHERE veh.DateShipped IS NOT NULL
                AND ISNULL(BilledInd,0) = 0
                AND ISNULL(veh.CreditHoldInd,0) = 0
                AND veh.VehicleStatus <> 'ShippedByTruck'
                AND ISNULL(veh.BillToInd,0) = 1
                GROUP BY
                veh.VoyageID,
                veh.CustomerID,
                ISNULL(BillToInd,0),
                veh.BillToCustomerID,
                CASE
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName
	                ELSE cus.CustomerName
                END,
                CASE
	                WHEN DATALENGTH(ISNULL(billtocus.ShortName,'')) > 0 THEN billtocus.ShortName
	                ELSE ISNULL(billtocus.CustomerName,'') 
                END,
                CONVERT(varchar(10),voy.VoyageDate,101) + ' ' + ves.VesselName,
                CONVERT(varchar(10),DateShipped,101)
                UNION
                -- Shipped By Truck, Customer
                SELECT 
                COUNT(*) AS units,
                veh.VoyageID,
                veh.CustomerID,
                ISNULL(veh.BillToInd,0) AS BillToInd,
                veh.BillToCustomerID,
                CASE
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName
	                ELSE cus.CustomerName
                END AS customer,
                CASE
	                WHEN DATALENGTH(ISNULL(billtocus.ShortName,'')) > 0 THEN billtocus.ShortName
	                ELSE ISNULL(billtocus.CustomerName,'') 
                END AS billtocustomer,
                CONVERT(varchar(10),veh.DateShipped,101) + ' Shipped By Truck ' AS voyinfo,
                'Uninvoiced' AS RecordStatus,
                CONVERT(varchar(10),DateShipped,101) AS DateShipped
                FROM
                AutoportExportVehicles veh
                LEFT OUTER JOIN Customer cus on cus.CustomerID=veh.CustomerID
                LEFT OUTER JOIN Customer billtocus on billtocus.CustomerID=veh.BilltoCustomerID
                LEFT OUTER JOIN AEVoyage voy on voy.AEVoyageID=veh.VoyageID
                LEFT OUTER JOIN AEVessel ves on ves.AEVesselID=voy.AEVesselID
                WHERE veh.DateShipped IS NOT NULL
                AND ISNULL(BilledInd,0) = 0
                AND ISNULL(veh.CreditHoldInd,0) = 0
                AND veh.VehicleStatus = 'ShippedByTruck'
                AND ISNULL(veh.BillToInd,0) = 0
                GROUP BY
                veh.VoyageID,
                veh.CustomerID,
                ISNULL(BillToInd,0),
                veh.BillToCustomerID,
                CASE
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName
	                ELSE cus.CustomerName
                END,
                CASE
	                WHEN DATALENGTH(ISNULL(billtocus.ShortName,'')) > 0 THEN billtocus.ShortName
	                ELSE ISNULL(billtocus.CustomerName,'') 
                END,
                CONVERT(varchar(10),voy.VoyageDate,101) + ' ' + ves.VesselName,
                CONVERT(varchar(10),veh.DateShipped,101),
                CONVERT(varchar(10),DateShipped,101)
                UNION
                -- Shipped by Truck, BilledToCustomer
                SELECT 
                COUNT(*) AS units,
                veh.VoyageID,
                veh.CustomerID,
                ISNULL(veh.BillToInd,0) AS BillToInd,
                veh.BillToCustomerID,
                CASE
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName
	                ELSE cus.CustomerName
                END AS customer,
                CASE
	                WHEN DATALENGTH(ISNULL(billtocus.ShortName,'')) > 0 THEN billtocus.ShortName
	                ELSE ISNULL(billtocus.CustomerName,'') 
                END AS billtocustomer,
                CONVERT(varchar(10),veh.DateShipped,101) + ' Shipped By Truck ' AS voyinfo,
                'Uninvoiced' AS RecordStatus,
               CONVERT(varchar(10),DateShipped,101) AS DateShipped
                FROM
                AutoportExportVehicles veh
                LEFT OUTER JOIN Customer cus on cus.CustomerID=veh.CustomerID
                LEFT OUTER JOIN Customer billtocus on billtocus.CustomerID=veh.BilltoCustomerID
                LEFT OUTER JOIN AEVoyage voy on voy.AEVoyageID=veh.VoyageID
                LEFT OUTER JOIN AEVessel ves on ves.AEVesselID=voy.AEVesselID
                WHERE veh.DateShipped IS NOT NULL
                AND ISNULL(veh.BilledInd,0) = 0
                AND ISNULL(veh.CreditHoldInd,0) = 0
                AND veh.VehicleStatus = 'ShippedByTruck'
                AND ISNULL(veh.BillToInd,0) = 1
                GROUP BY
                veh.VoyageID,
                veh.CustomerID,
                ISNULL(BillToInd,0),
                veh.BillToCustomerID,
                CASE
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName
	                ELSE cus.CustomerName
                END,
                CASE
	                WHEN DATALENGTH(ISNULL(billtocus.ShortName,'')) > 0 THEN billtocus.ShortName
	                ELSE ISNULL(billtocus.CustomerName,'') 
                END,
                CONVERT(varchar(10),veh.DateShipped,101) + ' Shipped By Truck ',
                CONVERT(varchar(10),DateShipped,101)
                ORDER BY voyinfo,customer,billtocustomer";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillGrid",
                        "No table returned from query");
                    return;
                }

                dgResults.DataSource = ds.Tables[0];

                btnGenInvoices.Enabled = false;
                lblInvRecords.Text = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnGenInvoices.Enabled = true;
                    lblInvRecords.Text = "Records: " + ds.Tables[0].Rows.Count.ToString("#,##0");
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillGrid", ex.Message);
            }
        }

        private void GenerateInvoices()
        {
            try
            {
                DataSet ds;
                List<SProcParameter> lsParams = new List<SProcParameter>();
                SProcParameter objParam;
                string strCustVoypairs = "";
                string strSproc = "spGenerateInvoices";
                string strResult;

                blnFillGrid = false;

                //Make sure at least one row is selected
                if (dgResults.SelectedRows.Count == 0)
                {
                    MessageBox.Show("You must select at least one row.", "NO ROWS SELECTED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Make sure all rows selected have status of 'Uninvoiced'
                foreach (DataGridViewRow dgRow in dgResults.SelectedRows)
                {
                    if (dgRow.Cells["RecordStatus"].Value.ToString() != "Uninvoiced")
                    {
                        MessageBox.Show("You can only generate invoices for rows with 'Uninvoiced' status.", 
                            "INVALID ROWS SELECTED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //Create strCustVoypairs for selected rows, with different delimiters for SProc
                //If Voyage is NOT NULL and Bill To Customer is NULL, CustID$VoyageID
                //If Voyage is NOT NULL and Bill To Customer is NOT NULL BillToCustID~VoyageID
                //If voyage is NULL and Bill To Customer is NULL, CustID#DateShipped
                //If voyage is NULL and Bill To Customer is NOT NULL, BillToCustID^DateShipped
                foreach (DataGridViewRow dgRow in dgResults.SelectedRows)
                {
                    if (dgRow.Cells["VoyageID"].Value.ToString().Length > 0)
                    {
                        //Row has voyage assignment
                        if (dgRow.Cells["BillToCustomerID"].Value.ToString().Length == 0)
                            strCustVoypairs += dgRow.Cells["CustomerID"].Value.ToString() +
                            "$" + dgRow.Cells["VoyageID"].Value.ToString() + ",";
                        else
                            strCustVoypairs += dgRow.Cells["BillToCustomerID"].Value.ToString() +
                            "~" + dgRow.Cells["VoyageID"].Value.ToString() + ",";
                    }  
                    else
                        //No voyage assignment, ShippedByTruck
                        if (dgRow.Cells["BillToCustomerID"].Value.ToString().Length == 0)
                            strCustVoypairs += dgRow.Cells["CustomerID"].Value.ToString() +
                            "#" + dgRow.Cells["DateShipped"].Value.ToString() + ",";
                        else
                        strCustVoypairs += dgRow.Cells["BillToCustomerID"].Value.ToString() +
                            "^" + dgRow.Cells["DateShipped"].Value.ToString() + ",";
                }
                    
                //Remove last "," in strCustVoypairs
                strCustVoypairs = strCustVoypairs.Substring(0, strCustVoypairs.Length - 1);

                //Params foSproc
                objParam = new SProcParameter();
                objParam.Paramname = "@custvoypairs";
                objParam.Paramvalue = strCustVoypairs;
                lsParams.Add(objParam);

                objParam = new SProcParameter();
                objParam.Paramname = "@user";
                objParam.Paramvalue = Globalitems.strUserName;
                lsParams.Add(objParam);

                //Invoke SProc
                ds = DataOps.GetDataset_with_SProc(strSproc, lsParams);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GenerateInvoices",
                        "No data returned from Sproc.");
                    return;
                }

                strResult = ds.Tables[0].Rows[0]["result"].ToString();
                {
                    if (strResult != "OK")
                    {
                        MessageBox.Show(strResult, "CANNOT UPDATE STATUS",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //Update status for selected rows to 'Processed
                    foreach (DataGridViewRow dgRow in dgResults.SelectedRows)
                        dgRow.Cells["RecordStatus"].Value = "Processed";

                    btnClearProcessed.Enabled = true;

                    InvoiceSummaryReport(ds.Tables[0]);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GenerateInvoices", ex.Message);
            }
        }

        private void InvoiceSummaryReport(DataTable dt)
        {

            try
            {
                DataSet ds;
                DataRow dtRow;
                List<ReportParameter> lsParams;
                ReportParameter rptParam;
                ReportDataSource rptSource;
                string strBillingIDs = "";
                string strDAIAddress;
                string strReport = "";
                string strSQL;

                strReport = Globalitems.SetReportPath(INVOICESUMMARYREPORT);

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

                //Get BillingIDs
                strBillingIDs = "bill.BillingID IN (";
                foreach (DataRow dtrow in dt.Rows)
                    strBillingIDs += dtrow["BillingID_new"].ToString() + ",";

                //Replace last "," in strBillingIDs with ")"
                strBillingIDs = strBillingIDs.Substring(0, strBillingIDs.Length - 1) + ")";

                //Get data for report
                strSQL = @"SELECT DISTINCT 
                cus.CustomerCode,
                bill.InvoiceNumber,
                CONVERT(varchar(10),bill.InvoiceDate,101) AS InvoiceDate,
                bill.InvoiceAmount,
                CASE 
	                WHEN DATALENGTH(cus.ShortName)>0 THEN cus.ShortName 
	                ELSE cus.CustomerName 
                END Customer,
                CASE
					WHEN veh.VoyageID IS NULL THEN 'Shipped By Truck - ' + CONVERT(varchar(10),veh.DateShipped,101)
					ELSE ves.VesselName
                END as Vessel
                FROM Billing bill
                LEFT JOIN Customer cus ON cus.CustomerID=bill.CustomerID
                LEFT JOIN AutoportExportVehicles veh ON veh.BillingID=bill.BillingID
                LEFT JOIN AEVoyage voy ON voy.AEVoyageID=veh.VoyageID
                LEFT JOIN AEVessel ves ON ves.AEVesselID=voy.AEVesselID
                WHERE bill.InvoiceType = 'ExportCharge' AND " + strBillingIDs;

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GroundedSummaryReport",
                        "No datatable returned");
                    return;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("There is no data for a Grounded Summary report",
                        "NO DATA FOR REPORT", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                //Return table in ds as a ReportDataSource named 'dsCustomsApprovedSheets'
                rptSource = new ReportDataSource("dsInvoiceSummary",
                    ds.Tables[0]);

                //Fill lsParams for CustomsApprovedCoversheet report
                lsParams = new List<ReportParameter>();
                rptParam = new ReportParameter("DAIAddress", strDAIAddress);
                lsParams.Add(rptParam);

                OpenReportDisplayForm(rptSource, strReport, "Invoice Summary", lsParams);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SummaryInvoiceReport", ex.Message);
            }
        }

        private void OpenReportDisplayForm(ReportDataSource rptSource, string strReportPath,
            string strMsg,
            List<ReportParameter> lsParams = null)
        {
            frmDisplayreport frm;
            try
            {
                frm = new frmDisplayreport(strMsg, strReportPath, rptSource,
                    900, 1100, lsParams);
                Formops.SetFormBackground(frm);
                frm.ShowDialog();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenReportDisplayForm", ex.Message);
            }
        }

        private void OpenVehicleDetailForm(string strFormMode)
        {
           

            try
            {
                
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenVehicleDetailForm", ex.Message);
            }
        }

        private void PrintInvoices()
        {
            frmPrintInvoices frm;

            try
            {
                frm = new frmPrintInvoices();
                frm.ShowDialog();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PrintInvoices", ex.Message);
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            //Make sure Main form displays and has the focus
            Globalitems.MainForm.Visible = true;
            Globalitems.MainForm.Focus();
        }

        private void btnGenInvoices_Click(object sender, EventArgs e)
        { GenerateInvoices(); }

        private void btnPrintInvoices_Click(object sender, EventArgs e)
        { PrintInvoices(); }

        private void btnClearProcessed_Click(object sender, EventArgs e)
        { ClearProcessed(); }

        private void frmGenerateInvoices_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}

       
    }
}
