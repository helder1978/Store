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
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.Store.Cart
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
		public override void AddCart(string CartID, int PortalID, int UserID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Cart_AddCart", CartID, PortalID, UserID);
		}

		public override void UpdateCart(string CartID, int UserID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Cart_UpdateCart", CartID, UserID);
		}

		public override void DeleteCart(string CartID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Cart_DeleteCart", CartID);
		}

		public override void PurgeCarts(DateTime PurgeDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Cart_PurgeCarts", PurgeDate);
		}

		public override IDataReader GetCart(string CartID, int PortalID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Cart_GetCart", CartID, PortalID);
		}

		public override int AddItem(string CartID, int ProductID, int Quantity)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_AddItem", CartID, ProductID, Quantity));
		}

        public override int AddItem(string CartID, int ProductID, int Quantity, int ProdDeliveryMethod, string ProdReference, string ProdName, decimal ProdCost)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_AddItem2", CartID, ProductID, Quantity, ProdDeliveryMethod, ProdReference, ProdName, ProdCost));
        }

		public override void UpdateItem(int ItemID, int Quantity)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_UpdateItem", ItemID, Quantity);
		}

		public override void DeleteItem(int ItemID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_DeleteItem", ItemID);
		}

		public override void DeleteItems(string CartID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_DeleteItems", CartID);
		}

		public override IDataReader GetItem(int ItemID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_GetItem", ItemID);
		}

		public override IDataReader GetItems(string CartID)
		{
			return  SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_CartItems_GetItems", CartID);
		}
		#endregion
	}
}
