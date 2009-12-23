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

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Summary description for ReviewInfo.
	/// </summary>
	public class ReviewInfo
	{
		#region Constructor

		public ReviewInfo()
		{
		}

		#endregion

		#region Private Declarations

		private int _reviewID;
		private int _portalID;
		private int _productID;
		private int _userID;
		private int _rating;
		private string _userName = string.Empty;
		private string _comments = string.Empty;
		private bool _authorized;
		private DateTime _createdDate;
        private string _modelName;

		#endregion

		#region Public Properties

		public int ReviewID 
		{
			get { return _reviewID; }
			set { _reviewID = value; }
		}

		public int PortalID 
		{
			get { return _portalID; }
			set { _portalID = value; }
		}

		public int ProductID 
		{
			get { return _productID; }
			set { _productID = value; }
		}

		public int UserID 
		{
			get { return _userID; }
			set { _userID = value; }
		}

		public int Rating 
		{
			get { return _rating; }
			set { _rating = value; }
		}

		public string UserName 
		{
			get
			{
				if (_userName == string.Empty)
				{
					if (_userID >= 0)
					{
						UserController controller = new UserController();
						UserInfo user = controller.GetUser(this.PortalID, _userID);
						// Need to replace any double apostrophe's because the PortalSecurity.InputFilter 
						// replaces the single apostrophe's when using the PortalSecurity.FilterFlag.NoSQL flag.
						// It is necessary here because the non-admin users have edit rights to this field.
						_userName = user.DisplayName.Replace("''","'");
					}
					else
					{
						_userName = "Guest";
					}
				}
				return _userName;
			}
			set { _userName = value; }
		}

		public string Comments 
		{
			// Need to replace any double apostrophe's because the PortalSecurity.InputFilter 
			// replaces the single apostrophe's when using the PortalSecurity.FilterFlag.NoSQL flag.
			// It is necessary here because the non-admin users have edit rights to this field.
			get { return _comments.Replace("''","'"); }
			set { _comments = value; }
		}

		public bool Authorized 
		{
			get { return _authorized; }
			set { _authorized = value; }
		}

		public DateTime CreatedDate 
		{
			get { return _createdDate; }
			set { _createdDate = value; }
		}

        public string ModelName
        {
            get { return _modelName; }
            set { _modelName = value; }
        }
		#endregion

		#region Object Overrides

		public override bool Equals(object obj) 
		{
			if ((obj == null) || (this.GetType() != obj.GetType())) 
			{
				return false;
			}

			ReviewInfo objInfo = (ReviewInfo) obj;
			return _reviewID.Equals(objInfo.ReviewID);
		}

		public override int GetHashCode() 
		{
			return _reviewID.GetHashCode();
		}

		#endregion
	}
}
