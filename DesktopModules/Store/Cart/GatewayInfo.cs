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
using DotNetNuke.Modules.Store.Admin;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for GatewayInfo.
	/// </summary>
	public class GatewayInfo
	{
		#region Constructors
		public GatewayInfo()
		{
		}

		public GatewayInfo(string gatewayName, string gatewayPath)
		{
			_gatewayName = gatewayName;
			_gatewayPath = gatewayPath;
		}
		#endregion

		#region Private Declarations
		protected string _gatewayName = string.Empty;
		protected string _gatewayPath = string.Empty;
		protected string _adminControl = "Admin.ascx";
		protected string _paymentControl = "Payment.ascx";
		#endregion

		#region Public Properties
		public string GatewayName
		{
			get { return _gatewayName; }
			set { _gatewayName = value; }
		}

		public string GatewayPath
		{
			get { return _gatewayPath; }
			set { _gatewayPath = value; }
		}

		public string AdminControl
		{
			get { return _adminControl; }
			set { _adminControl = value; }
		}

		public string PaymentControl
		{
			get { return _paymentControl; }
			set { _paymentControl = value; }		
		}
		#endregion

		#region Public Methods
		public string GetSettings(int portalID)
		{
			string gatewaySettings = string.Empty; 

			StoreController controller = new StoreController();
			StoreInfo storeInfo = controller.GetStoreInfo(portalID);
			if (storeInfo != null)
			{
				gatewaySettings = storeInfo.GatewaySettings;
			}

			return gatewaySettings;
		}

		public void SetSettings(int portalID, string gatewaySettings)
		{
			StoreController controller = new StoreController();
			StoreInfo storeInfo = controller.GetStoreInfo(portalID);
			if (storeInfo != null)
			{
				storeInfo.GatewaySettings = gatewaySettings;
				controller.UpdateStoreInfo(storeInfo);
			}
		}
		#endregion
	}
}
