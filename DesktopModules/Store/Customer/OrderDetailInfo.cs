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

namespace DotNetNuke.Modules.Store.Customer
{
  /// <summary>
  /// Summary description for AddressInfo.
  /// </summary>
  public class OrderDetailsInfo 
  {
    // Entity member variables for database table DotNetNukeStore_OrderDetails
    private Int32 mOrderID;
    private Int32 mProductID;
    private Int32 mQuantity;
    private decimal mUnitCost;
    private decimal mExtendedAmount;
    private string mModelName;
    private string mModelNumber;
    private Int32 mProdDeliveryMethod;
    private string mProdReference;
    private string mProdName;
    private decimal mProdCost;

    // Calculated Expression member variables for database table DotNetNukeStore_OrderDetails

    // Entity properties for database table DotNetNukeStore_OrderDetails
    public Int32 OrderID 
    {
      get { return mOrderID; }
      set { mOrderID = value; }
    }

    public Int32 ProductID 
    {
      get { return mProductID; }
      set { mProductID = value; }
    }

    public Int32 Quantity 
    {
      get { return mQuantity; }
      set { mQuantity = value; }
    }

    public decimal UnitCost 
    {
      get { return mUnitCost; }
      set { mUnitCost = value; }
    }

    public decimal ExtendedAmount 
    {
      get { return mExtendedAmount; }
      set { mExtendedAmount = value; }
    }

    public string ModelName 
    {
      get { return mModelName; }
      set { mModelName = value; }
    }

    public string ModelNumber 
    {
      get { return mModelNumber; }
      set { mModelNumber = value; }
    }

    public Int32 ProdDeliveryMethod
    {
        get { return mProdDeliveryMethod; }
        set { mProdDeliveryMethod = value; }
    }

    public decimal ProdCost
    {
        get { return mProdCost; }
        set { mProdCost = value; }
    }

    public string ProdReference
    {
        get { return mProdReference; }
        set { mProdReference = value; }
    }

    public string ProdName
    {
        get { return mProdName; }
        set { mProdName = value; }
    }

    // Equal and HashCode methods for easy compare in lists

    public override bool Equals(object obj) 
    {
      if ((obj == null) || (this.GetType() != obj.GetType())) 
      {
        return false;
      }
      OrderDetailsInfo objInfo = (OrderDetailsInfo) obj;
      return mOrderID.Equals(objInfo.OrderID) && mProductID.Equals(objInfo.ProductID);
    }

    public override Int32 GetHashCode() 
    {
      return mOrderID.GetHashCode() ^ mProductID.GetHashCode();
    }
  }
}
