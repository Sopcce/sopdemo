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
        <script src=""/Scripts/JQuery/jquery-1.9.0.min.js""></script>
        <script type=""text/javascript""> var editor = UE.getEditor('{0}'); </script>
        <script src=""/Scripts/UEditor/sop.ajax.ueditor.js""></script>";
      
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
                throw new ArgumentException("参数名称不能为空", "argsname is null");
            TagBuilder builder = new TagBuilder("div");
            Dictionary<string, object> htmlAttrs = new Dictionary<string, object>();
            if (htmlAttributes != null)
                htmlAttrs = new Dictionary<string, object>(htmlAttributes);
            var data = new Dictionary<string, object>();
            htmlAttrs.Add("data", JsonConvert.SerializeObject(data));
           // htmlAttrs.Add("plugin", "ueditor");
            builder.InnerHtml = string.Format(ueditorconfig,name) + htmlHelper.TextArea(name, value ?? string.Empty, htmlAttrs).ToString();
            return MvcHtmlString.Create(builder.ToString());
        }

            
        public static MvcHtmlString UEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,  Dictionary<string, object> htmlAttributes = null)
        {
            TagBuilder builder = new TagBuilder("div");
            Dictionary<string, object> htmlAttrs = new Dictionary<string, object>();
            if (htmlAttributes != null)
                htmlAttrs = new Dictionary<string, object>(htmlAttributes);
            var data = new Dictionary<string, object>();
            htmlAttrs.Add("data", JsonConvert.SerializeObject(data));
            //htmlAttrs.Add("plugin", "ueditor");
            var name = String.Join(".",GetMembersOnPath(expression.Body as MemberExpression)
                .Select(m => m.Member.Name)
                .Reverse());
            //var name = GetFullPropertyName(expression);

            builder.InnerHtml = string.Format(ueditorconfig, name) + htmlHelper.TextAreaFor(expression, htmlAttrs).ToString();
            var asda = builder.ToString();
            return MvcHtmlString.Create(asda);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// http://stackoverflow.com/questions/2789504/get-the-property-as-a-string-from-an-expressionfunctmodel-tproperty
        /// 
        private static IEnumerable<MemberExpression> GetMembersOnPath(MemberExpression expression)
        {
            while (expression != null)
            {
                yield return expression;
                expression = expression.Expression as MemberExpression;
            }
        }

        #region 未使用
        //// code adjusted to prevent horizontal overflow
        //static string GetFullPropertyName<T, TProperty>
        //(Expression<Func<T, TProperty>> exp)
        //{
        //    MemberExpression memberExp;
        //    if (!TryFindMemberExpression(exp.Body, out memberExp))
        //        return string.Empty;

        //    var memberNames = new Stack<string>();
        //    do
        //    {
        //        memberNames.Push(memberExp.Member.Name);
        //    }
        //    while (TryFindMemberExpression(memberExp.Expression, out memberExp));

        //    return string.Join(".", memberNames.ToArray());
        //}

        //// code adjusted to prevent horizontal overflow
        //private static bool TryFindMemberExpression
        //(Expression exp, out MemberExpression memberExp)
        //{
        //    memberExp = exp as MemberExpression;
        //    if (memberExp != null)
        //    {
        //        // heyo! that was easy enough
        //        return true;
        //    }

        //    // if the compiler created an automatic conversion,
        //    // it'll look something like...
        //    // obj => Convert(obj.Property) [e.g., int -> object]
        //    // OR:
        //    // obj => ConvertChecked(obj.Property) [e.g., int -> long]
        //    // ...which are the cases checked in IsConversion
        //    if (IsConversion(exp) && exp is UnaryExpression)
        //    {
        //        memberExp = ((UnaryExpression)exp).Operand as MemberExpression;
        //        if (memberExp != null)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //private static bool IsConversion(Expression exp)
        //{
        //    return (
        //        exp.NodeType == ExpressionType.Convert ||
        //        exp.NodeType == ExpressionType.ConvertChecked
        //    );
        //} 
        #endregion
    }
}
