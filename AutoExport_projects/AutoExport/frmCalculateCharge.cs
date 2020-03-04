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
    public partial class frmCalculateCharge : Form
    {
        private const string CURRENTMODULE = "frmCalculateCharge";

        private DateTime datCalcDate;
        private DateTime datDateReceived;
        private decimal decEntryRate;
        private int intCustomerID;
        private int intEntryRateOverrideInd;
        private int intPerDiemGraceDays;
        private int intVehicleID;
        private string strSizeClass;

        public frmCalculateCharge(string strVIN,
            int intVehID,
            decimal decEtryRate,
            int intCustID,
            int intEtryRateOverride,
            int intPerdiemgrace,
            int intPerDiemGraceDaysOverrideInd,
            string strszeClass,
            DateTime datDateRcvd)

        {
            InitializeComponent();

            //Store params as global variables
            decEntryRate = decEtryRate;
            intCustomerID = intCustID;
            intEntryRateOverrideInd = intEtryRateOverride;
            intPerDiemGraceDays = intPerdiemgrace;
            intVehicleID = intVehID;
            strSizeClass = strszeClass;
            datDateReceived = datDateRcvd;

            Formops.SetFormBackground(this);
            Globalitems.SetControlHeight(this);

            txtDateReceived.Text = datDateRcvd.ToString("M/d/yyyy");
            txtDate.Text = DateTime.Today.ToString("M/d/yyyy");
            txtVIN.Text = strVIN;
            intVehicleID = intVehID;
        }

        private void CalculateCharge()
        {
            //1/15/19 D.Maibor: correct calculation for number of days to charge

            DataTable dtRates;
            DateTime datCurrentChargeDay;
            DataView dv;
            Decimal decTotalCharge;
            int intDaysToCharge;
            string strCurrentChargeDay;
            string strFilter;

            try
            {
                //Make sure txtDate has a value
                if (txtDate.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter the date for the Charges",
                        "MISSING DATE", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtDate.Focus();
                    return;
                }

                //Make sure txtDate is not before DateReceived
                datCalcDate = Convert.ToDateTime(txtDate.Text.Trim());
                if (datCalcDate < datDateReceived)
                {
                    MessageBox.Show("The Charge To Date cannot be BEFORE the Date Received!",
                        "INVALID CHARGE TO DATE", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtDate.Focus();
                    return;
                }

                //If passed in EntryRate=0 and EntryOverrideInd=0, ck the AutoportExportRates table
                //  for EntryRates (same approach as DATS). If EntryRates exist, use the EntryRate
                //  and PerDiemGraceDays from the AutoportExportRates table
                if (decEntryRate==0 && intEntryRateOverrideInd==0)
                {
                    GetSingleRateFromTable();
                    if (decEntryRate == -1)
                    {
                        MessageBox.Show("The Rates table does not have an Entry rate for\n" +
                            "this customer, this Size Class, and this Date Received!",
                            "NO ENTRY RATE FOUND", 
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    if (intPerDiemGraceDays == -1)
                    {
                        MessageBox.Show("The Rates table does not have Per Diem Grace Days for\n" +
                            "this customer, this Size Class, and this Date Received!",
                            "NO PER DIEM GRACE DAYS FOUND",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }

                //Set decTotalCharge to EntryRate;
                decTotalCharge = decEntryRate;

                //Calculate the days to charge
                intDaysToCharge = (int) (datCalcDate - datDateReceived).TotalDays + 1;

                //Display just the Entry Rate if intDaysToCharge <= PerDiemGraceDays
                if (intDaysToCharge <= intPerDiemGraceDays)
                {
                    txtCharge.Text = Globalitems.FormatCurrency(decTotalCharge.ToString());
                    return;
                }

                //The Days to charge is more than the Grace Days
                //  Set CurrentChargeDay to DateReceived + Grace Days
                datCurrentChargeDay = datDateReceived.AddDays(intPerDiemGraceDays);

                //set datCurrentChargeDay to Date only, no Time
                datCurrentChargeDay = datCurrentChargeDay.Date;

                //Load dtRates with all the PerDiemRates
                dtRates = GetAllRatesFromTable();

                //Loop through each day from CurrentChargeDay to CalcDate and add the PerDiem
               while (datCurrentChargeDay <= datCalcDate)
                {
                    //Use a DataView to retrieve the row from dtRates
                    strFilter = "StartDate <= '" + datCurrentChargeDay.ToString() +
                        "' AND EndDate >= '" + datCurrentChargeDay + "'";
                    dv = new DataView(dtRates, strFilter, "StartDate", 
                        DataViewRowState.CurrentRows);

                    //S/B only one rate
                    if (dv.Count != 1 || DBNull.Value.Equals(dv[0]["PerDiem"]))
                    {
                        MessageBox.Show("The Rates table does not have a Per Diem Rate for\n" +
                           "this customer, this Size Class, and the Date: " +
                           datCurrentChargeDay.ToString("M/d/yyyy"),
                           "NO PER DIEM RATE FOUND",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);

                        return;
                    }

                    //Add PerDiem to decTotalCharge
                    decTotalCharge += (Decimal)dv[0]["PerDiem"];

                    datCurrentChargeDay = datCurrentChargeDay.AddDays(1);
                }

                txtCharge.Text = Globalitems.FormatCurrency(decTotalCharge.ToString());
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CalculateCharge", ex.Message);
            }
        }

        private DataTable GetAllRatesFromTable()
        {
            DataSet ds;
            string strSQL;

            try
            {
                strSQL = "SELECT " +
                "PerDiem," +
                "StartDate," +
                "ISNULL(EndDate, '12/31/2099') AS EndDate " +
                "FROM AutoportExportRates " +
                "WHERE CustomerID = " + intCustomerID + 
                " AND RateType = 'Size " + strSizeClass + " Rate' " +
                "ORDER BY StartDate";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                if (ds.Tables.Count==0 || ds.Tables[0].Rows.Count==0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GetAllRatesFromTable",
                        "No data returned from Rates table");
                    return null;
                }

                return ds.Tables[0];
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetAllRatesFromTable", ex.Message);
                return null;
            }
        }

        private void GetSingleRateFromTable()
        {
            DataSet ds;
            string strSQL;

            try
            {
                //Get the EntryRate & PerDiemGraceDays for the DateReceived. 
                //  Note EndDate still applies rate on that 
                //  day. New rate s/b next day after previous row's end date
                strSQL = "SELECT ISNULL(EntryFee,-1) AS EntryFee," +
                    "ISNULL(PerDiemGraceDays,-1) AS  PerDiemGraceDays" +
                    "FROM AutoportExportRates " +
                    "WHERE CustomerID = " + intCustomerID +
                    " AND '" + datDateReceived + "' >= StartDate " +
                    "AND '" + datDateReceived + "' < DATEADD(day, 1,ISNULL(EndDate,'12/31/2099')) " +
                    "AND RateType = 'Size " + strSizeClass + " Rate'";
                ds = DataOps.GetDataset_with_SQL(strSQL);

                //No records found, set decEntryRate to -1, same as a NULL return
                if (ds.Tables[0].Rows.Count == 0)
                {
                    decEntryRate = -1;
                    return;
                }
                decEntryRate = (Decimal) ds.Tables[0].Rows[0]["EntryFee"];
                intPerDiemGraceDays = (int)ds.Tables[0].Rows[0]["PerDiemGraceDays"];
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "RateIsMissing", ex.Message);
                decEntryRate = -1;
            }
        }

        private bool PerDiemRateIsMissing()
        {
            try
            {
                return true;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PerDiemRateIsMissing", ex.Message);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {this.Close();}

        private void txtDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtCharge.Text = "";
            Formops.DateKeyPress(txtDate, e);
            if (e.KeyChar == (char)13) btnSave.Focus();
        }

        private void txtDate_Validating(object sender, CancelEventArgs e)
        {ValidateTextbox(txtDate, e);}

        private void btnSave_Click(object sender, EventArgs e)
        {CalculateCharge();}

        private void frmCalculateCharge_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
