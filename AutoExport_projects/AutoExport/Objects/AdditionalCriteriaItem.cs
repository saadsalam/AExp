using System;
namespace AutoExport.Objects
{

    public class AdditionalCriteriaItem
    {
        private const string CURRENTMODULE = "AdditionalCriteriaItem";

        private bool mblnAddlCriteria = false;
        private string mBillTo;
        private string mCustomsApprovedDateFrom;
        private string mCustomsApprovedDateTo;
        private bool mCustomsApprovedIsBlank = false;
        private string mCustomsExDateFrom;
        private string mCustomsExDateTo;
        private bool mCustomsExIsBlank = false;
        private string mInvDateFrom;
        private string mInvDateTo;
        private bool mInvIsBlank = false;
        private string mInvNumber;
        private string mMake;
        private bool mMechExceptions = false;
        private string mModel;
        private string mMultiVins;
        private bool mNonRunners = false;
        private string mPhysicalDateFrom;
        private string mPhysicalDateTo;
        private bool mPhysicalIsBlank = false;
        private string mRcvdDateFrom;
        private string mRcvdDateTo;
        private bool mRcvdIsBlank = false;
        private string mRcvdExDateFrom;
        private string mRcvdExDateTo;
        private bool mRcvdExIsBlank = false;
        private bool mRerunSearch = false;
        private string mShipDateFrom;
        private string mShipDateTo;
        private bool mShipDateIsBlank = false;
        private bool mSizeClass = false;
        private string mSubDateFrom;
        private string mSubDateTo;
        private bool mSubIsBlank = false;
        private string mYear;

        public bool blnAddlCriteria
        {
            get { return mblnAddlCriteria; }
            set { mblnAddlCriteria = value; }
        }

        public string BillTo
        {
            get { return mBillTo; }
            set { mBillTo = value; }
        }

        public string CustomsApprovedDateFrom
        {
            get { return mCustomsApprovedDateFrom; }
            set { mCustomsApprovedDateFrom = value; }
        }

        public string CustomsApprovedDateTo
        {
            get { return mCustomsApprovedDateTo; }
            set { mCustomsApprovedDateTo = value; }
        }

        public bool CustomsApprovedIsBlank
        {
            get { return mCustomsApprovedIsBlank; }
            set { mCustomsApprovedIsBlank = value; }
        }

        public string CustomsExDateFrom
        {
            get { return mCustomsExDateFrom; }
            set { mCustomsExDateFrom = value; }
        }

        public string CustomsExDateTo
        {
            get { return mCustomsExDateTo; }
            set { mCustomsExDateTo = value; }
        }

        public bool CustomsExIsBlank
        {
            get { return mCustomsExIsBlank; }
            set { mCustomsExIsBlank = value; }
        }
        public string InvDateFrom
        {
            get { return mInvDateFrom; }
            set { mInvDateFrom = value; }
        }

        public string InvDateTo
        {
            get { return mInvDateTo; }
            set { mInvDateTo = value; }
        }

        public bool InvIsBlank
        {
            get { return mInvIsBlank; }
            set { mInvIsBlank = value; }
        }

        public string InvNumber
        {
            get { return mInvNumber; }
            set { mInvNumber = value; }
        }

        public string Make
        {
            get { return mMake; }
            set { mMake = value; }
        }

        public bool MechExceptions
        {
            get { return mMechExceptions; }
            set { mMechExceptions = value; }
        }

        public string Model
        {
            get { return mModel; }
            set { mModel = value; }
        }

        public string MultiVins
        {
            get { return mMultiVins; }
            set { mMultiVins = value; }
        }

        public bool NonRunners
        {
            get { return mNonRunners; }
            set { mNonRunners = value; }
        }

        public string PhysicalDateFrom
        {
            get { return mPhysicalDateFrom; }
            set { mPhysicalDateFrom = value; }
        }

        public string PhysicalDateTo
        {
            get { return mPhysicalDateTo; }
            set { mPhysicalDateTo = value; }
        }

        public bool PhysicalIsBlank
        {
            get { return mPhysicalIsBlank; }
            set { mPhysicalIsBlank = value; }
        }

        public string RcvdDateFrom
        {
            get { return mRcvdDateFrom; }
            set { mRcvdDateFrom = value; }
        }

        public string RcvdDateTo
        {
            get { return mRcvdDateTo; }
            set { mRcvdDateTo = value; }
        }

        public bool RcvdIsBlank
        {
            get { return mRcvdIsBlank; }
            set { mRcvdIsBlank = value; }
        }

