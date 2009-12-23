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

namespace DotNetNuke.Modules.Store.Cart
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
			objProvider = ((DataProvider)(DotNetNuke.Framework.Reflection.CreateObject("data", "DotNetNuke.Modules.Store.Cart", "DotNetNuke.Modules.Store.Cart"))); 
		} 

		public static DataProvider Instance() 
		{ 
			return objProvider; 
		}
		#endregion

		#region Abstract Functions
		public abstract void AddCart(string CartID, int PortalID, int UserID);
		public abstract void UpdateCart(string CartID, int UserID);
		public abstract void DeleteCart(string CartID);
		public abstract void PurgeCarts(DateTime PurgeDate);
		public abstract IDataReader GetCart(string CartID, int PortalID);

		public abstract int AddItem(string CartID, int ProductID, int Quantity);
        public abstract int AddItem(string CartID, int ProductID, int Quantity, int ProdDeliveryMethod, string ProdReference, string ProdName, decimal ProdCost);
		public abstract void UpdateItem(int ItemID, int Quantity);
		public abstract void DeleteItem(int ItemID);
		public abstract void DeleteItems(string CartID);
		public abstract IDataReader GetItem(int ItemID);
		public abstract IDataReader GetItems(string CartID);
		#endregion
	}
}
