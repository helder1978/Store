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
using DotNetNuke.Modules.Store.Providers.Shipping;

namespace DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider
{
	/// <summary>
	/// Summary description for AddressInfo.
	/// </summary>
	public class ShippingInfo : IShippingInfo
	{
		#region Private Members

		private int _id;
        private string _description;
        private decimal _minWeight;
        private decimal _maxWeight;
        private decimal _cost;
		
        #endregion

		#region Constructors
		public ShippingInfo()
		{
		}
		#endregion

		#region Public Properties

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public decimal MinWeight
        {
            get { return _minWeight; }
            set { _minWeight = value; }
        }

        public decimal MaxWeight
        {
            get { return _maxWeight; }
            set { _maxWeight = value; }
        }

        public decimal Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }
		
        #endregion
	}
}
