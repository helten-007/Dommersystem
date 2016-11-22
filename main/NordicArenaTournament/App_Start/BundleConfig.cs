using System.Web;
using System.Web.Optimization;

namespace NordicArenaTournament
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Scripts/jquery-{version}.js",
						"~/Content/js/jquery.min.js",
                        "~/Scripts/globalize/globalize.js",
                        "~/Scripts/globalize/cultures/globalize.culture.nb-NO.js"));

			bundles.Add(new ScriptBundle("~/bundles/skel").Include(
						"~/Content/js/skel.min.js",
						"~/Content/js/skel-layers.min.js",
						"~/Content/js/init.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/nordic-arena.jquery-val*"));


            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                        "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/speaker-scripts").Include(
                        "~/Scripts/jquery.signalR-{version}.js",
                        "~/Scripts/nordic-arena.speaker.js"));

            bundles.Add(new ScriptBundle("~/bundles/judge-scripts").Include(
                        "~/Scripts/jquery.signalR-{version}.js",
                        "~/Scripts/jquery.ui.touch-punch.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/nordic-arena.judge.js"));

			bundles.Add(new ScriptBundle("~/bundles/results-scripts").Include(
						"~/Scripts/jquery.signalR-{version}.js",
						"~/Scripts/nordic-arena.results.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

			bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

			bundles.Add(new StyleBundle("~/Content/skel-css").Include(
						"~/Content/skel.css",
						"~/Content/style.css",
						"~/Content/style-xlarge.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/bundles/judge-styles").Include(
                        "~/Content/themes/base/jquery-ui.css",
                        "~/Content/judge.css"));

			bundles.Add(new StyleBundle("~/bundles/results-styles").Include(
						"~/Content/themes/base/jquery-ui.css",
						"~/Content/results.css"));
        }
    }
}