using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoExport.Objects;
using System.Configuration;

namespace Export_Automation
{
    class Program
    {
        private const string CURRENTMODULE = "EXPORT_AUTOMATION";

        static void Main(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            Globalitems.runmode = appSettings["runmode"];

            DataOps.SetRunmodeAndConnection();
            if (Globalitems.blnException) return;

            Globalitems.SetUpGlobalVariables();
            Globalitems.HandleException(CURRENTMODULE, "Main", "Testing Export Automation console project",
                false);
        }
    }
}
