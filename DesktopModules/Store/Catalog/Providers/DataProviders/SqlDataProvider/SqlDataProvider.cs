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
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Summary description for SqlDataProvider.
	/// </summary>
	public class SqlDataProvider : DataProvider
	{
		#region Private Members
		private const string ProviderType = "data";
		private ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
		private string _connectionString;
		private string _providerPath;
		private string _objectQualifier;
		private string _databaseOwner;
		#endregion

		#region Constructors
		public SqlDataProvider()
		{
			Provider objProvider = ((Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]));

            _connectionString = Common.Utilities.Config.GetConnectionString();
            if (_connectionString == "")
            {
                if (objProvider.Attributes["connectionStringName"] != "" && System.Configuration.ConfigurationSettings.AppSettings[objProvider.Attributes["connectionStringName"]] != "")
                {
                    _connectionString = System.Configuration.ConfigurationSettings.AppSettings[objProvider.Attributes["connectionStringName"]];
                }
                else
                {
                    _connectionString = objProvider.Attributes["connectionString"];
                }
            }
			
			_providerPath = objProvider.Attributes["providerPath"]; 
			_objectQualifier = objProvider.Attributes["objectQualifier"]; 
			
			if (_objectQualifier != "" & _objectQualifier.EndsWith("_") == false) 
			{ 
				_objectQualifier += "_"; 
			} 
			
			_databaseOwner = objProvider.Attributes["databaseOwner"]; 
			
			if (_databaseOwner != "" & _databaseOwner.EndsWith(".") == false) 
			{ 
				_databaseOwner += "."; 
			}		
		}
		#endregion

		#region Properties
		public string ConnectionString 
		{ 
			get 
			{ 
				return _connectionString; 
			} 
		} 

		public string ProviderPath 
		{ 
			get 
			{ 
				return _providerPath; 
			} 
		} 

		public string ObjectQualifier 
		{ 
			get 
			{ 
				return _objectQualifier; 
			} 
		} 

		public string DatabaseOwner 
		{ 
			get 
			{ 
				return _databaseOwner; 
			} 
		}
		#endregion

		#region Private Functions
		private object GetNull(object Field) 
		{ 
			return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value); 
		} 
		#endregion

		#region Public Functions
		// Categories
		public override int AddCategory(int PortalID, string CategoryName, string CategoryDescription, string Message, bool Archived, string CreatedByUser, DateTime CreatedDate, int OrderID, int ParentCategoryID)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_AddCategory", PortalID, CategoryName, CategoryDescription, Message, Archived, CreatedByUser, CreatedDate, OrderID, ParentCategoryID));
		}
		public override void UpdateCategory(int CategoryID, string CategoryName, string CategoryDescription, string Message, bool Archived, int OrderID, int ParentCategoryID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_UpdateCategory", CategoryID, CategoryName, CategoryDescription, Message, Archived, OrderID, ParentCategoryID);
		}
		public override void DeleteCategories(int PortalID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_DeleteAll", PortalID);
		}
		public override void DeleteCategory(int CategoryID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_DeleteCategory", CategoryID);
		}
		public override int CategoryCount(int PortalID)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_CountAll", PortalID));
		}
		public override IDataReader GetCategories(int PortalID, bool IncludeArchived, int ParentCategoryID)
		{
			//TODO: Handle exclusion of archived categories, when requested
			return (IDataReader) SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_GetAll", PortalID, ParentCategoryID);
		}
        public override IDataReader GetCategoriesFromProducts(int PortalID, int CategoryID1, int CategoryID2, int CategoryID3, int ReturnCategory)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_GetFromProducts", PortalID, CategoryID1, CategoryID2, CategoryID3, ReturnCategory);
        }
        public override IDataReader GetCategory(int CategoryID)
		{
			return (IDataReader) SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Categories_GetCategory", CategoryID);
		}

		// Products
        /*
        public override int AddProduct(int PortalID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, bool Featured, bool Archived, string CreatedByUser, DateTime CreatedDate, decimal ProductWeight, decimal ProductHeight, decimal ProductLength, decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, decimal SalePrice) 
		{
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_AddProduct", PortalID, CategoryID, Manufacturer, ModelNumber, ModelName, ProductImage, UnitCost, Summary, Description, Featured, Archived, CreatedByUser, CreatedDate, ProductWeight, ProductHeight, ProductLength, ProductWidth, SaleStartDate, SaleEndDate, SalePrice));
		}

        public override void UpdateProduct(int ProductID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, bool Featured, bool Archived, decimal ProductWeight, decimal ProductHeight, decimal ProductLength, decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, decimal SalePrice) 
		{
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_UpdateProduct", ProductID, CategoryID, Manufacturer, ModelNumber, ModelName, ProductImage, UnitCost, Summary, Description, Featured, Archived, ProductWeight, ProductHeight, ProductLength, ProductWidth, SaleStartDate, SaleEndDate, SalePrice);
		}
        */
        /*
        public override int AddProduct(int PortalID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, bool Featured, bool Archived, string CreatedByUser, DateTime CreatedDate, decimal ProductWeight, decimal ProductHeight, decimal ProductLength, decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, decimal SalePrice, int CategoryID1, int CategoryID2, int CategoryID3, int NumPages, DateTime PublishDate, int DeliveryMethod, bool AvailableOnline, string ProductFile, string ProductPreview, string ProductImage2, string ProductImage3, string DescriptionTag, string TOC_Html, string PriceStr) 
		{
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_AddProduct", PortalID, CategoryID, Manufacturer, ModelNumber, ModelName, ProductImage, UnitCost, Summary, Description, Featured, Archived, CreatedByUser, CreatedDate, ProductWeight, ProductHeight, ProductLength, ProductWidth, SaleStartDate, SaleEndDate, SalePrice, CategoryID1, CategoryID2, CategoryID3, NumPages, PublishDate, DeliveryMethod, AvailableOnline, ProductFile, ProductPreview, ProductImage2, ProductImage3, DescriptionTag, TOC_Html, PriceStr));
		}

        public override void UpdateProduct(int ProductID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, bool Featured, bool Archived, decimal ProductWeight, decimal ProductHeight, decimal ProductLength, decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, decimal SalePrice, int CategoryID1, int CategoryID2, int CategoryID3, int NumPages, DateTime PublishDate, int DeliveryMethod, bool AvailableOnline, string ProductFile, string ProductPreview, string ProductImage2, string ProductImage3, string DescriptionTag, string TOC_Html, string PriceStr) 
		{
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_UpdateProduct", ProductID, CategoryID, Manufacturer, ModelNumber, ModelName, ProductImage, UnitCost, Summary, Description, Featured, Archived, ProductWeight, ProductHeight, ProductLength, ProductWidth, SaleStartDate, SaleEndDate, SalePrice, CategoryID1, CategoryID2, CategoryID3, NumPages, PublishDate, DeliveryMethod, AvailableOnline, ProductFile, ProductPreview, ProductImage2, ProductImage3, DescriptionTag, TOC_Html, PriceStr);
		}
        */

        public override int AddProduct(int PortalID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, string DescriptionTwo, string DescriptionThree, bool Featured, bool Archived, string CreatedByUser, DateTime CreatedDate, decimal ProductWeight, decimal ProductHeight, decimal ProductLength, decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, decimal SalePrice, int CategoryID1, int CategoryID2, int CategoryID3, int NumPages, DateTime PublishDate, int DeliveryMethod, bool AvailableOnline, string ProductFile, string ProductPreview, string ProductImage2, string ProductImage3, string DescriptionTag, string TOC_Html, string PriceStr)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_AddProduct", PortalID, CategoryID, Manufacturer, ModelNumber, ModelName, ProductImage, UnitCost, Summary, Description, DescriptionTwo, DescriptionThree, Featured, Archived, CreatedByUser, CreatedDate, ProductWeight, ProductHeight, ProductLength, ProductWidth, SaleStartDate, SaleEndDate, SalePrice, CategoryID1, CategoryID2, CategoryID3, NumPages, PublishDate, DeliveryMethod, AvailableOnline, ProductFile, ProductPreview, ProductImage2, ProductImage3, DescriptionTag, TOC_Html, PriceStr));
        }

        public override void UpdateProduct(int ProductID, int CategoryID, string Manufacturer, string ModelNumber, string ModelName, string ProductImage, decimal UnitCost, string Summary, string Description, string DescriptionTwo, string DescriptionThree, bool Featured, bool Archived, decimal ProductWeight, decimal ProductHeight, decimal ProductLength, decimal ProductWidth, DateTime SaleStartDate, DateTime SaleEndDate, decimal SalePrice, int CategoryID1, int CategoryID2, int CategoryID3, int NumPages, DateTime PublishDate, int DeliveryMethod, bool AvailableOnline, string ProductFile, string ProductPreview, string ProductImage2, string ProductImage3, string DescriptionTag, string TOC_Html, string PriceStr)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_UpdateProduct", ProductID, CategoryID, Manufacturer, ModelNumber, ModelName, ProductImage, UnitCost, Summary, Description, DescriptionTwo, DescriptionThree, Featured, Archived, ProductWeight, ProductHeight, ProductLength, ProductWidth, SaleStartDate, SaleEndDate, SalePrice, CategoryID1, CategoryID2, CategoryID3, NumPages, PublishDate, DeliveryMethod, AvailableOnline, ProductFile, ProductPreview, ProductImage2, ProductImage3, DescriptionTag, TOC_Html, PriceStr);
        }

        
		public override void DeleteProduct(int ProductID) 
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_DeleteProduct", ProductID);
		}

		public override IDataReader GetProduct(int ProductID) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetProduct", ProductID);
		}

		public override IDataReader GetPortalAllProducts(int PortalID) 
		{
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalAllProducts", PortalID);
		}

		public override IDataReader GetPortalProducts(int PortalID, bool Featured, bool Archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalProducts", PortalID, Featured, Archived);
		}

		public override IDataReader GetPortalFeaturedProducts(int PortalID, bool Archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalFeaturedProducts", PortalID, Archived);
		}

        public override IDataReader GetPortalNewProducts(int PortalID, bool Archived)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalNewProducts", PortalID, Archived);
        }

		public override IDataReader GetCategoryProducts(int CategoryID, bool Archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetProducts", CategoryID, Archived);
		}

		public override IDataReader GetFeaturedProducts(int CategoryID, bool Archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetFeaturedProducts", CategoryID, Archived);
		}

        public override IDataReader GetNewProducts(int CategoryID, bool Archived)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetNewProducts", CategoryID, Archived);
        }

        public override IDataReader GetSelectedProducts(int CategoryID, int CategoryID1, int CategoryID2, int CategoryID3, bool Archived)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetSelectedProducts", CategoryID, CategoryID1, CategoryID2, CategoryID3, Archived);
        }

        public override IDataReader GetRelatedProducts(int CategoryID, int CategoryID1, int CategoryID2, int CategoryID3, int CurrentProductId, bool Archived)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetRelatedProducts", CategoryID, CategoryID1, CategoryID2, CategoryID3, CurrentProductId, Archived);
        }

        public override IDataReader GetRelatedNewsProducts(int CategoryID, int CategoryID1, int CategoryID2, int CategoryID3, int CurrentProductId, bool Archived)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetRelatedNewsProducts", CategoryID, CategoryID1, CategoryID2, CategoryID3, CurrentProductId, Archived);
        }

        public override IDataReader GetSearchedProducts(int CategoryID, string SearchTerm, bool Archived)
        {
            return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetSearchedProducts", CategoryID, SearchTerm, Archived);
        }

        public override IDataReader GetPopularProducts(int PortalID, int CategoryID, bool Archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPopularProducts", PortalID, CategoryID, Archived);
		}

		public override IDataReader GetPortalPopularProducts(int PortalID, bool Archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetPortalPopularProducts", PortalID, Archived);
		}

		public override IDataReader GetAlsoBoughtProducts(int PortalID, int ProductID, bool Archived) 
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Products_GetAlsoBoughtProducts", PortalID, ProductID, Archived);
		}

		// Reviews
		public override int AddReview(int PortalID, int ProductID, int UserID, string UserName, int Rating, string Comments, bool Authorized, DateTime CreatedDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_AddReview", PortalID, ProductID, UserID, UserName, Rating, Comments, Authorized, CreatedDate));
		}
		public override void UpdateReview(int ReviewID, string UserName, int Rating, string Comments, bool Authorized)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_UpdateReview", ReviewID, UserName, Rating, Comments, Authorized);
		}
		public override void DeleteReview(int ReviewID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_DeleteReview", ReviewID);
		}
		public override IDataReader GetReview(int ReviewID)
		{
			return (IDataReader) SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_GetReview", ReviewID);
		}
		public override IDataReader GetReviews(int PortalID)
		{
			return (IDataReader) SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_GetAll", PortalID);
		}
		public override IDataReader GetReviewsByProduct(int PortalID, int ProductID)
		{
			return (IDataReader) SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Reviews_GetByProduct", PortalID, ProductID);
		}
		#endregion
	}
}

