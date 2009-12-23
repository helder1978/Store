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
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using DotNetNuke;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for PayPalCheckout.
	/// </summary>
	public partial class PayPalPayment : PaymentControlBase
	{
		#region Private Declarations
		public PayPalSettings _settings = null;
		private const string _sandboxVerificationURL = "https://www.sandbox.paypal.com/cgi-bin/webscr/";
		private const string _sandboxPaymentURL = "https://www.sandbox.paypal.com/";

        private string _verificationURL = string.Empty;
        private string _paymentURL = string.Empty;
        private string _Message = string.Empty;
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
			imageButton1.Click += new ImageClickEventHandler(imageButton1_Click);
            btnContinue.Click += new EventHandler(btnContinue_Click);
		}
		#endregion

		#region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            _settings = new PayPalSettings(CheckoutControl.StoreData.GatewaySettings);

            _verificationURL = _settings.UseSandbox ? _sandboxVerificationURL : _settings.VerificationURL;
            _paymentURL = _settings.UseSandbox ? _sandboxPaymentURL : _settings.PaymentURL;

            // Do we have any special handling?
            PayPalNavigation nav = new PayPalNavigation(Request.QueryString);
            switch (nav.PayPalExit.ToUpper())
            {
                case "CANCEL":
                {
                    invokePaymentCancelled();
                    CheckoutControl.Hide();
                    pnlProceedToPayPal.Visible = false;
                    return;
                }
                case "RETURN":
                case "NOTIFY":
                {
                    // Collect data, invoke success
                    if (VerifyPayment())
                    {
                        //Set order status to "Paid"...
                        int orderID;
                        if (!Int32.TryParse(Request.Form["invoice"], out orderID))
                        {
                            VerificationFailed();
                            return;
                        }

                        CheckoutControl.OrderInfo = UpdateOrderStatus(orderID, OrderInfo.OrderStatusList.Paid);                      

                        invokePaymentSucceeded();
                        CheckoutControl.Hide();
                        pnlProceedToPayPal.Visible = false;
                        return;
                    }
                    else
                    {
                        VerificationFailed();
                        return;
                    }
                }
            }

            if (nav.PayPalExit.Length > 0)
            {
                //If the PayPalExit is anything else with length > 0, then don't do any processing
                HttpContext.Current.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID), false);
                return;
            }

            // Continue with display of payment control...
            if ((_settings == null) || (!_settings.IsValid()))
            {
                lblError.Text = Localization.GetString("GatewayNotConfigured", this.LocalResourceFile);
                lblError.Visible = true;
                imageButton1.Visible = false;
                pnlProceedToPayPal.Visible = false;
                pnlContinue.Visible = false;
                return;
            }
            else
            {
                _Message = Localization.GetString("lblConfirmMessage", this.LocalResourceFile);
                lblConfirmMessage.Text = string.Format(_Message, PortalSettings.PortalName);
                _Message = Localization.GetString("paypalimage", this.LocalResourceFile);
                paypalimage.AlternateText = _Message;
                imageButton1.AlternateText = _Message;

                lblError.Text = string.Empty;
                lblError.Visible = false;
                //imageButton1.Visible = true;
                imageButton1.ImageUrl = _settings.ButtonURL;
                paypalimage.ImageUrl = _settings.ButtonURL;
                paypalimage2.ImageUrl = _settings.ButtonURL;
            }
        }

		private void imageButton1_Click(object sender, ImageClickEventArgs e)
		{
            ConfirmOrder();

		}

        protected void btnConfirmOrder_Click(object sender, EventArgs e)
        {
            ConfirmOrder();
        }

        private OrderInfo UpdateOrderStatus(int orderID, OrderInfo.OrderStatusList orderStatus)
        {
            OrderController orderController = new OrderController();
            OrderInfo order = orderController.GetOrder(orderID);

            switch (orderStatus)
            {
                case OrderInfo.OrderStatusList.Cancelled:
                    order.OrderStatusID = 6;
                    break;
                case OrderInfo.OrderStatusList.Paid:
                    order.OrderStatusID = 7;
                    break;
            }

            orderController.UpdateOrder(order.OrderID, order.OrderDate, order.OrderNumber, order.ShippingAddressID, order.BillingAddressID, order.Tax, order.ShippingCost, true, order.OrderStatusID, order.CustomerID);

            return order;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            //Does nothing, but when clicked should redirect to PayPal...
        }

        private void AddHiddenField(string name, string value)
        {
            System.Web.UI.HtmlControls.HtmlInputHidden htmlHidden = new System.Web.UI.HtmlControls.HtmlInputHidden();
            htmlHidden.ID = name;
            htmlHidden.Name = name;
            htmlHidden.Value = value;
            Page.Form.Controls.Add(htmlHidden);
        }

        private string GetFirstName(string FullName)
        {
            char[] splitter = { ' ' };
            string[] stringlist = FullName.Split(splitter);
            if (stringlist.Length > 0)
            {
                return stringlist[0];
            }
            else 
            { 
                return string.Empty; 
            }
        }

        private string GetSurname(string FullName)
        {
            char[] splitter = { ' ' };
            string[] stringlist = FullName.Split(splitter);
            if (stringlist.Length > 0)
            {
                return stringlist[stringlist.Length-1];
            }
            else
            {
                return string.Empty;
            }
        }

        private void SetupPayPalFields()
        {
            IAddressInfo shippingAddress = CheckoutControl.ShippingAddress;
            IAddressInfo billingAddress = CheckoutControl.BillingAddress;

            OrderInfo orderInfo = CheckoutControl.GetOrderDetails();

            CultureInfo ci_enUS = new CultureInfo("en-US");

            //Set the paypal url as form target
            btnContinue.PostBackUrl = _paymentURL;

            string returnURL = Request.Url + "&PayPalExit=return";
            string cancelURL = Request.Url + "&PayPalExit=cancel";
            string notifyURL = Request.Url + "&PayPalExit=notify";

            AddHiddenField("cmd", "_cart");
            AddHiddenField("upload", "1");
            AddHiddenField("business", _settings.PayPalID);
            AddHiddenField("handling_cart", orderInfo.ShippingCost.ToString("0.00", ci_enUS));
            AddHiddenField("charset", _settings.Charset);
            AddHiddenField("currency_code", _settings.Currency);
            AddHiddenField("invoice", orderInfo.OrderID.ToString());
            AddHiddenField("return", returnURL);
            AddHiddenField("cancel_return", cancelURL);
            AddHiddenField("notify_url", notifyURL);
            AddHiddenField("rm", "2");
            AddHiddenField("lc", _settings.Lc);
            _Message = Localization.GetString("PayPalReturnTo", this.LocalResourceFile);
            AddHiddenField("cbt", string.Format(_Message, PortalSettings.PortalName));

            //Tax...
            if (orderInfo.Tax > 0)
            {
                AddHiddenField("tax_cart", orderInfo.Tax.ToString("0.00", ci_enUS));
            }

            //Cart Contents...
            ArrayList cartItems = CurrentCart.GetItems(PortalId);
            int itemNumber = 1;
            foreach (ItemInfo itemInfo in cartItems)
            {
                AddHiddenField("item_name_" + itemNumber.ToString(), itemInfo.Manufacturer + (itemInfo.Manufacturer.Length > 0 ? " " : "") + itemInfo.ModelName);
                AddHiddenField("quantity_" + itemNumber.ToString(), itemInfo.Quantity.ToString());
                AddHiddenField("amount_" + itemNumber.ToString(), itemInfo.UnitCost.ToString("0.00", ci_enUS));
                itemNumber++;
            }

            //Customer Address...
            AddHiddenField("email", UserInfo.Membership.Email);
            AddHiddenField("first_name", GetFirstName(billingAddress.Name));
            AddHiddenField("last_name", GetSurname(billingAddress.Name));
            AddHiddenField("address1", billingAddress.Address1);
            AddHiddenField("address2", billingAddress.Address2);
            AddHiddenField("city", billingAddress.City);
            AddHiddenField("zip", billingAddress.PostalCode);
            AddHiddenField("country", (billingAddress.CountryCode.Equals("United Kingdom") ? "GB" : billingAddress.CountryCode));

            AddHiddenField("business_cs_email", UserInfo.Membership.Email);
            AddHiddenField("business_address1", billingAddress.Address1);
            AddHiddenField("business_address2", billingAddress.Address2);
            AddHiddenField("business_city", billingAddress.City);
            AddHiddenField("business_zip", billingAddress.PostalCode);
            AddHiddenField("business_country", billingAddress.CountryCode);
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

        private string CookieKey
        {
            get { return CookieName + PortalId.ToString(); }
        }

        private static string CookieName = "DotNetNuke_Store_Portal_";

		#endregion

		#region Private Methods

        private void VerificationFailed()
        {
            lblError.Text = Localization.GetString("VerificationFailed", this.LocalResourceFile);
            lblError.Visible = true;
            imageButton1.Visible = false;
            CheckoutControl.Hide();
            pnlProceedToPayPal.Visible = false;
        }

        private void ConfirmOrder()
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            //Adds order to db...
            OrderInfo orderInfo = CheckoutControl.GetFinalizedOrderInfo();

            _Message = Localization.GetString("lblOrderNumber", this.LocalResourceFile);
            lblOrderNumber.Text = string.Format(_Message, orderInfo.OrderID);
            _Message = Localization.GetString("paypalimage2", this.LocalResourceFile);
            paypalimage2.AlternateText = _Message;

            CheckoutControl.Hide();

            pnlContinue.Visible = true;
            pnlProceedToPayPal.Visible = false;

            SetupPayPalFields();

            generateOrderConfirmation();

            //Clear basket
            CurrentCart.ClearItems(PortalId);

            //Clear cookies
            SetOrderIdCookie(-1);
        }

		private bool VerifyPayment()
		{
			bool isVerified = false;

            PayPalIPNParameters ipn = new PayPalIPNParameters(Request.Form);
			if (ipn.IsValid)
			{
				HttpWebRequest request = WebRequest.Create(_verificationURL) as HttpWebRequest;
				if (request != null)
				{
					request.Method = "POST";
					request.ContentLength = ipn.PostString.Length;
					request.ContentType = "application/x-www-form-urlencoded";

					StreamWriter writer = new StreamWriter(request.GetRequestStream());
					writer.Write(ipn.PostString);
					writer.Close();

					HttpWebResponse response = request.GetResponse() as HttpWebResponse;
					if (response != null)
					{
						StreamReader reader = new StreamReader(response.GetResponseStream());
						string responseString = reader.ReadToEnd();
						reader.Close();

						if (string.Compare(responseString, "VERIFIED", true) == 0)
						{
							isVerified = true;
						}
						else
						{
							//Not verified, possible fraud
						}
					}
				}
			}

			return isVerified;
		}

		private string HTTPPOSTEncode(string postString)
		{
			postString = postString.Replace("\\", "");
			postString = System.Web.HttpUtility.UrlEncode(postString);
			postString = postString.Replace("%2f", "/");
			return postString;
		}

		#endregion
	}
}
