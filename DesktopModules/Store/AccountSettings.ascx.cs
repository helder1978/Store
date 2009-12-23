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

namespace DotNetNuke.Modules.Store.WebControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using DotNetNuke;
	using DotNetNuke.Services.Exceptions;
	/// <summary>
	///		Summary description for AccountSettings.
	/// </summary>
	public partial class AccountSettings : Entities.Modules.ModuleSettingsBase
	{
		#region Controls
		protected System.Web.UI.HtmlControls.HtmlTable tblGenSettings;
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

		}
		#endregion

		#region Base Method Implementations
		public override void LoadSettings()
		{
			try
			{
				if (!Page.IsPostBack)
				{
					if (TabModuleSettings.Count > 0)
					{
						string requireSSL = (string)TabModuleSettings["RequireSSL"];
						if ( requireSSL != null && requireSSL.Length > 0 )
						{
							try 
							{
								this.chkRequireSSL.Checked = bool.Parse(requireSSL);
							}
							catch(FormatException ex)
							{
								this.chkRequireSSL.Checked = false;
							}
						}
						else 
						{
							this.chkRequireSSL.Checked = false;
						}
					}
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
                if (Page.IsValid)
				{
                    Entities.Modules.ModuleController objModules = new Entities.Modules.ModuleController();
					
                    objModules.UpdateTabModuleSetting(TabModuleId, "RequireSSL", chkRequireSSL.Checked.ToString());
				}
			}
			catch(Exception ex)
			{
				Exceptions.ProcessModuleLoadException(this, ex);
			}
		}

		#endregion

	}
}
