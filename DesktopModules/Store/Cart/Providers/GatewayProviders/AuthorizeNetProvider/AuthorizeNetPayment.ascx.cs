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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using DotNetNuke;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for AuthNetCheckout.
	/// </summary>
    public partial class AuthorizeNetPayment : PaymentControlBase
	{
		#region Controls
		protected System.Web.UI.WebControls.Label lblNumber;
		protected System.Web.UI.WebControls.Label lblDate;
		protected System.Web.UI.WebControls.Label lblValidationNumber;
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

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!Request.IsSecureConnection)
            {
                StoreInfo storeInfo = CheckoutControl.StoreData;
                AuthNetSettings settings = new AuthNetSettings(storeInfo.GatewaySettings);

                if (!settings.IsTest)
                {
                    throw new ApplicationException(Localization.GetString("ErrorNotSecured", this.LocalResourceFile));
                }
            }

			if(! Page.IsPostBack)
			{
                String _Message = Localization.GetString("lblConfirmMessage", this.LocalResourceFile);
                lblConfirmMessage.Text = string.Format(_Message, PortalSettings.PortalName);

                for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 10; i++)
				{
					ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString().Substring(2)));
				}

                ddlMonth.Items.FindByValue(((string)("0" + DateTime.Now.Month.ToString())).Substring(0,2)).Selected = true;
			}
		}

		protected void btnProcess_Click(object sender, EventArgs e)
		{
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

			StoreInfo storeInfo = CheckoutControl.StoreData;
			IAddressInfo shippingAddress = CheckoutControl.ShippingAddress;
			IAddressInfo billingAddress = CheckoutControl.BillingAddress;
			OrderInfo orderInfo = CheckoutControl.GetFinalizedOrderInfo();
            //lblOrderNumber.Text = orderInfo.OrderID.ToString();

			PortalSecurity security = new PortalSecurity();
	
			TransactionDetails transaction = new TransactionDetails();
			//transaction.CardType = (TransactionDetails.CardTypes)Enum.Parse(typeof(TransactionDetails.CardTypes), rbCard.SelectedValue);
            //transaction.CardType = null;
			transaction.CardNumber = security.InputFilter(txtNumber.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);;
			//transaction.NameOnCard = security.InputFilter(txtName.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);;
            transaction.NameOnCard = string.Empty;
			transaction.VerificationCode = int.Parse(txtVer.Text);
			transaction.ExpirationMonth = int.Parse(ddlMonth.SelectedValue);
			transaction.ExpirationYear = int.Parse(ddlYear.SelectedValue);

			if (transaction.IsValid())
			{
				AuthNetGatewayProvider provider = new AuthNetGatewayProvider(storeInfo.GatewaySettings);

				TransactionResult orderResult = provider.ProcessTransaction(shippingAddress, billingAddress, orderInfo, transaction.ToString());
				if (!orderResult.Succeeded)
				{
                    litError.Text = Localization.GetString(orderResult.Message.ToString(), this.LocalResourceFile);
				}
				else
				{
					invokePaymentSucceeded();

                    //Clear basket
                    CurrentCart.ClearItems(PortalId);

                    //Clear cookies
                    SetOrderIdCookie(-1);
				}
			}
			else
			{
                litError.Text = Localization.GetString("ErrorCardNotValid", this.LocalResourceFile);
			}
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
	}
}
