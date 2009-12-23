Imports DotNetNuke
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports DotNetNuke.Security.PortalSecurity

Namespace nsolutions4u.Modules.DataAvailability

    Partial Class ViewDataAvailability

        Inherits Entities.Modules.PortalModuleBase

        Dim DataAvailabilityInfo_data As New DataAvailabilityInfo

        Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


            'SqlDataReader myReader = myCommand.ExecuteReader();
            'myReader.Read ();
            'this.dropdownlist.DataSource =myReader.GetValue(0);
            'this.dropdownlist.DataTextField = "THE NAME OF THE TEXT FIELD"
            'this.dropdownlist.DataValueField="THE NAME OF THE VALUE FIELD";
            'this.dropdownlist.DataBind();


            If IsInRole("Registered Users") Or IsInRole("Administrators") Then
                Add_My_Listing_LinkButton.Enabled = True
            Else
                Add_My_Listing_LinkButton.Text = "You must be logged in to add a Listing"
                Add_My_Listing_LinkButton.Enabled = False
            End If

        End Sub

        Protected Sub SetModuleId(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource_DataAvailability.Selecting

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
            Add_My_Listing_LinkButton.Text = "Update Successful - Add Another "

        End Sub

        Protected Sub HideEditButtons(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                DataAvailabilityInfo_data = CType(e.Row.DataItem, DataAvailabilityInfo)
                'If IsInRole("Administrators") Or (Entities.Users.UserController.GetCurrentUserInfo.UserID = CInt(DataAvailabilityInfo_data.UserID)) Then
                If IsInRole("Registered Users") Or IsInRole("Administrators") Then
                    e.Row.Cells.Item(0).Enabled = True
                Else
                    e.Row.Cells.Item(0).Text = "&nbsp;"
                End If
            End If

        End Sub

        Protected Sub GridView1_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBinding

        End Sub

        'Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound
        'Dim options As String() = {"Option1", "Option2", "Option3"}
        'Dim list As DropDownList = DirectCast(e.Item.FindControl("ItemDropDown"), DropDownList)
        '    list.DataSource = options
        '    list.DataBind()
        'End Sub
    End Class

End Namespace