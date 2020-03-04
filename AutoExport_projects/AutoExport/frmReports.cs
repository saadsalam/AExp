using AutoExport.Objects;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmReports : Form
    {
        private const string CUSTOMSSUBMITTEDREPORT = "rptCustomsSubmitted.rdlc";
        private const string CURRENTMODULE = "frmReports";
        private const string GROUNDEDSUMMARYREPORT = "rptGroundedSummary.rdlc";
        private const string GROUNDEDLANESUMMARYREPORT = "rptGroundedLaneSummary.rdlc";
        private const string VOYAGEPUSHCARLISTREPORT = "rptVoyagePushCarList.rdlc";

        private string strSheetPrinter;

        public frmReports()
        {
            var appSettings = ConfigurationManager.AppSettings;

            InitializeComponent();

            //Read appSettings from app.config file for SheetPrinter
            strSheetPrinter = appSettings["SheetPrinter"];


            if (Globalitems.blnCannotPrintLabels)
            {
                btnLabels.Enabled = false;
                btnNoLabels.Visible = true;
            }
        }

        private void CustomsClearedCoversheet()
        {
            try
            {
                frmCustomClearedSheets frm;
                PrintInfo objPrintInfo = new PrintInfo();

                objPrintInfo.Message = "";


                //Use Show method if not currently open
                if (Application.OpenForms.OfType<frmCustomClearedSheets>().Count() == 0)
                {
                    frm = new frmCustomClearedSheets(objPrintInfo);
                    Formops.SetFormBackground(frm);
                    frm.blnCustomsClearedReport = false;
                    frm.Show();
                }
                else //Already open, set as Active form
                {
                    frm = (frmCustomClearedSheets)Application.OpenForms["frmCustomClearedSheets"];
                    frm.objPrintInfo = objPrintInfo;
                    frm.blnCustomsClearedReport = false;
                    frm.Activate();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CustomsClearedcoversheet",
                    ex.Message);
            }
        }

        private void CustomsSubmitted()
        //12/20/18 D.Maibor: allow for blank start or end date when printing date range
        {
            try
            {
                DialogResult dlResult;
                DataSet ds;
                DataRow dtRow;
                frmPrintSelection frm = new frmPrintSelection();
                List<ReportParameter>  lsParams;
                ReportParameter rptParam;
                ReportDataSource rptSource;
                string strDAIAddress;
                string strFromDate = "";
                string[] strParams;
                string[] strNameValuePair;
                string strReport = "";
                string strReportTitle = "Export Vehicles Submitted To Customs ";
                string strSQL;
                string strToDate = "";

                dlResult = frm.ShowDialog();
                if (dlResult == DialogResult.OK)
                {
                    strReport = Globalitems.SetReportPath(CUSTOMSSUBMITTEDREPORT);

                    if (Globalitems.strSelection != "unprinted")
                    {

                        //Decode returned selection into 1-2 records
                        strParams = Globalitems.strSelection.Split(Globalitems.chrRecordSeparator);

                        if (strParams.Length == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "CustomsSubmitted",
                                "No elements split into strParams after frmPrintSelection.\n" +
                                "strSelection: " + Globalitems.strSelection);
                            return;
                        }

                        foreach (string strParam in strParams)
                        {
                            if (strParam.Length > 0)
                            {
                                strNameValuePair = strParam.Split(Globalitems.chrNameValueSeparator);
                                if (strNameValuePair.Length == 0)
                                {
                                    Globalitems.HandleException(CURRENTMODULE, "CustomsSubmitted",
                                        "No Name/Value Pair split from strParam: " + strParam);
                                    return;
                                }

                                if (strNameValuePair.Length != 2)
                                {
                                    Globalitems.HandleException(CURRENTMODULE, "CustomsSubmitted",
                                        "Name/Value Pair split <> 2 from strParam: " + strParam);
                                    return;
                                }

                                //Process depending on Name in strNameValuePair[0]
                                switch (strNameValuePair[0])
                                {
                                    case "DateFrom":
                                        strFromDate = strNameValuePair[1];
                                        break;
                                    case "DateTo":
                                        strToDate = strNameValuePair[1];
                                        break;
                                }
                            }
                            
                        }   // foreach strParam in strParams



                        //Add Date info depending on From/To Dates
                       
                            //4 possibilities:
                            //Only From Date - 'On Or After [fromdate]'
                            //From Date = To Date - 'On [fromdate]'
                            //From Date < To Date - 'Between [fromdate] And [todate]'
                            //Only To Date - 'On Or Before [todate]'
                            if (strFromDate.Length > 0)
                            {
                                //Only From Date
                                if (strToDate.Length == 0)
                                    strReportTitle += "On Or After " +
                                        Convert.ToDateTime(strFromDate).ToString("MM/dd/yyyy");

                                //From Date = To Date
                                else if (strFromDate == strToDate)
                                    strReportTitle += "On " +
                                        Convert.ToDateTime(strFromDate).ToString("MM/dd/yyyy");

                                //From Date < To Date
                                else
                                    strReportTitle += "Between " +
                                        Convert.ToDateTime(strFromDate).ToString("MM/dd/yyyy") +
                                        " AND " + Convert.ToDateTime(strToDate).ToString("MM/dd/yyyy");
                            }   // if startdate.length > 0
                            else
                                //Only To Date
                                strReportTitle += "On Or Before " +
                                    Convert.ToDateTime(strToDate).ToString("MM/dd/yyyy");
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

                    //Get report data
                    strSQL = @"SELECT 
                    CASE 
	                    WHEN DATALENGTH(ff.FreightForwarderShortName) > 0 THEN ff.FreightForwarderShortName 
	                    ELSE ff.FreightForwarderName 
                    END AS forwarder,
                    veh.DestinationName, 
                    COUNT(veh.AutoportExportVehiclesID) AS units,
                    CASE 
	                    WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
	                    ELSE cus.CustomerName 
                    END customer
                    FROM AutoportExportVehicles veh
                    LEFT JOIN Customer cus ON veh.CustomerID = cus.CustomerID
                    LEFT JOIN AEFreightForwarder ff ON veh.FreightForwarderID = ff.AEFreightForwarderID
                    WHERE veh.DateSubmittedCustoms IS NOT NULL ";

                    //Add add'l WHERE clauses based on frmPrintSelection
                    if (Globalitems.strSelection == "unprinted")
                        strSQL += "AND veh.CustomsCoverSheetPrintedInd = 0 ";
                    else
                    {
                        if (strFromDate.Length > 0)
                            strSQL += "AND veh.DateSubmittedCustoms >= '" +
                                strFromDate + "' ";

                        if (strToDate.Length > 0)
                        {
                            strSQL += "AND veh.DateSubmittedCustoms < DATEADD(day,1,'" + strToDate + "') ";
                        }
                    }

                    strSQL += @"GROUP BY 
                    CASE 
	                    WHEN DATALENGTH(ff.FreightForwarderShortName) > 0 THEN ff.FreightForwarderShortName 
	                    ELSE ff.FreightForwarderName 
                    END, 
                    veh.DestinationName,
                    CASE 
                    WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
                    ELSE cus.CustomerName 
                    END
                    ORDER BY customer, Forwarder, veh.DestinationName	";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "CustomsSubmitted",
                            "No data table returned from data query");
                        return;
                    }


                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There is no data for a Customs Submitted report",
                            "NO DATA FOR REPORT", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    //Return table in ds as a ReportDataSource named 'dsCustomsApprovedSheets'
                    rptSource = new ReportDataSource("dsCustomsSubmitted",
                        ds.Tables[0]);

                    //Fill lsParams for CustomsApprovedCoversheet report
                    lsParams = new List<ReportParameter>();
                    rptParam = new ReportParameter("DAIAddress", strDAIAddress);
                    lsParams.Add(rptParam);

                    rptParam = new ReportParameter("title", strReportTitle);
                    lsParams.Add(rptParam);

                    OpenReportDisplayForm(rptSource, strReport, "Customs Submitted", lsParams);

                }   // if dlResult = OK
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CustomsSubmitted", ex.Message);
            }
        }

        private void GroundedLaneSummaryExcel()
        {
            try
            {
                DataSet ds;
                DataTable dt;
                List<ControlInfo> lsCSVcols;
                string strFilename;
                string strSQL;

                strSQL = @"SELECT 
                CASE 
                    WHEN CHARINDEX(' ',veh.BayLocation) > 0 THEN 
                        SUBSTRING(veh.BayLocation,1,CHARINDEX(' ',veh.BayLocation)-1) 
                    ELSE veh.BayLocation 
                END AS lane,
                CASE 
                    WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
                    ELSE cus.CustomerName 
                END AS customer, 
                veh.DestinationName,
                COUNT(*) AS units
                FROM AutoportExportVehicles veh
                LEFT OUTER JOIN Customer cus ON veh.CustomerID = cus.CustomerID
                WHERE veh.DateShipped IS NULL
                AND veh.DateReceived IS NOT NULL
                GROUP BY 
                    CASE WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
                    ELSE cus.CustomerName 
                END, 
                veh.DestinationName,
                CASE 
                    WHEN CHARINDEX(' ',veh.BayLocation) > 0 THEN 
                        SUBSTRING(veh.BayLocation,1,CHARINDEX(' ',veh.BayLocation)-1) 
                    ELSE veh.BayLocation 
                END
                ORDER BY 
                CASE 
                    WHEN CHARINDEX(' ',veh.BayLocation) > 0 THEN 
                        SUBSTRING(veh.BayLocation,1,CHARINDEX(' ',veh.BayLocation)-1) 
                    ELSE veh.BayLocation 
                END,
                CASE 
                    WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
                    ELSE cus.CustomerName 
                END, 
                veh.DestinationName";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GroundedLaneSummaryExcel",
                        "No data table returned from data query");
                    return;
                }

                dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("There are no Lane Summary records.", "NO LANE RECORDS IN DB",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Get the file Directory & Filename from the SettingTable
                strSQL = "SELECT ValueKey,ValueDescription FROM SettingTable " +
                    "WHERE ValueKey IN ('ExportDirectory','GroundedLaneSummaryFileName') " +
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

                //Create a list, lsCSVcols with ControlInfo objects in the order to appear in the csv file
                // objects needed for Inv Report
                lsCSVcols = new List<ControlInfo>()
                {
                    new ControlInfo { RecordFieldName = "lane", HeaderText = "Lane"},
                    new ControlInfo { RecordFieldName = "customer", HeaderText = "Customer" },
                    new ControlInfo { RecordFieldName = "DestinationName",
                        HeaderText = "Destination" },
                    new ControlInfo { RecordFieldName = "units",
                        HeaderText = "Units"}
                };

                //Invoke CreateSCVFile to create, save, &open the csv file
                Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GroundedLaneSummaryExcel",
                    ex.Message);
            }
        }

        private void GroundedLaneSummaryReport()
        {
            try
            {
                ComboBox cbo = new ComboBox();
                ComboboxItem cboItem;
                DialogResult dlResult;
                DataSet ds;
                DataRow dtRow;
                frmSetSelection frm;
                List<ReportParameter> lsParams;
                ReportParameter rptParam;
                ReportDataSource rptSource;
                string strDAIAddress;
                string strReport = "";
                string strSQL;

                strReport = Globalitems.SetReportPath(GROUNDEDLANESUMMARYREPORT);

                //Get sort order w/frmSetSelection: By Lane or By Customer
                cboItem = new ComboboxItem();
                cboItem.cboText = "<select>";
                cboItem.cboValue = "select";
                cbo.Items.Add(cboItem);

                cboItem = new ComboboxItem();
                cboItem.cboText = "By Lane";
                cboItem.cboValue = "lane";
                cbo.Items.Add(cboItem);

                cboItem = new ComboboxItem();
                cboItem.cboText = "By Customer";
                cboItem.cboValue = "customer";
                cbo.Items.Add(cboItem);

                cbo.SelectedIndex = 0;

                frm = new frmSetSelection("Sort Order", cbo, "Please select the sort order", true);
                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.OK)
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

                    //Get data for report
                    strSQL = @"SELECT 
                    CASE 
	                    WHEN CHARINDEX(' ',veh.BayLocation) > 0 THEN 
                            SUBSTRING(veh.BayLocation,1,CHARINDEX(' ',veh.BayLocation)-1) 
	                    ELSE veh.BayLocation 
                    END AS Lane,
                    CASE 
	                    WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
	                    ELSE cus.CustomerName 
                    END AS customer, 
                    veh.DestinationName,
                    COUNT(*) AS UnitCount, 
                    SUM(
	                    CASE WHEN veh.SizeClass = 'A' THEN 1 
	                    ELSE 0 
	                    END) 
                    AS ACount, 
                    SUM(
	                    CASE WHEN veh.SizeClass = 'B' THEN 1 
	                    ELSE 0 
	                    END) 
                    AS BCount, 
                    SUM(
	                    CASE WHEN veh.SizeClass = 'C' THEN 1 
	                    ELSE 0 
	                    END)
                    AS CCount, 
                    SUM(
	                    CASE WHEN veh.SizeClass = 'E' THEN 1 
	                    ELSE 0 
	                    END)
                    AS ECount, 
                    SUM(
	                    CASE WHEN veh.SizeClass = 'Z' THEN 1 
	                    ELSE 0 
	                    END)
                    AS ZCount
                    FROM AutoportExportVehicles veh
                    LEFT JOIN Customer cus ON veh.CustomerID = cus.CustomerID
                    WHERE veh.DateShipped IS NULL
                    AND veh.DateReceived IS NOT NULL
                    GROUP BY 
	                    CASE WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
	                    ELSE cus.CustomerName 
                    END, 
                    veh.DestinationName,
                    CASE 
	                    WHEN CHARINDEX(' ',veh.BayLocation) > 0 THEN SUBSTRING(veh.BayLocation,1,CHARINDEX(' ',veh.BayLocation)-1) 
	                    ELSE veh.BayLocation 
                    END ";

                    //Set ORDER BY
                    if (Globalitems.strSelection == "lane")
                        strSQL += @"ORDER BY 
                        CASE 
                            WHEN CHARINDEX(' ',veh.BayLocation) > 0 THEN 
                                SUBSTRING(veh.BayLocation,1,CHARINDEX(' ',veh.BayLocation)-1) 
                            ELSE veh.BayLocation 
                        END,
                        CASE 
                            WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
                            ELSE cus.CustomerName 
                        END, 
                        veh.DestinationName";
                    else
                        strSQL += @"ORDER BY 
                        CASE 
                            WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
                            ELSE cus.CustomerName 
                        END, 
                        veh.DestinationName,
                        CASE 
                            WHEN CHARINDEX(' ',veh.BayLocation) > 0 THEN 
                                SUBSTRING(veh.BayLocation,1,CHARINDEX(' ',veh.BayLocation)-1) 
                            ELSE veh.BayLocation 
                        END";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "GroundedLaneSummaryReport",
                            "No datatable returned");
                        return;
                    }

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There is no data for a Grounded Lane Summary report",
                            "NO DATA FOR REPORT", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    //Return table in ds as a ReportDataSource named 'dsCustomsApprovedSheets'
                    rptSource = new ReportDataSource("dsGroundedLaneSummary",
                        ds.Tables[0]);

                    //Fill lsParams for CustomsApprovedCoversheet report
                    lsParams = new List<ReportParameter>();
                    rptParam = new ReportParameter("DAIAddress", strDAIAddress);
                    lsParams.Add(rptParam);

                    OpenReportDisplayForm(rptSource, strReport, "Grounded Lane Summary", lsParams);
                }
                
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GroundedSummaryLaneReport", ex.Message);
            }
        }

        private void GroundedSummaryReport()
        {
            try
            {
                DataSet ds;
                DataRow dtRow;
                List<ReportParameter> lsParams;
                ReportParameter rptParam;
                ReportDataSource rptSource;
                string strDAIAddress;
                string strReport = "";
                string strSQL;

                strReport = Globalitems.SetReportPath(GROUNDEDSUMMARYREPORT);

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

                //Get data for report
                strSQL = @"SELECT 
                CASE 
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
	                ELSE cus.CustomerName 
                END Customer,
                ISNULL(ves.VesselName,'') AS Vessel, 
                voy.VoyageDate, 
                COUNT(*) AS totGrounded,
                SUM(
	                CASE WHEN VehicleStatus = 'Received' THEN 1 
	                ELSE 0 
                END) AS totReceived,
                SUM(
	                CASE WHEN VehicleStatus = 'SubmittedCustoms' THEN 1 
	                ELSE 0 
                END) AS totSubmitted,
                SUM(CASE 
	                WHEN VehicleStatus = 'ClearedCustoms' THEN 1 
	                ELSE 0 
                END) AS totCleared,
                SUM(
	                CASE WHEN VehicleStatus = 'ReceivedException' THEN 1 
	                ELSE 0 
                END) AS totRcvException,
                SUM(CASE 
	                WHEN VehicleStatus = 'CustomsException' THEN 1 
	                ELSE 0 
                END) AS totCusException,
                SUM(ISNULL(veh.NoStartInd,0)) AS totNoStart,
                SUM(
	                CASE WHEN DATALENGTH(ISNULL(veh.VIVTagNumber,'')) > 0 THEN 1 
	                ELSE 0 
                END) AS totVIVTagNumber
                FROM AutoportExportVehicles veh
                LEFT JOIN Customer cus ON veh.CustomerID = cus.CustomerID
                LEFT JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID
                LEFT JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                WHERE veh.DateShipped IS NULL
                AND veh.DateReceived IS NOT NULL
                GROUP BY 
                CASE 
	                WHEN DATALENGTH(cus.ShortName) > 0 THEN cus.ShortName 
	                ELSE cus.CustomerName 
                END,
                ves.VesselName, 
                voy.VoyageDate
                ORDER BY voy.VoyageDate, Customer, ves.VesselName";

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
                rptSource = new ReportDataSource("dsGroundedSummary",
                    ds.Tables[0]);

                //Fill lsParams for CustomsApprovedCoversheet report
                lsParams = new List<ReportParameter>();
                rptParam = new ReportParameter("DAIAddress", strDAIAddress);
                lsParams.Add(rptParam);
                
                OpenReportDisplayForm(rptSource, strReport, "Grounded Summary", lsParams);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE,
                    "GroundedSummaryReport", ex.Message);
            }
           
        }

      
        private void OpenPrintCustomClearedSheetsForm()
        {

            try
            {
                frmCustomClearedSheets frm;
                PrintInfo objPrintInfo = new PrintInfo();

                objPrintInfo.Message = "";
                

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
                Globalitems.HandleException(CURRENTMODULE, "OpenPrintCustomClearedSheetsForm", 
                    ex.Message);
            }
        }

        private void OpenPrintLabelsForm()
        {
            try
            {
                frmLabels frm;
                PrintInfo objPrintInfo = new PrintInfo();

                objPrintInfo.Message = "";

                //Use Show method if not currently open
                if (Application.OpenForms.OfType<frmLabels>().Count() == 0)
                {
                    frm = new frmLabels(objPrintInfo);
                    Formops.SetFormBackground(frm);
                    frm.Show();
                }
                else //Already open, set as Active form
                {
                    frm = (frmLabels)Application.OpenForms["frmLabels"];
                    frm.objPrintInfo = objPrintInfo;
                    frm.Activate();
                }
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenPrintLabelsForm",
                    ex.Message);
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

        private void VehiclesOnHold(string rptType = "")
        {
            try
            {
                DataSet ds;
                string strSproc = "spVehiclesOnHold";

                //Open report at ReportServer if not csv file
                //Btn to go to SSRS server deleted. Consider implementing whenn 
                //SSRS server is no longer on DAITRACKER3
                if (rptType == "")
                {
                    ProcessStartInfo psInfo =
                    new ProcessStartInfo("http://daitracker3/Reports/Pages/Report.aspx?ItemPath=%2fDAIReports%2fAutoportExport%2fVehiclesOnHold");
                    Process.Start(psInfo);
                    return;
                }
                else
                {
                    //Get report data for csv file
                    ds = DataOps.GetDataset_with_SProc(strSproc);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "VehiclesOnHold",
                            "No datatable returned");
                        return;
                    }

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There is no data for the Vehicles On Hold report",
                            "NO DATA FOR REPORT", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    VehiclesOnHoldExcel(ds);
                }               
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "VehiclesOnHold", ex.Message);
            }
        }

        private void VehiclesOnHoldExcel(DataSet ds)
        {
            try
            {

                DataSet dsFilename;
                DataTable dt = ds.Tables[0];
                List<ControlInfo> lsCSVcols;
                string strFilename;
                string strSQL;

                //Get the file Directory & Filename from the SettingTable
                strSQL = "SELECT ValueKey,ValueDescription FROM SettingTable " +
                    "WHERE ValueKey IN ('ExportDirectory','VehiclesOnHoldFileName') " +
                    "AND RecordStatus='Active' ORDER BY ValueKey";
                dsFilename = DataOps.GetDataset_with_SQL(strSQL);

                // S/B just two active rows, row 1 ExportDirectory, row 2 VehicleExportFileName
                if (dsFilename.Tables.Count == 0 || dsFilename.Tables[0].Rows.Count != 2)
                {
                    Globalitems.HandleException(CURRENTMODULE, "VehiclesOnHoldExcel",
                        "No rows returned from SettingTable");
                    return;
                }
                // 1st Record s/b ExportDirectory, 2nd Record s/b VehicleExportFileName
                strFilename = dsFilename.Tables[0].Rows[0]["ValueDescription"].ToString();
                strFilename += @"\" + dsFilename.Tables[0].Rows[1]["ValueDescription"].ToString();

                //Create a list, lsCSVcols with ControlInfo objects in the order to appear in the csv file
                // objects needed for Inv Report
                lsCSVcols = new List<ControlInfo>()
                {
                    new ControlInfo { RecordFieldName = "rownum", HeaderText = "" },
                    new ControlInfo { RecordFieldName = "customer", HeaderText = "Customer" },
                    new ControlInfo { RecordFieldName = "VIN",HeaderText = "VIN" },
                    new ControlInfo { RecordFieldName = "Make",HeaderText = "Make"},
                    new ControlInfo { RecordFieldName = "Model",HeaderText = "Model"},
                    new ControlInfo { RecordFieldName = "BayLocation",HeaderText = "Bay Loc."},
                    new ControlInfo { RecordFieldName = "exception",HeaderText = "Exception"},
                    new ControlInfo { RecordFieldName = "exdate",HeaderText = "Updated Date"},
                    new ControlInfo { RecordFieldName = "DateReceived",HeaderText = "Rcvd Date"}
                };

                //Invoke CreateSCVFile to create, save, &open the csv file
                Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "VehiclesOnHoldExcel",
                    ex.Message);
            }
        }

       

    private void VoyagePushCarList()
        {
            try
            {
                ComboBox cbo = new ComboBox();
                ComboboxItem cboItem;
                DialogResult dlResult;
                DataSet ds;
                DataRow dtDataRow;
                DataTable dtDistinct;
                DataView dv;
                frmSetSelection frm;
                int intlinenumber;
                List<ReportParameter> lsParams;
                ReportParameter rptParam;
                ReportDataSource rptSource;
                string strDAIAddress;
                string strDestination;
                string strFilter;
                string strReport = "";
                string strSQL;

                strReport = Globalitems.SetReportPath(VOYAGEPUSHCARLISTREPORT);

                //Get VoyageID w/frmSetSelection. Build cbo to pass to frmSetSelection
                cboItem = new ComboboxItem();
                cboItem.cboText = "<select>";
                cboItem.cboValue = "select";
                cbo.Items.Add(cboItem);

                strSQL = @"SELECT 
                CONVERT(varchar(10),voy.VoyageDate,101) +' - ' + ves.VesselName AS voyinfo, 
                voy.AEVoyageID
                FROM AEVoyage voy
                LEFT JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                WHERE voy.VoyageDate < '1/1/3000'
                ORDER BY voy.VoyageDate DESC";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "VoyagePushCarList",
                        "No voyage data returned for cbo");
                    return;
                }

                foreach (DataRow dtrow in ds.Tables[0].Rows)
                {
                    cboItem = new ComboboxItem();
                    cboItem.cboText = dtrow["voyinfo"].ToString();
                    cboItem.cboValue = dtrow["AEVoyageID"].ToString();
                    cbo.Items.Add(cboItem);
                }

                cbo.SelectedIndex = 0;

                frm = new frmSetSelection("Voyage Date", cbo, "Please select the Voyage", true);
                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.OK)
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

                    dtDataRow = ds.Tables[0].Rows[0];
                    strDAIAddress = dtDataRow["CompanyName"] + "\n" +
                        dtDataRow["AddressLine1"] + "\n";

                    if (dtDataRow["AddressLine2"].ToString().Trim().Length > 0)
                        strDAIAddress += dtDataRow["AddressLine1"] + "\n";

                    strDAIAddress += dtDataRow["CitySTZip"] + "\n" +
                        dtDataRow["tel"];

                    //Get data for report
                    strSQL = @"SELECT 
                    0 AS linenumber,
                    veh.BayLocation,
                    veh.VIN, 
                    veh.VehicleYear + ' ' + veh.Make + ' ' + veh.Model AS yearmakemodel,
                    veh.SizeClass, 
                    veh.DestinationName
                    FROM AutoportExportVehicles veh
                    LEFT OUTER JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID
                    LEFT OUTER JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                    WHERE veh.VoyageID = " + Globalitems.strSelection +
                    @"AND veh.NoStartInd = 1
                    AND veh.DateShipped IS NULL
                    AND veh.CustomsApprovedDate IS NOT NULL
                    ORDER BY DestinationName,SizeClass,BayLocation";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "VoyagePushCarList",
                            "No datatable returned");
                        return;
                    }

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There is no data for a Voygate Push Car List",
                            "NO DATA FOR REPORT", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    //Put the DISTINCT DestinationNames into dtDistinct
                    dv = new DataView(ds.Tables[0], "VIN IS NOT NULL", "DestinationName", 
                        DataViewRowState.CurrentRows);
                    dtDistinct = dv.ToTable(true, "DestinationName");

                    //Assign linenumbers for each veh in each destination
                    foreach (DataRow dtRow in dtDistinct.Rows)
                    {
                        strDestination = dtRow["DestinationName"].ToString();
                        intlinenumber = 1;

                        //Get all the rows for the current destination
                        strFilter = "DestinationName = '" + strDestination + "'";
                        dv = new DataView(ds.Tables[0], strFilter, "SizeClass,BayLocation",
                            DataViewRowState.CurrentRows);

                        //Update linenumber for each row in dv
                        foreach (DataRowView dvRow in dv)
                        {
                            dvRow["linenumber"] = intlinenumber;
                            intlinenumber += 1;
                        }
                    }

                    //Return table in ds as a ReportDataSource named 'dsCustomsApprovedSheets'
                    rptSource = new ReportDataSource("dsVoyagePushCarList",
                        ds.Tables[0]);

                    //Fill lsParams for CustomsApprovedCoversheet report
                    lsParams = new List<ReportParameter>();
                    rptParam = new ReportParameter("DAIAddress", strDAIAddress);
                    lsParams.Add(rptParam);

                    OpenReportDisplayForm(rptSource, strReport, "Voyage Push Car List", lsParams);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "VoyagePushCarList", ex.Message);
            }
        }

        private void btnCustomsCleared_Click(object sender, EventArgs e)
        { OpenPrintCustomClearedSheetsForm(); }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }
        

        private void btnCusClearedCoversheet_Click(object sender, EventArgs e)
        {CustomsClearedCoversheet();}

        private void btnLabels_Click(object sender, EventArgs e)
        {OpenPrintLabelsForm();}

        private void btnGroundedSummary_Click(object sender, EventArgs e)
        {GroundedSummaryReport();}

        private void btnGroundedLane_Click(object sender, EventArgs e)
        {GroundedLaneSummaryReport();}

        private void btnGroundedLaneExcel_Click(object sender, EventArgs e)
        {GroundedLaneSummaryExcel();}

        private void btnPrintCustomsSub_Click(object sender, EventArgs e)
        {CustomsSubmitted();}

        private void btnVoyPushCarList_Click(object sender, EventArgs e)
        {VoyagePushCarList();}

        private void btnVehsOnHoldExcel_Click(object sender, EventArgs e)
        {VehiclesOnHold("EX");}

        private void btnNoLabels_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To print labels, a label printer " +
               "\nmust be set as the default printer" +
               "\n (E.g. Wasp or Zebra printer)", "LABEL PRINTER NOT THE DEFAULT",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pnlHandheldFile_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}

        private void frmReports_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
