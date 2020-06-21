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
            SelectPatternRepo docDet = new SelectPatternRepo();
            var data = docDet.Get(docId, pageType);
            ViewBag.Id = data.DocId.ToString();
            ViewBag.DocDetId = data.DocDetId;
            ViewBag.DocPageNo = data.DocPageNo;
            ViewBag.PageType = pageType;
            ViewBag.PDFPath = data.PageUrl;

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
                String sTempFolder = Consts.SLUserFlie + "\\FileUploads\\" + Common.GenZero(docInfo.CreateBy, 8) + "\\" + sFolder + "\\" + Common.GenZero(item.PageType, 5) + "\\";
                
                //Get DocumentDetail data.
                var docDet = docDetailRepo.GetList(item.DocId, item.PageType);
                //Get Leadtools License.
                Common.GetLicenseLeadTool();

                using (var documentConverter = new Leadtools.Document.Converter.DocumentConverter())
                {
                    var codecs = new Leadtools.Codecs.RasterCodecs();
                    codecs.Options.RasterizeDocument.Load.XResolution = 150;
                    codecs.Options.RasterizeDocument.Load.YResolution = 150;

                    Leadtools.Codecs.CodecsImageInfo info = codecs.GetInformation(sPath, true);

                    int iNum = 1;
                    int iRow = docDet.Count();
                    //Check and Create Folder.
                    Common.CreateDocFolder(sTempFolder);
                    //Delete all file in Folder
                    DirectoryInfo dir = new DirectoryInfo(sTempFolder);
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        file.Delete();
                    }

                    foreach (var doc in docDet)
                    {
                        //Get Page Number convert to integer.
                        int iPage = Convert.ToInt32(doc.DocPageNo);
                        //Get PageType for File Name.
                        String sSavePath = sTempFolder + "\\" + Common.GenZero(doc.DocPageNo, 4) + ".jpg";
                        //Check for Delete File for Initail.
                        if (System.IO.File.Exists(sSavePath))
                        {
                            System.IO.File.Delete(sSavePath);
                        }

                        Leadtools.RasterImage image = codecs.Load(sPath, 0, Leadtools.Codecs.CodecsLoadByteOrder.BgrOrGray, iPage, iPage);
                        //Generate File.
                        codecs.Save(image, sSavePath, Leadtools.RasterImageFormat.Jpeg, 24, 1, -1, 1, Leadtools.Codecs.CodecsSavePageMode.Append);
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