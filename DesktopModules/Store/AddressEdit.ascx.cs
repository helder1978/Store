/*
'  DotNetNuke -  http://www.dotnetnuke.com
'  Copyright (c) 2002-2005
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
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Modules.Store.Customer;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial  class AddressEdit : StoreControlBase
	{
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
			this.cboCountry.SelectedIndexChanged +=	new EventHandler(cboCountry_SelectedIndexChanged);
		}
		#endregion

		#region Controls =====================================================

		
		#endregion

		#region Private Declarations =========================================

		private CustomerNavigation _nav = null;
		private int _addressID;
		
		#endregion

		#region Event Handlers ===============================================

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
				// Get the navigation settings
				_nav = new CustomerNavigation(Request.QueryString);

				if (!Null.IsNull(dataSource))
				{
					_addressID = (int)dataSource;
					_nav.AddressID = _addressID.ToString();
				}
				else
				{
					_addressID = int.Parse(_nav.AddressID);
				}

				if (!Page.IsPostBack) 
				{
					// Load country & region lists
					loadCountryList();
					loadRegionList();

					// Set delete confirmation
					cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

					// Are we editing or creating new item?
					if (!Null.IsNull(_addressID))
					{
						AddressController controller = new AddressController();
						AddressInfo address = controller.GetAddress(_addressID);

						if (address != null) 
						{							
							cmdDelete.Visible = true;

							txtDescription.Text = address.Description;
							txtName.Text = address.Name;
							txtAddress1.Text = address.Address1;
							txtAddress2.Text = address.Address2;
							txtCity.Text = address.City;
							cboCountry.SelectedValue = address.CountryCode;
							cboRegion.SelectedValue = address.RegionCode;
							txtPostalCode.Text = address.PostalCode;
							txtPhone1.Text = address.Phone1;
							txtPhone2.Text = address.Phone2;
							chkPrimary.Checked = address.PrimaryAddress;
						} 
					}
				}
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			try 
			{
				if (Page.IsValid == true) 
				{
					PortalSecurity security = new PortalSecurity();

					AddressInfo address = new AddressInfo();

					address.AddressID = _addressID;
					address.PortalID = this.PortalId;
					address.UserID = this.UserId;
					address.Description = security.InputFilter(txtDescription.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
					address.Name = security.InputFilter(txtName.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
					address.Address1 = security.InputFilter(txtAddress1.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
					address.Address2 = security.InputFilter(txtAddress2.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
					address.City = security.InputFilter(txtCity.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
					address.CountryCode = cboCountry.SelectedValue;
					address.RegionCode = cboRegion.SelectedValue;
					address.PostalCode = security.InputFilter(txtPostalCode.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
					address.Phone1 = security.InputFilter(txtPhone1.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
					address.Phone2 = security.InputFilter(txtPhone2.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
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
				_nav.AddressID = Null.NullString;
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
				if (!Null.IsNull(_nav.AddressID)) 
				{
					AddressController controller = new AddressController();
					controller.DeleteAddress(int.Parse(_nav.AddressID));

					_nav.AddressID = Null.NullString;
				}

				invokeEditComplete();
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		private void cboCountry_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			loadRegionList();
		}
		#endregion

		#region Private Functions
		private void loadCountryList()
		{
			ListController ctlEntry = new ListController();
			ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Country");

			cboCountry.DataSource = entryCollection;
			cboCountry.DataBind();
		}

		private void loadRegionList()
		{
			string countryCode = cboCountry.SelectedItem.Value;
			string listKey = "Country." + countryCode;
			ListController ctlEntry = new ListController();
			ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Region", "", listKey);

			cboRegion.DataSource = entryCollection;
			cboRegion.DataBind();
		}
		#endregion
	}
}
