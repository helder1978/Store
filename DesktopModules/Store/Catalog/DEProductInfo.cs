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
    /// Summary description for DEProductInfo
	/// </summary>
	public class DEProductInfo
	{
		#region Constructor

        public DEProductInfo()
		{
		}

        public DEProductInfo(int cat1, string catName1, int cat2, string catName2)
        {
            _categoryID1 = cat1;
            _categoryName1 = catName1;
            _categoryID2 = cat2;
            _categoryName2 = catName2;
        }

        #endregion

		#region Private Declarations

		private int _categoryID1;
		private string _categoryName1;
        private int _categoryID2;
        private string _categoryName2;

		#endregion

		#region Public Properties

		public int CategoryID1 
		{
			get { return _categoryID1; }
			set { _categoryID1 = value; }
		}

		public string CategoryName1 
		{
			get { return _categoryName1; }
			set { _categoryName1 = value; }
		}


		public int CategoryID2
		{
			get { return _categoryID2; }
			set { _categoryID2 = value; }
		}

		public string CategoryName2 
		{
			get { return _categoryName2; }
			set { _categoryName2 = value; }
		}

        #endregion

		#region Object Overrides

		public override bool Equals(object obj) 
		{
			if ((obj == null) || (this.GetType() != obj.GetType())) 
			{
				return false;
			}

            DEProductInfo objInfo = (DEProductInfo)obj;
            return _categoryID1.Equals(objInfo.CategoryID1) && _categoryID2.Equals(objInfo.CategoryID2);
		}

		public override int GetHashCode() 
		{
			return _categoryID1.GetHashCode();
		}

		#endregion
	}
}
