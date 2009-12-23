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
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Customer;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.Providers.Address;
using DotNetNuke.Modules.Store.Providers.Shipping;
using DotNetNuke.Modules.Store.Providers.Tax;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Store.
	/// </summary>
	public partial  class StoreAdmin : StoreControlBase
	{
		private const string gatewayProviderPath = "/Providers/GatewayProviders/";

		private StoreInfo storeInfo;
		private StoreControlBase gatewayControl;

		#region Controls
		protected DotNetNuke.UI.UserControls.LabelControl lblParentTitle;
		protected DropDownList lstAddressProviders;
		protected DotNetNuke.UI.UserControls.LabelControl lblStoreName;
		protected DotNetNuke.UI.UserControls.LabelControl lblDescription;
		protected DotNetNuke.UI.UserControls.LabelControl lblKeywords;
		protected DotNetNuke.UI.UserControls.LabelControl lblEmail;
        protected DotNetNuke.UI.UserControls.LabelControl lblCurrencySymbol;
        protected DotNetNuke.UI.UserControls.LabelControl lblUsePortalTemplates;
        protected DotNetNuke.UI.UserControls.LabelControl lblShoppingCartPageID;
        protected DotNetNuke.UI.UserControls.LabelControl lblAuthorizeCancel;
		protected DotNetNuke.UI.UserControls.LabelControl lblGateway;
		protected DotNetNuke.UI.UserControls.LabelControl lblAddressProvider;
		protected PlaceHolder plhAddressProvider;
        protected DotNetNuke.UI.UserControls.SectionHeadControl dshShippingProvider;
        protected DotNetNuke.UI.UserControls.SectionHeadControl dshTaxProvider;
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

		#region Events
		protected void Page_Load(object sender, System.EventArgs e)
		{
			StoreNavigation nav = new StoreNavigation(Request.QueryString);
			string cartPageID = string.Empty;
            string storePageID = string.Empty;

			// Load store details
			StoreController storeController = new StoreController();
			storeInfo = storeController.GetStoreInfo(PortalId);

			if (storeInfo != null)// && !Page.IsPostBack)
			{
				txtStoreName.Text = storeInfo.Name;
				txtDescription.Text = storeInfo.Description;
				txtKeywords.Text = storeInfo.Keywords;
				txtEmail.Text = storeInfo.DefaultEmailAddress;
                txtCurrencySymbol.Text = storeInfo.CurrencySymbol;
                chkUsePortalTemplates.Checked = storeInfo.PortalTemplates;
                cartPageID = storeInfo.ShoppingCartPageID.ToString();
                storePageID = storeInfo.StorePageID.ToString();
                chkAuthorizeCancel.Checked = storeInfo.AuthorizeCancel;
				if (nav.GatewayName == Null.NullString)
				{
					nav.GatewayName = storeInfo.GatewayName;
				}
			}

			if (!Page.IsPostBack || lstShoppingCartPageID.Items.Count == 0)
			{
				// Load pages available to host the shopping cart
				loadTabs(cartPageID);

                //Load tabs available to host the store
                loadStoreTabs(storePageID);

				// Load available gateways
				loadGateways(nav.GatewayName);
			}

		    // Load tax provider
		    loadTaxProvider();

		    // Load shipping provider
		    loadShippingProvider();

		    // Load the current gateway control
		    if (lstGateway.Items.Count > 0)
		    {
			    if (nav.GatewayName == Null.NullString)
			    {
				    nav.GatewayName = lstGateway.SelectedItem.Text;
			    }

			    loadGatewayAdmin(nav.GatewayName);
		    }
		}

		protected override void OnPreRender(EventArgs e)
		{
			// Set the title in the parent control
			Store storeAdmin = (Store)parentControl;
			storeAdmin.ParentTitle = lblParentTitle.Text;

			base.OnPreRender (e);
		}


		protected void btnSave_Click(object sender, EventArgs e)
		{
			StoreController storeController = new StoreController();
			bool newStore = false;

			if (storeInfo == null)
			{
				storeInfo = new StoreInfo();
				newStore = true;
			}

			storeInfo.PortalID = PortalId;
			storeInfo.Name = txtStoreName.Text;
			storeInfo.Description = txtDescription.Text;
			storeInfo.Keywords = txtKeywords.Text;
			storeInfo.DefaultEmailAddress = txtEmail.Text;
            storeInfo.CurrencySymbol = txtCurrencySymbol.Text;
            storeInfo.PortalTemplates = chkUsePortalTemplates.Checked;
            storeInfo.GatewayName = lstGateway.SelectedItem.Text;
			storeInfo.ShoppingCartPageID = int.Parse(lstShoppingCartPageID.SelectedValue);
            storeInfo.StorePageID = int.Parse(lstStorePageID.SelectedValue);
            storeInfo.AuthorizeCancel = chkAuthorizeCancel.Checked;

            if ((gatewayControl != null) && (gatewayControl.DataSource != null))
            {
                storeInfo.GatewaySettings = gatewayControl.DataSource.ToString();
            }

            if (chkUsePortalTemplates.Checked)
            {
                string hostFolder = MapPath(ModulePath);
                string portalFolder = PortalSettings.HomeDirectoryMapPath + "Store\\";
                string[] fileList = null;

                // Templates
                if (!Directory.Exists(portalFolder + "Templates"))
                {
                    Directory.CreateDirectory(portalFolder + "Templates");

                    fileList = Directory.GetFiles(hostFolder + "Templates", "*.*");

                    foreach (string file in fileList)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        fileInfo.CopyTo(portalFolder + "Templates\\" + fileInfo.Name, false);
                    }
                }

                // Images
                if (!Directory.Exists(portalFolder + "Templates\\Images"))
                {
                    Directory.CreateDirectory(portalFolder + "Templates\\Images");

                    fileList = Directory.GetFiles(hostFolder + "Templates\\Images", "*.*");

                    foreach (string file in fileList)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        fileInfo.CopyTo(portalFolder + "Templates\\Images\\" + fileInfo.Name, false);
                    }
                }

                // StyleSheet
                if (!Directory.Exists(portalFolder + "Templates\\StyleSheet"))
                {
                    Directory.CreateDirectory(portalFolder + "Templates\\StyleSheet");

                    fileList = Directory.GetFiles(hostFolder + "Templates\\StyleSheet", "*.*");

                    foreach (string file in fileList)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        fileInfo.CopyTo(portalFolder + "Templates\\StyleSheet\\" + fileInfo.Name, false);
                    }
                }

            }

			if (newStore)
			{
				storeInfo.CreatedByUser = UserInfo.Username;
				storeController.AddStoreInfo(storeInfo);
			}
			else
			{
				storeController.UpdateStoreInfo(storeInfo);
			}
		}

		protected void lstGateway_SelectedIndexChanged(object sender, EventArgs e)
		{
            //StoreNavigation nav = new StoreNavigation();
            //nav.GatewayName = lstGateway.SelectedItem.Text;
            //Response.Redirect(nav.GetNavigationUrl(), false);
            if (lstGateway.SelectedItem.Text == String.Empty)
            {
                plhGateway.Controls.Clear();
            }
            else
            {
                loadGatewayAdmin(lstGateway.SelectedItem.Text);
            }
		}

        //private void lstAddressProviders_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    StoreNavigation nav = new StoreNavigation();
        //    nav.AddressProvider = lstAddressProviders.SelectedItem.Text;
        //    Response.Redirect(nav.GetNavigationUrl(), false);
        //}
		#endregion

		#region Private Functions
		private void loadTabs(string cartPageID)
		{
			TabController tabController = new TabController();
			ArrayList tabs = tabController.GetTabs(PortalId);

			foreach (TabInfo tabInfo in tabs)
			{
				if (tabInfo.IsVisible && !tabInfo.IsDeleted && !tabInfo.IsAdminTab && !tabInfo.IsSuperTab)
				{
					lstShoppingCartPageID.Items.Add(new ListItem(tabInfo.TabName, tabInfo.TabID.ToString()));
				}
			}

			if (cartPageID != string.Empty)
			{
				try
				{
					lstShoppingCartPageID.SelectedValue = cartPageID;
				}
				catch{}
			}
		}

        private void loadStoreTabs(string storePageID)
        {
            TabController tabController = new TabController();
            ArrayList tabs = tabController.GetTabs(PortalId);

            foreach (TabInfo tabInfo in tabs)
            {
                if (tabInfo.IsVisible && !tabInfo.IsDeleted && !tabInfo.IsAdminTab && !tabInfo.IsSuperTab)
                {
                    lstStorePageID.Items.Add(new ListItem(tabInfo.TabName, tabInfo.TabID.ToString()));
                }
            }

            if (storePageID != string.Empty)
            {
                try
                {
                    lstStorePageID.SelectedValue = storePageID;
                }
                catch { }
            }
        }

		private void loadShippingProvider()
		{
			plhShippingProvider.Controls.Clear();

			//Get an instance of the provider
			IShippingProvider shippingProvider = StoreController.GetShippingProvider(ModulePath);
			
			//Create an instance of the provider's admin control
			ProviderControlBase providerControl = shippingProvider.GetAdminControl(this, ModulePath);

			//providerControl.EditComplete += new EventHandler(editControl_EditComplete);

			plhShippingProvider.Controls.Add(providerControl);
		}

		private void loadTaxProvider()
		{
			plhTaxProvider.Controls.Clear();

			//Get an instance of the provider
			ITaxProvider taxProvider = StoreController.GetTaxProvider(ModulePath);
			
			//Create an instance of the provider's admin control
			ProviderControlBase providerControl = taxProvider.GetAdminControl(this, ModulePath);

			//providerControl.EditComplete += new EventHandler(editControl_EditComplete);

			plhTaxProvider.Controls.Add(providerControl);
		}

		private void loadGateways(string gatewayName)
		{
			GatewayController controller = new GatewayController(Server.MapPath(ModulePath));
			lstGateway.DataTextField = "GatewayName";
			lstGateway.DataValueField = "GatewayPath";
			lstGateway.DataSource = controller.GetGateways();
			lstGateway.DataBind();

			lstGateway.Items.Insert(0, new ListItem(Localization.GetString("EmptyComboValue", this.LocalResourceFile), Null.NullString));
                        
			if (gatewayName != Null.NullString)
			{
				try
				{
					GatewayInfo gateway = controller.GetGateway(gatewayName);
					lstGateway.SelectedValue = gateway.GatewayPath;
				}
				catch{}
			}
		}

		private void loadGatewayAdmin(string gatewayName)
		{
			// TODO: We may want to use caching here

			plhGateway.Controls.Clear();

			GatewayController controller = new GatewayController(Server.MapPath(ModulePath));
			GatewayInfo gateway = controller.GetGateway(gatewayName);
			if (gateway != null)
			{
				string controlPath = gateway.GatewayPath + "\\" + gateway.AdminControl;
				if (File.Exists(controlPath))
				{
					controlPath = controlPath.Replace(Server.MapPath(ModulePath), ModulePath);

					gatewayControl = (StoreControlBase)LoadControl(controlPath);
					gatewayControl.EnableViewState = true;
					gatewayControl.ParentControl = this as PortalModuleBase;
					gatewayControl.DataSource = gateway.GetSettings(PortalId);

					plhGateway.Controls.Add(gatewayControl);
				}
				else
				{
					LiteralControl error = new LiteralControl("<span class=\"NormalRed\">" + Localization.GetString("CouldNotFind", this.LocalResourceFile) + " " + controlPath + ".</span>");
					plhGateway.Controls.Add(error);
				}
			}
			else
			{
				LiteralControl error = new LiteralControl("<span class=\"NormalRed\">" + Localization.GetString("GatewayNotSelected", this.LocalResourceFile) + "</span>");
				plhGateway.Controls.Add(error);
			}
		}
		#endregion

        #region Public Functions
        public void ExpandShippingHeader()
        {
            dshShippingProvider.IsExpanded = true;
        }
        public void ExpandTaxHeader()
        {
            dshTaxProvider.IsExpanded = true;
        }
        #endregion
    }
}
