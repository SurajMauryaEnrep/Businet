using System.Web;
using System.Web.Optimization;
namespace EnRepMobileWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Content/Scripts/Common/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            "~/Content/Scripts/jquery.validate*"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            "~/Content/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            "~/Content/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/Css/Login").Include(
            "~/Content/Css/bootstrap/dist/css/bootstrap.min.css",
            "~/Content/Css/fonts/font-awesome/css/font-awesome.min.css",
            "~/Content/Css/fonts/iconic/css/material-design-iconic-font.min.css",
            "~/Content/Css/Login/util.css",
            "~/Content/Css/Login/popup.css",
            "~/Content/Css/Login/main.css"));

            bundles.Add(new StyleBundle("~/Content/Common").Include(
            "~/Content/Css/bootstrap/dist/css/bootstrap.min.css",
            "~/Content/Css/fonts/font-awesome-4.7.0/css/font-awesome.min.css",
            "~/Content/Css/nprogress/nprogress.css",
            "~/Content/Css/iCheck/skins/flat/green.css",
            "~/Content/Css/google-code-prettify/bin/prettify.min.css",
            "~/Content/Css/select2/dist/css/select2.min.css",
            "~/Content/Css/switchery/dist/switchery.min.css",
            "~/Content/Css/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css",
            "~/Content/Css/jqvmap/dist/jqvmap.min.css",
            "~/Content/Css/bootstrap-daterangepicker/daterangepicker.css",
            "~/Content/Css/build/css/custom.min.css",
            "~/Content/Css/build/css/jquery.stickytable.css",
            "~/Content/Css/build/css/sweetalert.css",
             "~/Content/Css/build/css/bootstrap-multiselect.css",
            "~/Content/Css/build/css/demo.css",
            "~/Content/Css/build/css/fileinput.css"));

            bundles.Add(new StyleBundle("~/Content/DatatablesCSS").Include(
            "~/Content/Css/datatables.net-bs/css/dataTables.bootstrap.min.css",
            "~/Content/Css/datatables.net-buttons-bs/css/buttons.bootstrap.min.css",
            "~/Content/Css/datatables.net-fixedheader-bs/css/fixedHeader.bootstrap.min.css",
            "~/Content/Css/datatables.net-responsive-bs/css/responsive.bootstrap.min.css",
            "~/Content/Css/datatables.net-scroller-bs/css/scroller.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/Images").Include(
            "~/Content/Images/icons/favicon.ico",
            "~/Content/Images/bg-01.jpg"));

            bundles.Add(new ScriptBundle("~/Content/Scripts").Include(
            "~/Content/Scripts/Login/Login.js",
            "~/Content/Scripts/Common/CommonValidation.js"));

            bundles.Add(new ScriptBundle("~/Content/Scripts/PageJs").Include(
            "~/Content/Css/jquery/dist/jquery.min.js",
            "~/Content/Css/bootstrap/dist/js/bootstrap.bundle.min.js",
            "~/Content/Css/build/js/bootstrap-datetimepicker.js",
            "~/Content/Css/fastclick/lib/fastclick.js",
            "~/Content/Css/nprogress/nprogress.js",
            "~/Content/Css/bootstrap-progressbar/bootstrap-progressbar.min.js",
            "~/Content/Css/iCheck/icheck.min.js",
            "~/Content/Css/jszip/dist/jszip.min.js",
            "~/Content/Css/pdfmake/build/vfs_fonts.js",
            "~/Content/Scripts/Dashboard/MainMenuPage.js",
            "~/Content/Css/moment/min/moment.min.js",
            "~/Content/Css/bootstrap-daterangepicker/daterangepicker.js",
            "~/Content/Css/bootstrap-wysiwyg/js/bootstrap-wysiwyg.min.js",
            "~/Content/Css/jquery.hotkeys/jquery.hotkeys.js",
            "~/Content/Css/google-code-prettify/src/prettify.js",
            "~/Content/Css/jquery.tagsinput/src/jquery.tagsinput.js",
            "~/Content/Css/switchery/dist/switchery.min.js",
            "~/Content/Css/select2/dist/js/select2.full.min.js",
            "~/Content/Css/parsleyjs/dist/parsley.min.js",
            "~/Content/Css/autosize/dist/autosize.min.js",
            "~/Content/Css/devbridge-autocomplete/dist/jquery.autocomplete.min.js",
            "~/Content/Css/starrr/dist/starrr.js",
            "~/Content/Css/build/js/custom.min.js",
            "~/Content/Css/jszip/dist/jszip.min.js",
            "~/Content/Css/pdfmake/build/pdfmake.min.js",
            "~/Content/Css/pdfmake/build/vfs_fonts.js",
            "~/Content/Css/build/js/sweetalert.js",            
            "~/Content/Css/build/js/bootstrap-multiselect.js",
            "~/Content/Css/build/js/fileinput.js",
            "~/Content/Scripts/Common/CommonJS.js"));

            bundles.Add(new ScriptBundle("~/Content/Scripts/ListJs").Include(
            "~/Content/Css/build/js/custom.min.js",
            "~/Content/Css/bootstrap/dist/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/Content/Scripts/DatatablesJs").Include(
            "~/Content/Css/datatables.net/js/jquery.dataTables.min.js",
            "~/Content/Css/datatables.net-bs/js/dataTables.bootstrap.min.js",
            "~/Content/Css/datatables.net-buttons/js/dataTables.buttons.min.js",
            "~/Content/Css/datatables.net-buttons-bs/js/buttons.bootstrap.min.js",
            "~/Content/Css/datatables.net-buttons/js/buttons.flash.min.js",
            "~/Content/Css/datatables.net-buttons/js/buttons.html5.min.js",
            "~/Content/Css/datatables.net-buttons/js/buttons.print.min.js",
            "~/Content/Css/datatables.net-fixedheader/js/dataTables.fixedHeader.min.js",
            "~/Content/Css/datatables.net-keytable/js/dataTables.keyTable.min.js",
            //"~/Content/Css/datatables.net-responsive/js/dataTables.responsive.min.js",
            //"~/Content/Css/datatables.net-responsive-bs/js/responsive.bootstrap.js",
            "~/Content/Css/datatables.net-scroller/js/dataTables.scroller.min.js"));

            bundles.Add(new ScriptBundle("~/Content/Scripts/GraphJs").Include(
            //"~/Content/Css/Chart.js/dist/Chart.min.js",
            "~/Content/Css/morris.js/morris.min.js",
            "~/Content/Css/raphael/raphael.min.js",

            "~/Content/Scripts/GraphJs/graph.js",
            "~/Content/Css/jquery-sparkline/dist/jquery.sparkline.min.js",
            "~/Content/Css/Flot/jquery.flot.js",
            "~/Content/Css/Flot/jquery.flot.pie.js",
            "~/Content/Css/Flot/jquery.flot.time.js",
            "~/Content/Css/Flot/jquery.flot.stack.js",
            "~/Content/Css/Flot/jquery.flot.resize.js",
            "~/Content/Css/flot.orderbars/js/jquery.flot.orderBars.js",
            "~/Content/Css/flot-spline/js/jquery.flot.spline.min.js",
            "~/Content/Css/DateJS/build/date.js"/*,
              "~/Content/Css/SignalRjs/fake-push.min.js",
            "~/Content/Css/SignalRjs/jquery.signalR-2.4.3.js",
            "~/Content/Scripts/Dashboard/Notification.js",
            "~/Content/Css/SignalRjs/jquery.signalR-2.4.3.min.js",
            "~/Content/Css/echarts/map/js/world.js"*/));

            bundles.Add(new ScriptBundle("~/Content/Scripts/ListJs").Include(
            "~/Content/Css/jquery/dist/jquery-3.5.1.min.js",
            "~/Content/Css/bootstrap/dist/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/Content/Scripts/OCListJs").Include(
            "~/Content/Scripts/OCSetup/OCList.js"
       ));
            bundles.Add(new ScriptBundle("~/Content/Scripts/DataTableRefreshJs").Include(
                  "~/Content/Css/build/js/customdatatable.min.js"
                  //,"~/Content/Css/build/js/custom.min.js"
          ));
        }
    }
}
