/*实例化editor
实例分为2中方法：
1、var editor = UE.getEditor('name');//渲染编辑器（MVC直接使用这一种）
2、根据plugin='ueditor'动态绑定，但是要引用jquery.livequery.min.js
默认是editor,使用mvc是不用初始化实例
*/
$(function () {
    //jquery.livequery.min.js
    /*动态绑定方法，这种方法也可以，mvc中使用这种也可以，这种方法就把
     UEditor的HtmlHelper输出方法，中实例化注释掉。
     jq Dynamic binding event method.
    */
    //$("textarea[plugin='ueditor']").livequery(function () {
    //    var id = $(this).attr("id");
    //    ue = UE.getEditor(id);
    //});

    var editor = UE.getEditor('myeditor');
    $("#btn").click(function () {
        var content = editor.getContent();
        editor.sync();//异步提交
        $.post("/Home/Post", { action: "post", editorValue: content },
				function (data) {
				    alert(data);
				});
    });
  
    
   
});
