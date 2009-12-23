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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using DotNetNuke.Modules.Store.Providers;

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	/// <summary>
	/// Summary description for CoreProfile.
	/// </summary>
	public partial class DefaultAddressProfile : ProviderControlBase
	{
		#region Controls

		protected TextBox txtDescription;
		protected TextBox txtName;
		protected TextBox txtAddress1;
		protected TextBox txtAddress2;
		protected TextBox txtCity;
		protected TextBox txtPostalCode;
		protected TextBox txtPhone1;
		protected TextBox txtPhone2;
		protected CountryListBox cboCountry;
		protected DropDownList cboRegion;

		protected DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider.StoreAddress addressEdit;
		
		#endregion

		#region Private Declarations

		private AddressNavigation addressNav = null;
		private int addressId;
		
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
			this.grdAddresses.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdAddresses_ItemDataBound);

		}
		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
				// Get the navigation settings
				addressNav = new AddressNavigation(Request.QueryString);

				if (addressNav.AddressID.Length > 0)
				{
					addressId = int.Parse(addressNav.AddressID);
				}
				else
				{
					addressId = -1;
				}

				if (!Page.IsPostBack) 
				{
					AddressController controller = new AddressController();
					ArrayList addresses = controller.GetAddresses(this.PortalId, this.UserId);

					if (addresses.Count > 0)
					{
						grdAddresses.DataSource = addresses;
						grdAddresses.DataBind();
					}


					// Set delete confirmation
					cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

					// Edit Address
					if (addressId > 0)
					{
						plhGrid.Visible = false;
						plhEditAddress.Visible = true;

						lblEditTitle.Text = Localization.GetString("EditAddress", this.LocalResourceFile);

						AddressInfo address = (AddressInfo)controller.GetAddress(addressId);

						if (address != null) 

						{	
							//BUG: This does not work because the page_load event is called prior to the controls load event.
							this.addressEdit.AddressInfo = address;
							cmdDelete.Visible = true;
							chkPrimary.Checked = address.PrimaryAddress;
						} 
					}
					// Add Address
					else if (addressId == 0)
					{
						plhGrid.Visible = false;
						plhEditAddress.Visible = true;

						lblEditTitle.Text = Localization.GetString("AddAddress", this.LocalResourceFile);
					}
					// No Action
					else
					{
						plhGrid.Visible = true;
						plhEditAddress.Visible = false;

						lblEditTitle.Text = Localization.GetString("Addresses", this.LocalResourceFile);
					}
				}
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		private void grdAddresses_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			IAddressInfo addressInfo = (IAddressInfo)e.Item.DataItem;

			Image imgPrimary = (Image)e.Item.FindControl("imgPrimary");
			if (imgPrimary != null)
			{
				imgPrimary.ImageUrl = "~/images/ratingplus.gif";

				if (addressInfo.PrimaryAddress)
				{
					imgPrimary.Visible = true;
				}
				else
				{
					imgPrimary.Visible = false;
				}
			}

			HyperLink lnkEdit = (HyperLink)e.Item.FindControl("lnkEdit");
			if (lnkEdit != null)
			{
				if (addressInfo.AddressID == 0)
				{
					//This is the registration address, so the profile editor should be used to modify this address
					//http://localhost/DotNetNuke/Catalog/StoreAccount/tabid/55/ctl/Register/Default.aspx
					lnkEdit.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId,"Register");
				}
				else 
				{
					addressNav.AddressID = addressInfo.AddressID.ToString();
					lnkEdit.NavigateUrl = addressNav.GetNavigationUrl();
				}
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			addressNav.AddressID = "0";
			Response.Redirect(addressNav.GetNavigationUrl(), false);
		}

		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			try 
			{
				if (Page.IsValid == true) 
				{
					PortalSecurity security = new PortalSecurity();

					IAddressInfo address = addressEdit.AddressInfo;

					address.AddressID = addressId;
					address.PortalID = this.PortalId;
					address.UserID = this.UserId;
					address.PrimaryAddress = chkPrimary.Checked;
					address.CreatedByUser = this.UserId.ToString();
					address.CreatedDate	= DateTime.Now;

					AddressController controller = new AddressController();

					if (Null.IsNull(address.AddressID) || address.AddressID == 0)
					{
						controller.AddAddress(address);
					} 
					else 
					{
						controller.UpdateAddress(address);
					}

					invokeEditComplete();
				}
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			try 
			{
				addressNav.AddressID = Null.NullString;
				invokeEditComplete();
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		protected void cmdDelete_Click(object sender, EventArgs e)
		{
			try 
			{
				if (addressId > 0) 
				{
					AddressController controller = new AddressController();
					controller.DeleteAddress(addressId);

					addressNav.AddressID = Null.NullString;
				}

				invokeEditComplete();
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion
	}
}
