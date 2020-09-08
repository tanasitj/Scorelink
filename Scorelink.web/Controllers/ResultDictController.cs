using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Scorelink.BO.Helper;
using Scorelink.MO.DataModel;
using Scorelink.BO.Repositories;
using System.IO;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;



namespace Scorelink.web.Controllers
{
    public class ResultDictController : Controller
    {
        ResultModel objModel = new ResultModel();
        public List<DataDict> CheckDict(int docId,string account_title)
        {
            DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
            var info = docInfoRepo.Get(docId);
            String sSaveFolder = Server.MapPath("..\\FileUploads\\" + Common.GenZero(info.CreateBy, 8) + "\\" + info.FileUID + "\\" + "ConvertStandard.xlsx");
           // string path = "D:/GitHub/ScoreLink/Scorelink.web/FileUploads/00000001/Result.xlsx";
            FileInfo fileInfo = new FileInfo(sSaveFolder.ToString());
            //-------------------------------------------------------------------------          
            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
            int rows = worksheet.Dimension.End.Row;
            int columns = worksheet.Dimension.Columns;

            List<DataDict> objTempmodel = new List<DataDict>();
            for (int i = 2; i <= rows; i++)
            {
                objTempmodel.Add(new DataDict
                {
                    Recovered = (worksheet.Cells[i, 2].Value ?? string.Empty).ToString()

                });
            }
            return objTempmodel;
        }
        public JsonResult AssignGrid(int docId,string account_title)
        {
            objModel.GetDict = CheckDict(docId,account_title);
            var resultobject = objModel.GetDict.ToList();
            return Json(resultobject,JsonRequestBehavior.AllowGet);
        }

    }
}