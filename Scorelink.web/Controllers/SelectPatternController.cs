using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scorelink.BO.Helper;
using Scorelink.MO.DataModel;
using Scorelink.BO.Repositories;

namespace Scorelink.web.Controllers
{

    public class SelectPatternController : Controller
    {
        //Fix code for Test.
        int iUserId = 1;
        int iDocId = 1047;
        //----------------//

        
        // GET: SelectPattern
        public ActionResult Index(int docId, string pageType)
        {
            SelectPatternRepo selPatRepo = new SelectPatternRepo();
            var data = selPatRepo.Get(docId, pageType);
            ViewBag.Id = data.DocId.ToString();
            ViewBag.DocDetId = data.DocDetId;
            ViewBag.DocPageNo = data.DocPageNo;
            ViewBag.PDFPath = data.PageUrl;

            return View("SelectPatternMain");
        }

        public JsonResult SavePattern(DocumentDetailModel item, string patternNo)
        {
            DocumentDetailRepo docDetRepo = new DocumentDetailRepo();
            var data = docDetRepo.UpdatePatternNo(item.DocId, patternNo);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}