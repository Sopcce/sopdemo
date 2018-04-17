/*实例化ueditor
实例分为2种方法：
1、var editor = UE.getEditor('idname');//渲染编辑器（MVC直接使用这一种）
2、根据plugin='ueditor'动态绑定，但是要引用jquery.livequery.min.js
*/
$(function () {
    //jquery.livequery.min.js
    /*动态绑定方法，这种方法也可以，mvc中使用这种也可以，这种方法就把
     UEditor的HtmlHelper输出方法，中实例化注释掉。
    */
    //$("textarea[plugin='ueditor']").livequery(function () {
    //    var id = $(this).attr("id");
    //    ue = UE.getEditor(id);
    //});
    //~/Home/AjaxPost页面
    var ue = UE.getEditor('myeditor');
    $("#btn").click(function () {
        var content = ue.getContent();
        ue.sync();//异步提交
        $.post("/Home/AjaxPost", { action: "AjaxPost", editorValue: content },
				function (data) {
				    alert(data);
				});
    });
  
    
   
});
