using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Scorelink.BO.Helper;
using Scorelink.MO.DataModel;
using Scorelink.BO.Repositories;
using System.IO;
using Spire.Xls;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;



namespace Scorelink.web.Controllers
{
    public class ScanResultController : Controller
    {
        // GET: ScanResult
        DocumentDetailRepo docDetailRepo = new DocumentDetailRepo();
        DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        ResultModel objModel = new ResultModel();
        ScanEditRepo GetField = new ScanEditRepo();
        public ActionResult Index(int docId, int pageType, string pageTypeName)
        {
            //Get Document Info data.
            var Info = GetField.GetInfo(docId);
            //Get Document Detail data.
            var Details = GetField.GetDetails(docId,pageType.ToString());
            string sPagePath = Common.getConstTxt("sUrl") + "/FileUploads/" + Common.GenZero(Info.CreateBy, 8) + "/" + Info.FileUID + "/";
            var data = docInfoRepo.Get(docId);
            ViewBag.docId = data.DocId;
            ViewBag.PageFileName = data.FileName;
            ViewBag.PageUrl = sPagePath + "SL" + Common.GenZero(Details.PageType, 5) + ".tif";
            ViewBag.TempPath = sPagePath;
            ViewBag.PageType = pageType;
            ViewBag.PageTypeName = pageTypeName;
            return View("ScanResult", objModel);
        }
       public List<DataResult> Grid_Row(int docId,string PageType)
        {
            DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
            var info = docInfoRepo.Get(docId);
            var details = GetField.GetDetails(docId, PageType);
            //=============================================================================
            String sSaveFolder = Server.MapPath("..\\FileUploads\\" + Common.GenZero(info.CreateBy, 8) + "\\" + info.FileUID + "\\" + "RST" + Common.GenZero(details.PageType, 5) + ".csv");
            var lines = System.IO.File.ReadAllLines(@sSaveFolder);
            List<DataResult> objTempmodel = new List<DataResult>();
            foreach (var line in lines)
            {
                Regex csv_file = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                string[] words = csv_file.Split(line);
                objTempmodel.Add(new DataResult
                {
                    Footnote_No = words[1].Trim(new Char[] {'"'}),
                    Divisions = DivisionStatus(),
                    Digitized_Account_Title = words[0].Trim(new Char[] {'"'}),
                    Recovered = "",//RecoveredStatus(),
                    Standard_Title = "",
                    Amount = words[2].Trim(new Char[] {'"'}),
                    Modified = "",
                    CLCTCD = ""
                });
            }
            return objTempmodel;
        }     
       
