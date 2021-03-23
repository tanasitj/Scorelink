using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using Scorelink.BO.Repositories;
using Scorelink.BO.Helper;
using Scorelink.MO.DataModel;

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
        class Acctitle
        {
            public string pageType { get; set; }
            public string acctitle { get; set; }
        }

       class retData
        {
            public List<string> stdValue { get; set; }
            public List<List<string>> RecoverData { get; set; }
            public List<List<string>> CustomData { get; set; }
            public List<string> RowHighLight { get; set; }
            public retData()
            {
                stdValue = new List<string>();
                RecoverData = new List<List<string>>();
                CustomData = new List<List<string>>();
                RowHighLight = new List<string> ();

            }

      
        }

        public JsonResult AssignGrid(String pagetype,int docId ,String[] account_title)
        {
            DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
            var docInfo = docInfoRepo.Get(docId);
            UserRepo userRepo = new UserRepo();
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
          
            string inputJson =  (new JavaScriptSerializer().Serialize(input));

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            //string dictPath = Common.getConstTxt("Dict") + Common.GenZero(docInfo.CreateBy, 5);
            
            UserModel compID = userRepo.GetUserById(Convert.ToInt16(docInfo.CreateBy));
            string uid = Common.GenZero(compID.Company.ToString(), 5);

            string json = client.UploadString(apiUrl + "/GetRecoveryDic/"+ uid + "/"+ docInfo.Language, inputJson);
            //List<List<retData>> result = (new JavaScriptSerializer()).Deserialize<List<List<retData>>>(json);
          
            retData result = (new JavaScriptSerializer()).Deserialize<retData>(json);
            //List<string> result = (new JavaScriptSerializer()).Deserialize<List<string>>(json);
            return Json(result, JsonRequestBehavior.AllowGet); 
        }
    }
}