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
using System.Data;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Providers.Shipping;
using DotNetNuke.Modules.Store.Providers.Tax;

namespace DotNetNuke.Modules.Store.Admin
{
	/// <summary>
	/// Summary description for StoreController.
	/// </summary>
	public class StoreController
	{
		#region Constructors
		public StoreController()
		{
		}
		#endregion

		#region Public Functions
		public StoreInfo GetStoreInfo(int portalID) 
		{ 
			return ((StoreInfo)(CBO.FillObject(DataProvider.Instance().GetStoreInfo(portalID), typeof(StoreInfo)))); 
		}

		public void AddStoreInfo(StoreInfo storeInfo)
		{
            DataProvider.Instance().AddStoreInfo(storeInfo.PortalID, storeInfo.Name, storeInfo.Description, storeInfo.Keywords, storeInfo.GatewayName, storeInfo.GatewaySettings, storeInfo.DefaultEmailAddress, storeInfo.ShoppingCartPageID, storeInfo.CreatedByUser, storeInfo.StorePageID, storeInfo.CurrencySymbol, storeInfo.PortalTemplates, storeInfo.AuthorizeCancel);
		}

		public void UpdateStoreInfo(StoreInfo storeInfo)
		{
            DataProvider.Instance().UpdateStoreInfo(storeInfo.PortalID, storeInfo.Name, storeInfo.Description, storeInfo.Keywords, storeInfo.GatewayName, storeInfo.GatewaySettings, storeInfo.DefaultEmailAddress, storeInfo.ShoppingCartPageID, storeInfo.StorePageID, storeInfo.CurrencySymbol, storeInfo.PortalTemplates, storeInfo.AuthorizeCancel);
		}

//		public void AddStoreProvider(StoreProviderInfo providerInfo)
//		{
//			DataProvider.Instance().AddStoreProvider(providerInfo.PortalID, (int)providerInfo.ProviderType, providerInfo.Name, providerInfo.Path);
//		}

//		public void UpdateStoreProvider(StoreProviderInfo providerInfo)
//		{
//			DataProvider.Instance().UpdateStoreProvider(providerInfo.PortalID, (int)providerInfo.ProviderType, providerInfo.Name, providerInfo.Path);
//		}

//		public StoreProviderInfo GetStoreProvider(int portalID, StoreProviderType providerType)
//		{
//			return (StoreProviderInfo)(CBO.FillObject(DataProvider.Instance().GetStoreProvider(portalID, (int)providerType), typeof(StoreProviderInfo))); 
//		}

//		public ArrayList GetStoreProviders(int portalID)
//		{
//			return CBO.FillCollection(DataProvider.Instance().GetStoreProviders(portalID), typeof(StoreProviderInfo)); 
//		}

		public static IAddressProvider GetAddressProvider(string modulePath)
		{
			//Initialize an address provider controller
			ProviderController providerController = new ProviderController(StoreProviderType.Address, HttpContext.Current.Server.MapPath(modulePath));
			
			//Get the provider info
			//TODO: get the provider name from a database table (Store_Providers)
			ProviderInfo providerInfo = providerController.GetProvider("Default");

			//Create an instance of the provider
			IAddressProvider addressProvider = (IAddressProvider)ProviderFactory.CreateProvider(providerInfo);
			
			return addressProvider;
		}

		public static IShippingProvider GetShippingProvider(string modulePath)
		{
			//Initialize an address provider controller
			ProviderController providerController = new ProviderController(StoreProviderType.Shipping, HttpContext.Current.Server.MapPath(modulePath));
			
			//Get the provider info
			//TODO: get the provider name from a database table (Store_Providers)
			ProviderInfo providerInfo = providerController.GetProvider("Default");

			//Create an instance of the provider
			IShippingProvider shippingProvider = (IShippingProvider)ProviderFactory.CreateProvider(providerInfo);
			
			return shippingProvider;
		}

		public static ITaxProvider GetTaxProvider(string modulePath)
		{
			//Initialize an address provider controller
			ProviderController providerController = new ProviderController(StoreProviderType.Tax, HttpContext.Current.Server.MapPath(modulePath));
			
			//Get the provider info
			//TODO: get the provider name from a database table (Store_Providers)
			ProviderInfo providerInfo = providerController.GetProvider("Default");

			//Create an instance of the provider
			ITaxProvider taxProvider = (ITaxProvider)ProviderFactory.CreateProvider(providerInfo);
			
			return taxProvider;
		}
		#endregion
	}
}
