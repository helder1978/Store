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
using DotNetNuke.Entities.Users;

namespace DotNetNuke.Modules.Store.Customer
{
	/// <summary>
	/// Summary description for CustomerInfo.
	/// </summary>
	public class CustomerInfo
	{
		#region Constructors
		public CustomerInfo()
		{
		}
		#endregion

		#region Private Declarations
		private int _userID;
		private string _userName = string.Empty;
		private string _lastName = string.Empty;
		private string _firstName = string.Empty;
		#endregion

		#region Public Properties

		public int UserID
		{
			get { return _userID; }
			set { _userID = value; }
		}

		public string Username
		{
			get { return _userName; }
			set { _userName = value; }
		}

		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		public string FullName
		{
			get
			{
				string fullName = string.Empty;
				if (_lastName != string.Empty)
				{
					fullName = _lastName;
					if (_firstName != string.Empty)
					{
						fullName += ", " + _firstName;
					}
				}
				return fullName;
			}
		}

		#endregion
	}
}
