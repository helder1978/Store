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
using System.Web.Services;
using AjaxControlToolkit;

using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;

using System.Data;
using System.Data.OleDb;

namespace DotNetNuke.Modules.Store.WebControls
{
	public enum ProductListTypes
	{
        New,
		Featured,
		Popular,
		Category
	}

	/// <summary>
	/// Summary description for Media.
	/// </summary>
	public partial class Catalog : PortalModuleBase, IActionable
	{
		#region Private Members
        private ModuleSettings moduleSettings;
        private StoreInfo storeInfo;
        private string templatesPath = "";
		private CatalogNavigation catalogNav;
		private CategoryInfo categoryInfo = new CategoryInfo();
		private ProductInfo productInfo = new ProductInfo();
        private string npTitle = null;
        private string fpTitle = null;
        private string ppTitle = null;
        private string cpTitle = null;
		#endregion

        protected PlaceHolder phSubcategory;
        protected PlaceHolder phSubSubcategory;
        protected PlaceHolder phSubcategoryDD;
        protected PlaceHolder phSubcategoryDEDD;
        protected PlaceHolder phSearch;
        protected PlaceHolder phSearchResults;
        //protected PlaceHolder phAddToCart;
        protected DropDownList ddHidden;
        protected DropDownList ddDEHidden;
        protected DropDownList DropDownList1;
        protected DropDownList DropDownList2;
        protected DropDownList ddListLocation;
        protected DropDownList ddListCategory;
        protected DropDownList ddSearchCategory;
        protected Label labelAreaName;
        protected Label labelCategoryName;
        protected Label labelSubSubCategory;
        protected Label labelSubSubcategoryNow;
        protected Label Label1;
        protected Label Label2;
        protected Label labelLog;
        protected Label labelLog2;
        protected Label labelError;
        protected GridView gvResults;
        protected GridView gvDECart;
        protected TextBox tbDEReference;
        protected TextBox txtSearch;
        protected ImageButton imgGo;
        protected HtmlTable tableSubSubCategory;

        //  execute Store_Products_GetProductsSearched '-1', 'beer'
        
		#region Public Properties
		public ModuleSettings ModuleSettings
		{
			get{return moduleSettings;}
		}

		public Int32 CategoryID
		{
			get {return catalogNav.CategoryID;}
		}

		public Int32 ProductID
		{
			get {return catalogNav.ProductID;}
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
            imgGo.Click += new ImageClickEventHandler(imgGo_Click);
        }

		#endregion

		#region Events
        
        protected void Page_PreRender(object sender, System.EventArgs e)
        {/*
            if (Page.IsPostBack)
            {
                labelLog.Text = labelLog.Text + "<br>Page PreRender PostBack" + DateTime.Now.ToString();
                int cat1 = -1;
                int.TryParse(DropDownList1.SelectedValue, out cat1);
                int cat2 = -1;
                int.TryParse(DropDownList2.SelectedValue, out cat2);
                labelLog.Text = labelLog.Text + "<br>dd1: " + cat1.ToString() + " dd2: " + cat2.ToString() + DateTime.Now.ToString();
            }
            else
            {
                labelLog.Text = labelLog.Text + "<br>Page PreRender - Not a PostBack" + DateTime.Now.ToString();
            }*/
        }
        
        protected void Page_Load(object sender, System.EventArgs e)
		{
            /*
            Response.Write("<br>Page_load2 ");
            if (Page.IsPostBack)
                Response.Write("<br>Postback2 ");
            */

            /*
            if (Page.IsPostBack)
            {
                labelLog.Text = labelLog.Text + "<br>Page load PostBack" + DateTime.Now.ToString();
                int cat1 = -1;
                int.TryParse(DropDownList1.SelectedValue, out cat1);
                int cat2 = -1;
                int.TryParse(DropDownList2.SelectedValue, out cat2);
                labelLog.Text = labelLog.Text + "<br>dd1: " + cat1.ToString() + " dd2: " + cat2.ToString() + DateTime.Now.ToString();
            }
            else
            {
                labelLog.Text = labelLog.Text + "<br>Page load - Not a PostBack" + DateTime.Now.ToString();
            } */
                //Response.Write("Page_PreRender");

                npTitle = Localization.GetString("NPTitle.Text", this.LocalResourceFile);
                fpTitle = Localization.GetString("FPTitle.Text", this.LocalResourceFile);
                ppTitle = Localization.GetString("PPTitle.Text", this.LocalResourceFile);
                cpTitle = Localization.GetString("CPTitle.Text", this.LocalResourceFile);

                try
                {
                    if (storeInfo == null)
                    {
                        StoreController storeController = new StoreController();
                        storeInfo = storeController.GetStoreInfo(PortalId);
                        if (storeInfo.PortalTemplates)
                        {
                            templatesPath = PortalSettings.HomeDirectoryMapPath + "Store\\";
                            CssTools.AddCss(this.Page, PortalSettings.HomeDirectory + "Store", PortalId);
                        }
                        else
                        {
                            templatesPath = MapPath(ModulePath) + "\\";
                            CssTools.AddCss(this.Page, this.TemplateSourceDirectory, PortalId);
                        }
                    }

                    moduleSettings = new ModuleSettings(this.ModuleId, this.TabId);

                    catalogNav = new CatalogNavigation(Request.QueryString);

                    if (catalogNav.CategoryID == Null.NullInteger)
                    {
                        if (bool.Parse(moduleSettings.General.UseDefaultCategory))
                        {
                            catalogNav.CategoryID = int.Parse(moduleSettings.General.DefaultCategoryID);
                        }
                    }

                    if (catalogNav.ProductID != Null.NullInteger)
                    {
                        ProductController productController = new ProductController();
                        productInfo = productController.GetProduct(catalogNav.ProductID);
                        catalogNav.CategoryID = productInfo.CategoryID;
                    }

                    if (catalogNav.CategoryID != Null.NullInteger)
                    {
                        CategoryController categoryController = new CategoryController();
                        categoryInfo = categoryController.GetCategory(catalogNav.CategoryID);
                    }

                    this.Controls.Add(TemplateController.ParseTemplate(templatesPath, moduleSettings.General.Template, new ProcessTokenDelegate(processToken)));

                    // Canadean changed: added current categoryid on a hidden dropdown, to use on the cascade categories
                    /*
                    if (catalogNav.CategoryID != Null.NullInteger)
                    {
                        ddHidden.Items.Add(new ListItem(catalogNav.CategoryID.ToString(), catalogNav.CategoryID.ToString()));
                    }
                    */

                }
                catch (Exception ex)
                {
                    string ErrorSettings = Localization.GetString("ErrorSettings", this.LocalResourceFile);
                    Response.Write("<br>" + ex.Message);
                    Response.Write("<br>" + ex.StackTrace);

                    //Exceptions.ProcessModuleLoadException(ErrorSettings, this, ex, true);
                }
                if (DotNetNuke.Framework.AJAX.IsInstalled())
                {
                    DotNetNuke.Framework.AJAX.RegisterScriptManager();
                }

            //}
        }
		#endregion

