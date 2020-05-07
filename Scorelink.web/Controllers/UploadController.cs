using Scorelink.BO.Repositories;
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
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it. 
                        CreateDocFolder(folder);

                        fname = Path.Combine(folder + "/", fname);
                        file.SaveAs(fname);

                        String sCreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                        DocumentInfoModel doc = new DocumentInfoModel();
                        doc.FileUID = Guid.NewGuid().ToString();
                        doc.FileName = allKeys[0];
                        doc.FilePath = folder +"\\"+ allKeys[0];
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

        public JsonResult DeleteDocumentInfo(string id)
        {
            var result = docInfoRepo.Delete(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void CreateDocFolder(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
        }

    }
}