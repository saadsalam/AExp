using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExport.Objects
{
    class User
    {
        // Set properties to the same names as columns in the datatable retrieved from the PerformSearch method
        //  in frmUserAdmin. 
        private int mUserID;
        private string mUserCode;
        private string mFirstName;
        private string mLastName;
        private string mPassword;
        private string mPIN;
        private string mPhone;
        private string mPhoneExtension;
        private string mCellPhone;
        private string mFaxNumber;
        private string mEmailAddress;
        private decimal mLabelXOffset;
        private decimal mLabelYOffset;
        private string mIMEI;
        private DateTime mLastConnection;
        private int mdatsVersion;
        private string mRecordStatus;
        private DateTime mCreationDate;
        private string mCreatedBy;
        private DateTime mUpdatedDate;
        private string mUpdatedBy;
        private string mEmployeeNumber;
        private string mPortPassIDNumber;
        private string mDepartment;
        private Decimal mStraightTimeRate;
        private Decimal mPieceRateRate;
        private Decimal mPDIRate;
        private Decimal mFlatBenefitPayRate;
        private string mAlternateEmailAddress;
        private DateTime mPasswordUpdatedDate;

        //Added columns in User SQL for Roles, and Action on corresponding record, Add/Modify/Delete
        private string mUserFullName;
        private int mAdmin;
        private int mYard;
        private int mBilling;
        private int mHide;
        private string mDBAction;

      public int UserID
        {
            get { return mUserID; }
            set { mUserID = value; }
        }

      public string UserCode
        {
            get { return mUserCode; }
            set { mUserCode = value; }
        }

      public string FirstName
        {
            get { return mFirstName; }
            set { mFirstName = value; }
        }

      public string LastName
        {
            get { return mLastName; }
            set { mLastName = value; }
        }

        public string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }

        public string PIN
        {
            get { return mPIN; }
        }

        public string Phone
        {
            get { return mPhone; }
            set { mPhone = value; }
        }

        public string PhoneExtension
        {
            get { return mPhoneExtension; }
            set { mPhoneExtension = value; }
        }

        public string CellPhone
        {
            get { return mCellPhone; }
            set { mCellPhone = value; }
        }

        public string FaxNumber
        {
            get { return mFaxNumber; }
            set { mFaxNumber = value; }
        }

        public string EmailAddress
        {
            get { return mEmailAddress; }
            set { mEmailAddress = value; }
        }

      public Decimal LabelXOffset
        {
            get { return mLabelXOffset; }
        }

      public Decimal LabelYOffset
        {
            get { return mLabelYOffset; }
        }

        public string IMEI
        {
            get { return mIMEI; }
        }

      public DateTime LastConnection
        {
            get { return mLastConnection; }
            set { mLastConnection = value; }
        }

      public int datsVersion
        {
            get { return mdatsVersion; }
            set { mdatsVersion = value; }
        }

        public string RecordStatus
        {
            get { return mRecordStatus; }
            set { mRecordStatus = value; }
        }

      public DateTime CreationDate
        {
            get { return mCreationDate; }
            set { mCreationDate = value; }
        }

      public string CreatedBy
        {
            get { return mCreatedBy; }
            set { mCreatedBy = value; }
        }

      public DateTime UpdatedDate
        {
            get { return mUpdatedDate; }
            set { mUpdatedDate = value; }
        }

        public string UpdatedBy
        {
            get { return mUpdatedBy; }
            set { mUpdatedBy = value; }
        }

        public string EmployeeNumber
        {
            get { return mEmployeeNumber; }
            set { mEmployeeNumber = value; }
        }

        public string PortPassIDNumber
        {
            get { return mPortPassIDNumber; }
            set { mPortPassIDNumber = value; }
        }

        public string Department
        {
            get { return mDepartment; }
            set { mDepartment = value; }
        }

      public Decimal StraightTimeRate
        {
            get { return mStraightTimeRate; }
            set { mStraightTimeRate = value; }
        }

      public Decimal PieceRateRate
        {
            get { return mPieceRateRate; }
            set { mPieceRateRate = value; }
        }

      public Decimal PDIRate
        {
            get { return mPDIRate; }
            set { mPDIRate = value; }
        }

      public Decimal FlatBenefitPayRate
        {
            get { return mFlatBenefitPayRate; }
            set { mFlatBenefitPayRate = value; }
        }

        public string AlternateEmailAddress
        {
            get { return mAlternateEmailAddress; }
            set { mAlternateEmailAddress = value; }
        }

      public DateTime PasswordUpdatedDate
        {
            get { return mPasswordUpdatedDate; }
            set { mPasswordUpdatedDate = value; }
        }

        public string UserFullName
        {
            get { return mUserFullName; }
            set { mUserFullName = value; }
        }

        public int Admin
        {
            get { return mAdmin; }
            set { mAdmin = value; }
        }

        public int Yard
        {
            get { return mYard; }
            set { mYard = value; }
        }

        public int Billing
        {
            get { return mBilling; }
            set { mBilling = value; }
        }

        public int Hide
        {
            get { return mHide; }
            set { mHide = value; }
        }
        public string DBAction
        {
            get { return mDBAction; }
            set { mDBAction = value; }
        }
    }
}
