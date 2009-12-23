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
using System.Reflection;
using DotNetNuke.Modules.Store.Admin;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for GatewayController.
	/// </summary>
	public class GatewayController
	{
		#region Constructors
		public GatewayController(string modulePath)
		{
			gatewayList = new ArrayList();

			string gatewayPath = modulePath + providerPath;
			string[] folderList = Directory.GetDirectories(gatewayPath);

			foreach(string folder in folderList)
			{
				GatewayInfo gateway = new GatewayInfo(GetGatewayName(folder), folder);

				// Lookup payment and admin controls
				string[] adminControls = Directory.GetFiles(folder, "*Admin.ascx");
				if (adminControls.Length > 0)
				{
					gateway.AdminControl = Path.GetFileName(adminControls[0]);
				}

				string[] paymentControls = Directory.GetFiles(folder, "*Payment.ascx");
				if (paymentControls.Length > 0)
				{
					gateway.PaymentControl = Path.GetFileName(paymentControls[0]);
				}

				gatewayList.Add(gateway);
			}
		}
		#endregion

		#region Private Declarations
		private const string providerPath = "Providers\\GatewayProviders\\";
		private ArrayList gatewayList = null;
		#endregion

		#region Public Methods
		public ArrayList GetGateways()
		{
			return gatewayList;
		}

		public GatewayInfo GetGateway(string gatewayName)
		{
			GatewayInfo gateway = null;
			foreach(GatewayInfo info in gatewayList)
			{
				if (string.Compare(info.GatewayName, gatewayName, true) == 0)
				{
					gateway = info;
					break;
				}
			}
			return gateway;
		}

		public string GetGatewayName(string gatewayPath)
		{
			string gatewayName = gatewayPath;
			gatewayName = gatewayName.TrimEnd(new char[] {'\\','/',' '});
			gatewayName = gatewayName.Substring(gatewayName.LastIndexOf("\\") + 1);
			return gatewayName;
		}
		#endregion
	}
}
