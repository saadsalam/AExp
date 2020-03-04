using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExport.Objects
{
    public class PrintInfo
    {
        private int mBatchID = 0;
        private List<int> mSelectedIDs = new List<int>();
        private List<int> mUnprintedIDs = new List<int>();
        private string mMessage = "";

        public int BatchID
        {
            get { return mBatchID; }
            set { mBatchID = value; }
        }

        public string Message
        {
            get { return mMessage; }
            set { mMessage = value; }
        }
        public List<int> SelectedIDs
        {
            get { return mSelectedIDs; }
            set { mSelectedIDs = value; }
        }

        public List<int> UnprintedIDs
        {
            get { return mUnprintedIDs; }
            set { mUnprintedIDs = value; }
        }
    }
}
