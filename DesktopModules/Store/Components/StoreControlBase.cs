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
using System.IO;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.Store.Components
{
	/// <summary>
	/// Summary description for StoreControlBase.
	/// </summary>
	public class StoreControlBase : PortalModuleBase
	{
		#region Private Declarations
		protected PortalModuleBase parentControl = new PortalModuleBase();
		protected object dataSource = new object();
		#endregion

		#region Constructors
		public StoreControlBase()
		{
		}
		#endregion
		
		#region PortalModuleBase Overrides
		protected override void OnLoad(EventArgs e)
		{
            try
            {
                //			this.LocalResourceFile = this.TemplateSourceDirectory + "/" +
                //				DotNetNuke.Services.Localization.Localization.LocalResourceDirectory + "/" +
                //				this.GetType().BaseType.Name;
                this.LocalResourceFile = Services.Localization.Localization.GetResourceFile(this, this.GetType().BaseType.Name + ".ascx");
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
		}
		#endregion

		#region Events
		public event EventHandler EditComplete;

		protected void invokeEditComplete()
		{
			if (EditComplete != null)
			{
				EditComplete(this, new EventArgs());
			}
		}
		#endregion

		#region Public Properties
		public PortalModuleBase ParentControl
		{
			get {return parentControl;}
			set {parentControl = value;}
		}

		public int StoreControlID
		{
			get 
			{
				if(Request.QueryString["scid"] != null)
				{
					return Convert.ToInt32(Request.QueryString["scid"]);
				}
				else
				{
					return 1;
				}
			}
		}

		public virtual object DataSource
		{
			get {return dataSource;}
			set {dataSource = value;}
		}

		public virtual bool IsValid
		{
			get { return true; }
		}

		#endregion
	}
}
