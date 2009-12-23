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
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for AuthNetGatewayProvider.
	/// </summary>
	public class AuthNetGatewayProvider
	{
		#region Constructors
		public AuthNetGatewayProvider(string gatewaySettings)
		{
			_gatewaySettings = gatewaySettings;
		}
		#endregion

		#region Private Declarations
		private string _gatewaySettings = string.Empty;
		#endregion

		#region Public Methods
		public TransactionResult ProcessTransaction(IAddressInfo shipping, IAddressInfo billing, 
			OrderInfo orderInfo, object transDetails)
		{
			TransactionResult result = new TransactionResult();

			// Check data before performing transaction
			AuthNetSettings settings = new AuthNetSettings(_gatewaySettings);
			if ((settings == null) || (!settings.IsValid()))
			{
				result.Succeeded = false;
                result.Message = "ErrorPaymentOption";
				return result;
			}

			if (billing == null)
			{
				result.Succeeded = false;
                result.Message = "ErrorBillingAddress";
				return result;
			}

			TransactionDetails trans = new TransactionDetails(transDetails as string);
			if ((trans == null) || (!trans.IsValid()))
			{
				result.Succeeded = false;
                result.Message = "ErrorCardInformation";
				return result;
			}

			// Gather transaction information
			string url = settings.GatewayURL;
			string firstName = string.Empty;
			string lastName = trans.NameOnCard;
			if (lastName.IndexOf(" ") >= 0)
			{
				firstName = lastName.Substring(0, lastName.IndexOf(" ")).Trim();
				lastName = lastName.Substring(lastName.LastIndexOf(" ")).Trim();
			}

			string address = billing.Address1 + " " + billing.Address2;
			address = address.Trim();

			NameValueCollection NVCol = new NameValueCollection();

			//NVCol.Add("x_version",				settings.Version);
			NVCol.Add("x_delim_data",			"True");
            NVCol.Add("x_relay_response",       "False");
			NVCol.Add("x_login",				settings.Username);
			NVCol.Add("x_tran_key",				settings.Password);
			NVCol.Add("x_test_request",			settings.IsTest.ToString());
			NVCol.Add("x_delim_char",			"~");
			NVCol.Add("x_encap_char",			"'");

			NVCol.Add("x_first_name",			firstName);
			NVCol.Add("x_last_name",			lastName);
			NVCol.Add("x_company",				"");
			NVCol.Add("x_address",				address);
			NVCol.Add("x_city",					billing.City);
			NVCol.Add("x_state",				billing.RegionCode);
			NVCol.Add("x_zip",					billing.PostalCode);
			NVCol.Add("x_country",				billing.CountryCode);
			NVCol.Add("x_phone",				billing.Phone1);
			NVCol.Add("x_invoice_num",			orderInfo.OrderID.ToString());

			NVCol.Add("x_amount",				orderInfo.OrderTotal.ToString());
			NVCol.Add("x_method",				"CC");
			NVCol.Add("x_card_num",				trans.CardNumber);
			NVCol.Add("x_card_code",			trans.VerificationCode.ToString());
			NVCol.Add("x_exp_date",				trans.ExpirationMonth.ToString() + "/" + trans.ExpirationYear.ToString());
			NVCol.Add("x_recurring_billing",	"NO");
			NVCol.Add("x_type",					settings.Capture.ToString());

			// Perform transaction
			try
			{
				Encoding enc = Encoding.GetEncoding(1252);
                StreamReader loResponseStream = new StreamReader(PostEx(url, NVCol).GetResponseStream(), enc);
                
				string lcHtml = loResponseStream.ReadToEnd();
				loResponseStream.Close();
				
				string[] resultArray = Microsoft.VisualBasic.Strings.Split(lcHtml.TrimStart("'".ToCharArray()), "'~'", -1, Microsoft.VisualBasic.CompareMethod.Binary);

				//TODO: What transaction details to return???
				result.Succeeded = (resultArray[0] == "1");
				result.Message = resultArray[3];
			}
			catch (Exception ex)
			{
				//Return error
				string[] resultArray = Microsoft.VisualBasic.Strings.Split("2|0|0|No Connection Available", "|", -1, Microsoft.VisualBasic.CompareMethod.Binary);

				//TODO: What transaction details to return???
				result.Succeeded = false;
                result.Message = ex.Message; 
                //result.Message = resultArray[3];
			}

			return result;
		}
		#endregion

		#region Private Methods
		public WebResponse PostEx(string url, NameValueCollection values)
		{
			WebRequest request = null;
			StringBuilder builder = null;
			string[] keys = null;
			Stream stream = null;
			byte[] bytes = null;
			try
			{
				request = WebRequest.Create(url);
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";

				if (values.Count == 0)
				{
					request.ContentLength = 0;
				}
				else
				{
					builder = new StringBuilder();
					keys = values.AllKeys;
					foreach (string key in keys)
					{
						if (builder.Length > 0)
						{
							builder.Append("&");
						}
						builder.Append(HttpUtility.UrlEncode(key));
						builder.Append("=");
						builder.Append(HttpUtility.UrlEncode(values[key]));
					}
					bytes = Encoding.UTF8.GetBytes(builder.ToString());
					request.ContentLength = bytes.Length;
					stream = request.GetRequestStream();
					stream.Write(bytes, 0, bytes.Length);
					stream.Close();
				}
				return request.GetResponse();
			}
			catch (Exception ex)
			{
                throw new Exception(ex.Message);
			}
		}
		#endregion
	}
}
