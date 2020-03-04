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
    public partial class frmPassword : Form
    {
        public frmPassword()
        {
            InitializeComponent();
        }

        private void frmPassword_Load(object sender, EventArgs e)
        {
            lblRules.Text = "1) Cannot be the same as your current Password.\n" +
                "2) Must be at least 4 characters.\n" + 
                "3) Cannot begin with DAI.\n" + 
                "4) Cannot be a sequence of numbers, e.g. 1234.\n" + 
                "5) Password is case sensitive.\n" +
                "6) Cannot be the same character repeated 4 times."; 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidChangeLogin())
                {
                    UpdatePassword();
                    DataOps.CreateLoginRecord();

                    MessageBox.Show("Your Password has been updated", "PASSWORD UPDATED",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Globalitems.blnPasswordChanged = true;
                    this.Close();
                }
            }
            
            catch (Exception ex)
            {
                Globalitems.HandleException("frmPassword", "btnUpdate_Click", ex.Message);
            }
        }

        private void UpdatePassword()
        {
            try
            {
                string strSQL;
                strSQL = "UPDATE Users SET Password = '" + txtNewPwd.Text + "'," +
                    "PasswordUpdatedDate = CURRENT_TIMESTAMP " +
                    "WHERE UserCode = '" + Globalitems.strUserName + "'";
                DataOps.PerformDBOperation(strSQL); 
            }

            catch (Exception ex)
            {
                Globalitems.HandleException("frmPassword", "UpdatePassword", ex.Message); 
            }
        }

        private bool ValidChangeLogin()
        {
            try
            {
                string strValidPwd;

                //Globalitems.strPassword = "9999";

                //Password entered?
                if (txtNewPwd.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter the New Password", "MISSING NEW PASSWORD", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtNewPwd.Focus();
                    return false;
                }
                txtNewPwd.Text = txtNewPwd.Text.Trim();

                //Confirm Password entered?
                if (txtConfirmPwd.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter the Confirm Password", "MISSING CONFIRM PASSWORD", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtConfirmPwd.Focus();
                    return false;
                }
                txtConfirmPwd.Text = txtConfirmPwd.Text.Trim();

                //Ck that Password & Confirm Password match
                if (txtNewPwd.Text != txtConfirmPwd.Text)
                {
                    MessageBox.Show("The New Password and the Confirm Password do not match", 
                        "PASSWORD MISMATCH", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtNewPwd.Focus();
                    return false;
                }

                //Ck that New Password is not the same as the current Password
                if (txtNewPwd.Text == Globalitems.strPassword)
                {
                    MessageBox.Show("The New Password cannot be the same as the current Password.",
                        "PASSWORD NOT CHANGED", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtNewPwd.Focus();
                    return false;
                }

                strValidPwd = Globalitems.validpassword(txtNewPwd.Text);

                if (strValidPwd != "OK")
                {
                    switch (strValidPwd)
                    {
                        case "SHORT":
                            MessageBox.Show("The new Password must be 4 or more characters.",
                                "PASSWORD TOO SHORT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtNewPwd.Focus();
                            break;

                        case "DAI":
                            MessageBox.Show("The new Password cannot begin with DAI.",
                                "PASSWORD STARTS WITH DAI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtNewPwd.Focus();
                            break;

                        case "REPEAT":
                            MessageBox.Show("The new Password cannot repeat the same character more than 3 times.",
                                "PASSWORD REPEATS CHARACTERS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtNewPwd.Focus();
                            break;

                        case "SEQ":
                            MessageBox.Show("The new Password cannot use a numerical sequence (e.g. 1234).",
                                "PASSWORD USES NUMERICAL SEQUENCE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtNewPwd.Focus();
                            break;

                    }
                    return false;
                }
                else
                {
                    Globalitems.strPassword = txtNewPwd.Text;
                    return true;
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException("ValidChangeLogin", "frmPassword", ex.Message);
                return false;
                    
            }
        }

        private void txtNewPwd_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) txtConfirmPwd.Focus();
        }

        private void txtConfirmPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) btnUpdate_Click(null, null);
        }

        private void frmPassword_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
