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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.Catalog
{
	public delegate Control ProcessTokenDelegate(string tokenName);
	/// <summary>
	/// Summary description for TemplateController.
	/// </summary>
	public class TemplateController
	{
		private static string TemplateFolder = "Templates\\";

		public TemplateController()
		{
		}

		#region Public Functions
        public static ArrayList GetTemplates(string templatesPath)
		{
			ArrayList templateList = new ArrayList();

            string templateFolder = (templatesPath + TemplateFolder);

			if (Directory.Exists(templateFolder))
			{
				string[] fileList = Directory.GetFiles(templateFolder, "*.htm");

				foreach(string file in fileList)
				{
					FileInfo fileInfo = new FileInfo(file);
					StreamReader reader = new StreamReader(fileInfo.FullName);

					TemplateInfo templateInfo = new TemplateInfo();
					templateInfo.Name = fileInfo.Name;
					templateInfo.Path = fileInfo.FullName;
					templateInfo.Content = reader.ReadToEnd();

					templateList.Add(templateInfo);
					reader.Close();
				}
			}

			return templateList;
		}

		public static TemplateInfo GetTemplate(string templatesPath, string templateName)
		{
			TemplateInfo templateInfo = null;
            string templateFolder = (templatesPath + TemplateFolder);
			
			try
			{
				if (Directory.Exists(templateFolder))
				{
					string file = templateFolder + templateName;

					if (File.Exists(file))
					{
						FileInfo fileInfo = new FileInfo(file);
						StreamReader reader = new StreamReader(fileInfo.FullName);

						templateInfo = new TemplateInfo();
						templateInfo.Name = fileInfo.Name;
						templateInfo.Path = fileInfo.FullName;
						templateInfo.Content = reader.ReadToEnd();

						reader.Close();
					}
				}
			}
			catch
			{
				//Ignore any errors
			}

			return templateInfo;
		}

        public static Control ParseTemplate(string templatesPath, string templateName, ProcessTokenDelegate processTokenDelegate)
		{
            TemplateInfo templateInfo = GetTemplate(templatesPath, templateName);
			Control templateControl = new Control();
			Control tokenControl;

			if (templateInfo != null)
			{
				string[] tokens = templateInfo.Content.Split(new char[]{'[',']'});

				foreach (string token in tokens)
				{
					char splitter = (char)254;
					string[] tokenParts = token.Replace("::", splitter.ToString()).Split(new char[]{splitter});

					if (tokenParts[0].Length > 0)
					{
						tokenControl = processTokenDelegate(tokenParts[0]);

						if (tokenControl != null)
						{
							if (tokenParts.Length > 1)
							{
								for (int i=1; i<tokenParts.Length; i++)
								{
									tokenControl = setProperty(tokenControl, tokenParts[i]);
								}
							}

							templateControl.Controls.Add(tokenControl);
						}
					}
				}
			}
			else
			{
                string _Message = Localization.GetString("TemplateError.Text", "~/DesktopModules/Store/App_LocalResources/Catalog.ascx");
                tokenControl = new LiteralControl(string.Format(_Message, templatesPath + templateName));
                templateControl.Controls.Add(tokenControl);
			}

			return templateControl;
		}
		#endregion

		#region Private Functions
		private static Control setProperty(Control control, string keyValuePair)
		{
			string key = keyValuePair.Split(new char[]{'='})[0];
			string value = keyValuePair.Split(new char[]{'='})[1];

			// Iterate thru all properties for this control
			PropertyInfo[] propertyList = control.GetType().GetProperties();

			foreach(PropertyInfo property in propertyList)
			{
				// Do we have a value for this property?
				if (property.Name.ToLower() == key.ToLower())
				{
					object objValue = null;

					switch(property.PropertyType.Name)
					{
						case "String":
							objValue = (object)value;
							break;

						case "Int32":
							objValue = (object)Convert.ToInt32(value);
							break;

						case "Boolean":
							objValue = (object)Convert.ToBoolean(value);
							break;

						case "Unit":
							UnitConverter converter = new UnitConverter();
							objValue = converter.ConvertFromString(value);
							break;
					}

					if (objValue != null)
					{
						property.SetValue(control, objValue, null);
					}
				}
			}

			return control;
		}
		#endregion
	}
}
