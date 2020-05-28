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
        int iDocId = 48;
        //----------------//

        DocumentDetailRepo docDetRepo = new DocumentDetailRepo();

        // GET: SelectArea
        public ActionResult Index()
        {
            var data = docDetRepo.Get(iDocId);
            ViewBag.Id = data.DocId;
            ViewBag.DocDetId = data.DocDetId;
            ViewBag.DocPageNo = data.DocPageNo;
            ViewBag.PageFileName = data.PageFileName;
            ViewBag.PagePath = data.PagePath;

            return View("SelectAreaMain");
        }

        public JsonResult GetDocument(string id)
        {
            var iParam = Convert.ToInt32(id);
            var data = docDetRepo.Get(iParam);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocumentList(int filterId)
        {
            var list = docDetRepo.GetList(filterId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveArea(List<String> values, int docId, int docDetId, string pageFileName)
        {
            //Get File UID for Create Folder to Save Area.
            DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
            var docInfo = docInfoRepo.Get(docId);

            var uploadNo = Common.GenZero(iUserId.ToString(), 8);
            String sFrom = Consts.SLUserFlie + "\\FileUploads\\" + uploadNo + "\\" + docInfo.FileUID + "\\" + pageFileName + ".jpg";
            for (int i = 0; i < values.Count; i++)
            {
                int iRunNo = i + 1;
                String sSaveFolder = Server.MapPath("..\\FileUploads\\" + uploadNo + "\\" + docInfo.FileUID + "\\" + pageFileName + "\\");
                String sSave = sSaveFolder + Common.GenZero(iRunNo.ToString(), 4) + ".tif";
                String sCrop = values[i];
                String[] aArea = sCrop.Split('|');
                int x, y, w, h;

                try 
                {
                    Common.CreateDocFolder(sSaveFolder);
                    //Check Convert String Before Crop Image.
                    if (Int32.TryParse(aArea[1], out x) && Int32.TryParse(aArea[2], out y) && Int32.TryParse(aArea[3], out w) && Int32.TryParse(aArea[4], out h))
                    {
                        cropImage(Image.FromFile(sFrom), new Rectangle(x, y, w, h)).Save(sSave, ImageFormat.Tiff);

                        //Insert table DocumentArea
                        DocumentAreaModel docArea = new DocumentAreaModel();
                        docArea.DocId = docId;
                        docArea.DocDetId = docDetId;
                        docArea.AreaX = x.ToString();
                        docArea.AreaY = y.ToString();
                        docArea.AreaH = h.ToString();
                        docArea.AreaW = w.ToString();
                        docArea.AreaPath = sSave;
                        docArea.CreateBy = iUserId.ToString();
                        docArea.CreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        docArea.UpdateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        DocumentAreaRepo docAreaRepo = new DocumentAreaRepo();
                        docAreaRepo.Add(docArea);

                        //Update ScanStatus table DocumentDetail
                        DocumentDetailModel docDet = new DocumentDetailModel();
                        docDet.DocDetId = docDetId;
                        docDet.ScanStatus = "Y";
                        docDet.UpdateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        DocumentDetailRepo docDetRepo = new DocumentDetailRepo();
                        docDetRepo.UpdateScanStatus(docDet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }

            //Return Next Page Data
            var data = docDetRepo.Get(docId);


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }
    }
}