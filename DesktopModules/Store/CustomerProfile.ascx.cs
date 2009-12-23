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
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CustomerProfile.
	/// </summary>
	public partial  class CustomerProfile : StoreControlBase
	{
		private CustomerNavigation accountNav;

		#region Controls

		protected DotNetNuke.UI.UserControls.LabelControl lblParentTitle;

		#endregion

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

		#region Events
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				accountNav = new CustomerNavigation(Request.QueryString);

				bool isLoggedIn = (this.UserId >= 0);

				pnlLoginMessage.Visible = !isLoggedIn;
				pnlAddressProvider.Visible = isLoggedIn;

				if (isLoggedIn)
				{
					loadAddressProvider();
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		private void editControl_EditComplete(object sender, EventArgs e)
		{
			accountNav.AddressID = Null.NullString;
			Response.Redirect(accountNav.GetNavigationUrl(), false);
		}

		protected override void OnPreRender(EventArgs e)
		{
			// Set the title in the parent control
			Account accountControl = (Account)parentControl;
			accountControl.ParentTitle = lblParentTitle.Text;

			base.OnPreRender (e);
		}

		#endregion

		#region Private Function
		private void loadAddressProvider()
		{
			plhAddressProvider.Controls.Clear();

			//Get an instance of the provider
			IAddressProvider addressProvider = StoreController.GetAddressProvider(ModulePath);
			
			//Create an instance of the provider's profile control
			ProviderControlBase providerControl = addressProvider.GetProfileControl(this, ModulePath);

			providerControl.EditComplete += new EventHandler(editControl_EditComplete);

			plhAddressProvider.Controls.Add(providerControl);
		}
		#endregion
	}
}
