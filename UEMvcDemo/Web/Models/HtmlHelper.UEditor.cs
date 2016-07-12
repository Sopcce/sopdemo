//<http://www.sopcce.com>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2016-7-8</createdate>
//<author>郭家秋</author>
//<email>sopcce@qq.com</email>
//<log date="2016-7-8" version="0.5">新建</log>
//--------------------------------------------------------------
//<http://www.sopcce.com>
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using Newtonsoft.Json;
using System.Linq;

namespace System.Web.Mvc.Html
{ 
    /// <summary>
    /// UEditor的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperUEditorExtensions
    {
        /// <summary>
        /// 配置UEditor需要的js 
        /// </summary>
        public static readonly string ueditorconfig = @"
        <script src=""/Scripts/UEditor/ueditor.config.js""></script> 
        <script src=""/Scripts/UEditor/ueditor.all.min.js""></script> 
        <script src=""/Scripts/UEditor/lang/zh-cn/zh-cn.js""></script>
        <script src=""/Scripts/UEditor/ueditor.parse.min.js""></script>
        <script src=""/Scripts/UEditor/jquery-1.9.0.min.js""></script>
        <script type=""text/javascript""> var ue = UE.getEditor('{0}'); 
        </script>
        </script>
        ";

        /// <summary>
        /// 通过使用指定的 HTML 帮助器、窗体字段的名称、文本内容和指定的 
        /// HTML 特性，返回指定的UEditor textarea标签 元素。
        /// </summary>
        /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例。</param>
        /// <param name="name">要返回的窗体字段的名称。</param>
        /// <param name="value"> 文本内容。</param>
        /// <param name="htmlAttributes">一个对象，其中包含要为该元素设置的 HTML 特性。</param>
        /// <returns>指定的UEditor textarea标签 元素。</returns>
        public static MvcHtmlString UEditor(this HtmlHelper htmlHelper, string name, string value = null, Dictionary<string, object> htmlAttributes = null)
        {
            //初始化判断页面是否引用js
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("参数名称不能为空", "argsname is null");
            TagBuilder builder = new TagBuilder("div");
            Dictionary<string, object> htmlAttrs = new Dictionary<string, object>();
            if (htmlAttributes != null)
                htmlAttrs = new Dictionary<string, object>(htmlAttributes);
            var data = new Dictionary<string, object>();
            htmlAttrs.Add("data", JsonConvert.SerializeObject(data));
           // htmlAttrs.Add("plugin", "ueditor"); //用于js动态绑定实例id 这里不用这种方法
            builder.InnerHtml = string.Format(ueditorconfig,name) + htmlHelper.TextArea(name, value ?? string.Empty, htmlAttrs).ToString();
            return MvcHtmlString.Create(builder.ToString());
        }
        /// <summary>
        /// 通过使用指定的 HTML 帮助器、窗体字段的名称、文本内容和指定的 
        /// HTML 特性，返回指定的UEditor textarea标签 元素。
        /// </summary>
        /// <typeparam name="TModel"> 模型的类型。</typeparam>
        /// <typeparam name="TProperty">属性的类型。</typeparam>
        /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例。</param>
        /// <param name="expression">  一个表达式，用于标识包含要呈现的属性的对象。</param>
        /// <param name="htmlAttributes">一个对象，其中包含要为该元素设置的 HTML 特性。</param>
        /// <returns>指定的UEditor textarea标签 元素。</returns>
        /// <exception cref="T:System.ArgumentNullException:">
        /// 异常: expression 参数为 null。
        ///  </exception>
        public static MvcHtmlString UEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,  Dictionary<string, object> htmlAttributes = null)
        {
            TagBuilder builder = new TagBuilder("div");
            Dictionary<string, object> htmlAttrs = new Dictionary<string, object>();
            if (htmlAttributes != null)
                htmlAttrs = new Dictionary<string, object>(htmlAttributes);
            var data = new Dictionary<string, object>();
            htmlAttrs.Add("data", JsonConvert.SerializeObject(data));
            // htmlAttrs.Add("plugin", "ueditor"); //用于js动态绑定实例id 这里不用这种方法
            var name = String.Join(".",GetMembersOnPath(expression.Body as MemberExpression)
                .Select(m => m.Member.Name)
                .Reverse());
            builder.InnerHtml = string.Format(ueditorconfig, name) + htmlHelper.TextAreaFor(expression, htmlAttrs).ToString();
            
            return MvcHtmlString.Create(builder.ToString());
        }

        /// <summary>
        /// MemberExpression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// http://stackoverflow.com/questions/2789504/get-the-property-as-a-string-from-an-expressionfunctmodel-tproperty
        private static IEnumerable<MemberExpression> GetMembersOnPath(MemberExpression expression)
        {
            while (expression != null)
            {
                yield return expression;
                expression = expression.Expression as MemberExpression;
            }
        }

        
    }
}
