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

namespace DotNetNuke.Modules.Store.Customer
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
			objProvider = ((DataProvider)(DotNetNuke.Framework.Reflection.CreateObject("data", "DotNetNuke.Modules.Store.Customer", "DotNetNuke.Modules.Store.Customer"))); 
		} 

		public static DataProvider Instance() 
		{ 
			return objProvider; 
		}
		#endregion

		#region Abstract Functions
		// Addresses
		public abstract int AddAddress(int UserID, int PortalID, string Description, string Name, string Address1, string Address2, string City, string RegionCode, string CountryCode, string PostalCode, string Phone1, string Phone2, bool PrimaryAddress, string CreatedByUser);
		public abstract void UpdateAddress(int AddressID, string Description, string Name, string Address1, string Address2, string City, string RegionCode, string CountryCode, string PostalCode, string Phone1, string Phone2, bool PrimaryAddress);
		public abstract void DeleteUserAddresses(int UserID);
		public abstract void DeleteAddress(int AddressID);
		public abstract IDataReader GetUserAddresses(int UserID);
		public abstract IDataReader GetAddress(int AddressID);

		//Orders
		public abstract IDataReader CreateOrder(string CartID);
		public abstract IDataReader GetCustomerOrders(Int32 PortalID, Int32 CustomerID);
		public abstract IDataReader GetOrder(Int32 OrderID);
		public abstract void UpdateOrder(int OrderID, DateTime OrderDate, string OrderNumber, int ShippingAddressID, int BillingAddressID, decimal tax, decimal shippingCost, int CustomerID);
        public abstract void UpdateOrder(int OrderID, DateTime OrderDate, string OrderNumber, int ShippingAddressID, int BillingAddressID, decimal tax, decimal shippingCost, bool orderIsPlaced, int CustomerID);
        public abstract void UpdateOrder(int OrderID, DateTime OrderDate, string OrderNumber, int ShippingAddressID, int BillingAddressID, decimal tax, decimal shippingCost, bool orderIsPlaced, int orderStatusID, int CustomerID);
		public abstract IDataReader GetCustomers(int PortalID);
        public abstract IDataReader GetOrders(Int32 PortalID, int OrderStatusID);

		//Order Detail
		public abstract IDataReader GetOrderDetails(Int32 OrderID);
		public abstract IDataReader UpdateOrderDetails(Int32 OrderID, string CartID);

        //Order Status
        public abstract IDataReader GetOrderStatuses();

		#endregion
	}
}
