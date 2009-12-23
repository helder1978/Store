using System;
using System.Data;
using System.Collections;
using System.Reflection;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for GridViewClass.
	/// </summary>
	public class GridViewClass
	{
		private string dataColumns = "";		
		private bool autoNestedColumns = false;
		private ArrayList columnsList = new ArrayList();
		private ArrayList componentList = new ArrayList();
		
		public bool AutoNestedColumns
		{
			get { return this.autoNestedColumns; }
			set { this.autoNestedColumns = value; }
		}

		/// <summary>
		/// Only thse specified will be columns in grid.
		/// </summary>
		public string DataColumns
		{
			get { return dataColumns; }
			set { dataColumns = value; 
			oninit();
			}
		}

		public GridViewClass()
		{					
		}

		private void oninit()
		{
			if ( dataColumns != "" )
			{
				columnsList = ParseCSVIntoArray(dataColumns,',');
				componentList = new ArrayList();
				
				for (int i=0; i<columnsList.Count; i++)
				{					
					string colname = columnsList[i].ToString();
					string cmpntName = colname; 
					int colSep = colname.LastIndexOf("__");
					if ( colSep != -1 )
					{
						cmpntName = colname.Substring(0,colSep);					
					}

					if ( !componentList.Contains(cmpntName.Trim()) )
						componentList.Add(cmpntName.Trim());
				}				
			}
		}		

		#region Create Data Table
		/// <summary>
		/// Create a datatable object out of the user defined object specified
		/// </summary>
		/// <param name="fromObj"></param>
		/// <returns></returns>
		public DataTable CreateDataTable(object fromObj)
		{
			DataTable dtExisting = new DataTable();
			//HttpContext.Current.Server.MapPath   
			dtExisting = null;
			dtExisting = this.CreateDataTable(fromObj, string.Empty, ref dtExisting);
			return dtExisting;
		}
		
		/// <summary>
		/// Creates a DataTable for the specified DataSource
		/// </summary>
		/// <param name="fromObj">DataSource</param>
		/// <param name="colPrefix">ColPrefix [leave it blank]</param>
		/// <param name="dtExisting">Existin Table Object</param>
		/// <returns>Modified Table Object</returns>
		public DataTable CreateDataTable(object fromObj, string colPrefix, ref DataTable dtExisting)
		{
			if(fromObj == null) return dtExisting;
			DataTable dt;			

			System.Type t = fromObj.GetType();
			
			//generate column prefix
			if(dtExisting == null)
			{			
				if(t.IsArray || t == typeof(ArrayList) || t == typeof(Hashtable))				
					colPrefix = "";
				else
					colPrefix = fromObj.GetType().Name;
				
				dt = new DataTable(colPrefix);
			}
			else
			{
				dt = dtExisting;

				if ( colPrefix == "" )
					colPrefix += fromObj.GetType().Name;
			}			
			
			//check if need to be fetched.
			if ( autoNestedColumns == false )
				if ( colPrefix != "" )
					if(ComponentToUse(colPrefix) == false && ComponentToFetch(colPrefix) == false ) return dtExisting;

			// check whether the the type is a collection.			
			if(t.IsArray || t == typeof(ArrayList) || t == typeof(Hashtable))
			{
				ICollection arrObj = (ICollection)fromObj;
				if(t == typeof(Hashtable)) 
				{
					arrObj = ((Hashtable) arrObj).Values;
				} 
						
				foreach(object arrObjVal in arrObj)
				{
					if(arrObjVal != null)
					{
						//add new row to the table
						DataRow dr = dt.NewRow();
						dt.Rows.Add(dr);
						dt = CreateDataTable(arrObjVal, colPrefix, ref dt);
					}
				}
			} 
			else 
			{
				DataRow dr;
				//if row already exist then use the previous row
				if(dt.Rows.Count>0)
					dr = dt.Rows[dt.Rows.Count-1];  
				else
				{
					dr = dt.NewRow();
					dt.Rows.Add(dr); 
				} 
				
				// Loop through all the properties and create columns and put the value in it.				
				foreach(PropertyInfo pi in fromObj.GetType().GetProperties())
				{
					if(pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string)
						|| pi.PropertyType == typeof(decimal) || pi.PropertyType == typeof(DateTime))
					{
						// this property is a primitive type or types that can be used to fill the
						//	columns.  Add the column to the table and set the values.
						//string colName = colPrefix + "__" + pi.Name;						// changed
                        string colName = pi.Name;						// changed
						//Only including columns in data table which are listed in aspx
						if ( dataColumns == "" || autoNestedColumns == true || columnsList.Contains(colName) || columnsList.Contains(colPrefix + "__*") )						
						{
							int colIndex = 0;
							if(dt.Columns.Contains(colName))
							{
								colIndex = dt.Columns.IndexOf(colName); 
							}
							else
							{
								colIndex = dt.Columns.Count;
								dt.Columns.Add(new DataColumn(colName, pi.PropertyType));
							}
							dr[colIndex] = pi.GetValue(fromObj, new object[] {});
						}
					} 
					else if(pi.PropertyType.IsArray || pi.PropertyType == typeof(ArrayList) 
						|| pi.PropertyType == typeof(Hashtable))
					{
						// create a data table for each object in the array/hashtable
						ICollection arrObj = (ICollection)pi.GetValue(fromObj, new object[] {});
						if(arrObj!=null)
						{
							if(pi.PropertyType == typeof(Hashtable)) 
							{
								arrObj = ((Hashtable) arrObj).Values;
							} 																											
							
							foreach(object arrObjVal in arrObj)
							{
								if(arrObjVal==null) continue;								
							
								//if still intrinsic type then ignore
								if(arrObjVal.GetType() == typeof(string) || arrObjVal.GetType() == typeof(decimal) || arrObjVal.GetType() == typeof(DateTime))
								{
									continue;
								}
								string arrayItem = colPrefix + "__" + pi.Name + "__" + arrObjVal.GetType().Name;
								
								if ( autoNestedColumns == false )
									if(( ComponentToUse(arrayItem)==false && ComponentToFetch(colPrefix) == false)  ) break;
								
								CreateDataTable(arrObjVal, arrayItem , ref dt);
							}
						}
					} 
					else if(pi.PropertyType.IsClass)
					{
						string tempColPrfx = colPrefix;						
						
						if ( tempColPrfx != "" )
							tempColPrfx += "__" + pi.Name + "";
						
						//Dont get into nested detail, if not part of component to use
						//e.g. not get into Employee.AddressCmpnt.Phonecmpnt while reading AddressCmpnt
						if (autoNestedColumns == false)
							if ( ComponentToFetch(tempColPrfx)==false ) continue;
						
						CreateDataTable(pi.GetValue(fromObj, new object[] {}), tempColPrfx, ref dt);
					}
				}
			}
			//return the updated datatable
			return dt;
		}
		#endregion	

		/// <summary>
		/// Compare component if need to be used.
		/// </summary>
		/// <param name="colName"></param>
		/// <returns></returns>
		private bool ComponentToUse(string colName)
		{						
			if( colName != "" )
			{				
				//If not in the list, delibrately .Contains() not used here, so that could trim() and compare info.
				for (int i=0; i<componentList.Count; i++)
				{
					if ( componentList[i].ToString().Trim() == colName.Trim() )
						return true;
				}					
				return false;
			}
			return true;
		}		

		/// <summary>
		/// Check component to fetch, even the nested ones.
		/// </summary>
		/// <param name="colName"></param>
		/// <returns></returns>
		private bool ComponentToFetch(string colName)
		{						
			if( colName != "")
			{				
				//If not in the list.
				for (int i=0; i<componentList.Count; i++)
				{
					if ( componentList[i].ToString().Trim().StartsWith(colName.Trim()) )
						return true;
				}					
				return false;
			}
			return true;
		}

		private ArrayList ParseCSVIntoArray(string csvString, char separateChar)
		{
			ArrayList strArray = new ArrayList();
			if(csvString != String.Empty && csvString.Length > 0)
			{
				string[] csvStringArr = csvString.Split(separateChar);
			
				foreach(string tmpCsvStr in csvStringArr)
				{
					strArray.Add(tmpCsvStr.Trim());
				}
			}

			return strArray;
		}
	}

}
