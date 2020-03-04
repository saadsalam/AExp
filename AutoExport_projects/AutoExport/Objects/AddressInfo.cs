using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExport.Objects
{
    public class AddressInfo
    {
        private string mcity = "";
        private string mcountry = "US";
        private string merror = "";
        private string mstate = "";

        public string city
        {
            get { return mcity; }
            set { mcity = value; }
        }

        public string country
        {
            get { return mcountry; }
            set { mcountry = value; }
        }

        public string error
        {
            get { return merror; }
            set { merror = value; }
        }


        public string state
        {
            get { return mstate; }
            set { mstate = value; }
        }
    }
}
