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
using System.Collections;
using System.IO;
using System.Text;
using System.Net.Mail;
using DotNetNuke.Common;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider;
using DotNetNuke.Services.Localization;

using DotNetNuke.Services.Mail;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for PaymentControlBase.
	/// </summary>
	public class PaymentControlBase : PortalModuleBase
	{
		#region Constructors

		public PaymentControlBase()
		{
			try
			{
				// Map control's resources to the correct resource file (if it exists)
				string resourceFile = this.GetType().ToString();
				resourceFile = resourceFile.Replace("ASP.","").Replace("_ascx",".ascx").Replace("_","");
				resourceFile = this.ModulePath + "App_LocalResources/" + resourceFile + ".resx";
				this.LocalResourceFile = resourceFile;
			}
			catch
			{
				//Don't care if it fails
			}
		}

		#endregion

		#region Private Declarations
		protected PortalModuleBase _parentControl = null;
		protected ICheckoutControl _checkout = null;
		#endregion

		#region Public Properties/Events

		public event EventHandler PaymentSucceeded;
		public event EventHandler PaymentCancelled;
		public event EventHandler PaymentFailed;

		public ICheckoutControl CheckoutControl
		{
			get { return _checkout; }
			set { _checkout = value; }
		}

		public PortalModuleBase ParentControl
		{
			get { return _parentControl; }
			set { _parentControl = value; }
		}

		#endregion

		#region Protected Methods

		protected void invokePaymentSucceeded()
		{
			if (PaymentSucceeded != null)
			{
				PaymentSucceeded(this, null);
			}

			generateOrderConfirmation();
		}

		protected void invokePaymentCancelled()
		{
			if (PaymentCancelled != null)
			{
				PaymentCancelled(this, null);
			}
		}

		protected void invokePaymentFailed()
		{
			if (PaymentFailed != null)
			{
				PaymentFailed(this, null);
			}
		}

		protected virtual void generateOrderConfirmation()
		{
            Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - Start generateOrderConfirmation", " ", "", "", "", "", "", "");

            StringBuilder emailText = new StringBuilder();
			string textLine = "";
			string storeEmail = "";
			string customerEmail = "";
			
			StoreInfo storeInfo = CheckoutControl.StoreData;
			IAddressInfo billingAddress = CheckoutControl.BillingAddress;
			IAddressInfo shippingAddress = CheckoutControl.ShippingAddress;

            //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 1", " ", "", "", "", "", "", "");
            
            OrderInfo orderInfo = CheckoutControl.OrderInfo;

            if (DotNetNuke.Common.Utilities.Null.IsNull(shippingAddress.Address1) || shippingAddress.Address1.Length == 0)
            {
                // canandean changed: load the address from the order if the address controls are empty
                if (DotNetNuke.Common.Utilities.Null.IsNull(billingAddress.Address1) || billingAddress.Address1.Length == 0)
                {
                    AddressController controller = new AddressController();

                    billingAddress = controller.GetAddress(orderInfo.BillingAddressID);
                    shippingAddress = controller.GetAddress(orderInfo.ShippingAddressID);
                }

                shippingAddress = billingAddress;
            }


            //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 2", " ", "", "", "", "", "", "");
            
            if (storeInfo == null)
			{
				StoreController storeController = new StoreController();
				storeInfo = storeController.GetStoreInfo(PortalId);
            }

            //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 3", " ", "", "", "", "", "", "");
            
            NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();

            if (storeInfo.CurrencySymbol != string.Empty)
            {
                LocalFormat.CurrencySymbol = storeInfo.CurrencySymbol;
            }

            //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 4", " ", "", "", "", "", "", "");
            
            UserController userController = new UserController();
			UserInfo userInfo = userController.GetUser(PortalId, orderInfo.CustomerID);

            //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 5", " ", "", "", "", "", "", "");

			if (storeInfo != null && orderInfo != null && userInfo != null)
			{
                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 6", " ", "", "", "", "", "", "");
                storeEmail = storeInfo.DefaultEmailAddress;
				customerEmail = userInfo.Membership.Email;

				OrderController orderController = new OrderController();
				ArrayList orderDetails = orderController.GetOrderDetails(orderInfo.OrderID);

                TabController tabControler = new TabController();
                TabInfo tabInfo = tabControler.GetTab(storeInfo.ShoppingCartPageID, storeInfo.PortalID, true);

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7", " ", "", "", "", "", "", "");
                
                //Order email header
                String _Message = Services.Localization.Localization.GetString("OrderEmailHeader", this.LocalResourceFile);
                textLine = String.Format(_Message, PortalSettings.PortalName, tabInfo.TabName, storeEmail);
                emailText.Append(textLine + "\r\n\r\n");

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.1", " ", "", "", "", "", "", "");

                //Order number and date
                _Message = Services.Localization.Localization.GetString("OrderNumber", this.LocalResourceFile);
                emailText.Append(_Message + " " + orderInfo.OrderID.ToString());
                emailText.Append("\r\n");
                _Message = Services.Localization.Localization.GetString("OrderDate", this.LocalResourceFile);
                String _DateFormat = Services.Localization.Localization.GetString("OrderDateFormat", this.LocalResourceFile);
                emailText.Append(_Message + " " + orderInfo.OrderDate.ToString(_DateFormat));
                emailText.Append("\r\n");
                emailText.Append("\r\n");

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.2", " ", "", "", "", "", "", "");

                //Order Contents
                _Message = Services.Localization.Localization.GetString("OrderContents", this.LocalResourceFile);
                emailText.Append(_Message);
                emailText.Append("\r\n");
                _Message = Services.Localization.Localization.GetString("OrderItems", this.LocalResourceFile);

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.2.1", " ", "", "", "", "", "", "");
                
                foreach (OrderDetailsInfo item in orderDetails)
                {
                    //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.2.2 " + item.ModelName, " ", "", "", "", "", "", "");

                    //textLine = String.Format(_Message, item.Quantity, item.ModelName, item.UnitCost.ToString("C", LocalFormat));
                    textLine = String.Format(_Message, item.Quantity, item.ModelName, item.ProdCost.ToString("C", LocalFormat));
                    emailText.Append(textLine + "\r\n");
                }
                emailText.Append("\r\n");
                _Message = Services.Localization.Localization.GetString("OrderSubTotal", this.LocalResourceFile);
                emailText.Append(String.Format(_Message, orderInfo.OrderTotal.ToString("C", LocalFormat)));
                emailText.Append("\r\n");
                _Message = Services.Localization.Localization.GetString("OrderShipping", this.LocalResourceFile);
                emailText.Append(String.Format(_Message, orderInfo.ShippingCost.ToString("C", LocalFormat)));

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.2.3", " ", "", "", "", "", "", "");

                if (orderInfo.Tax > 0)
                {
                    emailText.Append("\r\n");
                    _Message = Services.Localization.Localization.GetString("OrderTax", this.LocalResourceFile);
                    emailText.Append(String.Format(_Message, orderInfo.Tax.ToString("C", LocalFormat)));
                }
                emailText.Append("\r\n");
                _Message = Services.Localization.Localization.GetString("OrderTotal", this.LocalResourceFile);
                emailText.Append(String.Format(_Message, orderInfo.GrandTotal.ToString("C", LocalFormat)));
                emailText.Append("\r\n");
                emailText.Append("\r\n");

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.3", " ", "", "", "", "", "", "");

                // canadean changed: add information about company and VAT
                emailText.Append("Company: " + userInfo.Profile.ProfileProperties["Company"].PropertyValue);
                emailText.Append("\r\n");
                emailText.Append("VAT N.: " + userInfo.Profile.ProfileProperties["VATNo"].PropertyValue);
                emailText.Append("\r\n");
                emailText.Append("\r\n");

                //Billing Address
                _Message = Services.Localization.Localization.GetString("OrderBillingAddress", this.LocalResourceFile);

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.3.1", " ", "", "", "", "", "", "");
                
                emailText.Append(_Message);
                emailText.Append("\r\n");
                emailText.Append(billingAddress.Name);
                emailText.Append("\r\n");
                emailText.Append(billingAddress.Address1);

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.3.2", " ", "", "", "", "", "", "");

                if (billingAddress.Address2.Length > 0)
                {
                    emailText.Append("\r\n");
                    emailText.Append(billingAddress.Address2);
                }

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.4", " ", "", "", "", "", "", "");
                
                emailText.Append("\r\n");
                emailText.Append(billingAddress.City);
                emailText.Append("\r\n");
                emailText.Append(billingAddress.RegionCode);
                emailText.Append("\r\n");
                emailText.Append(billingAddress.PostalCode);
                emailText.Append("\r\n");
                emailText.Append(billingAddress.CountryCode);
                emailText.Append("\r\n");

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.5", " ", "", "", "", "", "", "");

                //Shipping Address
                emailText.Append("\r\n");
                _Message = Services.Localization.Localization.GetString("OrderShippingAddress", this.LocalResourceFile);
                emailText.Append(_Message);
                emailText.Append("\r\n");
                emailText.Append(shippingAddress.Name);
                emailText.Append("\r\n");
                emailText.Append(shippingAddress.Address1);
                emailText.Append("\r\n");
                if (shippingAddress.Address2.Length > 0)
                {
                    emailText.Append(shippingAddress.Address2);
                    emailText.Append("\r\n");
                }
                emailText.Append(shippingAddress.City);
                emailText.Append("\r\n");
                emailText.Append(shippingAddress.RegionCode);
                emailText.Append("\r\n");
                emailText.Append(shippingAddress.PostalCode);
                emailText.Append("\r\n");
                emailText.Append(shippingAddress.CountryCode);
                emailText.Append("\r\n");

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.6", " ", "", "", "", "", "", "");

                //Email body footer
                emailText.Append("\r\n");
                _Message = Services.Localization.Localization.GetString("OrderTermsOfUse", this.LocalResourceFile);
                emailText.Append(_Message);
                emailText.Append("\r\n");
                emailText.Append("\r\n");
                _Message = Services.Localization.Localization.GetString("OrderCannotBeProcessed", this.LocalResourceFile);
                emailText.Append(_Message);
                emailText.Append("\r\n");
                emailText.Append("\r\n");
                _Message = Services.Localization.Localization.GetString("OrderThanks", this.LocalResourceFile);
                emailText.Append(_Message);
                emailText.Append("\r\n");
                emailText.Append("\r\n");
                emailText.Append("http://" + PortalSettings.PortalAlias.HTTPAlias);
                emailText.Append("\r\n");

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.7", " ", "", "", "", "", "", "");

                // send email
                SmtpClient smtpClient = new SmtpClient((string)DotNetNuke.Common.Globals.HostSettings["SMTPServer"]);
                DotNetNuke.Services.Mail.Mail mail = new DotNetNuke.Services.Mail.Mail();

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.8", " ", "", "", "", "", "", "");

                System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential((string)DotNetNuke.Common.Globals.HostSettings["SMTPUsername"], (string)DotNetNuke.Common.Globals.HostSettings["SMTPPassword"]);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 25;
                smtpClient.EnableSsl = false;
                smtpClient.Credentials = networkCredential;

                //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 7.9", " ", "", "", "", "", "", "");

                MailMessage message = new MailMessage();
                try
                {
                    //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 8", " ", "", "", "", "", "", "");

                    MailAddress fromAddress = new MailAddress(storeEmail);

                    //From address will be given as a MailAddress Object
                    message.From = fromAddress;

                    // To address collection of MailAddress
                    message.To.Add(customerEmail);
                    _Message = Services.Localization.Localization.GetString("OrderSubject", this.LocalResourceFile);
                    message.Subject = String.Format(_Message, storeInfo.Name);

                    //Body can be Html or text format
                    //Specify true if it  is html message
                    message.IsBodyHtml = false;

                    message.BodyEncoding = Encoding.UTF8;

                    // Message body content
                    message.Body = emailText.ToString();

                    // Send SMTP mail
                    smtpClient.Send(message);

                    _Message = Services.Localization.Localization.GetString("OrderSubjectToAdmin", this.LocalResourceFile);
                    message.Subject = String.Format(_Message, orderInfo.OrderID);
                    message.To.Clear();
                    message.To.Add(storeEmail);
                    message.Priority = System.Net.Mail.MailPriority.High;
                    smtpClient.Send(message);
                    //Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 9", " ", "", "", "", "", "", "");

                }
                catch (Exception ex)
                {
                    Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation exception" + ex.Message + " " + ex.StackTrace, " ", "", "", "", "", "", "");
                    
                }
                Mail.SendMail("helder1978@gmail.com", "helder1978@gmail.com", "", "Canadean Payment Processing - generateOrderConfirmation 10 " + orderInfo.OrderID , " ", "", "", "", "", "", "");
            }
		}
		#endregion

        #region PortalModuleBase Overrides
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                this.LocalResourceFile = Services.Localization.Localization.GetResourceFile(this, this.GetType().BaseType.Name + ".ascx");
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
