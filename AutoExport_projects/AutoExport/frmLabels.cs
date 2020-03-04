using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Configuration;

//4/15/19 D.Maibor: remove changes to try new Label for Sallaum
namespace AutoExport
{
    public partial class frmLabels : Form
    {
     
        //CONSTANTS
        private const string CURRENTMODULE = "frmLabels";
        private const string LABELREPORT = "newlabel.rdlc";

        //Variables
        public PrintInfo objPrintInfo;

        private bool blnDisplayingLabel = false;

        public frmLabels(PrintInfo objPInfo)
        {
            var appSettings = ConfigurationManager.AppSettings;

            InitializeComponent();

            dgBatch.AutoGenerateColumns = false;
            dgPrint.AutoGenerateColumns = false;

            objPrintInfo = objPInfo;

            FillGrids();            
        }

        private void frmLabels_Activated(object sender, EventArgs e)
        {
            //Leave form settings unchanged if returning from Displaying Labels
            if (blnDisplayingLabel)
            {
                blnDisplayingLabel = false;
                return;
            }

            //Set rbSelected if blnSetSelected
            if (objPrintInfo.SelectedIDs.Count > 0)
            {
                rbSelected.Text = objPrintInfo.Message;                rbSelected.Enabled = true;
                rbSelected.Enabled = true;
                if (!rbVIN.Checked) rbSelected.Checked = true;
            }
            else
            {
                rbSelected.Text = "No rows selected from another form";
                rbSelected.Enabled = false;
            }

            //Set rbBatch
            if (objPrintInfo.BatchID != 0)
            {
                rbBatch.Checked = true;
                foreach (DataGridViewRow dgRow in dgBatch.Rows)
                {
                    if (dgRow.Cells["BatchID"].Value.ToString() == 
                        objPrintInfo.BatchID.ToString())
                    {
                        dgRow.Selected = true;
                        //Make sure selected row displays to User
                        dgBatch.FirstDisplayedScrollingRowIndex = dgBatch.SelectedRows[0].Index;
                        break;
                    }
                }
            }

            //Only enable rbAll if there are unprinted labels
            if (LabelsNotYetPrinted())
            {
                rbAll.Text = "Print All Unprinted Labels";
                rbAll.Enabled = true;
            }
            else
            {
                rbAll.Checked = false;
                rbAll.Text = "NO Unprinted Labels";
                rbAll.Enabled = false;
            }
        }

        private bool LabelsNotYetPrinted()
        {
            DataSet ds;
            int intRecs;
            string strSQL;
            
            try
            {
                strSQL = "SELECT COUNT(AutoportExportVehiclesID) AS totrec " +
                "FROM AutoportExportVehicles " +
                "WHERE BarCodeLabelPrintedInd = 0 ";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "LabelsNotYetPrinted", "No data " +
                        "returned from SQL.");
                    return false;
                }

                intRecs = (int) ds.Tables[0].Rows[0]["totrec"];

