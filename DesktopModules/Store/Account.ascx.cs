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
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Account.
	/// </summary>
	public partial  class Account : PortalModuleBase
	{
        private StoreInfo storeInfo = null;
        private string parentTitle;
		private CustomerNavigation _nav;

		#region Public Properties
		public string ParentTitle
		{
			get {return parentTitle;}
			set
			{
				parentTitle = value;
				lblParentTitle.Text = parentTitle;
			}
		}
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
            //Localize the linkbuttons
            btnCart.Text = Localization.GetString("btnCart");
            btnProfile.Text = Localization.GetString("btnProfile");
            btnOrders.Text = Localization.GetString("btnOrders");

            try
            {
                if (storeInfo == null)
                {
                    StoreController storeController = new StoreController();
                    storeInfo = storeController.GetStoreInfo(PortalId);

                    if (storeInfo.PortalTemplates)
                    {
                        CssTools.AddCss(this.Page, PortalSettings.HomeDirectory + "Store", PortalId);
                    }
                    else
                    {
                        CssTools.AddCss(this.Page, this.TemplateSourceDirectory, PortalId);
                    }
                }

                _nav = new CustomerNavigation(Request.QueryString);

			    lblParentTitle.Text = parentTitle;

			    if (_nav.PageID == Null.NullString)
			    {
				    // Load the default control
				    _nav.PageID = "CustomerCart";
				    loadAccountControl();
			    }
			    else
			    {
				    loadAccountControl();
			    }
            }
            catch (Exception ex)
            {
                string ErrorSettings = Localization.GetString("ErrorSettings", this.LocalResourceFile);
                Exceptions.ProcessModuleLoadException(ErrorSettings, this, ex, true);
            }
		}

		protected void btnCart_Click(object sender, EventArgs e)
		{
			_nav.PageID = "CustomerCart";
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		protected void btnProfile_Click(object sender, EventArgs e)
		{
			_nav.PageID = "CustomerProfile";
			_nav.AddressID = Null.NullString;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		protected void btnOrders_Click(object sender, EventArgs e)
		{
			_nav.PageID = "CustomerOrders";
            _nav.OrderID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void accountControl_EditComplete(object sender, EventArgs e)
		{
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}
		#endregion

		#region Private Functions
		private void loadAccountControl()
		{
			// TODO: We may want to use caching here
			StoreControlBase control = (StoreControlBase)LoadControl(ModulePath + _nav.PageID + ".ascx");
			control.ParentControl = this as PortalModuleBase;
			control.EditComplete += new EventHandler(accountControl_EditComplete);

			plhAccountControl.Controls.Clear();
			plhAccountControl.Controls.Add(control);
		}
		#endregion
	}
}
