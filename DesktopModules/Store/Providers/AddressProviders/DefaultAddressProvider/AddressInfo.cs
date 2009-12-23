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

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
	/// Summary description for AddressInfo.
	/// </summary>
	public class AddressInfo : IAddressInfo
	{
		#region Private Members
		private int addressID;
		private int portalID;
		private int userID;
		private string description;
		private string name;
		private string address1;
		private string address2;
		private string city;
		private string regionCode;
		private string countryCode;
		private string postalCode;
		private string phone1;
		private string phone2;
		private bool primaryAddress;
		private string createdByUser;
		private DateTime createdDate;
		#endregion

		#region Constructors
		public AddressInfo()
		{
		}
		#endregion

		#region Public Properties
		public int AddressID
		{
			get {return addressID;}
			set {addressID = value;}
		}

		public int PortalID
		{
			get {return portalID;}
			set {portalID = value;}
		}

		public int UserID
		{
			get {return userID;}
			set {userID = value;}
		}

		public string Description
		{
			get {return description;}
			set {description = value;}
		}

		public string Name
		{
			get {return name;}
			set {name = value;}
		}

		public string Address1
		{
			get {return address1;}
			set {address1 = value;}
		}

		public string Address2
		{
			get {return address2;}
			set {address2 = value;}
		}

		public string City
		{
			get {return city;}
			set {city = value;}
		}

		public string RegionCode
		{
			get {return regionCode;}
			set {regionCode = value;}
		}

		public string CountryCode
		{
			get {return countryCode;}
			set {countryCode = value;}
		}

		public string PostalCode
		{
			get {return postalCode;}
			set {postalCode = value;}
		}

		public string Phone1
		{
			get {return phone1;}
			set {phone1 = value;}
		}

		public string Phone2
		{
			get {return phone2;}
			set {phone2 = value;}
		}

		public bool PrimaryAddress
		{
			get {return primaryAddress;}
			set {primaryAddress = value;}
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
		#endregion
	}
}
