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
using System.Data;
using System.Data.SqlClient;
using DotNetNuke;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Summary description for DataProvider.
	/// </summary>
	public abstract class DataProvider
	{
		#region Private Members
		private static DataProvider objProvider = null; 
		#endregion

		#region Constructors
		static DataProvider() 
		{ 
			CreateProvider(); 
		} 

		private static void CreateProvider() 
		{ 
			objProvider = ((DataProvider)(DotNetNuke.Framework.Reflection.CreateObject("data", "DotNetNuke.Modules.Store.Catalog", "DotNetNuke.Modules.Store.Catalog"))); 
		} 

		public static DataProvider Instance() 
		{ 
			return objProvider; 
		}
		#endregion

		#region Abstract Functions
		// Categories
		public abstract int AddCategory(int PortalID, string CategoryName, string CategoryDescription, string Message, bool Archived, string CreatedByUser, DateTime CreatedDate, int OrderID, int ParentCategoryID);
		public abstract void UpdateCategory(int CategoryID, string CategoryName, string CategoryDescription, string Message, bool Archived, int OrderID, int ParentCategoryID);
		public abstract void DeleteCategories(int PortalID);
		public abstract void DeleteCategory(int CategoryID);
		public abstract int CategoryCount(int PortalID);
		public abstract IDataReader GetCategories(int PortalID, bool IncludeArchived, int ParentCategoryID);
        public abstract IDataReader GetCategoriesFromProducts(int PortalID, int CategoryID1, int CategoryID2, int CategoryID3, int ReturnCategory);
        public abstract IDataReader GetCategory(int CategoryID);

		// Products
        //public abstract int AddProduct(int PortalID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, bool Featured, bool Archived, string CreatedByUser, DateTime CreatedDate, Decimal ProductWeight, Decimal ProductHeight, Decimal ProductLength, Decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, Decimal SalePrice);
        //public abstract void UpdateProduct(int ProductID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, bool Featured, bool Archived, Decimal ProductWeight, Decimal ProductHeight, Decimal ProductLength, Decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, Decimal SalePrice);
        //public abstract int AddProduct(int PortalID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, bool Featured, bool Archived, string CreatedByUser, DateTime CreatedDate, Decimal ProductWeight, Decimal ProductHeight, Decimal ProductLength, Decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, Decimal SalePrice, int CategoryID1, int CategoryID2, int CategoryID3, int NumPages, DateTime PublishDate, int DeliveryMethod, bool AvailableOnline, string ProductFile, string ProductPreview, string ProductImage2, string ProductImage3);
        //public abstract void UpdateProduct(int ProductID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, bool Featured, bool Archived, Decimal ProductWeight, Decimal ProductHeight, Decimal ProductLength, Decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, Decimal SalePrice, int CategoryID1, int CategoryID2, int CategoryID3, int NumPages, DateTime PublishDate, int DeliveryMethod, bool AvailableOnline, string ProductFile, string ProductPreview, string ProductImage2, string ProductImage3);
        //public abstract int AddProduct(int PortalID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, bool Featured, bool Archived, string CreatedByUser, DateTime CreatedDate, Decimal ProductWeight, Decimal ProductHeight, Decimal ProductLength, Decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, Decimal SalePrice, int CategoryID1, int CategoryID2, int CategoryID3, int NumPages, DateTime PublishDate, int DeliveryMethod, bool AvailableOnline, string ProductFile, string ProductPreview, string ProductImage2, string ProductImage3, string DescriptionTag, string TOC_Html, string PriceStr);
        //public abstract void UpdateProduct(int ProductID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, bool Featured, bool Archived, Decimal ProductWeight, Decimal ProductHeight, Decimal ProductLength, Decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, Decimal SalePrice, int CategoryID1, int CategoryID2, int CategoryID3, int NumPages, DateTime PublishDate, int DeliveryMethod, bool AvailableOnline, string ProductFile, string ProductPreview, string ProductImage2, string ProductImage3, string DescriptionTag, string TOC_Html, string PriceStr);
        public abstract int AddProduct(int PortalID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, string DescriptionTwo, string DescriptionThree, bool Featured, bool Archived, string CreatedByUser, DateTime CreatedDate, Decimal ProductWeight, Decimal ProductHeight, Decimal ProductLength, Decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, Decimal SalePrice, int CategoryID1, int CategoryID2, int CategoryID3, int NumPages, DateTime PublishDate, int DeliveryMethod, bool AvailableOnline, string ProductFile, string ProductPreview, string ProductImage2, string ProductImage3, string DescriptionTag, string TOC_Html, string PriceStr);
        public abstract void UpdateProduct(int ProductID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, string DescriptionTwo, string DescriptionThree, bool Featured, bool Archived, Decimal ProductWeight, Decimal ProductHeight, Decimal ProductLength, Decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, Decimal SalePrice, int CategoryID1, int CategoryID2, int CategoryID3, int NumPages, DateTime PublishDate, int DeliveryMethod, bool AvailableOnline, string ProductFile, string ProductPreview, string ProductImage2, string ProductImage3, string DescriptionTag, string TOC_Html, string PriceStr);
        public abstract void DeleteProduct(int ProductID);
		public abstract IDataReader GetProduct(int ProductID);
		public abstract IDataReader GetPortalAllProducts(int PortalID);
		public abstract IDataReader GetPortalProducts(int PortalID, bool Featured, bool Archived);
		public abstract IDataReader GetPortalFeaturedProducts(int PortalID, bool Archived);
        public abstract IDataReader GetPortalNewProducts(int PortalID, bool Archived);
		public abstract IDataReader GetCategoryProducts(int CategoryID, bool Archived);
		public abstract IDataReader GetFeaturedProducts(int CategoryID, bool Archived);
        public abstract IDataReader GetNewProducts(int CategoryID, bool Archived);
        public abstract IDataReader GetSelectedProducts(int CategoryID, int CategoryID1, int CategoryID2, int CategoryID3, bool Archived);
        public abstract IDataReader GetRelatedProducts(int CategoryID, int CategoryID1, int CategoryID2, int CategoryID3, int CurrentProductId, bool Archived);
        public abstract IDataReader GetRelatedNewsProducts(int CategoryID, int CategoryID1, int CategoryID2, int CategoryID3, int CurrentProductId, bool Archived);
        public abstract IDataReader GetSearchedProducts(int CategoryID, string SearchTerm, bool Archived);
        public abstract IDataReader GetPopularProducts(int PortalID, int CategoryID, bool Archived);
		public abstract IDataReader GetPortalPopularProducts(int PortalID, bool Archived);
		public abstract IDataReader GetAlsoBoughtProducts(int PortalID, int ProductID, bool Archived);

		// Reviews
		public abstract int AddReview(int PortalID, int ProductID, int UserID, string UserName, int Rating, string Comments, bool Authorized, DateTime CreatedDate);
		public abstract void UpdateReview(int ReviewID, string UserName, int Rating, string Comments, bool Authorized);
		public abstract void DeleteReview(int ReviewID);
		public abstract IDataReader GetReview(int ReviewID);
		public abstract IDataReader GetReviews(int PortalID);
		public abstract IDataReader GetReviewsByProduct(int PortalID, int ProductID);

		#endregion
	}
}

