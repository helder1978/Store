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
using DotNetNuke.Services.Mail;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for WorldPayCheckout.
	/// </summary>
	public partial class WorldPayPayment : PaymentControlBase
	{
		#region Private Declarations
		public WorldPaySettings _settings = null;
		//private const string _sandboxVerificationURL = "https://www.sandbox.WorldPay.com/cgi-bin/webscr/";
		//private const string _sandboxPaymentURL = "https://www.sandbox.WorldPay.com/";

        private string _callbakURL = string.Empty;
        private string _callbakPassword = string.Empty;
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
            imageButton2.Click += new ImageClickEventHandler(imageButton2_Click);
        }
		#endregion

		#region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            //Mail.SendMail(PortalSettings.Email, "helder1978@gmail.com", "", "Canadean Payment Processing Page_Load ", " ", "", "", "", "", "", "");

            _settings = new WorldPaySettings(CheckoutControl.StoreData.GatewaySettings);

            //_callbakURL = _settings.CallbackURL;
            _callbakPassword = _settings.CallbackPassword;
            _paymentURL = _settings.PaymentURL;

            int orderID = -1;
            // Do we have any special handling?
            WorldPayNavigation nav = new WorldPayNavigation(Request.QueryString);
            switch (nav.WorldPayExit.ToUpper())
            {
                case "CANCEL":
                {
                    invokePaymentCancelled();
                    CheckoutControl.Hide();
                    pnlProceedToWorldPay.Visible = false;
                    return;
                }
                case "RETURN":
                case "NOTIFY":
                {
                    //Mail.SendMail(PortalSettings.Email, "helder1978@gmail.com", "", "Canadean Payment Processing - " + nav.WorldPayExit.ToUpper(), " ", "", "", "", "", "", "");

                    Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - " + nav.WorldPayExit.ToUpper(), " ", "", "", "", "", "", "");
                    // Collect data, invoke success
                    //if (VerifyPayment())
                    if (Request.Form["transStatus"] == "Y")
                    {
                        //Set order status to "Paid"...
                        
                        //if (!Int32.TryParse(Request.Form["cartId"], out orderID))
                        //if (!Int32.TryParse(Request.Form["cartId"], out orderID) && !Int32.TryParse(Request.Params["cartId"], out orderID))
                        if (!Int32.TryParse(Request.Form["cartId"], out orderID))
                        {
                            VerificationFailed();
                            Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - parsing failed ", " ", "", "", "", "", "", "");
                            return;
                        }
                        CheckoutControl.OrderInfo = UpdateOrderStatus(orderID, OrderInfo.OrderStatusList.Paid);

                        try
                        {
                            //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - parsing succeeded 1", " ", "", "", "", "", "", "");
                            readParams("Canadean Payment Processing - parsing succeeded 2");
                        }
                        catch (Exception ex)
                        { }

                        invokePaymentSucceeded();
                        CheckoutControl.Hide();
                        pnlProceedToWorldPay.Visible = false;
                        return;
                    }
                    else
                    {
                        if (Int32.TryParse(Request.Form["cartId"], out orderID))
                        {
                            CheckoutControl.OrderInfo = UpdateOrderStatus(orderID, OrderInfo.OrderStatusList.Cancelled);
                            
                            // Setup email notification
                            string subject = "Canadean - Customer canceled order " + orderID;
                            string body =
                                "This is an automatic notification generated because the customer clicked on 'cancel' on Worldpay when he was trying to pay for the order that he was submitting. Info about the customer below:\r\n\r\n" + 
                                 "Customer name: " + Request.Form["name"] + " (" + Request.Form["email"] + ")" + "\r\n" +
                                 "Address: " + Request.Form["address"] + " " + Request.Form["postcode"] + " " + Request.Form["countryString"] + "\r\n" +
                                 "Tel: " + Request.Form["tel"] + "\r\n" +
                                 "Product: " + Request.Form["desc"] + "\r\n" +
                                 "Cost: " + Request.Form["cost"] + " " + Request.Form["authCurrency"] + "\r\n";
                            Mail.SendMail("info@canadean.com", "debra.richards@canadean.com", "", subject, body, "", "", "", "", "", "");
                            Mail.SendMail("info@canadean.com", "work.helder@gmail.com", "", subject, body, "", "", "", "", "", "");
                            
                        }

                        VerificationFailed();
                        //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - VerifyPayment returned false 1", " ", "", "", "", "", "", "");
                        readParams("Canadean Payment Processing - VerifyPayment returned false 2");
                        return;
                    }
                }
            }

            if (nav.WorldPayExit.Length > 0)
            {
                Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "",
                            "Canadean Payment Processing " + nav.WorldPayExit,
                            "OrderId: " + orderID + " " + DateTime.Now.ToString() + " ",
                            "", "", "", "", "", "");

                //If the WorldPayExit is anything else with length > 0, then don't do any processing
                HttpContext.Current.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID), false);
                return;
            }

            // Continue with display of payment control...
            if ((_settings == null) || (!_settings.IsValid()))
            {
                lblError.Text = Localization.GetString("GatewayNotConfigured", this.LocalResourceFile);
                lblError.Visible = true;
                imageButton1.Visible = false;
                pnlProceedToWorldPay.Visible = false;
                pnlContinue.Visible = false;
                return;
            }
            else
            {
                _Message = Localization.GetString("lblConfirmMessage", this.LocalResourceFile);
                lblConfirmMessage.Text = string.Format(_Message, PortalSettings.PortalName);
                _Message = Localization.GetString("worldpayimage", this.LocalResourceFile);
                worldpayimage.AlternateText = _Message;
                //imageButton1.AlternateText = _Message;

                lblError.Text = string.Empty;
                lblError.Visible = false;
                //imageButton1.Visible = true;
                //imageButton1.ImageUrl = _settings.ButtonURL;
                worldpayimage.ImageUrl = _settings.ButtonURL;
                worldpayimage2.ImageUrl = _settings.ButtonURL;
            }
        }

        private void readParams(string subject)
        {
            string body = "Parâmetros da página" + "\n" + "\n";
            body = body + "Página: " + Request.ServerVariables["PATH_INFO"] + "\n";
            body = body + "Timestamp: " + System.DateTime.Now + "\n";

            body = body + "\n";

            body = body + "Request.Form" + "\n";
            body = body + "***********************" + "\n";
            foreach (string variavel in Request.Form)
            {
                body = body + variavel + ": " + Request.Form[variavel] + "\n";
            }

            body = body + "\n";

            body = body + "Request.QueryString" + "\n";
            body = body + "***********************" + "\n";
            foreach (string variavel in Request.QueryString)
            {
                body = body + variavel + ": " + Request.QueryString[variavel] + "\n";
            }

            body = body + "\n";

            body = body + "Request.ServerVariables" + "\n";
            body = body + "***********************" + "\n";

            foreach (string variavel in Request.ServerVariables)
            {
                body = body + variavel + ": " + Request.ServerVariables[variavel] + "\n";
            }


            body = body + "\n";

            body = body + "Request.Cookies" + "\n";
            body = body + "***********************" + "\n";
            foreach (string variavel in Request.Cookies)
            {
                body = body + variavel + ": " + Request.Cookies[variavel] + "\n";
            }

            body = body + "\n";

            body = body + "Request.ClientCertificate" + "\n";
            body = body + "***********************" + "\n";
            foreach (string variavel in Request.ClientCertificate)
            {
                body = body + variavel + ": " + Request.ClientCertificate[variavel] + "\n";
            }

            body = body + "\n";

            body = body + "Session.SessionID" + "\n";
            body = body + "***********************" + "\n";
            body = body + "SessionID" + ": " + Session.SessionID + "\n";

            try
            {
                processError(subject, body);
            }
            catch (Exception ex)
            {
                // Response.Write(ex.StackTrace);
            }
        }

        private void processError(string subject,string body)
        {
            Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", subject, body, "", "", "", "", "", "");
        }

		protected void imageButton1_Click(object sender, ImageClickEventArgs e)
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

            // ToDo: Insert the new fields into the order
            orderController.UpdateOrder(order.OrderID, order.OrderDate, order.OrderNumber, order.ShippingAddressID, order.BillingAddressID, order.Tax, order.ShippingCost, true, order.OrderStatusID, order.CustomerID);

            return order;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            //Does nothing, but when clicked should redirect to WorldPay...
        }

        protected void imageButton2_Click(object sender, ImageClickEventArgs e)
        {
            //Does nothing, but when clicked should redirect to WorldPay...
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

        private void SetupWorldPayFields()
        {
            IAddressInfo shippingAddress = CheckoutControl.ShippingAddress;
            IAddressInfo billingAddress = CheckoutControl.BillingAddress;

            OrderInfo orderInfo = CheckoutControl.GetOrderDetails();

            CultureInfo ci_enUS = new CultureInfo("en-US");

            //Set the WorldPay url as form target
            btnContinue.PostBackUrl = _paymentURL;

            imageButton2.PostBackUrl = _paymentURL;


            string returnURL = Request.Url + "&WorldPayExit=return";
            string cancelURL = Request.Url + "&WorldPayExit=cancel";
            string notifyURL = Request.Url + "&WorldPayExit=notify";

            //AddHiddenField("cmd", "_cart");
            //AddHiddenField("upload", "1");
            AddHiddenField("instId", _settings.WorldPayID);
            AddHiddenField("cartId", orderInfo.OrderID.ToString());
            //AddHiddenField("handling_cart", orderInfo.ShippingCost.ToString("0.00", ci_enUS));
            //AddHiddenField("charset", _settings.Charset);
            AddHiddenField("currency", _settings.Currency);
            //AddHiddenField("return", returnURL);
            //AddHiddenField("cancel_return", cancelURL);
            //AddHiddenField("notify_url", notifyURL);
            //AddHiddenField("rm", "2");
            //AddHiddenField("lc", _settings.Lc);
            //_Message = Localization.GetString("WorldPayReturnTo", this.LocalResourceFile);
            //AddHiddenField("cbt", string.Format(_Message, PortalSettings.PortalName));
            AddHiddenField("testMode", _settings.TestMode ? "100": "0");

            // hack because worldpay isn't getting the values from forms with enctype="multipart/form-data"
            //Page.Form.Enctype = "text/plain";
            
            Page.Form.Enctype = "application/x-www-form-urlencoded";


            //Tax...
            /*
            if (orderInfo.Tax > 0)
            {
                AddHiddenField("tax_cart", orderInfo.Tax.ToString("0.00", ci_enUS));
            }
            */
            //Cart Contents...
            /*
            ArrayList cartItems = CurrentCart.GetItems(PortalId);
            int itemNumber = 1;
            foreach (ItemInfo itemInfo in cartItems)
            {
                AddHiddenField("item_name_" + itemNumber.ToString(), itemInfo.Manufacturer + (itemInfo.Manufacturer.Length > 0 ? " " : "") + itemInfo.ModelName);
                AddHiddenField("quantity_" + itemNumber.ToString(), itemInfo.Quantity.ToString());
                AddHiddenField("amount_" + itemNumber.ToString(), itemInfo.UnitCost.ToString("0.00", ci_enUS));
                itemNumber++;
            }
            */
            //AddHiddenField("amount", CurrentCart.GetInfo(PortalId).Total.ToString("0.00"));
            AddHiddenField("amount", orderInfo.GrandTotal.ToString());
            //AddHiddenField("desc", CurrentCart.GetInfo(PortalId).CartID);
            string desc = "";
            ArrayList cartItems = CurrentCart.GetItems(PortalId);
            bool firstTime = true;
            string separator = "";
            foreach (ItemInfo itemInfo in cartItems)
            {
                if (firstTime)
                {
                    firstTime = false;
                }
                else {
                    separator = " + ";
                }
                desc += separator + itemInfo.ProductTitle;
            }
            AddHiddenField("desc", desc);


            //Customer Address...
            AddHiddenField("email", UserInfo.Membership.Email);
            AddHiddenField("name", billingAddress.Name);

            AddHiddenField("address", billingAddress.Address1 + " " + billingAddress.City);
            //AddHiddenField("address2", billingAddress.Address2);
            //AddHiddenField("city", billingAddress.City);
            AddHiddenField("postcode", billingAddress.PostalCode);
            //AddHiddenField("country", (billingAddress.CountryCode.Equals("United Kingdom") ? "GB" : billingAddress.CountryCode));
            AddHiddenField("country", getCountryCode(billingAddress.CountryCode));

            AddHiddenField("tel", billingAddress.Phone1);
        }

        private string getCountryCode(string countryName)
        {
            Hashtable ht = new Hashtable();
            ht.Add("Andorra", "AD");
            ht.Add("United Arab Emirates", "AE");
            ht.Add("Afghanistan", "AF");
            ht.Add("Antigua and Barbuda", "AG");
            ht.Add("Anguilla", "AI");
            ht.Add("Albania", "AL");
            ht.Add("Armenia", "AM");
            ht.Add("Netherlands Antilles", "AN");
            ht.Add("Angola", "AO");
            ht.Add("Antarctica", "AQ");
            ht.Add("Argentina", "AR");
            ht.Add("American Samoa", "AS");
            ht.Add("Austria", "AT");
            ht.Add("Australia", "AU");
            ht.Add("Aruba", "AW");
            ht.Add("Azerbaijan", "AZ");
            ht.Add("Bosnia and Herzegovina", "BA");
            ht.Add("Barbados", "BB");
            ht.Add("Bangladesh", "BD");
            ht.Add("Belgium", "BE");
            ht.Add("Burkina Faso", "BF");
            ht.Add("Bulgaria", "BG");
            ht.Add("Bahrain", "BH");
            ht.Add("Burundi", "BI");
            ht.Add("Benin", "BJ");
            ht.Add("Bermuda", "BM");
            ht.Add("Brunei Darussalam", "BN");
            ht.Add("Bolivia", "BO");
            ht.Add("Brazil", "BR");
            ht.Add("Bahamas", "BS");
            ht.Add("Bhutan", "BT");
            ht.Add("Bouvet Island", "BV");
            ht.Add("Botswana", "BW");
            ht.Add("Belarus", "BY");
            ht.Add("Belize", "BZ");
            ht.Add("Canada", "CA");
            ht.Add("Cocos (Keeling) Islands", "CC");
            ht.Add("Congo, The Democratic Republic of the", "CD");
            ht.Add("Central African Republic", "CF");
            ht.Add("Congo", "CG");
            ht.Add("Switzerland", "CH");
            ht.Add("Cote D'Ivoire", "CI");
            ht.Add("Cook Islands", "CK");
            ht.Add("Chile", "CL");
            ht.Add("Cameroon", "CM");
            ht.Add("China", "CN");
            ht.Add("Colombia", "CO");
            ht.Add("Costa Rica", "CR");
            ht.Add("Cuba", "CU");
            ht.Add("Cape Verde", "CV");
            ht.Add("Christmas Island", "CX");
            ht.Add("Cyprus", "CY");
            ht.Add("Czech Republic", "CZ");
            ht.Add("Germany", "DE");
            ht.Add("Djibouti", "DJ");
            ht.Add("Denmark", "DK");
            ht.Add("Dominica", "DM");
            ht.Add("Dominican Republic", "DO");
            ht.Add("Algeria", "DZ");
            ht.Add("Ecuador", "EC");
            ht.Add("Estonia", "EE");
            ht.Add("Egypt", "EG");
            ht.Add("Western Sahara", "EH");
            ht.Add("Eritrea", "ER");
            ht.Add("Spain", "ES");
            ht.Add("Ethiopia", "ET");
            ht.Add("Finland", "FI");
            ht.Add("Fiji", "FJ");
            ht.Add("Falkland Islands (Malvinas)", "FK");
            ht.Add("Micronesia, Federated States of", "FM");
            ht.Add("Faroe Islands", "FO");
            ht.Add("France", "FR");
            ht.Add("France, Metropolitan", "FX");
            ht.Add("Gabon", "GA");
            ht.Add("United Kingdom", "GB");
            ht.Add("Grenada", "GD");
            ht.Add("Georgia", "GE");
            ht.Add("French Guiana", "GF");
            ht.Add("Ghana", "GH");
            ht.Add("Gibraltar", "GI");
            ht.Add("Greenland", "GL");
            ht.Add("Gambia", "GM");
            ht.Add("Guinea", "GN");
            ht.Add("Guadeloupe", "GP");
            ht.Add("Equatorial Guinea", "GQ");
            ht.Add("Greece", "GR");
            ht.Add("South Georgia and the South Sandwich Islands", "GS");
            ht.Add("Guatemala", "GT");
            ht.Add("Guam", "GU");
            ht.Add("Guinea-Bissau", "GW");
            ht.Add("Guyana", "GY");
            ht.Add("Hong Kong", "HK");
            ht.Add("Heard Island and McDonald Islands", "HM");
            ht.Add("Honduras", "HN");
            ht.Add("Croatia", "HR");
            ht.Add("Haiti", "HT");
            ht.Add("Hungary", "HU");
            ht.Add("Indonesia", "ID");
            ht.Add("Ireland", "IE");
            ht.Add("Israel", "IL");
            ht.Add("India", "IN");
            ht.Add("British Indian Ocean Territory", "IO");
            ht.Add("Iraq", "IQ");
            ht.Add("Iran, Islamic Republic of", "IR");
            ht.Add("Iceland", "IS");
            ht.Add("Italy", "IT");
            ht.Add("Jamaica", "JM");
            ht.Add("Jordan", "JO");
            ht.Add("Japan", "JP");
            ht.Add("Kenya", "KE");
            ht.Add("Kyrgyzstan", "KG");
            ht.Add("Cambodia", "KH");
            ht.Add("Kiribati", "KI");
            ht.Add("Comoros", "KM");
            ht.Add("Saint Kitts and Nevis", "KN");
            ht.Add("Korea, Democratic People's Republic of", "KP");
            ht.Add("Korea, Republic of", "KR");
            ht.Add("Kuwait", "KW");
            ht.Add("Cayman Islands", "KY");
            ht.Add("Kazakstan", "KZ");
            ht.Add("Lao People's Democratic Republic", "LA");
            ht.Add("Lebanon", "LB");
            ht.Add("Saint Lucia", "LC");
            ht.Add("Liechtenstein", "LI");
            ht.Add("Sri Lanka", "LK");
            ht.Add("Liberia", "LR");
            ht.Add("Lesotho", "LS");
            ht.Add("Lithuania", "LT");
            ht.Add("Luxembourg", "LU");
            ht.Add("Latvia", "LV");
            ht.Add("Libyan Arab Jamahiriya", "LY");
            ht.Add("Morocco", "MA");
            ht.Add("Monaco", "MC");
            ht.Add("Moldova, Republic of", "MD");
            ht.Add("Madagascar", "MG");
            ht.Add("Marshall Islands", "MH");
            ht.Add("Macedonia, the Former Yugoslav Republic of", "MK");
            ht.Add("Mali", "ML");
            ht.Add("Myanmar", "MM");
            ht.Add("Mongolia", "MN");
            ht.Add("Macau", "MO");
            ht.Add("Northern Mariana Islands", "MP");
            ht.Add("Martinique", "MQ");
            ht.Add("Mauritania", "MR");
            ht.Add("Montserrat", "MS");
            ht.Add("Malta", "MT");
            ht.Add("Mauritius", "MU");
            ht.Add("Maldives", "MV");
            ht.Add("Malawi", "MW");
            ht.Add("Mexico", "MX");
            ht.Add("Malaysia", "MY");
            ht.Add("Mozambique", "MZ");
            ht.Add("Namibia", "NA");
            ht.Add("New Caledonia", "NC");
            ht.Add("Niger", "NE");
            ht.Add("Norfolk Island", "NF");
            ht.Add("Nigeria", "NG");
            ht.Add("Nicaragua", "NI");
            ht.Add("Netherlands", "NL");
            ht.Add("Norway", "NO");
            ht.Add("Nepal", "NP");
            ht.Add("Nauru", "NR");
            ht.Add("Niue", "NU");
            ht.Add("New Zealand", "NZ");
            ht.Add("Oman", "OM");
            ht.Add("Panama", "PA");
            ht.Add("Peru", "PE");
            ht.Add("French Polynesia", "PF");
            ht.Add("Papua New Guinea", "PG");
            ht.Add("Philippines", "PH");
            ht.Add("Pakistan", "PK");
            ht.Add("Poland", "PL");
            ht.Add("Saint Pierre and Miquelon", "PM");
            ht.Add("Pitcairn", "PN");
            ht.Add("Puerto Rico", "PR");
            ht.Add("Palestinian Territory, Occupied", "PS");
            ht.Add("Portugal", "PT");
            ht.Add("Palau", "PW");
            ht.Add("Paraguay", "PY");
            ht.Add("Qatar", "QA");
            ht.Add("Reunion", "RE");
            ht.Add("Romania", "RO");
            ht.Add("Russian Federation", "RU");
            ht.Add("Rwanda", "RW");
            ht.Add("Saudi Arabia", "SA");
            ht.Add("Solomon Islands", "SB");
            ht.Add("Seychelles", "SC");
            ht.Add("Sudan", "SD");
            ht.Add("Sweden", "SE");
            ht.Add("Singapore", "SG");
            ht.Add("Saint Helena", "SH");
            ht.Add("Slovenia", "SI");
            ht.Add("Svalbard and Jan Mayen", "SJ");
            ht.Add("Slovakia", "SK");
            ht.Add("Sierra Leone", "SL");
            ht.Add("San Marino", "SM");
            ht.Add("Senegal", "SN");
            ht.Add("Somalia", "SO");
            ht.Add("Suriname", "SR");
            ht.Add("Sao Tome and Principe", "ST");
            ht.Add("El Salvador", "SV");
            ht.Add("Syrian Arab Republic", "SY");
            ht.Add("Swaziland", "SZ");
            ht.Add("Turks and Caicos Islands", "TC");
            ht.Add("Chad", "TD");
            ht.Add("French Southern Territories", "TF");
            ht.Add("Togo", "TG");
            ht.Add("Thailand", "TH");
            ht.Add("Tajikistan", "TJ");
            ht.Add("Tokelau", "TK");
            ht.Add("Turkmenistan", "TM");
            ht.Add("Tunisia", "TN");
            ht.Add("Tonga", "TO");
            ht.Add("East Timor", "TP");
            ht.Add("Turkey", "TR");
            ht.Add("Trinidad and Tobago", "TT");
            ht.Add("Tuvalu", "TV");
            ht.Add("Taiwan, Province of China", "TW");
            ht.Add("Tanzania, United Republic of", "TZ");
            ht.Add("Ukraine", "UA");
            ht.Add("Uganda", "UG");
            ht.Add("United States Minor Outlying Islands", "UM");
            ht.Add("United States", "US");
            ht.Add("Uruguay", "UY");
            ht.Add("Uzbekistan", "UZ");
            ht.Add("Holy See (Vatican City State)", "VA");
            ht.Add("Saint Vincent and the Grenadines", "VC");
            ht.Add("Venezuela", "VE");
            ht.Add("Virgin Islands, British", "VG");
            ht.Add("Virgin Islands, U.S.", "VI");
            ht.Add("Vietnam", "VN");
            ht.Add("Vanuatu", "VU");
            ht.Add("Wallis and Futuna", "WF");
            ht.Add("Samoa", "WS");
            ht.Add("Yemen", "YE");
            ht.Add("Mayotte", "YT");
            ht.Add("Yugoslavia", "YU");
            ht.Add("South Africa", "ZA");
            ht.Add("Zambia", "ZM");
            ht.Add("Zaire", "ZR");
            ht.Add("Zimbabwe", "ZW");

            // {"N/A", "Asia/Pacific Region", "Europe", "Andorra", "United Arab Emirates", "Afghanistan", "Antigua and Barbuda", "Anguilla", "Albania", "Armenia", "Netherlands Antilles", "Angola", "Antarctica", "Argentina", "American Samoa", "Austria", "Australia", "Aruba", "Azerbaijan", "Bosnia and Herzegovina", "Barbados", "Bangladesh", "Belgium", "Burkina Faso", "Bulgaria", "Bahrain", "Burundi", "Benin", "Bermuda", "Brunei Darussalam", "Bolivia", "Brazil", "Bahamas", "Bhutan", "Bouvet Island", "Botswana", "Belarus", "Belize", "Canada", "Cocos (Keeling) Islands", "Congo, The Democratic Republic of the", "Central African Republic", "Congo", "Switzerland", "Cote D'Ivoire", "Cook Islands", "Chile", "Cameroon", "China", "Colombia", "Costa Rica", "Cuba", "Cape Verde", "Christmas Island", "Cyprus", "Czech Republic", "Germany", "Djibouti", "Denmark", "Dominica", "Dominican Republic", "Algeria", "Ecuador", "Estonia", "Egypt", "Western Sahara", "Eritrea", "Spain", "Ethiopia", "Finland", "Fiji", "Falkland Islands (Malvinas)", "Micronesia, Federated States of", "Faroe Islands", "France", "France, Metropolitan", "Gabon", "United Kingdom", "Grenada", "Georgia", "French Guiana", "Ghana", "Gibraltar", "Greenland", "Gambia", "Guinea", "Guadeloupe", "Equatorial Guinea", "Greece", "South Georgia and the South Sandwich Islands", "Guatemala", "Guam", "Guinea-Bissau", "Guyana", "Hong Kong", "Heard Island and McDonald Islands", "Honduras", "Croatia", "Haiti", "Hungary", "Indonesia", "Ireland", "Israel", "India", "British Indian Ocean Territory", "Iraq", "Iran, Islamic Republic of", "Iceland", "Italy", "Jamaica", "Jordan", "Japan", "Kenya", "Kyrgyzstan", "Cambodia", "Kiribati", "Comoros", "Saint Kitts and Nevis", "Korea, Democratic People's Republic of", "Korea, Republic of", "Kuwait", "Cayman Islands", "Kazakstan", "Lao People's Democratic Republic", "Lebanon", "Saint Lucia", "Liechtenstein", "Sri Lanka", "Liberia", "Lesotho", "Lithuania", "Luxembourg", "Latvia", "Libyan Arab Jamahiriya", "Morocco", "Monaco", "Moldova, Republic of", 
            //"Madagascar", "Marshall Islands", "Macedonia, the Former Yugoslav Republic of", "Mali", "Myanmar", "Mongolia", "Macau", "Northern Mariana Islands", "Martinique", "Mauritania", "Montserrat", "Malta", "Mauritius", "Maldives", "Malawi", "Mexico", "Malaysia", "Mozambique", "Namibia", "New Caledonia", "Niger", "Norfolk Island", "Nigeria", "Nicaragua", "Netherlands", "Norway", "Nepal", "Nauru", "Niue", "New Zealand", "Oman", "Panama", "Peru", "French Polynesia", "Papua New Guinea", "Philippines", "Pakistan", "Poland", "Saint Pierre and Miquelon", "Pitcairn", "Puerto Rico", "Palestinian Territory, Occupied", "Portugal", "Palau", "Paraguay", "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saudi Arabia", "Solomon Islands", "Seychelles", "Sudan", "Sweden", "Singapore", "Saint Helena", "Slovenia", "Svalbard and Jan Mayen", "Slovakia", "Sierra Leone", "San Marino", "Senegal", "Somalia", "Suriname", "Sao Tome and Principe", "El Salvador", "Syrian Arab Republic", "Swaziland", "Turks and Caicos Islands", "Chad", "French Southern Territories", "Togo", "Thailand", "Tajikistan", "Tokelau", "Turkmenistan", "Tunisia", "Tonga", "East Timor", "Turkey", "Trinidad and Tobago", "Tuvalu", "Taiwan, Province of China", "Tanzania, United Republic of", "Ukraine", "Uganda", "United States Minor Outlying Islands", "United States", "Uruguay", "Uzbekistan", "Holy See (Vatican City State)", "Saint Vincent and the Grenadines", "Venezuela", "Virgin Islands, British", "Virgin Islands, U.S.", "Vietnam", "Vanuatu", "Wallis and Futuna", "Samoa", "Yemen", "Mayotte", "Yugoslavia", "South Africa", "Zambia", "Zaire", "Zimbabwe", "Anonymous Proxy", "Satellite Provider"}
            // {"--", "AP", "EU", "AD", "AE", "AF", "AG", "AI", "AL", "AM", "AN", "AO", "AQ", "AR", "AS", "AT", "AU", "AW", "AZ", "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BM", "BN", "BO", "BR", "BS", "BT", "BV", "BW", "BY", "BZ", "CA", "CC", "CD", "CF", "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "CR", "CU", "CV", "CX", "CY", "CZ", "DE", "DJ", "DK", "DM", "DO", "DZ", "EC", "EE", "EG", "EH", "ER", "ES", "ET", "FI", "FJ", "FK", "FM", "FO", "FR", "FX", "GA", "GB", "GD", "GE", "GF", "GH", "GI", "GL", "GM", "GN", "GP", "GQ", "GR", "GS", "GT", "GU", "GW", "GY", "HK", "HM", "HN", "HR", "HT", "HU", "ID", "IE", "IL", "IN", "IO", "IQ", "IR", "IS", "IT", "JM", "JO", "JP", "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW", "KY", "KZ", "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU", "LV", "LY", "MA", "MC", "MD", "MG", "MH", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ", "NA", "NC", "NE", "NF", "NG", "NI", "NL", "NO", "NP", "NR", "NU", "NZ", "OM", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "PR", "PS", "PT", "PW", "PY", "QA", "RE", "RO", "RU", "RW", "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SR", "ST", "SV", "SY", "SZ", "TC", "TD", "TF", "TG", "TH", "TJ", "TK", "TM", "TN", "TO", "TP", "TR", "TT", "TV", "TW", "TZ", "UA", "UG", "UM", "US", "UY", "UZ", "VA", "VC", "VE", "VG", "VI", "VN", "VU", "WF", "WS", "YE", "YT", "YU", "ZA", "ZM", "ZR", "ZW", "A1", "A2"}
            return (string)ht[countryName];
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
            pnlProceedToWorldPay.Visible = false;
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
            _Message = Localization.GetString("worldpayimage2", this.LocalResourceFile);
            worldpayimage2.AlternateText = _Message;

            CheckoutControl.Hide();

            pnlContinue.Visible = true;
            pnlProceedToWorldPay.Visible = false;

            SetupWorldPayFields();

            //generateOrderConfirmation(); // canadean changed: why do we send email when the payment wasn't confirmed yet?

            //Clear basket
            CurrentCart.ClearItems(PortalId);

            //Clear cookies
            SetOrderIdCookie(-1);
        }

        // ToDo: check if we can verify if the order was confirmed
		private bool VerifyPayment()
		{
			// bool isVerified = true;
            
			bool isVerified = false;

            WorldPayIPNParameters ipn = new WorldPayIPNParameters(Request.Form);
			if (ipn.IsValid)
			{
			    isVerified = true;
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
