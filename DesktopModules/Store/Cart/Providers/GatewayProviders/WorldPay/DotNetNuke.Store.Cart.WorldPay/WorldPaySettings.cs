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
	/// Summary description for WorldPaySettings.
	/// </summary>
	public class WorldPaySettings : GatewaySettings
	{
		#region Constructors
		public WorldPaySettings()
		{
		}

		public WorldPaySettings(string xml)
		{
			FromString(xml);
		}
		#endregion

		#region Private Declarations
		private string _worldPayID = string.Empty;
        //private string _cartName = string.Empty;
		private string _currency = "GBP";
        private string _buttonURL = "http://www.canadean.com/images/canadean/worldpay.gif";
        private bool _testMode = false;
        //private string _callbackURL = "http://www.canadean.com/Default.aspx?TabId=60&ctl=Checkout&mid=423&WorldPayExit=return";
        private string _callbackPassword = "";
        private string _paymentURL = "https://select.worldpay.com/wcc/purchase";
        private string _lc = "en-GB";
        private string _charset = "UTF-8";
        #endregion

		#region Public Properties
		public string WorldPayID
		{
			get { return _worldPayID; }
			set { _worldPayID = value; }
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

        public string CallbackPassword
        {
            get { return _callbackPassword; }
            set { _callbackPassword = value; }
        }

        public bool TestMode
        {
            get { return _testMode; }
            set { _testMode = value; }
        }
        /*
        public string CallbackURL
        {
            get { return _callbackURL; }
            set { _callbackURL = value; }
        }
        */
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
			return ((_worldPayID != string.Empty) &&
					(_currency != string.Empty) && (_buttonURL != string.Empty));
		}
		#endregion
	}
}
