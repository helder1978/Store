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
		private CardTypes _cardType;
		private string _cardNumber = string.Empty;
		private string _nameOnCard = string.Empty;
		private int _expirationMonth = -1;
		private int _expirationYear = -1;
		private int _verificationCode = -1;
		#endregion

		#region Public Properties
		public enum CardTypes
		{
			Visa,
			MasterCard
		}

		public CardTypes CardType
		{
			get { return _cardType; }
			set { _cardType = value; }
		}

		public string CardNumber
		{
			get { return _cardNumber; }
			set { _cardNumber = value; }
		}

		public string NameOnCard
		{
			get { return _nameOnCard; }
			set { _nameOnCard = value; }
		}

		public int ExpirationMonth
		{
			get { return _expirationMonth; }
			set { _expirationMonth = value; }
		}

		public int ExpirationYear
		{
			get { return _expirationYear; }
			set { _expirationYear = value; }
		}

		public int VerificationCode
		{
			get { return _verificationCode; }
			set { _verificationCode = value; }
		}
		#endregion

		#region GatewaySettings Overrides
		public override object GetCustomType(string stringValue)
		{
			return (object)Enum.Parse(typeof(CardTypes), stringValue);
		}

		public override bool IsValid()
		{
			return (_cardNumber != string.Empty);
		}
		#endregion
	}
}
