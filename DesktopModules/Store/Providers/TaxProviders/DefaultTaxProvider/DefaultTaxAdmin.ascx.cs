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
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common.Lists;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Providers;

namespace DotNetNuke.Modules.Store.Providers.Tax.DefaultTaxProvider
{
	/// <summary>
	/// Summary description for CoreAdmin.
	/// </summary>
	public partial class DefaultTaxAdmin : ProviderControlBase
	{
		#region Controls

		protected DataList lstTaxRates;
        protected RegularExpressionValidator valRegex;

		#endregion

		private Hashtable taxRates = new Hashtable();

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
			this.btnSaveTaxRates.Click += new EventHandler(btnSaveTaxRates_Click);
		}

		#endregion

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				loadTaxRates();
			}
		}

		private void btnSaveTaxRates_Click(object sender, EventArgs e)
		{
			saveTaxRates();
            ExpandTaxAdmin();
		}

		#endregion

		#region Private Functions

		private void saveTaxRates()
		{
            decimal rate = -1;

            if (cbEnableTax.Checked && txtTaxRate.Text.Length == 0)
            {
                lblError.Visible = true;
                lblError.Text = Localization.GetString("lblErrorTax", this.LocalResourceFile);
                txtTaxRate.BorderColor = System.Drawing.Color.Red;
                txtTaxRate.ForeColor = System.Drawing.Color.Red;
                return;
            }
            else
            {
                lblError.Visible = false;
                txtTaxRate.BorderColor = System.Drawing.Color.Empty;
                txtTaxRate.ForeColor = System.Drawing.Color.Empty;
            }

            if (txtTaxRate.Text.Length > 0)
            {
                try
                {
                    rate = Decimal.Parse(txtTaxRate.Text);
                    lblError.Visible = false;
                    txtTaxRate.BorderColor = System.Drawing.Color.Empty;
                    txtTaxRate.ForeColor = System.Drawing.Color.Empty;
                    if (rate < 0)
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    lblError.Visible = true;
                    lblError.Text = Localization.GetString("lblErrorTax", this.LocalResourceFile);
                    txtTaxRate.BorderColor = System.Drawing.Color.Red;
                    txtTaxRate.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
            
            TaxController controller = new TaxController();
            controller.UpdateTaxRates(this.PortalId, rate, cbEnableTax.Checked);
		}

		private void loadTaxRates()
		{
            TaxController controller = new TaxController();
            TaxInfo taxInfo = controller.GetTaxRates(this.PortalId);
            txtTaxRate.Text = taxInfo.DefaultTaxRate < 0 ? "" : taxInfo.DefaultTaxRate.ToString("0.00");
            cbEnableTax.Checked = taxInfo.ShowTax;            
		}

        private void ExpandTaxAdmin()
        {
            ((DotNetNuke.Modules.Store.WebControls.StoreAdmin)((PlaceHolder)this.Parent).Parent.Parent.Parent.Parent).ExpandTaxHeader();
        }

		#endregion
	}
}
