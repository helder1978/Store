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

Imports System.Collections.Generic
Imports System.IO

Imports ICSharpCode.SharpZipLib.Zip

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Packages
Imports DotNetNuke.UI.Skins

Namespace DotNetNuke.Modules.Admin.Packages

    ''' -----------------------------------------------------------------------------
    ''' Project	 : DotNetNuke
    ''' Class	 : Install
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Supplies the functionality for Install packages to the Portal
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    '''     [cnurse]   07/26/2007    Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Install
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

#Region "Members"

        Private _Installer As Installer
        Private _Package As PackageInfo
        Private _PackageType As PackageType

#End Region

#Region "Public Properties"

        Public ReadOnly Property Installer() As Installer
            Get
                Return _Installer
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Path to the Manifest File
        ''' </summary>
        ''' <history>
        '''     [cnurse]   08/13/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ManifestFile() As String
            Get
                Return CStr(ViewState("ManifestFile"))
            End Get
            Set(ByVal value As String)
                ViewState("ManifestFile") = value
            End Set
        End Property

        Public ReadOnly Property Package() As PackageInfo
            Get
                Return _Package
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the Package Type
        ''' </summary>
        ''' <history>
        '''     [cnurse]   07/26/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property PackageType() As PackageType
            Get
                If _PackageType Is Nothing Then
                    Dim pType As String = Null.NullString
                    If Not String.IsNullOrEmpty(Request.QueryString("ptype")) Then
                        pType = Request.QueryString("ptype")
                    End If
                    _PackageType = PackageController.GetPackageType(pType)
                End If

                Return _PackageType
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the Return Url
        ''' </summary>
        ''' <history>
        '''     [cnurse]   07/26/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property ReturnURL() As String
            Get
                Dim TabID As Integer = PortalSettings.HomeTabId

                If Not Request.Params("rtab") Is Nothing Then
                    TabID = Integer.Parse(Request.Params("rtab"))
                End If
                Return NavigateURL(TabID)
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the Temporary Installation Folder
        ''' </summary>
        ''' <history>
        '''     [cnurse]   08/13/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property TempInstallFolder() As String
            Get
                Return CStr(ViewState("TempInstallFolder"))
            End Get
            Set(ByVal value As String)
                ViewState("TempInstallFolder") = value
            End Set
        End Property

#End Region

#Region "Private Methods"

        Private Sub BindPackage(ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs)
            CreateInstaller()

            If Installer.IsValid Then
                For Each kvp As KeyValuePair(Of String, PackageInfo) In _Installer.InstallerInfo.Packages
                    _Package = kvp.Value
                Next
                ctlPackage.EditMode = UI.WebControls.PropertyEditorMode.View
                ctlPackage.DataSource = _Package
                ctlPackage.DataBind()
            Else
                'Error reading Manifest
                Select Case e.CurrentStepIndex
                    Case 0
                        lblLoadMessage.Text = Localization.GetString("InstallError", LocalResourceFile)
                        phLoadLogs.Controls.Add(Installer.InstallerInfo.Log.GetLogsTable)
                    Case 1
                        lblAcceptMessage.Text = Localization.GetString("InstallError", LocalResourceFile)
                        phAcceptLogs.Controls.Add(Installer.InstallerInfo.Log.GetLogsTable)
                End Select
                e.Cancel = True
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' This routine checks the Access Security
        ''' </summary>
        ''' <history>
        '''     [cnurse]   07/26/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub CheckSecurity()

            Dim allowAccess As Boolean = True
            If PackageType Is Nothing Then
                allowAccess = UserInfo.IsSuperUser
            Else
                Select Case PackageType.SecurityAccessLevel
                    Case 3
                        allowAccess = UserInfo.IsSuperUser
                    Case 2
                        allowAccess = UserInfo.IsInRole(PortalSettings.AdministratorRoleName)
                End Select
            End If
            If Not allowAccess Then
                Response.Redirect(NavigateURL("Access Denied"), True)
            End If
        End Sub

        Private Sub CreateInstaller()
            _Installer = New Installer(TempInstallFolder, ManifestFile, Request.MapPath("."))

            'The Installer is created automatically with a SecurityAccessLevel of Host
            'Check if the User has lowere Security and update as neccessary
            If Not UserInfo.IsSuperUser Then
                If UserInfo.IsInRole(PortalSettings.AdministratorRoleName) Then
                    'Admin User
                    Installer.InstallerInfo.SecurityAccessLevel = SecurityAccessLevel.Admin
                ElseIf ModulePermissionController.HasModulePermission(ModuleConfiguration.ModulePermissions, "EDIT") Then
                    'Has Edit rights
                    Installer.InstallerInfo.SecurityAccessLevel = SecurityAccessLevel.Edit
                ElseIf ModulePermissionController.HasModulePermission(ModuleConfiguration.ModulePermissions, "VIEW") Then
                    'Has View rights
                    Installer.InstallerInfo.SecurityAccessLevel = SecurityAccessLevel.View
                Else
                    Installer.InstallerInfo.SecurityAccessLevel = SecurityAccessLevel.Anonymous
                End If
            End If

            If Me.IsHostMenu Then
                Installer.InstallerInfo.PortalID = Null.NullInteger
            Else
                Installer.InstallerInfo.PortalID = PortalId
            End If

            _Installer.InstallerInfo.ReadManifest()
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' This routine installs the uploaded package
        ''' </summary>
        ''' <history>
        '''     [cnurse]   07/26/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub InstallPackage(ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs)
            CreateInstaller()

            If Installer.IsValid Then
                'Reset Log
                Installer.InstallerInfo.Log.Logs.Clear()

                'Install
                Installer.Install()

                If Not Installer.IsValid Then
                    lblInstallMessage.Text = Localization.GetString("InstallError", LocalResourceFile)
                End If

                phInstallLogs.Controls.Add(Installer.InstallerInfo.Log.GetLogsTable)
            Else
                'Error reading Manifest
                Select Case e.CurrentStepIndex
                    Case 1
                        lblAcceptMessage.Text = Localization.GetString("InstallError", LocalResourceFile)
                        phAcceptLogs.Controls.Add(Installer.InstallerInfo.Log.GetLogsTable)
                    Case 2
                        lblInstallMessage.Text = Localization.GetString("InstallError", LocalResourceFile)
                        phInstallLogs.Controls.Add(Installer.InstallerInfo.Log.GetLogsTable)
                End Select
                e.Cancel = True
            End If
        End Sub

