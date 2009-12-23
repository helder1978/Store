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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial  class CategoryEdit : StoreControlBase
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

		}
		#endregion

		#region Controls

		//protected DotNetNuke.UI.UserControls.TextEditor txtMessage;
		
		#endregion

		#region Private Members

		private int _categoryID = Null.NullInteger;
		
		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, System.EventArgs e)
		{
				// Get the Category ID
				CategoryInfo category = new CategoryInfo();
				_categoryID = (int)dataSource;

				if (!Page.IsPostBack) 
				{
					cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
					if (!Null.IsNull(_categoryID)) 
					{
						CategoryController controller = new CategoryController();

                        //Bind parent category list...
                        ddlParentCategory.DataSource = controller.GetCategoriesPath(PortalId, true, -1);
                        ddlParentCategory.DataTextField = "CategoryPathName";
                        ddlParentCategory.DataValueField = "CategoryID";
                        ddlParentCategory.DataBind();
                        ddlParentCategory.Items.Insert(0, new ListItem(Localization.GetString("None", this.LocalResourceFile), "-1"));

						category = controller.GetCategory(_categoryID);
						if (category != null) 
						{							
							cmdDelete.Visible = true;

							txtCategoryName.Text	= category.CategoryName;
							//txtDescription.Text		= category.CategoryDescription;
							//chkArchived.Checked		= category.Archived;
							//txtMessage.Text			= category.Message;
                            //txtOrder.Text           = category.OrderID.ToString();
                            if (category.ParentCategoryID != Null.NullInteger
                                && category.ParentCategoryID > 0)
                            {
                                if (ddlParentCategory.Items.FindByValue(category.ParentCategoryID.ToString()) != null)
                                {
                                    ddlParentCategory.ClearSelection();
                                    ddlParentCategory.Items.FindByValue(category.ParentCategoryID.ToString()).Selected = true;
                                }
                            }
						} 
					}
				}
		}

		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			//try 
			//{
            
				if (Page.IsValid == true) 
				{
					PortalSecurity security = new PortalSecurity();
                    CategoryController controller = new CategoryController();
                    CategoryInfo existingCategory = controller.GetCategory(_categoryID);

					CategoryInfo category = new CategoryInfo();
					category = ((CategoryInfo)CBO.InitializeObject(category, typeof(CategoryInfo)));
					category.CategoryID				= _categoryID;
					category.PortalID				= this.PortalId;
					category.CategoryName			= security.InputFilter(txtCategoryName.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting);
                    category.CategoryDescription    = category.CategoryName; // security.InputFilter(txtDescription.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting);
                    category.Archived               = false; // chkArchived.Checked;
                    category.Message                = ""; // txtMessage.Text;
					category.CreatedByUser			= this.UserId.ToString();
					category.CreatedDate			= DateTime.Now;
                    
                    /*
                    try
                    {
                        category.OrderID = Convert.ToInt32(txtOrder.Text);
                    }
                    catch (ArgumentException)
                    {
                        category.OrderID = existingCategory.OrderID;
                    }
                    */
                    category.OrderID = 0;

                    try
                    {
                        category.ParentCategoryID = Convert.ToInt32(ddlParentCategory.SelectedItem.Value);
                    }
                    catch (ArgumentException)
                    {
                        category.ParentCategoryID = existingCategory.ParentCategoryID;
                    }

					if (_categoryID == 0)
					{
                        //No recursion check needed...
						controller.AddCategory(category);
                        invokeEditComplete();
					} 
					else 
					{
                        //Recursion Check...
                        if (category.ParentCategoryID != Null.NullInteger && !RecursionCheckPassed(category.CategoryID, category.ParentCategoryID))
                        {
                            lblRecursionWarning.Visible = true;
                        }
                        else
                        {
                            lblRecursionWarning.Visible = false;
                            controller.UpdateCategory(category);
                            invokeEditComplete();
                        }
					}
				}
                
			//} 
			//catch(Exception ex) 
			//{
			//	Exceptions.ProcessModuleLoadException(this, ex);
			//}
		}

        private bool RecursionCheckPassed(int CategoryID, int ParentCategoryID)
        {
            //Checks for recursive parent/child categories...
            if (CategoryID == ParentCategoryID)
            {
                return false;
            }

            CategoryController controller = new CategoryController();
            ArrayList categoryTree = new ArrayList();
            categoryTree.Add(CategoryID);
            categoryTree.Add(ParentCategoryID);

            CategoryInfo category = controller.GetCategory(ParentCategoryID);

            while (category.ParentCategoryID > 0)
            {
                foreach (int i in categoryTree)
                {
                    if (i == category.ParentCategoryID)
                    {
                        return false;
                    }
                }
                category = controller.GetCategory(category.ParentCategoryID);
            }
            return true;
            
        }

		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			try 
			{
				invokeEditComplete();
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		protected void cmdDelete_Click(object sender, EventArgs e)
		{
			try 
			{
				if (!Null.IsNull(_categoryID)) 
				{
					CategoryController controller = new CategoryController();
					controller.DeleteCategory(_categoryID);

					_categoryID = Null.NullInteger;
				}

				invokeEditComplete();
			} 
			catch(Exception ex) 
			{
                if (ex.InnerException != null)
                {
                    Exceptions.ProcessModuleLoadException(this, ex.InnerException);
                }
                else
                {
                    string ErrorDelete = Localization.GetString("ErrorDelete", this.LocalResourceFile);
                    Exceptions.ProcessModuleLoadException(ErrorDelete, this, ex);
                }
			}
		}

		#endregion
	}
}
