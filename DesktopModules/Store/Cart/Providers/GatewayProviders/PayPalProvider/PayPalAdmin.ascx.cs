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
	/// Summary description for PayPalAdmin.
	/// </summary>
	public partial class PayPalAdmin : StoreControlBase
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

				PayPalSettings settings = new PayPalSettings();
				settings.PayPalID		= security.InputFilter(txtPayPalID.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
				//settings.CartName		= security.InputFilter(txtPayPalCartName.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.VerificationURL = security.InputFilter(txtPayPalVerificationURL.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.PaymentURL = security.InputFilter(txtPayPalPaymentURL.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.Lc = security.InputFilter(txtPayPalLanguage.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.Charset = security.InputFilter(txtPayPalCharset.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
				settings.ButtonURL		= security.InputFilter(txtPayPalButtonURL.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.Currency = security.InputFilter(txtPayPalCurrency.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
                settings.UseSandbox     = chkUseSandbox.Checked;
                try
                {
                    settings.SurchargeFixed = Decimal.Parse(txtSurchargeFixed.Text);
                    lblError.Visible = false;                    
                    txtSurchargeFixed.ForeColor = System.Drawing.Color.Empty;
                    txtSurchargeFixed.BorderColor = System.Drawing.Color.Empty;
                }
                catch (Exception)
                {
                    lblError.Visible = true;
                    //lblError.Text = "Please specify a numeric fixed surcharge (don't include a currency symbol).";
                    lblError.Text = Localization.GetString("ErrorFixedSurcharge", this.LocalResourceFile);
                    txtSurchargeFixed.ForeColor = System.Drawing.Color.Red;
                    txtSurchargeFixed.BorderColor = System.Drawing.Color.Red;
                    txtSurchargeFixed.Text = "0.00";
                }

                try
                {
                    settings.SurchargePercent = Decimal.Parse(txtSurchargePercent.Text);
                    lblError.Visible = false;
                    txtSurchargePercent.ForeColor = System.Drawing.Color.Empty;
                    txtSurchargePercent.BorderColor = System.Drawing.Color.Empty;                    
                }
                catch (Exception)
                {
                    lblError.Visible = true;
                    //lblError.Text = "Please specify a numeric percentage surcharge (don't include a percent symbol).";
                    lblError.Text = Localization.GetString("ErrorPercentageSurcharge", this.LocalResourceFile);
                    txtSurchargePercent.ForeColor = System.Drawing.Color.Red;
                    txtSurchargePercent.BorderColor = System.Drawing.Color.Red;
                    txtSurchargePercent.Text = "0.00";
                }

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
						PayPalSettings settings = new PayPalSettings(gatewaySettings);
						txtPayPalID.Text = settings.PayPalID;
						//txtPayPalCartName.Text = settings.CartName;
                        txtPayPalVerificationURL.Text = settings.VerificationURL;
                        txtPayPalPaymentURL.Text = settings.PaymentURL;
                        txtPayPalLanguage.Text = settings.Lc;
                        txtPayPalCharset.Text = settings.Charset;
						txtPayPalButtonURL.Text = settings.ButtonURL;
						txtPayPalCurrency.Text = settings.Currency;
                        txtSurchargePercent.Text = settings.SurchargePercent < 0 ? "" : settings.SurchargePercent.ToString("0.00");
                        txtSurchargeFixed.Text = settings.SurchargeFixed < 0 ? "" : settings.SurchargeFixed.ToString("0.00");
                        chkUseSandbox.Checked = settings.UseSandbox;
					}
				}
			}
		}

		#endregion
	}
}
