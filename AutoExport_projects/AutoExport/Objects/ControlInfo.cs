using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExport.Objects
{
    class ControlInfo
    {
        private string mControlID;
        private string mControlPropertyToBind = "";
        private string mRecordFieldName;
        private string mHeaderText;
        private bool mReadOnly = false;
        private bool mUpdated = false;

        public string ControlID
        {
            get { return mControlID; }
            set { mControlID = value; }
        }

        public string ControlPropetyToBind
        {
            get { return mControlPropertyToBind; }
            set { mControlPropertyToBind = value; }
        }

        public string RecordFieldName
        {
            get { return mRecordFieldName; }
            set { mRecordFieldName = value; }
        }

        public string HeaderText
        {
            get { return mHeaderText; }
            set { mHeaderText = value; }
        }

        public bool ReadOnly
        {
            get { return mReadOnly; }
            set { mReadOnly = value; }
        }

        public bool Updated
        {
            get { return mUpdated; }
            set { mUpdated = value; }
        }
    }
}
