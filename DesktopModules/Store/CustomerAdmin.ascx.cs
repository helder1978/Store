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
using System.IO;
using System.Net;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Modules.Store.Admin;
using DotNetNuke.Modules.Store.Catalog;
using DotNetNuke.Modules.Store.Components;
using DotNetNuke.Modules.Store.Customer;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for CustomerAdmin.
	/// </summary>
	public partial  class CustomerAdmin : StoreControlBase
	{
		#region Private Declarations
		private AdminNavigation _nav;
		#endregion

		#region Controls
		protected DotNetNuke.UI.UserControls.LabelControl lblParentTitle;
		#endregion


        //protected System.Web.UI.WebControls.CheckBox cbExport;

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			lstCustomers.SelectedIndexChanged += new EventHandler(lstCustomers_SelectedIndexChanged);
            lstOrderStatus.SelectedIndexChanged += new EventHandler(lstOrderStatus_SelectedIndexChanged);
		}

		#endregion

		#region Events
		protected void Page_Load(object sender, System.EventArgs e)
		{
			_nav = new AdminNavigation(Request.QueryString);

			if (!IsPostBack)
			{
				OrderController orderController = new OrderController();
                lstCustomers.DataTextField = "Username";
				lstCustomers.DataValueField = "UserID";
				lstCustomers.DataSource = orderController.GetCustomers(PortalId);
				lstCustomers.DataBind();

				lstCustomers.Items.Insert(0, new ListItem("--- " + Localization.GetString("Select", this.LocalResourceFile) + " ---", ""));

                lstOrderStatus.DataTextField = "OrderStatusText";
                lstOrderStatus.DataValueField = "OrderStatusID";
                lstOrderStatus.DataSource = orderController.GetOrderStatuses();
                lstOrderStatus.DataBind();

                lstOrderStatus.Items.Insert(0, new ListItem("--- " + Localization.GetString("Select", this.LocalResourceFile) + " ---", ""));
			}

			if (_nav.CustomerID != Null.NullInteger)
			{
				lstCustomers.SelectedValue = _nav.CustomerID.ToString();

				StoreControlBase ordersControl = (StoreControlBase)LoadControl(ModulePath + "CustomerOrders.ascx");
				ordersControl.ParentControl = this.ParentControl;
                ((CustomerOrders)ordersControl).ShowOrdersInStatus = false;
                ((CustomerOrders)ordersControl).OrderStatusID = Null.NullInteger;

				plhOrders.Controls.Clear();
				plhOrders.Controls.Add(ordersControl);
				plhOrders.Visible = true;
			}
            
            if (_nav.OrderID != Null.NullInteger)
            {
                tbOrderNumber.Text = _nav.OrderID.ToString();
            }
		}

		protected override void OnPreRender(EventArgs e)
		{
			// Set the title in the parent control
			Store storeAdmin = (Store)parentControl;
			storeAdmin.ParentTitle = lblParentTitle.Text;

			base.OnPreRender (e);
		}

        private DataTable getOrdersData(int orderId, int customerId, int orderStatusId)
        {
            OrderController orderController = new OrderController();

            //Create Tempory Table
            DataTable dtTemp = new DataTable();

            //Creating Header Row
            dtTemp.Columns.Add("<b>Order Number</b>");  // 0
            dtTemp.Columns.Add("<b>Login</b>");         // 1
            dtTemp.Columns.Add("<b>Name</b>");          // 2
            dtTemp.Columns.Add("<b>Company(Profile)</b>");       // 3
            dtTemp.Columns.Add("<b>Company(Wisdom)</b>");       // 4
            dtTemp.Columns.Add("<b>Order Date</b>");    // 5
            dtTemp.Columns.Add("<b>Order Total</b>");   // 6
            dtTemp.Columns.Add("<b>Status</b>");        // 7
            dtTemp.Columns.Add("<b>Product</b>");       // 8
            dtTemp.Columns.Add("<b>Delivery</b>");      // 9
            dtTemp.Columns.Add("<b>Qty</b>");           // 10
            dtTemp.Columns.Add("<b>SubTotal</b>");      // 11

            UserController userController = new UserController();
            ArrayList orders = new ArrayList();
            DataRow drAddItem;
            if (orderId != -1)
            {
                OrderInfo orderInfo = orderController.GetOrder(orderId);
                ArrayList orderDetailsList = orderController.GetOrderDetails(orderId);

                UserInfo userInfo = userController.GetUser(this.PortalId, orderInfo.CustomerID);

                foreach (OrderDetailsInfo orderDetails in orderDetailsList)
                {
                    drAddItem = dtTemp.NewRow();
                    drAddItem[0] = orderInfo.OrderID + "";
                    drAddItem[1] = userInfo.Username;
                    drAddItem[2] = userInfo.DisplayName;
                    drAddItem[3] = userInfo.Profile.GetPropertyValue("Company");
                    drAddItem[4] = getUserCompanyName(orderInfo.CustomerID);
                    drAddItem[5] = orderInfo.OrderDate.ToString("dd/MM/yyyy HH:mm");
                    drAddItem[6] = orderInfo.OrderTotal + "";
                    drAddItem[7] = GetOrderStatus(orderInfo.OrderStatusID, orderInfo.OrderIsPlaced);
                    // drAddItem[7] = orderInfo.sta;

                    drAddItem[8] = getProductName(orderDetails.ModelName, orderDetails.ProdReference); //orderDetails.ModelName;
                    drAddItem[9] = getProdDeliveryMethodStr(orderDetails.ProdDeliveryMethod);
                    drAddItem[10] = orderDetails.Quantity;
                    drAddItem[11] = orderDetails.ProdCost;

                    dtTemp.Rows.Add(drAddItem);
                }

            }
            else if (customerId != -1)
            {
                orders = orderController.GetCustomerOrders(this.PortalId, customerId);
                foreach (OrderInfo orderInfo in orders)
                {
                    UserInfo userInfo = userController.GetUser(this.PortalId, orderInfo.CustomerID);
                    
                    ArrayList orderDetailsList = orderController.GetOrderDetails(orderInfo.OrderID);

                    foreach (OrderDetailsInfo orderDetails in orderDetailsList)
                    {
                        drAddItem = dtTemp.NewRow();
                        drAddItem[0] = orderInfo.OrderID + "";
                        drAddItem[1] = userInfo.Username;
                        drAddItem[2] = userInfo.DisplayName;
                        drAddItem[3] = userInfo.Profile.GetPropertyValue("Company");
                        drAddItem[4] = getUserCompanyName(orderInfo.CustomerID);
                        drAddItem[5] = orderInfo.OrderDate.ToString("dd/MM/yyyy HH:mm");
                        drAddItem[6] = orderInfo.OrderTotal + "";
                        drAddItem[7] = GetOrderStatus(orderInfo.OrderStatusID, orderInfo.OrderIsPlaced);
                        // drAddItem[7] = orderInfo.sta;

                        drAddItem[8] = getProductName(orderDetails.ModelName, orderDetails.ProdReference); //orderDetails.ModelName;
                        drAddItem[9] = getProdDeliveryMethodStr(orderDetails.ProdDeliveryMethod);
                        drAddItem[10] = orderDetails.Quantity;
                        drAddItem[11] = orderDetails.ProdCost;

                        dtTemp.Rows.Add(drAddItem);
                    }

                }
            }
            else if (orderStatusId != -1)
            {
                orders = orderController.GetOrders(this.PortalId, orderStatusId);
                foreach (OrderInfo orderInfo in orders)
                {
                    UserInfo userInfo = userController.GetUser(this.PortalId, orderInfo.CustomerID);

                    ArrayList orderDetailsList = orderController.GetOrderDetails(orderInfo.OrderID);

                    foreach (OrderDetailsInfo orderDetails in orderDetailsList)
                    {
                        drAddItem = dtTemp.NewRow();
                        drAddItem[0] = orderInfo.OrderID + "";
                        drAddItem[1] = userInfo.Username;
                        drAddItem[2] = userInfo.DisplayName;
                        drAddItem[3] = userInfo.Profile.GetPropertyValue("Company");
                        drAddItem[4] = getUserCompanyName(orderInfo.CustomerID);
                        drAddItem[5] = orderInfo.OrderDate.ToString("dd/MM/yyyy HH:mm");
                        drAddItem[6] = orderInfo.OrderTotal + "";
                        drAddItem[7] = GetOrderStatus(orderInfo.OrderStatusID, orderInfo.OrderIsPlaced);
                        // drAddItem[7] = orderInfo.sta;

                        drAddItem[8] = getProductName(orderDetails.ModelName, orderDetails.ProdReference); //orderDetails.ModelName;
                        drAddItem[9] = getProdDeliveryMethodStr(orderDetails.ProdDeliveryMethod);
                        drAddItem[10] = orderDetails.Quantity;
                        drAddItem[11] = orderDetails.ProdCost;

                        dtTemp.Rows.Add(drAddItem);
                    }
                }
            }

            {
                //                Response.Write("<br>" + ((Decimal)drW["VolumeMillionLitres"]).ToString());
            }
            return dtTemp;
        }

        protected String getProdDeliveryMethodStr(int deliveryMethod)
        {
            String deliveryMethodStr = "-";
            switch (deliveryMethod)
            {
                case 1:
                    deliveryMethodStr = "PDF";
                    break;
                case 2:
                    deliveryMethodStr = "Hard copy";
                    break;
            }
            return deliveryMethodStr;
        }

        private string getProductName(string modelName, string prodReference)
        {
            if (modelName != "DE Template Product")
                return modelName;
            else
                return getDEDetails(prodReference);
        }


        private string getDEDetails(string selectedDEStr)
        {
            string returnText = "";
            if (selectedDEStr.IndexOf(":") > 0)
            {
                char[] splitter = { ';' };
                char[] splitter2 = { ':' };

                String[] selectedDEs = selectedDEStr.Split(splitter);
                CategoryController categoryController = new CategoryController();
                foreach (String selectedDE in selectedDEs)
                {
                    if (selectedDE != "")
                    {
                        String[] options = selectedDE.Split(splitter2);
                        int cat1 = -1;
                        int.TryParse(options[0], out cat1);
                        int cat2 = -1;
                        int.TryParse(options[1], out cat2);

                        CategoryInfo catg1 = categoryController.GetCategory(cat1);
                        CategoryInfo catg2 = categoryController.GetCategory(cat2);
                        //DEProductInfo prod = new DEProductInfo(catg1.CategoryID, catg1.CategoryName, catg2.CategoryID, catg2.CategoryName);
                        returnText = returnText + catg1.CategoryName + " / " + catg2.CategoryName + "<br>";
                    }
                }
            }
            return returnText;
        }


        private string GetOrderStatus(int orderStatusID, bool orderIsPlaced)
        {
            // canadean changed: allow to get the status of orders that aren't placed
            if (!orderIsPlaced)
                return "Not Placed";

            OrderController orderController = new OrderController();
            ArrayList orderStatusList = orderController.GetOrderStatuses();

            if (orderStatusList == null)
            {
                orderStatusList = orderController.GetOrderStatuses();
            }

            string OrderStatusText = "";
            foreach (OrderStatus orderStatus in orderStatusList)
            {
                if (orderStatus.OrderStatusID == orderStatusID)
                {
                    OrderStatusText = orderStatus.OrderStatusText;
                    break;
                }
            }
            return OrderStatusText;
        }

        public static String getUserCompanyName(int userId)
        {
            String companyName = "";
            String _connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();

            // #Creating sql connection string and opening the connection#
            SqlConnection sqlConn = new SqlConnection(_connectionString);
            sqlConn.Open();

            String sqlquery = "select CompanyName from Canadean_Users inner join Canadean_Companies ON Canadean_Users.CompanyId = Canadean_Companies.CompanyId where UserId = " + userId;

            SqlCommand SelectCommand = new SqlCommand(sqlquery, sqlConn);

            //        bool firstTime = true;
            SqlDataReader dr = SelectCommand.ExecuteReader();
            if (dr.Read())
            {
                companyName = (String)dr["CompanyName"];
            }
            dr.Close();
            sqlConn.Close();
            return companyName;
        }

        private void ExportToExcel(HttpContext httpContext, string strFileName, DataTable dt)
        {
            //Temp Grid
            DataGrid dg = new DataGrid();
            dg.DataSource = dt;
            dg.DataBind();

            httpContext.Response.ClearContent();
            httpContext.Response.AddHeader("content-disposition", "attachment; filename=" + strFileName);
            httpContext.Response.ContentType = "application/excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            dg.RenderControl(htw);
            httpContext.Response.Write(sw.ToString());
            httpContext.Response.End();
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            {
                //Response.Write("<!-- Going to export to excel -->");
                string strFileName = "orders.xls";

                int orderId = -1;
                int customerID = -1;
                int orderStatusID = -1;
                if (tbOrderNumber.Text.Length > 0)  // Export only the given orderId
                {
                    try { orderId = Convert.ToInt32(tbOrderNumber.Text); }
                    catch (FormatException) { return; }

                }
                else if (lstCustomers.SelectedIndex > 0) // Export all the orders for a given customer
                {
                    customerID = int.Parse(lstCustomers.SelectedValue);

                }
                else if (lstOrderStatus.SelectedIndex > 0) // Export all the orders with the specified status
                {
                    try { orderStatusID = Convert.ToInt32(lstOrderStatus.SelectedValue); }
                    catch (System.FormatException)
                    {
                        return;
                    }
                }

                DataTable dtTemp = getOrdersData(orderId, customerID, orderStatusID);
                ExportToExcel(this.Context, strFileName, dtTemp);
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            /*
            if (cbExport.Checked)   // Export to excel option selected
            {
                //Response.Write("<!-- Going to export to excel -->");
                string strFileName = "orders.xls";

                int orderId = -1;
                int customerID = -1;
                int orderStatusID = -1;
                if (tbOrderNumber.Text.Length > 0)  // Export only the given orderId
                {
                    try { orderId = Convert.ToInt32(tbOrderNumber.Text); }
                    catch (FormatException) { return; }

                }
                else if (lstCustomers.SelectedIndex > 0) // Export all the orders for a given customer
                {
                    customerID = int.Parse(lstCustomers.SelectedValue);

                }
                else if (lstOrderStatus.SelectedIndex > 0) // Export all the orders with the specified status
                {
                    try { orderStatusID = Convert.ToInt32(lstOrderStatus.SelectedValue); }
                    catch (System.FormatException)
                    {
                        return;
                    }
                }

                DataTable dtTemp = getOrdersData(orderId, customerID, orderStatusID);
                ExportToExcel(this.Context, strFileName, dtTemp);
            }
            */
            if (tbOrderNumber.Text.Length > 0)
            {
                lstCustomers.ClearSelection();
                lstOrderStatus.ClearSelection();

                noOrdersFound.Visible = false;
                CustomerNavigation customerNav = new CustomerNavigation(Request.QueryString);
                try { customerNav.OrderID = Convert.ToInt32(tbOrderNumber.Text); }
                catch (FormatException)
                {
                    customerNav.OrderID = Null.NullInteger;
                }
                if (customerNav.OrderID != Null.NullInteger)
                {
                    OrderController orderController = new OrderController();
                    OrderInfo orderInfo = orderController.GetOrder(customerNav.OrderID);
                    if (orderInfo != null)
                    {
                        customerNav.CustomerID = orderInfo.CustomerID;
                        Response.Redirect(customerNav.GetNavigationUrl());
                    }
                    else
                    {
                        customerNav.OrderID = Null.NullInteger;
                        customerNav.CustomerID = Null.NullInteger;
                        noOrdersFound.Visible = true;
                    }
                }
            }
            else if (lstCustomers.SelectedIndex > 0)
            {
                lstOrderStatus.ClearSelection();
                _nav.OrderID = Null.NullInteger;
                
                noOrdersFound.Visible = false;
                _nav.CustomerID = int.Parse(lstCustomers.SelectedValue);
                _nav.OrderID = Null.NullInteger;
                Response.Redirect(_nav.GetNavigationUrl());                
            }
            else if (lstOrderStatus.SelectedIndex > 0)
            {
                //_nav.CustomerID = Null.NullInteger;

                noOrdersFound.Visible = false;
                StoreControlBase ordersControl = (StoreControlBase)LoadControl(ModulePath + "CustomerOrders.ascx");
                ordersControl.ParentControl = this.ParentControl;
                ((CustomerOrders)ordersControl).ShowOrdersInStatus = true;
                try { ((CustomerOrders)ordersControl).OrderStatusID = Convert.ToInt32(lstOrderStatus.SelectedValue); }
                catch (System.FormatException)
                {
                    return;
                }

                plhOrders.Controls.Clear();
                plhOrders.Controls.Add(ordersControl);
                plhOrders.Visible = true;
            }
        }

		private void lstCustomers_SelectedIndexChanged(object sender, EventArgs e)
		{
            
		}

        private void lstOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void tbOrderNumber_TextChanged(object sender, EventArgs e)
        {
           
        }
		#endregion
	}
}
