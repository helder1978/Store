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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Catalog;

using System.Diagnostics;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial  class CategoryMenu : PortalModuleBase, IActionable
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
			this.MyList.ItemDataBound += new DataListItemEventHandler(MyList_ItemDataBound);
			this.MyList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.MyList_ItemCommand);

		}
		#endregion

		#region Private Declarations

        private StoreInfo storeInfo;
        private CatalogNavigation _nav;
		private ModuleSettings _settings;
        private int selectedCategoryID = 0;
        private int catalogTabId;
        private ArrayList parentCategories = new ArrayList();
        private System.Web.UI.HtmlControls.HtmlContainerControl br;

		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
                // Load utility objects
				_nav = new CatalogNavigation(Request.QueryString);
				_nav.ProductID = Null.NullInteger;	//Product should not be displayed!
                if (_nav.CategoryID == 0)
                {
                    _nav.CategoryID = Null.NullInteger;
                }

                //Get category and parent category data
                CategoryController categoryController = new CategoryController();
                selectedCategoryID = _nav.CategoryID;
                if (selectedCategoryID != Null.NullInteger)
                {
                    CategoryInfo category = categoryController.GetCategory(selectedCategoryID);
                    parentCategories.Add(category);
                    if (category.CategoryID != category.ParentCategoryID)
                    {
                        while (category.ParentCategoryID != Null.NullInteger)
                        {
                            category = categoryController.GetCategory(category.ParentCategoryID);
                            parentCategories.Add(category);
                            foreach (CategoryInfo cat in parentCategories)
                            {
                                if (cat.CategoryID == category.CategoryID)
                                {
                                    //Cyclical categories found
                                    break;
                                }
                            }
                        }
                    }
                }
                if (parentCategories.Count > 0)
                {
                    parentCategories.Reverse();
                }

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

				_settings = new ModuleSettings(this.ModuleId, this.TabId);
                // Databind to list of categories
				CategoryController controller = new CategoryController();
				ArrayList categoryList = controller.GetCategories(this.PortalId, false, -2);
				MyList.RepeatColumns = int.Parse(_settings.CategoryMenu.ColumnCount);
				MyList.DataSource = categoryList;
				MyList.DataBind();

				//MyList.SelectedIndex = GetSelectedIndex();
			}
			catch(Exception ex) 
			{
                if (ex.InnerException != null)
                {
                    Exceptions.ProcessModuleLoadException(this, ex.InnerException);
                }
                else
                {
                    string ErrorSettings = Localization.GetString("ErrorSettings", this.LocalResourceFile);
                    Exceptions.ProcessModuleLoadException(ErrorSettings, this, ex, true);
                }
			}
		}

		private void MyList_ItemDataBound(object sender, DataListItemEventArgs e)
		{
            _settings = new ModuleSettings(this.ModuleId, this.TabId);
            catalogTabId = int.Parse(_settings.CategoryMenu.CatalogPage);
            if (catalogTabId <= 0)
            {
                catalogTabId = storeInfo.StorePageID;
            }
			// Do we have the category info?
			CategoryInfo categoryInfo = (e.Item.DataItem as CategoryInfo);
			if (categoryInfo != null)
			{
				// Did we find the hyperlink?
				HyperLink linkCategory = (e.Item.FindControl("linkCategory") as HyperLink);
				if (linkCategory != null)
				{
					// Build the nav URL for this item
					StringDictionary replaceParams = new StringDictionary();
					replaceParams["CategoryID"] = categoryInfo.CategoryID.ToString();
					replaceParams["ProductID"] = Null.NullString;
                    replaceParams["PageIndex"] = Null.NullString;

                    linkCategory.NavigateUrl = _nav.GetNavigationUrl(replaceParams, catalogTabId);

                    if (selectedCategoryID == categoryInfo.CategoryID)
                    {
                        //Set styling...
                        linkCategory.CssClass = "Store-CategoryMenu-ItemSelected";
                    }

                    if (parentCategories.Count > 0) 
                    {
                        if (categoryInfo.CategoryID == ((CategoryInfo)parentCategories[0]).CategoryID)
                        {
                            //this is a root cat that needs to be expanded...
                            //DisplayChildCategories((CategoryInfo[])parentCategories.ToArray(typeof(CategoryInfo)), e.Item, storeInfo.StorePageID, 1);
                            DisplayChildCategories((CategoryInfo)parentCategories[0], e.Item, catalogTabId, 1);
                        }
                    }
				}
			}
		}

        private void DisplayChildCategories(CategoryInfo category, DataListItem dataListItem, int storePageID, int indentLevel)
        {
            CategoryController controller = new CategoryController();
            StringDictionary replaceParams = new StringDictionary();
            replaceParams["ProductID"] = Null.NullString;
            replaceParams["PageIndex"] = Null.NullString;

            ArrayList childCategories = controller.GetCategories(PortalId, false, category.CategoryID);
            foreach (CategoryInfo childCategory in childCategories)
            {
                replaceParams["CategoryID"] = childCategory.CategoryID.ToString();
                dataListItem.Controls.Add(GetBreakRow(indentLevel));
                dataListItem.Controls.Add(CreateHyperLink(childCategory.CategoryName, _nav.GetNavigationUrl(replaceParams, storePageID), selectedCategoryID == childCategory.CategoryID));

                //If this category is in the parent array, then recurse...
                foreach (CategoryInfo myCategory in parentCategories)
                {
                    if (myCategory.CategoryID == childCategory.CategoryID)
                    {
                        int newIndentLevel = indentLevel + 1;
                        DisplayChildCategories(childCategory, dataListItem, storePageID, newIndentLevel);
                    }
                }
            }
        }

        //private void DisplayChildCategories(CategoryInfo[] categories, DataListItem dataListItem, int storePageID, int indentLevel)
        //{
        //    CategoryController controller = new CategoryController();
        //    StringDictionary replaceParams = new StringDictionary();
        //    replaceParams["ProductID"] = Null.NullString;
        //    replaceParams["PageIndex"] = Null.NullString;

        //    foreach (CategoryInfo category in categories)
        //    {
        //        ArrayList childCategories = controller.GetCategories(PortalId, false, category.CategoryID);
        //        foreach (CategoryInfo childCategory in childCategories)
        //        {
        //            replaceParams["CategoryID"] = childCategory.CategoryID.ToString();
        //            dataListItem.Controls.Add(GetBreakRow(indentLevel));
        //            dataListItem.Controls.Add(CreateHyperLink(childCategory.CategoryName, _nav.GetNavigationUrl(replaceParams, storePageID), selectedCategoryID == childCategory.CategoryID));
        //        }
        //    }
        //}

        private HyperLink CreateHyperLink(string Text, string NavigateURL, bool SelectedCategory)
        {
            HyperLink link = new HyperLink();
            link.Text = Text;
            link.NavigateUrl = NavigateURL;

            if (SelectedCategory)
            {
                //Set styling...
                link.CssClass = "SelectedCategory";
            }

            return link;
        }

        private LiteralControl GetBreakRow(int indentationLevel)
        {
            System.Web.UI.LiteralControl literalControl = new LiteralControl();         
            literalControl.Text += "<br/>";
            for (int i = 1; i <= indentationLevel; i++)
            {
                literalControl.Text += GetIndentString();
            }   
            return literalControl;
        }

        private string GetIndentString()
        {
            return "&nbsp;&nbsp;&nbsp;";
        }

		private void MyList_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			// Get selected category
			_nav.CategoryID = int.Parse(e.CommandArgument.ToString());
            string url = "";

			// Build URL
			url = string.Empty;
			switch(e.CommandName.ToLower())
			{
				case "edit":
					url = EditUrl("CategoryID", _nav.CategoryID.ToString());
					break;
				default:
                    //url = DotNetNuke.Common.Globals.NavigateURL(105);
					break;
			}

			// Do we have a url to navigate?
			if (url != string.Empty)
			{
				Response.Redirect(url);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Searches the list of categories for the index of the selected category.
		/// </summary>
		/// <returns></returns>
		private int GetSelectedIndex()
		{
			int selectedIndex = -1;

			ArrayList categoryList = MyList.DataSource as ArrayList;
			if (categoryList != null)
			{
				for(int i = 0; i < categoryList.Count; i++)
				{
					CategoryInfo category = (categoryList[i] as CategoryInfo);
					if ((category != null) && (category.CategoryID == _nav.CategoryID))
					{
						selectedIndex = i;
						break;
					}
				}
			}

			return selectedIndex;
		}

		#endregion

		#region IActionable Members

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
		{
			get
			{
				ModuleActionCollection actions = new ModuleActionCollection();
				actions.Add(GetNextActionID(), Localization.GetString("AddNewCategory", this.LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("CategoryID", "0"), false, SecurityAccessLevel.Edit, true, false); 
				return actions; 
			}
		}

		#endregion
	}
}
