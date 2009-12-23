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
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for GatewaySettings.
	/// </summary>
	public class GatewaySettings
	{
		#region Contstructors
		public GatewaySettings()
		{
		}

		public GatewaySettings(string xml)
		{
			FromString(xml);
		}
		#endregion

		#region Protected Declarations

        #endregion

		#region Public Properties

		#endregion

		#region Public Methods
		public override string ToString()
		{
			string xml = "<" + this.GetType().Name + ">";

			// Iterate thru all "public" properties for this type
			PropertyInfo[] propertyList = this.GetType().GetProperties();
			foreach(PropertyInfo property in propertyList)
			{
				// Add "node" for this property
				xml += "<" + property.Name + ">";
				object objValue = property.GetValue(this, null);
				if (objValue != null)
				{
                    switch (property.PropertyType.Name)
                    {
                        case "Decimal":
                            decimal Value = (decimal)objValue;
                            xml += Value.ToString("0.00", CultureInfo.InvariantCulture.NumberFormat);
                            break;
                        default:
                            xml += objValue.ToString();
                            break;
                    }
                    //xml += objValue.ToString();
				}
				xml += "</" + property.Name + ">";
			}

			xml += "</" + this.GetType().Name + ">";

			return xml;
		}

		public virtual void FromString(string xml)
		{
			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(xml);

				// Iterate thru all "public" properties for this type
				PropertyInfo[] propertyList = this.GetType().GetProperties();
				foreach(PropertyInfo property in propertyList)
				{
					if (xmlDoc.DocumentElement[property.Name] != null)
					{
						string xmlValue = xmlDoc.DocumentElement[property.Name].InnerText;
						object objValue = null;

						// Cast to the appropriate type
						switch(property.PropertyType.Name)
						{
							case "String":
								objValue = (object)xmlValue;
								break;
							case "Int32":
								objValue = (object)Convert.ToInt32(xmlValue);
								break;
                            case "Decimal":
                                objValue = (object)Convert.ToDecimal(xmlValue, CultureInfo.InvariantCulture.NumberFormat);
                                break;
							case "Boolean":
								objValue = (object)Convert.ToBoolean(xmlValue);
								break;
							case "CaptureTypes":
								objValue = GetCustomType(xmlValue);
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
			catch
			{
				// FAILURE - stop loading from string
			}
		}

		public virtual object GetCustomType(string stringValue)
		{
			return null;
		}

		public virtual bool IsValid()
		{
			return false;
		}
		#endregion
	}
}