		#region Private Functions

		private Control processToken(string tokenName)
		{
			ProductController productController = new ProductController();
			ArrayList productList = new ArrayList();

            Control parent = new Control();
            switch (tokenName)
			{
				case "MESSAGE":
					if(bool.Parse(moduleSettings.General.ShowMessage) && categoryInfo != null)
					{
                        // canadean changed: added category name
                        LiteralControl message = new LiteralControl(Server.HtmlDecode(categoryInfo.CategoryName));
                        parent.Controls.Add(message);
                        message = new LiteralControl(Server.HtmlDecode(categoryInfo.Message));
                        parent.Controls.Add(message);
                    }

                    if (catalogNav.CategoryID != Null.NullInteger)
                    {
                        //sub categories...
                        CategoryController controller = new CategoryController();
                        ArrayList subCategories = controller.GetCategories(PortalId, false, catalogNav.CategoryID);
                        if (subCategories.Count > 0)
                        {
                            LiteralControl BreakRow4 = new LiteralControl("<br /><div align=\"center\">");
                            parent.Controls.Add(BreakRow4);
                        }
                        //StoreController storeController = new StoreController();
                        //StoreInfo storeInfo = storeController.GetStoreInfo(PortalId);
                        foreach (CategoryInfo category in subCategories)
                        {
                            LiteralControl BreakRow = new LiteralControl("|&nbsp;");
                            parent.Controls.Add(BreakRow);
                            HyperLink link = new HyperLink();
                            link.Text = category.CategoryName;
                            StringDictionary replaceParams = new StringDictionary();
                            replaceParams["CategoryID"] = category.CategoryID.ToString();
			                replaceParams["ProductID"] = Null.NullString;
                            replaceParams["PageIndex"] = Null.NullString;
                            link.NavigateUrl = catalogNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
                            parent.Controls.Add(link);
                            LiteralControl BreakRow2 = new LiteralControl("&nbsp;");
                            parent.Controls.Add(BreakRow2);
                        }
                        if (subCategories.Count > 0)
                        {
                            LiteralControl BreakRow3 = new LiteralControl("|</div><br/>");
                            parent.Controls.Add(BreakRow3);
                        }
                        else
                        {
                            LiteralControl BreakRow5 = new LiteralControl("<br/>");
                            parent.Controls.Add(BreakRow5);
                        }
                    }
                    return parent;

                // Canadean changed: added subcategory token
                case "SUBCATEGORY":
                    /*LiteralControl lc = new LiteralControl("pId: " + catalogNav.ProductID);
                    parent.Controls.Add(lc);*/
                    if ((catalogNav.CategoryID != Null.NullInteger) && (catalogNav.ProductID == Null.NullInteger))
                    {
                        //sub categories...
                        CategoryController controller = new CategoryController();
                        ArrayList subCategories = controller.GetCategories(PortalId, false, catalogNav.CategoryID);

                        if (subCategories.Count > 0)
                        {
                            /*
                            String htmlstr = "<br><table cellspacing='3' cellpadding='0' border='0'><tr><td class='subtitleShop'>select report category</td></tr><tr><td>" +
					            "<table cellspacing='0' cellpadding='0' border='0'> " +
					            "    <tbody>" +
					            "        <tr>" +
					            "            <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/5/Default.aspx'>Soft Drinks</a></td>" +
					            "            <td><a href='/Shop/Reports/tabid/109/CategoryID/5/Default.aspx'><img alt='' border='0' src='/images/canadean/shop/shop_bullet.gif' /> </a></td>" +
					            "            <td>&nbsp;</td>" +
					            "            <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/8/Default.aspx'>Beverage Packaging</a></td>" +
					            "            <td><a href='/Shop/Reports/tabid/109/CategoryID/8/Default.aspx'><img alt='' border='0' src='/images/canadean/shop/shop_bullet.gif' /> </a></td>" +
					            "            <td>&nbsp;</td>" +
					            "        </tr>" +
					            "        <tr>" +
					            "            <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/6/Default.aspx'>Beer</a></td>" +
					            "            <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/6/Default.aspx'><img alt='' border='0' src='/images/canadean/shop/shop_bullet.gif' /> </a></td>" +
					            "            <td>&nbsp;</td>" +
					            "            <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/9/Default.aspx'>All Beverages</a></td>" +
					            "            <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/9/Default.aspx'><img alt='' border='0' src='/images/canadean/shop/shop_bullet.gif' /> </a></td>" +
					            "            <td>&nbsp;</td>" +
					            "        </tr>" +
					            "        <tr>" +
					            "            <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/7/Default.aspx'>Dairy</a></td>" +
					            "            <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/7/Default.aspx'><img alt='' border='0' src='/images/canadean/shop/shop_bullet.gif' /> </a></td>" +
					            "            <td>&nbsp;</td>" +
					            "            <td>&nbsp;</td>" +
					            "            <td>&nbsp;</td>" +
					            "            <td>&nbsp;</td>" +
					            "        </tr>" +
					            "    </tbody>" +
					            "</table></td></tr></table><p>&nbsp;</p>";
                            */
                            // LiteralControl title = new LiteralControl(htmlstr);
                            //parent.Controls.Add(title);
                            phSubcategory.Visible = true;
                        
                        }
                        /*
                        // Not used because of aesthetics questions
                        if (subCategories.Count > 0)
                        {
                            LiteralControl title = new LiteralControl("<br /><div class=subtitleShop>select report category</div>");
                            parent.Controls.Add(title);
                            LiteralControl BreakRow4 = new LiteralControl("<div class=categoryList><ul>");
                            parent.Controls.Add(BreakRow4);
                        }
                        //StoreController storeController = new StoreController();
                        //StoreInfo storeInfo = storeController.GetStoreInfo(PortalId);
                        foreach (CategoryInfo category in subCategories)
                        {
                            LiteralControl BreakRow = new LiteralControl("<li>");
                            parent.Controls.Add(BreakRow);
                            HyperLink link = new HyperLink();
                            link.Text = category.CategoryName;
                            link.CssClass = "newsMenuOn";
                            StringDictionary replaceParams = new StringDictionary();
                            replaceParams["CategoryID"] = category.CategoryID.ToString();
                            replaceParams["ProductID"] = Null.NullString;
                            replaceParams["PageIndex"] = Null.NullString;
                            link.NavigateUrl = catalogNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
                            parent.Controls.Add(link);
                            LiteralControl BreakRow2 = new LiteralControl("</li>");
                            parent.Controls.Add(BreakRow2);
                        }
                        if (subCategories.Count > 0)
                        {
                            LiteralControl BreakRow3 = new LiteralControl("<ul></div>");
                            parent.Controls.Add(BreakRow3);
                        }
                        //else
                        //{
                        //    LiteralControl BreakRow5 = new LiteralControl("<br/>");
                        //    parent.Controls.Add(BreakRow5);
                        //}
                        */


                        return parent;
                    }
                    return null;

                // Canadean changed: added subsubcategory token
                case "SUBSUBCATEGORY":
                    if ((catalogNav.CategoryID3 == Null.NullInteger) && (catalogNav.CategoryID != Null.NullInteger) && (categoryInfo.ParentCategoryID == 4))    // Only show DD sub categories when we're on the Drink Type category
                    {
                        phSubSubcategory.Visible = true;
                        int categoryId1 = -1;
                        int categoryId2 = -1;
                        int returnCategory = 2;
                        categoryId1 = catalogNav.CategoryID;
                        CategoryController controller = new CategoryController();

                        if (catalogNav.CategoryID2 != Null.NullInteger)     // Type & Continent already selected, missing Country
                        {
                            labelSubSubcategoryNow.Text = "now select country";
                            categoryId2 = catalogNav.CategoryID2;
                            returnCategory = 3;
                            CategoryInfo categoryInfo2 = new CategoryInfo();
                            categoryInfo2 = controller.GetCategory(catalogNav.CategoryID2);
                            //labelCategoryName.Text = categoryInfo.CategoryName + " in " + categoryInfo2.CategoryName;
                            labelCategoryName.Text = "<u><a href='" + FixHyperlinkCatalog(catalogNav, 1) + "' class='demoHeadingLink'>" + categoryInfo.CategoryName + "</a></u> in " +
                                "<u><a href='" + FixHyperlinkCatalog(catalogNav, 2) + "' class='demoHeadingLink'>" + categoryInfo2.CategoryName + "</a></u>";

                        }
                        else
                        {
                            labelSubSubcategoryNow.Text = "now select continent";
                            //labelCategoryName.Text = categoryInfo.CategoryName;
                            labelCategoryName.Text = "<u><a href='" + FixHyperlinkCatalog(catalogNav, 1) + "' class='demoHeadingLink'>" + categoryInfo.CategoryName + "</a></u>";
                        }

                        /*String str = "<br>Getting data. Portal " + PortalId + " c1: " + categoryId1 + " c2: " + categoryId2 + " rc: " + returnCategory;
                        labelLog2.Text = str;
                        Response.Write(str);*/

                        ArrayList subCategories = controller.GetCategoriesFromProducts(PortalId, categoryId1, categoryId2, -1, returnCategory);
                        if (subCategories.Count > 0)
                        {
                            //labelSubSubCategory.Text = "<ul>";
                            bool two_columns = false;
                            int count = 0;
                            if(subCategories.Count > 24)
                                two_columns = true;

                            //Response.Write("lines: " + subCategories.Count);

                            labelSubSubCategory.Text = "<br /><table cellspacing='0' cellpadding='0' border='0'>";
                            CatalogNavigation currentCatalogNav = catalogNav;
                            foreach (CategoryInfo cur_category in subCategories)
                            {
                                String cat_name = cur_category.CategoryName;
                                int cat_id = cur_category.CategoryID;

                                StringDictionary replaceParams = new StringDictionary();
                                replaceParams["CategoryID"] = categoryId1.ToString();
                                if (catalogNav.CategoryID2 != Null.NullInteger)
                                {
                                    replaceParams["CategoryID2"] = catalogNav.CategoryID2.ToString();
                                    replaceParams["CategoryID3"] = cat_id.ToString();
                                }
                                else
                                {
                                    replaceParams["CategoryID2"] = cat_id.ToString();
                                }
                                replaceParams["ProductID"] = Null.NullString;
                                replaceParams["PageIndex"] = Null.NullString;
                                String linkCat = currentCatalogNav.GetNavigationUrl(replaceParams);

                                //Response.Write("two_colums: " + two_columns + " mod: " + (count % 2));

                                if (!two_columns || (count % 2 == 0))
                                    labelSubSubCategory.Text += "<tr>";
                                //labelSubSubCategory.Text += "<li><a href='" + linkCat + "' class='newsMenuOff'>" + cat_name + "</a></li>";
                                labelSubSubCategory.Text += "<td><a class='shopIntroText' href='" + linkCat + "'>" + cat_name + "</a></td><td>&nbsp;</td>";
                                labelSubSubCategory.Text += "<td><a href='" + linkCat + "'><img border='0' alt='' src='/images/canadean/shop/shop_bullet.gif' /></a></td>";
                                labelSubSubCategory.Text += "<td width='20%'>&nbsp;</td>";
                                if (!two_columns || (count % 2 == 1))
                                    labelSubSubCategory.Text += "</tr>";
                                count++;
                            }
                            //labelSubSubCategory.Text += "</ul>";
                            labelSubSubCategory.Text += "</table><br />";
                        }

                        //Response.Write("SUBCATEGORYDD make it visible ");
                        return parent;
                    }
                    else
                    {   // Only show the products if we have all the categories information
                        if ((catalogNav.CategoryID != Null.NullInteger) && (catalogNav.CategoryID2 != Null.NullInteger) && (catalogNav.CategoryID3 != Null.NullInteger) && (catalogNav.ProductID == Null.NullInteger))    
                        {
                            phSubSubcategory.Visible = true;
                            tableSubSubCategory.Visible = false;
                            phSearchResults.Visible = true;
                            PopulateGridView(true);
                            Label1.Visible = true;
                            return parent;
                        }
                    }

                    return null;
                // Canadean changed: added subcategory token (unused for now)
                case "SUBCATEGORYDD":
                    if ((catalogNav.CategoryID != Null.NullInteger) && (categoryInfo.ParentCategoryID == 4))    // Only show DD sub categories when we're on the Drink Type category
                    {
                        phSubcategoryDD.Visible = true;
                        phSearchResults.Visible = true;
                        ddHidden.Items.Add(new ListItem(catalogNav.CategoryID.ToString(), catalogNav.CategoryID.ToString()));
                        labelAreaName.Text = categoryInfo.CategoryName;
                        //Response.Write("SUBCATEGORYDD make it visible ");
                        return parent;
                    }
                    return null;
                // Canadean changed: added Data Extracts subcategory token
                case "SUBCATEGORYDEDD":
                    //Response.Write("SUBCATEGORYDEDD make it visible {" + catalogNav.CategoryID + "} {" + categoryInfo.ParentCategoryID + "}");
                    //if ((catalogNav.CategoryID != Null.NullInteger) && (categoryInfo.ParentCategoryID == -1))    // Only show DD sub categories when we're on the Data Extracts category
                    if ((catalogNav.CategoryID != Null.NullInteger) && (categoryInfo.CategoryID == 2))    // Only show DD sub categories when we're on the Data Extracts category
                    {
                        phSubcategoryDEDD.Visible = true;
                        ddDEHidden.Items.Add(new ListItem(catalogNav.CategoryID.ToString(), catalogNav.CategoryID.ToString()));
                        //Response.Write("<br>Inside SUBCATEGORYDEDD make it visible. Selected item: " + ddDEHidden.SelectedIndex);
                        ddDEHidden.ClearSelection();

                        System.Collections.ArrayList prods = PopulateDEGridView(true);
                        int numProds = prods.Count;
                        Label2.Text = "Total cart cost: &pound;" + numProds * 50 + "";
                        
                        return parent;
                    }
                    return null;

                case "SELECTED":    // Disabled!!       Show the products that exist in the current selected categories
                    Response.Write("SELECTED: " + catalogNav.CategoryID + " - " + catalogNav.CategoryID2 + " - " + catalogNav.CategoryID3);
                    if ((catalogNav.CategoryID != Null.NullInteger) && (catalogNav.CategoryID2 != Null.NullInteger) && (catalogNav.CategoryID3 != Null.NullInteger))    // Only show the products if we all the categories information
                    {
                        int categoryID1 = catalogNav.CategoryID;
                        int categoryID2 = catalogNav.CategoryID2;
                        int categoryID3 = catalogNav.CategoryID3;

                        phSearchResults.Visible = true;
                        GetSelectedProducts(-1, categoryID1, categoryID2, categoryID3, productList);
                        //productList = truncateList(productList, int.Parse(moduleSettings.NewProducts.RowCount) * int.Parse(moduleSettings.NewProducts.ColumnCount));
                        return loadProductList(productList, ProductListTypes.Category);
                    }
                    return null;

                    
                // Canadean changed: added search token
                case "CATALOGSEARCH":
                    phSearchResults.Visible = true;

                    /*
                    int categoryID = 0;
                    string searchTerm = "";
                    Response.Write("here I am: " + searchTerm);
                    if (Page.IsPostBack)
                        Response.Write(" postback: ");
                    else
                        Response.Write(" not a postback: ");
                    */

                    /*
                    if (!Page.IsPostBack)
                    {
                        if (Request.Params["SearchCategoryId"] != null)
                            int.TryParse(Request.Params["SearchCategoryId"].ToString(), out categoryID);
                        if (Request.Params["Search"] != null)
                            searchTerm = Request.Params["Search"].ToString();
                        GetSearchedProducts(categoryID, searchTerm, productList);
                        //productList = truncateList(productList, int.Parse(moduleSettings.NewProducts.RowCount) * int.Parse(moduleSettings.NewProducts.ColumnCount));
                        if (productList.Count > 0)
                        {
                            gvResults.DataSource = productList;
                            gvResults.DataBind();
                        }
                    }
                    */
                    Label1.Visible = false;
                    PopulateGridView(true);

                    return parent;
                case "NEW":
                    if (bool.Parse(moduleSettings.General.ShowNewProducts))
                    {
                        if (catalogNav.CategoryID != Null.NullInteger)
                        {
                            GetNewProducts(catalogNav.CategoryID, productList);
                            productList = truncateList(productList, int.Parse(moduleSettings.NewProducts.RowCount) * int.Parse(moduleSettings.NewProducts.ColumnCount));
                            return loadProductList(productList, ProductListTypes.New);
                        }
                        else
                        {
                            productList = productController.GetPortalNewProducts(PortalId, false);
                            productList = truncateList(productList, int.Parse(moduleSettings.NewProducts.RowCount) * int.Parse(moduleSettings.NewProducts.ColumnCount));
                            return loadProductList(productList, ProductListTypes.New);
                        }
                    }
                    else
                    {
                        return null;
                    }

				case "FEATURED":
					if (bool.Parse(moduleSettings.General.ShowFeaturedProducts))
					{
						if(catalogNav.CategoryID != Null.NullInteger)
						{
                            GetFeaturedProducts(catalogNav.CategoryID, productList);
                            productList = SortProductsInRandomOrder(productList);
							productList = truncateList(productList, int.Parse(moduleSettings.FeaturedProducts.RowCount) * int.Parse(moduleSettings.FeaturedProducts.ColumnCount));
							return loadProductList(productList, ProductListTypes.Featured);
						}
						else
						{
							productList = productController.GetPortalFeaturedProducts(PortalId, false);
							productList = truncateList(productList, int.Parse(moduleSettings.FeaturedProducts.RowCount) * int.Parse(moduleSettings.FeaturedProducts.ColumnCount));
							return loadProductList(productList, ProductListTypes.Featured);
						}
					}
					else
					{
						return null;
					}

				case "POPULAR":
					if (bool.Parse(moduleSettings.General.ShowPopularProducts))
					{
						if(catalogNav.CategoryID != Null.NullInteger)
						{
                            GetPopularProducts(catalogNav.CategoryID, productList);
                            productList = SortProductsInRandomOrder(productList);
							productList = truncateList(productList, int.Parse(moduleSettings.PopularProducts.RowCount) * int.Parse(moduleSettings.PopularProducts.ColumnCount));
							return loadProductList(productList, ProductListTypes.Popular);
						}
						else
						{
							productList = productController.GetPortalPopularProducts(PortalId, false);
                            productList = truncateList(productList, int.Parse(moduleSettings.PopularProducts.RowCount) * int.Parse(moduleSettings.PopularProducts.ColumnCount));
							return loadProductList(productList, ProductListTypes.Popular);
						}
					}
					else
					{
						return null;
					}

				case "CATEGORY":
					if (bool.Parse(moduleSettings.General.ShowCategoryProducts))
					{
						if(catalogNav.CategoryID != Null.NullInteger && catalogNav.ProductID == Null.NullInteger)
						{
                            GetCategoryProducts(catalogNav.CategoryID, productList);
                            productList = SortProductsInAlphabeticalOrder(productList);
							return loadProductList(productList, ProductListTypes.Category);
						}
						else
						{
							return null;
						}
					}
					else
					{
						return null;
					}

				case "DETAIL":
					if(bool.Parse(moduleSettings.General.ShowProductDetail) && catalogNav.ProductID != Null.NullInteger)
					{
						return loadProductDetail();
					}
					else
					{
						return null;
					}

				default:
					LiteralControl litText = new LiteralControl(tokenName);
					return litText;
			}
		}

