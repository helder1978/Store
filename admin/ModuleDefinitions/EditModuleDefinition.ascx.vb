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

Imports System.Xml
Imports System.IO
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Definitions
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.Services.EventQueue
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Modules.Admin.ModuleDefinitions

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The EditModuleDefinition PortalModuleBase is used to edit a Module
    ''' Definition
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
    '''                       and localisation
    '''     [cnurse]    01/13/2005  Added IActionable Implementation for the Private Assembly Package creator
    '''     [cnurse]    04/18/2005  Added support for FolderName, ModuleName and BusinessControllerClass
    '''     [cnurse]    04/21/2005  Added DefaultCacheTime properties for Module Definition
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class EditModuleDefinition

        Inherits DotNetNuke.Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable

#Region "Private Members"

        Private DesktopModuleId As Integer

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' DeleteParentFolders deletes parent folders that are empty
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	11/17/2005	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub DeleteParentFolders(ByVal folder As String, ByVal isRecursive As Boolean)

            Try
                If folder.ToLower <> Request.MapPath("~/DesktopModules").ToLower AndAlso folder.ToLower <> Request.MapPath("~/").ToLower Then
                    Dim folderInfo As DirectoryInfo = New DirectoryInfo(folder)
                    Dim parentInfo As DirectoryInfo = folderInfo.Parent

                    'Check if Folder is empty
                    If Directory.GetFileSystemEntries(parentInfo.FullName).Length = 0 Then
                        Directory.Delete(parentInfo.FullName)
                    End If

                    'recursively check parent Folders
                    If isRecursive Then
                        DeleteParentFolders(parentInfo.FullName, isRecursive)
                    End If
                End If

            Catch ex As Exception

            End Try

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' DeleteSubFolders deletes sub-folders
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	11/17/2005	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub DeleteSubFolders(ByVal folder As String, ByVal isRecursive As Boolean)

            Try
                Dim strDesktopModules As String = Request.MapPath("~/DesktopModules")
                If folder.Contains("DesktopModules") And (folder = strDesktopModules Or folder = strDesktopModules & "\" Or folder.Replace(strDesktopModules, "") = folder) Then
                    Exit Sub
                End If

                Dim strAppCode As String = Request.MapPath("~/App_Code")
                If folder.Contains("App_Code") And (folder = strAppCode Or folder = strAppCode & "\" Or folder.Replace(strAppCode, "") = folder) Then
                    Exit Sub
                End If

                Directory.Delete(folder, isRecursive)

            Catch ex As Exception

            End Try

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadDefinitions fetches the control data from the database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModuleDefId">The Module definition Id</param>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub LoadControls(ByVal ModuleDefId As Integer)

            Dim objModuleControls As New ModuleControlController
            Dim arrModuleControls As ArrayList = objModuleControls.GetModuleControls(ModuleDefId)

            If DesktopModuleId = -2 Then
                Dim objModuleControl As ModuleControlInfo
                Dim intIndex As Integer
                For intIndex = arrModuleControls.Count - 1 To 0 Step -1
                    objModuleControl = CType(arrModuleControls(intIndex), ModuleControlInfo)
                    If objModuleControl.ControlType <> SecurityAccessLevel.SkinObject Then
                        arrModuleControls.RemoveAt(intIndex)
                    End If
                Next
            End If

            grdControls.DataSource = arrModuleControls
            grdControls.DataBind()

            cmdAddControl.Visible = True
            grdControls.Visible = True

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadDefinitions fetches the definitions from the database and updates the controls
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub LoadDefinitions()

            Dim objModuleDefinitions As New ModuleDefinitionController
            cboDefinitions.DataSource = objModuleDefinitions.GetModuleDefinitions(DesktopModuleId)
            cboDefinitions.DataBind()

            If cboDefinitions.Items.Count <> 0 Then
                rowDefinitions.Visible = True
                tabControls.Visible = True
                cboDefinitions.SelectedIndex = 0
                Dim ModuleDefId As Integer = Integer.Parse(cboDefinitions.SelectedItem.Value)
                LoadCacheProperties(ModuleDefId)
                LoadControls(ModuleDefId)
                tabCache.Visible = True
            Else
                rowDefinitions.Visible = False
                cmdAddControl.Visible = False
                tabControls.Visible = False
                txtCacheTime.Text = "0"
                tabCache.Visible = False
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadCacheProperties loads the Module Definitions Default Cache Time properties
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	4/21/2005   created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub LoadCacheProperties(ByVal ModuleDefId As Integer)

            Dim objModuleDefinitionController As New ModuleDefinitionController
            Dim objModuleDefinition As ModuleDefinitionInfo = objModuleDefinitionController.GetModuleDefinition(ModuleDefId)

            txtCacheTime.Text = objModuleDefinition.DefaultCacheTime.ToString

        End Sub

        Private Sub UpdateModuleInterfaces(ByVal BusinessControllerClass As String)
            'Check to see if Interfaces (SupportedFeatures) Need to be Updated
            If BusinessControllerClass <> "" Then
                'this cannot be done directly at this time because 
                'the module may not be loaded into the app domain yet
                'So send an EventMessage that will process the update 
                'after the App recycles
                Dim oAppStartMessage As New EventQueue.EventMessage
                oAppStartMessage.Sender = UserInfo.Username
                oAppStartMessage.Priority = MessagePriority.High
                oAppStartMessage.ExpirationDate = Now.AddYears(-1)
                oAppStartMessage.SentDate = System.DateTime.Now
                oAppStartMessage.Body = ""
                oAppStartMessage.ProcessorType = "DotNetNuke.Entities.Modules.EventMessageProcessor, DotNetNuke"
                oAppStartMessage.ProcessorCommand = "UpdateSupportedFeatures"

                'Add custom Attributes for this message
                oAppStartMessage.Attributes.Add("BusinessControllerClass", BusinessControllerClass)
                oAppStartMessage.Attributes.Add("DesktopModuleId", DesktopModuleId.ToString())

                'send it to occur on next App_Start Event
                EventQueueController.SendMessage(oAppStartMessage, "Application_Start")

                'force an app restart
                DotNetNuke.Common.Utilities.Config.Touch()
            End If
        End Sub

#End Region

#Region "Public Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' FormatURL formats the url correctly (added a key=value onto the Querystring
        ''' </summary>
        ''' <param name="strKeyName">A Key</param>
        ''' <param name="strKeyValue">The Module definition Id</param>
        ''' <returns>A correctly formatted url</returns>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation        
        ''' 	[smcculloch]10/11/2004	Updated to use EditUrl overload
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function FormatURL(ByVal strKeyName As String, ByVal strKeyValue As String) As String
            Dim _FormatUrl As String = Null.NullString
            Try
                If DesktopModuleId <> -2 Then
                    _FormatUrl = EditUrl(strKeyName, strKeyValue, "Control", "desktopmoduleid=" & DesktopModuleId.ToString(), "moduledefid=" & cboDefinitions.SelectedItem.Value)
                Else
                    _FormatUrl = EditUrl(strKeyName, strKeyValue, "Control", "desktopmoduleid=" & DesktopModuleId.ToString(), "moduledefid=-1")
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

            Return _FormatUrl
        End Function

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Init runs when the control is initialised.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	01/26/2007	
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            If Not (Request.QueryString("desktopmoduleid") Is Nothing) Then
                DesktopModuleId = Int32.Parse(Request.QueryString("desktopmoduleid"))
            Else
                DesktopModuleId = Null.NullInteger
            End If

            If Null.IsNull(DesktopModuleId) Then
                Me.ModuleConfiguration.ModuleTitle = Localization.GetString("Create", Me.LocalResourceFile)
                cmdUpdate.Text = Localization.GetString("cmdAdd", Me.LocalResourceFile)
            Else
                Me.ModuleConfiguration.ModuleTitle = Localization.GetString("Edit", Me.LocalResourceFile)
                cmdUpdate.Text = Localization.GetString("cmdUpdate", Me.LocalResourceFile)
            End If


        End Sub


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        '''     [vmasanas]  31/10/2004  Populate Premium list when we are adding
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                Dim objDesktopModules As New DesktopModuleController

                If Page.IsPostBack = False Then
                    'Localize Grid
                    Services.Localization.Localization.LocalizeDataGrid(grdControls, Me.LocalResourceFile)

                    ClientAPI.AddButtonConfirm(cmdDelete, Services.Localization.Localization.GetString("DeleteItem"))
                    ClientAPI.AddButtonConfirm(cmdDeleteDefinition, Services.Localization.Localization.GetString("DeleteItem"))

                    If Null.IsNull(DesktopModuleId) Then

                        'Enable ReadOnly Controls for Add Mode only
                        txtModuleName.Enabled = True
                        txtFolderName.Enabled = True
                        txtVersion.Enabled = True
                        txtVersion.Text = "01.00.00"
                        txtBusinessClass.Enabled = True
                        txtCompatibleVersions.Enabled = True
                        txtDependencies.Enabled = True
                        txtPermissions.Enabled = True

                        cmdDelete.Visible = False
                        chkDelete.Visible = False
                        tabDefinitions.Visible = False
                        tabCache.Visible = False
                        tabControls.Visible = False

                    Else
                        Dim objDesktopModule As DesktopModuleInfo

                        If DesktopModuleId = -2 Then
                            objDesktopModule = New DesktopModuleInfo
                            objDesktopModule.ModuleName = Services.Localization.Localization.GetString("SkinObjects")
                            objDesktopModule.FolderName = Services.Localization.Localization.GetString("SkinObjects")
                            objDesktopModule.FriendlyName = Services.Localization.Localization.GetString("SkinObjects")
                            objDesktopModule.Description = Services.Localization.Localization.GetString("SkinObjectsDescription")
                            objDesktopModule.IsPremium = False
                            objDesktopModule.Version = ""

                            cmdUpdate.Visible = False
                            cmdDelete.Visible = False
                            chkDelete.Visible = False
                            tabDefinitions.Visible = False
                            tabCache.Visible = False
                            txtDescription.Enabled = False
                            chkPremium.Enabled = False

                            LoadControls(Null.NullInteger)
                        Else
                            If Request.IsLocal Then
                                txtModuleName.Enabled = True
                                txtFolderName.Enabled = True
                                txtVersion.Enabled = True
                                txtBusinessClass.Enabled = True
                                txtCompatibleVersions.Enabled = True
                                txtDependencies.Enabled = True
                                txtPermissions.Enabled = True
                                chkDelete.Checked = False
                            Else
                                chkDelete.Checked = True
                            End If

                            objDesktopModule = objDesktopModules.GetDesktopModule(DesktopModuleId)

                            LoadDefinitions()
                        End If

                        If Not objDesktopModule Is Nothing Then
                            txtModuleName.Text = objDesktopModule.ModuleName
                            txtFolderName.Text = objDesktopModule.FolderName
                            txtFriendlyName.Text = objDesktopModule.FriendlyName
                            txtDescription.Text = objDesktopModule.Description
                            txtVersion.Text = objDesktopModule.Version
                            If txtVersion.Text = "" Then
                                txtVersion.Text = "01.00.00"
                            End If
                            txtCompatibleVersions.Text = objDesktopModule.CompatibleVersions
                            txtDependencies.Text = objDesktopModule.Dependencies
                            txtPermissions.Text = objDesktopModule.Permissions
                            txtBusinessClass.Text = objDesktopModule.BusinessControllerClass
                            chkUpgradeable.Checked = objDesktopModule.IsUpgradeable
                            chkPortable.Checked = objDesktopModule.IsPortable
                            chkSearchable.Checked = objDesktopModule.IsSearchable
                            chkPremium.Checked = objDesktopModule.IsPremium
                        End If
                    End If

                    Dim objPortals As New PortalController
                    Dim arrPortals As ArrayList = objPortals.GetPortals
                    Dim arrPortalDesktopModules As ArrayList = objDesktopModules.GetPortalDesktopModules(Null.NullInteger, DesktopModuleId)

                    Dim objPortal As PortalInfo
                    Dim objPortalDesktopModule As PortalDesktopModuleInfo
                    For Each objPortalDesktopModule In arrPortalDesktopModules
                        For Each objPortal In arrPortals
                            If objPortal.PortalID = objPortalDesktopModule.PortalID Then
                                arrPortals.Remove(objPortal)
                                Exit For
                            End If
                        Next
                    Next

                    ctlPortals.Available = arrPortals
                    ctlPortals.Assigned = arrPortalDesktopModules
                    ctlPortals.Visible = chkPremium.Checked

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdAddControl_Click runs when the Add Control Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdAddControl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddControl.Click

            Response.Redirect(FormatURL("modulecontrolid", "-1"))

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdAddDefinition_Click runs when the Add Definition Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdAddDefinition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddDefinition.Click

            Dim ModuleDefId As Integer = -1

            If txtDefinition.Text <> "" Then
                Dim objModuleDefinition As New ModuleDefinitionInfo

                objModuleDefinition.DesktopModuleID = DesktopModuleId
                objModuleDefinition.FriendlyName = txtDefinition.Text
                Try
                    objModuleDefinition.DefaultCacheTime = Integer.Parse(txtCacheTime.Text)
                    If Not objModuleDefinition.DefaultCacheTime >= 0 Then
                        DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("UpdateCache.ErrorMessage", Me.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                        Exit Sub
                    End If
                Catch
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("UpdateCache.ErrorMessage", Me.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                    Exit Sub
                End Try

                Dim objModuleDefinitions As New ModuleDefinitionController

                Try
                    ModuleDefId = objModuleDefinitions.AddModuleDefinition(objModuleDefinition)
                Catch
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("AddDefinition.ErrorMessage", Me.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                    Exit Sub
                End Try

                LoadDefinitions()

                If ModuleDefId > -1 Then
                    'Set the Combo
                    cboDefinitions.SelectedIndex = -1
                    cboDefinitions.Items.FindByValue(ModuleDefId.ToString).Selected = True
                    LoadCacheProperties(ModuleDefId)
                    LoadControls(ModuleDefId)
                    'Clear the Text Box
                    txtDefinition.Text = ""
                End If
            Else
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("MissingDefinition.ErrorMessage", Me.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdCancel_Click runs when the Cancel Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cboDefinitions_SelectedIndexChanged runs when item in the Definitions combo is changed
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cboDefinitions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDefinitions.SelectedIndexChanged

            Dim ModuleDefId As Integer = Integer.Parse(cboDefinitions.SelectedItem.Value)
            LoadCacheProperties(ModuleDefId)
            LoadControls(ModuleDefId)

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdDelete_Click runs when the Delete Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
            Try
                Dim strFileName As String
                Dim strFileExtension As String
                Dim arrFiles As String()

                If Not Null.IsNull(DesktopModuleId) Then
                    Dim strRoot As String = Request.MapPath("~/DesktopModules/" & txtFolderName.Text) & "\"

                    ' process uninstall script
                    Dim strProviderType As String = "data"
                    Dim objProviderConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(strProviderType)
                    Dim strUninstallScript As String = "Providers\DataProviders\" & objProviderConfiguration.DefaultProvider & "\Uninstall." & objProviderConfiguration.DefaultProvider

                    ' if file does not exist, try the legacy location in the root folder for the module
                    If Not File.Exists(strRoot & strUninstallScript) Then
                        strUninstallScript = "Uninstall." & objProviderConfiguration.DefaultProvider
                    End If

                    If File.Exists(strRoot & strUninstallScript) Then
                        ' read uninstall script
                        Dim objStreamReader As StreamReader
                        objStreamReader = File.OpenText(strRoot & strUninstallScript)
                        Dim strScript As String = objStreamReader.ReadToEnd
                        objStreamReader.Close()

                        ' execute uninstall script
                        Entities.Portals.PortalSettings.ExecuteScript(strScript)
                    End If

                    If Directory.Exists(strRoot) Then
                        If chkDelete.Checked Then
                            'runtime so remove files/folders
                            ' find dnn manifest file
                            arrFiles = Directory.GetFiles(strRoot, "*.dnn.config")
                            If arrFiles.Length = 0 Then
                                arrFiles = Directory.GetFiles(strRoot, "*.dnn") ' legacy versions stored the *.dnn files unprotected
                            End If
                            If arrFiles.Length <> 0 Then
                                If File.Exists(strRoot & Path.GetFileName(arrFiles(0))) Then
                                    Dim xmlDoc As New XmlDocument
                                    Dim nodeFile As XmlNode

                                    ' load the manifest file
                                    xmlDoc.Load(strRoot & Path.GetFileName(arrFiles(0)))

                                    ' check version
                                    Dim nodeModule As XmlNode = Nothing
                                    Select Case xmlDoc.DocumentElement.LocalName.ToLower()
                                        Case "module"
                                            nodeModule = xmlDoc.SelectSingleNode("//module")
                                        Case "dotnetnuke"
                                            Dim version As String = xmlDoc.DocumentElement.Attributes("version").InnerText
                                            Select Case version
                                                Case "2.0"
                                                    ' V2 allows for multiple folders in a single DNN definition - we need to identify the correct node
                                                    For Each nodeModule In xmlDoc.SelectNodes("//dotnetnuke/folders/folder")
                                                        If nodeModule.SelectSingleNode("name").InnerText.Trim = txtFriendlyName.Text Then
                                                            Exit For
                                                        End If
                                                    Next
                                                Case "3.0"
                                                    ' V3 also allows for multiple folders in a single DNN definition - but uses module name
                                                    For Each nodeModule In xmlDoc.SelectNodes("//dotnetnuke/folders/folder")
                                                        If nodeModule.SelectSingleNode("name").InnerText.Trim = txtModuleName.Text Then
                                                            Exit For
                                                        End If
                                                    Next
                                            End Select
                                    End Select

                                    ' loop through file nodes
                                    For Each nodeFile In nodeModule.SelectNodes("files/file")
                                        strFileName = nodeFile.SelectSingleNode("name").InnerText.Trim
                                        strFileExtension = Path.GetExtension(strFileName).Replace(".", "")
                                        If strFileExtension = "dll" Then
                                            ' remove DLL from /bin
                                            strFileName = Request.MapPath("~/bin/") & strFileName
                                        End If
                                        If File.Exists(strFileName) Then
                                            File.SetAttributes(strFileName, FileAttributes.Normal)
                                            File.Delete(strFileName)
                                        End If
                                    Next

                                    'Recursively Delete any sub Folders
                                    DeleteSubFolders(strRoot, True)

                                    'Recursively delete AppCode folders
                                    Dim appCode As String = strRoot.Replace("DesktopModules", "App_Code")
                                    DeleteSubFolders(appCode, True)

                                    'Delete the <codeSubDirectory> node in web.config
                                    DotNetNuke.Common.Utilities.Config.RemoveCodeSubDirectory(txtFolderName.Text)
                                End If
                            End If
                        End If
                    End If

                    'Delete Custom Module Permissions
                    Dim objModuleDefinitions As New ModuleDefinitionController
                    For Each objModuleDefinition As ModuleDefinitionInfo In objModuleDefinitions.GetModuleDefinitions(DesktopModuleId)
                        Dim objPermissions As New PermissionController
                        For Each objPermission As PermissionInfo In objPermissions.GetPermissionsByModuleDefID(objModuleDefinition.ModuleDefID)
                            objPermissions.DeletePermission(objPermission.PermissionID)
                        Next
                    Next

                    ' delete the desktopmodule database record
                    Dim objDesktopModules As New DesktopModuleController
                    objDesktopModules.DeleteDesktopModule(DesktopModuleId)

                End If

                Response.Redirect(NavigateURL(), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdDeleteDefinition_Click runs when the Delete Definition Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdDeleteDefinition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDeleteDefinition.Click

            Dim objModuleDefinitions As New ModuleDefinitionController

            objModuleDefinitions.DeleteModuleDefinition(Integer.Parse(cboDefinitions.SelectedItem.Value))

            LoadDefinitions()

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdUpdate_Click runs when the Update Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            Try

                If Page.IsValid = True Then
                    Dim objDesktopModule As New DesktopModuleInfo

                    objDesktopModule.DesktopModuleID = DesktopModuleId
                    objDesktopModule.ModuleName = txtModuleName.Text
                    objDesktopModule.FolderName = txtFolderName.Text
                    objDesktopModule.FriendlyName = txtFriendlyName.Text
                    If objDesktopModule.FolderName = "" Then
                        objDesktopModule.FolderName = objDesktopModule.ModuleName
                    End If
                    objDesktopModule.Description = txtDescription.Text
                    objDesktopModule.IsPremium = chkPremium.Checked
                    objDesktopModule.IsAdmin = False

                    If txtVersion.Text <> "" Then
                        objDesktopModule.Version = txtVersion.Text
                    Else
                        objDesktopModule.Version = Null.NullString
                    End If

                    If txtBusinessClass.Text <> "" Then
                        objDesktopModule.BusinessControllerClass = txtBusinessClass.Text
                    Else
                        objDesktopModule.BusinessControllerClass = Null.NullString
                    End If

                    If txtCompatibleVersions.Text <> "" Then
                        objDesktopModule.CompatibleVersions = txtCompatibleVersions.Text
                    Else
                        objDesktopModule.CompatibleVersions = Null.NullString
                    End If

                    If txtDependencies.Text <> "" Then
                        objDesktopModule.Dependencies = txtDependencies.Text
                    Else
                        objDesktopModule.Dependencies = Null.NullString
                    End If

                    If txtPermissions.Text <> "" Then
                        objDesktopModule.Permissions = txtPermissions.Text
                    Else
                        objDesktopModule.Permissions = Null.NullString
                    End If

                    Dim objDesktopModules As New DesktopModuleController

                    If Null.IsNull(DesktopModuleId) Then
                        Try
                            objDesktopModule.DesktopModuleID = objDesktopModules.AddDesktopModule(objDesktopModule)
                        Catch
                            DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("AddModule.ErrorMessage", Me.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                            Exit Sub
                        End Try
                    Else
                        objDesktopModules.UpdateDesktopModule(objDesktopModule)
                    End If

                    ' delete old portal module assignments
                    objDesktopModules.DeletePortalDesktopModules(Null.NullInteger, objDesktopModule.DesktopModuleID)
                    ' add new portal module assignments
                    If objDesktopModule.IsPremium Then
                        Dim objListItem As ListItem
                        For Each objListItem In ctlPortals.Assigned
                            objDesktopModules.AddPortalDesktopModule(Integer.Parse(objListItem.Value), objDesktopModule.DesktopModuleID)
                        Next
                    End If

                    ' update interfaces
                    UpdateModuleInterfaces(objDesktopModule.BusinessControllerClass)

                    Response.Redirect(EditUrl("desktopmoduleid", objDesktopModule.DesktopModuleID.ToString), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdUpdateCacheTime_Click runs when the Update Cache Time Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	4/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdUpdateCacheTime_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdateCacheTime.Click

            If Not cboDefinitions.SelectedItem Is Nothing Then
                Dim ModuleDefId As Integer = Integer.Parse(cboDefinitions.SelectedItem.Value)
                Dim objModuleDefinitions As New ModuleDefinitionController
                Dim objModuleDefinition As ModuleDefinitionInfo = objModuleDefinitions.GetModuleDefinition(ModuleDefId)

                Try
                    objModuleDefinition.DefaultCacheTime = Integer.Parse(txtCacheTime.Text)
                    objModuleDefinitions.UpdateModuleDefinition(objModuleDefinition)
                Catch ex As Exception
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("UpdateCache.ErrorMessage", Me.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                End Try
            Else
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("MissingDefinition.ErrorMessage", Me.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
            End If

        End Sub

        Private Sub chkPremium_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPremium.CheckedChanged
            ctlPortals.Visible = chkPremium.Checked
        End Sub

#End Region

#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim objDesktopModules As New DesktopModuleController
                Dim mid As Integer
                If Not (Request.QueryString("desktopmoduleid") Is Nothing) Then
                    mid = Int32.Parse(Request.QueryString("desktopmoduleid"))
                Else
                    mid = Null.NullInteger
                End If
                Dim objDesktopModule As DesktopModuleInfo = objDesktopModules.GetDesktopModule(mid)
                Dim Actions As New ModuleActionCollection
                If Not objDesktopModule Is Nothing Then
                    If Not objDesktopModule.IsAdmin Then
                        'Create the DirectoryInfo object for the folder
                        Dim folder As New DirectoryInfo(Common.Globals.ApplicationMapPath & "\DesktopModules\" & objDesktopModule.FolderName)
                        If folder.Exists Then
                            Actions.Add(GetNextActionID, Services.Localization.Localization.GetString("PrivateAssemblyCreate.Action", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("desktopmoduleid", mid.ToString, "package"), False, SecurityAccessLevel.Host, True, False)
                        End If
                    End If
                End If
                Return Actions
            End Get
        End Property
#End Region

    End Class

End Namespace
