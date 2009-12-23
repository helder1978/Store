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
using System.IO;
using System.Text;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Providers;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for ShippingControlBase.
	/// </summary>
	public class ShippingControlBase : ProviderControlBase
	{
		#region Private Declarations

		protected PortalModuleBase _parentControl = null;
		protected StoreInfo _storeInfo = null;
		protected IAddressInfo _shippingAddress = null;
		protected IAddressInfo _billingAddress = null;
		protected OrderInfo _orderInfo = null;
		
		#endregion

		#region Public Properties/Events

		public PortalModuleBase ParentControl
		{
			get { return _parentControl; }
			set { _parentControl = value; }
		}

		public StoreInfo StoreData
		{
			get { return _storeInfo; }
			set { _storeInfo = value; }
		}

		public IAddressInfo ShippingAddress
		{
			get { return _shippingAddress; }
			set { _shippingAddress = value; }
		}

		public IAddressInfo BillingAddress
		{
			get { return _billingAddress; }
			set { _billingAddress = value; }
		}

		public OrderInfo OrderInfo
		{
			get { return _orderInfo; }
			set { _orderInfo = value; }
		}

		#endregion
	}
}
