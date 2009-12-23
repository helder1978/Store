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

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke

Namespace DotNetNuke.Modules.Admin.Search
    Partial  Class SearchAdmin
		Inherits Entities.Modules.PortalModuleBase

#Region "Controls"



        'tasks

#End Region

        Private _settings As Hashtable
        Private _defaultSettings As Hashtable


#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetSetting gets a Search Setting from the Portal Modules Settings table (or
        ''' from the Host Settings if not defined)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	11/16/2004	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetSetting(ByVal txtName As String) As String

            Dim settingValue As String = ""

            'Try Portal setting first
            If _settings(txtName) Is Nothing = False Then
                settingValue = CType(_settings(txtName), String)
            Else
                'Get Default setting
                If _defaultSettings(txtName) Is Nothing = False Then
                    settingValue = CType(_defaultSettings(txtName), String)
                End If
            End If

            Return settingValue

        End Function

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	11/16/2004 created
        '''     [cnurse]    01/10/2005 added UrlReferrer code so Cancel returns to previous page
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If Not Page.IsPostBack Then
                'Get Host Settings (used as default)
                _defaultSettings = Common.Globals.HostSettings

                'Get Search Settings (HostSettings if on Host Tab, Module Settings Otherwise)
                Dim objModules As New ModuleController
                If PortalSettings.ActiveTab.ParentId = PortalSettings.SuperTabId Then
                    _settings = Common.Globals.HostSettings
                Else
                    _settings = PortalSettings.GetModuleSettings(ModuleId)
                End If

                txtMaxWordLength.Text = GetSetting("MaxSearchWordLength")
                txtMinWordLength.Text = GetSetting("MinSearchWordLength")
                If GetSetting("SearchIncludeCommon") = "Y" Then
                    chkIncludeCommon.Checked = True
                End If
                If GetSetting("SearchIncludeNumeric") = "Y" Then
                    chkIncludeNumeric.Checked = True
                End If

                ' Store URL Referrer to return to portal
                If Not Request.UrlReferrer Is Nothing Then
                    If Request.UrlReferrer.AbsoluteUri = Request.Url.AbsoluteUri Then
                        ViewState("UrlReferrer") = ""
                    Else
                        ViewState("UrlReferrer") = Convert.ToString(Request.UrlReferrer)
                    End If
                Else
                    ViewState("UrlReferrer") = ""
                End If
            End If

            If Convert.ToString(ViewState("UrlReferrer")) = "" Then
                cmdCancel.Visible = False
            Else
                cmdCancel.Visible = True
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdCancel_Click runs when the Cancel LinkButton is clicked.  It returns the user
        ''' to the referring page
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	11/16/2004 created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(Convert.ToString(Viewstate("UrlReferrer")), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdReIndex_Click runs when the ReIndex LinkButton is clicked.  It re-indexes the
        ''' site (or application if run on Host page)
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	11/16/2004 created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdReIndex_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdReIndex.Click
            Try
                Dim se As New Services.Search.SearchEngine
                If PortalSettings.ActiveTab.ParentId = PortalSettings.SuperTabId Then
                    se.IndexContent()
                Else
                    se.IndexContent(PortalId)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdUpdate_Click runs when the Update LinkButton is clicked.
        ''' It saves the current Search Settings
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/9/2004	Modified
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            Try

                If PortalSettings.ActiveTab.ParentId = PortalSettings.SuperTabId Then
                    Dim objHostSettings As New Entities.Host.HostSettingsController
                    objHostSettings.UpdateHostSetting("MaxSearchWordLength", txtMaxWordLength.Text)
                    objHostSettings.UpdateHostSetting("MinSearchWordLength", txtMinWordLength.Text)
                    objHostSettings.UpdateHostSetting("SearchIncludeCommon", CType(IIf(chkIncludeCommon.Checked, "Y", "N"), String))
                    objHostSettings.UpdateHostSetting("SearchIncludeNumeric", CType(IIf(chkIncludeNumeric.Checked, "Y", "N"), String))

                    ' clear host settings cache
                    DataCache.ClearHostCache(False)

                Else
                    Dim objModules As New Entities.Modules.ModuleController
                    objModules.UpdateModuleSetting(ModuleId, "MaxSearchWordLength", txtMaxWordLength.Text)
                    objModules.UpdateModuleSetting(ModuleId, "MinSearchWordLength", txtMinWordLength.Text)
                    objModules.UpdateModuleSetting(ModuleId, "SearchIncludeCommon", CType(IIf(chkIncludeCommon.Checked, "Y", "N"), String))
                    objModules.UpdateModuleSetting(ModuleId, "SearchIncludeNumeric", CType(IIf(chkIncludeNumeric.Checked, "Y", "N"), String))
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