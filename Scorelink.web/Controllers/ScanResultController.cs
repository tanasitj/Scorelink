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

namespace Scorelink.web.Controllers
{
    public class ScanResultController : Controller
    {
        // GET: ScanResult
        DocumentDetailRepo docDetailRepo = new DocumentDetailRepo();
        DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        ResultModel objModel = new ResultModel();
        public ActionResult Index(int id = 21,string get_values = "")
        {
            var data = docDetailRepo.Get(id);
            ViewBag.Id = data.DocId;
            ViewBag.PageFileName = data.PageFileName;
            ViewBag.PagePath = data.PagePath;
            ViewBag.PageUrl = data.PageUrl;
            ViewBag.xcheck = get_values;
            // ViewBag.CreateBy = data.CreateBy;

            //string textboxValue = Request.Form["txtOne"];
            //get_values = "check";
            if (get_values == "check")
            {
                objModel.ScanEdit = CheckResult();
                ViewData["Message"] = "check";
            }
            else
            {
                objModel.ScanEdit = MergeRow();
            }

            return View("ScanResult", objModel);
            //return PartialView("~Views/ScanResult/ScanResult.cshtml",empobj);
            //return View();
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
        public List<DataResult> MergeRow()
        {
            //string path = "C:/sample/OCR/Aftercommit.xlsx";
            //string path = "D:/GitHub/ScoreLink/Scorelink.web/FileUploads/00000001/Fs0000_TmpMultiTFF.xlsx";
            //FileInfo file = new FileInfo(path);
            //=============================================================================
            var lines = System.IO.File.ReadAllLines(@"D:\\GitHub\\ScoreLink\\Scorelink.web\\FileUploads\\00000001\\OCR00001.csv");
            List<DataResult> objTempmodel = new List<DataResult>();
            foreach (var line in lines)
            {
                string[] words = line.Split(',');
                // List<DataResult> Developer = new List<DataResult>(line.Length);             

                objTempmodel.Add(new DataResult
                {
                    Footnote_No = words[0],
                    Divisions = DivisionStatus(),
                    Digitized_Account_Title = words[1],
                    Recovered = RecoveredStatus(),
                    Amount = words[2],
                    Modified = "",
                    CLCTCD = ""
                });
            }
            return objTempmodel;
        }     
        public List<DataResult> CheckResult()
        {
            // D:\\GitHub\\Scorelink\\Scorelink.web
            //string folder = Consts.SLUserFlie + "\\FileUploads\\00000001\\Result.xlsx" + uploadNo;
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
        public JsonResult AssignGridMerge()
        {
            objModel.ScanEdit = MergeRow();
            var resultobject = objModel.ScanEdit.ToList();
            return Json(resultobject,JsonRequestBehavior.AllowGet);
        }

    }
}