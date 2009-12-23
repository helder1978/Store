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

Imports DotNetNuke.UI.Skins
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Definitions
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Security.Permissions


Namespace DotNetNuke.Modules.Admin.Modules

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The ModuleSettingsPage PortalModuleBase is used to edit the settings for a 
    ''' module.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	10/18/2004	documented
    ''' 	[cnurse]	10/19/2004	modified to support custm module specific settings
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial  Class ModuleSettingsPage
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

#Region "Controls"

        'Module Section

        'General Section

        'Security Section

        'Specific Settings Section
        Protected ctlSpecific As ModuleSettingsBase

        'Page Settings Section

        'Appearance Section

        'Other Section

        'tasks

#End Region

#Region "Private Members"

        Private Shadows ModuleId As Integer = -1
        Private Shadows TabModuleId As Integer = -1

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindData loads the settings from the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/18/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindData()

            Dim HasSetViewPermissions As Boolean = False
            Dim objModulePermissionController As New DotNetNuke.Security.Permissions.ModulePermissionController
            Dim objModulePermissionsCollection As DotNetNuke.Security.Permissions.ModulePermissionCollection = objModulePermissionController.GetModulePermissionsCollectionByModuleID(ModuleId, TabId)

            ' declare roles
            Dim arrAvailableAuthViewRoles As New ArrayList
            Dim arrAvailableAuthEditRoles As New ArrayList

            ' add an entry of All Users for the View roles
            arrAvailableAuthViewRoles.Add(New ListItem("All Users", glbRoleAllUsers))
            ' add an entry of Unauthenticated Users for the View roles
            arrAvailableAuthViewRoles.Add(New ListItem("Unauthenticated Users", glbRoleUnauthUser))
            ' add an entry of All Users for the Edit roles
            arrAvailableAuthEditRoles.Add(New ListItem("All Users", glbRoleAllUsers))

            ' process portal roles
            Dim objRoles As New RoleController
            Dim objRole As RoleInfo
            Dim arrRoleInfo As ArrayList = objRoles.GetPortalRoles(PortalId)
            For Each objRole In arrRoleInfo
                arrAvailableAuthViewRoles.Add(New ListItem(objRole.RoleName, objRole.RoleID.ToString))
            Next
            For Each objRole In arrRoleInfo
                arrAvailableAuthEditRoles.Add(New ListItem(objRole.RoleName, objRole.RoleID.ToString))
            Next

            ' get module
            Dim objModules As New ModuleController
            Dim objModule As ModuleInfo = objModules.GetModule(ModuleId, TabId, False)
            If Not objModule Is Nothing Then
                ' configure grid
                Dim objDeskMod As New DesktopModuleController
                Dim desktopModule As DesktopModuleInfo = objDeskMod.GetDesktopModule(objModule.DesktopModuleID)
                dgPermissions.ResourceFile = Common.Globals.ApplicationPath + "/DesktopModules/" + desktopModule.FolderName + "/" + Services.Localization.Localization.LocalResourceDirectory + "/" + Services.Localization.Localization.LocalSharedResourceFile

                chkInheritPermissions.Checked = objModule.InheritViewPermissions
                dgPermissions.InheritViewPermissionsFromTab = objModule.InheritViewPermissions

                txtTitle.Text = objModule.ModuleTitle
                ctlIcon.Url = objModule.IconFile
                
                If Not cboTab.Items.FindByValue(objModule.TabID.ToString) Is Nothing Then
                    cboTab.Items.FindByValue(objModule.TabID.ToString).Selected = True
                End If

                chkAllTabs.Checked = objModule.AllTabs
                cboVisibility.SelectedIndex = objModule.Visibility

                Dim objModuleDefController As New ModuleDefinitionController
                Dim objModuleDef As ModuleDefinitionInfo = objModuleDefController.GetModuleDefinition(objModule.ModuleDefID)
                If objModuleDef.DefaultCacheTime = Null.NullInteger Then
                    rowCache.Visible = False
                Else
                    txtCacheTime.Text = objModule.CacheTime.ToString
                End If

                Dim strRole As String
                Dim objListItem As ListItem

                ' populate view roles
                Dim arrAssignedAuthViewRoles As New ArrayList
                Dim arrAuthViewRoles As Array = Split(objModule.AuthorizedViewRoles, ";")
                For Each strRole In arrAuthViewRoles
                    If strRole <> "" Then
                        For Each objListItem In arrAvailableAuthViewRoles
                            If objListItem.Value = strRole Then
                                arrAssignedAuthViewRoles.Add(objListItem)
                                arrAvailableAuthViewRoles.Remove(objListItem)
                                Exit For
                            End If
                        Next
                    End If
                Next

                ' populate edit roles
                Dim arrAssignedAuthEditRoles As New ArrayList
                Dim arrAuthEditRoles As Array = Split(objModule.AuthorizedEditRoles, ";")
                For Each strRole In arrAuthEditRoles
                    If strRole <> "" Then
                        For Each objListItem In arrAvailableAuthEditRoles
                            If objListItem.Value = strRole Then
                                arrAssignedAuthEditRoles.Add(objListItem)
                                arrAvailableAuthEditRoles.Remove(objListItem)
                                Exit For
                            End If
                        Next
                    End If
                Next

                cboAlign.Items.FindByValue(objModule.Alignment).Selected = True
                cboTab.Items.FindByValue(CType(TabId, String)).Selected = True
                txtColor.Text = objModule.Color
                txtBorder.Text = objModule.Border

                txtHeader.Text = objModule.Header
                txtFooter.Text = objModule.Footer

                If Not Null.IsNull(objModule.StartDate) Then
                    txtStartDate.Text = objModule.StartDate.ToShortDateString
                End If
                If Not Null.IsNull(objModule.EndDate) Then
                    txtEndDate.Text = objModule.EndDate.ToShortDateString
                End If

                ctlModuleContainer.Width = "250px"
                ctlModuleContainer.SkinRoot = UI.Skins.SkinInfo.RootContainer
                ctlModuleContainer.SkinSrc = objModule.ContainerSrc

                chkDisplayTitle.Checked = objModule.DisplayTitle
                chkDisplayPrint.Checked = objModule.DisplayPrint
                chkDisplaySyndicate.Checked = objModule.DisplaySyndicate
            End If


        End Sub

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/18/2004	documented
        ''' 	[cnurse]	10/19/2004	modified to support custm module specific settings
        '''     [vmasanas]  11/28/2004  modified to support modules in admin tabs
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                Dim objModules As New ModuleController
                ' Verify that the current user has access to edit this module
                If PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) = False And PortalSecurity.IsInRoles(PortalSettings.ActiveTab.AdministratorRoles.ToString) = False Then
                    Response.Redirect(NavigateURL("Access Denied"), True)
                End If

                'this needs to execute always to the client script code is registred in InvokePopupCal
                cmdStartCalendar.NavigateUrl = Common.Utilities.Calendar.InvokePopupCal(txtStartDate)
                cmdEndCalendar.NavigateUrl = Common.Utilities.Calendar.InvokePopupCal(txtEndDate)

                If Page.IsPostBack = False Then
                    ctlIcon.FileFilter = glbImageFileTypes

                    dgPermissions.TabId = PortalSettings.ActiveTab.TabID
                    dgPermissions.ModuleID = ModuleId

					DotNetNuke.UI.Utilities.ClientAPI.AddButtonConfirm(cmdDelete, Services.Localization.Localization.GetString("DeleteItem"))

                    cboTab.DataSource = GetPortalTabs(PortalSettings.DesktopTabs, -1, False, True, False, False, True)
                    cboTab.DataBind()
                    'if is and admin or host tab, then add current tab
                    If PortalSettings.ActiveTab.ParentId = PortalSettings.AdminTabId Or PortalSettings.ActiveTab.ParentId = PortalSettings.SuperTabId Then
                        cboTab.Items.Insert(0, New ListItem(PortalSettings.ActiveTab.TabName, PortalSettings.ActiveTab.TabID.ToString))
                    End If

                    ' tab administrators can only manage their own tab
                    If PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) = False Then
                        chkAllTabs.Enabled = False
                        chkDefault.Enabled = False
                        chkAllModules.Enabled = False
                        cboTab.Enabled = False
                    End If

                    If ModuleId <> -1 Then
                        BindData()
                    Else
                        cboVisibility.SelectedIndex = 0       ' maximized
                        chkAllTabs.Checked = False
                        cmdDelete.Visible = False
                    End If

                    'Set visibility of Specific Settings
                    If ctlSpecific Is Nothing = False Then
                        'Get the module settings from the PortalSettings and pass the
                        'two settings hashtables to the sub control to process
                        ctlSpecific.LoadSettings()
                        dshSpecific.Visible = True
                        tblSpecific.Visible = True
                    Else
                        dshSpecific.Visible = False
                        tblSpecific.Visible = False
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' chkInheritPermissions_CheckedChanged runs when the Inherit View Permissions
        '''	check box is changed
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/18/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub chkInheritPermissions_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInheritPermissions.CheckedChanged
            If chkInheritPermissions.Checked Then
                dgPermissions.InheritViewPermissionsFromTab = True
            Else
                dgPermissions.InheritViewPermissionsFromTab = False
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
        ''' 	[cnurse]	10/18/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdDelete_Click runs when the Delete LinkButton is clicked.
        ''' It deletes the current portal form the Database.  It can only run in Host
        ''' (SuperUser) mode
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/18/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
            Try

                Dim objModules As New ModuleController

                objModules.DeleteTabModule(TabId, ModuleId)

                Response.Redirect(NavigateURL(), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdUpdate_Click runs when the Update LinkButton is clicked.
        ''' It saves the current Site Settings
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/18/2004	documented
        ''' 	[cnurse]	10/19/2004	modified to support custm module specific settings
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdUpdate_Click(ByVal Sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            Try
                If Page.IsValid Then
                    Dim objModules As New ModuleController
                    Dim AllTabsChanged As Boolean = False

                    ' tab administrators can only manage their own tab
                    If PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) = False Then
                        chkAllTabs.Enabled = False
                        chkDefault.Enabled = False
                        chkAllModules.Enabled = False
                        cboTab.Enabled = False
                    End If

                    ' update module
                    Dim objModule As ModuleInfo = objModules.GetModule(ModuleId, TabId, False)

                    objModule.ModuleID = ModuleId
                    objModule.ModuleTitle = txtTitle.Text
                    objModule.Alignment = cboAlign.SelectedItem.Value
                    objModule.Color = txtColor.Text
                    objModule.Border = txtBorder.Text
                    objModule.IconFile = ctlIcon.Url
                    If txtCacheTime.Text <> "" Then
                        objModule.CacheTime = Int32.Parse(txtCacheTime.Text)
                    Else
                        objModule.CacheTime = 0
                    End If
                    objModule.TabID = TabId
                    If objModule.AllTabs <> chkAllTabs.Checked Then
                        AllTabsChanged = True
                    End If
                    objModule.AllTabs = chkAllTabs.Checked
                    Select Case Int32.Parse(cboVisibility.SelectedItem.Value)
                        Case 0 : objModule.Visibility = VisibilityState.Maximized
                        Case 1 : objModule.Visibility = VisibilityState.Minimized
                        Case 2 : objModule.Visibility = VisibilityState.None
                    End Select
                    objModule.IsDeleted = False
                    objModule.Header = txtHeader.Text
                    objModule.Footer = txtFooter.Text
                    If txtStartDate.Text <> "" Then
                        objModule.StartDate = Convert.ToDateTime(txtStartDate.Text)
                    Else
                        objModule.StartDate = Null.NullDate
                    End If
                    If txtEndDate.Text <> "" Then
                        objModule.EndDate = Convert.ToDateTime(txtEndDate.Text)
                    Else
                        objModule.EndDate = Null.NullDate
                    End If
                    objModule.ContainerSrc = ctlModuleContainer.SkinSrc
                    objModule.ModulePermissions = dgPermissions.Permissions
                    objModule.InheritViewPermissions = chkInheritPermissions.Checked
                    objModule.DisplayTitle = chkDisplayTitle.Checked
                    objModule.DisplayPrint = chkDisplayPrint.Checked
                    objModule.DisplaySyndicate = chkDisplaySyndicate.Checked
                    objModule.IsDefaultModule = chkDefault.Checked
                    objModule.AllModules = chkAllModules.Checked
                    objModules.UpdateModule(objModule)

                    'Update Custom Settings
                    If ctlSpecific Is Nothing = False Then
                        ctlSpecific.UpdateSettings()
                    End If

                    'These Module Copy/Move statements must be 
                    'at the end of the Update as the Controller code assumes all the 
                    'Updates to the Module have been carried out.

                    'Check if the Module is to be Moved to a new Tab
                    If Not chkAllTabs.Checked Then
                        Dim newTabId As Integer = Int32.Parse(cboTab.SelectedItem.Value)
                        If TabId <> newTabId Then
                            objModules.MoveModule(ModuleId, TabId, newTabId, "")
                        End If
                    End If

                    ''Check if Module is to be Added/Removed from all Tabs
                    If AllTabsChanged Then
                        Dim arrTabs As ArrayList = GetPortalTabs(PortalSettings.DesktopTabs, False, True)
                        If chkAllTabs.Checked Then
                            objModules.CopyModule(ModuleId, TabId, arrTabs, True)
                        Else
                            objModules.DeleteAllModules(ModuleId, TabId, arrTabs, False, False)
                        End If
                    End If

                    ' Navigate back to admin page
                    Response.Redirect(NavigateURL(), True)

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

            Dim objModules As New ModuleController
            Dim objModuleControlInfo As ModuleControlInfo
            Dim arrModuleControls As New ArrayList

            ' get ModuleId
            If Not (Request.QueryString("ModuleId") Is Nothing) Then
                ModuleId = Int32.Parse(Request.QueryString("ModuleId"))
            End If

            ' get module
            Dim objModule As ModuleInfo = objModules.GetModule(ModuleId, TabId, False)
            If Not objModule Is Nothing Then
                TabModuleId = objModule.TabModuleID

                'get Settings Control(s)
                arrModuleControls = ModuleControlController.GetModuleControlsByKey("Settings", objModule.ModuleDefID)

                If arrModuleControls.Count > 0 Then
                    objModuleControlInfo = CType(arrModuleControls(0), ModuleControlInfo)
                    Dim src As String = "~/" + objModuleControlInfo.ControlSrc
                    ctlSpecific = CType(LoadControl(src), ModuleSettingsBase)
                    ctlSpecific.ID = System.IO.Path.GetFileNameWithoutExtension(src).Replace("."c, "-"c)
                    ctlSpecific.ModuleConfiguration = objModule
                    dshSpecific.Text = Services.Localization.Localization.LocalizeControlTitle(objModuleControlInfo.ControlTitle, objModuleControlInfo.ControlSrc, "settings")
                    pnlSpecific.Controls.Add(ctlSpecific)

                    If Services.Localization.Localization.GetString(Entities.Modules.Actions.ModuleActionType.HelpText, ctlSpecific.LocalResourceFile) <> "" Then
                        rowspecifichelp.Visible = True
                        imgSpecificHelp.AlternateText = Services.Localization.Localization.GetString(Entities.Modules.Actions.ModuleActionType.ModuleHelp, Services.Localization.Localization.GlobalResourceFile)
                        lnkSpecificHelp.Text = Services.Localization.Localization.GetString(Entities.Modules.Actions.ModuleActionType.ModuleHelp, Services.Localization.Localization.GlobalResourceFile)
                        lnkSpecificHelp.NavigateUrl = NavigateURL(TabId, "Help", "ctlid=" & objModuleControlInfo.ModuleControlID.ToString, "moduleid=" & ModuleId)
                    Else
                        rowspecifichelp.Visible = False
                    End If
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
