using AutoExport.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace AutoExport
{
    public partial class frmDamageCodeNote : Form
    {
        private const string CURRENTMODULE = "frmDamageCodeNote";

        public frmDamageCodeNote(string strVIN, string strInspecType,
            string strDamageCode, string strLocationType, string strNote)
        {
            //Use to also display Note for particular VIN from frmEventProcessing when clicking
            //View in DataGrid, strLocationType is blank
            //params used
            //strVIN - VIN
            //strInspecType - Customer
            //strDamageCode - Exception Date
            //strLocationType - blank
            

            char chrSeverity;
            int intpos;
            string strSeverityDesc;

            List<string> lsExcludes = new List<string>
            {{"txtNote"}};

            try
            {
                InitializeComponent();

                //Change form text if displaying Exception Note
                if (strLocationType.Length == 0) this.Text = "Export - Exception Note";

                Formops.SetFormBackground(this);
                Globalitems.SetControlHeight(this, lsExcludes);

                txtVIN.Text = strVIN;
                txtInspectionType.Text = strInspecType;
                txtDamageCode.Text = strDamageCode;

                //If displaying Exception note, re-label InspectionType,DamageCode
                //  and hide Location, Type, Severity
                if (strLocationType.Length == 0)
                {
                    lblInspectionType.Text = "Customer";
                    lblDamageCode.Text = "Excep. Date";
                    lblLocation.Visible = false;
                    txtLocation.Visible = false;
                    lblType.Visible = false;
                    txtType.Visible = false;
                    lblSeverity.Visible = false;
                    txtSevirity.Visible = false;
                }
                else
                {
                    //strLocationType contains Location + '; ' + Damage Type
                    intpos = strLocationType.IndexOf(";");
                    txtLocation.Text = strLocationType.Substring(0, intpos);
                    txtType.Text = strLocationType.Substring(intpos + 2);

                    //Retrieve Servity from Code table
                    chrSeverity = strDamageCode[strDamageCode.Length - 1];
                    strSeverityDesc = Globalitems.GetDamageCodeSeverityDescription(chrSeverity);

                    txtSevirity.Text = strSeverityDesc;
                }

                txtNote.Text = strNote;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmDamageCodeNote", ex.Message);
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void frmDamageCodeNote_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
