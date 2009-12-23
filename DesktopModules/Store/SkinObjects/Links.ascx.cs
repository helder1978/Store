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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Admin;

namespace DotNetNuke.Modules.Store.SkinObjects
{
    public partial class Links : DotNetNuke.UI.Skins.SkinObjectBase
    {

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
        private string _imageCssClass = "Normal";
        private string _imageName = "cart.png";
        private string _linkAction = "Cart";
        private string _textCssClass = "Normal";
        private bool _textVisible = true;

        public string ImageCssClass
        {
            get
            {
                return _imageCssClass;
            }
            set
            {
                _imageCssClass = value;
            }
        }

        public string ImageName
        {
            get
            {
                return _imageName;
            }
            set
            {
                _imageName = value;
            }
        }

        public string LinkAction
        {
            get
            {
                return _linkAction;
            }
            set
            {
                _linkAction = value;
            }
        }

        public string TextCssClass
        {
            get
            {
                return _textCssClass;
            }
            set
            {
                _textCssClass = value;
            }
        }

        public bool TextVisible
        {
            get
            {
                return _textVisible;
            }
            set
            {
                _textVisible = value;
            }
        }
        #endregion
        
        #region event Handlers
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsPostBack)
            {
                string _resource = this.TemplateSourceDirectory + "/App_LocalResources/Links.ascx.resx";
                string _text = "";

                try
                {
                    StoreController storeController = new StoreController();
                    StoreInfo storeInfo = storeController.GetStoreInfo(PortalSettings.PortalId);

                    int _tabID = 0;

                    switch (_linkAction.ToLower())
                    {
                        case "cart":
                            _text = Localization.GetString("CartTitle.Text", _resource);
                            _tabID = storeInfo.ShoppingCartPageID;
                            break;
                        case "store":
                            _text = Localization.GetString("StoreTitle.Text", _resource);
                            _tabID = storeInfo.StorePageID;
                            break;
                        default:
                            _text = Localization.GetString("UnknowAction.Text", _resource);
                            _tabID = PortalSettings.HomeTabId;
                            _textVisible = true;
                            break;
                    }

                    if (_textVisible)
                    {
                        lnkAction.CssClass = _textCssClass;
                        lnkAction.Text = _text;
                        lnkAction.PostBackUrl = Globals.NavigateURL(_tabID);
                    }
                    lnkAction.Visible = _textVisible;

                    if (_imageName != string.Empty)
                    {
                        if (storeInfo.PortalTemplates)
                        {
                            btnImage.ImageUrl = PortalSettings.HomeDirectory + "Store/Templates/Images/" + _imageName;
                        }
                        else
                        {
                            btnImage.ImageUrl = this.TemplateSourceDirectory + "/../Templates/Images/" + _imageName;
                        }
                        btnImage.CssClass = _imageCssClass;
                        btnImage.ToolTip = _text;
                        btnImage.PostBackUrl = Globals.NavigateURL(_tabID);
                    }
                    else
                    {
                        btnImage.Visible = false;
                    }
                }
                catch
                {
                    lnkAction.CssClass = _textCssClass;
                    _text = Localization.GetString("Error.Text", _resource);
                    lnkAction.Text = _text;
                    btnImage.Visible = false;
                }
            }
        }
        #endregion
    }
}
