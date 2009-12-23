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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.UI.WebControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Admin;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
	/// Summary description for CoreProfile.
	/// </summary>
	public partial class DefaultAddressCheckout : AddressCheckoutControlBase
	{
		#region Controls

		protected DefaultAddressProvider.StoreAddress addressBilling;
		protected DefaultAddressProvider.StoreAddress addressShipping;

		protected LabelControl lblBillAddress;
		protected LabelControl lblNone;
		protected LabelControl lblShipAddress;
		protected LabelControl lblUseBillingAddress;
		protected LabelControl lblUseShippingAddress;

		protected System.Web.UI.HtmlControls.HtmlTableRow Tr1;

		protected Label lblBillingMessage; 
		protected LinkButton lnkSaveAddress;
		protected LinkButton lnkCancelEditAddress;

		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion
		
		#region Private Declarations

		//private AddressNavigation addressNav = null;
		private bool _ShippingAddressSelectionEnabled = true;
		private bool _ShippingAddressSelectionVisible = true;
		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                //Load the billing address
                AddressController controller = new AddressController();
                ArrayList addresses = controller.GetAddresses(PortalId, UserId);

                lstBillAddress.DataSource = addresses;
                lstBillAddress.DataTextField = "Description";
                lstBillAddress.DataValueField = "AddressID";
                lstBillAddress.DataBind();
                lstBillAddress.Items.Insert(0, new ListItem(Localization.GetString("SelectBillingAddress", this.LocalResourceFile), "-1"));
                //lstBillAddress.ClearSelection();
                //lstBillAddress.Items[1].Selected = true;
                //lstBillAddress_SelectedIndexChanged(lstBillAddress, new EventArgs());

                lstShipAddress.DataSource = addresses;
                lstShipAddress.DataTextField = "Description";
                lstShipAddress.DataValueField = "AddressID";
                lstShipAddress.DataBind();
                lstShipAddress.Items.Insert(0, new ListItem(Localization.GetString("SelectShippingAddress", this.LocalResourceFile), "-1"));
                //populateShipAddress(int.Parse(lstBillAddress.SelectedValue));

                ShippingAddressSelectionEnabled = false;
            }
		}
        
		protected void lnkAddNewAddress_Click(object sender, System.EventArgs e)
		{
			HttpContext.Current.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID,"", new string[] {"PageID=CustomerProfile"}), true);
		}

		protected void lstBillAddress_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			populateBillAddress(int.Parse(lstBillAddress.SelectedValue));
			this.SendBillingAddressChangedEvent();
		}

		protected void lstShipAddress_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			populateShipAddress(int.Parse(lstShipAddress.SelectedValue));
			this.SendShippingAddressChangedEvent();
		}

		protected void radNone_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( radNone.Checked )
			{
				ShippingAddressSelectionVisible = false;
				ShippingAddressSelectionEnabled = false;
				populateShipAddress(-1);
				this.SendShippingAddressChangedEvent();
			}
		}

		protected void radBilling_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radBilling.Checked)
			{
				ShippingAddressSelectionVisible = true;
				ShippingAddressSelectionEnabled = false;
				copyBillingToShipping();
				this.SendShippingAddressChangedEvent();
                addressShipping = new StoreAddress();
			}
		}

		protected void radShipping_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radShipping.Checked)
			{
				ShippingAddressSelectionVisible = true;
				ShippingAddressSelectionEnabled = true;
				//populateShipAddress(int.Parse(lstShipAddress.SelectedValue));
				this.SendShippingAddressChangedEvent();
			}		
		}
		#endregion

		#region Private Functions

		/// <summary>
		/// Retrieve an IAddressInfo from the address controller using 
		/// the address id.
		/// </summary>
		/// <param name="addressId">Address ID to be loaded.</param>
		/// <returns>A populated IAddressID if found the address is was found; otherwise null</returns>
		private IAddressInfo loadAddress(int addressId)
		{
			AddressController controller = new AddressController();
			if (addressId > 0)
			{
				return controller.GetAddress(addressId);
			}
			else
			{
				return controller.GetAddress(PortalId, UserId);
			}
		}

		public void populateBillAddress(int addressId)
		{
			IAddressInfo address = null;

			if (addressId >= 0)
			{
				address = loadAddress(addressId);
			}	

			if (address != null)
			{
				addressBilling.AddressInfo = address;
			} 
			else 
			{
				addressBilling.AddressInfo = new AddressInfo();
			}

			this.addressBilling.PopulateAddress();
		}

		public void populateShipAddress(int addressId)
		{
			IAddressInfo address = null;

            if (this.radBilling.Checked)
            {
                addressShipping.AddressInfo = addressBilling.AddressInfo;
                this.addressShipping.PopulateAddress();
            }
            else
            {
                if (addressId >= 0)
                {
                    address = loadAddress(addressId);
                }

                if (address != null)
                {
                    addressShipping.AddressInfo = address;
                }
                else
                {
                    addressShipping.AddressInfo = new AddressInfo();
                }
            }

            this.addressShipping.PopulateAddress();
		}

		private void copyBillingToShipping() 
		{
			IAddressInfo copyOfBilling = new AddressInfo();

			copyOfBilling.Description = addressBilling.AddressInfo.Description;
			copyOfBilling.Name = addressBilling.AddressInfo.Name;
			copyOfBilling.Address1 = addressBilling.AddressInfo.Address1;
			copyOfBilling.Address2 = addressBilling.AddressInfo.Address2;
			copyOfBilling.City = addressBilling.AddressInfo.City;
			copyOfBilling.CountryCode = addressBilling.AddressInfo.CountryCode;
			copyOfBilling.RegionCode = addressBilling.AddressInfo.RegionCode;
			copyOfBilling.PostalCode = addressBilling.AddressInfo.PostalCode;
			copyOfBilling.Phone1 = addressBilling.AddressInfo.Phone1;
			copyOfBilling.Phone2 = addressBilling.AddressInfo.Phone2;
			copyOfBilling.AddressID = addressBilling.AddressInfo.AddressID;

			addressShipping.AddressInfo = copyOfBilling;

			addressShipping.PopulateAddress();
		}

		private bool ShippingAddressSelectionEnabled
		{
			get 
			{
				return _ShippingAddressSelectionEnabled;
			}
			set 
			{
				_ShippingAddressSelectionEnabled = value;
				lblShipAddress.Visible = _ShippingAddressSelectionEnabled;
				lstShipAddress.Visible = _ShippingAddressSelectionEnabled;
				addressShipping.Enabled = _ShippingAddressSelectionEnabled;
			}
		}
		
		private bool ShippingAddressSelectionVisible
		{
			get 
			{
				return _ShippingAddressSelectionVisible;
			}
			set 
			{
				_ShippingAddressSelectionVisible = value;
				lblShipAddress.Visible = _ShippingAddressSelectionVisible;
				lstShipAddress.Visible = _ShippingAddressSelectionVisible;
				addressShipping.Visible = _ShippingAddressSelectionVisible;
			}
		}
		#endregion

		#region public Functions/Properties

		public override IAddressInfo ShippingAddress
		{
			get{ return addressShipping.AddressInfo; }
			set{ addressShipping.AddressInfo = value; }
		}

		public override IAddressInfo BillingAddress
		{
			get{ return addressBilling.AddressInfo; }
			set{ addressBilling.AddressInfo = value; }
		}
		#endregion
	}
}
