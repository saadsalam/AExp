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
    public partial class frmInvRptParams : Form
    {
        //Inv. types: "POST","PRE", "BOOK"
        //POST: includes Destination; excludes Voyage Date
        //PRE: includes Voyage Date; excludes Destination 
        //BOOK: Exp Booking Records params; Customer, Voyage Date, Not Shipped/Shipped
        //BOOKSUM: Exp Booking Summary params; Customer, Voyage Date (include voyage#)
        //EX: Exp Voy Exceptions; Customer, Voyage Date (different query from BOOK/BOOKSUM
        public string strInvtype;

        private const string CURRENTMODULE = "frmInvrptParams";

        bool blnInitialFill;

        private List<ControlInfo> lsControls_primary = new List<ControlInfo>()
        {
            //Controls in Search Panel, and Detail Panel associated with the Customer table
            new ControlInfo {ControlID="txtVIN",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="cboInspector",ControlPropetyToBind="SelectedValue"},
            new ControlInfo {ControlID="txtFrom",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="txtTo",ControlPropetyToBind="Text"},
            new ControlInfo {ControlID="btnDisplay" },
            new ControlInfo {ControlID="btnSave" },
            new ControlInfo {ControlID="btnCancel" }
        };

        public frmInvRptParams()
        {
            InitializeComponent();
            dgResults.AutoGenerateColumns = false;   
        }

        private void frmInvRptParams_Activated(object sender, EventArgs e)
        {
            try
            {
                if (strInvtype == "BOOK") this.Text = "DAI Export: Booking Records params";
                if (strInvtype == "BOOKSUM") this.Text = "DAI Export: Booking Summary params";
                if (strInvtype == "EX") this.Text = "DAI Export:Voyage Exceptions params";
                if (strInvtype == "PRE") this.Text = "DAI Export:Pre-Ship Inventory params";

                Formops.SetFormBackground(this);
                Globalitems.SetControlHeight(this);

                blnInitialFill = true;

                FillCombos();

                blnInitialFill = false;

                if (strInvtype.Contains("BOOK"))
                {
                    SetUpForBOOKReport();
                    return;
                }

                if (strInvtype.Contains("BOOK") || strInvtype.Contains("EX"))
                {
                    SetUpForBOOKReport();
                }
                else
                {
                    pnlShipped.Visible = false;

                    pnlBatchDate.Visible = true;
                    pnlDest.Visible = true;
                    pnlPhyDate.Visible = true;
                    dgResults.Visible = true;

                    ckActive.Text = "Only ACTIVE Customers && Destinations";

                    //change lblMsg
                    lblMsg.Text = lblMsg.Text.Replace("xxx", strInvtype + "-Ship ");

                    if (strInvtype == "POST")
                    {
                        //Hide Voyage info for POST SHIP Inv. rpt
                        lblVoyage.Visible = false;
                        cboVoyageDate.Visible = false;
                    }
                    else
                    {
                        //Hide Dest info for PRE SHIP Inv. rpt
                        // Shorten height of form
                        ckActive.Text = "Only ACTIVE Customers";
                        pnlDest.Visible = false;
                        btnSave.Top = pnlDest.Top;
                        btnCancel.Top = pnlDest.Top;
                        this.Height = pnlDest.Top + 75;
                    }

                    FillGrid();
                }
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmInvRptParams_Activated",
                    ex.Message);
            }
        }

        private void SetUpForBOOKReport()
        {
            try
            {
                pnlBatchDate.Visible = false;
                pnlShipped.Visible = true;
                pnlDest.Visible = false;
                pnlPhyDate.Visible = false;
                dgResults.Visible = false;

                ckActive.Text = "Only ACTIVE Customers";

                btnSave.Top = pnlBatchDate.Top;
                btnCancel.Top = pnlBatchDate.Top;

                //change lblMsg
                if (strInvtype.Contains("BOOK"))
                {
                    lblMsg.Text = "Select the parameters for the Booking ";

                    if (strInvtype == "BOOK")
                        lblMsg.Text += "Records report.";
                    else
                        lblMsg.Text += "Summary report.";
                }
                else
                {
                    pnlShipped.Visible = false;
                    lblMsg.Text = "Select the parameters for the Voyage Exceptions report. ";
                }
                

                //Adjust height of form to 50 pixels below buttons
                this.Height = btnSave.Top + btnSave.Height + 50;

                //Hide Not Shipped/Shipped if BOOKSUM
                if (strInvtype == "BOOKSUM") pnlShipped.Visible = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetUpForBOOKReport",
                    ex.Message);
            }
        }

        private void FillCombos()
        {
            try
            {
                ComboBox cbo = new ComboBox();
                ComboboxItem cboItem;
                DataSet ds;
                string strFilter;
                string strSQL;

                if (strInvtype != "POST")
                {
                    cboVoyageDate.Items.Clear();

                    if (strInvtype != "EX")
                    {
                        //Limit to top 100 to speed up form display
                        strSQL = @"SELECT TOP 100 AEVoyageID, 
                        CONVERT(varchar(10),voy.VoyageDate,101) + ' ' ";

                        //If BOOKSUM params, add VoyageNumber
                        if (strInvtype == "BOOKSUM")
                            strSQL += @" + '-' + voy.VoyageNumber + ' - ' ";

                        strSQL += @" + ves.VesselName
                        AS voyageinfo
                        FROM AEVoyage voy 
                        LEFT JOIN AEVessel ves on ves.AEVesselID = voy.AEVesselID
                        WHERE voy.VoyageDate IS NOT NULL AND voy.VoyageDate < '1/1/9000' 
                        ORDER BY voy.VoyageDate DESC";
                    }
                    else
                    {
                        //separate query for Voy. Exceptions 
                        strSQL = @"SELECT 
                        DISTINCT ISNULL(CONVERT(varchar(10),voy.VoyageDate,101),'') + ' - ' +
                            voy.VoyageNumber + ' - ' + ves.VesselName AS voyageinfo, 
                        voy.AEVoyageID
                        FROM AEVoyage voy
                        LEFT OUTER JOIN AEVessel ves ON voy.AEVesselID = ves.AEVesselID ";

                        //Add JOIN to AEVoyageCustomer table, if CustomerID is not all
                        if (cboCust.SelectedIndex > 0)
                        {
                            strSQL += @"LEFT OUTER JOIN AEVoyageCustomer voycust 
                                ON voy.AEVoyageID = voycust.AEVoyageID ";
                        }

                        //Add general WHERE clause
                        strSQL += @"WHERE voy.VoyageDate >= 
                            CONVERT(varchar(10),CURRENT_TIMESTAMP,101) AND 
                            voy.VoyageDate < '1/1/3000' ";

                        //Add CustomerID to WHERE clause, if not all
                        if (cboCust.SelectedIndex > 0)
                        {
                            strSQL += "AND voycust.CustomerID = " + 
                                (cboCust.SelectedItem as ComboboxItem).cboValue + " ";
                        }

                        strSQL += "ORDER BY voyageinfo DESC";
                    }
                    

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "FillCombos",
                            "No rows returned from Voyage table");
                        return;
                    }

                    // Add select or all to cbo
                    cboItem = new ComboboxItem();
                    if (strInvtype == "PRE" || strInvtype == "BOOKSUM" || strInvtype == "EX")
                    {
                        cboItem.cboText = "<select>";
                        cboItem.cboValue = "select";
                    }
                    else
                    {
                        cboItem.cboText = "<all>";
                        cboItem.cboValue = "all";
                    }
                    cboVoyageDate.Items.Add(cboItem);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        cboItem = new ComboboxItem();
                        cboItem.cboText = dr["voyageinfo"].ToString();
                        cboItem.cboValue = dr["AEVoyageID"].ToString();
                        cboVoyageDate.Items.Add(cboItem);
                    }

                    cboVoyageDate.DisplayMember = "cboText";
                    cboVoyageDate.ValueMember = "cboValue";
                    cboVoyageDate.SelectedIndex = 0;
                }

                //Load cboCust if blnInitialfill
                if (blnInitialFill)
                {
                    cboCust.Items.Clear();
                    strSQL = @"SELECT CustomerID, " +
                        "CASE WHEN LEN(RTRIM(ISNULL(ShortName,''))) > 0 THEN RTRIM(ShortName) " +
                        "else RTRIM(CustomerName) END AS CustName " +
                        "FROM Customer ";
                    if (ckActive.Checked) strSQL += "WHERE RecordStatus='Active' ";
                    strSQL += "ORDER BY CustName";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "FillCombos",
                            "No data returned from Customer query");
                        return;
                    }

                    //Set 1st item as <all>
                    cboItem = new ComboboxItem();
                    cboItem.cboText = "<all>";
                    cboItem.cboValue = "all";
                    cboCust.Items.Add(cboItem);

                    //Fill remaining items from ds
                    foreach (DataRow drow in ds.Tables[0].Rows)
                    {
                        cboItem = new ComboboxItem();
                        cboItem.cboText = drow["CustName"].ToString();
                        cboItem.cboValue = drow["CustomerID"].ToString();
                        cboCust.Items.Add(cboItem);
                    }

                    cboCust.DisplayMember = "cboText";
                    cboCust.ValueMember = "cboValue";
                    cboCust.SelectedIndex = 0;
                }
               

                //Fill lbDest
                //Store the Destinations in lsDestinations
                //Use FillComboboxFromCodeTable method to put items into
                //  a Combobox
                if (strInvtype == "POST")
                {
                    lbDest.Items.Clear();
                    strFilter = "CodeType='ExportDischargePort' AND Code <> '' ";
                    if (ckActive.Checked) strFilter += "AND RecordStatus='Active'";
                    Globalitems.FillComboboxFromCodeTable(strFilter, cbo, false, false);

                    //Load the values from the cbo into lbDest
                    foreach (ComboboxItem cbDest in cbo.Items)
                        lbDest.Items.Add(cbDest.cboValue);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void FillGrid()
        {
            try
            {
                DataSet ds;
                string strSQL;

                //Clear dgResults
                dgResults.DataSource = null;

                strSQL = @"SELECT 
                BatchID,
                CONVERT(varchar(10),CreationDate,101) AS CreationDate,
                COUNT(BatchID) AS totrecs
                FROM AutoportExportVehiclesImport
                WHERE RecordStatus = 'Bay Location Updated' 
                AND LEN(RTRIM(ISNULL(DestinationName,''))) < 1 
                GROUP BY BatchID,CONVERT(VARCHAR(10),CreationDate,101)
                ORDER BY BatchID DESC";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dgResults.DataSource = ds.Tables[0];
                }                
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillGrid", ex.Message);
            }
        }

        private void KeyPressTextbox(KeyPressEventArgs e)
        {
            if (!Globalitems.ValidDateKeyStroke(e.KeyChar)) e.Handled = true;
        }

        private void PerformSave()
        {
            //Store each param as name/value(s) sets, using
            // Globalitems:
            //chrRecordSeparator to separate each set
            //chrNameValueSeparator to spearate name from value(s)
            //chrNameValueSeparator to separate values
            //E.g. 
            //  CustomerID:8$BatchID:24704,24703,24702$
            //  DateFrom:6/1/2017$DateTo:7/1/17$
            //  Destination: Aqaba,Beirut$ 
            //  ShipStatus: NotShipped$
            try
            {
                string strParams = "";

                if (!strInvtype.Contains("BOOK") && !strInvtype.Contains("EX"))
                {
                    //Ck for Valid params for PRE & POST types
                    if (ValidParams())
                    {


                        //Ck Customer
                        if ((cboCust.SelectedItem as ComboboxItem).cboValue != "all")
                        {
                            strParams = "CustomerID" + Globalitems.chrNameValueSeparator +
                                (cboCust.SelectedItem as ComboboxItem).cboValue +
                                Globalitems.chrRecordSeparator;
                        }

                        //Ck VoyageDate
                        if (strInvtype == "PRE" &&
                            (cboVoyageDate.SelectedItem as ComboboxItem).cboValue != "select")
                        {
                            strParams += "VoyageID" + Globalitems.chrNameValueSeparator +
                                (cboVoyageDate.SelectedItem as ComboboxItem).cboValue +
                                Globalitems.chrRecordSeparator;
                        }

                        //Ck BatchID
                        if (rbBatch.Checked)
                        {
                            strParams += "BatchID" + Globalitems.chrNameValueSeparator;
                            foreach (DataGridViewRow dgRow in dgResults.SelectedRows)
                            {
                                strParams += dgRow.Cells["BatchID"].Value.ToString() +
                                    Globalitems.chrMultiValueSeparator;
                            }

                            //Remove last chrMultiValueSeparator and add chrRecordSeparator
                            strParams = strParams.Remove(strParams.Length - 1) +
                                Globalitems.chrRecordSeparator;
                        }

                        //Ck Dates
                        if (rbDate.Checked)
                        {
                            if (txtFrom.Text.Trim().Length > 0)
                                strParams += "DateFrom" +
                                    Globalitems.chrNameValueSeparator +
                                    txtFrom.Text.Trim() + "$";

                            if (txtTo.Text.Trim().Length > 0)
                                strParams += "DateTo" +
                                    Globalitems.chrNameValueSeparator +
                                    txtTo.Text.Trim() +
                                    Globalitems.chrRecordSeparator;
                        }

                        //Ck Destinations
                        if (strInvtype == "POST" && rbDest.Checked)
                        {
                            strParams += "Destination" + Globalitems.chrNameValueSeparator;
                            foreach (ComboboxItem lbItem in lbDest.SelectedItems)
                            {
                                strParams += lbItem.cboValue +
                                    +Globalitems.chrNameValueSeparator;
                            }

                            //Remove last chrMultiValueSeparator and add chrRecordSeparator
                            strParams = strParams.Remove(strParams.Length - 1) +
                                 Globalitems.chrRecordSeparator;
                        }
                    }
                    else
                        return;
                }
                else
                {
                    //BOOK or BOOKSUM params

                    //Ck Customer
                    if ((cboCust.SelectedItem as ComboboxItem).cboValue != "all")
                    {
                        strParams = "CustomerID" + Globalitems.chrNameValueSeparator +
                            (cboCust.SelectedItem as ComboboxItem).cboValue +
                            Globalitems.chrRecordSeparator;
                        //store text selected to avoid lookup on calling form
                        Globalitems.strTextSelected = 
                            (cboCust.SelectedItem as ComboboxItem).cboText;
                    }

                    //Ck VoyageDate
                    if ((cboVoyageDate.SelectedItem as ComboboxItem).cboValue == "select")
                    {
                        MessageBox.Show("Please select a Voyage.", "MISSING VOYAGE",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                        if ((cboVoyageDate.SelectedItem as ComboboxItem).cboValue != "all")
                    {
                        strParams += "VoyageID" + Globalitems.chrNameValueSeparator +
                            (cboVoyageDate.SelectedItem as ComboboxItem).cboValue +
                            Globalitems.chrRecordSeparator;
                    }

                    //If BOOK, Ck NotShipped/Shipped
                    if (strInvtype == "BOOK")
                    {
                        if (rbNotShipped.Checked)
                            strParams += "ShipStatus" + Globalitems.chrNameValueSeparator +
                                "NotShipped" + Globalitems.chrRecordSeparator;
                        else
                            strParams += "ShipStatus" + Globalitems.chrNameValueSeparator +
                                "Shipped" + Globalitems.chrRecordSeparator;
                    }
                }

                //Remove last '$' in strParams
                strParams = strParams.Remove(strParams.Length - 1);

                //Store strParams in global variable, strSelection, 
                Globalitems.strSelection = strParams;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformSave", ex.Message);
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

        private bool ValidDateRange()
        {
            try
            {
                DateTime datFrom = Convert.ToDateTime("1/1/1800");
                DateTime datTo = Convert.ToDateTime("1/1/3000");

                if (txtFrom.Text.Trim().Length > 0)
                    datFrom = Convert.ToDateTime(txtFrom.Text.Trim());

                if (txtTo.Text.Trim().Length > 0)
                    datTo = Convert.ToDateTime(txtTo.Text.Trim());

                if (datFrom > datTo)
                {
                    MessageBox.Show("The Date Received To Date " +
                        "cannot be BEFORE the Date Received From Date",
                        "INVALID DATE RANGE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFrom.Focus();
                    txtFrom.SelectionStart = 0;
                    txtFrom.SelectionLength = txtFrom.Text.Length;
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidDateRange", ex.Message);
                return false;
            }
        }

        private bool ValidParams()
        {
            try
            {
                //Verify that By Batch or By Date is selected
                if (!rbBatch.Checked && !rbDate.Checked)
                {
                    MessageBox.Show("Please enter By Batch or By Date parameters.",
                            "NO PARAMETERS SELECTED", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    return false;
                }

                //Ck Dates,if selected
                if (rbDate.Checked)
                {
                    //Look for at least one date
                    if (txtFrom.Text.Trim().Length == 0 &&
                        txtTo.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Please enter at least ONE Physical Date",
                            "NO PHYSICAL DATE ENTERED", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return false;
                    }

                    if (!ValidDateRange()) return false;
                }


                if (strInvtype != "POST" &&
                    (cboVoyageDate.SelectedItem as ComboboxItem).cboValue == "select")
                {
                    MessageBox.Show("Please select the Voyage Date for the report",
                            "NO VOYAGE DATE SELECTED", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    return false;
                }

                //Ck that Selected Destinations or All Destinations are ck'd, if POST
                if (strInvtype == "POST" && !rbDest.Checked && !rbAllDest.Checked)
                {
                    MessageBox.Show("Please select Selected Destinations or All Destinations",
                            "NO DESTINATION INFO SELECTED", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    return false;
                }


                //Ck Destinations, if selected
                if (strInvtype == "POST" && rbDest.Checked)
                {
                    if (lbDest.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Please select at least one Destination",
                            "NO DESTINATIONS SELECTED", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return false;
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidParams", ex.Message);
                return false;
            }
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {FillGrid();}

        private void btnCancel_Click(object sender, EventArgs e)
        {this.Close();}

        private void txtFrom_KeyPress(object sender, KeyPressEventArgs e)
        {KeyPressTextbox(e);}

        private void txtTo_KeyPress(object sender, KeyPressEventArgs e)
        { KeyPressTextbox(e); }

        private void txtFrom_Validating(object sender, CancelEventArgs e)
        {ValidateTextbox(txtFrom, e);}

        private void txtTo_Validating(object sender, CancelEventArgs e)
        { ValidateTextbox(txtTo, e); }

        private void btnSave_Click(object sender, EventArgs e)
        {PerformSave();}

        private void ckActive_CheckedChanged(object sender, EventArgs e)
        {FillCombos();}

        private void cboCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            //Refill cboVoyageDate if EX Invtype & User changed cboCust
            if (!blnInitialFill && strInvtype == "EX") FillCombos();
        }

        private void frmInvRptParams_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
