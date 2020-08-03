using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scorelink.BO.Helper;
using Scorelink.MO.DataModel;
using Scorelink.BO.Repositories;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Text.RegularExpressions;

namespace Scorelink.web.Controllers
{
    public class ScanResultController : Controller
    {
        // GET: ScanResult
        DocumentDetailRepo docDetailRepo = new DocumentDetailRepo();
        DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        ResultModel objModel = new ResultModel();
        ScanEditRepo GetField = new ScanEditRepo();
        public ActionResult Index(int docId, int pageType)
        {
            //Get Document Info data.
            var Info = GetField.GetInfo(docId);
            //Get Document Detail data.
            var Details = GetField.GetDetails(docId,pageType.ToString());
            string sPagePath = Consts.sUrl + "/FileUploads/" + Common.GenZero(Info.CreateBy, 8) + "/" + Info.FileUID + "/" + "SL" + Common.GenZero(Details.PageType, 5) + ".tif";
            var data = docInfoRepo.Get(docId);
            ViewBag.docId = data.DocId;
            ViewBag.PageFileName = data.FileName;
            ViewBag.PageUrl = sPagePath;
            ViewBag.PageType = pageType;
            return View("ScanResult", objModel);
        }
        public ActionResult CheckData(int id)
        {
            var data = docDetailRepo.Get(id);
            ViewBag.Id = data.DocId;
            ViewBag.PageFileName = data.PageFileName;
            ViewBag.PagePath = data.PagePath;
            ViewBag.PageUrl = data.PageUrl;

            objModel.ScanEdit = CheckResult();
            return View("ScanResult",objModel);
            //return View();
        }

       public List<DataResult> MergeRow(int docId,string PageType)
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
                    Recovered = RecoveredStatus(),
                    Amount = words[2].Trim(new Char[] {'"'}),
                    Modified = "",
                    CLCTCD = ""
                });
            }
            return objTempmodel;
        }     
        public List<DataResult> CheckResult()
        {
            string path = "D:/GitHub/ScoreLink/Scorelink.web/FileUploads/00000001/Result.xlsx";
            FileInfo fileInfo = new FileInfo(path);
            //-------------------------------------------------------------------------          
            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
            int rows = worksheet.Dimension.End.Row;
            int columns = worksheet.Dimension.Columns;
            List<DataResult> objTempmodel = new List<DataResult>();
            for (int i = 2; i <= rows; i++)
            {
                objTempmodel.Add(new DataResult
                {
                    Footnote_No = (worksheet.Cells[i, 1].Value ?? string.Empty).ToString(),
                    Divisions = DivisionStatus(),
                    Digitized_Account_Title = (worksheet.Cells[i, 3].Value ?? string.Empty).ToString(),
                    Recovered = RecoveredStatus(),
                    Amount = (worksheet.Cells[i, 5].Value ?? string.Empty).ToString(),
                    Modified = (worksheet.Cells[i, 6].Value ?? string.Empty).ToString(),
                    CLCTCD = (worksheet.Cells[i, 7].Value ?? string.Empty).ToString()
                });
            }         
            return objTempmodel;
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
       public JsonResult AssignGridCheck()
        {
            objModel.ScanEdit = CheckResult();
            var resultobject = objModel.ScanEdit.ToList();
            return Json(resultobject);
        }
        public JsonResult AssignGridMerge(int docId,string PageType)
        {
            objModel.ScanEdit = MergeRow(docId,PageType);
            var resultobject = objModel.ScanEdit.ToList();
            return Json(resultobject,JsonRequestBehavior.AllowGet);
        }

    }
}