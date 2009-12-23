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
using System.Reflection;
using System.Runtime.CompilerServices;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Summary description for Defaults.
	/// </summary>
	public class ModuleSettings
	{
		public GeneralSettings General;
        public NewProductsSettings NewProducts;
        public FeaturedProductsSettings FeaturedProducts;
		public PopularProductsSettings PopularProducts;
		public CategoryProductsSettings CategoryProducts;
		public ProductDetailSettings ProductDetail;
		public CategoryMenuSettings CategoryMenu;

		public ModuleSettings(int moduleId, int tabId)
		{
			General = new GeneralSettings(moduleId, tabId);
            NewProducts = new NewProductsSettings(moduleId, tabId);
            FeaturedProducts = new FeaturedProductsSettings(moduleId, tabId);
			PopularProducts = new PopularProductsSettings(moduleId, tabId);
			CategoryProducts = new CategoryProductsSettings(moduleId, tabId);
			ProductDetail = new ProductDetailSettings(moduleId, tabId);
			CategoryMenu = new CategoryMenuSettings(moduleId, tabId);
		}
	}

	#region General Settings
	public class GeneralSettings : SettingsWrapper
	{
		[ModuleSetting("template", "Catalog.htm")]
		public string Template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("usedefaultcategory", "false")]
		public string UseDefaultCategory
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("defaultcategoryid", "0")]
		public string DefaultCategoryID
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("showmessage", "true")]
		public string ShowMessage
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

        [ModuleSetting("shownewproducts", "true")]
        public string ShowNewProducts
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

		[ModuleSetting("showfeaturedproducts", "true")]
		public string ShowFeaturedProducts
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("showpopularproducts", "true")]
		public string ShowPopularProducts
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("showcategoryproducts", "true")]
		public string ShowCategoryProducts
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("showproductdetail", "true")]
		public string ShowProductDetail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		public GeneralSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}
	}
	#endregion

    #region New Product Settings

    public class NewProductsSettings : SettingsWrapper
    {
        [ModuleSetting("nplcontainertemplate", "ListContainer.htm")]
        public string ContainerTemplate
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("npltemplate", "NewProduct.htm")]
        public string Template
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("nplrowcount", "10")]
        public string RowCount
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("nplcolumncount", "2")]
        public string ColumnCount
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("nplcolumnwidth", "200")]
        public string ColumnWidth
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("nplrepeatdirection", "H")]
        public string RepeatDirection
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("nplshowthumbnail", "true")]
        public string ShowThumbnail
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("nplthumbnailwidth", "90")]
        public string ThumbnailWidth
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("npldetailtabid", "0")]
        public string DetailPage
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        public NewProductsSettings(int moduleId, int tabId)
            : base(moduleId, tabId)
        {
        }
    }
    #endregion

	#region Featured Product Settings
	public class FeaturedProductsSettings : SettingsWrapper
	{
        [ModuleSetting("fplcontainertemplate", "ListContainer.htm")]
        public string ContainerTemplate
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("fpltemplate", "FeaturedProduct.htm")]
		public string Template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("fplrowcount", "10")]
		public string RowCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("fplcolumncount", "2")]
		public string ColumnCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("fplcolumnwidth", "200")]
		public string ColumnWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

        [ModuleSetting("fplrepeatdirection", "H")]
        public string RepeatDirection
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("fplshowthumbnail", "true")]
		public string ShowThumbnail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("fplthumbnailwidth", "90")]
		public string ThumbnailWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("fpldetailtabid", "0")]
		public string DetailPage
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		public FeaturedProductsSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}
	}
	#endregion

	#region Popular Product Settings
	public class PopularProductsSettings : SettingsWrapper
	{
        [ModuleSetting("pplcontainertemplate", "ListContainer.htm")]
        public string ContainerTemplate
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("ppltemplate", "PopularProduct.htm")]
		public string Template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("pplrowcount", "10")]
		public string RowCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("pplcolumncount", "2")]
		public string ColumnCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("pplcolumnwidth", "200")]
		public string ColumnWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

        [ModuleSetting("pplrepeatdirection", "H")]
        public string RepeatDirection
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("pplshowthumbnail", "true")]
		public string ShowThumbnail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("pplthumbnailwidth", "90")]
		public string ThumbnailWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("ppldetailtabid", "0")]
		public string DetailPage
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		public PopularProductsSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}
	}
	#endregion

	#region Category Product Settings
	public class CategoryProductsSettings : SettingsWrapper
	{
        [ModuleSetting("cplcontainertemplate", "CategoryContainer.htm")]
        public string ContainerTemplate
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

		[ModuleSetting("cpltemplate", "ProductList.htm")]
		public string Template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("cplrowcount", "10")]
		public string RowCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("cplcolumncount", "2")]
		public string ColumnCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("cplcolumnwidth", "200")]
		public string ColumnWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

        [ModuleSetting("cplrepeatdirection", "H")]
        public string RepeatDirection
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("cplshowthumbnail", "true")]
		public string ShowThumbnail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("cplthumbnailwidth", "90")]
		public string ThumbnailWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("cpldetailtabid", "0")]
		public string DetailPage
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		public CategoryProductsSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}
	}
	#endregion

	#region Product Detail Settings
	public class ProductDetailSettings : SettingsWrapper
	{
		[ModuleSetting("detailtemplate", "ProductDetail.htm")]
		public string Template
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("detailshowthumbnail", "true")]
		public string ShowThumbnail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("detailthumbnailwidth", "300")]
		public string ThumbnailWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

        [ModuleSetting("detailshowreviews", "true")]
        public string ShowReviews
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

        [ModuleSetting("pdlcategoriestabid", "0")]
        public string ReturnPage
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                return getSetting(m);
            }
            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                MethodBase m = MethodBase.GetCurrentMethod();
                setSetting(m, value);
            }
        }

		public ProductDetailSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}
	}
	#endregion

	#region Category Menu Settings

	public class CategoryMenuSettings : SettingsWrapper
	{
		public CategoryMenuSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}

		[ModuleSetting("MenuColumnCount", "1")]
		public string ColumnCount
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("MenuCatalogTabId", "0")]
		public string CatalogPage
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}
	}
	#endregion
}
