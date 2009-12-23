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
using System.Web;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for CurrentCart.
	/// </summary>
	public class CurrentCart
	{
		private static string CartCookieName = "DotNetNuke_Store_Portal_";
		private static bool _isCartVerified = false;

		#region Public Functions
		public static void AddItem(int portalID, int productID, int quantity)
		{
			string cartID = getCartID(portalID);

			ItemInfo itemInfo = new ItemInfo();
			itemInfo.CartID = cartID;
			itemInfo.ProductID = productID;
			itemInfo.Quantity = quantity;

			CartController controller = new CartController();
			controller.AddItem(itemInfo);
		}
        /*
        public static void AddItem(int portalID, int productID, int quantity, int deliveryMethod)
        {
            string cartID = getCartID(portalID);

            ItemInfo itemInfo = new ItemInfo();
            itemInfo.CartID = cartID;
            itemInfo.ProductID = productID;
            itemInfo.Quantity = quantity;
            itemInfo.DeliveryMethod = deliveryMethod;

            CartController controller = new CartController();
            controller.AddItem(itemInfo);
        }
        */
        // canadean changed: added method for the Data Extracts products
        public static void AddItem(int portalID, int productID, int quantity, int deliveryMethod, string modelNumber, string modelName, decimal cost)
        {
            string cartID = getCartID(portalID);

            ItemInfo itemInfo = new ItemInfo();
            itemInfo.CartID = cartID;
            if (productID == -1)
                productID = 15;        // DE product. Only used as a reference
            itemInfo.ProductID = productID;
            itemInfo.Quantity = quantity;
            itemInfo.DeliveryMethod = deliveryMethod;

            itemInfo.ModelNumber = modelNumber;
            itemInfo.ModelName = modelName;
            itemInfo.UnitCost = cost;

            CartController controller = new CartController();
            controller.AddItem(itemInfo);
        }

        public static void UpdateItem(int portalID, int itemID, int quantity)
		{
			string cartID = getCartID(portalID);

			CartController controller = new CartController();
			ItemInfo itemInfo = controller.GetItem(itemID);

			if (itemInfo != null)
			{
				itemInfo.CartID = cartID;
				itemInfo.ItemID = itemID;
				itemInfo.Quantity = quantity;

				controller.UpdateItem(itemInfo);
			}
		}

		public static void RemoveItem(int itemID)
		{
			CartController controller = new CartController();
			controller.DeleteItem(itemID);
		}

		public static void ClearItems(int portalID)
		{
			string cartID = getCartID(portalID);

			CartController controller = new CartController();
			controller.DeleteItems(cartID);
		}

		public static ArrayList GetItems(int portalID)
		{
			string cartID = getCartID(portalID);

			CartController controller = new CartController();
			return controller.GetItems(cartID);
		}

		public static CartInfo GetInfo(int portalID)
		{
			string cartID = getCartID(portalID);

			CartController controller = new CartController();
			CartInfo cartInfo = controller.GetCart(cartID, portalID);

			return cartInfo;
		}

		public static void DeleteCart(int portalID)
		{
			string cartID = getCartID(portalID);

			CartController controller = new CartController();
			controller.DeleteItems(cartID);
			controller.DeleteCart(cartID);

			setCartID(portalID, null);
		}
		#endregion

		#region Private Functions
		private static string getCartID(int portalID)
		{
			string cartID = null;

			// Get cart ID from cookie
			HttpCookie cartCookie = HttpContext.Current.Request.Cookies[CartCookieName + portalID.ToString()];
			if (cartCookie != null)
			{
				cartID = cartCookie["CartID"];
			}

			// Do we need to verify?
			if ((cartID != null) && (!_isCartVerified))
			{
				CartController controller = new CartController();
				_isCartVerified = (controller.GetCart(cartID, portalID) != null);
				if (!_isCartVerified)
				{
					cartID = null;
				}
			}

			// Do we need to create a new cart?
			if (cartID == null) 
			{
				cartID = createCart(portalID);
				setCartID(portalID, cartID);
			}

			return cartID;
		}

		private static void setCartID(int portalID, string cartID)
		{
			if (cartID != null)
			{
				HttpCookie cartCookie = new HttpCookie(CartCookieName + portalID.ToString());
				cartCookie["CartID"] = cartID;
				//cartCookie.Expires = DateTime.Today.AddDays(30);

				HttpContext.Current.Response.Cookies.Add(cartCookie);
			}
			else
			{
				HttpCookie cartCookie = new HttpCookie(CartCookieName + portalID.ToString());
				cartCookie.Expires = DateTime.Today.AddDays(-100);

				HttpContext.Current.Response.Cookies.Add(cartCookie);
			}
		}

		private static string createCart(int portalID)
		{
			CartController controller = new CartController();
			string cartID = null;

			cartID = System.Guid.NewGuid().ToString();

            DotNetNuke.Entities.Users.UserInfo mUser = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo();
            controller.AddCart(cartID, portalID, mUser.UserID);
			//controller.AddCart(cartID, portalID, Null.NullInteger);

			return cartID;
		}
		#endregion
	}
}
