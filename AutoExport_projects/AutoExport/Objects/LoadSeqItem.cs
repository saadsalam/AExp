using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExport.Objects
{
    class LoadSeqItem
    {
        //To simplify code using CustomerID, store as both string & int
        private string mCustomerID_string;
        private int mCustomerID_int;

        private string mCustomerName;
        private string mDestinationName;

        //To simplify code using Sequence, store as both string & int
        private int mSequence_int = -1;
        private string mSequence_string = "";

        private string mSizeClass;

        public string CustomerID_string
        {
            get { return mCustomerID_string; }
            set
            {
                //Update both string & int values
                mCustomerID_string = value;
                mCustomerID_int = Convert.ToInt32(mCustomerID_string);
            }
        }

        public int CustomerID_int
        {
            get { return mCustomerID_int; }
            set
            {
                //Update both string & int values
                mCustomerID_int = value;
                mCustomerID_string = mCustomerID_int.ToString();
            }
        }

        public string CustomerName
        {
            get { return mCustomerName; }
            set { mCustomerName = value; }
        }

        public string DestinationName
        {
            get { return mDestinationName; }
            set { mDestinationName = value; }
        }

        public string Sequence_string
        {
            get { return mSequence_string; }
            set
            {
                //Update both string & int values
                mSequence_string = value;
                mSequence_int = Convert.ToUInt16(mSequence_string);
            }
        }

        public int Sequence_int
        {
            get { return mSequence_int; }
            set
            {
                //Update both string & int values
                mSequence_int = value;
                mSequence_string = mSequence_int.ToString();
            }
        }

        public string SizeClass
        {
            get { return mSizeClass; }
            set { mSizeClass = value; }
        }

        public LoadSeqItem MakeCopy(LoadSeqItem objOrig)
        {
            LoadSeqItem objCopy = new LoadSeqItem();
            objCopy.CustomerID_int = objOrig.CustomerID_int;
            objCopy.CustomerName = objOrig.CustomerName;
            objCopy.DestinationName = objOrig.DestinationName;
            objCopy.Sequence_int = objOrig.Sequence_int;
            objCopy.SizeClass = objOrig.SizeClass;
            return objCopy;
        }
    }
}
