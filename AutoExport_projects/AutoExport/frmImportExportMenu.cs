using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class frmImportExportMenu : Form
    {
        private const string CURRENTMODULE = "frmImportExportMenu";

        public frmImportExportMenu()
        {
            InitializeComponent();
        }

        private void BookingRecordsReport()
        {
            try
            {
                DialogResult dlResult;
                DataSet ds;
                frmInvRptParams frm;
                string strCustomerID = "";
                string[] strParams;
                string[] strNameValuePair;
                string strShipStatus = "";
                string strVoyageID = "";
                frm = new frmInvRptParams();
                frm.strInvtype = "BOOK";
                string strSQL;

                dlResult = frm.ShowDialog();
                if (dlResult == DialogResult.OK)
                {
                    //Decode returned string into strParams
                    strParams = Globalitems.strSelection.Split(Globalitems.chrRecordSeparator);

                    if (strParams.Length == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "BookingRecordsReport",
                            "No elements split into strParams after frmInvRptParams.\n" +
                            "strSelection: " + Globalitems.strSelection);
                        return;
                    }

                    foreach (string strParam in strParams)
                    {
                        strNameValuePair = strParam.Split(Globalitems.chrNameValueSeparator);
                        if (strNameValuePair.Length == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "BookingRecordsReport",
                                "No Name/Value Pair split from strParam: " + strParam);
                            return;
                        }

                        if (strNameValuePair.Length != 2)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "BookingRecordsReport",
                                "Name/Value Pair split <> 2 from strParam: " + strParam);
                            return;
                        }

                        //Process depending on Name in strNameValuePair[0]
                        switch (strNameValuePair[0])
                        {
                            case "CustomerID":
                                strCustomerID = strNameValuePair[1];
                                break;
                            case "VoyageID":
                                strVoyageID = strNameValuePair[1];
                                break;
                            case "ShipStatus":
                                strShipStatus = strNameValuePair[1];
                                break;
                        }
                    }   // foreach strParam in strParams

                    strSQL = @"SELECT 
                    CASE 
	                    WHEN DATALENGTH(ShortName) > 0 THEN ShortName 
	                    ELSE CustomerName 
                    END AS Customer,
                    veh.BookingNumber, 
                    ves.VesselName, 
                    voy.VoyageNumber, 
                    veh.DestinationName, 
                    ff.FreightForwarderName,
                    loc.AddressLine1, 
                    loc.City, 
                    loc.State, 
                    loc.Zip, 
                    veh.VIN, 
                    veh.VehicleYear, 
                    veh.Make, 
                    veh.Model, 
                    veh.SizeClass, 
                    veh.VehicleLength,
                    veh.VehicleWidth,
                    veh.VehicleHeight,
                    veh.VehicleCubicFeet,
                    veh.VehicleWeight, 
                    veh.DateReceived, 
                    veh.ReceivedExceptionDate, 
                    veh.VoyageChangeHoldDate, veh.DateSubmittedCustoms, 
                    veh.CustomsApprovedDate, 
                    veh.CustomsExceptionDate,
                    veh.DateShipped, 
                    veh.BayLocation, 
                    veh.VIVTagNumber, 
                    Code.CodeDescription,
                    veh.VehicleStatus
                    FROM AutoportExportVehicles veh
                    LEFT JOIN Customer cus ON veh.CustomerID = cus.CustomerID
                    LEFT JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID
                    LEFT JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                    LEFT JOIN AEFreightForwarder ff ON veh.FreightForwarderID = ff.AEFreightForwarderID
                    LEFT JOIN Location loc ON ff.FreightForwarderAddressID = loc.LocationID
                    LEFT JOIN Code ON veh.VehicleStatus = Code.Code
                    AND Code.CodeType = 'AutoportExportVehicleStatus' 
                    WHERE veh.VIN IS NOT NULL ";

                    // Add params to WHERE clause
                    //Shipped status
                    if (strShipStatus.Length > 0)
                    {
                        if (strShipStatus == "NotShipped")
                            strSQL += "AND veh.DateShipped IS NULL ";
                        else
                            strSQL += "AND veh.DateShipped IS NOT NULL ";
                    }

                    //VoyageID
                    if (strVoyageID.Length > 0) strSQL += "AND veh.VoyageID = " +
                            strVoyageID + " ";

                    //CustomerID
                    if (strCustomerID.Length > 0) strSQL += "AND veh.CustomerID = " +
                            strCustomerID + " ";

                    strSQL += @"AND veh.DateReceived IS NOT NULL 
                    ORDER BY Customer,ff.FreightForwarderName, veh.DateReceived, veh.VIN";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There are no Booking Records to export.",
                            "NO BOOKING RECORDS", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    OpenBookingRecordsRpt(ds.Tables[0]);

                }   //iF dlResult = OK
              }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "BookingRecordsReport",
                    ex.Message);
            }
        }

        private void BookingSummary()
        {
            try
            {
                DialogResult dlResult;
                DataSet ds;
                DataTable dtSummary_1;
                DataTable dtSummary_2;
                frmInvRptParams frm;
                string strCustomerID = "";
                string[] strParams;
                string[] strNameValuePair;
                string strVoyageID = "";

                frm = new frmInvRptParams();
                frm.strInvtype = "BOOKSUM";
                string strSQL;

                dlResult = frm.ShowDialog();
                if (dlResult == DialogResult.OK)
                {
                    //Decode returned string into strParams
                    strParams = Globalitems.strSelection.Split(Globalitems.chrRecordSeparator);

                    if (strParams.Length == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "BookingRecordsReport",
                            "No elements split into strParams after frmInvRptParams.\n" +
                            "strSelection: " + Globalitems.strSelection);
                        return;
                    }

                    foreach (string strParam in strParams)
                    {
                        strNameValuePair = strParam.Split(Globalitems.chrNameValueSeparator);
                        if (strNameValuePair.Length == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "BookingRecordsReport",
                                "No Name/Value Pair split from strParam: " + strParam);
                            return;
                        }

                        if (strNameValuePair.Length != 2)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "BookingRecordsReport",
                                "Name/Value Pair split <> 2 from strParam: " + strParam);
                            return;
                        }

                        //Process depending on Name in strNameValuePair[0]
                        switch (strNameValuePair[0])
                        {
                            case "CustomerID":
                                strCustomerID = strNameValuePair[1];
                                break;
                            case "VoyageID":
                                strVoyageID = strNameValuePair[1];
                                break;
                        }
                    }   // foreach strParam in strParams


                    //Get first summary data
                    strSQL = @"SELECT 
                    veh.DestinationName,
                    ves.VesselName, 
                    voy.VoyageDate,
                    COUNT(*) OnTerminalTot,
                    SUM(CASE 
                        WHEN ISNULL(veh.SIZECLASS, 'A') = 'A' THEN 1 
                        ELSE 0 
                    END) AS OnTerminalA,
                    SUM(CASE 
                        WHEN ISNULL(veh.SIZECLASS, 'A') = 'B' THEN 1 
                        ELSE 0 
                    END) AS OnTerminalB,
                    SUM(CASE 
                        WHEN ISNULL(veh.SIZECLASS, 'A') = 'C' THEN 1 
                        ELSE 0 
                    END) AS OnTerminalC,
                    SUM(CASE 
                        WHEN ISNULL(veh.SIZECLASS, 'A') = 'E' THEN 1 
                        ELSE 0 
                    END) AS OnTerminalE,
                    SUM(CASE 
                        WHEN ISNULL(veh.SIZECLASS, 'A') = 'Z' THEN 1 
                        ELSE 0 
                    END) as OnTerminalZ,
                    SUM(CASE 
                        WHEN VEH.CustomsApprovedDate IS NULL THEN 1 
                        ELSE 0 
                    END) AS NotCleared,
                    SUM(CASE 
                        WHEN VEH.CustomsApprovedDate IS NOT NULL THEN 1 
                        ELSE 0 
                    END) AS ClearedTot,
                    SUM(CASE 
                        WHEN veh.CustomsApprovedDate IS NOT NULL AND 
                            ISNULL(veh.SIZECLASS, 'A') = 'A' THEN 1 
                        ELSE 0 
                    END) AS ClearedA,
                    SUM(CASE
                        WHEN veh.CustomsApprovedDate IS NOT NULL AND 
                            ISNULL(veh.SIZECLASS, 'A') = 'B' THEN 1 
                        ELSE 0 
                    END) AS ClearedB,
                    SUM(CASE 
                        WHEN veh.CustomsApprovedDate IS NOT NULL AND 
                            ISNULL(veh.SIZECLASS, 'A') = 'C' THEN 1 
                        ELSE 0 
                    END) AS ClearedC,
                    SUM(CASE 
                        WHEN veh.CustomsApprovedDate IS NOT NULL AND 
                            ISNULL(veh.SIZECLASS, 'A') = 'E' THEN 1 
                        ELSE 0 
                    END) AS ClearedE,
                    SUM(CASE
                        WHEN veh.CustomsApprovedDate IS NOT NULL AND 
                            ISNULL(veh.SIZECLASS, 'A') = 'Z' THEN 1 
                        ELSE 0 
                    END) AS ClearedZ,
                    SUM(CASE
                        WHEN veh.DateShipped IS NOT NULL THEN 1 
                        ELSE 0 
                    END) AS Shipped
                    FROM AutoportExportVehicles veh
                    LEFT JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID
                    LEFT JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                    WHERE voy.AEVoyageID = " + strVoyageID +
                  @"AND veh.DateReceived IS NOT NULL
                    AND veh.DestinationName IN 
                    (SELECT voydest.DestinationName 
                        FROM AEVoyageDestination voydest 
                        WHERE voydest.AEVoyageID = voy.AEVoyageID) ";

                    //CustomerID
                    if (strCustomerID.Length > 0) strSQL += "AND veh.CustomerID = " +
                            strCustomerID + " ";

                    strSQL += @"GROUP BY veh.DestinationName, 
                        ves.VesselName, 
                        voy.VoyageDate
                        ORDER BY veh.DestinationName";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There are no Booking Summary records to export.",
                            "NO BOOKING SUMMARY RECORDS", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    dtSummary_1 = ds.Tables[0];

                    //Get 2nd Summary data
                    strSQL = @"SELECT 
                    COUNT(veh.AutoportExportVehiclesID) AS OnTerminal,
                    ISNULL(
                        SUM(CASE 
                            WHEN veh.CustomsApprovedDate IS NULL THEN 1 
                            ELSE 0 
                        END),0) 
                    AS NotCleared,
                    ISNULL(
                        SUM(CASE 
                            WHEN veh.CustomsApprovedDate IS NOT NULL THEN 1 
                            ELSE 0 END),0) 
                    AS Cleared
                    FROM AutoportExportVehicles veh
                    WHERE (veh.VoyageID <> " + strVoyageID +
                    @"OR veh.VoyageID IS NULL)
                    AND veh.DateReceived IS NOT NULL
                    AND veh.DestinationName IN 
                        (SELECT DestinationName 
                        FROM AEVoyageDestination 
                        WHERE AEVoyageID = " + strVoyageID + @")
                    AND veh.DateShipped IS NULL ";

                    if (strCustomerID.Length > 0)
                        strSQL += "AND veh.CustomerID = " + strCustomerID;

                    ds = DataOps.GetDataset_with_SQL(strSQL);

                    dtSummary_2 = ds.Tables[0];

                    if (strCustomerID.Length == 0)
                        OpenBookingSummaryRpt("All Customers",strVoyageID, dtSummary_1, dtSummary_2);
                    else
                        OpenBookingSummaryRpt(Globalitems.strTextSelected, strVoyageID,
                            dtSummary_1, dtSummary_2);

                }   //iF dlResult = OK
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "BookingRecordsSummary", ex.Message);
            }
        }

        private void ExportCustomsApproved()
            //3/26/18 D.Maibor: replace <all> with <select> in voyage cbo;
            //place data in tables, instead of txt file
        {
            try
            {
                //For old txtfile method
                DataTable dtVoyageResult;
                string strFileName = "";
                string strFirstLine;
                StreamWriter strWriter;

                ComboboxItem cboItem;
                ComboBox cboVoyage;
                DataSet ds;
                DataTable dtVoyageSelection;
                DialogResult dlResult;
                DataView dv;
                frmSetSelection frm;
                string strFilter;
                string strSQL;
                string strVoyageID = "";

                //Create cbo for frmSetSelect
                cboVoyage = new ComboBox();

                //Set <select> as 1st item
                cboItem = new ComboboxItem();
                cboItem.cboText = "<select>";
                cboItem.cboValue = "select";
                cboVoyage.Items.Add(cboItem);

                //Get Voyage info for cbo & Line 1 of table
                strSQL = @"SELECT 
                CONVERT(varchar(10),voy.VoyageDate,101)+' '+ ves.VesselName AS voyinfo, 
                voy.AEVoyageID,
                Cast(voy.VoyageDate AS Date) AS VoyageDate,
                ves.VesselName
                FROM AEVoyage voy
                LEFT JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                WHERE voy.VoyageDate < '3000'
                ORDER BY voy.VoyageDate DESC";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ExportCustoms",
                        "No voyage info returned", true);
                    return;
                }
                    
                dtVoyageSelection = ds.Tables[0];

                //Add items to cbo
                foreach (DataRow dtrow in dtVoyageSelection.Rows)
                {
                    cboItem = new ComboboxItem();
                    cboItem.cboText = dtrow["voyinfo"].ToString();
                    cboItem.cboValue = dtrow["AEVoyageID"].ToString();
                    cboVoyage.Items.Add(cboItem);
                }

                cboVoyage.SelectedIndex = 0;

                //Open frmSetSelection in modal form, to get new Destination
                frm = new frmSetSelection("Voyage", cboVoyage,"Please select the Voyage for\n" +
                    "the Customs Approved Export",true);
                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.Cancel) return;

                strVoyageID = Globalitems.strSelection;

                //OLD APPROACH - CREATE txt file on shared drive
                //Get Directory & Filename from SettingTable
                //strSQL = @"select ValueKey,ValueDescription 
                //FROM SettingTable 
                //WHERE ValueKey LIKE 'AECustomsApprovedExport%'
                //ORDER BY ValueKey";
                //ds = DataOps.GetDataset_with_SQL(strSQL);
                //if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                //{
                //    Globalitems.HandleException(CURRENTMODULE, "ExportCustomsApproved",
                //        "No data returned from query to SettingTable table");
                //    return;
                //}

                ////Rows should appear in the order: 
                ////AECustomsApprovedExportDirectory
                ////AECustomsApprovedExportFileName
                //foreach (DataRow dtrow in ds.Tables[0].Rows)
                //{
                //    if (dtrow["ValueKey"].ToString().Contains("Dir"))
                //        strFileName = dtrow["ValueDescription"].ToString();
                //    else
                //        strFileName += dtrow["ValueDescription"].ToString();
                //}

                //// \\192.168.1.186\c$\ESD\ProdDAI\Store\LookUpFiles\CustomsApprovedForLoading.txt

                ////Ck that file exists
                //if (!File.Exists(strFileName))
                //{
                //    MessageBox.Show("The Customs Approved file does not exist!",
                //        "CUSTOMS APPROVED FILE MISSING", MessageBoxButtons.OK,
                //        MessageBoxIcon.Error);
                //    return;
                //}

                ////Retrieve the data from the veh table
                //strSQL = @"SELECT
                //VIN, DestinationName
                //FROM AutoportExportVehicles
                //WHERE CustomsApprovedDate IS NOT NULL
                //AND DateShipped IS NULL ";

                ////Add VoyageID if not all
                //if (strVoyageID.Length > 0) strSQL += "AND VoyageID = " + strVoyageID;

                //strSQL += " ORDER BY VIN";
                //ds = DataOps.GetDataset_with_SQL(strSQL);
                //if (ds.Tables.Count == 0)
                //{
                //    Globalitems.HandleException(CURRENTMODULE, "ExportCustomsApproved",
                //        "No table returned from query to vehicle table");
                //    return;
                //}

                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    MessageBox.Show("There are no Customs Approved records to export.",
                //        "NO CUSTOMS APPROVED RECORDS", MessageBoxButtons.OK,
                //        MessageBoxIcon.Error);
                //    return;
                //}

                //dtVoyageResult = ds.Tables[0];

                //strWriter = new StreamWriter(strFileName);

                ////Write 1st line for text file
                //if (strVoyageID.Length == 0)
                //    strFirstLine = "All voyages " + DateTime.Now.ToString("HH:mm");
                //else
                //{
                //    //Get voy info from dtVoyage
                //    strFilter = "AEVoyageID = " + strVoyageID;
                //    dv = new DataView(dtVoyageSelection, strFilter,
                //        "AEVoyageID", DataViewRowState.CurrentRows);
                //    strFirstLine = strVoyageID + "," + dv[0]["voyinfo"].ToString() +
                //       " " + DateTime.Now.ToString("HH:mm");
                //}
                //strWriter.WriteLine(strFirstLine);

                ////Write each line in dtVoyageResult to file
                //foreach (DataRow dtrow in dtVoyageResult.Rows)
                //    strWriter.WriteLine(dtrow["VIN"].ToString() + "," +
                //        dtrow["DestinationName"].ToString());

                //strWriter.Close();

                //NEW APPROACH - PLACE DATA IN 2 TABLES
                //Activate when James Wilson makes change for handheld

                //Truncate data in CustomsApproved tables
                strSQL = "TRUNCATE TABLE CustomsApprovedLine1";
                DataOps.PerformDBOperation(strSQL);

                strSQL = "TRUNCATE TABLE CustomsApprovedVINs";
                DataOps.PerformDBOperation(strSQL);

                //Ck that there are VINs for voyage w/status 'ClearedCustoms'
                strSQL = @"SELECT COUNT(AutoportExportVehiclesID) AS totrecs
                FROM AutoportExportVehicles WHERE 
                VoyageID = " + strVoyageID + " AND VehicleStatus = 'ClearedCustoms'";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ExportCustomsApproved",
                        "No tables returned from Select Count");
                }

                if (Convert.ToInt16(ds.Tables[0].Rows[0]["totrecs"]) == 0)
                {
                    MessageBox.Show("There are no Customs Approved records to export.",
                    "NO CUSTOMS APPROVED RECORDS", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }

                //Load CustomsApprovedLine1 from dtVoyageSelection
                strFilter = "AEVoyageID = " + strVoyageID;
                dv = new DataView(dtVoyageSelection, strFilter, "AEVoyageID",
                    DataViewRowState.CurrentRows);
                strSQL = @"INSERT INTO CustomsApprovedLine1 
                (VoyageID,VoyageDate,VesselName,CreationTime) 
                VALUES (" + strVoyageID + ",";
                strSQL += "'" + dv[0]["VoyageDate"].ToString() + "',";
                strSQL += "'" + dv[0]["VesselName"] + "',";
                strSQL += "CURRENT_TIMESTAMP)";
                DataOps.PerformDBOperation(strSQL);

                //Load the data from the veh table
                strSQL = @"INSERT INTO CustomsApprovedVINs (VIN,DestinationName)
                SELECT
                VIN, DestinationName
                FROM AutoportExportVehicles
                WHERE CustomsApprovedDate IS NOT NULL
                AND VehicleStatus = 'ClearedCustoms'
                AND DateShipped IS NULL AND VoyageID = " + strVoyageID;
                strSQL += " ORDER BY VIN";
                DataOps.PerformDBOperation(strSQL);

                MessageBox.Show("The Customs Approved records were successfully exported",
                "SUCCESSFUL EXPORT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ExportCustomsApproved",
                    ex.Message);
            }
        }

        private void OpenImportVehYMSForm(string strImportType)
        {
            try
            {
                frmImportVehYMS frm;              

                //If frmImportVehYMS is already open, Activate it
                if (Application.OpenForms.OfType<frmImportVehYMS>().Count() == 1)
                {
                    frm = (frmImportVehYMS)Application.OpenForms["frmImportVehYMS"];
                    frm.strImportType = strImportType;
                    frm.Focus();
                }
                else
                {
                    frm = new frmImportVehYMS();
                    frm.strImportType = strImportType;
                    Formops.OpenNewForm(frm);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenImportVehYMSForm", 
                    ex.Message);
            }
        }

        private void PreShipInvReport()
        {
            try
            {
                DataSet ds;
                DialogResult dlResult;
                frmInvRptParams frm;

                string[] strParams;
                string[] strNameValuePair;
                string strCustomerID = "";
                string strBatchIDs = "";
                string strFromDate = "";
                string strToDate = "";
                string strVoyageID = "";
                string strSQL = "";
                string strval = "";

                frm = new frmInvRptParams();
                frm.strInvtype = "PRE";
                dlResult = frm.ShowDialog();
                if (dlResult == DialogResult.OK)
                {
                    //Decode returned string into strParams
                    strParams = Globalitems.strSelection.Split(Globalitems.chrRecordSeparator);

                    if (strParams.Length == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "InvReport",
                            "No elements split into strParams after frmInvRptParams.\n" +
                            "strSelection: " + Globalitems.strSelection);
                        return;
                    }

                    foreach (string strParam in strParams)
                    {
                        strNameValuePair = strParam.Split(Globalitems.chrNameValueSeparator);
                        if (strNameValuePair.Length == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "InvReport",
                                "No Name/Value Pair split from strParam: " + strParam);
                            return;
                        }

                        if (strNameValuePair.Length != 2)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "InvReport",
                                "Name/Value Pair split <> 2 from strParam: " + strParam);
                            return;
                        }

                        //Process depending on Name in strNameValuePair[0]
                        switch (strNameValuePair[0])
                        {
                            case "CustomerID":
                                strCustomerID = strNameValuePair[1];
                                break;
                            case "BatchID":
                                strBatchIDs = strNameValuePair[1];
                                break;
                            case "DateFrom":
                                strFromDate = strNameValuePair[1];
                                break;
                            case "DateTo":
                                strToDate = strNameValuePair[1];
                                break;
                            case "VoyageID":
                                strVoyageID = strNameValuePair[1];
                                break;
                        }
                    }   // foreach strParam in strParams

                    //Create SQL for report, using params in WHERE clauses (same as DATS)
                    //1st SELECT for 'Scanned - Not On Voyage' exception
                    //
                    strSQL = @"SELECT 
                    CASE 
	                    WHEN DATALENGTH(ShortName) > 0 THEN ShortName 
	                    ELSE CustomerName 
                    END AS Customer,
                    veh.VIN, 
                    veh.Model, 
                    ves.VesselName, 
                    veh.VehicleStatus, 
                    veh.BayLocation,
                    CONVERT(varchar(10),veh.CustomsApprovedDate,101) AS CustomsApprovedDate, 
                    'Scanned - Not On Voyage' AS Exception
                    FROM AutoportExportVehicles veh
                    LEFT OUTER JOIN Customer cus ON veh.CustomerID = cus.CustomerID
                    LEFT OUTER JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID
                    LEFT OUTER JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                    LEFT OUTER JOIN AEFreightForwarder ff ON veh.FreightForwarderID = ff.AEFreightForwarderID
                    WHERE veh.VoyageID <> " + strVoyageID +
                    @" AND veh.DateShipped IS NULL
                    AND veh.VIN IN 
                    (SELECT veh.VIN FROM AutoportExportVehiclesImport imp
                    WHERE ";

                    //Finish WHERE clause based on params chosen
                    if (strBatchIDs.Length > 0)
                        { strSQL += "imp.BatchID IN  (" + strBatchIDs + "))"; }
                    else
                    {
                        //Ck Creation From Date
                        if (strFromDate.Trim().Length > 0)
                        { strval = "CreationDate >= '" + strFromDate + "' "; }

                        //Ck Creation To Date
                        if (strToDate.Length > 0)
                        {
                            if (strval.Length > 0) strval += " AND ";
                            strval += " CreationDate < DATEADD(day,1,'" + strToDate + "') ";
                        }

                        strSQL += strval + ")" ;
                    }

                    //Ck CustomerID
                    if (strCustomerID.Length > 0) strSQL += " AND veh.CustomerID = " +
                            strCustomerID;

                    //2nd SELECT for 'Not Scanned' exception
                    strval = "";
                    strSQL += @" UNION 
                    SELECT 
                    CASE 
	                    WHEN DATALENGTH(ShortName) > 0 THEN ShortName 
	                    ELSE CustomerName 
                    END AS Customer,
                    veh.VIN, 
                    veh.Model, 
                    ves.VesselName, 
                    veh.VehicleStatus,
                    veh.BayLocation,
                    CONVERT(varchar(10),veh.CustomsApprovedDate,101) AS CustomsApprovedDate,
                     'Not Scanned' AS Exception
                    FROM AutoportExportVehicles veh
                    LEFT OUTER JOIN Customer cus ON veh.CustomerID = cus.CustomerID
                    LEFT OUTER JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID
                    LEFT OUTER JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                    LEFT OUTER JOIN AEFreightForwarder ff ON veh.FreightForwarderID = ff.AEFreightForwarderID
                    WHERE veh.VoyageID = " + strVoyageID +
                    @" AND veh.DateShipped IS NULL
                    AND veh.VIN NOT IN (SELECT imp.VIN 
                        FROM AutoportExportVehiclesImport imp
		                WHERE ";

                    //Finish WHERE clause based on params chosen
                    if (strBatchIDs.Length > 0)
                    { strSQL += "imp.BatchID IN  (" + strBatchIDs + "))"; }
                    else
                    {
                        //Ck Creation From Date
                        if (strFromDate.Trim().Length > 0)
                        { strval = "CreationDate >= '" + strFromDate + "' "; }

                        //Ck Creation To Date
                        if (strToDate.Length > 0)
                        {
                            if (strval.Length > 0) strval += " AND ";
                            strval += " CreationDate < DATEADD(day,1,'" + strToDate + "') ";
                        }

                        strSQL += strval + ")";
                    }

                    //Ck CustomerID
                    if (strCustomerID.Length > 0) strSQL += " AND veh.CustomerID = " +
                            strCustomerID;

                    strSQL += " AND veh.DateReceived IS NOT NULL ";

                    //3rd SELECT for 'VIN Not Found' exception
                    strval = "";
                    strSQL += @"
                    UNION
                    SELECT '' AS Customer,
                    imp.VIN,
                    '' AS Model, 
                    '' AS VesselName, 
                    '' AS VehicleStatus, 
                    imp.BayLocation, 
                    '' AS CustomsApprovedDate,
                    'VIN Not Found' AS Exception
                    FROM AutoportExportVehiclesImport imp
                    WHERE ";

                    //Finish WHERE clause based on params chosen
                    if (strBatchIDs.Length > 0)
                    { strSQL += "imp.BatchID IN  (" + strBatchIDs + ") "; }
                    else
                    {
                        strval = "";

                        //Ck Creation From Date
                        if (strFromDate.Trim().Length > 0)
                        { strval = "CreationDate >= '" + strFromDate + "' "; }

                        //Ck Creation To Date
                        if (strToDate.Length > 0)
                        {
                            if (strval.Length > 0) strval += " AND ";
                            strval += " CreationDate < DATEADD(day,1,'" + strToDate + "') ";
                        }

                        strSQL += strval;
                    }

                    strSQL += @" AND imp.VIN NOT IN 
                    (SELECT veh.VIN FROM AutoportExportVehicles veh 
                    WHERE veh.DateShipped IS NULL) ";

                    strSQL += "ORDER BY Customer,Exception,BayLocation";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No vehicles meet the report criteria",
                            "NO VEHICLES TO INCLUDE IN THE REPORT", MessageBoxButtons.OK,
                            MessageBoxIcon.Hand);
                        return;
                    }

                    OpenPreShipInvRpt(ds.Tables[0]);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PreShipInvReport", ex.Message);
            }
        }

        private void OpenVoyageExceptionsRpt(DataTable dt)
        {
            try
            {
                DataSet ds;
                List<ControlInfo> lsCSVcols;
                string strFilename;
                string strSQL;

                //Get the file Directory & Filename from the SettingTable
                strSQL = "SELECT ValueKey,ValueDescription FROM SettingTable " +
                    "WHERE ValueKey IN ('ExportDirectory','VoyageExceptionsFileName') " +
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
                    new ControlInfo { RecordFieldName = "VIN", HeaderText = "VIN"},
                    new ControlInfo { RecordFieldName = "BookingNumber", HeaderText = "Booking#" },
                    new ControlInfo { RecordFieldName = "Forwarder",
                        HeaderText = "Forwarder" },
                    new ControlInfo { RecordFieldName = "DestinationName",
                        HeaderText = "Destination" },
                    new ControlInfo { RecordFieldName = "VoyageDate",
                        HeaderText = "Voy. Date"},
                    new ControlInfo { RecordFieldName = "VesselName",
                        HeaderText = "Vessel"},
                    new ControlInfo { RecordFieldName = "DateReceived",
                        HeaderText = "Received"},
                    new ControlInfo { RecordFieldName = "CustomsApprovedDate",
                        HeaderText = "Cleared"},

                    new ControlInfo { RecordFieldName = "Exception", HeaderText = "Exception" }
                };

                //Invoke CreateSCVFile to create, save, &open the csv file
                Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenPreShipInvRptCSV", ex.Message);
            }
        }

        private void OpenBookingRecordsRpt(DataTable dt)
        {
            try
            {
                DataSet ds;
                List<ControlInfo> lsCSVcols;
                string strFilename;
                string strSQL;

                //Get the file Directory & Filename from the SettingTable
                strSQL = "SELECT ValueKey,ValueDescription FROM SettingTable " +
                    "WHERE ValueKey IN ('ExportDirectory','BookingRecordsFileName') " +
                    "AND RecordStatus='Active' ORDER BY ValueKey DESC";
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
                    new ControlInfo { RecordFieldName = "Customer", HeaderText = "Customer" },
                    new ControlInfo { RecordFieldName = "BookingNumber", HeaderText = "Booking#" },
                    new ControlInfo { RecordFieldName = "VesselName", HeaderText = "Vessel" },
                    new ControlInfo { RecordFieldName = "VoyageNumber", HeaderText = "Voyage# " },
                    new ControlInfo { RecordFieldName = "DestinationName",
                        HeaderText = "Discharge Port" },
                    new ControlInfo { RecordFieldName = "FreightForwarderName",
                        HeaderText = "Forwarder" },
                    new ControlInfo { RecordFieldName = "AddressLine1",
                        HeaderText = "Forwarder Address" },
                    new ControlInfo { RecordFieldName = "City",
                        HeaderText = "Forwarder City" },
                    new ControlInfo { RecordFieldName = "State",
                        HeaderText = "Forwarder State" },
                    new ControlInfo { RecordFieldName = "Zip",
                        HeaderText = "Forwarder Zip" },
                    new ControlInfo { RecordFieldName = "VIN", HeaderText = "VIN"},
                    new ControlInfo { RecordFieldName = "VehicleYear",
                        HeaderText = "Year" },
                    new ControlInfo { RecordFieldName = "Make", HeaderText = "Make"},
                    new ControlInfo { RecordFieldName = "Model", HeaderText = "Model"},
                    new ControlInfo { RecordFieldName = "SizeClass", HeaderText = "Size Class"},
                    new ControlInfo { RecordFieldName = "VehicleLength", HeaderText = "Length"},
                    new ControlInfo { RecordFieldName = "VehicleWidth", HeaderText = "Width"},
                    new ControlInfo { RecordFieldName = "VehicleHeight", HeaderText = "Height"},
                    new ControlInfo { RecordFieldName = "VehicleCubicFeet",
                        HeaderText = "Cubic Feet"},
                    new ControlInfo { RecordFieldName = "VehicleWeight",
                        HeaderText = "Weight (kgs)"},
                    new ControlInfo { RecordFieldName = "DateReceived",
                        HeaderText = "Date Received"},
                    new ControlInfo { RecordFieldName = "ReceivedExceptionDate",
                        HeaderText = "Received Hold Date"},
                    new ControlInfo { RecordFieldName = "VoyageChangeHoldDate",
                        HeaderText = "Voyage Change Hold Date"},
                    new ControlInfo { RecordFieldName = "DateSubmittedCustoms",
                        HeaderText = "Customs Submit Date"},
                    new ControlInfo { RecordFieldName = "CustomsApprovedDate",
                        HeaderText = "Customs Clear Date"},
                    new ControlInfo { RecordFieldName = "CustomsExceptionDate",
                        HeaderText = "Customs Hold Date"},
                    new ControlInfo { RecordFieldName = "DateShipped",
                        HeaderText = "Ship Date"},
                    new ControlInfo { RecordFieldName = "BayLocation", HeaderText = "Bay Loc." },
                    new ControlInfo { RecordFieldName = "VIVTagNumber",
                        HeaderText = "VIV Tag#" },
                    new ControlInfo { RecordFieldName = "VehicleStatus", HeaderText = "Status" }
                };

                //Invoke CreateSCVFile to create, save, &open the csv file
                Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenPreShipInvRptCSV", ex.Message);
            }
        }

        private void OpenBookingSummaryRpt(string strCustomer,string strVoyageID,
            DataTable dtSummary1,DataTable dtSummary2)
        {
            try
            {
                DataRow drow;
                DataSet ds;
                StringBuilder sb = new StringBuilder();
                object objval;
                string strFilename;
                string strSQL;
                string strval;

                //Get the file Directory & Filename from the SettingTable
                strSQL = "SELECT ValueKey,ValueDescription FROM SettingTable " +
                    "WHERE ValueKey IN ('ExportDirectory','BookingSummaryFileName') " +
                    "AND RecordStatus='Active' ORDER BY ValueKey DESC";
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

                //Create csv file (requires special processing, can't use Formops.CreateCSVFile)
                //Use a StringBuilder for .csv data (faster than a string)

                //line 1 blank
                sb.AppendLine();

                //line 2 'All Customers' or Customer Name in 2nd col
                sb.Append("," + strCustomer);
                sb.AppendLine();

                //line 3 blank
                sb.AppendLine();

                //line 4, Col 1 blank, Col 2 blank, Col 3 'DAILY BOOKING REPORT'
                sb.Append(",,DAILY BOOKING REPORT");
                sb.AppendLine();

                //line 5 blank
                sb.AppendLine();

                //line 6, Col 1 blank, Col 2, VESSEL, Col 3 Vessel Name (from dtSummary1) 
                strval = dtSummary1.Rows[0]["VesselName"].ToString();
                sb.AppendLine(",VESSEL," + strval);

                //Get add'l info for csv
                drow = GetVoyageInfo(strVoyageID);

                //line 7, Col 1 blank, Col 2 'VOYAGE NUMBER', Col 3 VoyageNumber
                sb.AppendLine(",VOYAGE NUMBER," + 
                    drow["VoyageNumber"].ToString());

                //line 8, Col 1, blank, Col 2 'LOADING PORT', Col 3 'BOSTON'
                sb.AppendLine(",LOADING PORT," + "BOSTON");

                //line 9, Col 1 blank, Col 2 'EXPECTED TO LOAD (DATE)', 
                //  Col 3 VoyageDate (from dtSummary1)
                strval = Convert.ToDateTime(dtSummary1.Rows[0]["VoyageDate"]).ToString("M/d/yyyy");
                sb.AppendLine(",EXPECTED TO LOAD (DATE)," + strval);

                //line 10, Col 1 blank, Col 2 'LOADING DATE', Col 3 blank
                sb.AppendLine(",LOADING DATE");

                //line 11, Col 1 blank, Col 2 'REPORT DATE (DATE), Col 3 (current date)
                sb.AppendLine(",REPORT DATE (DATE)," +
                    DateTime.Now.ToString("M/d/yyyy"));

                //line 12, Col 1 blank, Col 2 'REPORTED BY (NAME)', Col 3 'John O'Donnell'
                sb.AppendLine(",REPORTED BY (NAME)," +
                    "John O'Donnell");

                //line 13, blank line
                sb.AppendLine();

                //line 14, Col 1 '1)', Col 2 'CURRENT VOYAGE CARGO'
                sb.AppendLine("1),CURRENT VOYAGE CARGO");

                //line 15, Col 1 blank, Col 2 'STATUS', Col 3 'COUNT', Col 4 'CBM',
                //  Col 5 'SCALE WEIGHT (mt)'
                sb.AppendLine(",STATUS,COUNT,CBM,SCALE WEIGHT (mt)");

                //line 16, Col 1 '(A)', Col 2 '(BOOKED - EXPECTED)', Col 3 '0', Col 4 '0',
                //  Col 5 '0'
                sb.AppendLine("(A),(BOOKED - EXPECTED),0,0,0");

                //line 17 Col 1 '(B)', Col 2 '(ON TERMINAL) Total', SUM(OnTerminalTot) [dtSummary1],
                //  Col 4 '0', Col 5 '0'
                objval = dtSummary1.Compute("SUM(OnTerminalTot)", "");
                sb.AppendLine("(B),(ON TERMINAL) Total," +
                  objval.ToString() + ",0,0");

                //1st Loop for each row in dtSummary1
                foreach (DataRow dtRow in dtSummary1.Rows)
                {
                    //line a 
                    //  Col 1 blank 
                    //  Col 2'(ON TERMINAL) To ' + [dtSummary1.DestinationName] + 'Total'
                    //  Col 3 [dtSummary1.OnTerminalTot]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(ON TERMINAL) To " + dtRow["DestinationName"] +
                        " Total," + dtRow["OnTerminalTot"] + ",0,0");

                    //line b
                    //  Col 1 blank
                    //  Col 2'(ON TERMINAL) To ' + [dtSummary1.DestinationName] + 'Car'
                    //  Col 3 [dtSummary1.OnTerminalA]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(ON TERMINAL) To " + dtRow["DestinationName"] +
                       " Car," + dtRow["OnTerminalA"] + ",0,0");

                    //line c
                    //  Col 1 blank
                    //  Col 2'(ON TERMINAL) To ' + [dtSummary1.DestinationName] + 'Small Truck'
                    //  Col 3 [dtSummary1.OnTerminalB]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(ON TERMINAL) To " + dtRow["DestinationName"] +
                       " Small Truck," + dtRow["OnTerminalB"] + ",0,0");

                    //line d
                    //  Col 1 blank
                    //  Col 2'(ON TERMINAL) To ' + [dtSummary1.DestinationName] + 'Truck'
                    //  Col 3 [dtSummary1.OnTerminalC]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(ON TERMINAL) To " + dtRow["DestinationName"] +
                       " Truck," + dtRow["OnTerminalC"] + ",0,0");

                    //line e
                    //  Col 1 blank
                    //  Col 2'(ON TERMINAL) To ' + [dtSummary1.DestinationName] + 'Misc.'
                    //  Col 3 [dtSummary1.OnTerminalE]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(ON TERMINAL) To " + dtRow["DestinationName"] +
                       " Misc.," + dtRow["OnTerminalE"] + ",0,0");

                    //line f
                    //  Col 1 blank
                    //  Col 2'(ON TERMINAL) To ' + [dtSummary1.DestinationName] + 'Oversize'
                    //  Col 3 [dtSummary1.OnTerminalE]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(ON TERMINAL) To " + dtRow["DestinationName"] +
                       " Oversize," + dtRow["OnTerminalZ"] + ",0,0");
                }

                //Next line after last Destination
                //  Col 1 '(C)'
                //  Col 2 '(NOT CLEARED)'
                //  Col 3 '0'
                //  Col 4 '0'
                //  Col 5 '0'
                sb.AppendLine("(C),(NOT CLEARED),0,0,0");

                //2nd Loop for each row in dtSummary1
                strval = "D";
                foreach (DataRow dtRow in dtSummary1.Rows)
                {
                    //line a 
                    //  Col 1 strval [1st line 'D'], all other lines blank 
                    //  Col 2'(CLEARED) To ' + [dtSummary1.DestinationName] + 'Total'
                    //  Col 3 [dtSummary1.ClearedTot]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(CLEARED To " + dtRow["DestinationName"] +
                        " Total," + dtRow["ClearedTot"] + ",0,0");

                    //line b
                    //  Col 1 blank
                    //  Col 2'(CLEARED) To ' + [dtSummary1.DestinationName] + 'Car'
                    //  Col 3 [dtSummary1.ClearedA]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(CLEARED) To " + dtRow["DestinationName"] +
                       " Car," + dtRow["ClearedA"] + ",0,0");

                    //line c
                    //  Col 1 blank
                    //  Col 2'(ON TERMINAL) To ' + [dtSummary1.DestinationName] + 'Small Truck'
                    //  Col 3 [dtSummary1.ClearedB]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(CLEARED) To " + dtRow["DestinationName"] +
                       " Small Truck," + dtRow["ClearedB"] + ",0,0");

                    //line d
                    //  Col 1 blank
                    //  Col 2'(CLEARED) To ' + [dtSummary1.DestinationName] + 'Truck'
                    //  Col 3 [dtSummary1.ClearedC]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(CLEARED) To " + dtRow["DestinationName"] +
                       " Truck," + dtRow["ClearedC"] + ",0,0");

                    //line e
                    //  Col 1 blank
                    //  Col 2'(CLEARED) To ' + [dtSummary1.DestinationName] + 'Misc.'
                    //  Col 3 [dtSummary1.ClearedE]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(CLEARED) To " + dtRow["DestinationName"] +
                       " Misc.," + dtRow["ClearedE"] + ",0,0");

                    //line f
                    //  Col 1 blank
                    //  Col 2'(CLEARED) To ' + [dtSummary1.DestinationName] + 'Oversize'
                    //  Col 3 [dtSummary1.ClearedE]
                    //  Col 4 '0'
                    //  Col 5 '0'
                    sb.AppendLine(",(CLEARED) To " + dtRow["DestinationName"] +
                       " Oversize," + dtRow["ClearedZ"] + ",0,0");
                }

                //Next line after last Destination
                //  Col 1 blank
                //  Col 2 '(CLEARED) Total'
                //  Col 3 SUM(ClearedTot)
                //  Col 4 '0'
                //  Col 5 '0'
                objval = dtSummary1.Compute("SUM(ClearedTot)", "");
                sb.AppendLine(",(CLEARED) Total," +
                objval.ToString() + ",0,0");

                //Next line 
                //  Col 1 '(E)'
                //  Col 2 '(LOADED)'
                //  Col 3 SUM(Shipped)
                //  Col 4 '0'
                //  Col 5 '0'
                objval = dtSummary1.Compute("SUM(Shipped)", "");

                sb.AppendLine("(E),(LOADED)," +
                  objval.ToString() + ",0,0");

                //Next line - blank
                sb.AppendLine();

                //Next line
                //  Col 1 '2)'
                //  Col 2 'NEXT VOYAGE CARGO'
                sb.AppendLine("2),NEXT VOYAGE CARGO");

                //Next line
                //  Col 1 blank
                //  Col 2 'STATUS'
                //  Col 3 'COUNT'
                //  Col 4 'CBM'
                //  Col 5 'SCALE WEIGHT (mt)'
                sb.AppendLine(",STATUS,COUNT,CBM,SCALE WEIGHT (mt)");

                //Next line
                //  Col 1 '(A)'
                //  Col 2 '(BOOKED - EXPECTED)'
                //  Col 3 '0'
                //  Col 4 '0'
                //  Col 5 '0'
                sb.AppendLine("(A),(BOOKED - EXPECTED),0,0,0");

                //Next line
                //  Col 1 '(B)'
                //  Col 2 '(ON TERMINAL)'
                //  Col 3 dtSummary2.OnTerminal
                //  Col 4 '0'
                //  Col 5 '0'
                sb.AppendLine("(B),(ON TERMINAL)," + 
                    dtSummary2.Rows[0]["OnTerminal"].ToString() + ",0,0");

                //Next line
                //  Col 1 '(C)'
                //  Col 2 '(NOT CLEARED)'
                //  Col 3 dtSummary2.NotCleared
                //  Col 4 '0'
                //  Col 5 '0'
                sb.AppendLine("(C),(NOT CLEARED)," +
                    dtSummary2.Rows[0]["NotCleared"].ToString() + ",0,0");

                //Last line
                //  Col 1 '(D)'
                //  Col 2 '(CLEARED)'
                //  Col 3 dtSummary2.Cleared
                //  Col 4 '0'
                //  Col 5 '0'
                sb.AppendLine("(D),(CLEARED)," +
                    dtSummary2.Rows[0]["Cleared"].ToString() + ",0,0");

                strval = sb.ToString();

                File.WriteAllText(strFilename, strval);
                Process.Start(strFilename);
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenBookingSummaryRpt", ex.Message);
            }
        }

        private DataRow GetVoyageInfo(string strVoyageID)
        {
            try
            {
                DataSet ds;
                string strSQL;

                strSQL = @"SELECT
                VoyageNumber
                FROM AEVoyage
                WHERE AEVoyageID = " + strVoyageID;
                ds = DataOps.GetDataset_with_SQL(strSQL);
                return ds.Tables[0].Rows[0];
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetVoyageInfo", ex.Message);
                return null;
            }
        }

        private void OpenPreShipInvRpt(DataTable dt)
        {
            try
            {
                DataSet ds;
                List<ControlInfo> lsCSVcols;
                string strFilename;
                string strSQL;

                //Get the file Directory & Filename from the SettingTable
                strSQL = "SELECT ValueKey,ValueDescription FROM SettingTable " +
                    "WHERE ValueKey IN ('ExportDirectory','InvCompFileName') " +
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
                    new ControlInfo { RecordFieldName = "Customer", HeaderText = "Customer" },
                    new ControlInfo { RecordFieldName = "VIN", HeaderText = "VIN"},
                    new ControlInfo { RecordFieldName = "Model", HeaderText = "Model"},
                    new ControlInfo { RecordFieldName = "VesselName", HeaderText = "Vessel" },
                    new ControlInfo { RecordFieldName = "VehicleStatus", HeaderText = "Veh. Status" },
                    new ControlInfo { RecordFieldName = "BayLocation", HeaderText = "Bay Loc." },
                    new ControlInfo { RecordFieldName = "CustomsApprovedDate", HeaderText = "Cust. Clear Date" },
                    new ControlInfo { RecordFieldName = "Exception", HeaderText = "Exception" }                    
                };

                //Invoke CreateSCVFile to create, save, &open the csv file
                Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenPreShipInvRptCSV", ex.Message);
            }
        }

        private void ShipManifest()
        {
            try
            {
                ComboboxItem cboItem;
                ComboBox cboVoyage;
                DialogResult dlResult;
                DataSet ds;
                DataTable dtManifest;
                frmSetSelection frm;
                int intLinenum = 1;
                List<ControlInfo> lsCols;
                string strFileName = "";
                string strSQL;

                //Create cboVoyage for frm
                strSQL = @"SELECT 
                CONVERT(varchar(10),VoyageDate,101) AS VoyDate, 
                AEVoyageID
                FROM AEVoyage
                WHERE VoyageDate < '3000'
                ORDER BY VoyageDate DESC";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ShipManifest",
                        "No data returned from AEVoyage table for cboVoyage");
                    return;
                }

                cboVoyage = new ComboBox();

                //Set '<select>' as 1st item
                cboItem = new ComboboxItem();
                cboItem.cboText = "<select>";
                cboItem.cboValue = "select";
                cboVoyage.Items.Add(cboItem);

                //Load voyage recs
                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    cboItem = new ComboboxItem();
                    cboItem.cboText = dtRow["VoyDate"].ToString();
                    cboItem.cboValue = dtRow["AEVoyageID"].ToString();
                    cboVoyage.Items.Add(cboItem);
                }

                cboVoyage.SelectedIndex = 0;

                //Open frmSetSelection in modal form, to get new Destination
                frm = new frmSetSelection("Voyage Date", cboVoyage, "Please select the Voyage Date for\n" +
                    "the Ship Manifest.", true);
                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                {
                    //Get Directory  from SettingTable
                    strSQL = @"select ValueKey,ValueDescription 
                    FROM SettingTable 
                    WHERE ValueKey LIKE 'AEShipManifest%'
                    ORDER BY ValueKey";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "ShipManifest",
                            "No data returned from query to SettingTable table");
                        return;
                    }

                    //Set strFileName to Directory from SettingTable
                    strFileName = ds.Tables[0].Rows[0]["ValueDescription"].ToString();

                    //Create Directory if it doesn't exist
                    Directory.CreateDirectory(strFileName);

                    //Add unique filename
                    strFileName += "ShipManifest" +
                        DateTime.Now.ToString("yyyyMMddHHmm") + ".csv";

                    strSQL = @"SELECT
                    0 AS linenum, 
                    veh.VIN, 
                    veh.DateShipped, 
                    ves.VesselName, 
                    veh.DestinationName, 
                    ISNULL(veh.VehicleWeight,0) AS VehicleWeight
                    FROM AutoportExportVehicles veh
                    LEFT OUTER JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID
                    LEFT OUTER JOIN AEVessel VES ON voy.AEVesselID = ves.AEVesselID
                    WHERE veh.DateShipped IS NOT NULL
                    AND veh.VoyageID = " + Globalitems.strSelection +
                    "ORDER BY veh.VIN";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There are no records for the Voyage Date",
                            "NO RECORDS FOR MANIFEST", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    dtManifest = ds.Tables[0];

                    //Update linenum in dtManifest
                    foreach (DataRow dtRow in dtManifest.Rows)
                    {
                        dtRow["linenum"] = intLinenum;
                        intLinenum += 1;
                    }

                    //Create lsCols for csv file
                    lsCols = new List<ControlInfo>()
                    {
                         new ControlInfo { RecordFieldName = "linenum", HeaderText = "" },
                         new ControlInfo { RecordFieldName = "VIN", HeaderText = "VIN" },
                         new ControlInfo { RecordFieldName = "VehicleWeight",
                             HeaderText = "Weight" },
                         new ControlInfo { RecordFieldName = "DateShipped",
                             HeaderText = "Ship Date" },
                         new ControlInfo { RecordFieldName = "VesselName",
                             HeaderText = "Vessel" },
                         new ControlInfo { RecordFieldName = "DestinationName",
                             HeaderText = "Destination" }
                    };

                    Formops.CreateCSVFile(dtManifest, lsCols, strFileName);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ShipManifest", ex.Message);
            }
        }

        private void VoyageExceptions()
        {
            try
            {
                DialogResult dlResult;
                DataSet ds;
                frmInvRptParams frm;
                string strCustomerID = "";
                string[] strParams;
                string[] strNameValuePair;
                string strVoyageID = "";

                frm = new frmInvRptParams();
                frm.strInvtype = "EX";
                string strSQL;

                dlResult = frm.ShowDialog();
                if (dlResult == DialogResult.OK)
                {
                    //Decode returned string into strParams
                    strParams = Globalitems.strSelection.Split(Globalitems.chrRecordSeparator);

                    if (strParams.Length == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "VoyageExceptions",
                            "No elements split into strParams after frmInvRptParams.\n" +
                            "strSelection: " + Globalitems.strSelection);
                        return;
                    }

                    foreach (string strParam in strParams)
                    {
                        strNameValuePair = strParam.Split(Globalitems.chrNameValueSeparator);
                        if (strNameValuePair.Length == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "BookingRecordsReport",
                                "No Name/Value Pair split from strParam: " + strParam);
                            return;
                        }

                        if (strNameValuePair.Length != 2)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "BookingRecordsReport",
                                "Name/Value Pair split <> 2 from strParam: " + strParam);
                            return;
                        }

                        //Process depending on Name in strNameValuePair[0]
                        switch (strNameValuePair[0])
                        {
                            case "CustomerID":
                                strCustomerID = strNameValuePair[1];
                                break;
                            case "VoyageID":
                                strVoyageID = strNameValuePair[1];
                                break;
                        }
                    }   // foreach strParam in strParams

                    strSQL = @" SELECT 
                    veh.VIN, 
                    veh.BookingNumber, 
                    veh.DestinationName, 
                    voy.VoyageDate, 
                    ves.VesselName, 
                    veh.DateReceived, 
                    veh.CustomsApprovedDate,
                    CASE 
                        WHEN DATALENGTH(ff.FreightForwarderShortName) > 0 THEN 
                            ff.FreightForwarderShortName 
                        ELSE ff.FreightForwarderName 
                    END AS Forwarder,
                    'Not Customs Cleared Status' AS Exception
                    FROM AutoportExportVehicles veh
                    LEFT OUTER JOIN AEFreightForwarder ff ON 
                        VEH.FreightForwarderID = ff.AEFreightForwarderID
                    LEFT OUTER JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID
                    LEFT OUTER JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                    WHERE veh.VoyageID = " + strVoyageID + @" AND 
                      veh.CustomsApprovedDate IS NULL ";

                    // Add params to WHERE clause
                    //CustomerID
                    if (strCustomerID.Length > 0) strSQL += "AND veh.CustomerID = " +
                            strCustomerID + " ";

                    //Add UNION SELECT
                    strSQL += @" UNION 
                    SELECT 
                    veh.VIN, 
                    veh.BookingNumber, 
                    veh.DestinationName, 
                    voy.VoyageDate, 
                    ves.VesselName, 
                    veh.DateReceived, 
                    veh.CustomsApprovedDate,
                    CASE 
                        WHEN DATALENGTH(ff.FreightForwarderShortName) > 0 THEN 
                            ff.FreightForwarderShortName 
                        ELSE ff.FreightForwarderName 
                    END AS Forwarder,
                    'Customs Cleared - Needs Approval To Load' AS Exception
                    FROM AutoportExportVehicles veh
                    LEFT OUTER JOIN AEFreightForwarder ff ON 
                        veh.FreightForwarderID = ff.AEFreightForwarderID
                    LEFT OUTER JOIN AEVoyage voy ON veh.VoyageID = voy.AEVoyageID
                    LEFT OUTER JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID
                    WHERE veh.VoyageID <> " + strVoyageID + @"
                    AND veh.CustomsApprovedDate IS NOT NULL ";

                    // Add params to WHERE clause
                    //CustomerID
                    if (strCustomerID.Length > 0) strSQL += "AND veh.CustomerID = " +
                            strCustomerID + " ";

                    strSQL += @"AND veh.CustomerID IN 
                        (SELECT voycust.CustomerID 
                        FROM AEVoyageCustomer voycust 
                        WHERE voycust.AEVoyageID = " + strVoyageID + @")
                        AND veh.DateShipped IS NULL
                        ORDER BY Exception, Forwarder, veh.VIN";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("There are no Voyage Exception records to export.",
                            "NO VOYAGE EXCEPTION RECORDS", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    OpenVoyageExceptionsRpt(ds.Tables[0]);

                }   //iF dlResult = OK
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "VoyageExceptionsReport",
                    ex.Message);
            }
        }

        private void btnImportShp_Click(object sender, EventArgs e)
        { OpenImportVehYMSForm("SHIP"); }

        private void btnImportRcvd_Click(object sender, EventArgs e)
        { OpenImportVehYMSForm("RCVD"); }

        private void btnImportPhyClone_Click(object sender, EventArgs e)
        { OpenImportVehYMSForm("CLONE"); }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }

        private void btnPreShipInv_Click(object sender, EventArgs e)
        { PreShipInvReport(); }

        private void btnPostShipInv_Click(object sender, EventArgs e)
        {Formops.InventoryComparisonReport();}

        private void btnExpCustApproved_Click(object sender, EventArgs e)
        {ExportCustomsApproved();}

        private void btnCBPApproved_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not sure needs to be implemented",
                "NECESSARY?", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnShipManifest_Click(object sender, EventArgs e)
        {ShipManifest();}

        private void btnExpBookRecs_Click(object sender, EventArgs e)
        { BookingRecordsReport(); }

        private void btnExpBookSummary_Click(object sender, EventArgs e)
        {BookingSummary();}

        private void btnExpVoyExceptions_Click(object sender, EventArgs e)
        { VoyageExceptions(); }

        private void frmImportExportMenu_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
