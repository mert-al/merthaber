using System.Web;
using System.Web.Optimization;

namespace HaberSitesiAdmin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Content/assets/js/dashboards-analytics.js",
                      "~/Content/assets/js/main.js",
                      "~/Content/assets/vendor/libs/apex-charts/apexcharts.js",
                      "~/Content/assets/vendor/js/menu.js",
                      "~/Content/assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.js",
                      "~/Content/assets/vendor/js/bootstrap.js",
                      "~/Content/assets/vendor/libs/popper/popper.js",
                      "~/Content/assets/vendor/libs/jquery/jquery.js",
                      "~/Content/assets/vendor/js/helpers.js",
                      "~/Content/assets/js/config.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/assets/vendor/css/core.css",
                      "~/Content/assets/vendor/css/theme-default.css",
                      "~/Content/assets/css/demo.css",
                      "~/Content/assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.css",
                      "~/Content/assets/vendor/libs/apex-charts/apex-charts.css",
                      "~/Content/assets/vendor/fonts/boxicons.css"));
        }
    }
}