        private ArrayList GetSelectedProducts(int categoryID, int categoryID1, int categoryID2, int categoryID3, ArrayList products)
        {
            ProductController productController = new ProductController();

            foreach (ProductInfo product in productController.GetSelectedProducts(categoryID, categoryID1, categoryID2, categoryID3, false))
            {
                products.Add(product);
            }

            return products;
        }

        private ArrayList GetSearchedProducts(int categoryID, string searchTerm, ArrayList products)
        {
            ProductController productController = new ProductController();

            foreach (ProductInfo product in productController.GetSearchedProducts(categoryID, searchTerm, false))
            {
                products.Add(product);
            }

            return products;
        }

        private ArrayList GetNewProducts(int categoryID, ArrayList products)
        {
            CategoryController categoryController = new CategoryController();
            ProductController productController = new ProductController();

            CategoryInfo category = categoryController.GetCategory(categoryID);

            foreach (ProductInfo product in productController.GetNewProducts(categoryID, false))
            {
                products.Add(product);
            }

            foreach (CategoryInfo childCategory in categoryController.GetCategories(PortalId, false, categoryID))
            {
                if (childCategory.CategoryID != Null.NullInteger)
                {
                    GetNewProducts(childCategory.CategoryID, products);
                }
            }

            return products;
        }

