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

namespace DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using DotNetNuke.Common.Lists;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Entities.Modules;
	using DotNetNuke.Framework;
	using DotNetNuke.Modules.Store.Providers;
	using DotNetNuke.Modules.Store.Providers.Address;
	using DotNetNuke.Services.Exceptions;
    using DotNetNuke.UI.UserControls;
    using DotNetNuke.UI.WebControls;
	/// <summary>
	///		Summary description for StoreAddress.
	/// </summary>
	public partial class StoreAddress : DotNetNuke.Framework.UserControlBase
	{
		protected LabelControl plDescription;
		protected LabelControl plToName;

		private int _ModuleId; 
		private string _LabelColumnWidth = ""; 
		private string _ControlColumnWidth = ""; 
		private int _StartTabIndex = 1; 

		private bool _Enabled = true;

		private IAddressInfo _AddressInfo = null;

		private bool _ShowStreet = true; 
		private bool _ShowUnit = true; 
		private bool _ShowCity = true; 
		private bool _ShowCountry = true; 
		private bool _ShowRegion = true; 
		private bool _ShowPostal = true; 
		private bool _ShowTelephone = true; 
		private bool _ShowCell = false; 
		private bool _ShowDescription = false;

		private string _CountryData = "Text"; 
		private string _RegionData = "Text";

		private string MyFileName = "StoreAddress.ascx"; 
		
		public string LocalResourceFile
		{
			get { return Services.Localization.Localization.GetResourceFile(this, MyFileName); }
		}
		
		public bool Enabled
		{
			get { return _Enabled; }
			set 
			{
				_Enabled = value;
				foreach (System.Web.UI.WebControls.WebControl control in this.GetInputControls())
				{
					control.Enabled = _Enabled;
				}
			}
		}
		
		public IAddressInfo AddressInfo
		{
			get 
			{
                if (_AddressInfo == null)
                {
                    _AddressInfo = new AddressInfo();
                    /*
                    Response.Write("<br>Filling _AddressInfo fields");
                    _AddressInfo.AddressID = int.Parse(this.hiddenAddressId.Value.Length > 0 ? this.hiddenAddressId.Value : "-1");
                    UserInfo userInfo = Entities.Users.UserController.GetCurrentUserInfo();
                    _AddressInfo.Name = userInfo.FullName;
                    //_AddressInfo.Address1 = userInfo.Profile.Street;
                    //_AddressInfo.Address2 = this.txtUnit.Text;
                    _AddressInfo.City = userInfo.Profile.City;
                    _AddressInfo.CountryCode = userInfo.Profile.Country;
                    _AddressInfo.RegionCode = userInfo.Profile.Region;
                    _AddressInfo.PostalCode = userInfo.Profile.PostalCode;
                    _AddressInfo.Phone1 = userInfo.Profile.Telephone;
                    //_AddressInfo.Phone2 = this.txtCell.Text;
                    //_AddressInfo.Description = this.txtDescription.Text;

                    //Response.Write(userInfo.Email);
                    */
                }
                //Response.Write("<br>Getting _AddressInfo fields from the txt");
                //-1 indicates that this is a new address and not one selected from the drop down list.
                _AddressInfo.AddressID = int.Parse(this.hiddenAddressId.Value.Length > 0 ? this.hiddenAddressId.Value : "-1");
                _AddressInfo.Name = this.txtToName.Text;
                _AddressInfo.Address1 = this.txtStreet.Text;
                _AddressInfo.Address2 = this.txtUnit.Text;
                _AddressInfo.City = this.txtCity.Text;
                _AddressInfo.CountryCode = GetSelectedCountry();
                _AddressInfo.RegionCode = GetSelectedRegion();
                _AddressInfo.PostalCode = this.txtPostal.Text;
                _AddressInfo.Phone1 = this.txtTelephone.Text;
                _AddressInfo.Phone2 = this.txtCell.Text;
                _AddressInfo.Description = this.txtDescription.Text;
            
				return _AddressInfo; 
			}
			set 
			{ 
				_AddressInfo = value; 
			}
		}
		public int ModuleId 
		{ 
			get 
			{ 
				return Convert.ToInt32(ViewState["ModuleId"]); 
			} 
			set 
			{ 
				_ModuleId = value; 
			} 
		} 

		public string LabelColumnWidth 
		{ 
			get 
			{ 
				return Convert.ToString(ViewState["LabelColumnWidth"]); 
			} 
			set 
			{ 
				_LabelColumnWidth = value; 
			} 
		} 

		public string ControlColumnWidth 
		{ 
			get 
			{ 
				return Convert.ToString(ViewState["ControlColumnWidth"]); 
			} 
			set 
			{ 
				_ControlColumnWidth = value; 
			} 
		} 

		public int StartTabIndex 
		{ 
			set 
			{ 
				_StartTabIndex = value; 
			} 
		} 

		private ArrayList GetInputControls() 
		{
			ArrayList inputControls = new ArrayList();
			inputControls.Add(this.txtDescription);
			inputControls.Add(this.txtToName);
			inputControls.Add(this.txtStreet);
			inputControls.Add(this.txtUnit);
			inputControls.Add(this.txtCity);
			inputControls.Add(this.cboRegion);
			inputControls.Add(this.txtRegion);
			inputControls.Add(this.txtPostal);
            inputControls.Add(this.cboCountry);
			inputControls.Add(this.txtTelephone);
			inputControls.Add(this.txtCell);

            inputControls.Add(this.valDescription);
            inputControls.Add(this.valToName);
            inputControls.Add(this.valStreet);
            inputControls.Add(this.valCity);
            if (this.cboRegion.Visible)
            {
                inputControls.Add(this.valRegion1);
            }
            if (this.txtRegion.Visible)
            {
                inputControls.Add(this.valRegion2);
            }
            inputControls.Add(this.valPostal);
            inputControls.Add(this.valCountry);
            inputControls.Add(this.valTelephone);
            inputControls.Add(this.valCell);

            return inputControls;
		}

		private string GetSelectedCountry() 
		{
            string retvalue = ""; 
            if (!(cboCountry.SelectedItem == null)) 
            { 
                if (_CountryData.ToLower() == "text") 
                { 
                    if (cboCountry.SelectedIndex == 0) 
                    { 
                        retvalue = ""; 
                    } 
                    else 
                    { 
                        retvalue = cboCountry.SelectedItem.Text; 
                    } 
                } 
                else if (_CountryData.ToLower() == "value") 
                { 
                    retvalue = cboCountry.SelectedItem.Value; 
                } 
            } 
            if (cboCountry.SelectedItem != null)
            {
                return cboCountry.SelectedItem.Text;
            }

            return retvalue; 
		}


		private string GetSelectedRegion()
		{ 
			string retvalue = ""; 
			if (cboRegion.Visible) 
			{ 
				if (!(cboRegion.SelectedItem == null)) 
				{ 
					if (_RegionData.ToLower() == "text") 
					{ 
						if (cboRegion.SelectedIndex > 0) 
						{ 
							retvalue = cboRegion.SelectedItem.Text; 
						} 
					} 
					else if (_RegionData.ToLower() == "value") 
					{ 
						retvalue = cboRegion.SelectedItem.Value; 
					} 
				} 
			} 
			else 
			{ 
				retvalue = txtRegion.Text; 
			}

            if (!(cboRegion.SelectedItem == null))
            {
                return cboRegion.SelectedItem.Text;
            }

			return retvalue; 
		} 


		public bool ShowStreet 
		{ 
			set 
			{ 
				_ShowStreet = value; 
			} 
		} 

		public bool ShowUnit 
		{ 
			set 
			{ 
				_ShowUnit = value; 
			} 
		} 

		public bool ShowCity 
		{ 
			set 
			{ 
				_ShowCity = value; 
			} 
		} 

		public bool ShowCountry 
		{ 
			set 
			{ 
				_ShowCountry = value; 
			} 
		} 

		public bool ShowRegion 
		{ 
			set 
			{ 
				_ShowRegion = value; 
			} 
		} 

		public bool ShowPostal 
		{ 
			set 
			{ 
				_ShowPostal = value; 
			} 
		} 

		public bool ShowTelephone 
		{ 
			set 
			{ 
				_ShowTelephone = value; 
			} 
		} 

		public bool ShowCell 
		{ 
			set 
			{ 
				_ShowCell = value; 
			} 
		} 
		public bool ShowDescription
		{ 
			set 
			{ 
				_ShowDescription = value; 
			} 
		} 

		public string CountryData 
		{ 
			set 
			{ 
				_CountryData = value; 
			} 
		} 

		public string RegionData 
		{ 
			set 
			{ 
				_RegionData = value; 
			} 
		} 
		
		public void PopulateAddress()
		{
            //Response.Write("<br>Start PopulateAddress");
			if (_AddressInfo != null) 
			{
                //Response.Write("<br>_AddressInfo != null");
                UserInfo userInfo = Entities.Users.UserController.GetCurrentUserInfo();

                _CountryData = "text";
                _AddressInfo.CountryCode = userInfo.Profile.Country;

                if (_CountryData.ToLower() == "text") 
				{ 
					if (_AddressInfo.CountryCode == "") 
					{ 
						cboCountry.SelectedIndex = 0; 
					} 
					else 
					{ 
						if (!(cboCountry.Items.FindByText(_AddressInfo.CountryCode) == null)) 
						{ 
							cboCountry.ClearSelection(); 
							cboCountry.Items.FindByText(_AddressInfo.CountryCode).Selected = true; 
						} 
					} 
				} 
				else if (_CountryData.ToLower() == "value") 
				{ 
					if (!(cboCountry.Items.FindByValue(_AddressInfo.CountryCode) == null)) 
					{ 
						cboCountry.ClearSelection(); 
						cboCountry.Items.FindByValue(_AddressInfo.CountryCode).Selected = true; 
					} 
				} 

				//Localize(); 

				if (cboRegion.Visible) 
				{
                    if (_RegionData.ToLower() == "text")
                    {
                        if (_AddressInfo.RegionCode == "")
                        {
                            cboRegion.ClearSelection();
                            cboRegion.SelectedIndex = 0;
                        }
                        else
                        {
                            if (!(cboRegion.Items.FindByText(_AddressInfo.RegionCode) == null))
                            {
                                cboRegion.ClearSelection();
                                cboRegion.Items.FindByText(_AddressInfo.RegionCode).Selected = true;
                            }
                        }
                    }
                    else if (_RegionData.ToLower() == "value")
                    {
                        if (!(cboRegion.Items.FindByValue(_AddressInfo.RegionCode) == null))
                        {
                            cboRegion.ClearSelection();
                            cboRegion.Items.FindByValue(_AddressInfo.RegionCode).Selected = true;
                        }
                    } 
				} 
				else 
				{ 
					//txtRegion.Text = _AddressInfo.RegionCode; 
                    txtRegion.Text = userInfo.Profile.Region;
				}

                

                //Response.Write("<br>PopulateAddress filling fields {" + txtCity.Text + "} {" + userInfo.Profile.City + "}" );
                /*
				txtStreet.Text = _AddressInfo.Address1;
				txtUnit.Text = _AddressInfo.Address2;
				txtCity.Text = _AddressInfo.City;
				txtPostal.Text = _AddressInfo.PostalCode;
				txtTelephone.Text = _AddressInfo.Phone1;
				txtCell.Text = _AddressInfo.Phone2;
				txtToName.Text = _AddressInfo.Name;
				txtDescription.Text = _AddressInfo.Description;
				hiddenAddressId.Value = Convert.ToString(_AddressInfo.AddressID);
                */

                txtStreet.Text = userInfo.Profile.Street;
                txtUnit.Text = userInfo.Profile.Unit;
                txtCity.Text = userInfo.Profile.City;
                txtPostal.Text = userInfo.Profile.PostalCode;
                txtTelephone.Text = userInfo.Profile.Telephone;
                txtCell.Text = userInfo.Profile.Cell;
                txtToName.Text = userInfo.Profile.FullName;
                txtDescription.Text = _AddressInfo.Description;
                hiddenAddressId.Value = Convert.ToString(_AddressInfo.AddressID);

            }
			else 
			{
                Response.Write("<br>else, _AddressInfo == null");

				//Localize();
			}
		}
			   

		private void Localize() 
		{ 
			string countryCode = cboCountry.SelectedItem.Value; 

			ListController ctlEntry = new ListController(); 
			string listKey = "Country." + countryCode; 
			ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Region", "", listKey); 

			if (entryCollection.Count != 0) 
			{ 
				cboRegion.Items.Clear(); 
				cboRegion.DataSource = entryCollection; 
				cboRegion.DataBind(); 
				cboRegion.Items.Insert(0, new ListItem("<" + Services.Localization.Localization.GetString("Not_Specified", Services.Localization.Localization.SharedResourceFile) + ">", "")); 
				
				if (countryCode.ToLower() == "us") 
				{ 
					//valRegion1.ErrorMessage = Services.Localization.Localization.GetString("StateRequired", Services.Localization.Localization.GetResourceFile(this, MyFileName)); 
					//plRegion.Text = Services.Localization.Localization.GetString("plState", LocalResourceFile); 
					//plRegion.HelpText = Services.Localization.Localization.GetString("plState.Help", LocalResourceFile); 
					//plPostal.Text = Services.Localization.Localization.GetString("plZip", LocalResourceFile); 
					//plPostal.HelpText = Services.Localization.Localization.GetString("plZip.Help", LocalResourceFile); 
				} 
				else 
				{ 
					//valRegion1.ErrorMessage = Services.Localization.Localization.GetString("ProvinceRequired", LocalResourceFile); 
					//plRegion.Text = Services.Localization.Localization.GetString("plProvince", LocalResourceFile); 
					//plRegion.HelpText = Services.Localization.Localization.GetString("plProvince.Help", LocalResourceFile); 
					//plPostal.Text = Services.Localization.Localization.GetString("plPostal", LocalResourceFile); 
					//plPostal.HelpText = Services.Localization.Localization.GetString("plPostal.Help", LocalResourceFile); 
				} 

				cboRegion.Visible = true; 
				txtRegion.Visible = false;
				valRegion1.Enabled = true; 
				valRegion2.Enabled = false; 
			} 
			else 
			{ 
				cboRegion.ClearSelection(); 
				cboRegion.Visible = false; 
				txtRegion.Visible = true; 
				valRegion1.Enabled = false; 
				valRegion2.Enabled = true; 
				//valRegion2.ErrorMessage = Services.Localization.Localization.GetString("RegionRequired", LocalResourceFile); 
				//plRegion.Text = Services.Localization.Localization.GetString("plRegion", LocalResourceFile); 
				//plRegion.HelpText = Services.Localization.Localization.GetString("plRegion.Help", LocalResourceFile); 
				//plPostal.Text = Services.Localization.Localization.GetString("plPostal", LocalResourceFile); 
				//plPostal.HelpText = Services.Localization.Localization.GetString("plPostal.Help", LocalResourceFile); 
			} 
		} 


		private void loadCountryList() 
		{
			ListController ctlEntry = new ListController(); 
			ListEntryInfoCollection entryCollection = ctlEntry.GetListEntryInfoCollection("Country"); 
			cboCountry.DataSource = entryCollection; 
			cboCountry.DataBind();
            cboCountry_SelectedIndexChanged(cboCountry, null);
			//cboCountry.Items.Insert(0, new ListItem("<" + Services.Localization.Localization.GetString("Not_Specified", Services.Localization.Localization.SharedResourceFile) + ">", "")); 
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
				if (!(Page.IsPostBack)) 
				{ 
                    valToName.ErrorMessage = Services.Localization.Localization.GetString("ToNameRequired", LocalResourceFile);
                    valStreet.ErrorMessage = Services.Localization.Localization.GetString("StreetRequired", LocalResourceFile);
                    valCity.ErrorMessage = Services.Localization.Localization.GetString("CityRequired", LocalResourceFile);
                    valRegion1.ErrorMessage = Services.Localization.Localization.GetString("RegionRequired", LocalResourceFile);
                    valRegion2.ErrorMessage = Services.Localization.Localization.GetString("RegionRequired", LocalResourceFile);
                    valCountry.ErrorMessage = Services.Localization.Localization.GetString("CountryRequired", LocalResourceFile);
                    valPostal.ErrorMessage = Services.Localization.Localization.GetString("PostalRequired", LocalResourceFile);
                    valTelephone.ErrorMessage = Services.Localization.Localization.GetString("TelephoneRequired", LocalResourceFile);
                    valCell.ErrorMessage = Services.Localization.Localization.GetString("CellRequired", LocalResourceFile); 

					if (_LabelColumnWidth != "") 
					{ 
					} 
					ArrayList inputControls = GetInputControls();
					if (_ControlColumnWidth != "") 
					{ 
						foreach (System.Web.UI.WebControls.WebControl control in inputControls)
						{
							control.Width = System.Web.UI.WebControls.Unit.Parse(_ControlColumnWidth);
						}
					} 

					short tabIndex = (short)_StartTabIndex;
					foreach (System.Web.UI.WebControls.WebControl control in inputControls)
					{
						control.TabIndex = tabIndex++;
					}					
				
			
					loadCountryList();

					PopulateAddress();  // canadean changed: was commented...

					rowDescription.Visible = _ShowDescription;
					rowStreet.Visible = _ShowStreet; 
					rowUnit.Visible = _ShowUnit; 
					rowCity.Visible = _ShowCity; 
					rowCountry.Visible = _ShowCountry; 
					rowRegion.Visible = _ShowRegion; 
					rowPostal.Visible = _ShowPostal; 
					rowTelephone.Visible = _ShowTelephone; 
					rowCell.Visible = _ShowCell; 
					

					ViewState["ModuleId"] = Convert.ToString(_ModuleId); 
					ViewState["LabelColumnWidth"] = _LabelColumnWidth; 
					ViewState["ControlColumnWidth"] = _ControlColumnWidth; 

//					ShowRequiredFields(); 
				} 

                //Hide validators if controls are disabled...
                if (txtDescription.Enabled)
                    valDescription.Visible = true;
                else
                    valDescription.Visible = false;

                if (txtToName.Enabled)
                    valToName.Visible = true;
                else
                    valToName.Visible = false;

                if (txtStreet.Enabled)
                    valStreet.Visible = true;
                else
                    valStreet.Visible = false;

                if (txtCity.Enabled)
                    valCity.Visible = true;
                else
                    valCity.Visible = false;

                if (cboCountry.Enabled)
                    valCountry.Visible = true;
                else
                    valCountry.Visible = false;

                if (cboRegion.Enabled)
                    valRegion1.Visible = true;
                else
                    valRegion1.Visible = false;

                if (txtRegion.Enabled)
                    valRegion2.Visible = true;
                else
                    valRegion2.Visible = false;

                if (txtPostal.Enabled)
                    valPostal.Visible = true;
                else
                    valPostal.Visible = false;

                if (txtTelephone.Enabled)
                    valTelephone.Visible = true;
                else
                    valTelephone.Visible = false;

                if (txtCell.Enabled)
                    valCell.Visible = true;
                else
                    valCell.Visible = false;
			} 
			catch (Exception exc) 
			{ 
				Exceptions.ProcessModuleLoadException(this, exc); 
			} 
		}

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

		protected void cboCountry_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try 
			{ 
				Localize(); 
			} 
			catch (Exception exc) 
			{ 
				Exceptions.ProcessModuleLoadException(this, exc); 
			} 
		}




	}
}
