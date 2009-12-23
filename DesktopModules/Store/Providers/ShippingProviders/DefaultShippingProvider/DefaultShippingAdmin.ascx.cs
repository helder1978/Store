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
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Providers;
using DotNetNuke.Modules.Store.WebControls;

namespace DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider
{
	/// <summary>
	/// Summary description for CoreAdmin.
	/// </summary>
	public partial class DefaultShippingAdmin : ProviderControlBase
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
            this.grdShippingRates.ItemCommand += new DataGridCommandEventHandler(grdShippingRates_ItemCommand);
			this.btnSaveShippingFee.Click += new EventHandler(btnSaveShippingFee_Click);
		}
		#endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
                BindShippingRates();
			}
		}

        private void grdShippingRates_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            // Add the new item to the dataset.  I use an array here for efficiency.
            if (e.CommandName == "Add")
            {
                // Ajouté : Localisation des messages d'erreur
                ExpandShippingAdmin();
                ShippingInfo newShippingInfo = new ShippingInfo();

                if (((TextBox)e.Item.FindControl("txtNewDescription")).Text.Length == 0)
                {
                    ShowError(Localization.GetString("ErrorRateDescription", this.LocalResourceFile), ((TextBox)e.Item.FindControl("txtNewDescription")));
                    return;
                }
                newShippingInfo.Description = ((TextBox)e.Item.FindControl("txtNewDescription")).Text;
                ClearError(((TextBox)e.Item.FindControl("txtNewDescription")));
                try
                {
                    newShippingInfo.MinWeight = Decimal.Parse(((TextBox)e.Item.FindControl("txtNewMinWeight")).Text);
                    ClearError(((TextBox)e.Item.FindControl("txtNewMinWeight")));
                }
                catch (Exception)
                {
                    ShowError(Localization.GetString("ErrorMinWeight", this.LocalResourceFile), ((TextBox)e.Item.FindControl("txtNewMinWeight")));
                    return;
                }
                try
                {
                    newShippingInfo.MaxWeight = Decimal.Parse(((TextBox)e.Item.FindControl("txtNewMaxWeight")).Text);
                    ClearError(((TextBox)e.Item.FindControl("txtNewMaxWeight")));
                }
                catch (Exception)
                {
                    ShowError(Localization.GetString("ErrorMaxWeight", this.LocalResourceFile), ((TextBox)e.Item.FindControl("txtNewMaxWeight")));
                    return;
                }
                try
                {
                    newShippingInfo.Cost = Decimal.Parse(((TextBox)e.Item.FindControl("txtNewCost")).Text);
                    ClearError(((TextBox)e.Item.FindControl("txtNewCost")));
                }
                catch (Exception)
                {
                    ShowError(Localization.GetString("ErrorCost", this.LocalResourceFile), ((TextBox)e.Item.FindControl("txtNewCost")));
                    return;
                }
                ShippingController controller = new ShippingController();
                controller.AddShippingRate(PortalId, newShippingInfo);

                BindShippingRates();
                lblError.Visible = false;
            }
        }

		private void btnSaveShippingFee_Click(object sender, EventArgs e)
		{
            // Loop through the items in the datagrid.
            ShippingController controller = new ShippingController();
            ExpandShippingAdmin();

            foreach (DataGridItem di in grdShippingRates.Items)
            {
                // Make sure this is an item and not the header or footer.
                if (di.ItemType == ListItemType.Item || di.ItemType == ListItemType.AlternatingItem)
                {
                    // Get the current row for update or delete operations later.
                    int ID = (int)grdShippingRates.DataKeys[di.ItemIndex];

                    // See if this one needs to be deleted.
                    if (((CheckBox)di.FindControl("chkDelete")).Checked)
                    {
                        controller.DeleteShippingRate(ID);
                    }
                    else
                    {
                        // Update the row instead.
                        ShippingInfo shippingInfo = new ShippingInfo();
                        shippingInfo.ID = ID;

                        // Ajouté : Localisation des messages d'erreur
                        if (((TextBox)di.FindControl("lblDescription")).Text.Length == 0)
                        {
                            ShowError(Localization.GetString("ErrorRateDescription", this.LocalResourceFile), ((TextBox)di.FindControl("lblDescription")));
                            return;
                        }
                        shippingInfo.Description = ((TextBox)di.FindControl("lblDescription")).Text;
                        ClearError(((TextBox)di.FindControl("lblDescription")));
                        try
                        {
                            shippingInfo.MinWeight = Decimal.Parse(((TextBox)di.FindControl("lblMinWeight")).Text);
                            ClearError(((TextBox)di.FindControl("lblMinWeight")));
                        }
                        catch (Exception)
                        {
                            ShowError(Localization.GetString("ErrorMinWeight", this.LocalResourceFile), ((TextBox)di.FindControl("lblMinWeight")));
                            return;
                        }
                        try
                        {
                            shippingInfo.MaxWeight = Decimal.Parse(((TextBox)di.FindControl("lblMaxWeight")).Text);
                            ClearError(((TextBox)di.FindControl("lblMaxWeight")));
                        }
                        catch (Exception)
                        {
                            ShowError(Localization.GetString("ErrorMaxWeight", this.LocalResourceFile), ((TextBox)di.FindControl("lblMaxWeight")));
                            return;
                        }
                        try
                        {
                            shippingInfo.Cost = Decimal.Parse(((TextBox)di.FindControl("lblCost")).Text);
                            ClearError(((TextBox)di.FindControl("lblCost")));
                        }
                        catch (Exception)
                        {
                            ShowError(Localization.GetString("ErrorCost", this.LocalResourceFile), ((TextBox)di.FindControl("lblCost")));
                            return;
                        }

                        controller.UpdateShippingRate(shippingInfo);
                    }
                }
            }

            BindShippingRates();
            lblError.Visible = false;
		}

		#endregion

        #region Helper Functions

        private void BindShippingRates()
        {
            ShippingController controller = new ShippingController();
            ArrayList shippingRates = controller.GetAllShippingRates(parentControl.PortalId);
            grdShippingRates.DataSource = shippingRates;
            grdShippingRates.DataBind();
        }

        private void ExpandShippingAdmin()
        {
            ((StoreAdmin)((PlaceHolder)this.Parent).Parent.Parent.Parent.Parent).ExpandShippingHeader();
        }

        private void ShowError(string ErrorMessage, TextBox control)
        {
            lblError.Visible = true;
            lblError.Text = ErrorMessage;
            control.ForeColor = System.Drawing.Color.Red;
            control.BorderColor = System.Drawing.Color.Red;
        }

        private void ClearError(TextBox control)
        {
            control.ForeColor = System.Drawing.Color.Empty;
            control.BorderColor = System.Drawing.Color.Empty;
        }
        #endregion
    }
}
