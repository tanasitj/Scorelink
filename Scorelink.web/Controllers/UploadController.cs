using Scorelink.BO.Repositories;
using Scorelink.BO.Helper;
using Scorelink.MO;
using Scorelink.MO.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Leadtools.Pdf;
using System.Net;
using System.ServiceModel.Channels;
using Spire.Xls;
using System.Drawing;

namespace Scorelink.web.Controllers
{
    public class UploadController : Controller
    {
        //int iUserId = 1;

        DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        // GET: Upload
        public ActionResult Index()
        {
            try
            {
                ViewBag.Id = "";
                //ViewBag.UserId = "1";

                if (Session["UserId"] == null)
                {
                    Response.Redirect("/Home/Index");
                }
                else
                {
                    ViewBag.UserId = Session["UserId"].ToString();
                    int iUserId = 0;
                    Int32.TryParse(Session["UserId"].ToString(), out iUserId);

                    //iUserId = 0;

                    //Get User Info.
                    UserRepo userRepo = new UserRepo();
                    var userDB = userRepo.Get(iUserId);
                    ViewBag.Name = userDB.Name;
                    ViewBag.Surname = userDB.Surname;

                    //Check and Update online date time.
                    OnlineUserRepo onlineRepo = new OnlineUserRepo();
                    var online = onlineRepo.Get(iUserId);
                    onlineRepo.Update(online);
                }
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
            }
            return View("Upload");
        }

        public ActionResult UploadFiles(string userId, string language)
        {
            String FileName = "";
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //string filesize = Request.Cookies.Get("filesize").Value;
                    //string[] allKeys = Request.Files.AllKeys[0].Split('|');
                    var uploadNo = Common.GenZero(userId, 8);
                    string sLanguage = language;
                    //string folder = Consts.SLUserFlie + "\\FileUploads\\" + uploadNo + "\\" + sUID;

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        string sUID = Guid.NewGuid().ToString();
                        string folder = Common.getConstTxt("SLUserFlie") + uploadNo + "\\" + sUID;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = sUID;
                        }

                        // Get the complete folder path and store the file inside it. 
                        Common.CreateDocFolder(folder);

                        //FileInfo fi = new FileInfo(folder + "\\" + allKeys[i]);
                        FileInfo fi = new FileInfo(folder + "\\" + file.FileName);
                        fname = sUID + fi.Extension;
                        fname = Path.Combine(folder + "\\", fname);
                        file.SaveAs(fname);

                        String sCreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        String sFileUrl = Common.getConstTxt("sUrl") + "/FileUploads/"+uploadNo + "/" + sUID + "/"+sUID+fi.Extension;

                        DocumentInfoModel doc = new DocumentInfoModel();
                        doc.FileUID = sUID;
                        //doc.FileName = allKeys[i];
                        doc.FileName = file.FileName;
                        doc.FilePath = fname;
                        doc.FileUrl = sFileUrl;
                        doc.Language = sLanguage;
                        doc.CreateBy = userId;
                        doc.CreateDate = sCreateDate;

                        DocumentInfoRepo documentInfoRepo = new DocumentInfoRepo();
                        documentInfoRepo.Add(doc);

