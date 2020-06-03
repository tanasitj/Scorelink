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
        int iDocId = 1038;
        //----------------//

        DocumentDetailRepo docDetRepo = new DocumentDetailRepo();
        // GET: SelectPattern
        public ActionResult Index()
        {
            var data = docDetRepo.Get(iDocId);
            ViewBag.Id = data.DocId;
            ViewBag.DocDetId = data.DocDetId;
            ViewBag.DocPageNo = data.DocPageNo;
            ViewBag.PageFileName = data.PageFileName;
            ViewBag.PagePath = data.PagePath;

            return View("SelectPatternMain");
        }

        public JsonResult SavePattern(DocumentDetailModel item, string patternNo)
        {
            var data = docDetRepo.UpdatePatternNo(item.DocId, patternNo);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}