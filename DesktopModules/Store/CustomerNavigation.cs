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

using System.Collections.Specialized;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CustomerNavigation.
	/// </summary>
	public class CustomerNavigation : NavigateWrapper
	{
		#region Constructors

		public CustomerNavigation() : base()
		{
		}

		public CustomerNavigation(NameValueCollection queryString) : base(queryString)
		{
		}

		#endregion

		#region Declarations

		private string _pageID = Null.NullString;
		private string _addressID = Null.NullString;
		private int _customerID = Null.NullInteger;
		private int _orderID = Null.NullInteger;
        private string _payPalExit = Null.NullString;

		#endregion

		#region Public Properties

		public string PageID
		{
			get { return _pageID; }
			set { _pageID = value; }
		}

		public string AddressID
		{
			get { return _addressID; }
			set { _addressID = value; }
		}

		public int CustomerID
		{
			get { return _customerID; }
			set { _customerID = value; }
		}

		public int OrderID
		{
			get { return _orderID; }
			set { _orderID = value; }
		}

        public string PayPalExit
        {
            get { return _payPalExit; }
            set { _payPalExit = value; }
        }

		#endregion
	}
}
