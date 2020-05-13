﻿using Scorelink.BO.Repositories;
using Scorelink.MO;
using Scorelink.MO.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scorelink.web.Controllers
{
    public class UploadController : Controller
    {
        DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        // GET: Upload
        public ActionResult Index()
        {
            return View("Upload");
        }

        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    string[] allKeys = Request.Files.AllKeys[0].Split('|');
                    // var id = allKeys[0];
                    //var uploadNo = allKeys[0];
                    var uploadNo = "Tanasit";

                    //string folder = Server.MapPath("..\\FileUploads\\" + id + "\\" + uploadNo);
                    string folder = Server.MapPath("..\\FileUploads\\" + uploadNo);

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
                            fname = Guid.NewGuid().ToString();
                        }

                        // Get the complete folder path and store the file inside it. 
                        CreateDocFolder(folder);

                        FileInfo fi = new FileInfo(folder + "\\" + allKeys[0]);
                        fname = Guid.NewGuid().ToString()+fi.Extension;
                        fname = Path.Combine(folder + "\\", fname);
                        file.SaveAs(fname);

                        String sCreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                        DocumentInfoModel doc = new DocumentInfoModel();
                        doc.FileUID = Guid.NewGuid().ToString();
                        doc.FileName = allKeys[0];
                        doc.FilePath = fname;
                        doc.CreateBy = "Tanasitj";
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
            var users = docInfoRepo.GetList(filterId).ToList();
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDocumentInfo(string id,string filePath)
        {
            var result = "";

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        result = docInfoRepo.Delete(id);
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
            GetLicenseLeadTool();
            PDFConverter(path, folder);
            return View("Upload");
        }

        private void CreateDocFolder(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
        }

        private bool PDFConverter(string path,string folder)
        {
            try
            {
                using (var documentConverter = new Leadtools.Document.Converter.DocumentConverter())
                {
                    // RasterCodecsオブジェクトを初期化します。
                    var codecs = new Leadtools.Codecs.RasterCodecs();
                    // 水平および垂直方向の表示解像度（DPI）を設定します。
                    codecs.Options.RasterizeDocument.Load.XResolution = 300;
                    codecs.Options.RasterizeDocument.Load.YResolution = 300;
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

                        // ロードしたページをTifで保存します。
                        string pageFileName = Path.Combine(folder, file.FullName + pageNumber.ToString() + file.Extension);
                        codecs.Save(image, pageFileName, Leadtools.RasterImageFormat.Tif, 24);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private void GetLicenseLeadTool()
        {
            string sPath = Server.MapPath("..\\LicenseLeadTools\\");
            try
            {
                // ライセンスファイル（xxx.lic）が配置されているパス
                string licenseFilePath = sPath + @"tis-ImgPro-190-20171019.txt";
                // キーファイル（xxx.key）内に記載されている文字列
                string developerkeyPath = sPath + @"Leadtools.lic.key.txt";
                developerkeyPath = Path.Combine(developerkeyPath);
                //string developerkey = System.IO.File.ReadAllText(developerkeyPath);
                string developerkey = "0cwaXp9T2ZaebVHDFtE+k4CRUcBJLjcIvt383qJp6jPtoM/YamPF1yiYkXqsCmFEbJzGcuyaOCTXpLdGpuHLl0wjSKF9nx/u";


                // ' ライセンスファイル（xxx.lic）が配置されているパス
                // Dim licenseFilePath As String = FCCS.PresetValues.EnvironmentPath.App() & "\tis-ImgPro-190-20171019.lic"
                // ' キーファイル（xxx.key）内に記載されている文字列
                // Dim developerkey As String = "0cwaXp9T2ZaebVHDFtE+k4CRUcBJLjcIvt383qJp6jPtoM/YamPF1yiYkXqsCmFEbJzGcuyaOCTXpLdGpuHLl0wjSKF9nx/u"
                Leadtools.RasterSupport.SetLicense(licenseFilePath, developerkey);
            }
            catch (Exception ex)
            {
                //FCM.Common.PutStakTrace(ex);
                //MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}