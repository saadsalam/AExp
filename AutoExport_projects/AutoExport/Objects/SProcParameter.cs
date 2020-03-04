using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExport.Objects
{
    public class SProcParameter
    {
        private string mParamname;
        private object mParamvalue;

        public string Paramname
        {
            get { return mParamname; }
            set { mParamname = value; }
        }

        public object Paramvalue
        {
            get { return mParamvalue; }
            set { mParamvalue = value; }
        }
    }
}
