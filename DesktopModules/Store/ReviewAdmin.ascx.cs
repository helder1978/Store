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
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Admin;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Store.
	/// </summary>
	public partial class ReviewAdmin : StoreControlBase
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
			this.grdReviews.ItemDataBound += new DataGridItemEventHandler(grdReviews_ItemDataBound);
		}

		#endregion

		#region Private Declarations

		private AdminNavigation _nav;

		#endregion

		#region Events
		protected void Page_Load(object sender, EventArgs e)
		{
			_nav = new AdminNavigation(Request.QueryString);

			// Do we show list or edit view?
			if (_nav.ReviewID != Null.NullInteger)
			{
				ShowEditControl();
			}
			else
			{
				if (!IsPostBack)
				{
					FillStatusCombo();
					FillCategoryCombo();
					FillProductCombo();
					FillReviewGrid();
				}
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			// Set the title in the parent control
			Store store = (Store)parentControl;
			store.ParentTitle = lblParentTitle.Text;

			base.OnPreRender (e);
		}

		private void grdReviews_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			ReviewInfo reviewInfo = e.Item.DataItem as ReviewInfo;
			if (reviewInfo != null)
			{
				// Update edit link using this item's ID
				HyperLink linkEdit = (e.Item.FindControl("linkEdit") as HyperLink);
				if (linkEdit != null)
				{
					StringDictionary replaceParams = new StringDictionary();
					replaceParams["ReviewID"] = reviewInfo.ReviewID.ToString();
					linkEdit.NavigateUrl = _nav.GetNavigationUrl(replaceParams);
				}

				// Add rating images
				PlaceHolder phRating = (e.Item.FindControl("phRating") as PlaceHolder);
				if (phRating != null)
				{
					phRating.Controls.Add(GetRatingImages(reviewInfo.Rating));
				}
			}
		}

		protected void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
		{
			_nav.PageIndex = Null.NullInteger;
			_nav.StatusID = int.Parse(cmbStatus.SelectedValue);
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		protected void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			_nav.PageIndex = Null.NullInteger;
			_nav.CategoryID = int.Parse(cmbCategory.SelectedValue);
			_nav.ProductID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		protected void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
		{
            _nav.PageIndex = Null.NullInteger;
			_nav.ProductID = int.Parse(cmbProduct.SelectedValue);
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void linkAddNew_Click(object sender, EventArgs e)
		{
			_nav.ReviewID = 0;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void reviewEdit_EditComplete(object sender, EventArgs e)
		{
			_nav.ReviewID = Null.NullInteger;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		private void grdReviews_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			_nav.PageIndex = e.NewPageIndex;
			Response.Redirect(_nav.GetNavigationUrl(), false);
		}

		#endregion

		#region Private Methods

		private void FillReviewGrid()
		{
			panelList.Visible = true;
			panelEdit.Visible = false;

			grdReviews.DataSource = null;

			// Get the status filter
			ReviewController.StatusFilter filter = ReviewController.StatusFilter.All;
			if (cmbStatus.SelectedValue == "0")
			{
				filter = ReviewController.StatusFilter.NotApproved;
			}
			else if (cmbStatus.SelectedValue == "1")
			{
				filter = ReviewController.StatusFilter.Approved;
			}

			// Get the product list...
			ArrayList reviewList;
			ReviewController controller = new ReviewController();
			if (cmbProduct.SelectedValue != Null.NullInteger.ToString())
			{
				// Select by product
				reviewList = controller.GetReviewsByProduct(this.PortalId, int.Parse(cmbProduct.SelectedValue), filter);
			}
			else if (cmbCategory.SelectedValue != Null.NullInteger.ToString())
			{
				// Select by category
                reviewList = controller.GetReviewsByCategory(this.PortalId, int.Parse(cmbCategory.SelectedValue), filter);
			}
			else
			{
				// Select all reviews
				reviewList = controller.GetReviews(this.PortalId, filter);
			}

			// Has page index been initialized?
			if (_nav.PageIndex == Null.NullInteger)
			{
				_nav.PageIndex = 0;
			}

			// Update the grid
			grdReviews.PageSize = 20;
			grdReviews.AllowPaging = true;
			grdReviews.DataSource = reviewList;
			grdReviews.CurrentPageIndex = _nav.PageIndex;
			grdReviews.DataBind();
		}

		private void FillStatusCombo()
		{
			cmbStatus.Items.Clear();
			cmbStatus.Items.Add(new ListItem("--- " + Localization.GetString("All.Text") + " ---", Null.NullInteger.ToString()));
			cmbStatus.Items.Add(new ListItem(Localization.GetString("NotApproved", this.LocalResourceFile), "0"));
			cmbStatus.Items.Add(new ListItem(Localization.GetString("ApprovedOnly", this.LocalResourceFile), "1"));

            cmbStatus.SelectedValue = _nav.StatusID.ToString();
		}

		private void FillCategoryCombo()
		{
			CategoryController controller = new CategoryController();
			ArrayList categoryList = controller.GetCategories(this.PortalId, true, -1);
			cmbCategory.DataSource = categoryList;
			cmbCategory.DataBind();
			cmbCategory.Items.Insert(0, new ListItem("--- " + Localization.GetString("All.Text") + " ---", Null.NullInteger.ToString()));

			cmbCategory.SelectedValue = _nav.CategoryID.ToString();
		}

		private void FillProductCombo()
		{
			ProductController controller = new ProductController();
			ArrayList productList = null;

			string categoryID = cmbCategory.SelectedValue;
			if (categoryID == Null.NullInteger.ToString())
			{
				productList = controller.GetPortalProducts(this.PortalId, false, false);
			}
			else
			{
				productList = controller.GetCategoryProducts(int.Parse(categoryID), true);
			}

			cmbProduct.DataSource = productList;
			cmbProduct.DataBind();
			cmbProduct.Items.Insert(0, new ListItem("--- " + Localization.GetString("All.Text") + " ---", Null.NullInteger.ToString()));

			cmbProduct.SelectedValue = _nav.ProductID.ToString();			
		}

		private void ShowEditControl()
		{
			panelList.Visible = false;
			panelEdit.Visible = true;

			// Inject the edit control
			StoreControlBase reviewEdit = (StoreControlBase)LoadControl(ModulePath + "ReviewEdit.ascx");
			reviewEdit.ParentControl = this as PortalModuleBase;
			reviewEdit.DataSource = _nav.ReviewID;
			reviewEdit.EditComplete += new EventHandler(reviewEdit_EditComplete);

			editControl.Controls.Clear();
			editControl.Controls.Add(reviewEdit);
		}

		private Table GetRatingImages(int rating)
		{
			TableRow row = new TableRow();
			for(int i = 0; i < rating; i++)
			{
				Image image = new Image();
				image.ImageUrl = "~/images/ratingplus.gif";

				TableCell cell = new TableCell();
				cell.Controls.Add(image);

				row.Cells.Add(cell);
			}

			Table table = new Table();
			table.BorderWidth = 0;
			table.CellPadding = 0;
			table.CellSpacing = 0;
			table.Rows.Add(row);

			return table;
		}

		#endregion
	}
}
