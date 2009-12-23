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
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial class MiniCart : PortalModuleBase
	{

		private ModuleSettings moduleSettings;
		private CartNavigation cartNav;
		private decimal cartTotal = 0;
		private int itemCount = 0;
        private StoreInfo storeInfo = null;
        private NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        //public Label lblCount;
        //public Label lblTotal;

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
			this.grdItems.ItemDataBound += new DataGridItemEventHandler(grdItems_ItemDataBound);
			this.btnViewCart.Click += new EventHandler(btnViewCart_Click);
		}
		#endregion

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            try
            {
                updateCartGrid();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

		#region Events
        protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                if (storeInfo == null)
                {
                    StoreController storeController = new StoreController();
                    storeInfo = storeController.GetStoreInfo(PortalId);
                    if (storeInfo.CurrencySymbol != string.Empty)
                    {
                        LocalFormat.CurrencySymbol = storeInfo.CurrencySymbol;
                    }

                    if (storeInfo.PortalTemplates)
                    {
                        CssTools.AddCss(this.Page, PortalSettings.HomeDirectory + "Store", PortalId);
                    }
                    else
                    {
                        CssTools.AddCss(this.Page, this.TemplateSourceDirectory, PortalId);
                    }
                }

			    moduleSettings = new ModuleSettings(ModuleId, TabId);
			    cartNav = new CartNavigation(Request.QueryString);
            }
            catch (Exception ex)
            {
                string ErrorSettings = Localization.GetString("ErrorSettings", this.LocalResourceFile);
                Exceptions.ProcessModuleLoadException(ErrorSettings, this, ex, true);
            }

			try 
			{
				updateCartGrid();
            }
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		private void grdItems_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			ItemInfo itemInfo = (ItemInfo)e.Item.DataItem;

			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				//Label lblPrice = (Label)e.Item.FindControl("lblPrice");
				//if (lblPrice != null)
				{
                    //lblPrice.Text = (itemInfo.UnitCost * itemInfo.Quantity).ToString("C", LocalFormat);
					cartTotal += itemInfo.UnitCost * itemInfo.Quantity;
					itemCount += itemInfo.Quantity;
				}

                string Message = null;
                /*
				LinkButton lnkAdd = (LinkButton)e.Item.FindControl("lnkAdd");
				if (lnkAdd != null)
				{
                    Message = Localization.GetString("AddAnother", this.LocalResourceFile);
					lnkAdd.Attributes.Add("title", string.Format(Message, itemInfo.ProductTitle));
					lnkAdd.CommandName = itemInfo.ItemID.ToString();
					lnkAdd.CommandArgument = itemInfo.Quantity.ToString();
					lnkAdd.Command += new CommandEventHandler(btnAdd_Click);
				}
                
				LinkButton lnkRemove = (LinkButton)e.Item.FindControl("lnkRemove");
				if (lnkRemove != null)
				{
                    Message = Localization.GetString("RemoveOne", this.LocalResourceFile);
                    lnkRemove.Attributes.Add("title", string.Format(Message, itemInfo.ProductTitle));
					lnkRemove.CommandName = itemInfo.ItemID.ToString();
					lnkRemove.CommandArgument = itemInfo.Quantity.ToString();
					lnkRemove.Command += new CommandEventHandler(btnRemove_Click);
				}
                */
				LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
                if (lnkDelete != null)
				{
                    Message = Localization.GetString("RemoveAll", this.LocalResourceFile);
                    lnkDelete.Attributes.Add("title", string.Format(Message, itemInfo.ProductTitle));
					lnkDelete.CommandName = itemInfo.ItemID.ToString();
                    
					lnkDelete.CommandArgument = itemInfo.Quantity.ToString();
					lnkDelete.Command += new CommandEventHandler(btnDelete_Click);
				}
                
			}
            /*
            if (e.Item.ItemType == ListItemType.Footer)
            {
                //if (lblCount != null)
                {
                    lblCount.Text = itemCount.ToString();
                }
                //if (lblTotal != null)
                {
                    lblTotal.Text = cartTotal.ToString("C", LocalFormat);
                }
            }*/
            /*
			if (e.Item.ItemType == ListItemType.Footer)
			{
				Label lblCount = (Label)e.Item.FindControl("lblCount");
				if (lblCount != null)
				{
					lblCount.Text = itemCount.ToString();
				}

				Label lblTotal = (Label)e.Item.FindControl("lblTotal");
				if (lblTotal != null)
				{
                    lblTotal.Text = cartTotal.ToString("C", LocalFormat);
				}
			}
            */
		}

		private void btnAdd_Click(object sender, CommandEventArgs e)
		{
			int itemID = int.Parse(e.CommandName);
			int quantity = int.Parse(e.CommandArgument.ToString()) + 1;

			CurrentCart.UpdateItem(PortalId, itemID, quantity);
			updateCartGrid();
        }

		private void btnRemove_Click(object sender, CommandEventArgs e)
		{
            int itemID = int.Parse(e.CommandName);
			int quantity = int.Parse(e.CommandArgument.ToString()) - 1;

            if (quantity < 1)
			{
				CurrentCart.RemoveItem(itemID);
			}
			else
			{
				CurrentCart.UpdateItem(PortalId, itemID, quantity);
			}

			updateCartGrid();
        }

		private void btnDelete_Click(object sender, CommandEventArgs e)
		{
            int itemID = int.Parse(e.CommandName);

			CurrentCart.RemoveItem(itemID);
			updateCartGrid();
        }

		private void btnViewCart_Click(object sender, EventArgs e)
		{
			CartInfo cartInfo = CurrentCart.GetInfo(PortalId);

			if (cartInfo != null)
			{
				Response.Redirect(Globals.NavigateURL(cartInfo.CartPageID));
			}
		}
		#endregion

		#region Private Functions
		private void updateCartGrid()
		{
			cartTotal = 0;
			itemCount = 0;
			grdItems.DataSource = CurrentCart.GetItems(PortalId);
			grdItems.DataBind();

            if (lblCount != null)
            {
                this.lblCount.Text = itemCount.ToString();
            }
            //if ((lblTotal != null) && (cartTotal != 0))
            if (lblTotal != null) 
            {
                this.lblTotal.Text = "Total: " + cartTotal.ToString("C", LocalFormat);
            }

        }
		#endregion
	}
}