        public string RcvdExDateFrom
        {
            get { return mRcvdExDateFrom; }
            set { mRcvdExDateFrom = value; }
        }

        public string RcvdExDateTo
        {
            get { return mRcvdExDateTo; }
            set { mRcvdExDateTo = value; }
        }

        public bool RcvdExIsBlank
        {
            get { return mRcvdExIsBlank; }
            set { mRcvdExIsBlank = value; }
        }

        public bool RerunSearch
        {
            get { return mRerunSearch; }
            set { mRerunSearch = value; }
        }

        public string ShipDateFrom
        {
            get { return mShipDateFrom; }
            set { mShipDateFrom = value; }
        }

        public string ShipDateTo
        {
            get { return mShipDateTo; }
            set { mShipDateTo = value; }
        }

        public bool ShipDateIsBlank
        {
            get { return mShipDateIsBlank; }
            set { mShipDateIsBlank = value; }
        }

        public bool SizeClass
        {
            get { return mSizeClass; }
            set { mSizeClass = value; }
        }

        public string SubDateFrom
        {
            get { return mSubDateFrom; }
            set { mSubDateFrom = value; }
        }

        public string SubDateTo
        {
            get { return mSubDateTo; }
            set { mSubDateTo = value; }
        }

        public bool SubIsBlank
        {
            get { return mSubIsBlank; }
            set { mSubIsBlank = value; }
        }

        public string Year
        {
            get { return mYear; }
            set { mYear = value; }
        }

        public void Initialize()
        {
            //Omit RerunSearch from initialization
            blnAddlCriteria = false;
            BillTo = "";
            CustomsApprovedDateFrom = "";
            CustomsApprovedDateTo = "";
            CustomsApprovedIsBlank = false;
            CustomsExDateFrom = "";
            CustomsExDateTo = "";
            CustomsExIsBlank = false;
            InvDateFrom = ""; 
            InvDateTo = "";
            InvIsBlank = false;
            InvNumber = "";
            Make = "";
            MechExceptions = false;
            Model = ""; 
            MultiVins = "";
            NonRunners = false;
            PhysicalDateFrom = "";
            PhysicalDateTo = "";
            PhysicalIsBlank = false;
            RcvdDateFrom = "";
            RcvdDateTo = "";
            RcvdIsBlank = false;
            RcvdExDateFrom = "";
            RcvdExDateTo = "";
            RcvdExIsBlank = false;
            ShipDateFrom = "";
            ShipDateTo = "";
            SizeClass = false;
            SubDateFrom = "";
            SubDateTo = "";
            SubIsBlank = false;
            Year = "";

        }