        private ArrayList GetFeaturedProducts(int categoryID, ArrayList products)
        {
            CategoryController categoryController = new CategoryController();
            ProductController productController = new ProductController();

            CategoryInfo category = categoryController.GetCategory(categoryID);

            foreach (ProductInfo product in productController.GetFeaturedProducts(categoryID, false))
            {
                products.Add(product);
            }

            foreach (CategoryInfo childCategory in categoryController.GetCategories(PortalId, false, categoryID))
            {
                if (childCategory.CategoryID != Null.NullInteger)
                {
                    GetFeaturedProducts(childCategory.CategoryID, products);
                }
            }

            return products;
        }

        private ArrayList GetPopularProducts(int categoryID, ArrayList products)
        {
            CategoryController categoryController = new CategoryController();
            ProductController productController = new ProductController();

            CategoryInfo category = categoryController.GetCategory(categoryID);

            foreach (ProductInfo product in productController.GetPopularProducts(PortalId, categoryID, false))
            {
                products.Add(product);
            }

            foreach (CategoryInfo childCategory in categoryController.GetCategories(PortalId, false, categoryID))
            {
                if (childCategory.CategoryID != Null.NullInteger)
                {
                    GetPopularProducts(childCategory.CategoryID, products);
                }
            }

            return products;
        }


