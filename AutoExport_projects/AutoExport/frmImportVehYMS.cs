using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmImportVehYMS : Form
    {
        //strImportType can be:
        //RCVD: User clicked Import Rcvd Vehicles button
        //CLONE: User clicked Import Phy Clone button
        //SHIP: User clicked 
        public string strImportType;

        private const string CURRENTMODULE = "frmImportVehYMS";
  
        private DataTable dtImports;
        private DataTable dtScannedVehicles = null;
        private int intBatchID;
        private string strStatus;

        //strAction can be:
        //Import: new recs in Import table, but not yet in Veh. table
        //Process: recs are in BOTH Import & Veh table 
        private string strAction;

        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
             // objects needed for csv file  
            new ControlInfo {RecordFieldName="VIN",HeaderText="VIN"},
            new ControlInfo {RecordFieldName="BayLocation",HeaderText="Bay Loc."},
            new ControlInfo {RecordFieldName="DestinationName",HeaderText="Dest. Abbrev."},
            new ControlInfo {RecordFieldName="Make",HeaderText="Make"},
            new ControlInfo {RecordFieldName="Model",HeaderText="Model"},
            new ControlInfo {RecordFieldName="Bodystyle",HeaderText="Body Style"},
            new ControlInfo {RecordFieldName="SizeClass",HeaderText="Size Class"},
            new ControlInfo {RecordFieldName="VehicleHeight",HeaderText="Height"},
            new ControlInfo {RecordFieldName="BookingNumber",HeaderText="Booking #"},
            new ControlInfo {RecordFieldName="NonRunner",HeaderText="Non Runner?"},
            new ControlInfo {RecordFieldName="CustomerName",HeaderText="Cust. Abbrev."},
            new ControlInfo {RecordFieldName="RecordStatus",HeaderText="Status"},
            new ControlInfo {RecordFieldName="labels",HeaderText="Label to Print?"},

            //For csv files
            new ControlInfo { RecordFieldName = "Customer", HeaderText = "Customer" },
            new ControlInfo { RecordFieldName = "VoyageNumber", HeaderText = "Voyage #" },
            new ControlInfo { RecordFieldName = "VoyageDate", HeaderText = "Voyage Date" },
            new ControlInfo { RecordFieldName = "DestinationName", HeaderText = "Destination" },
            new ControlInfo { RecordFieldName = "VesselName", HeaderText = "Vessel" },
            new ControlInfo { RecordFieldName = "VehicleStatus", HeaderText = "Veh. Status" },
            new ControlInfo { RecordFieldName = "VehicleYear", HeaderText = "Year" },
            new ControlInfo { RecordFieldName = "Color", HeaderText = "Color" },
            new ControlInfo { RecordFieldName = "DateReceived", HeaderText = "Date Rcv'd" },
            new ControlInfo { RecordFieldName = "DateSubmittedCustoms", HeaderText = "Date Sub'd Customs" },
            new ControlInfo { RecordFieldName = "CustomsExceptionDate", HeaderText = "Cust. Exc. Date" },
            new ControlInfo { RecordFieldName = "CustomsApprovedDate", HeaderText = "Cust. Clear Date" },
            new ControlInfo { RecordFieldName = "DateShipped", HeaderText = "Date Shipped" },
            new ControlInfo { RecordFieldName = "ExceptionCode", HeaderText = "Exception" }
        };

        public frmImportVehYMS()
        {
            InitializeComponent();

            dgResults.AutoGenerateColumns = false;
            btnSizeClass.Enabled = false;
            btnRunStatus.Enabled = false;
            btnNoLabels.Visible = false;
            btnResubVoyage.Enabled = false;
            btnCust.Enabled = false;
            btnDest.Enabled = false;

            if (Globalitems.blnCannotPrintLabels)
            {
                btnLabels.Enabled = false;
                btnNoLabels.Visible = true;
            }

            FillCombos();
        }

        private void frmImportVehYMS_Activated(object sender, EventArgs e)
        {
            
            int intFormWidth_rcvd = 1530;    //Full wide for Import Vehicle
            int intFormWidth_not_rcvd = 1185;
            int intbtnExport_x_rcvd = 1190;
            int intbtnExport_x_not_rcvd = 635;

            //Display for VEH/CLONE imports
            btnProcess.Visible = true;
            btnLabels.Visible = true;
            lblReports.Visible = true;
            pnlReports.Visible = true;
            dgResults.Columns["BayLoc"].Visible = true;

            //Hide for RCVD/CLONE imports
            ckExcludeShippedVehs.Visible = false;           

            //Hide for CLONE/SHIP
            btnSizeClass.Visible = false;
            btnRunStatus.Visible = false;
            btnResubVoyage.Visible = false;
            ckActive.Visible = false;
            lblUsers.Visible = false;
            cboUsers.Visible = false;
            dgResults.Columns["Destination"].Visible = false;
            dgResults.Columns["Make"].Visible = false;
            dgResults.Columns["Model"].Visible = false;
            dgResults.Columns["BodyStyle"].Visible = false;
            dgResults.Columns["SizeClass"].Visible = false;
            dgResults.Columns["VehicleHeight"].Visible = false;
            dgResults.Columns["BookingNumber"].Visible = false;
            dgResults.Columns["NonRunner"].Visible = false;
            dgResults.Columns["CustomerName"].Visible = false;

            if (strImportType == "RCVD")
            {
                btnExport.Left = intbtnExport_x_rcvd;
                this.Width = intFormWidth_rcvd;
            }
            else
            {
                btnExport.Left = intbtnExport_x_not_rcvd;
                this.Width = intFormWidth_not_rcvd;
            }

            //Change various form/control properties, based on Import type
            switch (strImportType)
            {
                case "RCVD":
                    this.Text = "AutoExport: Import Received Vehicles";        
                    txtImportType.Text = "IMPORT RECEIVED VEHICLES";
                    btnSizeClass.Visible = true;
                    btnRunStatus.Visible = true;
                    btnResubVoyage.Visible = true;
                    ckActive.Visible = true;
                    lblUsers.Visible = true;
                    cboUsers.Visible = true;
                    dgResults.Columns["Destination"].Visible = true;
                    dgResults.Columns["Make"].Visible = true;
                    dgResults.Columns["Model"].Visible = true;
                    dgResults.Columns["BodyStyle"].Visible = true;
                    dgResults.Columns["SizeClass"].Visible = true;
                    dgResults.Columns["VehicleHeight"].Visible = true;
                    dgResults.Columns["BookingNumber"].Visible = true;
                    dgResults.Columns["NonRunner"].Visible = true;
                    dgResults.Columns["CustomerName"].Visible = true;
                    dgResults.Columns["BayLoc"].Visible = true;
                    break;
                case "CLONE":
                    this.Text = "AutoExport: Import Phy. Inven. Clone";
                    txtImportType.Text = "IMPORT PHY. INVEN. CLONE";
                    ckActive.Visible = false;
                    lblUsers.Visible = false;
                    cboUsers.Visible = false;
                    break;
                case "SHIP":
                    this.Text = "AutoExport: Import Shipped - YMS";
                    ckExcludeShippedVehs.Visible = true;
                    btnProcess.Visible = false;
                    btnLabels.Visible = false;
                    txtImportType.Text = "IMPORT SHIPPED";
                    lblReports.Visible = false;
                    pnlReports.Visible = false;
                    dgResults.Columns["BayLoc"].Visible = false;
                    break;
            }

            if (Globalitems.runmode == "TEST") this.Text = "TEST - " + this.Text;
            
            progbar.Visible = false;
            txtStatus.Visible = false;

            if (dgResults.RowCount == 0)
            {
                btnExport.Enabled = false;
                btnProcess.Enabled = false;
            }
        }

        private void CreateTmpTableForImport(string strImporter)
        {
            try
            {
                DateTime datNow = DateTime.Now;
                string strDateNow;
                string strSQL;
                string strtmptable = "";

                //Create a unique string based on datetime for tmp table name in SQL DB
                strDateNow = datNow.ToString("yyyyMMddHHmmss");

                strtmptable = "tmpReceivedVehicles_" + strDateNow;

                //Create tmp table in DB for data returned from 
                //SProc DAIYMSP_GetExportReceiptData for RCVD import
                //SProc DAIYMSP_GetExportPhyCloneData for CLONE import
                //SProc DAIYMSP_GetExportInventoryData for SHIP import
                strSQL = @"CREATE TABLE " + strtmptable;
                switch (strImportType)
                {
                    case "RCVD":
                        strSQL += @"(VIN varchar(20),
                        VIVTagNumber varchar(10),
                        DestinationPortName varchar(100),
                        Row varchar(10),
                        Bay varchar(10),                    
                        CustomerName varchar(100),
                        Color varchar(100),
                        IsRunner bit,
                        HandheldActionDate datetime,
                        UserCode varchar(20));";
                        break;
                    case "CLONE":
                        strSQL += @"(VINNumberAndVINKey varchar(20), 
			            Row varchar(10), 
			            Bay varchar(10), 
			            ByRowFlg bit,
			            HandheldActionDate datetime, 
			            UserCode varchar(20));";
                        break;
                    case "SHIP":
                        strSQL += @"(VINNumberAndVINKey varchar(20), 
			            Port varchar(50), 
			            VoyageDate varchar(50), 
			            VesselName varchar(50), 
			            HandheldActionDate datetime, 
			            UserCode varchar(20));";
                        break;
                }
                    
                DataOps.PerformDBOperation(strSQL);

                strStatus = "Create new Import recs";
                bckLoadData.ReportProgress(40);

                //Use SQLBulkCopy to load dt into tmp table
                DataOps.PerformBulkCopy(strtmptable, dtScannedVehicles);

                strStatus = "Review status";
                bckLoadData.ReportProgress(75);

                CopyReceiptDataToImportTable(intBatchID, strtmptable,strImporter);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CreateTmpTableForImport", ex.Message);
            }
        }

        private void CheckVehInfo()
        {
            try
            {
                string strval;

                //Check SizeClass: enable if missing
                btnSizeClass.Enabled = false;
                btnCust.Enabled = false;
                btnDest.Enabled = false;

                if (dgResults.SelectedRows.Count > 0)
                {
                    strval = dgResults.SelectedRows[0].Cells["Status"].Value.ToString();
                    if (strval.Contains("NEEDED")) btnSizeClass.Enabled = true;

                    if(strval.Contains("NOT FOUND"))
                    {
                        btnDest.Enabled = true;
                        btnCust.Enabled = true;
                    }

                    if (strval.Contains("Imported"))
                        strAction = "Process";
                    else
                    {
                        //ck if any rows (1-n) have Imported status
                        foreach (DataGridViewRow dgRow in dgResults.Rows)
                        {
                            strval = (dgRow.Cells["Status"].Value.ToString());
                            if (strval.Contains("Imported")) strAction = "Process";
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CheckVehInfo",
                    ex.Message);
            }
        }

        private void ClearGrid()
        {
            dgResults.DataSource = null;
            btnExport.Enabled = false;
            lblBatchID.Text = "";
            lblBatchDetails.Text = "";
            btnLabels.Enabled = false;
            lblBatchRecords.Text = "";
            lblStatus.Text = "";
        }

        private void CopyReceiptDataToImportTable(int intBatchID, string strTmptable,
            string strImporter)
           
        {
            try
            {
                DataSet ds;
                string strSQL = "";
                string strVIN;
                string strStatus;
                VINInfo objVINInfo;

                //Insert new recs into AutoportExportVehiclesImport table from tmp table &
                // associated tables
                //NOTE: in SQL Server, SUBSTRING starting at 0, works differently
                //than starting at 1, safer to use SUBSTRIN(VIN,1,8)

                //1/18/18 David Maibor: use HandheldActionDate as CreationDate for RCVD processing. This will make the HandheldActionDate the DateReceived
                //  value in the AutoportExportVehicles table when the user clicks the Process File button.
               

                switch (strImportType)
                {
                    case "RCVD":
                        strSQL = @"INSERT INTO AutoportExportVehiclesImport (BatchID,VIN,
                        BayLocation,BookingNumber,VehicleYear,Make,Model,
                        Bodystyle,VehicleLength,VehicleWidth,VehicleHeight,
                        VehicleWeight,VehicleCubicFeet,VINDecodedInd,
                        DestinationName,CustomerName,RecordStatus,Inspector,
                        CreationDate,CreatedBy,SizeClass,Color,RunnerInd,ImportedInd)
                        SELECT " + intBatchID + @" AS BatchID,
                        tmp.VIN,
                        CASE
                            WHEN LEN(tmp.Bay) = 1 THEN RTRIM(tmp.Row) + ' 0' + RTRIM(tmp.Bay) 
                            ELSE RTRIM(tmp.Row) + ' ' + RTRIM(tmp.Bay) 
                        END AS BayLocation,
                        '' AS BookingNumber,
                        veh.VehicleYear,
                        veh.Make,
                        veh.Model,
                        veh.Bodystyle,
                        veh.VehicleLength,
                        veh.VehicleWidth,veh.VehicleHeight,veh.VehicleWeight,
                        veh.VehicleCubicFeet,
                        0 AS VINDecodedInd,
                        tmp.DestinationPortName AS DestinationName,
                        tmp.CustomerName,
                        CASE
                            WHEN LEN(RTRIM(ISNULL(veh.SizeClass,''))) = 0 THEN 'SIZE CLASS NEEDED'
                            ELSE 'Import Pending'
                        END AS RecordStatus,
                        tmp.UserCode AS Inspector,
                        tmp.HandheldActionDate AS CreationDate,
                        '" + strImporter + @"' AS CreatedBy,
                        veh.SizeClass,
                        tmp.Color,
			            tmp.IsRunner AS RunnerInd,
                        0 AS ImportedInd
                        FROM " + strTmptable + @" tmp 
                        LEFT OUTER JOIN AutoportExportVehicles veh on
                            veh.VIN <> tmp.VIN
                            AND veh.VIN LIKE SUBSTRING(tmp.VIN, 1, 8) + '_' + SUBSTRING(tmp.VIN, 10, 2) + '%'
                            AND veh.CreationDate = 
                            (SELECT TOP 1 CreationDate FROM AutoportExportVehicles
	                        WHERE VIN <> tmp.VIN
                            AND VIN LIKE SUBSTRING(tmp.VIN, 1, 8) + '_' + SUBSTRING(tmp.VIN, 10, 2) + '%'
                            AND VINDecodedInd=1 AND LEN(RTRIM(ISNULL(VehicleHeight,''))) > 0 
                                    AND VehicleHeight<> '0' 
					        ORDER BY CreationDate DESC)";
                        break;
                    case "CLONE":
                        strSQL = @"INSERT INTO AutoportExportVehiclesImport(BatchID, VIN,
                        BayLocation, Inspector,RecordStatus,CreationDate, CreatedBy,ImportedInd) 
                        SELECT " + intBatchID + @" AS BatchID,
                        tmp.VINNumberAndVINKey AS VIN,
                        CASE
                            WHEN LEN(tmp.Bay) = 1 THEN RTRIM(tmp.Row) + ' O' + RTRIM(tmp.Bay) 
                            ELSE RTRIM(tmp.Row) + ' ' + RTRIM(tmp.Bay) 
                        END AS BayLocation,
                        tmp.UserCode AS Inspector,
                       'Import Pending' AS RecordStatus,
                        CURRENT_TIMESTAMP AS CreationDate,
                        '" + strImporter + @"' AS CreatedBy,
                        0 AS ImportedInd 
                        FROM " + strTmptable + " tmp";
                        break;
                    case "SHIP":
                        strSQL = @"INSERT INTO AutoportExportShippedVehiclesImport(BatchID, VIN,
                        DateShipped,ImportedInd,ImportedDate,Importedby,RecordStatus,
                        CreationDate,Createdby) 
                        SELECT " + intBatchID + @" AS BatchID,
                        tmp.VINNumberAndVINKey AS VIN,
                        tmp.HandheldActionDate AS DateShipped,
                        0 AS ImportedInd,
                        CURRENT_TIMESTAMP AS ImportedDate,
                        '" + strImporter + @"' AS ImportedBy,
                        'Import Pending' AS RecordStatus,
                        CURRENT_TIMESTAMP AS CreationDate,
                        '" + strImporter + @"' AS CreatedBy
                        FROM " + strTmptable + " tmp;";
                        break;
                }

                bckLoadData.ReportProgress(95);

                DataOps.PerformDBOperation(strSQL);             

                //Drop tmp table in DB
                strSQL = "DROP TABLE " + strTmptable;
                DataOps.PerformDBOperation(strSQL);

                //If recs in AutoportExportVehiclesImport table are missing VIN info, 
                // use DecodeVIN to get the info, if VEH ImportType
                if (strImportType == "RCVD")
                {
                    strSQL = @"select DISTINCT VIN
                    from AutoportExportVehiclesImport 
                    WHERE BatchID = " + intBatchID + @" AND 
                    LEN(RTRIM(ISNULL(VehicleHeight,''))) = 0";

                    ds = DataOps.GetDataset_with_SQL(strSQL);

                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "CopyReceiptDataToImportTable",
                            "No table returned when checking for VINs needing decoding.");
                        return;
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drow in ds.Tables[0].Rows)
                        {
                            strVIN = drow["VIN"].ToString();
                            objVINInfo = Globalitems.DecodeVIN(strVIN);

                            strStatus = "Import Pending";

                            if (objVINInfo.Error)
                            {
                                MessageBox.Show(Globalitems.DecodeVINErrorMsg(objVINInfo),
                                       "VIN CANNOT BE DECODED",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);

                                //Send email & log error
                                //Globalitems.HandleException(CURRENTMODULE, 
                                //    "CopyReceiptDataToImportTable",
                                //    "The following VIN Decoding error occurred for VIN: " +
                                //    objVINInfo.VIN + ": " + objVINInfo.ErrorDesc,false);
                            }

                            //Set VIN info in record to new values retrieved
                            strSQL = @"UPDATE AutoportExportVehiclesImport
                            SET VehicleYear = '" + objVINInfo.VehicleYear + @"',
                            Make = '" + objVINInfo.Make + @"',
                            Model = '" + objVINInfo.Model + @"',
                            Bodystyle = '" + objVINInfo.Bodystyle + @"',
                            VehicleLength = '" + objVINInfo.VehicleLength + @"',
                            VehicleWidth = '" + objVINInfo.VehicleWidth + @"',
                            VehicleHeight = '" + objVINInfo.VehicleHeight + @"',
                            VehicleWeight = '" + objVINInfo.VehicleWeight + @"',
                            VehicleCubicFeet = '" + objVINInfo.VehicleCubicFeet + @"',";

                            //Set VINDecodedInd
                            if (objVINInfo.VINDecoded)
                                strSQL += "VINDecodedInd = 1,";
                            else
                                strSQL += "VINDecodedInd = 0,";

                            //Set cboSize if value returned
                            if (objVINInfo.SizeClass.Length > 0)
                                strSQL += "SizeClass = '" + objVINInfo.SizeClass + "',";
                            else
                                strStatus = "SIZE CLASS NEEDED";

                            strSQL += "RecordStatus = '" + strStatus + "' ";
                            strSQL += " WHERE VIN = '" + strVIN + "'";
                            DataOps.PerformDBOperation(strSQL);
                        }   // foreach row 
                    }
                }

                bckLoadData.ReportProgress(100);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CopyReceiptDatatoImportTable",
                    ex.Message);
            }
        }

        private void FillCombos()
        {
            try
            {
                ComboboxItem cboitem;
                DataSet ds;
                string strSQL;

                //For PROD, JOIN w/DataHub tables
                //#DATAHUB
                if (Globalitems.runmode == "PROD")
                strSQL = @"WITH CTE AS
                    (SELECT us.UserCode,us.LastName,us.FirstName,
                    COUNT(rec.DAIYMS_Ref_InspectorId) as totrecs,
                    ROW_NUMBER() OVER (ORDER BY us.LastName,us.FirstName) AS linenum
                    FROM DataHub.dbo.DAIYMS_Output_ExpRec rec
                    INNER JOIN DataHub.dbo.DAIYMS_Ref_Inspector ins 
                    ON ins.DAIYMS_Ref_InspectorId=rec.DAIYMS_Ref_InspectorId
                    INNER JOIN Users us on us.UserCode=ins.InspectorCode
                    LEFT JOIN UserRole ur ON ur.UserId = us.UserID 
                    WHERE rec.HandheldActionDate > CONVERT(varchar(4),YEAR(GETDATE()))
                    AND ur.RoleName = 'YardOperations' ";
                else
                    strSQL = @"WITH CTE AS
                    (SELECT us.UserCode,us.LastName,us.FirstName,
                    COUNT(rec.DAIYMS_Ref_InspectorId) as totrecs,
                    ROW_NUMBER() OVER (ORDER BY us.LastName,us.FirstName) AS linenum
                    FROM DAIYMS_Output_ExpRec rec
                    INNER JOIN DAIYMS_Ref_Inspector ins 
                    ON ins.DAIYMS_Ref_InspectorId=rec.DAIYMS_Ref_InspectorId
                    INNER JOIN Users us on us.UserCode=ins.InspectorCode
                    LEFT JOIN UserRole ur ON ur.UserId = us.UserID 
                    WHERE rec.HandheldActionDate > CONVERT(varchar(4),YEAR(GETDATE()))
                    AND ur.RoleName = 'YardOperations' ";

                if (ckActive.Checked)
                    strSQL += "AND us.RecordStatus = 'Active' ";

                strSQL += @"GROUP BY us.UserCode,us.LastName,us.FirstName
                    HAVING COUNT(rec.DAIYMS_Ref_InspectorId) > 5)
                    SELECT UserCode,LastName,FirstName,linenum
                    FROM CTE
                    UNION
                    SELECT u.UserCode,u.LastName,u.FirstName, 99999 AS linenum
                    FROM Users u
                    LEFT JOIN UserRole ur ON ur.UserId = u.UserID 
                    WHERE u.RecordStatus = 'Active'
                    AND LEN(RTRIM(ISNULL(u.FirstName,''))) > 0
                    AND LEN(RTRIM(ISNULL(u.LastName,''))) > 0
                    AND ur.RoleName = 'YardOperations '
                    AND u.UserCode NOT IN 
                    (SELECT UserCode FROM CTE)
                    ORDER BY linenum,LastName,FirstName";
                        
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds == null || ds.Tables.Count == 0)
                {
                    MessageBox.Show("There are no active Autoport Export users " +
                        "in the DB.", "NO ACTIVE USERS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //There is at least one user. Add '<select>' as first record
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";

                cboUsers.Items.Add(cboitem);

                //Add retrieved Users in ds
                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = drow["LastName"].ToString().Trim() + 
                        ", " + drow["FirstName"].ToString().Trim();
                    cboitem.cboValue = drow["UserCode"].ToString();
                    cboUsers.Items.Add(cboitem);
                }

                cboUsers.DisplayMember = "cboText";
                cboUsers.ValueMember = "cboValue";
                cboUsers.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void GetGridData()
        {
            //10/19/18 D.Maibor: fix SELECT statement for 'SHIP' import type
            //Retrieve data for grid & store in dtImports
            try
            {
                DataSet ds;
                string strSQL;

                //Set up SQL based on Import Type

                if (strImportType != "SHIP")
                {
                    strSQL = @"SELECT DISTINCT AutoportExportVehiclesImportID AS RecordID,
                    imp.ImportedInd,
                    imp.VIN,imp.BayLocation,
                    imp.DestinationName,imp.Make,imp.Model,imp.Bodystyle,imp.SizeClass,
                    imp.VehicleHeight,imp.BookingNumber,
                    CASE
                        WHEN imp.RunnerInd = 0 THEN 'YES'
                        ELSE 'NO'
                    END AS NonRunner,";

                    if (strAction == "Import")
                    {
                        strSQL += "'Import pending' AS Recordstatus,";
                    }
                    else
                    {
                        //strSQL += @"CASE
                        //    WHEN veh.VIN IS NULL THEN 'VIN NOT FOUND'
                        //    ELSE imp.RecordStatus
                        //END AS RecordStatus,";

                        strSQL += "imp.RecordStatus,";
                    }
					
                    strSQL += @"imp.CustomerName,imp.Inspector,imp.CreatedBy,imp.CreationDate
                    FROM AutoportExportVehiclesImport imp
					LEFT OUTER JOIN AutoportExportVehicles veh on veh.VIN=imp.VIN
                    WHERE BatchID = " + intBatchID.ToString();

                    //Change ORDER BY depending on Import type
                    if (strImportType == "RCVD")
                        strSQL += " ORDER BY RecordStatus DESC, VIN;";
                    else
                        strSQL += " ORDER BY VIN,RecordID;";
                }
                else
                {
                    //Sta: SELECT AutoportExportShippedVehiclesImportID,BatchID,VIN,RecordStatus
                    strSQL = @"SELECT AutoportExportShippedVehiclesImportID AS RecordID,
                        0 AS ImportedInd,
                        ImportedBy AS Inspector,
                        BatchID,VIN,RecordStatus,
                        CreatedBy
                        FROM AutoportExportShippedVehiclesImport
                        WHERE BatchID = " + intBatchID.ToString() +
                        "ORDER BY AutoportExportShippedVehiclesImportID";
                }
                
                ds = DataOps.GetDataset_with_SQL(strSQL);

                //Update dtImports  with new data
                dtImports = ds.Tables[0];
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillGrid", ex.Message);
            }
        }

        private void ImportVehicles_PHY()
        {
            try
            {
                DialogResult dlResult;
               
                DataSet ds;
              
                frmSetSelection frm;
                string strMessage;
                string strSQL;

                //Ck that there is data to import in the DAIYMS_Output_ExpPhyClone table
                strSQL = @"SELECT COUNT(DAIYMS_Output_ExpPhyCloneId) AS totrec 
                    FROM DAIYMS_Output_ExpPhyClone 
                    WHERE BatchID IS NULL";
                //#DATAHUB
                if (Globalitems.runmode == "PROD")
                    ds = DataOps.GetDataset_with_SQL(strSQL,"DATAHUB");
                else
                    ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 0 ||
                    ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ImportFile",
                        "No data returned when checking for physical data to import");
                    return;
                }

                if (ds.Tables[0].Rows[0]["totrec"].ToString() == "0")
                {
                    MessageBox.Show("There are no vehicles to import for the Physical. ",
                        "NO VEHICLES TO IMPORT", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                //Note: The program is left open all day; therefore cannot assume user logged
                //  in, is the user who clicked the Import File button
                //Open frmSetSelection in modal form, to get strImporter
                strMessage = "Please select the User importing the file";
                cboUsers.SelectedIndex = 0;
                frm = new frmSetSelection("Importer", cboUsers, strMessage, true);
                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                {
                    strAction = "Import";
                    progbar.Visible = true;
                    progbar.Value = 1;

                    txtStatus.Visible = true;
                    txtStatus.Text = "Starting Import processs";

                    //Retrieve the data with backgroundworker to allow progress bar increments
                    bckLoadData.RunWorkerAsync();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ImportVehicles_PHY", ex.Message);
            }
        }

        private void ImportVehicles_SHIP()
        {
            try
            {
                DialogResult dlResult;

                DataSet ds;

                frmSetSelection frm;
                string strMessage;
                string strSQL;

                //Ck that there is data to import in the DAIYMS_Output_ExpPhyClone table
                //Sta: SELECT AutoportExportShippedVehiclesImportID,BatchID,VIN,RecordStatus
                strSQL = @"SELECT COUNT(DAIYMS_Output_ExpInventoryId) AS totrec 
                    FROM [DAIYMS_Output_ExpInventory] 
                    WHERE BatchID IS NULL";
                //#DATAHUB
                if (Globalitems.runmode == "PROD")
                    ds = DataOps.GetDataset_with_SQL(strSQL,"DATAHUB");
                else
                    ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 0 ||
                    ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ImportFile",
                        "No data returned when checking for shipped data to import");
                    return;
                }

                if (ds.Tables[0].Rows[0]["totrec"].ToString() == "0")
                {
                    MessageBox.Show("There are no vehicles to import for the Shipped Import. ",
                        "NO VEHICLES TO IMPORT", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                //Note: The program is left open all day; therefore cannot assume user logged
                //  in, is the user who clicked the Import File button
                //Open frmSetSelection in modal form, to get strImporter
                strMessage = "Please select the User importing the file";
                cboUsers.SelectedIndex = 0;
                frm = new frmSetSelection("Importer", cboUsers, strMessage, true);
                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                {

                    strAction = "Shipped";
                    progbar.Visible = true;
                    progbar.Value = 1;

                    txtStatus.Visible = true;
                    txtStatus.Text = "Starting Import processs";

                    //Retrieve the data with backgroundworker to allow progress bar increments
                    bckLoadData.RunWorkerAsync();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ImportVehicles_SHIP", ex.Message);
            }
        }

        private void FinishImport()
        {
            DataRow drow;
            DataSet ds = null;
            
            int intBatchID_created;
            SProcParameter objParam;
            List<SProcParameter> Paramobjects = new List<SProcParameter>();
            string strImporter;
            string strSProc;
            string strResult;

            try
            {
                strStatus = "Getting scanned veh's";
                bckLoadData.ReportProgress(10);
                strImporter = Globalitems.strSelection;

                switch (strImportType)
                {
                    case "RCVD":
                        strSProc = "DAIYMSP_GetExportReceiptData";
                        objParam = new SProcParameter();
                        objParam.Paramname = "@userCode";
                        objParam.Paramvalue = strImporter;
                        Paramobjects.Add(objParam);
                        //#DATAHUB
                        if (Globalitems.runmode == "PROD")
                            ds = DataOps.GetDataset_with_SProc(strSProc, Paramobjects,"DATAHUB");
                        else
                            ds = DataOps.GetDataset_with_SProc(strSProc, Paramobjects);
                        break;
                    case "CLONE":
                        strSProc = "DAIYMSP_GetExportPhyCloneData";
                        //#DATAHUB
                        if (Globalitems.runmode == "PROD")
                            ds = DataOps.GetDataset_with_SProc(strSProc,null,"DATAHUB");
                        else
                            ds = DataOps.GetDataset_with_SProc(strSProc, null);
                        break;
                    case "SHIP":
                        strSProc = "DAIYMSP_GetExportInventoryData";
                        //#DATAHUB
                        if (Globalitems.runmode == "PROD")
                            ds = DataOps.GetDataset_with_SProc(strSProc,null,"DATAHUB");
                        else
                            ds = DataOps.GetDataset_with_SProc(strSProc, null);
                        break;
                }

                strStatus = "Getting next Batch ID";
                bckLoadData.ReportProgress(20);

                //Save in global var, dtScannedVehicles
                dtScannedVehicles = ds.Tables[0];

                //Get BatchID & increment using SProc. If SHIP import type, let SProc know
                //  use different BatchID from SettingTable
                strSProc = "spGetBatchIDAndIncrement";
                if (strImportType == "SHIP")
                {
                    objParam = new SProcParameter();
                    objParam.Paramname = "@ImportType";
                    objParam.Paramvalue = "SHIP";
                    Paramobjects.Add(objParam);
                    ds = DataOps.GetDataset_with_SProc(strSProc,Paramobjects);
                }
                else
                {
                    ds = DataOps.GetDataset_with_SProc(strSProc);
                }
               
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ImportFile",
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
                    Globalitems.HandleException(CURRENTMODULE, "ImportFile",
                        strResult);
                    return;
                }

                strStatus = "Review scanned veh's";
                //bckLoadData.ReportProgress(30);

                intBatchID_created = (int)drow["batchID"];

                //Store current BatchID, Action (Import,Process)
                intBatchID = intBatchID_created;
                strAction = "Import";

                CreateTmpTableForImport(strImporter);
                GetGridData();

                bckLoadData.ReportProgress(100);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FinishImport", ex.Message);
            }
        }

        private void ImportVehicles_VEH()
        {
            try
            {
                DialogResult dlResult;
                DataSet ds;
                frmSetSelection frm;
               
                string strImporter;
                string strMessage;
                string strReceiver;
                string strSQL;

                //If strImportType = VEH, Ck that User is selected
                strReceiver = (cboUsers.SelectedItem as ComboboxItem).cboValue;
                if (strReceiver == "select")
                {
                    MessageBox.Show("Please select the User who received the vehicles " +
                        "to import", "NO USER SELECTED", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                //Ck that there is data to import in the DAIYMS_Output_ExpRec &
                //  DAIYMS_Ref_Inspector tables
                strSQL = @"SELECT COUNT(DAIYMS_Output_ExpRecId) AS totrec
                FROM DAIYMS_Output_ExpRec rec
                INNER JOIN DAIYMS_Ref_Inspector ins
                    ON rec.DAIYMS_Ref_InspectorId = ins.DAIYMS_Ref_InspectorId
                WHERE   rec.BatchID IS NULL AND ins.InspectorCode = '";
                strSQL += strReceiver + "';";

                //#DATAHUB
                if (Globalitems.runmode == "PROD")
                    ds = DataOps.GetDataset_with_SQL(strSQL,"DATAHUB");
                else
                    ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 0 ||
                    ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ImportFile",
                        "No data returned when checking for data to import");
                    return;
                }

                if (ds.Tables[0].Rows[0]["totrec"].ToString() == "0")
                {
                    MessageBox.Show("There are no vehicles to import for the User ",
                        "NO VEHICLES TO IMPORT", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                strImporter = strReceiver;
                //Note: The program is left open all day; therefore cannot assume user logged
                //  in, is the user who clicked the Import File button
                //Open frmSetSelection in modal form, to get strImporter
                strMessage = "Please select the User importing the file";
                frm = new frmSetSelection("Importer", cboUsers, strMessage, true);
                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                {
                    strAction = "Import";
                    progbar.Visible = true;
                    progbar.Value = 1;

                    txtStatus.Visible = true;
                    txtStatus.Text = "Starting Import processs";

                    //Retrieve the data with backgroundworker to allow progress bar increments
                   bckLoadData.RunWorkerAsync();                   
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "Import_VEH", ex.Message);
            }
        }

        private void FillGrid()
        {
            try
            {
                bool blnHasData = false;
                DataView dv;
                int intAvailToPrint = 0;
                int intNeedSizeClass = 0;
                string strFilter;
                string strStatus = "";

                //Set labels, btn enabling, & dg Datasource based on dtVehicles
                dgResults.DataSource = dtImports;

                if (dtImports.Rows.Count > 0) blnHasData = true;

                btnProcess.Enabled = false;
                btnResubVoyage.Enabled = false;

                //Enable/Disable btns based on blnHasData
                btnExport.Enabled = blnHasData;
                btnSizeClass.Enabled = blnHasData;
                btnRunStatus.Enabled = blnHasData;

                //Set label text based on blnHasData
                if (blnHasData)
                {
                    lblBatchRecords.Text = "Records: " + dtImports.Rows.Count.ToString();
                    lblBatchRecords.Visible = true;

                    lblBatchID.Text = "BatchID: " + intBatchID.ToString();
                    lblBatchID.Visible = true;

                    lblBatchDetails.Text = "Received By: " + dtImports.Rows[0]["Inspector"].ToString() +
                        "  Processed by: " + dtImports.Rows[0]["CreatedBy"].ToString();
                    lblBatchDetails.Visible = true;
                    lblStatus.Visible = true;

                    //Enable btnResVoyage if status 'NEXT VOYAGE NOT FOUND' exists
                    strFilter = "RecordStatus = 'NEXT VOYAGE NOT FOUND'";
                    dv = new DataView(dtImports, strFilter, "VIN", DataViewRowState.CurrentRows);
                    if (dv.Count > 0) btnResubVoyage.Enabled = true;

                    //Use different status text depending on Action just performed
                    if (strAction == "Import")
                    {
                        strStatus = "File imported.";

                        if (strImportType == "RCVD")
                        {
                            //Find recs in dtImports w/SIZE CLASS NEEDED
                            strFilter = "RecordStatus = 'SIZE CLASS NEEDED'";
                            dv = new DataView(dtImports, strFilter, "VIN", DataViewRowState.CurrentRows);
                            intNeedSizeClass = dv.Count;

                            if (intNeedSizeClass > 0) strStatus += " SIZE CLASS NEEDED: " +
                                intNeedSizeClass.ToString() + ".";
                        }

                        lblStatus.Text = strStatus;
                    }
                    else
                    {
                        //Action is Process or Load
                        if (strImportType == "RCVD")
                        {
                            //
                            strFilter = "RecordStatus IN ('Imported','SIZE CLASS NEEDED (Imported)')";
                            dv = new DataView(dtImports, strFilter, "VIN", DataViewRowState.CurrentRows);
                            intAvailToPrint = dv.Count;
                            strStatus = "Vehicles available to print: " +
                                intAvailToPrint.ToString() + ".";

                            if (intNeedSizeClass > 0) strStatus += " SIZE CLASS NEEDED: " +
                                    intNeedSizeClass.ToString() + ".";
                            lblStatus.Text = strStatus;


                        }
                        else
                            SetBatchStatusWithDistinctVINs();
                    }

                    CheckVehInfo();

                    //Enable btnProcess, if at least one rec w/Status 'Import Pending' or
                    //  'SIZE CLASS NEEDED' and ImportedInd=0
                    strFilter = "RecordStatus IN ('Import Pending','NEXT VOYAGE NOT FOUND') AND " +
                        "ISNULL(ImportedInd,0) = 0";
                    dv = new DataView(dtImports, strFilter, "VIN", DataViewRowState.CurrentRows);
                    if (dv.Count > 0) btnProcess.Enabled = true;

                    strFilter = "RecordStatus LIKE 'NEXT%' OR RecordStatus LIKE 'SIZE%'";
                    dv = new DataView(dtImports, strFilter, "VIN", DataViewRowState.CurrentRows);
                    if (dv.Count > 0) HighlightRows();


                }
                else
                {
                    //No data for grid
                    lblBatchRecords.Visible = false;
                    lblBatchID.Visible = false;
                    lblBatchDetails.Visible = false;
                    lblStatus.Visible = false;
                    btnLabels.Enabled = false;
                }


                progbar.Visible = false;
                txtStatus.Visible = false;

                dgResults.DataSource = dtImports;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillGrid", ex.Message);
            }   
        }

        private void HighlightRows()
        {
            //Set color to Red if status is NEXT VOYAGE NOT FOUND
            foreach (DataGridViewRow dgrow in dgResults.Rows)
            {
                if (dgrow.Cells["Status"].Value.ToString().Contains("NEXT"))
                {
                    dgrow.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    dgrow.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }

                if (dgrow.Cells["Status"].Value.ToString().Contains("SIZE"))
                {
                    dgrow.DefaultCellStyle.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                }
            }
        }

        private void InvReport()
        {Formops.InventoryComparisonReport(); }        

        private void ProcessFile()
        {
            try
            {
                DataRow drow;
                DataSet ds;
                SProcParameter objParam;
                List<SProcParameter> Paramobjects = new List<SProcParameter>();
                string strSQL;
                string strSProc = "spImportAutoportExportVehicles_phy";
                string strReceiver;
                string strResult;

                //Ck for BatchID
                if (dgResults.RowCount == 0)
                {
                    MessageBox.Show("There is no Batch selected & displayed.",
                       "NO BATCH IDENTIFIED", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return;
                }

                //If strImportType = RCVD, Ck that User is selected
                strReceiver = (cboUsers.SelectedItem as ComboboxItem).cboValue;
                if (strImportType=="RCVD" && strReceiver == "select")
                {
                    MessageBox.Show("Please select the User who is importing the vehicles. ", 
                        "NO USER SELECTED", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                //Ck for recs based on Import type
                if (strImportType == "RCVD")
                {
                    //Ck if there are any recs in the batch to process
                    strSQL = @"SELECT COUNT(AutoportExportVehiclesImportID) AS totrec
                    FROM AutoportExportVehiclesImport
                    WHERE BatchID = " + intBatchID + " AND ImportedInd=0";

                    strSProc = "spImportAutoportExportVehicles_veh";
                }
                else
                {
                    //Ck if any recs in the batch need processing (RecordStatus = 'Import Pending')
                    strSQL = @"SELECT COUNT(AutoportExportVehiclesImportID) AS totrec
                        FROM AutoportExportVehiclesImport
                        WHERE BatchID = " + intBatchID + " AND RecordStatus = 'Import Pending'";

                    strSProc = "spImportAutoportExportVehicles_phy";
                }

                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ProcessFile",
                        "No data returned from checking Import table");
                    return;
                }

                if (Convert.ToInt16(ds.Tables[0].Rows[0]["totrec"]) == 0)
                {
                    MessageBox.Show("There are no vehicles to process for this Batch",
                        "BATCH ALREADY PROCESSED", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                strAction = "Process";

                objParam = new SProcParameter();
                objParam.Paramname = "@BatchID";
                objParam.Paramvalue = intBatchID;
                Paramobjects.Add(objParam);

                objParam = new SProcParameter();
                objParam.Paramname = "@UserCode";
                if (strImportType == "RCVD")
                    objParam.Paramvalue = strReceiver;
                else
                    objParam.Paramvalue = Globalitems.strUserName;
                Paramobjects.Add(objParam);

                ds = DataOps.GetDataset_with_SProc(strSProc, Paramobjects);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ProcessFile",
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
                    Globalitems.HandleException(CURRENTMODULE, "ProcessFile",
                        strResult);
                    return;
                }

                GetGridData();
                FillGrid();

                if (!Globalitems.blnCannotPrintLabels && strImportType == "RCVD")
                    btnLabels.Enabled = HasLabelsToPrint();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ProcessFile", ex.Message);
            }
        }

      
        private void ResubmitVoyageNotFound()
        {
            try
            {
                DataRow drow;
                DataSet ds;
                DataView dv;
                SProcParameter objParam;
                List<SProcParameter> Paramobjects = new List<SProcParameter>();
                string strFilter;
                string strSProc = "spImportAutoportExportVehicles_veh";
                string strSQL;
                string strWhere;
                string strResult;

                //Get the rows from dtImports with a status of 'NEXT VOYAGE NOT FOUND'
                strFilter = "RecordStatus = 'NEXT VOYAGE NOT FOUND'";
                dv = new DataView(dtImports, strFilter, "VIN", DataViewRowState.CurrentRows);

                if (dv.Count == 0)
                {
                    MessageBox.Show("There are no vehicles with a status of NEXT VOYAGE NOT FOUND",
                        "NO ROWS TO RESUBMIT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strWhere = "WHERE BatchID = " + intBatchID.ToString() + " AND VIN IN (";

                //Set the BatchID to -1 in the AutoportExportVehiclesImport table for the 
                //  NEXT VOYAGE NOT FOUND rows
                foreach (DataRowView dvrow in dv)
                    strWhere += "'" + dvrow["VIN"] + "',";

                //Replace last "'" in strWhere with ")"
                strWhere = strWhere.Substring(0, strWhere.Length - 1) + ")";

                strSQL = "UPDATE AutoportExportVehiclesImport SET BatchID = -1 " +
                    strWhere;
                DataOps.PerformDBOperation(strSQL);

                //Invoke SProc
                strAction = "Process";

                objParam = new SProcParameter();
                objParam.Paramname = "@BatchID";
                objParam.Paramvalue = -1;
                Paramobjects.Add(objParam);

                objParam = new SProcParameter();
                objParam.Paramname = "@UserCode";
                objParam.Paramvalue = Globalitems.strUserName;
                Paramobjects.Add(objParam);

                ds = DataOps.GetDataset_with_SProc(strSProc, Paramobjects);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ProcessFile",
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
                    Globalitems.HandleException(CURRENTMODULE, "ProcessFile",
                        strResult);
                    return;
                }

                //Change BatchID back to original
                strSQL = "UPDATE AutoportExportVehiclesImport SET BatchID = " + 
                    intBatchID.ToString() + " WHERE BatchID = -1";
                DataOps.PerformDBOperation(strSQL);

                GetGridData();
                FillGrid();

            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ResubmitVoyageNotFound", ex.Message);
            }
        }

        private void SetBatchStatusWithDistinctVINs()
        {
            try
            {
                DataTable dtDistinct;
                DataView dv;
                int intDistinctVINs;
                string strStatus;

                //Get DISTINCT VINS 
                dv = new DataView(dtImports);
                dtDistinct = dv.ToTable(true, "VIN");
                intDistinctVINs = dtDistinct.Rows.Count;
                strStatus = "Vehicles in batch: " + intDistinctVINs.ToString() +
                    " Dup. scans: " + (dv.Count - intDistinctVINs).ToString();

                lblStatus.Text = strStatus;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetBatchStatus", ex.Message);
            }
        }


        private bool HasLabelsToPrint()
        {
            //Return true if at least one row in the dg has a status of Imported or 
            //  SIZE CLASS NEEDED
            try
            {
                string strStatus;

                foreach (DataGridViewRow dgrow in dgResults.Rows)
                {
                    strStatus = dgrow.Cells["Status"].Value.ToString();
                    if (strStatus == "Imported" || strStatus == "SIZE CLASS NEEDED") return true;
                }

                return false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "HasLabelsToPrint", ex.Message);
                return false;
            }
        }

        private void LoadBatch()
        {
            try
            {
                DialogResult dlResult;
                frmSelectBatch frm = new frmSelectBatch();

                frm.strImportType = strImportType;

                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                {
                    strAction = "Load";
                    intBatchID = Convert.ToInt32(Globalitems.strSelection);
                    GetGridData();
                    FillGrid();
                    if (!Globalitems.blnCannotPrintLabels && HasLabelsToPrint()) btnLabels.Enabled = true;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "LoadBatch", ex.Message);
            }
        }

       

        private void OpenCSVFile()
        {
            try
            {
                try
                {
                    DataSet ds;
                    DataTable dt = null;
                    DataView dv;
                    List<ControlInfo> objctrlinfo = new List<ControlInfo>();
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

                    //If SHIP import, create new dt
                    if (strImportType == "SHIP")
                    {
                        strSQL = @"SELECT 
                        imp.VIN,
                        veh.VehicleYear,
                        veh.Make,
                        veh.Model,
                        veh.Color,
                        veh.BayLocation,
                        voy.VoyageDate,
                        voy.VoyageNumber,
                        ves.VesselName,
                        ISNULL(veh.VehicleStatus,'VIN NOT FOUND') AS VehicleStatus
                        FROM
                        AutoportExportShippedVehiclesImport imp 
                        LEFT OUTER JOIN AutoportExportVehicles veh on veh.VIN=imp.VIN
                        LEFT OUTER JOIN AEVoyage voy on voy.AEVoyageID=veh.VoyageID
                        LEFT OUTER JOIN AEVessel ves on ves.AEVesselID=voy.AEVesselID
                        WHERE imp.BatchID = " + intBatchID + " ";

                        if (ckExcludeShippedVehs.Checked) strSQL +=
                                "AND veh.VehicleStatus <> 'Shipped'";

                        strSQL += "ORDER BY imp.VIN, voy.VoyageDate";

                        ds = DataOps.GetDataset_with_SQL(strSQL);

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            MessageBox.Show("No data for the Export",
                                "NO EXPORT DATA", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        dt = ds.Tables[0];
                    }

                    //2. Create a copy of the datatable used for the datagridview datasource
                    //  if not SHIP import
                    if (strImportType != "SHIP") dt = dtImports.Copy();

                    //3. If the gridview is sorted, use a dv to sort the table copy the same way
                    if (dgResults.SortedColumn != null)
                    {
                        strSort = dgResults.SortedColumn.DataPropertyName;
                        if (dgResults.SortOrder == SortOrder.Descending) strSort += " DESC";
                        dv = new DataView(dt, "", strSort, DataViewRowState.CurrentRows);
                        dt = dv.ToTable();
                    }

                    //4. Create a list, lsCSVcols with ControlInfo objects in the order to 
                    //  appear in the csv file 
                    //Get ctrlinfo object from lsControls for RecordFieldName & add to lsCSVcols.
                    //Get from dgResults if not SHIP import
                    if (strImportType != "SHIP")
                    {
                        foreach (DataGridViewColumn dgCol in dgResults.Columns)
                        {
                            if (dgCol.Visible)
                            {
                                objctrlinfo = lsControls.Where(obj => obj.RecordFieldName == dgCol.DataPropertyName).ToList();
                                lsCSVcols.Add(objctrlinfo[0]);
                            }
                        }
                    }
                    else
                    {
                        foreach (DataColumn dtCol in dt.Columns)
                        {
                            objctrlinfo = lsControls.Where(obj => obj.RecordFieldName == dtCol.ColumnName).ToList();
                                lsCSVcols.Add(objctrlinfo[0]);                            
                        }
                    } 
                    
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

        private void OpenPrintLabelsForm()
        {
            try
            {
                frmLabels frm; 
                PrintInfo objPrintInfo = new PrintInfo();
                objPrintInfo.BatchID = intBatchID;

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


        private void SetRunStatus()
        {
            ComboboxItem cboitem;
            DialogResult dlResult;
            System.Windows.Forms.ComboBox cboNoRunner = new System.Windows.Forms.ComboBox();
            frmSetSelection frm;
            int intRunnerind;
            string strMessage;
            string strNoRunner;
            string strSQL = "";

            //No Import & Vehicle tables are a bit confusing with col. names:
            //AutoportExportVehiclesImport uses RunnerInd: 0-No, 1-Yes to indicate if veh. runs
            //AutoportExportVechicles uses NoStartInd: 0-No (car runs), 1-Yes (car does not run) 

            //Fill cboRunStatus with items
            cboitem = new ComboboxItem();
            cboitem.cboText = "<select>";
            cboitem.cboValue = "select";
            cboNoRunner.Items.Add(cboitem);

            cboitem = new ComboboxItem();
            cboitem.cboText = "Yes";
            cboitem.cboValue = "Yes";
            cboNoRunner.Items.Add(cboitem);

            cboitem = new ComboboxItem();
            cboitem.cboText = "No";
            cboitem.cboValue = "No";
            cboNoRunner.Items.Add(cboitem);

            cboNoRunner.DisplayMember = "cboText";
            cboNoRunner.ValueMember = "cboValue";

            strNoRunner = dgResults.SelectedRows[0].Cells["NonRunner"].Value.ToString();

            //Set cboNoRunner to current status
            foreach (ComboboxItem item in cboNoRunner.Items)
                if (item.cboValue.ToUpper() == strNoRunner) cboNoRunner.SelectedItem = item;

            strMessage = "Is this vehicle a non runner?";

            frm = new frmSetSelection("Non Runner Status", cboNoRunner, strMessage, true);
            dlResult = frm.ShowDialog();

            if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
            {
                //Make sure NoRunner status has changed
                if (strNoRunner != Globalitems.strSelection)
                {
                    //Update AutoportExportVehiclesImport
                    if (Globalitems.strSelection == "Yes")
                        intRunnerind = 0;
                    else
                        intRunnerind = 1;

                    //Use appropriate SQL depending on Action
                    if (strAction == "Import")
                        strSQL = @"UPDATE AutoportExportVehiclesImport SET RunnerInd = " + 
                        intRunnerind.ToString() + @"WHERE AutoportExportVehiclesImportID = " +
                            dgResults.SelectedRows[0].Cells["RecordID"].Value.ToString();
                    else
                        strSQL =@"UPDATE imp
                        SET imp.RunnerInd = " + intRunnerind.ToString() +
                        @"FROM AutoportExportVehicles veh
                        INNER JOIN AutoportExportVehiclesImport imp 
	                        on imp.BatchID=" + intBatchID + @"and imp.VIN=veh.VIN
                        WHERE AutoportExportVehiclesID = " +
                           dgResults.SelectedRows[0].Cells["RecordID"].Value.ToString();

                    DataOps.PerformDBOperation(strSQL);

                    //Update AutoportExportVehicles if Action = Process
                    if (strAction == "Process")
                    {
                        strSQL = @"UPDATE AutoportExportVehicles 
                            SET NoStartInd = ";

                        if (intRunnerind == 0)
                            strSQL += "1,";
                        else
                            strSQL += "0,";

                        strSQL += @"UpdatedBy = CreatedBy,
                            UpdatedDate = CURRENT_TIMESTAMP
                            WHERE AutoportExportVehiclesID = " + intBatchID;

                        DataOps.PerformDBOperation(strSQL);
                    }

                    GetGridData();
                    dgResults.DataSource = dtImports;
                }
            }
        }

        private ComboBox SetDestinationCombobox()
        {
            try
            {
                ComboboxItem cboitem;
                ComboBox cboDest = new ComboBox();
                DataView dv;
                string strFilter;

                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboDest.Items.Add(cboitem);

                //Retrieve values from dtCode for remaining items of cbo
                strFilter = "CodeType='ExportDischargePort' AND Value2 <> '" +
                      dgResults.SelectedRows[0].Cells["Destination"].Value.ToString() + "' " +
                      "AND RecordStatus='Active'";

                //Use Dataview to filter dtCode for RecordStatus
                dv = new DataView(Globalitems.dtCode, strFilter, "Value2", DataViewRowState.CurrentRows);

                // Fill cbo or list with values from dtCode
                foreach (DataRowView dvrow in dv)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dvrow["CodeDescription"].ToString();
                    cboitem.cboValue = dvrow["Value2"].ToString();
                    cboDest.Items.Add(cboitem);
                }

                cboDest.DisplayMember = "cboText";
                cboDest.ValueMember = "cboValue";
                cboDest.SelectedIndex = 0;

                return cboDest;
            }
         
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetCustomer", ex.Message);
                return null;
            }
        }   

        private void SetDestination()
        {
            try
            {
                DialogResult dlResult;
                DataRow drow;
                DataSet ds;
                System.Windows.Forms.ComboBox cboDest;
                frmSetSelection frm;
                int intNegBatchID = intBatchID * -1;
                SProcParameter objParam;
                List<SProcParameter> Paramobjects = new List<SProcParameter>();
                string strSProc = "spImportAutoportExportVehicles_veh";
                string strRecordID;
                string strMessage;
                string strResult;
                string strSQL;

                // Use frmAreYouSure to confirm changing Customer
                strMessage = "Are you sure you want to update the Destination?";
                frmAreYouSure frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    cboDest = SetDestinationCombobox();

                    strMessage = "Please select the Destination.";
                    frm = new frmSetSelection("Destination", cboDest, strMessage, true);
                    dlResult = frm.ShowDialog();

                    if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                    {
                        //Update record in import table, change Customer, and set BatchID to intNegBatchID
                        strRecordID = dgResults.SelectedRows[0].Cells["RecordID"].Value.ToString();
                        strSQL = "Update AutoportExportVehiclesImport SET DestinationName='" + Globalitems.strSelection + "',";
                        strSQL += "BatchID = " + intNegBatchID;
                        strSQL += " WHERE AutoportExportVehiclesImportID = " + strRecordID;
                        DataOps.PerformDBOperation(strSQL);
                        if (Globalitems.blnException) return;


                        objParam = new SProcParameter();
                        objParam.Paramname = "@BatchID";
                        objParam.Paramvalue = intNegBatchID;
                        Paramobjects.Add(objParam);

                        objParam = new SProcParameter();
                        objParam.Paramname = "@UserCode";
                        objParam.Paramvalue = Globalitems.strUserName;
                        Paramobjects.Add(objParam);

                        ds = DataOps.GetDataset_with_SProc(strSProc, Paramobjects);

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "ProcessFile",
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
                            Globalitems.HandleException(CURRENTMODULE, "ProcessFile",
                                strResult);
                            return;
                        }

                        //Change BatchID back to positive
                        strSQL = "Update AutoportExportVehiclesImport SET BatchID = " + intBatchID;
                        strSQL += " WHERE AutoportExportVehiclesImportID = " + strRecordID;
                        DataOps.PerformDBOperation(strSQL);
                        if (Globalitems.blnException) return;

                        GetGridData();
                        FillGrid();

                        if (!Globalitems.blnCannotPrintLabels) btnLabels.Enabled = HasLabelsToPrint();
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetCustomer", ex.Message);
            }
        }

        private ComboBox SetCustomerComboBox()
        {
            try
            {
                ComboBox cboCust = new ComboBox();
                ComboboxItem cboitem;
                DataSet ds;
                string strSQL;

                //Fill combobox with active customers
                strSQL = "SELECT HandheldScannerCustomerCode, " +
                 "CASE WHEN LEN(RTRIM(ISNULL(ShortName,''))) > 0 THEN RTRIM(ShortName) " +
                 "else RTRIM(CustomerName) END AS CustName " +
                 "FROM Customer " +
                 "WHERE RecordStatus='Active' AND  HandheldScannerCustomerCode <> '" +
                 dgResults.SelectedRows[0].Cells["CustomerName"].Value.ToString() + "' " +
                 "ORDER BY CustName";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "SetCustomer",
                        "No rows returned from Customer table");
                    return null;
                }

                // Add All to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboCust.Items.Add(cboitem);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dr["CustName"].ToString();
                    cboitem.cboValue = dr["HandheldScannerCustomerCode"].ToString();
                    cboCust.Items.Add(cboitem);
                }

                cboCust.DisplayMember = "cboText";
                cboCust.ValueMember = "cboValue";
                cboCust.SelectedIndex = 0;

                return cboCust;
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetCustomer", ex.Message);
                return null;
            }

        }

        private void SetCustomer()
        {
            try
            {
                DialogResult dlResult;
                DataRow drow;
                DataSet ds;
                ComboBox cboCust;
                frmSetSelection frm;
                int intNegBatchID = intBatchID * -1;
                SProcParameter objParam;
                List<SProcParameter> Paramobjects = new List<SProcParameter>();
                string strSProc = "spImportAutoportExportVehicles_veh";
                string strRecordID;
                string strMessage;
                string strResult;
                string strSQL;

                // Use frmAreYouSure to confirm changing Customer
                strMessage = "Are you sure you want to update the Customer?";
                frmAreYouSure frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    cboCust = SetCustomerComboBox();
                    strMessage = "Please select the Customer.";
                    frm = new frmSetSelection("Customer", cboCust, strMessage, true);
                    dlResult = frm.ShowDialog();

                    if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                    {
                        //Update record in import table, change Customer, and set BatchID to intNegBatchID
                        strRecordID = dgResults.SelectedRows[0].Cells["RecordID"].Value.ToString();
                        strSQL = "Update AutoportExportVehiclesImport SET CustomerName='" + Globalitems.strSelection + "',";
                        strSQL += "BatchID = " + intNegBatchID;
                        strSQL += " WHERE AutoportExportVehiclesImportID = " + strRecordID;
                        DataOps.PerformDBOperation(strSQL);
                        if (Globalitems.blnException) return;


                        objParam = new SProcParameter();
                        objParam.Paramname = "@BatchID";
                        objParam.Paramvalue = intNegBatchID;
                        Paramobjects.Add(objParam);

                        objParam = new SProcParameter();
                        objParam.Paramname = "@UserCode";
                        objParam.Paramvalue = Globalitems.strUserName;
                        Paramobjects.Add(objParam);

                        ds = DataOps.GetDataset_with_SProc(strSProc, Paramobjects);

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "ProcessFile",
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
                            Globalitems.HandleException(CURRENTMODULE, "ProcessFile",
                                strResult);
                            return;
                        }

                        //Change BatchID back to positive
                        strSQL = "Update AutoportExportVehiclesImport SET BatchID = " + intBatchID;
                        strSQL += " WHERE AutoportExportVehiclesImportID = " + strRecordID;
                        DataOps.PerformDBOperation(strSQL);
                        if (Globalitems.blnException) return;

                        GetGridData();
                        FillGrid();

                        if (!Globalitems.blnCannotPrintLabels) btnLabels.Enabled = HasLabelsToPrint();
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetCustomer", ex.Message);
            }
        }


        private void SetSizeClass()
        {
            try
            {
                ComboboxItem cboitem;
                DialogResult dlResult;
                System.Windows.Forms.ComboBox cboSize = new System.Windows.Forms.ComboBox();
                frmSetSelection frm;
                string strFilter;
                string strHgt;
                string strImportedInd;
                string strMake;
                string strMessage;
                string strModel;
                string strSizeClass;
                string strSQL;
                string strVIN;

                strFilter = "CodeType='SizeClass' AND Code <> '' AND RecordStatus='Active'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboSize, true, false);

                //Change first item in cbo to <select>
                cboitem = cboSize.Items[0] as ComboboxItem;
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboSize.SelectedIndex = 0;

                strVIN = dgResults.SelectedRows[0].Cells["VIN"].Value.ToString();
                strMake = dgResults.SelectedRows[0].Cells["Make"].Value.ToString();
                strModel = dgResults.SelectedRows[0].Cells["Model"].Value.ToString();
                strHgt = dgResults.SelectedRows[0].Cells["VehicleHeight"].Value.ToString();

                strMessage = "Vehicle missing Size Class:\n" +
                        "VIN: " + strVIN + "\n" +
                        "Make: " + strMake + "\nModel: " + strModel +
                        "\nHeight: " + strHgt;

                frm = new frmSetSelection("Size Class", cboSize,strMessage,true);
                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                {
                    strSizeClass = Globalitems.strSelection;
                    strImportedInd = dgResults.SelectedRows[0].Cells["importedind"].Value.ToString();

                    //Update AutoportExportVehiclesImport
                    strSQL = @"UPDATE AutoportExportVehiclesImport SET SizeClass = '" +
                            strSizeClass + @"',";
                    //Change status to either Import Pendind or Imported, based on importedind
                    if (strImportedInd == "0")
                        strSQL += "RecordStatus = 'Import Pending' ";
                    else
                        strSQL += "RecordStatus = 'Imported' ";

                    strSQL += "WHERE AutoportExportVehiclesImportID = " +
                            dgResults.SelectedRows[0].Cells["RecordID"].Value.ToString();
                    DataOps.PerformDBOperation(strSQL);

                    //Update AutoportExportVehicles, if importedind = 1 (rec in veh table)
                    if (strImportedInd == "1")
                        Globalitems.SetSizeClass(strSizeClass, strVIN, true);
                }

                GetGridData();
                dgResults.DataSource = dtImports;
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetSizeClass", ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            switch (strImportType)
            {
                case "RCVD":
                    ImportVehicles_VEH();
                    break;
                case "CLONE":
                    ImportVehicles_PHY();
                    break;
                case "SHIP":
                    ImportVehicles_SHIP();
                    break;
            }
        }

        private void ckActive_CheckedChanged(object sender, EventArgs e)
        {FillCombos();}

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }

        private void btnSizeClass_Click(object sender, EventArgs e)
        {SetSizeClass();}

        private void dgResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {if (e.RowIndex > -1) CheckVehInfo();}

        private void btnProcess_Click(object sender, EventArgs e)
        {ProcessFile();}

        private void btnRunStatus_Click(object sender, EventArgs e)
        {SetRunStatus();}

        private void btnClear_Click(object sender, EventArgs e)
        { ClearGrid(); }

        private void btnLoad_Click(object sender, EventArgs e)
        {LoadBatch();}

        private void btnExport_Click(object sender, EventArgs e)
        {OpenCSVFile();}

        private void btnLabels_Click(object sender, EventArgs e)
        {OpenPrintLabelsForm();}

        private void btnInv_Click(object sender, EventArgs e)
        {InvReport();}

        private void bckLoadData_DoWork(object sender, DoWorkEventArgs e)
        {FinishImport();}

        private void bckLoadData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progbar.Value = e.ProgressPercentage;
            txtStatus.Text = strStatus;
        }

        private void bckLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {FillGrid();}

        private void btnNoLabels_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To print labels, a label printer " +
               "\nmust be set as the default printer" +
               "\n (E.g. Wasp or Zebra printer)", "LABEL PRINTER NOT THE DEFAULT",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmImportVehYMS_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}

        private void btnResubVoyage_Click(object sender, EventArgs e)
        {ResubmitVoyageNotFound();}

        private void btnCust_Click(object sender, EventArgs e)
        {SetCustomer();}

        private void btnDest_Click(object sender, EventArgs e)
        {SetDestination();}
    }
}