#End Region

#Region "Protected Methods"

        Protected Function GetText(ByVal type As String)
            Dim text As String = Null.NullString
            If type = "Title" Then
                text = Localization.GetString(wizInstall.ActiveStep.Title + ".Title", Me.LocalResourceFile)
            ElseIf type = "Help" Then
                text = Localization.GetString(wizInstall.ActiveStep.Title + ".Help", Me.LocalResourceFile)
            End If
            Return text
        End Function

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The Page_Init runs when the page is initialised
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''     [cnurse]   07/26/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'Customise the Control Title
            If (PackageType IsNot Nothing) AndAlso (Not String.IsNullOrEmpty(PackageType.PackageType)) Then
                ModuleConfiguration.ModuleTitle = String.Format(Localization.GetString("InstallCustomPackage", LocalResourceFile), PackageType.Description)
            Else
                ModuleConfiguration.ModuleTitle = Localization.GetString("InstallPackage", LocalResourceFile)
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The Page_Load runs when the page loads
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''     [cnurse]   07/26/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                CheckSecurity()

                wizInstall.CancelButtonText = "<img src=""" + ApplicationPath + "/images/lt.gif"" border=""0"" /> " + Localization.GetString("Cancel", Me.LocalResourceFile)
                wizInstall.StartNextButtonText = "<img src=""" + ApplicationPath + "/images/rt.gif"" border=""0"" /> " + Localization.GetString("Next", Me.LocalResourceFile)
                wizInstall.StepNextButtonText = "<img src=""" + ApplicationPath + "/images/rt.gif"" border=""0"" /> " + Localization.GetString("Next", Me.LocalResourceFile)
                wizInstall.FinishCompleteButtonText = "<img src=""" + ApplicationPath + "/images/lt.gif"" border=""0"" /> " + Localization.GetString("Cancel", Me.LocalResourceFile)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Sub wizInstall_ActiveStepChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wizInstall.ActiveStepChanged
            Select Case wizInstall.ActiveStepIndex
                Case 2
                    wizInstall.DisplayCancelButton = False
            End Select
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' wizInstall_CancelButtonClick runs when the Cancel Button on the Wizard is clicked.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	08/13/2007	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub wizInstall_CancelButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles wizInstall.CancelButtonClick
            Try
                'Redirect to Definitions page
                Response.Redirect(ReturnURL(), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Sub wizInstall_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles wizInstall.FinishButtonClick
            Try
                'Redirect to Definitions page
                Response.Redirect(ReturnURL(), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Wizard_NextButtonClickruns when the next Button is clicked.  It provides
        '''	a mechanism for cancelling the page change if certain conditions aren't met.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	08/13/2007	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub wizInstall_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles wizInstall.NextButtonClick

            Select Case e.CurrentStepIndex
                Case 0    'Upload Package
                    Dim strFileName As String
                    Dim strExtension As String = ""
                    Dim strMessage As String = ""
                    Dim strInvalid As String = Localization.GetString("InvalidExt", LocalResourceFile)

                    Dim postedFile As HttpPostedFile = cmdBrowse.PostedFile

                    strFileName = System.IO.Path.GetFileName(postedFile.FileName)
                    strExtension = Path.GetExtension(strFileName)

                    If strExtension.ToLower <> ".zip" Then
                        strMessage += strInvalid & " " & strFileName
                    End If

                    If String.IsNullOrEmpty(strMessage) Then
                        If postedFile.FileName <> "" Then
                            _Installer = New Installer(CType(postedFile.InputStream, Stream), Request.MapPath("."))
                            TempInstallFolder = Installer.TempInstallFolder
                            ManifestFile = Path.GetFileName(Installer.InstallerInfo.ManifestFile.TempFileName)
                        Else
                            strMessage = Localization.GetString("NoFile", LocalResourceFile)
                        End If
                    End If

                    If Not String.IsNullOrEmpty(strMessage) Then
                        lblLoadMessage.Text = strMessage
                        e.Cancel = True
                    ElseIf Installer Is Nothing Then
                        lblLoadMessage.Text = Localization.GetString("ZipCriticalError", LocalResourceFile)
                        e.Cancel = True
                    ElseIf Not Installer.IsValid Then
                        lblLoadMessage.Text = Localization.GetString("ZipError", LocalResourceFile)

                        'Error parsing zip
                        phLoadLogs.Controls.Add(Installer.InstallerInfo.Log.GetLogsTable)
                        e.Cancel = True
                    Else
                        BindPackage(e)
                    End If
                Case 1  'Accept Terms
                    If chkAcceptLicense.Checked Then
                        InstallPackage(e)
                    Else
                        lblAcceptMessage.Text = Localization.GetString("AcceptTerms", LocalResourceFile)
                        e.Cancel = True

                        'Rebind package
                        BindPackage(e)
                    End If

            End Select


        End Sub

#End Region

    End Class

End Namespace
