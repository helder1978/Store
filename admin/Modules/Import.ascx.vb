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
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports System.Reflection
Imports System.IO
Imports DotNetNuke.Entities.Modules
Imports System.Xml
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.FileSystem

Namespace DotNetNuke.Modules.Admin.Modules

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' </summary>
	''' <returns></returns>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' </history>
	''' -----------------------------------------------------------------------------
    Partial  Class Import
		Inherits Entities.Modules.PortalModuleBase

#Region "Controls"



#End Region

#Region "Private Members"

        Private Shadows ModuleId As Integer = -1

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
                If Not Request.QueryString("moduleid") Is Nothing Then
                    ModuleId = Int32.Parse(Request.QueryString("moduleid"))
                End If

                If Not Page.IsPostBack Then
                    cboFolders.Items.Insert(0, New ListItem("<" + Services.Localization.Localization.GetString("None_Specified") + ">", "-"))
                    Dim folders As ArrayList = FileSystemUtils.GetFoldersByUser(PortalId, False, False, "READ, WRITE")
                    For Each folder As FolderInfo In folders
                        Dim FolderItem As New ListItem
                        If folder.FolderPath = Null.NullString Then
                            FolderItem.Text = Localization.GetString("Root", Me.LocalResourceFile)
                        Else
                            FolderItem.Text = folder.FolderPath
                        End If
                        FolderItem.Value = folder.FolderPath
                        cboFolders.Items.Add(FolderItem)
                    Next
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cboFolders_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFolders.SelectedIndexChanged
            cboFiles.Items.Clear()
            If cboFolders.SelectedIndex <> 0 Then
                Dim objModules As New ModuleController
                Dim objModule As ModuleInfo = objModules.GetModule(ModuleId, TabId, False)
                If Not objModule Is Nothing Then
                    Dim arrFiles As ArrayList = Common.Globals.GetFileList(PortalId, "xml", False, cboFolders.SelectedItem.Value)
                    Dim objFile As FileItem
                    For Each objFile In arrFiles
                        If objFile.Text.IndexOf("content." & CleanName(objModule.ModuleName) & ".") <> -1 Then
                            cboFiles.Items.Add(New ListItem(objFile.Text.Replace("content." & CleanName(objModule.ModuleName) & ".", ""), objFile.Text))
                        End If
                        ' legacy support for files which used the FriendlyName
                        If objFile.Text.IndexOf("content." & CleanName(objModule.FriendlyName) & ".") <> -1 Then
                            cboFiles.Items.Add(New ListItem(objFile.Text.Replace("content." & CleanName(objModule.FriendlyName) & ".", ""), objFile.Text))
                        End If
                    Next
                End If
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
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
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImport.Click
            Try
                If Not cboFiles.SelectedItem Is Nothing Then
                    Dim objModules As New ModuleController
                    Dim objModule As ModuleInfo = objModules.GetModule(ModuleId, TabId, False)
                    If Not objModule Is Nothing Then
                        Dim strMessage As String = ImportModule(ModuleId, cboFiles.SelectedItem.Value, cboFolders.SelectedItem.Value)
                        If strMessage = "" Then
                            Response.Redirect(NavigateURL(), True)
                        Else
                            UI.Skins.Skin.AddModuleMessage(Me, strMessage, UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                        End If
                    End If
                Else
                    UI.Skins.Skin.AddModuleMessage(Me, "Please specify the file to import", UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

#Region "Private Methods"

        Private Function ImportModule(ByVal ModuleId As Integer, ByVal FileName As String, ByVal Folder As String) As String

            Dim strMessage As String = ""

            Dim objModules As New ModuleController
            Dim objModule As ModuleInfo = objModules.GetModule(ModuleId, TabId, False)
            If Not objModule Is Nothing Then
                If FileName.IndexOf("." & CleanName(objModule.ModuleName) & ".") <> -1 Or FileName.IndexOf("." & CleanName(objModule.FriendlyName) & ".") <> -1 Then
                    If objModule.BusinessControllerClass <> "" And objModule.IsPortable Then
                        Try
                            Dim objObject As Object = Framework.Reflection.CreateObject(objModule.BusinessControllerClass, objModule.BusinessControllerClass)

                            If TypeOf objObject Is IPortable Then

                                Dim objStreamReader As StreamReader
                                objStreamReader = File.OpenText(PortalSettings.HomeDirectoryMapPath & Folder & FileName)
                                Dim Content As String = objStreamReader.ReadToEnd
                                objStreamReader.Close()

                                Dim xmlDoc As New XmlDocument
                                Try
                                    xmlDoc.LoadXml(Content)
                                Catch
                                    strMessage = Localization.GetString("NotValidXml", Me.LocalResourceFile)
                                End Try

                                If strMessage = "" Then
                                    Dim strType As String = xmlDoc.DocumentElement.GetAttribute("type").ToString
                                    If strType = CleanName(objModule.ModuleName) Or strType = CleanName(objModule.FriendlyName) Then
                                        Dim strVersion As String = xmlDoc.DocumentElement.GetAttribute("version").ToString

                                        CType(objObject, IPortable).ImportModule(ModuleId, xmlDoc.DocumentElement.InnerXml, strVersion, UserInfo.UserID)

                                        Response.Redirect(NavigateURL(), True)
                                    Else
                                        strMessage = Localization.GetString("NotCorrectType", Me.LocalResourceFile)
                                    End If
                                End If
                            Else
                                strMessage = Localization.GetString("ImportNotSupported", Me.LocalResourceFile)
                            End If
                        Catch
                            strMessage = Localization.GetString("Error", Me.LocalResourceFile)
                        End Try
                    Else
                        strMessage = Localization.GetString("ImportNotSupported", Me.LocalResourceFile)
                    End If
                Else
                    strMessage = Localization.GetString("NotCorrectType", Me.LocalResourceFile)
                End If
            End If

            Return strMessage

        End Function

        Private Function CleanName(ByVal Name As String) As String

            Dim strName As String = Name
            Dim strBadChars As String = ". ~`!@#$%^&*()-_+={[}]|\:;<,>?/" & Chr(34) & Chr(39)

            Dim intCounter As Integer
            For intCounter = 0 To Len(strBadChars) - 1
                strName = strName.Replace(strBadChars.Substring(intCounter, 1), "")
            Next intCounter

            Return strName

        End Function

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
