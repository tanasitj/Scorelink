using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scorelink.BO.Helper;
using Scorelink.MO.DataModel;
using Scorelink.BO.Repositories;
using System.IO;

namespace Scorelink.web.Controllers
{

    public class SelectPatternController : Controller
    {
        DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        DocumentDetailRepo docDetailRepo = new DocumentDetailRepo();

        // GET: SelectPattern
        public ActionResult Index(int docId, string pageType)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("/Home/Index");
            }
            else
            {
                ViewBag.UserId = Session["UserId"].ToString();
                int iUserId = 0;
                Int32.TryParse(Session["UserId"].ToString(), out iUserId);

                //Get User Info.
                UserRepo userRepo = new UserRepo();
                var userDB = userRepo.Get(iUserId);
                ViewBag.Name = userDB.Name;
                ViewBag.Surname = userDB.Surname;

                //Check and Update online date time.
                OnlineUserRepo onlineRepo = new OnlineUserRepo();
                var online = onlineRepo.Get(iUserId);
                onlineRepo.Update(online);

                //Get Document Detail.
                SelectPatternRepo docDet = new SelectPatternRepo();
                var data = docDet.Get(docId, pageType);
                ViewBag.Id = data.DocId.ToString();
                ViewBag.DocDetId = data.DocDetId;
                ViewBag.DocPageNo = data.DocPageNo;
                ViewBag.PageType = pageType;
                ViewBag.PDFPath = data.PageUrl;


                int iStmId = 0;
                Int32.TryParse(pageType, out iStmId);

                StatementTypeRepo stm = new StatementTypeRepo();
                var stms = stm.Get(iStmId);
                ViewBag.PageTypeName = stms.StatementName;
            }
            return View("SelectPatternMain");
        }

        public JsonResult SavePattern(DocumentDetailModel item, string patternNo)
        {
            var result = "";
            try
            {
                //Get DocumentInfo data.
                var docInfo = docInfoRepo.Get(item.DocId);
                String sFolder = docInfo.FileUID;
                String sPath = docInfo.FilePath;
                //Save Folder for New File.
                String sTempFolder = Common.getConstTxt("SLUserFlie") + Common.GenZero(docInfo.CreateBy, 8) + "\\" + sFolder + "\\";
                
                //Get DocumentDetail data.
                var docDet = docDetailRepo.GetList(item.DocId, item.PageType);
                //Get Leadtools License.
                Common.GetLicenseLeadTool();

                using (var documentConverter = new Leadtools.Document.Converter.DocumentConverter())
                {
                    var codecs = new Leadtools.Codecs.RasterCodecs();
                    codecs.Options.RasterizeDocument.Load.XResolution = 600;
                    codecs.Options.RasterizeDocument.Load.YResolution = 600;

                    Leadtools.Codecs.CodecsImageInfo info = codecs.GetInformation(sPath, true);

                    int iNum = 1;
                    int iRow = docDet.Count();
                    //Check and Create Folder.
                    Common.CreateDocFolder(sTempFolder);
                    //Delete all file in Folder
                    //DirectoryInfo dir = new DirectoryInfo(sTempFolder);
                    //foreach (FileInfo file in dir.GetFiles())
                    //{
                    //    file.Delete();
                    //}

                    foreach (var doc in docDet)
                    {
                        //Get Page Number convert to integer.
                        int iPage = Convert.ToInt32(doc.DocPageNo);
                        //Get PageType for File Name.
                        String sSavePath = sTempFolder + "\\" + "PG" + Common.GenZero(item.PageType, 5) + Common.GenZero(doc.DocPageNo, 4) + ".jpg";
                        //String sSavePathTif = sTempFolder + "\\" + "PG" + Common.GenZero(item.PageType, 5) + Common.GenZero(doc.DocPageNo, 4) + ".tif";
                        //Check for Delete File for Initail.
                        if (iNum == 1)
                        {
                            //Delete Old Area Select files.
                            Common.DeleteAllFile(sTempFolder, "AR" + Common.GenZero(item.PageType, 5) + "*.tif");
                            //Delete Old Page Select files.
                            Common.DeleteAllFile(sTempFolder, "PG" + Common.GenZero(item.PageType, 5) + "*.jpg");
                            //Delete Old OCR Result files.
                            Common.DeleteAllFile(sTempFolder, "OCR" + Common.GenZero(item.PageType, 5) + "*.csv");
                            //Delete Old OCR Merge Result files.
                            Common.DeleteAllFile(sTempFolder, "RST" + Common.GenZero(item.PageType, 5) + ".csv");
                        }

                        Leadtools.RasterImage image = codecs.Load(sPath, 0, Leadtools.Codecs.CodecsLoadByteOrder.BgrOrGray, iPage, iPage);
                        //Generate File.
                        codecs.Save(image, sSavePath, Leadtools.RasterImageFormat.Jpeg, 24, 1, -1, 1, Leadtools.Codecs.CodecsSavePageMode.Append);
                        //codecs.Save(image, sSavePathTif, Leadtools.RasterImageFormat.Tif, 24, 1, -1, 1, Leadtools.Codecs.CodecsSavePageMode.Append);
                        //codecs.Save(image, sSavePath, Leadtools.RasterImageFormat.Jpeg, 24);
                        //Check for Clear Temp.
                        if (iNum == iRow)
                        {
                            image.Dispose();
                        }
                        else
                        {
                            iNum++;
                        }
                    }
                }

                //Update Pattern No.
                DocumentDetailModel docDetMo = new DocumentDetailModel();
                docDetMo.DocId = item.DocId;
                docDetMo.PageType = item.PageType;
                docDetMo.PatternNo = patternNo;
                docDetMo.ScanStatus = "";
                //Update.
                SelectPatternRepo obj = new SelectPatternRepo();
                result = obj.Update(docDetMo);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}