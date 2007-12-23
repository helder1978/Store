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
using System.Globalization;
using System.Data;
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
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Admin;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Store.
	/// </summary>
	public partial  class ProductAdmin : StoreControlBase
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
			this.cmbCategory.SelectedIndexChanged += new EventHandler(cmbCategory_SelectedIndexChanged);
			this.linkAddNew.Click += new EventHandler(linkAddNew_Click);
			this.linkAddImage.Click += new EventHandler(linkAddNew_Click);
			this.grdProducts.ItemDataBound += new DataGridItemEventHandler(grdProducts_ItemDataBound);
			this.grdProducts.PageIndexChanged += new DataGridPageChangedEventHandler(grdProducts_PageIndexChanged);
		}
		#endregion

		#region Private Declarations

        private StoreInfo storeInfo;
        private NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        private AdminNavigation _nav;

		#endregion

		#region Events
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (storeInfo == null)
            {
                StoreController storeController = new StoreController();
                storeInfo = storeController.GetStoreInfo(PortalId);
            }

            if (storeInfo.CurrencySymbol != string.Empty)
            {
                LocalFormat.CurrencySymbol = storeInfo.CurrencySymbol;
            }
            
			_nav = new AdminNavigation(Request.QueryString);
            // Do we show list or edit view?
            if (_nav.ProductID != Null.NullInteger)
            {
                ShowEditControl();
            }
            /*
            // canadean changed: Now we do it only on the PreRender, to allow filtering by the text entered
            else
            {
                ShowProductList();
            }*/

            //Response.Write("<br>Page_load: " + this.tbProductFilter.Text + " - " + _nav.ProductID);
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            //Response.Write("<br>Page_PreRender: " + this.tbProductFilter.Text + " - " + _nav.ProductID);
            // Do we show list or edit view?
            if (_nav.ProductID == Null.NullInteger)
            {
                ShowProductList();
            }
        }

		protected override void OnPreRender(EventArgs e)
		{
            // Set the title in the parent control

            Store store = (Store)parentControl;
			store.ParentTitle = lblParentTitle.Text;

			base.OnPreRender (e);

		}

		private void grdProducts_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			ProductInfo productInfo = e.Item.DataItem as ProductInfo;

			if (productInfo != null)
			{
                Label labelArchived = (e.Item.FindControl("labelArchived") as Label);
                if (labelArchived != null)
                {
                    labelArchived.Text = productInfo.Archived ? Localization.GetString("Yes", Localization.SharedResourceFile) : Localization.GetString("No", Localization.SharedResourceFile);
                }

                Label labelFeatured = (e.Item.FindControl("labelFeatured") as Label);
                if (labelFeatured != null)
                {
                    labelFeatured.Text = productInfo.Featured ? Localization.GetString("Yes", Localization.SharedResourceFile) : Localization.GetString("No", Localization.SharedResourceFile);
                }

                HyperLink linkEdit = (e.Item.FindControl("linkEdit") as HyperLink);
                if (linkEdit != null)
                {
	                // Update navURL using this item's ID
	                StringDictionary replaceParams = new StringDictionary();
	                replaceParams["ProductID"] = productInfo.ProductID.ToString();
	                linkEdit.NavigateUrl = _nav.GetNavigationUrl(replaceParams);
                }

                Label labelPrice = (e.Item.FindControl("labelPrice") as Label);
                if (labelPrice != null)
                {
                    labelPrice.Text = productInfo.UnitCost.ToString("C", LocalFormat);
                }
			}
		}

		private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			_nav.PageIndex = Null.NullInteger;
			_nav.CategoryID = int.Parse(cmbCategory.SelectedValue);
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void linkAddNew_Click(object sender, EventArgs e)
		{
			_nav.ProductID = 0;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void productEdit_EditComplete(object sender, EventArgs e)
		{
			_nav.ProductID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void grdProducts_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			_nav.PageIndex = e.NewPageIndex;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		#endregion

		#region Private Methods

		private void ShowProductList()
		{
			panelList.Visible = true;
			panelEdit.Visible = false;

			// Load category combo
            /*
            // Canadean changed: removed the dd display
			CategoryController categoryController = new CategoryController();
			ArrayList categoryList = categoryController.GetCategories(this.PortalId, true, -1);
			cmbCategory.DataSource = categoryList;
			cmbCategory.DataBind();

			// Load product grid
			if (cmbCategory.Items.Count > 0)
			{
				// Do we need to select an existing category?
				if (_nav.CategoryID != Null.NullInteger)
				{
					cmbCategory.SelectedValue = _nav.CategoryID.ToString();
				}
				else
				{
					cmbCategory.SelectedIndex = 0;
				}

				UpdateProductList();
			}
            */

            // canadean changed: added in replacement of the above
            if (_nav.CategoryID != Null.NullInteger)
            {
                UpdateProductList(_nav.CategoryID);
            }
            else
                UpdateProductList(4);   // CategoryId = 4 = Drink Type (to display reports only)

		}

        private void UpdateProductList(int categoryID)
		{
			grdProducts.DataSource = null;
            
			// Get current category
			//int categoryID = int.Parse(cmbCategory.SelectedValue);
			if (categoryID >= 0)
			{
				ProductController controller = new ProductController();
				//ArrayList productList = controller.GetCategoryProducts(categoryID, true);
                String searchTerm = this.tbProductFilter.Text;
                //ArrayList productList = controller.GetSearchedProducts(-1, searchTerm, true);
                bool archived = false;
                if (int.Parse(this.ddArchive.SelectedValue) == 1) archived = true;
                ArrayList productList = controller.GetSearchedProducts(-1, searchTerm, archived);

				if (productList.Count > 0)
				{
					// Has page index been initialized?
					if (_nav.PageIndex == Null.NullInteger)
					{
						_nav.PageIndex = 0;
					}

					// Update the grid
					grdProducts.PageSize = 20;
					grdProducts.AllowPaging = true;
					grdProducts.DataSource = productList;
					grdProducts.CurrentPageIndex = _nav.PageIndex;
					grdProducts.DataBind();
				}
			}
		}

		private void ShowEditControl()
		{
			panelList.Visible = false;
			panelEdit.Visible = true;

			// Inject the edit control
			ProductEdit productEdit = (ProductEdit)LoadControl(ModulePath + "ProductEdit.ascx");
			productEdit.DataSource = _nav.ProductID;
			productEdit.EditComplete += new EventHandler(productEdit_EditComplete);
			editControl.Controls.Add(productEdit);
		}

		#endregion
	}
}
