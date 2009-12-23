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
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Customer;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CustomerCart.
	/// </summary>
	public partial  class CustomerCart : StoreControlBase
	{
		#region Controls

		protected DotNetNuke.UI.UserControls.LabelControl lblParentTitle;

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
            lblInfos.Visible = false;

            // Canadean changed: added code to ignore conditions when confirming an order
            if(Request.QueryString.Get("WorldPayExit") != "")
            {
                //LinkButton btnCheckout;
                //ImageButton imgCheckout;
                if (parentControl.UserId < 1)
                {
                    panLoginRegister.Visible = true;
                    //imgCheckout.ImageUrl = "/images/canadean/shop/shop_login_checkout.jpg";
                    //this.imgCheckout.Click += new ImageClickEventHandler(btnCheckout_Click_LoginToCheckout);

                    //btnCheckout.Text = Localization.GetString("LoginToCheckout", this.LocalResourceFile);
                    //this.btnCheckout.Click += new EventHandler(btnCheckout_Click_LoginToCheckout);
                    lblInfos.Text = Localization.GetString("CheckoutInfos", this.LocalResourceFile);
                    //lblInfos.Visible = true;
                }
                else if (CurrentCart.GetItems(PortalId).Count <= 0)
                {
                    imgCheckout.Visible=true;
                    imgCheckout.ImageUrl = "/images/canadean/shop/shop_addItems_checkout.jpg";
                    this.imgCheckout.Click += new ImageClickEventHandler(btnCheckout_Click_AddItemsToCheckout);

                    //btnCheckout.Text = Localization.GetString("AddItemsToCheckout", this.LocalResourceFile);
                    //this.btnCheckout.Click += new EventHandler(btnCheckout_Click_AddItemsToCheckout);
                }
                else
                {
                    imgCheckout.Visible = true;
                    imgCheckout.ImageUrl = "/images/canadean/shop/shop_continue_checkout.jpg";
                    this.imgCheckout.Click += new ImageClickEventHandler(btnCheckout_Click);

                    //btnCheckout.Text = Localization.GetString("btnCheckout", this.LocalResourceFile);
                    //this.btnCheckout.Click += new EventHandler(btnCheckout_Click);
                }
            }
			loadCartControl();
		}

		protected override void OnPreRender(EventArgs e)
		{
			// Set the title in the parent control
			Account accountControl = (Account)parentControl;
			accountControl.ParentTitle = lblParentTitle.Text;

			base.OnPreRender (e);
		}

		private void btnCheckout_Click(object sender, EventArgs e)
		{
            //redirect to the edit page with current skin and container.
            string[] additionalParams = new string[1];
            additionalParams[0] = "mid=" + parentControl.ModuleId.ToString();
            Response.Redirect(Globals.NavigateURL(parentControl.TabId, "Checkout", additionalParams));
        }

        private void btnCheckout_Click_AddItemsToCheckout(object sender, EventArgs e)
		{
            StoreController storeController = new StoreController();
            StoreInfo storeInfo = storeController.GetStoreInfo(PortalSettings.PortalId);

            Response.Redirect(Globals.NavigateURL(storeInfo.StorePageID));
		}

		private void btnCheckout_Click_LoginToCheckout(object sender, EventArgs e)
		{
            //this.Page.Form.Target = "_top";
            Response.Redirect(Globals.NavigateURL(parentControl.TabId, "login"));
        }

		#endregion

		#region Private Function
        
		private void loadCartControl()
		{
			plhCart.Controls.Clear();

			// TODO: We may want to use caching here
			StoreControlBase cartControl = (StoreControlBase)LoadControl(ModulePath + "CartDetail.ascx");
			cartControl.ParentControl = this.parentControl as PortalModuleBase;
			//cartControl.LocalResourceFile = ModulePath + "App_LocalResources\\" + filename + ".resx";

			plhCart.Controls.Add(cartControl);
		}
		#endregion
	}
}
