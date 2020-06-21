using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Leadtools;
using Leadtools.Pdf;

namespace Scorelink.BO.Helper
{
    public class Common
    {
        //For get License Leadtools in Server.
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

        //Create Folder
        //Parameter : Path
        public static void CreateDocFolder(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
        }

        //Delete Before Create Folder
        //Parameter : Path
        public static void InitailDocFolder(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
        }

        //Example   : Value 1 , Digit 3 = "001" | Value 10 , Digit 3 = "010"
        //Parameter : Value String , Digit Integer.
        //Return    : String.
        public static string GenZero(string value, int iDigit)
        {
            int iLen = value.Length;
            int iLoop = iDigit - iLen;
            string sZero = "";

            for (int i = 0; i < iLoop; i++)
            {
                sZero += "0";
            }

            string sOutput = sZero + value;

            return sOutput;
        }

        public static void PDFFileDeletePages(string sFrom , string sTo ,int iPage)
        {
            GetLicenseLeadTool();

            string sourceFileName = sFrom; //Path.Combine(LEAD_VARS.ImagesDir, @"Leadtools.pdf");
            string destinationFileName = sTo; //Path.Combine(LEAD_VARS.ImagesDir, @"LEAD_DeletePages.pdf");

            // Get the number of pages in the source file 
            PDFFile file = new PDFFile(sourceFileName);
            int pageCount = file.GetPageCount();
            Console.WriteLine("Pages in source file : {0}", pageCount);

            // If the file has more than 1 page, delete all except the first page 
            if (pageCount > 1)
            {
                // -1 is (up to and including last page) 
                file.DeletePages(2, -1, destinationFileName);
            }
        }
    }
}
