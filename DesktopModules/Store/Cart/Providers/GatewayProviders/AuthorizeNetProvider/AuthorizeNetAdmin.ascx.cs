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
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for AuthNetAdmin.
	/// </summary>
	public partial class AuthorizeNetAdmin : StoreControlBase
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

		#region Events

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		#endregion

		#region StoreControlBase Overrides

		public override object DataSource
		{
			get
			{
				PortalSecurity security = new PortalSecurity();

				AuthNetSettings settings = new AuthNetSettings();
				settings.GatewayURL		= security.InputFilter(txtGateway.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
				settings.Version		= security.InputFilter(txtVersion.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
				settings.Username		= security.InputFilter(txtUsername.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
				settings.Password		= security.InputFilter(txtPassword.Text, PortalSecurity.FilterFlag.NoMarkup | PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoSQL);
				settings.IsTest			= cbTest.Checked;
				settings.Capture		= (AuthNetSettings.CaptureTypes)settings.GetCustomType(ddlCapture.SelectedValue);
				base.DataSource			= settings.ToString();

				return base.DataSource;
			}
			set
			{
				base.DataSource = value;

				if (base.DataSource != null)
				{
					string gatewaySettings = base.DataSource as string;
					if (gatewaySettings != null)
					{
						AuthNetSettings settings = new AuthNetSettings(gatewaySettings);
						txtGateway.Text = settings.GatewayURL;
						txtVersion.Text = settings.Version.ToString();
						txtUsername.Text = settings.Username;
						txtPassword.Text = settings.Password;
						cbTest.Checked = settings.IsTest;
						ddlCapture.SelectedValue = settings.Capture.ToString();
					}
				}
			}
		}

		#endregion
	}
}
