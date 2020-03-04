using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using System.Configuration;

namespace AutoExport
{
    public partial class frmLogin : Form
    {
        const string CURRENTMODULE = "frmLogin";

        bool blnKeepCredentials = false;
        string strLoginResult = "";

        public frmLogin()
        {
            InitializeComponent();

            //Read appSettings from app.config file
            var appSettings = ConfigurationManager.AppSettings;
            Globalitems.runmode = appSettings["runmode"];

            Globalitems.LoginForm = this;
            Globalitems.SetUpGlobalVariables();
            Globalitems.SetControlHeight(this);
            Formops.SetFormBackground(this);

            //Set strOSVersion
            // NT 6.1 = Windows 7
            // NT 10.0 = Windows 10
            if (Environment.OSVersion.ToString().Contains("NT 6.1"))
                Globalitems.strOSVersion = "WINDOWS 7";
            else
                Globalitems.strOSVersion = "WINDOWS 10";
        }
        
        public void CloseApp()
        {Application.Exit();}

        private void btnQuit_Click(object sender, EventArgs e)
        {CloseApp();}

        private void btnLogin_Click(object sender, EventArgs e)
        {PerformLogin();}

        private void PerformLogin()
        {
            try
            {
                strLoginResult = "";

                Globalitems.strRoleNames.Clear();

                if (ValidLogin())
                {
                    //Close MainForm so new name appears for user
                    if (Globalitems.MainForm != null) Globalitems.MainForm.Close();

                    Globalitems.MainForm = new frmMain();
                    Globalitems.MainForm.Show();

                    // Need to hide initial form. Closing it causes app to exit
                    this.Hide();
                    return;
                }

                // Not ValidLogin, check strLoginResult
                if (strLoginResult == "FIRST" || strLoginResult == "CHANGE")
                {
                    ChangeLogin();
                    return;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerformLogin", ex.Message);
                MessageBox.Show(Globalitems.EXCEPTION_MESSAGE, Globalitems.EXCEPTION_SUBJECT);
            }
        }

        private void ChangeLogin()
        {
            try
            {
                frmPassword frm = new frmPassword();
                Formops.SetFormBackground(frm);
                frm.ShowDialog();

                // Open Main Form is User updated Password
                if (Globalitems.blnPasswordChanged)
                {
                    this.Hide();
                    Globalitems.MainForm = new frmMain();
                    Globalitems.MainForm.Show();
                }
            }

            catch(Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FirstLogin", ex.Message);
            }
        }

        private bool ValidLogin()
        {
            var appSettings = ConfigurationManager.AppSettings;
            DataSet dsLogin = null;
            DateTime dtbuilddate = Convert.ToDateTime("1/1/1800");
            string strHostName = "";
            frmLockout frm_lockout;
            PrinterSettings prSettings;

            try
            {
               
                string strSProc;
                string strResult;

                strHostName = System.Environment.GetEnvironmentVariable("COMPUTERNAME"); 
                strLoginResult = "";                    
                dtbuilddate = Globalitems.GetBuildDate(Assembly.GetExecutingAssembly());

                SProcParameter Paramobj;
                List<SProcParameter> Paramobjects = new List<SProcParameter>();

                //User Name entered?
                if (string.IsNullOrWhiteSpace(txtUserName.Text))
                {
                    MessageBox.Show("Please enter the User Name", "MISSING USER NAME", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtUserName.Focus();
                    return false;
                }
                txtUserName.Text = txtUserName.Text.Trim();

                //Password entered?
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please enter the Password", "MISSING PASSWORD", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtPassword.Focus();
                    return false;
                }
                txtPassword.Text = txtPassword.Text.Trim();

                //Store UserName & Password globally
                Globalitems.strUserName = txtUserName.Text;
                Globalitems.strPassword = txtPassword.Text;

                //Check DB if valid credentials
                strSProc = "spAutoExportLogin";

                Paramobj = new SProcParameter();
                Paramobj.Paramname = "@userID";
                Paramobj.Paramvalue = Globalitems.strUserName;
                Paramobjects.Add(Paramobj);

                Paramobj = new SProcParameter();
                Paramobj.Paramname = "@password";
                Paramobj.Paramvalue = Globalitems.strPassword;
                Paramobjects.Add(Paramobj);

                Paramobj = new SProcParameter();
                Paramobj.Paramname = "@HostName";
                Paramobj.Paramvalue = strHostName;
                Paramobjects.Add(Paramobj);

                Paramobj = new SProcParameter();
                Paramobj.Paramname = "@BuildDate";
                Paramobj.Paramvalue = dtbuilddate;
                Paramobjects.Add(Paramobj);

                dsLogin = DataOps.GetDataset_with_SProc(strSProc, Paramobjects);
                if (Globalitems.blnException)
                {
                    MessageBox.Show(Globalitems.EXCEPTION_MESSAGE, Globalitems.EXCEPTION_SUBJECT,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (dsLogin == null || dsLogin.Tables.Count == 0 || dsLogin.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show(Globalitems.EXCEPTION_MESSAGE, Globalitems.EXCEPTION_SUBJECT,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
                if (dsLogin.Tables[0].Rows[0]["result"].ToString() != "OK")
                {
                    strResult = dsLogin.Tables[0].Rows[0]["result"].ToString();
                    if (strResult == "FIRST" || strResult == "CHANGE")
                    {
                        strLoginResult = strResult;
                        return false;
                    }

                    //Replace ~ with \r\n to implement new line in Messagebox. Doesn't work if \r\n is embedded
                    //  in string returned as "result" field
                    strResult = strResult.Replace("~", "\r\n");

                    //Open frmLockout if result contains 'locked out'
                    if (strResult.Contains("locked out"))
                    {
                        Globalitems.strLockoutMsg = strResult;
                        frm_lockout = new frmLockout();
                        this.Hide();
                        frm_lockout.Show();
                        return false;
                    }

                    blnKeepCredentials = true;
                    MessageBox.Show(strResult, "INVALID CREDENTIALS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUserName.Focus();
                    return false;
                }
                
                // Store RoleNames
                foreach (DataRow dr in dsLogin.Tables[0].Rows)
                    Globalitems.strRoleNames.Add(dr["RoleName"].ToString());

                //Check if Admin
                if (Globalitems.strRoleNames.Contains("Administrator"))
                    Globalitems.blnAdmin = true;

                // Store FullName
                Globalitems.strUserFullName = dsLogin.Tables[0].Rows[0]["FullName"].ToString();
                
                // If default printer is not a Wasp or Zebra, assume not a label printer,
                //  set blnCannotPrintLabels to true, and set strSheetprinter to the default;
                //  otherwise set strLabelPrinter to the default printer, 
                //      prompt for sheet printer, & update app.config with the sheet printer
                prSettings = new PrinterSettings();
                if (!prSettings.PrinterName.Contains("Wasp") &&
                    !prSettings.PrinterName.Contains("ZT"))
                {
                    Globalitems.blnCannotPrintLabels = true;
                    Globalitems.strSheetPrinter = prSettings.PrinterName;
                }
                else
                {
                    //Set strLabelPrinter to the default printer
                    Globalitems.strLabelPrinter = prSettings.PrinterName;

                    //If app.config has a SheetPrinter value, use that
                    if (appSettings["SheetPrinter"].ToString().Length > 0)
                        Globalitems.strSheetPrinter = appSettings["SheetPrinter"];
                    else
                        Globalitems.SetSheetPrinter();
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(nameof(frmLogin), nameof(ValidLogin), ex.Message);
                return false;
            }
            return true;
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            //When user presses Enter key, treat as btnLogin_Click 
            if (e.KeyData == Keys.Enter) btnLogin_Click(null, null);
        }

        private void frmLogin_Activated(object sender, EventArgs e)
        {
            //Initialize if current User clicks Log Out from frmMain, rather than invalid login
            if (!blnKeepCredentials)
            {
                txtUserName.Text = "";
                txtPassword.Text = "";
                txtUserName.Focus();
            }
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            //When user presses Enter key, treat as btnLogin_Click 
            if (e.KeyData == Keys.Enter) txtPassword.Focus();
        }
    }
}
