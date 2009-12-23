function ieModuleHoverFix(menuId,adminMode) {

	var oMenu = document.getElementById(menuId);
	var ieULs = oMenu.getElementsByTagName('ul');
	
	/** IE script to cover <select> elements with <iframe>s **/
	for (j=0; j<ieULs.length; j++) {
		ieULs[j].innerHTML = ('<iframe src="javascript:false;" scrolling="no" frameborder="0"></iframe>' + ieULs[j].innerHTML);
		var oShim = ieULs[j].firstChild;
			//oShim.style.width=(ieULs[j].offsetWidth+2)+"px";
			oShim.style.width=ieULs[j].offsetWidth+"px";
			oShim.style.height=ieULs[j].offsetHeight+"px";
			oShim.style.left = "-1px";
			oShim.style.position = "absolute";
			oShim.style.display = "block";
			oShim.style.zIndex = "0";
			oShim.style.filter = "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
			ieULs[j].style.zIndex = "11111";
	} 
	/** IE script to change class on mouseover **/
		var ieLIs = oMenu.getElementsByTagName("LI");
		//for (var i=0; i<ieLIs.length; i++) if (ieLIs[i]) {
		//	ieLIs[i].onmouseover=function() {this.className+=" sfhover";}
		//	ieLIs[i].onmouseout=function() {this.className=this.className.replace(new RegExp("( ?|^)sfhover\\b"), "");}
		//} 
		for (var i=0; i<ieLIs.length; i++) {
			ieLIs[i].onmouseover=function() {this.className+=" sfhover";}
			ieLIs[i].onmouseout=function() {this.className=this.className.replace(new RegExp(" sfhover\\b"), "");}
		} 
	
	//placeObject(oMenu,adminMode);
}

function placeObject(obj,adminMode) {
	var x = findPosX(obj.parentElement);
	var y = findPosY(obj.parentElement);
	var w = obj.parentElement.offsetWidth;
	//obj.parentElement.removeChild(obj);
	if (adminMode == "True") {obj.parentElement.removeChild(obj);}
	obj.style.left = x;
	obj.style.top = y;
	obj.style.width = w;
	//document.body.appendChild(obj);
	if (adminMode == "True") {document.body.appendChild(obj);}
}

function findPosX(obj) {
	var curleft = 0;
	if (obj.offsetParent) {
		while (obj.offsetParent) {
			curleft += obj.offsetLeft;
			obj = obj.offsetParent;
		}
	} else if (obj.x) {
		curleft += obj.x 
	}
	return curleft;
}

function findPosY(obj) {
	var curtop = 0;
	if (obj.offsetParent) {
		while (obj.offsetParent) {
			curtop += obj.offsetTop;
			obj = obj.offsetParent;
		}
	} else if (obj.y) {
		curtop += obj.y;
	}
	return curtop;
}
