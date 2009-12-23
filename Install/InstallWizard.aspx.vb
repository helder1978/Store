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

Imports System.Data.Common
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Framework.Providers
Imports DotNetNuke.Modules.Admin.ResourceInstaller
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Services.Localization

Namespace DotNetNuke.Services.Install

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The InstallWizard class provides the Installation Wizard for DotNetNuke
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	01/23/2007 Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class InstallWizard
        Inherits System.Web.UI.Page : Implements DotNetNuke.UI.Utilities.IClientAPICallbackEventHandler

#Region "Private Members"

        Private connectionString As String = Null.NullString
        Private dataProvider As DataProvider = dataProvider.Instance
        Private _localesFile As String = "/Install/App_LocalResources/Locales.xml"

        Private _installTemplate As XmlDocument

#End Region

#Region "Protected Members"

        Protected ReadOnly Property AssemblyVersion() As Integer()
            Get
                Return Services.Upgrade.Upgrade.GetVersion(glbAppVersion)
            End Get
        End Property

        Protected ReadOnly Property DatabaseVersion() As Integer()
            Get
                Dim intVersion(2) As Integer
                Dim strDataBaseVersion As String = GetDatabaseVersion(".")
                If String.IsNullOrEmpty(strDataBaseVersion) OrElse strDataBaseVersion.ToUpper().StartsWith("ERROR") Then
                    intVersion(0) = 0
                    intVersion(1) = 0
                    intVersion(2) = 0
                Else
                    intVersion = Services.Upgrade.Upgrade.GetVersion(strDataBaseVersion)
                End If
                Return intVersion
            End Get
        End Property

        Protected ReadOnly Property BaseVersion() As Integer()
            Get
                Return Services.Upgrade.Upgrade.GetInstallVersion(InstallTemplate)
            End Get
        End Property

        Protected ReadOnly Property InstallTemplate() As XmlDocument
            Get
                If _installTemplate Is Nothing Then
                    _installTemplate = New XmlDocument
                    Services.Upgrade.Upgrade.GetInstallTemplate(_installTemplate)
                End If
                Return _installTemplate
            End Get
        End Property

        Protected LocalResourceFile As String = "~/Install/App_LocalResources/InstallWizard.aspx.resx"

        Protected Property PermissionsValid() As Boolean
            Get
                Dim _Valid As Boolean = False
                If Not ViewState("PermissionsValid") Is Nothing Then
                    _Valid = Convert.ToBoolean(ViewState("PermissionsValid"))
                End If
                Return _Valid
            End Get
            Set(ByVal value As Boolean)
                ViewState("PermissionsValid") = value
            End Set
        End Property

        Protected Property PortalId() As Integer
            Get
                Dim _PortalId As Integer = Null.NullInteger
                If Not ViewState("PortalId") Is Nothing Then
                    _PortalId = Convert.ToInt32(ViewState("PortalId"))
                End If
                Return _PortalId
            End Get
            Set(ByVal value As Integer)
                ViewState("PortalId") = value
            End Set
        End Property

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindConnectionString binds the connection String info
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/23/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindConnectionString()
            Dim connection As String = Config.GetConnectionString()
            Dim connectionParams As String() = connection.Split(";")
            For Each connectionParam As String In connection.Split(";")
                Dim index As Integer = connectionParam.IndexOf("=")
                If index > 0 Then
                    Dim key As String = connectionParam.Substring(0, index)
                    Dim value As String = connectionParam.Substring(index + 1)
                    Select Case key.ToLower
                        Case "server", "data source", "address", "addr", "network address"
                            txtServer.Text = value
                        Case "database", "initial catalog"
                            txtDatabase.Text = value
                        Case "uid", "user id", "user"
                            txtUserId.Text = value
                        Case "pwd", "password"
                            txtPassword.Text = value
                        Case "integrated security"
                            chkIntegrated.Checked = (value.ToLower = "true")
                        Case "attachdbfilename"
                            txtFile.Text = value.Replace("|DataDirectory|", "")
                    End Select
                End If
            Next
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindDatabases binds the supported databases
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/23/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindDatabases()

            If (Config.GetDefaultProvider("data").Name = "SqlDataProvider") Then

                Dim connection As String = Config.GetConnectionString()
                If connection.ToLower().Contains("attachdbfilename") Then
                    rblDatabases.Items.FindByValue("SQLFile").Selected = True
                Else
                    rblDatabases.Items.FindByValue("SQLDatabase").Selected = True
                End If
            End If

            If (Config.GetDefaultProvider("data").Name = "OracleDataProvider") Then
                rblDatabases.Items.Add(New ListItem(LocalizeString("Oracle"), "Oracle"))
                rblDatabases.SelectedIndex = 0
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindLanguages binds the languages checkbox list
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/20/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindLanguages()
            Dim arrFiles As String()
            Dim strFile As String

            Dim InstallPath As String = ApplicationMapPath & "\Install\Language"
            lstLanguages.Items.Clear()
            If Directory.Exists(InstallPath) Then
                arrFiles = Directory.GetFiles(InstallPath)
                Dim iFile As Integer = 0
                For Each strFile In arrFiles
                    Dim strResource As String = strFile.Replace(InstallPath + "\", "")
                    If strResource.ToLower.EndsWith(".zip") OrElse strResource.ToLower.EndsWith(".resources") Then
                        Dim language As New ListItem()
                        language.Value = strResource
                        language.Selected = False
                        language.Enabled = True
                        language.Text = strResource.Replace(".zip", "").Replace(".resources", "")

                        lstLanguages.Items.Add(language)
                    End If
                Next
            End If

            If lstLanguages.Items.Count > 0 Then
                lblNoLanguages.Visible = False
            Else
                lblNoLanguages.Visible = True
                lblNoLanguages.Text = LocalizeString("NoLanguages")
            End If
            lblLanguagesError.Text = Null.NullString
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindModules binds the modules checkbox list
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/19/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindModules()
            Dim arrFiles As String()
            Dim strFile As String

            Dim InstallPath As String = ApplicationMapPath & "\Install\Module"
            lstModules.Items.Clear()
            If Directory.Exists(InstallPath) Then
                arrFiles = Directory.GetFiles(InstallPath)
                Dim iFile As Integer = 0
                For Each strFile In arrFiles
                    Dim strResource As String = strFile.Replace(InstallPath + "\", "")
                    If strResource.ToLower.EndsWith(".zip") OrElse strResource.ToLower.EndsWith(".resources") Then
                        Dim moduleItem As ListItem = New ListItem()
                        ' *.zip packages are installed by default
                        If strResource.ToLower.EndsWith(".zip") Then
                            moduleItem.Selected = True
                            moduleItem.Enabled = False
                        Else ' *.resources packages will be optional
                            moduleItem.Selected = False
                            moduleItem.Enabled = True
                        End If
                        moduleItem.Value = strResource
                        strResource = strResource.Replace(".zip", "")
                        strResource = strResource.Replace(".resources", "")
                        strResource = strResource.Replace("_Install", ")")
                        strResource = strResource.Replace("_Source", ")")
                        strResource = strResource.Replace("_", " (")
                        moduleItem.Text = strResource

                        lstModules.Items.Add(moduleItem)
                    End If
                Next
            End If

            If lstModules.Items.Count > 0 Then
                lblNoModules.Visible = False
            Else
                lblNoModules.Visible = True
                lblNoModules.Text = LocalizeString("NoModules")
            End If
            lblModulesError.Text = Null.NullString
        End Sub

        Private Sub BindPackages()
            Dim arrFiles As String()
            Dim strFile As String

            Dim InstallPath As String = ApplicationMapPath & "\Install\Package"
            lstPackages.Items.Clear()
            If Directory.Exists(InstallPath) Then
                arrFiles = Directory.GetFiles(InstallPath)
                Dim iFile As Integer = 0
                For Each strFile In arrFiles
                    Dim strResource As String = strFile.Replace(InstallPath + "\", "")
                    If strResource.ToLower.EndsWith(".zip") OrElse strResource.ToLower.EndsWith(".resources") Then
                        Dim package As New ListItem()
                        ' *.zip packages are installed by default
                        If strResource.ToLower.EndsWith(".zip") Then
                            package.Selected = True
                            package.Enabled = False
                        Else ' *.resources packages will be optional
                            package.Selected = False
                            package.Enabled = True
                        End If
                        package.Value = strResource
                        strResource = strResource.Replace(".zip", "")
                        strResource = strResource.Replace(".resources", "")
                        strResource = strResource.Replace("_Install", ")")
                        strResource = strResource.Replace("_Source", ")")
                        strResource = strResource.Replace("_", " (")
                        package.Text = strResource

                        lstPackages.Items.Add(package)
                    End If
                Next
            End If

            If lstPackages.Items.Count > 0 Then
                lblNoPackages.Visible = False
            Else
                lblNoPackages.Visible = True
                lblNoPackages.Text = LocalizeString("NoPackages")
            End If
            lblPackagesError.Text = Null.NullString
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindPermissions binds the permissions checkbox list
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/23/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindPermissions(ByVal test As Boolean)

            PermissionsValid = True

            lstPermissions.Items.Clear()

            'FolderCreate
            Dim permissionItem As New ListItem()
            If test Then permissionItem.Selected = VerifyFolderCreate()
            permissionItem.Enabled = False
            permissionItem.Text = LocalizeString("FolderCreate")
            lstPermissions.Items.Add(permissionItem)

            'FileCreate
            permissionItem = New ListItem()
            If test Then permissionItem.Selected = VerifyFileCreate()
            permissionItem.Enabled = False
            permissionItem.Text = LocalizeString("FileCreate")
            lstPermissions.Items.Add(permissionItem)

            'FileDelete
            permissionItem = New ListItem()
            If test Then permissionItem.Selected = VerifyFileDelete()
            permissionItem.Enabled = False
            permissionItem.Text = LocalizeString("FileDelete")
            lstPermissions.Items.Add(permissionItem)

            'FolderDelete
            permissionItem = New ListItem()
            If test Then permissionItem.Selected = VerifyFolderDelete()
            permissionItem.Enabled = False
            permissionItem.Text = LocalizeString("FolderDelete")
            lstPermissions.Items.Add(permissionItem)

            If test Then
                If PermissionsValid Then
                    lblPermissionsError.Text = LocalizeString("PermissionsOk")
                Else
                    lblPermissionsError.Text = LocalizeString("PermissionsError")
                End If
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindPortal binds the portal information
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/19/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindPortal()
            Dim node As XmlNode = InstallTemplate.SelectSingleNode("//dotnetnuke/portals/portal")
            If Not node Is Nothing Then
                Dim adminNode As XmlNode = node.SelectSingleNode("administrator")
                usrAdmin.FirstName = XmlUtils.GetNodeValue(adminNode, "firstname")
                usrAdmin.LastName = XmlUtils.GetNodeValue(adminNode, "lastname")
                usrAdmin.UserName = XmlUtils.GetNodeValue(adminNode, "username")
                usrAdmin.Email = XmlUtils.GetNodeValue(adminNode, "email")
                txtPortalTitle.Text = XmlUtils.GetNodeValue(node, "portalname")

                Dim strTemplate As String = XmlUtils.GetNodeValue(node, "templatefile")
                Dim strFolder As String = Common.Globals.HostMapPath
                If System.IO.Directory.Exists(strFolder) Then
                    cboPortalTemplate.Items.Clear()
                    Dim fileEntries As String() = System.IO.Directory.GetFiles(strFolder, "*.template")

                    For Each strFileName As String In fileEntries
                        If Path.GetFileNameWithoutExtension(strFileName) = "admin" Then
                            'lblMessage.Text = ""
                        Else
                            cboPortalTemplate.Items.Add(Path.GetFileNameWithoutExtension(strFileName))
                        End If
                    Next

                    If cboPortalTemplate.Items.Count = 0 Then
                        'lblMessage.Text = Localization.GetString("PortalMissing", Me.LocalResourceFile)
                    End If

                    If Not cboPortalTemplate.Items.FindByValue(strTemplate.Replace(".template", "")) Is Nothing Then
                        cboPortalTemplate.Items.FindByValue(strTemplate.Replace(".template", "")).Selected = True
                    Else
                        cboPortalTemplate.SelectedIndex = 0
                    End If
                End If
            End If
            lblPortalError.Text = Null.NullString
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindSkins binds the skins checkbox list
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/16/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindSkins()
            Dim arrFiles As String()
            Dim strFile As String

            Dim InstallPath As String = ApplicationMapPath & "\Install\Skin"
            lstSkins.Items.Clear()
            If Directory.Exists(InstallPath) Then
                arrFiles = Directory.GetFiles(InstallPath)
                Dim iFile As Integer = 0
                For Each strFile In arrFiles
                    Dim strResource As String = strFile.Replace(InstallPath + "\", "")
                    If strResource.ToLower.EndsWith(".zip") OrElse strResource.ToLower.EndsWith(".resources") Then
                        Dim skinItem As New ListItem()
                        skinItem.Value = strResource
                        skinItem.Text = strResource.Replace(".zip", "").Replace(".resources", "")
                        skinItem.Selected = (skinItem.Text = glbDefaultSkinFolder.Replace("/", ""))
                        skinItem.Enabled = Not (skinItem.Text = glbDefaultSkinFolder.Replace("/", ""))

                        lstSkins.Items.Add(skinItem)
                    End If
                Next
            End If

            InstallPath = ApplicationMapPath & "\Install\Container"
            If Directory.Exists(InstallPath) Then
                arrFiles = Directory.GetFiles(InstallPath)
                Dim iFile As Integer = 0
                For Each strFile In arrFiles
                    Dim strResource As String = strFile.Replace(InstallPath + "\", "")
                    If strResource.ToLower.EndsWith(".zip") OrElse strResource.ToLower.EndsWith(".resources") Then
                        Dim skinItem As New ListItem()
                        skinItem.Value = strResource
                        skinItem.Selected = False
                        skinItem.Enabled = True
                        skinItem.Text = strResource.Replace(".zip", "").Replace(".resources", "")

                        lstContainers.Items.Add(skinItem)
                    End If
                Next
            End If

            If lstSkins.Items.Count > 0 Then
                lblNoSkins.Visible = False
            Else
                lblNoSkins.Visible = True
                lblNoSkins.Text = LocalizeString("NoSkins")
            End If
            If lstContainers.Items.Count > 0 Then
                lblNoContainers.Visible = False
            Else
                lblNoContainers.Visible = True
                lblNoContainers.Text = LocalizeString("NoContainers")
            End If
            lblSkinsError.Text = Null.NullString
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindSiperUser binds the superuser information
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/16/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindSuperUser()
            Dim superUser As UserInfo = Services.Upgrade.Upgrade.GetSuperUser(InstallTemplate, False)

            If Not superUser Is Nothing Then
                usrHost.FirstName = superUser.FirstName
                usrHost.LastName = superUser.LastName
                usrHost.UserName = superUser.Username
                usrHost.Email = superUser.Email
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' EnableButton enables/Disables a Navigation Button
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/28/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub EnableButton(ByVal button As LinkButton, ByVal enabled As Boolean)
            If Not button Is Nothing Then
                button.OnClientClick = "return !checkDisabled(this);"
                If enabled Then
                    button.CssClass = "WizardButton"
                Else
                    button.CssClass = "WizardButtonDisabled"
                End If
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetInstallerLocales gets an ArrayList of the Locales
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/23/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetInstallerLocales() As ArrayList

            Dim supportedLocales As ArrayList = New ArrayList
            Dim filePath As String = Common.Globals.ApplicationMapPath & _localesFile.Replace("/", "\")

            If File.Exists(filePath) Then
                Dim doc As New XPathDocument(filePath)
                For Each nav As XPathNavigator In doc.CreateNavigator.Select("root/language")
                    If nav.NodeType <> XmlNodeType.Comment Then
                        Dim objLocale As New Locale
                        objLocale.Text = nav.GetAttribute("name", "")
                        objLocale.Code = nav.GetAttribute("key", "")
                        objLocale.Fallback = nav.GetAttribute("fallback", "")

                        supportedLocales.Add(objLocale)
                    End If
                Next
            Else
                Dim objLocale As New Locale
                objLocale.Text = "English"
                objLocale.Code = "en-US"
                objLocale.Fallback = ""
                supportedLocales.Add(objLocale)
            End If

            Return supportedLocales
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetNextScriptVersion gets the next script to Install
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/15/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetNextScriptVersion(ByVal strProviderPath As String, ByVal currentVersion As Integer()) As String
            Dim arrScripts As ArrayList = Services.Upgrade.Upgrade.GetUpgradeScripts(strProviderPath, currentVersion)
            Dim strScriptVersion As String = Null.NullString
            Dim strNextVersion As String = "Done"

            If arrScripts.Count > 0 Then
                'First Script is next script
                strScriptVersion = Path.GetFileNameWithoutExtension(CType(arrScripts(0), String))
            End If

            If Not String.IsNullOrEmpty(strScriptVersion) Then
                Dim intVersion As Integer() = Services.Upgrade.Upgrade.GetVersion(strScriptVersion)
                strNextVersion = intVersion(0).ToString + "." + intVersion(1).ToString + "." + intVersion(2).ToString
            End If

            Return strNextVersion
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetWizardButton gets a wizard button from the template
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/28/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetWizardButton(ByVal containerID As String, ByVal buttonID As String) As LinkButton
            Dim navContainer As Control = wizInstall.FindControl(containerID)
            Dim button As LinkButton = Nothing
            If Not navContainer Is Nothing Then
                button = TryCast(navContainer.FindControl(buttonID), LinkButton)
            End If
            Return button
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Initialise configures the first Wizard page
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/15/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Initialise()

            If TestDataBaseInstalled() Then
                'Installed
                tblLanguage.Visible = False
                lblStep0Detail.Text = LocalizeString("Installed")
                EnableButton(GetWizardButton("StartNavigationTemplateContainerID", "StartNextLinkButton"), False)
            Else
                If DatabaseVersion(0) > 0 OrElse DatabaseVersion(1) > 0 OrElse DatabaseVersion(2) > 0 Then
                    'Upgrade
                    tblLanguage.Visible = False
                    lblStep0Title.Text = String.Format(LocalizeString("UpgradeTitle"), glbAppVersion)
                    lblStep0Detail.Text = String.Format(LocalizeString("Upgrade"), Services.Upgrade.Upgrade.GetStringVersion(DatabaseVersion))
                Else
                    'Install
                    UpdateMachineKey()
                End If
            End If

            'Parse the conneection String to the form
            BindConnectionString()

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' InstallDatabase intsalls the base Database scripts
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/14/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function InstallDatabase() As String
            Dim strErrorMessage As String = Null.NullString

            Dim strProviderPath As String = PortalSettings.GetProviderPath()
            If Not strProviderPath.StartsWith("ERROR:") Then
                'Install Base Version
                strErrorMessage = Services.Upgrade.Upgrade.InstallDatabase(BaseVersion, strProviderPath, InstallTemplate, False)
            Else
                'provider error
                strErrorMessage = strProviderPath
            End If

            If String.IsNullOrEmpty(strErrorMessage) Then
                'Get Next Version
                strErrorMessage = GetNextScriptVersion(strProviderPath, BaseVersion)
            ElseIf Not strErrorMessage.StartsWith("ERROR:") Then
                strErrorMessage = "ERROR: " + String.Format(LocalizeString("ScriptError"), Services.Upgrade.Upgrade.GetLogFile(strProviderPath, BaseVersion))
            End If

            Return strErrorMessage
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' InstallHostUser installs the Super User
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/16/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function InstallHostUser() As Boolean
            Dim success As Boolean = False
            Dim strErrorMessage As String = usrHost.Validate()

            If Not String.IsNullOrEmpty(strErrorMessage) Then
                Dim strError As String = LocalizeString(strErrorMessage)
                If strErrorMessage = "PasswordLength" Then
                    strError = String.Format(strError, MembershipProviderConfig.MinPasswordLength)
                End If
                lblHostUserError.Text = String.Format(LocalizeString("HostUserError"), strError)
            Else
                Try
                    'Initialise Host Settings
                    Services.Upgrade.Upgrade.InitialiseHostSettings(InstallTemplate, False)

                    'Create Host User
                    Dim objSuperUserInfo As UserInfo = Services.Upgrade.Upgrade.GetSuperUser(InstallTemplate, False)
                    objSuperUserInfo.FirstName = usrHost.FirstName
                    objSuperUserInfo.LastName = usrHost.LastName
                    objSuperUserInfo.Username = usrHost.UserName
                    objSuperUserInfo.DisplayName = usrHost.FirstName + " " + usrHost.LastName
                    objSuperUserInfo.Membership.Password = usrHost.Password
                    objSuperUserInfo.Email = usrHost.Email
                    UserController.CreateUser(objSuperUserInfo)

                    Services.Upgrade.Upgrade.InstallFiles(InstallTemplate, False)

                    success = True
                Catch ex As Exception
                    lblHostUserError.Text = String.Format(LocalizeString("HostUserError"), ex.Message)
                End Try
            End If

            Return success

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' InstallLanguages installs the Optional Languages
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/20/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function InstallLanguages() As Boolean
            Dim success As Boolean = False
            Dim strErrorMessage As String = Null.NullString

            Try
                Dim InstallPath As String = ApplicationMapPath & "\Install\Language"
                For Each language As ListItem In lstLanguages.Items
                    If language.Selected Then
                        ResourceInstaller.InstallLanguage(InstallPath + "\" + language.Value, True, False, 0)
                    End If
                Next

                success = True
            Catch ex As Exception
                strErrorMessage = ex.Message
            End Try

            If Not success Then
                lblLanguagesError.Text = String.Format(LocalizeString("InstallLanguagesError"), strErrorMessage)
            End If

            Return success

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' InstallModules installs the Optional Modules
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/19/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function InstallModules() As Boolean
            Dim success As Boolean = False
            Dim strErrorMessage As String = Null.NullString

            Try
                Dim InstallPath As String = ApplicationMapPath & "\Install\Module"
                For Each moduleItem As ListItem In lstModules.Items
                    If moduleItem.Selected Then
                        success = ResourceInstaller.InstallModule(InstallPath + "\" + moduleItem.Value, True, False, 0)
                        If Not success Then
                            strErrorMessage += String.Format(LocalizeString("InstallModuleError"), moduleItem.Text)
                        End If
                    End If
                Next

                success = String.IsNullOrEmpty(strErrorMessage)
            Catch ex As Exception
                strErrorMessage = ex.StackTrace
            End Try

            If Not success Then
                lblModulesError.Text = strErrorMessage
            End If

            Return success
        End Function

        Private Function InstallPackages() As Boolean
            Dim success As Boolean = False
            Dim strErrorMessage As String = Null.NullString

            Try
                Dim InstallPath As String = ApplicationMapPath & "\Install\Package"
                For Each packageItem As ListItem In lstPackages.Items
                    If packageItem.Selected Then
                        success = ResourceInstaller.InstallPackage(InstallPath + "\" + packageItem.Value, True, False, 0)
                        If Not success Then
                            strErrorMessage += String.Format(LocalizeString("InstallPackageError"), packageItem.Text)
                        End If
                    End If
                Next

                success = String.IsNullOrEmpty(strErrorMessage)
            Catch ex As Exception
                strErrorMessage = ex.StackTrace
            End Try

            If Not success Then
                lblPackagesError.Text = strErrorMessage
            End If

            Return success
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' InstallPortal installs the Host Portal
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/19/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function InstallPortal() As Boolean
            Dim success As Boolean = False
            Dim strErrorMessage As String = usrAdmin.Validate()

            If Not String.IsNullOrEmpty(strErrorMessage) Then
                Dim strError As String = LocalizeString(strErrorMessage)
                If strErrorMessage = "PasswordLength" Then
                    strError = String.Format(strError, MembershipProviderConfig.MinPasswordLength)
                End If
                lblPortalError.Text = String.Format(LocalizeString("AdminUserError"), strError)
            Else
                Try
                    Dim objPortalController As New PortalController
                    Dim strServerPath As String = ApplicationMapPath & "\"
                    Dim strPortalAlias As String = GetDomainName(HttpContext.Current.Request, True).Replace("/Install", "")
                    Dim strTemplate As String = cboPortalTemplate.SelectedValue + ".template"

                    'Create Portal
                    PortalId = objPortalController.CreatePortal(txtPortalTitle.Text, usrAdmin.FirstName, usrAdmin.LastName, _
                        usrAdmin.UserName, usrAdmin.Password, usrAdmin.Email, "", "", Common.Globals.HostMapPath, strTemplate, _
                        "", strPortalAlias, strServerPath, "", False)

                    success = (PortalId > Null.NullInteger)
                Catch ex As Exception
                    success = False
                    strErrorMessage = ex.Message
                End Try

                If Not success Then
                    lblPortalError.Text = String.Format(LocalizeString("InstallPortalError"), strErrorMessage)
                End If
            End If

            Return success
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' InstallSkins installs the Optional Skins
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/19/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function InstallSkins() As Boolean
            Dim success As Boolean = False
            Dim strErrorMessage As String = Null.NullString

            Try
                Dim InstallPath As String = ApplicationMapPath & "\Install\Skin"
                For Each skinItem As ListItem In lstSkins.Items
                    If skinItem.Selected Then
                        strErrorMessage += ResourceInstaller.InstallSkin(InstallPath + "\" + skinItem.Value, True, False, 0)
                    End If
                Next

                InstallPath = ApplicationMapPath & "\Install\Container"
                For Each skinItem As ListItem In lstContainers.Items
                    If skinItem.Selected Then
                        strErrorMessage += ResourceInstaller.InstallContainer(InstallPath + "\" + skinItem.Value, True, False, 0)
                    End If
                Next

                success = True
            Catch ex As Exception
                strErrorMessage = ex.Message
            End Try

            If Not success Then
                lblSkinsError.Text = String.Format(LocalizeString("InstallSkinsError"), strErrorMessage)
            End If

            Return success

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' InstallVersion intsalls the a single version script
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/16/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function InstallVersion(ByVal strVersion As String) As String
            Dim strErrorMessage As String = Null.NullString
            Dim version As Integer() = Services.Upgrade.Upgrade.GetVersion(strVersion)

            Dim strProviderPath As String = PortalSettings.GetProviderPath()
            If Not strProviderPath.StartsWith("ERROR:") Then
                'Install Version
                Dim strScriptFile As String = Services.Upgrade.Upgrade.GetScriptFile(strProviderPath, version)
                Services.Upgrade.Upgrade.UpgradeVersion(strScriptFile, False)
            Else
                'provider error
                strErrorMessage = strProviderPath
            End If

            If String.IsNullOrEmpty(strErrorMessage) Then
                'Get Next Version
                strErrorMessage = GetNextScriptVersion(strProviderPath, version)
            End If

            Return strErrorMessage
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LocalizePage sets up the Localized Text
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/09/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub LocalizePage()

            'Main Title
            Me.Title = LocalizeString("Title") + " - " + LocalizeString("Page" + wizInstall.ActiveStepIndex.ToString() + ".Title")

            'Page Titles
            For i As Integer = 0 To wizInstall.WizardSteps.Count - 1
                wizInstall.WizardSteps(i).Title = LocalizeString("Page" + i.ToString() + ".Title")
            Next

            'Wizard Buttons
            wizInstall.StartNextButtonText = "<img src=""" + ApplicationPath + "/images/rt.gif"" border=""0"" /> " + LocalizeString("Next")
            wizInstall.FinishPreviousButtonText = "<img src=""" + ApplicationPath + "/images/lt.gif"" border=""0"" /> " + LocalizeString("Previous")
            wizInstall.FinishCompleteButtonText = "<img src=""" + ApplicationPath + "/images/rt.gif"" border=""0"" /> " + LocalizeString("Finished")

            'Page 0 - Introduction
            lblStep0Title.Text = String.Format(LocalizeString("IntroTitle"), glbAppVersion)
            lblStep0Detail.Text = LocalizeString("IntroDetail")
            lblChooseInstall.Text = LocalizeString("ChooseInstall")
            lblChooseLanguage.Text = LocalizeString("ChooseLanguage")

            rblInstall.Items(0).Text = LocalizeString("Full")
            rblInstall.Items(1).Text = LocalizeString("Typical")
            rblInstall.Items(2).Text = LocalizeString("Auto")

            'Page 1 - File Permissions
            lblStep1Title.Text = LocalizeString("PermissionsTitle")
            lblStep1Detail.Text = LocalizeString("PermissionsDetail")
            lblPermissions.Text = LocalizeString("Permissions")
            'File Permissions
            BindPermissions(False)

            'Page 2 - Database Configuration
            lblStep2Title.Text = LocalizeString("DatabaseConfigTitle")
            lblStep2Detail.Text = LocalizeString("DatabaseConfigDetail")
            lblChooseDatabase.Text = LocalizeString("ChooseDatabase")
            lblServerHelp.Text = LocalizeString("ServerHelp")
            lblServer.Text = LocalizeString("Server")
            lblFile.Text = LocalizeString("DatabaseFile")
            lblDatabaseFileHelp.Text = LocalizeString("DatabaseFileHelp")
            lblDataBase.Text = LocalizeString("Database")
            lblDatabaseHelp.Text = LocalizeString("DatabaseHelp")
            lblIntegrated.Text = LocalizeString("Integrated")
            lblIntegratedHelp.Text = LocalizeString("IntegratedHelp")
            lblUserId.Text = LocalizeString("UserId")
            lblUserHelp.Text = LocalizeString("UserHelp")
            lblPassword.Text = LocalizeString("Password")
            lblPasswordHelp.Text = LocalizeString("PasswordHelp")
            lblOwner.Text = LocalizeString("Owner")
            lblOwnerHelp.Text = LocalizeString("OwnerHelp")
            lblQualifier.Text = LocalizeString("Qualifier")
            lblQualifierHelp.Text = LocalizeString("QualifierHelp")

            rblDatabases.Items(0).Text = LocalizeString("SQLServerXPress")
            rblDatabases.Items(1).Text = LocalizeString("SQLServer")

            'Page 3 - Database Installation
            lblStep3Title.Text = LocalizeString("DatabaseInstallTitle")
            lblStep3Detail.Text = LocalizeString("DatabaseInstallDetail")

            'Page 4 - Host User
            lblStep4Title.Text = LocalizeString("HostUserTitle")
            lblStep4Detail.Text = LocalizeString("HostUserDetail")
            usrHost.FirstNameLabel = LocalizeString("FirstName")
            usrHost.LastNameLabel = LocalizeString("LastName")
            usrHost.UserNameLabel = LocalizeString("UserName")
            usrHost.PasswordLabel = LocalizeString("Password")
            usrHost.ConfirmLabel = LocalizeString("Confirm")
            usrHost.EmailLabel = LocalizeString("Email")

            'Page 5 - Modules
            lblStep5Title.Text = LocalizeString("ModulesTitle")
            lblStep5Detail.Text = LocalizeString("ModulesDetail")
            lblModules.Text = LocalizeString("Modules")

            'Page 6 - Packages
            lblStep6Title.Text = LocalizeString("PackagesTitle")
            lblStep6Detail.Text = LocalizeString("PackagesDetail")
            lblPackages.Text = LocalizeString("Packages")

            'Page 7 - Skins/Containers
            lblStep7Title.Text = LocalizeString("SkinsTitle")
            lblStep7Detail.Text = LocalizeString("SkinsDetail")
            lblSkins.Text = LocalizeString("Skins")
            lblContainers.Text = LocalizeString("Containers")

            'Page 8 - Languages
            lblStep8Title.Text = LocalizeString("LanguagesTitle")
            lblStep8Detail.Text = LocalizeString("LanguagesDetail")
            lblLanguages.Text = LocalizeString("Languages")

            'Page 9 - Portal
            lblStep9Title.Text = LocalizeString("PortalTitle")
            lblStep9Detail.Text = LocalizeString("PortalDetail")
            usrAdmin.FirstNameLabel = LocalizeString("FirstName")
            usrAdmin.LastNameLabel = LocalizeString("LastName")
            usrAdmin.UserNameLabel = LocalizeString("UserName")
            usrAdmin.PasswordLabel = LocalizeString("Password")
            usrAdmin.ConfirmLabel = LocalizeString("Confirm")
            usrAdmin.EmailLabel = LocalizeString("Email")
            lblAdminUser.Text = LocalizeString("AdminUser")
            lblPortal.Text = LocalizeString("Portal")
            lblPortalTitle.Text = LocalizeString("PortalTitle")
            lblPortalTemplate.Text = LocalizeString("PortalTemplate")

            'Page 10 - Complete
            lblCompleteTitle.Text = LocalizeString("CompleteTitle")
            lblCompleteDetail.Text = LocalizeString("CompleteDetail")

        End Sub

        Private Sub SetupDatabasePage()
            Dim nextButton As LinkButton = GetWizardButton("StepNavigationTemplateContainerID", "StepNextButton")
            Dim customButton As LinkButton = GetWizardButton("StepNavigationTemplateContainerID", "CustomButton")
            SetupDatabasePage(customButton, nextButton)
        End Sub

        Private Sub SetupDatabasePage(ByVal customButton As LinkButton, ByVal nextButton As LinkButton)
            If rblDatabases.SelectedIndex > Null.NullInteger Then
                Dim isSQLFile As Boolean = (rblDatabases.SelectedValue = "SQLFile")
                Dim isSQLDb As Boolean = (rblDatabases.SelectedValue = "SQLDatabase")
                Dim isOracle As Boolean = (rblDatabases.SelectedValue = "Oracle")
                tblDatabase.Visible = True
                trFile.Visible = isSQLFile
                trDatabase.Visible = isSQLDb
                trIntegrated.Visible = Not isOracle
                trUser.Visible = Not chkIntegrated.Checked OrElse isOracle
                trPassword.Visible = Not chkIntegrated.Checked OrElse isOracle

                chkOwner.Checked = (Config.GetDataBaseOwner() = "dbo.")
                txtqualifier.Text = Config.GetObjectQualifer()

                customButton.Visible = True
                EnableButton(nextButton, True)
            Else
                tblDatabase.Visible = False
                customButton.Visible = False
                EnableButton(nextButton, False)
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' SetupPage updates the WizardPage
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/23/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub SetupPage()

            Dim nextButton As LinkButton = GetWizardButton("StepNavigationTemplateContainerID", "StepNextButton")
            Dim prevButton As LinkButton = GetWizardButton("StepNavigationTemplateContainerID", "StepPreviousButton")
            Dim customButton As LinkButton = GetWizardButton("StepNavigationTemplateContainerID", "CustomButton")
            EnableButton(nextButton, True)
            EnableButton(prevButton, True)
            ShowButton(customButton, False)

            Select Case wizInstall.ActiveStepIndex
                Case 0
                    'Page 0 - Welcome
                Case 1
                    'Page 1 Permissions
                    ShowButton(customButton, True)
                Case 2
                    '    'Page 2 - Database Configuration
                    SetupDatabasePage(customButton, nextButton)
                Case 3
                    lblInstallError.Text = ""
                    lblInstallError.Visible = False
                    EnableButton(nextButton, False)
                    ShowButton(prevButton, False)
                Case 4
                    'Page 4 - SuperUser Configuration
                    BindSuperUser()
                    ShowButton(prevButton, False)
                Case 5
                    'Page 5 - Modules
                    BindModules()
                    ShowButton(prevButton, False)
                Case 6
                    'Page 6 - Packages
                    BindPackages()
                    ShowButton(prevButton, True)
                Case 7
                    'Page 7 - Skins/Conatiners
                    BindSkins()
                    ShowButton(prevButton, False)
                Case 8
                    'Page 8 - Languages
                    BindLanguages()
                    ShowButton(prevButton, True)
                Case 9
                    'Page 9 - Portal
                    BindPortal()
                    ShowButton(prevButton, True)
            End Select

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' ShowButton shows/hides a Navigation Button
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/28/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub ShowButton(ByVal button As LinkButton, ByVal enabled As Boolean)
            If Not button Is Nothing Then
                button.Visible = enabled
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' TestDatabaseConnection checks the Database connection properties provided
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/23/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function TestDatabaseConnection() As Boolean
            Dim success As Boolean = False

            If String.IsNullOrEmpty(rblDatabases.SelectedValue) Then
                connectionString = "ERROR:" + LocalizeString("ChooseDbError")
            Else
                Dim isSQLFile As Boolean = (rblDatabases.SelectedValue = "SQLFile")
                Dim builder As DbConnectionStringBuilder = dataProvider.GetConnectionStringBuilder()
                If Not String.IsNullOrEmpty(txtServer.Text) Then
                    builder("Data Source") = txtServer.Text
                End If
                If Not String.IsNullOrEmpty(txtDatabase.Text) And Not isSQLFile Then
                    builder("Initial Catalog") = txtDatabase.Text
                End If
                If Not String.IsNullOrEmpty(txtFile.Text) And isSQLFile Then
                    builder("attachdbfilename") = "|DataDirectory|" + txtFile.Text
                End If
                If chkIntegrated.Checked Then
                    builder("integrated security") = "true"
                End If
                If Not String.IsNullOrEmpty(txtUserId.Text) Then
                    builder("uid") = txtUserId.Text
                End If
                If Not String.IsNullOrEmpty(txtPassword.Text) Then
                    builder("pwd") = txtPassword.Text
                End If
                If isSQLFile Then
                    builder("user instance") = "true"
                End If

                Dim owner As String = txtUserId.Text
                If chkOwner.Checked Then
                    owner = "dbo."
                End If

                connectionString = dataProvider.Instance.TestDatabaseConnection(builder, owner, txtqualifier.Text)
            End If

            If connectionString.StartsWith("ERROR:") Then
                lblDataBaseError.Text = String.Format(LocalizeString("ConnectError"), connectionString.Replace("ERROR:", ""))
            Else
                success = True
                lblDataBaseError.Text = LocalizeString("ConnectSuccess")
            End If
            Return success
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' TestDataBaseInstalled checks whether the Database is the current version
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/16/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function TestDataBaseInstalled() As Boolean
            Dim success As Boolean = True
            Dim strDatabaseVersion As String = GetDatabaseVersion()
            Dim strAssemblyVersion As String = glbAppVersion.Replace(".", "")
            If strDatabaseVersion <> strAssemblyVersion Then
                success = False
            End If
            If Not success Then
                lblInstallError.Text = LocalizeString("Install.Error")
            End If

            Return success
        End Function

        Private Sub UpdateMachineKey()
            Dim installationDate As String = Config.GetSetting("InstallationDate")

            If installationDate Is Nothing Or installationDate = "" Then
                Dim strError As String = Config.UpdateMachineKey()

                If strError = "" Then
                    ' send a new request to the application to initiate step 2
                    Response.Redirect(HttpContext.Current.Request.RawUrl, True)
                Else
                    '403-3 Error - Redirect to ErrorPage
                    Dim strURL As String = "~/ErrorPage.aspx?status=403_3&error=" & strError
                    HttpContext.Current.Response.Clear()
                    HttpContext.Current.Server.Transfer(strURL)
                End If
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' VerifyFileCreate checks whether a file can be created
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/09/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function VerifyFileCreate() As Boolean
            Dim verifyPath As String = Server.MapPath("~/Verify/Verify.txt")
            Dim verified As Boolean = VerifyFolderCreate()

            If verified Then
                'Attempt to create the File
                Try
                    If File.Exists(verifyPath) Then
                        File.Delete(verifyPath)
                    End If

                    Dim fileStream As Stream = File.Create(verifyPath)
                    fileStream.Close()

                Catch ex As Exception
                    verified = False
                End Try
            End If
            If Not verified Then PermissionsValid = False

            Return verified
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' VerifyFileDelete checks whether a file can be deleted
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/09/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function VerifyFileDelete() As Boolean
            Dim verifyPath As String = Server.MapPath("~/Verify/Verify.txt")
            Dim verified As Boolean = VerifyFileCreate()

            If verified Then
                'Attempt to delete the File
                Try
                    File.Delete(verifyPath)
                Catch ex As Exception
                    verified = False
                End Try
            End If
            If Not verified Then PermissionsValid = False

            Return verified
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' VerifyFolderCreate checks whether a folder can be created
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/23/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function VerifyFolderCreate() As Boolean
            Dim verifyPath As String = Server.MapPath("~/Verify")
            Dim verified As Boolean = True

            'Attempt to create the Directory
            Try
                If Directory.Exists(verifyPath) Then
                    Directory.Delete(verifyPath, True)
                End If

                Directory.CreateDirectory(verifyPath)
            Catch ex As Exception
                verified = False
            End Try
            If Not verified Then PermissionsValid = False

            Return verified
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' VerifyFolderDelete checks whether a folder can be deleted
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/23/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function VerifyFolderDelete() As Boolean
            Dim verifyPath As String = Server.MapPath("~/Verify")
            Dim verified As Boolean = VerifyFolderCreate()

            If verified Then
                'Attempt to delete the Directory
                Try
                    Directory.Delete(verifyPath)
                Catch ex As Exception
                    verified = False
                End Try
            End If
            If Not verified Then PermissionsValid = False

            Return verified
        End Function

#End Region

#Region "Protected Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetBaseDataBaseVersion gets the Base Database Version as a string
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/02/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Function GetBaseDataBaseVersion() As String
            Return BaseVersion(0).ToString + "." + BaseVersion(1).ToString + "." + BaseVersion(2).ToString
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LocalizeString is a helper function for localizing strings
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/23/2007 Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Function LocalizeString(ByVal key As String) As String
            Return Services.Localization.Localization.GetString(key, Me.LocalResourceFile, cboLanguages.SelectedValue.ToLower)
        End Function

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Init runs when the Page is initialised
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/14/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            DotNetNuke.UI.Utilities.ClientAPI.HandleClientAPICallbackEvent(Me)

            'Attach Event(s) to the Buttons
            Dim button As LinkButton = GetWizardButton("StepNavigationTemplateContainerID", "CustomButton")
            If Not button Is Nothing Then
                AddHandler button.Click, AddressOf wizInstall_CustomButtonClick
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the Page loads
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/09/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'register variable ActionCallback with script necessary to perform callback
            '[ACTIONTOKEN] will be replaced on the client side with real action
            DotNetNuke.UI.Utilities.ClientAPI.RegisterClientVariable(Me.Page, "ActionCallback", DotNetNuke.UI.Utilities.ClientAPI.GetCallbackEventReference(Me, "[ACTIONTOKEN]", "successFunc", "this", "errorFunc"), True)

            If Not Page.IsPostBack Then
                cboLanguages.DataSource = GetInstallerLocales()
                cboLanguages.DataBind()

                rblInstall.Items.Clear()
                rblInstall.Items.Add(New ListItem(LocalizeString("Full"), "Full"))
                rblInstall.Items.Add(New ListItem(LocalizeString("Typical"), "Typical"))
                rblInstall.Items.Add(New ListItem(LocalizeString("Auto"), "Auto"))
                rblInstall.SelectedIndex = 1

                rblDatabases.Items.Clear()
                rblDatabases.Items.Add(New ListItem(LocalizeString("SQLServerXPress"), "SQLFile"))
                rblDatabases.Items.Add(New ListItem(LocalizeString("SQLServer"), "SQLDatabase"))

                LocalizePage()

                Initialise()

                wizInstall.ActiveStepIndex = 0

                SetupPage()

                'Database Choices
                BindDatabases()
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_PreRender runs just before the page is rendered
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/15/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Make sure that the password is not cleared on pastback
            txtPassword.Attributes("value") = txtPassword.Text
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Runs when the Selected Language is changed
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/09/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub cboLanguages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLanguages.SelectedIndexChanged
            LocalizePage()
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Runs when the Integrated Security Checkbox value is changed
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/23/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub chkIntegrated_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIntegrated.CheckedChanged
            trUser.Visible = Not chkIntegrated.Checked
            trPassword.Visible = Not chkIntegrated.Checked
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Runs when there is a ClientCallback
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/09/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function ProcessAction(ByVal someAction As String) As String Implements DotNetNuke.UI.Utilities.IClientAPICallbackEventHandler.RaiseClientAPICallbackEvent
            If someAction = GetBaseDataBaseVersion() Then
                Dim result As String = InstallDatabase()
                If result = "Done" Then
                    'Complete Installation
                    Services.Upgrade.Upgrade.UpgradeApplication()
                End If
                Return result
            ElseIf someAction.Contains(".") Then
                'Upgrade Database
                Dim result As String = InstallVersion(someAction)
                If result = "Done" Then
                    'Complete Installation
                    Services.Upgrade.Upgrade.UpgradeApplication()
                End If
                Return result
            Else
                Return "Done"
            End If
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Runs when the Selected Database is changed
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/23/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub rblDatabases_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblDatabases.SelectedIndexChanged
            BindConnectionString()
            SetupDatabasePage()
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Runs when the Active Wizard Step has changed
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/09/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub wizInstall_ActiveStepChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wizInstall.ActiveStepChanged

            'Main Title
            Me.Title = LocalizeString("Title") + " - " + LocalizeString("Page" + wizInstall.ActiveStepIndex.ToString() + ".Title")
            If wizInstall.ActiveStepIndex > 0 Then
                Dim nextButton As LinkButton = GetWizardButton("StepNavigationTemplateContainerID", "StepNextButton")
                Dim prevButton As LinkButton = GetWizardButton("StepNavigationTemplateContainerID", "StepPreviousButton")
                nextButton.Text = "<img src=""" + ApplicationPath + "/images/rt.gif"" border=""0"" /> " + LocalizeString("Next")
                prevButton.Text = "<img src=""" + ApplicationPath + "/images/lt.gif"" border=""0"" /> " + LocalizeString("Previous")
            End If

            Select Case wizInstall.ActiveStepIndex
                Case 1
                    'Page 1 - Permissions
                    Dim customButton As LinkButton = GetWizardButton("StepNavigationTemplateContainerID", "CustomButton")
                    customButton.Text = "<img src=""" + ApplicationPath + "/images/icon_filemanager_16px.gif"" border=""0"" /> " + LocalizeString("TestPerm")
                Case 2
                    'Page 2 - Database
                    Dim customButton As LinkButton = GetWizardButton("StepNavigationTemplateContainerID", "CustomButton")
                    customButton.Text = "<img src=""" + ApplicationPath + "/images/icon_sql_16px.gif"" border=""0"" /> " + LocalizeString("TestDB")
                Case 5
                    'Page 5 - Modules
                    If rblInstall.SelectedValue = "Typical" Then
                        BindModules()

                        If InstallModules() Then
                            'Skip Modules Page
                            wizInstall.ActiveStepIndex = 6
                        End If
                    End If
                Case 6
                    'Page 6 - Packages (Auth Systems)
                    If rblInstall.SelectedValue = "Typical" Then
                        BindPackages()

                        If InstallPackages() Then
                            'Skip Packages Page
                            wizInstall.ActiveStepIndex = 7
                        End If
                    End If
                Case 7
                    'Page 7 - Skins/Conatiners
                    If rblInstall.SelectedValue = "Typical" Then
                        BindSkins()

                        If InstallSkins() Then
                            'Skip Skins Page
                            wizInstall.ActiveStepIndex = 8
                        End If
                    End If
                Case 8
                    'Page 8 - Languages
                    If rblInstall.SelectedValue = "Typical" Then
                        BindLanguages()

                        If InstallLanguages() Then
                            'Skip Languages Page
                            wizInstall.ActiveStepIndex = 9
                        End If
                    End If
            End Select

            SetupPage()
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Runs when the Wizard's Custom button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/28/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub wizInstall_CustomButtonClick(ByVal sender As Object, ByVal e As System.EventArgs)

            Select Case wizInstall.ActiveStepIndex
                Case 1
                    'Page 1 - File Permissions
                    BindPermissions(True)
                Case 2
                    'Page 2 - Database Connection String
                    TestDatabaseConnection()
            End Select
        End Sub

        Protected Sub wizInstall_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles wizInstall.FinishButtonClick
            'Force an App Restart
            Config.Touch()

            'Redirect to site home page
            Response.Redirect("~/Default.aspx", True)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Runs when the Wizard's Next button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	02/20/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub wizInstall_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles wizInstall.NextButtonClick

            Dim nextStep As WizardStep = wizInstall.WizardSteps(e.NextStepIndex)

            Select Case e.CurrentStepIndex
                Case 0
                    'Page 0 - Database Installed?
                    e.Cancel = TestDataBaseInstalled()

                    If Not e.Cancel Then
                        'Upgrade ?
                        If DatabaseVersion(0) > 0 OrElse DatabaseVersion(1) > 0 OrElse DatabaseVersion(2) > 0 Then
                            Response.Redirect("~/Install/Install.aspx?mode=upgrade")
                        End If
                        If rblInstall.SelectedValue = "Auto" Then
                            Response.Redirect("~/Install/Install.aspx?mode=install")
                        End If
                    End If
                Case 1
                    'Page 1 - File Permissions
                    BindPermissions(True)
                    e.Cancel = Not PermissionsValid
                Case 2
                    'Page 2 - Database Configuration
                    Dim canConnect As Boolean = TestDatabaseConnection()
                    If canConnect Then
                        'Update Connection String
                        Config.UpdateConnectionString(connectionString)

                        'TODO:  Update Provider Settings (dbo/oq)
                        Dim dbOwner As String = txtUserId.Text
                        If chkOwner.Checked Then
                            dbOwner = "dbo"
                        End If

                        If rblDatabases.SelectedValue = "Oracle" Then
                            Config.UpdateDataProvider("OracleDataProvider", "", txtqualifier.Text)
                        Else
                            Config.UpdateDataProvider("SqlDataProvider", dbOwner, txtqualifier.Text)
                        End If

                        'Get Base DatabaseVersion
                        GetBaseDataBaseVersion()
                    Else
                        e.Cancel = True
                    End If
                Case 3
                    'Page 3 - Database Installation
                    e.Cancel = Not TestDataBaseInstalled()
                Case 4
                    'Page 4 - Host User
                    e.Cancel = Not InstallHostUser()
                Case 5
                    'Page 5 - Modules
                    e.Cancel = Not InstallModules()
                Case 6
                    'Page 6 - Packages
                    e.Cancel = Not InstallPackages()
                Case 7
                    'Page 7 - Skins/Containers
                    e.Cancel = Not InstallSkins()
                Case 8
                    'Page 8 - Languages
                    e.Cancel = Not InstallLanguages()
                Case 9
                    'Page 9 - Portal
                    e.Cancel = Not InstallPortal()
            End Select
        End Sub

#End Region

    End Class

End Namespace

