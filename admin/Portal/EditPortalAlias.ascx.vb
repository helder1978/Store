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

Imports DotNetNuke.Services.Localization

Namespace DotNetNuke.Modules.Admin.Portals

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The EditPortalAlias PortalModuleBase is used to edit a portal alias
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	01/17/2005	documented
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class EditPortalAlias

        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindData fetches the data from the database and updates the controls
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/17/2005	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindData()

            If Not Request.QueryString("paid") Is Nothing Then
                Dim objPortalAliasInfo As PortalAliasInfo
                Dim intPortalAliasID As Integer = CType(Request.QueryString("paid"), Integer)

                Dim p As New PortalAliasController
                objPortalAliasInfo = p.GetPortalAliasByPortalAliasID(intPortalAliasID)

                ViewState.Add("PortalAliasID", intPortalAliasID)
                ViewState.Add("PortalID", objPortalAliasInfo.PortalID)

                If Not UserInfo.IsSuperUser Then
                    If objPortalAliasInfo.PortalID <> PortalSettings.PortalId Then
                        UI.Skins.Skin.AddModuleMessage(Me, "You do not have access to view this Portal Alias.", UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                        Exit Sub
                    End If
                End If

                txtAlias.Text = objPortalAliasInfo.HTTPAlias
                SetDeleteVisibility(objPortalAliasInfo.PortalID)

                cmdUpdate.Text = Localization.GetString("cmdUpdate", Me.LocalResourceFile)

            ElseIf Request.QueryString("pid") <> "" Then
                If UserInfo.IsSuperUser Then
                    ViewState.Add("PortalID", CType(Request.QueryString("pid"), Integer))
                End If
                SetDeleteVisibility(CType(Request.QueryString("pid"), Integer))

                cmdUpdate.Text = Localization.GetString("cmdAdd", Me.LocalResourceFile)
            Else
                ViewState.Add("PortalID", PortalSettings.PortalId)
                SetDeleteVisibility(PortalSettings.PortalId)

                cmdUpdate.Text = Localization.GetString("cmdAdd", Me.LocalResourceFile)
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' SetDeleteVisibility determines whether the Delete button should be displayed
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/17/2005	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub SetDeleteVisibility(ByVal PortalID As Integer)
            Dim colPortalAlias As PortalAliasCollection
            Dim p As New PortalAliasController
            colPortalAlias = p.GetPortalAliasByPortalID(PortalID)
            'Disallow delete if there is only one portal alias
            If colPortalAlias.Count <= 1 Then
                cmdDelete.Visible = False
            End If
        End Sub

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/17/2005	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                If Not Page.IsPostBack Then
                    ' Store URL Referrer to return to portal
                    If Not Request.UrlReferrer Is Nothing Then
                        ViewState("UrlReferrer") = Convert.ToString(Request.UrlReferrer)
                    Else
                        ViewState("UrlReferrer") = ""
                    End If

                    BindData()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdCancel_Click runs when the Cancel button is clicked
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/17/2005	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Response.Redirect(Convert.ToString(ViewState("UrlReferrer")), True)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdDelete_Click runs when the Delete button is clicked
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/17/2005	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
            Try
                Dim intPortalAliasID As Integer
                intPortalAliasID = CType(ViewState("PortalAliasID"), Integer)
                Dim objPortalAliasInfo As PortalAliasInfo
                Dim p As New PortalAliasController
                objPortalAliasInfo = p.GetPortalAliasByPortalAliasID(intPortalAliasID)

                If Not UserInfo.IsSuperUser Then
                    If objPortalAliasInfo.PortalID <> PortalSettings.PortalId Then
                        UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("AccessDenied", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                        Exit Sub
                    End If
                End If
                p.DeletePortalAlias(intPortalAliasID)

                Response.Redirect(Convert.ToString(ViewState("UrlReferrer")), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdUpdate_Click runs when the Update button is clicked
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/17/2005	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            Try
                Dim strAlias As String = txtAlias.Text
                If strAlias <> "" Then
                    If strAlias.IndexOf("://") <> -1 Then
                        strAlias = strAlias.Remove(0, strAlias.IndexOf("://") + 3)
                    End If
                    If strAlias.IndexOf("\\") <> -1 Then
                        strAlias = strAlias.Remove(0, strAlias.IndexOf("\\") + 2)
                    End If

                    Dim p As New PortalAliasController
                    If Not ViewState("PortalAliasID") Is Nothing Then
                        Dim objPortalAliasInfo As New PortalAliasInfo
                        objPortalAliasInfo.PortalAliasID = Convert.ToInt32(ViewState("PortalAliasID"))
                        objPortalAliasInfo.PortalID = Convert.ToInt32(ViewState("PortalID"))
                        objPortalAliasInfo.HTTPAlias = strAlias
                        Try
                            p.UpdatePortalAliasInfo(objPortalAliasInfo)
                        Catch
                            UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("DuplicateAlias", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                            Exit Sub
                        End Try
                    Else
                        Dim objPortalAliasInfo As PortalAliasInfo
                        objPortalAliasInfo = p.GetPortalAlias(strAlias, Convert.ToInt32(ViewState("PortalAliasID")))
                        If objPortalAliasInfo Is Nothing Then
                            objPortalAliasInfo = New PortalAliasInfo
                            objPortalAliasInfo.PortalID = Convert.ToInt32(ViewState("PortalID"))
                            objPortalAliasInfo.HTTPAlias = strAlias
                            p.AddPortalAlias(objPortalAliasInfo)
                        Else
                            UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("DuplicateAlias", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                            Exit Sub
                        End If
                    End If
                    UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("Success", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess)
                    Response.Redirect(Convert.ToString(ViewState("UrlReferrer")), True)
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

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
