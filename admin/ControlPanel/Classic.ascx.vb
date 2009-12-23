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
Imports DotNetNuke.Entities.Modules.Definitions


Namespace DotNetNuke.UI.ControlPanels

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The Classic ControlPanel provides athe Classic Page/Module manager
	''' </summary>
    ''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cnurse]	10/06/2004	Updated to reflect design changes for Help, 508 support
	'''                       and localisation
	''' </history>
	''' -----------------------------------------------------------------------------
    Partial  Class Classic

        Inherits ControlPanelBase

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/06/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                If Not Page.IsPostBack Then
                    'Localization
                    lblAdmin.Text = Localization.GetString("Admin", Me.LocalResourceFile)
                    cmdAddTab.Text = Localization.GetString("AddTab", Me.LocalResourceFile)
                    cmdAddTab.ToolTip = Localization.GetString("AddTab.ToolTip", Me.LocalResourceFile)
                    cmdEditTab.Text = Localization.GetString("EditTab", Me.LocalResourceFile)
                    cmdEditTab.ToolTip = Localization.GetString("EditTab.ToolTip", Me.LocalResourceFile)
                    cmdCopyTab.Text = Localization.GetString("CopyTab", Me.LocalResourceFile)
                    cmdCopyTab.ToolTip = Localization.GetString("CopyTab.ToolTip", Me.LocalResourceFile)
                    cmdHelpShow.AlternateText = Localization.GetString("ShowHelp.AlternateText", Me.LocalResourceFile)
                    cmdHelpHide.AlternateText = Localization.GetString("HideHelp.AlternateText", Me.LocalResourceFile)
                    lblModule.Text = Localization.GetString("Module", Me.LocalResourceFile)
                    lblPane.Text = Localization.GetString("Pane", Me.LocalResourceFile)
                    lblAlign.Text = Localization.GetString("Align", Me.LocalResourceFile)
                    cmdAdd.Text = Localization.GetString("AddModule", Me.LocalResourceFile)
                    cmdAdd.ToolTip = Localization.GetString("AddModule.ToolTip", Me.LocalResourceFile)
                    chkContent.Text = Localization.GetString("Content.Text", Me.LocalResourceFile)
                    chkPreview.Text = Localization.GetString("Preview.Text", Me.LocalResourceFile)
                    chkContent.ToolTip = Localization.GetString("Content.ToolTip", Me.LocalResourceFile)
                    chkPreview.ToolTip = Localization.GetString("Preview.ToolTip", Me.LocalResourceFile)

                    cmdAddTab.NavigateUrl = NavigateURL("Tab")
                    If Not PortalSettings.ActiveTab.IsAdminTab Then
                        cmdEditTab.NavigateUrl = NavigateURL(PortalSettings.ActiveTab.TabID, "Tab", "action=edit")
                        cmdCopyTab.NavigateUrl = NavigateURL(PortalSettings.ActiveTab.TabID, "Tab", "action=copy")
                    Else
                        cmdEditTab.Visible = False
                        cmdCopyTab.Visible = False
                    End If

                    cmdHelpShow.Visible = False
                    cmdHelpHide.Visible = False

                    Dim objDesktopModules As New DesktopModuleController
                    cboDesktopModules.DataSource = objDesktopModules.GetDesktopModulesByPortal(PortalSettings.PortalId)
                    cboDesktopModules.DataBind()

                    Dim intItem As Integer
                    For intItem = 0 To PortalSettings.ActiveTab.Panes.Count - 1
                        cboPanes.Items.Add(Convert.ToString(PortalSettings.ActiveTab.Panes(intItem)))
                    Next intItem
                    cboPanes.Items.FindByValue(glbDefaultPane).Selected = True

                    If cboAlign.Items.Count > 0 Then
                        cboAlign.SelectedIndex = 0
                    End If

                    chkContent.Checked = ShowContent
                    chkPreview.Checked = IsPreview

                    If (PortalSecurity.IsInRole(PortalSettings.AdministratorRoleName.ToString) = False And PortalSecurity.IsInRoles(PortalSettings.ActiveTab.AdministratorRoles.ToString) = False) Then
                        tblIconBarModule.Visible = False
                        tblIconBarTab.Visible = False
                        lblDescription.Visible = False
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdAdd_Click runs when the Add Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/06/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        '''     [vmasanas]  01/07/2005  Modified to add view perm. to all roles with edit perm.
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
            Try
                Dim permissionType As ViewPermissionType = ViewPermissionType.View
                Dim position As Integer = Null.NullInteger

                ' save to database
                AddNewModule("", Int32.Parse(cboDesktopModules.SelectedItem.Value), cboPanes.SelectedItem.Text, Null.NullInteger, ViewPermissionType.View, cboAlign.SelectedItem.Value)

                ' Redirect to the same page to pick up changes
                Response.Redirect(Request.RawUrl, True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' chkContent_CheckedChanged runs when the Content check box is changed
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/06/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub chkContent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkContent.CheckedChanged
            Try

                SetContentMode(chkContent.Checked)

                Response.Redirect(Request.RawUrl, True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' chkPreview_CheckedChanged runs when the Preview check box is changed
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/06/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub chkPreview_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPreview.CheckedChanged
            Try
                SetPreviewMode(chkPreview.Checked)

                Response.Redirect(Request.RawUrl, True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdHelpHide_Click runs when the Hide Help Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/06/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdHelpHide_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdHelpHide.Click
            Try
                lblDescription.Text = ""

                cmdHelpShow.Visible = True
                cmdHelpHide.Visible = False

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdHelpShow_Click runs when the Hide Help Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/06/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdHelpShow_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdHelpShow.Click
            Try
                Dim objDesktopModules As New DesktopModuleController
                Dim objDesktopModule As DesktopModuleInfo

                objDesktopModule = objDesktopModules.GetDesktopModule(Int32.Parse(cboDesktopModules.SelectedItem.Value))
                If Not objDesktopModule Is Nothing Then
                    lblDescription.Text = "<br>" & objDesktopModule.Description & "<br>"
                End If

                cmdHelpShow.Visible = False
                cmdHelpHide.Visible = True

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

#Region " Web Form Designer Generated Code "


        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

            Me.ID = "Classic.ascx"

        End Sub

#End Region

    End Class

End Namespace
