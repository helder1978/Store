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
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Text;
using DotNetNuke;
using DotNetNuke.Entities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Search;

using DotNetNuke.Services.Mail;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Summary description for MediaController.
	/// </summary>
    public class ProductController : PortalModuleBase, Entities.Modules.IPortable, ISearchable
	{ 
		#region Constructors
		public ProductController()
		{
		}
		#endregion

		#region Public Functions
		public ArrayList GetPortalAllProducts(int portalID) 
		{ 
			return CBO.FillCollection(DataProvider.Instance().GetPortalAllProducts(portalID), typeof(ProductInfo)); 
		} 

		public ArrayList GetPortalProducts(int portalID, bool featured, bool archived) 
		{ 
			return CBO.FillCollection(DataProvider.Instance().GetPortalProducts(portalID, featured, archived), typeof(ProductInfo)); 
		} 

		public ArrayList GetPortalFeaturedProducts(int portalID, bool archived) 
		{ 
			return CBO.FillCollection(DataProvider.Instance().GetPortalFeaturedProducts(portalID, archived), typeof(ProductInfo)); 
		}

        public ArrayList GetPortalNewProducts(int portalID, bool archived)
        {
            return CBO.FillCollection(DataProvider.Instance().GetPortalNewProducts(portalID, archived), typeof(ProductInfo));
        } 

		public ArrayList GetCategoryProducts(int categoryID, bool archived) 
		{ 
			return CBO.FillCollection(DataProvider.Instance().GetCategoryProducts(categoryID, archived), typeof(ProductInfo)); 
		} 

		public ArrayList GetFeaturedProducts(int categoryID, bool archived) 
		{ 
			return CBO.FillCollection(DataProvider.Instance().GetFeaturedProducts(categoryID, archived), typeof(ProductInfo)); 
		}

        public ArrayList GetNewProducts(int categoryID, bool archived)
        {
            return CBO.FillCollection(DataProvider.Instance().GetNewProducts(categoryID, archived), typeof(ProductInfo));
        }

        public ArrayList GetSelectedProducts(int categoryID, int categoryID1, int categoryID2, int categoryID3, bool archived)
        {
            return CBO.FillCollection(DataProvider.Instance().GetSelectedProducts(categoryID, categoryID1, categoryID2, categoryID3, archived), typeof(ProductInfo));
        }

        public ArrayList GetRelatedProducts(int categoryID, int categoryID1, int categoryID2, int categoryID3, int currentProductId, bool archived)
        {
            return CBO.FillCollection(DataProvider.Instance().GetRelatedProducts(categoryID, categoryID1, categoryID2, categoryID3, currentProductId, archived), typeof(ProductInfo));
        }

        public ArrayList GetRelatedNewsProducts(int categoryID, int categoryID1, int categoryID2, int categoryID3, int currentProductId, bool archived)
        {


            return CBO.FillCollection(DataProvider.Instance().GetRelatedNewsProducts(categoryID, categoryID1, categoryID2, categoryID3, currentProductId, archived), typeof(ProductInfo));
        }

        public ArrayList GetSearchedProducts(int categoryID, string searchTerm, bool archived)
        {
            return CBO.FillCollection(DataProvider.Instance().GetSearchedProducts(categoryID, searchTerm, archived), typeof(ProductInfo));
        }

        public ArrayList GetPopularProducts(int portalID, int categoryID, bool archived) 
		{ 
			return CBO.FillCollection(DataProvider.Instance().GetPopularProducts(portalID, categoryID, archived), typeof(ProductInfo)); 
		} 

		public ArrayList GetPortalPopularProducts(int portalID, bool archived) 
		{ 
			return CBO.FillCollection(DataProvider.Instance().GetPortalPopularProducts(portalID, archived), typeof(ProductInfo)); 
		} 

		public ArrayList GetAlsoBoughtProducts(int portalID, int productID, bool archived) 
		{ 
			return CBO.FillCollection(DataProvider.Instance().GetAlsoBoughtProducts(portalID, productID, archived), typeof(ProductInfo)); 
		} 

		public ProductInfo GetProduct(int productID) 
		{ 
			return ((ProductInfo)(CBO.FillObject(DataProvider.Instance().GetProduct(productID), typeof(ProductInfo)))); 
		}

		public void AddProduct(ProductInfo productInfo)
		{
            /*  // canadean changed
			DataProvider.Instance().AddProduct(productInfo.PortalID, productInfo.CategoryID, 
				productInfo.Manufacturer, productInfo.ModelNumber, productInfo.ModelName, 
				productInfo.ProductImage, productInfo.UnitCost, productInfo.Summary, 
				productInfo.Description, productInfo.Featured, productInfo.Archived, 
				productInfo.CreatedByUser, productInfo.CreatedDate, productInfo.ProductWeight, 
                productInfo.ProductHeight, productInfo.ProductLength, productInfo.ProductWidth, 
                productInfo.SaleStartDate, productInfo.SaleEndDate, productInfo.SalePrice);
             */
            /*
			DataProvider.Instance().AddProduct(productInfo.PortalID, productInfo.CategoryID, 
				productInfo.Manufacturer, productInfo.ModelNumber, productInfo.ModelName, 
				productInfo.ProductImage, productInfo.UnitCost, productInfo.Summary, 
				productInfo.Description, productInfo.Featured, productInfo.Archived, 
				productInfo.CreatedByUser, productInfo.CreatedDate, productInfo.ProductWeight, 
                productInfo.ProductHeight, productInfo.ProductLength, productInfo.ProductWidth, 
                productInfo.SaleStartDate, productInfo.SaleEndDate, productInfo.SalePrice,
                productInfo.CategoryID1, productInfo.CategoryID2, productInfo.CategoryID3, 
                productInfo.NumPages, productInfo.PublishDate, productInfo.DeliveryMethod, 
                productInfo.AvailableOnline, productInfo.ProductFile, productInfo.ProductPreview, 
                productInfo.ProductImage2, productInfo.ProductImage3, productInfo.DescriptionTag,
                productInfo.TOC_Html, productInfo.PriceStr);*/
			DataProvider.Instance().AddProduct(productInfo.PortalID, productInfo.CategoryID, 
				productInfo.Manufacturer, productInfo.ModelNumber, productInfo.ModelName, 
				productInfo.ProductImage, productInfo.UnitCost, productInfo.Summary,
                productInfo.Description, productInfo.DescriptionTwo, productInfo.DescriptionThree, 
                    productInfo.Featured, productInfo.Archived, 
				productInfo.CreatedByUser, productInfo.CreatedDate, productInfo.ProductWeight, 
                productInfo.ProductHeight, productInfo.ProductLength, productInfo.ProductWidth, 
                productInfo.SaleStartDate, productInfo.SaleEndDate, productInfo.SalePrice,
                productInfo.CategoryID1, productInfo.CategoryID2, productInfo.CategoryID3, 
                productInfo.NumPages, productInfo.PublishDate, productInfo.DeliveryMethod, 
                productInfo.AvailableOnline, productInfo.ProductFile, productInfo.ProductPreview, 
                productInfo.ProductImage2, productInfo.ProductImage3, productInfo.DescriptionTag,
                productInfo.TOC_Html, productInfo.PriceStr);
		}

		public void UpdateProduct(ProductInfo productInfo)
		{
            /*  // canadean changed
                DataProvider.Instance().UpdateProduct(productInfo.ProductID, productInfo.CategoryID, 
				productInfo.Manufacturer, productInfo.ModelNumber, productInfo.ModelName, 
				productInfo.ProductImage, productInfo.UnitCost, productInfo.Summary,
                productInfo.Description, productInfo.Featured, productInfo.Archived, productInfo.ProductWeight, 
                productInfo.ProductHeight, productInfo.ProductLength, productInfo.ProductWidth, 
                productInfo.SaleStartDate, productInfo.SaleEndDate, productInfo.SalePrice);
 */
            /*
            DataProvider.Instance().UpdateProduct(productInfo.ProductID, productInfo.CategoryID, 
				productInfo.Manufacturer, productInfo.ModelNumber, productInfo.ModelName, 
				productInfo.ProductImage, productInfo.UnitCost, productInfo.Summary,
                productInfo.Description, productInfo.Featured, productInfo.Archived, productInfo.ProductWeight, 
                productInfo.ProductHeight, productInfo.ProductLength, productInfo.ProductWidth, 
                productInfo.SaleStartDate, productInfo.SaleEndDate, productInfo.SalePrice,
                productInfo.CategoryID1, productInfo.CategoryID2, productInfo.CategoryID3, 
                productInfo.NumPages, productInfo.PublishDate, productInfo.DeliveryMethod, 
                productInfo.AvailableOnline, productInfo.ProductFile, productInfo.ProductPreview,
                productInfo.ProductImage2, productInfo.ProductImage3, productInfo.DescriptionTag,
                productInfo.TOC_Html, productInfo.PriceStr);*/
            DataProvider.Instance().UpdateProduct(productInfo.ProductID, productInfo.CategoryID, 
				productInfo.Manufacturer, productInfo.ModelNumber, productInfo.ModelName, 
				productInfo.ProductImage, productInfo.UnitCost, productInfo.Summary,
                productInfo.Description, productInfo.DescriptionTwo, productInfo.DescriptionThree, 
                    productInfo.Featured, productInfo.Archived, productInfo.ProductWeight, 
                productInfo.ProductHeight, productInfo.ProductLength, productInfo.ProductWidth, 
                productInfo.SaleStartDate, productInfo.SaleEndDate, productInfo.SalePrice,
                productInfo.CategoryID1, productInfo.CategoryID2, productInfo.CategoryID3, 
                productInfo.NumPages, productInfo.PublishDate, productInfo.DeliveryMethod, 
                productInfo.AvailableOnline, productInfo.ProductFile, productInfo.ProductPreview,
                productInfo.ProductImage2, productInfo.ProductImage3, productInfo.DescriptionTag,
                productInfo.TOC_Html, productInfo.PriceStr);

		}

		public void DeleteProduct(int productID)
		{
			DataProvider.Instance().DeleteProduct(productID);
		}
		#endregion

		#region ISearchable Members

        public SearchItemInfoCollection GetSearchItems(ModuleInfo moduleInfo)
		{
			// Create search item collection
			SearchItemInfoCollection searchItemList = new SearchItemInfoCollection();

            string subject = "Canadean - Indexing executed for module " + moduleInfo.ModuleID;
            if (moduleInfo.ModuleID == 422) // Only index through the main shop page navigation (possibilities: 422 [Shop,Reports], 514 [Data Extracts / Volumes by Beverage/Country], 621 [Shop/Search])
			{
				// Get all products
				//ArrayList productList = GetPortalAllProducts(moduleInfo.PortalID);
				//ArrayList productList = GetPortalProducts(moduleInfo.PortalID, false, false);   // canadean change: only index products that aren't archived 
				ArrayList productList = GetCategoryProducts(4, false);   // canadean change: only index products that aren't archived (and real products [categoryId = 4], not DE [categoryId = 2])
				foreach(ProductInfo product in productList)
				{
					// Get user identifier
					int userID = Null.NullInteger;
					userID = int.Parse(product.CreatedByUser);

					// Create title
					string title = System.Web.HttpUtility.HtmlDecode(product.ProductTitle);

					// Create content
					string content = System.Web.HttpUtility.HtmlDecode(title) + ": " + System.Web.HttpUtility.HtmlDecode(product.Description) + " " + System.Web.HttpUtility.HtmlDecode(product.Summary);

					SearchItemInfo searchItem = new SearchItemInfo(title,
						System.Web.HttpUtility.HtmlDecode(product.Summary), userID, product.CreatedDate, moduleInfo.ModuleID,
						product.ProductID.ToString(), content, "ProductID=" + product.ProductID.ToString());

					searchItemList.Add(searchItem);
				}
			}
            string body = "Number of products added to index: " + searchItemList.Count;
            Mail.SendMail("info@canadean.com", "helder1978@gmail.com", "", subject, body, "", "", "", "", "", "");
            return searchItemList;
		}

		#endregion

        #region IPortable Members ==========================================

        string DotNetNuke.Entities.Modules.IPortable.ExportModule(int ModuleID)
        {
            StringBuilder strXML = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            XmlWriter Writer = XmlWriter.Create(strXML, settings);

            // Export des catégories
            CategoryController categoriesControler = new CategoryController();
            ArrayList alCategories = categoriesControler.GetCategories(PortalId, true, -3);

            if (alCategories.Count > 0)
            {
                Writer.WriteStartElement("Catalog");
                Writer.WriteStartElement("Categories");
                foreach (CategoryInfo categoryInfo in alCategories)
                {
                    Writer.WriteStartElement("Category");
                    Writer.WriteElementString("CategoryID", categoryInfo.CategoryID.ToString());
                    Writer.WriteElementString("Name", categoryInfo.CategoryName);
                    Writer.WriteElementString("Description", categoryInfo.CategoryDescription);
                    Writer.WriteElementString("Message", categoryInfo.Message);
                    Writer.WriteElementString("Archived", categoryInfo.Archived.ToString());
                    Writer.WriteElementString("OrderID", categoryInfo.OrderID.ToString());
                    Writer.WriteElementString("ParentCategoryID", categoryInfo.ParentCategoryID.ToString());
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();

                ArrayList alProducts = GetPortalAllProducts(PortalId);
                CultureInfo invariantCulture = CultureInfo.InvariantCulture;
                string strProductImage = Null.NullString;

                if (alProducts.Count > 0)
                {
                    Writer.WriteStartElement("Products");
                    foreach (ProductInfo productInfo in alProducts)
                    {
                        Writer.WriteStartElement("Product");
                        Writer.WriteElementString("CategoryID", productInfo.CategoryID.ToString());
                        Writer.WriteElementString("Manufacturer", productInfo.Manufacturer);
                        Writer.WriteElementString("ModelName", productInfo.ModelName);
                        Writer.WriteElementString("ModelNumber", productInfo.ModelNumber);
                        strProductImage = productInfo.ProductImage.Replace("Portals/" + PortalId.ToString(), "[PortalId]");
                        Writer.WriteElementString("ProductImage", strProductImage);
                        Writer.WriteElementString("UnitCost", productInfo.UnitCost.ToString("0.00", invariantCulture));
                        Writer.WriteElementString("Summary", productInfo.Summary);
                        Writer.WriteElementString("Description", productInfo.Description);
                        Writer.WriteElementString("Archived", productInfo.Archived.ToString());
                        Writer.WriteElementString("Featured", productInfo.Featured.ToString());
                        Writer.WriteElementString("ProductWeight", productInfo.ProductWeight.ToString("0.00", invariantCulture));
                        Writer.WriteElementString("ProductHeight", productInfo.ProductHeight.ToString("0.00", invariantCulture));
                        Writer.WriteElementString("ProductLength", productInfo.ProductLength.ToString("0.00", invariantCulture));
                        Writer.WriteElementString("ProductWidth", productInfo.ProductWidth.ToString("0.00", invariantCulture));
                        if (productInfo.SaleStartDate != Null.NullDate)
                        {
                            Writer.WriteElementString("SaleStartDate", productInfo.SaleStartDate.ToString(invariantCulture));
                        }
                        else
                        {
                            Writer.WriteElementString("SaleStartDate", "");
                        }
                        if (productInfo.SaleEndDate != Null.NullDate)
                        {
                            Writer.WriteElementString("SaleEndDate", productInfo.SaleEndDate.ToString(invariantCulture));
                        }
                        else
                        {
                            Writer.WriteElementString("SaleEndDate", "");
                        }
                        if (productInfo.SalePrice != Null.NullDecimal)
                        {
                            Writer.WriteElementString("SalePrice", productInfo.SalePrice.ToString("0.00", invariantCulture));
                        }
                        else
                        {
                            Writer.WriteElementString("SalePrice", "");
                        }
                        Writer.WriteEndElement();
                    }
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();
                Writer.Close();
            }
            else
            {
                return String.Empty;
            }
            return strXML.ToString();
        }

        void DotNetNuke.Entities.Modules.IPortable.ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            XmlNode xmlCategories = DotNetNuke.Common.Globals.GetContent(Content, "Catalog/Categories");
            SortedList slCategories = new SortedList(xmlCategories.ChildNodes.Count);
            int intIndexID = Null.NullInteger;

            CategoryController categoriesControler = new CategoryController();

            foreach (XmlNode xmlCategory in xmlCategories)
            {
                CategoryInfo categoryInfo = new CategoryInfo();
                categoryInfo.PortalID = PortalId;
                categoryInfo.CategoryName = xmlCategory["Name"].InnerText;
                categoryInfo.CategoryDescription = xmlCategory["Description"].InnerText;
                categoryInfo.Message = xmlCategory["Message"].InnerText;
                categoryInfo.Archived = Convert.ToBoolean(xmlCategory["Archived"].InnerText);
                categoryInfo.CreatedByUser = UserId.ToString();
                categoryInfo.CreatedDate = DateTime.Now;
                categoryInfo.OrderID = Convert.ToInt32(xmlCategory["OrderID"].InnerText);
                intIndexID = slCategories.IndexOfKey(Convert.ToInt32(xmlCategory["ParentCategoryID"].InnerText));
                if (intIndexID > -1)
                {
                    categoryInfo.ParentCategoryID = (int)slCategories.GetByIndex(intIndexID);
                }
                else
                {
                    categoryInfo.ParentCategoryID = -1;
                }
                slCategories.Add(Convert.ToInt32(xmlCategory["CategoryID"].InnerText), categoriesControler.AddCategory(categoryInfo));
            }

            XmlNode xmlProducts = DotNetNuke.Common.Globals.GetContent(Content, "Catalog/Products");

            string strValue = Null.NullString;
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;

            foreach (XmlNode xmlProduct in xmlProducts)
            {
                ProductInfo productInfo = new ProductInfo();
                productInfo.PortalID = PortalId;
                productInfo.CategoryID = (int)slCategories.GetByIndex(slCategories.IndexOfKey(Convert.ToInt32(xmlProduct["CategoryID"].InnerText)));
                productInfo.Manufacturer = xmlProduct["Manufacturer"].InnerText;
                productInfo.ModelName = xmlProduct["ModelName"].InnerText;
                productInfo.ModelNumber = xmlProduct["ModelNumber"].InnerText;
                strValue = xmlProduct["ProductImage"].InnerText;
                productInfo.ProductImage = strValue.Replace("[PortalId]", "Portals/" + PortalId.ToString());
                productInfo.UnitCost = Convert.ToDecimal(xmlProduct["UnitCost"].InnerText, invariantCulture);
                productInfo.Summary = xmlProduct["Summary"].InnerText;
                productInfo.Description = xmlProduct["Description"].InnerText;
                productInfo.Archived = Convert.ToBoolean(xmlProduct["Archived"].InnerText);
                productInfo.Featured = Convert.ToBoolean(xmlProduct["Featured"].InnerText);
                productInfo.CreatedByUser = UserId.ToString();
                productInfo.CreatedDate = DateTime.Now;
                productInfo.ProductWeight = Convert.ToDecimal(xmlProduct["ProductWeight"].InnerText, invariantCulture);
                productInfo.ProductHeight = Convert.ToDecimal(xmlProduct["ProductHeight"].InnerText, invariantCulture);
                productInfo.ProductLength = Convert.ToDecimal(xmlProduct["ProductLength"].InnerText, invariantCulture);
                productInfo.ProductWidth = Convert.ToDecimal(xmlProduct["ProductWidth"].InnerText, invariantCulture);
                strValue = xmlProduct["SaleStartDate"].InnerText;
                if (strValue != Null.NullString)
                {
                    productInfo.SaleStartDate = Convert.ToDateTime(strValue, invariantCulture);
                }
                else
                {
                    productInfo.SaleStartDate = DateTime.Parse("01/01/9999");
                }
                strValue = xmlProduct["SaleEndDate"].InnerText;
                if (strValue != Null.NullString)
                {
                    productInfo.SaleEndDate = Convert.ToDateTime(strValue, invariantCulture);
                }
                else
                {
                    productInfo.SaleEndDate = DateTime.Parse("01/01/9999");
                }
                strValue = xmlProduct["SalePrice"].InnerText;
                if (strValue != Null.NullString)
                {
                    productInfo.SalePrice = Convert.ToDecimal(strValue, invariantCulture);
                }
                else
                {
                    productInfo.SalePrice = -1;
                }
                AddProduct(productInfo);
            }
        }
        #endregion
    }
}

