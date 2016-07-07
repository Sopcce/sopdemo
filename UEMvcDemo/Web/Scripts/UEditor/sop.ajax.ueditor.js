
var editor = new UE.ui.Editor();//实例
editor.render('myeditor');//渲染编辑器
$(function () {
	$("#btn").click(function () {
		var content = editor.getContent();
		editor.sync();//异步提交
		$.post("/Home/Post", { action: "post", editorValue: content },
				function (data) {
					alert(data);
				});

	});
});
