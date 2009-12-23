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
	''' This script contains initialization and helper classes and should be loaded
	''' prior to any other DNN Ajax script.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	'''     Version 1.0.0: Feb. 28, 2007, Nik Kalyani, nik.kalyani@dotnetnuke.com 
	''' </history>
	''' -----------------------------------------------------------------------------
*/

var $MSAJAX_SCRIPT_LOAD_ATTEMPTS = 0;

var $dnnHostUrl = (typeof($dnnHostUrl) == "undefined" ? "" : $dnnHostUrl);
var $baseDnnScriptUrl = $dnnHostUrl + "Resources/Shared/scripts/";

if (typeof(Sys) === "undefined")
{
	// Asynchronously load the MS Ajax library
	var msAjax = document.createElement("script");
	msAjax.type = "text/javascript";
	msAjax.src = $baseDnnScriptUrl + "MSAJAX/MicrosoftAjax.js"
       	document.getElementsByTagName("head")[0].appendChild(msAjax);


	// Asynchronously load the MS Ajax Webforms library
	var msAjaxWF = document.createElement("script");
	msAjaxWF.type = "text/javascript";
	msAjaxWF.src = $baseDnnScriptUrl + "MSAJAX/MicrosoftAjaxWebForms.js"
    document.getElementsByTagName("head")[0].appendChild(msAjaxWF);
}
$checkIfMsAjaxScriptLoaded();

function $checkIfMsAjaxScriptLoaded()
{
	if (typeof(Sys) == "undefined")
	{
		$MSAJAX_SCRIPT_LOAD_ATTEMPTS++;
		if ($MSAJAX_SCRIPT_LOAD_ATTEMPTS < 20)
			window.setTimeout($checkIfMsAjaxScriptLoaded, 1500);
	}
	else
	{
		Type.registerNamespace("DotNetNuke.UI.WebControls");		
	}
}
