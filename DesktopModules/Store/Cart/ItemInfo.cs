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

namespace DotNetNuke.Modules.Store.Cart
{
	/// <summary>
	/// Summary description for ItemInfo.
	/// </summary>
	public class ItemInfo
	{
		private int itemID;
		private string cartID;
		private int productID;
		private string manufacturer;
		private string modelNumber;
		private string modelName;
		private string productImage;
		private decimal unitCost;
		private int quantity;
		private DateTime dateCreated;
        private decimal productWeight;

        private int deliveryMethod;    // 1 - file download, 2 - hard-copy, 3 - both

		public int ItemID
		{
			get {return itemID;}
			set {itemID = value;}
		}

		public string CartID
		{
			get {return cartID;}
			set {cartID = value;}
		}

		public int ProductID
		{
			get {return productID;}
			set {productID = value;}
		}

		public string Manufacturer
		{
			get {return manufacturer;}
			set {manufacturer = value;}
		}

		public string ModelNumber
		{
			get {return modelNumber;}
			set {modelNumber = value;}
		}

		public string ModelName
		{
			get {return modelName;}
			set {modelName = value;}
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
                title += " " + modelName;
                title = title.Trim();
                if (deliveryMethod == 1)
                    title += "(Download PDF)";
                else if (deliveryMethod == 2)
                    title += "(Hard Copy)";
                title = title.Trim();
                return title;
            }
		}

		public string ProductImage
		{
			get {return productImage;}
			set {productImage = value;}
		}

		public decimal UnitCost
		{
			get {return unitCost;}
			set {unitCost = value;}
		}

		public int Quantity
		{
			get {return quantity;}
			set {quantity = value;}
		}

		public DateTime DateCreated
		{
			get {return dateCreated;}
			set {dateCreated = value;}
		}

        public decimal ProductWeight
        {
            get { return productWeight; }
            set { productWeight = value; }
        }

        public int DeliveryMethod
        {
            get { return deliveryMethod; }
            set { deliveryMethod = value; }
        }

        public ItemInfo()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
