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
using System.Collections.Specialized;
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
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CategoryAdmin.
	/// </summary>
	public partial  class CategoryAdmin : StoreControlBase
	{
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
			this.linkAddNew.Click += new EventHandler(linkAddNew_Click);
			this.linkAddImage.Click += new EventHandler(linkAddNew_Click);
			this.grdCategories.ItemDataBound += new DataGridItemEventHandler(grdCategories_ItemDataBound);
			this.grdCategories.PageIndexChanged += new DataGridPageChangedEventHandler(grdCategories_PageIndexChanged);
		}
		#endregion

		#region Private Declarations

		private AdminNavigation _nav;

		#endregion

		#region Events
		protected void Page_Load(object sender, System.EventArgs e)
		{

			_nav = new AdminNavigation(Request.QueryString);

          
            if (_nav.CategoryID != Null.NullInteger)
            {
                plhGrid.Visible = false;
                plhForm.Visible = true;
                
                if (_nav.CategoryID == 0)
                {
                    lblEditTitle.Text = Localization.GetString("AddCategory", this.LocalResourceFile);
                    loadEditControl("CategoryEdit.ascx", _nav.CategoryID);
                }
                else
                {
                    lblEditTitle.Text = Localization.GetString("EditCategory", this.LocalResourceFile);
                    loadEditControl("CategoryEdit.ascx", _nav.CategoryID);
                }
            }
            else
            {
                plhGrid.Visible = true;
                plhForm.Visible = false;

                CategoryController categoryController = new CategoryController();
                ArrayList categoryList = categoryController.GetCategoriesPath(PortalId, false, -1);

                if (categoryList.Count > 0)
                {
                    // Has page index been initialized?
                    if (_nav.PageIndex == Null.NullInteger)
                    {
                        _nav.PageIndex = 0;
                    }

                    // Update the grid
                    grdCategories.PageSize = 20;
                    grdCategories.AllowPaging = true;
                    grdCategories.DataSource = categoryList;
                    grdCategories.CurrentPageIndex = _nav.PageIndex;
                    grdCategories.DataBind();
                }
            }
		}

		protected override void OnPreRender(EventArgs e)
		{
			// Set the title in the parent control
			Store storeAdmin = (Store)parentControl;
			storeAdmin.ParentTitle = lblParentTitle.Text;            
			base.OnPreRender (e);
		}


		private void grdCategories_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			CategoryInfo categoryInfo = e.Item.DataItem as CategoryInfo;
			if (categoryInfo != null)
			{
                /*
                Label lblArchived = (e.Item.FindControl("lblArchived") as Label);
                if (lblArchived != null)
                {
                    lblArchived.Text = categoryInfo.Archived ? Localization.GetString("Yes", Localization.SharedResourceFile) : Localization.GetString("No", Localization.SharedResourceFile);
                }
                */
                Label lblParentCategory = (e.Item.FindControl("lblParentCategory") as Label);
                if (categoryInfo.ParentCategoryID > 0)
                {
                    lblParentCategory.Text = categoryInfo.ParentCategoryName;
                }
                else
                {
                    lblParentCategory.Text = Localization.GetString("ParentCategoryNone", this.LocalResourceFile);
                }

                HyperLink linkEdit = (HyperLink)e.Item.FindControl("linkEdit");
			    if (linkEdit != null)
			    {
				    // Update navURL using this item's ID
				    StringDictionary replaceParams = new StringDictionary();
				    replaceParams["CategoryID"] = categoryInfo.CategoryID.ToString();
				    linkEdit.NavigateUrl = _nav.GetNavigationUrl(replaceParams);
			    }
			}
		}

		private void grdCategories_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			_nav.PageIndex = e.NewPageIndex;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void linkAddNew_Click(object sender, EventArgs e)
		{
            //throw new NotImplementedException("HGFHGFH3");
			_nav.CategoryID = 0;
            StringDictionary replaceParams = new StringDictionary();
            replaceParams["CategoryID"] = "0";
			Response.Redirect(_nav.GetNavigationUrl(replaceParams));
		}

		private void editControl_EditComplete(object sender, EventArgs e)
		{
			_nav.CategoryID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}
		#endregion

		#region Private Function
		private void loadEditControl(string filename, int categoryID)
		{
			plhEditControl.Controls.Clear();

			// TODO: We may want to use caching here
			StoreControlBase editControl = (StoreControlBase)LoadControl(ModulePath + filename);
			editControl.ParentControl = this as PortalModuleBase;
			editControl.DataSource = categoryID;
			editControl.EditComplete += new EventHandler(editControl_EditComplete);

			plhEditControl.Controls.Add(editControl);
		}
		#endregion
	}
}
