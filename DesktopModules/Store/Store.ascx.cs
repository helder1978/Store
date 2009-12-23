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
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Media.
	/// </summary>
	public partial  class Store : PortalModuleBase
	{
        private StoreInfo storeInfo = null;
        private string parentTitle;
		private AdminNavigation adminNav;

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
			this.btnStoreInfo.Click += new EventHandler(btnStoreInfo_Click);
			this.btnCategories.Click += new EventHandler(btnCategories_Click);
			this.btnProducts.Click += new EventHandler(btnProducts_Click);
			this.btnCustomers.Click += new EventHandler(btnCustomers_Click);
			this.btnReviews.Click += new EventHandler(btnReviews_Click);
			this.btnHelp.Click += new EventHandler(btnHelp_Click);
		}
		#endregion

		#region Events
		protected void Page_Load(object sender, System.EventArgs e)
		{
            StoreController storeController = new StoreController();
            storeInfo = storeController.GetStoreInfo(PortalId);

            if (storeInfo == null)
            {
                CssTools.AddCss(this.Page, this.TemplateSourceDirectory, PortalId);
            }
            else
            {
                if (storeInfo.PortalTemplates)
                {
                    CssTools.AddCss(this.Page, PortalSettings.HomeDirectory + "Store", PortalId);
                }
                else
                {
                    CssTools.AddCss(this.Page, this.TemplateSourceDirectory, PortalId);
                }
            }

            adminNav = new AdminNavigation(Request.QueryString);

			lblParentTitle.Text = parentTitle;
            
            
            // canadean changed
            if(!PortalSecurity.IsInRole("Administrators"))
            {
                btnStoreInfo.Visible = false;
                lblSpacer1.Visible = false;
                //Response.Write("Not Administrator");
            }
            //else
            //    Response.Write("Administrator");

            if (adminNav.PageID == Null.NullString)
			{
				// Load the default control
				adminNav = new AdminNavigation();
                adminNav.PageID = "StoreAdmin";
                
                // canadean changed
                adminNav.PageID = "StoreAdmin";
                if (!PortalSecurity.IsInRole("Administrators"))
                    adminNav.PageID = "CustomerAdmin";

				loadAdminControl();
			}
			else
			{
				loadAdminControl();
			}
		}

		private void btnStoreInfo_Click(object sender, EventArgs e)
		{
			adminNav = new AdminNavigation();
			adminNav.PageID = "StoreAdmin";
			Response.Redirect(adminNav.GetNavigationUrl(), false);
		}

		private void btnCategories_Click(object sender, EventArgs e)
		{
			adminNav = new AdminNavigation();
			adminNav.PageID = "CategoryAdmin";
			Response.Redirect(adminNav.GetNavigationUrl(), false);
		}

		private void btnProducts_Click(object sender, EventArgs e)
		{
			adminNav = new AdminNavigation();
			adminNav.PageID = "ProductAdmin";
			Response.Redirect(adminNav.GetNavigationUrl(), false);
		}

		private void btnCustomers_Click(object sender, EventArgs e)
		{
			adminNav = new AdminNavigation();
			adminNav.PageID = "CustomerAdmin";
			Response.Redirect(adminNav.GetNavigationUrl(), false);
		}

		private void btnReviews_Click(object sender, EventArgs e)
		{
			adminNav = new AdminNavigation();
			adminNav.PageID = "ReviewAdmin";
			Response.Redirect(adminNav.GetNavigationUrl(), false);
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			adminNav = new AdminNavigation();
			adminNav.PageID = "HelpAdmin";
			Response.Redirect(adminNav.GetNavigationUrl(), false);
		}
		private void adminControl_EditComplete(object sender, EventArgs e)
		{
			Response.Redirect(adminNav.GetNavigationUrl(), false);
		}
		#endregion

		#region Private Functions
		private void loadAdminControl()
		{
			// TODO: We may want to use caching here
			StoreControlBase adminControl = (StoreControlBase)LoadControl(ModulePath + adminNav.PageID + ".ascx");
			adminControl.ParentControl = this as PortalModuleBase;
			adminControl.EditComplete += new EventHandler(adminControl_EditComplete);

			plhAdminControl.Controls.Clear();
			plhAdminControl.Controls.Add(adminControl);
		}
		#endregion

        #region Public Functions

        
        #endregion
    }
}
