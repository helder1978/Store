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

Imports System.IO
Imports System.Xml.Serialization
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Modules.Admin.ResourceInstaller
Imports DotNetNuke.Services.Localization

Namespace DotNetNuke.Modules.Admin.ModuleDefinitions

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The PrivateAssembly PortalModuleBase is used to create a Private Assembly Package
    ''' from this module
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	1/14/2005	created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class PrivateAssembly
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Init runs when the control is initialised.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	1/14/2005	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	1/14/2005	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not Page.IsPostBack Then
                If Not (Request.QueryString("desktopmoduleid") Is Nothing) Then
                    Dim objDesktopModules As New DesktopModuleController
                    Dim objDesktopModule As DesktopModuleInfo = objDesktopModules.GetDesktopModule(Int32.Parse(Request.QueryString("desktopmoduleid")))
                    If Not objDesktopModule Is Nothing Then
                        txtFileName.Text = objDesktopModule.ModuleName & "_" & objDesktopModule.Version & "_Install" & ".zip"

                        If Not objDesktopModule.IsAdmin Then
                            'Create the DirectoryInfo object for the folder
                            Dim folder As New DirectoryInfo(Common.Globals.ApplicationMapPath & "\DesktopModules\" & objDesktopModule.FolderName)
                            If folder.Exists Then
                                'Determine Visibility of Source check box
                                rowSource.Visible = (folder.GetFiles("*.??proj").Length > 0)
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdCancel_Click runs when the Cancel Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	1/14/2005	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL())
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdCreate_Click runs when the Create Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	1/14/2005	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click

            Dim strFileName As String = txtFileName.Text
            If strFileName = "" Then
                UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("Create.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
            Else
                If Not strFileName.ToLower.EndsWith(".zip") Then
                    strFileName += ".zip"
                End If

                Dim mid As Integer = Null.NullInteger
                If Not (Request.QueryString("desktopmoduleid") Is Nothing) Then
                    mid = Int32.Parse(Request.QueryString("desktopmoduleid"))
                End If

                If mid > 0 Then
                    Dim PaWriter As New PaWriter(chkSource.Checked, chkManifest.Checked, chkPrivate.Checked, strFileName)

                    Dim PaZipName As String = PaWriter.CreatePrivateAssembly(mid)

                    If PaWriter.ProgressLog.Valid Then
                        lblMessage.Text = String.Format(Localization.GetString("LOG.MESSAGE.Success", LocalResourceFile), PaZipName)
                        lblMessage.CssClass = "Head"
                    Else
                        lblMessage.Text = Localization.GetString("LOG.MESSAGE.Error", LocalResourceFile)
                        lblMessage.CssClass = "NormalRed"
                    End If

                    Dim FileManagerModule As ModuleInfo = (New ModuleController).GetModuleByDefinition(Null.NullInteger, "File Manager")

                    If Not FileManagerModule Is Nothing Then
                        lblLink.Text = String.Format(Localization.GetString("lblLink", LocalResourceFile), NavigateURL(FileManagerModule.TabID))
                    End If
                    divLog.Controls.Add(PaWriter.ProgressLog.GetLogsTable)
                    pnlLogs.Visible = True

                End If
            End If

        End Sub

#End Region

    End Class
End Namespace