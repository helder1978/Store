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
using System.Xml;
using System.Xml.Serialization;

namespace DotNetNuke.Modules.Store.Providers
{
	/// <summary>
	/// Summary description for ProviderController.
	/// </summary>
	public class ProviderController
	{
		#region Private Members
		private string[] providerPaths = new string[] {"",
									"Providers\\AddressProviders\\",
									"Providers\\CatalogProviders\\",
									"Providers\\PromotionProviders\\",
									"Providers\\ShippingProviders\\",
									"Providers\\TaxProviders\\",
									"Providers\\PaymentProviders\\",
									"Providers\\FulfillmentProviders\\",
									"Providers\\SubscriptionProviders\\"
		};

		private ArrayList providerList;
		#endregion

		#region Constructors
		public ProviderController(StoreProviderType providerType, string modulePath)
		{
			string providerPath;
			string virtualPath;
			string[] folderList;

			providerList = new ArrayList();

			if (!modulePath.EndsWith("\\")) modulePath += "\\";
			providerPath = modulePath + providerPaths[(int)providerType];

			folderList = Directory.GetDirectories(providerPath);

			foreach(string folder in folderList)
			{
				virtualPath = providerPaths[(int)providerType].Replace("\\", "/");
				virtualPath += getTrailingFolder(folder);

				ProviderInfo providerInfo = getProviderInfo(folder);
				providerInfo.Path = folder;
				providerInfo.VirtualPath = virtualPath;
				providerInfo.Type = providerType;

				providerList.Add(providerInfo);
			}
		}
		#endregion

		#region Public Functions
		public ArrayList GetProviders()
		{
			return providerList;
		}

		public ProviderInfo GetProvider(string providerName)
		{
			ProviderInfo providerInfo = null;

			foreach (ProviderInfo info in providerList)
			{
				if (info.Name == providerName)
				{
					providerInfo = info;
					break;
				}
			}

			return providerInfo;
		}
		#endregion

		#region Private Functions
		ProviderInfo getProviderInfo(string providerPath)
		{
			ProviderInfo providerInfo = new ProviderInfo();

			string providerName = getTrailingFolder(providerPath);
			string infoFile = providerPath + "\\" + providerName + "Info.xml";

			if (!providerPath.EndsWith("\\")) providerPath += "\\";

			if (Directory.Exists(providerPath))
			{
				if (File.Exists(infoFile))
				{
					try
					{
						Stream infoDoc = File.Open(infoFile, FileMode.Open, FileAccess.Read, FileShare.Read);
						XmlSerializer serializer = new XmlSerializer(typeof(ProviderInfo));

						providerInfo = (ProviderInfo)serializer.Deserialize(infoDoc);
						
						infoDoc.Close();

						if (providerInfo.Controls == null || providerInfo.Controls.Length < 1)
						{
							throw new ApplicationException("No controls are defined in '" + providerPath + "ProviderInfo.xml'.");
						}
					}
					catch (Exception ex)
					{
						throw new ApplicationException("An error ocurred while reading '" + providerPath + "ProviderInfo.xml'.", ex);
					}
				}
				else
				{
					throw new FileNotFoundException("No ProviderInfo.xml file was found in '" + providerPath + "'.");
				}
			}
			else
			{
				throw new DirectoryNotFoundException("The provider path '" + providerPath + "' does not exist.");
			}

			return providerInfo;
		}

		private string getTrailingFolder(string path)
		{
			string folder = "";

			while (path.EndsWith("\\") && path.Length > 0)
			{
				path = path.Remove(path.Length - 1, 1);
			}

			if (path.Length > 0)
			{
				int s = path.LastIndexOf("\\");

				if (s >= 0)
				{
					folder = path.Substring(s + 1);
				}
				else
				{
					folder = path;
				}
			}

			return folder;
		}
		#endregion
	}
}
