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
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Cart;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CustomerOrders.
	/// </summary>
	public partial  class CustomerOrders : StoreControlBase
	{
        public bool ShowOrdersInStatus = false;
        public int OrderStatusID = Null.NullInteger;
        private CustomerNavigation customerNav;
        private StoreController storeController = new StoreController();
        private StoreInfo storeInfo = null;

        private string _Message = string.Empty;
        private string _OrderDateFormat = string.Empty;
        private NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();

		#region Controls
		protected DotNetNuke.UI.UserControls.LabelControl lblParentTitle;
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
            this.grdOrders.ItemDataBound += new DataGridItemEventHandler(this.grdOrders_ItemDataBound);
            this.grdOrderDetails.ItemDataBound += new DataGridItemEventHandler(this.grdOrderDetails_ItemDataBound);
            this.lnkbtnSave.Click += new EventHandler(lnkbtnSave_Click);
            this.btnPayViaPayPal.Click += new EventHandler(btnPayViaPayPal_Click);
            storeInfo = storeController.GetStoreInfo(PortalId);
            if (storeInfo.CurrencySymbol != string.Empty)
            {
                LocalFormat.CurrencySymbol = storeInfo.CurrencySymbol;
            }
            
            //_OrderDateFormat = Localization.GetString("OrderDateFormat", this.LocalResourceFile);
            _OrderDateFormat = "dd/MM/yyyy HH:mm";
		}


		#endregion

		#region Events
		protected void Page_Load(object sender, System.EventArgs e)
		{
			customerNav = new CustomerNavigation(Request.QueryString);
			lblError.Text = "";  //Initialize the Error Label.

            if (customerNav.PayPalExit != null)
            {
                switch (customerNav.PayPalExit.ToUpper())
                {
                    case "CANCEL":
                        {
                            customerNav.OrderID = Null.NullInteger;
                            customerNav.PageID = "CustomerOrders";
                            customerNav.PayPalExit = null;
                            //Response.Redirect(customerNav.GetNavigationUrl(), false);
                            break;
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

                                }
                                else
                                {
                                    UpdateOrderStatus(orderID, OrderInfo.OrderStatusList.Paid);
                                }

                                customerNav.OrderID = Null.NullInteger;
                                customerNav.PageID = "CustomerOrders";
                                customerNav.PayPalExit = null;
                                //Response.Redirect(customerNav.GetNavigationUrl(), false);
                            }
                            else
                            {
                                TabController tabControler = new TabController();
                                TabInfo tabInfo = tabControler.GetTab(storeInfo.ShoppingCartPageID, storeInfo.PortalID, true);
                                _Message = Localization.GetString("VerificationFailed", this.LocalResourceFile);
                                lblPaymentError.Text = string.Format(_Message, tabInfo.TabName);
                                lblPaymentError.Visible = true;
                                customerNav.OrderID = Null.NullInteger;
                                customerNav.PageID = "CustomerOrders";
                                customerNav.PayPalExit = null;
                                //Response.Redirect(customerNav.GetNavigationUrl(), false);
                            }
                            break;
                        }
                }
            }

			Store storeControl = parentControl as Store;
			if (storeControl != null)
			{
				storeControl.ParentTitle = lblParentTitle.Text;
			}

            if (ShowOrdersInStatus
                && OrderStatusID != Null.NullInteger)
            {
                plhGrid.Visible = true;
                plhForm.Visible = false;

                OrderController orderController = new OrderController();
                ArrayList orders = orderController.GetOrders(PortalId, OrderStatusID);

                if (orders.Count > 0)
                {
                    orderStatusList = orderController.GetOrderStatuses();
                    grdOrders.DataSource = orders;
                    grdOrders.DataBind();
                }
                else
                {
                    lblError.Text = Localization.GetString("NoOrdersFound", this.LocalResourceFile);
                    grdOrders.DataSource = null;
                    grdOrders.DataBind();
                }
            }
            else
            {
                if (customerNav.OrderID != Null.NullInteger)
                {
                    plhGrid.Visible = false;
                    //plhForm.Visible = true;

                    if (customerNav.OrderID != 0)
                    {
                        lblEditTitle.Text = Localization.GetString("ViewDetails", this.LocalResourceFile);
                        showOrderDetails(customerNav.OrderID);
                    }
                }
                else
                {
                    plhGrid.Visible = true;
                    plhForm.Visible = false;

                    if (customerNav.CustomerID == Null.NullInteger)
                    {
                        customerNav.CustomerID = UserId;
                    }

                    DisplayCustomerOrders();
                }
            }
		}

        ArrayList orderStatusList;

		protected override void OnPreRender(EventArgs e)
		{
			// Set the title in the parent control
			Account accountControl = parentControl as Account;
			if (accountControl != null)
			{
				accountControl.ParentTitle = lblParentTitle.Text;
			}

			base.OnPreRender (e);
		}

		protected void grdOrders_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderInfo orderInfo = (OrderInfo)e.Item.DataItem;

                HyperLink lnkEdit = (HyperLink)e.Item.FindControl("lnkEdit");
                if (lnkEdit != null)
                {
                    customerNav.OrderID = orderInfo.OrderID;
                    customerNav.CustomerID = orderInfo.CustomerID;
                    lnkEdit.NavigateUrl = customerNav.GetNavigationUrl();
                }

                //Order Date
                Label lblOrderDateText = (Label)e.Item.FindControl("lblOrderDateText");
                lblOrderDateText.Text = orderInfo.OrderDate.ToString(_OrderDateFormat);

                //Order Status
                Label lblOrderStatusText = (Label)e.Item.FindControl("lblOrderStatusText");
                //string OrderStatusText = "";
                //foreach (OrderStatus orderStatus in orderStatusList)
                //{
                //    if (orderStatus.OrderStatusID == orderInfo.OrderStatusID)
                //    {
                //        OrderStatusText = orderStatus.OrderStatusText;
                //        break;
                //    }
                //}
                //lblOrderStatusText.Text = OrderStatusText;

                //if (!orderInfo.OrderIsPlaced)
                //    lblOrderStatusText.Text = "Not Placed";
                //else
                    lblOrderStatusText.Text = GetOrderStatus(orderInfo.OrderStatusID, orderInfo.OrderIsPlaced);

                //Order Total
                Label lblOrderTotalText = (Label)e.Item.FindControl("lblOrderTotalText");
                lblOrderTotalText.Text = orderInfo.GrandTotal.ToString("C", LocalFormat);

                //Ship Date (Status Date)
                Label lblShipDateText = (Label)e.Item.FindControl("lblShipDateText");
                if (orderInfo.OrderStatusID > 1)
                {
                    lblShipDateText.Text = orderInfo.ShipDate.ToString(_OrderDateFormat);
                }
                else
                {
                    lblShipDateText.Text = "";
                }

                //Cancel link
                LinkButton lnkCancel = (LinkButton)e.Item.FindControl("lnkCancel");
                if (lnkCancel != null)
                {
                    if (storeInfo.AuthorizeCancel && !UserInfo.IsSuperUser)
                    {
                        if (orderInfo.OrderStatusID == (int)OrderInfo.OrderStatusList.AwaitingPayment
                            || orderInfo.OrderStatusID == (int)OrderInfo.OrderStatusList.AwaitingStock
                            || orderInfo.OrderStatusID == (int)OrderInfo.OrderStatusList.Paid)
                        {
                            lnkCancel.CommandArgument = orderInfo.OrderID.ToString();
                            lnkCancel.Click += new EventHandler(lnkCancel_Click);
                            lnkCancel.Visible = true;
                        }
                        else 
                        {
                            lnkCancel.Visible = false;
                        }
                    }
                    else 
                    {
                        lnkCancel.Visible = false;
                    }
                }
            }
		}

        private string GetOrderStatus(int orderStatusID, bool orderIsPlaced)
        {
            // canadean changed: allow to get the status of orders that aren't placed
            if (!orderIsPlaced)
                return "Not Placed";

            if (orderStatusList == null)
            {
                OrderController orderController = new OrderController();
                orderStatusList = orderController.GetOrderStatuses();
            }

            string OrderStatusText = "";
            foreach (OrderStatus orderStatus in orderStatusList)
            {
                if (orderStatus.OrderStatusID == orderStatusID)
                {
                    OrderStatusText = orderStatus.OrderStatusText;
                    break;
                }
            }
            return OrderStatusText;
        }

        private void lnkCancel_Click(object sender, EventArgs e)
        {
            int orderID;
            if (!(Int32.TryParse(((LinkButton)sender).CommandArgument, out orderID)))
            {
                return;
            }

            CancelOrder(orderID);

            //Force a re-binding of the search grid
            customerNav.OrderID = Null.NullInteger;
            customerNav.PayPalExit = null;
            Response.Redirect(customerNav.GetNavigationUrl(), false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            int orderID;
            if (!(Int32.TryParse(((Button)sender).CommandArgument, out orderID)))
            {
                return;
            }

            CancelOrder(orderID);
            customerNav.OrderID = Null.NullInteger;
            Response.Redirect(customerNav.GetNavigationUrl(), false);
        }

		private void grdOrderDetails_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
            //if ((e.Item.ItemType == ListItemType.Item) // canadean change: it is only displaying "impar" lines details
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderDetailsInfo orderDetailsInfo = (OrderDetailsInfo)e.Item.DataItem;

                //Unit Cost
                Label lblODPriceText = (Label)e.Item.FindControl("lblODPriceText");
                lblODPriceText.Text = orderDetailsInfo.UnitCost.ToString("C", LocalFormat);

                //Extended Amount
                Label lblODSubtotalText = (Label)e.Item.FindControl("lblODSubtotalText");
                lblODSubtotalText.Text = orderDetailsInfo.ExtendedAmount.ToString("C", LocalFormat);


                // Show the category / country info
                Label LabelProdDescription = (Label)e.Item.FindControl("LabelProdDescription");
                String selectedDEStr = orderDetailsInfo.ProdReference;
                LabelProdDescription.Text = getDEDetails(selectedDEStr);
                // Response.Write("description: " + LabelProdDescription.Text);
            }
            
		}

        private string getDEDetails(string selectedDEStr)
        {
            string returnText = "";
            if (selectedDEStr.IndexOf(":") > 0)
            {
                char[] splitter = { ';' };
                char[] splitter2 = { ':' };

                String[] selectedDEs = selectedDEStr.Split(splitter);
                CategoryController categoryController = new CategoryController();
                foreach (String selectedDE in selectedDEs)
                {
                    if (selectedDE != "")
                    {
                        String[] options = selectedDE.Split(splitter2);
                        int cat1 = -1;
                        int.TryParse(options[0], out cat1);
                        int cat2 = -1;
                        int.TryParse(options[1], out cat2);

                        CategoryInfo catg1 = categoryController.GetCategory(cat1);
                        CategoryInfo catg2 = categoryController.GetCategory(cat2);
                        //DEProductInfo prod = new DEProductInfo(catg1.CategoryID, catg1.CategoryName, catg2.CategoryID, catg2.CategoryName);
                        returnText = returnText + catg1.CategoryName + " / " + catg2.CategoryName + "<br>";
                    }
                }
            }
            return returnText;
        }


		private void editControl_EditComplete(object sender, EventArgs e)
		{
			customerNav.OrderID = Null.NullInteger;
			Response.Redirect(customerNav.GetNavigationUrl(), false);
		}

        private void lnkbtnSave_Click(object sender, EventArgs e)
        {
            //Update the order status...
            OrderController orderController = new OrderController();
            OrderInfo orderInfo = orderController.GetOrder(customerNav.OrderID);
            
            if (orderInfo != null)
            {
                orderController.UpdateOrder(orderInfo.OrderID, orderInfo.OrderDate, orderInfo.OrderNumber, orderInfo.ShippingAddressID, orderInfo.BillingAddressID, orderInfo.Tax, orderInfo.ShippingCost, true, Convert.ToInt32(ddlOrderStatus.SelectedValue), orderInfo.CustomerID);
                SendOrderStatusChangeEmail(orderInfo.CustomerID, orderInfo.OrderID, ddlOrderStatus.SelectedItem.Text, ddlOrderStatus.SelectedItem.Value == "2", false);
                customerNav.OrderID = Null.NullInteger;
                Response.Redirect(customerNav.GetNavigationUrl(), false);
            }
        }

        private void btnPayViaPayPal_Click(object sender, EventArgs e)
        {
            OrderController orderController = new OrderController();
            OrderInfo orderInfo = orderController.GetOrder(customerNav.OrderID);

            if (orderInfo.OrderStatusID == 2)
            {
                SetupPayPalFields(orderInfo);
            }
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

        private bool VerifyPayment()
        {
            bool isVerified = false;

            GatewayController controller = new GatewayController(Server.MapPath(ModulePath));
            GatewayInfo gateway = controller.GetGateway(storeInfo.GatewayName);
            PayPalSettings _settings = new PayPalSettings(gateway.GetSettings(PortalId));

            _verificationURL = _settings.UseSandbox ? _sandboxVerificationURL : _settings.VerificationURL;
            _paymentURL = _settings.UseSandbox ? _sandboxPaymentURL : _settings.PaymentURL;

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

		#endregion

		#region Private Function

        //private const string _liveVerificationURL = "https://www.paypal.com/cgi-bin/webscr/";
        //private const string _livePaymentURL = "https://www.paypal.com/";
        private const string _sandboxVerificationURL = "https://www.sandbox.paypal.com/cgi-bin/webscr/";
        private const string _sandboxPaymentURL = "https://www.sandbox.paypal.com/";

        private string _verificationURL = string.Empty;
        private string _paymentURL = string.Empty;

        private void CancelOrder(int orderID)
        {
            OrderController orderController = new OrderController();
            OrderInfo orderInfo = orderController.GetOrder(orderID);
            orderInfo.OrderStatusID = (int)OrderInfo.OrderStatusList.Cancelled;
            orderController.UpdateOrder(orderInfo.OrderID, orderInfo.OrderDate, orderInfo.OrderNumber, orderInfo.ShippingAddressID, orderInfo.BillingAddressID, orderInfo.Tax, orderInfo.ShippingCost, true, orderInfo.OrderStatusID, orderInfo.CustomerID);
            //SendOrderStatusChangeEmail(orderInfo.CustomerID, orderInfo.OrderID, GetOrderStatus(orderInfo.OrderStatusID), false, true);
            SendOrderStatusChangeEmail(orderInfo.CustomerID, orderInfo.OrderID, GetOrderStatus(orderInfo.OrderStatusID, orderInfo.OrderIsPlaced), false, true);
        }

        private void DisplayCustomerOrders()
        {
            OrderController orderController = new OrderController();
            ArrayList orders = orderController.GetCustomerOrders(PortalId, customerNav.CustomerID);

            if (orders.Count > 0)
            {
                orderStatusList = orderController.GetOrderStatuses();
                grdOrders.DataSource = orders;
                grdOrders.DataBind();
            }
            else
            {
                lblError.Text = Localization.GetString("NoOrdersFound", this.LocalResourceFile);
            }
        }

        private void SetupPayPalFields(OrderInfo orderInfo)
        {
            IAddressInfo shippingAddress = getShipToAddress(orderInfo.ShippingAddressID);
            IAddressInfo billingAddress = getBillToAddress(orderInfo.BillingAddressID);

            GatewayController controller = new GatewayController(Server.MapPath(ModulePath));
            GatewayInfo gateway = controller.GetGateway(storeInfo.GatewayName);

            PayPalSettings _settings = new PayPalSettings(gateway.GetSettings(PortalId));

            // Ajouté pour la localisation
            CultureInfo ci_enUS = new CultureInfo("en-US");

            //_verificationURL = _settings.UseSandbox ? _sandboxVerificationURL : _liveVerificationURL;
            //_paymentURL = _settings.UseSandbox ? _sandboxPaymentURL : _livePaymentURL;
            _verificationURL = _settings.UseSandbox ? _sandboxVerificationURL : _settings.VerificationURL;
            _paymentURL = _settings.UseSandbox ? _sandboxPaymentURL : _settings.PaymentURL;

            //Set the paypal url as form target
            pnlPayPalTransfer.Visible = true;
            pnlOrderDetails.Visible = false;
            btnGoToPayPal.PostBackUrl = _paymentURL;
            paypalimage2.ImageUrl = _settings.ButtonURL;

            string returnURL = Request.Url + "&PayPalExit=return";
            string cancelURL = Request.Url + "&PayPalExit=cancel";
            string notifyURL = Request.Url + "&PayPalExit=notify";
            
            AddHiddenField("cmd", "_cart");
            AddHiddenField("upload", "1");
            AddHiddenField("business", _settings.PayPalID);
            AddHiddenField("handling_cart", orderInfo.ShippingCost.ToString("0.00", ci_enUS));
            //AddHiddenField("handling_cart", orderInfo.ShippingCost.ToString("0.00"));
            AddHiddenField("charset", _settings.Charset);
            AddHiddenField("currency_code", _settings.Currency);
            AddHiddenField("invoice", orderInfo.OrderID.ToString());
            AddHiddenField("return", returnURL);
            AddHiddenField("cancel_return", cancelURL);
            AddHiddenField("notify_url", notifyURL);
            AddHiddenField("rm", "2");
            AddHiddenField("lc", _settings.Lc);
            //AddHiddenField("lc", "GB");
            _Message = Localization.GetString("PayPalReturnTo", this.LocalResourceFile);
            AddHiddenField("cbt", string.Format(_Message, PortalSettings.PortalName));
            //AddHiddenField("cbt", "Back to " + PortalSettings.PortalName);

            if (orderInfo.Tax > 0)
            {
                AddHiddenField("tax_cart", orderInfo.Tax.ToString("0.00", ci_enUS));
                //AddHiddenField("tax_cart", orderInfo.Tax.ToString("0.00"));
            }

            //Cart Contents...
            OrderController orderController = new OrderController();
            ArrayList cartItems = orderController.GetOrderDetails(orderInfo.OrderID);
            int itemNumber = 1;
            foreach (OrderDetailsInfo itemInfo in cartItems)
            {
                AddHiddenField("item_name_" + itemNumber.ToString(), itemInfo.ModelName);
                AddHiddenField("quantity_" + itemNumber.ToString(), itemInfo.Quantity.ToString());
                AddHiddenField("amount_" + itemNumber.ToString(), itemInfo.UnitCost.ToString("0.00", ci_enUS));
                //AddHiddenField("amount_" + itemNumber.ToString(), itemInfo.UnitCost.ToString("0.00"));
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
                return stringlist[stringlist.Length - 1];
            }
            else
            {
                return string.Empty;
            }
        }

        private void SendOrderStatusChangeEmail(int customerID, int OrderID, string newOrderStatus, bool RequiresPayment, bool SendToStoreOwner)
        {
            StringBuilder emailText = new StringBuilder();
            UserController userController = new UserController();
            UserInfo userInfo = userController.GetUser(PortalId, customerID);

            string storeEmail = storeInfo.DefaultEmailAddress;
            string customerEmail = userInfo.Membership.Email;

            _Message = Localization.GetString("PlacedOrder", this.LocalResourceFile);
            emailText.Append(string.Format(_Message, PortalSettings.PortalName));
            emailText.Append("\r\n");
            emailText.Append("\r\n");
            _Message = Localization.GetString("ChangedStatus", this.LocalResourceFile);
            emailText.Append(string.Format(_Message, OrderID.ToString(), newOrderStatus));

            TabController tabControler = new TabController();
            TabInfo tabInfo = tabControler.GetTab(storeInfo.ShoppingCartPageID, storeInfo.PortalID, true);
            if (RequiresPayment)
            {
                emailText.Append("\r\n");
                emailText.Append("\r\n");
                _Message = Localization.GetString("RequiresPayment", this.LocalResourceFile);
                emailText.Append(string.Format(_Message, tabInfo.TabName, lblParentTitle.Text, storeEmail));
            }
            else
            {
                emailText.Append("\r\n");
                emailText.Append("\r\n");
                _Message = Localization.GetString("AboutChange", this.LocalResourceFile);
                emailText.Append(string.Format(_Message, storeEmail));
            }
            emailText.Append("\r\n");
            emailText.Append("\r\n");
            _Message = Localization.GetString("CheckStatus", this.LocalResourceFile);
            emailText.Append(string.Format(_Message, tabInfo.TabName));
            emailText.Append("\r\n");
            emailText.Append("\r\n");
            emailText.Append(Localization.GetString("OrderThanks", this.LocalResourceFile));
            emailText.Append("\r\n");
            emailText.Append("\r\n");
            emailText.Append("http://" + PortalSettings.PortalAlias.HTTPAlias);
            emailText.Append("\r\n");

            // send email
            SmtpClient smtpClient = new SmtpClient((string)DotNetNuke.Common.Globals.HostSettings["SMTPServer"]);
            System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential((string)DotNetNuke.Common.Globals.HostSettings["SMTPUsername"], (string)DotNetNuke.Common.Globals.HostSettings["SMTPPassword"]);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 25;
            smtpClient.EnableSsl = false;
            smtpClient.Credentials = networkCredential;

            MailMessage message = new MailMessage();
            try
            {
                MailAddress fromAddress = new MailAddress(storeEmail);

                //From address will be given as a MailAddress Object
                message.From = fromAddress;

                // To address collection of MailAddress
                message.To.Add(customerEmail);
                _Message = Localization.GetString("StatusChanged", this.LocalResourceFile);
                message.Subject = string.Format(_Message, storeInfo.Name);

                //Body can be Html or text format
                //Specify true if it  is html message
                message.IsBodyHtml = false;

                message.BodyEncoding = Encoding.Default;

                // Message body content
                message.Body = emailText.ToString();

                // Send SMTP mail
                smtpClient.Send(message);

                if (SendToStoreOwner)
                {
                    message.To.Clear();
                    message.To.Add(storeInfo.DefaultEmailAddress);
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

		private void showOrderDetails(int orderID)
		{
			pnlOrderDetails.Visible = true;
			// Obtain Order Details from Database
			//      OrdersDB orderHistory = new OrdersDB();
			//      Commerce.OrderDetails myOrderDetails = orderHistory.GetOrderDetails(OrderID, CustomerId);
			OrderController orderController = new OrderController();
			OrderInfo order = orderController.GetOrder(orderID);

			if(order != null)
			{
				// Update labels with summary details
                lblTotal.Text = order.OrderTotal.ToString("C", LocalFormat);
                lblPostage.Text = order.ShippingCost.ToString("C", LocalFormat);
                lblTotalIncPostage.Text = order.GrandTotal.ToString("C", LocalFormat);
				lblOrderNumber.Text = order.OrderID.ToString();
                lblOrderDate.Text = order.OrderDate.ToString(_OrderDateFormat);
				lblShipDate.Text = order.ShipDate.ToShortDateString();

                UserController userController = new UserController();
                UserInfo userInfo = userController.GetUser(this.PortalId, order.CustomerID);
                lblOrderCompany.Text = "Profile: " + userInfo.Profile.GetPropertyValue("Company") + "<br>" + "Wisdom: " + DotNetNuke.Modules.Store.WebControls.CustomerAdmin.getUserCompanyName(order.CustomerID);

                //Tax
                if (order.Tax > 0)
                {
                    lblTax.Text = order.Tax.ToString("C", LocalFormat);
                    trTax.Visible = true;
                }
                else
                {
                    trTax.Visible = false;
                }

				getShipToAddress(order.ShippingAddressID);
                getBillToAddress(order.BillingAddressID);
				ArrayList orderDetails = orderController.GetOrderDetails(orderID);

				// if order was found, display it
				if (orderDetails != null) 
				{
					detailsTable.Visible = true;

					// Bind Items to GridControl
					grdOrderDetails.DataSource = orderDetails;
					grdOrderDetails.DataBind();
				}
				// otherwise display an error message
				else 
				{
					lblDetailsError.Text = Localization.GetString("DetailsNotFound", this.LocalResourceFile);
					detailsTable.Visible = false;
				}
			}

            if (order.OrderStatusID == (int)OrderInfo.OrderStatusList.AwaitingPayment
                && storeInfo.GatewayName == "PayPalProvider")
            {
                btnPayViaPayPal.Visible = true;
            }
            else
            {
                btnPayViaPayPal.Visible = false;
            }

            //Cancel button
            spanBR.Visible = false;
            if (storeInfo.AuthorizeCancel && !UserInfo.IsSuperUser)
            {
                if (order.OrderStatusID == (int)OrderInfo.OrderStatusList.AwaitingPayment
                    || order.OrderStatusID == (int)OrderInfo.OrderStatusList.AwaitingStock
                    || order.OrderStatusID == (int)OrderInfo.OrderStatusList.Paid)
                {
                    btnCancel.CommandArgument = order.OrderID.ToString();
                    btnCancel.Click += this.btnCancel_Click;
                    btnCancel.Visible = true;
                    if (btnPayViaPayPal.Visible)
                    {
                        spanBR.Visible = true;
                    }
                }
                else
                {
                    btnCancel.Visible = false;
                }
            }
            else
            {
                btnCancel.Visible = false;
            }

            //if (UserInfo.IsInRole("Administrators"))
            if (UserInfo.IsSuperUser || CheckForAdminRole() || UserInfo.IsInRole("ShopAdmin") || UserInfo.IsInRole("Webmaster"))
            {
                ddlOrderStatus.Visible = true;
                lnkbtnSave.Visible = true;
                lblOrderStatus.Visible = false;

                //Bind order status list...
                ddlOrderStatus.DataSource = orderController.GetOrderStatuses();
                ddlOrderStatus.DataTextField = "OrderStatusText";
                ddlOrderStatus.DataValueField = "OrderStatusID";
                ddlOrderStatus.DataBind();

                //Value...
                if (ddlOrderStatus.Items.FindByValue(order.OrderStatusID.ToString()) != null)
                {
                    ddlOrderStatus.Items.FindByValue(order.OrderStatusID.ToString()).Selected = true;
                }
            }
            else
            {
                /*
                ArrayList orderStatusList = orderController.GetOrderStatuses();
                string OrderStatusText = "";
                foreach (OrderStatus orderStatus in orderStatusList) 
                {
                    if (orderStatus.OrderStatusID == order.OrderStatusID) 
                    {
                        if (order.OrderStatusID > 1)
                        {
                            OrderStatusText = orderStatus.OrderStatusText + " - " + order.ShipDate.ToString(_OrderDateFormat);
                        }
                        else
                        {
                            OrderStatusText = orderStatus.OrderStatusText;
                        }
                        break;
                    }
                }
                */
                string OrderStatusText = "";
                OrderStatusText = GetOrderStatus(order.OrderStatusID, order.OrderIsPlaced);
                if (order.OrderStatusID > 1)
                {
                    OrderStatusText = OrderStatusText + " - " + order.ShipDate.ToString(_OrderDateFormat);
                } 
                ddlOrderStatus.Visible = false;
                lnkbtnSave.Visible = false;
                lblOrderStatus.Visible = true;
                lblOrderStatus.Text = OrderStatusText;
            }

		}

        private bool CheckForAdminRole()
        {
            DotNetNuke.Security.Roles.RoleController roleController = new DotNetNuke.Security.Roles.RoleController();
            //ArrayList roles = roleController.GetUsersInRole(PortalId, "Administrators");
            ArrayList roles = roleController.GetUserRolesByRoleName(PortalId, "Administrators");
            foreach (UserRoleInfo userRoleInfo in roles)
            {
                if (userRoleInfo.UserID == UserId)
                {
                    return true;
                }
            }
            return false;
        }

		private IAddressInfo getShipToAddress(int addressID)
		{
			//Get an instance of the provider
			IAddressProvider addressProvider = StoreController.GetAddressProvider(ModulePath);
			
			IAddressInfo address = addressProvider.GetAddress(addressID);
			
			//SqlDataReader dr = cust.GetSingleAddresses(AddressID);
			//if(dr.Read())

			if(address != null)
			{
				//        lblShipTo.Text = "<i>" + dr["ATTN"].ToString() + "</i><br>" +
				//          dr["Address1"].ToString() + (dr["Address2"].ToString() != "" ? " " + dr["Address2"].ToString() : "") + "<br>" +
				//          dr["City"].ToString() + ", " + dr["RegionCode"].ToString() + " " +
				//          dr["PostalCode"].ToString() + "<br>" + dr["Phone1"].ToString() + (dr["Phone2"].ToString() != "" ? " " + dr["Phone2"].ToString() : "");
				lblShipTo.Text = "<i>" + address.Name + "</i><br>" +
				address.Address1 + (address.Address2 != "" ? "<br>" + address.Address2 : "") + "<br>" +
                address.City + "<br>" + (address.RegionCode != "" ? address.RegionCode + "<br>" : "") +
				address.PostalCode + "<br>" + address.Phone1 + (address.Phone2 != "" ? "<br>" + address.Phone2 : "");
			}
			else
			{
				lblShipTo.Text = Localization.GetString("ShipToAddressNotAvailable", this.LocalResourceFile);
			}

            return address;
		}

        private IAddressInfo getBillToAddress(int addressID)
        {
            //Get an instance of the provider
            IAddressProvider addressProvider = StoreController.GetAddressProvider(ModulePath);
            IAddressInfo address = addressProvider.GetAddress(addressID);

            //SqlDataReader dr = cust.GetSingleAddresses(AddressID);
            //if(dr.Read())

            if (address != null)
            {
                //        lblShipTo.Text = "<i>" + dr["ATTN"].ToString() + "</i><br>" +
                //          dr["Address1"].ToString() + (dr["Address2"].ToString() != "" ? " " + dr["Address2"].ToString() : "") + "<br>" +
                //          dr["City"].ToString() + ", " + dr["RegionCode"].ToString() + " " +
                //          dr["PostalCode"].ToString() + "<br>" + dr["Phone1"].ToString() + (dr["Phone2"].ToString() != "" ? " " + dr["Phone2"].ToString() : "");
                lblBillTo.Text = "<i>" + address.Name + "</i><br>" +
                address.Address1 + (address.Address2 != "" ? "<br>" + address.Address2 : "") + "<br>" +
                address.City + "<br>" + (address.RegionCode != "" ? address.RegionCode + "<br>" : "") +
                address.PostalCode + "<br>" + address.Phone1 + (address.Phone2 != "" ? "<br>" + address.Phone2 : "");
            }
            else
            {
                lblBillTo.Text = Localization.GetString("BillToAddressNotAvailable", this.LocalResourceFile);
            }

            return address;
        }

		#endregion


        protected String getProdDeliveryMethodStr(int deliveryMethod)
        {
            String deliveryMethodStr = "-";
            switch (deliveryMethod)
            {
                case 1:
                    deliveryMethodStr = "PDF";
                    break;
                case 2:
                    deliveryMethodStr = "Hard copy";
                    break;
            }
            return deliveryMethodStr;
        }
	}
}
