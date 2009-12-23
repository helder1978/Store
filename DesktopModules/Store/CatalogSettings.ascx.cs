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
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
    public partial class CatalogSettings : DotNetNuke.Entities.Modules.ModuleSettingsBase
	{
		protected ModuleSettings moduleSettings;
        private StoreInfo storeInfo;
        private string templatesPath = "";

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);

			moduleSettings = new ModuleSettings(this.ModuleId,  this.TabId);
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
			try
			{
				if (!Page.IsPostBack)
				{
                    CategoryController categoryController = new CategoryController();
					ArrayList categoryList = categoryController.GetCategories(this.PortalId, false, -1);
					bool defaultExists = false;

                    lstDefaultCategory.Items.Add(new ListItem(Localization.GetString("SelectDefaultCategory", this.LocalResourceFile), "-1"));

					foreach (CategoryInfo categoryInfo in categoryList)
					{
						lstDefaultCategory.Items.Add(new ListItem(categoryInfo.CategoryName, categoryInfo.CategoryID.ToString()));

						if (categoryInfo.CategoryID.ToString() == moduleSettings.General.DefaultCategoryID)
						{
							defaultExists = true;
						}
					}

					if (lstDefaultCategory.Items.Count > 1 && Int32.Parse(moduleSettings.General.DefaultCategoryID) > 0 && defaultExists)
					{
						lstDefaultCategory.SelectedValue = moduleSettings.General.DefaultCategoryID;
					}
				}
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}
		#endregion

		#region Base Method Implementations
		public override void LoadSettings()
		{
			try
			{
				if (!Page.IsPostBack)
				{
                    if (storeInfo == null)
                    {
                        StoreController storeController = new StoreController();
                        storeInfo = storeController.GetStoreInfo(PortalId);
                        if (storeInfo.PortalTemplates)
                        {
                            templatesPath = PortalSettings.HomeDirectoryMapPath + "Store\\";
                        }
                        else
                        {
                            templatesPath = MapPath(ModulePath) + "\\";
                        }
                    }

					TabController tabController = new TabController();
					ArrayList tabs = tabController.GetTabs(PortalId);

                    lstNPLDetailPage.Items.Add(new ListItem(Localization.GetString("NPLSamePage", this.LocalResourceFile), "0"));
                    lstFPLDetailPage.Items.Add(new ListItem(Localization.GetString("FPLSamePage", this.LocalResourceFile), "0"));
                    lstPPLDetailPage.Items.Add(new ListItem(Localization.GetString("PPLSamePage", this.LocalResourceFile), "0"));
                    lstCPLDetailPage.Items.Add(new ListItem(Localization.GetString("CPLSamePage", this.LocalResourceFile), "0"));
                    lstPDSReturnPage.Items.Add(new ListItem(Localization.GetString("PDSSamePage", this.LocalResourceFile), "0"));

					foreach (TabInfo tabInfo in tabs)
					{
						if (!tabInfo.IsDeleted && !tabInfo.IsAdminTab && !tabInfo.IsSuperTab)
						{
                            lstNPLDetailPage.Items.Add(new ListItem(tabInfo.TabName, tabInfo.TabID.ToString()));
                            lstFPLDetailPage.Items.Add(new ListItem(tabInfo.TabName, tabInfo.TabID.ToString()));
							lstPPLDetailPage.Items.Add(new ListItem(tabInfo.TabName, tabInfo.TabID.ToString()));
							lstCPLDetailPage.Items.Add(new ListItem(tabInfo.TabName, tabInfo.TabID.ToString()));
                            lstPDSReturnPage.Items.Add(new ListItem(tabInfo.TabName, tabInfo.TabID.ToString()));
						}
					}
					
					loadTemplates();

                    String repeatDirection = Localization.GetString("RepeatDirectionHoriz", this.LocalResourceFile);
                    lstNPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "H"));
                    lstFPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "H"));
                    lstPPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "H"));
                    lstCPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "H"));

                    repeatDirection = Localization.GetString("RepeatDirectionVert", this.LocalResourceFile);
                    lstNPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "V"));
                    lstFPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "V"));
                    lstPPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "V"));
                    lstCPLRepeatDirection.Items.Add(new ListItem(repeatDirection, "V"));

					// General Player Settings
					chkUseDefaultCategory.Checked = bool.Parse(moduleSettings.General.UseDefaultCategory);
					chkShowMessage.Checked = bool.Parse(moduleSettings.General.ShowMessage);
                    chkShowNew.Checked = bool.Parse(moduleSettings.General.ShowNewProducts);
					chkShowFeatured.Checked = bool.Parse(moduleSettings.General.ShowFeaturedProducts);
					chkShowPopular.Checked = bool.Parse(moduleSettings.General.ShowPopularProducts);
					chkShowCategory.Checked = bool.Parse(moduleSettings.General.ShowCategoryProducts);
					chkShowDetail.Checked = bool.Parse(moduleSettings.General.ShowProductDetail);
					lstDefaultCategory.SelectedValue = moduleSettings.General.DefaultCategoryID;
                    ListItem itemTemplate = lstTemplate.Items.FindByText(moduleSettings.General.Template);
                    if (itemTemplate != null)
                    {
                        itemTemplate.Selected = true;
                    }

                    // New list settings
                    ListItem itemNPLContainerTemplate = lstNPLContainerTemplate.Items.FindByText(moduleSettings.NewProducts.ContainerTemplate);
                    if (itemNPLContainerTemplate != null)
					{
                        itemNPLContainerTemplate.Selected = true;
					}
                    ListItem itemNPLTemplate = lstNPLTemplate.Items.FindByText(moduleSettings.NewProducts.Template);
                    if (itemNPLTemplate != null)
                    {
                        itemNPLTemplate.Selected = true;
                    }
                    txtNPLRowCount.Text = moduleSettings.NewProducts.RowCount;
                    txtNPLColumnCount.Text = moduleSettings.NewProducts.ColumnCount;
                    txtNPLColumnWidth.Text = moduleSettings.NewProducts.ColumnWidth;
                    ListItem itemNPLDirection = lstNPLRepeatDirection.Items.FindByValue(moduleSettings.NewProducts.RepeatDirection);
                    if (itemNPLDirection != null)
                    {
                        itemNPLDirection.Selected = true;
                    }
                    txtNPLThumbnailWidth.Text = moduleSettings.NewProducts.ThumbnailWidth;
                    chkNPLShowThumbnail.Checked = bool.Parse(moduleSettings.NewProducts.ShowThumbnail);
                    lstNPLDetailPage.SelectedValue = moduleSettings.NewProducts.DetailPage;

					// Featured list settings
                    ListItem itemFPLContainerTemplate = lstFPLContainerTemplate.Items.FindByText(moduleSettings.FeaturedProducts.ContainerTemplate);
                    if (itemFPLContainerTemplate != null)
					{
                        itemFPLContainerTemplate.Selected = true;
					}
					ListItem itemFPLTemplate = lstFPLTemplate.Items.FindByText(moduleSettings.FeaturedProducts.Template);
					if (itemFPLTemplate != null)
					{
						itemFPLTemplate.Selected = true;
					}
					txtFPLRowCount.Text = moduleSettings.FeaturedProducts.RowCount;
					txtFPLColumnCount.Text = moduleSettings.FeaturedProducts.ColumnCount;
					txtFPLColumnWidth.Text = moduleSettings.FeaturedProducts.ColumnWidth;
                    ListItem itemFPLDirection = lstFPLRepeatDirection.Items.FindByValue(moduleSettings.FeaturedProducts.RepeatDirection);
                    if (itemFPLDirection != null)
                    {
                        itemFPLDirection.Selected = true;
                    }
					txtFPLThumbnailWidth.Text = moduleSettings.FeaturedProducts.ThumbnailWidth;
					chkFPLShowThumbnail.Checked = bool.Parse(moduleSettings.FeaturedProducts.ShowThumbnail);
					lstFPLDetailPage.SelectedValue = moduleSettings.FeaturedProducts.DetailPage;

					// Popular list settings
                    ListItem itemPPLContainerTemplate = lstPPLContainerTemplate.Items.FindByText(moduleSettings.PopularProducts.ContainerTemplate);
                    if (itemPPLContainerTemplate != null)
					{
                        itemPPLContainerTemplate.Selected = true;
					}
					ListItem itemPPLTemplate = lstPPLTemplate.Items.FindByText(moduleSettings.PopularProducts.Template);
					if (itemPPLTemplate != null)
					{
						itemPPLTemplate.Selected = true;
					}
					txtPPLRowCount.Text = moduleSettings.PopularProducts.RowCount;
					txtPPLColumnCount.Text = moduleSettings.PopularProducts.ColumnCount;
					txtPPLColumnWidth.Text = moduleSettings.PopularProducts.ColumnWidth;
                    ListItem itemPPLDirection = lstPPLRepeatDirection.Items.FindByValue(moduleSettings.PopularProducts.RepeatDirection);
                    if (itemPPLDirection != null)
                    {
                        itemPPLDirection.Selected = true;
                    }
					txtPPLThumbnailWidth.Text = moduleSettings.PopularProducts.ThumbnailWidth;
					chkPPLShowThumbnail.Checked = bool.Parse(moduleSettings.PopularProducts.ShowThumbnail);
					lstPPLDetailPage.SelectedValue = moduleSettings.PopularProducts.DetailPage;

					// Category list settings
					ListItem itemCPLContainerTemplate = lstCPLContainerTemplate.Items.FindByText(moduleSettings.CategoryProducts.ContainerTemplate);
                    if (itemCPLContainerTemplate != null)
					{
                        itemCPLContainerTemplate.Selected = true;
					}
					ListItem itemCPLTemplate = lstCPLTemplate.Items.FindByText(moduleSettings.CategoryProducts.Template);
					if (itemCPLTemplate != null)
					{
						itemCPLTemplate.Selected = true;
					}
					txtCPLRowCount.Text = moduleSettings.CategoryProducts.RowCount;
					txtCPLColumnCount.Text = moduleSettings.CategoryProducts.ColumnCount;
					txtCPLColumnWidth.Text = moduleSettings.CategoryProducts.ColumnWidth;
                    ListItem itemCPLDirection = lstCPLRepeatDirection.Items.FindByValue(moduleSettings.CategoryProducts.RepeatDirection);
                    if (itemCPLDirection != null)
                    {
                        itemCPLDirection.Selected = true;
                    }
					txtCPLThumbnailWidth.Text = moduleSettings.CategoryProducts.ThumbnailWidth;
					chkCPLShowThumbnail.Checked = bool.Parse(moduleSettings.CategoryProducts.ShowThumbnail);
					lstCPLDetailPage.SelectedValue = moduleSettings.CategoryProducts.DetailPage;

					// Detail settings
					ListItem itemDetailTemplate = lstDetailTemplate.Items.FindByText(moduleSettings.ProductDetail.Template);
					if (itemDetailTemplate != null)
					{
						itemDetailTemplate.Selected = true;
					}
					chkDetailShowThumbnail.Checked = bool.Parse(moduleSettings.ProductDetail.ShowThumbnail);
					txtDetailThumbnailWidth.Text = moduleSettings.ProductDetail.ThumbnailWidth;
					chkDetailShowReviews.Checked = bool.Parse(moduleSettings.ProductDetail.ShowReviews);
                    lstPDSReturnPage.SelectedValue = moduleSettings.ProductDetail.ReturnPage;
				}
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		public override void UpdateSettings()
		{
			try
			{
				PortalSecurity security = new PortalSecurity();

				// General Settings
				moduleSettings.General.Template = lstTemplate.SelectedItem.Text;
				moduleSettings.General.UseDefaultCategory = chkUseDefaultCategory.Checked.ToString();
				moduleSettings.General.ShowMessage = chkShowMessage.Checked.ToString();
                moduleSettings.General.ShowNewProducts = chkShowNew.Checked.ToString();
                moduleSettings.General.ShowFeaturedProducts = chkShowFeatured.Checked.ToString();
				moduleSettings.General.ShowPopularProducts = chkShowPopular.Checked.ToString();
				moduleSettings.General.ShowCategoryProducts = chkShowCategory.Checked.ToString();
				moduleSettings.General.ShowProductDetail = chkShowDetail.Checked.ToString();
                moduleSettings.General.Template = lstTemplate.SelectedItem.Text;

				if (chkUseDefaultCategory.Checked)
				{
					moduleSettings.General.DefaultCategoryID = lstDefaultCategory.SelectedItem.Value;
				}

                // New list settings
                moduleSettings.NewProducts.ContainerTemplate = lstNPLContainerTemplate.SelectedItem.Text;
                moduleSettings.NewProducts.Template = lstNPLTemplate.SelectedItem.Text;
                moduleSettings.NewProducts.RowCount = security.InputFilter(txtNPLRowCount.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting);
                moduleSettings.NewProducts.ColumnCount = security.InputFilter(txtNPLColumnCount.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting);
                moduleSettings.NewProducts.ColumnWidth = security.InputFilter(txtNPLColumnWidth.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting);
                moduleSettings.NewProducts.RepeatDirection = lstNPLRepeatDirection.SelectedItem.Value;
                moduleSettings.NewProducts.ThumbnailWidth = security.InputFilter(txtNPLThumbnailWidth.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting);
                moduleSettings.NewProducts.ShowThumbnail = chkNPLShowThumbnail.Checked.ToString();
                moduleSettings.NewProducts.DetailPage = lstNPLDetailPage.SelectedItem.Value;

				// Featured list settings
                moduleSettings.FeaturedProducts.ContainerTemplate = lstFPLContainerTemplate.SelectedItem.Text;
				moduleSettings.FeaturedProducts.Template = lstFPLTemplate.SelectedItem.Text;
				moduleSettings.FeaturedProducts.RowCount = security.InputFilter(txtFPLRowCount.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
				moduleSettings.FeaturedProducts.ColumnCount = security.InputFilter(txtFPLColumnCount.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
				moduleSettings.FeaturedProducts.ColumnWidth = security.InputFilter(txtFPLColumnWidth.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
                moduleSettings.FeaturedProducts.RepeatDirection = lstFPLRepeatDirection.SelectedItem.Value;
				moduleSettings.FeaturedProducts.ThumbnailWidth = security.InputFilter(txtFPLThumbnailWidth.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
				moduleSettings.FeaturedProducts.ShowThumbnail = chkFPLShowThumbnail.Checked.ToString();
				moduleSettings.FeaturedProducts.DetailPage = lstFPLDetailPage.SelectedItem.Value;

				// Popular list settings
                moduleSettings.PopularProducts.ContainerTemplate = lstPPLContainerTemplate.SelectedItem.Text;
				moduleSettings.PopularProducts.Template = lstPPLTemplate.SelectedItem.Text;
				moduleSettings.PopularProducts.RowCount = security.InputFilter(txtPPLRowCount.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
				moduleSettings.PopularProducts.ColumnCount = security.InputFilter(txtPPLColumnCount.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
				moduleSettings.PopularProducts.ColumnWidth = security.InputFilter(txtPPLColumnWidth.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
                moduleSettings.PopularProducts.RepeatDirection = lstPPLRepeatDirection.SelectedItem.Value;
				moduleSettings.PopularProducts.ThumbnailWidth = security.InputFilter(txtPPLThumbnailWidth.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
				moduleSettings.PopularProducts.ShowThumbnail = chkPPLShowThumbnail.Checked.ToString();
				moduleSettings.PopularProducts.DetailPage = lstPPLDetailPage.SelectedItem.Value;

				// Category list settings
                moduleSettings.CategoryProducts.ContainerTemplate = lstCPLContainerTemplate.SelectedItem.Text;
				moduleSettings.CategoryProducts.Template = lstCPLTemplate.SelectedItem.Text;
				moduleSettings.CategoryProducts.RowCount = security.InputFilter(txtCPLRowCount.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
				moduleSettings.CategoryProducts.ColumnCount = security.InputFilter(txtCPLColumnCount.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
				moduleSettings.CategoryProducts.ColumnWidth = security.InputFilter(txtCPLColumnWidth.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
                moduleSettings.CategoryProducts.RepeatDirection = lstCPLRepeatDirection.SelectedItem.Value;
				moduleSettings.CategoryProducts.ThumbnailWidth = security.InputFilter(txtCPLThumbnailWidth.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
				moduleSettings.CategoryProducts.ShowThumbnail = chkCPLShowThumbnail.Checked.ToString();
				moduleSettings.CategoryProducts.DetailPage = lstCPLDetailPage.SelectedItem.Value;

				// Detail settings
				moduleSettings.ProductDetail.Template = lstDetailTemplate.SelectedItem.Text;
				moduleSettings.ProductDetail.ShowThumbnail = chkDetailShowThumbnail.Checked.ToString();
				moduleSettings.ProductDetail.ThumbnailWidth = security.InputFilter(txtDetailThumbnailWidth.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting );
                moduleSettings.ProductDetail.ShowReviews = chkDetailShowReviews.Checked.ToString();
                moduleSettings.ProductDetail.ReturnPage = lstPDSReturnPage.SelectedItem.Value;
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion

		#region Private Functions
		private void loadTemplates()
		{
            ArrayList templates = TemplateController.GetTemplates(templatesPath);

			foreach (TemplateInfo templateInfo in templates)
			{
				lstTemplate.Items.Add(new ListItem(templateInfo.Name,  templateInfo.Path));
                lstNPLContainerTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstNPLTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
                lstFPLContainerTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
				lstFPLTemplate.Items.Add(new ListItem(templateInfo.Name,  templateInfo.Path));
                lstPPLContainerTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
				lstPPLTemplate.Items.Add(new ListItem(templateInfo.Name,  templateInfo.Path));
                lstCPLContainerTemplate.Items.Add(new ListItem(templateInfo.Name, templateInfo.Path));
				lstCPLTemplate.Items.Add(new ListItem(templateInfo.Name,  templateInfo.Path));
				lstDetailTemplate.Items.Add(new ListItem(templateInfo.Name,  templateInfo.Path));
			}
		}
		#endregion
	}
}
