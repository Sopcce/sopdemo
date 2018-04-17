using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 输出mvc HtmlHelper
        /// </summary>
        /// <returns></returns>
        public ActionResult demo()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult MvcPost()
        {
            return View();
        }
        /// <summary>
        /// 异步提交页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxPost()
        {
            return View();
        }
     
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AjaxPost(FormCollection context)
        {
            var name = context["editorValue"];
            //存入数据库或者其他操作
            return Content("提交成功，我是后端数据"+ name);
        }









        
    }
}
