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
    public partial class frmVesselAdmin : Form
    {
        public bool blnNewVesselRQFromOtherForm = false;

        private const string CURRENTMODULE = "frmVesselAdmin";

        private BindingSource bs1 = new BindingSource();
        private DataTable dtVessels = new DataTable();
        private List<string> lsExcludes = new List<string>
            {
                {"txtNote_record"}
            };
        private List<string> lsVesselIDs = new List<string>();
        private string strMode;

        //Set up List of ControlInfo objects, lsControlInfo, as follows:
        //  Order in list establishes Indexes for tabbing, implemented by SetTabIndex() method
        //  AlwaysReadOnly identifies if control cannot be modified by User
        //  ControlPropertyToBind identifies what controls are initialized 
        //  RecordFieldName identify what controls display record detail
        //  HeaderText sets column header to use for Export to csv file
        //  Updated property establishes what controls User has modified

        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="txtVesselName",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboStatus",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtFrom",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtTo",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtVesselName_record", RecordFieldName="VesselName",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtVesselShortName_record",
                RecordFieldName ="VesselShortName",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtLloydsCode",
                RecordFieldName ="LloydsCode",ControlPropetyToBind="Text"},
             new ControlInfo {ControlID="cboStatus_record",ControlPropetyToBind="SelectedValue",
             RecordFieldName="RecordStatus"},
             new ControlInfo {ControlID="txtNote_record",
                RecordFieldName ="Notes",ControlPropetyToBind="Text"},
             new ControlInfo {ControlID="txtCreationDate", RecordFieldName="CreationDate",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCreatedBy", RecordFieldName="CreatedBy",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtUpdatedDate", RecordFieldName="UpdatedDate",
                ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtUpdatedBy", RecordFieldName="UpdatedBy",
                ControlPropetyToBind="Text"},
            // objects needed for csv file  HeaderText="Cust. Name"
            new ControlInfo {RecordFieldName="VesselName",HeaderText="Vessel"},
            new ControlInfo {RecordFieldName="RecordStatus",HeaderText="Status"}
        };

        public frmVesselAdmin()
        {
            InitializeComponent();
            dgResults.AutoGenerateColumns = false;
            btnExport.Enabled = false;
            lblVesselRecords.Text = "";

            strMode = "READ";
            FillCombos();
            Globalitems.SetControlHeight(this,lsExcludes);
            Formops.SetTabIndex(this, lsControls);

            // Assign methods to the recbuttons public event variables
            recbuttons.CancelRecord += btnCancel_Clicked;
            recbuttons.MovePrev += btnPrev_Clicked;
            recbuttons.MoveNext += btnNext_Clicked;
            recbuttons.DeleteRecord += btnDelete_Clicked;
            recbuttons.NewRecord += btnNew_Clicked;
            recbuttons.ModifyRecord += btnModify_Clicked;
            recbuttons.SaveRecord += btnSave_Clicked;

            DisplayMode();
        }

        private void AdjustReadOnlyStatus(bool blnReadOnly)
        {
            Formops.SetReadOnlyStatus(this, lsControls, blnReadOnly, recbuttons);
        }

        private void CancelSetup()
        {
            int intCurrentBSPosition = -1;

            //1. Set Status label
            lblStatus.Text = "Read only";

            //2. Enable Search/Results panels
            pnlSearch.Enabled = true;
            pnlResults.Enabled = true;

            //3. Display other open forms
            Globalitems.DisplayOtherForms(this, true);

            //4. Turn on ReadOnly on form's controls
            AdjustReadOnlyStatus(true);

            //5. Clear Record Controls
            Formops.ClearRecordData(this, lsControls);

            //6. Set Mode to READ, recbuttons with no records to display, & recbuttons to READONLY
            strMode = "READ";
            recbuttons.blnRecordsToDisplay = false;
            recbuttons.SetButtons(RecordButtons.ACTION_READONLY);

            //7. Enable Menu, Search, Clear buttons
            btnMenu.Enabled = true;
            btnSearch.Enabled = true;
            btnClear.Enabled = true;

            //7. Store Binding Source position if there are values, set record details, set Nav Buttons
            if (bs1.Count > 0) intCurrentBSPosition = bs1.Position;
            if (strMode == "MODIFY") intCurrentBSPosition = bs1.Position;

            //8. If dgResults has 1 or more rows: enable btnExport,reset navbuttons, reset record details 
            if (dgResults.RowCount > 0)
            {
                btnExport.Enabled = true;
                recbuttons.blnRecordsToDisplay = true;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                Globalitems.SetNavButtons(recbuttons, bs1);
                FillDetailRecord();
            }
        }

        private void ClearForm()
        {
            try
            {
                //1. Clear all items in lsControls
                Formops.ClearSetup(this, lsControls);

                //2. Clear Form unique grids
                ClearGridView();

                //3. Set Form unique Readonly/enabled status for controls
                AdjustReadOnlyStatus(true);

                //4. Clear Record Data
                Formops.ClearRecordData(this, lsControls);
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
                lblRecordDetails.Text = "";

                dtVessels.Clear();

                // Binding dgResults to lsUsers after the Clear method, can lead to runtime error because
                //  the CurrencyManager pointing to the Current position in lsUsers, doesn't reset to -1
                dgResults.DataSource = dtVessels;

                recbuttons.blnRecordsToDisplay = false;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearGridView", ex.Message);
            }
        }

        private void DisplayMode()
        {
            try
            {
                if (strMode == "READ")
                {
                    //Show panels, Action & Nav btns
                    lblStatus.Text = "Read Only";
                    AdjustReadOnlyStatus(true);

                    pnlSearch.Enabled = true;
                    pnlResults.Enabled = true;
                }
                else
                {
                    pnlSearch.Enabled = false;
                    pnlResults.Enabled = false;

                    if (strMode == "NEW" && bs1 != null) Formops.ClearRecordData(this, lsControls);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "DisplayMode", ex.Message);
            }
        }

        private void FillCombos()
        {
            //Use FillComboboxFromCodeTable in Globalitems to fill comboboxes from dtCode
            try
            {
                string strFilter;

                // To retrieve Selected value from a combobox need to assign a datasource to the combobox

                strFilter = "CodeType = 'RecordStatus'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboStatus, true, false);
                Globalitems.FillComboboxFromCodeTable(strFilter, cboStatus_record, false, true);
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void FillDetailRecord()
        {
            DataRow drow;
            DataSet ds;
            DataTable dtDetail;
            string strSQL;

            try
            {

                Formops.ClearRecordData(this, lsControls);

                //Qry returns a row for each CustomerID/DestinationName combination
                //  E.g., voyage w/two customers & 4 destinations returns 8 rows
                strSQL = "SELECT AEVesselID,VesselName,VesselShortName," +
                      "RecordStatus,LloydsCode,Notes,CreationDate," +
                      "CreatedBy,UpdatedDate,UpdatedBy " +
                      "FROM AEVessel WHERE AEVesselID = " + txtVesselID.Text;
                
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillDetailRecord",
                        "No data returned from query");
                    return;
                }

                dtDetail = ds.Tables[0];


                //Set Voyage#, VoyageDate,Created info, & Updated info from 1st Row
                //  (same info on each row)
                drow = dtDetail.Rows[0];
                Formops.SetDetailRecord(drow, this, lsControls);

                //Format Date textboxes
                txtCreationDate.Text = Globalitems.FormatDatetime(txtCreationDate.Text);
                txtUpdatedDate.Text = Globalitems.FormatDatetime(txtUpdatedDate.Text);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillDetailRecord", ex.Message);
            }
        }

        private void FilterBindingSource()
        {
            try
            {
                //User has selected a row in the Gridview, dgResults.
                //Change the current binding source to the selected row

                //Because dgResults is not multiselect, 
                //create a list of 1 VoyageID for the Form's binding source, 
                //for use by the nav buttons
                lsVesselIDs.Clear();
                lsVesselIDs.Add(dgResults.SelectedRows[0].Cells[0].Value.ToString());

                bs1.DataSource = lsVesselIDs;

                //6e. Fill detail record with first row AEVoyageID
                txtVesselID.Text = lsVesselIDs[0];
                FillDetailRecord();

                //6f. Update recbuttons
                recbuttons.blnRecordsToDisplay = true;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "filterBindingSource", ex.Message);
            }
        }

        private void NewRecordSetup()
        {
            try
            {
                //1. Set Mode
                strMode = "NEW";

                //2. Set Status label
                lblStatus.Text = "Add new record";

                //3. Disable Search/Results panels, & related buttons
                pnlSearch.Enabled = false;
                pnlResults.Enabled = false;
                btnSearch.Enabled = false;
                btnClear.Enabled = false;
                btnExport.Enabled = false;

                //4. Hide Other Forms
                Globalitems.DisplayOtherForms(this, false);

                //5. Turn off ReadOnly on form's controls
                AdjustReadOnlyStatus(false);

                //6. Clear Record controls
                Formops.ClearRecordData(this, lsControls);

                //7. Set recbuttons to New
                recbuttons.SetButtons(RecordButtons.ACTION_NEWRECORD);

                //8. Set Created By/Date 
                txtCreatedBy.Text = Globalitems.strUserName;
                txtCreationDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

                //9. Set focus on first contol
                txtVesselName_record.Focus();

                //10. Handle Form unique controls
                cboStatus_record.SelectedValue = "Active";
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "NewRecordSetup", ex.Message);
            }
        }

        private void ModifyRecordSetup()
        {
            try
            {
                //1. Set Mode
                strMode = "MODIFY";

                //2. Set Status label
                lblStatus.Text = "Modify current record";

                //3. Disable Search/Results panels & Menu btn
                pnlSearch.Enabled = false;
                pnlResults.Enabled = false;
                btnMenu.Enabled = false;
                btnSearch.Enabled = false;
                btnClear.Enabled = false;
                btnExport.Enabled = false;

                //4. Hide other open forms
                Globalitems.DisplayOtherForms(this, false);

                //5. Turn off ReadOnly on form's controls
                AdjustReadOnlyStatus(false);

                //6. Reset form's controls to Updated = false
                Formops.ResetControls(this, lsControls);

                //7. Set recbuttons to Modify
                recbuttons.SetButtons(RecordButtons.ACTION_MODIFYRECORD);

                //8. Set Updated By/Date to new value
                txtUpdatedBy.Text = Globalitems.strUserName;
                txtUpdatedDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

                //9. Set focus on first control
                txtVesselName_record.Focus();

                //10. Handle Form unique controls
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ModifyRecordSetup", ex.Message);
            }
        }

        private void OpenCSVFile()
        {
            try
            {
                DataSet ds;
                DataTable dt;
                DataView dv;
                string strFilename;
                string strSort = "";
                string strSQL;
                List<ControlInfo> lsCSVcols = new List<ControlInfo>();

                //1. Get the file Directory & Filename from the SettingTable
                strSQL = "SELECT ValueKey,ValueDescription FROM SettingTable " +
                    "WHERE ValueKey IN ('ExportDirectory','VesselExportFileName') " +
                    "AND RecordStatus='Active' ORDER BY ValueKey";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                // S/B just two active rows, row 1 ExportDirectory, row 2 VesselExportFileName
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 2)
                {
                    Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile",
                        "No rows returned from SettingTable");
                    return;
                }
                // 1st Record s/b ExportDirectory, 2nd Record s/b VesselExportFileName
                strFilename = ds.Tables[0].Rows[0]["ValueDescription"].ToString();
                strFilename += @"\" + ds.Tables[0].Rows[1]["ValueDescription"].ToString();

                //2. Create a copy of the datatable used for the datagridview datasource
                dt = dtVessels.Copy();

                //3. If the gridview is sorted, use a dv to sort the table copy the same way
                if (dgResults.SortedColumn != null)
                {
                    strSort = dgResults.SortedColumn.DataPropertyName;
                    if (dgResults.SortOrder == SortOrder.Descending) strSort += " DESC";
                    dv = new DataView(dt, "", strSort, DataViewRowState.CurrentRows);
                    dt = dv.ToTable();
                }

                //4. Create a list, lsCSVcols with ControlInfo objects in the order to appear in the csv file 
                //Get ctrlinfo object from lsControls for Vessel & add to lsCSVcols. Use HeaderText to ID objects
                var objctrlinfo = lsControls.Where(obj => obj.HeaderText == "Vessel").ToList();
                lsCSVcols.Add(objctrlinfo[0]);

                objctrlinfo = lsControls.Where(obj => obj.HeaderText == "Status").ToList();
                lsCSVcols.Add(objctrlinfo[0]);

                //5. Invoke CreateSCVFile to create, save, & open the csv file
                Formops.CreateCSVFile(dt, lsCSVcols, strFilename);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenCSVFile", ex.Message);
            }
        }

        private void PerformDeleteRecord()
        {
            try
            {
                DataSet ds;
                frmAreYouSure frmConfirm;
                string strMessage;
                string strSQL;

                //Check if AutoportExportVehicles table has Shipped veh's 
                //  for the Vessel to delete
                strSQL = "SELECT COUNT(veh.AutoportExportVehiclesID) AS totrec " +
                    "FROM AutoportExportVehicles veh " +
                    "INNER JOIN AEVoyage voy on voy.AEVoyageID = veh.VoyageID " +
                    "INNER JOIN AEVessel ves on ves.AEVesselID = voy.AEVesselID " +
                    "WHERE ves.AEVesselID = " + 
                    txtVesselID.Text;
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count==0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord",
                        "No data returned from the query when checking " +
                        "Shipped Veh. Status");
                    return;
                }

                //There are veh's associated with the Vessel to delete 
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["totrec"]) > 0)
                {
                    MessageBox.Show("The Vessel cannot be deleted because there are " +
                            "Shipped vehicles associated with the Vessel in the DB.",
                            "VESSEL CANNOT BE DELETED",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                //There are no Shipped veh's associated with the Vessel.
                //Ck if there are voyages associated with the Vessel
                strSQL = "SELECT voy.VoyageDate " +
                    "FROM AEVoyage voy " +
                    "INNER JOIN AEVessel ves on ves.AEVesselID = voy.AEVesselID " +
                    "WHERE ves.AEVesselID = " + txtVesselID.Text;
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord",
                        "No data returned from the query when checking " +
                        "Voyages associated with the Vessel");
                    return;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    strMessage = "The following Voyages are associated with this Vessel:";
                    foreach (DataRow drow in ds.Tables[0].Rows)
                        strMessage += "\nVoyage Date: " + 
                           Convert.ToDateTime(drow["VoyageDate"]).ToString("M/d/yyyy");

                    strMessage += "\n\n You must change the Vessel associated with " +
                        "each Voyage to delete this Vessel";

                    MessageBox.Show(strMessage, "VESSEL CANNOT BE DELETED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }


                strMessage = "You are about to Delete a Vessel!\n\n" +
                    "You could instead change the Status to Inactive.\n\n" +
                    "Are you sure you want to delete the Vessel?";

                frmConfirm = new frmAreYouSure(strMessage);
                var result = frmConfirm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    //Delete the Vessel
                    strSQL = "DELETE AEVessel WHERE AEVesselID = " +
                        txtVesselID.Text;
                    DataOps.PerformDBOperation(strSQL);

                    PerformSearch();

                    MessageBox.Show("The Vessel has been removed from the DB", "VESSEL DELETED",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord", ex.Message);
            }
        }

        private void PerformMoveNext()
        {
            try
            {
                bs1.MoveNext();
                FillDetailRecord();
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMoveNext", ex.Message);
            }
        }

        private void PerformMovePrevious()
        {
            try
            {
                bs1.MovePrevious();
                FillDetailRecord();
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMovePrevious", ex.Message);
            }
        }

        private void PerformSaveRecord()
        {
            string strUpdateVesselSQL;
            try
            {
                if (ValidRecord())
                {
                    if (strMode == "NEW")
                        strUpdateVesselSQL = SQLForNewRecord();
                    else
                        strUpdateVesselSQL = SQLForModifiedRecord();

                    DataOps.PerformDBOperation(strUpdateVesselSQL);

                    MessageBox.Show("The Voyage info has been updated in the DB.",
                       "VOYAGE INFO UPDATED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Display other forms
                    Globalitems.DisplayOtherForms(this, true);

                    //Set Mode to READ
                    strMode = "READ";
                    recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                    AdjustReadOnlyStatus(true);

                    //Enable Search/Results panels
                    pnlSearch.Enabled = true;
                    pnlResults.Enabled = true;

                    btnSearch.Enabled = true;
                    btnClear.Enabled = true;

                    //Set Status label to Read only
                    lblStatus.Text = "Read only";

                    //Perform new search
                    PerformSearch();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord", ex.Message);
            }
        }


        private void PerformSearch()
        {
            DataSet ds;
            string strSQL;
            string strval;

            try
            {
                //1. Disable Export button
                btnExport.Enabled = false;

                //2. Clear Results gridview
                ClearGridView();

                //3. Clear Record data
                Formops.ClearRecordData(this, lsControls);

                //4. Set recbuttons to display = false
                recbuttons.blnRecordsToDisplay = false;

                //5. Retrieve data as datatable
                strSQL = "SELECT AEVesselID,VesselName," +
                     "RecordStatus " +
                     "FROM AEVessel WHERE LEN(RTRIM(ISNULL(VesselName,''))) > 0 ";

                //Add VesselName
                if (txtVesselName.Text.Trim().Length > 0)
                    strSQL += "AND (VesselName LIKE '%" + txtVesselName.Text.Trim() + "%' " +
                        "OR VesselShortName LIKE '%" + txtVesselName.Text.Trim() + "%') ";

                //Add RecordStatus
                if ((cboStatus.SelectedItem as ComboboxItem).cboValue != "All")
                {
                    strval = (cboStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    Globalitems.HandleSingleQuoteForSQL(strval);
                    strSQL += "AND RecordStatus ='" + strval + "' ";
                }

                //Add Created From
                if (txtFrom.Text.Trim().Length > 0)
                    strSQL += " AND CreationDate >= '" + txtFrom.Text + "' ";

                //Add Created To
                if (txtTo.Text.Trim().Length > 0)
                    strSQL += " AND CreationDate <= '" + txtTo.Text + "' ";

                strSQL += "ORDER BY VesselName";

                ds = DataOps.GetDataset_with_SQL(strSQL);

                // Use a DataTable as the DataSource for the DataGridView to make sorting by Col Header
                //  clicks, automatic
                dtVessels = ds.Tables[0].Copy();
                if (dtVessels.Rows.Count == 0) return;

                //6. If data found:
                //6a. Enable Export button
                btnExport.Enabled = true;

                //6b. Assign Datatable to gridvirew
                dgResults.DataSource = dtVessels;

                //6c. Update # records label
                lblVesselRecords.Text = "Records: " + dtVessels.Rows.Count;

                //6d. Because dgResults is not multiselect, 
                //create a list of 1 CustomerID for the Form's binding source, 
                //for use by the nav buttons
                lsVesselIDs.Clear();
                lsVesselIDs.Add(dtVessels.Rows[0]["AEVesselID"].ToString());

                bs1.DataSource = lsVesselIDs;

                //6e. Fill detail record with first row AEVoyageID
                txtVesselID.Text = lsVesselIDs[0] ;
                FillDetailRecord();

                //6f. Update recbuttons
                recbuttons.blnRecordsToDisplay = true;
                recbuttons.SetButtons(RecordButtons.ACTION_READONLY);
                Globalitems.SetNavButtons(recbuttons, bs1);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSearch", ex.Message);
            }
        }

        private string SQLForModifiedRecord()
        {
            CheckBox ckBox;
            ComboBox cboBox;
            Control[] ctrls;
            string strSQL = "";
            string strval;

            try
            {
                strSQL = "UPDATE AEVessel SET ";

                // Use linq to get a list of updated controls, 
                //  For this form, textboxes, combobox, checkbox; exclude listboxes
                var changedlist = lsControls.Where(ctrlinfo =>ctrlinfo.Updated == true);

                foreach (ControlInfo ctrlinfo in changedlist)
                {
                    strSQL += ctrlinfo.RecordFieldName + " = ";

                    //Place the control into the array ctrls, s/b only one
                    ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            strval = ctrls[0].Text.Trim();

                            // Use HandleSingleQuoteForSQL 
                            //to replace ' in text to '' for SQL
                            strval = Globalitems.HandleSingleQuoteForSQL(strval);

                            if (strval.Length == 0)
                                strSQL += "NULL";
                            else
                                strSQL += "'" + strval + "'";

                            break;
                        case "SelectedValue":
                            //Cast control to ComboBox. All cbo's except cboVehStatus,
                            //cboRecordStatus, cboDest have an int value in the
                            //AutoportExportVehicles table
                            cboBox = (ComboBox)ctrls[0];
                            strval = (cboBox.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                            strval = Globalitems.HandleSingleQuoteForSQL(strval);
                            if (strval == "select")
                                strSQL += "NULL";
                            else
                                strSQL += "'" + strval + "'" ;
                            break;
                        case "Checked":
                            //Cast control to Checkbox
                            ckBox = (CheckBox)ctrls[0];
                            if (ckBox.Checked)
                                strSQL += "1";
                            else
                                strSQL += "0";
                            break;
                    }
                    strSQL += ",";
                }

                //Add UpdatedDate & UpdatedBy
                strSQL += "UpdatedDate = GetDate(),";
                strSQL += "UpdatedBy = '" + Globalitems.strUserName + "' ";

                // Add WHERE clause
                strSQL += " WHERE AEVesselID = " + txtVesselID.Text;
                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForModifiedRecord", ex.Message);
                return "";
            }
        }

        private string SQLForNewRecord()
        {
            string strSQL;
            string strval;

            try
            {
                strSQL = "INSERT INTO AEVessel (VesselName,VesselShortName,Notes," +
                    "RecordStatus,CreationDate,CreatedBy,LloydsCode) VALUES (";

                //VesselName
                strval = Globalitems.HandleSingleQuoteForSQL(txtVesselName_record.Text.Trim());
                strSQL += "'" + strval + "',";

                //VesselShortName
                if (txtVesselShortName_record.Text.Trim().Length > 0)
                {
                    strval =
                        Globalitems.HandleSingleQuoteForSQL(txtVesselShortName_record.Text.Trim());
                    strSQL += "'" + strval + "',";
                }
                else
                    strSQL += "NULL,";

                //Notes
                if (txtNote_record.Text.Trim().Length > 0)
                {                    strval =
                        Globalitems.HandleSingleQuoteForSQL(txtNote_record.Text.Trim());
                    strSQL += "'" + strval + "',";
                }
                else
                    strSQL += "NULL,";

                //RecordStatus
                strval = (cboStatus_record.SelectedItem as ComboboxItem).cboValue.ToString();
                if (strval == "select")
                    strSQL += "NULL,";
                else
                    strSQL += "'" + Globalitems.HandleSingleQuoteForSQL(strval) + "',";

                //CreationDate, CreatedBy
                strSQL += "GetDate(),'" + Globalitems.strUserName + "',";

                //Lloyds Code
                if (txtLloydsCode.Text.Trim().Length > 0)
                {
                    strval =
                        Globalitems.HandleSingleQuoteForSQL(txtLloydsCode.Text.Trim());
                    strSQL += "'" + strval + "')";
                }
                else
                    strSQL += "NULL)";

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForNewRecord", ex.Message);
                return "";
            }
        }

        private bool ValidRecord()
        {
            DataSet ds;
            string strSQL;
            string strVesselName;
            string strVesselShortName;

            try
            {
                //Ck Vessel Name
                if (txtVesselName_record.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter the Vessel Name",
                        "MISSING VESSEL NAME", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtVesselName.Focus();
                    return false;
                }

                //For NEW records, make sure Vessel Name & Shortname (if entered) are both unique                
                strVesselName =
                Globalitems.HandleSingleQuoteForSQL(txtVesselName_record.Text.Trim());

                strVesselShortName =
                Globalitems.HandleSingleQuoteForSQL(txtVesselShortName_record.Text.Trim());
                if (strVesselShortName.Length == 0) strVesselShortName = "~~";

                strSQL = "SELECT COUNT(AEVesselID) AS totrec FROM AEVessel " +
                    "WHERE (RTRIM(VesselName) IN ('" + strVesselName + "','" +
                    strVesselShortName + "') OR RTRIM(VesselShortName) IN ('" +
                    strVesselName + "','" + strVesselShortName + "'))";

                if (strMode == "MODIFY") strSQL += " AND AEVesselID <> " + txtVesselID.Text;

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ValidRecord",
                        "No data returned from query");
                    return false;
                }
                
                if (Convert.ToInt16(ds.Tables[0].Rows[0]["totrec"]) > 0)
                {
                    MessageBox.Show("Another Vessel exists with the same Name or Short Name." +
                        "\n\nThe Vessel Name and Short Name must be unique.",
                        "DUPLICATE VESSEL NAME/SHORT NAME", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtVesselName.Focus();
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidRecord", ex.Message);
                return false;
            }
        }

        private void btnCancel_Clicked()
        {CancelSetup();}

        private void btnPrev_Clicked()
        {PerformMovePrevious();}

        private void btnNext_Clicked()
        {PerformMoveNext();}

        private void btnDelete_Clicked()
        {PerformDeleteRecord();}

        private void btnNew_Clicked()
        {NewRecordSetup();}

        private void btnModify_Clicked()
        { ModifyRecordSetup(); }

        private void btnSave_Clicked()
        {PerformSaveRecord();}

        private void btnSearch_Click(object sender, EventArgs e)
        {PerformSearch();}

        private void dgResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            //As long as row clicked is not the Column Header row, index = -1, 
            //change the binding source
            if (e.RowIndex > -1) FilterBindingSource();
        }

        private void txtFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            { Globalitems.CheckDateKeyPress(e); }
        }

        private void txtTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            { Globalitems.CheckDateKeyPress(e); }
        }

        private void txtFrom_Validating(object sender, CancelEventArgs e)
        {
            { Globalitems.ValidateDate(txtFrom, e); }
        }

        private void txtTo_Validating(object sender, CancelEventArgs e)
        {
            { Globalitems.ValidateDate(txtTo, e); }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {OpenCSVFile();}

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void frmVesselAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (strMode != "READ" && !Globalitems.blnException)
            {
                MessageBox.Show("You must SAVE or Cancel the current changes to close this form",
                   "CANNOT CLOSE THIS FORM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void txtVesselName_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtVesselName_record", lsControls);
        }

        private void txtVesselShortName_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtVesselShortName_record", lsControls);
        }

        private void txtLloydsCode_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtLloydsCode", lsControls);
        }

        private void cboStatus_record_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboStatus_record", lsControls);
        }

        private void txtNote_record_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtNote_record", lsControls);
        }

        private void frmVesselAdmin_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
