using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace BoxOfficeUI.Util
{
    public class LogExceptions
    {
        public static void LogException(Exception exception)
        {
            try
            {
                if (exception is SqlException)
                {
                    LogExceptions.LogExceptionInFile(exception);
                    return;
                }

                LogExceptionInFile(exception);
            }
            catch (Exception ex)
            {

            }
        }

        private static void LogExceptionInFile(Exception ex)
        {
            try
            {
                var appLocation = System.AppDomain.CurrentDomain.BaseDirectory + "ErrorLog";

                if (!System.IO.File.Exists(appLocation))
                    System.IO.Directory.CreateDirectory(appLocation);

                string filePath = appLocation + @"\" + string.Format(@"Errors_on_{0}.txt", Convert.ToString(DateTime.Now.Date).Replace(" ", "_").Replace("\"", "_").Replace("-", "_").Replace(":", "_"));

                if (!System.IO.File.Exists(filePath))
                    System.IO.File.Create(filePath);

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("MESSAGE :" + ex.Message + "<br/>" + Environment.NewLine + "STACKTRACE :" + ex.StackTrace +
                       "" + Environment.NewLine + "DATE :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
            catch(Exception exc)
            {
            }
        }

        private static void LogExceptionInDataBase(Exception ex)
        {
           
        }
    }
}
