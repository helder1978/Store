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

Imports DotNetNuke.Entities.Modules.Actions

Namespace DotNetNuke.Modules.Admin.Portals

	Partial Class PortalAlias
		Inherits DotNetNuke.Entities.Modules.PortalModuleBase
		Implements Entities.Modules.IActionable


#Region "Private Members"

        Private intPortalID As Integer

#End Region

#Region "Private Methods"

        Private Sub BindData()
            Dim arr As ArrayList
            Dim p As New PortalAliasController

            arr = p.GetPortalAliasArrayByPortalID(intPortalID)
            dgPortalAlias.DataSource = arr
            dgPortalAlias.DataBind()
        End Sub

#End Region

#Region "Public Methods"

        Public Function IsNotCurrent(ByVal Id As String) As Boolean

            Dim portalAliasId As Integer = Int32.Parse(Id)
            If portalAliasId = Me.PortalAlias.PortalAliasID() Then
                Return False
            Else
                Return True
            End If

        End Function

#End Region

#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                Services.Localization.Localization.LocalizeDataGrid(dgPortalAlias, Me.LocalResourceFile)
                BindData()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New ModuleActionCollection
                If Not (Request.QueryString("pid") Is Nothing) And (PortalSettings.ActiveTab.ParentId = PortalSettings.SuperTabId Or UserInfo.IsSuperUser) Then
                    intPortalID = Int32.Parse(Request.QueryString("pid"))
                Else
                    intPortalID = PortalId
                End If

                Actions.Add(GetNextActionID, Services.Localization.Localization.GetString(ModuleActionType.AddContent, LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("pid", intPortalID.ToString, "Edit"), False, SecurityAccessLevel.Admin, True, False)
                Return Actions
            End Get
        End Property
#End Region

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

    End Class


End Namespace