                if (intRecs == 0) return false;

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "LabelsNotYetPrinted", ex.Message);
                return false;
            }
        }

        private void FillGrids()
        {
            DataSet ds;
            string strSQL;

            //Get Batch info for dgBatch, NOTE qry below gets accurate count when 
            // AutoportExportVehiclesImport table has dup scans for the same VIN
            // due to BayLoc move. DATS shows incorrect totals.
            strSQL = @"WITH CTE AS
            (SELECT TOP(500) imp.BatchID, 
            CONVERT(varchar(10),imp.ImportedDate,101) AS ImportDate,
            Users.UserCode AS ReceivedBy,
            COUNT(imp.BatchID) AS Recs 
            FROM AutoportExportVehiclesImport imp
            LEFT OUTER JOIN Users on Users.UserCode=imp.Inspector
            WHERE ImportedInd = 1 
            GROUP BY imp. BatchID, convert(varchar(10), ImportedDate, 101),Users.UserCode
            ORDER BY BatchID DESC)
            SELECT CTE.BatchID,CTE.ImportDate,CTE.ReceivedBy,
            COUNT(DISTINCT imp.VIN) as Recs
            FROM AutoportExportVehiclesImport imp
            INNER JOIN CTE on CTE.BatchID=imp.BatchID
            GROUP BY CTE.BatchID,CTE.ImportDate,CTE.ReceivedBy
            ORDER BY CTE.BatchID DESC";
            ds = DataOps.GetDataset_with_SQL(strSQL);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                dgBatch.DataSource = ds.Tables[0];
            dgBatch.ClearSelection();

            //Get Printed info for top 500
            strSQL = @"SELECT TOP(500) CONVERT(varchar(10),
                BarCodeLabelPrintedDate,101) AS PrintDate,
                BarcodeLabelPrintedDate AS FullDate, 
                COUNT(AutoportExportVehiclesID) AS Recs 
                FROM AutoportExportVehicles 
                WHERE BarCodeLabelPrintedDate IS NOT NULL 
                GROUP BY BarCodeLabelPrintedDate 
                ORDER BY BarCodeLabelPrintedDate DESC";
            ds = DataOps.GetDataset_with_SQL(strSQL);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                dgPrint.DataSource = ds.Tables[0];
            dgPrint.ClearSelection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MissingSizeClass(List<DataGridViewRow> lsMissingRows)
        {
            //See if user want to enter the Size for each vehicle
            try
            {
                DialogResult dlResult;
                System.Windows.Forms.ComboBox cboSize = new System.Windows.Forms.ComboBox();
                frmSetSelection frm;
                string strFilter;
                string strMake;
                string strMessage;
                string strModel;
                string strSQL;
                string strVIN;
                string strYear;

                strFilter = "CodeType='SizeClass' AND Code <> '' AND RecordStatus='Active'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboSize, true, false);

                foreach (DataGridViewRow dgRow in lsMissingRows)
                {
                    strVIN = "";
                    if (dgRow.Cells["VIN"].Value != null)
                        strVIN = dgRow.Cells["VIN"].Value.ToString();

                    strYear = "";
                    if (dgRow.Cells["VehicleYear"].Value != null)
                        strYear = dgRow.Cells["VehicleYear"].Value.ToString();

                    strMake = "";
                    if (dgRow.Cells["Make"].Value != null)
                        strMake = dgRow.Cells["Make"].Value.ToString();

                    strModel = "";
                    if (dgRow.Cells["Model"].Value != null)
                        strMake = dgRow.Cells["Model"].Value.ToString();

                    strMessage = "Vehicle missing Size Class:\n" +
                        "VIN: " + strVIN + "\n" +
                        "Year: " + strYear + "\n" +
                        "Make" + strMake + "\n" +
                        "Model" + strModel;

                    frm = new frmSetSelection("Size Class", cboSize);
                    dlResult = frm.ShowDialog();

                    if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0)
                    {
                        strSQL = "UPDATE AutoportExportVehicles SET SizeClass = '" +
                            Globalitems.strSelection + "',UpdatedBy = '" +
                            Globalitems.strUserName + "',UpdateDate = CURRENT_TIMESTAMP " +
                            "WHERE AutoportExportVehiclesID = " +
                            dgRow.Cells["VehID"].Value.ToString();
                        DataOps.PerformDBOperation(strSQL);
                    }

                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "MissingSizeClass", ex.Message);
            }
        }

        private ReportDataSource GetReportDataSource()
        {
            ComboBox cboSize;
            DataSet ds;
            DataTable dtDistinctIDs;
            DataView dv;
            DialogResult dlResult;
            frmSetSelection frmSelect;
            PrintInfo objMissingSizeClass = new PrintInfo();
            int intBatchID = 0;
            string strPrintDate = "";
            string strFilter;
            string strMsg;
            string strSQL;

            try
            {
                strSQL = "SELECT DISTINCT veh.AutoportExportVehiclesID,";

                //Only show Destination if single label selected
                if (rbSingle.Checked)
                    strSQL += "veh.DestinationName AS Destination,";
                else
                    strSQL += "'' AS Destination,";

                //Basic SELECT for all labels
                strSQL += "CASE " +
                        "WHEN LEN(RTRIM(ISNULL(cus.ShortName, ''))) > 0 THEN cus.ShortName " +
                        "ELSE cus.CustomerName " +
                     "END AS Customer," +
                     "veh.Make + ' ' + veh.Model AS MakeModel," +
                     "RTRIM(ISNULL(veh.Make,'')) AS Make," +
                     "RTRIM(ISNULL(veh.Model,'')) AS Model," +
                     "RTRIM(ISNULL(veh.VehicleYear,'')) AS VehicleYear," +
                     "veh.VIN AS VIN_alpha," +
                     "RTRIM(ISNULL(veh.SizeClass,'')) AS Size," +
                     "RTRIM(ISNULL(veh.VehicleHeight,'')) AS Height," +
                    "'Size: ' + ISNULL(veh.SizeClass, '') AS SizeClass," +
                    "'*' + veh.VIN + '*' AS VIN_barcode," +
                    "'Bay: ' + ISNULL(veh.BayLocation, '') AS BayLocation," +
                    "'Rec: ' + CONVERT(varchar(10), veh.DateReceived, 101) AS DateReceived, " +
                    "veh.BarCodeLabelPrintedInd " +
                    "FROM AutoportExportVehicles veh " +
                    "LEFT JOIN Customer cus on cus.CustomerID = veh.CustomerID ";
               
                if (rbSelected.Checked)
                {
                    //Set WHERE clause to veh. IDs in lsSelectedIDs
                    strSQL += "WHERE veh.AutoportExportVehiclesID IN (";

                    foreach (int intID in objPrintInfo.SelectedIDs)
                        strSQL += intID.ToString() + ",";

                    //Replace ',' with ')' in last position of strSQL
                    strSQL = strSQL.Substring(0, strSQL.Length - 1) + ") ";
                }

                if (rbAll.Checked)
                {
                    strSQL += "WHERE veh.BarCodeLabelPrintedInd=0 ";
                }

                if (rbBatch.Checked)
                {
                    if (dgBatch.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Please select a Batch from the list", "NO BATCH SELECTED",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }

                    //Place the VehID from all selected rows into intBatchID, only one
                    foreach (DataGridViewRow dgRow in dgBatch.SelectedRows)
                    {intBatchID = Convert.ToInt32(dgRow.Cells["BatchID"].Value.ToString());}

                    //Add LEFT JOIN to AutoportExportVehiclesImport
                    strSQL += "LEFT JOIN AutoportExportVehiclesImport imp on imp.VIN=veh.VIN " +
                        "WHERE imp.BatchID = " + intBatchID + " AND imp.ImportedInd=1 ";
                }

                if (rbVIN.Checked)
                {
                    strSQL += "WHERE veh.VIN LIKE '%" + txtVIN.Text.Trim() + "%' ";
                }

                if (rbPrintDate.Checked)
                {
                    if (dgPrint.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Please select a Print Date from the list", 
                            "NO PRINT DATE SELECTED",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }

                    //Place the FullDate from all selected rows into strPrinte
                    strSQL += "WHERE BarCodeLabelPrintedDate IN (";

                    foreach (DataGridViewRow dgRow in dgPrint.SelectedRows)
                    {
                        strPrintDate += "'" + dgRow.Cells["FullDate"].Value.ToString() + "',";
                    }

                    //Replace last ',' with ')'
                    strPrintDate = strPrintDate.Substring(0, strPrintDate.Length - 1) + ")";

                    strSQL += strPrintDate;
                }

                //Add UNION ALL to Select if rbDouble is checked
                if (rbDouble.Checked)
                    strSQL += " UNION ALL " + strSQL;

                strSQL += " ORDER BY ";

                //If two sets, first order by Customer
                if (rbDouble.Checked) strSQL += "Customer, ";

                //Always order by BayLocation, VIN_alpha 
                strSQL += "BayLocation,VIN_alpha";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GetReportDataSource",
                        "No data returned");
                    return null;
                }

                //Check if any rows are missing Size
                strFilter = "Size=''";
                dv = new DataView(ds.Tables[0], strFilter, "VIN_alpha", 
                    DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    //Allow user to enter Size through frmSetSelection
                    cboSize = new ComboBox();
                    strFilter = "CodeType='SizeClass' AND Code <> '' AND RecordStatus='Active'";
                    Globalitems.FillComboboxFromCodeTable(strFilter, cboSize, false, false);

                    cboSize.DisplayMember = "cboText";
                    cboSize.ValueMember = "cboValue";

                    dtDistinctIDs = dv.ToTable(true, "AutoportExportVehiclesID", 
                        "VIN_alpha","Make","Model","Height");

                    foreach (DataRow drow in dtDistinctIDs.Rows)
                    {
                        strMsg = "Please select the Size Class for:\n" +
                            "VIN: " + drow["VIN_alpha"] + "\n" +
                            "Make: " + drow["Make"] + "  Model: " + drow["Model"] + "\n" +
                            "Height: " + drow["Height"];
                        frmSelect = new frmSetSelection("Size Class", cboSize,strMsg);
                        dlResult = frmSelect.ShowDialog();

                        if (dlResult == DialogResult.OK)
                        {
                            Globalitems.SetSizeClass(Globalitems.strSelection,
                                drow["AutoportExportVehiclesID"].ToString());

                            //Update dvrows
                            foreach (DataRowView dvrow in dv)
                            {
                                if (dvrow["AutoportExportVehiclesID"].ToString() ==
                                    drow["AutoportExportVehiclesID"].ToString())
                                {
                                    dvrow["Size"] = Globalitems.strSelection;
                                    dvrow["SizeClass"] = "Size: " + Globalitems.strSelection;
                                }
                            } 
                        }
                        else
                        {
                            MessageBox.Show("You cannot print any labels if the group has " +
                                "VINS missing the Size Class.", "MISSING SIZE CLASS - NO LABELS PRINTE",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }
                    }
                }

                //Store DISTINCT vehIDs with no BarCodeLabel print ind, in UnprintedIDs
                objPrintInfo.UnprintedIDs.Clear();
                strFilter = "BarCodeLabelPrintedInd = 0";
                dv = new DataView(ds.Tables[0],strFilter, "AutoportExportVehiclesID",
                    DataViewRowState.CurrentRows);
               if (dv.Count > 0)
                {
                    dtDistinctIDs = dv.ToTable(true, "AutoportExportVehiclesID");
                    foreach (DataRow drow in dtDistinctIDs.Rows)
                        objPrintInfo.UnprintedIDs.Add(
                            Convert.ToInt32(drow["AutoportExportVehiclesID"].ToString()));                    
                }

                //Return table in ds as a ReportDataSource named 'dsLabels'
                return new ReportDataSource("dsLabels", ds.Tables[0]);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetReportDataSource", ex.Message);
                return null;
            }
        }

        private bool SelectionMade()
        {
            try
            {
                DataSet ds;
                int intRecs;
                string strSQL;

                //Make sure a selection rb is selected
                if (!rbSelected.Checked && !rbAll.Checked && !rbBatch.Checked && 
                    !rbPrintDate.Checked && !rbVIN.Checked)
                {
                    MessageBox.Show("Please indicate if the labels are from Selected Rows,\n  Unprinted labels," +
                        "VIN, Batch ID, or Print Date ", "NO SELECTION TYPE CHOSEN",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //Ck VIN
                if (rbVIN.Checked)
                {
                    if (txtVIN.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Please enter the VIN", "MISSING VIN",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        rbVIN.Checked = true;
                        txtVIN.Focus();
                        return false;
                    }

                    //Ck that VIN is in Veh. table
                    strSQL = @"SELECT AutoportExportVehiclesID 
                    FROM AutoportExportVehicles WHERE VIN LIKE '%" + 
                    txtVIN.Text.Trim() + "%'";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "SelectionMade",
                            "No table returned when check AutoportExportVehicles table");
                        return false;
                    }

                    intRecs = ds.Tables[0].Rows.Count;

                    if (intRecs == 0)
                    {
                        MessageBox.Show("There are no vehicles with the entered VIN", 
                            "NO VEHICLES FOR VIN",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        rbVIN.Checked = true;
                        txtVIN.Focus();
                        txtVIN.SelectionStart = 0;
                        txtVIN.SelectionLength = txtVIN.Text.Length;
                        return false;
                    }

                    if (intRecs > 1)
                    {
                        if (MessageBox.Show("There are " + intRecs.ToString() +
                            " vehicles with the VIN info entered.\n\n" +
                            "Print labels for all the vehicles?", "MULTIPLE VEHICLES FOUND",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.No)
                        {
                            rbVIN.Checked = true;
                            txtVIN.Focus();
                            txtVIN.SelectionStart = 0;
                            txtVIN.SelectionLength = txtVIN.Text.Length;
                            return false;
                        }

                        rbVIN.Checked = true;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SelectionMade", ex.Message);
                return false;
            }
        }

        private void StartRequest(string strRQ)
        {
            ReportDataSource rptSource;
            string strIDs;
            string strSQL;

            try
            {   
                if (!SelectionMade()) return;

                rptSource = GetReportDataSource();
                if (rptSource == null) return;

                if (strRQ == "DISPLAY")
                    OpenReportDisplayForm(rptSource);
                else
                    PrintLabelsDirectly(rptSource);

                //Updated unprinted
                //Since no exception, assume labels printed. Update BarCodeLabelPrintedDate,BarCodeLabelPrintedInd 
                //  in AutoportExportVehicles table, with no print data
                if (objPrintInfo.UnprintedIDs.Count > 0)
                {
                    strIDs = "AND AutoportExportVehiclesID IN (";

                    foreach (int intID in objPrintInfo.UnprintedIDs)
                        strIDs += intID.ToString() + ",";

                    //Replace last ',' with ')'
                    strIDs = strIDs.Substring(0, strIDs.Length - 1) + ") ";

                    strSQL = "UPDATE AutoportExportVehicles SET BarCodeLabelPrintedInd=1," +
                        "BarCodeLabelPrintedDate='" + DateTime.Now.ToString() + "' " +
                        "WHERE BarCodeLabelPrintedInd=0 " + strIDs;
                    DataOps.PerformDBOperation(strSQL);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "StartRequest", ex.Message);
            }
        }

        private void PrintLabelsDirectly(ReportDataSource rptSource)
        {
            LocalReport rpt;
            PrintReport objPrint;
            string strReportPath;

            try
            {
                strReportPath = Globalitems.SetReportPath(LABELREPORT);

                //Instantiate a local report, and set its ReportPath & Datasource
                rpt = new LocalReport();
                rpt.ReportPath = strReportPath;
                rpt.DataSources.Add(rptSource);

                //Use the PrintReport object to print the report 
                objPrint = new PrintReport(rpt, Globalitems.strLabelPrinter,"LABEL");
                objPrint.PrintAReport();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PrintLabelsDirectly", ex.Message);
            }
        }

        private void OpenReportDisplayForm(ReportDataSource rptSource)
        {
            frmDisplayreport frm;
            string strMsg;
            string strReportPath;

            try
            {
                blnDisplayingLabel = true;
                strMsg = "Labels";
                strReportPath = Globalitems.SetReportPath(LABELREPORT);

                frm = new frmDisplayreport(strMsg, strReportPath, rptSource, 
                    265,700);
                Formops.SetFormBackground(frm);
                frm.ShowDialog();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenReportDisplayForm", ex.Message);
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

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            StartRequest("DISPLAY");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            StartRequest("PRINT");
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }

        private void frmLabels_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
