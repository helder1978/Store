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
using DotNetNuke;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for WorldPayAdmin.
	/// </summary>
	public partial class WorldPayAdmin : StoreControlBase
	{
		#region Controls
		protected Label lblGateway;
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
		}

		#endregion

		#region StoreControlBase Overrides

		public override object DataSource
		{
			get
			{
				PortalSecurity security = new PortalSecurity();

				WorldPaySettings settings = new WorldPaySettings();
				settings.WorldPayID		= security.InputFilter(txtWorldPayID.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
				//settings.CartName		= security.InputFilter(txtWorldPayCartName.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                //settings.CallbackURL = security.InputFilter(txtWorldPayCallbackURL.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.CallbackPassword = security.InputFilter(txtWorldPayCallbackPassword.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.PaymentURL = security.InputFilter(txtWorldPayPaymentURL.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.Lc = security.InputFilter(txtWorldPayLanguage.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.Charset = security.InputFilter(txtWorldPayCharset.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
				settings.ButtonURL		= security.InputFilter(txtWorldPayButtonURL.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.Currency = security.InputFilter(txtWorldPayCurrency.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.TestMode = chkTestMode.Checked;

                base.DataSource = settings.ToString();
                DataSource = settings.ToString();
				return base.DataSource;
			}
			set
			{
				base.DataSource = value;

				if (base.DataSource != null)
				{
					string gatewaySettings = base.DataSource as string;
					if (gatewaySettings != null)
					{
						WorldPaySettings settings = new WorldPaySettings(gatewaySettings);
						txtWorldPayID.Text = settings.WorldPayID;
						//txtWorldPayCartName.Text = settings.CartName;
                        //txtWorldPayCallbackURL.Text = settings.CallbackURL;
                        txtWorldPayCallbackPassword.Text = settings.CallbackPassword;
                        txtWorldPayPaymentURL.Text = settings.PaymentURL;
                        txtWorldPayLanguage.Text = settings.Lc;
                        txtWorldPayCharset.Text = settings.Charset;
						txtWorldPayButtonURL.Text = settings.ButtonURL;
						txtWorldPayCurrency.Text = settings.Currency;
                        chkTestMode.Checked = settings.TestMode;
					}
				}
			}
		}

		#endregion
	}
}
