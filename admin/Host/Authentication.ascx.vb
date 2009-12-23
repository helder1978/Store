'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'

Imports DotNetNuke.HttpModules.Config
Imports DotNetNuke.UI.WebControls
Imports DotNetNuke.Services.Authentication
Imports DotNetNuke.Services.Localization

Imports System.Collections.Generic
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Imports DotNetNuke.Services.Packages

Namespace DotNetNuke.Modules.Admin.Host

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Authentication PortalModuleBase is used to manage the Authentication Systems
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	07/05/2007 Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Authentication
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

#Region "Private Methods"

        Private Sub BindData()
            grdAuthentication.DataSource = AuthenticationController.GetAuthenticationServices()
            grdAuthentication.DataBind()
        End Sub

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Init runs when the control is initialised
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	07/05/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            For Each column As DataGridColumn In grdAuthentication.Columns
                If column.GetType Is GetType(CheckBoxColumn) Then
                    Dim cbColumn As CheckBoxColumn = CType(column, CheckBoxColumn)
                    AddHandler cbColumn.CheckedChanged, AddressOf grdAuthentication_ItemCheckedChanged
                ElseIf column.GetType Is GetType(ImageCommandColumn) Then
                    'Localize Image Column Text
                    Dim imageColumn As ImageCommandColumn = CType(column, ImageCommandColumn)
                    If imageColumn.CommandName <> "" Then
                        imageColumn.Text = Localization.GetString(imageColumn.CommandName, Me.LocalResourceFile)
                    End If
                End If
            Next
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	07/05/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                'Localize the Data Grid
                Localization.LocalizeDataGrid(grdAuthentication, LocalResourceFile)
                BindData()
            End If
        End Sub

        Protected Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
            Response.Redirect(Util.InstallURL(TabId, "Auth_System"), True)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' grdAuthentication_ItemCheckedChanged runs when a checkbox in the grid
        ''' is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	07/05/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub grdAuthentication_ItemCheckedChanged(ByVal sender As Object, ByVal e As DNNDataGridCheckChangedEventArgs)
            Dim propertyName As String = e.Field
            Dim propertyValue As Boolean = e.Checked
            Dim isAll As Boolean = e.IsAll
            Dim index As Integer = e.Item.ItemIndex

            'Get the Authentication Systems
            Dim systems As List(Of AuthenticationInfo) = AuthenticationController.GetAuthenticationServices()

            Dim system As AuthenticationInfo = systems(index)
            system.IsEnabled = e.Checked

            'Update Authentication System
            AuthenticationController.UpdateAuthentication(system)

            BindData()

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' grdAuthentication_DeleteCommand runs when a delete button is clicked in the grid
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	07/31/2007  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub grdAuthentication_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdAuthentication.DeleteCommand

            Dim item As DataGridItem = e.Item
            Dim packageId As Integer = Int32.Parse(e.CommandArgument.ToString)

            If packageId > 0 Then
                Response.Redirect(Util.UnInstallURL(TabId, packageId), True)
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' grdAuthentication_ItemDataBound runs when a row in the grid is bound
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	07/31/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub grdAuthentication_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdAuthentication.ItemDataBound
            Dim item As DataGridItem = e.Item

            If item.ItemType = ListItemType.Item Or _
                    item.ItemType = ListItemType.AlternatingItem Or _
                    item.ItemType = ListItemType.SelectedItem Then

                Dim imgColumnControl As Control = item.Controls(0).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim delImage As ImageButton = CType(imgColumnControl, ImageButton)
                    Dim auth As AuthenticationInfo = CType(item.DataItem, AuthenticationInfo)

                    delImage.Visible = (auth.PackageID > 0)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace

