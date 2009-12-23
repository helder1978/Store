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
using Microsoft.ApplicationBlocks.Data;
using DotNetNuke;
using DotNetNuke.Framework.Providers;

namespace DotNetNuke.Modules.Store.Admin
{
	/// <summary>
	/// Summary description for Class1.
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
        public override Int32 AddStoreInfo(int PortalID, string Name, string Description, string Keywords, string GatewayName, string GatewaySettings, string DefaultEmailAddress, int ShoppingCartPageID, string CreatedByUser, int StorePageID, string CurrencySymbol, bool PortalTemplates, bool AuthorizeCancel)
		{
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_AddStoreInfo", PortalID, Name, Description, Keywords, GatewayName, GatewaySettings, DefaultEmailAddress, ShoppingCartPageID, CreatedByUser, StorePageID, CurrencySymbol, PortalTemplates, AuthorizeCancel));
		}

        public override void UpdateStoreInfo(int PortalID, string Name, string Description, string Keywords, string GatewayName, string GatewaySettings, string DefaultEmailAddress, int ShoppingCartPageID, int StorePageID, string CurrencySymbol, bool PortalTemplates, bool AuthorizeCancel)
		{
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_UpdateStoreInfo", PortalID, Name, Description, Keywords, GatewayName, GatewaySettings, DefaultEmailAddress, ShoppingCartPageID, StorePageID, CurrencySymbol, PortalTemplates, AuthorizeCancel);
		}

		public override IDataReader GetStoreInfo(int PortalID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_GetStoreInfo", PortalID);
		}

//		public override Int32 AddStoreProvider(int PortalID, int ProviderType, string Name, string Path)
//		{
//			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_AddStoreProvider", PortalID, ProviderType, Name, Path));
//		}
//
//		public override void UpdateStoreProvider(int PortalID, int ProviderType, string Name, string Path)
//		{
//			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_UpdateStoreProvider", PortalID, ProviderType, Name, Path);
//		}
//
//		public override IDataReader GetStoreProvider(int PortalID, int ProviderType)
//		{
//			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_GetStoreProvider", PortalID, ProviderType);
//		}
//
//		public override IDataReader GetStoreProviders(int PortalID)
//		{
//			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_GetStoreProviders", PortalID);
//		}
		#endregion
	}
}
