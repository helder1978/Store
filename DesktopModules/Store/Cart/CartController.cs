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
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for OrdersController.
	/// </summary>
	public class CartController
	{
		public CartController()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region Public Functions
		public void AddCart(string cartID, int portalID, int userID)
		{
			DataProvider.Instance().AddCart(cartID, portalID, userID);
		}

		public void UpdateCart(CartInfo cartInfo)
		{
			DataProvider.Instance().UpdateCart(cartInfo.CartID, cartInfo.UserID);
		}

		public void UpdateCart(string cartID, int userID)
		{
			DataProvider.Instance().UpdateCart(cartID, userID);
		}

		public void DeleteCart(string cartID)
		{
			DataProvider.Instance().DeleteCart(cartID);
		}

		public void PurgeCarts(DateTime purgeDate)
		{
			DataProvider.Instance().PurgeCarts(purgeDate);
		}

		public CartInfo GetCart(string cartID, int portalID)
		{
			return (CartInfo)(CBO.FillObject(DataProvider.Instance().GetCart(cartID, portalID), typeof(CartInfo))); 
		}

		public void AddItem(ItemInfo itemInfo)
		{
            // canadean changed: added new product cart attributes
            DataProvider.Instance().AddItem(itemInfo.CartID, itemInfo.ProductID, itemInfo.Quantity, itemInfo.DeliveryMethod, itemInfo.ModelNumber, itemInfo.ModelName, itemInfo.UnitCost);
		}

		public void UpdateItem(ItemInfo itemInfo)
		{
			DataProvider.Instance().UpdateItem(itemInfo.ItemID, itemInfo.Quantity);
		}

		public void DeleteItem(int itemID)
		{
			DataProvider.Instance().DeleteItem(itemID);
		}

		public void DeleteItems(string cartID)
		{
			DataProvider.Instance().DeleteItems(cartID);
		}

		public ItemInfo GetItem(int itemID)
		{
			return (ItemInfo)CBO.FillObject(DataProvider.Instance().GetItem(itemID), typeof(ItemInfo)); 
		}

		public ArrayList GetItems(string cartID)
		{
			return CBO.FillCollection(DataProvider.Instance().GetItems(cartID), typeof(ItemInfo));
		}
		#endregion
	}
}
