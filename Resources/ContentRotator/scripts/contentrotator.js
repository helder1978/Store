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
	''' This script enables client-side rotation of content.
	'''
	''' Ensure that ~/Resources/Shared/scripts/init.js is called from the browser before calling this script
	''' This script will fail if the required AJAX libraries loaded by init.js are not present.
	''' </summary>
	''' <remarks>
	'''	Based mostly on GreyWyvern's HTML content Scroller & Marquee script
	'''	Portions Copyright GreyWyvern 2007
	'''	Licenced for free distribution under the BSDL
	''' </remarks>
	''' <history>
	'''     Version 1.0.0: Feb. 28, 2007, Nik Kalyani, nik.kalyani@dotnetnuke.com 
	''' </history>
	''' -----------------------------------------------------------------------------
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// C O N T E N T R O T A T O R                                                                                //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("DotNetNuke.UI.WebControls.ContentRotator");
// END: Namespace management

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: Rotator class                                                                                       //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
DotNetNuke.UI.WebControls.ContentRotator.Rotator = function(instanceVarName, containerId, width, height, direction, interval, elementIdPrefix, theme, resourcesFolderUrl)
{
      DotNetNuke.UI.WebControls.ContentRotator.Rotator.initializeBase(this, [instanceVarName, resourcesFolderUrl, theme, elementIdPrefix]);
      this._control = "ContentRotator";
      this._theme = (typeof(theme) == "undefined" ? "Simple" : theme);     
      this._rssProxyUrl = (typeof(rssProxyUrl) == "undefined" ? "http://pipes.yahoo.com/pipes/Aqn6j8_H2xG4_N_1JhOy0Q/run?_render=json&_callback=[CALLBACK]&feedUrl=" : rssProxyUrl);
      this.addStyleSheet();
        
      this._height = (typeof(height) == "undefined" ? 200 : height);
      this._width = (typeof(width) == "undefined" ? 400 : width);
      this._direction = (typeof(direction) == "undefined" ? "left" : (direction == "left" || direction == "right" || direction == "up" || direction == "down" ? direction : "left"));
      this._interval = (typeof(interval) == "undefined" ? 2500 : interval);
      
      this._container = $get(containerId);
      this._container.className = "Normal " + this.getStylePrefix() + "Container";
      this._container.style.position = "relative";
      this._container.style.width = this._width + "px";
      this._container.style.height = this._height + "px";
      this._container.innerHTML = this.displayLoader();
      this._container.style.overflow = "hidden";
      this._offset = (this._direction == "up" || this._direction == "down") ? this._height : this._width;

      this._content = [];
      this._contentprev = 0;
      this._contentcurr = 1;
      this._motion = false;
      this._mouse = false;      
      this._readyToScroll = true;
}

