using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Scorelink.BO.Helper;

namespace Scorelink.BO.Helper
{
    public class Logger
    {
        private string sLogFormat;
        private string sLogDate;

        public Logger()
        {
            //sLogFormat used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString("");
            string sMonth = DateTime.Now.Month.ToString("00");
            string sDay = DateTime.Now.Day.ToString("00");
            sLogDate = sYear + sMonth + sDay;
        }

        public void ErrorLog(string sErrMsg)
        {
            string sLogPath = Common.getConstTxt("LogPath") + "Error_" + sLogDate;

            StreamWriter sw = new StreamWriter(sLogPath, true);
            sw.WriteLine(sLogFormat + sErrMsg);
            sw.Flush();
            sw.Close();
        }
    }
}
