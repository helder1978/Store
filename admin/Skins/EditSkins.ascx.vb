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
Imports System.Configuration
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Localization

Namespace DotNetNuke.Modules.Admin.Skins

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The EditSkins PortalModuleBase is used to manage the Available Skins
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	9/13/2004	Updated to reflect design changes for Help, 508 support
    '''                       and localisation
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class EditSkins
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase
        Implements DotNetNuke.Entities.Modules.IActionable

#Region "Controls"

        Protected WithEvents lblMessage As System.Web.UI.WebControls.Label

#End Region

#Region "Private Methods"

        Private Function isFallbackSkin(ByVal skinPath As String) As Boolean
            Dim strDefaultSkinPath As String = (Common.Globals.HostMapPath & SkinInfo.RootSkin + glbDefaultSkinFolder).Replace("/", "\")
            If strDefaultSkinPath.EndsWith("\") Then
                strDefaultSkinPath = strDefaultSkinPath.Substring(0, strDefaultSkinPath.Length - 1)
            End If
            Return skinPath.IndexOf(strDefaultSkinPath, StringComparison.CurrentCultureIgnoreCase) <> -1
        End Function

        Private Function isFallbackContainer(ByVal skinPath As String) As Boolean
            Dim strDefaultContainerPath As String = (Common.Globals.HostMapPath & SkinInfo.RootContainer + glbDefaultSkinFolder).Replace("/", "\")
            If strDefaultContainerPath.EndsWith("\") Then
                strDefaultContainerPath = strDefaultContainerPath.Substring(0, strDefaultContainerPath.Length - 1)
            End If
            Return skinPath.IndexOf(strDefaultContainerPath, StringComparison.CurrentCultureIgnoreCase) <> -1
        End Function

        Private Sub ShowSkins()

            Dim strSkinPath As String = ApplicationMapPath.ToLower + cboSkins.SelectedItem.Value
            cboContainers.ClearSelection()

            Dim strGallery As String = ""

            If cboSkins.SelectedIndex > 0 Then
                strGallery += ProcessSkins(strSkinPath)
                strGallery += ProcessContainers(strSkinPath.Replace("\" & SkinInfo.RootSkin.ToLower & "\", "\" & SkinInfo.RootContainer.ToLower & "\"))
                pnlSkin.Visible = True
                If UserInfo.IsSuperUser Or strSkinPath.IndexOf(Common.Globals.HostMapPath.ToLower) = -1 Then
                    cmdParse.Visible = True
                    pnlParse.Visible = True
                    cmdDelete.Visible = (Not isFallbackSkin(strSkinPath)) And SkinController.CanDeleteSkin(strSkinPath, PortalSettings.HomeDirectoryMapPath)

                Else
                    cmdParse.Visible = False
                    pnlParse.Visible = False
                    cmdDelete.Visible = False
                End If
            Else
                pnlSkin.Visible = False
                pnlParse.Visible = False
            End If

            lblGallery.Text = strGallery

        End Sub

        Private Sub ShowContainers()

            Dim strContainerPath As String = ApplicationMapPath.ToLower + cboContainers.SelectedItem.Value
            cboSkins.ClearSelection()

            Dim strGallery As String = ""

            If cboContainers.SelectedIndex > 0 Then
                strGallery = ProcessContainers(strContainerPath)
                pnlSkin.Visible = True
                If UserInfo.IsSuperUser Or strContainerPath.IndexOf(Common.Globals.HostMapPath.ToLower) = -1 Then
                    cmdParse.Visible = True
                    pnlParse.Visible = True
                    cmdDelete.Visible = (Not isFallbackSkin(strContainerPath)) And SkinController.CanDeleteSkin(strContainerPath, PortalSettings.HomeDirectoryMapPath)
                Else
                    cmdParse.Visible = False
                    pnlParse.Visible = False
                    cmdDelete.Visible = False
                End If
            Else
                pnlSkin.Visible = False
                pnlParse.Visible = False
            End If

            lblGallery.Text = strGallery

        End Sub

        Private Sub LoadSkins()

            Dim strRoot As String
            Dim strFolder As String
            Dim arrFolders As String()
            Dim strName As String
            Dim strSkin As String

            cboSkins.Items.Clear()
            cboSkins.Items.Add("<" & Services.Localization.Localization.GetString("Not_Specified") & ">")

            ' load host skins
            If chkHost.Checked Then
                strRoot = Request.MapPath(Common.Globals.HostPath & SkinInfo.RootSkin)
                If Directory.Exists(strRoot) Then
                    arrFolders = Directory.GetDirectories(strRoot)
                    For Each strFolder In arrFolders
                        strName = Mid(strFolder, InStrRev(strFolder, "\") + 1)
                        strSkin = strFolder.Replace(ApplicationMapPath, "")
                        If strName <> "_default" Then
                            cboSkins.Items.Add(New ListItem(strName, strSkin.ToLower))
                        End If
                    Next
                End If
            End If

            ' load portal skins
            If chkSite.Checked Then
                strRoot = PortalSettings.HomeDirectoryMapPath & SkinInfo.RootSkin
                If Directory.Exists(strRoot) Then
                    arrFolders = Directory.GetDirectories(strRoot)
                    For Each strFolder In arrFolders
                        strName = Mid(strFolder, InStrRev(strFolder, "\") + 1)
                        strSkin = strFolder.Replace(ApplicationMapPath, "")
                        cboSkins.Items.Add(New ListItem(strName, strSkin.ToLower))
                    Next
                End If
            End If

            If Not Page.IsPostBack Then
                Dim strURL As String
                If Not Request.QueryString("Name") Is Nothing Then
                    strURL = Request.MapPath(GetSkinPath(Convert.ToString(Request.QueryString("Type")), SkinInfo.RootSkin, Convert.ToString(Request.QueryString("Name"))))
                Else
                    'Get the current portal skin
                    Dim objSkins As New UI.Skins.SkinController
                    Dim objSkin As UI.Skins.SkinInfo
                    Dim skinSrc As String
                    objSkin = SkinController.GetSkin(SkinInfo.RootSkin, PortalSettings.PortalId, SkinType.Portal)
                    If Not objSkin Is Nothing Then
                        skinSrc = objSkin.SkinSrc
                    Else
                        skinSrc = "[G]" & SkinInfo.RootSkin & glbDefaultSkinFolder & glbDefaultSkin
                    End If
                    strURL = Request.MapPath(SkinController.FormatSkinPath(SkinController.FormatSkinSrc(skinSrc, PortalSettings)))
                    strURL = strURL.Substring(0, strURL.LastIndexOf("\"))
                End If
                strSkin = strURL.Replace(ApplicationMapPath, "")
                If Not cboSkins.Items.FindByValue(strSkin.ToLower) Is Nothing Then
                    cboSkins.Items.FindByValue(strSkin.ToLower).Selected = True
                    ShowSkins()
                End If
            End If

        End Sub

        Private Sub LoadContainers()

            Dim strRoot As String
            Dim strFolder As String
            Dim arrFolders As String()
            Dim strName As String
            Dim strSkin As String

            cboContainers.Items.Clear()
            cboContainers.Items.Add("<" & Services.Localization.Localization.GetString("Not_Specified") & ">")

            ' load host containers
            If chkHost.Checked Then
                strRoot = Request.MapPath(Common.Globals.HostPath & SkinInfo.RootContainer)
                If Directory.Exists(strRoot) Then
                    arrFolders = Directory.GetDirectories(strRoot)
                    For Each strFolder In arrFolders
                        strName = Mid(strFolder, InStrRev(strFolder, "\") + 1)
                        strSkin = strFolder.Replace(ApplicationMapPath, "")
                        If strName <> "_default" Then
                            cboContainers.Items.Add(New ListItem(strName, strSkin.ToLower))
                        End If
                    Next
                End If
            End If

            ' load portal containers
            If chkSite.Checked Then
                strRoot = PortalSettings.HomeDirectoryMapPath & SkinInfo.RootContainer
                If Directory.Exists(strRoot) Then
                    arrFolders = Directory.GetDirectories(strRoot)
                    For Each strFolder In arrFolders
                        strName = Mid(strFolder, InStrRev(strFolder, "\") + 1)
                        strSkin = strFolder.Replace(ApplicationMapPath, "")
                        cboContainers.Items.Add(New ListItem(strName, strSkin.ToLower))
                    Next
                End If
            End If

            If Not Page.IsPostBack Then
                Dim strURL As String
                If Not Request.QueryString("Name") Is Nothing Then
                    strURL = Request.MapPath(GetSkinPath(Convert.ToString(Request.QueryString("Type")), Convert.ToString(Request.QueryString("Root")), Convert.ToString(Request.QueryString("Name"))))
                    strSkin = strURL.Replace(ApplicationMapPath, "")
                    If Not cboContainers.Items.FindByValue(strSkin.ToLower) Is Nothing Then
                        cboContainers.Items.FindByValue(strSkin.ToLower).Selected = True
                        ShowContainers()
                    End If
                End If
            End If

        End Sub

        Private Function ProcessSkins(ByVal strFolderPath As String) As String

            Dim strFile As String
            Dim strFolder As String
            Dim arrFiles As String()
            Dim strGallery As String = ""
            Dim strSkinType As String = ""
            Dim strURL As String
            Dim intIndex As Integer = 0
            Dim fallbackSkin As Boolean = isFallbackSkin(strFolderPath)

            If Directory.Exists(strFolderPath) Then
                If strFolderPath.ToLower.IndexOf(Common.Globals.HostMapPath.ToLower) <> -1 Then
                    strSkinType = "G"
                Else
                    strSkinType = "L"
                End If

                Dim canDeleteSkin As Boolean = SkinController.CanDeleteSkin(strFolderPath, PortalSettings.HomeDirectoryMapPath)

                strGallery = "<table border=""1"" cellspacing=""0"" cellpadding=""2"" width=""100%"">"
                strGallery += "<tr><td align=""center"" bgcolor=""#CCCCCC"" class=""Head"">" & Services.Localization.Localization.GetString("plSkins.Text", Me.LocalResourceFile) & "</td></tr>"
                strGallery += "<tr><td align=""center"">"
                strGallery += "<table border=""0"" cellspacing=""4"" cellpadding=""4""><tr>"
                If fallbackSkin Or Not canDeleteSkin Then
                    strGallery += "<td colspan=""3"" class=""NormalRed"">" & Services.Localization.Localization.GetString("CannotDeleteSkin.ErrorMessage", Me.LocalResourceFile) & "</td></tr><tr>"
                End If
                arrFiles = Directory.GetFiles(strFolderPath, "*.ascx")
                If arrFiles.Length = 0 Then
                    strGallery += "<td align=""center"" valign=""bottom"" class=""NormalBold"">" & Services.Localization.Localization.GetString("NoSkin.ErrorMessage", Me.LocalResourceFile) & "</td>"
                End If

                strFolder = Mid(strFolderPath, InStrRev(strFolderPath, "\") + 1)
                For Each strFile In arrFiles
                    intIndex += 1
                    If intIndex = 4 Then
                        strGallery += "</tr><tr>"
                        intIndex = 1
                    End If

                    ' name
                    strFile = strFile.ToLower
                    strGallery += "<td align=""center"" valign=""bottom"" class=""NormalBold"">"
                    strGallery += Path.GetFileNameWithoutExtension(strFile) & "<br>"
                    ' thumbnail
                    If File.Exists(strFile.Replace(".ascx", ".jpg")) Then
                        strURL = strFile.Substring(strFile.LastIndexOf("\portals\"))
                        strGallery += "<a href=""" & ResolveUrl("~" + strURL.Replace(".ascx", ".jpg")) & """ target=""_new""><img src=""" & CreateThumbnail(strFile.Replace(".ascx", ".jpg")) & """ border=""1""></a>"
                    Else
                        strGallery += "<img src=""" & ResolveUrl("~/images/thumbnail.jpg") & """ border=""1"">"
                    End If
                    ' options 
                    strURL = strFile.Substring(strFile.IndexOf("\" & SkinInfo.RootSkin.ToLower & "\"))
                    strURL.Replace(".ascx", "")
                    strGallery += "<br><a class=""CommandButton"" href=""" & NavigateURL(PortalSettings.HomeTabId) & "?SkinSrc=[" & strSkinType & "]" & QueryStringEncode(strURL.Replace(".ascx", "").Replace("\", "/")) & """ target=""_new"">" & Services.Localization.Localization.GetString("cmdPreview", Me.LocalResourceFile) & "</a>"
                    strGallery += "&nbsp;&nbsp;|&nbsp;&nbsp;"
                    strGallery += "<a class=""CommandButton"" href=""" & Common.Globals.ApplicationPath & Common.Globals.ApplicationURL.Replace("~", "") & "&Root=" & SkinInfo.RootSkin & "&Type=" & strSkinType & "&Name=" & strFolder & "&Src=" & Path.GetFileName(strFile) & "&action=apply"">" & Services.Localization.Localization.GetString("cmdApply", Me.LocalResourceFile) & "</a>"
                    If (UserInfo.IsSuperUser = True Or strSkinType = "L") AndAlso (Not fallbackSkin And canDeleteSkin) Then
                        strGallery += "&nbsp;&nbsp;|&nbsp;&nbsp;"
                        strGallery += "<a class=""CommandButton"" href=""" & Common.Globals.ApplicationPath & Common.Globals.ApplicationURL.Replace("~", "") & "&Root=" & SkinInfo.RootSkin & "&Type=" & strSkinType & "&Name=" & strFolder & "&Src=" & Path.GetFileName(strFile) & "&action=delete"">" & Services.Localization.Localization.GetString("cmdDelete") & "</a>"
                    End If
                    strGallery += "</td>"
                Next

                strGallery += "</tr></table></td></tr>"
                If File.Exists(strFolderPath & "/" & glbAboutPage) Then
                    strGallery += AddCopyright(strFolderPath & "/" & glbAboutPage, strFolder)
                End If
                strGallery += "</table><br>"
            End If

            Return strGallery

        End Function

        Private Function ProcessContainers(ByVal strFolderPath As String) As String

            Dim strFile As String
            Dim strFolder As String
            Dim arrFiles As String()
            Dim strGallery As String = ""
            Dim strContainerType As String = ""
            Dim strURL As String
            Dim intIndex As Integer = 0
            Dim fallbackSkin As Boolean = isFallbackContainer(strFolderPath)

            If Directory.Exists(strFolderPath) Then
                If Not cboContainers.Items.FindByValue(strFolderPath.Replace(ApplicationMapPath.ToLower, "")) Is Nothing Then
                    cboContainers.Items.FindByValue(strFolderPath.Replace(ApplicationMapPath.ToLower, "")).Selected = True
                End If

                If strFolderPath.ToLower.IndexOf(Common.Globals.HostMapPath.ToLower) <> -1 Then
                    strContainerType = "G"
                Else
                    strContainerType = "L"
                End If

                Dim canDeleteSkin As Boolean = SkinController.CanDeleteSkin(strFolderPath, PortalSettings.HomeDirectoryMapPath)

                strGallery = "<table border=""1"" cellspacing=""0"" cellpadding=""2"" width=""100%"">"
                strGallery += "<tr><td align=""center"" bgcolor=""#CCCCCC"" class=""Head"">" & Services.Localization.Localization.GetString("plContainers.Text", Me.LocalResourceFile) & "</td></tr>"
                strGallery += "<tr><td align=""center"">"
                strGallery += "<table border=""0"" cellspacing=""4"" cellpadding=""4""><tr>"
                If fallbackSkin Or Not canDeleteSkin Then
                    strGallery += "<td colspan=""3"" class=""NormalRed"">" & Services.Localization.Localization.GetString("CannotDeleteContainer.ErrorMessage", Me.LocalResourceFile) & "</td></tr><tr>"
                End If

                arrFiles = Directory.GetFiles(strFolderPath, "*.ascx")
                If arrFiles.Length = 0 Then
                    strGallery += "<td align=""center"" valign=""bottom"" class=""NormalBold"">" & Services.Localization.Localization.GetString("NoContainer.ErrorMessage", Me.LocalResourceFile) & "</td>"
                End If
                strFolder = Mid(strFolderPath, InStrRev(strFolderPath, "\") + 1)
                For Each strFile In arrFiles
                    intIndex += 1
                    If intIndex = 4 Then
                        strGallery += "</tr><tr>"
                        intIndex = 1
                    End If

                    ' name
                    strFile = strFile.ToLower
                    strGallery += "<td align=""center"" valign=""bottom"" class=""NormalBold"">"
                    strGallery += Path.GetFileNameWithoutExtension(strFile) & "<br>"
                    ' thumbnail
                    If File.Exists(strFile.Replace(".ascx", ".jpg")) Then
                        strURL = strFile.Substring(strFile.LastIndexOf("\portals\"))
                        strGallery += "<a href=""" & ResolveUrl("~" + strURL.Replace(".ascx", ".jpg")) & """ target=""_new""><img src=""" & CreateThumbnail(strFile.Replace(".ascx", ".jpg")) & """ border=""1""></a>"
                    Else
                        strGallery += "<img src=""" & ResolveUrl("~/images/thumbnail.jpg") & """ border=""1"">"
                    End If
                    ' options 
                    strURL = strFile.Substring(strFile.IndexOf("\" & SkinInfo.RootContainer.ToLower & "\"))
                    strURL.Replace(".ascx", "")
                    strGallery += "<br><a class=""CommandButton"" href=""" & NavigateURL(PortalSettings.HomeTabId) & "?ContainerSrc=[" & strContainerType & "]" & QueryStringEncode(strURL.Replace(".ascx", "").Replace("\", "/")) & """ target=""_new"">" & Services.Localization.Localization.GetString("cmdPreview", Me.LocalResourceFile) & "</a>"
                    strGallery += "&nbsp;&nbsp;|&nbsp;&nbsp;"
                    strGallery += "<a class=""CommandButton"" href=""" & Common.Globals.ApplicationPath & Common.Globals.ApplicationURL.Replace("~", "") & "&Root=" & SkinInfo.RootContainer & "&Type=" & strContainerType & "&Name=" & strFolder & "&Src=" & Path.GetFileName(strFile) & "&action=apply"">" & Services.Localization.Localization.GetString("cmdApply", Me.LocalResourceFile) & "</a>"
                    If (UserInfo.IsSuperUser = True Or strContainerType = "L") AndAlso (Not fallbackSkin And canDeleteSkin) Then
                        strGallery += "&nbsp;&nbsp;|&nbsp;&nbsp;"
                        strGallery += "<a class=""CommandButton"" href=""" & Common.Globals.ApplicationPath & Common.Globals.ApplicationURL.Replace("~", "") & "&Root=" & SkinInfo.RootContainer & "&Type=" & strContainerType & "&Name=" & strFolder & "&Src=" & Path.GetFileName(strFile) & "&action=delete"">" & Services.Localization.Localization.GetString("cmdDelete") & "</a>"
                    End If
                    strGallery += "</td>"
                Next

                strGallery += "</tr></table></td></tr>"
                If File.Exists(strFolderPath & "/" & glbAboutPage) Then
                    strGallery += AddCopyright(strFolderPath & "/" & glbAboutPage, strFolder)
                End If
                strGallery += "</table><br>"
            End If

            Return strGallery

        End Function

        Private Function AddCopyright(ByVal strFile As String, ByVal Skin As String) As String

            Dim strGallery As String = ""
            Dim strURL As String

            strGallery += "<tr><td align=""center"" bgcolor=""#CCCCCC"">"
            strURL = strFile.Substring(strFile.IndexOf("\portals\"))
            strGallery += "<a class=""CommandButton"" href=""" & ResolveUrl("~" + strURL) & """ target=""_new"">" & String.Format(Localization.GetString("About", Me.LocalResourceFile), Skin) & "</a>"
            strGallery += "</td></tr>"

            Return strGallery

        End Function

        Private Function CreateThumbnail(ByVal strImage As String) As String

            Dim blnCreate As Boolean = True

            Dim strThumbnail As String = strImage.Replace(Path.GetFileName(strImage), "thumbnail_" & Path.GetFileName(strImage))

            ' check if image has changed
            If File.Exists(strThumbnail) Then
                Dim d1 As Date = File.GetLastWriteTime(strThumbnail)
                Dim d2 As Date = File.GetLastWriteTime(strImage)
                If File.GetLastWriteTime(strThumbnail) = File.GetLastWriteTime(strImage) Then
                    blnCreate = False
                End If
            End If

            If blnCreate Then

                Dim dblScale As Double
                Dim intHeight As Integer
                Dim intWidth As Integer

                Dim intSize As Integer = 150    ' size of the thumbnail 

                Dim objImage As System.Drawing.Image
                Try
                    objImage = System.Drawing.Image.FromFile(strImage)

                    ' scale the image to prevent distortion
                    If objImage.Height > objImage.Width Then
                        'The height was larger, so scale the width 
                        dblScale = intSize / objImage.Height
                        intHeight = intSize
                        intWidth = CInt(objImage.Width * dblScale)
                    Else
                        'The width was larger, so scale the height 
                        dblScale = intSize / objImage.Width
                        intWidth = intSize
                        intHeight = CInt(objImage.Height * dblScale)
                    End If

                    ' create the thumbnail image
                    Dim objThumbnail As System.Drawing.Image
                    objThumbnail = objImage.GetThumbnailImage(intWidth, intHeight, Nothing, IntPtr.Zero)

                    ' delete the old file ( if it exists )
                    If File.Exists(strThumbnail) Then
                        File.Delete(strThumbnail)
                    End If

                    ' save the thumbnail image 
                    objThumbnail.Save(strThumbnail, objImage.RawFormat)

                    ' set the file attributes
                    File.SetAttributes(strThumbnail, FileAttributes.Normal)
                    File.SetLastWriteTime(strThumbnail, File.GetLastWriteTime(strImage))

                    ' tidy up
                    objImage.Dispose()
                    objThumbnail.Dispose()

                Catch

                    ' problem creating thumbnail

                End Try

            End If

            strThumbnail = Common.Globals.ApplicationPath & "/" & strThumbnail.Substring(strThumbnail.IndexOf("portals\"))
            strThumbnail = strThumbnail.Replace("\", "/")

            ' return thumbnail filename
            Return strThumbnail

        End Function

        Private Function ParseSkinPackage(ByVal strType As String, ByVal strRoot As String, ByVal strName As String, ByVal strFolder As String, ByVal strParse As String) As String

            Dim strRootPath As String = Null.NullString
            Select Case strType
                Case "G"    ' global
                    strRootPath = Request.MapPath(Common.Globals.HostPath)
                Case "L"    ' local
                    strRootPath = Request.MapPath(PortalSettings.HomeDirectory)
            End Select

            Dim objSkinFiles As New UI.Skins.SkinFileProcessor(strRootPath, strRoot, strName)
            Dim arrSkinFiles As New ArrayList

            Dim strFile As String
            Dim arrFiles As String()

            If Directory.Exists(strFolder) Then
                arrFiles = Directory.GetFiles(strFolder)
                For Each strFile In arrFiles
                    Select Case Path.GetExtension(strFile)
                        Case ".htm", ".html", ".css"
                            If strFile.ToLower.IndexOf(glbAboutPage.ToLower) < 0 Then
                                arrSkinFiles.Add(strFile)
                            End If
                        Case ".ascx"
                            If File.Exists(strFile.Replace(".ascx", ".htm")) = False And File.Exists(strFile.Replace(".ascx", ".html")) = False Then
                                arrSkinFiles.Add(strFile)
                            End If
                    End Select
                Next
            End If

            Select Case strParse
                Case "L"    ' localized
                    Return objSkinFiles.ProcessList(arrSkinFiles, SkinParser.Localized)
                Case "P"    ' portable
                    Return objSkinFiles.ProcessList(arrSkinFiles, SkinParser.Portable)
            End Select

            Return Null.NullString

        End Function

        Private Function GetSkinPath(ByVal Type As String, ByVal Root As String, ByVal Name As String) As String

            Dim strPath As String = Null.NullString

            Select Case Type
                Case "G"    ' global
                    strPath = Common.Globals.HostPath & Root & "/" & Name
                Case "L"    ' local
                    strPath = PortalSettings.HomeDirectory & Root & "/" & Name
            End Select

            Return strPath

        End Function

#End Region

#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                If Page.IsPostBack = False Then
                    DotNetNuke.UI.Utilities.ClientAPI.AddButtonConfirm(cmdDelete, Services.Localization.Localization.GetString("DeleteItem"))

                    LoadSkins()
                    LoadContainers()

                    If Not Request.QueryString("action") Is Nothing Then

                        Dim strType As String = Request.QueryString("Type")
                        Dim strRoot As String = Request.QueryString("Root")
                        Dim strName As String = Request.QueryString("Name")
                        Dim strSrc As String = "[" & strType & "]" & strRoot & "/" & strName & "/" & Request.QueryString("Src")

                        Select Case Request.QueryString("action")
                            Case "apply"
                                If strRoot = SkinInfo.RootSkin Then
                                    If chkPortal.Checked Then
                                        SkinController.SetSkin(SkinInfo.RootSkin, PortalId, SkinType.Portal, strSrc)
                                    End If
                                    If chkAdmin.Checked Then
                                        SkinController.SetSkin(SkinInfo.RootSkin, PortalId, SkinType.Admin, strSrc)
                                    End If
                                End If
                                If strRoot = SkinInfo.RootContainer Then
                                    If chkPortal.Checked Then
                                        SkinController.SetSkin(SkinInfo.RootContainer, PortalId, SkinType.Portal, strSrc)
                                    End If
                                    If chkAdmin.Checked Then
                                        SkinController.SetSkin(SkinInfo.RootContainer, PortalId, SkinType.Admin, strSrc)
                                    End If
                                End If
                                DataCache.ClearPortalCache(PortalId, True)
                                Response.Redirect(Request.RawUrl.Replace("&action=apply", ""))
                            Case "delete"
                                File.Delete(Request.MapPath(SkinController.FormatSkinSrc(strSrc, PortalSettings)))
                                LoadSkins()
                                LoadContainers()
                        End Select
                    End If
                End If

                If PortalSettings.ActiveTab.IsSuperTab Then
                    typeRow.Visible = False
                Else
                    typeRow.Visible = True
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cboSkins_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSkins.SelectedIndexChanged

            ShowSkins()

        End Sub

        Private Sub cboContainers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboContainers.SelectedIndexChanged

            ShowContainers()

        End Sub

        Private Sub chkHost_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkHost.CheckedChanged

            LoadSkins()
            LoadContainers()

            ShowSkins()
            ShowContainers()

        End Sub

        Private Sub chkSite_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSite.CheckedChanged

            LoadSkins()
            LoadContainers()

            ShowSkins()
            ShowContainers()

        End Sub

        Private Sub cmdRestore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRestore.Click

            Dim objSkins As New UI.Skins.SkinController
            If chkPortal.Checked Then
                SkinController.SetSkin(SkinInfo.RootSkin, PortalId, SkinType.Portal, "")
                SkinController.SetSkin(SkinInfo.RootContainer, PortalId, SkinType.Portal, "")
            End If
            If chkAdmin.Checked Then
                SkinController.SetSkin(SkinInfo.RootSkin, PortalId, SkinType.Admin, "")
                SkinController.SetSkin(SkinInfo.RootContainer, PortalId, SkinType.Admin, "")
            End If
            DataCache.ClearPortalCache(PortalId, True)
            Response.Redirect(Request.RawUrl)

        End Sub

        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            Dim failure As Boolean = False
            Dim strSkinPath As String = ApplicationMapPath.ToLower + cboSkins.SelectedItem.Value
            Dim strContainerPath As String = ApplicationMapPath.ToLower + cboContainers.SelectedItem.Value

            Dim strMessage As String

            If UserInfo.IsSuperUser = False And cboSkins.SelectedItem.Value.IndexOf("\portals\_default\", 0) <> -1 Then
                strMessage = Services.Localization.Localization.GetString("SkinDeleteFailure", Me.LocalResourceFile)
                UI.Skins.Skin.AddModuleMessage(Me, strMessage, UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                failure = True
            Else
                If cboSkins.SelectedIndex > 0 Then
                    If Directory.Exists(strSkinPath) Then
                        Common.Globals.DeleteFolderRecursive(strSkinPath)
                    End If
                    If Directory.Exists(strSkinPath.Replace("\" & SkinInfo.RootSkin.ToLower & "\", "\" & SkinInfo.RootContainer & "\")) Then
                        Common.Globals.DeleteFolderRecursive(strSkinPath.Replace("\" & SkinInfo.RootSkin.ToLower & "\", "\" & SkinInfo.RootContainer & "\"))
                    End If
                End If

                If cboContainers.SelectedIndex > 0 Then
                    If Directory.Exists(strContainerPath) Then
                        Common.Globals.DeleteFolderRecursive(strContainerPath)
                    End If
                End If
            End If

            If Not failure Then
                LoadSkins()
                LoadContainers()

                ShowSkins()
                ShowContainers()
            End If
        End Sub

        Private Sub cmdParse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdParse.Click

            Dim strFolder As String
            Dim strType As String
            Dim strRoot As String
            Dim strName As String
            Dim strSkinPath As String = ApplicationMapPath.ToLower + cboSkins.SelectedItem.Value
            Dim strContainerPath As String = ApplicationMapPath.ToLower + cboContainers.SelectedItem.Value
            Dim strParse As String = ""

            If cboSkins.SelectedIndex > 0 Then
                strFolder = strSkinPath
                If strFolder.IndexOf(Common.Globals.HostMapPath.ToLower) <> -1 Then
                    strType = "G"
                Else
                    strType = "L"
                End If
                strRoot = SkinInfo.RootSkin
                strName = cboSkins.SelectedItem.Text
                strParse += ParseSkinPackage(strType, strRoot, strName, strFolder, optParse.SelectedItem.Value)

                strFolder = strSkinPath.Replace("\" & SkinInfo.RootSkin.ToLower & "\", "\" & SkinInfo.RootContainer.ToLower & "\")
                strRoot = SkinInfo.RootContainer
                strParse += ParseSkinPackage(strType, strRoot, strName, strFolder, optParse.SelectedItem.Value)
                DataCache.ClearPortalCache(PortalId, True)
            End If

            If cboContainers.SelectedIndex > 0 Then
                strFolder = strContainerPath
                If strFolder.IndexOf(Common.Globals.HostMapPath.ToLower) <> -1 Then
                    strType = "G"
                Else
                    strType = "L"
                End If
                strRoot = SkinInfo.RootContainer
                strName = cboContainers.SelectedItem.Text
                strParse += ParseSkinPackage(strType, strRoot, strName, strFolder, optParse.SelectedItem.Value)
                DataCache.ClearPortalCache(PortalId, True)
            End If

            lblOutput.Text = strParse

            If cboSkins.SelectedIndex > 0 Then
                ShowSkins()
            Else
                If cboContainers.SelectedIndex > 0 Then
                    ShowContainers()
                End If
            End If

        End Sub

#End Region

#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New ModuleActionCollection
                If Convert.ToString(PortalSettings.HostSettings("SkinUpload")) <> "G" Or UserInfo.IsSuperUser Then
                    Dim intPortalId As Integer
                    If PortalSettings.ActiveTab.IsSuperTab Then
                        intPortalId = Null.NullInteger
                    Else
                        intPortalId = PortalId
                    End If
                    Dim FileManagerModule As ModuleInfo = (New ModuleController).GetModuleByDefinition(intPortalId, "File Manager")
                    Dim params(2) As String

                    params(0) = "mid=" & FileManagerModule.ModuleID
                    params(1) = "ftype=" & UploadType.Skin.ToString
                    params(2) = "rtab=" & Me.TabId
                    Actions.Add(GetNextActionID, Services.Localization.Localization.GetString("SkinUpload.Action", LocalResourceFile), ModuleActionType.AddContent, "", "", NavigateURL(FileManagerModule.TabID, "Edit", params), False, SecurityAccessLevel.Admin, True, False)

                    params(1) = "ftype=" & UploadType.Container.ToString
                    Actions.Add(GetNextActionID, Services.Localization.Localization.GetString("ContainerUpload.Action", LocalResourceFile), ModuleActionType.AddContent, "", "", NavigateURL(FileManagerModule.TabID, "Edit", params), False, SecurityAccessLevel.Admin, True, False)
                End If
                Return Actions
            End Get
        End Property
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