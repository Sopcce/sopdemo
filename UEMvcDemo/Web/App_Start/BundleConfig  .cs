using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Code removed for clarity.
            BundleTable.EnableOptimizations = false;
            bundles.UseCdn = true;   //enable CDN support
            var jqueryCdnPath = "http://apps.bdimg.com/libs/jquery/2.1.4/jquery.js";


            bundles.Add(new ScriptBundle("~/bundles/jquery", jqueryCdnPath).Include(
                       "~/Scripts/JQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/ueditor").Include(
                       "~/Scripts/JQuery/jquery-1.9.0.min.js",
                       "~/Scripts/UEditor/ueditor.config.js",
                       "~/Scripts/UEditor/ueditor.all.min.js",
                       "~/Scripts/UEditor/lang/zh-cn/zh-cn.js",
                       "~/Scripts/UEditor/sop.ajax.ueditor.js"



                        ));

            bundles.Add(new StyleBundle("~/bundles/amazeuicss").Include(
                       "~/Scripts/AmazeUI/css/amazeui.flat.min.css",
                       "~/Scripts/AmazeUI/css/amazeui.min.css",
                       "~/Scripts/AmazeUI/css/admin.css",
                       "~/Scripts/AmazeUI/css/app.css"
                       ));








            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/AmazeUI/css").Include(
                     "~/Content/bootstrap.css",
                     "~/Content/site.css"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
