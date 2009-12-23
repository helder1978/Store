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
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Communications;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider;
using DotNetNuke.Modules.Store.Providers.Shipping;
using DotNetNuke.Modules.Store.Providers.Tax;
using DotNetNuke.Services.Mail;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Account.
	/// </summary>
	public partial  class Checkout : PortalModuleBase, ICheckoutControl
	{
		#region Controls

		protected PlaceHolder plhShipAddress;
		protected DropDownList lstBillAddress;
		protected DropDownList lstShipAddress;
		protected DataList ctlBillAddress;
		protected DataList ctlShipAddress;
		protected AddressCheckoutControlBase ctlAddressCheckout;
		protected DotNetNuke.Modules.Store.Cart.ICheckoutControl taxControl;
		protected DotNetNuke.Modules.Store.Cart.ICheckoutControl shippingControl;
		protected DotNetNuke.UI.UserControls.LabelControl lblCartTotal;
		protected PaymentControlBase paymentControl;
        protected System.Web.UI.HtmlControls.HtmlTableRow trRow3;
        protected DotNetNuke.UI.UserControls.LabelControl lblGatewayTitle;
		#endregion       

		#region Private/Protected Declarations
		private static string CookieName = "DotNetNuke_Store_Portal_";
		private StoreInfo _storeInfo = null;
		private OrderInfo _orderInfo = null;
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

		#region Events
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //Response.Write("Page_Load");
            // return;

            if (_storeInfo == null)
            {
                StoreController storeController = new StoreController();
                _storeInfo = storeController.GetStoreInfo(PortalId);

                if (_storeInfo.CurrencySymbol != string.Empty)
                {
                    LocalFormat.CurrencySymbol = _storeInfo.CurrencySymbol;
                }

                if (_storeInfo.PortalTemplates)
                {
                    CssTools.AddCss(this.Page, PortalSettings.HomeDirectory + "Store", PortalId);
                }
                else
                {
                    CssTools.AddCss(this.Page, this.TemplateSourceDirectory, PortalId);
                }
            }

            lblGatewayTitle.Visible = false;

            if (forceSSL())
            {
                SSLHelper.RequestSecurePage();
            }

            //Mail.SendMail(PortalSettings.Email, "helder1978@gmail.com", "", "Canadean Checkout ", " ", "", "", "", "", "", "");
            if (Request.QueryString["WorldPayExit"] != null)
            {
                //Mail.SendMail(PortalSettings.Email, "helder1978@gmail.com", "", "Canadean Payment Checkout - " + Request.QueryString["WorldPayExit"], " ", "", "", "", "", "", "");
                loadPaymentControl();

                return;
            }



            loadCartControl();
            loadAddressControl();
            loadPaymentControl();
            loadTaxCheckoutControl();
            loadShippingCheckoutControl();

            _orderInfo = GetExistingOrder();

            updateCheckoutAddress();

            if (Request.QueryString["PayPalExit"] == null)
            {
                if (!this.Page.IsPostBack)
                {
                    if (_orderInfo == null)
                    {
                        _orderInfo = CreateOrder();
                    }

                    //Update the order details with the most recent cart items.
                    OrderController orderController = new OrderController();

                    CartInfo cartInfo = CurrentCart.GetInfo(PortalId);
                    if (cartInfo != null & _orderInfo != null)
                    {
                        _orderInfo = orderController.UpdateOrderDetails(_orderInfo.OrderID, cartInfo.CartID);
                    }
                }

                CalculateTaxandShipping(_orderInfo);
                updateCheckoutOrder(_orderInfo);
                updateStoreInfo();
            }
        }

        // ns4u changes: added this function so that the vat value is displayed for United Kingdom countries
		protected void Page_PreRender(object sender, System.EventArgs e)
		{
            // Response.Write("Page_PreRender");


            //Response.Write("<br>address: " + shippingControl.BillingAddress.CountryCode);
            //Response.Write("isplaced: " + _orderInfo.OrderIsPlaced);
            //Response.Write("cart items: " + CurrentCart.GetInfo(PortalId).Total);
            //Response.Write("order id: " + GetOrderIDFromCookie());

            if (GetOrderIDFromCookie() != -1)   // Only display if the order wasn't already submited
            {
                CalculateTaxandShipping(_orderInfo);
                updateCheckoutOrder(_orderInfo);
                updateStoreInfo();
            }

		}


		private void paymentControl_PaymentSucceeded(object sender, EventArgs e)
		{
			// Get the order
			OrderController orderController = new OrderController();
			
			//CalculateTaxandShipping(_orderInfo);
			
			// Generate new order number
			//string orderNumber = PortalId.ToString("00#") + "-" + UserId.ToString("0000#") + "-" + _orderInfo.OrderID.ToString("0000#") + "-" + _orderInfo.OrderDate.ToString("yyyyMMdd");
			//_orderInfo.OrderNumber = orderNumber;
			//orderController.UpdateOrder(_orderInfo.OrderID, _orderInfo.OrderDate, _orderInfo.OrderNumber, _orderInfo.ShippingAddressID, _orderInfo.BillingAddressID, _orderInfo.Tax, _orderInfo.ShippingCost, UserId);
			
			// Update display
			lblOrderNumber.Text = Localization.GetString("lblOrderNumber", this.LocalResourceFile) + " " + _orderInfo.OrderID.ToString();
			plhCheckout.Visible = false;
			plhOrder.Visible = true;

			// Get the order details
			ArrayList orders = orderController.GetOrderDetails(_orderInfo.OrderID);
			lstOrder.DataSource = orders;
			lstOrder.DataBind();

            lblTaxTotal.Text = _orderInfo.Tax.ToString("C", LocalFormat);
            lblShippingTotal.Text = _orderInfo.ShippingCost.ToString("C", LocalFormat);
            lblTotal.Text = _orderInfo.GrandTotal.ToString("C", LocalFormat);

            if (_orderInfo.Tax > 0)
            {
                trConfirmedOrderTax.Visible = true;
            }
            else
            {
                trConfirmedOrderTax.Visible = false;
            }
						
			// Cleanup the cart records
			CurrentCart.ClearItems(PortalId);

			base.ModuleConfiguration.DisplayPrint = true;
		}

		private void paymentControl_PaymentCancelled(object sender, EventArgs e)
		{
			HttpContext.Current.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID,"", new string[] {"PageID=CustomerOrders"}), false);
		}

		private void paymentControl_PaymentFailed(object sender, EventArgs e)
		{
			// Future Use: Do nothing, let payment control handle
		}

		private void ctlAddressCheckout_BillingAddressChanged(object sender, AddressEventArgs e)
		{
            //Response.Write("<br>ctlAddressCheckout_BillingAddressChanged ");

            taxControl.BillingAddress = e.address;
			shippingControl.BillingAddress = e.address;

            //Response.Write("<br>changed: " + e.address.CountryCode);

			CalculateTaxandShipping(_orderInfo);

			updateCheckoutOrder(_orderInfo);
		}

		private void ctlAddressCheckout_ShippingAddressChanged(object sender, AddressEventArgs e)
		{
            //Response.Write("<br>ctlAddressCheckout_BillingAddressChanged ");

            taxControl.ShippingAddress = e.address;
			shippingControl.ShippingAddress = e.address;	

			CalculateTaxandShipping(_orderInfo);

			updateCheckoutOrder(_orderInfo);
		}

		/// <summary>
		/// This event should occur each time a change is made to the cart.  
		/// All of the Checkout controls should be updated with the new cart information and the 
		/// order updated.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cartControl_EditComplete(object sender, EventArgs e)
		{
			OrderController orderController = new OrderController();

			CartInfo cartInfo = CurrentCart.GetInfo(PortalId);
			if (cartInfo != null & _orderInfo != null)
			{
				_orderInfo = orderController.UpdateOrderDetails(_orderInfo.OrderID, cartInfo.CartID);
			}

			CalculateTaxandShipping(_orderInfo);

			updateCheckoutOrder(_orderInfo);
		}
		#endregion

		#region Private Functions

		/// <summary>
		/// This informs the checkout controls that the order has been updated.
		/// </summary>
		/// <param name="orderInfo"></param>
		private void updateCheckoutOrder(OrderInfo orderInfo)
		{
			if (orderInfo != null)
			{
				this.taxControl.OrderInfo = orderInfo;
				this.shippingControl.OrderInfo = orderInfo;
				updateCartTotal(orderInfo);
			}
			else 
			{
				//A null order should NOT be allowed to checkout, so redirect the tab id without specifying the checkout control.
                if (Request.QueryString["PayPalExit"] == null)
                {
                    HttpContext.Current.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID), true);
                }
			}
		}

		private void updateCheckoutAddress()
		{
			taxControl.BillingAddress = this.BillingAddress;
			taxControl.ShippingAddress = this.ShippingAddress;
			shippingControl.BillingAddress = this.BillingAddress;
			shippingControl.ShippingAddress = this.ShippingAddress;			
		}


		private void updateStoreInfo()
		{
            this.taxControl.StoreData = _storeInfo;
            this.shippingControl.StoreData = _storeInfo;
		}

		/// <summary>
		/// Examine the module setting "RequireSSL" to determine if checkout should force a 
		/// redirect to HTTPS.
		/// </summary>
		/// <returns>true if HTTPS should be used</returns>
		private bool forceSSL()
		{
			// Determine if checkout should be forced to SSL according the module setting.
			string requireSSLSetting = (string)Settings["RequireSSL"];
			bool requireSSL = false;
			if ( requireSSLSetting != null && requireSSLSetting.Length > 0 )
			{
				try
				{
					requireSSL = bool.Parse(requireSSLSetting);
				}
				catch
				{
					requireSSL = false;
				}
			}
			return requireSSL;
		}

		private void updateCartTotal(OrderInfo orderInfo)
		{
			if (orderInfo != null)
			{
                this.txtCartTotal.Text = orderInfo.GrandTotal.ToString("C", LocalFormat);
			} 
			else 
			{
                this.txtCartTotal.Text = 0M.ToString("C", LocalFormat);
			}
		}

		private void loadCartControl()
		{
			plhCart.Controls.Clear();
			//TODO: Subscribe to the cart update event.
			StoreControlBase cartControl = (StoreControlBase)LoadControl(ModulePath + "CartDetail.ascx");
			cartControl.ParentControl = this as PortalModuleBase;
			cartControl.EditComplete += new EventHandler(cartControl_EditComplete);
			plhCart.Controls.Add(cartControl);
		}

		private void loadTaxCheckoutControl() 
		{
			this.plhTaxCheckout.Controls.Clear();
			ITaxProvider taxProvider = StoreController.GetTaxProvider(ModulePath);
			this.taxControl = (ICheckoutControl)taxProvider.GetCheckoutControl(this, ModulePath);
			this.plhTaxCheckout.Controls.Add((ProviderControlBase)this.taxControl);
		}

		private void loadShippingCheckoutControl()
		{
			this.plhShippingCheckout.Controls.Clear();
			IShippingProvider shippingProvider = StoreController.GetShippingProvider(ModulePath);
			this.shippingControl = (ICheckoutControl)shippingProvider.GetCheckoutControl(this, ModulePath);
			this.plhShippingCheckout.Controls.Add((ProviderControlBase)this.shippingControl);
		}

		///<summary>
		///Load the selected address checkout control and add to the billing address placeholder.
		///</summary>
		private void loadAddressControl() 
		{
			plhAddressCheckout.Controls.Clear();
			IAddressProvider addressProvider = StoreController.GetAddressProvider(ModulePath);
			ctlAddressCheckout = (AddressCheckoutControlBase)addressProvider.GetCheckoutControl(this, ModulePath);
			ctlAddressCheckout.ShippingAddressChanged += new ShippingAddressChangedEventHandler(ctlAddressCheckout_ShippingAddressChanged);
			ctlAddressCheckout.BillingAddressChanged += new BillingAddressChangedEventHandler(ctlAddressCheckout_BillingAddressChanged);
			plhAddressCheckout.Controls.Add(ctlAddressCheckout);
		}


		private void loadPaymentControl()
		{
            //StoreController storeController = new StoreController();
            //StoreInfo storeInfo = storeController.GetStoreInfo(PortalId);

			GatewayController controller = new GatewayController(Server.MapPath(ModulePath));
            GatewayInfo gateway = controller.GetGateway(_storeInfo.GatewayName);

			if (gateway != null)
			{
				string controlPath = gateway.GatewayPath + "\\" + gateway.PaymentControl;
				if (File.Exists(controlPath))
				{
					controlPath = controlPath.Replace(Server.MapPath(ModulePath), ModulePath);

					paymentControl = (PaymentControlBase)LoadControl(controlPath);
					paymentControl.ID = "ctlGateway";
					paymentControl.ParentControl = this as PortalModuleBase;
					//paymentControl.StoreData = storeInfo;
					paymentControl.CheckoutControl = this as ICheckoutControl;

					paymentControl.EnableViewState = true;
					paymentControl.PaymentSucceeded += new EventHandler(paymentControl_PaymentSucceeded);
					paymentControl.PaymentCancelled += new EventHandler(paymentControl_PaymentCancelled);
					paymentControl.PaymentFailed += new EventHandler(paymentControl_PaymentFailed);

					plhGateway.Controls.Clear();
					plhGateway.Controls.Add(paymentControl);
				}
				else
				{
					//LiteralControl error = new LiteralControl("<span class=\"NormalRed\">Could not find " + controlPath + ".</span>");
                    LiteralControl error = new LiteralControl("<span class=\"NormalRed\">" + Localization.GetString("ErrorCouldNotFind", this.LocalResourceFile) + " " + controlPath + ".</span>");
			
					plhGateway.Controls.Clear();
					plhGateway.Controls.Add(error);
				}
			}
			else
			{ 
					//LiteralControl error = new LiteralControl("<span class=\"NormalRed\">A Payment option has not be setup. Please contact the administrator to setup a payment option.</span>");
                    LiteralControl error = new LiteralControl("<span class=\"NormalRed\">" + Localization.GetString("ErrorPaymentOption", this.LocalResourceFile) + "</span>");
			
					plhGateway.Controls.Clear();
					plhGateway.Controls.Add(error);				
			}
		}

		/// <summary>
		/// Retrieves the OrderID from the cookie if available; otherwise a -1 is returned
		/// </summary>
		/// <returns>OrderID if found in cookie; otherwise a -1 is returned.</returns>
		private int GetOrderIDFromCookie()
		{
			int orderID = -1;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieKey];
            if ((cookie != null) && (cookie["OrderID"] != null))
            {
                orderID = int.Parse(cookie["OrderID"]);
            }
            
			return orderID;
		}

		private void SetOrderIdCookie(int OrderID)
		{
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieKey];
            if (cookie == null)
            {
                cookie = new HttpCookie(CookieKey);
            }
            cookie["OrderID"] = OrderID.ToString();
            //cookie.Expires = DateTime.Today.AddDays(30);
            HttpContext.Current.Response.Cookies.Add(cookie);
            Session["OrderID"] = OrderID;
		}

        private void ClearOrderIdCookie()
        {
            Session["OrderID"] = null;
        }

		private string CookieKey
		{
            get { return CookieName + PortalId.ToString(); }
		}

		/// <summary>
		/// Calculate the tax and shipping cost for the order.
		/// </summary>
		/// <param name="orderInfo"></param>
		private void CalculateTaxandShipping(OrderInfo orderInfo)
		{
			if (orderInfo != null)
			{
				ArrayList cartItems = CurrentCart.GetItems(PortalId);

                //Shipping...
				IShippingProvider shippingProvider = StoreController.GetShippingProvider(ModulePath);
                decimal cartWeight = 0;
                foreach (ItemInfo itemInfo in cartItems)
                {
                    cartWeight += (itemInfo.ProductWeight * itemInfo.Quantity);
                }
                IShippingInfo shippingInfo = shippingProvider.CalculateShippingFee(PortalId, cartWeight);
                if (shippingInfo == null)
                {
                    plhAddressCheckout.Visible = false;
                    plhGateway.Visible = false;
                    plhTaxCheckout.Visible = false;
                    plhShippingCheckout.Visible = false;
                    trRow4.Visible = false;
                    trRow5.Visible = false;
                    trRow6.Visible = false;
                    trRow8.Visible = false;
                    //lblError.Text = "<br/>Sorry, but we don't have any shipping rates for the weight of items in your basket.  Please <a href=\"mailto:" + this.PortalSettings.Email + "\">contact us</a> stating the list of products you'd like to order and we'll get a shipping quote for you.";
                    lblError.Text = String.Format(Localization.GetString("ErrorShippingRates", this.LocalResourceFile), this._storeInfo.DefaultEmailAddress);
                    //lblError.Text = String.Format(Localization.GetString("ErrorShippingRates", this.LocalResourceFile), this.PortalSettings.Email);
                    lblError.Visible = true;
                    return;
                }
                else
                {
                    plhAddressCheckout.Visible = true;
                    plhGateway.Visible = true;
                    plhTaxCheckout.Visible = true;
                    plhShippingCheckout.Visible = true;
                    trRow4.Visible = true;
                    trRow5.Visible = true;
                    trRow6.Visible = true;
                    trRow8.Visible = true;
                    lblError.Visible = false;
                }
                orderInfo.ShippingCost = shippingInfo.Cost;

                //Surcharges...
                try
                {
                    PayPalSettings payPalSettings = new PayPalSettings(StoreData.GatewaySettings);
                    decimal m_FixedSurcharge = payPalSettings.SurchargeFixed;
                    decimal m_PercentSurcharge = payPalSettings.SurchargePercent;
                    orderInfo.ShippingCost = orderInfo.ShippingCost + m_FixedSurcharge + ((orderInfo.OrderTotal + orderInfo.ShippingCost + m_FixedSurcharge) * (m_PercentSurcharge / 100));
                    shippingInfo.Cost = orderInfo.ShippingCost;
                }
                catch
                {
                    //Not paypal
                }
                                

                //Tax...
				ITaxProvider taxProvider = StoreController.GetTaxProvider(ModulePath);
			   
				ITaxInfo taxInfo = taxProvider.CalculateSalesTax(PortalId, cartItems, shippingInfo, ShippingAddress);
                //if (taxInfo.ShowTax)
                //Response.Write("<br>CountryCode0: " + BillingAddress.CountryCode);
                try
                {
                    //Response.Write("<br>CountryCode2: " + shippingControl.BillingAddress.CountryCode);
                    //Response.Write("<br>CountryCode3: " + shippingProvider.BillingAddress.CountryCode);
                }
                catch (Exception ex)
                { }
                
                if (taxInfo.ShowTax && (BillingAddress.CountryCode == "United Kingdom"))  // ns4u changes: only the UK countries pay VAT
                {
                    trTax.Visible = true;
                    orderInfo.Tax = taxInfo.SalesTax;
                }
                else
                {
                    trTax.Visible = false;
                    orderInfo.Tax = 0;
                }
			}
		}

		/// <summary>
		/// Retrieve the current order from the database as is.
		/// </summary>
		/// <returns></returns>
		private OrderInfo GetExistingOrder()
		{
			OrderController orderController = new OrderController();
			OrderInfo orderInfo = null;

			int orderID = GetOrderIDFromCookie();

			if (orderID >= 0)
			{				
				try
				{
					orderInfo = orderController.GetOrder(orderID);
				}
				catch 
				{
					orderInfo = null;	
				}
			}

			return orderInfo;
		}

		private OrderInfo CreateOrder()
		{
			// Associate cart with this user
			CartController cartController = new CartController();
			cartController.UpdateCart(CurrentCart.GetInfo(PortalId).CartID, UserId);

			// Create order (copies cart to create order)
			OrderController orderController = new OrderController();
			OrderInfo orderInfo = orderController.CreateOrder(CurrentCart.GetInfo(PortalId).CartID);
			if (orderInfo != null)
			{
				SetOrderIdCookie(orderInfo.OrderID);
			}

			return orderInfo;
		}
		#endregion

		#region ICheckoutControl Members

        public void Hide() 
        {
            this.trRow1.Visible = false;
            this.trRow2.Visible = false;
            this.trRow4.Visible = false;
            this.trRow5.Visible = false;
            this.trRow6.Visible = false;
            lblGatewayTitle.Visible = false;
        }

		public IAddressInfo BillingAddress
		{
			get
			{
				if (ctlAddressCheckout != null) 
				{
					return ctlAddressCheckout.BillingAddress;
				}
				else 
				{
					return new AddressInfo();
				}
			}
			set
			{
				ctlAddressCheckout.BillingAddress = value;
			}
		}

		public StoreInfo StoreData
		{
			get
			{
				if (_storeInfo == null)
				{
					StoreController storeController = new StoreController();
					_storeInfo = storeController.GetStoreInfo(PortalId);
				}
				return _storeInfo;
			}
			set
			{
				_storeInfo = value;
			}
		}

		public IAddressInfo ShippingAddress
		{
			get
			{
				if (ctlAddressCheckout != null) 
				{
					return ctlAddressCheckout.ShippingAddress;
				}
				else 
				{
					return new AddressInfo();
				}
			}
			set
			{
				ctlAddressCheckout.ShippingAddress = value;
			}
		}
		public OrderInfo OrderInfo 
		{
			get 
			{
				//CalculateTaxandShipping(_orderInfo);
				return _orderInfo;
			}
			set 
			{
                _orderInfo = value;
			}
		}
		/// <summary>
		/// Calculate the final order and place it into a "waiting for payment" state.
		/// </summary>
		/// <returns>OrderInfo with the final cart, tax, and shipping totals.</returns>
		public OrderInfo GetFinalizedOrderInfo()
		{		
			// Save the address information, so that it can be associated with the order.
			//
			// NOTE: This may just update the address with duplicate information, but it is easier than 
			//       trying to determine if a change was made the existing address.  In the future the
			//       AddressProvider may be able to supply a "Modified" property.
			AddressController controller = new AddressController();

            int m_BillingAddressID = 0;
            int m_ShippingAddressID = 0;

			if (Null.IsNull(BillingAddress.AddressID) || BillingAddress.AddressID == -1)
			{
				BillingAddress.PortalID = this.PortalId;
				BillingAddress.UserID = this.UserId;
                m_BillingAddressID = controller.AddAddress(BillingAddress);
			} 
			else 
			{
				controller.UpdateAddress(BillingAddress);
			}

            if (Null.IsNull(ShippingAddress.AddressID) || ShippingAddress.AddressID == -1)
            {
                ShippingAddress.PortalID = this.PortalId;
                ShippingAddress.UserID = this.UserId;

                if (ShippingAddress.Address1.Length == 0)
                {
                    m_ShippingAddressID = controller.AddAddress(BillingAddress);
                }
                else
                {
                    m_ShippingAddressID = controller.AddAddress(ShippingAddress);
                }
            }
            else
            {
                controller.UpdateAddress(ShippingAddress);
            }		

			//Now that the addresses are saved update the tax and shipping.
			//CalculateTaxandShipping(_orderInfo);

			//Save order details
			OrderController orderController = new OrderController();
            orderController.UpdateOrder(_orderInfo.OrderID, System.DateTime.Now,
                "",
                m_ShippingAddressID,
                m_BillingAddressID,
                _orderInfo.Tax,
                _orderInfo.ShippingCost,
                true,
                1,
                UserId);

            BillingAddress = controller.GetAddress(m_BillingAddressID);
            ShippingAddress = controller.GetAddress(m_ShippingAddressID);

            //throw new NotImplementedException("gfhgfH");

			return _orderInfo;
		}

        public OrderInfo GetOrderDetails()
        {
            OrderController orderController = new OrderController();
            return orderController.GetOrder(_orderInfo.OrderID);
        }
		#endregion
	}
}