        private ArrayList GetCategoryProducts(int categoryID, ArrayList products)
        {
            CategoryController categoryController = new CategoryController();
            ProductController productController = new ProductController();

            CategoryInfo category = categoryController.GetCategory(categoryID);

            foreach (ProductInfo product in productController.GetCategoryProducts(categoryID, false))
            {
                products.Add(product);
            }

            foreach (CategoryInfo childCategory in categoryController.GetCategories(PortalId, false, categoryID))
            {
                if (childCategory.CategoryID != Null.NullInteger)
                {
                    GetCategoryProducts(childCategory.CategoryID, products);
                }
            }

            return products;
        }

        private ArrayList SortProductsInAlphabeticalOrder(ArrayList products)
        {
            products.Sort();
            return products;
        }

        private ArrayList SortProductsInRandomOrder(ArrayList products)
        {
            return products;
        }

		private Control loadProductList(ArrayList products, ProductListTypes listType)
		{
			if (products != null && products.Count > 0)
			{
				ProductList productList = (ProductList)LoadControl(ModulePath + "ProductList.ascx");
				productList.ParentControl = this as PortalModuleBase;
				productList.CategoryID = catalogNav.CategoryID;

				switch (listType)
				{
                    case ProductListTypes.New:
                        productList.Title = npTitle;
                        productList.ContainerTemplate = moduleSettings.NewProducts.ContainerTemplate;
                        productList.Template = moduleSettings.NewProducts.Template;
                        productList.RowCount = int.Parse(moduleSettings.NewProducts.RowCount);
                        productList.ColumnCount = int.Parse(moduleSettings.NewProducts.ColumnCount);
                        productList.ColumnWidth = int.Parse(moduleSettings.NewProducts.ColumnWidth);
                        productList.Direction = moduleSettings.NewProducts.RepeatDirection;
                        productList.ShowThumbnail = bool.Parse(moduleSettings.NewProducts.ShowThumbnail);
                        productList.ThumbnailWidth = int.Parse(moduleSettings.NewProducts.ThumbnailWidth);
                        productList.DetailPage = int.Parse(moduleSettings.NewProducts.DetailPage);
                        break;

					case ProductListTypes.Featured:
                        productList.Title = fpTitle;
                        productList.ContainerTemplate = moduleSettings.FeaturedProducts.ContainerTemplate;
						productList.Template = moduleSettings.FeaturedProducts.Template;
						productList.RowCount = int.Parse(moduleSettings.FeaturedProducts.RowCount);
						productList.ColumnCount = int.Parse(moduleSettings.FeaturedProducts.ColumnCount);
						productList.ColumnWidth = int.Parse(moduleSettings.FeaturedProducts.ColumnWidth);
                        productList.Direction = moduleSettings.FeaturedProducts.RepeatDirection;
						productList.ShowThumbnail = bool.Parse(moduleSettings.FeaturedProducts.ShowThumbnail);
						productList.ThumbnailWidth = int.Parse(moduleSettings.FeaturedProducts.ThumbnailWidth);
						productList.DetailPage = int.Parse(moduleSettings.FeaturedProducts.DetailPage);
						break;

					case ProductListTypes.Popular:
                        productList.Title = ppTitle;
                        productList.ContainerTemplate = moduleSettings.PopularProducts.ContainerTemplate;
						productList.Template = moduleSettings.PopularProducts.Template;
						productList.RowCount = int.Parse(moduleSettings.PopularProducts.RowCount);
						productList.ColumnCount = int.Parse(moduleSettings.PopularProducts.ColumnCount);
						productList.ColumnWidth = int.Parse(moduleSettings.PopularProducts.ColumnWidth);
                        productList.Direction = moduleSettings.PopularProducts.RepeatDirection;
						productList.ShowThumbnail = bool.Parse(moduleSettings.PopularProducts.ShowThumbnail);
						productList.ThumbnailWidth = int.Parse(moduleSettings.PopularProducts.ThumbnailWidth);
						productList.DetailPage = int.Parse(moduleSettings.PopularProducts.DetailPage);
						break;

					case ProductListTypes.Category:
                        productList.Title = cpTitle;
                        productList.ContainerTemplate = moduleSettings.CategoryProducts.ContainerTemplate;
						productList.Template = moduleSettings.CategoryProducts.Template;
						productList.RowCount = int.Parse(moduleSettings.CategoryProducts.RowCount);
						productList.ColumnCount = int.Parse(moduleSettings.CategoryProducts.ColumnCount);
						productList.ColumnWidth = int.Parse(moduleSettings.CategoryProducts.ColumnWidth);
                        productList.Direction = moduleSettings.CategoryProducts.RepeatDirection;
						productList.ShowThumbnail = bool.Parse(moduleSettings.CategoryProducts.ShowThumbnail);
						productList.ThumbnailWidth = int.Parse(moduleSettings.CategoryProducts.ThumbnailWidth);
						productList.DetailPage = int.Parse(moduleSettings.CategoryProducts.DetailPage);
						break;
				}

				productList.DataSource = products;
				
				return productList;
			}
			else
			{
				return null;
			}
		}

