using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExport.Objects
{
    public class PrintReport
    {
        //MS example for printing a LocalReport directly to a printer
        //https://msdn.microsoft.com/en-us/library/ms252091.aspx
        // Code below added PageSettings & PrinterSettings for the PrintDocument object in the Print method

        public const string CURRENTMODULE = "PrintReport";

        private int mCurrentPageIndex = 0;
        private IList<MemoryStream> mStreams;
        private LocalReport mReport;
        private string mReportType;
        private string mPrinter;

        public string Printer
        {
            get { return mPrinter; }
            set { mPrinter = value; }
        }

        public LocalReport Report
        {
            get { return mReport; }
            set { mReport = value; }
        }

        public string ReportType
        {
            get { return mReportType; }
            set { mReportType = value; }
        }

        public  PrintReport(LocalReport rpt, string strPrinter, string strReportType)
        {
            Report = rpt;
            Printer = strPrinter;
            ReportType = strReportType;
        }

        public void PrintAReport()
        {
            RenderReportToStream();
            Print();
        }

        private MemoryStream CreateStream(string name,
      string fileNameExtension, Encoding encoding,
      string mimeType, bool willSeek)
        {
             MemoryStream stream = new MemoryStream();
            mStreams.Add(stream);
            return stream;
        }

        private void RenderReportToStream()
        {
            string deviceInfo;

            try
            {
                if (ReportType == "LABEL")
                    deviceInfo = @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3.1in</PageWidth>
                <PageHeight>1.1in</PageHeight>
                </DeviceInfo>";
                else
                    deviceInfo = @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                </DeviceInfo>";

                Warning[] warnings;
                mStreams = new List<MemoryStream>();
                Report.Render("Image", deviceInfo, CreateStream,
                   out warnings);
                foreach (MemoryStream stream in mStreams)
                    stream.Position = 0;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "RenderReportToStream", ex.Message);
            }            
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            try
            {
                Metafile pageImage;
                Rectangle rect;

                pageImage = new Metafile(mStreams[mCurrentPageIndex]);


                // PageSettings don't seem to work completely for margins for Sheets, 
                //  so set Left, Top at 25 hundreds of an inch, .25 "
                if (ReportType == "LABEL")
                    rect = new Rectangle(ev.PageBounds.Left, ev.PageBounds.Top,
                    ev.PageBounds.Width, ev.PageBounds.Height);
                else
                    rect = new Rectangle(25, 25,
                    ev.PageBounds.Width, ev.PageBounds.Height);

                ev.Graphics.DrawImage(pageImage, rect);

                mCurrentPageIndex++;
                ev.HasMorePages = (mCurrentPageIndex < mStreams.Count);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "PrintPage", ex.Message);
            }
        }

        private void Print()
        {
            System.Drawing.Printing.PrinterSettings prSettings = new System.Drawing.Printing.PrinterSettings(); 
            try
            {
                if (mStreams == null || mStreams.Count == 0)
                    throw new Exception("Error: no stream to print.");
                PrintDocument printDoc = new PrintDocument();

                //Set PageSetup to Portrait for ReportViewer
                PageSettings pgSettings = new PageSettings();
                pgSettings.Landscape = false;

                if (ReportType == "LABEL")
                {
                    pgSettings.Margins = new Margins(10, 10, 14, 0);
                    prSettings.PrinterName = Globalitems.strLabelPrinter;
                }
                else
                {
                    prSettings.PrinterName = Globalitems.strSheetPrinter;
                    pgSettings.Margins = new Margins(50, 0, 50, 0);
                    pgSettings.PaperSize = new PaperSize("Letter_cust", 850, 1100);
                }

                //Set PrinterSettings
                prSettings.Copies = 1;
                prSettings.PrinterName = Printer;

                //Associate PrinterSettings w/PageSettings
                pgSettings.PrinterSettings = prSettings;

                //Associate PageSettings & PrinterSettings w/printDoc
                printDoc.DefaultPageSettings = pgSettings;
                
                printDoc.PrinterSettings = prSettings;
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                printDoc.Print();
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "Print", ex.Message);
            }

            finally
            {
                if (mStreams != null)
                {
                    foreach (Stream strm in mStreams) strm.Close();
                    mStreams = null;
                }
            }
            
        }
    }
}
