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
    public partial class frmSelectBatch : Form
    {
        private const string CURRENTMODULE = "frmSelectBatch";

        public string strImportType;

        private List<ControlInfo> lsControls = new List<ControlInfo>()
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

        public frmSelectBatch()
        {
            InitializeComponent();
            Formops.SetFormBackground(this);
            Globalitems.SetControlHeight(this);
            Formops.SetTabIndex(this, lsControls);
            dgResults.AutoGenerateColumns = false;
        }

        private void frmSelectBatch_Activated(object sender, EventArgs e)
        {
            //Hide ccontrols, dgResults column, depending on Import type
            switch (strImportType)
            {
                case "RCVD":
                    FillCombos();
                    this.Text = "Select Batch for Import Vehicle";
                    break;
                case "CLONE":
                    lblFilterInspector.Visible = false;
                    cboInspector.Visible = false;
                    lblFilterVIN.Visible = false;
                    txtVIN.Visible = false;
                    dgResults.Columns["Inspector"].Visible = false;
                    this.Text = "Select Batch for Import Phy. Clone";
                    break;
                case "SHIP":
                    lblFilterInspector.Visible = false;
                    cboInspector.Visible = false;
                    lblFilterDate.Visible = false;
                    lblFrom.Visible = false;
                    txtFrom.Visible = false;
                    lblTo.Visible = false;
                    txtTo.Visible = false;
                    dgResults.Columns["Inspector"].Visible = false;
                    this.Text = "Select Batch for Import Shipped";
                    break;
            }

            if (Globalitems.runmode == "TEST") this.Text = "TEST - " + this.Text;

            btnSave.Enabled = false;
            lblBatches.Text = "";
        }

        private void FillCombos()
        {
            try
            {
                ComboboxItem cboItem;
                DataSet ds;
                string strSQL;
                DateTime today = DateTime.Now;

                //Load with current year users at top of list, than all other
                strSQL = @"WITH CTE AS
                    (select DISTINCT imp.Inspector, RTRIM(Users.LastName) + ', ' + 
                    RTRIM(Users.FirstName) AS name
                    from AutoportExportVehiclesImport imp
                    INNER JOIN Users on Users.UserCode=imp.Inspector
                    WHERE LEN(RTRIM(ISNULL(Inspector,''))) > 0
                    AND imp.CreationDate > '1/1/" + today.Year + @"')
                SELECT 1 AS list,* FROM CTE
                UNION
                select DISTINCT 2 AS list, imp.Inspector, RTRIM(Users.LastName) + ', ' + RTRIM(Users.FirstName) AS name
                from AutoportExportVehiclesImport imp
                INNER JOIN Users on Users.UserCode=imp.Inspector
                WHERE LEN(RTRIM(ISNULL(Inspector,''))) > 0
                ORDER BY list,name";


                //strSQL = @"select DISTINCT 2 AS list, 
                //imp.Inspector, RTRIM(Users.LastName) + ', ' + RTRIM(Users.FirstName) AS name
                //from AutoportExportVehiclesImport imp
                //INNER JOIN Users on Users.UserCode = imp.Inspector
                //WHERE LEN(RTRIM(ISNULL(Inspector, ''))) > 0
                //ORDER BY name";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "FillCombos",
                        "No data returned from query");
                    return;
                }

                //Set 1st item as <all>
                cboItem = new ComboboxItem();
                cboItem.cboText = "<all>";
                cboItem.cboValue = "all";
                cboInspector.Items.Add(cboItem);

                //Fill remaining items from ds
                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    cboItem = new ComboboxItem();
                    cboItem.cboText = drow["name"].ToString();
                    cboItem.cboValue = drow["Inspector"].ToString();
                    cboInspector.Items.Add(cboItem);
                }

                cboInspector.DisplayMember = "cboText";
                cboInspector.ValueMember = "cboValue";
                cboInspector.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FillCombos", ex.Message);
            }
        }

        private void FillGrid()
        //4/12/18 D.Maibor: modify CLONE query
        {
            try
            {
                DataSet ds;
                string strSQL = "";

                //Check date range
                if (!ValidDateRange()) return;

                //Clear dgResults
                dgResults.DataSource = null;
                lblBatches.Text = "";

                //Setup SQL based on Import type
                switch (strImportType)
                {
                    case "RCVD":
                        strSQL = @"SELECT
                        BatchID,";
                        //CONVERT(varchar(10),CreationDate,101) AS CreationDate,
                        strSQL += @"Cast(CreationDate AS Date) AS CreationDate,
                        Inspector,
                        COUNT(BatchID) AS totrecs
                        FROM AutoportExportVehiclesImport
                        WHERE LEN(RTRIM(ISNULL(DestinationName,''))) > 0 ";

                        //Ck VIN
                        if (txtVIN.Text.Trim().Length > 0)
                            strSQL += "AND VIN LIKE '%" + txtVIN.Text.Trim() + "%' ";

                        //Ck Inspector
                        if ((cboInspector.SelectedItem as ComboboxItem).cboValue != "all")
                            strSQL += "AND Inspector = '" +
                                (cboInspector.SelectedItem as ComboboxItem).cboValue + "' ";

                        //Ck Date Rcv'd From
                        if (txtFrom.Text.Trim().Length > 0)
                            strSQL += "AND CreationDate >= '" + txtFrom.Text.Trim() + "' ";

                        //Date Rcv'd To
                        if (txtTo.Text.Trim().Length > 0)
                            strSQL += "AND CreationDate < DATEADD(day,1,'" + 
                                txtTo.Text.Trim() + "') ";

                        strSQL += @"GROUP BY BatchID,Cast(CreationDate AS Date),
                            Inspector
                            ORDER BY BatchID DESC";
                        break;
                    case "CLONE":
                        strSQL = @"SELECT 
                        BatchID,
                        CAST(CreationDate AS Date) AS CreationDate,
                        COUNT(BatchID) AS totrecs
                        FROM AutoportExportVehiclesImport
                        WHERE DestinationName IS NULL AND VIN IN
                        (SELECT VINNumberAndVINKey FROM DataHub.dbo.DAIYMS_Output_ExpPhyClone where BatchID is not null
                        and SentToDATSDate is not null)
                        GROUP BY BatchID,CAST(CreationDate AS Date)
                        ORDER BY BatchID DESC";
                        break;
                    case "SHIP":
                        strSQL = @"SELECT
                        BatchID,
                        CreationDate,
                        COUNT(BatchID) AS totrecs
                        FROM AutoportExportShippedVehiclesImport ";

                        //Ck VIN
                        //Ck VIN
                        if (txtVIN.Text.Trim().Length > 0)
                            strSQL += "WHERE VIN LIKE '%" + txtVIN.Text.Trim() + "%' ";

                        strSQL += @"GROUP BY BatchID, 
                        CreationDate
                        ORDER BY BatchID DESC";
                        break;
                }
                
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dgResults.DataSource = ds.Tables[0];
                    btnSave.Enabled = true;
                    lblBatches.Visible = true;
                    lblBatches.Text = "Batches: " + ds.Tables[0].Rows.Count.ToString("#,##0");
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
            //Store BatchID in global variable, strSelection
            Globalitems.strSelection =
                dgResults.SelectedRows[0].Cells["BatchID"].Value.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();   
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
                        MessageBoxButtons.OK,MessageBoxIcon.Error);
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

        private void frmSelectBatch_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
