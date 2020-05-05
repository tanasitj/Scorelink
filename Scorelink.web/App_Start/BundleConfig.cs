using System.Web.Optimization;
using WebHelpers.Mvc5;

namespace Scorelink.web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Bundles/css")
                .Include("~/plugins/datatables-bs4/css/dataTables.bootstrap4.css")
                .Include("~/plugins/fontawesome-free/css/all.min.css")
                .Include("~/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css")
                .Include("~/plugins/toastr/toastr.min.css")
                .Include("~/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css")
                .Include("~/plugins/icheck-bootstrap/icheck-bootstrap.min.css")
                .Include("~/plugins/jqvmap/jqvmap.min.css")
                .Include("~/dist/css/adminlte.min.css")
                .Include("~/plugins/overlayScrollbars/css/OverlayScrollbars.min.css")
                .Include("~/plugins/daterangepicker/daterangepicker.css")
                .Include("~/plugins/summernote/summernote-bs4.css")
                );

            bundles.Add(new ScriptBundle("~/Bundles/js")
                .Include("~/Content/js/plugins/jquery/jquery-3.3.1.js")
                .Include("~/Content/js/plugins/bootstrap/bootstrap.js")
                .Include("~/Content/js/plugins/fastclick/fastclick.js")
                .Include("~/Content/js/plugins/slimscroll/jquery.slimscroll.js")
                .Include("~/Content/js/plugins/bootstrap-select/bootstrap-select.js")
                .Include("~/plugins/datatables/jquery.dataTables.js")
                .Include("~/plugins/datatables-bs4/js/dataTables.bootstrap4.js")
                .Include("~/dist/js/adminlte.min.js")
                .Include("~/plugins/sweetalert2/sweetalert2.min.js")
                .Include("~/plugins/toastr/toastr.min.js")
                .Include("~/Content/js/plugins/datepicker/bootstrap-datepicker.js")
                .Include("~/Content/js/plugins/jquery.blockUI/jquery.blockUI.js")
                .Include("~/Content/js/plugins/knockout/knockout-3.4.2.js")
                );

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
