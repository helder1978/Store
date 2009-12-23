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

namespace DotNetNuke.Modules.Store.Admin
{
	/// <summary>
	/// Summary description for StoreInfo.
	/// </summary>
	public class StoreInfo
	{
		#region Private Members
		private int portalID;
		private string name;
		private string description;
		private string keywords;
		private string gatewayName;
		private string gatewaySettings;
		private string defaultEmailAddress;
		private int shoppingCartPageID;
		private string createdByUser;
		private DateTime createdDate;
        private int storePageID;
        private string currencySymbol;
        private bool portalTemplates;
        private bool authorizeCancel;
		#endregion

		#region Public Properties
		public int PortalID
		{
			get {return portalID;}
			set {portalID = value;}
		}

		public string Name
		{
			get {return name;}
			set {name = value;}
		}

		public string Description
		{
			get {return description;}
			set {description = value;}
		}

		public string Keywords
		{
			get {return keywords;}
			set {keywords = value;}
		}

		public string GatewayName
		{
			get {return gatewayName;}
			set {gatewayName = value;}
		}

		public string GatewaySettings
		{
			get {return gatewaySettings;}
			set {gatewaySettings = value;}
		}

		public string DefaultEmailAddress
		{
			get {return defaultEmailAddress;}
			set {defaultEmailAddress = value;}
		}

		public int ShoppingCartPageID
		{
			get {return shoppingCartPageID;}
			set {shoppingCartPageID = value;}
		}

		public string CreatedByUser
		{
			get {return createdByUser;}
			set {createdByUser = value;}
		}

		public DateTime CreatedDate
		{
			get {return createdDate;}
			set {createdDate = value;}
		}

        public int StorePageID
        {
            get { return storePageID; }
            set { storePageID = value; }
        }

        public string CurrencySymbol
        {
            get { return currencySymbol; }
            set { currencySymbol = value; }
        }

        public bool PortalTemplates
        {
            get { return portalTemplates; }
            set { portalTemplates = value; }
        }

        public bool AuthorizeCancel
        {
            get { return authorizeCancel; }
            set { authorizeCancel = value; }
        }

		#endregion

		#region Constructors
		public StoreInfo()
		{
		}
		#endregion

		#region Object Overrides
		public override bool Equals(object obj) 
		{
			if ((obj == null) || (this.GetType() != obj.GetType())) 
			{
				return false;
			}

			StoreInfo objInfo = (StoreInfo) obj;
			return portalID.Equals(objInfo.PortalID);
		}

		public override int GetHashCode() 
		{
			return portalID.GetHashCode();
		}
		#endregion
	}
}