		private Control loadProductDetail()
		{
			ProductDetail productDetail = (ProductDetail)LoadControl(ModulePath + "ProductDetail.ascx");

			productDetail.ParentControl = this as PortalModuleBase;
			productDetail.CategoryID = productInfo.CategoryID;
			productDetail.ShowThumbnail = bool.Parse(moduleSettings.ProductDetail.ShowThumbnail);
			productDetail.ThumbnailWidth = int.Parse(moduleSettings.ProductDetail.ThumbnailWidth);
			productDetail.ShowReviews = bool.Parse(moduleSettings.ProductDetail.ShowReviews);
			productDetail.DataSource = productInfo;

			return productDetail;
		}

		private ArrayList truncateList(ArrayList list, int maxCount)
		{
			if (list.Count > maxCount)
			{
				list.RemoveRange(maxCount, list.Count - maxCount);
			}
			return list;
		}
		#endregion

		#region IActionable Members

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
		{
			get
			{
				ModuleActionCollection actions = new ModuleActionCollection();
				actions.Add(GetNextActionID(), Localization.GetString("AddNewProduct", this.LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("ProductID", "0"), false, SecurityAccessLevel.Edit, true, false); 
				return actions; 
			}
		}

		#endregion

        
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //labelLog.Text = labelLog.Text + "<br>DropDownList2_SelectedIndexChanged" + DateTime.Now.ToString();
            PopulateGridView(true);
            Label1.Visible = true;
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


        //protected void FixHyperLink(HyperLink s,  EventArgs e) 
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
            String link = catalogNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
            return link;
            //s.NavigateUrl = Request.ServerVariables["PATH_INFO"].Substring(0, Request.ServerVariables["PATH_INFO"].LastIndexOf("/") + 1) + s.NavigateUrl;
            //return "aaaa"+ Request.ServerVariables["PATH_INFO"].Substring(0, Request.ServerVariables["PATH_INFO"].LastIndexOf("/") + 1) + s;
        }

        protected String FixHyperLinkTOC(String productId)
        {
            StringDictionary replaceParams = new StringDictionary();
            replaceParams["CategoryID"] = catalogNav.CategoryID.ToString();
            replaceParams["ProductID"] = productId;
            replaceParams["PageIndex"] = Null.NullString;
            replaceParams["IsTOC"] = "1";
            String link = catalogNav.GetNavigationUrl(replaceParams, storeInfo.StorePageID);
            return link;
            //s.NavigateUrl = Request.ServerVariables["PATH_INFO"].Substring(0, Request.ServerVariables["PATH_INFO"].LastIndexOf("/") + 1) + s.NavigateUrl;
            //return "aaaa"+ Request.ServerVariables["PATH_INFO"].Substring(0, Request.ServerVariables["PATH_INFO"].LastIndexOf("/") + 1) + s;
        }

        protected String FixHyperlinkCatalog(CatalogNavigation currentCatalogNav, int categoryLevel)
        {
            StringDictionary replaceParams = new StringDictionary();
            if (categoryLevel < 3) replaceParams["CategoryID3"] = Null.NullString;
            if (categoryLevel < 2) replaceParams["CategoryID2"] = Null.NullString;
            if (categoryLevel < 1) replaceParams["CategoryID"] = Null.NullString;

            return currentCatalogNav.GetNavigationUrl(replaceParams);
        }

        protected bool HasTOC_Html(String productId) 
        {
            ProductController productController = new ProductController();
            productInfo = productController.GetProduct(Int32.Parse(productId));
            if (productInfo.TOC_Html != "")
                return true;
            else
                return false;
        }


        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }

        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    break;

