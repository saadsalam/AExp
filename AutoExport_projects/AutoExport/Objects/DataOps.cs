using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace AutoExport.Objects
{
    public static class DataOps
    {
        // 1/15/19 D.Maibor: Create each SQLConnection within the method; see if occaisonal Transport Exceptions are eliminated.

         public const string CURRENTMODULE = "DataOps";
         public const String PROD_CONNECTIONSTRING_AUTOEXPORT = 
            @"Data Source = 192.168.1.33; Initial Catalog = AutoExport;
            user id = daiuser; password=daiuser";
         public const String TEST_CONNECTIONSTRING_AUTOEXPORT = 
            @"Data Source = 192.168.1.33; Initial Catalog = AutoExport_Test;
            user id = daiuser; password=daiuser";

        //Store DataHub name in SettingTable of PROD/TEST AUTOEXPORT DB, once separate
        //  PROD/TEST DATAHUB DB's are set up for the new AutoExport program
        static private string PROD_CONNECTIONSTRING_DATAHUB =
            @"Data Source = 192.168.1.33; Initial Catalog = DataHub;
            user id = daiuser; password=daiuser";
        static private string TEST_CONNECTIONSTRING_DATAHUB =
           @"Data Source = 192.168.1.33; Initial Catalog = DataHub_Test;
            user id = daiuser; password=daiuser";

        static private string GetConnectionString(string strDB = "AUTOEXPORT")
        {
            if (strDB == "AUTOEXPORT")
            {
                if (Globalitems.runmode == "PROD")
                    return PROD_CONNECTIONSTRING_AUTOEXPORT;
                else
                    return TEST_CONNECTIONSTRING_AUTOEXPORT;
            }
            else
                if (Globalitems.runmode == "PROD")
                return PROD_CONNECTIONSTRING_DATAHUB;
            else
                return TEST_CONNECTIONSTRING_DATAHUB;
        }

        static public void CreateLoginRecord()
        {
            try
            {
                DateTime dtbuilddate;
                string strHostName = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
                string strSQL;

                dtbuilddate = Globalitems.GetBuildDate(Assembly.GetExecutingAssembly());

                strSQL = "INSERT INTO UserLoginLog (UserCode,LoginDate,SPID,HostName,BuildDate) " +
                    "VALUES ('" + Globalitems.strUserName + "'," +
                    "CURRENT_TIMESTAMP," +
                    "@@SPID," +
                    "'" + strHostName + "'," +
                    "'" + dtbuilddate.ToString() + "')";
                PerformDBOperation(strSQL);
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "CreateLoginRecord", ex.Message);
            }
        }

        static public void PerformBulkCopy(string strDestTable, DataTable dtInsertValues,
            string strDB = "AUTOEXPORT")
        {
            try
            { 
                using (SqlConnection connection = new SqlConnection(GetConnectionString(strDB)))
                {
                    connection.Open();
                    SqlBulkCopy sqlBulkcopy;
                    sqlBulkcopy = new SqlBulkCopy(connection);
                    sqlBulkcopy.DestinationTableName = strDestTable;
                    sqlBulkcopy.WriteToServer(dtInsertValues);
                }
            }

            catch (Exception ex)
            {Globalitems.HandleException(CURRENTMODULE, "PerformBulkCopy", ex.Message);}
        }

        static public void PerformDBOperation(string strSQL,string strDB = "AUTOEXPORT")
        {
            //12/22/17 D.Maibor: Remove try/catch so exception is passed back to calling method

            using (SqlConnection connection = new SqlConnection(GetConnectionString(strDB)))
            {
                SqlCommand sqlCmd = new SqlCommand();
                connection.Open();
                sqlCmd.Connection = connection;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = strSQL;
                sqlCmd.ExecuteNonQuery();
            }
        }

       
        static public DataSet GetDataset_with_SProc(string strSproc, 
            List<SProcParameter> sprocparams = null, 
            string strDB = "AUTOEXPORT")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString(strDB)))
                {
                    connection.Open();
                    SqlDataAdapter dsAdapter = new SqlDataAdapter();
                    DataSet dsData = new DataSet();
                    SqlCommand sqlCmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = strSproc,
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = 0
                    };

                    //Add SProc parameters, if any
                    if (sprocparams != null)
                    {
                        foreach (SProcParameter p in sprocparams)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(p.Paramname, p.Paramvalue));
                        }
                    }

                    dsAdapter.SelectCommand = sqlCmd;
                    dsAdapter.Fill(dsData);
                    return dsData;
                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetDataset_with_SProc", ex.Message);
                return null;
            }
        }        

        static public DataSet GetDataset_with_SQL(string strSQL, string strDB = "AUTOEXPORT")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString(strDB)))
                {
                    connection.Open();
                    SqlDataAdapter dsAdapter = new SqlDataAdapter();
                    DataSet dsData = new DataSet();
                    SqlCommand sqlCmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = strSQL,
                        CommandType = CommandType.Text,
                        CommandTimeout = 0
                    };

                    dsAdapter.SelectCommand = sqlCmd;
                    dsAdapter.Fill(dsData);
                    return dsData;

                }
            }
            catch (Exception ex)
            {
                Globalitems.HandleException(CURRENTMODULE, "GetDataset_with_SQL", ex.Message);
                return null;
            }
        }

        static public void CreateExceptionLogRecord(string strMessage)
        {
            // create a record in the ProgramException table
            try
            {
                string strSQL;

                // Handle Single quote in strMessage
                strMessage = strMessage.Replace("'", "''");

                strSQL = "INSERT INTO ProgramException (ExceptionMessage,Usercode,CreationDate) " +
                    "VALUES ('" + strMessage + "','" +
                    Globalitems.strUserName + "',CURRENT_TIMESTAMP)";
                PerformDBOperation(strSQL);
            }

            catch(Exception ex)
            {
                //Only send an email since there was a DB problem creating an Exception record
                string strBody;

                strBody = "The following exception occurred:<br>User Name: " + Globalitems.strUserName + "<br>" +
               "Date/Time: " + DateTime.Now.ToString("M/d/yy h:mm tt") + "<br>" +
                "Module: DataOps <br>" +
               "Method: CreateExceptionLogRecord<br>" +
               "Exception: " + ex.Message;

                Globalitems.sendemail(Globalitems.EXCEPTION_TO_LIST, "DATS AUTOPORT EXPORT EXCEPTION", strBody);
            }
        }

     }
}
