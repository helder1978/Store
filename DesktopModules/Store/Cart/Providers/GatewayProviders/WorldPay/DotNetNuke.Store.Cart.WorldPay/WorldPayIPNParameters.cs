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
using System.Collections.Specialized;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for WorldPayIPNParameters.
	/// </summary>
	public class WorldPayIPNParameters : RequestFormWrapper
	{
		#region Constructors

		public WorldPayIPNParameters() : base()
		{
		}

		public WorldPayIPNParameters(NameValueCollection requestForm) : base(requestForm)
		{
			_postString = "cmd=_notify-validate";
			foreach(string paramName in requestForm)
			{
				_postString += String.Format("&{0}={1}", paramName, HTTPPOSTEncode(requestForm[paramName]));
                if (paramName == "transStatus") _payment_status = requestForm[paramName];
                if (paramName == "transId") _txn_id = requestForm[paramName];
                
			}
		}

		#endregion

		#region Declarations

		private string _postString = string.Empty;
		private string _payment_status = string.Empty;
		private string _txn_id = string.Empty;
		private string _receiver_email = string.Empty;
		private string _email = string.Empty;
		private int _custom = -1;
		private int _item_number = -1;
		private decimal _mc_gross = -1;
		private decimal _shipping = -1;
		private decimal _tax = -1;

		#endregion

		#region Public Properties

		public string PostString
		{
			get { return _postString; }
			set { _postString = value; }
		}

		public string payment_status
		{
			get { return _payment_status; }
			set { _payment_status = value; }
		}

		public string txn_id
		{
			get { return _txn_id; }
			set { _txn_id = value; }
		}

		public string receiver_email
		{
			get { return _receiver_email; }
			set { _receiver_email = value; }
		}

		public string email
		{
			get { return _email; }
			set { _email = value; }
		}

		public int custom
		{
			get { return _custom; }
			set { _custom = value; }
		}

		public int item_number
		{
			get { return _item_number; }
			set { _item_number = value; }
		}

		public decimal mc_gross
		{
			get { return _mc_gross; }
			set { _mc_gross = value; }
		}

		public decimal shipping
		{
			get { return _shipping; }
			set { _shipping = value; }
		}

		public decimal tax
		{
			get { return _tax; }
			set { _tax = value; }
		}

		// Same as item_number
		public int CartID
		{
			get { return _item_number; }
		}

		public int ShipToID
		{
			get
			{
				int shipToID = -1;
				if (_custom >= 0)
				{
					shipToID = _custom;
				}
				else
				{
					shipToID = _item_number;
				}
				return shipToID;
			}
		}

		public bool IsValid
		{
			get
			{
				if (_payment_status == "Y")
				{
					return true;
				}
                else
				    return false;
			}
		}

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
