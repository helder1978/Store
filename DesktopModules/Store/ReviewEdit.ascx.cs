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
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	public partial  class ReviewEdit : StoreControlBase
	{
		#region Controls
		protected DotNetNuke.UI.UserControls.LabelControl labelAuthorized;
		#endregion

		#region Private Declarations
		private AdminNavigation _nav;
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
			this.cmdUpdate.Click += new EventHandler(cmdUpdate_Click);
			this.cmdCancel.Click += new EventHandler(cmdCancel_Click);
			this.cmdDelete.Click += new EventHandler(cmdDelete_Click);

		}
		#endregion

		#region Event Handlers

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_nav = new AdminNavigation(Request.QueryString);

			try 
			{
				// Get the Review ID
				ReviewInfo review = new ReviewInfo();

				if (!Page.IsPostBack) 
				{
					cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
					if (!Null.IsNull(_nav.ReviewID)) 
					{
						ReviewController controller = new ReviewController();
						review = controller.GetReview(_nav.ReviewID);
						if (review != null) 
						{							
							cmdDelete.Visible = true;
							txtUserName.Text			= review.UserName;
							cmbRating.SelectedValue		= review.Rating.ToString();
							txtComments.Text			= review.Comments;
							chkAuthorized.Checked		= review.Authorized;
						} 
					}

					plhRating.Controls.Clear();
					plhRating.Controls.Add(GetRatingImages(int.Parse(cmbRating.SelectedValue)));
				}

				// Which controls do we display?
				if (string.Compare(_nav.PageID, "ReviewAdmin", true) == 0)
				{
					txtUserName.Enabled = false;
					cmbRating.Enabled = false;
					labelAuthorized.Visible = true;
					chkAuthorized.Visible = true;
				}
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		protected void cmbRating_SelectedIndexChanged(object sender, EventArgs e)
		{
			plhRating.Controls.Clear();
			plhRating.Controls.Add(GetRatingImages(int.Parse(cmbRating.SelectedValue)));
		}

		private void cmdUpdate_Click(object sender, EventArgs e)
		{
			try 
			{
				if (Page.IsValid == true) 
				{
					PortalSecurity security = new PortalSecurity();

					ReviewInfo review = new ReviewInfo();
					review = ((ReviewInfo)CBO.InitializeObject(review, typeof(ReviewInfo)));
					review.ReviewID					= _nav.ReviewID;
					review.PortalID					= this.PortalId;
					review.ProductID				= _nav.ProductID;
					//review.UserID					= this.UserId;
					review.UserName					= security.InputFilter(txtUserName.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);;
					review.Rating					= int.Parse(cmbRating.SelectedValue);
					review.Comments					= security.InputFilter(txtComments.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);;
					review.Authorized				= chkAuthorized.Checked;
					review.CreatedDate				= DateTime.Now;

					ReviewController controller = new ReviewController();
					if (_nav.ReviewID == 0)
					{
						controller.AddReview(review);
					} 
					else 
					{
						controller.UpdateReview(review);
					}

					invokeEditComplete();
				}
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		private void cmdCancel_Click(object sender, EventArgs e)
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

		private void cmdDelete_Click(object sender, EventArgs e)
		{
			try 
			{
				if (!Null.IsNull(_nav.ReviewID)) 
				{
					ReviewController controller = new ReviewController();
					controller.DeleteReview(_nav.ReviewID);

					_nav.ReviewID = Null.NullInteger;
				}

				invokeEditComplete();
			} 
			catch(Exception ex) 
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion

		#region Private Methods
		private Table GetRatingImages(int rating)
		{
			TableRow row = new TableRow();
			for(int i = 0; i < rating; i++)
			{
				Image image = new Image();
				image.ImageUrl = "~/images/ratingplus.gif";

				TableCell cell = new TableCell();
				cell.Controls.Add(image);

				row.Cells.Add(cell);
			}

			Table table = new Table();
			table.BorderWidth = 0;
			table.CellPadding = 0;
			table.CellSpacing = 0;
			table.Rows.Add(row);

			return table;
		}
		#endregion
	}
}