                case "DESC":
                    GridViewSortDirection = "ASC";
                    break;
            }

            return GridViewSortDirection;
        }

        protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                //labelLog.Text = labelLog.Text + "<br>Has datatable" + dataTable.Rows.Count.ToString();
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                    }
                }
                return dataView;
            }
            else
            {
                //labelLog.Text = labelLog.Text + "<br>Empty datatable";
                return new DataView();
            }
        }

        protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //PopulateGridView(true);
            DataTable dt = GetPopulatedDataTable(true);
            //labelLog.Text = labelLog.Text + "<br>-----------------------> gvResults_PageIndexChanging " + gvResults.PageIndex + " " + DateTime.Now.ToString();
            gvResults.DataSource = SortDataTable(dt, true);
            /*gvResults.DataSource = SortDataTable(gvResults.DataSource as DataTable, true);*/
            gvResults.PageIndex = e.NewPageIndex;
            gvResults.DataBind();
            //labelLog.Text = labelLog.Text + "<br>-----------------------> gvResults_PageIndexChanging Bound " + gvResults.PageIndex + " " + DateTime.Now.ToString();
        }

        protected void gvResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetPopulatedDataTable(true);
            //labelLog.Text = labelLog.Text + "<br>-----------> gvResults_Sorting " + gvResults.PageIndex + " " + DateTime.Now.ToString();
            GridViewSortExpression = e.SortExpression;
            int pageIndex = gvResults.PageIndex;
            gvResults.DataSource = SortDataTable(dt, false);
            /*gvResults.DataSource = SortDataTable(gvResults.DataSource as DataTable, false);*/
            gvResults.DataBind();
            gvResults.PageIndex = pageIndex;
        }

        protected DataTable GetPopulatedDataTable(bool shouldBind)
        { 
            ArrayList al = PopulateGridView(shouldBind);
            GridViewClass gridView = new GridViewClass();
            gridView.AutoNestedColumns = true;
            return gridView.CreateDataTable(al);
        }

        protected ArrayList PopulateGridView(bool shouldBind)
        {
            ArrayList productList = new ArrayList();
            int categoryID = 0;
            string searchTerm = "";
            try
            {
                if (Request.Params["Search"] != null)
                    searchTerm = Request.Params["Search"].ToString();
                if (Request.Params["SearchCategoryId"] != null)
                    int.TryParse(Request.Params["SearchCategoryId"].ToString(), out categoryID);
                if ((Request.Params["Search"] != null) && (Request.Params["SearchCategoryId"] != null))
                {
                    GetSearchedProducts(categoryID, searchTerm, productList);
                    //productList = truncateList(productList, int.Parse(moduleSettings.NewProducts.RowCount) * int.Parse(moduleSettings.NewProducts.ColumnCount));
                    if (productList.Count > 0)
                    {
                        Label1.Text = "your results";
                        Label1.Visible = true;
                        gvResults.DataSource = productList;
                        if (shouldBind)
                            gvResults.DataBind();
                    }
                    else
                    {
                        Label1.Text = "no results found";
                        Label1.Visible = true;
                    }

                }
                else
                {

                    /*
                    // Get selected values
                    string continent = DropDownList1.SelectedItem.Text;
                    string country = DropDownList2.SelectedItem.Text;
                    //labelLog.Text = labelLog.Text + "<br>DD1: " + continent + " DD2: " + country + DateTime.Now.ToString();
                    int cat1 = -1;
                    int cat2 = -1;
                    int.TryParse(DropDownList1.SelectedValue, out cat1);
                    int.TryParse(DropDownList2.SelectedValue, out cat2);
                    //labelLog.Text = labelLog.Text + "<br>Populate GV dd1: " + cat1.ToString() + " dd2: " + cat2.ToString() + DateTime.Now.ToString();

                    // Output result string based on which values are specified
                    if (string.IsNullOrEmpty(continent))
                    {
                        Label1.Text = "Please select a continent.";
                    }
                    else if (string.IsNullOrEmpty(country))
                    {
                        Label1.Text = "Please select a country.";
                    }
                    else
                    {*/
                    //Label1.Text = string.Format("You have chosen {0} {1}. ", continent, country);

                        CategoryInfo categoryInfo = new CategoryInfo();
                        CategoryInfo categoryInfo2 = new CategoryInfo();
                        CategoryInfo categoryInfo3 = new CategoryInfo();
                        CategoryController categoryController = new CategoryController();
                        categoryInfo = categoryController.GetCategory(catalogNav.CategoryID);
                        categoryInfo2 = categoryController.GetCategory(catalogNav.CategoryID2);
                        categoryInfo3 = categoryController.GetCategory(catalogNav.CategoryID3);
                        //Response.Write("category: " + categoryInfo.CategoryID + " name: " + categoryInfo.CategoryName);
                        //labelCategoryName.Text = categoryInfo.CategoryName + " in " + categoryInfo2.CategoryName + " / " + categoryInfo3.CategoryName;
                        if ((categoryInfo != null) && (categoryInfo2 != null) && (categoryInfo3 != null))
                        {
                            labelCategoryName.Text = "<u><a href='" + FixHyperlinkCatalog(catalogNav, 1) +
                                "' class='demoHeadingLink'>" + categoryInfo.CategoryName + "</a></u> in " +
                                "<u><a href='" + FixHyperlinkCatalog(catalogNav, 2) + "' class='demoHeadingLink'>" + categoryInfo2.CategoryName + "</a></u> / " +
                                "<u><a href='" + FixHyperlinkCatalog(catalogNav, 3) + "' class='demoHeadingLink'>" + categoryInfo3.CategoryName + "</a></u>";


                            Label1.Text = "your results";
                            //Response.Write("dd1: " + DropDownList1.Items.Count + " dd2: " + DropDownList2.Items.Count);
                            //if ((DropDownList1.Items.Count > 1) && (DropDownList2.Items.Count > 1) && (catalogNav.CategoryID != Null.NullInteger))
                            {
                                /*
                                int categoryID1 = catalogNav.CategoryID;
                                int categoryID2 = int.Parse(DropDownList1.SelectedValue);
                                int categoryID3 = int.Parse(DropDownList2.SelectedValue);
                                */
                                int categoryID1 = catalogNav.CategoryID;
                                int categoryID2 = catalogNav.CategoryID2;
                                int categoryID3 = catalogNav.CategoryID3;

                                GetSelectedProducts(-1, categoryID1, categoryID2, categoryID3, productList);

                                gvResults.DataSource = productList;
                                if (shouldBind)
                                    gvResults.DataBind();

                                if (productList.Count == 0)
                                {
                                    Label1.Text = "no results found";
                                    Label1.Visible = true;
                                }

                            }
                        }
                    //}
                }
            }
            catch (Exception ex)
            {
                Response.Write("<br>" + ex.Message);
                Response.Write("<br>" + ex.StackTrace);
            }
            return productList;
        }


        protected void gvDECart_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
