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
using System.Reflection;
using System.Xml;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for PayPalSettings.
	/// </summary>
	public class PayPalSettings : GatewaySettings
	{
		#region Constructors
		public PayPalSettings()
		{
		}

		public PayPalSettings(string xml)
		{
			FromString(xml);
		}
		#endregion

		#region Private Declarations
		private string _payPalID = string.Empty;
        //private string _cartName = string.Empty;
		private string _currency = "USD";
        private string _buttonURL = "https://www.paypal.com/en_US/i/bnr/horizontal_solution_PP.gif";
        private decimal _surchargePercent = 0;
        private decimal _surchargeFixed = 0;
        private bool _useSandbox = false;
        private string _verificationURL = "https://www.paypal.com/cgi-bin/webscr/";
        private string _paymentURL = "https://www.paypal.com/";
        private string _lc = "US";
        private string _charset = "UTF-8";
        #endregion

		#region Public Properties
		public string PayPalID
		{
			get { return _payPalID; }
			set { _payPalID = value; }
		}

        //public string CartName
        //{
        //    // Need to replace any double apostrophe's because the PortalSecurity.InputFilter 
        //    // replaces the single apostrophe's when using the PortalSecurity.FilterFlag.NoSQL flag.
        //    // It is necessary here because the non-admin users have edit rights to this field.
        //    get { return _cartName.Replace("''", "'"); }
        //    set { _cartName = value; }
        //}

		public string Currency
		{
			get { return _currency; }
			set { _currency = value; }
		}

		public string ButtonURL
		{
			get { return _buttonURL; }
			set { _buttonURL = value; }
		}

        public decimal SurchargePercent
        {
            get { return _surchargePercent; }
            set { _surchargePercent = value; }
        }

        public decimal SurchargeFixed
        {
            get { return _surchargeFixed; }
            set { _surchargeFixed = value; }
        }

        public bool UseSandbox
        {
            get { return _useSandbox; }
            set { _useSandbox = value; }
        }

        public string VerificationURL
        {
            get { return _verificationURL; }
            set { _verificationURL = value; }
        }

        public string PaymentURL
        {
            get { return _paymentURL; }
            set { _paymentURL = value; }
        }

        public string Lc
        {
            get { return _lc; }
            set { _lc = value; }
        }

        public string Charset
        {
            get { return _charset; }
            set { _charset = value; }
        }
        #endregion

		#region GatewaySettings Overrides
		public override bool IsValid()
		{
			return ((_payPalID != string.Empty) &&
					(_currency != string.Empty) && (_buttonURL != string.Empty));
		}
		#endregion
	}
}
