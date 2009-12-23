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
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security.Roles;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial  class Edit : PortalModuleBase
	{

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		#region Private Declarations

		private CatalogNavigation _nav = null;
		
		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
				// Get the navigation settings
				_nav = new CatalogNavigation(Request.QueryString);

				if (_nav.ProductID != Null.NullInteger)
				{
					ProductEdit productEdit = (ProductEdit)LoadControl(ModulePath + "ProductEdit.ascx");
					productEdit.DataSource = _nav.ProductID;
					productEdit.EditComplete += new EventHandler(editControl_EditComplete);

					plhControls.Controls.Add(productEdit);
				}
				else if (_nav.CategoryID != Null.NullInteger)
				{
					CategoryEdit categoryEdit = (CategoryEdit)LoadControl(ModulePath + "CategoryEdit.ascx");
					categoryEdit.DataSource = _nav.CategoryID;
					categoryEdit.EditComplete += new EventHandler(editControl_EditComplete);

					plhControls.Controls.Add(categoryEdit);
				}
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		private void editControl_EditComplete(object sender, EventArgs e)
		{
			_nav.ProductID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}
		#endregion
	}
}
