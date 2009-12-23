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

using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
	/// Summary description for AddresssController.
	/// </summary>
	public class AddressController : ProviderControllerBase, IAddressProvider
	{
		#region Constructors
		public AddressController()
		{
		}
		#endregion

		#region IAddressProvider Members

		public int AddAddress(IAddressInfo addressInfo)
		{
			return DataProvider.Instance().AddAddress(addressInfo.PortalID, addressInfo.UserID, addressInfo.Description, addressInfo.Name, addressInfo.Address1, addressInfo.Address2, addressInfo.City, addressInfo.RegionCode, addressInfo.CountryCode, addressInfo.PostalCode, addressInfo.Phone1, addressInfo.Phone2, addressInfo.PrimaryAddress, addressInfo.CreatedByUser);
		}

		public void UpdateAddress(IAddressInfo addressInfo)
		{
			DataProvider.Instance().UpdateAddress(addressInfo.AddressID, addressInfo.Description, addressInfo.Name, addressInfo.Address1, addressInfo.Address2, addressInfo.City, addressInfo.RegionCode, addressInfo.CountryCode, addressInfo.PostalCode, addressInfo.Phone1, addressInfo.Phone2, addressInfo.PrimaryAddress);
		}

		public ArrayList GetAddresses(int portalID, int userID)
		{
			ArrayList addresses = CBO.FillCollection(DataProvider.Instance().GetAddresses(portalID, userID), typeof(AddressInfo));

			UserController controller = new UserController();
            UserInfo userInfo = controller.GetUser(portalID, userID);

            AddressInfo addressInfo = new AddressInfo();

			addressInfo.AddressID = 0;
			addressInfo.Name = "Registration";
			addressInfo.Description = "Registration Address";
			addressInfo.Address1 = userInfo.Profile.Street;
            //addressInfo.Address2 = "";
            addressInfo.Address2 = userInfo.Profile.Unit;
			addressInfo.City = userInfo.Profile.City;
			addressInfo.RegionCode = userInfo.Profile.Region;
			addressInfo.CountryCode = userInfo.Profile.Country;
			addressInfo.PostalCode = userInfo.Profile.PostalCode;
			addressInfo.Phone1 = userInfo.Profile.Telephone;
			addressInfo.Phone2 = userInfo.Profile.Fax;

			addresses.Insert(0, addressInfo);

			return addresses;
		}

		public IAddressInfo GetAddress(int addressID)
		{
            return (CBO.FillObject(DataProvider.Instance().GetAddress(addressID), typeof(AddressInfo)) as AddressInfo);
		}

		public IAddressInfo GetAddress(int portalID, int userID)
		{
			UserController controller = new UserController();
			UserInfo userInfo = controller.GetUser(portalID, userID);

			AddressInfo addressInfo = new AddressInfo();

			addressInfo.AddressID = 0;
            addressInfo.Name = userInfo.DisplayName;
			addressInfo.Description = "Registration Address";
			addressInfo.Address1 = userInfo.Profile.Street;
			addressInfo.Address2 = userInfo.Profile.Unit;
			addressInfo.City = userInfo.Profile.City;
			addressInfo.RegionCode = userInfo.Profile.Region;
			addressInfo.CountryCode = userInfo.Profile.Country;
			addressInfo.PostalCode = userInfo.Profile.PostalCode;
			addressInfo.Phone1 = userInfo.Profile.Telephone;
			addressInfo.Phone2 = userInfo.Profile.Fax;

			return addressInfo;
		}

		public void DeleteAddresses(int portalID, int userID)
		{
			DataProvider.Instance().DeleteAddresses(portalID, userID);
		}

		public void DeleteAddress(int addressID)
		{
			DataProvider.Instance().DeleteAddress(addressID);
		}

		public ProviderControlBase GetProfileControl(PortalModuleBase parentControl, string modulePath)
		{
			ProviderControlBase profileControl = loadControl(parentControl, modulePath, "Profile");
			return profileControl;
		}

		/// <summary>
		/// Builds the proper checkout control according the modulePath specified.
		/// <strong>NOTE: The control must have suffix of "Checkout"</strong>
		/// </summary>
		/// <param name="parentControl">Parent for the address checkout control.</param>
		/// <param name="modulePath">Path to the address controllers for this Address Provider.</param>
		/// <returns>The address checkout control for configured address provider</returns>
		public ProviderControlBase GetCheckoutControl(PortalModuleBase parentControl, string modulePath) 
		{
			ProviderControlBase checkoutControl = loadControl(parentControl, modulePath, "Checkout");
			return checkoutControl;
		}
		#endregion
	}
}
