using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmMain : Form
    {
        private const string CURRENTMODULE = "frmMain";
        private bool blnExitingApp = true;
        private bool blnExitStarted = false;
        private bool blnExitClicked = false;
        private bool blnOKToCloseForm = true;

        public frmMain()
        //1/24/18 D.Maibor: Add CheckForEventProcessingData
        {
            InitializeComponent();
            lblFullName.Text = Globalitems.strUserFullName;
            if (Globalitems.runmode == "TEST") this.Text = "TEST - " + this.Text;

            // Display Version if IsNetworkDeployed; doesn't work in debug mode
            if (ApplicationDeployment.IsNetworkDeployed)
                this.Text = this.Text + " (Ver: " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4) + ")";

            if (!Globalitems.blnAdmin) adminToolStripMenuItem.Enabled = false;
            if (!Globalitems.strRoleNames.Contains("Billing")) billingToolStripMenuItem.Enabled = false;
            if (Globalitems.runmode == "TEST") refreshDataToolStripMenuItem.Visible = true;

            if (!HasBarcodeFont())
                MessageBox.Show("This program requires the Font: 3 of 9 Barcode " +
                    "to print items with barcodes.\n\n" +
                    "Please contact the IT dept. to install it.",
                    "MISSING 3 OF 9 BARCODE FONT", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            Globalitems.StartActivityTimer();

            CheckForEventProcessingData();
        }

        private void 
            CheckForEventProcessingData()
        {
            try
            {
                DataSet ds;
                frmEventProcessing frm;
                string strMsg;
                string strSQL;

                strSQL = "SELECT * FROM EventProcessingRecoveryData WHERE UserCode = '" + Globalitems.strUserName + "'";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
                {
                    strMsg = "You have Pending Event Processing data from the last Program Time Out.\n\n";
                    strMsg += "Do you want to restore the data ?\n\n";
                    strMsg += "NOTE: you may want to check the current Veh. Status of the VINS before Processing the events.";
                    if (MessageBox.Show(strMsg,"RESTORE EVENT PROCESSING", 
                        MessageBoxButtons.YesNo,MessageBoxIcon.Hand) == DialogResult.Yes)
                    {
                        frm = new frmEventProcessing(ds.Tables[0]);
                        Formops.SetFormBackground(frm);
                        frm.Show();
                    }

                    strSQL = "DELETE EventProcessingRecoveryData WHERE UserCode = '" + Globalitems.strUserName + "'";
                    DataOps.PerformDBOperation(strSQL);
                }

            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CheckForEventProcessingData", ex.Message);
            }
        }


        private bool HasBarcodeFont()
        {
            //Check if system has the font 3 of 9 Barcode 
            try
            {
                System.Drawing.FontFamily barcodefont =
                    new System.Drawing.FontFamily("3 of 9 Barcode");
                return true;
            }

            catch (Exception)
            { return false; }
        }

        private void RefreshData()
        {
            frmRefreshData frm = new frmRefreshData();
            frm.ShowDialog();
        }

        private void addNewRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVehDetail frm;
            try
            {
                //If frmVehSearch is already open, Activate it
                if (Application.OpenForms.OfType<frmVehDetail>().Count() == 1)
                {
                    frm = (frmVehDetail)Application.OpenForms["frmVehDetail"];
                    frm.strMode = "NEW";
                    Formops.SetActiveForm("frmVehDetail");
                }
                else
                {
                    frm = new frmVehDetail();
                    frm.strMode = "NEW";
                    Formops.OpenNewForm(frm);
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException("frmMain", "editRecordsToolStripMenuItem_Click", ex.Message);
            }
        }

        private void importExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmImportExportMenu frm;
            try
            {
                //If frmAdd is already open, Activate it
                if (Application.OpenForms.OfType<frmImportExportMenu>().Count() == 1)
                {
                    Formops.SetActiveForm("frmImportExportMenu");
                }
                else
                {
                    frm = new frmImportExportMenu();
                    Formops.OpenNewForm(frm);
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException("frmMain", "importExportToolStripMenuItem_Click", ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                blnExitClicked = true;

                //If forms open, ck w/User to exit
                if (Application.OpenForms.Count > 2)
                {
                    OKToCloseForm();
                    if (!blnOKToCloseForm)
                    {
                        blnExitClicked = false;
                        return;
                    }
                }

                Application.Exit();
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "exitToolStripMenuItem_Click", 
                    ex.Message);
            }
        }

        private void OKToCloseForm()
        {
            try
            {
                string strMsg = "Are you sure you want to Exit and\nclose all the Open Forms?";

                if (!blnExitingApp)
                    strMsg = "Are you sure you want to Log out and\nclose all the Open Forms?";

                // Don't use frmAreYouSure, so Users don't habitually ignore it
                //frmAreYouSure frmConfirm = new frmAreYouSure(strMsg);
                //DialogResult dlResult = frmConfirm.ShowDialog();

                DialogResult dlResult = MessageBox.Show(strMsg, "OK TO CLOSE",
                    MessageBoxButtons.OKCancel,MessageBoxIcon.Question);

                if (dlResult == DialogResult.OK)
                    blnOKToCloseForm = true;
                else
                   blnOKToCloseForm = false;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmMain_FormClosing", ex.Message);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!blnExitingApp || blnExitStarted || blnExitClicked) return;

            //If forms open, ck w/User to exit
            if (Application.OpenForms.Count > 2 && !Globalitems.blnException)
            {
                OKToCloseForm();
                if (!blnOKToCloseForm)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (blnExitingApp)
            {
                blnExitStarted = true;
                Application.Exit();
            }
        }

        private void userAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserAdmin frm;
            try
            {
                //If frmAdd is already open, Activate it
                if (Application.OpenForms.OfType<frmUserAdmin>().Count() == 1)
                {
                    Formops.SetActiveForm("frmUserAdmin");
                }
                else
                {
                    frm = new frmUserAdmin();
                    Formops.OpenNewForm(frm);
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException("frmMain", "importExportToolStripMenuItem_Click", ex.Message);
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Create a List of each open form except frmLogin, frmMain
                List<Form> OpenForms = new List<Form>();

                blnExitingApp = false;
                blnOKToCloseForm = true;

                //Ask are you sure if open forms > 2 (Login & Menu forms)
                if (Application.OpenForms.Count > 2)
                {
                    OKToCloseForm();
                    if (!blnOKToCloseForm)
                    {
                        //Reset blnExitingApp to true, in case User closes for or clicks Exit
                        blnExitingApp = true;
                        blnOKToCloseForm = true;
                        return;
                    }  
                }

                foreach (Form frm in Application.OpenForms)
                {
                    // Can't close each form here, causes error in foreach loop, with change to OpenForms
                    // Can't close frmLogin here, causes app to exit
                    if (frm.Name != "frmLogin" && frm.Name != "frmMain") OpenForms.Add(frm);
                }

                // Close each form in OpenForms
                foreach (Form frm in OpenForms) frm.Close();

                blnExitingApp = true;
                blnOKToCloseForm = true;

                Globalitems.strUserName = "";
                Globalitems.strUserFullName = "";
                Globalitems.MainForm.Hide();

                Globalitems.LoginForm.Show();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException("frmMain", "logOutToolStripMenuItem_Click", ex.Message);
            }
        }

        private void customerAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCustomerAdmin frm;
            try
            {
                //If frm is already open, Activate it
                if (Application.OpenForms.OfType<frmCustomerAdmin>().Count() == 1)
                {
                    Formops.SetActiveForm("frmCustomerAdmin");
                }
                else
                {
                    frm = new frmCustomerAdmin();
                    Formops.OpenNewForm(frm);
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException("frmMain", "customerAdminToolStripMenuItem_Click", ex.Message);
            }
        }

        private void voyageAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVoyageAdmin frm;
            try
            {
                //If frm is already open, Activate it
                if (Application.OpenForms.OfType<frmVoyageAdmin>().Count() == 1)
                {
                    Formops.SetActiveForm("frmVoyageAdmin");
                }
                else
                {
                    frm = new frmVoyageAdmin();
                    Formops.OpenNewForm(frm);
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException("frmMain", "customerAdminToolStripMenuItem_Click", ex.Message);
            }
        }

        private void vesselAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVesselAdmin frm;
            try
            {
                //If frm is already open, Activate it
                if (Application.OpenForms.OfType<frmVesselAdmin>().Count() == 1)
                {
                    Formops.SetActiveForm("frmVesselAdmin");
                }
                else
                {
                    frm = new frmVesselAdmin();
                    Formops.OpenNewForm(frm);
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException("frmMain", "customerAdminToolStripMenuItem_Click", ex.Message);
            }
        }

        private void destinationAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDestinationAdmin frm;
            try
            {
                //If frm is already open, Activate it
                if (Application.OpenForms.OfType<frmDestinationAdmin>().Count() == 1)
                {
                    Formops.SetActiveForm("frmDestinationAdmin");
                }
                else
                {
                    frm = new frmDestinationAdmin();
                    Formops.OpenNewForm(frm);
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException("frmMain", "destinationAdminToolStripMenuItem_Click", ex.Message);
            }
        }

        private void vehLocatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVehSearch frm;
            try
            {
                //If frm is already open, Activate it
                if (Application.OpenForms.OfType<frmVehSearch>().Count() == 1)
                {
                    Formops.SetActiveForm("frmVehSearch");
                }
                else
                {
                    frm = new frmVehSearch();
                    Formops.OpenNewForm(frm);
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException("frmMain", "customerAdminToolStripMenuItem_Click", ex.Message);
            }
        }

        private void freightForwarderAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFreightForwarderAdmin frm;
            try
            {
                //If frm is already open, Activate it
                if (Application.OpenForms.OfType<frmFreightForwarderAdmin>().Count() == 1)
                {
                    Formops.SetActiveForm("frmFreightForwarderAdmin");
                }
                else
                {
                    frm = new frmFreightForwarderAdmin();
                    Formops.OpenNewForm(frm);
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException("frmMain", "customerAdminToolStripMenuItem_Click", ex.Message);
            }
        }

        private void exporterAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExporterAdmin frm;
            try
            {
                //If frm is already open, Activate it
                if (Application.OpenForms.OfType<frmExporterAdmin>().Count() == 1)
                {
                    Formops.SetActiveForm("frmExporterAdmin");
                }
                else
                {
                    frm = new frmExporterAdmin();
                    Formops.OpenNewForm(frm);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "exporterAdminToolsStripMenuItem",
                    ex.Message);
            }
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReports frm;
            try
            {
                //If frm is already open, Activate it
                if (Application.OpenForms.OfType<frmReports>().Count() == 1)
                {
                    Formops.SetActiveForm("frmReports");
                }
                else
                {
                    frm = new frmReports();
                    Formops.OpenNewForm(frm);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "exporterAdminToolsStripMenuItem",
                    ex.Message);
            }
        }

        private void eventProcessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //12/22/17 D.Maibor: OK w/John, remove opening frmEventProcessing in modal form
            frmEventProcessing frm;
            try
            {
                //If frm is already open, Activate it
                if (Application.OpenForms.OfType<frmEventProcessing>().Count() == 1)
                {
                    Formops.SetActiveForm("frmEventProcessing");
                }
                else
                {
                    frm = new frmEventProcessing();
                    Formops.OpenNewForm(frm);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "exporterAdminToolsStripMenuItem",
                    ex.Message);
            }
        }

        private void generateInvoicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGenerateInvoices frm;
            try
            {
                //If frm is already open, Activate it
                if (Application.OpenForms.OfType<frmGenerateInvoices>().Count() == 1)
                {
                    Formops.SetActiveForm("frmGenerateInvoices");
                }
                else
                {
                    frm = new frmGenerateInvoices();
                    Formops.OpenNewForm(frm);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "exporterAdminToolsStripMenuItem",
                    ex.Message);
            }
        }

        private void printInvoicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPrintInvoices frm;

            try
            {
                frm = new frmPrintInvoices();
                frm.ShowDialog();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "printInvoicesToolStripMenuItem_Click", ex.Message);
            }
        }

        private void exportBillingRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPrintInvoices frm;

            try
            {
                frm = new frmPrintInvoices("EXPORT");
                frm.ShowDialog();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "exportToolStripMenuItem_Click", ex.Message);
            }
        }

        private void sheetPrinterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Globalitems.SetSheetPrinter();
        }

        private void refreshDataToolStripMenuItem_Click(object sender, EventArgs e)
        {RefreshData();}

        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        { Globalitems.ResetActivityTimer(); }

        private void mnuMain_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
