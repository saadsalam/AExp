using AutoExport.Objects;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace AutoExport.Objects
{
    static class Formops
    {
        private const string CURRENTMODULE = "Formops";

        public static AddressInfo CheckZipCode(Form frm, TextBox txtZipcode, ref bool blnIgnoreZipcode)
        {
            try
            {
                bool blnIsNumeric;
                Int64 intZip;
                AddressInfo objAddress;
                string strMsg;

                //Make sure zip is at least 5 digits
                if (txtZipcode.Text.Trim().Length < 5)
                {
                    strMsg = "The Zip code must be at least 5 digits in the U.S.\n\n" +
                        "Ignore Zip code checking for this address?";

                    if (MessageBox.Show(strMsg, "INVALID ZIP CODE", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error) == DialogResult.Yes)
                        blnIgnoreZipcode = true;
                    else
                    {
                        txtZipcode.Focus();
                        txtZipcode.SelectionStart = 0;
                        txtZipcode.SelectionLength = txtZipcode.Text.Length;
                    }

                    return null;
                }

                ////Make sure zip is an integer
                blnIsNumeric = Int64.TryParse(txtZipcode.Text.Trim(), out intZip);
                if (!blnIsNumeric)
                {
                    strMsg = "The Zip code must be all numbers in the U.S.\n\n" +
                       "Ignore Zip code checking for this address?";

                    if (MessageBox.Show(strMsg, "INVALID ZIP CODE", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Error) == DialogResult.Yes)
                        blnIgnoreZipcode = true;
                    else
                    {
                        txtZipcode.Focus();
                        txtZipcode.SelectionStart = 0;
                        txtZipcode.SelectionLength = txtZipcode.Text.Length;
                    }
                    return null;
                }

                objAddress = Globalitems.DecodeZipPCMiler(txtZipcode.Text.Trim());

                //Ck if error returned
                if (objAddress.error.Length > 0)
                {
                    strMsg = "Invalid Zip Code";
                    if (objAddress.error.Length > 0) strMsg = objAddress.error;

                    strMsg += "\n\nIgnore Zip code checking for this address ? ";

                    if (MessageBox.Show(strMsg, "INVALID ZIP CODE", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error) == DialogResult.Yes)
                        blnIgnoreZipcode = true;
                    else
                    {
                        txtZipcode.Focus();
                        txtZipcode.SelectionStart = 0;
                        txtZipcode.SelectionLength = txtZipcode.Text.Length;
                    }

                    return objAddress;
                }

                return objAddress;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CheckZipCode", ex.Message);
                return null;
            }
        }

        public static void ChangeControlUpdatedStatus(string strCtrl, List<ControlInfo> lsControls)
        {
            ControlInfo ctrlinfo;

            try
            {
                //Find the controlinfo object in lsControls and set Updated to true
                ctrlinfo = lsControls.Find(obj => obj.ControlID == strCtrl);
                ctrlinfo.Updated = true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ChangeControlUpdatedStatus", ex.Message);
            }
        }

        public static void InventoryComparisonReport()
        {
            //NOTE: Export group is supposed to perform a physical the next day after
            //  a ship date. This report looks for three exceptions:
            //  1)'Shows As Shipped': (should not happen), veh. table has veh. status of 'shipped'
            //      but VIN appears in the Import table in the physical's BatchID
            //  2) VIN Not Scanned: VIN is in the veh. table, but was not included 
            //      in the Import table in the physical's BatchID. Means not all veh's in yard
            //      were scanned in the physical
            //  3) VIN Not Found: VIN is part of the physical, but is not in the veh. table. 
            //     Happens when veh's were dropped off in yard, 
            //     but not yet Imported in Import/Export form

            //3/29/18 D.Maibor: For 1st query, Shows As Shipped, add importedind=1 to where clause

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
                string strDestinations = "";
                string strSQL;

                frm = new frmInvRptParams();
                frm.strInvtype = "POST";
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
                            case "Destination":
                                strDestinations = strNameValuePair[1];
                                break;
                        }
                    }   // foreach strParam in strParams

                    //Create SQL for report, using params in WHERE clauses (same as DATS)
                    //1st SELECT for 'Shows As Shipped' exception
                    //
                    strSQL = @"SELECT 
                    CASE
	                    WHEN LEN(RTRIM(ISNULL(cus.ShortName,''))) > 0 THEN RTRIM(cus.ShortName)
	                    ELSE RTRIM(cus.CustomerName)
                    END AS Customer,
                    veh.VIN,
                    veh.Make,
                    veh.Model,
                    voy.VoyageNumber,
                    veh.BookingNumber,
                    veh.DestinationName,
                    ves.VesselName,
                    veh.VehicleStatus,
                    veh.SizeClass,
                    veh.BayLocation,
                    CONVERT(varchar(10),veh.DateReceived,101) AS DateReceived,
                    CONVERT(varchar(10),veh.DateSubmittedCustoms,101) AS DateSubmittedCustoms,
                    CONVERT(varchar(10),veh.CustomsExceptionDate,101) AS CustomsExceptionDate,
                    CONVERT(varchar(10),veh.CustomsApprovedDate,101) AS CustomsApprovedDate,
                    CONVERT(varchar(10),veh.DateShipped,101) AS DateShipped,
                    'Shows As Shipped' AS ExceptionCode
                    FROM AutoportExportVehicles veh
                    LEFT OUTER JOIN Customer cus on cus.CustomerID=veh.CustomerID
                    LEFT OUTER JOIN AEVoyage voy on voy.AEVoyageID=veh.VoyageID
                    LEFT OUTER JOIN AEVessel ves on ves.AEVesselID=voy.AEVesselID
                    WHERE veh.VIN IN 
                    (SELECT VIN FROM AutoportExportVehiclesImport WHERE 
                    AutoportExportVehiclesImportID IS NOT NULL AND ImportedInd = 1 ";

                    //Add WHERE clause based on Params

                    //Ck BatchIDs
                    if (strBatchIDs.Length > 0)
                    { strSQL += "AND BatchID IN  (" + strBatchIDs + ") "; }

                    //Ck Creation From Date
                    if (strFromDate.Length > 0)
                    { strSQL += "AND CreationDate >= '" + strFromDate + "' "; }

                    //Ck Creation To Date
                    if (strToDate.Length > 0)
                    { strSQL += "AND CreationDate < DATEADD(day,1,'" + strToDate + "' "; }

                    strSQL += ") ";

                    //Ck Customer
                    if (strCustomerID.Length > 0) strSQL +=
                            "AND veh.CustomerID=" + strCustomerID + " ";

                    //Add Veh. Status
                    strSQL += "AND veh.VehicleStatus LIKE 'Shipped%' ";

                    //2nd Select for 'VIN Not Scanned' exception
                    strSQL += @"UNION 
                    SELECT 
                     CASE
	                    WHEN LEN(RTRIM(ISNULL(cus.ShortName,''))) > 0 THEN RTRIM(cus.ShortName)
	                    ELSE RTRIM(cus.CustomerName)
                    END AS Customer,
                    veh.VIN,
                    veh.Make,
                    veh.Model,
                    voy.VoyageNumber,
                    veh.BookingNumber,
                    veh.DestinationName,
                    ves.VesselName,
                    veh.VehicleStatus,
                    veh.SizeClass,
                    veh.BayLocation,
                    CONVERT(varchar(10),veh.DateReceived,101) AS DateReceived,
                    CONVERT(varchar(10),veh.DateSubmittedCustoms,101) AS DateSubmittedCustoms,
                    CONVERT(varchar(10),veh.CustomsExceptionDate,101) AS CustomsExceptionDate,
                    CONVERT(varchar(10),veh.CustomsApprovedDate,101) AS CustomsApprovedDate,
                    CONVERT(varchar(10),veh.DateShipped,101) AS DateShipped,
                    'VIN Not Scanned' AS ExceptionCode 
                    FROM AutoportExportVehicles veh
                    LEFT OUTER JOIN Customer cus on cus.CustomerID=veh.CustomerID
                    LEFT OUTER JOIN AEVoyage voy on voy.AEVoyageID=veh.VoyageID
                    LEFT OUTER JOIN AEVessel ves on ves.AEVesselID=voy.AEVesselID
                    WHERE veh.VIN NOT IN 
                    (SELECT VIN FROM AutoportExportVehiclesImport WHERE 
                    AutoportExportVehiclesImportID IS NOT NULL ";

                    //Ck BatchIDs
                    if (strBatchIDs.Length > 0)
                    { strSQL += "AND BatchID IN  (" + strBatchIDs + ") "; }

                    //Ck Creation From Date
                    if (strFromDate.Length > 0)
                    { strSQL += "AND CreationDate >= '" + strFromDate + "' "; }

                    //Ck Creation To Date
                    if (strToDate.Length > 0)
                    { strSQL += "AND CreationDate < DATEADD(day,1,'" + strToDate + "' "; }

                    strSQL += ") ";

                    //Add Veh. Status
                    strSQL += "AND veh.VehicleStatus NOT LIKE 'Shipped%' ";

                    //Add DateReceived
                    strSQL += "AND veh.DateReceived IS NOT NULL ";

                    //Ck Destination
                    if (strDestinations.Length > 0)
                    {
                        //Add (' at beginning and ') at end
                        strDestinations = "('" + strDestinations + "')";

                        //Add ' before & after each ,
                        strDestinations = strDestinations.Replace(",", "','");

                        strSQL += "AND veh.Destination IN " + strDestinations;
                    }

                    //Ck Customer
                    if (strCustomerID.Length > 0) strSQL +=
                            "AND veh.CustomerID=" + strCustomerID + " ";

                    //3rd Select for 'VIN Not Found' exception
                    strSQL += @"UNION 
                    SELECT 
                     '' AS Customer,
                    imp.VIN,
                    '' AS Make,
                    '' AS Model,
                    '' AS VoyageNumber,
                    '' AS BookingNumber,
                    '' AS DestinationName,
                    '' AS VesselName,
                    '' AS VehicleStatus,
                    '' AS SizeClass,
                    imp.BayLocation,
                    '' AS DateReceived,
                    '' AS DateSubmittedCustoms,
                    '' AS CustomsExceptionDate,
                    '' AS CustomsApprovedDate,
                    '' AS DateShipped,
                    'VIN Not Found' AS ExceptionCode 
                    FROM AutoportExportVehiclesImport imp 
                    WHERE imp.AutoportExportVehiclesImportID IS NOT NULL ";


                    //Ck BatchIDs
                    if (strBatchIDs.Length > 0)
                    { strSQL += "AND imp.BatchID IN  (" + strBatchIDs + ") "; }

                    //Ck Creation From Date
                    if (strFromDate.Length > 0)
                    { strSQL += "AND imp.CreationDate >= '" + strFromDate + "' "; }

                    //Ck Creation To Date
                    if (strToDate.Length > 0)
                    { strSQL += "AND imp.CreationDate < DATEADD(day,1,'" + strToDate + "' "; }

                    //Add VIN not in veh table
                    strSQL += @"AND imp.VIN NOT IN 
                    (SELECT VIN FROM AutoportExportVehicles veh )
                    ORDER BY Customer,ExceptionCode,BayLocation";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No vehicles meet the report criteria",
                            "NO VEHICLES TO INCLUDE IN THE REPORT", MessageBoxButtons.OK,
                            MessageBoxIcon.Hand);
                        return;
                    }

                    OpenInvRpt(ds.Tables[0]);

                }   //if dlResult == OK
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "InventoryComparisonReport", ex.Message);
                return;
            }
        }

        private static void OpenInvRpt(DataTable dt)
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
                    new ControlInfo { RecordFieldName = "Make", HeaderText = "Make"},
                    new ControlInfo { RecordFieldName = "Model", HeaderText = "Model"},
                    new ControlInfo { RecordFieldName = "VoyageNumber", HeaderText = "Voyage #" },
                    new ControlInfo { RecordFieldName = "BookingNumber", HeaderText = "Book #" },
                    new ControlInfo { RecordFieldName = "DestinationName", HeaderText = "Destination" },
                    new ControlInfo { RecordFieldName = "VesselName", HeaderText = "Vessel" },
                    new ControlInfo { RecordFieldName = "VehicleStatus", HeaderText = "Veh. Status" },
                    new ControlInfo { RecordFieldName = "SizeClass", HeaderText = "Size Class" },
                    new ControlInfo { RecordFieldName = "BayLocation", HeaderText = "Bay Loc." },
                    new ControlInfo { RecordFieldName = "DateReceived", HeaderText = "Date Rcv'd" },
                    new ControlInfo { RecordFieldName = "DateSubmittedCustoms", HeaderText = "Date Sub'd Customs" },
                    new ControlInfo { RecordFieldName = "CustomsExceptionDate", HeaderText = "Cust. Exc. Date" },
                    new ControlInfo { RecordFieldName = "CustomsApprovedDate", HeaderText = "Cust. Clear Date" },
                    new ControlInfo { RecordFieldName = "DateShipped", HeaderText = "Date Shipped" },
                    new ControlInfo { RecordFieldName = "ExceptionCode", HeaderText = "Exception" }                    
                };

                //Invoke CreateSCVFile to create, save, &open the csv file
                CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenInvRpt", ex.Message);
            }
        }

        public static void NumericKeyPress(TextBox txtbox, KeyPressEventArgs e)
        {
            //Only allow digits & backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        static public void ClearRecordData(Form frm, List<ControlInfo> lsControls,
            bool blnReset = true)
        {
            try
            {
                Control[] ctrls;
                CheckBox ckBox;
                ComboBox cboBox;
                ListBox lbBox;

                //Use linq to get a list of controls where RecordFieldName != null and ControlID != null
                var recordlist = lsControls.Where(ctrlinfo =>
                    ctrlinfo.RecordFieldName != null && ctrlinfo.ControlID != null).ToList();

                //Initialize each control
                foreach (ControlInfo ctrlinfo in recordlist)
                {
                    ctrls = frm.Controls.Find(ctrlinfo.ControlID, true);
                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            ctrls[0].Text = "";
                            break;
                        case "SelectedValue":
                            //Cast control to ComboBox
                            cboBox = (ComboBox)ctrls[0];

                            //If 1st selection in cboBox is all, set to 1st item, otherwise set to no value
                            if ((cboBox.Items[0] as ComboboxItem).cboValue.ToString() == "All" ||
                                (cboBox.Items[0] as ComboboxItem).cboValue.ToString() == "select")
                                cboBox.SelectedIndex = 0;
                            else
                                cboBox.SelectedIndex = -1;
                            break;
                        case "Checked":
                            ckBox = (CheckBox)ctrls[0];
                            ckBox.Checked = false;
                            break;
                        case "Listbox":
                            lbBox = (ListBox)ctrls[0];
                            lbBox.DataSource = null;
                            break;
                    }
                }

                if (blnReset) ResetControls(frm, lsControls);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "clearRecordData", ex.Message);
            }
        }

        public static void ClearSetup(Form frm, List<ControlInfo> lsControls)
        {
            CheckBox ckBox;
            ComboBox cboBox;
            Control[] ctrls;
            Control ctrlx;

            ControlInfo ctrlin;

            try
            {
                //Use linq to get a list of controls where ControlPropetyToBind != "" and ControlID != null
                var recordlist = lsControls.Where(ctrlinfo =>
                    ctrlinfo.ControlPropetyToBind != "" && ctrlinfo.ControlID != null).ToList();

                foreach (ControlInfo ctrlinfo in recordlist)
                {
                    ctrlin = ctrlinfo;

                    //Get the Control assoiated with ctrlinfo.ControlID
                    ctrls = frm.Controls.Find(ctrlinfo.ControlID, true);
                    ctrlx = ctrls[0];

                    //Initialize control value if  ControlPropetyToBind != ""
                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            ctrlx.Text = "";
                            break;
                        case "SelectedValue":
                            //Cast control to ComboBox
                            cboBox = (ComboBox)ctrls[0];

                            if (cboBox.Items.Count > 0)
                            {
                                //If 1st selection in cboBox is all, set to 1st item, otherwise set to no value
                                if ((cboBox.Items[0] as ComboboxItem).cboValue.ToString() == "All" ||
                                    (cboBox.Items[0] as ComboboxItem).cboValue.ToString() == "select")
                                    cboBox.SelectedIndex = 0;
                                else
                                    cboBox.SelectedIndex = -1;
                            }
                            break;
                        case "Checked":
                            ckBox = (CheckBox)ctrls[0];
                            ckBox.Checked = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearSetup", ex.Message);
            }
        }

        static public void SetDetailRecord(DataRow dr, Form frm, List<ControlInfo> lsControls)
        {
            Control[] ctrls;
            Control ctrlx;
            ComboBox cboBox;
            CheckBox ckBox;

            try
            {
                //Use linq to get a list of controls where RecordFieldName != null and ControlID != null
                var recordlist = lsControls.Where(ctrlinfo =>
                    ctrlinfo.RecordFieldName != null && ctrlinfo.ControlID != null).ToList();
                foreach (ControlInfo ctrlinfo in recordlist)
                {
                    //Get the Control associated with ctrlinfo.ControlID
                    ctrls = frm.Controls.Find(ctrlinfo.ControlID, true);
                    ctrlx = ctrls[0];

                    //Set control value if RecordFieldName != 'custom' && field in drow != null 
                    if (ctrlinfo.RecordFieldName != "custom" && dr[ctrlinfo.RecordFieldName] != null)
                    {
                        switch (ctrlinfo.ControlPropetyToBind)
                        {
                            case "Text":
                                ctrlx.Text = "";
                                if (dr[ctrlinfo.RecordFieldName] != DBNull.Value)
                                    ctrlx.Text = dr[ctrlinfo.RecordFieldName].ToString();
                                break;
                            case "SelectedValue":
                                cboBox = (ComboBox)ctrls[0];

                                if (cboBox.Items.Count > 0)
                                {
                                    cboBox.SelectedIndex = 0;

                                    if (dr[ctrlinfo.RecordFieldName] != DBNull.Value)
                                    {
                                        foreach (ComboboxItem cboitem in cboBox.Items)
                                            if (cboitem.cboValue == dr[ctrlinfo.RecordFieldName].ToString())
                                                cboBox.SelectedItem = cboitem;
                                    }
                                }
                                    
                                break;
                            case "Checked":
                                ckBox = (CheckBox)ctrls[0];
                                if (dr[ctrlinfo.RecordFieldName] != DBNull.Value &&
                                    (Convert.ToInt32(dr[ctrlinfo.RecordFieldName].ToString()) == 1))
                                    ckBox.Checked = true;
                                break;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetDetailRecord", ex.Message);
            }
        }

        static public void CreateCSVFile(DataTable dt, List<ControlInfo> lsControls,
            string strFileName)
        {
            //Open a new window as .csv file 
            // dt contains the rows for the csv file
            // lsControls contains the Order, Header Text, & Record Field for each column
            // strFileName is the path to create the csv file
            try
            {
                StringBuilder sb = new StringBuilder();
                char chrComma = ',';
                DateTime datval;
                string strVal;

                //Use a StringBuilder for .csv data (faster than a string)

                //Enter Column Headings to sb from lsControls
                for (int i = 0; i < lsControls.Count; i++)
                {
                    sb.Append(lsControls[i].HeaderText);
                    if (i < lsControls.Count - 1) sb.Append(chrComma);
                }

                sb.AppendLine();

                //Enter each row of data in dt
                foreach (DataRow dtRow in dt.Rows)
                {
                    for (int i = 0; i < lsControls.Count; i++)
                    {
                        //Include field's value if not null
                        if (dtRow[lsControls[i].RecordFieldName] != null)
                        {
                            strVal = dtRow[lsControls[i].RecordFieldName].ToString();

                            //Handle ',' in field, encapsultate string with double quote (")
                            if (strVal.Contains(",")) strVal = "\"" + strVal + "\"";

                            //If date, put in M/D/YYYY format
                            if (lsControls[i].RecordFieldName.Contains("Date") && 
                                DateTime.TryParse(strVal, out datval))
                                strVal = datval.ToString("M/d/yyyy");

                            sb.Append(strVal);
                        }

                        // Add comma if not last field in row
                        if (i < lsControls.Count - 1) sb.Append(chrComma);
                    }
                    sb.AppendLine();
                }

                strVal = sb.ToString();

                File.WriteAllText(strFileName, strVal);
                Process.Start(strFileName);
            }

            catch (System.IO.IOException exIO)
            {
                MessageBox.Show("You have a .csv file open after clicking the Export button.\n\n" +
                    "You must close the open file, before clicking the Export button to open " +
                    "another file.", "OPEN CSV FILE", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CreateCSVFromDataTable", ex.Message);
            }
        }

        static public void ResetControls(Form frm, List<ControlInfo> lsControls)
        {
            //Use linq to get a list of controls where RecordFieldName != null and ControlID != null
            var resetlist = lsControls.Where(ctrlinfo =>
                ctrlinfo.RecordFieldName != null && ctrlinfo.ControlID != null).ToList();

            //Set updated to false
            foreach (ControlInfo ctrlinfo in resetlist) ctrlinfo.Updated = false;
        }

        static public void SetTabIndex(Form frm, List<ControlInfo> lsControls)
        {
            //Set the TabIndex for each control in lsControls based on its order in the list

            string strbadid;

            try
            {
                Control[] ctrls;
                int intIndex = 0;
                string strID;

                foreach (ControlInfo ctrlinfo in lsControls)
                {
                    if (ctrlinfo.ControlID != null)
                    {
                        strID = ctrlinfo.ControlID;
                        strbadid = strID;
                        ctrls = frm.Controls.Find(strID, true);
                        ctrls[0].TabIndex = intIndex;
                        intIndex += 1;
                    }
                }
            }

            catch (Exception ex)
            { Globalitems.HandleException(CURRENTMODULE, "SetTabIndex", ex.Message); }
        }

        public static void DateKeyPress(TextBox txtbox, KeyPressEventArgs e)
        {
            //Only allow digits, backspace, forward slash (/), dash (-), Tab 
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) &&
                e.KeyChar != '/' && e.KeyChar != '-' &&
                e.KeyChar != (char)Keys.Tab) e.Handled = true;
        }

        public static void OpenNewForm(Form newform)
        {
            //newform.MdiParent = mainform;
            SetFormBackground(newform);
            newform.Show();
        }
        public static void SetFormBackground(Form frm)
        {
            frm.Icon = Globalitems.icoApp;

            if (Globalitems.runmode == "TEST")
            {
                frm.BackgroundImage = Globalitems.imgTestImage;
                frm.Text = "TEST - " + frm.Text;
            }
            
        }

        public static void SetActiveForm(string strForm)
        {
            foreach (Form activeform in Application.OpenForms)
            {
                if (activeform.GetType().Name == strForm)
                {
                    activeform.Focus();
                    return;
                }
            }
        }

        public static void SetReadOnlyStatus(Form frm, List<ControlInfo> lsControls, bool blnReadOnly,
           RecordButtons recbuttons)
        {
            try
            {
                ComboBox cboBox;
                CheckBox ckBox;
                Control[] ctrls;
                Control ctrlx;
                ListBox lbBox;
                MaskedTextBox mtxtBox;
                TextBox txtBox;

                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);

                //Use linq to get a list of controls where RecordFieldName != null and 
                //  ControlID != null and ReadOnly = false
                var recordlist = lsControls.Where(ctrlinfo => ctrlinfo.RecordFieldName != null &&
                    ctrlinfo.ControlID != null && ctrlinfo.ReadOnly == false).ToList();
                foreach (ControlInfo ctrlinfo in recordlist)
                {
                    //Get the Control associated with ctrlinfo.ControlID
                    ctrls = frm.Controls.Find(ctrlinfo.ControlID, true);
                    ctrlx = ctrls[0];

                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            if (ctrlx is TextBox)
                            {
                                txtBox = (TextBox)ctrls[0];
                                txtBox.ReadOnly = blnReadOnly;
                                txtBox.Enabled = !blnReadOnly;
                                //Set textbox background to very light gray used in DataGridview
                                //  if Read Only, otherwise White
                                if (blnReadOnly)
                                    txtBox.BackColor = Color.FromArgb(244, 244, 244);
                                else
                                    txtBox.BackColor = Color.White;
                            }

                            if (ctrlx is MaskedTextBox)
                            {
                                mtxtBox = (MaskedTextBox)ctrls[0];
                                mtxtBox.ReadOnly = blnReadOnly;
                                mtxtBox.Enabled = !blnReadOnly;
                                if (blnReadOnly)
                                    mtxtBox.BackColor = Color.FromArgb(244, 244, 244);
                                else
                                    mtxtBox.BackColor = Color.White;
                            }

                            break;
                        case "SelectedValue":
                            cboBox = (ComboBox)ctrls[0];
                            cboBox.Enabled = !blnReadOnly;
                            if (blnReadOnly)
                                cboBox.BackColor = Color.FromArgb(244, 244, 244);
                            else
                                cboBox.BackColor = Color.White;

                            break;
                        case "Checked":
                            ckBox = (CheckBox)ctrls[0];
                            ckBox.AutoCheck = !blnReadOnly;
                            ckBox.Enabled = !blnReadOnly;
                            break;

                        case "Listbox":
                            lbBox = (ListBox)ctrls[0];
                            lbBox.Enabled = !blnReadOnly;
                            break;
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetReadOnlyStatus", ex.Message);
            }
        }
    }
}
