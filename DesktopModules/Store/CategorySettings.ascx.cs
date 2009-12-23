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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Modules.Store.Catalog;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial  class CategorySettings : ModuleSettingsBase
	{
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);

			// Load utility object
			_settings = new ModuleSettings(this.ModuleId, this.TabId);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		#region Private Members

		private ModuleSettings _settings;
		
		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (!Page.IsPostBack)
				{
				}
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
		#endregion

		#region ModuleSettingsBase Implementation

		public override void LoadSettings()
		{
			try
			{
				if (!Page.IsPostBack)
				{
					// Fill Catalog Page combo
					TabController tabController = new TabController();
					ArrayList tabs = tabController.GetTabs(PortalId);

					cmbCatalogPage.Items.Add(new ListItem(Localization.GetString("SamePage", this.LocalResourceFile), "0"));

					foreach (TabInfo tabInfo in tabs)
					{
						if (tabInfo.IsVisible && !tabInfo.IsDeleted && !tabInfo.IsAdminTab && !tabInfo.IsSuperTab)
						{
							cmbCatalogPage.Items.Add(new ListItem(tabInfo.TabName, tabInfo.TabID.ToString()));
						}
					}

					// Get values from settings
					txtColumnCount.Text = _settings.CategoryMenu.ColumnCount;

					int catalogTabID = int.Parse(_settings.CategoryMenu.CatalogPage);
					if (catalogTabID <= 0)
					{
						cmbCatalogPage.SelectedIndex = 0;
					}
					else
					{
						cmbCatalogPage.SelectedValue = catalogTabID.ToString();
					}
				}
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		public override void UpdateSettings()
		{
			try
			{
				PortalSecurity security = new PortalSecurity();

				_settings.CategoryMenu.ColumnCount = security.InputFilter(txtColumnCount.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
				_settings.CategoryMenu.CatalogPage = cmbCatalogPage.SelectedValue;
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion
	}
}
