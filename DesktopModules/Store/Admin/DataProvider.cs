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
using DotNetNuke;

namespace DotNetNuke.Modules.Store.Admin
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
			objProvider = ((DataProvider)(DotNetNuke.Framework.Reflection.CreateObject("data", "DotNetNuke.Modules.Store.Admin", "DotNetNuke.Modules.Store.Admin"))); 
		} 

		public static DataProvider Instance() 
		{ 
			return objProvider; 
		}
		#endregion

		#region Abstract Functions
        public abstract Int32 AddStoreInfo(int PortalID, string Name, string Description, string Keywords, string GatewayName, string GatewaySettings, string DefaultEmailAddress, int ShoppingCartPageID, string CreatedByUser, int StorePageID, string CurrencySymbol, bool PortalTemplates, bool AuthorizeCancel);
        public abstract void UpdateStoreInfo(int PortalID, string Name, string Description, string Keywords, string GatewayName, string GatewaySettings, string DefaultEmailAddress, int ShoppingCartPageID, int StorePageID, string CurrencySymbol, bool PortalTemplates, bool AuthorizeCancel);
		public abstract IDataReader GetStoreInfo(int PortalID);
//		public abstract Int32 AddStoreProvider(int PortalID, int ProviderType, string Name, string Path);
//		public abstract void UpdateStoreProvider(int PortalID, int ProviderType, string Name, string Path);
//		public abstract IDataReader GetStoreProvider(int PortalID, int ProviderType);
//		public abstract IDataReader GetStoreProviders(int PortalID);
		#endregion
	}
}
