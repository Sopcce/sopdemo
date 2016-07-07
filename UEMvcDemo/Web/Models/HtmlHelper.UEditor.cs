using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using Newtonsoft.Json;


namespace System.Web.Mvc.Html
{ 
    /// <summary>
    /// UEditor的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperUEditorExtensions
    {
        //string name, string value,
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例。</param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString UEditor(this HtmlHelper htmlHelper, string name, string value = null, Dictionary<string, object> htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("参数名不能为空", "argsname is null");
            }
            
            //htmlHelper.Script("~/Bundle/Scripts/UEditor");
            TagBuilder builder = new TagBuilder("<div>");
            Dictionary<string, object> htmlAttrs = new Dictionary<string, object>();
            if (htmlAttributes != null)
                htmlAttrs = new Dictionary<string, object>(htmlAttributes);
            var data = new Dictionary<string, object>();
            
            htmlAttrs.Add("data", JsonConvert.SerializeObject(data));
            builder.InnerHtml = htmlHelper.TextArea(name, value ?? string.Empty, htmlAttrs).ToString();
            var asd = builder.ToString();
            return MvcHtmlString.Create(asd);
        }

            
        public static MvcHtmlString UEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string tenantTypeId, long associateId = 0, Dictionary<string, object> htmlAttributes = null)
        {
            TagBuilder builder = new TagBuilder("span");
            Dictionary<string, object> htmlAttrs = new Dictionary<string, object>();
            if (htmlAttributes != null)
                htmlAttrs = new Dictionary<string, object>(htmlAttributes);
            var data = new Dictionary<string, object>();
            data.Add("tenantTypeId", tenantTypeId);
            data.Add("associateId", associateId);
            htmlAttrs.Add("data", JsonConvert.SerializeObject(data));
            htmlAttrs.Add("plugin", "ueditor");
            builder.InnerHtml = htmlHelper.TextAreaFor(expression, htmlAttrs).ToString();
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}
