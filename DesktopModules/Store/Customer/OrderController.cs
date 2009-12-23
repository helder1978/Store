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
using System;
using System.Text;
using System.Net.Mail;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Admin;

namespace DotNetNuke.Modules.Store.Customer
{
	/// <summary>
	/// Summary description for OrdersController.
	/// </summary>
	public class OrderController
	{
		#region Constructors
		public OrderController()
		{
		}
		#endregion

        ArrayList arOrderStatus = null;

		#region Public Functions
//		public void AddOrder(OrdersInfo OrdersInfo)
//		{
//			DataProvider.Instance().AddOrder(OrdersInfo.UserID, OrdersInfo.PortalID, OrdersInfo.Attention, OrdersInfo.Orders1, OrdersInfo.Orders2, OrdersInfo.City, OrdersInfo.RegionCode, OrdersInfo.CountryCode, OrdersInfo.PostalCode, OrdersInfo.Phone1, OrdersInfo.Phone2, OrdersInfo.PrimaryOrders, OrdersInfo.CreatedByUser);
//		}
//
//		public void UpdateOrder(OrdersInfo OrdersInfo)
//		{
//			DataProvider.Instance().UpdateOrder(OrdersInfo.OrdersID, OrdersInfo.Attention, OrdersInfo.Orders1, OrdersInfo.Orders2, OrdersInfo.City, OrdersInfo.RegionCode, OrdersInfo.CountryCode, OrdersInfo.PostalCode, OrdersInfo.Phone1, OrdersInfo.Phone2, OrdersInfo.PrimaryOrders);
//		}
//
//		public void DeleteUserOrders(int userID)
//		{
//			DataProvider.Instance().DeleteUserOrders(userID);
//		}
//
//		public void DeleteOrder(int OrdersID)
//		{
//			DataProvider.Instance().DeleteOrder(OrdersID);
//		}
		
		public OrderInfo CreateOrder(string cartID)
		{
			return (CBO.FillObject(DataProvider.Instance().CreateOrder(cartID), typeof(OrderInfo)) as OrderInfo);
		}

        public void UpdateOrder(int orderID, DateTime OrderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, int CustomerID)
		{
			//TODO: Determine if the shipping and billing address exist
			//      If not then create a new address and associate this order with it.
            DataProvider.Instance().UpdateOrder(orderID, OrderDate, orderNumber, shippingAddressID, billingAddressID, tax, shippingCost, CustomerID);
		}

        public void UpdateOrder(int orderID, DateTime OrderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, bool orderIsPlaced, int CustomerID)
        {
            //TODO: Determine if the shipping and billing address exist
            //      If not then create a new address and associate this order with it.
            DataProvider.Instance().UpdateOrder(orderID, OrderDate, orderNumber, shippingAddressID, billingAddressID, tax, shippingCost, orderIsPlaced, CustomerID);
        }

        public void UpdateOrder(int orderID, DateTime OrderDate, string orderNumber, int shippingAddressID, int billingAddressID, decimal tax, decimal shippingCost, bool orderIsPlaced, int orderStatusID, int CustomerID)
        {
            //TODO: Determine if the shipping and billing address exist
            //      If not then create a new address and associate this order with it.
            DataProvider.Instance().UpdateOrder(orderID, OrderDate, orderNumber, shippingAddressID, billingAddressID, tax, shippingCost, orderIsPlaced, orderStatusID, CustomerID);
        }
		
		public OrderInfo UpdateOrderDetails(int orderID, string cartID)
		{
			return (CBO.FillObject(DataProvider.Instance().UpdateOrderDetails(orderID, cartID), typeof(OrderInfo)) as OrderInfo);
		}

		public OrderInfo GetOrder(int orderID)
		{
			return (CBO.FillObject(DataProvider.Instance().GetOrder(orderID), typeof(OrderInfo)) as OrderInfo);
		}

        public ArrayList GetOrders(int portalID, int orderStatusID)
        {
            return (CBO.FillCollection(DataProvider.Instance().GetOrders(portalID, orderStatusID), typeof(OrderInfo)));
        }

		public ArrayList GetCustomerOrders(int portalID, int userID)
		{
			return CBO.FillCollection(DataProvider.Instance().GetCustomerOrders(portalID, userID), typeof(OrderInfo));
		}

