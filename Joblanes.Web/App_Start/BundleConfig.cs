using System.Web;
using System.Web.Optimization;

namespace Web.Joblanes
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery-1.*",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/jquery.validate.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/jquery.validate.unobtrusive.min.js", "~/Scripts/bootbox.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datetimepicker-moment").Include("~/Scripts/moment.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include("~/Scripts/bootstrap-datepicker.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datetimepicker").Include("~/Scripts/bootstrap-datetimepicker.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/commonjs").Include("~/Scripts/Common.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datepicker.css",
                     "~/Content/style.css",
                      "~/Content/site.css",
                      "~/Content/sidebar.css"));

            bundles.Add(new StyleBundle("~/Content/cssPdf").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-datepicker").Include("~/Content/bootstrap-datepicker.min.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap-datetimepicker").Include("~/Content/bootstrap-datetimepicker.min.css"));
           
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include("~/Vendor/DataTables-1.10.2/js/jquery.dataTables.js"));
            bundles.Add(new StyleBundle("~/Content/styledatatable").Include("~/Vendor/DataTables-1.10.2/css/jquery.dataTables.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-multiselect").Include("~/Content/bootstrap-multiselect/js/bootstrap-multiselect.js"));
            bundles.Add(new StyleBundle("~/Content/bootstrap-multiselect").Include("~/Content/bootstrap-multiselect/css/bootstrap-multiselect.css"));
           

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
