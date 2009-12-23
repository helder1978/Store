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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Shipping;

namespace DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider
{
	/// <summary>
	/// Summary description for AddresssController.
	/// </summary>
	public class ShippingController : ProviderControllerBase, IShippingProvider
	{
		#region Constructors
		public ShippingController()
		{
		}
		#endregion

		#region Public Functions
		public ShippingInfo GetShippingRate(int portalId, int ShippingRateID)
		{
            return CBO.FillObject(DataProvider.Instance().GetShippingRate(portalId, ShippingRateID), typeof(ShippingInfo)) as ShippingInfo;
		}

        public ArrayList GetAllShippingRates(int portalId)
        {
            return CBO.FillCollection(DataProvider.Instance().GetShippingRates(portalId), typeof(ShippingInfo)); 
        }

		public void UpdateShippingRate(ShippingInfo shippingInfo)
		{
            DataProvider.Instance().UpdateShippingRate(shippingInfo.ID, shippingInfo.Description, shippingInfo.MinWeight, shippingInfo.MaxWeight, shippingInfo.Cost);
		}

        public void AddShippingRate(int portalID, ShippingInfo shippingInfo)
        {
            DataProvider.Instance().AddShippingRate(portalID, shippingInfo.Description, shippingInfo.MinWeight, shippingInfo.MaxWeight, shippingInfo.Cost);
        }

        public void DeleteShippingRate(int ShippingRateID)
        {
            DataProvider.Instance().DeleteShippingRate(ShippingRateID);
        }

		#endregion

		#region IShippingProvider Members

		public IShippingInfo CalculateShippingFee(int portalId, decimal cartWeight)
		{
            return (CBO.FillObject(DataProvider.Instance().GetShippingFee(portalId, cartWeight), typeof(ShippingInfo))) as ShippingInfo;
		}

		#endregion
	}
}
