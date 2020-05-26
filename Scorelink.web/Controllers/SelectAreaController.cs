using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using Scorelink.BO.Repositories;
using System.Linq;

namespace Scorelink.web.Controllers
{
    public class SelectAreaController : Controller
    {
        DocumentDetailRepo docDetRepo = new DocumentDetailRepo();

        // GET: SelectArea
        public ActionResult Index()
        {
            ViewBag.Id = "48";
            return View("SelectAreaMain");
        }

        public JsonResult GetDocumentList(string filterId)
        {
            var list = docDetRepo.GetList(filterId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveArea(List<String> values, string fileName)
        {
            String sFrom = "C:\\Tanasit\\MFEC\\Score Link\\SRC\\Scorelink\\Scorelink.web\\Temp\\" + fileName;
            for (int i = 0; i < values.Count; i++)
            {
                int iRunNo = i + 1;
                String sSave = "C:\\Tanasit\\MFEC\\Score Link\\SRC\\Scorelink\\Scorelink.web\\Temp\\Temp\\Test" + fileName + "_" + iRunNo + ".jpg";
                String sCrop = values[i];
                String[] aArea = sCrop.Split('|');
                int x, y, w, h;

                //Check Convert String Before Crop Image.
                if (Int32.TryParse(aArea[1], out x) && Int32.TryParse(aArea[2], out y) && Int32.TryParse(aArea[3], out w) && Int32.TryParse(aArea[4], out h))
                {
                    cropImage(Image.FromFile(sFrom), new Rectangle(x, y, w, h)).Save(sSave, ImageFormat.Jpeg);
                }
            }
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }
    }
}