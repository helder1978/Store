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
	/// Summary description for EmailCheckout.
	/// </summary>
	public partial class EmailPayment : PaymentControlBase
	{
		#region Private Declarations
		public EmailSettings _settings = null;
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
			try
			{
				lblError.Text = string.Empty;
				lblError.Visible = false;
                String _Message = Localization.GetString("lblConfirmMessage", this.LocalResourceFile);
                lblConfirmMessage.Text = string.Format(_Message, PortalSettings.PortalName);
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

        protected void btnConfirmOrder_Click(object sender, EventArgs e)
        {
            ConfirmOrder();
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

            String _Message = Localization.GetString("lblOrderNumber", this.LocalResourceFile);
            lblOrderNumber.Text = string.Format(_Message, orderInfo.OrderID);

            generateOrderConfirmation();

            CheckoutControl.Hide();

            pnlConfirmed.Visible = true;
            pnlProceedToEmail.Visible = false;

            //Clear basket
            CurrentCart.ClearItems(PortalId);

            //Clear cookies
            SetOrderIdCookie(-1);
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