        public string CreateWhereClauseForAdditionalCriteria()
        {
            //Loop through the properties in this class. For the string values where value is not NULL add to strWhereClauseAddition.
            //  For the bool values where value is true, add to strWhereClauseAddition 

            // 1/23/18 D.Maibor. Correct Is Blank criteria. 
            string strWhereClauseAddition = "";
            string[] strMultiVINs;
            string strval;
            
            try
            {

                if (BillTo != "")
                {
                    switch (BillTo)
                    {
                        case "blank":
                            strWhereClauseAddition += " AND ISNULL(veh.BillToInd,0) = 0";
                            break;
                        case "notblank":
                            strWhereClauseAddition += " AND ISNULL(veh.BillToInd,0) = 1";
                            break;
                        default:
                            strWhereClauseAddition += " AND ISNULL(veh.BillToInd,0) = 1 " +
                                "AND BillToCustomerID = " + BillTo;
                            break;
                    }
                }

                if (InvDateFrom != "") strWhereClauseAddition += " AND bill.InvoiceDate  >= '" +
                      InvDateFrom + "'";

                if (InvDateTo != "")
                {
                    strWhereClauseAddition += " AND bill.InvoiceDate <= '" +
                        InvDateTo + "'";
                }

                //Revise per Colin, 7/14/17
                //if (InvIsBlank) strWhereClauseAddition += " AND (bill.InvoiceDate IS NULL OR bill.InvoiceDate='')";
                if (InvIsBlank) strWhereClauseAddition += " AND ISNULL(veh.BilledInd,0) = 0";

                if (InvNumber != "") strWhereClauseAddition += " AND bill.InvoiceNumber LIKE '" +
                        InvNumber + "%'";

                if (Make != "") strWhereClauseAddition += " AND veh.Make LIKE '%" + Make + "%'";

                if (MechExceptions) strWhereClauseAddition += " AND veh.MechanicalExceptionInd = 1";

                if (Model != "") strWhereClauseAddition += " AND veh.Model LIKE '%" + Model + "%'";

                //MultiVINS  Stored as a string with comma(',') as separator
                //Use strval to store VINS with OR in clause, (VIN LIKE '%abc%' OR VIN LIKE '%def%')
                if (MultiVins != "")
                {
                    strval = "";
                    strMultiVINs = MultiVins.Split(',');
                    if (strMultiVINs.Length > 0)
                    {
                        strWhereClauseAddition += " AND (";
                        foreach (string strVIN in strMultiVINs)
                        {
                            if (strVIN.Trim().Length > 0)
                            {
                                if (strval.Length > 0) strval += " OR ";
                                strval += "veh.VIN LIKE '%" + strVIN + "%' ";
                            }
                        }

                        strWhereClauseAddition += strval + ") ";
                    }
                }

                if (NonRunners) strWhereClauseAddition += " AND veh.NoStartInd  = 1";

                if (CustomsApprovedDateFrom != "") strWhereClauseAddition += " AND veh.CustomsApprovedDate  >= '" +
                     CustomsApprovedDateFrom + "'";

                if (CustomsApprovedDateTo != "") strWhereClauseAddition += " AND veh.CustomsApprovedDate  <= '" +
                        CustomsApprovedDateTo + "'";

                if (CustomsApprovedIsBlank) strWhereClauseAddition += " AND veh.CustomsApprovedDate IS NULL ";

                if (CustomsExDateFrom != "") strWhereClauseAddition += " AND veh.CustomsExceptionDate  >= '" +
                     CustomsExDateFrom + "'";

                if (CustomsExDateTo != "") strWhereClauseAddition += " AND veh.CustomsExceptionDate  <= '" +
                        CustomsExDateTo + "'";

                if (CustomsExIsBlank) strWhereClauseAddition += " AND veh.CustomsExceptionDate IS NULL ";

                if (PhysicalDateFrom != "") strWhereClauseAddition += " AND veh.LastPhysicalDate  >= '" +
                     PhysicalDateFrom + "'";

                if (PhysicalDateTo != "") strWhereClauseAddition += " AND veh.LastPhysicalDate  <= '" +
                        PhysicalDateTo + "'";

                if (PhysicalIsBlank) strWhereClauseAddition += " AND veh.LastPhysicalDate IS NULL ";

                if (RcvdDateFrom != "") strWhereClauseAddition += " AND veh.DateReceived   >= '" +
                    RcvdDateFrom + "'";

                if (RcvdDateTo != "") strWhereClauseAddition += " AND veh.DateReceived   <= '" +
                        RcvdDateTo + "'";

                if (RcvdIsBlank) strWhereClauseAddition += " AND veh.DateReceived IS NULL ";

                if (RcvdExDateFrom != "") strWhereClauseAddition += " AND veh.ReceivedExceptionDate   >= '" +
                    RcvdExDateFrom + "'";

                if (RcvdExDateTo != "") strWhereClauseAddition += " AND veh.ReceivedExceptionDate   <= '" +
                        RcvdExDateTo + "'";

                if (RcvdExIsBlank) strWhereClauseAddition += " AND veh.ReceivedExceptionDate IS NULL ";

                if (ShipDateFrom != "") strWhereClauseAddition += " AND veh.DateShipped    >= '" +
                   ShipDateFrom + "'";

                if (ShipDateTo != "") strWhereClauseAddition += " AND veh.DateShipped    <= '" +
                        ShipDateTo + "'";

                if (ShipDateIsBlank) strWhereClauseAddition += " AND veh.DateShipped IS NULL ";

                if (SubDateFrom != "") strWhereClauseAddition += " AND veh.DateSubmittedCustoms     >= '" +
                  SubDateFrom + "'";

                if (SubDateTo != "") strWhereClauseAddition += " AND veh.DateSubmittedCustoms     <= '" +
                        SubDateTo + "'";

                if (SubIsBlank) strWhereClauseAddition += " AND veh.DateSubmittedCustoms IS NULL ";

                if (SizeClass) strWhereClauseAddition += " AND (veh.SizeClass IS NULL or veh.SizeClass='')";

                // Note: VehicleYear field in AutoportExportVehicles table is varchar(6)
                if (Year != "") strWhereClauseAddition += " AND veh.VehicleYear LIKE '" + Year + "%'";

                return strWhereClauseAddition;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "Class AddlCriteriaItem.CreateWhereClauseAddition", ex.Message);
                return "";
            }
        }
    }
}
