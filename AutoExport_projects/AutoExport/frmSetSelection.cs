using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmSetSelection : Form
    {
        //CONSTANTS
        private const string CURRENTMODULE = "frmSetSelection";

        public string strNoteInDB = "";

        private string strItemToSelect;
        public frmSetSelection(string strItem, ComboBox cboSource = null, 
            string strMsg = "",bool blnSetcbo = false,bool blnUseRichtxt = false)
        {
            try
            {
                InitializeComponent();

                this.Text = "DAI Export - Set " + strItem;
                Formops.SetFormBackground(this);

                strItemToSelect = strItem;
                Globalitems.strSelection = "";

                //Hide all three controls
                cbo.Visible = false;
                txt.Visible = false;
                richtxt.Visible = false;

                //cbo selection may include a specific selection msg in strMsg
                //Make cbo, txt, richtxt visible depending on params
                if (cboSource == null)
                {
                    lblMsg.Text = "Please enter the " + strItem;

                    if (blnUseRichtxt)
                    {
                        richtxt.Visible = true;
                        
                        //Move richtxt to top & extend hgt
                        richtxt.Top = txt.Top;
                        richtxt.Height += 25;
                    }
                        
                    else
                        txt.Visible = true;
                }
                else
                {
                    cbo.Visible = true;
                    if (strMsg.Length == 0)
                        lblMsg.Text = "Please select the " + strItem;
                    else
                        lblMsg.Text = strMsg;

                    //Add items from cboSelector to cbo on Form, except "All"
                    foreach (ComboboxItem cboItem in cboSource.Items)
                    {
                        if (cboItem.cboValue != "All")
                            cbo.Items.Add(cboItem);
                    }

                    cbo.DisplayMember = "cboText";
                    cbo.ValueMember = "cboValue";
                    if (blnSetcbo)
                        cbo.SelectedIndex = cboSource.SelectedIndex;
                    else
                        //Set to 1st item if <select> otherwise leave cbo blank
                        if ((cbo.Items[0] as ComboboxItem).cboValue == "select")
                        cbo.SelectedIndex = 0;
                    else
                        cbo.SelectedIndex = -1;

                    //Move cbo down to richtxt if Size Class, because more space need for lbl
                    if (strItem == "Size Class") cbo.Top = richtxt.Top + 20;

                    //Also display richtxt if 
                    if (blnUseRichtxt) richtxt.Visible = true;

                }
                this.DialogResult = DialogResult.Cancel;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmSetSelection", ex.Message);
            }
        }

        private void frmSetSelection_Activated(object sender, EventArgs e)
        {
            ;
            if (cbo.Visible)
                cbo.Focus();
            else
            {
                if (txt.Visible)
                    txt.Focus();
                else
                {
                    richtxt.Text = strNoteInDB;
                    richtxt.Focus();
                }

            }

        }

        private bool ValidSelection()
        {
            try
            {
                if (cbo.Visible)
                {
                    if (cbo.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please select the " + strItemToSelect,
                           "NO " + strItemToSelect.ToUpper() + " SELECTED",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cbo.Focus();
                        return false;
                    }

                    if ((cbo.SelectedItem as ComboboxItem).cboValue == "select")
                    {
                        MessageBox.Show("Please select the " + strItemToSelect,
                           "NO " + strItemToSelect.ToUpper() + " SELECTED",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cbo.Focus();
                        return false;
                    }

                    Globalitems.strSelection = (cbo.SelectedItem as ComboboxItem).cboValue.ToString().Trim();
                }

                if (txt.Visible)
                {
                    if (txt.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Please enter the " + strItemToSelect,
                            "NO " + strItemToSelect.ToUpper() + " ENTERED",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    Globalitems.strSelection = txt.Text.Trim();
                }
                
                
                if (richtxt.Visible)
                {
                    if (richtxt.Text.Trim().Length == 0)
                    {
                        if (strItemToSelect.Contains("Bill To"))
                        {
                            MessageBox.Show("Please enter the Bill To Note",
                                "NO " + strItemToSelect.ToUpper() + " ENTERED",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            richtxt.Focus();
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("Please enter the " + strItemToSelect,
                                "NO " + strItemToSelect.ToUpper() + " ENTERED",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            richtxt.Focus();
                            return false;
                        }
                    } 
                        

                    //store both cbo & richtxt if both visible. Use ~ as delimiter
                    if (cbo.Visible)
                        Globalitems.strSelection = (cbo.SelectedItem as ComboboxItem).cboValue.ToString().Trim() +
                            "~" + richtxt.Text.Trim();
                    else
                        Globalitems.strSelection = richtxt.Text.Trim();
                }

                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidSelection", ex.Message);
                return false;
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidSelection())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void KeyPressTextbox(TextBox txtbox, KeyPressEventArgs e)
        {if (!Globalitems.ValidDateKeyStroke(e.KeyChar)) e.Handled = true;}

        private void frmSetSelection_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
