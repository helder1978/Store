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
using DotNetNuke;
using DotNetNuke.UI;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Catalog;
using System.Globalization;

using System.Text.RegularExpressions;
using System.IO;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial class ProductEdit : StoreControlBase
	{
		#region Controls

        //protected DotNetNuke.UI.UserControls.UrlControl image1;
        //protected DotNetNuke.UI.UserControls.UrlControl image2;
        //protected DotNetNuke.UI.UserControls.UrlControl image3;
        protected DotNetNuke.UI.UserControls.UrlControl file1;
        protected DotNetNuke.UI.UserControls.UrlControl preview1;
        protected DotNetNuke.UI.UserControls.TextEditor txtDescription;
        protected DotNetNuke.UI.UserControls.TextEditor txtDescriptionTwo;
        protected DotNetNuke.UI.UserControls.TextEditor txtDescriptionThree;
        protected DotNetNuke.UI.UserControls.TextEditor txtSummary;
        protected DotNetNuke.UI.UserControls.TextEditor txtTOC_Html;
        protected DotNetNuke.UI.UserControls.SectionHeadControl dshSpecialOffer;

        protected TextBox tbFile1;
        protected TextBox tbPreview1;
        protected TextBox tbDescriptionTag;
        protected TextBox tbPriceStr;

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
            this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            this.Load += new System.EventHandler(this.Page_Load);
            this.PreRender += new System.EventHandler(this.Page_PreRender);
        }
		#endregion

		#region Private Declarations

		private CatalogNavigation _nav = null;
		
		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
				// Get the navigation settings
				_nav = new CatalogNavigation(Request.QueryString);
				_nav.ProductID = (int)dataSource;

				// Handle ProductID=0 as Null.NullInteger
				if (_nav.ProductID == 0)
				{
					_nav.ProductID = Null.NullInteger;
				}
                
				if (!Page.IsPostBack) 
				{
					// Load category list
					CategoryController categoryController = new CategoryController();
                    cmbCategory1.DataSource = categoryController.GetCategoriesPath(this.PortalId, true, 4);
                    cmbCategory1.DataBind();
                    cmbCategory2.DataSource = categoryController.GetCategoriesPath(this.PortalId, true, 10);
                    cmbCategory2.DataBind();
                    cmbCategory3.DataSource = categoryController.GetCategoriesPath(this.PortalId, true, 18);
                    cmbCategory3.DataBind();
                    
					// Set delete confirmation
					cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

					// Are we editing or creating new item?
					if (_nav.ProductID != Null.NullInteger)
					{
						ProductController controller = new ProductController();
                        ProductInfo product = controller.GetProduct(_nav.ProductID);

                        if (product != null) 
						{
							cmdDelete.Visible = true;
							//txtManufacturer.Text		    = product.Manufacturer;
							txtModelName.Text			    = product.ModelName;
							txtModelNumber.Text			    = product.ModelNumber;
                            txtSummary.Text                 = product.Summary;
                            //txtSummary2.Text                = product.Summary;
							txtUnitPrice.Text			    = product.UnitCost.ToString("0.00");
                            tbPriceStr.Text                 = product.PriceStr;

                            /*
                            image1.FileFilter = "bmp,png,jpg,jpeg,gif";
                            image1.ShowDatabase = true;
                            image1.ShowFiles = true;
                            image1.ShowLog = false;
                            image1.ShowNewWindow = false;
                            image1.ShowNone = true;
                            image1.ShowSecure = true;
                            image1.ShowTabs = false;
                            image1.ShowTrack = false;
                            image1.ShowUpLoad = true;
                            image1.ShowUrls = true;
                            image1.ShowUsers = false;
                            */
                            /*PrepareImage(image1);

                            if (product.ProductImage.StartsWith("http://"))
                            {
                                image1.UrlType = "U";
                            }
                            else
                            {
                                image1.UrlType = "F";
                            }
                            image1.Url = product.ProductImage;
                            */


                            //cmbCategory.SelectedValue       = product.CategoryID.ToString();
                            cmbCategory1.SelectedValue      = product.CategoryID1.ToString();
                            cmbCategory2.SelectedValue      = product.CategoryID2.ToString();
                            cmbCategory3.SelectedValue      = product.CategoryID3.ToString();
                            txtNumPages.Text                = product.NumPages.ToString();

                            //System.Web.UI.WebControls.Calendar calPublishDate;

                            //calPublishDate.SelectedDate     = product.PublishDate == Null.NullDate ? Null.NullDate : product.PublishDate;
                            ///calPublishDate.SelectedDate     = product.PublishDate;
                            ///calPublishDate.VisibleDate      = product.PublishDate;
                            tbPublishDate.Text              = product.PublishDate.ToString("dd/MM/yyyy");
                            //Response.Write(product.PublishDate.ToString());

                            chkArchived.Checked			    = product.Archived;
							//chkFeatured.Checked			    = product.Featured;
                            txtDescription.Text = product.Description;
                            txtDescriptionTwo.Text = product.DescriptionTwo;
                            txtDescriptionThree.Text = product.DescriptionThree;
                            tbDescriptionTag.Text = product.DescriptionTag;
                            txtTOC_Html.Text = product.TOC_Html;
                            //txtTOC_Html.Text = RemoveHTML( product.Description );

                            //Response.Write("<br>before {" + product.ProductFile + "}{" + file1.Url + "}");
                            file1 = PrepareFile(file1);
                            if (product.ProductFile == Null.NullString) product.ProductFile = "_no_report_file_available.pdf";
                            SetFileUrl(file1, "products654654/", product.ProductFile);
                            //Response.Write("<br>after {" + product.ProductFile + "}{" + file1.Url + "}");

                            //Response.Write("<br>preview before {" + product.ProductPreview + "}{" + preview1.Url + "}");
                            preview1 = PrepareFile(preview1);
                            if (product.ProductPreview == Null.NullString) product.ProductPreview = "_no_report_file_available.pdf";
                            SetFileUrl(preview1, "documents/", product.ProductPreview);
                            //Response.Write("<br>preview after {" + product.ProductPreview + "}{" + preview1.Url + "}");
                            
                            tbFile1.Text = product.ProductFile;
                            tbPreview1.Text = product.ProductPreview;

                            //txtDescription2.Text            = product.Description;
                            //txtUnitWeight.Text              = product.ProductWeight == Null.NullDecimal ? string.Empty : product.ProductWeight.ToString("0.00");
                            //txtUnitHeight.Text              = product.ProductHeight == Null.NullDecimal ? string.Empty : product.ProductHeight.ToString("0.00");
                            //txtUnitLength.Text              = product.ProductLength == Null.NullDecimal ? string.Empty : product.ProductLength.ToString("0.00");
                            //txtUnitWidth.Text               = product.ProductWidth == Null.NullDecimal ? string.Empty : product.ProductWidth.ToString("0.00");
                            calSaleStartDate.SelectedDate   = product.SaleStartDate == Null.NullDate ? Null.NullDate : product.SaleStartDate;
                            calSaleEndDate.SelectedDate     = product.SaleEndDate == Null.NullDate ? Null.NullDate : product.SaleEndDate;
                            txtSalePrice.Text               = product.SalePrice == Null.NullDecimal ? string.Empty : product.SalePrice.ToString("0.00");

                            //Response.Write("AvailableONlie: " + product.AvailableOnline);
                            chkAvailableOnline.Checked = product.AvailableOnline;

                            if (calSaleStartDate.SelectedDate != Null.NullDate)
                            {
                                calSaleStartDate.VisibleDate = calSaleStartDate.SelectedDate;
                            }

                            if (calSaleEndDate.SelectedDate != Null.NullDate)
                            {
                                calSaleEndDate.VisibleDate = calSaleEndDate.SelectedDate;
                            }

                            Session["ok"] = true;
						} 
						else 
						{
							// Handle as new item
							PrepareNew();
						}
					}
					else
					{
						// Handle as new item
						PrepareNew();
					}
				}

            } 
			catch(Exception ex) 
			{
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
                Response.Write(ex.InnerException);
                Response.Write(ex.GetBaseException().Message);


                // Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

        protected string RemoveHTML( string strText )
        {
            strText = Regex.Replace( Server.HtmlDecode(strText), "<[^>]*>", "");
            return strText;
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            //imgPublishDate.Enabled = true;
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }
            imgPublishDate.Visible = true;

            //    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
            //        EnableScriptLocalization="true" ID="ScriptManager1" />
        }

        protected void btnClearStartDate_Click(object sender, EventArgs e)
        {
            calSaleStartDate.SelectedDate = Null.NullDate;
            dshSpecialOffer.IsExpanded = true;
        }

        protected void btnClearEndDate_Click(object sender, EventArgs e)
        {
            calSaleEndDate.SelectedDate = Null.NullDate;
            dshSpecialOffer.IsExpanded = true;
        }

        /*
        protected void btnClearPublishDate_Click(object sender, EventArgs e)
        {
            calPublishDate.SelectedDate = Null.NullDate;
        }
        */

        protected void calSaleStartDate_SelectionChanged(object sender, EventArgs e)
        {
            dshSpecialOffer.IsExpanded = true;
        }

        protected void calSaleEndDate_SelectionChanged(object sender, EventArgs e)
        {
            dshSpecialOffer.IsExpanded = true;
        }

        protected void calSaleStartDate_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            dshSpecialOffer.IsExpanded = true;
        }

        protected void calSaleEndDate_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            dshSpecialOffer.IsExpanded = true;
        }

        protected void txtSalePrice_TextChanged(object sender, EventArgs e)
        {
            dshSpecialOffer.IsExpanded = true;
        }

		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			try 
			{
				if (Page.IsValid == true) 
				{
					PortalSecurity security = new PortalSecurity();

                    ProductInfo product = new ProductInfo();
					product = ((ProductInfo)CBO.InitializeObject(product, typeof(ProductInfo)));

					product.ProductID		= _nav.ProductID;
					product.PortalID		= this.PortalId;
                    product.Manufacturer    = ""; // security.InputFilter(txtManufacturer.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.MultiLine);
					product.ModelName		= security.InputFilter(txtModelName.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.MultiLine);
					product.ModelNumber		= security.InputFilter(txtModelNumber.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.MultiLine);
					//product.Summary			= security.InputFilter(txtSummary.Text, PortalSecurity.FilterFlag.MultiLine);
                    product.Summary         = security.InputFilter(txtSummary.Text, PortalSecurity.FilterFlag.NoScripting);
					product.UnitCost		= Decimal.Parse(txtUnitPrice.Text);
                    product.PriceStr        = tbPriceStr.Text;
                    //product.ProductImage	= GetImageUrl(image1);
                    product.CategoryID      = 4; // int.Parse(cmbCategory.SelectedValue);
					product.Archived		= chkArchived.Checked;
                    product.Featured        = false;  // chkFeatured.Checked;
                    product.Description     = security.InputFilter(txtDescription.Text, PortalSecurity.FilterFlag.NoScripting);
                    product.DescriptionTwo  = security.InputFilter(txtDescriptionTwo.Text, PortalSecurity.FilterFlag.NoScripting);
                    product.DescriptionThree = security.InputFilter(txtDescriptionThree.Text, PortalSecurity.FilterFlag.NoScripting);
                    product.CreatedByUser = this.UserId.ToString();
					product.CreatedDate		= DateTime.Now;
                    product.ProductWeight   = 0; //txtUnitWeight.Text.Length > 0 ? Decimal.Parse(txtUnitWeight.Text) : -1;
                    product.ProductHeight   = 0; //txtUnitHeight.Text.Length > 0 ? Decimal.Parse(txtUnitHeight.Text) : -1;
                    product.ProductLength   = 0; //txtUnitLength.Text.Length > 0 ? Decimal.Parse(txtUnitLength.Text) : -1;
                    product.ProductWidth    = 0; //txtUnitWidth.Text.Length > 0 ? Decimal.Parse(txtUnitWidth.Text) : -1;
                    product.SaleStartDate   = calSaleStartDate.SelectedDate != Null.NullDate ? calSaleStartDate.SelectedDate : DateTime.Parse("01/01/9999");
                    product.SaleEndDate     = calSaleEndDate.SelectedDate != Null.NullDate ? calSaleEndDate.SelectedDate : DateTime.Parse("01/01/9999");
                    product.SalePrice       = txtSalePrice.Text.Length > 0 ? Decimal.Parse(txtSalePrice.Text) : -1;

                    // canadean changed: added new parameters
                    product.CategoryID1 = int.Parse(cmbCategory1.SelectedValue);
                    product.CategoryID2 = int.Parse(cmbCategory2.SelectedValue);
                    product.CategoryID3 = int.Parse(cmbCategory3.SelectedValue);
                    product.NumPages = txtNumPages.Text.Length > 0 ? int.Parse(txtNumPages.Text) : -1;
                    ///product.PublishDate = calPublishDate.SelectedDate != Null.NullDate ? calPublishDate.SelectedDate : DateTime.Parse("01/01/9999");

                    CultureInfo ukCulture = new CultureInfo("en-GB");
                    product.PublishDate = tbPublishDate.Text != "" ? DateTime.Parse(tbPublishDate.Text, ukCulture.DateTimeFormat) : DateTime.Parse("01/01/9999");
                    //product.DeliveryMethod = txtDeliveryMethod.Text.Length > 0 ? int.Parse(txtDeliveryMethod.Text) : -1;
                    product.DeliveryMethod = int.Parse(cmbDeliveryMethod.SelectedValue);
                    //product.AvailableOnline = true; // chkAvailableOnline.Checked;
                    product.AvailableOnline = chkAvailableOnline.Checked;
                    //product.ProductImage2 = GetImageUrl(image2);
                    //product.ProductImage3 = GetImageUrl(image3);
                    product.DescriptionTag = tbDescriptionTag.Text;
                    product.TOC_Html = txtTOC_Html.Text;

                    product.ProductFile = GetImageUrl(file1);
                    product.ProductPreview = GetImageUrl(preview1);
                    
                    //product.ProductFile = tbFile1.Text;
                    //product.ProductPreview = tbPreview1.Text;

                    calSaleStartDate.SelectedDate = product.SaleStartDate == Null.NullDate ? Null.NullDate : product.SaleStartDate;
                    calSaleEndDate.SelectedDate = product.SaleEndDate == Null.NullDate ? Null.NullDate : product.SaleEndDate;
                    txtSalePrice.Text = product.SalePrice == Null.NullDecimal ? string.Empty : product.SalePrice.ToString("0.00");

					ProductController controller = new ProductController();
					if (Null.IsNull(product.ProductID))
					{
						controller.AddProduct(product);
					} 
					else 
					{
                        // Ajouté pour contourner le bug System.NullReferenceException
                        if (product.ProductImage.StartsWith("http:///"))
                        {
                            product.ProductImage = product.ProductImage.Replace("http://", "");
                        }
                        if (product.ProductImage2.StartsWith("http:///"))
                        {
                            product.ProductImage2 = product.ProductImage2.Replace("http://", "");
                        }
                        if (product.ProductImage3.StartsWith("http:///"))
                        {
                            product.ProductImage3 = product.ProductImage3.Replace("http://", "");
                        }
                        if (product.ProductFile.StartsWith("http:///"))
                        {
                            product.ProductFile = product.ProductFile.Replace("http://", "");
                        }
                        if (product.ProductPreview.StartsWith("http:///"))
                        {
                            product.ProductPreview = product.ProductPreview.Replace("http://", "");
                        }

                        controller.UpdateProduct(product);
					}

					invokeEditComplete();
				}
			} 
			catch(Exception ex) 
			{
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);

				//Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			try 
			{
				_nav.ProductID = Null.NullInteger;
				invokeEditComplete();
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		protected void cmdDelete_Click(object sender, EventArgs e)
		{
			if (!Null.IsNull(_nav.ProductID)) 
			{
				ProductController controller = new ProductController();
				controller.DeleteProduct(_nav.ProductID);

				_nav.ProductID = Null.NullInteger;
			}

			invokeEditComplete();
		}

		#endregion

		#region Private Methods

		private void PrepareNew()
		{
			// Do we have a catetory to use as a default?
            /*
			if (_nav.CategoryID != Null.NullInteger)
			{
				cmbCategory.SelectedValue = _nav.CategoryID.ToString();
            }
            */
            /*
            image1.FileFilter = "bmp,png,jpg,jpeg,gif";
            image1.ShowDatabase = true;
            image1.ShowFiles = true;
            image1.ShowLog = false;
            image1.ShowNewWindow = false;
            image1.ShowNone = true;
            image1.ShowSecure = true;
            image1.ShowTabs = false;
            image1.ShowTrack = false;
            image1.ShowUpLoad = true;
            image1.ShowUrls = true;
            image1.ShowUsers = false;
            image1.UrlType = "F";
            */
            //PrepareImage(image1);
            //PrepareImage(image2);
            //PrepareImage(image3);
            //PrepareFile(file1);
            //PrepareFile(preview1);

            txtUnitPrice.Text = (0D).ToString("0.00");
            //txtUnitWeight.Text = (0D).ToString("0.00");
            //txtUnitHeight.Text = (0D).ToString("0.00");
            //txtUnitLength.Text = (0D).ToString("0.00");
            //txtUnitWidth.Text = (0D).ToString("0.00");
            txtSalePrice.Text = (0D).ToString("0.00");
		}

        private UrlControl PrepareImage(DotNetNuke.UI.UserControls.UrlControl image)
        {
            image.FileFilter = "bmp,png,jpg,jpeg,gif";
            //image.ShowDatabase = true;
            image.ShowFiles = true;
            //image.ShowLog = false;
            //image.ShowNewWindow = false;
            image.ShowNone = true;
            //image.ShowSecure = true;
            //image.ShowTabs = false;
            //image.ShowTrack = false;
            image.ShowUpLoad = true;
            //image.ShowUrls = true;
            //image.ShowUsers = false;
            image.UrlType = "F";
            return image;
        }

        private UrlControl PrepareFile(DotNetNuke.UI.UserControls.UrlControl file)
        {
            file.FileFilter = "pdf,doc,xls,zip,rar";
            //file.ShowDatabase = true;
            file.ShowFiles = true;
            file.ShowLog = false;
            file.ShowNewWindow = false;
            file.ShowNone = true;
            //file.ShowSecure = true;
            file.ShowTabs = false;
            file.ShowTrack = false;
            file.ShowUpLoad = true;
            file.ShowUrls = true;
            //file.ShowUsers = false;
            file.UrlType = "F";
            return file;
        }

        private string GetImageUrl(DotNetNuke.UI.UserControls.UrlControl image)
		{
            
            string imagePath = string.Empty;

			// Is this an internal image?
            if (image.UrlType == "F")
            {
                DropDownList cboFolders = (DropDownList) image.FindControl("cboFolders");
                DropDownList cboFiles = (DropDownList) image.FindControl("cboFiles");

                //imagePath = PortalSettings.HomeDirectory;
                //imagePath += cboFolders.SelectedValue + cboFiles.SelectedItem.Text;
                imagePath += cboFiles.SelectedItem.Text;
            }
            else
            {
                imagePath = image.Url;
            }

			return imagePath;
		}

        private void SetFileUrl(DotNetNuke.UI.UserControls.UrlControl file, string folder, string fileName)
        {

            string imagePath = string.Empty;

            // Is this an internal image?
            /*
            if (file.UrlType == "F")
            {
                DropDownList cboFolders = (DropDownList)file.FindControl("cboFolders");
                DropDownList cboFiles = (DropDownList)file.FindControl("cboFiles");
                cboFolders.SelectedIndex = 5;
                cboFiles.Text = fileName;
                Response.Write("<br>here: " + file.ClientID + " " + cboFolders.SelectedIndex + " " + cboFiles.Text);

                ((DropDownList)file.FindControl("cboFolders")).SelectedIndex = 5;
                Response.Write("<br>here2: " + file.ClientID + " " + ((DropDownList)file.FindControl("cboFolders")).SelectedIndex);
            
            }
            else*/
            {
                //file.Url = PortalSettings.HomeDirectory + folder + fileName;
                file.Url = folder + fileName;
            }

        }

        /*
        private void setSelectedItem(DropDownList dd, string itemValue)
        {
            //dd.Items
            foreach(ListIt)
            {if(item.) }
        }
        */


        #endregion
    }
}

