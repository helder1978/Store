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

namespace DotNetNuke.Modules.Store.Customer
{
  /// <summary>
  /// Summary description for AddressInfo.
  /// </summary>
    public class OrderInfo
    {
        // Entity member variables for database table DotNetNukeStore_Orders
        private Int32 mOrderID;
        private Int32 mCustomerID;
        private string mCustomerUsername;
        private string mOrderNumber;
        private DateTime mOrderDate;
        private DateTime mShipDate;
        private Int32 mShippingAddressID;
        private Int32 mBillingAddressID;
        private decimal mOrderTotal;
        private decimal mTaxTotal;
        private decimal mShippingCost;
        private int mOrderStatusID;
        private bool mOrderIsPlaced;

        // Calculated Expression member variables for database table DotNetNukeStore_Orders

        // Entity properties for database table DotNetNukeStore_Orders
        public Int32 OrderID
        {
            get { return mOrderID; }
            set { mOrderID = value; }
        }

        public Int32 CustomerID
        {
            get { return mCustomerID; }
            set { mCustomerID = value; }
        }

        public String CustomerUsername
        {
            get { return mCustomerUsername; }
            set { mCustomerUsername = value; }
        }

        public string OrderNumber
        {
            get { return mOrderNumber; }
            set { mOrderNumber = value; }
        }

        public DateTime OrderDate
        {
            get { return mOrderDate; }
            set { mOrderDate = value; }
        }

        public DateTime ShipDate
        {
            get { return mShipDate; }
            set { mShipDate = value; }
        }

        public Int32 ShippingAddressID
        {
            get { return mShippingAddressID; }
            set { mShippingAddressID = value; }
        }

        public Int32 BillingAddressID
        {
            get { return mBillingAddressID; }
            set { mBillingAddressID = value; }
        }
        /// <summary>
        /// The subtotal of the cart items in the order.
        /// NOTE: Does NOT include tax and shipping charges.
        /// </summary>
        public decimal OrderTotal
        {
            get { return mOrderTotal; }
            set { mOrderTotal = value; }
        }

        /// <summary>
        /// The grand total summing the OrderTotal, TaxTotal, and ShippingCost
        /// </summary>
        public decimal GrandTotal
        {
            get { return mOrderTotal + mTaxTotal + mShippingCost; }
        }
        /// <summary>
        /// Total tax that has been calculated for this order.
        /// </summary>
        public decimal Tax
        {
            get { return mTaxTotal; }
            set { mTaxTotal = value; }
        }
        /// <summary>
        /// Shiping costs associated with this order.
        /// </summary>
        public decimal ShippingCost
        {
            get { return mShippingCost; }
            set { mShippingCost = value; }
        }
        // Equal and HashCode methods for easy compare in lists

        public override bool Equals(object obj)
        {
            if ((obj == null) || (this.GetType() != obj.GetType()))
            {
                return false;
            }
            OrderInfo objInfo = (OrderInfo)obj;
            return mOrderID.Equals(objInfo.OrderID);
        }

        public override Int32 GetHashCode()
        {
            return mOrderID.GetHashCode();
        }

        public Int32 OrderStatusID
        {
            get { return mOrderStatusID; }
            set { mOrderStatusID = value; }
        }

        public Boolean OrderIsPlaced
        {
            get { return mOrderIsPlaced; }
            set { mOrderIsPlaced = value; }
        }

        public enum OrderStatusList
        {
            Processing = 1,
            AwaitingPayment = 2,
            AwaitingStock = 3,
            Packing = 4,
            Dispatched = 5,
            Cancelled = 6,
            Paid = 7,
            NotPlaced = 8
        }
    }
}
