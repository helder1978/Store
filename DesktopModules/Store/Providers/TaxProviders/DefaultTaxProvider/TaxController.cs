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

using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Providers.Tax;

namespace DotNetNuke.Modules.Store.Providers.Tax.DefaultTaxProvider
{
	/// <summary>
	/// Summary description for TaxController.
	/// </summary>
	public class TaxController : DotNetNuke.Modules.Store.Providers.ProviderControllerBase, ITaxProvider
	{
		#region Constructors
		public TaxController()
		{
		}
		#endregion

		#region Public Functions
		public TaxInfo GetTaxRates(int portalID) 
		{ 
			IDataReader reader = DataProvider.Instance().GetTaxRates(portalID);
            TaxInfo taxInfo = new TaxInfo();
			
			if (reader.Read())
			{
				taxInfo.DefaultTaxRate = (reader["DefaultTaxRate"] == System.DBNull.Value) ? -1 : (decimal)reader["DefaultTaxRate"];
                taxInfo.ShowTax = (bool)reader["ShowTax"];
			}

			return taxInfo;
		}

		public void UpdateTaxRates(int portalID, decimal rate, bool ShowTax)
		{
			DataProvider.Instance().UpdateTaxRates(portalID, rate, ShowTax);
		}
		#endregion

		#region ITaxProvider Members

		/// <summary>
		/// Calculate tax according the region in the deliveryAddress argument.
		/// </summary>
		/// <param name="portalID">ID of the portal</param>
		/// <param name="cartItems">ArrayList of ItemInfo that need to have taxes calculated on.</param>
		/// <param name="shippingInfo">ShippingInfo in the case that taxes need to be applied to shipping</param>
		/// <param name="deliveryAddress">The address that the taxes should be applied for.</param>
		/// <returns>ITaxInfo with the total amount of tax due for the cart items shipping cost.</returns>
		public ITaxInfo CalculateSalesTax(int portalID, ArrayList cartItems, Shipping.IShippingInfo shippingInfo, Address.IAddressInfo deliveryAddress)
		{
            
			TaxInfo taxInfo = new TaxInfo();

            taxInfo = GetTaxRates(portalID);
			//decimal regionTaxRate = 0M;
			//if (deliveryAddress.RegionCode != null && deliveryAddress.RegionCode.Length > 0) 
			//{
				//NOTE: The registration address uses country and region text(ex. Idaho) rather than code(ex. ID).
				//      As a result all address are stored using the coutnry and region text rather then code.
				//      We have to lookup the region code here, because taxes are associated with region codes.
				//      This is only done for the United States since the DefaultTaxProvider only recognizes the United States.
			//	if ( "united states".Equals(deliveryAddress.CountryCode.ToLower()) || "us".Equals(deliveryAddress.CountryCode.ToLower() ) )
			//	{
			//		ListController ctlEntry = new ListController(); 
			//		ListEntryInfoCollection regionCollection = ctlEntry.GetListEntryInfoCollection("Region", "", "Country.US"); 
				
			//		foreach(DotNetNuke.Common.Lists.ListEntryInfo entry in regionCollection)
			//		{
			//			if (entry.Text.Equals(deliveryAddress.RegionCode))
			//			{
			//				if (taxRates.Contains(entry.Value))
			//				{
			//					regionTaxRate = decimal.Parse((string)taxRates[entry.Value]);
			//				}
			//				break;
			//			}
			//		}
			//	}
			//}

            decimal cartTotal = shippingInfo.Cost;
			foreach (DotNetNuke.Modules.Store.Cart.ItemInfo itemInfo in cartItems)
			{
				cartTotal += itemInfo.Quantity * itemInfo.UnitCost;
			}

			taxInfo.SalesTax = cartTotal * (taxInfo.DefaultTaxRate/100);

			return taxInfo;
		}

        public ITaxInfo GetDefautTaxRates(int portalID)
        {
            TaxInfo taxInfo = new TaxInfo();

            taxInfo = GetTaxRates(portalID);
			return taxInfo;
        }
		#endregion

		#region Private Functions

		/// <summary>
		/// Retrieve the tax rates for the specified portal
		/// </summary>
		/// <param name="portalId">Portal that the tax rates should be retrieved for.</param>
		/// <returns>decimal containg the tax rate</returns>
		private decimal loadTaxRates(int portalId)
		{
			return this.GetTaxRates(portalId).DefaultTaxRate;
		}

		#endregion

	}
}
