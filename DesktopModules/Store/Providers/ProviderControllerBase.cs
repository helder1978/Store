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

using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.Store.Providers
{
	/// <summary>
	/// Summary description for ProviderControllerBase.
	/// </summary>
	public class ProviderControllerBase
	{
		protected ProviderInfo info = new ProviderInfo();

		public ProviderControllerBase()
		{
		}

		#region IProvider Members

		public ProviderInfo Info
		{
			get{return info;}
			set{info = value;}
		}

		public ProviderControlBase GetCheckoutControl(PortalModuleBase parentControl, string modulePath)
		{
			ProviderControlBase checkoutControl = loadControl(parentControl, modulePath, "Checkout");
			return checkoutControl;
		}

		public ProviderControlBase GetAdminControl(PortalModuleBase parentControl, string modulePath)
		{
			ProviderControlBase adminControl = loadControl(parentControl, modulePath, "Admin");
			return adminControl;
		}

		#endregion

		#region Protected Functions
		protected ProviderControlBase loadControl(PortalModuleBase parentControl, string controlPath, string controlName)
		{
			ProviderControlBase childControl;
			string controlValue = "";

			if (!controlPath.EndsWith("/")) controlPath += "/";
			controlPath += info.VirtualPath;

			if (!controlPath.EndsWith("/")) controlPath += "/";

			for (int i=0; i<info.Controls.Length; i++)
			{
				if (info.Controls[i].Name == controlName)
				{
					controlValue = info.Controls[i].Value;
					break;
				}
			}

			childControl = (ProviderControlBase)parentControl.LoadControl(controlPath + controlValue);
			childControl.ParentControl = parentControl;

			return childControl;
		}
		#endregion
	}
}
