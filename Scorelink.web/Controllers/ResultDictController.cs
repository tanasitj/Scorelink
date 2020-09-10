﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;


namespace Scorelink.web.Controllers
{
    public class ResultDictController : Controller
    {
      

        //ResultModel objModel = new ResultModel();
        //public List<DataDict> CheckDict(int docId,string account_title)
        //{
        //    DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        //    var info = docInfoRepo.Get(docId);
        //    String sSaveFolder = Server.MapPath("..\\FileUploads\\" + Common.GenZero(info.CreateBy, 8) + "\\" + info.FileUID + "\\" + "ConvertOCRResult.xlsx");
        //   // string path = "D:/GitHub/ScoreLink/Scorelink.web/FileUploads/00000001/Result.xlsx";
        //    FileInfo fileInfo = new FileInfo(sSaveFolder.ToString());
        //    //-------------------------------------------------------------------------          
        //    ExcelPackage package = new ExcelPackage(fileInfo);
        //    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
        //    int rows = worksheet.Dimension.End.Row;
        //    int columns = worksheet.Dimension.Columns;

        //    List<DataDict> objTempmodel = new List<DataDict>();
        //    for (int i = 2; i <= rows; i++)
        //    {
        //        objTempmodel.Add(new DataDict
        //        {
        //            Recovered = (worksheet.Cells[i, 2].Value ?? string.Empty).ToString()

        //        });
        //    }
        //    return objTempmodel;
        //}
        //public JsonResult AssignGrid(int docId,string account_title)
        //{
        //    objModel.GetDict = CheckDict(docId,account_title);
        //    //Debug.Write(account_title+"\n");
        //    var resultobject = objModel.GetDict.ToList();

        //    return Json(resultobject,JsonRequestBehavior.AllowGet);
        //}
        public class Acctitle
        {
            public string pageType { get; set; }
            public string acctitle { get; set; }
        }
        public JsonResult AssignGrid(String pagetype,String[] account_title)
        {
            //List<DataResult> d= JsonConvert.DeserializeObject<List<DataResult>>(jsonData);
              List<Acctitle> input=new List<Acctitle>() ;
            string apiUrl = "https://localhost:44378/api/dic";
            for (int i = 0; i < account_title.Length; i++)
            {

                input.Add(new Acctitle
                {
                    pageType = pagetype,
                    acctitle = account_title[i].ToString() 
                }
               );
            }

            string inputJson = (new JavaScriptSerializer()).Serialize(input);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string json = client.UploadString(apiUrl + "/GetRecoveryDic", inputJson);
            List<List<string>> result = (new JavaScriptSerializer()).Deserialize<List<List<string>>>(json);
            //List<string> result = (new JavaScriptSerializer()).Deserialize<List<string>>(json);
            return Json(result, JsonRequestBehavior.AllowGet); 
        }
    }
}