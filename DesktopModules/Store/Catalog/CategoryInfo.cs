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

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Summary description for CategoryInfo.
	/// </summary>
	public class CategoryInfo
	{
		#region Constructor

		public CategoryInfo()
		{
		}

		#endregion

		#region Private Declarations

		private int _categoryID;
		private int _portalID;
		private string _categoryName;
		private string _categoryDescription;
		private string _message;
		private bool _archived;
		private string _createdByUser;
		private DateTime _createdDate;
        private int _orderID;
        private int _parentCategoryID;
        private string _parentCategoryName;
        private string _categoryPathName;

		#endregion

		#region Public Properties

		public int CategoryID 
		{
			get { return _categoryID; }
			set { _categoryID = value; }
		}

		public int PortalID 
		{
			get { return _portalID; }
			set { _portalID = value; }
		}

		public string CategoryName 
		{
			get { return _categoryName; }
			set { _categoryName = value; }
		}

		public string CategoryDescription 
		{
			get { return _categoryDescription; }
			set { _categoryDescription = value; }
		}

		public string Message 
		{
			get { return _message; }
			set { _message = value; }
		}

		public bool Archived 
		{
			get { return _archived; }
			set { _archived = value; }
		}

		public string CreatedByUser
		{
			get { return _createdByUser; }
			set { _createdByUser = value; }
		}

		public DateTime CreatedDate
		{
			get { return _createdDate; }
			set { _createdDate = value; }
		}

        public int OrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }

        public int ParentCategoryID
        {
            get { return _parentCategoryID; }
            set { _parentCategoryID = value; }
        }

        public string ParentCategoryName
        {
            get { return _parentCategoryName; }
            set { _parentCategoryName = value; }
        }

        public string CategoryPathName
        {

            get { return _categoryPathName; }
            set { _categoryPathName = value; }
        }

		#endregion

		#region Object Overrides

		public override bool Equals(object obj) 
		{
			if ((obj == null) || (this.GetType() != obj.GetType())) 
			{
				return false;
			}

			CategoryInfo objInfo = (CategoryInfo) obj;
			return _categoryID.Equals(objInfo.CategoryID);
		}

		public override int GetHashCode() 
		{
			return _categoryID.GetHashCode();
		}

		#endregion
	}
}
