using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Scorelink.BO.Helper
{
    public class Common
    {
        public static void GetLicenseLeadTool()
        {
            string sPath = Consts.LeadtoolsLIC;
            try
            {
                string licenseFilePath = sPath + @"LEADTOOLS.LIC";
                string developerkeyPath = sPath + @"LEADTOOLS.LIC.key";
                developerkeyPath = Path.Combine(developerkeyPath);
                string developerkey = System.IO.File.ReadAllText(developerkeyPath);
                Leadtools.RasterSupport.SetLicense(licenseFilePath, developerkey);
            }
            catch (Exception ex)
            {

            }
        }

        public static void CreateDocFolder(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
