using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.ComponentModel;

using System.Web;
using System.Web.Script.Serialization;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using static System.Net.WebRequestMethods;
using System.Timers;

namespace AutoExport.Objects
{
    public static class Globalitems
    {
        private const string CURRENTMODULE = "Globalitems";
        //public const int INACTIVEMINUTESTOCLOSEPROGRAM = 15;

        //Public Global Variables
        static public bool blnCannotPrintLabels = false; //assume default printer is a label printer
        static public bool blnAdmin = false;
        static public bool blnException = false;
        static public bool blnHideRates = false;
        static public bool blnPasswordChanged = false;
        static public bool blnTimeout = false;

        static public char chrRecordSeparator = '$';
        static public char chrNameValueSeparator = ':';
        static public char chrMultiValueSeparator = ',';

        static public string DATE_TOOLTIP = "";
        //static public string DATE_TOOLTIP = "1) Only accepts digits, '/', or '-'.\n" +
        //   "2) Cannot use BOTH '/' and '-'.\n" +
        //   "3) Illegal entries will not leave textbox.\n" +
        //   "4) To enter day of current month, enter 1-2 digits.\n" +
        //   "5) To enter day and month of current year,\n" +
        //   "     enter 3-4 digits.\n" +
        //   "   If 1st two digits are less than 12, format is MM/DD.\n" +
        //   "   If 1st two digits are 13-31, format is DD/MM";
        static public string EMAIL_CLIENT = "192.168.1.148";
        //static public string EMAIL_CLIENT = "192.168.150.244";
        static public string EMAIL_FROM;
        static public string EMAIL_PASSWORD;
        static public string EXCEPTION_MESSAGE;

        static public DateTime NULLDATE = new DateTime(1800, 1, 1);

        // \n embedded in string from SettingTable don't display newlnes in Messagebox displayed
        static public List<string> lsExceptionMessageLines = new List<string>();

        static public string EXCEPTION_SUBJECT;
        static public List<string> EXCEPTION_TO_LIST = new List<string>();

        static public DataTable dtCode;
        static public Icon icoApp = new Icon(AutoExport.Properties.Resources.logo_png, icoheight, icowidth);
        static public Image imgTestImage = AutoExport.Properties.Resources.TEST;

        static public frmLogin LoginForm;
        static public frmMain MainForm;
        static public string runmode = "";
        static public string strLabelPrinter;
        static public string strSheetPrinter = "";
        static public string strLockoutMsg;
        static public string strOSVersion;
        static public string strPassword = "";
        static public string strSavedSQL = "";
        static public List<string> strRoleNames = new List<string>();
        static public string strSelection = "";
        static public string strTextSelected = "";
        static public string strUserName = "";
        static public string strUserFullName = "";
       
        static private int icoheight = AutoExport.Properties.Resources.logo_png.Height;
        static private int icowidth = AutoExport.Properties.Resources.logo_png.Width;

        static private int INACTIVEMINUTESTOCLOSEPROGRAM;
        static public int SECONDSFORCLOSINGPROGRAM;

        static public System.Timers.Timer timActivity = new System.Timers.Timer();
        static public System.Timers.Timer timResponse = new System.Timers.Timer();
        static private string strZipCodeURL = "";

        public static void StartActivityTimer()
        {
            //Ck in ms. every INACTIVEMINUTESTOCLOSEPROGRAM
            timActivity.Interval = INACTIVEMINUTESTOCLOSEPROGRAM * 60 * 1000;
            timActivity.Elapsed += CheckForInactivity;
            timActivity.Start();
        }

        public static void ResetActivityTimer()
        {
            timActivity.Stop();
            timActivity.Start();
        }

        public static void CheckForInactivity(object source, ElapsedEventArgs e)
        {
            DialogResult dlResult;
            frmTimeOutNotice frm;

            //Set response to SecondsForClosingProgram + 3 to allow for opening form
            timResponse.Interval = (SECONDSFORCLOSINGPROGRAM + 3) * 1000;
            timResponse.Elapsed += ProgramTimedOut;
            timResponse.Start();

            //Show frmTimeOutNotice in modal form, if another modal form is not open 
            frm = new frmTimeOutNotice();
            dlResult = frm.ShowDialog();
            timResponse.Stop();
            timActivity.Stop();
            if (dlResult == DialogResult.Cancel)
                ResetActivityTimer();
            else
                ProgramTimedOut(null,null);   
        }

        private static void ProgramTimedOut(object source, ElapsedEventArgs e)
        {
            bool blnSavedEvents;
            frmEventProcessing frm = SaveEventProcessingData();

            timActivity.Stop();
            timResponse.Stop();

            blnTimeout = true;

            if (frm != null)
            {
                blnSavedEvents = frm.SaveVehiclesFromGrid();
                RecordTimeOut();
                blnException = true;
                Application.Exit();
            }
            else
            {
                //blnSavedEvents = frm.SaveVehiclesFromGrid();
                RecordTimeOut();
                blnException = true;
                Application.Exit();
            }
        }

        private static frmEventProcessing SaveEventProcessingData()
        {
            try
            {
                //Only invoke SaveVehiclesFromGrid if frmEventProcessing is open
                if (Application.OpenForms.OfType<frmEventProcessing>().Count() == 1)
                {
                    foreach (Form eventprocessing in Application.OpenForms)
                    {
                        if (eventprocessing.GetType().Name == "frmEventProcessing")
                            return (frmEventProcessing)eventprocessing;
                    }

                    return null;
                }
                else
                    return null;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SaveEventProcessingData", ex.Message);
                return null;
            }
        }

        private static void RecordTimeOut()
        {
            string strMsg;
            string strSQL;

            strMsg = "Program Timed Out";

            strSQL = @"INSERT INTO ProgramException (ExceptionMessage,Usercode,CreationDate)
            VALUES ('" + strMsg + "','" + strUserName + "',CURRENT_TIMESTAMP)";
            DataOps.PerformDBOperation(strSQL);
        }

        public static void SetSheetPrinter()
        {
            try
            {
                ComboBox cbo = new ComboBox();
                ComboboxItem cboitem;
                DialogResult dlResult;
                frmSetSelection frm;

                //Enter all available non-label printers into cbo
                foreach (string strPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (!strPrinter.Contains("Wasp") && !strPrinter.Contains("ZT"))
                    {
                        cboitem = new ComboboxItem();
                        cboitem.cboText = strPrinter;
                        cboitem.cboValue = strPrinter;
                        cbo.Items.Add(cboitem);
                    }
                }

                //Use frmSetSelection to get the default sheet printer
                frm = new frmSetSelection("Sheet Printer for non-labels", cbo);
                dlResult = frm.ShowDialog();

                if (dlResult == DialogResult.OK && Globalitems.strSelection.Length > 0
                    && !Globalitems.blnException)
                {
                    Globalitems.strSheetPrinter = Globalitems.strSelection;

                    //Update app.config file with SheetPrinter so User only sets once
                    string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string configFile = appPath + "\\AutoExport.exe.config";
                    ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                    configFileMap.ExeConfigFilename = configFile;
                    System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

                    config.AppSettings.Settings["SheetPrinter"].Value = Globalitems.strSheetPrinter;
                    config.Save();
                }
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "SetSheetPrinter", ex.Message);
            }
        }

