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
using System.Globalization;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Providers.Tax;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Media.
	/// </summary>
	public partial  class ProductList : StoreControlBase
	{
		#region Private Declarations
		private CatalogNavigation moduleNav;
		private ProductInfo productInfo;
		private int categoryID = 0;
		private string templatesPath = "";
		private string imagesPath = "";
		private string title = "";
        private string containerTemplate = "";
		private string template = "";
		private int rowCount = 10;
		private int columnCount = 2;
		private int columnWidth = 200;
        private string direction = "";
		private bool showThumbnail = true;
		private int thumbnailWidth = 90;
		private int detailPageID = 0;
        private StoreInfo storeInfo;
        private NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        private ArrayList labelsPageInfo = new ArrayList();
        private ArrayList panelsPageNav = new ArrayList();
        private ArrayList buttonsPrevious = new ArrayList();
        private ArrayList buttonsNext = new ArrayList();
        private ArrayList placeholdersPageList = new ArrayList();
        private DataList lstProducts;
        #endregion

		#region Public Properties

		public int CategoryID
		{
			get {return categoryID;}
			set {categoryID = value;}
		}

		public string Title
		{
			get {return title;}
			set {title = value;}
		}

        public string ContainerTemplate
		{
            get { return containerTemplate; }
            set { containerTemplate = value; }
		}

		public string Template
		{
			get {return template;}
			set {template = value;}
		}

		public int RowCount
		{
			get {return rowCount;}
			set {rowCount = value;}
		}

		public int ColumnCount
		{
			get {return columnCount;}
			set {columnCount = value;}
		}

		public int ColumnWidth
		{
			get {return columnWidth;}
			set {columnWidth = value;}
		}

        public string Direction
		{
            get { return direction; }
            set { direction = value; }
		}

		public bool ShowThumbnail
		{
			get {return showThumbnail;}
			set {showThumbnail = value;}
		}
		
		public int ThumbnailWidth
		{
			get {return thumbnailWidth;}
			set {thumbnailWidth = value;}
		}

		public int DetailPage
		{
			get {return detailPageID;}
			set {detailPageID = value;}
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
            //this.SearchButton.Click += new System.Web.UI.ImageClickEventHandler(this.SearchButton_Click);
		}
		#endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
            if (storeInfo == null)
            {
                StoreController storeController = new StoreController();
                storeInfo = storeController.GetStoreInfo(PortalId);
                if (storeInfo.PortalTemplates)
                {
                    templatesPath = PortalSettings.HomeDirectoryMapPath + "Store\\";
                    imagesPath = PortalSettings.HomeDirectory + "Store/Templates/Images/";
                }
                else
                {
                    //templatesPath = MapPath(ModulePath) + "\\";
                    templatesPath = MapPath(ModulePath);
                    imagesPath = parentControl.ModulePath + "Templates/Images/";
                }
            }

            if (storeInfo.CurrencySymbol != string.Empty)
            {
                LocalFormat.CurrencySymbol = storeInfo.CurrencySymbol;
            }

            moduleNav = new CatalogNavigation(Request.QueryString);

            //0 indicates that no detail page is being used, so use current tabid
			if (this.DetailPage == 0) 
			{
				this.DetailPage = this.TabId;
			}
			moduleNav.TabId = this.DetailPage;

			if (moduleNav.PageIndex == Null.NullInteger)
			{
				moduleNav.PageIndex = 1;
			}

            if (containerTemplate == string.Empty)
            {
                this.Controls.Add(TemplateController.ParseTemplate(templatesPath, "ListContainer.htm", new ProcessTokenDelegate(processToken)));
            }
            else
            {
                this.Controls.Add(TemplateController.ParseTemplate(templatesPath, containerTemplate, new ProcessTokenDelegate(processToken)));
            }
            if (lstProducts != null) BindData();
        }

		private void lstProducts_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			productInfo = (ProductInfo)e.Item.DataItem;

			e.Item.Width = columnWidth;

			HtmlTable listTable = new HtmlTable();
			listTable.Border = 0;
			listTable.CellPadding = 0;
			listTable.CellSpacing = 0;
			listTable.Height = "100%";
			listTable.Width = columnWidth.ToString();

			HtmlTableRow listRow = new HtmlTableRow();

			HtmlTableCell listCell = new HtmlTableCell();
			listCell.Height = "100%";
			listCell.Width = columnWidth.ToString();

            ProductDetail productListItem = (ProductDetail)LoadControl(ModulePath + "ProductDetail.ascx");
            productListItem.Template = template;
            productListItem.ParentControl = this.ParentControl;
            productListItem.CategoryID = productInfo.CategoryID;
            productListItem.ShowThumbnail = ShowThumbnail;
            productListItem.ThumbnailWidth = ThumbnailWidth;
            productListItem.DataSource = productInfo;
            productListItem.DetailID = detailPageID;
            productListItem.InList = true;

            listCell.Controls.Add(productListItem);
            listRow.Controls.Add(listCell);
            listTable.Controls.Add(listRow);

			e.Item.Controls.Add(listTable);
		}

        //private void SearchButton_Click(object sender, ImageClickEventArgs e)
        //{
        //    if(SearchBox.Text == "")
        //    {
        //        return;
        //    }
        //}

		#endregion

		#region Protected Functions
		protected void BindData()
		{
			PagedDataSource pagedData = null;

			// Get the product data
			ArrayList productArray = (dataSource as ArrayList);
			if ((productArray == null) || (productArray.Count == 0))
			{
                foreach (Panel pnlPageNav in panelsPageNav)
                {
                    pnlPageNav.Visible = false;
                }
            }
			else
			{
				int itemCount		= productArray.Count;
				int pageSize		= rowCount * columnCount;
				int currentPage		= moduleNav.PageIndex - 1;	// Convert to zero-based index

                foreach (Panel pnlPageNav in panelsPageNav)
                {
                    pnlPageNav.Visible = true;
                }

				// Created paged data source
				pagedData = new PagedDataSource();
                if (ViewState["productArray"] != null && IsPostBack)
                {
                    pagedData.DataSource = (ArrayList)ViewState["productArray"];
                }
                else
                {
                    pagedData.DataSource = productArray;
                    ViewState["productArray"] = pagedData.DataSource;
                }
				pagedData.AllowPaging = true;
				pagedData.PageSize = pageSize;
				pagedData.CurrentPageIndex = currentPage;

				UpdatePagingControls(itemCount, pageSize, moduleNav.PageIndex);
			}

			// Databind with product list
            if (lstProducts != null)
            {
                switch (direction)
                {
                    case "H" :
                        lstProducts.RepeatDirection = RepeatDirection.Horizontal;
                        break;
                    case "V" :
                        lstProducts.RepeatDirection = RepeatDirection.Vertical;
                        break;
                    default :
                        lstProducts.RepeatDirection = RepeatDirection.Horizontal;
                        break;
                }
                lstProducts.RepeatColumns = columnCount;
                lstProducts.ItemStyle.VerticalAlign = VerticalAlign.Top;
                lstProducts.DataSource = pagedData;
                lstProducts.DataBind();

                if (lstProducts.Items.Count == 0)
                {
                    this.Visible = false;
                }
            }
        }

		protected void UpdatePagingControls(int itemCount, int pageSize, int currentPage)
		{
			StringDictionary replaceParams = new StringDictionary();

			// Get total pages
			int rem;
			int totalPages = Math.DivRem(itemCount, pageSize, out rem);
			if (rem > 0)
			{
				totalPages++;
			}

			// Hide and return if only one page
			if (totalPages == 1)
			{
                foreach (Panel pnlPageNav in panelsPageNav)
                {
                    pnlPageNav.Visible = false;
                }
                return;
			}

			////////////////////////////
			// Previous/Next Buttons

			int prevIndex = currentPage - 1;
			if ((prevIndex < 1) || (prevIndex > totalPages))
			{
				prevIndex = 1;
			}

			replaceParams["PageIndex"] = prevIndex.ToString();
            foreach (HyperLink btnPrev in buttonsPrevious)
            {
                btnPrev.NavigateUrl = moduleNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
            }

			int nextIndex = currentPage + 1;
			if (nextIndex >= totalPages)
			{
				nextIndex = totalPages;
			}

			replaceParams["PageIndex"] = nextIndex.ToString();
            foreach (HyperLink btnNext in buttonsNext)
            {
                btnNext.NavigateUrl = moduleNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
            }

			////////////////////////////
			// Page Index List

			// Determine page range to display
			int rangeMin = currentPage - 10;
			int rangeMax = currentPage + 10;

			if (rangeMin < 1)
			{
				rangeMax = rangeMax + Math.Abs(rangeMin) + 1;
				rangeMin = 1;
			}

			if (rangeMax >= totalPages)
			{
				rangeMin = rangeMin - (rangeMax - totalPages);
				if (rangeMin <= 1)
				{
					rangeMin = 1;
				}

				rangeMax = totalPages;
			}

            foreach (PlaceHolder phPageList in placeholdersPageList)
            {
                // Create link for each page
                for (int i = rangeMin; i <= rangeMax; i++)
                {
                    replaceParams["PageIndex"] = i.ToString();

                    if (i == currentPage)
                    {
                        HyperLink pageLink = new HyperLink();
                        pageLink.Text = i.ToString();
                        pageLink.CssClass = "NormalRed";

                        HyperLink pageLink2 = new HyperLink();
                        pageLink2.Text = i.ToString();
                        pageLink2.CssClass = "NormalRed";

                        phPageList.Controls.Add(pageLink);
                        phPageList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    }
                    else
                    {
                        HyperLink pageLink = new HyperLink();
                        pageLink.Text = i.ToString();
                        pageLink.NavigateUrl = moduleNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
                        pageLink.CssClass = "NormalRed";

                        HyperLink pageLink2 = new HyperLink();
                        pageLink2.Text = i.ToString();
                        pageLink2.NavigateUrl = moduleNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
                        pageLink2.CssClass = "NormalRed";

                        phPageList.Controls.Add(pageLink);
                        phPageList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    }

                }
            }

            foreach (Label lblPageInfo in labelsPageInfo)
            {
                lblPageInfo.Text = string.Format(Localization.GetString("PageInfo.Text", this.LocalResourceFile), currentPage, totalPages);
            }
        }
		#endregion

		#region Private Functions
		private Control processToken(string tokenName)
		{
			switch (tokenName)
			{
                case "LISTTITLE":
                    Label lblTitle = new Label();
                    lblTitle.CssClass = "NormalBold";
                    lblTitle.Text = title;
                    return lblTitle;

                case "PAGENAV":
                    HyperLink btnPrevious = new HyperLink();
                    btnPrevious.Text = Localization.GetString("Previous.Text", this.LocalResourceFile);
                    btnPrevious.CssClass = "NormalBold";
                    buttonsPrevious.Add(btnPrevious);

                    Literal lblSpace = new Literal();
                    lblSpace.Text = "&nbsp;&nbsp;";

                    HyperLink btnNext = new HyperLink();
                    btnNext.Text = Localization.GetString("Next.Text", this.LocalResourceFile);
                    btnNext.CssClass = "NormalBold";
                    buttonsNext.Add(btnNext);

                    PlaceHolder phPageList = new PlaceHolder();
                    placeholdersPageList.Add(phPageList);

                    Panel pnlPageNav = new Panel();
                    pnlPageNav.Controls.Add(btnPrevious);
                    pnlPageNav.Controls.Add(lblSpace);
                    pnlPageNav.Controls.Add(phPageList);
                    pnlPageNav.Controls.Add(btnNext);
                    panelsPageNav.Add(pnlPageNav);
                    return pnlPageNav;

                case "PAGEINFO":
                    Label lblPageInfo = new Label();
                    lblPageInfo.CssClass = "NormalBold";
                    lblPageInfo.Text = string.Format(Localization.GetString("PageInfo.Text", this.LocalResourceFile), 1, 1);
                    labelsPageInfo.Add(lblPageInfo);
                    return lblPageInfo;

                case "PRODUCTS":
                    lstProducts = new DataList();
                    lstProducts.CellPadding = 0;
                    lstProducts.CellSpacing = 5;
                    lstProducts.HorizontalAlign = HorizontalAlign.Center;
                    lstProducts.RepeatColumns = columnCount;
                    lstProducts.ItemDataBound += new DataListItemEventHandler(lstProducts_ItemDataBound);
                    return lstProducts;

				default:
					LiteralControl litText = new LiteralControl(tokenName);
					return litText;
			}
		}
		#endregion
	}
}
