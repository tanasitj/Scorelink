using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using Scorelink.BO.Repositories;
using System.Linq;
using Scorelink.BO.Helper;
using Scorelink.MO.DataModel;
using Leadtools.Document;

namespace Scorelink.web.Controllers
{
    public class SelectAreaController : Controller
    {
        //Fix code for Test.
        int iUserId = 1;
        //----------------//

        SelectAreaRepo doc = new SelectAreaRepo();

        // GET: SelectArea
        public ActionResult Index(DocumentDetailModel item)
        {
            ViewBag.Id = item.DocId;
            ViewBag.PatternNo = item.PatternNo;
            //Get Document Info data.
            var docInfo = doc.GetDocInfo(item.DocId);
            //Get Document Detail data.
            var docDet = doc.GetDocDet(item.DocId, item.PageType);
            //Set Path for Image.
            string sPagePath = Consts.sUrl + "/FileUploads/" + Common.GenZero(docInfo.CreateBy, 8) + "/" + docInfo.FileUID + "/" + Common.GenZero(docDet.PageType, 5) + "/" + Common.GenZero(docDet.DocPageNo, 4) + ".jpg";

            ViewBag.DocDetId = docDet.DocDetId;
            ViewBag.DocPageNo = docDet.DocPageNo;
            ViewBag.PageType = docDet.PageType;
            ViewBag.PageFileName = docDet.PageFileName;
            ViewBag.PagePath = sPagePath;

            return View("SelectAreaMain");
        }

        public JsonResult SaveArea(List<String> values, int docId, int docDetId)
        {
            //Get File UID for Create Folder to Save Area.
            DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
            var docInfo = docInfoRepo.Get(docId);

            //Get Document Detail data.
            var docDet = doc.GetDocDet(docDetId);
            //Set Path
            String sFrom = Consts.SLUserFlie + "\\FileUploads\\" + Common.GenZero(docInfo.CreateBy, 8) + "\\" + docInfo.FileUID + "\\" + Common.GenZero(docDet.PageType,5) + "\\" + Common.GenZero(docDet.DocPageNo, 4) + ".jpg";
            String sSaveFolder = Server.MapPath("..\\FileUploads\\" + Common.GenZero(docInfo.CreateBy, 8) + "\\" + docInfo.FileUID + "\\" + Common.GenZero(docDet.PageType, 5) + "\\" + Common.GenZero(docDet.DocPageNo, 4) + "\\");
            String sUrlPath = Consts.sUrl + "/FileUploads/" + Common.GenZero(docInfo.CreateBy, 8) + "/" + docInfo.FileUID + "/" + Common.GenZero(docDet.PageType, 5) + "/" + Common.GenZero(docDet.DocPageNo, 4) + "/";
            //Initail Document Folder.
            Common.InitailDocFolder(sSaveFolder);

            for (int i = 0; i < values.Count; i++)
            {
                int iRunNo = i + 1;
                String sSave = sSaveFolder + Common.GenZero(iRunNo.ToString(), 4) + ".tif";
                String sSaveUrl = sUrlPath + Common.GenZero(iRunNo.ToString(), 4) + ".tif";
                String sCrop = values[i];
                String[] aArea = sCrop.Split('|');
                int x, y, w, h;

                try 
                {
                    
                    //Check Convert String Before Crop Image.
                    if (Int32.TryParse(aArea[1], out x) && Int32.TryParse(aArea[2], out y) && Int32.TryParse(aArea[3], out w) && Int32.TryParse(aArea[4], out h))
                    {
                        cropImage(Image.FromFile(sFrom), new Rectangle(x, y, w, h)).Save(sSave, ImageFormat.Tiff);
                        
                        //Insert or Update table DocumentArea
                        DocumentAreaModel docArea = new DocumentAreaModel();
                        docArea.AreaNo = iRunNo;
                        docArea.DocId = docId;
                        docArea.DocDetId = docDetId;
                        docArea.DocPageNo = docDet.DocPageNo;
                        docArea.PageType = docDet.PageType;
                        docArea.AreaX = x.ToString();
                        docArea.AreaY = y.ToString();
                        docArea.AreaH = h.ToString();
                        docArea.AreaW = w.ToString();
                        docArea.AreaPath = sSave;
                        docArea.AreaUrl = sSaveUrl;
                        docArea.CreateBy = iUserId.ToString();
                        docArea.CreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        docArea.UpdateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        //Check Existing Data before Update or Insert data.
                        DocumentAreaRepo docAreaRepo = new DocumentAreaRepo();
                        if (docAreaRepo.CheckDocumentArea(iRunNo, docDetId, docDet.DocPageNo))
                        {
                            docAreaRepo.Update(docArea);
                        }
                        else
                        {
                            docAreaRepo.Add(docArea);
                        }

                        //Update ScanStatus table DocumentDetail
                        DocumentDetailModel objDocDet = new DocumentDetailModel();
                        objDocDet.DocDetId = docDetId;
                        objDocDet.ScanStatus = "Y";
                        objDocDet.UpdateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        DocumentDetailRepo docDetRepo = new DocumentDetailRepo();
                        docDetRepo.UpdateScanStatus(objDocDet);
                    }

                    GC.Collect(0);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }

            //Return Next Page Data
            var data = doc.GetDocDet(docId,docDet.PageType);
            if (data == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                data.PagePath = Consts.sUrl + "/FileUploads/" + Common.GenZero(docInfo.CreateBy, 8) + "/" + docInfo.FileUID + "/" + Common.GenZero(data.PageType, 5) + "/" + Common.GenZero(data.DocPageNo, 4) + ".jpg";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            //bmpCrop.Dispose();
            bmpImage.Dispose();
            return (Image)(bmpCrop);
        }
    }
}