using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Reporting.WinForms;
using AutoExport.Objects;
using System.Drawing.Printing;
using System.Data;

namespace AutoExport
{
    public partial class frmDisplayreport : Form
    {
        //CONSTANTS
        private const string CURRENTMODULE = "frmDisplayreport";

        public frmDisplayreport(string strMessage, string strReportPath, 
            ReportDataSource rptDataSource,
            int rptviewHgt, int rptviewWdth,
            List<ReportParameter> lsParams = null)
        {
            PageSettings pgSettings = new PageSettings();
            PrinterSettings prSettings = new PrinterSettings();

            try
            {
                InitializeComponent();

                //Set form Hgt +150 pixels from rptViewHgt, form Wdth +75 pixels from rptviewWdth
                //this.Height = rptviewHgt + 150;
                //this.Width = rptviewWdth + 75;
                //this.Height = rptviewHgt;
                //this.Width = rptviewWdth;

                //Set rptviewer Hgt & Wdth
                //rptViewer.Height = rptviewHgt;
                //rptViewer.Width = rptviewWdth;

                //Set Message
                lblMessage.Text = "You can review " + strMessage + " in the Viewer below.\n (Click the " +
                    "forward and back arrows to see each record.)\n" +
                    "Click the Printer icon when you want to print the " + strMessage +
                    ". (Select the Label printer if not already selected.)";

                //Set Form title
                this.Text = "Export - Review & Print " + strMessage;

                //Set PageSetup to Portrait for ReportViewer
                //rptViewer.SetPageSettings(new System.Drawing.Printing.PageSettings() { Landscape = false });
               
                pgSettings.Landscape = false;

                //If not labels, sheet printing. Set Margins, in hundreds of an inch, to .5",
                //  and change Paper Size to Letter, if default printer is a Label 
                if (strMessage == "Labels")
                {
                    //pgSettings.Margins = new Margins(10, 10, 15, 0);
                    pgSettings.Margins = new Margins(0, 0, 0, 0);
                    prSettings.PrinterName = Globalitems.strLabelPrinter;
                }
                else
                {
                    prSettings.PrinterName = Globalitems.strSheetPrinter;
                    pgSettings.Margins = new Margins(50, 0, 50, 0);
                    //If a Label printer is the default printer (blnCannotPrintLabels is false)
                    //  create a custom Letter PaperSize, Width, Height in hundreds of an inch
                    if (!Globalitems.blnCannotPrintLabels)
                        pgSettings.PaperSize = new PaperSize("Letter_cust", 850, 1100);

                    if (strMessage == "Grounded Summary" || 
                        strMessage == "Vehicles On Hold" ||
                        strMessage == "Destination BarCodes" ||
                        strMessage == "Invoices")
                    {
                        pgSettings.Landscape = true;
                        //Set Right & Left margins to 1/4 ", top/bottom 1/2" for Invoices
                        if (strMessage == "Invoices") pgSettings.Margins = 
                                new Margins(25, 25, 50, 50);
                    }
                }

                rptViewer.SetPageSettings(pgSettings);

                //Set DisplayMode to PrintLayout to print All pages
                rptViewer.SetDisplayMode(DisplayMode.PrintLayout);

                //Set PrinterSettings
                prSettings.Copies = 1;

                //prSettings.PrinterName = strPrinter;
                rptViewer.PrinterSettings = prSettings;

                //Assign the report to rptViewer
                rptViewer.LocalReport.ReportPath = strReportPath;
                rptViewer.ZoomMode = ZoomMode.PageWidth;

                //Assign the datasource to rptViewer
                rptViewer.LocalReport.DataSources.Add(rptDataSource);

                //Assign lsParams, if any, to the report
                if (lsParams != null) rptViewer.LocalReport.SetParameters(lsParams);

                //Add SubreportProcessing event handler to load Subrpt data 
                //if Invoices report
                if (strMessage == "Invoices")
                {
                    rptViewer.LocalReport.SubreportProcessing +=
                        new SubreportProcessingEventHandler(SubRptProcessingEventHandler);
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmDisplayreport", ex.Message);
            }
        }

        private void frmDisplayreport_Load(object sender, EventArgs e)
        {
            try
            {
                this.rptViewer.RefreshReport();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "frmDisplayreport_Load", ex.Message);
            }
        }

        void SubRptProcessingEventHandler (object sender, 
            SubreportProcessingEventArgs e)
        {
            DataSet ds;
            string strBillingID = e.Parameters["BillingID"].Values[0].ToString();

            string strSQL = @"SELECT
            ISNULL(ex.Exportername,'') AS exporter,
            st.ValueDescription AS InvoiceInquiryMsg,
            SUM(veh.EntryRate) as ProcessingFee,
            SUM(veh.TotalCharge)-SUM(veh.EntryRate) as StorageFee,
            SUM(veh.TotalCharge) AS Rate,
            COUNT(veh.VIN) AS VINCount
            FROM 
            Billing bill
            INNER JOIN AutoportExportVehicles veh on veh.BillingID=bill.BillingID
            LEFT OUTER JOIN AEExporter ex ON ex.AEExporterID=veh.ExporterID
            INNER JOIN SettingTable st ON st.ValueKey = 'InvoiceInquiryMessage' 
            WHERE bill.BillingID= " + strBillingID + " " + 
            @"GROUP BY ex.ExporterName, st.ValueDescription
            ORDER BY Exportername";

            ds = DataOps.GetDataset_with_SQL(strSQL);

            e.DataSources.Add(new ReportDataSource("dsInvoice_export", ds.Tables[0]));

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDisplayreport_MouseMove(object sender, MouseEventArgs e)
        {Globalitems.ResetActivityTimer();}
    }
}
