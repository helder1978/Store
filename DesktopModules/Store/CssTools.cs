/*
'  DotNetNuke -  http://www.dotnetnuke.com
'  Copyright (c) 2002-2007
'  by Shaun Walker ( sales@perpetualmotion.ca ) of Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
' 
'  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
'  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
'  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
'  to permit persons to whom the Software is furnished to do so, subject to the following conditions:
' 
'  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
'  of the Software.
' 
'  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
'  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
'  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
'  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
'  DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace DotNetNuke.Modules.Store.WebControls
{
    public class CssTools
    {
        private static string linkID = "_Store_Portals_";
        private static string templatesCss = "/Templates/template.css";

        public static void AddCss(Page page, string cssPath, int portalID)
        {
            cssPath += templatesCss;

            HtmlLink htmlLink = (HtmlLink)page.Header.FindControl(linkID + portalID.ToString());

            if (htmlLink == null)
            {
                htmlLink = new HtmlLink();
                htmlLink.ID = linkID + portalID.ToString();
                htmlLink.Attributes.Add("rel", "stylesheet");
                htmlLink.Attributes.Add("type", "text/css");
                htmlLink.Href = cssPath;
                page.Header.Controls.Add(htmlLink);
            }
            else
            {
                htmlLink.Href = cssPath;
            }
        }
    }
}
