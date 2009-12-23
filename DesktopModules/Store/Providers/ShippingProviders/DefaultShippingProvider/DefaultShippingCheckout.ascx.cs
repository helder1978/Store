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
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;


namespace DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider
{
	/// <summary>
	/// Summary description for CoreProfile.
	/// </summary>
	public partial class DefaultShippingCheckout : ProviderControlBase, ICheckoutControl
	{
		#region Private Properties
		private StoreInfo storeInfo;
		private IAddressInfo shippingAddress;
		private IAddressInfo billingAddress;
		private OrderInfo orderInfo;
        private NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        #endregion
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (storeInfo == null)
            {
                StoreController storeController = new StoreController();
                storeInfo = storeController.GetStoreInfo(PortalId);
                if (storeInfo.CurrencySymbol != string.Empty)
                {
                    LocalFormat.CurrencySymbol = storeInfo.CurrencySymbol;
                }
            }

            UpdateShippingTotal(this.OrderInfo);
		}
		#endregion

		#region ICheckoutControl Members

        public void Hide() { this.Visible = false; }

		public StoreInfo StoreData
		{
			get{ return null; }
			set{}
		}

		public IAddressInfo ShippingAddress
		{
			get{ return null; }
			set{}
		}

		public IAddressInfo BillingAddress
		{
			get{ return null; }
			set{}
		}

		public OrderInfo OrderInfo
		{
			get{ return this.orderInfo; }
			set
			{
				this.orderInfo = value;
				UpdateShippingTotal(value);
			}
		}

		public OrderInfo GetFinalizedOrderInfo(){return orderInfo;}
        public OrderInfo GetOrderDetails() { return orderInfo; }	
		#endregion

		private void UpdateShippingTotal(OrderInfo orderInfo)
		{
			if( orderInfo != null)
			{
                this.txtShippingTotal.Text = this.OrderInfo.ShippingCost.ToString("C", LocalFormat);
			}
			else 
			{
                this.txtShippingTotal.Text = 0M.ToString("C", LocalFormat);
			}
		}
	}
}