        private static void ProcessPanelControls(ref Panel pnl, List<string> lsExcludes = null)
        {
            try
            {
                Panel pnlfound;
                TabControl tabctrl;

                foreach (Control ctrl in pnl.Controls)
                {
                    if (lsExcludes == null || !lsExcludes.Contains(ctrl.Name))
                    {
                        if (ctrl.GetType() == typeof(TextBox) || ctrl.GetType() == typeof(ComboBox) ||
                       ctrl.GetType() == typeof(MaskedTextBox))
                        {
                            ctrl.AutoSize = false;
                            ctrl.Height = 25;
                        }

                        if (ctrl.GetType() == typeof(TabControl))
                        {
                            tabctrl = (TabControl)ctrl;
                            if (lsExcludes == null)
                                ProcessTabControl(ref tabctrl);
                            else
                                ProcessTabControl(ref tabctrl,lsExcludes);
                        }

                        if (ctrl.GetType() == typeof(Panel))
                        {
                            pnlfound = (Panel)ctrl;
                            if (lsExcludes == null)
                                ProcessPanelControls(ref pnlfound);
                            else
                                ProcessPanelControls(ref pnlfound, lsExcludes);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "ProcessPanelControls", ex.Message);
            }
        }

        private static void ProcessTabControl(ref TabControl tabctrl, List<string> lsExcludes = null)
        {
            try
            {
                Panel pnl;

                foreach (Control ctrl in tabctrl.Controls)
                {
                        if (ctrl.GetType() == typeof(TabPage))
                        {
                            foreach (Control tabpagectrl in ctrl.Controls)
                            {
                                if (lsExcludes == null || !lsExcludes.Contains(tabpagectrl.Name))
                                {
                                if (tabpagectrl.GetType() == typeof(TextBox) || 
                                    tabpagectrl.GetType() == typeof(ComboBox) ||
                                    tabpagectrl.GetType() == typeof(MaskedTextBox))
                                {
                                    tabpagectrl.AutoSize = false;
                                    tabpagectrl.Height = 25;
                                }

                                if (tabpagectrl.GetType() == typeof(Panel))
                                {
                                    pnl = (Panel)tabpagectrl;
                                    if (lsExcludes == null)
                                        ProcessPanelControls(ref pnl);
                                    else
                                        ProcessPanelControls(ref pnl,lsExcludes);
                                }
                            }
                                    
                          }
                        }
                }     
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "ProcessTabControl", ex.Message);
            }
        }

       

        public static AddressInfo DecodeZip_USPS(string strZip)
        {
        //Use USPS service; requires registering w/USPS, & receiving UserID,  
        //Note: DMaibor, registered and received UserID for use.
        //Use HttpWebRequest/HttpWebResponse

        //Sample from User Manual for CityStateLookup:
        //http://production.shippingapis.com/ShippingAPITest.dll?API=CityStateLookup &XML=<CityStateLookupRequest USERID="xxxxxxx"><ZipCode ID= "0">
        //<Zip5>90210</Zip5></ZipCode></CityStateLookupRequest >

        //Response: XML document
        //<? xml version = "1.0" ?> 
        //< CityStateLookupResponse >
        //< ZipCode ID = "0" >< Zip5 > 90210 </ Zip5 > 
        //< City > BEVERLY HILLS </ City >
        //< State > CA </ State ></ ZipCode >
        //</ CityStateLookupResponse >

            try
            {
                AddressInfo objAddress = new AddressInfo();
                DataSet ds;
                HttpWebRequest httprequest;
                HttpWebResponse httpresponse;
                string strRQURL;
                string strSQL;
                string strval;
                XmlDocument xmlDoc;
                XmlNodeList nodelist;

                if (strZip.Trim().Length < 5)
                {
                    objAddress.error = "Invalid Zipcode. Must be at least 5 digits.";
                    return objAddress;
                }

                if (strZip.Trim().Length > 5) strZip = strZip.Trim().Substring(0, 5);

                //Get the USPSServerURL from SettingTable, if 2nd time
                if (strZipCodeURL.Length == 0)
                {
                    strSQL = @"SELECT ValueDescription FROM SettingTable 
                    WHERE ValueKey = 'USPSServerURL';";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "DecodeZip_USPS",
                            "No data returned from SettingTable");
                        return null;
                    }

                    strZipCodeURL = ds.Tables[0].Rows[0]["ValueDescription"].ToString();
                }
                
                strRQURL = strZipCodeURL.Replace("nnnnn", strZip);                

                //Retrieve web service VIN info & load into an XmlDocument
                httprequest = WebRequest.Create(strRQURL) as HttpWebRequest;
                httpresponse = httprequest.GetResponse() as HttpWebResponse;
                xmlDoc = new XmlDocument();
                xmlDoc.Load(httpresponse.GetResponseStream());

                //Ck for error returned. Error - XML Element: <Error>, then <Description> 
                nodelist = xmlDoc.GetElementsByTagName("Error");
                if (nodelist.Count > 0)
                {
                    nodelist = xmlDoc.GetElementsByTagName("Description");
                    strval = nodelist[0].InnerText;
                    objAddress.error = strval;
                    return objAddress;
                }

                //Get City, State
                nodelist = xmlDoc.GetElementsByTagName("City");
                strval = nodelist[0].InnerText;
                objAddress.city = strval;

                nodelist = xmlDoc.GetElementsByTagName("State");
                strval = nodelist[0].InnerText;
                objAddress.state = strval;
                return objAddress;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "DecodeZip_USPS", ex.Message);
                return null;
            }
        }

        public static AddressInfo DecodeZipPCMiler(string strZip)
        {
            //To get the info from PCMiler
            //Open a TCP connection
            //Create a NetworkStream to send/receive data
            //PCMSNewTrip() - returns new TripID & READY
            //PCMSAddStop(TripID,"Zipcode") - 1 <CR> READY
            //PCMSGetStop(TripID,"0")
            //PCMSDeleteTrip(,TripID) - close the trip
            //Close the NetworkStream
            //Close the TCP connection

            DataSet ds;
            int intPort = 0;
            string strIPAddress = "";
            string strSQL;

            Byte[] data;
            Int32 bytes;
            int intTripID = 0;

            AddressInfo objAddress = new AddressInfo();

            NetworkStream stream = null;
            string strMsg;
            string strResponse = "";
            string[] strReplysections;
            TcpClient client = null;

            try
            {
                objAddress = new AddressInfo();

                //Get PCMiler IP Addr & Port from SettingTable
                strSQL = @"SELECT ValueKey,ValueDescription
                    FROM SettingTable 
                    WHERE ValueKey IN ('PCMilerServerIPAddress','PCMilerServerPort')";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (blnException) return null;

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    HandleException(CURRENTMODULE, "DecodeZipPCMiler",
                        "No data returned from the SettingTable");
                    return null;
                }

                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    if (drow["ValueKey"].ToString() == "PCMilerServerIPAddress")
                        strIPAddress = drow["ValueDescription"].ToString();
                    else
                        intPort = Convert.ToInt16(drow["ValueDescription"]);
                }

                //Steps to use TcpClient [client]
                //1. Connect with IP Address & port#
                //2. Associate a NetworkStream with the TcpClient [stream]
                //3. Create a string message to send with the TcpClient [strMsg]
                //4. Convert the message to an array of bytes [data]
                //5. Use stream.Write to send the message
                //6. Use stream.Read to retrieve the Tcp response
                //7. Convert response to a string [strResponse]   
                //8. Close stream & client when done

                client = new TcpClient();

                client.Connect(strIPAddress, intPort);
                stream = client.GetStream();

                //DATS format: add (char)13 [CR] + (char(10) [LineFeed]
                strMsg = "PCMSNewTrip()" + (char)13 + (char)10;
                data = System.Text.Encoding.ASCII.GetBytes(strMsg);

                //Send TCP RQ
                stream.Write(data, 0, data.Length);

                //Get TCP Response
                data = new Byte[5000];

                //Delay 1/4 sec to allow PCMiler to generate a trip id; otherwise may
                //  fail to get the full response and not receive a trip id
                Thread.Sleep(250);

                bytes = stream.Read(data, 0, data.Length);
                strResponse = System.Text.Encoding.ASCII.GetString(data, 0,
                    bytes);

                //Reply adds CR's [\n in C#] to response. 
                strReplysections = strResponse.Split('\n');

                //Make sure there are at least 3 lines, & 3rd line is "READY"
                if (strReplysections.Length > 2 && strReplysections[2] == "READY")
                {
                    //2nd line s/b new trip id
                    intTripID = Convert.ToInt16(strReplysections[1]);

                    //Format from DATS: PCMSAddStop(233,"02126") [trip id, "zip code"]
                    strMsg = "PCMSAddStop(" + intTripID.ToString() + ",\"" +
                        strZip + "\")" + (char)13 + (char)10;
                    data = System.Text.Encoding.ASCII.GetBytes(strMsg);

                    //Send 2nd TCP RQ
                    //await stream.WriteAsync(data, 0, data.Length);
                    stream.Write(data, 0, data.Length);

                    //Get TCP Response
                    strResponse = "";
                    data = new Byte[5000];

                    //bytes = await stream.ReadAsync(data, 0, data.Length);
                    bytes = stream.Read(data, 0, data.Length);
                    strResponse = System.Text.Encoding.ASCII.GetString(data, 0,
                        bytes);

                    strReplysections = strResponse.Split('\n');
                    if (strReplysections.Length > 0 && strReplysections[0] == "1" &&
                        strReplysections[1] == "READY")
                    {
                        //Format from DATS: PCMSGetStop(240,"0") [trip id, "0"]
                        strMsg = "PCMSGetStop(" + intTripID.ToString() + ",\"0\")" +
                            (char)13 + (char)10;
                        data = System.Text.Encoding.ASCII.GetBytes(strMsg);

                        //Send 3nd TCP RQ
                        stream.WriteAsync(data, 0, data.Length);

                        //Get TCP Response
                        strResponse = "";
                        data = new Byte[100000];

                        //bytes = await stream.ReadAsync(data, 0, data.Length);
                        bytes = stream.Read(data, 0, data.Length);
                        strResponse = System.Text.Encoding.ASCII.GetString(data, 0,
                            bytes);

                        //Response in form
                        //27
                        //02126 Mattapan, MA, Suffolk  [zipcode City, State, County]
                        //READY
                        strReplysections = strResponse.Split('\n');

                        if (strReplysections.Length > 2 && strReplysections[2] == "READY")
                        {
                            strResponse = strReplysections[1];

                            //Remove Zip code 
                            int intIndex = strResponse.IndexOf(" ");
                            strResponse = strResponse.Substring(intIndex + 1,
                                strResponse.Length - intIndex - 1);
                            strReplysections = strResponse.Split(',');
                            objAddress.city = strReplysections[0];
                            objAddress.state = strReplysections[1];
                        }
                        else
                        {
                            if (strReplysections[0] == "-1")
                                objAddress.error = "INVALID ZIP";
                            else
                                HandleException(CURRENTMODULE, "DecodeZipPCMiler",
                                "No data returned from PCMiler");
                        }
                    }
                    else
                    {
                        if (strReplysections[0] == "-1")
                            objAddress.error = "INVALID ZIP";
                    }
                }

                return objAddress;
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "DecodeZipPCMiler", ex.Message);
                return null;
            }

            finally
            {
                if (intTripID != 0)
                {
                    strMsg = "PCMSDeleteTrip()";
                    data = System.Text.Encoding.ASCII.GetBytes(strMsg);
                    //await stream.WriteAsync(data, 0, data.Length);
                    stream.Write(data, 0, data.Length);
                }
                if (stream != null) stream.Close();
                if (client != null) client.Close();
            }
        }

        public static AddressInfo DecodeZipJSON(string strZip)
        {
            //Alternate approach for ZipCodes, if server for DecodeZip_usps becomes unavailable 
            //Uses free webservice ziptasticapi.com to retrieve Country, City, State or error
            //  as a JSON file. Seems significantly slower, at different times of the day
            // Use HttpWebRequest/HttpWebResponse
            try
            {
                AddressInfo objAddress = new AddressInfo();
                DataSet ds;
                HttpWebRequest httprequest;
                HttpWebResponse httpresponse;
                string strRQURL;
                string strSQL;

                //Get the ZipCodeServerURL from SettingTable, if not 1st time
                if (strZipCodeURL.Length == 0)
                {
                    strSQL = @"SELECT ValueDescription FROM SettingTable 
                        WHERE ValueKey = 'ZipCodeServerURL';";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "DecodeZip",
                            "No data returned from SettingTable");
                        return null;
                    }

                    strZipCodeURL = ds.Tables[0].Rows[0]["ValueDescription"].ToString();
                }
                

                strRQURL = strZipCodeURL + "/" + strZip;

                //Retrieve web service VIN info & load into an XmlDocument
                httprequest = WebRequest.Create(strRQURL) as HttpWebRequest;
                httpresponse = httprequest.GetResponse() as HttpWebResponse;

                //Read response into strJson
                Stream strm = httpresponse.GetResponseStream();
                StreamReader strmrdr = new StreamReader(strm);
                string strJson = strmrdr.ReadToEnd();

                //Deserialize stgrJson into a Json object, and load into objAddress
                JavaScriptSerializer jsonser = new JavaScriptSerializer();
                objAddress = jsonser.Deserialize<AddressInfo>(strJson);

                return objAddress;
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "DecodeZip", ex.Message);
                return null;
            }
        }

        public static string NewDecodVIN(string strVIN)
        {
            try
            {
                //Use new DataOne service, Ver. 7.0.1, to decode VIN
                //Receive VIN and return XML as a string

                DataSet ds;
                HttpWebRequest httprequest;
                HttpWebResponse httpresponse;
                Stream dataStream;
                string strDBError;
                string strPostdata;
                string strRQAccessCode = "";
                string strRQURL = "";
                string strRQUserID = "";
                string strSQL;
                string strXMLRQ;
                string strval;
                XmlDocument xmlDoc;

                strDBError =
                "<query_error>" +
                    "<error_code>DB</error_code>" +
                    "<error_message>DB error retreving DB info</error_message >" +
                "</query_error >";

                //Get the DataOne V7 settings
                strSQL = @"SELECT ValueKey,ValueDescription 
                    FROM SettingTable
                    WHERE ValueKey LIKE 'DataOne%V7'";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return strDBError;

                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    strval = drow["ValueKey"].ToString();
                    switch (strval)
                    {
                        case "DataOneAccessCodeV7":
                            strRQAccessCode = drow["ValueDescription"].ToString();
                            break;
                        case "DataOneClientNameV7":
                            strRQUserID = drow["ValueDescription"].ToString();
                            break;
                        case "DataOneServerURLV7":
                            strRQURL = drow["ValueDescription"].ToString();
                            break;
                    }
                }

                //client_id = 1102 & authorization_code = nxjedzhzraxtzrncxsxxzcxqqfesmnesbfwefevw & 
                //decoder_query =< decoder_query >< decoder_settings > < display > full </ display > < version > 7.0.1 </ version > < styles > on </ styles > < style_data_packs > < basic_data > on </ basic_data > < engines > on </ engines > < transmissions > on </ transmissions > < colors > on </ colors > </ style_data_packs > < common_data > on </ common_data > < common_data_packs > < basic_data > on </ basic_data > < engines > on </ engines > < transmissions > on </ transmissions > < colors > on </ colors > </ common_data_packs > </ decoder_settings > < query_requests > < query_request > < vin > 5J6RE3H50AL037263 </ vin > </ query_request > </ query_requests > </ decoder_query >

                 strXMLRQ = "<decoder_query>" +
                    "<decoder_settings>" +
                        "<display>full</display>" +
                        "<version>7.0.1</version>" +
                        "<styles>on</styles>" +
                        "<style_data_packs>" +
                            "<basic_data>on</basic_data>" +
                            "<engines>on</engines>" +
                            "<transmissions>on</transmissions>" +
                            "<colors>on</colors>" +
                        "</style_data_packs>" +
                        "<common_data>on</common_data>" +
                        "<common_data_packs>" +
                            "<basic_data>on</basic_data>" +
                            "<engines>on</engines>" +
                            "<transmissions>on</transmissions>" +
                            "<colors>on</colors>" +
                        "</common_data_packs>" +
                    "</decoder_settings>" +
                    "<query_requests>" +
                        "<query_request>" +
                            "<vin>xxx</vin>" +
                        "</query_request>" +
                    "</query_requests>" +
                "</decoder_query>";

                //Replace xxx in XMRQ with strVIN
                strXMLRQ = strXMLRQ.Replace("xxx", strVIN);

                strPostdata = "client_id=" + strRQUserID +
                "&authorization_code=" + strRQAccessCode +
                "&decoder_query=" + strXMLRQ;

                var dataRQ = Encoding.ASCII.GetBytes(strPostdata);

                httprequest = WebRequest.Create(strRQURL) as HttpWebRequest;
                httprequest.Method = "POST";
                httprequest.ContentType = "application/x-www-form-urlencoded";
                httprequest.ContentLength = dataRQ.Length;

                dataStream = httprequest.GetRequestStream();
                dataStream.Write(dataRQ, 0, dataRQ.Length);
                dataStream.Close();

                httpresponse = httprequest.GetResponse() as HttpWebResponse;

                //Load response as an XmlDocument
                xmlDoc = new XmlDocument();
                xmlDoc.Load(httpresponse.GetResponseStream());

                //Write xmlDoc content 
                StringWriter strwriter = new StringWriter();
                XmlWriter xmltextwriter = XmlWriter.Create(strwriter);
                xmlDoc.WriteTo(xmltextwriter);
                xmltextwriter.Flush();
                string strXML = strwriter.ToString();

                return strXML;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "NewDecodeVIN", ex.Message);
                return null;
            }   
        }

        public static VINInfo DecodeVIN(string strVIN)
        {
            //Use WebClient to download string 

            const double KGCONVERSIONRATE = 2.2046226218;
            const Int16 INCUBICFOOTCONVERSIONRATE = 1728;

            bool blnContinue = true;
            DataSet ds = null;
            DataRow drow;
            decimal decHeight;
            decimal decLength;
            int intpos;
            Decimal decval;
            decimal decWeight;
            decimal decWidth;
            double dblval;
            int intDataOneVersion = 4;
            int intval;
            string strAccessCode = "";
            string strClient = "";
            string strRQURL = "";
            string strSQL;
            string strTagname;
            string strURL = "";
            string strval;
            //Note: string.Substring starts with 0 index as 1st char
            string strVINSquish = "";
            VINInfo objVINInfo = new VINInfo();
            XmlDocument xmlDoc;
            XmlNodeList nodelist;
            WebClient client;

            string strResponse;

            try
            {
                //1st Try to get VIN values from AutoportExportVehicles tbl, use latest record
                //  Get latest rec with Vindecodeind=1 and Veh. Hgt. info, if avail 
                //otherwise
                //  get latest rec.
                //  Use latest rec because BodyStyle description is 
                //  different on earlier records of same VINs
                //NOTE: with SQL 
                if (strVIN.Length > 16)
                {
                    strSQL = @"SELECT TOP(1)  ISNULL(VehicleYear,'') as VehicleYear, 
                    ISNULL(Make,'') AS Make,  
                    ISNULL(Model,'') AS Model, 
                    ISNULL(Bodystyle,'') AS Bodystyle, 
                    ISNULL(VehicleLength,'') AS VehicleLength, 
                    ISNULL(VehicleWidth,'') AS VehicleWidth, 
                    ISNULL(VehicleHeight,'') AS VehicleHeight, 
                    ISNULL(VehicleWeight,'') AS VehicleWeight, 
                    ISNULL(VehicleCubicFeet,'') AS VehicleCubicFeet, 
                    ISNULL(Bodystyle,'') AS Bodystyle,  
                    SizeClass  
                    FROM AutoportExportVehicles  
                    WHERE  VIN <> '" + strVIN + @"' AND  
                    VIN LIKE ('" + strVIN.Substring(0, 8) + "_" +
                       strVIN.Substring(9, 2) + @"%') AND
                    VINDecodedInd=1 AND LEN(RTRIM(ISNULL(VehicleHeight,''))) > 0 
                    AND VehicleHeight<>'0' 
                    ORDER BY
                    CreationDate DESC";
                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "DecodeVIN",
                            "No tables returned from query to AutoportExportVehicles table");
                        return null;
                    }

                    //Don't need to continue, 1st row contains data
                    //NOTE: AutoportExportVehicles shows weight in Kg, no need for conversion
                    if (ds.Tables[0].Rows.Count > 0) blnContinue = false;
                }
               
                // If blnContinue, Check if in VINDecode table. Get latest record, because there are 
                //  multiple recs in VINDecode table for the same VINSquish. VINDecode table
                //  doesn't have fields for VehicleCubicFeet or Sizeclass. Make sure VehicleHeight
                //  is available for SizeClass
                if (blnContinue && strVIN.Length > 16)
                {
                    strVINSquish = strVIN.Substring(0, 8) + strVIN.Substring(9, 2);
                    strSQL = @"SELECT TOP(1) ISNULL(VehicleYear,'') AS VehicleYear,
                   ISNULL(Make,'') AS Make,
                   ISNULL(Model,'') AS Model,
                   ISNULL(Bodystyle,'') AS Bodystyle,
                   ISNULL(VehicleLength,'') AS VehicleLength,
                   ISNULL(VehicleWidth,'') AS VehicleWidth,
                   ISNULL(VehicleHeight,'') AS VehicleHeight, 
                   ISNULL(VehicleWeight,'') AS VehicleWeight, 
                   '' AS VehicleCubicFeet, 
                   '' AS SizeClass  
                   FROM VINDecode 
                   WHERE VINSquish = '" + strVINSquish + @"' AND 
                    ISNULL(VehicleHeight,'0') <> '0' AND VehicleHeight <> ''
                   ORDER BY CreationDate DESC";

                    ds = DataOps.GetDataset_with_SQL(strSQL);
                    if (ds.Tables.Count == 0)
                    {
                        Globalitems.HandleException(CURRENTMODULE, "DecodeVIN",
                            "No tables returned from query to VINDecode table");
                        return null;
                    }

                    //Don't need to continue, 1st row contains data
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        blnContinue = false;

                        //VINDecode table shows weight in Lbs. Need to convert to Kg
                        strval = ds.Tables[0].Rows[0]["VehicleWeight"].ToString().Trim();
                        if (strval.Length > 0)
                        {
                            dblval = Convert.ToDouble(strval);
                            decWeight = Convert.ToDecimal(dblval / KGCONVERSIONRATE);
                            strval = Convert.ToInt16(decWeight).ToString();
                            ds.Tables[0].Rows[0]["VehicleWeight"] = strval;
                        }
                    }
                }

                //if !blnContinue, Load objVINInfo from 1st row in ds.Tables[0]
                if (!blnContinue)
                {
                    drow = ds.Tables[0].Rows[0];
                    objVINInfo.VehicleYear = drow["VehicleYear"].ToString();
                    objVINInfo.Make = drow["Make"].ToString();
                    objVINInfo.Model = drow["Model"].ToString();
                    objVINInfo.Bodystyle = drow["Bodystyle"].ToString();
                    objVINInfo.VehicleLength = drow["VehicleLength"].ToString();
                    objVINInfo.VehicleWidth = drow["VehicleWidth"].ToString();
                    objVINInfo.VehicleHeight = drow["VehicleHeight"].ToString();
                    objVINInfo.VehicleWidth = drow["VehicleWidth"].ToString();
                    objVINInfo.VehicleWeight = drow["VehicleWeight"].ToString();
                    objVINInfo.VehicleCubicFeet = drow["VehicleCubicFeet"].ToString();
                    if (drow["SizeClass"] != DBNull.Value)
                        objVINInfo.SizeClass = drow["SizeClass"].ToString();
                    blnContinue = true;
                }

                //If blnContinue, get VIN Info from Web Service
                //Notes:
                //  DataOne Web Service Ver. 4: used to date, GET request sent to 
                //      URL: http://xmlvindecoder.com/harnessXML/harnessRest5.php?
                //      returns XML with particular
                //      element tag names for Year/Make/Model/etc.
                //  DateOne Web Service Ver. 7: latest version of web service, 
                //      URL: internal web service created,
                        //http://milagedai.diversifiedauto.com:9334/api/VINDecode/[UserID]/[User Password]/[VIN] 
                        //returns different element tag names for Year/Make/Model/etc.
                else
                {
                    if (intDataOneVersion == 4)
                    {
                        //Current DataOne Ver. 4 service
                        //Get Web service & User info from SettingTable
                        strSQL = @"SELECT ValueKey,ValueDescription,SettingName,Description from 
                        SettingTable WHERE ValueKey 
                        IN ('DataOneServerURL','DataOneClientName','DataOneAccessCode')";
                        ds = DataOps.GetDataset_with_SQL(strSQL);
                        if (blnException) return null;

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 3)
                        {
                            HandleException(CURRENTMODULE, "DecodeVIN", "No data returned " +
                                "from SettingTable");
                            return null;
                        }

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            switch (dr["ValueKey"].ToString())
                            {
                                case "DataOneServerURL":
                                    strURL = dr["ValueDescription"].ToString();
                                    break;

                                case "DataOneClientName":
                                    strClient = dr["ValueDescription"].ToString();
                                    break;

                                case "DataOneAccessCode":
                                    strAccessCode = dr["ValueDescription"].ToString();
                                    break;
                            }
                        }

                        //Sample valid URL
                        //http://xmlvindecoder.com/harnessXML/harnessRest5.php?
                        //clientName=DiversifiedAutomotiveInc&
                        //vinNumbers=KMHDN45D13U707753&bodyStyles=&accessCode=c9b9de21586011bf2ff8d5b5b7db4fc9df1c0c22

                        //  Set up URL with parameters
                        strRQURL = strURL + "?clientName=" + strClient +
                            "&vinNumbers=" + strVIN + "&bodyStyles=" +
                            "&accessCode=" + strAccessCode;
                    }
                    else
                    {
                        //DataOne Ver. 7. Use new service set up on DAITRACKER3
                        strSQL = @"select * from SettingTable 
                        WHERE ValueKey IN ('DAIDataOneSvsURL','DAIDataOneSvsV7_DAI')";
                        ds = DataOps.GetDataset_with_SQL(strSQL);
                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 2)
                        {
                            HandleException(CURRENTMODULE, "DecodeVIN", "No data returned " +
                                "from SettingTable");
                            return null;
                        }

                        strClient = "DAIDataOneSvsV7_DAI";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            switch (dr["ValueKey"].ToString())
                            {
                                case "DAIDataOneSvsV7_DAI":
                                    strAccessCode = dr["ValueDescription"].ToString();
                                    break;

                                case "DAIDataOneSvsURL":
                                    strRQURL = dr["ValueDescription"].ToString();
                                    break;
                            }
                        }

                        //DataOneSvsV7DAI_URL value in SettingTable:
                        //http://milagedai.diversifiedauto.com:9334/api/VINDecode/[UserID]/[User Password]/[VIN]
                        //Update strRQURL with DAI UserID, User Password, VINs
                        strRQURL = strRQURL.Replace("[UserID]", strClient);
                        strRQURL = strRQURL.Replace("[User Password]", strAccessCode);
                        strRQURL = strRQURL.Replace("[VIN]", strVIN);
                    }

                    client = new WebClient();
                    strResponse = client.DownloadString(strRQURL);  

                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(strResponse);                    

                    objVINInfo.VIN = strVIN;

                    //Ck for error returned. Error - XML Element: <error_message> 
                    nodelist = xmlDoc.GetElementsByTagName("error_message");
                    if (nodelist.Count > 0)
                    {
                        strval = nodelist[0].InnerText;

                        if (strval.Length > 0)
                        {
                            objVINInfo.Error = true;
                            objVINInfo.ErrorDesc = strval;
                            return objVINInfo;
                        }
                    }

                    //No error. Retrieve VIN values

                    //Note: Ver. 7 uses different tag names than Ver. 4
                    //Bodystyle - XML Element: available_style/style, Attribute: name
                    //  text up to '('
                    strTagname = "available_style";
                    if (intDataOneVersion > 4) strTagname = "style";
                    nodelist = xmlDoc.GetElementsByTagName(strTagname);
                    if (nodelist.Count > 0)
                    {
                        strval = nodelist[0].Attributes["name"].InnerText;
                        if (strval.Length == 0 && nodelist.Count > 1)
                            strval = nodelist[1].Attributes["name"].InnerText;

                        intpos = strval.IndexOf("(");

                        if (intpos > -1) strval = strval.Substring(0, intpos);

                        //Limit to 50 chars because Bodystyle in veh table is varchar(50)
                        if (strval.Length > 50) strval = strval.Substring(0, 50);

                        objVINInfo.Bodystyle = strval;

                        //Add Rear axle info if available, 
                        //e.g. <specification name='Rear axle'>DRW</specification>
                        strTagname = "//specification[@name='Rear axle']";
                        nodelist = xmlDoc.SelectNodes(strTagname);
                        if (nodelist.Count > 0)
                        {
                            strval = nodelist[0].InnerText;
                            if (strval.Length > 0) objVINInfo.Bodystyle += ", " + strval;
                        }
                    }

                    //Make - XML Element: common_make/make
                    strTagname = "common_make";
                    if (intDataOneVersion > 4) strTagname = "make";
                    nodelist = xmlDoc.GetElementsByTagName(strTagname);
                    if (nodelist.Count > 0)
                    {
                        strval = nodelist[0].InnerText;
                        if (strval.Length == 0 && nodelist.Count > 1)
                            strval = nodelist[1].InnerText;
                        objVINInfo.Make = strval;
                    }

                    //Model - XML Element: common_model/model
                    strTagname = "common_model";
                    if (intDataOneVersion > 4) strTagname = "model";
                    nodelist = xmlDoc.GetElementsByTagName(strTagname);
                    
                    if (nodelist.Count > 0)
                    {
                        strval = nodelist[0].InnerText;
                        if (strval.Length == 0 && nodelist.Count > 1)
                            strval = nodelist[1].InnerText;
                        objVINInfo.Model = strval;
                    }

                    //VehicleCubicFeet [NOT IN VER 7] - XML Element: cube, Attribute: value
                    nodelist = xmlDoc.GetElementsByTagName("cube");
                    if (nodelist.Count > 0)
                    {
                        //strval = nodelist[0].InnerText;
                        strval = nodelist[0].Attributes["value"].InnerText;
                        if (strval.Length == 0 && nodelist.Count > 1)
                            strval = nodelist[1].InnerText;
                        objVINInfo.VehicleCubicFeet = strval;
                    }

                    //VehicleHeight - XML Element: height(attr 'value'/
                    //  specification attr name="Height", innerText
                    strval = "";
                    if (intDataOneVersion == 4)
                    {
                        strTagname = "height";
                        nodelist = xmlDoc.GetElementsByTagName(strTagname);
                        if (nodelist.Count > 0)
                            strval = nodelist[0].Attributes["value"].InnerText;
                    }
                    else
                    {
                        strTagname = "//specification[@name='Height']";
                        nodelist = xmlDoc.SelectNodes(strTagname);
                        if (nodelist.Count > 0)
                            strval = nodelist[0].InnerText;
                    }


                    objVINInfo.VehicleHeight = strval;

                    //VehicleLength  - XML Element: length, Attribute: value /
                    //specification attr name="Length", innerText
                    strval = "";
                    if (intDataOneVersion == 4)
                    {
                        strTagname = "length";
                        nodelist = xmlDoc.GetElementsByTagName(strTagname);
                        if (nodelist.Count > 0)
                            strval = nodelist[0].Attributes["value"].InnerText;
                    }
                    else
                    {
                        strTagname = "//specification[@name='Length']";
                        nodelist = xmlDoc.SelectNodes(strTagname);
                        if (nodelist.Count > 0)
                            strval = nodelist[0].InnerText;
                    }

                    objVINInfo.VehicleLength = strval;


                    //VehicleWight   - XML Element: curb_weight, weight provided in lbs.
                    //  specification attr name="Curb Weight", innerText
                    //  Convert to Kg., and display as integer
                    strval = "";
                    if (intDataOneVersion == 4)
                    {
                        strTagname = "curb_weight";
                        nodelist = xmlDoc.GetElementsByTagName(strTagname);
                        if (nodelist.Count > 0)
                        {
                            strval = nodelist[0].Attributes["value"].InnerText;
                            if (strval.Length == 0 && nodelist.Count > 1)
                                strval = nodelist[1].InnerText;
                        }                        
                    }
                    else
                    {
                        strTagname = "//specification[@name='Curb Weight']";
                        nodelist = xmlDoc.SelectNodes(strTagname);
                        if (nodelist.Count > 0)
                            strval = nodelist[0].InnerText;
                    }

                    if (strval.Length > 0)
                    {
                        dblval = Convert.ToDouble(strval);
                        decWeight = Convert.ToDecimal(dblval / KGCONVERSIONRATE);
                        objVINInfo.VehicleWeight = Convert.ToInt16(decWeight).ToString();
                    }

                    //VehicleWidth - XML Element: width /
                    //  specification attr name="Width", innerText
                    strval = "";
                    if (intDataOneVersion == 4)
                    {
                        strTagname = "width";
                        nodelist = xmlDoc.GetElementsByTagName(strTagname);
                        if (nodelist.Count > 0)
                        {
                            strval = nodelist[0].Attributes["value"].InnerText;
                            if (strval.Length == 0 && nodelist.Count > 1)
                                strval = nodelist[1].InnerText;
                        }
                    }
                    else
                    {
                        strTagname = "//specification[@name='Width']";
                        nodelist = xmlDoc.SelectNodes(strTagname);
                        if (nodelist.Count > 0)
                            strval = nodelist[0].InnerText;
                    }

                    objVINInfo.VehicleWidth = strval;

                    //VehicleYear - XML Element: common_year/year
                    strTagname = "common_year";
                    if (intDataOneVersion > 4) strTagname = "year";
                    nodelist = xmlDoc.GetElementsByTagName(strTagname);
                    if (nodelist.Count > 0)
                    {
                        strval = nodelist[0].InnerText;
                        if (strval.Length == 0 && nodelist.Count > 1)
                            strval = nodelist[1].InnerText;
                        objVINInfo.VehicleYear = strval;

                    }

                    objVINInfo.VINDecoded = true;
                }

                //objVINInfo should have its info. Ck for VehCubicFt

                //Calculate VehicleCubicFeet if no value (if info from VINDecode table)
                if (objVINInfo.VehicleCubicFeet.Length == 0 &&
                    objVINInfo.VehicleLength.Length > 0 &&
                    objVINInfo.VehicleWidth.Length > 0 &&
                    objVINInfo.VehicleHeight.Length > 0)
                {
                    decLength = Convert.ToDecimal(objVINInfo.VehicleLength);
                    decWidth = Convert.ToDecimal(objVINInfo.VehicleWidth);
                    decHeight = Convert.ToDecimal(objVINInfo.VehicleHeight);
                    decval =
                        (decLength * decWidth * decHeight) / INCUBICFOOTCONVERSIONRATE;
                    intval = Convert.ToInt16(decval);
                    objVINInfo.VehicleCubicFeet = intval.ToString();
                }

                if (objVINInfo.VINDecoded) UpdateVINDecodeTable(objVINInfo);

                return objVINInfo;
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "DecodeVIN", ex.Message);
                return null;
            }
        }

        public static string DecodeVINErrorMsg (VINInfo objVIN)
        {
            try
            {
                return "The VIN: " + objVIN.VIN +
                    " could not be decoded.\n\n" +
                    "Please manually enter " +
                    "Veh. info (Year, Make, Model, etc.) & Size Class using the " +
                    "Veh. Search && Veh. Detail forms.";
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "DecodeVINErrorMsg",ex.Message);
                return "";
            }
        }

        private static void UpdateVINDecodeTable(VINInfo objVIN)
        {
            try
            {
                string strSQL;

                strSQL = "INSERT INTO VINDecode (VINSquish,VehicleYear,Make,Model," + 
                    "Bodystyle,VehicleLength,VehicleWidth,VehicleHeight," +
                    "VehicleWeight,CreationDate,CreatedBy) " + 
                    "VALUES ('" + objVIN.VIN.Substring(0,8) + 
                    objVIN.VIN.Substring(9,2) + "'," + 
                    "'" + objVIN.VehicleYear + "'," +
                    "'" + objVIN.Make + "'," + 
                    "'" + objVIN.Model + "'," + 
                    "'" + HandleSingleQuoteForSQL(objVIN.Bodystyle) + "'," +
                    "'" + objVIN.VehicleLength + "'," +
                    "'" + objVIN.VehicleWidth + "'," +
                    "'" + objVIN.VehicleHeight + "'," +
                    "'" + objVIN.VehicleWeight + "'," +
                    "GetDate()," +
                    "'DataOneDecode')";

                DataOps.PerformDBOperation(strSQL);
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "UpdateVINdecodeTable", ex.Message);
            }
        }

        public static string SetReportPath(string strReport)
        {
            string strFolder;
            try
            {
                // All report files, rdlc's, must be in the Reports folder. The Reports folder must be in the
                //  same folder as the .exe file if there is no bin folder, If there is a bin folder, 
                //  the Reports folder must be in the same folder, holding the bin folder.
                //Put the path holding the .exe file into strFolder

                strFolder = System.Environment.CurrentDirectory;

                //Remove \\bin, if in strFolder
                if (strFolder.Contains("\\bin"))
                    strFolder = strFolder.Substring(0, strFolder.LastIndexOf("\\bin"));

                return strFolder + "\\Reports\\" + strReport;
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "SetReportPath", ex.Message);
                return "";
            }
        }

        public static void SetControlHeight(Form frm, List<string> lsExcludes = null)
        {
            //Search form for controls: TextBox, MaskedTextBox, ComboBox, Panel, TabControl
            //If Textbox, MaskedTextbox, ComboBox, set hgt
            //If panel, call ProcessPanelControls to process controls within panel
            //If TabControl, call ProcessTabControl to process controls on each tabPage
            try
            {
                Panel pnl;
                TabControl tabctrl;

                foreach (Control ctrl in frm.Controls)
                {
                    if (lsExcludes == null || !lsExcludes.Contains(ctrl.Name))
                    {
                        //Get all controls at form level
                        if ((ctrl.GetType() == typeof(TextBox) || ctrl.GetType() == typeof(MaskedTextBox)))
                        {
                            ctrl.AutoSize = false;
                            ctrl.Height = 25;
                        }

                        //Get all controls as child of panels
                        if (ctrl.GetType() == typeof(Panel))
                        {
                            pnl = (Panel)ctrl;
                            if (lsExcludes == null)
                                ProcessPanelControls(ref pnl);
                            else
                                ProcessPanelControls(ref pnl,lsExcludes);
                        }


                        //Get all controls as child of tab control
                        if (ctrl.GetType() == typeof(TabControl))
                        {
                            tabctrl = (TabControl)ctrl;

                            if (lsExcludes == null)
                                ProcessTabControl(ref tabctrl);
                            else
                                ProcessTabControl(ref tabctrl,lsExcludes);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "SetControlHeight", ex.Message);
            }
        }

       

       

        

        static public void SetNavButtons(RecordButtons recbuttons,BindingSource bs1)
        {
            if (bs1.Count == 0) return;

            if (bs1.Count == 1)
            {
                recbuttons.EnablebtnMovePrev(false);
                recbuttons.EnablebtnMoveNext(false);
            }
            else
            // bs1 count > 1
            {
                recbuttons.EnablebtnMoveNext(true);
                recbuttons.EnablebtnMovePrev(true);
                if (bs1.Position == 0) recbuttons.EnablebtnMovePrev(false);
                if (bs1.Position == bs1.Count - 1) recbuttons.EnablebtnMoveNext(false);
            }
        }

        

        

        public static void DisplayOtherForms(Form frmLocked, bool blnDisplay)
        {
            try
            {
                FormCollection frmsOpen = Application.OpenForms;

                foreach (Form frm in frmsOpen) if (frm != frmLocked && frm.Name != "frmLogin")
                        frm.Visible = blnDisplay;
                frmLocked.Focus();
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "DisplayOtherForms", ex.Message);
            }
        }

        public static void SetSizeClass(string strSizeClass,string strVehID, 
            bool blnVINpassed=false)
        {
            //Update AutoportExportVehicles table for record with VehID or VIN passed in
            //  as strVehID
            //Update the recs:
            //SizeClass
            //EntryFee
            //PerDiemGraceDays
            //UpdatedBy
            //UpdatedDate
            try
            {
                string strSQL;

                strSQL = @"UPDATE veh
                SET veh.SizeClass='" + strSizeClass + "'," +
                @"veh.EntryRate=rates.EntryFee,
                veh.PerDiemGraceDays=rates.PerDiemGraceDays,
                UpdatedBy = '" + Globalitems.strUserName + "'," +
                @"UpdatedDate = CURRENT_TIMESTAMP
                FROM AutoportExportVehicles veh
                LEFT OUTER JOIN AutoportExportRates rates on 
                rates.CustomerID = veh.CustomerID
                    AND rates. RateType = 'Size " + strSizeClass + @" Rate' ";

                //ID row by AutoportExportVehiclesID or VIN in WHERE Clause
                if (blnVINpassed)
                    strSQL += "WHERE veh.VIN = '" + strVehID + "' ";
                else
                    strSQL += "WHERE veh.AutoportExportVehiclesID = " + strVehID;

                strSQL += @" AND veh.DateReceived >= rates.StartDate
                AND veh.DateReceived < 
                    DATEADD(day, 1, ISNULL(rates.EndDate, '12/31/2099'))";

                DataOps.PerformDBOperation(strSQL);
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "SetSizeClass", ex.Message);
            }
        }

        public static void DisplayExceptiontoUser()
        {
            try
            {
                string strMsg = "";

                // \n embedded in string from SettingTable don't display newlnes in Messagebox displayed
                for (int i = 0; i < Globalitems.lsExceptionMessageLines.Count; i++)
                {
                    if (i > 0) strMsg += "\n\n";
                    strMsg += Globalitems.lsExceptionMessageLines[i];
                }

                MessageBox.Show(strMsg, Globalitems.EXCEPTION_SUBJECT,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "DisplayExceptiontoUser", ex.Message);
            }
            
        }
        public static void SetUpGlobalVariables()
        {
            try
            {
                string strSQL;
                string[] strExceptionMsgs;
                DataSet ds;

                // Retrieve Code table
                strSQL = "SELECT * FROM Code";
                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (blnException) return;

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    HandleException(CURRENTMODULE, "SetUpGlobalVariables",
                        "No records were returned from Code table");
                    return;
                }
                dtCode = ds.Tables[0];

                // Retrieve EmailExceptionToMember, ExceptionSubject,ExceptionMessage,
                // InactiveMinutesToCloseProgram,SMTPUsername,
                //SMTPServer,SMTPPassword  Global items from SettingTable
                strSQL = @"Select ValueKey,ValueDescription 
                    FROM SettingTable
                    WHERE ValueKey IN('EmailExceptionToMember', 'ExceptionSubject', 
                    'ExceptionMessage', 'InactiveMinutesToCloseProgram','SecondsForClosingProgram',
                    'SMTPUsername', 'SMTPServer', 'SMTPPassword'); ";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (blnException) return;

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    HandleException(CURRENTMODULE, "SetUpGlobalVariables", 
                        "No records were returned from SettingTable");
                    return;
                }

                foreach (DataRow dtRow in ds.Tables[0].Rows)
                {
                    switch (dtRow["ValueKey"].ToString())
                    {
                        case "SMTPUsername":
                            EMAIL_FROM = dtRow["ValueDescription"].ToString();
                            break;

                        case "SMTPServer":
                            EMAIL_CLIENT = dtRow["ValueDescription"].ToString();
                            break;

                        case "SMTPPassword":
                            EMAIL_PASSWORD = dtRow["ValueDescription"].ToString();
                            break;

                        case "EmailExceptionToMember":
                            //Just email me if in TEST mode
                            if (runmode == "TEST")
                            {
                                if (dtRow["ValueDescription"].ToString().Contains("Maibor"))
                                    EXCEPTION_TO_LIST.Add(dtRow["ValueDescription"].ToString());
                            }
                            else
                                EXCEPTION_TO_LIST.Add(dtRow["ValueDescription"].ToString());
                            break;

                        case "ExceptionSubject":
                            EXCEPTION_SUBJECT = dtRow["ValueDescription"].ToString();
                            break;

                        case "InactiveMinutesToCloseProgram":
                            INACTIVEMINUTESTOCLOSEPROGRAM =
                                Convert.ToInt16(dtRow["ValueDescription"]);
                            break;

                        case "SecondsForClosingProgram":
                            SECONDSFORCLOSINGPROGRAM =
                               Convert.ToInt16(dtRow["ValueDescription"]);
                            break;

                        case "ExceptionMessage":
                            EXCEPTION_MESSAGE = dtRow["ValueDescription"].ToString();

                            // \n embedded in string from SettingTable don't display newlnes in Messagebox displayed
                            //Split on '~' and load into lsExceptionMessageLines so MessageBox displays
                            // new lines for Exceptions
                            strExceptionMsgs = EXCEPTION_MESSAGE.Split('~');

                            for (int i = 0; i < strExceptionMsgs.Length; i++)
                                lsExceptionMessageLines.Add(strExceptionMsgs[i]);
                            break;
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "SetUpglobalVariables", ex.Message);
            }
        }

        public static string FormatDatetime(string strval)
        {
            //Put strval into m/d/yyyy h:mm AM/PM if it is not empty
            try
            {
                bool blnDate;
                DateTime datval;

                //Return empty string if blank
                if (strval.Trim().Length == 0) return "";

                //Verify that strval is a date
                blnDate = DateTime.TryParse(strval,out datval);

                //Return in standard format if successful conversion
                if (blnDate)
                    // Only return date if time is 12:00 AM (midnight)
                    if (datval.TimeOfDay.Ticks == 0)
                        return datval.ToString("M/d/yyyy");
                    else
                        return datval.ToString("M/d/yyyy h:mm tt");
                else
                    return "INVALID";
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "FormatDatetime", ex.Message);
                return "";
            }
        }


        public static string HandleSingleQuoteForSQL(string strText)
        {
            try
            {
                strText = strText.Trim();
                strText = strText.Replace("'", "''");
                return strText;
            }
            
            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "HandleSingleQuoteForSQL", ex.Message);
                return "";
            }
        }

        public static string FormatCurrency(string strText)
        {
            decimal decval;

            try
            {
                if (strText.Length == 0) return "";

                // if strText has a value convert to Decimal, and return using Formt 'C', currency
                decval = Convert.ToDecimal(strText);
                return string.Format("{0:C}",decval);
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "FormatCurrency", ex.Message);
                return "";
            }
        }

        public static void FormatTwoDecimal(ref string strDec)
        {
            //strDec will have 0 or 1 decimal points
            //If 1st char is decimal point, add leading zero
            //If one digit after decimal point, add trailing zero
            //If more than two digits after decimal point, round up, e.g. 1.559 -> 1.56

            try
            {
                decimal decval;

                decval = decimal.Round(Convert.ToDecimal(strDec),2);
                strDec = decval.ToString("0.00");                 
                    }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "ValidDecimal", ex.Message);
            }
        }


        public static void CheckDateKeyPress(KeyPressEventArgs e)
        {
            //Only allow digits, backspace, forward slash (/), dash (-) 
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) &&
                e.KeyChar != '/' && e.KeyChar != '-') e.Handled = true;
        }

        public static void ValidateDate(TextBox txtbox, CancelEventArgs e)
        {
            // Use ValidDate. If true, strval is in proper date format
            //  If false, don't allow movement from control
            string strval = txtbox.Text.Trim();

            if (ValidDate(ref strval))
            { txtbox.Text = strval; }
            else
                e.Cancel = true;
        }

        public static Boolean ValidDateKeyStroke(char chr)
        {
            try
            {
                if (!char.IsDigit(chr) && !char.IsControl(chr) &&
                    chr != '/' && chr != '-') return false;

                return true;
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "ValidDateKeyStroke", ex.Message);
                return false;
            }
        }

        public static Boolean ValidDate(ref string strDate)
        {
            try
            {
                //Valid entries:
                //strDate can be an empty string 
                //If not an empty string:
                //First char must be a digit
                //Valid separators are '/' or '-'
                //strDate cannot contain both '/' and '-'
                //strDate must contain all digits or up to two separators
                //Last char cannot be separator

                //If strDate passes the above criteria, use ValidDateWithSeparator or ValidDateWithDigits to decode

                bool blnDate;
                char chrSeparator = '~';
                int intval;

                string strval = strDate;

                if (strDate.Length == 0) return true;

                //1st char must be a digit. Use int.TryParse to check for digit
                if (!int.TryParse(strval.Substring(0, 1), out intval)) return false;

                //strDate cannot contain both '/' and '-'
                if (strval.Contains("/") && strval.Contains("-")) return false;

                //Remove '/' & '-', then check if value is an integer
                strval = strval.Replace("/", "");
                strval = strval.Replace("-", "");
                if (!int.TryParse(strval, out intval)) return false;

                //Make sure no more than two separators in strDate
                intval = strDate.Length - strDate.Replace(chrSeparator.ToString(), "").Length;
                if (intval > 2) return false;

                //Check that last char is not a separator
                if (strDate[strDate.Length - 1] == chrSeparator) return false;

                //strDate contains only digits and possibly '/' or '-' 

                //Use GetDateWithSeparator if strDate has '/' or '-'
                if (strDate.Contains("/")) chrSeparator = '/';
                if (strDate.Contains("-")) chrSeparator = '-';
                if (chrSeparator != '~')
                {
                    blnDate = ValidDateWithSeparator(ref strDate, chrSeparator);

                    return blnDate;
                }

                //Use GetDateWithDigits
                blnDate = ValidDateWithDigits(ref strDate);

                return blnDate;
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "ValidDate", ex.Message);
                return false;
            }
        }

        private static bool ValidDateWithDigits(ref string strDate)
        {
            try
            {
                int intval;
                int intDay = 0;
                int intDaysInMonth;
                int intMonth = DateTime.Today.Month;
                int intYear = DateTime.Today.Year;

                //Valid entries
                // Two digits, representing must be 1-12; representing day must be 1-31.
                // Day specified must be valid for the specified month. E.g.: 0230 (2/30/17) is invalid.
                // Length 1: must be 1-9, day of current month, year.
                // Length 2: digits 1-2 must be 1-31. intDay of current month, year.
                // Length 3: digits 1-2 must be 1-31. If 1-12 set to intMonth; otherwise set to intDay. 
                //  Last digit is month or day, depending on setting of digits 1-2
                // Length 4: digits 1-2 must be 1-31. Decode as above. digits 3-4 decode as month/day, based
                //  on first what digits 1-2 decode to.
                // Length 5-7: decode 1st 4 digits as above. Add digits 5-7 as integer value to 2000 for the year.
                // Length 8: decode last 4 digits as year.
                // Length 9 or higher. Decode 1st 8 digits. Ignore the rest 

                    if (strDate.Length == 1)
                    {
                        //Convert digit 1 to int value, intDay
                        if (!int.TryParse(strDate[0].ToString(), out intDay))
                        {
                            HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert Day");
                            return false;
                        };

                        //intDay cannot be 0
                        if (intDay == 0) return false;
                    }

                    if (strDate.Length == 2)
                    {
                        //Convert digits 1-2 to int value, intDay
                        if (!int.TryParse(strDate.Substring(0,2), out intDay))
                        {
                            HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert digits 1-2 to intval");
                            return false;
                        };

                        //intDay cannot be 0 or > 31
                        if (intDay == 0 || intDay > 31) return false;
                    }   // if strDate length = 2

                    if (strDate.Length == 3)
                    {
                        //Convert digits 1-2 to int value, intval
                        if (!int.TryParse(strDate.Substring(0,2), out intval))
                        {
                            HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert digits 1-2 to intval");
                            return false;
                        };

                        //intval cannot be 0 or > 31
                        if (intval == 0 || intval > 31) return false;

                        //Set intval to intMonth or intDay
                        if (intval < 13)
                            { intMonth = intval;}
                        else
                            { intDay = intval;}

                        //Convert last digit
                        if (!int.TryParse(strDate[2].ToString(), out intval))
                        {
                            HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert last digit 1 to intval");
                            return false;
                        };

                        //intval cannot be 0
                        if (intval == 0) return false;

                        //Set intval to intMonth or intDay
                        if (intDay == 0)
                            {intDay = intval;}
                        else
                            {intMonth = intval;}
                    }   // if strDate length = 3


                    if (strDate.Length == 4)
                    {
                        //Convert digits 1-2 to int value, intval
                        if (!int.TryParse(strDate.Substring(0,2), out intval))
                        {
                            HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert digits 1-2 to intval");
                            return false;
                        };

                        //intval cannot be 0 or > 31
                        if (intval == 0 || intval > 31) return false;

                        //Set intval to intMonth or intDay
                        if (intval < 13)
                        { intMonth = intval;
                        }
                        else
                        { intDay = intval;
                        }

                        //Convert last two digits
                        if (!int.TryParse(strDate.Substring(2,2), out intval))
                        {
                            HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert last digit 1 to intval");
                            return false;
                        };

                        //intval cannot be 0 or > 31
                        if (intval == 0 || intval > 31) return false;

                        //Set intval to intMonth or intDay
                        if (intDay == 0)
                            {intDay = intval;}
                        else
                            {intMonth = intval;}

                    }   // if strDate length = 4

                if (strDate.Length > 4 && strDate.Length < 8)
                {
                    //Convert digits 1-2 to int value, intval
                    if (!int.TryParse(strDate.Substring(0,2), out intval))
                    {
                        HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert digits 1-2 to intval");
                        return false;
                    };

                    //intval cannot be 0 or > 31
                    if (intval == 0 || intval > 31) return false;

                    //Set intval to intMonth or intDay
                    if (intval < 13)
                        {intMonth = intval;}
                    else
                        {intDay = intval;}

                    //Convert next two digits
                    if (!int.TryParse(strDate.Substring(2, 2), out intval))
                    {
                        HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert next two digits to intval");
                        return false;
                    };

                    //intval cannot be 0 or > 31
                    if (intval == 0 || intval > 31) return false;

                    //Set intval to intMonth or intDay
                    if (intDay == 0)
                        {intDay = intval;}
                    else
                        {intMonth = intval;}

                    //Convert last 1-3 digits to 2000 for year
                    if (!int.TryParse(strDate.Substring(4), out intval))
                    {
                        HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert final year digits to intval");
                        return false;
                    };

                    intYear = 2000 + intval;

                }   // if strDate length 5-7

                //Length is 8 or more. Decode digits 5-8 as year. 
                if (strDate.Length > 7)
                {
                    //Convert digits 1-2 to int value, intval
                    if (!int.TryParse(strDate.Substring(0,2), out intval))
                    {
                        HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert digits 1-2 to intval");
                        return false;
                    };

                    //intval cannot be 0 or > 31
                    if (intval == 0 || intval > 31) return false;

                    //Set intval to intMonth or intDay
                    if (intval < 13)
                        {intMonth = intval;}
                    else
                        {intDay = intval;}

                    //Convert next two digits
                    if (!int.TryParse(strDate.Substring(2, 2), out intval))
                    {
                        HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert next two digits to intval");
                        return false;
                    };

                    //intval cannot be 0 or > 31
                    if (intval == 0 || intval > 31) return false;

                    //Set intval to intMonth or intDay
                    if (intDay == 0)
                        {intDay = intval;}
                    else
                        {intMonth = intval;}

                    //Convert next 4 digits as year
                    if (!int.TryParse(strDate.Substring(4,4), out intYear))
                    {
                        HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert final year digits to intval");
                        return false;
                    };
                }   // if strDate length > 7

                //Verify intMonth is 1-12
                if (intMonth > 12) return false;

                //Verify intDay is within the number of days in the  month
                intDaysInMonth = DateTime.DaysInMonth(intYear, intMonth);
                    if (intDay > intDaysInMonth) return false;

                // format is OK. return mm/dd/yyyy
                strDate = intMonth.ToString("00") + "/" + intDay.ToString("00") +
               "/" + intYear.ToString("0000");

                return true;
            }   // end try

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "GetDateWithDigits", ex.Message);
                return false;
            }
        }   //validDateWithDigits

        private static bool ValidDateWithSeparator(ref string strDate, char chrSeparator)
        {
            //Check if string with separator can be decoded as a valid date
            // Separator must appear after 1-2 digits if a separator is used; 317/5 is invalid. 

            //Aproach:
            //Use int.TryParse([string],out [int variable]). Returns bool value, whether conversion was 
            //  successful. if successful value is returned in int variable.
            //Initialize day = 0, month, year = current month, year
            //strDate must be 3 or more chas
            //Chars 1-3, up to chars 1-5 must be month & day, m/d up to mm/dd or dd/mm
            //Get Char 1 or Chars 1-2, if both digits, & convert to int. 
            //Int value must be 1-31. If 1-12, value is a month, otherwise a day.
            //Next char must be a separator
            //Get next Char 1 or Chars 1-2, if both digits.  
            //Int value must be 1-31. If intDay = 0, value must be day, otherwise month (must be 1-12).
            //If additional chars, decode year.
            //If 1-3 remaining chars, int value is 0-999. Add to 2000 to get year.
            //If remaining chars are 4 or more, take 1st chars as year
            // If a valid entry, return mm/dd/yyyy format

            try
                {
                bool blnContinue = true;
                char chr;
                int intNextpos = 0;
                int intval;
                int intDay = 0;
                int intDaysInMonth;
                int intMonth = DateTime.Today.Month;
                int intYear = DateTime.Today.Year;
                string strval = "";

                if (strDate.Length < 3) return false;

                //Char 1-2 must be 'nn' or 'n[separator] n/ or n-

                //If char 2 is separator, digit 1 must be month 1-9
                chr = strDate[1];
                if (chr == chrSeparator)
                {
                    //Convert month to int value, intMonth
                    if (!int.TryParse(strDate[0].ToString(), out intMonth))
                    {
                        HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert Month");
                        return false;
                    };

                    //intMonth must be 1-9
                    if (intMonth == 0) return false;

                    intNextpos = 2;
                }
                
                // If char 2 is a digit, 1st 2 chars must be 1-31
                if (chr != chrSeparator)
                {
                    //Convert digits 1-2 to int value, intval
                    if (!int.TryParse(strDate.Substring(0,2), out intval))
                    {
                        HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert Digits 1-2");
                        return false;
                    };

                    //intval must be 1-31
                    if (intval == 0 || intval > 31) return false;

                    //Digits 1-2 are 1-31, determine if month or day
                    if (intval < 13)
                        { intMonth = intval; }
                    else
                        { intDay = intval; }

                    intNextpos = 3;
                }

                //Char at intNextpos must be a digit
                chr = strDate[intNextpos];
                if (chr == chrSeparator) return false;

                //Check if string only contains month & day
                if (intNextpos+1 == strDate.Length)
                {
                    //Convert digits 1 to int value, intval
                    if (!int.TryParse(strDate.Substring(intNextpos, 1), out intval))
                    {
                        HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert Digit 1 to intval.");
                        return false;
                    };

                    //intval must be 1-9
                    if (intval == 0) return false;

                    if (intDay == 0)
                        { intDay = intval; }
                    else
                        { intMonth = intval; }

                    blnContinue = false;
                }

                //Check if string ends with two digits
                if (intNextpos+2 == strDate.Length)
                {
                    //Convert digits 1-2 to int value, intval
                    if (!int.TryParse(strDate.Substring(intNextpos, 2), out intval))
                    {
                        HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert Digit 1 to intval.");
                        return false;
                    };

                    //intval must be 1-9
                    if (intval == 0 || intval > 31) return false;

                    if (intDay == 0)
                    { intDay = intval; }
                    else
                    {
                        intMonth = intval;
                        if (intMonth > 12) return false;
                    }
                    blnContinue = false;
                }

                //Check intNextpos + 1 Char
                if (blnContinue)
                {
                    //At this point we have intMonth or intDay. If blnContinue, decode month or day, and Year
                    //  intNextpos is start of Digits 1-2
                    chr = strDate[intNextpos + 1];

                    //Single digit for second value
                    if (chr == chrSeparator)
                    {
                        //Convert digit 1 to int value, intval
                        if (!int.TryParse(strDate.Substring(intNextpos, 1), out intval))
                        {
                            HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert Digit 1 to intval.");
                            return false;
                        };

                        //intval must be 1-9
                        if (intval == 0) return false;

                        if (intDay == 0)
                        { intDay = intval; }
                        else
                        { intMonth = intval; }

                        intNextpos += 2;
                    }

                    //Two digits for second value
                    if (chr != chrSeparator)
                    {
                        //Convert digits 1-2 to int value, intval
                        if (!int.TryParse(strDate.Substring(intNextpos, 2), out intval))
                        {
                            HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert Digit 1-2 to intval.");
                            return false;
                        };

                        //intval must be 1-31
                        if (intval == 0 || intval > 31) return false;

                        if (intDay == 0)
                        { intDay = intval; }
                        else
                        { intMonth = intval; }

                        intNextpos += 3;
                    }

                    // Get year
                    strval = strDate.Substring(intNextpos);

                    //If year entry is less than 4 digits, add to 2000 for year. 
                    //  Otherwise take 1st 4 digits as year
                        if (strval.Length < 4)
                        {
                            //Convert strval to int value, intval
                            if (!int.TryParse(strval, out intval))
                            {
                                HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert Digit 1 at pos: " + intNextpos + " to intval");
                                return false;
                            };

                            intYear = 2000 + intval;
                        }
                        else
                        {
                            //Convert 1st 4 digits of strval to int value, intYear
                            if (!int.TryParse(strval.Substring(0, 4), out intYear))
                            {
                                HandleException(CURRENTMODULE, "ValidDateWithSeparator", "Could not convert Digit 1 at pos: " + intNextpos + " to intval");
                                return false;
                            };
                        }
                }   // if blnContinue

                // Get intDaysInMonth and make sure intDay is valid
                intDaysInMonth = DateTime.DaysInMonth(intYear, intMonth);
                if (intDay > intDaysInMonth) return false;

                // format is OK. return mm/dd/yyyy
                strDate = intMonth.ToString("00") + "/" + intDay.ToString("00") +
               "/" + intYear.ToString("0000");

                return true;
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "ValidDateWithSeparator", ex.Message);
                return false;
            }
        }   //ValidDateWithSeparator

        public static string GetDamageCodeSeverityDescription(char chrSeverity)
        {
            DataView dv;
            string strFilter;

            try
            {
                strFilter = "CodeType = 'DamageSeverityCode' AND Code = '" + chrSeverity + "'";
                dv = new DataView(dtCode, strFilter, "Code", DataViewRowState.CurrentRows);
                if (dv.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GetDamageCodeSeverityDescription",
                        "No Severity found in Code table");
                    return "";
                }

                return dv[0]["CodeDescription"].ToString();

            }

            catch(Exception ex)
            {
                HandleException(CURRENTMODULE, "GetDamageCodeSeverityDescription", ex.Message);
                return "";
            }
        }
        
        public static string GetDamageCodeDescription(string strDamageCode)
        {
            DataView dv;
            string strDescription = "";
            string strFilter;
            string strval;

            try
            {
                //Get Location, Digits:1-2 of strDamageCode
                strval = strDamageCode.Substring(0, 2);
                strFilter = "CodeType = 'DamageAreaCode' AND Code = '" + strval + "'";
                dv = new DataView(dtCode, strFilter, "Code", DataViewRowState.CurrentRows);
                if (dv.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GetDamageCodeDescription",
                        "No Location found in Code table");
                    return "";
                }
                strDescription = dv[0]["CodeDescription"].ToString();

                //Get Type, Digits:3,4 of strDamageCode
                strval = strDamageCode.Substring(2, 2);
                strFilter = "CodeType = 'DamageTypeCode' AND Code = '" + strval + "'";
                dv = new DataView(dtCode, strFilter, "Code", DataViewRowState.CurrentRows);
                if (dv.Count == 0)
                {
                    Globalitems.HandleException(CURRENTMODULE, "GetDamageCodeDescription",
                        "No Area found in Code table");
                    return "";
                }
                strDescription += "; " + dv[0]["CodeDescription"].ToString();

                strDescription = HandleSingleQuoteForSQL(strDescription);
                return strDescription;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetDamageCodeDescription", ex.Message);
                return "";
            }
        }       

        public static void FillComboboxFromCodeTable(string strFilter,ComboBox cbo,Boolean blnAll,
           Boolean blnList)
        {
            //Retrieve items from dtCode and put into cbo
            // If blnAll add 'All' as 1st item
            // If blnList, create a list of Comboboxitems and set as cbo's datasource
            // NOTE: if the cbo is used in record detail, and needs to set the cbo's Selected Value, the cbo
            //  must have a datasource assigned to it
            try
            {
                ComboboxItem cboitem;
                DataView dv;
                List<ComboboxItem> lsComboboxitems = new List<ComboboxItem>();

                //Add 'All' as 1st item, if blnAll
                if (blnAll)
                {
                    // Add All to cbo
                    cboitem = new ComboboxItem();
                    cboitem.cboText = "All";
                    cboitem.cboValue = "All";
                   
                    if (blnList)
                    {
                        lsComboboxitems.Add(cboitem);
                    }
                    else
                    {
                        cbo.Items.Add(cboitem);
                    }
                }

                //Retrieve values from dtCode for remaining items of cbo

                //Use Dataview to filter dtCode for RecordStatus
                dv = new DataView(Globalitems.dtCode, strFilter, "SortOrder,Code", DataViewRowState.CurrentRows);

                // Fill cbo or list with values from dtCode
                foreach (DataRowView dvrow in dv)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dvrow["CodeDescription"].ToString();
                    cboitem.cboValue = dvrow["Code"].ToString();

                    if (blnList)
                    {
                        lsComboboxitems.Add(cboitem);
                    }
                    else
                    {
                        cbo.Items.Add(cboitem);
                    }
                }

                cbo.DisplayMember = "cboText";
                cbo.ValueMember = "cboValue";

                if (blnAll)
                {
                    cbo.SelectedIndex = 0;
                }
                else
                {
                    cbo.SelectedIndex = -1;

                    if (blnList) cbo.DataSource = lsComboboxitems;                    
                }
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "FillComboboxFromCodeTable", ex.Message);
            }
        }

        public static List<T> CreateListFromDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            T obj;
            int introw = 0;

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    obj = GetObject<T>(dr);
                    if (blnException)
                    {
                        return data;
                    }

                    data.Add(obj);
                    introw += 1;
                }
                return data;
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "CreateListFromDataTable", ex.Message);
                return data;
            }
        }

        private static T GetObject<T>(DataRow dr)
        {
            T obj = Activator.CreateInstance<T>();

            try
            {
                Type typeObj = typeof(T);   
                PropertyInfo[] propinfo;
                List<PropertyInfo> lsPropinfo;

                //Load propinfo array with all the properties in the class T
                propinfo = typeObj.GetProperties();

                //Loop thru each col in the table
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    if (!Convert.IsDBNull(dr[dc.ColumnName]))
                    {
                        //    //Use LINQ to get the matching propertyinfo for the current ColumnName 
                        //    // from the array propinfo
                        var query = from propx in propinfo
                                    where propx.Name == dc.ColumnName
                                    select propx;

                        //Put the property info records found into a list
                        lsPropinfo = query.ToList<PropertyInfo>();

                        //Update obj if the property can be updated
                        if (lsPropinfo[0].CanWrite) lsPropinfo[0].SetValue(obj, dr[dc.ColumnName]);
                    }
                 }
                return obj;
            }

            catch(Exception ex)
            {
                HandleException(CURRENTMODULE, "GetObject", ex.Message);
                return obj;
            }
        }

        public static void SetBillToCbo(ref ComboBox cbo,string strCustomerID)
        {
            try
            {
                ComboboxItem cboitem;
                DataSet ds;
                string strSQL;

                cbo.Items.Clear();

                //Get BillToCustomerID as cbovalue, BillToCustomer Name/Shortname as cbotext
                strSQL = @"SELECT
                Code.Value2 as cbovalue,
                CASE
                    WHEN LEN(RTRIM(ISNULL(BIllto.ShortName, ''))) > 0 THEN RTRIM(billto.ShortName)
                     ELSE RTRIM(billto.CustomerName)
                 END AS cbotext
                 FROM CODE
                 INNER JOIN Customer billto on billto.CustomerID = Code.Value2
                 WHERE CodeType = 'BillToCustomer' and Value1 = '" + 
                 strCustomerID + "'";

                ds = DataOps.GetDataset_with_SQL(strSQL);
                if (blnException) return;

                if (ds.Tables[0].Rows.Count == 0) return;

                //There is at least one row for BillTo
                //Set 1st item as <select>;
                cboitem = new ComboboxItem();
                cboitem.cboText = "<select>";
                cboitem.cboValue = "select";
                cbo.Items.Add(cboitem);

                //Set 2nd item as <none>;
                cboitem = new ComboboxItem();
                cboitem.cboText = "(none)";
                cboitem.cboValue = "none";
                cbo.Items.Add(cboitem);

                //Set remaining items from ds
                foreach (DataRow dtrow in ds.Tables[0].Rows)
                {
                    cboitem = new ComboboxItem();
                    cboitem.cboText = dtrow["cbotext"].ToString();
                    cboitem.cboValue = dtrow["cbovalue"].ToString();
                    cbo.Items.Add(cboitem);
                }

                cbo.DisplayMember = "cboText";
                cbo.ValueMember = "cboValue";
                cbo.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "SetBilltoCbo", ex.Message);
            }
        }

        
        public static DateTime GetBuildDate(this Assembly assembly)
        {
            try
            {
                //Using System.Reflection, retrieve Build date from .exe assembly
                const int c_PeHeaderOffset = 60;
                const int c_LinkerTimestampOffset = 8;

                
                string filePath = assembly.Location;
                byte[] buffer = new byte[2048];
                TimeZoneInfo target = TimeZoneInfo.Local;

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    stream.Read(buffer, 0, 2048);

                Int32 offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
                Int32 secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                DateTime linkTimeUtc = epoch.AddSeconds(secondsSince1970);

                //TimeZoneInfo tz = target ?? TimeZoneInfo.Local;
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, target);

                return localTime;
            }

            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetLinkerTime", ex.Message);
                return DateTime.MinValue;
            }
        }

        static public void HandleException(string strModule, string strMethod, string strEx,
            bool blnManualProgram = true)
        {
            string strBody;

            if (!blnManualProgram) blnException = true;
            strBody = "The following exception occurred:<br>User Name: " + Globalitems.strUserName + "<br>" +
                "Date/Time: " + DateTime.Now.ToString("M/d/yy h:mm tt") + "<br>" +
                 "Module: " + strModule + "<br>" +
                "Method: " + strMethod + "<br>" + 
                "Exception: " + strEx;

            //if (blnManualProgram) sendemail(EXCEPTION_TO_LIST, 
            //    "PROGRAM EXCEPTION IN EXPORT PROGRAM", strBody);
            if (blnManualProgram) sendemail(EXCEPTION_TO_LIST,
                "PROGRAM EXCEPTION IN EXPORT PROGRAM", strBody);

            //Replace <br> for email with ',' for the ExceptionMessage field in the ProgramException table
            strBody = strBody.Replace("<br>", " ~ ");
            DataOps.CreateExceptionLogRecord(strBody);

            if (blnManualProgram)
            {
                DisplayExceptiontoUser();
                Application.Exit();
            } 
        }

        static public string validpassword(string strPassword)
        {
            try
            {
                char chr;
                int intrepeatchars;

                //Ck that Password is > 3 chars
                if (strPassword.Length < 4) return "SHORT";


                //Ck that Password does not start with DAI
                if (strPassword.ToUpper().StartsWith("DAI")) return "DAI";
               

                //Ck that Password does not repeat the same character 4 or more times
                chr = strPassword[0];
                intrepeatchars = 0;

                for (int i = 1; i < strPassword.Length; i++)
                {
                    if (strPassword[i] == chr)
                    {
                        intrepeatchars += 1;
                        if (intrepeatchars == 3) return "REPEAT";
                    }
                    else //current chr is not the same as previous
                    {
                        intrepeatchars = 0;
                        chr = strPassword[i];
                    }
                }

                //Confirm Password is not ascending or descending numerical sequence
                if (strPassword.Contains("0123") || strPassword.Contains("1234") ||
                    strPassword.Contains("2345") || strPassword.Contains("3456")) return "SEQ";

                if (strPassword.Contains("4567") || strPassword.Contains("5678") ||
                    strPassword.Contains("6789")) return "SEQ";

                if (strPassword.Contains("3210") || strPassword.Contains("4321") ||
                    strPassword.Contains("5432") || strPassword.Contains("6543")) return "SEQ";

                if (strPassword.Contains("7654") || strPassword.Contains("8765") ||
                    strPassword.Contains("9876")) return "SEQ";

                return "OK";
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "validpassword", ex.Message);
                return "EXCEPTION";
            }
        }

        static public bool validemailaddress(string strEmailAddress)
        {
            try
            {
                MailAddress mlAddr;
                mlAddr = new MailAddress(strEmailAddress);
                return true;
            }

            catch (FormatException)
            {
                return false;
            }

            catch (Exception ex)
            {
                HandleException(CURRENTMODULE, "validemailaddress", ex.Message);
                return false;
            }
        }

        static public void sendemail(List<string> strTo, string strSubject, string strBody,
           List<string> strCC = null, List<string> strBCC = null)
        {
            // 12/7/17 Per Kenny:
            // 192.168.1.148, with credentials, allows sending of INTERNAL emails only
            // 192.168.1.147, with credentials, allows sending of EXTERNAL emails only &
            //  will be going away
            // 192.168.150.244, no credentials needed, MUST BE RUN FROM DAITRACKER 3, 
            //  allows BOTH internal & external emails using Edgewave Email security appliance
            //
            // If BOTH internal & external emails need to be sent from this program, will
            //  need Email Web service, similar to VINDecode service, running on DAITRACKER 3   `
            try
            {
                //Make sure strTo has at least one value
                if (strTo.Count == 0)
                {
                    HandleException("DataOps", "sendemail", "No elements in strTo array");
                    return;
                }

                MailMessage message = new MailMessage();
                // Add each To address
                foreach (string toemail in strTo) message.To.Add(new MailAddress(toemail));

                // Add standard From address
                message.From = new MailAddress(Globalitems.EMAIL_FROM);

                // Add any CC addresses
                if (strCC != null)
                {
                    foreach (var ccrecipient in strCC) message.CC.Add(new MailAddress(ccrecipient));
                }

                // Add any BCC addresses
                if (strBCC != null)
                {
                    foreach (var bccrecipient in strCC) message.CC.Add(new MailAddress(bccrecipient));
                }

                //NOTE: cannot send external emails w/current credentials and SMTP setting
                message.Subject = strSubject;
                message.Body = strBody;
                message.IsBodyHtml = true;

                SmtpClient mailserver = new SmtpClient(Globalitems.EMAIL_CLIENT,25);

                mailserver.EnableSsl = false;
                mailserver.Credentials = new System.Net.NetworkCredential(Globalitems.EMAIL_FROM,
                    Globalitems.EMAIL_PASSWORD);


                mailserver.Send(message);
            }
            catch (Exception ex)
            {
                string strMsg;

                // Create only Log record since exception occurred in sendemail method
                strMsg = "The following exception occurred:~User Name: " + Globalitems.strUserName + "~" +
                 "Date/Time: " + DateTime.Now.ToString("M/d/yy h:mm tt") + "~" +
                  "Module: " + CURRENTMODULE + "~" +
                 "Method: sendemail~" +
                 "Email Subject: " + strSubject + "~" +
                 "Email Body: " + strBody +
                 "Exception: " + ex.Message;
                
                DataOps.CreateExceptionLogRecord(strMsg);
            }
        }
    }
}
