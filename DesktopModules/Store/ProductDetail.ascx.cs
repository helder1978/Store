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
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Providers.Tax;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Media.
	/// </summary>
	public partial  class ProductDetail : StoreControlBase
	{
		#region Private Declarations
        private string template = "";
        private CatalogNavigation catalogNav;
		private ProductInfo productInfo = null;
		private int categoryID = 0;
		private int productID = 0;
		private bool showThumbnail = true;
		private int thumbnailWidth = 90;
		private bool showReviews = false;
        private int detailID = 0;
        private bool inList = false;
        private decimal defaultTaxRate;
        private bool showTax;
        private string templatesPath = "";
        private string imagesPath = "";
        private string virtualtemplatesPath = "";
        private StoreInfo storeInfo;
        private NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();

        private string ControlIdQty;
        //protected Label labelLog;
        #endregion

		#region Public Properties
        public string Template
        {
            get { return template; }
            set { template = value; }
        }
        
        public int CategoryID
		{
			get {return categoryID;}
			set {categoryID = value;}
		}

		public int ProductID
		{
			get {return productID;}
			set {productID = value;}
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

        public bool ShowReviews
		{
            get { return showReviews; }
            set { showReviews = value; }
		}

        public int DetailID
        {
            get { return detailID; }
            set { detailID = value; }
        }

        public bool InList
        {
            get { return inList; }
            set { inList = value; }
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
            //readParams();

            //labelLog.Text = labelLog.Text + "<br>xxxx";

            Catalog parent = (Catalog)parentControl;
            catalogNav = new CatalogNavigation(Request.QueryString);

            ITaxProvider taxProvider = StoreController.GetTaxProvider(ModulePath);
            ITaxInfo taxInfo = (ITaxInfo)taxProvider.GetDefautTaxRates(this.PortalId);
            defaultTaxRate = taxInfo.DefaultTaxRate;
            showTax = taxInfo.ShowTax;

            if (storeInfo == null)
            {
                StoreController storeController = new StoreController();
                storeInfo = storeController.GetStoreInfo(PortalId);
                if (storeInfo.PortalTemplates)
                {
                    templatesPath = PortalSettings.HomeDirectoryMapPath + "Store\\";
                    imagesPath = PortalSettings.HomeDirectory + "Store/Templates/Images/";
                    virtualtemplatesPath = PortalSettings.HomeDirectory + "Store/Templates/";
                }
                else
                {
                    //templatesPath = MapPath(parent.ModulePath) + "\\";
                    templatesPath = MapPath(parent.ModulePath);
                    imagesPath = parentControl.ModulePath + "Templates/Images/";
                    virtualtemplatesPath = parentControl.ModulePath + "Templates/";
                }
            }

            if (storeInfo.CurrencySymbol != string.Empty)
            {
                LocalFormat.CurrencySymbol = storeInfo.CurrencySymbol;
            }

            productInfo = (dataSource as ProductInfo);
            if (productInfo != null)
            {
                
                productID = productInfo.ProductID;
                if (template == "") template = parent.ModuleSettings.ProductDetail.Template;
                plhDetails.Controls.Add(TemplateController.ParseTemplate(templatesPath, template, new ProcessTokenDelegate(processToken)));

                if (catalogNav.IsTOC != Null.NullInteger)
                {
                    ((DotNetNuke.Framework.CDefault)this.Page).Title = "TOC: " + productInfo.ModelName + " - Industry Market Research Reports & Data";
                    ((DotNetNuke.Framework.CDefault)this.Page).Description = productInfo.DescriptionTag;
                }
                else
                {
                    //this.Page.Title = productInfo.ModelName;
                    ((DotNetNuke.Framework.CDefault)this.Page).Title = productInfo.ModelName + " - Industry Market Research Reports & Data";
                    ((DotNetNuke.Framework.CDefault)this.Page).Description = productInfo.DescriptionTag;

                }
                ((DotNetNuke.Framework.CDefault)this.Page).KeyWords = cleanHTMLTags(productInfo.Summary);
                //this.Page.dekeyTitle = 


                // Show info about the related products
                loadRelatedProducts();

            }

            ArrayList products = new ArrayList();
            products.Add(dataSource);

			String productId = productInfo.ProductID.ToString();
			String cat1 = productInfo.CategoryID1.ToString();
			String cat2 = productInfo.CategoryID2.ToString();
			String cat3 = productInfo.CategoryID3.ToString();
			String title = productInfo.ModelName.ToString();
			if (catalogNav.IsTOC != Null.NullInteger)
				lnkReturn.NavigateUrl = FixHyperLink(productId, cat1, cat2, cat3, title);
			else
				lnkReturn.NavigateUrl = FixHyperLinkCategory(cat1, cat2, cat3);
			
			//lnkReturn.NavigateUrl = GetReturnUrl(int.Parse(parent.ModuleSettings.ProductDetail.ReturnPage));
			
            // Show review panel ?
            if (ShowReviews && !InList)
            {
                // Show review list or edit?
                if (catalogNav.ReviewID != Null.NullInteger)
                {
                    loadReviewEditControl();
                }
                else
                {
                    loadReviewListControl();
                }
                //pnlReviews.Visible = true;
                pnlReviews.Visible = false; // canadean changed: never show reviews
            }
            else
            {
                pnlReviews.Visible = false;
            }

            pnlReturn.Visible = !InList;
            //Response.Write("<br>-------------------------------------------------<br>clientId: " + ControlIdQty);
        }
        /*
        private Control getControl(string controlId)
        {
            labelLog.Text = labelLog.Text + "<br>Getting controls";
            foreach (Control ctl in plhDetails.Controls)
            {
                labelLog.Text = labelLog.Text + "<br>" + ctl.ID + " " + ctl.ClientID;
                if (ctl.ClientID == controlId)
                    return ctl;
            }
            return null;
        }
        */

        private string cleanHTMLTags(string str)
        {
            
            str = str.Replace("&lt;p&gt;", " ");
            str = str.Replace("&lt;br /&gt;", " ");
            str = str.Replace("<br>", " ");
            str = str.Replace("&lt;/p&gt;", " ");
            str = str.Replace("&lt;div&gt;", " ");
            str = str.Replace("&lt;/div&gt;", " ");
            str = str.Replace("&amp;nbsp;", " ");
            str = str.Replace("     ", " ");
            str = str.Replace("  ", " ");
            str = str.Replace("  ", " ");
            str = str.Replace("  ", " ");
            str = str.Replace("  ", " ");
            str = str.Trim();

            /*
            str = str.Replace("&amp;lt;p&amp;gt;", " ");
            str = str.Replace("&amp;lt;br /&amp;gt;", " ");
            str = str.Replace("&lt; br >", " ");
            str = str.Replace("&amp;lt;/p&amp;gt;", " ");
             * */
            return str;
        }

        private string getControlId(string controlStr)
        {
            foreach (string variavel in Request.Form)
            {
                if (variavel.IndexOf(controlStr) != -1)
                    return variavel;
            }

            return "";
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            LinkButton button = (sender as LinkButton);
            /*TextBox tbQty = (TextBox)getControl("tbQty");
            int quantity = 1;
            if (tbQty != null)
                quantity = int.Parse(tbQty.Text);
            */
            //int quantity = int.Parse(Request.Form[getControlId("tbQuantity")]);
            //int deliveryMethod = int.Parse(Request.Form[getControlId("ddDelivery")]);
            int quantity = 1;
            int deliveryMethod = 1; // PDF

            if (button != null)
            {
                AddToCart(int.Parse(button.Attributes["ProductID"]), quantity, deliveryMethod);
            }
        }

        private void btnAddToCartImg_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            /*TextBox tbQty = (TextBox)getControl("tbQty");
            int quantity = 1;
            if (tbQty != null)
                quantity = int.Parse(tbQty.Text);
            */
            //int quantity = int.Parse(Request.Form[getControlId("tbQuantity")]);
            //int deliveryMethod = int.Parse(Request.Form[getControlId("ddDelivery")]);
            int quantity = 1;
            int deliveryMethod = 1; // PDF

            
            if (button != null)
            {
                AddToCart(int.Parse(button.Attributes["ProductID"]), quantity, deliveryMethod);
            }
        }

        private void AddToCart(int productID, int quantity, int deliveryMethod)
        {
            ProductController productController = new ProductController();
            ProductInfo product = productController.GetProduct(productID);
            CurrentCart.AddItem(PortalId, productID, quantity, deliveryMethod, product.ModelNumber, product.ModelName, product.UnitCost * quantity);
            Response.Redirect(catalogNav.GetNavigationUrl(), false);
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            LinkButton button = (sender as LinkButton);
            if (button != null)
            {
                Purchase(int.Parse(button.Attributes["ProductID"]));
            }
        }

        private void btnPurchaseImg_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            if (button != null)
            {
                Purchase(int.Parse(button.Attributes["ProductID"]));
            }
        }

        private void Purchase(int productID)
        {
            CurrentCart.AddItem(PortalId, productID, 1);
            CartNavigation cartnav = new CartNavigation();
            StoreController storeController = new StoreController();
            StoreInfo storeInfo = storeController.GetStoreInfo(PortalId);
            cartnav.TabId = storeInfo.ShoppingCartPageID;
            Response.Redirect(cartnav.GetNavigationUrl(), false);
        }

        private void btnEdit_Click(object sender, EventArgs e)
		{
			LinkButton button = (sender as LinkButton);
			if (button != null)
			{
				Response.Redirect(parentControl.EditUrl("ProductID", button.CommandArgument.ToString()));
			}
		}

        private void btnLinkDetailImg_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            if (button != null)
            {
                StringDictionary rplcLinkImg = new StringDictionary();
                rplcLinkImg.Add("ProductID", int.Parse(button.Attributes["ProductID"]).ToString());
                Response.Redirect(catalogNav.GetNavigationUrl(rplcLinkImg, detailID));
            }
        }

        private void reviewList_EditComplete(object sender, EventArgs e)
		{
			Response.Redirect(catalogNav.GetNavigationUrl(), false);
		}

		private void reviewEdit_EditComplete(object sender, EventArgs e)
		{
			catalogNav.ReviewID = Null.NullInteger;
			Response.Redirect(catalogNav.GetNavigationUrl(), false);
		}
		#endregion

		#region Protected Functions
        protected string GetReturnUrl(int TabId)
        {
            string url = string.Empty;

            ArrayList settingList = new ArrayList();

            if (catalogNav.CategoryID > -1)
            {
                settingList.Add("CategoryID=" + catalogNav.CategoryID.ToString());
            }
            settingList.Add("PageIndex=" + catalogNav.PageIndex.ToString());

            if (TabId == 0)
            {
                url = Globals.NavigateURL(parentControl.TabId, "", settingList.ToArray(typeof(string)) as string[]);
            }
            else
            {
                url = Globals.NavigateURL(TabId, "", settingList.ToArray(typeof(string)) as string[]);
            }

            return url;
        }

		protected string GetCartUrl(int productID)
		{
			string url = "";

			return url;
		}

		protected string GetImageUrl(string image) 
		{ 
			return parentControl.ModulePath + "Thumbnail.aspx?IP=" + image + "&IW=" + thumbnailWidth; 
		}
		#endregion

		#region Private Functions
		private void loadReviewListControl()
		{
			// TODO: We may want to use caching here
			StoreControlBase reviewList = (StoreControlBase)LoadControl(ModulePath + "ReviewList.ascx");
			reviewList.ParentControl = this as PortalModuleBase;
			reviewList.EditComplete += new EventHandler(reviewList_EditComplete);

			plhReviews.Controls.Clear();
			plhReviews.Controls.Add(reviewList);
		}

		private void loadReviewEditControl()
		{
			// Inject the edit control
			StoreControlBase reviewEdit = (StoreControlBase)LoadControl(ModulePath + "ReviewEdit.ascx");
			reviewEdit.ParentControl = this as PortalModuleBase;
			reviewEdit.DataSource = catalogNav.ReviewID;
			reviewEdit.EditComplete += new EventHandler(reviewEdit_EditComplete);

			plhReviews.Controls.Clear();
			plhReviews.Controls.Add(reviewEdit);
		}

		private Control processToken(string tokenName)
		{
			switch (tokenName)
			{
				case "IMAGE":
                    if (String.IsNullOrEmpty(productInfo.ProductImage))
                    {
                        Literal litImage = new Literal();
                        litImage.Text = String.Empty;
                        return litImage;
                    }
                    else
                    {
                        if (InList)
                        {
                            StringDictionary replacements2 = new StringDictionary();
                            replacements2.Add("ProductID", productInfo.ProductID.ToString());

                            HyperLink lnkImage = new HyperLink();
                            lnkImage.NavigateUrl = catalogNav.GetNavigationUrl(replacements2, detailID);
                            lnkImage.ToolTip = productInfo.Summary;
                            lnkImage.BorderStyle = BorderStyle.None;
                            lnkImage.Visible = ShowThumbnail;

                            HtmlImage image = new HtmlImage();
                            image.Src = GetImageUrl(productInfo.ProductImage);
                            image.Border = 1;
                            image.Style.Add("Border-Color", "#000");
                            image.Alt = productInfo.Summary;
                            image.Width = thumbnailWidth;

                            lnkImage.Controls.Add(image);
                            return lnkImage;
                        }
                        else
                        {
                            Image imgProduct = new Image();
                            imgProduct.ImageUrl = GetImageUrl(productInfo.ProductImage);
                            imgProduct.Visible = showThumbnail;
                            imgProduct.BorderColor = System.Drawing.Color.Black;
                            imgProduct.BorderStyle = BorderStyle.Solid;
                            imgProduct.BorderWidth = Unit.Pixel(1);
                            imgProduct.AlternateText = productInfo.Summary;
                            return imgProduct;
                        }
                    }
				case "EDIT":
					LinkButton btnEdit = new LinkButton();
					btnEdit.Visible = parentControl.IsEditable;
					btnEdit.CommandArgument = productInfo.ProductID.ToString();
					btnEdit.Click += new EventHandler(btnEdit_Click);

					Image imgEdit = new Image();
					imgEdit.ImageUrl = "~/images/Edit.gif";
					imgEdit.Visible = parentControl.IsEditable;

					btnEdit.Controls.Add(imgEdit);
					return btnEdit;

                case "MANUFACTURER":
                    Label lblManufacturer = new Label();
                    lblManufacturer.Text = productInfo.Manufacturer;
                    return lblManufacturer;

                case "MODELNUMBER":
                    Label lblModelNumber = new Label();
                    lblModelNumber.Text = productInfo.ModelNumber;
                    return lblModelNumber;

                case "MODELNAME":
                    Label lblModelName = new Label();
                    lblModelName.Text = productInfo.ModelName;
                    return lblModelName;

                case "TITLE":
                    if (InList)
                    {
                        StringDictionary rplcTitle = new StringDictionary();
                        rplcTitle.Add("ProductID", productInfo.ProductID.ToString());

                        HyperLink lnkTitle = new HyperLink();
                        lnkTitle.Text = productInfo.ProductTitle;
                        lnkTitle.NavigateUrl = catalogNav.GetNavigationUrl(rplcTitle, detailID);
                        lnkTitle.CssClass = "NormalBold";
                        return lnkTitle;
                    }
                    else
                    {
                        Label lblTitle = new Label();
                        lblTitle.Text = productInfo.ProductTitle;
                        return lblTitle;
                    }

                case "LINKDETAIL":
                    StringDictionary rplcLink = new StringDictionary();
                    rplcLink.Add("ProductID", productInfo.ProductID.ToString());

                    HyperLink lnkDetail = new HyperLink();
                    lnkDetail.Text = Localization.GetString("LinkDetail", this.LocalResourceFile);
                    lnkDetail.NavigateUrl = catalogNav.GetNavigationUrl(rplcLink, detailID);
                    lnkDetail.CssClass = "NormalBold";
                    return lnkDetail;

                case "LINKDETAILIMG":
                    ImageButton btnLinkDetailImg = new ImageButton();
                    btnLinkDetailImg.ImageUrl = imagesPath + "linkdetailimg.gif";
                    btnLinkDetailImg.ToolTip = Localization.GetString("LinkDetail", this.LocalResourceFile);
                    btnLinkDetailImg.CommandArgument = productInfo.ProductID.ToString();
                    btnLinkDetailImg.Click += new ImageClickEventHandler(btnLinkDetailImg_Click);
                    btnLinkDetailImg.Attributes.Add("ProductID", productInfo.ProductID.ToString());
                    return btnLinkDetailImg;

                case "SUMMARY":
					Label lblSummary = new Label();
					lblSummary.Text = productInfo.Summary;
					return lblSummary;

                case "WEIGHT":
                    Label lblWeight = new Label();
                    lblWeight.Text = string.Format(Localization.GetString("WeightText", this.LocalResourceFile), productInfo.ProductWeight.ToString(Localization.GetString("WeightFormat", this.LocalResourceFile), LocalFormat));
                    return lblWeight;

                case "HEIGHT":
                    Label lblHeight = new Label();
                    lblHeight.Text = string.Format(Localization.GetString("HeightText", this.LocalResourceFile), productInfo.ProductHeight.ToString(Localization.GetString("HeightFormat", this.LocalResourceFile), LocalFormat));
                    return lblHeight;

                case "LENGTH":
                    Label lblLength = new Label();
                    lblLength.Text = string.Format(Localization.GetString("LengthText", this.LocalResourceFile), productInfo.ProductLength.ToString(Localization.GetString("LengthFormat", this.LocalResourceFile), LocalFormat));
                    return lblLength;

                case "WIDTH":
                    Label lblWidth = new Label();
                    lblWidth.Text = string.Format(Localization.GetString("WidthText", this.LocalResourceFile), productInfo.ProductWidth.ToString(Localization.GetString("WidthFormat", this.LocalResourceFile), LocalFormat));
                    return lblWidth;

                case "SURFACE":
                    Label lblSurface = new Label();
                    decimal dblSurface = productInfo.ProductLength * productInfo.ProductWidth;
                    lblSurface.Text = string.Format(Localization.GetString("SurfaceText", this.LocalResourceFile), dblSurface.ToString(Localization.GetString("SurfaceFormat", this.LocalResourceFile), LocalFormat));
                    return lblSurface;

                case "VOLUME":
                    Label lblVolume = new Label();
                    decimal dblVolume = productInfo.ProductHeight * productInfo.ProductLength * productInfo.ProductWidth;
                    lblVolume.Text = string.Format(Localization.GetString("VolumeText", this.LocalResourceFile), dblVolume.ToString(Localization.GetString("VolumeFormat", this.LocalResourceFile), LocalFormat));
                    return lblVolume;

                case "DIMENSIONS":
                    Label lblDimensions = new Label();
                    string strHeight = productInfo.ProductHeight.ToString(Localization.GetString("HeightFormat", this.LocalResourceFile), LocalFormat);
                    string strLength = productInfo.ProductLength.ToString(Localization.GetString("LengthFormat", this.LocalResourceFile), LocalFormat);
                    string strWidth = productInfo.ProductWidth.ToString(Localization.GetString("WidthFormat", this.LocalResourceFile), LocalFormat);
                    lblDimensions.Text = string.Format(Localization.GetString("DimensionsText", this.LocalResourceFile), strHeight, strLength, strWidth);
                    return lblDimensions;

                case "PRICE":
                    Label lblPrice = new Label();
                    if ((productInfo.PriceStr == Null.NullString) || (productInfo.PriceStr == ""))
                    {
                        if (productInfo.SalePrice != Null.NullDecimal
                            && System.DateTime.Now > productInfo.SaleStartDate && System.DateTime.Now < productInfo.SaleEndDate)
                        {
                            //Product is on sale...
                            lblPrice.Text = string.Format(Localization.GetString("SpecialOffer", this.LocalResourceFile), productInfo.UnitCost.ToString("C", LocalFormat), productInfo.SalePrice.ToString("C", LocalFormat));
                        }
                        else
                        {
                            lblPrice.Text = string.Format(Localization.GetString("Price", this.LocalResourceFile), productInfo.UnitCost.ToString("C", LocalFormat));
                        }
                    }
                    else
                        lblPrice.Text = productInfo.PriceStr;


                    return lblPrice;

				case "VATPRICE":
					Label lblVATPrice = new Label();
                    if (showTax)
                    {
                        if (productInfo.SalePrice != Null.NullDecimal
                            && System.DateTime.Now > productInfo.SaleStartDate && System.DateTime.Now < productInfo.SaleEndDate)
                        {
                            decimal dblVATPrice = (productInfo.UnitCost + (productInfo.UnitCost * (defaultTaxRate / 100)));
                            decimal dblVATSalePrice = (productInfo.SalePrice + (productInfo.SalePrice * (defaultTaxRate / 100)));
                            //Product is on sale...
                            lblVATPrice.Text = string.Format(Localization.GetString("SpecialOffer", this.LocalResourceFile), dblVATPrice.ToString("C", LocalFormat), dblVATSalePrice.ToString("C", LocalFormat));
                        }
                        else
                        {
                            lblVATPrice.Text = string.Format(Localization.GetString("VATPrice", this.LocalResourceFile), (productInfo.UnitCost + (productInfo.UnitCost * (defaultTaxRate / 100))).ToString("C", LocalFormat));
                        }
                    }
                    return lblVATPrice;

                case "PURCHASE":
                    LinkButton btnPurchase = new LinkButton();
                    btnPurchase.Text = Localization.GetString("Purchase", this.LocalResourceFile);
                    btnPurchase.CommandArgument = productInfo.ProductID.ToString();
                    btnPurchase.Click += new EventHandler(btnPurchase_Click);
                    btnPurchase.Attributes.Add("ProductID", productInfo.ProductID.ToString());
                    return btnPurchase;

                case "PURCHASEIMG":
                    ImageButton btnPurchaseImg = new ImageButton();
                    btnPurchaseImg.ImageUrl = imagesPath + "purchaseimg.gif";
                    btnPurchaseImg.ToolTip = Localization.GetString("Purchase", this.LocalResourceFile);
                    btnPurchaseImg.CommandArgument = productInfo.ProductID.ToString();
                    btnPurchaseImg.Click += new ImageClickEventHandler(btnPurchaseImg_Click);
                    btnPurchaseImg.Attributes.Add("ProductID", productInfo.ProductID.ToString());
                    return btnPurchaseImg;

                case "ADDTOCART":
                    LinkButton btnAddToCart = new LinkButton();
                    btnAddToCart.Text = Localization.GetString("AddToCart", this.LocalResourceFile);
                    btnAddToCart.CommandArgument = productInfo.ProductID.ToString();
                    btnAddToCart.Click += new EventHandler(btnAddToCart_Click);
                    btnAddToCart.Attributes.Add("ProductID", productInfo.ProductID.ToString());
                    return btnAddToCart;

                case "ADDTOCARTIMG":
                    ImageButton btnAddToCartImg = new ImageButton();
                    btnAddToCartImg.ImageUrl = imagesPath + "addtocartimg.gif";
                    btnAddToCartImg.ToolTip = Localization.GetString("AddToCart", this.LocalResourceFile);
                    btnAddToCartImg.CommandArgument = productInfo.ProductID.ToString();
                    btnAddToCartImg.Click += new ImageClickEventHandler(btnAddToCartImg_Click);
                    btnAddToCartImg.Attributes.Add("ProductID", productInfo.ProductID.ToString());
                    return btnAddToCartImg;

                case "DETAILPDFIMG":
                    if ((productInfo.ProductPreview != "") && (!productInfo.ProductPreview.Contains("_no_report_file_available.pdf")))
                    {
                        HyperLink hlDetaiPdfImg = new HyperLink();
                        hlDetaiPdfImg.NavigateUrl = "/Portals/0/documents/" + productInfo.ProductPreview;
                        hlDetaiPdfImg.Target = "_blank";
                        hlDetaiPdfImg.ToolTip = "View content PDF";
                        hlDetaiPdfImg.ImageUrl = imagesPath + "shop_detail_bg_left_pdf_on.gif";
                        return hlDetaiPdfImg;
                    }
                    else
                    {
                        Image imgDetaiPdfImg = new Image();
                        imgDetaiPdfImg.ImageUrl = imagesPath + "shop_detail_bg_left_pdf_off.gif";
                        imgDetaiPdfImg.ToolTip = "View content PDF";
                        return imgDetaiPdfImg;
                    }
                case "WHYBUYIMG":
                    if ((productInfo.DescriptionTwo.Length == 0) || (catalogNav.IsTOC != Null.NullInteger))
                        return null;
                    else
                    {
                        Image imgWhyBuyImg = new Image();
                        imgWhyBuyImg.ImageUrl = imagesPath + "shop_why_buy.gif";
                        imgWhyBuyImg.ToolTip = "Why buy this report";
                        return imgWhyBuyImg;
                    }

                case "KEYINFOIMG":
                    if ((productInfo.DescriptionThree.Length == 0) || (catalogNav.IsTOC != Null.NullInteger))
                        return null;
                    else
                    {
                        Image imgKeyInfoImg = new Image();
                        imgKeyInfoImg.ImageUrl = imagesPath + "shop_key_information.gif";
                        imgKeyInfoImg.ToolTip = "Key information provided in this Report";
                        return imgKeyInfoImg;
                    }

                case "DESCRIPTIONIMG":
                    Image imgDescriptionImg = new Image();
                    if (catalogNav.IsTOC != Null.NullInteger)
                    {
                        imgDescriptionImg.ImageUrl = imagesPath + "shop_contents.gif";
                        imgDescriptionImg.ToolTip = "Contents";
                    }
                    else
                    {
                        imgDescriptionImg.ImageUrl = imagesPath + "shop_description.gif";
                        imgDescriptionImg.ToolTip = "Description";
                    }
                    return imgDescriptionImg;

                case "DETAILHTMLIMG":
                    String productId = productInfo.ProductID.ToString();
                    String cat1 = productInfo.CategoryID1.ToString();
                    String cat2 = productInfo.CategoryID2.ToString();
                    String cat3 = productInfo.CategoryID3.ToString();
                    String title = productInfo.ModelName.ToString();
                    if (HasTOC_Html(productId))
                    {
                        HyperLink hlDetaiHtmlImg = new HyperLink();
                        hlDetaiHtmlImg.ImageUrl = imagesPath + "shop_detail_bg_left_html_on.gif";
                        //hlDetaiHtmlImg.NavigateUrl = FixHyperLinkTOC(productId);
                        hlDetaiHtmlImg.NavigateUrl = FixHyperLinkTOC(productId, cat1, cat2, cat3, title);
                        hlDetaiHtmlImg.ToolTip = "View content HTML";
                        return hlDetaiHtmlImg;
                    }
                    else
                    {
                        Image imgDetaiHtmlImg = new Image();
                        imgDetaiHtmlImg.ImageUrl = imagesPath + "shop_detail_bg_left_html_off.gif";
                        imgDetaiHtmlImg.ToolTip = "View content HTML";
                        return imgDetaiHtmlImg;
                    }

                case "DESCRIPTION":
					Label lblDescription = new Label();
                    if (catalogNav.IsTOC != Null.NullInteger)
                        lblDescription.Text = System.Web.HttpUtility.HtmlDecode(productInfo.TOC_Html);
                    else
					    lblDescription.Text = System.Web.HttpUtility.HtmlDecode(productInfo.Description);
					return lblDescription;

                case "DESCRIPTIONTWO":
                    Label lblDescriptionTwo = new Label();
                    if ((productInfo.DescriptionTwo.Length == 0) || (catalogNav.IsTOC != Null.NullInteger))
                        return null;
                    else
                    {
                        lblDescriptionTwo.Text = System.Web.HttpUtility.HtmlDecode(productInfo.DescriptionTwo);
                        return lblDescriptionTwo;
                    }

                case "DESCRIPTIONTHREE":
                    Label lblDescriptionThree = new Label();
                    if ((productInfo.DescriptionThree.Length == 0) || (catalogNav.IsTOC != Null.NullInteger))
                        return null;
                    else
                    {
                        lblDescriptionThree.Text = System.Web.HttpUtility.HtmlDecode(productInfo.DescriptionThree);
                        return lblDescriptionThree;
                    }

                case "DESCRIPTIONTAG":
                    Label lblDescriptionTag = new Label();
                    lblDescriptionTag.Text = System.Web.HttpUtility.HtmlDecode(productInfo.DescriptionTag);
                    return lblDescriptionTag;

                case "LOCALE":
                    return new LiteralControl(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());

                case "TEMPLATESBASEURL":
                    return new LiteralControl(virtualtemplatesPath);

                case "IMAGESBASEURL":
                    return new LiteralControl(imagesPath);

                case "PRODUCTDETAILURL":
                    StringDictionary urlLink = new StringDictionary();
                    urlLink.Add("ProductID", productInfo.ProductID.ToString());

                    return new LiteralControl(catalogNav.GetNavigationUrl(urlLink, detailID));


                case "PAGES":
                    Label lblPages = new Label();
                    lblPages.Text = System.Web.HttpUtility.HtmlDecode(productInfo.NumPages.ToString());
                    return lblPages;

                case "PUBLISHDATE":
                    Label lblPublishDate = new Label();
                    lblPublishDate.Text = System.Web.HttpUtility.HtmlDecode(productInfo.PublishDate.ToString("dd/MM/yyyy"));
                    return lblPublishDate;


                case "BUYONLINEVISIBILITY":
                    bool buyOnline = productInfo.AvailableOnline;
                    Literal litClass = new Literal();
                    litClass.Text = "BuyOnlineVisibility" + buyOnline.ToString();
                    return litClass;
                case "DELIVERYMETHOD":
                    int deliveryMethod = productInfo.DeliveryMethod;    // 1 - file download, 2 - hard-copy, 3 - both
                    DropDownList ddDelivery = new DropDownList();
                    ddDelivery.ID = "ddDelivery";
                    switch (deliveryMethod)
                    { 
                        case 1:
                            ddDelivery.Items.Add(new ListItem("Download PDF", "1"));
                            break;
                        case 2:
                            ddDelivery.Items.Add(new ListItem("Hard copy", "2"));
                            break;
                        case 3:
                            ddDelivery.Items.Add(new ListItem("Download PDF", "1"));
                            ddDelivery.Items.Add(new ListItem("Hard copy", "2"));
                            break;
                    }
                    return ddDelivery;

                case "QUANTITY":
                    TextBox tbQty = new TextBox();
                    tbQty.ID = "tbQuantity";
                    tbQty.Width = new Unit(25);
                    tbQty.Text = "1";
                    //ControlIdQty = tbQty.ClientID;
                    return tbQty;

                default:
					LiteralControl litText = new LiteralControl(tokenName);
					return litText;
			}
		}
		#endregion

        protected bool HasTOC_Html(String productId)
        {
            ProductController productController = new ProductController();
            productInfo = productController.GetProduct(Int32.Parse(productId));
            if (productInfo.TOC_Html != "")
                return true;
            else
                return false;
        }

        private void loadRelatedProducts()
        {
            int currentProductId = productInfo.ProductID;
            int categoryID1 = productInfo.CategoryID1;
            int categoryID2 = productInfo.CategoryID2;
            int categoryID3 = -1; // Should select from all countries productInfo.CategoryID3;
            ProductController productController = new ProductController();
            ArrayList productList = new ArrayList();
            foreach (ProductInfo product in productController.GetRelatedProducts(-1, categoryID1, categoryID2, categoryID3, currentProductId, false))
            {
                productList.Add(product);
                if (productList.Count >= 1) break;
            }

            gvResults.DataSource = productList;
            gvResults.DataBind();
        }

        //protected String FixHyperLinkTOC(String productId)
        protected String FixHyperLinkTOC(String productId, String cat1, String cat2, String cat3, String title)
        {
            StringDictionary replaceParams = new StringDictionary();
            //replaceParams["CategoryID"] = catalogNav.CategoryID.ToString();
            replaceParams["CategoryID"] = cat1;
            replaceParams["CategoryID2"] = cat2;
            replaceParams["CategoryID3"] = cat3;
            replaceParams["Title"] = GetProductFriendlyTitle(title);

            replaceParams["PageIndex"] = Null.NullString;
            replaceParams["IsTOC"] = "1";
            String link = catalogNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
            return link;
            //s.NavigateUrl = Request.ServerVariables["PATH_INFO"].Substring(0, Request.ServerVariables["PATH_INFO"].LastIndexOf("/") + 1) + s.NavigateUrl;
            //return "aaaa"+ Request.ServerVariables["PATH_INFO"].Substring(0, Request.ServerVariables["PATH_INFO"].LastIndexOf("/") + 1) + s;
        }

        //protected String FixHyperLink(String productId)
        protected String FixHyperLink(String productId, String cat1, String cat2, String cat3, String title) 
        {
            StringDictionary replaceParams = new StringDictionary();
            //replaceParams["CategoryID"] = catalogNav.CategoryID.ToString();
            replaceParams["CategoryID"] = cat1;
            replaceParams["CategoryID2"] = cat2;
            replaceParams["CategoryID3"] = cat3;
            replaceParams["Title"] = GetProductFriendlyTitle(title);

            replaceParams["ProductID"] = productId;
            replaceParams["PageIndex"] = Null.NullString;
            replaceParams["IsTOC"] = Null.NullString;
            String link = catalogNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
            return link;
            //s.NavigateUrl = Request.ServerVariables["PATH_INFO"].Substring(0, Request.ServerVariables["PATH_INFO"].LastIndexOf("/") + 1) + s.NavigateUrl;
            //return "aaaa"+ Request.ServerVariables["PATH_INFO"].Substring(0, Request.ServerVariables["PATH_INFO"].LastIndexOf("/") + 1) + s;
        }

        protected String FixHyperLinkCategory(String cat1, String cat2, String cat3) 
        {
            StringDictionary replaceParams = new StringDictionary();
            //replaceParams["CategoryID"] = catalogNav.CategoryID.ToString();
            replaceParams["CategoryID"] = cat1;
            replaceParams["CategoryID2"] = cat2;
            replaceParams["CategoryID3"] = cat3;
            replaceParams["Title"] = Null.NullString;
            replaceParams["ProductID"] = Null.NullString;
            replaceParams["PageIndex"] = Null.NullString;
            String link = catalogNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
            return link;
        }

        protected string GetProductFriendlyTitle(String title)
        {
            String separator = "_";
            title = title.Replace("«", "");
            title = title.Replace("»", "");
            title = title.Replace("'", "");
            title = title.Replace("\"", "");
            title = title.Replace("\\", " ");
            title = title.Replace("/", " ");
            title = title.Replace("(", " ");
            title = title.Replace(")", " ");
            title = title.Replace("[", " ");
            title = title.Replace("]", " ");
            title = title.Replace("  ", " ");
            title = title.Replace(" - ", separator);
            title = title.Replace(" ", separator);
            title = title.Replace("&", separator);
            title = title.Replace("=", separator);
            title = title.Replace("?", separator);
            title = title.Replace("!", separator);
            title = title.Replace("#", separator);
            title = title.Replace("+", separator);
            title = title.Replace(".", separator);
            title = title.Replace(":", separator);
            title = title.Replace(",", separator);
            title = title.Replace(";", separator);
            title = Server.UrlEncode(title);
            if (title.Length > 50)
                title = title.Substring(0, 50);

            return title;
        }

        private void readParams()
        {
            string body = "Parâmetros da página" + "\n" + "\n";
            body = body + "Página: " + Request.ServerVariables["PATH_INFO"] + "\n";
            body = body + "Timestamp: " + System.DateTime.Now + "\n";

            body = body + "\n";

            body = body + "Request.ServerVariables" + "\n";
            body = body + "***********************" + "\n";

            foreach (string variavel in Request.ServerVariables)
            {
                body = body + variavel + ": " + Request.ServerVariables[variavel] + "\n";
            }

            body = body + "\n";

            body = body + "Request.Form" + "\n";
            body = body + "***********************" + "\n";
            foreach (string variavel in Request.Form)
            {
                body = body + variavel + ": " + Request.Form[variavel] + "\n";
            }

            body = body + "\n";

            body = body + "Request.QueryString" + "\n";
            body = body + "***********************" + "\n";
            foreach (string variavel in Request.QueryString)
            {
                body = body + variavel + ": " + Request.QueryString[variavel] + "\n";
            }

            body = body + "\n";

            body = body + "Request.Cookies" + "\n";
            body = body + "***********************" + "\n";
            foreach (string variavel in Request.Cookies)
            {
                body = body + variavel + ": " + Request.Cookies[variavel] + "\n";
            }

            body = body + "\n";

            body = body + "Request.ClientCertificate" + "\n";
            body = body + "***********************" + "\n";
            foreach (string variavel in Request.ClientCertificate)
            {
                body = body + variavel + ": " + Request.ClientCertificate[variavel] + "\n";
            }


            body = body + "\n";

            body = body + "Session.SessionID" + "\n";
            body = body + "***********************" + "\n";
            body = body + "SessionID" + ": " + Session.SessionID + "\n";

            Response.Write(body);
        }
	}
}
