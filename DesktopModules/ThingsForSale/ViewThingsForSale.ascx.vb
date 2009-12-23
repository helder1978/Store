Imports DotNetNuke
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports DotNetNuke.Security.PortalSecurity


Namespace YourCompany.Modules.ThingsForSale

    Partial Class ViewThingsForSale

        Inherits Entities.Modules.PortalModuleBase

        Dim ThingsForSaleInfo_data As New ThingsForSaleInfo

        Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


            If IsInRole("Registered Users") Or IsInRole("Administrators") Then
                Add_My_Listing_LinkButton.Enabled = True
            Else
                Add_My_Listing_LinkButton.Text = "You must be logged in to add a Listing"
                Add_My_Listing_LinkButton.Enabled = False
            End If

        End Sub

        Protected Sub SetModuleId(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource_ThingsForSale.Selecting

            e.InputParameters("ModuleId") = ModuleId.ToString

        End Sub

        Protected Sub InsertingItem(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewInsertEventArgs) Handles FormView1.ItemInserting

            e.Values.Item("UserID") = Entities.Users.UserController.GetCurrentUserInfo.UserID
            e.Values.Item("ModuleId") = ModuleId.ToString()
            e.Values.Item("ID") = 0

        End Sub

        Protected Sub InsertCancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

            Me.FormView1.Visible = False

        End Sub

        Protected Sub Add_My_Listing_LinkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Add_My_Listing_LinkButton.Click

            Me.FormView1.Visible = True

        End Sub

        Protected Sub InsertButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

            Me.FormView1.Visible = False
            Add_My_Listing_LinkButton.Text = "Update Successful - Add Another Listing"

        End Sub

        Protected Sub HideEditButtons(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                ThingsForSaleInfo_data = CType(e.Row.DataItem, ThingsForSaleInfo)
                If IsInRole("Administrators") Or (Entities.Users.UserController.GetCurrentUserInfo.UserID = CInt(ThingsForSaleInfo_data.UserID)) Then
                    e.Row.Cells.Item(0).Enabled = True
                Else
                    e.Row.Cells.Item(0).Text = "&nbsp;"
                End If
            End If

        End Sub

    End Class

End Namespace