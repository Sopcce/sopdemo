var login = document.getElementById("login");
var yhzc = document.getElementById("yhzc");
var h2 = login.getElementsByTagName("h2");
var dl = login.getElementsByTagName("dl");
var ul = login.getElementsByTagName("ul");
var lis = ul[0].getElementsByTagName("li");
var allspan = ul[0].getElementsByTagName("span");
var qlog = h2[0].getElementsByTagName("a");
var divs = login.getElementsByTagName("div");
qlog[0].onclick = function()
{
for(var i = 0; i < divs.length; i++)
{divs[i].className = null;}
for(var i = 0; i < allspan.length; i++)
{allspan[i].id = null;}
lis[0].className = "none";
dl[0].className = "qlogin block";
}
for(var x = 1; x < 4; x++)
{show();}
function show()
{
var test = "btype" +x;
var btype = document.getElementById("btype" + x);
var as = btype.getElementsByTagName("a");
var bdivs = btype.getElementsByTagName("div");
var spans = btype.getElementsByTagName("span");
for(var i = 0; i < spans.length; i++)
{
spans[i].num = i;
spans[i].onmouseover = type;
}
function type()
{
for(var i = 0; i < lis.length; i++)
{lis[i].className = null;}
for(var i = 0; i < allspan.length; i++)
{allspan[i].id = null;}
for(var i = 0; i < divs.length; i++)
{divs[i].className = null;}
dl[0].className = "qlogin";
yhzc.id = null;
spans[this.num].id = spans[this.num].className;
bdivs[this.num].className = "block";
btype.className = "hoverli";
}
}