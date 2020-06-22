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

namespace Scorelink.web.Controllers
{
    public class UploadController : Controller
    {
        //int iUserId = 1;

        DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        // GET: Upload
        public ActionResult Index()
        {
            ViewBag.Id = "";
            ViewBag.UserId = "1";
            return View("Upload");
        }

        public ActionResult UploadFiles(string userId)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    string[] allKeys = Request.Files.AllKeys[0].Split('|');
                    var uploadNo = Common.GenZero(userId, 8);
                    string sUID = Guid.NewGuid().ToString();
                    string folder = Consts.SLUserFlie + "\\FileUploads\\" + uploadNo;

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

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

                        FileInfo fi = new FileInfo(folder + "\\" + allKeys[0]);
                        fname = sUID + fi.Extension;
                        fname = Path.Combine(folder + "\\", fname);
                        file.SaveAs(fname);

                        String sCreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        String sFileUrl = Consts.sUrl + "/FileUploads/"+uploadNo+"/"+sUID+fi.Extension;

                        DocumentInfoModel doc = new DocumentInfoModel();
                        doc.FileUID = sUID;
                        doc.FileName = allKeys[0];
                        doc.FilePath = fname;
                        doc.FileUrl = sFileUrl;
                        doc.CreateBy = userId;
                        doc.CreateDate = sCreateDate;

                        DocumentInfoRepo documentInfoRepo = new DocumentInfoRepo();
                        documentInfoRepo.Add(doc);

                    }
                    // Returns message that successfully uploaded  
                    return Json("OK");
                }
                catch (Exception ex)
                {
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
            var doc = docInfoRepo.GetList(filterId).ToList();
            return Json(doc, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDocumentInfo(int id,string filePath)
        {
            var result = "";

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        //Get Document Info.
                        var doc = docInfoRepo.Get(id);
                        string sPath = Server.MapPath("..\\FileUploads\\" + Common.GenZero(doc.CreateBy,8) + "\\" + doc.FileUID + "\\");
                        //Delete Folder.
                        Common.DeleteFolder(sPath);
                        //Delete File.
                        Common.DeleteFile(filePath);
                        //Delete All Data by DocId.
                        result = docInfoRepo.Delete(id.ToString());
                    }
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
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
                return false;
            }

            return true;
        }

        
    }
}