/*            
            //PopulateGridView(true);
            DataTable dt = GetPopulatedDataTable(true);
            //labelLog.Text = labelLog.Text + "<br>-----------------------> gvResults_PageIndexChanging " + gvResults.PageIndex + " " + DateTime.Now.ToString();
            gvResults.DataSource = SortDataTable(dt, true);
            gvResults.PageIndex = e.NewPageIndex;
            gvResults.DataBind();
            //labelLog.Text = labelLog.Text + "<br>-----------------------> gvResults_PageIndexChanging Bound " + gvResults.PageIndex + " " + DateTime.Now.ToString();
  */
        }

        protected void gvDECart_Sorting(object sender, GridViewSortEventArgs e)
        {
            /*
            DataTable dt = GetPopulatedDataTable(true);
            //labelLog.Text = labelLog.Text + "<br>-----------> gvResults_Sorting " + gvResults.PageIndex + " " + DateTime.Now.ToString();
            GridViewSortExpression = e.SortExpression;
            int pageIndex = gvResults.PageIndex;
            gvResults.DataSource = SortDataTable(dt, false);
            gvResults.DataBind();
            gvResults.PageIndex = pageIndex;
            */
        }

        /*
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static CascadingDropDownNameValue[] GetDropDownContentsPageMethod(string knownCategoryValues, string category)
        {
            return new CarsService().GetDropDownContents(knownCategoryValues, category);
        }
        */

        protected void imgAddDECart_OnClick(object sender, ImageClickEventArgs e)
        {
            //labelLog.Text = labelLog.Text + "<br>selected location: " + ddListLocation.SelectedValue + " category: " + ddListCategory.SelectedValue;
            String addedDE = ddListLocation.SelectedValue + ":" + ddListCategory.SelectedValue;
            String selDE = (String)Session["selectedDE"];
            if (selDE == null || selDE == "")
                selDE = ";";
            if (selDE.IndexOf(";" + addedDE + ";") == -1)
            {
                Session["selectedDE"] = selDE + addedDE + ";";
                //Label2.Text = Label2.Text + "<br>selected location: " + ddListLocation.SelectedValue + " category: " + ddListCategory.SelectedValue;
                System.Collections.ArrayList prods = PopulateDEGridView(true);
                int numProds = prods.Count;
                Label2.Text = "Total cart cost: &pound;" + numProds * 50 + "";
                Session["selectedDECost"] = new Decimal(numProds * 50);
            }
            else
            {
                //labelError.Text = "Data Extract already selected";
                //this.Page.RegisterClientScriptBlock("alert_already_selected", "<script language=javascript> alert('Data Extract already selected');</script>");

                //String js = "<script language=javascript> alert('Data Extract already selected.');<" + "/script>";
                //this.Page.RegisterClientScriptBlock("alert_already_selected", js);
                String js = "alert('Category Volume already selected.');";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert_already_selected", js, true);
            }
            //phAddToCart.Visible = true;
        }

        protected ArrayList PopulateDEGridView(bool shouldBind)
        {
            ArrayList productList = new ArrayList();
            if (Session["selectedDE"] != null)
            {
                try
                {
                    String selectedDEStr = (String)Session["selectedDE"];
                    char[] splitter = { ';' };
                    char[] splitter2 = { ':' };
                    String[] selectedDEs = selectedDEStr.Split(splitter);
                    CategoryController categoryController = new CategoryController();
                    foreach (String selectedDE in selectedDEs)
                    {
                        if (selectedDE != "")
                        {

                            String[] options = selectedDE.Split(splitter2);
                            int cat1 = -1;
                            int.TryParse(options[0], out cat1);
                            int cat2 = -1;
                            int.TryParse(options[1], out cat2);


                            CategoryInfo catg1 = categoryController.GetCategory(cat1);
                            CategoryInfo catg2 = categoryController.GetCategory(cat2);
                            //Response.Write("<!-- cat1 {" + cat1 + "} -->");
                            //Response.Write("<!-- cat2 {" + cat2 + "} -->");
                            //Response.Write("<!-- catg1 {" + catg1.CategoryID + "} {" + catg1.CategoryName + "} -->");
                            //Response.Write("<!-- catg2 {" + catg2.CategoryID + "} {" + catg2.CategoryName + "} -->");

                            DEProductInfo prod = new DEProductInfo(catg1.CategoryID, catg1.CategoryName, catg2.CategoryID, catg2.CategoryName);
                            productList.Add(prod);
                            //productList.Add(catg1);
                        }
                    }
                    gvDECart.DataSource = productList;
                    if (shouldBind)
                        gvDECart.DataBind();

                }
                catch (Exception ex)
                {
                    Response.Write("<!-- Error with selectedDE {" + Session["selectedDE"] + "} " + ex.Message);
                    Response.Write("<br>" + ex.StackTrace + "-->");
                }
            }
            return productList;
        }

        protected void imgResetDECart_OnClick(object sender, ImageClickEventArgs e)
        {
            ResetDEGridView();
        }

        protected void imgAddCart_OnClick(object sender, ImageClickEventArgs e)
        {
            if (Session["selectedDE"] != null && Session["selectedDE"] != "")
            {
                string modelNumber = (String)Session["selectedDE"];
                string modelName = tbDEReference.Text;
                decimal cost = 0;
                if (Session["selectedDECost"] != null)
                    cost = (Decimal)Session["selectedDECost"];
                DotNetNuke.Modules.Store.Cart.CurrentCart.AddItem(PortalId, -1, 1, -1, modelNumber, modelName, cost);

                ResetDEGridView();

                ddListLocation.ClearSelection();
                ddListCategory.ClearSelection();
                ddListLocation.SelectedIndex = 0;
                ddListCategory.SelectedIndex = 0;

                // Hack to clear the selected location / categories 
                Response.Redirect("/Shop/BuyDataExtracts/tabid/97/Default.aspx");
            }
            else
            {
                //String js = "<script language=javascript> alert('You must select a Data Extract first.');<" + "/script>";
                //this.Page.RegisterClientScriptBlock("alert_must_select", js);

                String js = "alert('You must select a Category Volume first.');";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert_must_select", js, true);
            }
        }

        protected void ResetDEGridView()
        {
            Session["selectedDE"] = "";
            Session["selectedDECost"] = "0";
            System.Collections.ArrayList prods = PopulateDEGridView(true);
            int numProds = prods.Count;
            Session["selectedDECost"] = new Decimal(numProds * 50);
            Label2.Text = "Total cart cost: &pound;" + numProds * 50 + "";
        }

        //private void imgGo_Click(object sender, EventArgs e)
        private void imgGo_Click(object sender, ImageClickEventArgs e)
        {
            string separator = "";
            string searchText = txtSearch.Text;
            int SearchTabid = 95;
            if(DotNetNuke.Entities.Host.HostSettings.GetHostSetting("UseFriendlyUrls") == "Y")
                separator = "?";
            else
                separator = "&";

            StringDictionary replaceParams = new StringDictionary();
            //replaceParams["Search"] = searchText;
            replaceParams["categoryID"] = Null.NullString;
            //Response.Redirect(catalogNav.GetNavigationUrl(replaceParams, SearchTabid));
            Response.Redirect(catalogNav.GetNavigationUrl(replaceParams, SearchTabid) + separator + "Search=" + searchText + "&SearchCategoryId=" + ddSearchCategory.SelectedItem.Value);
        }

        protected void gvResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink hl = (HyperLink) e.Row.FindControl("imgContent");
                //if(hl.NavigateUrl.EndsWith("/documents/"))
                if (hl.NavigateUrl.EndsWith("/documents/") || hl.NavigateUrl.EndsWith("/documents/_no_report_file_available.pdf"))
                    hl.Visible = false;
            }
        }
	}
}
