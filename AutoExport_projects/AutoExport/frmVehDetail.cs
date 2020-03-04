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
    public partial class frmVehDetail : Form
    {
        private const string CURRENTMODULE = "frmVehDetail";

        //Binding Source set by frmVehSearch
        public BindingSource bs1 = new BindingSource();

        public frmVehSearch frmSearch;
        
        //strMode set by frmVehSearch
        public string strMode;

        //Damage info set by frmDamageCodes
        public bool blnNewDamageCodeInfo = false;
        public DataTable dtNewDamageCodes = new DataTable();

        //Private variables
        private bool blnChangeStarted = false;
        private DataRow drow;
        private DataTable dtOriginalDamageCodes = new DataTable();
        private string strPrevVIN;

        private List<string> lsExcludes = new List<string>
            {
                {"txtNote"},
                {"txtSpecialInstr"},
                {"txtBillToNote"}
            };

        private List<ControlInfo> lsControls = new List<ControlInfo>()
        {
            new ControlInfo {ControlID="cboCust",ControlPropetyToBind="SelectedValue",RecordFieldName="CustomerID"},
            new ControlInfo {ControlID="cboBillTo",ControlPropetyToBind="SelectedValue",RecordFieldName="BillToCustomerID"},
            new ControlInfo {ControlID="ckBillTo", RecordFieldName="BillToInd",
                ControlPropetyToBind="Checked"},
            new ControlInfo {ControlID="txtCustNo",RecordFieldName="CustomerCode",ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="txtVIN",RecordFieldName="VIN",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="ckDecoded", RecordFieldName="VINDecodedInd",
                ControlPropetyToBind="Checked"},
            new ControlInfo {ControlID="txtBayLoc",RecordFieldName="BayLocation",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboForwarder",RecordFieldName="FreightForwarderID",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="btnNewForwarder"},
            new ControlInfo {ControlID="cboExporter",RecordFieldName="ExporterID",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="btnNewExporter"},
            new ControlInfo {ControlID="cboDest",RecordFieldName="DestinationName",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="btnNewDestination"},
            new ControlInfo {ControlID="txtVessel",RecordFieldName="vessel",ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="cboTShipPort",RecordFieldName="TransshipPortName",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="btnNewT_ShipPort"},
            new ControlInfo {ControlID="txtPortReceipt",RecordFieldName="PortReceiptNumber",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtYear",RecordFieldName="VehicleYear",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtMake",RecordFieldName="Make",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtModel",RecordFieldName="Model",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtBodystyle",RecordFieldName="Bodystyle",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboVehStatus",RecordFieldName="VehicleStatus",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtCustIdent",RecordFieldName="CustomerIdentification",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtLength",RecordFieldName="VehicleLength",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtColor",RecordFieldName="Color",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtWidth",RecordFieldName="VehicleWidth",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtReceivedBy",RecordFieldName="ReceivedBy",ControlPropetyToBind="Text", ReadOnly=true},
            new ControlInfo {ControlID="txtHeight",RecordFieldName="VehicleHeight",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDateReceived",RecordFieldName="DateReceived",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtWeight",RecordFieldName="VehicleWeight",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDateReceivedException",RecordFieldName="ReceivedExceptionDate",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtCubicFeet",RecordFieldName="VehicleCubicFeet",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtVoyageChangeHold",RecordFieldName="VoyageChangeHoldDate",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboSize",RecordFieldName="SizeClass",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtDateSubToCustoms",RecordFieldName="DateSubmittedCustoms",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtBookingNumber",RecordFieldName="BookingNumber",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDateCustomsException",RecordFieldName="CustomsExceptionDate",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtVoyageNumber",RecordFieldName="VoyageNumber",ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="txtDateCustomsApproval",RecordFieldName="CustomsApprovedDate",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtVIVTagNumber",RecordFieldName="VIVTagNumber",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtDateShipped",RecordFieldName="DateShipped",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtITNNumber",RecordFieldName="ITNNumber",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="ckEntryRateOverride", ControlPropetyToBind="Checked",
                RecordFieldName ="EntryRateOverrideInd" },
            new ControlInfo {ControlID="txtEntryRate",RecordFieldName="EntryRate",ControlPropetyToBind="Text", ReadOnly=true},
            new ControlInfo {ControlID="txtLastPhysicalBy",RecordFieldName="LastPhysicalBy",ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="ckPerDiemGraceOverride", ControlPropetyToBind="Checked",
                RecordFieldName ="PerDiemGraceDaysOverrideInd" },
            new ControlInfo {ControlID="txtPerDiemGraceDays",RecordFieldName="PerDiemGraceDays",ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="txtPerDiemTotalCharge",RecordFieldName="PerDiemTotal",ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="txtLastPhysicalDate",RecordFieldName="LastPhysicalDate",ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="txtTotalCharge",RecordFieldName="TotalCharge",ControlPropetyToBind="Text", ReadOnly=true},
            new ControlInfo {ControlID="ckAudio", ControlPropetyToBind="Checked",
                RecordFieldName ="HasAudioSystemInd" },
            new ControlInfo {ControlID="ckNav", ControlPropetyToBind="Checked",
                RecordFieldName ="HasNavigationSystemInd" },
            new ControlInfo {ControlID="txtInvoiceNumber",RecordFieldName="InvoiceNumber",ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="ckPushcar", ControlPropetyToBind="Checked",
                RecordFieldName ="NoStartInd" },
            new ControlInfo {ControlID="ckMechException", ControlPropetyToBind="Checked",
                RecordFieldName ="MechanicalExceptionInd" },
            new ControlInfo {ControlID="txtInvoiceDate",RecordFieldName="InvoiceDate",ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="txtNote",RecordFieldName="Note",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtSpecialInstr",RecordFieldName="SpecialInstructions",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtBillToNote",RecordFieldName="BillToNote",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtInternalID",RecordFieldName="AutoportExportVehiclesID",ControlPropetyToBind="Text",ReadOnly=true},
            new ControlInfo {ControlID="cboRecordStatus",RecordFieldName="RecordStatus",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtCreationDate",RecordFieldName="CreationDate",ControlPropetyToBind="Text", ReadOnly=true},
            new ControlInfo {ControlID="txtUpdatedDate",RecordFieldName="UpdatedDate",ControlPropetyToBind="Text", ReadOnly=true},
            new ControlInfo {ControlID="txtCreatedBy",RecordFieldName="CreatedBy",ControlPropetyToBind="Text", ReadOnly=true },
            new ControlInfo {ControlID="txtUpdatedBy",RecordFieldName="UpdatedBy",ControlPropetyToBind="Text", ReadOnly=true}  
        };

        public frmVehDetail()
        {
            InitializeComponent();

            dgStatus.AutoGenerateColumns = false;
            dgInspectionsDamage.AutoGenerateColumns = false;
            dgLocation.AutoGenerateColumns = false;

            Globalitems.SetControlHeight(this,lsExcludes);

            FillCombos();

            // Assign methods to the recbuttons public event variables
            recbuttons.CancelRecord += btnCancel_Clicked;
            recbuttons.MovePrev += btnPrev_Clicked;
            recbuttons.MoveNext += btnNext_Clicked;
            recbuttons.DeleteRecord += btnDelete_Clicked;
            recbuttons.NewRecord += btnNew_Clicked;
            recbuttons.ModifyRecord += btnModify_Clicked;
            recbuttons.SaveRecord += btnSave_Clicked;

            //Assign Global DATE_TOOLTIP for Start/End date controls
            tipDate.SetToolTip(txtCreationDate, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtDateCustomsApproval, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtDateCustomsException, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtDateReceived, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtDateReceivedException, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtDateShipped, Globalitems.DATE_TOOLTIP);
            tipDate.SetToolTip(txtDateSubToCustoms, Globalitems.DATE_TOOLTIP);

            //Hide NEW btns if User not Administrator
            if (!Globalitems.strRoleNames.Contains("Administrator"))
            {
                btnNewCustomer.Visible = false;
                btnNewDestination.Visible = false;
                btnNewExporter.Visible = false;
                btnNewForwarder.Visible = false;
            }
        }

        private void frmVehDetail_Activated(object sender, EventArgs e)
        {
            if (strMode == "READ")
            {
                lblStatus.Text = "Read Only";
                recbuttons.blnRecordsToDisplay = false;

                if (bs1 != null && bs1.Count > 0)
                {
                    //Set recbuttons
                    recbuttons.blnRecordsToDisplay = true;

                    AdjustReadOnlyStatus(true);
                    PerformMove();
                }

                return;              
            }

            if (strMode == "NEW" && !blnChangeStarted)
            {
                btnNew_Clicked();                
                return;
            }
        }

        private void AdjustReadOnlyStatus(bool blnReadOnly)
        {
            Control[] ctrls;
            TextBox txtBox;

            try
            {
                //Only enable buttons if in READ mode. Otherwise, User has to Add/Modify 
                //  record details
                btnMenu.Enabled = blnReadOnly;
                btnNewCustomer.Enabled = blnReadOnly;
                btnNewDestination.Enabled = blnReadOnly;
                btnNewExporter.Enabled = blnReadOnly;
                btnNewForwarder.Enabled = blnReadOnly;

                //Enable AddDamageCode when not in READ mode
                btnAddDamageCode.Enabled = !blnReadOnly;

                Formops.SetReadOnlyStatus(this, lsControls, blnReadOnly, recbuttons);

                //Disable cboBillTo/txtBillTo if not Admin
                if (!Globalitems.blnAdmin)
                {
                    txtBillToNote.Enabled = false;
                    cboBillTo.Enabled = false;
                }

                //Disable cboBillTo/txtBillTo if Customer is Glovis
                if (strMode == "MODIFY" && 
                    (cboCust.SelectedItem as ComboboxItem).cboText == "LGL")
                {
                    txtBillToNote.Enabled = false;
                    cboBillTo.Enabled = false;
                }

                //Set Enable=true for items in lsExcludes, so User can use scrollbars
                if (blnReadOnly)
                {
                    foreach (string strItem in lsExcludes)
                    {
                        //Place the control into the array ctrls, s/b only one
                        ctrls = this.Controls.Find(strItem, true);

                        txtBox = (TextBox)ctrls[0];
                        txtBox.Enabled = true;
                    }
                }                
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "AdjustReadOnlyStatus", ex.Message);
            }
        }

        private void ClearForm()
        {
            try
            {
                //Clear all items in lsControls
                Formops.ClearSetup(this, lsControls);

                //Clear all DataGridViews
                dgInspectionsDamage.DataSource = null;
                dgLocation.DataSource = null;
                dgStatus.DataSource = null;
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ClearForm", ex.Message);
            }
        }

        private void FillDetailRecord()
        {
            try
            {
                ClearForm();

                drow = GetDetailRow();

                Formops.SetDetailRecord(drow, this, lsControls);

                //Enable btnCurrAmt if not billed yet
                btnCurrAmt.Enabled = false;
                if (Convert.ToDecimal (txtTotalCharge.Text) == 0) btnCurrAmt.Enabled = true;

                //Display Push Car if ckPushcar checked
                lblPushCar.Visible = false;
                if (ckPushcar.Checked) lblPushCar.Visible = true;

                //Format Currency values
                txtEntryRate.Text = Globalitems.FormatCurrency(txtEntryRate.Text);
                txtTotalCharge.Text = Globalitems.FormatCurrency(txtTotalCharge.Text);
                txtPerDiemTotalCharge.Text = Globalitems.FormatCurrency(txtPerDiemTotalCharge.Text);

                //Format Date values
                txtDateReceived.Text = Globalitems.FormatDatetime(txtDateReceived.Text);
                txtCreationDate.Text = Globalitems.FormatDatetime(txtCreationDate.Text);
                txtDateReceivedException.Text = Globalitems.FormatDatetime(txtDateReceivedException.Text);
                txtVoyageChangeHold.Text = Globalitems.FormatDatetime(txtVoyageChangeHold.Text);
                txtDateSubToCustoms.Text = Globalitems.FormatDatetime(txtDateSubToCustoms.Text);
                txtDateCustomsException.Text = Globalitems.FormatDatetime(txtDateCustomsException.Text);
                txtDateCustomsApproval.Text = Globalitems.FormatDatetime(txtDateCustomsApproval.Text);
                txtDateShipped.Text = Globalitems.FormatDatetime(txtDateShipped.Text);
                txtLastPhysicalDate.Text = Globalitems.FormatDatetime(txtLastPhysicalDate.Text);
                txtCreationDate.Text = Globalitems.FormatDatetime(txtCreationDate.Text);
                txtUpdatedDate.Text = Globalitems.FormatDatetime(txtUpdatedDate.Text);

                FillStatusGrid();
                FillInspectionsDamageGrid();
                FillLocationHistoryGrid();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillDetailRecord", ex.Message);
            }
        }

        private void FillInspectionsDamageGrid()
        {
            DataSet ds;
            string strSQL;

            try
            {
                dgInspectionsDamage.DataSource = null;
                dtOriginalDamageCodes.Clear();
                dtNewDamageCodes.Clear();

                //Retrieve all columns needed by both dgInspectionDamage and insertion of new
                //  Inspections, Damage Codes
                strSQL = "SELECT ins.AutoportExportVehiclesID," +
                    "ins.AEVehicleInspectionID," +
                    "dam.AEVehicleDamageDetailID," + 
                    "CAST(Code.Code AS int) AS InspectionType," +
                    "Code.CodeDescription AS Inspectiontype_desc," +
                    "ins.InspectionDate," +
                    "ins.InspectedBy," +
                    "DamageCodeCount," +
                    "CASE " +
                    "WHEN LEN(RTRIM(ISNULL(ins.Notes,''))) = 0 THEN '' " +
                    "ELSE 'VIEW' " +
                    "END AS Note, " +
                    "RTRIM(ISNULL(ins.Notes,'')) AS FullNote, " + 
                    "ins.CreationDate, " +
                    "dam.DamageCode, " +
                    "dam.DamageDescription " +
                    "FROM AEVehicleInspection ins " +
                    "INNER JOIN Code ON Code.CodeType = 'InspectionType' AND " + 
                        "Code.Code = ins.InspectionType " +
                    "LEFT OUTER JOIN AEVehicleDamageDetail dam ON " +
                        "ins.AEVehicleInspectionID = dam.AEVehicleInspectionID " +
                    "LEFT OUTER JOIN AutoportExportVehicles veh ON " +
                        "ins.AutoportExportVehiclesID = veh.AutoportExportVehiclesID " +
                    "WHERE veh.VIN = '" + txtVIN.Text + "' " +
                    "ORDER BY ins.InspectionDate, ins.AEVehicleInspectionID";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds == null || ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillInspectionGrid", "No data " +
                        "returned from query");
                    return;
                }

                dtOriginalDamageCodes = ds.Tables[0];
                dtNewDamageCodes = dtOriginalDamageCodes.Clone();

                if (ds.Tables[0].Rows.Count > 0)
                dgInspectionsDamage.DataSource = dtOriginalDamageCodes;
            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillInspectionGrid", ex.Message);
            }
        }

        private void FillLocationHistoryGrid()
        {
            string strSQL;
            DataSet ds;

            try
            {
                dgLocation.DataSource = null;

                strSQL = "SELECT BayLocation,CreationDate,CreatedBy " +
                    "FROM VehicleLocationHistory " +
                    "WHERE " +
                    "VehicleID = " + txtInternalID.Text +
                    " ORDER BY CreationDate DESC";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds == null || ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillLocationHistoryGrid", "No data " +
                        "returned from query");
                    return;
                }

                if (ds.Tables[0].Rows.Count > 0) dgLocation.DataSource = ds.Tables[0];
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillLocationHistoryGrid", ex.Message);
            }
        }

        private void FillStatusGrid()
        {
            string strSQL;
            DataSet ds;

            try
            {
                dgStatus.DataSource = null;

                strSQL = "SELECT VehicleStatus, CreationDate, CreatedBy " +
                    "FROM AEVehicleStatusHistory " +
                    "WHERE " +
                    "AutoportExportVehiclesID = " + txtInternalID.Text +
                    " ORDER BY CreationDate DESC";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds == null || ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillStatusGrid", "No data " +
                        "returned from query");
                    return;
                }

                if (ds.Tables[0].Rows.Count > 0) dgStatus.DataSource = ds.Tables[0];
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillStatusGrid", ex.Message);
            }
        }

        private DataRow GetDetailRow()
        {
            try
            {
                DataColumn col;
                DataSet ds;
                DataTable dt;
                int intPosition = bs1.Position;
                int intVehID = (int)bs1[intPosition];
                string strSQL;

                strSQL = "SELECT " +
                "veh.AutoportExportVehiclesID," +
                "veh.CustomerID," +
                "veh.BillToInd," +
                "veh.BillToCustomerID," +
                "veh.BillToNote," +
                "cus.CustomerCode," +
                "veh.VIN," +
                "ISNULL(veh.VINDecodedInd,0) AS VinDecodedInd," +
                "veh.BayLocation," +
                "veh.FreightForwarderID," +
                "veh.ExporterID," +
                "veh.DestinationName, " +
                "CASE " +
                "WHEN LEN(RTRIM(ISNULL(ves.VesselShortName, ''))) > 0 THEN RTRIM(ves.VesselShortName) " +
                    "ELSE RTRIM(ves.VesselName) " +
                "END AS vessel," +
                "ves.AEVesselID," +
                "veh.TransshipPortName," +
                "veh.PortReceiptNumber," +
                "veh.VehicleYear," +
                "veh.Make," +
                "veh.Model," +
                "veh.Bodystyle," +
                "veh.VehicleStatus," +
                "veh.CustomerIdentification," +
                "veh.VehicleLength," +
                "veh.Color," +
                "veh.VehicleWidth," +
                "veh.ReceivedBy," +
                "veh.VehicleHeight," +
                "veh.DateReceived," +
                "veh.VehicleWeight," +
                "veh.ReceivedExceptionDate," +
                "veh.VehicleCubicFeet," +
                "VoyageChangeHoldDate," +
                "veh.SizeClass," +
                "veh.DateSubmittedCustoms," +
                "veh.BookingNumber," +
                "veh.CustomsExceptionDate," +
                "voy.VoyageNumber," +
                "veh.CustomsApprovedDate," +
                "veh.VIVTagNumber," +
                "veh.DateShipped," +
                "veh.ITNNumber," +
                "ISNULL(veh.EntryRateOverrideInd,0) AS EntryRateOverrideInd," +
                "veh.EntryRate," +
                "veh.LastPhysicalBy," +
                "veh.PerDiemGraceDaysOverrideInd," +
                "veh.PerDiemGraceDays," +
                "veh.LastPhysicalDate," +
                "veh.TotalCharge," +
                "veh.HasAudioSystemInd," +
                "veh.HasNavigationSystemInd," +
                "veh.BillingID," +
                "bill.InvoiceNumber," +
                "bill.InvoiceDate," +
                "veh.NoStartInd," +
                "veh.MechanicalExceptionInd," +
                "RTRIM(ISNULL(veh.Note,'')) AS Note," +
                "RTRIM(ISNULL(veh.SpecialInstructions,'')) AS SpecialInstructions," +
                "veh.RecordStatus," +
                "veh.CreationDate," +
                "veh.CreatedBy," +
                "veh.UpdatedDate," +
                "veh.UpdatedBy," +
                "ISNULL(veh.BilledInd,0) AS BilledInd " +
                "FROM AutoportExportVehicles veh " +
                "LEFT OUTER JOIN Customer cus on cus.CustomerID=veh.customerID " +
                "LEFT OUTER JOIN AEVoyage voy on voy.AEVoyageID=veh.VoyageID " +
                "LEFT OUTER JOIN AEVessel ves on ves.AEVesselID = voy.AEVesselID " +
                "LEFT OUTER JOIN Billing bill on bill.BillingID = veh.BillingID " +
                "WHERE veh.AutoportExportVehiclesID = " + intVehID.ToString();

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GetDetailRow",
                        "No data returned for veh. ID");
                    return null;
                }

                dt = ds.Tables[0];

                //Add a column for PerDiemTotal
                col = new DataColumn();
                col.ColumnName = "PerDiemTotal";
                col.DataType = typeof(System.Int32);
                dt.Columns.Add(col);

                //Get TotalPerDiem from 
                strSQL = "SELECT SUM(PerDiem) AS totcharge from AutoportExportPerDiem " +
                    "WHERE AutoportExportVehiclesID=" + intVehID.ToString();
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GetDetailRow",
                        "No data returned for SUM(PerDiem)");
                    return null;
                }

                if (ds.Tables[0].Rows[0]["totcharge"] == DBNull.Value)
                    dt.Rows[0]["PerDiemTotal"] = 0;
                else
                    dt.Rows[0]["PerDiemTotal"] = ds.Tables[0].Rows[0]["totcharge"];

                return dt.Rows[0];
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetDetailRow", ex.Message);
                return null;
            }
        }

        private void FillBillToCombo()
        {
            ComboboxItem cboitem;
            List<ComboboxItem> lsBillTo;
            string strCustomerID = (cboCust.SelectedItem as ComboboxItem).cboValue;
            string strFilter;

            try
            {
                cboBillTo.DataSource = null;
                cboBillTo.Enabled = false;

                //Fill cboBillto from Code table for customer in cboCust, use list as cboBillTo source
                strFilter = $"CodeType='BillToCustomer' AND Value1 = '{strCustomerID}' ";
                if (ckActive.Checked) strFilter += "AND RecordStatus='Active'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboBillTo, false, true);

                if (cboBillTo.Items.Count == 0) return;

                lsBillTo = new List<ComboboxItem>();

                //Create 1st item as <none>
                cboitem = new ComboboxItem();
                cboitem.cboText = "<none>";
                cboitem.cboValue = "none";
                lsBillTo.Add(cboitem);

                //Add items put into cboBillTo from Code table
                lsBillTo.AddRange(cboBillTo.DataSource as List<ComboboxItem>);

                cboBillTo.DataSource = lsBillTo;

                cboBillTo.DisplayMember = "cboText";
                cboBillTo.ValueMember = "cboValue";
                cboBillTo.SelectedIndex = 0;
                cboBillTo.Enabled = true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillBillToCombo", ex.Message);
            }            
        }

        private void FillCombos()
        {
            ComboboxItem cboitem;
            DataSet ds;
            string strFilter;
            string strSQL;

            try
            {
                //cboExporter
                // Add Select to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboExporter.Items.Add(cboitem);

                cboExporter.DisplayMember = "cboText";
                cboExporter.ValueMember = "cboValue";
                cboExporter.SelectedIndex = 0;

                //cboForwarder
                // Add Select to cbo
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cboForwarder.Items.Add(cboitem);

                cboForwarder.DisplayMember = "cboText";
                cboForwarder.ValueMember = "cboValue";
                cboForwarder.SelectedIndex = 0;

                //cboCust
                strSQL = @"SELECT CustomerID, 
                    CASE WHEN LEN(RTRIM(ISNULL(ShortName,''))) > 0 
                        THEN RTRIM(ShortName) 
                        ELSE RTRIM(CustomerName) 
                    END AS CustName 
                    FROM Customer ";

                if (ckActive.Checked) strSQL += "WHERE RecordStatus='Active' ";
                    
                strSQL += "ORDER BY CustName";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillCombos",
                        "No rows returned from Customer table");
                    return;
                }

                // Add Select to cbo
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

                //cboDest/cboTShipPort
                strFilter = "CodeType='ExportDischargePort' AND Code <> '' ";
                if (ckActive.Checked) strFilter += "AND RecordStatus='Active'";

                Globalitems.FillComboboxFromCodeTable(strFilter, cboDest,true, false);
                
                //Replace 'All; in 1st item with <select>
                cboitem = (ComboboxItem) cboDest.Items[0];
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";

                cboDest.DisplayMember = "cboText";
                cboDest.ValueMember = "cboValue";
                cboDest.SelectedIndex = 0;

                Globalitems.FillComboboxFromCodeTable(strFilter, cboTShipPort, true, false);

                //Replace 'All; in 1st item with <select>
                cboitem = (ComboboxItem)cboTShipPort.Items[0];
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                
                cboTShipPort.DisplayMember = "cboText";
                cboTShipPort.ValueMember = "cboValue";
                cboTShipPort.SelectedIndex = 0;

                //cboVehStatus    
                strFilter = "CodeType='AutoportExportVehicleStatus' AND Code <> '' ";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboVehStatus, true, false);

                //Replace 'All; in 1st item with <select>
                cboitem = (ComboboxItem)cboVehStatus.Items[0];
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";

                cboVehStatus.DisplayMember = "cboText";
                cboVehStatus.ValueMember = "cboValue";
                cboVehStatus.SelectedIndex = 0;

                //cboSize 
                strFilter = "CodeType='SizeClass' AND Code <> '' ";
                if (ckActive.Checked) strFilter += "AND RecordStatus='Active'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboSize, true, false);

                //Replace 'All; in 1st item with <select>
                cboitem = (ComboboxItem)cboSize.Items[0];
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";

                cboSize.DisplayMember = "cboText";
                cboSize.ValueMember = "cboValue";
                cboSize.SelectedIndex = 0;

                //cboRecordStatus
                strFilter = "CodeType = 'RecordStatus'";
                Globalitems.FillComboboxFromCodeTable(strFilter, cboRecordStatus, true, false);

                //Replace 'All; in 1st item with <select>
                cboitem = (ComboboxItem)cboRecordStatus.Items[0];
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";

                cboRecordStatus.DisplayMember = "cboText";
                cboRecordStatus.ValueMember = "cboValue";
                cboRecordStatus.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCBOs", ex.Message);
            }
        }

        private void CancelSetup()
        {
            blnChangeStarted = false;

            strMode = "READ";
            lblStatus.Text = "Read Only";
            btnMenu.Enabled = true;
            btnVehLocator.Enabled = true;

            ClearForm();

            //Display other open forms
            Globalitems.DisplayOtherForms(this, true);

            //Turn on ReadOnly on form's controls
            AdjustReadOnlyStatus(true);
        }

        private void ModifyRecordSetup()
        {
            try
            {
                blnChangeStarted = true;

                lblStatus.Text = "Modify Current record";
                strMode = "MODIFY";

                btnAddDamageCode.Enabled = true;

                btnMenu.Enabled = false;
                btnVehLocator.Enabled = false;
                btnCurrAmt.Enabled = false;

                //Hide other open forms
                Globalitems.DisplayOtherForms(this, false);

                //Reset form's controls to Updated = false
                Formops.ResetControls(this, lsControls);

                AdjustReadOnlyStatus(false);

                //Set recbuttons to New
                recbuttons.SetButtons(RecordButtons.ACTION_MODIFYRECORD);

                //Set Updated By/Date to new value
                txtUpdatedBy.Text = Globalitems.strUserName;
                txtUpdatedDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ModifyRecordSetup", ex.Message);
            }
            
        }

        private void NewRecordSetup()
        {
            try
            {
                blnChangeStarted = true;

                //Set Status labels
                lblStatus.Text = "Add New record";
                lblPushCar.Visible = false;
                strMode = "NEW";
                cboRecordStatus.SelectedValue = "active";

                btnMenu.Enabled = false;
                btnVehLocator.Enabled = false;
                btnCurrAmt.Enabled = false;

                ClearForm();

                //Hide other open forms
                Globalitems.DisplayOtherForms(this, false);

                //Turn off ReadOnly on form's controls
                AdjustReadOnlyStatus(false);

                //Set Record Status to Active
                foreach (ComboboxItem cboitem in cboRecordStatus.Items)
                    if (cboitem.cboValue.ToString() == "Active")
                        cboRecordStatus.SelectedItem = cboitem;

                //Set Vehicle Status to Received
                foreach (ComboboxItem cboitem in cboVehStatus.Items)
                    if (cboitem.cboValue.ToString() == "Received")
                        cboVehStatus.SelectedItem = cboitem;

                //Set txtInternalID to 0
                txtInternalID.Text = "0";

                //Set recbuttons to New
                recbuttons.SetButtons(RecordButtons.ACTION_NEWRECORD);

                //Set Created By/Date to new value
                txtCreatedBy.Text = Globalitems.strUserName;
                txtCreationDate.Text = DateTime.Now.ToString("M/d/yyyy h:mm tt");

                txtDateReceived.Text = txtCreationDate.Text;
                txtReceivedBy.Text = txtCreatedBy.Text;

                //Set txtPerDiemTotalCharge,txtTotalCharge to $0.00
                txtPerDiemTotalCharge.Text = "$0.00";
                txtTotalCharge.Text = "$0.00";

                //Run FillInspectionsDamageGrid to create a blank dtNewDamageCodes
                FillInspectionsDamageGrid();
                
                //. Set focus on first record detail control
                cboCust.Focus();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "NewRecordSetup", ex.Message);
            }
            
        }

        private void OpenDamageForm()
        {
            bool blnRecords = false;
            DialogResult dlResult;
            frmDamageCodes frm;

            try
            {
                blnNewDamageCodeInfo = false;
                if (dgInspectionsDamage.RowCount > 0) blnRecords = true;
                frm = new frmDamageCodes(this,blnRecords,Convert.ToInt32(txtInternalID.Text), 
                    txtVIN.Text);
                dlResult = frm.ShowDialog();

                //Update dgInspectionsDamage if new DamageInfo
                if (blnNewDamageCodeInfo) UpdateDamageCodeInfo();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenDamageForm", ex.Message);
            }
        }

        private void PerformDeleteRecord()
        {
            DialogResult dlResult;
            frmAreYouSure frmConfirm;
            string strMessage;
            string strSQL;

            try
            {
                strMessage = "Are you sure you want to delete this Vehicle?";
                frmConfirm = new frmAreYouSure(strMessage);
                dlResult = frmConfirm.ShowDialog();

                if (dlResult == DialogResult.OK)
                {
                    strMode = "DELETE";

                    //Delete Veh Record. See DeleteToAutoportExportVehicles trigger on
                    //  AutoportExportVehicles table to see other tables that also delete records
                    strSQL = "DELETE AutoportExportVehicles " +
                        "WHERE AutoportExportVehiclesID = " + txtInternalID.Text;
                    DataOps.PerformDBOperation(strSQL);

                    //Create Action History Record
                    strSQL = "INSERT INTO ActionHistory (RecordID, RecordTableName," +
                        "ActionType, Comments, CreationDate, CreatedBy) " +
                        "VALUES(" + txtInternalID.Text + "," +
                        "'AutoportExportVehicles'," +
                        "'Autoport Export Vehicle Deleted'," +
                        "'VIN: " + txtVIN.Text + " Deleted from Autoport Export Table " +
                        "for Customer: " +
                        (cboCust.SelectedItem as ComboboxItem).cboText.ToString().Trim() + "'," +
                        "GetDate(),'" + Globalitems.strUserName + "')";
                    DataOps.PerformDBOperation(strSQL);

                    //Deletion was successful rerun Search in frmVehSearch
                    frmSearch.objAddlCriteria.RerunSearch = true;

                    MessageBox.Show("The Vehicle is deleted.\n\n" + 
                        "In the Veh. Locator form click the Search button for an updated list", 
                        "VEHICLE DELETED",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Change mode to allow form to close
                    strMode = "READ";

                    // Close if only 1 record in bs1
                    if (bs1.Count == 1)
                    {
                        this.Close();
                        return;
                    }

                    //Remove current record from bs1
                    bs1.RemoveCurrent();

                    //Display 1st Record
                    bs1.MoveFirst();
                    FillDetailRecord();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformDeleteRecord", ex.Message);
            }
        }

        private void PerformMove()
        {
            try
            {
                FillDetailRecord();

                Globalitems.SetNavButtons(recbuttons, bs1);

                //Cannot delete or modify record if already billed. 
                if (drow["BilledInd"].ToString() == "0")
                {
                    recbuttons.EnableDeleteButton(true);
                    recbuttons.EnableModifyButton(true);
                }
                else
                {
                    recbuttons.EnableDeleteButton(false);
                    recbuttons.EnableModifyButton(false);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMove", ex.Message);
            }
        }

        private void PerformMoveNext()
        {
            try
            {
                bs1.MoveNext();
                PerformMove();
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
                PerformMove();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformMoveNext", ex.Message);
            }
        }

        private string SQLForNewRecord(DateTime datCreationDate)
        {
            string strSQL = "";
            string strval;

            try
            {
                //Omit fields not relevant to new record
                strSQL = "INSERT INTO AutoportExportVehicles (CustomerID, VehicleYear," +
                            "Make, Model, Bodystyle, VIN, Color, VehicleLength, VehicleWidth," +
                            " VehicleHeight, VehicleWeight, VehicleCubicFeet, VehicleStatus," +
                            "DestinationName, ExporterID," +
                            "FreightForwarderID, BookingNumber, " +
                            "CustomerIdentification, SizeClass, BayLocation, EntryRate," +
                            "EntryRateOverrideInd, PerDiemGraceDays," +
                            "PerDiemGraceDaysOverrideInd, TotalCharge, DateReceived," +
                            "ReceivedExceptionDate, DateSubmittedCustoms," +
                            "CustomsExceptionDate, CustomsApprovedDate, DateShipped," +
                            "Note, RecordStatus, CreationDate, CreatedBy," +
                            "NoStartInd, " +
                            "HasAudioSystemInd, HasNavigationSystemInd," +
                            "TransshipPortName, SpecialInstructions," +
                            "PortReceiptNumber," +
                            "VoyageChangeHoldDate, ReceivedBy," +
                            "VIVTagNumber," +
                            "MechanicalExceptionInd,LeftBehindInd," +
                            "ITNNumber,BillToInd,BillToCustomerID,BillToNote) " +
                            "VALUES (";

                //CustomerID
                strval = (cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strval == "select")
                    strSQL += "NULL";
                else
                    strSQL += strval;

                strSQL += ",";

                //VehicleYear
                if (txtYear.Text.Trim().Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + txtYear.Text + "'";

                strSQL += ",";

                //Make
                strval = txtMake.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //Model
                strval = txtModel.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //Bodystyle
                strval = txtBodystyle.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //VIN
                strval = txtVIN.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //Color
                strval = txtColor.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //VehicleLength
                strval = txtLength.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //VehicleWidth
                strval = txtWidth.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //VehicleHeight
                strval = txtHeight.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //VehicleWeight
                strval = txtWeight.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //VehicleCubicFeet
                strval = txtCubicFeet.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //VehicleStatus
                strval = (cboVehStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval == "select")
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //DestinationName
                strval = (cboDest.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval == "select")
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //ExporterID
                strval = (cboExporter.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval == "select")
                    strSQL += "NULL";
                else
                    strSQL += strval;

                strSQL += ",";

                //FreightForwarderID
                strval = (cboForwarder.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval == "select")
                    strSQL += "NULL";
                else
                    strSQL += strval;

                strSQL += ",";

                //BookingNumber
                strval = txtBookingNumber.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL";
                else
                    strSQL += "'" + strval + "'";

                strSQL += ",";

                //CustomerIdentification
                strSQL += "NULL,";

                //SizeClass
                strval = (cboSize.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval == "select")
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strval + "',";

                //BayLocation
                strval = txtBayLoc.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strval + "',";

                //EntryRate
                strval = txtEntryRate.Text.Trim();
                strval = strval.Replace("$", "");
                if (strval.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strval + "',";

                //EntryRateOverrideInd
                if (ckEntryRateOverride.Checked)
                    strSQL += "1,";
                else
                    strSQL += "0,";

                //PerDiemGraceDays
                strval = txtPerDiemGraceDays.Text.Trim();
                if (strval.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strval + "',";

                //PerDiemGraceDaysOverrideInd
                if (ckPerDiemGraceOverride.Checked)
                    strSQL += "1,";
                else
                    strSQL += "0,";

                //TotalCharge
                strval = txtTotalCharge.Text;
                strval = strval.Replace("$", "");
                if (strval.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += strval + ",";

                //DateReceived
                if (txtDateReceived.Text.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + txtDateReceived.Text + "',";

                //ReceivedExceptionDate
                if (txtDateReceivedException.Text.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + txtDateReceivedException.Text + "',";

                //DateSubmittedCustoms
                if (txtDateSubToCustoms.Text.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + txtDateSubToCustoms.Text + "',";

                //CustomsExceptionDate
                if (txtDateCustomsException.Text.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + txtDateCustomsException.Text + "',";

                //CustomsApprovedDate
                if (txtDateCustomsApproval.Text.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + txtDateCustomsApproval.Text + "',";

                //DateShipped
                if (txtDateShipped.Text.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + txtDateShipped.Text + "',";

                //Note
                strval = txtNote.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strval + "',";

                //RecordStatus
                strval = (cboRecordStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval == "select")
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strval + "',";

                //CreationDate
                strSQL += "'" + datCreationDate + "',";

                //CreatedBy
                strval = txtCreatedBy.Text;
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                strSQL += "'" + strval + "',";  

                //NoStartInd
                if (ckPushcar.Checked)
                    strSQL += "1,";
                else
                    strSQL += "0,";

                //HasAudioSystemInd
                if (ckAudio.Checked)
                    strSQL += "1,";
                else
                    strSQL += "0,";

                //HasNavigationSystemInd
                if (ckNav.Checked)
                    strSQL += "1,";
                else
                    strSQL += "0,";

                //TransshipPortName
                strval = (cboTShipPort.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval == "select")
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strval + "',";

                //SpecialInstructions
                strval = txtSpecialInstr.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strval + "',";

                //PortReceiptNumber
                strval = txtPortReceipt.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strSQL + "',";

                //VoyageChangeHoldDate
                strSQL += "NULL,";

                //ReceivedBy
                strSQL += "'" + txtCreatedBy.Text + "',";

                //VIVTagNumber
                strval = txtVIVTagNumber.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strval + "',";

                //MechanicalExceptionInd
                if (ckMechException.Checked)
                    strSQL += "1,";
                else
                    strSQL += "0,";

                //LeftBehindInd
                strSQL += "0,";

                //ITNNumber
                strval = txtITNNumber.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL,";
                else
                    strSQL += "'" + strval + "',";

                //BillToInd,BillToCustomerID
                strval = "";
                if (cboBillTo.SelectedIndex > -1)
                    strval = (cboBillTo.SelectedItem as ComboboxItem).cboValue;
                if (strval.Length == 0 || strval == "none" || strval == "select")
                    strSQL += "0,NULL,";                    
                else
                    strSQL += "1," + strval + ",";

                //BillToNote
                strval = txtBillToNote.Text.Trim();
                strval = Globalitems.HandleSingleQuoteForSQL(strval);
                if (strval.Length == 0)
                    strSQL += "NULL)";
                else
                    strSQL += "'" + strval + "')";

                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForNewRecord", ex.Message);
                return "";
            }
        }

        private string SQLForModifiedRecord(DateTime datUpdateDate)
        {
            //bool blnDynamicSQL = false;
            CheckBox ckBox;
            ComboBox cboBox;
            Control[] ctrls;
            string strSQL = "";
            string strval;

            try
            {
                //if (dtNewDamageCodes.Rows.Count > 0) blnDynamicSQL = true;

                // Use linq to get a list of updated controls, 
                //  For this form, textboxes and comboboxes
                var changedlist = lsControls.Where(ctrlinfo => (ctrlinfo.Updated == true)).ToList();

                strSQL = "UPDATE AutoportExportVehicles SET ";

                foreach (ControlInfo ctrlinfo in changedlist)
                {
                    strSQL += ctrlinfo.RecordFieldName + " = ";

                    //Place the control into the array ctrls, s/b only one
                    ctrls = this.Controls.Find(ctrlinfo.ControlID, true);

                    switch (ctrlinfo.ControlPropetyToBind)
                    {
                        case "Text":
                            strval = ctrls[0].Text.Trim();

                            //EntryRate is Decimal & PerDiemGraceDays is int in table 
                            if (ctrlinfo.ControlID == "txtEntryRate" ||
                                ctrlinfo.ControlID == "txtPerDiemGraceDays")
                            {
                                strval.Replace("$", "");
                                strSQL += strval;
                            }
                            else
                            {
                                // Use HandleSingleQuoteForSQL 
                                //to replace ' in text to '' for SQL
                                strval = Globalitems.HandleSingleQuoteForSQL(strval);

                                if (strval.Length == 0)
                                    strSQL += "NULL"; 
                                else
                                    strSQL += "'" + strval + "'";
                            }

                            break;
                        case "SelectedValue":
                            //Cast control to ComboBox. All cbo's except cboVehStatus,
                            //cboRecordStatus, cboDest have an int value in the
                            //AutoportExportVehicles table
                            cboBox = (ComboBox)ctrls[0];
                            strval = (cboBox.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                            strval = Globalitems.HandleSingleQuoteForSQL(strval);
                            if (strval == "select" || strval == "none")
                                strSQL += "NULL";
                            else
                                //cbo's that store string data, add single quotes (')
                                if (ctrlinfo.ControlID == "cboRecordStatus" ||
                                ctrlinfo.ControlID == "cboSize" ||
                                ctrlinfo.ControlID == "cboVehStatus" ||
                                ctrlinfo.ControlID == "cboDest")
                                    strSQL += "'" + strval + "'";
                                else
                                    strSQL += strval;
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

                //Set VoyageID to NULL if ShippedByTruck status
                if ((cboVehStatus.SelectedItem as ComboboxItem).cboValue.Contains("Truck"))
                    strSQL += "VoyageID = NULL,";

                //Add UpdatedDate & UpdatedBy
                strSQL += "UpdatedDate = '" + datUpdateDate + "',";
                strSQL += "UpdatedBy = '" + Globalitems.strUserName + "' ";

                // Add WHERE clause
                strSQL += " WHERE AutoportExportVehiclesID = " + txtInternalID.Text;
                return strSQL;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SQLForModifiedRecord", ex.Message);
                return "";
            }
        }

        private void PerformSaveRecord()
        {
            DateTime datNow = DateTime.Now;
            DataRow drow;
            DataSet ds;
            DataTable dtDamageInfo;
            int intCurrentBSPosition;
            SProcParameter objParam;
            List<SProcParameter> Paramobjects = new List<SProcParameter>();
            string strDateNow;
            string strResult;
            string strSProc = "spUpdateVehicleInfo";
            string strSQL;
            string strtmptable;
            string strUpdateVehSQL;

            try
            {
                if (ValidRecord())
                {
                    //Get the SQL for the AutoportExportVehicles table based on Mode
                    if (strMode == "NEW")
                    {
                        strUpdateVehSQL = SQLForNewRecord(datNow);
                    }
                    else //MODIFY mode
                    {
                        intCurrentBSPosition = bs1.Position;
                        strUpdateVehSQL = SQLForModifiedRecord(datNow);
                    }

                    if (dtNewDamageCodes.Rows.Count > 0)
                    {
                        //There is new Damage Code info. Need to update three tables:
                        //  Insert new Inspection rows in AEVehicleInspection table
                        //  Insert new Damage Detail rows in AEVehicleDamageDetail table
                        //  Insert or Update a row in AutoportExportVehicles tables

                        //Create a copy of dtNewDamageCodes to save as tmp table in DB
                        dtDamageInfo = dtNewDamageCodes.Copy();

                        //Remove unneeded columns
                        dtDamageInfo.Columns.Remove("InspectionType_desc");
                        dtDamageInfo.Columns.Remove("Note");

                        //Rename col. FullNote -> Notes to match AEVehicleInspection table
                        dtDamageInfo.Columns["FullNote"].ColumnName = "Notes";

                        //Create a unique string based on datetime for tmp table name in SQL DB
                        strDateNow = datNow.ToString("yyyyMMddHHmmss");

                        strtmptable = "tmpVehicleInfoUpdate_" + strDateNow;

                        //Create the table tmpVehicleInfoUpdate_[strDateNow] in SQL DB
                        //  with the same columns as dtDamageInfo
                        strSQL = "CREATE TABLE " + strtmptable +
                            " (AutoportExportVehiclesID int," +
                            "AEVehicleInspectionID int," +
                            "AEVehicleDamageDetailID int," +
                            "InspectionType int," +
                            "InspectionDate datetime," +
                            "InspectedBy varchar(20)," +
                            "DamageCodeCount int," +
                            "Notes varchar(1000)," +
                            "CreationDate datetime," +
                            "DamageCode varchar(10)," +
                            "DamageDescription varchar(100))";

                        DataOps.PerformDBOperation(strSQL);

                        //Bulk copy dtDamageInfo into tmpVehicleInfoUpdate
                        DataOps.PerformBulkCopy("tmpVehicleInfoUpdate_" + strDateNow,
                            dtDamageInfo);

                        //Invoke the Sproc spUpdateVehicleInfo to update the three tables
                        //   and drop the tmpVehicleInfoUpdate table

                        //NOTE: don't need to insert new recs into AEVehicleStatusHistory table
                        //  or VehicleLocationHistory table
                        //  because of Triggers on AutoportExportVehicles table

                        //Set up the parameters for the SProc
                        objParam = new SProcParameter();
                        objParam.Paramname = "@Mode";
                        objParam.Paramvalue = strMode;
                        Paramobjects.Add(objParam);

                        objParam = new SProcParameter();
                        objParam.Paramname = "@tmptable";
                        objParam.Paramvalue = strtmptable;
                        Paramobjects.Add(objParam);

                        objParam = new SProcParameter();
                        objParam.Paramname = "@VehSQL";
                        objParam.Paramvalue = strUpdateVehSQL;
                        Paramobjects.Add(objParam);

                        objParam = new SProcParameter();
                        objParam.Paramname = "@Createdby";
                        objParam.Paramvalue = Globalitems.strUserName;
                        Paramobjects.Add(objParam);

                        objParam = new SProcParameter();
                        objParam.Paramname = "@CreationDate";
                        objParam.Paramvalue = datNow;
                        Paramobjects.Add(objParam);

                        ds = DataOps.GetDataset_with_SProc(strSProc, Paramobjects);
                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count==0)
                        {
                            Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord",
                                "No data returned after invoking SProc");
                            return;
                        }

                        //Ck for error in returned table
                        drow = ds.Tables[0].Rows[0];
                        if (drow["result"].ToString() == "ERROR")
                        {
                            strResult = "DamageSProc ERROR:<br>" + 
                                "ERROR NUMBER: " + drow["ErrorNumber"]  + "<br>" +
                                "ERROR SEVERITY: " + drow["ErrorSeverity"] + "<br>" + 
                                "ERROR STATE: " + drow["ErrorState"] + "<br>" +
                                "ERROR PROCEDURE: " + drow["ErrorProcedure"] + "<br>" +
                                "ERROR LINE: " + drow["ErrorLine"] + "<br>" +
                                "ERROR MESSAGE: " + drow["ErrorMessage"];
                            Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord",
                                strResult);
                            return;
                        }                       
                    }
                    else  //No Damage code info added just run strUpdateVehSQL
                    {
                        DataOps.PerformDBOperation(strUpdateVehSQL);
                    }                    

                    //Set Mode to READ
                    strMode = "READ";
                    btnMenu.Enabled = true;
                    btnVehLocator.Enabled = true;

                    blnChangeStarted = false;

                    //Inform User of success
                    MessageBox.Show("The Vehicle information has been modified in the DB.",
                        "VEHICLE INFO MODIFIED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Display other forms
                    Globalitems.DisplayOtherForms(this, true);

                    AdjustReadOnlyStatus(true);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSaveRecord", ex.Message);
            }
        }

        private bool ValidRecord()
        {
            //3/12/18 D.Maibor: Add Shipped status requires DateShipped
            try
            {
                DateTime datEarlier;
                DateTime datLater;
                DataSet ds;    
                string strSQL;
                string strMsg;
                string strStatus;
                string strval;

                tbDetails.SelectedTab = vehdetails;

                //Ck that Customer is not <select>
                if (cboCust.SelectedIndex==-1 || 
                    (cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim()
                    == "select")
                {
                    MessageBox.Show("Please select the Customer",
                        "NO CUSTOMER SELECTED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboCust.Focus();
                    return false;
                }

                //Ck for BillToNote if Billto is not <none> or <select>
                if (cboBillTo.SelectedIndex != -1)
                {
                    strval = (cboBillTo.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                    if (strval != "none" && strval != "select" &&
                        txtBillToNote.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("You must provide a Bill To Note",
                            "NO BILL TO NOTE",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tbDetails.SelectedTab = otherinfo;
                        txtBillToNote.Focus();
                        return false;
                    }
                }

                //Dest cannot be blank
                if (cboDest.SelectedIndex==-1)
                    strval = "";
                else
                    strval = (cboDest.SelectedItem as ComboboxItem).cboValue;
                if (strval == "" || strval == "select")
                {
                    MessageBox.Show("You must select a Destination",
                        "NO DESTINATION SELECTED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboDest.Focus();
                    return false;
                }

                //VIN cannot be blank
                if (txtVIN.Text.Trim().Length == 0)
                {
                    MessageBox.Show("The VIN cannot be blank",
                       "MISSING VIN",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtVIN.Focus();   
                    return false;
                }

                //Ck Veh. Year
                if (txtYear.Text.Trim().Length == 0)
                {
                    MessageBox.Show("The Vehicle Year cannot be blank",
                       "MISSING VEHICLE YEAR",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtYear.Focus();
                    return false;
                }

                //Ck Make
                if (txtMake.Text.Trim().Length == 0)
                {
                    MessageBox.Show("The Vehicle Make cannot be blank",
                       "MISSING VEHICLE MAKE",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMake.Focus();
                    return false;
                }

                //Ck Model
                if (txtModel.Text.Trim().Length == 0)
                {
                    MessageBox.Show("The Vehicle Model cannot be blank",
                       "MISSING VEHICLE MODEL",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtModel.Focus();
                    return false;
                }

                //Ck for Veh. Status
                if (cboVehStatus.SelectedIndex == -1)
                    strStatus = "";
                else
                    strStatus = (cboVehStatus.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strStatus == "" || strStatus == "select")
                {
                    MessageBox.Show("Please select the Vehicle Status",
                        "NO VEHICLE STATUS SELECTED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboVehStatus.Focus();
                    return false;
                }

                //Must have Date Rcv'd if not Pending
                if (!strStatus.Contains("Pending"))
                {
                    //Make sure DateReceived is not null
                    if (txtDateReceived.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Date Received cannot be blank.",
                        "NO DATE RECEIVED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateReceived.Focus();
                        return false;
                    }
                }

                //Ck  that dates are/are not blank, based on status

                //ClearedCustoms
                if (strStatus.StartsWith("Cleared"))
                {
                    //Must be blank -- Date Rcvd Ex
                    if (txtDateReceivedException.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Received Exception must be blank for " +
                            "this veh. status",
                        "DATE RECEIVED EXCEPTION MUST BE BLANK",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateReceivedException.Focus();
                        return false;
                    }

                    //Must be blank -- Date Customs Ex
                    if (txtDateCustomsException.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Customs Exception must be blank for " +
                            "this veh. status",
                        "DATE RECEIVED EXCEPTION MUST BE BLANK",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsException.Focus();
                        return false;
                    }

                    //Must be blank -- Date Voy Change Hold
                    if (txtVoyageChangeHold.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Voyage Change Hold must be blank for " +
                           "this veh. status",
                       "DATE VOYAGE CHANGE HOLD MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtVoyageChangeHold.Focus();
                        return false;
                    }

                    //Must have date values

                    //Make sure DateSubmitted is not null
                    if (txtDateSubToCustoms.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Date Sub'd to Customs cannot be blank.",
                        "NO DATE SUBMITTED TO CUSTOMS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateSubToCustoms.Focus();
                        return false;
                    }

                    //Make sure DateApproved is not null
                    if (txtDateCustomsApproval.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Date Customs Approved cannot be blank.",
                        "NO DATE APPROVED BY CUSTOMS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsApproval.Focus();
                        return false;
                    }
                }

                //CustomsException
                if (strStatus == "CustomsException")
                {
                    //Make sure DateSubmitted is not null
                    if (txtDateSubToCustoms.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Date Sub'd to Customs cannot be blank.",
                        "NO DATE SUBMITTED TO CUSTOMS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateSubToCustoms.Focus();
                        return false;
                    }

                    // Make sure DateCustException is not null
                    if (txtDateCustomsException.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Date Customs Exception cannot be blank.",
                        "NO DATE CUSTOMS EXCEPTION",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsException.Focus();
                        return false;
                    }
                }

                //ReceivedException
                if (strStatus == "ReceivedException")
                {
                    //Must be blank -- Date Subm'd
                    if (txtDateSubToCustoms.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Sub'd To Customs must be blank for " +
                           "this veh. status",
                       "DATE SUBMITTED TO CUSTOMS MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateSubToCustoms.Focus();
                        return false;
                    }

                    //Must be blank -- Date Customs Ex
                    if (txtDateCustomsException.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Customs Exception must be blank for " +
                           "this veh. status",
                       "DATE CUSTOMS EXCEPTION MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsException.Focus();
                        return false;
                    }

                    //Must be blank -- Date Customs Approval
                    if (txtDateCustomsApproval.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Customs Approval must be blank for " +
                           "this veh. status",
                       "DATE CUSTOMS APPROVAL MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsApproval.Focus();
                        return false;
                    }

                    //Must be blank -- Date Voy Change Hold
                    if (txtVoyageChangeHold.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Voyage Change Hold must be blank for " +
                           "this veh. status",
                       "DATE VOYAGE CHANGE HOLD MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtVoyageChangeHold.Focus();
                        return false;
                    }

                    // Make sure DateException is not null
                    if (txtDateReceivedException.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Date Rcv'd Exception cannot be blank.",
                        "NO DATE RECEIVED EXCEPTION",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateReceivedException.Focus();
                        return false;
                    }
                }

                //Shipped or ShippedByTruck
                if (strStatus.StartsWith("Shipped")) 
                {
                    // Make sure DateShipped is not null
                    if (txtDateShipped.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Date Shipped cannot be blank.",
                        "NO DATE SHIPPED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateShipped.Focus();
                        return false;
                    }
                }

                //SubmittedCustoms
                if (strStatus == "SubmittedCustoms")
                {
                    //Must be blank -- Date Rcv'd Ex
                    if (txtDateReceivedException.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Rcv'd Exception must be blank for " +
                           "this veh. status",
                       "DATE RECEIVED EXCEPTION MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateReceivedException.Focus();
                        return false;
                    }

                    //Must be blank -- Date Customs Ex
                    if (txtDateCustomsException.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Customs Exception must be blank for " +
                           "this veh. status",
                       "DATE CUSTOMS EXCEPTION MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsException.Focus();
                        return false;
                    }

                    //Must be blank -- Date Customs Approval
                    if (txtDateCustomsApproval.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Customs Approval must be blank for " +
                           "this veh. status",
                       "DATE CUSTOMS APPROVAL MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsApproval.Focus();
                        return false;
                    }

                    //Must be blank -- Date Voy Change Hold
                    if (txtVoyageChangeHold.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Voyage Change Hold must be blank for " +
                           "this veh. status",
                       "DATE VOYAGE CHANGE HOLD MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtVoyageChangeHold.Focus();
                        return false;
                    }

                    // Make sure DateSub'd is not null
                    if (txtDateSubToCustoms.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Date Sub'd to Customs cannot be blank.",
                        "NO DATE SUBMITTED TO CUSTOMS",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateSubToCustoms.Focus();
                        return false;
                    }
                }

                //VoyageChangeHold
                if (strStatus == "VoyageChangeHold")
                {
                    //Must be blank -- Date Subm'd
                    if (txtDateSubToCustoms.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Sub'd To Customs must be blank for " +
                           "this veh. status",
                       "DATE SUBMITTED TO CUSTOMS MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateSubToCustoms.Focus();
                        return false;
                    }

                    //Must be blank -- Date Customs Ex
                    if (txtDateCustomsException.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Customs Exception must be blank for " +
                           "this veh. status",
                       "DATE CUSTOMS EXCEPTION MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsException.Focus();
                        return false;
                    }

                    //Must be blank -- Date Customs Approval
                    if (txtDateCustomsApproval.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Customs Approval must be blank for " +
                           "this veh. status",
                       "DATE CUSTOMS APPROVAL MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsApproval.Focus();
                        return false;
                    }

                    //Must be blank -- Date Shipped
                    if (txtDateShipped.Text.Trim().Length > 0)
                    {
                        MessageBox.Show("Date Shipped must be blank for " +
                           "this veh. status",
                       "DATE SHIPPED MUST BE BLANK",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsApproval.Focus();
                        return false;
                    }

                    // Make sure Voy. Change Hold is not null
                    if (txtVoyageChangeHold.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Voyage Change Hold Date cannot be blank.",
                        "NO VOYAGE CHANGE HOLD DATE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtVoyageChangeHold.Focus();
                        return false;
                    }
                }


                //Ck for note if status: CustomsException / HoldCustomerRequest / HoldMechanical
                // ReceivedException / ShippedByTruck / VoyageChangeHold
                if (strStatus.Contains("Hold") || strStatus.Contains("Except") ||
                    strStatus== "ShippedByTruck")
                {
                    if (txtNote.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("You must provide a Note for this status.",
                            "MISSING NOTE",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tbDetails.SelectedTab = otherinfo;
                        txtNote.Focus();
                        return false;
                    }
                }

                //Ck that certain dates are later than other dates
                // if date displayed has no time element, set to 11:59:59 PM to allow
                //  same day comparison
                datEarlier = Convert.ToDateTime(txtDateReceived.Text);

                //Date Rcv'd Excep must be after Date Rcv'd
                strval = txtDateReceivedException.Text.Trim();
                if (strval.Length > 0)
                {
                    //Set as end of day, if no time stamp
                    if (!strval.Contains(":")) strval += " 11:59:59 PM";
                    datLater = Convert.ToDateTime(strval);

                    if (datLater < datEarlier)
                    {
                        MessageBox.Show("The Date Of Received Exception cannot be " +
                            "before the Date Received.",
                        "RECEIVED EXCEPTION DATE IS BEFORE DATE RECEIVED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateReceivedException.Focus();
                        txtDateReceivedException.Select(0, txtDateReceivedException.Text.Length);
                        return false;
                    }
                }

                // Date Voy Change Hold must be after Date Rcv'd
                strval = txtVoyageChangeHold.Text.Trim();
                if (strval.Length > 0)
                {
                    //Set as end of day, if no time stamp
                    if (!strval.Contains(":")) strval += " 11:59:59 PM";
                    datLater = Convert.ToDateTime(strval);

                    if (datLater < datEarlier)
                    {
                        MessageBox.Show("The Voyage Change Hold Date cannot be " +
                            "before the Date Received.",
                        "VOY. CHANGE HOLD DATE IS BEFORE DATE RECEIVED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtVoyageChangeHold.Focus();
                        txtVoyageChangeHold.Select(0, txtVoyageChangeHold.Text.Length);
                        return false;
                    }
                }

                // Date Sub'd to Customs must be after Date Rcv'd
                strval = txtDateSubToCustoms.Text.Trim();
                if (strval.Length > 0)
                {
                    //Set as end of day, if no time stamp
                    if (!strval.Contains(":")) strval += " 11:59:59 PM";
                    datLater = Convert.ToDateTime(strval);

                    if (datLater < datEarlier)
                    {
                        MessageBox.Show("The Date Sub'd to Customs cannot be " +
                            "before the Date Received.",
                        "DATE SUBMITTED TO CUSTOMS IS BEFORE DATE RECEIVED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateSubToCustoms.Focus();
                        txtDateSubToCustoms.Select(0, txtDateSubToCustoms.Text.Length);
                        return false;
                    }
                }

                // Date Customs Excep must be after Date Rcv'd
                strval = txtDateCustomsException.Text.Trim();
                if (strval.Length > 0)
                {
                    //Set as end of day, if no time stamp
                    if (!strval.Contains(":")) strval += " 11:59:59 PM";
                    datLater = Convert.ToDateTime(strval);

                    if (datLater < datEarlier)
                    {
                        MessageBox.Show("The Date Customs Exception cannot be " +
                            "before the Date Received.",
                        "CUSTOMS EXCEPTION DATE IS BEFORE DATE RECEIVED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsException.Focus();
                        txtDateCustomsException.Select(0, txtDateCustomsException.Text.Length);
                        return false;
                    }
                }

                // Date Customs Approval must be after Date Rcv'd
                strval = txtDateCustomsApproval.Text.Trim();
                if (strval.Length > 0)
                {
                    //Set as end of day, if no time stamp
                    if (!strval.Contains(":")) strval += " 11:59:59 PM";
                    datLater = Convert.ToDateTime(strval);

                    if (datLater < datEarlier)
                    {
                        MessageBox.Show("The Date Customs Approval cannot be " +
                            "before the Date Received.",
                        "CUSTOMS APPROVAL DATE IS BEFORE DATE RECEIVED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateCustomsApproval.Focus();
                        txtDateCustomsApproval.Select(0, txtDateCustomsApproval.Text.Length);
                        return false;
                    }
                }

                // Date Shipped must be after Date Rcv'd
                strval = txtDateShipped.Text.Trim();
                if (strval.Length > 0)
                {
                    //Set as end of day, if no time stamp
                    if (!strval.Contains(":")) strval += " 11:59:59 PM";
                    datLater = Convert.ToDateTime(strval);

                    if (datLater < datEarlier)
                    {
                        MessageBox.Show("The Shipped Date cannot be " +
                            "before the Date Received.",
                        "SHIPPED DATE IS BEFORE DATE RECEIVED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDateShipped.Focus();
                        txtDateShipped.Select(0, txtDateShipped.Text.Length);
                        return false;
                    }
                }

                //Ck dates that must be after DateSubCustoms
                if (txtDateSubToCustoms.Text.Trim().Length > 0)
                {
                    datEarlier = Convert.ToDateTime(txtDateSubToCustoms.Text);

                    // Date Customs Excep must be after Date Sub'd Customs
                    strval = txtDateCustomsException.Text.Trim();
                    if (strval.Length > 0)
                    {
                        //Set as end of day, if no time stamp
                        if (!strval.Contains(":")) strval += " 11:59:59 PM";
                        datLater = Convert.ToDateTime(strval);

                        if (datLater < datEarlier)
                        {
                            MessageBox.Show("The Customs Exception Date cannot be " +
                                "before the Date Submitted to Customs.",
                            "CUSTOMS EXCEPTION DATE IS BEFORE DATE SUB'D TO CUSTOMS",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtDateCustomsException.Focus();
                                txtDateCustomsException.Select(0, 
                                    txtDateCustomsException.Text.Length);
                            return false;
                        }
                    }

                    // Date Customs Approval must be after Date Sub'd Customs
                    strval = txtDateCustomsApproval.Text.Trim();
                    if (strval.Length > 0)
                    {
                        //Set as end of day, if no time stamp
                        if (!strval.Contains(":")) strval += " 11:59:59 PM";
                        datLater = Convert.ToDateTime(strval);

                        if (datLater < datEarlier)
                        {
                            MessageBox.Show("The Customs Approval Date cannot be " +
                                "before the Date Submitted to Customs.",
                            "CUSTOMS APPROVAL DATE IS BEFORE DATE SUB'D TO CUSTOMS",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtDateCustomsApproval.Focus();
                            txtDateCustomsApproval.Select(0,
                                txtDateCustomsApproval.Text.Length);
                            return false;
                        }
                    }
                }

                //Ck Size Class
                if ((cboSize.SelectedItem as ComboboxItem).cboValue.ToString().Trim()
                    == "select")
                {
                    MessageBox.Show("Please select the Vehicle Size Class",
                        "NO VEHICLE SIZE CLASS SELECTED",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboSize.Focus();
                    return false;
                }

                //Ck that at least one control has changed if mode is MODIFY
                if (strMode == "MODIFY")
                {
                    //Use linq to find all updated controls
                    var changedlist = lsControls.Where(ctrlinfo => ctrlinfo.Updated == true).ToList();
                    if (changedlist.Count == 0)
                    {
                        MessageBox.Show("You have not changed anything for this Vehicle.\r\n" +
                           "There is nothing to update", "NO CHANGES MADE",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                //If new record, check for recs w/same VIN where DateShipped IS NULL
                //If edit record check for recs w/same VIN  where DateShipped IS NULL 
                //  & different vehID
                
                //Get existing rows for VIN from veh table
                strSQL = @"SELECT 
                COUNT(AutoportExportVehiclesID) AS totrec
                FROM AutoportExportVehicles
                WHERE VIN = '" + txtVIN.Text + @"' AND DateShipped IS NULL ";

                if (strMode == "MODIFY") strSQL += "AND AutoportExportVehiclesID <> " +
                        txtInternalID.Text;

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "ValidRecord",
                        "Query to veh. table returned no data");
                    return false;
                }

                if (Convert.ToInt16(ds.Tables[0].Rows[0]["totrec"].ToString()) > 0)
                {
                    strMsg = "The VIN already exists as an open record\n";

                    if (strMode == "NEW")
                        strMsg += "You cannot add a new record with the same VIN.";
                    else
                        strMsg += "You cannot edit this record with the same VIN.";

                    MessageBox.Show(strMsg,"VIN ALREADY EXISTS", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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

        private void CalculateChargeRequest()
        {
            DateTime datDateReceived;
            Decimal decEntryRate;
            int intCustomerID;
            int intEntryRateOverrideInd = 0;
            int intPerDiemGraceDays;
            int intPerDiemGraceDaysOverrideInd = 0;
            int intVehID;
            string strSizeClass;
            string strval;
            string strVIN = "";

            try
            {
                //To calculate current charge, CustomerID, EntryRate, PerDiemGraceDays,
                //  DateReceived, & SizeClass are necessary. 

                //Get strVIN
                if (txtVIN.Text.Trim().Length > 0) strVIN = txtVIN.Text.Trim();

                //Get intVehID
                intVehID = Convert.ToInt32(txtInternalID.Text);

                //Get EntryRate
                if (txtEntryRate.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Cannot calculate Current Charge without an Entry Rate!",
                       "ENTRY RATE IS MISSING", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return;
                }
                strval = txtEntryRate.Text.Trim();
                strval = strval.Replace("$", "");
                decEntryRate = Convert.ToDecimal(strval);

                //Get EntryRateOverrideInd
                if (ckEntryRateOverride.Checked) intEntryRateOverrideInd = 1;

                //Get CustomerID
                if (cboCust.SelectedIndex < 1)
                {
                    MessageBox.Show("Cannot calculate Current Charge without a Customer!",
                        "CUSTOMER IS MISSING", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                intCustomerID = Convert.ToInt32((cboCust.SelectedItem as ComboboxItem).cboValue);

                //Get PerDiemGraceDays
                if (txtPerDiemGraceDays.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Cannot calculate Current Charge without Per Diem Grace Days!",
                       "PER DIEM GRACE DAYS ARE MISSING", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return;
                }
                intPerDiemGraceDays = Convert.ToInt16(txtPerDiemGraceDays.Text);

                //Get PerDiemGraceDaysOverrideInd
                if (ckPerDiemGraceOverride.Checked) intPerDiemGraceDaysOverrideInd = 1;

                //Get SizeClass
                if (cboSize.SelectedIndex < 1)
                {
                    MessageBox.Show("Cannot calculate Current Charge without Size Class!",
                      "SIZE CLASS IS MISSING", MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
                    return;
                }
                strSizeClass = (cboSize.SelectedItem as ComboboxItem).cboValue.ToString();

                //Get DateReceived
                if (txtDateReceived.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Cannot calculate Current Charge without Date Received!",
                      "DATE RECEIVED CLASS IS MISSING", MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
                    return;
                }
                datDateReceived = Convert.ToDateTime(txtDateReceived.Text.Trim());

                    frmCalculateCharge frm = new frmCalculateCharge(strVIN,
                        intVehID,
                        decEntryRate,
                        intCustomerID,
                        intEntryRateOverrideInd,
                        intPerDiemGraceDays,
                        intPerDiemGraceDaysOverrideInd,
                        strSizeClass,
                        datDateReceived);
                frm.ShowDialog();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CalculateChargeRequest", ex.Message);
            }

        }

        private void btnCancel_Clicked()
        {
            try { CancelSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnCancel_Clicked", ex.Message); }
        }

        private void btnDelete_Clicked()
        {
            try { PerformDeleteRecord(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnDelete_Clicked", ex.Message); }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globalitems.MainForm.Show();
            Globalitems.MainForm.Focus();
        }

        private void btnModify_Clicked()
        {
            try { ModifyRecordSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnModify_Clicked", ex.Message); }
        }

        private void btnNew_Clicked()
        {
            try { NewRecordSetup(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnNew_Clicked", ex.Message); }
        }

        private void btnNext_Clicked()
        {
            try { PerformMoveNext(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnPrev_Clicked", ex.Message); }
        }

        private void btnPrev_Clicked()
        {
            try { PerformMovePrevious(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnPrev_Clicked", ex.Message); }
        }

        private void btnSave_Clicked()
        {
            try { PerformSaveRecord(); }
            catch (Exception ex) { Globalitems.HandleException(CURRENTMODULE, "btnSave_Clicked", ex.Message); }
        }

        private void cboCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem cboitem;
            DataSet ds;
            DialogResult dlResult;
            frmAreYouSure frmConfirm;
            string strCustomerID = (cboCust.SelectedItem as ComboboxItem).cboValue;
            string strMessage;
            string strSQL;

            try
            {
                if (strCustomerID == "select")
                {
                    cboBillTo.Items.Clear();
                    cboBillTo.Enabled = false;
                }
                else
                    Globalitems.SetBillToCbo(ref cboBillTo, strCustomerID);

                //Enable cboBillTo if it's not blank
                if (strMode != "READ" && cboBillTo.Items.Count > 0) cboBillTo.Enabled = true;

                if (strMode == "MODIFY")
                {
                    //Make sure User wants to change Customer
                    strMessage = "Are you sure you want to update the Customer?\n\n" +
                   "You will also have to reset the Voyage, Forwarder and\n  Exporter for the vehicles; " +
                   "and reprint any previously\n printed labels.";
                   frmConfirm = new frmAreYouSure(strMessage);
                    dlResult = frmConfirm.ShowDialog();
                    if (dlResult != DialogResult.OK) return;

                    //Record control value changed if in MODIFY mode
                    Formops.ChangeControlUpdatedStatus("cboCust", lsControls);

                    //Clear affected info w/new Customer
                    txtVessel.Text = "";
                    txtVoyageNumber.Text = "";

                    //Get EntryRate & PerDiemGraceDays
                }

                //Clear Customer Number
                txtCustNo.Text = "";

                //Retrieve Customer Number
                if (strCustomerID != "select")
                {
                    strSQL = "SELECT CustomerCode FROM Customer WHERE CustomerID = " +
                    (cboCust.SelectedItem as ComboboxItem).cboValue.ToString();
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "cboCust_SelectedIndexChanged",
                            "No Customer Code returned from Customer table");
                        return;
                    }

                    txtCustNo.Text = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                }
                
                //Clear cboForwarder & add <select> as 1st item
                cboForwarder.Items.Clear();

                if (strCustomerID != "select")
                {

                    // Add <select> to cbo
                    cboitem = new ComboboxItem();
                    cboitem.cboText = "<select>";
                    cboitem.cboValue = "select";
                    cboForwarder.Items.Add(cboitem);

                    cboForwarder.DisplayMember = "cboText";
                    cboForwarder.ValueMember = "cboValue";
                    cboForwarder.SelectedIndex = 0;

                    //Retrieve Freight Forwarders for selected Customer
                    strSQL = "SELECT  AEFreightForwarderID, " +
                        "CASE WHEN LEN(RTRIM(ISNULL(FreightForwarderShortName,''))) > 0 THEN " +
                            "FreightForwarderShortName " +
                            "ELSE FreightForwarderName " +
                        "END AS Forwarder " +
                        "FROM AEFreightForwarder " +
                        "WHERE AECustomerID IS NOT NULL ";

                    //Add CustomerID
                    strSQL += "AND AECustomerID = " +
                        (cboCust.SelectedItem as ComboboxItem).cboValue.ToString() + " ";
                    strSQL += "ORDER BY Forwarder";

                    ds = DataOps.GetDataset_with_SQL(strSQL);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        cboitem = new ComboboxItem();
                        cboitem.cboText = dr["Forwarder"].ToString();
                        cboitem.cboValue = dr["AEFreightForwarderID"].ToString();
                        cboForwarder.Items.Add(cboitem);
                    }

                    UpdateEntryRateGraceDays();
                }
            }

            catch (Exception ex)
            {
                 Globalitems.HandleException(CURRENTMODULE, "cboCust_SelectedIndexChanged", ex.Message);
            }
        }

        private void UpdateEntryRateGraceDays()
        {
            DateTime dat;
            DataRow drow;
            DataSet ds;
            string strCurrentMode = strMode;
            string strCustomerID;
            string strSize;
            string strSQL;

            try
            {
                if (strMode == "READ") return;

                //Need Customer, Size, & DateReceived to get EntryFee & PerDiemGraceDays from
                //  AutoportExportRates table
                strCustomerID = (cboCust.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strCustomerID == "select") return;

                //Ck for date in txtDateReceived
                if (!DateTime.TryParse(txtDateReceived.Text.Trim(), out dat)) return;

                strSize = (cboSize.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                if (strSize== "select") return;
                strSize = "Size " + strSize + " Rate";

                if (txtDateReceived.Text.Length == 0) return;

                txtEntryRate.Text = "0";
                txtPerDiemGraceDays.Text = "0";

                strSQL = "SELECT ISNULL(EntryFee,0) AS EntryFee," +
                    "ISNULL(PerDiemGraceDays,0) As PerDiemGraceDays " +
                    "from AutoportExportRates " +
                    "WHERE CustomerID = " + strCustomerID +
                    " AND StartDate <= '" + dat.ToString("M/d/yyyy") + "'" + 
                    " AND ISNULL(EndDate,'12/21/9999') >= '" + dat.ToString("M/d/yyyy") +
                    "' AND RateType = '" + strSize + "'";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "UpdateEntryRateGraceDays",
                        "No table returned from query");
                    return;
                }

                //Set the EntryRate & PerDiem Grace Days, & unCk Overrides
                if (ds.Tables[0].Rows.Count > 0)
                {
                    drow = ds.Tables[0].Rows[0];
                    txtEntryRate.Text = drow["EntryFee"].ToString();
                    txtPerDiemGraceDays.Text = drow["PerDiemGraceDays"].ToString();
                }

                txtEntryRate.Text = Globalitems.FormatCurrency(txtEntryRate.Text);               
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "UpdateEntryRateGraceDays", ex.Message);
            }
        }

        private void cboForwarder_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                ComboboxItem cboitem;
                DataSet ds;
                string strSQL;

                try
                {
                    if ((cboForwarder.SelectedItem as ComboboxItem).cboValue.ToString().Trim() == "select") return;

                    //Record control value changed if in MODIFY mode
                    if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboForwarder", lsControls);

                    cboExporter.Items.Clear();

                    strSQL = "SELECT  AEExporterID, " +
                        "CASE WHEN LEN(RTRIM(ISNULL(ExporterShortName,''))) > 0 THEN " +
                            "ExporterShortName " +
                            "ELSE ExporterName  " +
                        "END AS Exporter " +
                        "FROM AEExporter " +
                        "WHERE ExporterName IS NOT NULL ";

                    //Add AEFreightForwarderID if cboForwarder != All
                    strSQL += "AND AEFreightForwarderID  = " +
                        (cboForwarder.SelectedItem as ComboboxItem).cboValue.ToString() + " ";

                    strSQL += "ORDER BY Exporter";

                    ds = DataOps.GetDataset_with_SQL(strSQL);

                    // Add All to cbo
                    cboitem = new ComboboxItem();
                    cboitem.cboText = "<select>";
                    cboitem.cboValue = "select";
                    cboExporter.Items.Add(cboitem);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        cboitem = new ComboboxItem();
                        cboitem.cboText = dr["Exporter"].ToString();
                        cboitem.cboValue = dr["AEExporterID"].ToString();
                        cboExporter.Items.Add(cboitem);
                    }

                    cboExporter.DisplayMember = "cboText";
                    cboExporter.ValueMember = "cboValue";
                    cboExporter.SelectedIndex = 0;
                }

                catch (Exception ex)
                {
                    Globalitems.HandleException(CURRENTMODULE, "cboForwarder_SelectedIndexChanged", ex.Message);
                }
            }
        }

        private void btnVehLocator_Click(object sender, EventArgs e)
        {
            frmVehSearch frm;

            try
            {
                //If frmVehSearch is already open set frm to it
                if (Application.OpenForms.OfType<frmVehSearch>().Count() == 0)
                {
                    frm = new frmVehSearch();
                    Formops.OpenNewForm(frm);
                }
                else
                {
                    frm = (frmVehSearch)Application.OpenForms["frmVehSearch"];
                    frm.Focus();
                }
            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "btnVehLocator_Click", ex.Message);
            }
        }

        private void btnAddDamageCode_Click(object sender, EventArgs e)
        {
            OpenDamageForm();
        }


        private void btnCurrAmt_Click(object sender, EventArgs e)
        {
            CalculateChargeRequest();
        }

        private void OpenDamageCodeNoteForm(string strDamageCode,
            string strInspectionType,string strLocationType, string strFullNote)
        {
            frmDamageCodeNote frm;

            frm = new frmDamageCodeNote(txtVIN.Text, 
                strInspectionType,strDamageCode, strLocationType, strFullNote);
            frm.ShowDialog();
        }

        private void UpdateDamageCodeInfo()
        {
            int intLastRec = dtNewDamageCodes.Rows.Count - 1;
            DataTable dtMerged;

            try
            {
                //Combine dtOriginal + dtNew
                dtMerged = dtOriginalDamageCodes.Copy();
                dtMerged.Merge(dtNewDamageCodes);

                //Refresh dg
                dgInspectionsDamage.DataSource = dtMerged;

                //Add note to Other Info, if 1st new Inspection 
                if ((int) dtNewDamageCodes.Rows[intLastRec]["AEVehicleInspectionID"] == 1 )
                txtNote.Text += Environment.NewLine + "------" + Environment.NewLine +
                    "New damage codes added by " + Globalitems.strUserName +
                    " on " + DateTime.Now.ToString("M/d/yyy h:mm tt") +
                    Environment.NewLine + "------";

                Formops.ChangeControlUpdatedStatus("txtNote", lsControls);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "UpdateDamageCodeInfo", ex.Message);
            }
        }

        private void OpenFormToAddItem(string strForm)
        {
            frmCustomerAdmin frmCus;
            frmExporterAdmin frmEx;
            frmFreightForwarderAdmin frmForwarder;

            try
            {
                switch (strForm)
                {
                    case ("frmCustomerAdmin"):
                        //Open new form if not already open
                        if (Application.OpenForms.OfType<frmCustomerAdmin>().Count() == 0)
                        {
                            frmCus = new frmCustomerAdmin();
                            frmCus.blnNewCustomerRQFromOtherForm = true;
                            Formops.OpenNewForm(frmCus);
                        }
                        else
                        {
                            frmCus = (frmCustomerAdmin)Application.OpenForms["frmCustomerAdmin"];
                            frmCus.blnNewCustomerRQFromOtherForm = true;
                            frmCus.Focus();
                        }
                        
                        break;
                    case ("frmExporterAdmin"):
                        //Open new form if not already open
                        if (Application.OpenForms.OfType<frmExporterAdmin>().Count() == 0)
                        {
                            frmEx = new frmExporterAdmin();
                            frmEx.blnNewExporterRQFromOtherForm = true;
                            Formops.OpenNewForm(frmEx);
                        }
                        else
                        {
                            frmEx = (frmExporterAdmin)Application.OpenForms["frmExporterAdmin"];
                            frmEx.blnNewExporterRQFromOtherForm = true;
                            frmEx.Focus();
                        }

                        break;
                    case ("frmFreightForwarderAdmin"):
                        //Open new form if not already open
                        if (Application.OpenForms.OfType<frmFreightForwarderAdmin>().Count() == 0)
                        {
                            frmForwarder = new frmFreightForwarderAdmin();
                            frmForwarder.blnNewForwarderRQFromOtherForm = true;
                            Formops.OpenNewForm(frmForwarder);
                        }
                        else
                        {
                            frmForwarder = (frmFreightForwarderAdmin)Application.OpenForms["frmFreightForwarderAdmin"];
                            frmForwarder.blnNewForwarderRQFromOtherForm = true;
                            frmForwarder.Focus();
                        }
                        break;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "OpenFormtoAddItem", ex.Message);
            }
        }

        private void CheckVIN()
        {
            VINInfo objVINInfo;
            string strNewVIN = "";

            try
            {
                if (txtVIN.Text.Trim().Length > 0) strNewVIN = txtVIN.Text.Trim();

                //Make sure VIN changed
                if (strNewVIN != strPrevVIN)
                {
                    //Clear VIN-related values
                    txtYear.Text = "";
                    txtMake.Text = "";
                    txtModel.Text = "";
                    txtBodystyle.Text = "";
                    txtLength.Text = "";
                    txtWidth.Text = "";
                    txtHeight.Text = "";
                    txtWeight.Text = "";
                    txtCubicFeet.Text = "";

                    //Use DecodeVIN if new 17 char VIN entered
                    if (strNewVIN.Length == 17)
                    {
                        objVINInfo = Globalitems.DecodeVIN(strNewVIN);
                        
                        if (objVINInfo.Error)
                        {
                            MessageBox.Show(Globalitems.DecodeVINErrorMsg(objVINInfo),
                                        "VIN CANNOT BE DECODED",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                            //Send email & log error
                            Globalitems.HandleException(CURRENTMODULE,
                                "CheckVIN",
                                "The following VIN Decoding error occurred for VIN: " +
                                objVINInfo.VIN + ": " + objVINInfo.ErrorDesc, false);

                            //Reset blnException to continue processing
                            Globalitems.blnException = false;
                        }

                        //Set VIN info controls to new values
                        txtYear.Text = objVINInfo.VehicleYear;
                        txtMake.Text = objVINInfo.Make;
                        txtModel.Text = objVINInfo.Model;
                        txtBodystyle.Text = objVINInfo.Bodystyle;
                        txtLength.Text = objVINInfo.VehicleLength;
                        txtWidth.Text = objVINInfo.VehicleWidth;
                        txtHeight.Text = objVINInfo.VehicleHeight;
                        txtWeight.Text = objVINInfo.VehicleWeight;
                        txtCubicFeet.Text = objVINInfo.VehicleCubicFeet;

                        //Set cboSize if value returned
                        if (objVINInfo.SizeClass.Length > 0)
                        {
                            foreach (ComboboxItem cboitem in cboSize.Items)
                                if (cboitem.cboValue == objVINInfo.SizeClass)
                                    cboSize.SelectedItem = cboitem;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CheckVIN", ex.Message);
            }
        }

        private void dgInspectionsDamage_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string strDamageCode;
            string strInspectionType;
            string strLocationType;
            string strFullNote;

            //Cast generic sender as a DataGridView
            var senderGrid = (DataGridView)sender;

            //If Note column was clicked and value==VIEW open DamageCodeNote form
            if (senderGrid.Columns[e.ColumnIndex].Name.ToString() == "Note" &&
                senderGrid.Rows[e.RowIndex].Cells["Note"].Value.ToString() == "VIEW")
            {
                //Get InspectionID & DamageCode
                strDamageCode = senderGrid.Rows[e.RowIndex].Cells["DamageCode"].Value.ToString();
                strInspectionType = senderGrid.Rows[e.RowIndex].Cells["Inspectype"].Value.ToString();
                strLocationType = senderGrid.Rows[e.RowIndex].Cells["DamageDesc"].Value.ToString();
                strFullNote = senderGrid.Rows[e.RowIndex].Cells["FullNote"].Value.ToString();

                OpenDamageCodeNoteForm(strDamageCode,strInspectionType,strLocationType,strFullNote);
            }
        }

        private void ckEntryRateOverride_CheckedChanged(object sender, EventArgs e)
        {
            if (ckEntryRateOverride.Checked)
            {
                txtEntryRate.ReadOnly = false;
                txtEntryRate.Enabled = true;
                txtEntryRate.BackColor = Color.White;
            }
            else
            {
                txtEntryRate.ReadOnly = true;
                txtEntryRate.Enabled = false;
                txtEntryRate.BackColor = Color.FromArgb(244, 244, 244);
            }
        }

        private void ckPerDiemGraceOverride_CheckedChanged(object sender, EventArgs e)
        {
            if (ckPerDiemGraceOverride.Checked)
            {
                txtPerDiemGraceDays.ReadOnly = false;
                txtPerDiemGraceDays.Enabled = true;
                txtPerDiemGraceDays.BackColor = Color.White;
            }
            else
            {
                txtPerDiemGraceDays.ReadOnly = true;
                txtPerDiemGraceDays.Enabled = false;
                txtPerDiemGraceDays.BackColor = Color.FromArgb(244, 244, 244);
            }
        }

        private void btnNewCustomer_Click(object sender, EventArgs e)
        {
            OpenFormToAddItem("frmCustomerAdmin");
        }

        private void txtVIN_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtVIN", lsControls);
        }

        private void ckDecoded_CheckedChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("ckDecoded", lsControls);
        }

        private void cboDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboDest", lsControls);
        }

        private void cboTShipPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboTShipPort", lsControls);
        }

        private void txtBayLoc_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtBayLoc", lsControls);
        }

        private void cboExporter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboExporter", lsControls);
        }


        private void txtPortReceipt_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtPortReceipt", lsControls);
        }

        private void txtYear_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtYear", lsControls);
        }

        private void txtModel_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtModel", lsControls);
        }

        private void cboVehStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboVehStatus", lsControls);
        }

        private void txtLength_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtLength", lsControls);
        }

        private void txtWidth_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtWidth", lsControls);
        }

        private void txtHeight_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtHeight", lsControls);
        }

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtWeight", lsControls);
        }

        private void txtCubicFeet_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtCubicFeet", lsControls);
        }

        private void cboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEntryRateGraceDays();

            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("cboSize", lsControls);
        }

        private void txtBookingNumber_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtBookingNumber", lsControls);
        }

        private void txtVoyageNumber_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtVoyageNumber", lsControls);
        }

        private void txtVIVTagNumber_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtVIVTagNumber", lsControls);
        }

        private void txtITNNumber_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtITNNumber", lsControls);
        }

        private void txtLastPhysicalBy_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtLastPhysicalBy", lsControls);
        }

        private void txtLastPhysicalDate_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtLastPhysicalDate", lsControls);
        }

        private void ckAudio_CheckedChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("ckAudio", lsControls);
        }

        private void ckPushcar_CheckedChanged(object sender, EventArgs e)
        {
            lblPushCar.Visible = false;
            if (ckPushcar.Checked) lblPushCar.Visible = true;

            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("ckPushcar", lsControls);
        }

        private void ckNav_CheckedChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("ckNav", lsControls);
        }

        private void ckMechException_CheckedChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("ckMechException", lsControls);
        }

        private void txtMake_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtMake", lsControls);
        }

        private void txtBodystyle_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtBodystyle", lsControls);
        }

        private void txtCustIdent_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtCustIdent", lsControls);
        }

        private void txtColor_TextChanged(object sender, EventArgs e)
        {
            //Record control value changed if in MODIFY mode
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtColor", lsControls);
        }

        private void txtVIN_Leave(object sender, EventArgs e)
        {CheckVIN();}

        private void txtVIN_Enter(object sender, EventArgs e)
        {
            strPrevVIN = "";
            if (txtVIN.Text.Trim().Length > 0) strPrevVIN = txtVIN.Text.Trim();
        }

        private void txtNote_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtNote", lsControls);
        }

        private void txtSpecialInstr_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtSpecialInstr", lsControls);
        }

        private void txtEntryRate_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtEntryRate", lsControls);
        }

        private void txtEntryRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only allow digits, backspace, and decimal point, '.' 
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) &&
                e.KeyChar != '.') e.Handled = true;

            // Only allow 1 decimal point ('.')
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1) e.Handled = true;
        }

        private void txtEntryRate_Leave(object sender, EventArgs e)
        {
            string strval = txtEntryRate.Text.Trim();

            //Remove $ if there
            strval = strval.Replace("$", "");

            //User cannot leave blank, set to default of 0.00
            if (strval.Length > 0)
            {
                Globalitems.FormatTwoDecimal(ref strval);
                txtEntryRate.Text = "$" + strval;
            }
            else
            {
                txtEntryRate.Text = "$0.00";
            }
        }

        private void txtPerDiemGraceDays_TextChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtPerDiemGraceDays", lsControls);
        }

        private void txtPerDiemGraceDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only allow digits & backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void txtDateReceived_KeyPress(object sender, KeyPressEventArgs e)
        {Globalitems.CheckDateKeyPress(e);}

        private void txtDateReceived_Validating(object sender, CancelEventArgs e)
        {Globalitems.ValidateDate(txtDateReceived, e);}

        private void txtDateReceived_TextChanged(object sender, EventArgs e)
        {
            Formops.ChangeControlUpdatedStatus("txtDateReceived", lsControls);
        }

        private void txtDateReceived_Leave(object sender, EventArgs e)
        { UpdateEntryRateGraceDays(); }

        private void txtDateReceivedException_KeyPress(object sender, KeyPressEventArgs e)
            { Globalitems.CheckDateKeyPress(e); }

        private void txtDateReceivedException_TextChanged(object sender, EventArgs e)
        {Formops.ChangeControlUpdatedStatus("txtDateReceivedException", lsControls);}

        private void txtDateReceivedException_Validating(object sender, CancelEventArgs e)
            { Globalitems.ValidateDate(txtDateReceivedException, e); }

        private void txtVoyageChangeHold_KeyPress(object sender, KeyPressEventArgs e)
        { Globalitems.CheckDateKeyPress(e); }

        private void txtVoyageChangeHold_TextChanged(object sender, EventArgs e)
        { Formops.ChangeControlUpdatedStatus("txtVoyageChangeHold", lsControls); }

        private void txtVoyageChangeHold_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtVoyageChangeHold, e); }

        private void txtDateSubToCustoms_KeyPress(object sender, KeyPressEventArgs e)
        { Globalitems.CheckDateKeyPress(e); }

        private void txtDateSubToCustoms_TextChanged(object sender, EventArgs e)
        { Formops.ChangeControlUpdatedStatus("txtDateSubToCustoms", lsControls); }

        private void txtDateSubToCustoms_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtDateSubToCustoms, e); }

        private void txtDateCustomsException_KeyPress(object sender, KeyPressEventArgs e)
        { Globalitems.CheckDateKeyPress(e); }

        private void txtDateCustomsException_TextChanged(object sender, EventArgs e)
        { Formops.ChangeControlUpdatedStatus("txtDateCustomsException", lsControls); }

        private void txtDateCustomsException_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtDateCustomsException, e); }

        private void txtDateCustomsApproval_KeyPress(object sender, KeyPressEventArgs e)
        { Globalitems.CheckDateKeyPress(e); }

        private void txtDateCustomsApproval_TextChanged(object sender, EventArgs e)
        { Formops.ChangeControlUpdatedStatus("txtDateCustomsApproval", lsControls); }

        private void txtDateCustomsApproval_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtDateCustomsApproval, e); }

        private void txtDateShipped_KeyPress(object sender, KeyPressEventArgs e)
        { Globalitems.CheckDateKeyPress(e); }

        private void txtDateShipped_TextChanged(object sender, EventArgs e)
        { Formops.ChangeControlUpdatedStatus("txtDateShipped", lsControls); }

        private void txtDateShipped_Validating(object sender, CancelEventArgs e)
        { Globalitems.ValidateDate(txtDateShipped, e); }

        private void frmVehDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (strMode != "READ" && !Globalitems.blnException)
            {
                MessageBox.Show("You must SAVE or Cancel the current changes to close this form",
                    "CANNOT CLOSE THIS FORM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void btnNewForwarder_Click(object sender, EventArgs e)
        {OpenFormToAddItem("frmFreightForwarderAdmin");}

        private void btnNewDestination_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet implement. Stay tuned!",
                "UNDER CONSTRUCTION", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNewExporter_Click(object sender, EventArgs e)
        { OpenFormToAddItem("frmExporterAdmin"); }

        private void txtBayLoc_Leave(object sender, EventArgs e)
        {
            int intpos;
            string strBayloc;
            string strPos;

            //Make entry UCase, so aa1 -> AA1
            if (txtBayLoc.Text.Trim().Length > 0)
            {
                strBayloc = txtBayLoc.Text.ToUpper();
                txtBayLoc.Text = strBayloc;

                //Make sure 2 digits after space, e.g. K1 01 not K1 1
                intpos = strBayloc.IndexOf(" ");
                if (intpos < 0)
                    MessageBox.Show("The Bay Loc. is missing the car position, e.g. 01",
                        "INCORRECT BAY LOCATION?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    strPos = strBayloc.Substring(intpos + 1);
                    if (strPos.Length == 1) strPos = "0" + strPos;
                    txtBayLoc.Text = strBayloc.Substring(0,intpos) + " " + strPos;
                }
            }
        }

        private void ckActive_CheckedChanged(object sender, EventArgs e)
        {FillCombos();}

        private void dgInspectionsDamage_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                string strFullNote;
                if ((e.ColumnIndex == this.dgInspectionsDamage.Columns["Note"].Index)
                    && e.Value != null && e.Value.ToString() != "")
                {
                    //Get the full note from col. FullNote
                    strFullNote = this.dgInspectionsDamage.Rows[e.RowIndex].Cells["FullNote"].Value.ToString();
                    DataGridViewCell cell =
                        this.dgInspectionsDamage.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    cell.ToolTipText = strFullNote;                    
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "dgInspectionsDamage_CellFormatting",
                    ex.Message);
            }   
        }

        private void cboBillTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strMode == "MODIFY")
            {
                Formops.ChangeControlUpdatedStatus("cboBillTo", lsControls);
                //Update ckBillTo so BillToInd is properly handled
                if ((cboBillTo.SelectedItem as ComboboxItem).cboValue == "none")
                    ckBillTo.Checked = false;
                else
                    ckBillTo.Checked = true;

                Formops.ChangeControlUpdatedStatus("ckBillTo", lsControls);
            }
        }

        private void txtBillToNote_TextChanged(object sender, EventArgs e)
        { if (strMode == "MODIFY") Formops.ChangeControlUpdatedStatus("txtBillToNote", lsControls); }

        private void frmVehDetail_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
