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
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial class CartDetail : StoreControlBase
	{

		private ModuleSettings moduleSettings;
		private CartNavigation cartNav;
		private decimal cartTotal = 0;
		private int itemCount = 0;
        private StoreInfo storeInfo = null;
        private NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();

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
		}
		#endregion

		#region Events
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (storeInfo == null)
            {
                StoreController storeController = new StoreController();
                storeInfo = storeController.GetStoreInfo(PortalId);
                if (storeInfo.CurrencySymbol != string.Empty)
                {
                    LocalFormat.CurrencySymbol = storeInfo.CurrencySymbol;
                }
            }

            moduleSettings = new ModuleSettings(parentControl.ModuleId, parentControl.TabId);
			cartNav = new CartNavigation(Request.QueryString);
            
			//try 
			//{
            
				updateCartGrid();
			//}
			//catch(Exception ex) 
			//{
			//	Exceptions.ProcessModuleLoadException(this, ex);
			//}
		}

		private void grdItems_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			ItemInfo itemInfo = (ItemInfo)e.Item.DataItem;

			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Image imgProductImage = (Image)e.Item.FindControl("imgProductImage");
				if (imgProductImage != null)
				{
					imgProductImage.Width = int.Parse(moduleSettings.MainCart.ThumbnailWidth);
					imgProductImage.ImageUrl = getImageUrl(itemInfo.ProductImage);
					imgProductImage.AlternateText = itemInfo.ProductTitle;
					imgProductImage.Visible = bool.Parse(moduleSettings.MainCart.ShowThumbnail);
				}

				Label lblPrice = (Label)e.Item.FindControl("lblPrice");
				if (lblPrice != null)
				{
                    lblPrice.Text = itemInfo.UnitCost.ToString("C", LocalFormat);
				}

				Label lblSubtotal = (Label)e.Item.FindControl("lblSubtotal");
				if (lblSubtotal != null)
				{
                    lblSubtotal.Text = (itemInfo.UnitCost * itemInfo.Quantity).ToString("C", LocalFormat);
					cartTotal += itemInfo.UnitCost * itemInfo.Quantity;
					itemCount += itemInfo.Quantity;
				}

				LinkButton lnkAdd = (LinkButton)e.Item.FindControl("lnkAdd");
				if (lnkAdd != null)
				{
                    // Traduction effectu�e
                    lnkAdd.Attributes.Add("title", Localization.GetString("AddAnother", this.LocalResourceFile) + " " + itemInfo.ProductTitle + " " + Localization.GetString("ToTheCart", this.LocalResourceFile));
					lnkAdd.CommandName = itemInfo.ItemID.ToString();
					lnkAdd.CommandArgument = itemInfo.Quantity.ToString();
					lnkAdd.Command += new CommandEventHandler(btnAdd_Click);
					lnkAdd.CausesValidation = false;
				}

				LinkButton lnkRemove = (LinkButton)e.Item.FindControl("lnkRemove");
				if (lnkRemove != null)
				{
					//TODO: Needs localization
                    // Traduction effectu�e
                    lnkRemove.Attributes.Add("title", Localization.GetString("RemoveA", this.LocalResourceFile) + " " + itemInfo.ProductTitle + " " + Localization.GetString("FromTheCart", this.LocalResourceFile));
					lnkRemove.CommandName = itemInfo.ItemID.ToString();
					lnkRemove.CommandArgument = itemInfo.Quantity.ToString();
					lnkRemove.Command += new CommandEventHandler(btnRemove_Click);
					lnkRemove.CausesValidation = false;
				}

				LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
				if (lnkDelete != null)
				{
					//TODO: Needs localization
                    // Traduction effectu�e
                    lnkDelete.Attributes.Add("title", Localization.GetString("RemoveAll", this.LocalResourceFile) + " " + itemInfo.ProductTitle + " " + Localization.GetString("SFromTheCart", this.LocalResourceFile));
					lnkDelete.CommandName = itemInfo.ItemID.ToString();
					lnkDelete.CommandArgument = itemInfo.Quantity.ToString();
					lnkDelete.Command += new CommandEventHandler(btnDelete_Click);
					lnkDelete.CausesValidation = false;
				}
			}

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
		}

		private void btnAdd_Click(object sender, CommandEventArgs e)
		{
			int itemID = int.Parse(e.CommandName);
			int quantity = int.Parse(e.CommandArgument.ToString()) + 1;

			CurrentCart.UpdateItem(PortalId, itemID, quantity);
			updateCartGrid();

			this.invokeEditComplete();
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
			this.invokeEditComplete();
		}

		private void btnDelete_Click(object sender, CommandEventArgs e)
		{
			int itemID = int.Parse(e.CommandName);

			CurrentCart.RemoveItem(itemID);
			updateCartGrid();
			this.invokeEditComplete();
		}
		#endregion

		#region Private Functions
		private void updateCartGrid()
		{
			cartTotal = 0;
			itemCount = 0;
			grdItems.DataSource = CurrentCart.GetItems(PortalId);
			grdItems.DataBind();
            
            if (grdItems.Items.Count == 0)
            {
                grdItems.Visible = false;
                lblBasketEmpty.Visible = true;
            }
            else
            {
                grdItems.Visible = true;
                lblBasketEmpty.Visible = false;
            }
		}

		private string getImageUrl(string image) 
		{ 
			return parentControl.ModulePath + "Thumbnail.aspx?IP=" + image + "&IW=" + moduleSettings.MainCart.ThumbnailWidth; 
		}
		#endregion
	}
}
