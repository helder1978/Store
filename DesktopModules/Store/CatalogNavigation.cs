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
using System.Collections.Specialized;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Store.Components;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CatalogNavigation.
	/// </summary>
	public class CatalogNavigation : NavigateWrapper
	{
		#region Constructors

		public CatalogNavigation() : base()
		{
		}

		public CatalogNavigation(NameValueCollection queryString) : base(queryString)
		{
		}

		#endregion

		#region Declarations

        private int _categoryID = Null.NullInteger;
        private int _categoryID2 = Null.NullInteger;
        private int _categoryID3 = Null.NullInteger;
		private int _productID = Null.NullInteger;
        private int _reviewID = Null.NullInteger;
        private int _isTOC = Null.NullInteger;
        private int _pageIndex = Null.NullInteger;
        private String _title = Null.NullString;

		#endregion

		#region Public Properties

		public int CategoryID
		{
			get { return _categoryID; }
			set { _categoryID = value; }
		}

        public int CategoryID2
        {
            get { return _categoryID2; }
            set { _categoryID2 = value; }
        }

        public int CategoryID3
        {
            get { return _categoryID3; }
            set { _categoryID3 = value; }
        }

        public int ProductID
		{
			get { return _productID; }
			set { _productID = value; }
		}

		public int ReviewID
		{
			get { return _reviewID; }
			set { _reviewID = value; }
		}

        public int IsTOC
        {
            get { return _isTOC; }
            set { _isTOC = value; }
        }
        
        public int PageIndex
		{
			get { return _pageIndex; }
			set { _pageIndex = value; }
		}

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

		#endregion
	}
}
