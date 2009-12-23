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
using System.Reflection;
using System.Runtime.CompilerServices;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for Defaults.
	/// </summary>
	public class ModuleSettings
	{
		public MiniCartSettings MiniCart;
		public MainCartSettings MainCart;

		public ModuleSettings(int moduleId, int tabId)
		{
			MiniCart = new MiniCartSettings(moduleId, tabId);
			MainCart = new MainCartSettings(moduleId, tabId);
		}
	}

	#region Mini Cart Settings
	public class MiniCartSettings : SettingsWrapper
	{
		[ModuleSetting("minicartshowthumbnail", "false")]
		public string ShowThumbnail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("minicartthumbnailwidth", "50")]
		public string ThumbnailWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		public MiniCartSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}
	}
	#endregion

	#region Main Cart Settings Settings
	public class MainCartSettings : SettingsWrapper
	{
		[ModuleSetting("maincartshowthumbnail", "true")]
		public string ShowThumbnail
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		[ModuleSetting("maincartthumbnailwidth", "100")]
		public string ThumbnailWidth
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				return getSetting(m);
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				MethodBase m = MethodBase.GetCurrentMethod();
				setSetting(m, value);
			}
		}

		public MainCartSettings(int moduleId, int tabId) : base(moduleId, tabId)
		{
		}
	}
	#endregion
}
