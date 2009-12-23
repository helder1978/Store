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
using System.Text;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Summary description for ProductInfo.
	/// </summary>
    /// 
    [Serializable]
    public class ProductInfo : IComparable
	{
		#region Constructor

		public ProductInfo()
		{
		}

		#endregion

		#region Private Declarations

		private int _productID;
		private int _portalID;
		private int _categoryID;
		private string _manufacturer;
		private string _modelName;
		private string _modelNumber;
		private string _productImage;
		private decimal _unitCost;
		private string _summary;
		private string _description;
		private bool _archived;
		private bool _featured;
		private string _createdByUser;
		private DateTime _createdDate;
        private decimal _productWeight;
        private decimal _productHeight;
        private decimal _productLength;
        private decimal _productWidth;
        private DateTime _saleStartDate;
        private DateTime _saleEndDate;
        private decimal _salePrice;
        private decimal _VATPrice;


        // canadean changed: added new properties
        private int _categoryID1;
        private int _categoryID2;
        private int _categoryID3;
        private int _numPages;
        private DateTime _publishDate;
        private int _deliveryMethod;    // 1 - file download, 2 - hard-copy, 3 - both
        private bool _availableOnline;    // 1 - yes, 0 - no
        private string _productFile;
        private string _productPreview;
        private string _productImage2;
        private string _productImage3;
        private string _descriptionTag;
        private string _TOC_Html;
        private string _PriceStr;
        private string _descriptionTwo;
        private string _descriptionThree;


		#endregion

		#region Public Properties

		public int ProductID
		{
			get { return _productID; }
			set { _productID = value; }
		}

		public int PortalID
		{
			get { return _portalID; }
			set { _portalID = value; }
		}

		public int CategoryID
		{
			get { return _categoryID; }
			set { _categoryID = value; }
		}

		public string Manufacturer
		{
			get { return _manufacturer; }
			set { _manufacturer = value; }
		}

		public string ModelName
		{
			get { return _modelName; }
			set { _modelName = value; }
		}

		public string ModelNumber
		{
			get { return _modelNumber; }
			set { _modelNumber = value; }
		}
		
		public string ProductImage
		{
			get { return _productImage; }
			set { _productImage = value; }
		}

		public decimal UnitCost
		{
			get { return _unitCost; }
			set { _unitCost = value; }
		}

		public string Summary
		{
			get { return _summary; }
			set { _summary = value; }
		}
		
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public bool Archived
		{
			get { return _archived; }
			set { _archived = value; }
		}

		public bool Featured
		{
			get { return _featured; }
			set { _featured = value; }
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

		public string ProductTitle
		{
			get
			{
				string title = string.Empty;
				/*
                title += _manufacturer;
				title += " " + _modelName;
				title = title.Trim();
				title += " " + _modelNumber;
				title = title.Trim();
                */
                title += " " + _modelName;
                title = title.Trim();
                if(_deliveryMethod == 1)
                    title += "(Download PDF)";
                else if (_deliveryMethod == 2)
                        title += "(Hard Copy)";
                title = title.Trim();
                return title;
			}
		}

        public decimal ProductWeight
        {
            get { return _productWeight; }
            set { _productWeight = value; }
        }

        public decimal ProductHeight
        {
            get { return _productHeight; }
            set { _productHeight = value; }
        }

        public decimal ProductLength
        {
            get { return _productLength; }
            set { _productLength = value; }
        }

        public decimal ProductWidth
        {
            get { return _productWidth; }
            set { _productWidth = value; }
        }

        public DateTime SaleStartDate
        {
            get { return _saleStartDate; }
            set { _saleStartDate = value; }
        }

        public DateTime SaleEndDate
        {
            get { return _saleEndDate; }
            set { _saleEndDate = value; }
        }

        public decimal SalePrice
        {
            get { return _salePrice; }
            set { _salePrice = value; }
        }

        public decimal VATPrice
        {
            get { return _VATPrice; }
            set { _VATPrice = value; }
        }

        // canadean changed: added new product access methods
        public int CategoryID1
        {
            get { return _categoryID1; }
            set { _categoryID1 = value; }
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

        public int NumPages
        {
            get { return _numPages; }
            set { _numPages = value; }
        }

        public DateTime PublishDate
        {
            get { return _publishDate; }
            set { _publishDate = value; }
        }

        public int DeliveryMethod
        {
            get { return _deliveryMethod; }
            set { _deliveryMethod = value; }
        }

        public bool AvailableOnline
        {
            get { return _availableOnline; }
            set { _availableOnline = value; }
        }

        public string ProductFile
        {
            get { return _productFile; }
            set { _productFile = value; }
        }

        public string ProductPreview
        {
            get { return _productPreview; }
            set { _productPreview = value; }
        }

        public string ProductImage2
        {
            get { return _productImage2; }
            set { _productImage2 = value; }
        }

        public string ProductImage3
        {
            get { return _productImage3; }
            set { _productImage3 = value; }
        }

        public string DescriptionTag
        {
            get { return _descriptionTag; }
            set { _descriptionTag = value; }
        }

        public string TOC_Html
        {
            get { return _TOC_Html; }
            set { _TOC_Html = value; }
        }

        public string PriceStr
        {
            get { return _PriceStr; }
            set { _PriceStr = value; }
        }

        public string DescriptionTwo
        {
            get { return _descriptionTwo; }
            set { _descriptionTwo = value; }
        }

        public string DescriptionThree
        {
            get { return _descriptionThree; }
            set { _descriptionThree = value; }
        }

        #endregion

		#region Object Overrides

		public override bool Equals(object obj) 
		{
			if ((obj == null) || (this.GetType() != obj.GetType())) 
			{
				return false;
			}

			ProductInfo objInfo = (ProductInfo) obj;
			return _productID.Equals(objInfo.ProductID);
		}

		public override int GetHashCode() 
		{
			return _productID.GetHashCode();
		}

		#endregion

        #region IComparable Interface
        public int CompareTo(object obj)
        {
            ProductInfo product = (ProductInfo)obj;
            return this.ModelName.CompareTo(product.ModelName);
        } 
        #endregion
    }
}
