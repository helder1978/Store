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
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Modules.Admin.ModuleDefinitions

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' </summary>
	''' <returns></returns>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' </history>
	''' -----------------------------------------------------------------------------
    Partial Class ImportModuleDefinition
        Inherits Entities.Modules.PortalModuleBase

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' </summary>
        ''' <param name="strRoot">The Root folder to parse from</param>
        ''' <param name="blnRecurse">True to iterate sub-folders</param>
        ''' <remarks>
        ''' Loads the cboSource control list with locations of controls.
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindManifestList(ByVal strRoot As String, Optional ByVal blnRecurse As Boolean = True)
            Dim strFolder As String
            Dim arrFolders As String()
            Dim strFile As String
            Dim arrFiles As String()

            If Directory.Exists(Request.MapPath(Common.Globals.ApplicationPath & "/" & strRoot)) Then
                arrFolders = Directory.GetDirectories(Request.MapPath(Common.Globals.ApplicationPath & "/" & strRoot))
                If blnRecurse Then
                    For Each strFolder In arrFolders
                        BindManifestList(strFolder.Substring(Request.MapPath(Common.Globals.ApplicationPath).Length + 1).Replace("\"c, "/"c), blnRecurse)
                    Next
                End If
                arrFiles = Directory.GetFiles(Request.MapPath(Common.Globals.ApplicationPath & "/" & strRoot), "*.dnn")
                For Each strFile In arrFiles
                    cboManifest.Items.Add(New ListItem(Path.GetFileName(strFile), strFile))
                Next
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' </summary>
        ''' <param name="strRoot">The Root folder to parse from</param>
        ''' <param name="blnRecurse">True to iterate sub-folders</param>
        ''' <remarks>
        ''' Loads the cboSource control list with locations of controls.
        ''' </remarks>
        ''' <history>
        ''' 	[pgaryga]	18/08/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindControlList(ByVal strRoot As String, Optional ByVal blnRecurse As Boolean = True)
            Dim strFolder As String
            Dim arrFolders As String()
            Dim strFile As String
            Dim arrFiles As String()

            If Directory.Exists(Request.MapPath(Common.Globals.ApplicationPath & "/" & strRoot)) Then
                arrFolders = Directory.GetDirectories(Request.MapPath(Common.Globals.ApplicationPath & "/" & strRoot))
                If blnRecurse Then
                    For Each strFolder In arrFolders
                        BindControlList(strFolder.Substring(Request.MapPath(Common.Globals.ApplicationPath).Length + 1).Replace("\"c, "/"c), blnRecurse)
                    Next
                End If
                arrFiles = Directory.GetFiles(Request.MapPath(Common.Globals.ApplicationPath & "/" & strRoot), "*.ascx")
                For Each strFile In arrFiles
                    strFile = strRoot.Replace("\"c, "/"c) & "/" & Path.GetFileName(strFile)
                    cboControl.Items.Add(New ListItem(strFile, strFile))
                Next
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' </summary>
        ''' <remarks>
        ''' Loads the cboSource control list with locations of controls.
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub ImportManifest(ByVal strManifest As String)
            Try
                Dim doc As New XmlDocument
                doc.Load(strManifest)

                Dim dnnRoot As XmlNode = doc.DocumentElement
                Dim FolderElement As XmlElement
                For Each FolderElement In dnnRoot.SelectNodes("folders/folder")
                    Dim objDesktopModules As New DesktopModuleController
                    Dim objDesktopModule As New DesktopModuleInfo

                    objDesktopModule.DesktopModuleID = Null.NullInteger
                    objDesktopModule.ModuleName = XmlUtils.GetNodeValue(FolderElement, "modulename")
                    objDesktopModule.FolderName = XmlUtils.GetNodeValue(FolderElement, "foldername")
                    objDesktopModule.FriendlyName = XmlUtils.GetNodeValue(FolderElement, "friendlyname")
                    If objDesktopModule.FolderName = "" Then
                        objDesktopModule.FolderName = objDesktopModule.ModuleName
                    End If
                    objDesktopModule.Description = XmlUtils.GetNodeValue(FolderElement, "description")
                    objDesktopModule.IsPremium = False
                    objDesktopModule.IsAdmin = False
                    objDesktopModule.Version = XmlUtils.GetNodeValue(FolderElement, "version")
                    objDesktopModule.BusinessControllerClass = XmlUtils.GetNodeValue(FolderElement, "businesscontrollerclass")
                    objDesktopModule.CompatibleVersions = XmlUtils.GetNodeValue(FolderElement, "compatibleversions")
                    objDesktopModule.Dependencies = XmlUtils.GetNodeValue(FolderElement, "dependencies")
                    objDesktopModule.Permissions = XmlUtils.GetNodeValue(FolderElement, "permissions")

                    objDesktopModule.DesktopModuleID = objDesktopModules.AddDesktopModule(objDesktopModule)

                    If objDesktopModule.BusinessControllerClass <> "" Then
                        Try
                            Dim objController As Object = Framework.Reflection.CreateObject(objDesktopModule.BusinessControllerClass, "")
                            objDesktopModule.SupportedFeatures = 0
                            objDesktopModule.IsPortable = (TypeOf objController Is IPortable)
                            objDesktopModule.IsSearchable = (TypeOf objController Is ISearchable)
                            objDesktopModule.IsUpgradeable = (TypeOf objController Is IUpgradeable)
                            objDesktopModules.UpdateDesktopModule(objDesktopModule)
                        Catch
                            ' error instantiating BusinessControllerClass
                        End Try
                    End If

                    Dim ModuleElement As XmlElement
                    For Each ModuleElement In FolderElement.SelectNodes("modules/module")
                        Dim objModuleDefinitions As New ModuleDefinitionController
                        Dim objModuleDefinition As New ModuleDefinitionInfo

                        objModuleDefinition.ModuleDefID = Null.NullInteger
                        objModuleDefinition.DesktopModuleID = objDesktopModule.DesktopModuleID
                        objModuleDefinition.FriendlyName = XmlUtils.GetNodeValue(ModuleElement, "friendlyname")
                        objModuleDefinition.DefaultCacheTime = XmlUtils.GetNodeValueInt(ModuleElement, "cachetime")

                        objModuleDefinition.ModuleDefID = objModuleDefinitions.AddModuleDefinition(objModuleDefinition)

                        Dim ControlElement As XmlElement
                        For Each ControlElement In ModuleElement.SelectNodes("controls/control")
                            Dim objModuleControls As New ModuleControlController
                            Dim objModuleControl As New ModuleControlInfo

                            objModuleControl.ModuleControlID = Null.NullInteger
                            objModuleControl.ModuleDefID = objModuleDefinition.ModuleDefID
                            objModuleControl.ControlKey = XmlUtils.GetNodeValue(ControlElement, "key")
                            objModuleControl.ControlSrc = XmlUtils.GetNodeValue(ControlElement, "src")
                            objModuleControl.ControlTitle = XmlUtils.GetNodeValue(ControlElement, "title")
                            Select Case XmlUtils.GetNodeValue(ControlElement, "type")
                                Case "Anonymous" : objModuleControl.ControlType = SecurityAccessLevel.Anonymous
                                Case "View" : objModuleControl.ControlType = SecurityAccessLevel.View
                                Case "Edit" : objModuleControl.ControlType = SecurityAccessLevel.Edit
                                Case "Admin" : objModuleControl.ControlType = SecurityAccessLevel.Admin
                                Case "Host" : objModuleControl.ControlType = SecurityAccessLevel.Host
                            End Select
                            objModuleControl.HelpURL = XmlUtils.GetNodeValue(ControlElement, "helpurl")
                            objModuleControl.IconFile = XmlUtils.GetNodeValue(ControlElement, "iconfile")
                            objModuleControl.ViewOrder = XmlUtils.GetNodeValueInt(ControlElement, "vieworder")
                            If XmlUtils.GetNodeValue(ControlElement, "supportspartialrendering") <> "" Then
                                objModuleControl.SupportsPartialRendering = Boolean.Parse(XmlUtils.GetNodeValue(ControlElement, "supportspartialrendering"))
                            Else
                                objModuleControl.SupportsPartialRendering = False
                            End If

                            ModuleControlController.AddModuleControl(objModuleControl)
                        Next ControlElement
                    Next ModuleElement

                Next FolderElement

                Response.Redirect(NavigateURL(), True)
            Catch
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("ImportManifest.ErrorMessage", Me.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' </summary>
        ''' <remarks>
        ''' Loads the cboSource control list with locations of controls.
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub ImportControl(ByVal strControl As String)
            Try
                Dim strFile As String = Path.GetFileName(strControl).Replace(".ascx", "")
                Dim strFolder As String = Path.GetDirectoryName(strControl).Replace("DesktopModules\", "")

                Dim objDesktopModules As New DesktopModuleController
                Dim objDesktopModule As New DesktopModuleInfo

                objDesktopModule.DesktopModuleID = Null.NullInteger
                objDesktopModule.ModuleName = strFile
                objDesktopModule.FolderName = strFolder
                objDesktopModule.FriendlyName = strFile
                objDesktopModule.Description = strFile
                objDesktopModule.IsPremium = False
                objDesktopModule.IsAdmin = False
                objDesktopModule.Version = "01.00.00"
                objDesktopModule.BusinessControllerClass = ""
                objDesktopModule.CompatibleVersions = ""
                objDesktopModule.Dependencies = ""
                objDesktopModule.Permissions = ""

                objDesktopModule.DesktopModuleID = objDesktopModules.AddDesktopModule(objDesktopModule)

                Dim objModuleDefinitions As New ModuleDefinitionController
                Dim objModuleDefinition As New ModuleDefinitionInfo

                objModuleDefinition.ModuleDefID = Null.NullInteger
                objModuleDefinition.DesktopModuleID = objDesktopModule.DesktopModuleID
                objModuleDefinition.FriendlyName = strFile
                objModuleDefinition.DefaultCacheTime = 0

                objModuleDefinition.ModuleDefID = objModuleDefinitions.AddModuleDefinition(objModuleDefinition)

                Dim objModuleControls As New ModuleControlController
                Dim objModuleControl As New ModuleControlInfo

                objModuleControl.ModuleControlID = Null.NullInteger
                objModuleControl.ModuleDefID = objModuleDefinition.ModuleDefID
                objModuleControl.ControlKey = ""
                objModuleControl.ControlSrc = strControl
                objModuleControl.ControlTitle = ""
                objModuleControl.ControlType = SecurityAccessLevel.View
                objModuleControl.HelpURL = ""
                objModuleControl.IconFile = ""
                objModuleControl.ViewOrder = 0
                objModuleControl.SupportsPartialRendering = False

                ModuleControlController.AddModuleControl(objModuleControl)

                Response.Redirect(NavigateURL(), True)
            Catch
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("ImportControl.ErrorMessage", Me.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
            End Try
        End Sub

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack Then
                    BindManifestList("DesktopModules")
                    cboManifest.Items.Insert(0, New ListItem("<" + Services.Localization.Localization.GetString("None_Specified") + ">", ""))
                    BindControlList("DesktopModules")
                    cboControl.Items.Insert(0, New ListItem("<" + Services.Localization.Localization.GetString("None_Specified") + ">", ""))
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdImportControl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdImportControl.Click
            If Not cboControl.SelectedItem Is Nothing Then
                If cboControl.SelectedItem.Value <> "" Then
                    ImportControl(cboControl.SelectedItem.Value)
                End If
            End If
        End Sub

        Private Sub cmdImportManifest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdImportManifest.Click
            If Not cboManifest.SelectedItem Is Nothing Then
                If cboManifest.SelectedItem.Value <> "" Then
                    ImportManifest(cboManifest.SelectedItem.Value)
                End If
            End If
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

    End Class

End Namespace