DotNetNuke.UI.WebControls.ContentRotator.Rotator.prototype = 
{
        addContent :
        function(content)
        {
            this._content[this._content.length] = content;
        },
        
        addFeedContent :
        function(url, attributeToUse)
        {
            this._readyToScroll = false;
            
            // Create a new function
            var counter = 0;
            try
            {
                while(eval(this._instanceVarName + counter))
                    counter++;
            }
            catch(e)
            {
            }

            // Dynamically create a callback function and pass to it the instance name and callback data
            eval(this._instanceVarName + counter + " = new Function(\"data\", \"rssRenderingHandler('" + this._instanceVarName + "', data,'" + attributeToUse + "')\")");

            var newScript = document.createElement("script");
            newScript.type = "text/javascript";
            newScript.src = this.getRssProxyUrl(this._instanceVarName + counter) + url.urlEncode();
            document.getElementsByTagName("head")[0].appendChild(newScript);            
        },
                
        getRssProxyUrl :
        function(callback)
        {
            return(this._rssProxyUrl.replace("[CALLBACK]", callback));
        },

        // BEGIN: scrollLoop
        scrollLoop : 
        function() 
        {
            if (!this._motion && this._mouse) 
                return false;
        
            if (this._offset == 1) 
            {
                this._contentprev = this._contentcurr;
                this._contentcurr = (this._contentcurr + 1 >= this._content.length) ? 0 : this._contentcurr + 1;
                if (this._direction == "up" || this._direction == "down") 
                {
                    this._content[this._contentcurr].style.top = ((this._direction == "down") ? "-" : "") + this._height + "px";
                    this._content[this._contentprev].style.top = "0px";
                    this._offset = this._height;
                } 
                else 
                {
                    this._content[this._contentcurr].style.left = ((this._direction == "right") ? "-" : "") + this._width + "px";
                    this._content[this._contentprev].style.left = "0px";
                    this._offset = this._width;
                } 
                
                this._motion = false;
            } 
            else 
            {
                if (!this._motion) 
                {
                    this._motion = true; 
                    var x = -1;
                    while (true) 
                    { 
                        if (Math.abs(this._offset) - Math.pow(2, ++x) <= Math.abs(this._offset) / 2) break; 
                    }
                    this._offset = (this._direction == "up" || this._direction == "left") ? Math.pow(2, x) : -Math.pow(2, x);
                } 
                else 
                    this._offset /= 2;
                    
                if (this._direction == "up" || this._direction == "down") 
                {
                    this._content[this._contentcurr].style.top = this._offset + "px";
                    this._content[this._contentprev].style.top = (((this._direction == "down") ? this._height : -(this._height + 2)) + this._offset) + "px";
                } 
                else 
                {
                    this._content[this._contentcurr].style.left = this._offset + "px";
                    this._content[this._contentprev].style.left = (((this._direction == "right") ? this._width : -(this._width + 2)) + this._offset) + "px";
                } 
                
                setTimeout
                (
                    this.getInstanceVarName() + ".scrollLoop()",
                        30
                 );
            }
        },
        // END: scrollLoop
        
        // BEGIN: scroll
        scroll : 
        function(isReady)
        {
            if (isReady)
                this._readyToScroll = true;

            if (!this._readyToScroll)
                return; 
                
            if (this._content.length == 0)
            {
                this.addFeedContent("http://rss.news.yahoo.com/rss/world","description");
                return;
            }
                        
            while (this._container.firstChild) 
                    this._container.removeChild(this._container.firstChild);

            for (var x = 0; x < this._content.length; x++) 
            {
                    var table = document.createElement("table");
                    table.style.position = "absolute";
                    table.style.width = this._width + "px";
                    table.style.height = this._height + "px";
                    table.style.overflow = "hidden";
                    table.cellPadding = table.cellSpacing = table.border = "0";
                    table.style.left = table.style.top = "0px";
                    if (x) 
                    {
                        switch (this._direction) 
                        {
                            case "up": table.style.top = this._height + "px"; break;
                            case "down": table.style.top = -(this._height + 2) + "px"; break;
                            case "left": table.style.left = this._width + "px"; break;
                            case "right": table.style.left = -(this._width + 2) + "px"; break;
                        }
                    }
                    
                    var tbody = document.createElement("tbody");
                    var tr = document.createElement("tr");
                    var td = document.createElement("td");
                    td.innerHTML = this._content[x];
                    tr.appendChild(td);
                    tbody.appendChild(tr);
                    table.appendChild(tbody);
                    this._container.appendChild(this._content[x] = table);
            }
            
            if (this._content.length > 1) 
            {
                this._container.onmouseover = function() 
                { 
                    this._mouse = true; 
                }
                
                this._container.onmouseout = function() 
                { 
                    this._mouse = false; 
                }
                
                setInterval        
                (
                    this.getInstanceVarName() + ".scrollLoop()",
                    this._interval
                );
            }
        }                
        // END: scroll        
}
DotNetNuke.UI.WebControls.ContentRotator.Rotator.registerClass("DotNetNuke.UI.WebControls.ContentRotator.Rotator", DotNetNuke.UI.WebControls.BaseControl);
// END: Rotator class

function rssRenderingHandler(instanceVarName, result, attributeToUse)
{
    var instance = eval(instanceVarName);

    if (result != null)
    {
        var itemCollection = result.value["items"];
        for(var item in itemCollection)
        {   
            var itemData = [];
            if (typeof(itemCollection[item]) == "object")
            {
                var ivalues = DotNetNuke.UI.WebControls.Utility.recurseElement("", itemCollection[item]);
                for(var iv in ivalues)
                    itemData[iv] = ivalues[iv];
            }
            else
                itemData[item] = itemCollection[item];

            if (itemData[attributeToUse])
                instance.addContent(itemData[attributeToUse])                
        }

    }
    
    instance.scroll(true);
}
