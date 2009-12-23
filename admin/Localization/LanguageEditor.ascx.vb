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
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Xml
Imports DNNControls = DotNetNuke.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Services.Localization

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Manages translations for Resource files
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[vmasanas]	10/04/2004  Created
    ''' 	[vmasanas]	25/03/2006	Modified to support new host resources and incremental saving
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class LanguageEditor
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

#Region "Private Enums"
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Identifies images in TreeView
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	07/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Enum eImageType
            Folder = 0
            Page = 1
        End Enum
#End Region

#Region "Private Methods"
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Initializes ResourceFile treeView
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	25/03/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub InitTree()
            Dim idx, idx2 As Integer

            ' configure tree
            DNNTree.SystemImagesPath = ResolveUrl("~/images/")
            DNNTree.ImageList.Add(ResolveUrl("~/images/folder.gif"))
            DNNTree.ImageList.Add(ResolveUrl("~/images/file.gif"))
            DNNTree.IndentWidth = 10
            DNNTree.CollapsedNodeImage = ResolveUrl("~/images/max.gif")
            DNNTree.ExpandedNodeImage = ResolveUrl("~/images/min.gif")

            'Local resources
            idx = DNNTree.TreeNodes.Add("Local Resources")
            DNNTree.TreeNodes(idx).Key = "Local Resources"
            DNNTree.TreeNodes(idx).ToolTip = "Local Resources"
            DNNTree.TreeNodes(idx).ImageIndex = eImageType.Folder
            DNNTree.TreeNodes(idx).ClickAction = DNNControls.eClickAction.Expand

            'admin
            idx2 = DNNTree.TreeNodes(idx).TreeNodes.Add("Admin")
            DNNTree.TreeNodes(idx).TreeNodes(idx2).Key = "Admin"
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ToolTip = "Admin"
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ImageIndex = eImageType.Folder
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ClickAction = DNNControls.eClickAction.Expand
            PopulateTree(DNNTree.TreeNodes(idx).TreeNodes(idx2).TreeNodes, Server.MapPath("~\admin"))
            'controls
            idx2 = DNNTree.TreeNodes(idx).TreeNodes.Add("Controls")
            DNNTree.TreeNodes(idx).TreeNodes(idx2).Key = "Controls"
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ToolTip = "Controls"
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ImageIndex = eImageType.Folder
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ClickAction = DNNControls.eClickAction.Expand
            PopulateTree(DNNTree.TreeNodes(idx).TreeNodes(idx2).TreeNodes, Server.MapPath("~\controls"))
            'desktopmodules
            idx2 = DNNTree.TreeNodes(idx).TreeNodes.Add("DesktopModules")
            DNNTree.TreeNodes(idx).TreeNodes(idx2).Key = "DesktopModules"
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ToolTip = "DesktopModules"
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ImageIndex = eImageType.Folder
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ClickAction = DNNControls.eClickAction.Expand
            PopulateTree(DNNTree.TreeNodes(idx).TreeNodes(idx2).TreeNodes, Server.MapPath("~\desktopmodules"))
            'providers
            idx2 = DNNTree.TreeNodes(idx).TreeNodes.Add("Providers")
            DNNTree.TreeNodes(idx).TreeNodes(idx2).Key = "Providers"
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ToolTip = "Providers"
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ImageIndex = eImageType.Folder
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ClickAction = DNNControls.eClickAction.Expand
            PopulateTree(DNNTree.TreeNodes(idx).TreeNodes(idx2).TreeNodes, Server.MapPath("~\providers"))
            'install folder
            idx2 = DNNTree.TreeNodes(idx).TreeNodes.Add("Install")
            DNNTree.TreeNodes(idx).TreeNodes(idx2).Key = "Install"
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ToolTip = "Install"
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ImageIndex = eImageType.Folder
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ClickAction = DNNControls.eClickAction.Expand
            PopulateTree(DNNTree.TreeNodes(idx).TreeNodes(idx2).TreeNodes, Server.MapPath("~\Install"))

            ' add application resources
            idx = DNNTree.TreeNodes.Add("Global Resources")
            DNNTree.TreeNodes(idx).Key = "Global Resources"
            DNNTree.TreeNodes(idx).ToolTip = "Global Resources"
            DNNTree.TreeNodes(idx).ImageIndex = eImageType.Folder
            DNNTree.TreeNodes(idx).ClickAction = DNNControls.eClickAction.Expand
            idx2 = DNNTree.TreeNodes(idx).TreeNodes.Add(Path.GetFileNameWithoutExtension(Localization.GlobalResourceFile))
            DNNTree.TreeNodes(idx).TreeNodes(idx2).Key = Server.MapPath(Localization.GlobalResourceFile)
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ToolTip = DNNTree.TreeNodes(idx).TreeNodes(idx2).Text
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ImageIndex = eImageType.Page
            idx2 = DNNTree.TreeNodes(idx).TreeNodes.Add(Path.GetFileNameWithoutExtension(Localization.SharedResourceFile))
            DNNTree.TreeNodes(idx).TreeNodes(idx2).Key = Server.MapPath(Localization.SharedResourceFile)
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ToolTip = DNNTree.TreeNodes(idx).TreeNodes(idx2).Text
            DNNTree.TreeNodes(idx).TreeNodes(idx2).ImageIndex = eImageType.Page

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads Local Resource files in the tree
        ''' </summary>
        ''' <param name="Nodes">Node collection where to add new nodes</param>
        ''' <param name="_path">Folder to search for</param>
        ''' <returns>true if a Local Resource file is found in the given path</returns>
        ''' <remarks>
        ''' The Node collection will only contain en-US resources
        ''' Only folders with Resource files will be included in the tree
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	07/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function PopulateTree(ByVal Nodes As DNNControls.TreeNodeCollection, ByVal _path As String) As Boolean
            Dim folders As String() = Directory.GetDirectories(_path)
            Dim folder As String
            Dim found As Boolean = False
            Dim nodeIndex As Integer
            Dim objFile As IO.FileInfo
            Dim objFolder As DirectoryInfo
            Dim node, leaf As DNNControls.TreeNode

            For Each folder In folders
                objFolder = New System.IO.DirectoryInfo(folder)
                node = New DNNControls.TreeNode(objFolder.Name)
                node.Key = objFolder.FullName
                node.ToolTip = objFolder.Name
                node.ImageIndex = eImageType.Folder
                node.ClickAction = DNNControls.eClickAction.Expand
                Nodes.Add(node)

                If objFolder.Name = Localization.LocalResourceDirectory Then
                    ' found local resource folder, add resources
                    For Each objFile In objFolder.GetFiles("*.ascx.resx")
                        leaf = New DNNControls.TreeNode(Path.GetFileNameWithoutExtension(objFile.Name))
                        leaf.Key = objFile.FullName
                        leaf.ToolTip = objFile.Name
                        leaf.ImageIndex = eImageType.Page
                        node.TreeNodes.Add(leaf)
                    Next
                    For Each objFile In objFolder.GetFiles("*.aspx.resx")
                        leaf = New DNNControls.TreeNode(Path.GetFileNameWithoutExtension(objFile.Name))
                        leaf.Key = objFile.FullName
                        leaf.ToolTip = objFile.Name
                        leaf.ImageIndex = eImageType.Page
                        node.TreeNodes.Add(leaf)
                    Next
                    ' add LocalSharedResources if found
                    If File.Exists(Path.Combine(folder, Localization.LocalSharedResourceFile)) Then
                        objFile = New System.IO.FileInfo(Path.Combine(folder, Localization.LocalSharedResourceFile))
                        leaf = New DNNControls.TreeNode(Path.GetFileNameWithoutExtension(objFile.Name))
                        leaf.Key = objFile.FullName
                        leaf.ToolTip = objFile.Name
                        leaf.ImageIndex = eImageType.Page
                        node.TreeNodes.Add(leaf)
                    End If
                    found = True
                Else
                    'recurse
                    If PopulateTree(node.TreeNodes, folder) Then
                        ' found resources
                        found = True
                    Else
                        ' not found, remove node
                        Nodes.Remove(node)
                    End If
                End If
            Next

            Return found
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads suported locales
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindLocaleList()
            Dim ds As New DataSet
            Dim dv As DataView
            Dim i As Integer
            Dim localeKey As String
            Dim localeName As String


            ds.ReadXml(Server.MapPath(Localization.SupportedLocalesFile))
            dv = ds.Tables(0).DefaultView
            dv.Sort = "name ASC"

            cboLocales.Items.Clear()
            For i = 0 To dv.Count - 1
                localeKey = Convert.ToString(dv(i)("key"))
                Dim cinfo As New CultureInfo(localeKey)

                Try
                    If rbDisplay.SelectedValue = "Native" Then
                        localeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cinfo.NativeName)
                    Else
                        localeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cinfo.EnglishName)
                    End If
                Catch
                    localeName = Convert.ToString(dv(i)("name")) + " (" + localeKey + ")"
                End Try
                cboLocales.Items.Add(New ListItem(localeName, localeKey))
            Next
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Saves / Gets the selected resource file being edited in viewstate
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	07/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Property SelectedResourceFile() As String
            Get
                Return Viewstate("SelectedResourceFile").ToString
            End Get
            Set(ByVal Value As String)
                viewstate("SelectedResourceFile") = Value
                lblResourceFile.Text = Value.Replace(ApplicationMapPath, "")
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads Resource information into the datagrid
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        '''     [vmasanas}  25/03/2006  Modified to support new features
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindGrid()
            Dim EditTable As Hashtable
            Dim DefaultTable As Hashtable


            EditTable = LoadFile(rbMode.SelectedValue, "Edit")
            DefaultTable = LoadFile(rbMode.SelectedValue, "Default")

            ' check edit table
            ' if empty, just use default
            If EditTable.Count = 0 Then
                EditTable = DefaultTable
            Else
                'remove obsolete keys
                Dim ToBeDeleted As New ArrayList
                For Each key As String In EditTable.Keys
                    If Not DefaultTable.Contains(key) Then
                        ToBeDeleted.Add(key)
                    End If
                Next
                If ToBeDeleted.Count > 0 Then
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("Obsolete", LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    For Each key As String In ToBeDeleted
                        EditTable.Remove(key)
                    Next
                End If

                'add missing keys
                For Each key As String In DefaultTable.Keys
                    If Not EditTable.Contains(key) Then
                        EditTable.Add(key, DefaultTable(key))
                    Else
                        ' Update default value
                        Dim p As Pair = CType(EditTable(key), Pair)
                        p.Second = CType(DefaultTable(key), Pair).First
                        EditTable(key) = p
                    End If
                Next
            End If

            Dim s As New SortedList(EditTable)

            dgEditor.DataSource = s
            dgEditor.DataBind()

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Returns the resource file name for a given resource and language
        ''' </summary>
        ''' <param name="mode">Identifies the resource being searched (System, Host, Portal)</param>
        ''' <returns>Localized File Name</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' 	[vmasanas]	25/03/2006	Modified to support new host resources and incremental saving
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function ResourceFile(ByVal language As String, ByVal mode As String) As String
            Dim resourcefilename As String = SelectedResourceFile

            If Not resourcefilename.EndsWith(".resx") Then
                resourcefilename &= ".resx"
            End If

            If language <> Localization.SystemLocale Then
                resourcefilename = resourcefilename.Substring(0, resourcefilename.Length - 5) + "." + language + ".resx"
            End If

            If mode = "Host" Then
                resourcefilename = resourcefilename.Substring(0, resourcefilename.Length - 5) + "." + "Host.resx"
            ElseIf mode = "Portal" Then
                resourcefilename = resourcefilename.Substring(0, resourcefilename.Length - 5) + "." + "Portal-" + PortalId.ToString + ".resx"
            End If

            Return resourcefilename

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads resources from file 
        ''' </summary>
        ''' <param name="mode">Active editor mode</param>
        ''' <param name="type">Resource being loaded (edit or default)</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Depending on the editor mode, resources will be overrided using default DNN schema.
        ''' "Edit" resources will only load selected file.
        ''' When loading "Default" resources (to be used on the editor as helpers) fallback resource
        ''' chain will be used in order for the editor to be able to correctly see what 
        ''' is the current default value for the any key. This process depends on the current active
        ''' editor mode:
        ''' - System: when editing system base resources on en-US needs to be loaded
        ''' - Host: base en-US, and base locale especific resource
        ''' - Portal: base en-US, host override for en-US, base locale especific resource, and host override 
        ''' for locale
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	25/03/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function LoadFile(ByVal mode As String, ByVal type As String) As Hashtable
            Dim file As String
            Dim ht As New Hashtable

            Select Case type
                Case "Edit"
                    ' Only load resources from the file being edited
                    file = ResourceFile(cboLocales.SelectedValue, mode)
                    ht = LoadResource(ht, file)
                Case "Default"
                    ' Load system default
                    file = ResourceFile(Localization.SystemLocale, "System")
                    ht = LoadResource(ht, file)

                    Select Case mode
                        Case "Host"
                            If cboLocales.SelectedValue <> Localization.SystemLocale Then
                                ' Load base file for selected locale
                                file = ResourceFile(cboLocales.SelectedValue, "System")
                                ht = LoadResource(ht, file)
                            End If
                        Case "Portal"
                            'Load host override for default locale
                            file = ResourceFile(Localization.SystemLocale, "Host")
                            ht = LoadResource(ht, file)

                            If cboLocales.SelectedValue <> Localization.SystemLocale Then
                                ' Load base file for locale
                                file = ResourceFile(cboLocales.SelectedValue, "System")
                                ht = LoadResource(ht, file)

                                'Load host override for selected locale
                                file = ResourceFile(cboLocales.SelectedValue, "Host")
                                ht = LoadResource(ht, file)
                            End If
                    End Select

            End Select

            Return ht

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads resources from file into the HastTable
        ''' </summary>
        ''' <param name="ht">Current resources HashTable</param>
        ''' <param name="filepath">Resources file</param>
        ''' <returns>Base table updated with new resources </returns>
        ''' <remarks>
        ''' Returned hashtable uses resourcekey as key.
        ''' Value contains a Pair object where:
        '''  First=>value to be edited
        '''  Second=>default value
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	25/03/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function LoadResource(ByVal ht As Hashtable, ByVal filepath As String) As Hashtable
            Dim d As New XmlDocument
            Dim xmlLoaded As Boolean = False
            Try
                d.Load(filepath)
                xmlLoaded = True
            Catch    'exc As Exception
                xmlLoaded = False
            End Try
            If xmlLoaded Then
                Dim n As XmlNode
                For Each n In d.SelectNodes("root/data")
                    If n.NodeType <> XmlNodeType.Comment Then
                        Dim val As String = n.SelectSingleNode("value").InnerXml
                        If ht(n.Attributes("name").Value) Is Nothing Then
                            ht.Add(n.Attributes("name").Value, New Pair(val, val))
                        Else
                            ht(n.Attributes("name").Value) = New Pair(val, val)
                        End If
                    End If
                Next n
            End If
            Return ht
        End Function

#End Region

#Region "Protected Methods"
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Configures the initial visibility status of the default label
        ''' </summary>
        ''' <param name="p"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Vicenç]	26/03/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Function ExpandDefault(ByVal p As Pair) As Boolean

            Return p.Second.ToString().Length < 150

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Builds the url for the lang. html editor control
        ''' </summary>
        ''' <param name="name"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	07/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Function OpenFullEditor(ByVal name As String) As String
            Dim file As String
            file = SelectedResourceFile.Replace(Server.MapPath(Common.Globals.ApplicationPath + "/"), "")
            Return EditUrl("name", name, "fulleditor", "locale=" & cboLocales.SelectedValue, "resourcefile=" & QueryStringEncode(file), "mode=" & rbMode.SelectedValue)
        End Function
#End Region

#Region "Event Handlers"
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads suported locales and shows default values
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' 	[vmasanas]	25/03/2006	Modified to support new host resources and incremental saving
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try
                If Not Page.IsPostBack Then
                    ClientAPI.AddButtonConfirm(cmdDelete, Localization.GetString("DeleteItem"))

                    ' initialized available locales
                    BindLocaleList()

                    ' init tree
                    InitTree()

                    ' If returning from full editor, use params
                    ' else load system global resource file by default
                    If Request.QueryString("locale") <> "" Then
                        cboLocales.SelectedValue = Request.QueryString("locale").ToString
                    Else
                        cboLocales.SelectedValue = Localization.SystemLocale
                    End If
                    If (PortalSettings.ActiveTab.ParentId = PortalSettings.AdminTabId) Then
                        rbMode.SelectedValue = "Portal"
                        rowMode.Visible = False
                    Else
                        ' portal mode only available on admin menu
                        rbMode.Items.RemoveAt(2)
                        Dim mode As String = Request.QueryString("mode")
                        If mode <> "" AndAlso Not rbMode.Items.FindByValue(mode) Is Nothing Then
                            rbMode.SelectedValue = mode
                        End If
                    End If
                    If Request.QueryString("resourcefile") <> "" Then
                        SelectedResourceFile = Server.MapPath("~/" + QueryStringDecode(Request.QueryString("resourcefile")))
                    Else
                        SelectedResourceFile = Server.MapPath(Localization.GlobalResourceFile)
                    End If
                    DNNTree.SelectNodeByKey(SelectedResourceFile)

                    BindGrid()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads localized file
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' If a localized file does not exist for the selected language it is created using default values
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' 	[vmasanas]	25/03/2006	Modified to support new host resources and incremental saving
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cboLocales_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLocales.SelectedIndexChanged
            Try
                BindGrid()
            Catch
                UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Rebinds the grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	25/03/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub rbMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbMode.SelectedIndexChanged
            Try
                BindGrid()
            Catch
                UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Rebinds the grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	25/03/2006	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub chkHighlight_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkHighlight.CheckedChanged
            Try
                BindGrid()
            Catch exc As Exception    'Module failed to load
                UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Open the selected resource file in editor or expand/collapse node if is folder
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	07/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub DNNTree_NodeClick(ByVal source As Object, ByVal e As DotNetNuke.UI.WebControls.DNNTreeNodeClickEventArgs) Handles DNNTree.NodeClick
            Try
                If e.Node.ImageIndex = eImageType.Page Then
                    SelectedResourceFile = e.Node.Key
                    Try
                        BindGrid()
                    Catch
                        UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    End Try
                ElseIf e.Node.IsExpanded Then
                    e.Node.Collapse()
                Else
                    e.Node.Expand()
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Updates all values from the datagrid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' 	[vmasanas]	25/03/2006	Modified to support new host resources and incremental saving
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            Dim di As DataGridItem
            Dim node, nodeData, parent As XmlNode
            Dim attr As XmlAttribute
            Dim resDoc As New XmlDocument
            Dim defDoc As New XmlDocument
            Dim filename As String

            Try
                filename = ResourceFile(cboLocales.SelectedValue, rbMode.SelectedValue)
                If Not File.Exists(filename) Then
                    ' load system default
                    resDoc.Load(ResourceFile(Localization.SystemLocale, "System"))
                Else
                    resDoc.Load(filename)
                End If
                defDoc.Load(ResourceFile(Localization.SystemLocale, "System"))

                Select Case rbMode.SelectedValue
                    Case "System"
                        ' this will save all items
                        For Each di In dgEditor.Items
                            If (di.ItemType = ListItemType.Item Or di.ItemType = ListItemType.AlternatingItem) Then
                                Dim ctl1 As TextBox = CType(di.Cells(0).FindControl("txtValue"), TextBox)
                                node = resDoc.SelectSingleNode("//root/data[@name='" + di.Cells(1).Text + "']/value")
                                If node Is Nothing Then
                                    ' missing entry
                                    nodeData = resDoc.CreateElement("data")
                                    attr = resDoc.CreateAttribute("name")
                                    attr.Value = di.Cells(1).Text
                                    nodeData.Attributes.Append(attr)
                                    resDoc.SelectSingleNode("//root").AppendChild(nodeData)

                                    node = nodeData.AppendChild(resDoc.CreateElement("value"))
                                End If
                                node.InnerXml = Server.HtmlEncode(ctl1.Text)
                            End If
                        Next
                    Case "Host", "Portal"
                        ' only items different from default will be saved
                        For Each di In dgEditor.Items
                            If (di.ItemType = ListItemType.Item Or di.ItemType = ListItemType.AlternatingItem) Then
                                Dim ctl1 As TextBox = CType(di.Cells(0).FindControl("txtValue"), TextBox)
                                Dim ctl2 As Label = CType(di.Cells(0).FindControl("lblDefault"), Label)

                                node = resDoc.SelectSingleNode("//root/data[@name='" + di.Cells(1).Text + "']/value")
                                If ctl1.Text <> ctl2.Text Then
                                    If node Is Nothing Then
                                        ' missing entry
                                        nodeData = resDoc.CreateElement("data")
                                        attr = resDoc.CreateAttribute("name")
                                        attr.Value = di.Cells(1).Text
                                        nodeData.Attributes.Append(attr)
                                        resDoc.SelectSingleNode("//root").AppendChild(nodeData)

                                        node = nodeData.AppendChild(resDoc.CreateElement("value"))
                                    End If
                                    node.InnerXml = Server.HtmlEncode(ctl1.Text)
                                ElseIf Not node Is Nothing Then
                                    ' remove item = default
                                    resDoc.SelectSingleNode("//root").RemoveChild(node.ParentNode)
                                End If
                            End If
                        Next
                End Select

                ' remove obsolete keys
                For Each node In resDoc.SelectNodes("//root/data")
                    If defDoc.SelectSingleNode("//root/data[@name='" + node.Attributes("name").Value + "']") Is Nothing Then
                        parent = node.ParentNode
                        parent.RemoveChild(node)
                    End If
                Next
                ' remove duplicate keys
                For Each node In resDoc.SelectNodes("//root/data")
                    If resDoc.SelectNodes("//root/data[@name='" + node.Attributes("name").Value + "']").Count > 1 Then
                        parent = node.ParentNode
                        parent.RemoveChild(node)
                    End If
                Next

                Select Case rbMode.SelectedValue
                    Case "System"
                        resDoc.Save(filename)
                    Case "Host", "Portal"
                        If resDoc.SelectNodes("//root/data").Count > 0 Then
                            ' there's something to save
                            resDoc.Save(filename)
                        Else
                            ' nothing to be saved, if file exists delete
                            If File.Exists(filename) Then
                                File.Delete(filename)
                            End If
                        End If
                End Select
                BindGrid()
            Catch exc As Exception    'Module failed to load
                UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
            End Try

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Deletes the localized file for a given locale
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' System Default file cannot be deleted
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' 	[vmasanas]	25/03/2006	Modified to support new host resources and incremental saving
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
            Try
                If cboLocales.SelectedValue = Localization.SystemLocale And rbMode.SelectedValue = "System" Then
                    UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("Delete.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                Else
                    Try
                        If File.Exists(ResourceFile(cboLocales.SelectedValue, rbMode.SelectedValue)) Then
                            File.SetAttributes(ResourceFile(cboLocales.SelectedValue, rbMode.SelectedValue), FileAttributes.Normal)
                            File.Delete(ResourceFile(cboLocales.SelectedValue, rbMode.SelectedValue))
                            UI.Skins.Skin.AddModuleMessage(Me, String.Format(Localization.GetString("Deleted", Me.LocalResourceFile), ResourceFile(cboLocales.SelectedValue, rbMode.SelectedValue)), UI.Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess)

                            cboLocales.SelectedValue = Localization.SystemLocale
                            BindGrid()
                        End If
                    Catch
                        UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    End Try
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Binds a data item to the datagrid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' Adds a warning message before leaving the page to edit in full editor so the user can save changes
        ''' Customizes edit textbox and default value
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	20/10/2004	Created
        ''' 	[vmasanas]	25/03/2006	Modified to support new host resources and incremental saving
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub dgEditor_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgEditor.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                    Dim c As HyperLink
                    c = CType(e.Item.FindControl("lnkEdit"), HyperLink)
                    If Not c Is Nothing Then
                        ClientAPI.AddButtonConfirm(c, Localization.GetString("SaveWarning", Me.LocalResourceFile))
                    End If

                    Dim p As Pair = CType(CType(e.Item.DataItem, DictionaryEntry).Value, Pair)

                    Dim t As TextBox
                    t = CType(e.Item.FindControl("txtValue"), TextBox)
                    If p.First.ToString() = p.Second.ToString() And chkHighlight.Checked And Not p.Second.ToString = "" Then
                        t.CssClass = "Pending"
                    End If
                    If p.First.ToString().Length > 30 Then
                        t.Height = New Unit("100")
                    End If
                    t.Text = Server.HtmlDecode(p.First.ToString())

                    Dim l As Label
                    l = CType(e.Item.FindControl("lblDefault"), Label)
                    l.Text = Server.HtmlDecode(p.Second.ToString())
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Returns to main control
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL())
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub rbDisplay_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDisplay.SelectedIndexChanged
            BindLocaleList()
        End Sub
#End Region

    End Class

End Namespace
