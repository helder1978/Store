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
using DotNetNuke;

namespace DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider
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
            objProvider = ((DataProvider)(DotNetNuke.Framework.Reflection.CreateObject("data", "DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider", "DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider"))); 
		} 

		public static DataProvider Instance() 
		{ 
			return objProvider; 
		}
		#endregion

		#region Abstract Functions
        public abstract void UpdateShippingRate(int ID, string Description, decimal MinWeight, decimal MaxWeight, decimal Cost);
        public abstract void AddShippingRate(int PortalID, string Description, decimal MinWeight, decimal MaxWeight, decimal Cost);        
        public abstract IDataReader GetShippingRate(int PortalID, int ShippingRateID);
        public abstract IDataReader GetShippingRates(int PortalID);
        public abstract void DeleteShippingRate(int ID);
        public abstract IDataReader GetShippingFee(int PortalID, decimal Weight);
		#endregion
	}
}
