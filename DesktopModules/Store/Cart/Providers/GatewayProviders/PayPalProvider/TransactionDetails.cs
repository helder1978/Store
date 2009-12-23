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
	/// Summary description for TransactionDetails.
	/// </summary>
	public class TransactionDetails : GatewaySettings
	{
		#region Constructors
		public TransactionDetails()
		{
		}

		public TransactionDetails(string xml)
		{
			FromString(xml);
		}
		#endregion

		#region Private Declarations
		private string _returnURL = string.Empty;
		private string _cancelURL = string.Empty;
		private string _notifyURL = string.Empty;
		#endregion

		#region Public Properties
		public string ReturnURL
		{
			get { return _returnURL; }
			set { _returnURL = value; }
		}

		public string CancelURL
		{
			get { return _cancelURL; }
			set { _cancelURL = value; }
		}

		public string NotifyURL
		{
			get { return _notifyURL; }
			set { _notifyURL = value; }
		}
		#endregion

		#region GatewaySettings Overrides
		public override bool IsValid()
		{
			return (_returnURL != string.Empty);
		}
		#endregion
	}
}
