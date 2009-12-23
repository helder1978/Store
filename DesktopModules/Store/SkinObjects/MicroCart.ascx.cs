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
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Cart;

namespace DotNetNuke.Modules.Store.SkinObjects
{
    public partial class MicroCart : DotNetNuke.UI.Skins.SkinObjectBase
    {
        private StoreInfo storeInfo = null;
        private NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
        private int portalId = -1;
        private CartInfo cartInfo = null;
        private string _resource = "";
        private string _text = "";

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion

        #region Properties
        private string _itemsTitleCssClass = "NormalBold";
        private string _itemsCssClass = "Normal";
        private string _totalTitleCssClass = "NormalBold";
        private string _totalCssClass = "Normal";

        public string ItemsTitleCssClass
        {
            get
            {
                return _itemsTitleCssClass;
            }
            set
            {
                _itemsTitleCssClass = value;
            }
        }

        public string ItemsCssClass
        {
            get
            {
                return _itemsCssClass;
            }
            set
            {
                _itemsCssClass = value;
            }
        }

        public string TotalTitleCssClass
        {
            get
            {
                return _totalTitleCssClass;
            }
            set
            {
                _totalTitleCssClass = value;
            }
        }

        public string TotalCssClass
        {
            get
            {
                return _totalCssClass;
            }
            set
            {
                _totalCssClass = value;
            }
        }
        #endregion

        #region event Handlers
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            lblStoreMicroCartItemsTitle.CssClass = _itemsTitleCssClass;
            lblStoreMicroCartTotalTitle.CssClass = _totalTitleCssClass;
            lblStoreMicroCartItems.CssClass = _itemsCssClass;
            lblStoreMicroCartTotal.CssClass = _totalCssClass;

            _resource = this.TemplateSourceDirectory + "/App_LocalResources/MicroCart.ascx.resx";
            lblStoreMicroCartItemsTitle.Text = Localization.GetString("CartItemsTitle.Text", _resource);
            lblStoreMicroCartTotalTitle.Text = Localization.GetString("CartTotalTitle.Text", _resource);

            try
            {
                if (storeInfo == null)
                {
                    portalId = this.PortalSettings.PortalId;
                    cartInfo = CurrentCart.GetInfo(portalId);

                    StoreController storeController = new StoreController();
                    storeInfo = storeController.GetStoreInfo(portalId);
                    if (storeInfo.CurrencySymbol != string.Empty)
                    {
                        LocalFormat.CurrencySymbol = storeInfo.CurrencySymbol;
                    }
                }

                _text = Localization.GetString("CartItems.Text", _resource);
                int _items = cartInfo.Items;

                if (_items > 0)
                {
                    lblStoreMicroCartItems.Text = string.Format(_text, _items);
                    lblStoreMicroCartTotal.Text = cartInfo.Total.ToString("C", LocalFormat);
                }
                else
                {
                    lblStoreMicroCartItems.Text = string.Format(_text, 0);
                    lblStoreMicroCartTotal.Text = (0D).ToString("C", LocalFormat);
                }
            }
            catch
            {
                _text = Localization.GetString("Error.Text", _resource);
                lblStoreMicroCartItems.Text = _text;
                lblStoreMicroCartTotalTitle.Text = _text;
            }
        }
        #endregion
    }
}