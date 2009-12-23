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
using System.Collections.Specialized;
using System.Reflection;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.Store.Components
{
	/// <summary>
	/// Summary description for RequestFormWrapper.
	/// </summary>
	public abstract class RequestFormWrapper
	{
		#region Constructors

		public RequestFormWrapper()
		{
		}

		public RequestFormWrapper(NameValueCollection requestForm)
		{
			LoadRequestForm(requestForm);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Parses the Request Form parameters and sets properties, if they exist,
		/// in the derived object.
		/// </summary>
		/// <param name="requestForm"></param>
		public void LoadRequestForm(NameValueCollection requestForm)
		{
			// Iterate thru all properties for this type
			PropertyInfo[] propertyList = this.GetType().GetProperties();
			foreach(PropertyInfo property in propertyList)
			{
				// Do we have a value for this property?
                string val = requestForm[property.Name];
				if (val != null)
				{
					object objValue = null;

					try
					{
						// Cast to the appropriate type
						switch(property.PropertyType.Name)
						{
							case "String":
								objValue = (object)val;
								break;
							case "Int32":
								objValue = (object)Convert.ToInt32(val);
								break;
							case "Boolean":
								objValue = (object)Convert.ToBoolean(val);
								break;
							case "Decimal":
								objValue = (object)Convert.ToDecimal(val);
								break;
						}
					}
					catch
					{
						//Cast failed - Skip this property
					}

					// Set the value
					if (objValue != null)
					{
						property.SetValue(this, objValue, null);
					}
				}
			}
		}

		#endregion
	}
}
