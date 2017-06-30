using System.Web;
using System.Web.Optimization;

namespace TASRequestForm.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/calendar").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/fullcalendar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/jquery-2.1.1.min.js",
                        "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/timepicker")
                .Include("~/Scripts/moment.js")
                .Include("~/Scripts/bootstrap-datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
            "~/Scripts/jquery.inputmask/inputmask.js",
            "~/Scripts/jquery.inputmask/jquery.inputmask.js",
            "~/Scripts/jquery.inputmask/inputmask.extensions.js",
            "~/Scripts/jquery.inputmask/inputmask.date.extensions.js",
            "~/Scripts/jquery.inputmask/inputmask.numeric.extensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-3.2.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/pushy").Include(
                        "~/Scripts/pushy.min.js"));

            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Content/css/bootstrap.min.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/css/font-awesome.min.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/css/site.css", new CssRewriteUrlTransformWrapper()));

            bundles.Add(new StyleBundle("~/bundles/css/timepicker")
                .Include("~/Content/css/bootstrap-datetimepicker.min.css", new CssRewriteUrlTransformWrapper()));

            bundles.Add(new StyleBundle("~/bundles/css/calendar")
                .Include("~/Content/css/fullcalendar.min.css", new CssRewriteUrlTransformWrapper()));

        }
    }

    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}
