/*
  DotNetNuke® - http://www.dotnetnuke.com
  Copyright (c) 2002-2007
  by DotNetNuke Corporation
 
  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
  to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 
  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
  of the Software.
 
  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
  DEALINGS IN THE SOFTWARE.

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' This script provides shared functionality for DNN Ajax classes. It is not
	''' intended to be referenced directly and is loaded by init.js.
	'''
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	'''     Version 1.0.0: Feb. 28, 2007, Nik Kalyani, nik.kalyani@dotnetnuke.com 
	''' </history>
	''' -----------------------------------------------------------------------------
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// S H A R E D                                                                                                //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Type.registerNamespace("DotNetNuke.UI.WebControls.Utility");

DotNetNuke.UI.WebControls.Utility.recurseElement = function(prefix, element)
        {
            var info = [];
            for(var e in element)
            {
                if (typeof(element[e]) == "object")
                {
                   var values = DotNetNuke.UI.WebControls.Utility.recurseElement(prefix + e + ".", element[e]);
                   for (var v in values)
                        info[v] = values[v];
                }
                else
                    info[prefix + e] = element[e];
            }
            return(info);
        }
        
DotNetNuke.UI.WebControls.Utility.createStyleSheet = function(url)
        {
        	if (document.createStyleSheet)
	            document.createStyleSheet(url);
	        else
	        {
	            var head = document.getElementsByTagName("head")[0];
	            head.innerHTML += "<link rel=\"stylesheet\" type=\"text/css\" href=\"" + url + "\" />";
	        }	
        }

DotNetNuke.UI.WebControls.Utility.checkEnter = function(event, clickHandlerId)
{ 	
    var NS4 = (document.layers) ? true : false;
    var code = 0;	
    if (NS4)
        code = event.which;
    else
        code = event.keyCode;
    if (code==13) 
    {
        var clickHandler = $get(clickHandlerId);
        clickHandler.click();
        return(false);
    }
}

String.prototype.replaceAll = function(find, replace)
{
    var str = this;
    str += "";
    var indexOfMatch = str.indexOf(find);
     
    while (indexOfMatch != -1)
    {
        str = str.replace(find, replace);
        indexOfMatch = str.indexOf(find);
    }
     
    return(str);
}

String.prototype.xmlEntityReplace = function()
{
    var str = this;
    str += "";
    str = str.replace(/&amp;/gi, "&");
    str = str.replace(/&quot;/gi, "\"");
    str = str.replace(/&apos;/gi, "'");
    str = str.replace(/&lt;/gi, "<");
    str = str.replace(/&gt;/gi, ">");

    return(str);
}

String.prototype.urlEncode = function()
{
    var str = this;
    str += "";
    str = escape(str).replace(/\+/gi, "%20");
    str = str.replace(/\//gi, "%2F");
    str = str.replace(/%26/gi, "%26amp%3B");

    return(str);        
}


////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// B A S E C O N T R O L                                                                                      //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: BaseControl class                                                                                   //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
DotNetNuke.UI.WebControls.BaseControl = function(instanceVarName, resourcesFolderUrl, theme, elementIdPrefix)
{
    if (!instanceVarName) 
        return(null);
    
    this._instanceVarName = instanceVarName;
    this._elementIdPrefix = (typeof(elementIdPrefix) == "undefined" ? "" : elementIdPrefix);
    this._control = "Shared";
    this._theme = theme;
    this._resourcesFolderUrl = (typeof(resourcesFolderUrl) == "undefined" ? "Resources/" : resourcesFolderUrl);
    this._styleSheetUrl = "";
    this._stylePrefix = "";
}

DotNetNuke.UI.WebControls.BaseControl.prototype = 
{
        getStylePrefix :
        function()
        {
            if (this._stylePrefix == "")
            	return(this._control + "-" + this._theme + "-");
	    else
		return(this._stylePrefix);
        },
        
        getTheme :
        function()
        {
            return(this._theme);
        },

        getResourcesFolderUrl :
        function()
        {
            return(this._resourcesFolderUrl);
        },

        getElementIdPrefix :
        function()
        {
            return(this._elementIdPrefix);
        },

        getInstanceVarName :        
        function()
        {
            return(this._instanceVarName);
        },

        getStyleSheetUrl :
        function()
        {
            return(this._styleSheetUrl);
        },
        
        setTheme :
        function(theme)
        {
            this._theme = theme;
        },

        setResourcesFolderUrl :
        function(resourcesFolderUrl)
        {
            this._resourcesFolderUrl = resourcesFolderUrl;
        },

        setElementIdPrefix :
        function(elementIdPrefix)
        {
            this._elementIdPrefix = elementIdPrefix;
        },

        setInstanceVarName :        
        function(instanceVarName)
        {
            this._instanceVarName = instanceVarName;
        },

        setStylePrefix :
        function(stylePrefix)
        {
            this._stylePrefix = stylePrefix;
        },

        setStyleSheetUrl :
        function(styleSheetUrl)
        {
            this._styleSheetUrl = styleSheetUrl;
        },
        
        // BEGIN: displayLoader
        // Returns HTML snippet with loading animation
        displayLoader :         
        function()
        {
            return("<div class=\"" + this.getStylePrefix() + "Loader\">&nbsp;</div>");
        },
        //END: displayLoader

        // BEGIN: addStyleSheet
        addStyleSheet : 
        function()
        {
            var url = this._resourcesFolderUrl + this._control + "/themes/" + this._theme + "/" + this._theme + ".css";
            if (this._styleSheetUrl != "")
                url = this._styleSheetUrl;
                
            DotNetNuke.UI.WebControls.Utility.createStyleSheet(url);
        }                
        // END: addStyleSheet        
}
DotNetNuke.UI.WebControls.BaseControl.registerClass("DotNetNuke.UI.WebControls.BaseControl");
// END: BaseControl class

