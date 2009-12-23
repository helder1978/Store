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

namespace DotNetNuke.Modules.Store.Providers.Tax.DefaultTaxProvider
{
	/// <summary>
	/// Summary description for SqlDataProvider.
	/// </summary>
	public class SqlDataProvider : DataProvider
	{
		#region Private Members
		private const string ProviderType = "data";

		private DotNetNuke.Framework.Providers.ProviderConfiguration _providerConfiguration = DotNetNuke.Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType);
		private string _connectionString;
		private string _providerPath;
		private string _objectQualifier;
		private string _databaseOwner;
		#endregion

		#region Constructors
		public SqlDataProvider()
		{
			DotNetNuke.Framework.Providers.Provider objProvider = ((DotNetNuke.Framework.Providers.Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]));

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
		public override void UpdateTaxRates(int PortalID, decimal Rate, bool ShowTax)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_UpdateTaxRates", PortalID, Rate, ShowTax);
		}

		public override IDataReader GetTaxRates(int PortalID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner + ObjectQualifier + "Store_Administration_GetTaxRates", PortalID);
		}
		#endregion
	}
}
