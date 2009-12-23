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
Namespace DotNetNuke.UI.Containers
    ''' -----------------------------------------------------------------------------
    ''' <summary></summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 	[cniknet]	10/15/2004	Replaced public members with properties and removed
    '''                             brackets from property names
    ''' </history>
    ''' -----------------------------------------------------------------------------

    Partial  Class Title

        Inherits UI.Skins.SkinObjectBase

        ' private members
        Private _cssClass As String

        Const MyFileName As String = "Title.ascx"

#Region "Public Members"

        Public Property CssClass() As String
            Get
                Return _cssClass
            End Get
            Set(ByVal Value As String)
                _cssClass = Value
            End Set
        End Property
#End Region

        Private Function CanEditModule() As Boolean
            Dim blnCanEdit As Boolean = False
            Dim objModule As Entities.Modules.PortalModuleBase = Container.GetPortalModuleBase(Me)
            If (Not objModule Is Nothing) AndAlso (objModule.ModuleId > Null.NullInteger) Then
                blnCanEdit = (PortalSettings.UserMode = PortalSettings.Mode.Edit) AndAlso (PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) OrElse PortalSecurity.IsInRoles(PortalSettings.ActiveTab.AdministratorRoles.ToString)) AndAlso (IsAdminControl() = False) AndAlso (PortalSettings.ActiveTab.IsAdminTab = False)
            End If
            Return blnCanEdit
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Assign the CssClass and Text Attributes for the Title label.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[sun1]	2/1/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                ' public attributes
                If CssClass <> "" Then
                    lblTitle.CssClass = CssClass
                End If

                Dim objPortalModule As Entities.Modules.PortalModuleBase = Container.GetPortalModuleBase(Me)

                Dim strTitle As String = Null.NullString
                If Not objPortalModule Is Nothing Then
                    strTitle = objPortalModule.ModuleConfiguration.ModuleTitle
                End If
                If strTitle = Null.NullString Then
                    strTitle = "&nbsp;"
                End If
                lblTitle.Text = strTitle

                If CanEditModule() = False OrElse Entities.Portals.PortalSettings.GetSiteSetting(objPortalModule.PortalId, "InlineEditorEnabled") = "False" Then
                    lblTitle.EditEnabled = False
                    tbEIPTitle.Visible = False
                Else
                    For Each objButton As WebControls.DNNToolBarButton In Me.tbEIPTitle.Buttons
                        objButton.ToolTip = Services.Localization.Localization.GetString("cmd" & objButton.ToolTip, Services.Localization.Localization.GetResourceFile(Me, MyFileName))
                    Next
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

		Private Sub lblTitle_UpdateLabel(ByVal source As Object, ByVal e As WebControls.DNNLabelEditEventArgs) Handles lblTitle.UpdateLabel
			If CanEditModule() Then
                Dim objModule As New DotNetNuke.Entities.Modules.ModuleController
                Dim objPortalModule As Entities.Modules.PortalModuleBase = Container.GetPortalModuleBase(Me)
                Dim objModInfo As DotNetNuke.Entities.Modules.ModuleInfo = objModule.GetModule(objPortalModule.ModuleId, objPortalModule.TabId, False)

                objModInfo.ModuleTitle = e.Text
                objModule.UpdateModule(objModInfo)
			End If

		End Sub


    End Class

End Namespace
