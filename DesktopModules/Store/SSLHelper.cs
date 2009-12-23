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
using System.Web;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Provides static methods for ensuring that a page is rendered 
	/// securely via SSL or unsecurely.
	/// </summary>
	public sealed class SSLHelper
	{
		// Protocol prefixes.
		private const string UnsecureProtocolPrefix = "http://";
		private const string SecureProtocolPrefix = "https://";

		/// <summary>
		/// Prevent creating an instance of this class.
		/// </summary>
		private SSLHelper() 
		{
		}

		/// <summary>
		/// Determines the secure page that should be requested if a redirect occurs.
		/// </summary>
		/// <param name="ignoreCurrentProtocol">
		/// A flag indicating whether or not to ingore the current protocol when determining.
		/// </param>
		/// <returns>A string containing the absolute URL of the secure page to redirect to.</returns>
		public static string DetermineSecurePage(bool ignoreCurrentProtocol) 
		{
			string Result = null;

			// Is this request already secure?
			string RequestPath = HttpContext.Current.Request.Url.AbsoluteUri;
			if (ignoreCurrentProtocol || RequestPath.StartsWith(UnsecureProtocolPrefix)) 
			{
				// Replace the protocol of the requested URL with "https".
				Result = RequestPath.Replace(UnsecureProtocolPrefix, SecureProtocolPrefix);
			}

			return Result;
		}

		/// <summary>
		/// Requests the current page over a secure connection, if it is not already.
		/// </summary>
		public static void RequestSecurePage() 
		{
			// Determine the response path, if any.
			string ResponsePath = DetermineSecurePage(false);
			if (ResponsePath != null && ResponsePath != string.Empty)
			{
				// Redirect to the secure page.
				HttpContext.Current.Response.Redirect(ResponsePath, true);
			}
		}
	}
}
