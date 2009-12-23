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
using System.Xml;
using System.Text;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.Store.Catalog
{
	/// <summary>
	/// Summary description for CategoryController.
	/// </summary>
    public class CategoryController : PortalModuleBase, Entities.Modules.IPortable
	{
		#region Constructor

		public CategoryController()
		{
		}

		#endregion

        #region Private Methods =========================================

        private string CreatePath(int ParentId, string CategoryName)
        {
            string strpath = CategoryName;
            CategoryInfo nfocategory;
            while (ParentId > 0)
            {
                nfocategory = new CategoryInfo();
                nfocategory = GetCategory(ParentId);
                strpath = nfocategory.CategoryName + " > " + strpath;
                ParentId = nfocategory.ParentCategoryID;
            }
            return strpath;
        }

        #endregion

		#region Public Methods

        public CategoryInfo GetCategoryPath(int categoryId)
        {
            CategoryInfo nfocategory = new CategoryInfo();
            nfocategory = GetCategory(categoryId);
            if (nfocategory.ParentCategoryID > 0)
            {
                nfocategory.CategoryPathName = CreatePath(nfocategory.ParentCategoryID, nfocategory.CategoryName);
            }
            else
            {
                nfocategory.CategoryPathName = nfocategory.CategoryName;
            }
            return nfocategory;
        }

        public ArrayList GetCategoriesPath(int portalID, bool includeArchived, int ParentCategoryID)
        {
            ArrayList arrCategories = new ArrayList();

            arrCategories = GetCategories(portalID, includeArchived, ParentCategoryID);
            foreach (CategoryInfo nfocategory in arrCategories)
            {
                if (nfocategory.ParentCategoryID > 0)
                {
                    nfocategory.CategoryPathName = CreatePath(nfocategory.ParentCategoryID, nfocategory.CategoryName);
                }
                else
                {
                    nfocategory.CategoryPathName = nfocategory.CategoryName;
                }
            }
            arrCategories.Sort(new CategroyInfoCompare());
            return arrCategories;
        }

        public CategoryInfo GetCategory(int categoryID)
		{
			return (CBO.FillObject(DataProvider.Instance().GetCategory(categoryID), typeof(CategoryInfo)) as CategoryInfo);
		}

		public ArrayList GetCategories(int portalID, bool includeArchived, int ParentCategoryID)
		{
			return CBO.FillCollection(DataProvider.Instance().GetCategories(portalID, includeArchived, ParentCategoryID), typeof(CategoryInfo));
		}

        public ArrayList GetCategoriesFromProducts(int PortalID, int CategoryID1, int CategoryID2, int CategoryID3, int ReturnCategory)
		{
			return CBO.FillCollection(DataProvider.Instance().GetCategoriesFromProducts(PortalID, CategoryID1, CategoryID2, CategoryID3, ReturnCategory), typeof(CategoryInfo));
		}
		public int CategoryCount(int portalID)
		{
			return DataProvider.Instance().CategoryCount(portalID);
		}

		public int AddCategory(CategoryInfo categoryInfo)
		{
			return DataProvider.Instance().AddCategory(categoryInfo.PortalID, categoryInfo.CategoryName, 
				categoryInfo.CategoryDescription, categoryInfo.Message, categoryInfo.Archived, 
				categoryInfo.CreatedByUser, categoryInfo.CreatedDate, categoryInfo.OrderID, categoryInfo.ParentCategoryID);
		}

		public void UpdateCategory(CategoryInfo categoryInfo)
		{
			DataProvider.Instance().UpdateCategory(categoryInfo.CategoryID, categoryInfo.CategoryName, 
				categoryInfo.CategoryDescription, categoryInfo.Message, categoryInfo.Archived, categoryInfo.OrderID, categoryInfo.ParentCategoryID);
		}

		public void DeleteCategory(int categoryID)
		{
			DataProvider.Instance().DeleteCategory(categoryID);
		}

		public void DeleteCategories(int portalID)
		{
			DataProvider.Instance().DeleteCategories(portalID);
		}

		#endregion

        #region IPortable Members

        string DotNetNuke.Entities.Modules.IPortable.ExportModule(int ModuleID)
        {
            StringBuilder strXML = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            XmlWriter Writer = XmlWriter.Create(strXML, settings);

            ArrayList alCategories = GetCategories(PortalId, true, -3);

            if (alCategories.Count > 0)
            {
                Writer.WriteStartElement("Categories");
                foreach (CategoryInfo categoryInfo in alCategories)
                {
                    Writer.WriteStartElement("Category");
                    Writer.WriteElementString("CategoryID", categoryInfo.CategoryID.ToString());
                    Writer.WriteElementString("Name", categoryInfo.CategoryName);
                    Writer.WriteElementString("Description", categoryInfo.CategoryDescription);
                    Writer.WriteElementString("Message", categoryInfo.Message);
                    Writer.WriteElementString("Archived", categoryInfo.Archived.ToString());
                    Writer.WriteElementString("OrderID", categoryInfo.OrderID.ToString());
                    Writer.WriteElementString("ParentCategoryID", categoryInfo.ParentCategoryID.ToString());
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();
                Writer.Close();
            }
            else
            {
                return String.Empty;
            }
            return strXML.ToString();
        }

        void DotNetNuke.Entities.Modules.IPortable.ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            XmlNode xmlCategories = DotNetNuke.Common.Globals.GetContent(Content, "Categories");
            SortedList slCategories = new SortedList(xmlCategories.ChildNodes.Count);
            int intIndexID = Null.NullInteger;

            foreach (XmlNode xmlCategory in xmlCategories)
            {
                CategoryInfo categoryInfo = new CategoryInfo();
                categoryInfo.PortalID = PortalId;
                categoryInfo.CategoryName = xmlCategory["Name"].InnerText;
                categoryInfo.CategoryDescription = xmlCategory["Description"].InnerText;
                categoryInfo.Message = xmlCategory["Message"].InnerText;
                categoryInfo.Archived = Convert.ToBoolean(xmlCategory["Archived"].InnerText);
                categoryInfo.CreatedByUser = UserId.ToString();
                categoryInfo.CreatedDate = DateTime.Now;
                categoryInfo.OrderID = Convert.ToInt32(xmlCategory["OrderID"].InnerText);
                intIndexID = slCategories.IndexOfKey(Convert.ToInt32(xmlCategory["ParentCategoryID"].InnerText));
                if (intIndexID > -1)
                {
                    categoryInfo.ParentCategoryID = (int)slCategories.GetByIndex(intIndexID);
                }
                else
                {
                    categoryInfo.ParentCategoryID = -1;
                }
                slCategories.Add(Convert.ToInt32(xmlCategory["CategoryID"].InnerText), AddCategory(categoryInfo));
            }
        }

        #endregion
    }

    public class CategroyInfoCompare : IComparer
    {
        #region IComparer Membres

        public int Compare(object x, object y)
        {
            if (!(x is CategoryInfo) && !(y is CategoryInfo))
                return -1;
            CategoryInfo c1 = (CategoryInfo)x;
            CategoryInfo c2 = (CategoryInfo)y;
            return c1.CategoryPathName.CompareTo(c2.CategoryPathName);
        }

        #endregion
    }
}
