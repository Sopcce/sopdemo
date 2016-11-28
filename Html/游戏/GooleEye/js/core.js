(function ($) {
	var eyeLeft,eyeRight,pupilLeft,pupilRight;
	/**主体函数,在页面加载时自动执行
	 * eyeLeft,左边的眼睛 span
	 * eyeRight,右边的眼睛 span
	 * pupilLeft,左边的瞳孔图片
	 * pupilRight,右边的瞳孔图片
	 */
	
	//当页面加载完成后,初始化
	addEvent(window,"load",function () {
		//分别获取元素
		eyeLeft=$("eyeLeft");
		eyeRight=$("eyeRight");
		pupilLeft=$("pupilLeft");
		pupilRight=$("pupilRight");
		
		//当鼠标上页面上移动时,执行movePupil
		addEvent(document,"mousemove",movePupil);
	});
	
	function movePupil(evt) {
		/**movePupil,获取鼠标位置,并调用setPupilPos函数
		 * 
		 * @param evt Event 在支持W3C标准事件机制的浏览器中,事件对象会作为第一个参数传递给事件处理函数
		 * 
		 * 在IE中,事件对象作为全局event出现
		 */
		evt = evt || window.event;//获取事件对象
		var MousePos = {//获取鼠标位置
			x:evt.clientX,
			y:evt.clientY
		};
		
		//调用setPupilPos函数,去计算并设置瞳孔图片应在的位置
		setPupilPos(eyeLeft,pupilLeft,MousePos);
		setPupilPos(eyeRight,pupilRight,MousePos);
	}
	
	function setPupilPos(eye,pupil,MousePos) {
		/**setPupilPos 计算并设置瞳孔图片的位置
		 * 
		 * @param eye Object 瞳孔容器眼睛 DOM对象
		 * @param pupil Obejct 与眼睛对应的瞳孔图片
		 * @param MousePos Object 包含鼠标位置信息的对象
		 */
		
		var eyePos=getPos(eye);//获取眼睛对象的位置
		var eyeHPos={//获取眼睛中心的位置
			x:eyePos.x+parseInt(eye.offsetWidth/2),
			y:eyePos.y+parseInt(eye.offsetHeight/2)
		};
		
		//计算眼睛中心与鼠标所在点的距离
		var distance=calcHyp(eyeHPos.x-MousePos.x,eyeHPos.y-MousePos.y);
		if (Math.abs(distance)<=34 ) {//当距离小于34,即在眼睛内部时,直接将瞳孔的位置设为鼠标的位置
			pupil.style.left = (MousePos.x-eyePos.x-6)+"px";//6是瞳孔的半径,这样使瞳孔中心位置与鼠标位置相重合
			pupil.style.top = (MousePos.y-eyePos.y-6)+"px";
		} else {//当鼠标上眼睛外面时,需要计算角度
			var sin= (eyeHPos.y-MousePos.y)/distance;//角度正弦值
			var cos=(eyeHPos.x-MousePos.x)/distance;//角度余弦值
			//瞳孔图片是采用相对于眼睛(其父元素)的定位方式,40是瞳孔活动范围半径,58是眼睛半径
			pupil.style.left= 58-cos*40-6+"px";
			pupil.style.top=58-sin*40-6+"px";
		}
	}
	
	function calcHyp(x,y) {
		/**calcHyp 通过直角三角形的已知两边计算斜边的长度
		 * 
		 * @param x Number 直角三角形的底边
		 * @param y Number 直角三角形的高
		 * @return Number 直角三角形斜边的长度
		 * 
		 */
		return Math.sqrt(Math.pow(x,2)+Math.pow(y,2));
	}
	function getPos(obj) {
		/**getPos 获取DOM对象在页面中的绝对坐标
		 * 
		 * @param obj Object DOM对象
		 * @return pos Object 包括坐标信息的对象
		 * 
		 * 获取DOM元素相对于定位参考祖先元素的位置是使用对象的offsetLeft及offsetTop属性
		 * 获取DOM元素的定位参考祖先元素是使用对象的offsetParent属性
		 * 通过递归,可以获取元素相对于页面(document.body,document.body.offsetParent为null)的坐标
		 */
		var pos={//初始值为0
			x:0,
			y:0
		};
		do {
			pos.x+= obj.offsetLeft;
			pos.y+=obj.offsetTop;
		} while(obj=obj.offsetParent);//当存在offsetParent时(即没有到达顶层body时)一直循环,并更新obj
		return pos;
	}
	
	function addEvent(obj,evtype,fn) {
		/**addEvent 给元素添加事件监听
		 * 
		 * @param obj Object DOM元素或window对象等
		 * @param evtype String 不带"on"前缀的事件名称
		 * @param fn Function 事件处理函数
		 * @return Object 该元素
		 */
		//优先考虑W3C事件机制
		if (obj.addEventListener) obj.addEventListener(evtype,fn,false);
		//否则,再考虑IE的情况,IE的事件名称需要加上"on"前缀
		else if (obj.attachEvent) obj.attachEvent("on"+evtype,fn);
		//如果上面两种事件注册方法都不支持,则使用传统的方法(这种情况几乎不会出现)
		else obj["on"+evtype]=fn;
		return obj;
	}
})(function (id) {
	/**DOM元素 ID选择器
	 * 
	 * @param id String 元素的ID
	 * @return Object DOM元素
	 * 
	 * 这个函数作为一个参数传递给了另一个匿名函数,采用这种写法仅仅因为更习惯^0^
	 * 因为jQuery中也将选择器做为第一个参数传递给jQuery(function ($){})其中的匿名函数
	 */
	return document.getElementById(id);
});