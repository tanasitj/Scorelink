using Scorelink.BO.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scorelink.web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DeletePageInfo(int page)
        {
            var result = "";

            //3a5a231c-1dbf-4a38-9efa-a983607ba97a
            string sFrom = "C:\\Tanasit\\MFEC\\Score Link\\SRC\\Scorelink\\Scorelink.web\\FileUploads\\00000001\\3a5a231c-1dbf-4a38-9efa-a983607ba97a.pdf";
            string sTo = "C:\\Tanasit\\MFEC\\Score Link\\SRC\\Scorelink\\Scorelink.web\\FileUploads\\00000001\\EX\\3a5a231c-1dbf-4a38-9efa-a983607ba97a.pdf";

            Common.PDFFileDeletePages(sFrom, sTo, page);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}