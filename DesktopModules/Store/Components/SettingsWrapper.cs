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
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;

namespace DotNetNuke.Modules.Store.Components
{
	public enum SettingsWrapperType
	{
		Module,
		TabModule
	}
	/// <summary>
	/// Summary description for SettingsWrapper.
	/// </summary>
	public abstract class SettingsWrapper
	{
		protected int moduleId;
		protected int tabModuleId;
		protected ModuleController controller;
		protected SettingsWrapperType wrapperType;

		#region Public Properties
		public int ModuleId
		{
			get
			{
				return moduleId;
			}
		}

		public int TabModuleId
		{
			get
			{
				return tabModuleId;
			}
		}

		public SettingsWrapperType WrapperType
		{
			get
			{
				return wrapperType;
			}
		}
		#endregion

		#region Constructors
		public SettingsWrapper(int moduleId)
		{
			this.moduleId = moduleId;
			this.tabModuleId = 0;
			this.wrapperType = SettingsWrapperType.Module;

			this.controller = new ModuleController();
		}

		public SettingsWrapper(int moduleId, int tabId)
		{
			this.moduleId = moduleId;
			this.wrapperType = SettingsWrapperType.TabModule;

			this.controller = new ModuleController();

			ModuleInfo moduleInfo = controller.GetModule(moduleId, tabId);

			if (moduleInfo != null)
			{
				this.tabModuleId = moduleInfo.TabModuleID;
			}
			else
			{
				throw new ApplicationException("ModuleID " + moduleId.ToString() + " does not exist on TabID " + tabId.ToString() + ".");
			}
		}
		#endregion

		#region Protected Functions
		protected string getSetting(MethodBase mb)
		{
			PropertyInfo propertyInfo;
			ModuleSettingAttribute settingInfo = null;
			object setting = null;

			propertyInfo = mb.DeclaringType.GetProperty(mb.Name.Remove(0, 4), BindingFlags.Public | BindingFlags.Instance);
			
			if (propertyInfo != null)
			{
				settingInfo = getPropertyAttribute(propertyInfo);
			}


			if (settingInfo != null)
			{
				switch (wrapperType)
				{
					case SettingsWrapperType.Module:
						setting = controller.GetModuleSettings(moduleId)[settingInfo.Name];
						break;

					case SettingsWrapperType.TabModule:
						setting = controller.GetTabModuleSettings(tabModuleId)[settingInfo.Name];
						break;
				}

				if(setting != null && setting.ToString() != "")
				{
					return setting.ToString();
				}
				else
				{
					if(settingInfo != null)
					{
						return settingInfo.Default;
					}
					else
					{
						return null;
					}
				}
			}
			else
			{
				return null;
			}
		}

		protected void setSetting(MethodBase mb, string settingValue)
		{
			PropertyInfo propertyInfo;
			ModuleSettingAttribute settingInfo = null;

			string propName = mb.Name;
			if (propName.StartsWith("set_"))
			{
				propName = propName.Substring(4);
			}

			propertyInfo = mb.DeclaringType.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
			
			if (propertyInfo != null)
			{
				settingInfo = getPropertyAttribute(propertyInfo);
			}

			if (settingInfo != null)
			{
				if (settingValue == null)
				{
					settingValue = String.Empty;
				}

				switch (wrapperType)
				{
					case SettingsWrapperType.Module:
						controller.UpdateModuleSetting(moduleId, settingInfo.Name, settingValue);
						break;

					case SettingsWrapperType.TabModule:
						controller.UpdateTabModuleSetting(tabModuleId, settingInfo.Name, settingValue);
						break;
				}
			}
		}
		#endregion

		#region Private Functions
		private ModuleSettingAttribute getPropertyAttribute(PropertyInfo propertyInfo)
		{
			object[] attributes;

			if (propertyInfo != null)
			{
				attributes = propertyInfo.GetCustomAttributes(Type.GetType("DotNetNuke.Modules.Store.Components.ModuleSettingAttribute"), false);

				if (attributes.Length > 0)
				{
					return (ModuleSettingAttribute)attributes[0];
				}
				else
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}
		#endregion
	}

	#region Custom Attribute
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
	public class ModuleSettingAttribute : System.Attribute
	{
		private string settingName;
		private string settingDefault;

		public string Name
		{
			get
			{
				return settingName;
			}
		}

		public string Default
		{
			get
			{
				return settingDefault;
			}
		}

		public ModuleSettingAttribute(string name, string defaultValue)
		{
			settingName = name;
			settingDefault = defaultValue;
		}
	}
	#endregion
}
