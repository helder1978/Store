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
using System.Xml.Serialization;

namespace DotNetNuke.Modules.Store.Providers
{
	/// <summary>
	/// Summary description for GatewayInfo.
	/// </summary>
	[XmlRoot("provider")]
	public class ProviderInfo
	{
		#region Private Declarations
		protected string _Name = string.Empty;
		protected string _Description = string.Empty;
		protected string _Class = string.Empty;
		protected string _Assembly = string.Empty;
		protected string _Path = string.Empty;
		protected string _VirtualPath = string.Empty;
		protected StoreProviderType _Type;
		#endregion

		#region Public Properties
		[XmlElement("name")]
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		[XmlElement("description")]
		public string Description
		{
			get { return _Description; }
			set { _Description = value; }
		}

		[XmlElement("class")]
		public string Class
		{
			get { return _Class; }
			set { _Class = value; }
		}

		[XmlElement("assembly")]
		public string Assembly
		{
			get { return _Assembly; }
			set { _Assembly = value; }
		}

		[XmlArray("controls"), XmlArrayItem("control", typeof(ProviderControlInfo))]
		public ProviderControlInfo[] Controls;

		public string Path
		{
			get { return _Path; }
			set { _Path = value; }		
		}

		public string VirtualPath
		{
			get { return _VirtualPath; }
			set { _VirtualPath = value; }		
		}

		public StoreProviderType Type
		{
			get {return _Type;}
			set {_Type = value;}
		}
		#endregion
	}

	[XmlRoot("control")]
	public class ProviderControlInfo
	{
		#region Private Declarations
		protected string _Name = string.Empty;
		protected string _Value = string.Empty;
		#endregion

		#region Public Properties
		[XmlAttribute("name")]
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		[XmlAttribute("value")]
		public string Value
		{
			get { return _Value; }
			set { _Value = value; }
		}
		#endregion
	}
}