        public JsonResult Commit_file(int docId,string csv_file,string filenames)
        {
            var Info = docInfoRepo.Get(docId);
            //Get Document Detail data.
            String Folder_Path = Server.MapPath("..\\FileUploads\\" + Common.GenZero(Info.CreateBy, 8) + "\\" + Info.FileUID + "\\" + filenames);
            FileInfo files = new FileInfo(Folder_Path);
            using (var sw = new StreamWriter(files.ToString(), false, Encoding.UTF8))
            {
                sw.WriteLine(csv_file);
            }
            //System.IO.File.WriteAllText(files.ToString(), csv_file);
            return Json("Success full");
        }
        public JsonResult ExportAllResult(int docId)
        {
            var Info = docInfoRepo.Get(docId);
            //Get Document Detail data.
            String FolderPath = Server.MapPath("..\\FileUploads\\" + Common.GenZero(Info.CreateBy, 8) + "\\" + Info.FileUID + "\\");
            List<string> files = new List<string>();
            files.Add(@"Tmp001");
            files.Add(@"Tmp002");
            //Call Procedure create file
            Create_Temp_Files(files, FolderPath);
            CombineFiles(files, FolderPath);
            return Json("Success fully");
        }
        public void Create_Temp_Files(List<string> files,string FolderPath)
        {
            Workbook newbook = new Workbook();
            newbook.Version = ExcelVersion.Version2013;
            newbook.Worksheets.Clear();
            Workbook workbook = new Workbook();
            for (int i = 0; i < files.Count; i++)
            {
                workbook.LoadFromFile(FolderPath + files[i].ToString() + ".csv", ",", 1, 1);
                Worksheet sheet = workbook.Worksheets[0];
                int last = sheet.LastRow;
                sheet.Name = files[i].ToString();
                switch(sheet.Name)
                {
                    case "Tmp001": { sheet.Name = "Income Statement";break; }
                    case "Tmp002": { sheet.Name = "Balance Sheet";break; }
                }

                sheet.Range["C2:E" + last].Style.Color = Color.Gold;
                sheet.Range["C2:E" + last].Style.Font.FontName = "Segoe UI";
                sheet.Range["C2:E" + last].Style.Font.Size = 11.5;
                sheet.Range["C1" + sheet.LastColumn].Style.Font.IsBold = true;
                sheet.SetColumnWidth(2, 15);
                sheet.SetColumnWidth(3, 30);
                sheet.SetColumnWidth(4, 30);
                sheet.SetColumnWidth(5, 30);
                sheet.SetColumnWidth(6, 20);
                workbook.SaveToFile(FolderPath.ToString() + files[i].ToString() + ".xlsx", ExcelVersion.Version2010);
            }
        }
        public void CombineFiles(List<string> files,string FolderPath)
        {
            Workbook newbook = new Workbook();
            newbook.Version = ExcelVersion.Version2013;
            newbook.Worksheets.Clear();
            Workbook tempbook = new Workbook();
            for (int i = 0; i < files.Count; i++)
            {
                tempbook.LoadFromFile(FolderPath + files[i] + ".xlsx");
                foreach (Worksheet sheet in tempbook.Worksheets)
                {
                    newbook.Worksheets.AddCopy(sheet);
                }
            }
            newbook.SaveToFile(FolderPath + "AllReSult.xlsx", ExcelVersion.Version2013);
            System.Diagnostics.Process.Start(FolderPath + "AllReSult.xlsx");
        }
        public SelectList DivisionStatus()
        {
            List<Status> status = new List<Status>();
            status.Add(new Status { ID = 1, StatusName = "Major" });
            status.Add(new Status { ID = 2, StatusName = "Major1" });
            status.Add(new Status { ID = 3, StatusName = "Major2" });
            status.Add(new Status { ID = 4, StatusName = "Major3" });
            SelectList objinfo = new SelectList(status, "ID", "StatusName");
            return objinfo;
        }
        public SelectList RecoveredStatus()
        {
            List<Status> status = new List<Status>();
            status.Add(new Status { ID = 1, StatusName = "direct costs" });
            status.Add(new Status { ID = 2, StatusName = "cash and bank balance" });
            status.Add(new Status { ID = 3, StatusName = "gain on investment" });
            status.Add(new Status { ID = 4, StatusName = "increase in other receivables" });
            status.Add(new Status { ID = 5, StatusName = "state subsidy" });
            status.Add(new Status { ID = 6, StatusName = "indent sales" });
            status.Add(new Status { ID = 7, StatusName = "other direct costs" });
            status.Add(new Status { ID = 8, StatusName = "purchases" });
            status.Add(new Status { ID = 9, StatusName = "investment income" });
            status.Add(new Status { ID = 10, StatusName = "-other plant and equipment" });
            status.Add(new Status { ID = 11, StatusName = "financial asset" });
            SelectList objinfo = new SelectList(status, "ID", "StatusName");
            return objinfo;
        }
        public JsonResult AssignGrid(int docId,string PageType)
        {
            objModel.ScanEdit = Grid_Row(docId,PageType);
            var resultobject = objModel.ScanEdit.ToList();
            return Json(resultobject,JsonRequestBehavior.AllowGet);
        }

    }
}