        public ArrayList GetOrderStatuses()
        {
            //return CBO.FillCollection(DataProvider.Instance().GetOrderStatuses(), typeof(OrderStatus));
            if (arOrderStatus == null)
            {
                arOrderStatus = new ArrayList();
                OrderStatus orderStatus = null;
                string strStatusResource = "OrderStatus";

                int i = 1;
                /*
                orderStatus = new OrderStatus();
                orderStatus.OrderStatusID = 2;  // Awaiting payment
                //orderStatus.OrderStatusText = Localization.GetString(strStatusResource + i.ToString(), "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
                orderStatus.OrderStatusText = Localization.GetString(strStatusResource + "2", "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
                orderStatus.ListOrder = i;
                arOrderStatus.Add(orderStatus);
                
                i++; //2
                */

                orderStatus = new OrderStatus();
                orderStatus.OrderStatusID = 7; // Paid
                orderStatus.OrderStatusText = "Paid"; // Localization.GetString(strStatusResource + "7", "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
                orderStatus.ListOrder = i;
                arOrderStatus.Add(orderStatus);

                i++; //3
                orderStatus = new OrderStatus();
                orderStatus.OrderStatusID = 1;  // Processing
                orderStatus.OrderStatusText = "Processing"; //  Localization.GetString(strStatusResource + "1", "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
                orderStatus.ListOrder = i;
                arOrderStatus.Add(orderStatus);
                /*
                i++; //4
                orderStatus = new OrderStatus();
                orderStatus.OrderStatusID = 3;  // Awaiting Stock
                orderStatus.OrderStatusText = "";  Localization.GetString(strStatusResource + "3", "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
                orderStatus.ListOrder = i;
                arOrderStatus.Add(orderStatus);
                /*
                i++; //5
                orderStatus = new OrderStatus();
                orderStatus.OrderStatusID = 4;  // Packing
                orderStatus.OrderStatusText = Localization.GetString(strStatusResource + "4", "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
                orderStatus.ListOrder = i;
                arOrderStatus.Add(orderStatus);
                */
                i++; //6
                orderStatus = new OrderStatus();
                orderStatus.OrderStatusID = 5;  // Archived
                orderStatus.OrderStatusText = "Archived"; //  Localization.GetString(strStatusResource + "5", "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
                orderStatus.ListOrder = i;
                arOrderStatus.Add(orderStatus);
                
                i++; //7
                orderStatus = new OrderStatus();
                orderStatus.OrderStatusID = 6;  // Canceled
                orderStatus.OrderStatusText = "Canceled"; // Localization.GetString(strStatusResource + "6", "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
                orderStatus.ListOrder = i;
                arOrderStatus.Add(orderStatus);

                i++; //8
                orderStatus = new OrderStatus();
                orderStatus.OrderStatusID = 8;  // Not placed
                orderStatus.OrderStatusText = "Not placed"; //  Localization.GetString(strStatusResource + "8", "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
                orderStatus.ListOrder = i;
                arOrderStatus.Add(orderStatus);

                i++; //9
                orderStatus = new OrderStatus();
                orderStatus.OrderStatusID = 9;  // All placed
                orderStatus.OrderStatusText = "All placed"; // Localization.GetString(strStatusResource + "9", "~/DesktopModules/Store/App_LocalResources/CustomerOrders.ascx");
                orderStatus.ListOrder = i;
                arOrderStatus.Add(orderStatus);
            }
            return arOrderStatus;
        }

        public ArrayList GetOrderDetails(int orderID)
		{
			OrderDetailsInfo info = null;
			ArrayList al = new ArrayList();
			SqlDataReader dr = (SqlDataReader)DataProvider.Instance().GetOrderDetails(orderID);
		      
		//    DotNetNukeStore_Products.ProductID, 
		//    DotNetNukeStore_Products.ModelName,
		//    DotNetNukeStore_Products.ModelNumber,
		//    DotNetNukeStore_OrderDetails.UnitCost,
		//    DotNetNukeStore_OrderDetails.Quantity,
		//    (DotNetNukeStore_OrderDetails.Quantity * DotNetNukeStore_OrderDetails.UnitCost) as ExtendedAmount

			while(dr.Read())
			{
				info = new OrderDetailsInfo();
				info.ModelName = (string)dr["ModelName"];
				info.ModelNumber = (string)dr["ModelNumber"];
				info.OrderID = orderID;
				info.ProductID = (int)dr["ProductID"];
				info.UnitCost = (decimal)dr["UnitCost"];
				info.Quantity = (int)dr["Quantity"];
				info.ExtendedAmount = (decimal)dr["ExtendedAmount"];
                info.ProdDeliveryMethod = (int)dr["ProdDeliveryMethod"];
                info.ProdCost = (decimal)dr["ProdCost"];
                info.ProdName = (string)dr["ProdName"];
                info.ProdReference = (string)dr["ProdReference"];
                al.Add(info);
			}
			dr.Close();

			return al;
			//return CBO.FillCollection(, typeof(OrderDetailsInfo));
		}

		public ArrayList GetCustomers(int portalID)
		{
			return CBO.FillCollection(DataProvider.Instance().GetCustomers(portalID), typeof(CustomerInfo));
		}

		#endregion
	}
}
