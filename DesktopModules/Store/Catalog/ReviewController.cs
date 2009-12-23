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
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Summary description for ReviewController.
	/// </summary>
	public class ReviewController
	{
		public enum StatusFilter
		{
			All,
			Approved,
			NotApproved
		}

		#region Constructor
		public ReviewController()
		{
		}
		#endregion

		#region Public Methods

		public ReviewInfo GetReview(int reviewID)
		{
			return (CBO.FillObject(DataProvider.Instance().GetReview(reviewID), typeof(ReviewInfo)) as ReviewInfo);
		}

		public ArrayList GetReviews(int portalID)
		{
			return CBO.FillCollection(DataProvider.Instance().GetReviews(portalID), typeof(ReviewInfo));
		}

		public ArrayList GetReviews(int portalID, StatusFilter filter)
		{
			return GetFilteredList(GetReviews(portalID), filter);
		}

		public ArrayList GetReviewsByProduct(int portalID, int productID, StatusFilter filter)
		{
			return GetFilteredList(CBO.FillCollection(DataProvider.Instance().GetReviewsByProduct(portalID, productID), typeof(ReviewInfo)), filter);
		}

		public ArrayList GetReviewsByCategory(int portalID, int categoryID, StatusFilter filter)
		{
			ProductController productController = new ProductController();
			ArrayList productList = productController.GetCategoryProducts(categoryID, true);

			ArrayList reviewList = new ArrayList();
			foreach(ProductInfo productInfo in productList)
			{
				reviewList.AddRange(GetReviewsByProduct(portalID, productInfo.ProductID, filter));
			}
			return reviewList;
		}

		public void AddReview(ReviewInfo reviewInfo)
		{
			DataProvider.Instance().AddReview(
				reviewInfo.PortalID, 
				reviewInfo.ProductID, 
				reviewInfo.UserID, 
				reviewInfo.UserName, 
				reviewInfo.Rating, 
				reviewInfo.Comments, 
				reviewInfo.Authorized, 
				reviewInfo.CreatedDate);
		}

		public void UpdateReview(ReviewInfo reviewInfo)
		{
			DataProvider.Instance().UpdateReview(
				reviewInfo.ReviewID, 
				reviewInfo.UserName, 
				reviewInfo.Rating, 
				reviewInfo.Comments, 
				reviewInfo.Authorized);
		}

		public void DeleteReview(int reviewID)
		{
			DataProvider.Instance().DeleteReview(reviewID);
		}

		#endregion

		#region Private Methods

		private ArrayList GetFilteredList(ArrayList fullList, StatusFilter filter)
		{
			// Should the list be filtered?
			if (filter == StatusFilter.All)
			{
				return fullList;
			}

			// Create filtered list
			ArrayList filteredList = new ArrayList();
			foreach(ReviewInfo reviewInfo in fullList)
			{
				bool filterByApproved = (filter == StatusFilter.Approved);
				if (filterByApproved == reviewInfo.Authorized)
				{
					filteredList.Add(reviewInfo);
				}
			}

			return filteredList;
		}

		#endregion
	}
}