                        FileName = file.FileName;
                    }
                    // Returns message that successfully uploaded  
                    return Json(FileName, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Logger Err = new Logger();
                    Err.ErrorLog(ex.ToString());
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public JsonResult GetDocumentList(string filterId)
        {
            var doc = docInfoRepo.GetDocStatusList(filterId).ToList();
            return Json(doc, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDocumentInfo(int id)
        {
            var result = "";

            try
            {
                //Get Document Info.
                var doc = docInfoRepo.Get(id);
                string sPath = Server.MapPath("..\\FileUploads\\" + Common.GenZero(doc.CreateBy, 8) + "\\" + doc.FileUID + "\\");

                //Delete All Data by DocId.
                result = docInfoRepo.Delete(id.ToString());

                if (Directory.Exists(sPath))
                {
                    //Delete Folder.
                    DeleteDirectory(sPath);
                }
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                return Json(ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAllDocumentInfo(int[] id)
        {
            var result = "";

            try
            {
                for (int i = 0; i < id.Length; i++)
                {
                    //Get Document Info.
                    var doc = docInfoRepo.Get(id[i]);
                    string sPath = Server.MapPath("..\\FileUploads\\" + Common.GenZero(doc.CreateBy, 8) + "\\" + doc.FileUID + "\\");

                    //Delete All Data by DocId.
                    result = docInfoRepo.Delete(id[i].ToString());

                    if (Directory.Exists(sPath))
                    {
                        //Delete Folder.
                        DeleteDirectory(sPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                return Json(ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditDocumentInfo(DocumentInfoModel item)
        {
            var result = "";

            try
            {
                result = docInfoRepo.Update(item);
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                return Json(ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ScanDocumentInfo(string path, string folder)
        {
            //try
            //{
            //    int iLoop = 1;
            //    for (int i = 0; i < iLoop; i++)
            //    {
            //        Thread t = new Thread(delegate ()
            //        {
            //            PDFConverter(path, folder);
            //            Console.WriteLine("Thread >> " + i);
            //        });
            //        t.Start();
            //    }
            //}
            //catch (Exception ex)
            //{
                
            //}
            //Console.WriteLine("END >> ");

            PDFConverter(path, folder);
            return View("Upload");
        }

        private bool PDFConverter(string path,string folder)
        {
            int iUserId = 1;
            string sTest = Guid.NewGuid().ToString();
            try
            {
                Common.GetLicenseLeadTool();
                using (var documentConverter = new Leadtools.Document.Converter.DocumentConverter())
                {
                    // RasterCodecsオブジェクトを初期化します。
                    var codecs = new Leadtools.Codecs.RasterCodecs();
                    // 水平および垂直方向の表示解像度（DPI）を設定します。
                    //codecs.Options.RasterizeDocument.Load.XResolution = 300;
                    //codecs.Options.RasterizeDocument.Load.YResolution = 300;
                    codecs.Options.RasterizeDocument.Load.XResolution = 150;
                    codecs.Options.RasterizeDocument.Load.YResolution = 150;
                    // ファイルに含まれているページ数を調べます。
                    Leadtools.Codecs.CodecsImageInfo info = codecs.GetInformation(path, true);

                    // ページごとにロードして保存します。
                    int pageNumber;
                    var loopTo = info.TotalPages;
                    for (pageNumber = 1; pageNumber <= loopTo; pageNumber++)
                    {
                        // 画像をロードします。
                        Leadtools.RasterImage image = codecs.Load(path, 0, Leadtools.Codecs.CodecsLoadByteOrder.BgrOrGray, pageNumber, pageNumber);

                        FileInfo file = new FileInfo(path);

                        //string sTempFolder = Consts.SLUserFlie + "\\FileUploads\\"+ sTest + "\\"+folder+"\\";
                        //string sTempFolder = Consts.SLUserFlie + "\\FileUploads\\" + Common.GenZero(iUserId.ToString(), 8) + "\\" + folder + "\\Temp\\" + sTest + "\\";
                        string sTempFolder = Consts.SLUserFlie + "\\FileUploads\\" + Common.GenZero(iUserId.ToString(), 8) + "\\" + folder + "\\";
                        Common.CreateDocFolder(sTempFolder);

                        // ロードしたページをTifで保存します。
                        //string pageFileName = sTempFolder + Guid.NewGuid().ToString() + ".tif";
                        //string pageFileName = sTempFolder + Common.GenZero(pageNumber.ToString(), 4) + ".jpg";
                        //string pageFileName = sTempFolder + Common.GenZero(pageNumber.ToString(), 4) + ".tif";
                        string pageFileName = sTempFolder + sTest + ".tif";
                        //codecs.Save(image, pageFileName, Leadtools.RasterImageFormat.Jpeg, 24);
                        codecs.Save(image, pageFileName, Leadtools.RasterImageFormat.Tif, 24, 1, -1, 1, Leadtools.Codecs.CodecsSavePageMode.Append);
                        //image.Dispose();

                        if(pageNumber == loopTo)
                        {
                            image.Dispose();
                        }
                    }

                    codecs.Dispose();
                    documentConverter.Dispose();
                    GC.Collect(0);
                }
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                return false;
            }

            return true;
        }

        public static void DeleteDirectory(string target_dir)
        {
            try
            {
                string[] files = Directory.GetFiles(target_dir);
                string[] dirs = Directory.GetDirectories(target_dir);

                foreach (string file in files)
                {
                    System.IO.File.SetAttributes(file, FileAttributes.Normal);
                    System.IO.File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(target_dir, false);
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
            }
        }

        public JsonResult ExportAllResult(int docId)
        {
            var Info = docInfoRepo.Get(docId);
            //Get Document Detail data.
            String FolderPath = Server.MapPath("..\\FileUploads\\" + Common.GenZero(Info.CreateBy, 8) + "\\" + Info.FileUID + "\\");
            //String UrlPath = Common.getConstTxt("sUrl") + "/FileUploads/" + Common.GenZero(Info.CreateBy, 8) + "/" + Info.FileUID + "/";
            String UrlPath = "~/FileUploads/" + Common.GenZero(Info.CreateBy, 8) + "/" + Info.FileUID + "/";
            List<string> files = new List<string>();
            string Statement = FolderPath + "Tmp001.csv";
            string BalanceSheet = FolderPath + "Tmp002.csv";
            string Cashflow = FolderPath + "Tmp003.csv";
            //var fileName = "AllReSult" + ".xlsx";
            string sDateTime = DateTime.Now.ToString("yyyyMMddHHmmssFFFF");
            var UserFileName = "ARS" + Info.FileUID + ".xlsx";
            var TmpFileName = "EX" + sDateTime + ".xlsx";

            //Check File for insert parameter
            try
            {
                if (System.IO.File.Exists(Statement))
                {
                    files.Add(@"Tmp001");
                }
                if (System.IO.File.Exists(BalanceSheet))
                {
                    files.Add(@"Tmp002");
                }
                if (System.IO.File.Exists(Cashflow))
                {
                    files.Add(@"Tmp003");
                }
                //Call Procedure create file
                if (files.Count > 0)
                {
                    Create_Temp_Files(files, FolderPath);
                    //CombineFiles(files, FolderPath, UrlPath);

                    //Save the file to server temp folder
                    //string fullPath = Path.Combine(Server.MapPath("~/temp"), fileName);
                    string TmpPath = Path.Combine(Server.MapPath(UrlPath), TmpFileName);

                    Workbook newbook = new Workbook();
                    newbook.Version = ExcelVersion.Version2013;
                    newbook.Worksheets.Clear();
                    Workbook tempbook = new Workbook();
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            tempbook.LoadFromFile(FolderPath + files[i] + ".xlsx");
                            foreach (Worksheet sheet in tempbook.Worksheets)
                            {
                                newbook.Worksheets.AddCopy(sheet);
                            }
                        }
                        //create file to save on server
                        //newbook.SaveToFile(FolderPath + "AllReSult.xlsx", ExcelVersion.Version2013);
                        newbook.SaveToFile(FolderPath + UserFileName, ExcelVersion.Version2013);
                        //create file to folder temp for client download
                        newbook.SaveToFile(TmpPath, ExcelVersion.Version2013);

                    }

                }
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                return Json(ex.Message);
            }
            //return Json(new { fileName = fileName });
            //return Json("Success");
            return Json(TmpFileName);
        }

        public void Create_Temp_Files(List<string> files, string FolderPath)
        {
            Workbook newbook = new Workbook();
            newbook.Version = ExcelVersion.Version2013;
            newbook.Worksheets.Clear();
            Workbook workbook = new Workbook();
            if (files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    workbook.LoadFromFile(FolderPath + files[i].ToString() + ".csv", ",", 1, 1);
                    Worksheet sheet = workbook.Worksheets[0];
                    int last = sheet.LastRow;
                    sheet.Name = files[i].ToString();
                    switch (sheet.Name)
                    {
                        case "Tmp001": { sheet.Name = "Income Statement"; break; }
                        case "Tmp002": { sheet.Name = "Balance Sheet"; break; }
                        case "Tmp003": { sheet.Name = "Cash flow"; break; }
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
        }

        public ActionResult Download(string file, int docId)
        {
            //Get the temp folder and file path in server
            try
            {
                var Info = docInfoRepo.Get(docId);
                String UrlPath = "~/FileUploads/" + Common.GenZero(Info.CreateBy, 8) + "/" + Info.FileUID + "/";

                string fullPath = Path.Combine(Server.MapPath(UrlPath), file);
                byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
                return File(fileByteArray, "application/vnd.ms-excel", file);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}