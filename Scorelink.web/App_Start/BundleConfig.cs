using System.Web.Optimization;
using WebHelpers.Mvc5;

namespace Scorelink.web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Bundles/css")
                //.Include("~/")
                .Include("~/plugins/fontawesome-free/css/all.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/font-awesome.min.css", new CssRewriteUrlTransformAbsolute())
                //.Include("~/Content/css/bootstrap.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/plugins/datatables-bs4/css/dataTables.bootstrap4.css")
                .Include("~/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css")
                .Include("~/plugins/toastr/toastr.min.css")
                //.Include("~/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css")
                //.Include("~/plugins/icheck-bootstrap/icheck-bootstrap.min.css")
                //.Include("~/plugins/jqvmap/jqvmap.min.css")
                .Include("~/dist/css/adminlte.min.css", new CssRewriteUrlTransformAbsolute())
                .Include("~/Content/css/pnotify.custom.min.css")
                //.Include("~/Content/css/skins/skin-blue.css")
                //.Include("~/plugins/overlayScrollbars/css/OverlayScrollbars.min.css")
                //.Include("~/plugins/daterangepicker/daterangepicker.css")
                //.Include("~/plugins/summernote/summernote-bs4.css")
                );

            bundles.Add(new ScriptBundle("~/Bundles/js")
                //.Include("~/")
                //<!-- jQuery -->
                //.Include("~/Content/js/plugins/jquery/jquery-3.3.1.js")
                .Include("~/plugins/jquery/jquery.min.js")
                //<!-- Bootstrap 4 -->
                .Include("~/plugins/bootstrap/js/bootstrap.bundle.min.js")
                .Include("~/plugins/sweetalert2/sweetalert2.min.js")
                .Include("~/plugins/toastr/toastr.min.js")
                //.Include("~/Content/js/plugins/bootstrap/bootstrap.js")
                //.Include("~/Content/js/plugins/fastclick/fastclick.js")
                //.Include("~/Content/js/plugins/slimscroll/jquery.slimscroll.js")
                //.Include("~/Content/js/plugins/bootstrap-select/bootstrap-select.js")
                .Include("~/plugins/datatables/jquery.dataTables.js")
                //.Include("~/Content/js/plugins/moment/moment.js")
                //.Include("~/Content/js/plugins/datepicker/bootstrap-datepicker.js")
                .Include("~/Content/js/plugins/pnotify/pnotify.custom.min.js")
                //.Include("~/Content/js/plugins/icheck/icheck.js")
                //.Include("~/Content/js/plugins/validator/validator.js")
                //.Include("~/Content/js/plugins/inputmask/jquery.inputmask.bundle.js")
                .Include("~/dist/js/adminlte.min.js")
                .Include("~/Content/js/plugins/jquery.blockUI/jquery.blockUI.js")
                //.Include("~/Content/js/init.js")
                .Include("~/Content/js/plugins/knockout/knockout-3.4.2.js")
                );
            //bundles.Add(new ScriptBundle("~/Bundles/Leadtools")
            //    .Include()

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
