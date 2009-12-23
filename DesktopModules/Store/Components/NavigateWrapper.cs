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
	/// Summary description for NavigateWrapper.
	/// </summary>
	public abstract class NavigateWrapper
	{
		#region Constructors

		public NavigateWrapper()
		{
		}

		public NavigateWrapper(NameValueCollection queryString)
		{
			LoadQueryString(queryString);
		}

		#endregion

		#region Declarations

		private int _tabId = Null.NullInteger;
		private string _controlKey = Null.NullString;

		#endregion

		#region Public Properties

		public int TabId
		{
			get { return _tabId; }
			set { _tabId = value; }
		}

		public string ControlKey
		{
			get { return _controlKey; }
			set { _controlKey = value; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Parses the QueryString parameters and sets properties, if they exist,
		/// in the derived object.
		/// </summary>
		/// <param name="queryString"></param>
		public void LoadQueryString(NameValueCollection queryString)
		{
			// Iterate thru all properties for this type
			PropertyInfo[] propertyList = this.GetType().GetProperties();
			foreach(PropertyInfo property in propertyList)
			{
				// Do we have a value for this property?
                string val = queryString[property.Name];
				if (val != null)
				{
					object objValue = null;

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
					}

					// Set the value
					if (objValue != null)
					{
						property.SetValue(this, objValue, null);
					}
				}
			}
		}

		/// <summary>
		/// Gets an array of query string parameters to be used with the 
		/// DotNetNuke NavigateUrl(), EditUrl() etc.
		/// </summary>
		/// <returns></returns>
		public string[] GetNavigateParameters()
		{
			return GetNavigateParameters(new StringDictionary());
		}

		/// <summary>
		/// Gets an array of query string parameters to be used with the 
		/// DotNetNuke NavigateUrl(), EditUrl() etc.
		/// </summary>
		/// <param name="replaceParams">List of parameters to use as replacements for existing parameters.</param>
		/// <returns></returns>
		public string[] GetNavigateParameters(StringDictionary replaceParams)
		{
			ArrayList settingList = new ArrayList();

			// Iterate thru all properties for this type
			PropertyInfo[] propertyList = this.GetType().GetProperties();
			foreach(PropertyInfo property in propertyList)
			{
				if (replaceParams.ContainsKey(property.Name))
				{
					if (replaceParams[property.Name] != Null.NullString)
					{
						// Add name and value pair for NavigateUrl() parameters
						settingList.Add(property.Name + "=" + replaceParams[property.Name]);
					}
					else
					{
						// SKIP if property set to Null.NullString
					}
				}
				else
				{
					// Get property's value
					object objValue = property.GetValue(this, null);
					if ((objValue != null) && (!Null.IsNull(objValue)) && 
						(property.Name != "TabId") && (property.Name != "ControlKey"))
					{
						// Add name and value pair for NavigateUrl() parameters
						settingList.Add(property.Name + "=" + objValue.ToString());
					}
				}
			}

			// Cast to string array and return
			return (settingList.ToArray(typeof(string)) as string[]);			
		}

		/// <summary>
		/// Gets the url for navigation including the appropriate parameters.
		/// </summary>
		/// <returns></returns>
		public string GetNavigationUrl()
		{
			return Globals.NavigateURL(_tabId, _controlKey, GetNavigateParameters());
		}

		/// <summary>
		/// Gets the url for navigation including the appropriate parameters.
		/// </summary>
		/// <param name="replaceParams">List of parameters to use as replacements for existing parameters.</param>
		/// <returns></returns>
		public string GetNavigationUrl(StringDictionary replaceParams)
		{
			return Globals.NavigateURL(_tabId, _controlKey, GetNavigateParameters(replaceParams));
		}

        /// <summary>
        /// Gets the url for navigation including the appropriate parameters.
        /// </summary>
        /// <param name="replaceParams">List of parameters to use as replacements for existing parameters.</param>
        /// <returns></returns>
        public string GetNavigationUrl(StringDictionary replaceParams, int TabID)
        {
            return Globals.NavigateURL(TabID, _controlKey, GetNavigateParameters(replaceParams));
        }

		#endregion
	}
}
