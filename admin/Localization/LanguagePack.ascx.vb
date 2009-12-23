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
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.Checksums
Imports ICSharpCode.SharpZipLib.GZip
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Modules.Admin.ResourceInstaller

Namespace DotNetNuke.Services.Localization
    Partial Class LanguagePack
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not Page.IsPostBack Then
                Dim locales As LocaleCollection = Localization.GetSupportedLocales()
                cboLanguage.DataSource = New LocaleCollectionWrapper(locales)
                cboLanguage.DataTextField = "Text"
                cboLanguage.DataValueField = "Code"
                cboLanguage.DataBind()
                cboLanguage.Items.Insert(0, New ListItem("English", Localization.SystemLocale))

                rowitems.Visible = False

                lbItems.Attributes.Add("onchange", "changeItem('" + lbItems.ClientID + "','" + txtFileName.ClientID + "')")
            End If
        End Sub

        Private Sub rbPackType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbPackType.SelectedIndexChanged
            pnlLogs.Visible = False
            Select Case [Enum].Parse(GetType(LanguagePackType), rbPackType.SelectedValue)
                Case LanguagePackType.Core
                    rowitems.Visible = False
                    txtFileName.Text = "Core"
                    lblFilenameFix.Text = Server.HtmlEncode(".<version>.<locale>.zip")

                Case LanguagePackType.Module
                    rowitems.Visible = True
                    lbItems.Items.Clear()
                    lbItems.ClearSelection()
                    lbItems.SelectionMode = ListSelectionMode.Multiple
                    txtFileName.Text = "Module.version"
                    lblFilenameFix.Text = Server.HtmlEncode(".<locale>.zip")

                    Dim objDesktopModules As New DesktopModuleController
                    For Each objDM As DesktopModuleInfo In objDesktopModules.GetDesktopModules()
                        If Null.IsNull(objDM.Version) Then
                            lbItems.Items.Add(New ListItem(objDM.FriendlyName, objDM.FolderName))
                        Else
                            lbItems.Items.Add(New ListItem(objDM.FriendlyName + " [" + objDM.Version + "]", objDM.FolderName))
                        End If
                    Next
                    lblItems.Text = Localization.GetString("SelectModules", LocalResourceFile)

                Case LanguagePackType.Provider
                    rowitems.Visible = True
                    lbItems.Items.Clear()
                    lbItems.ClearSelection()
                    lbItems.SelectionMode = ListSelectionMode.Multiple
                    txtFileName.Text = "Provider.version"
                    lblFilenameFix.Text = Server.HtmlEncode(".<locale>.zip")

                    Dim objFolder As New DirectoryInfo(Server.MapPath("~/Providers/HtmlEditorProviders"))
                    For Each folder As DirectoryInfo In objFolder.GetDirectories()
                        lbItems.Items.Add(New ListItem(folder.Name, folder.Name))
                    Next

                    lblItems.Text = Localization.GetString("SelectProviders", LocalResourceFile)

                Case LanguagePackType.Full
                    rowitems.Visible = False
                    txtFileName.Text = "Full"
                    lblFilenameFix.Text = Server.HtmlEncode(".<version>.<locale>.zip")
            End Select
        End Sub

        Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
            Dim LangPackWriter As New LocaleFilePackWriter
            Dim LocaleCulture As New Locale
            LocaleCulture.Code = cboLanguage.SelectedValue
            LocaleCulture.Text = cboLanguage.SelectedItem.Text

            Dim packtype As LanguagePackType = CType([Enum].Parse(GetType(LanguagePackType), rbPackType.SelectedValue), LanguagePackType)
            Dim basefolders As New ArrayList
            If packtype = LanguagePackType.Module Or packtype = LanguagePackType.Provider Then
                For Each l As ListItem In lbItems.Items
                    If l.Selected Then
                        basefolders.Add(l.Value)
                    End If
                Next
            End If
            'verify filename
            txtFileName.Text = DotNetNuke.Common.Globals.CleanFileName(txtFileName.Text)

            Dim LangPackName As String = LangPackWriter.SaveLanguagePack(LocaleCulture, packtype, basefolders, txtFileName.Text)

            If LangPackWriter.ProgressLog.Valid Then
                lblMessage.Text = String.Format(Localization.GetString("LOG.MESSAGE.Success", LocalResourceFile), LangPackName)
                lblMessage.CssClass = "Head"
                hypLink.Text = String.Format(Localization.GetString("Download", LocalResourceFile), Path.GetFileName(LangPackName))
                hypLink.NavigateUrl = DotNetNuke.Common.Globals.HostPath & Path.GetFileName(LangPackName)
                hypLink.Visible = True
            Else
                lblMessage.Text = Localization.GetString("LOG.MESSAGE.Error", LocalResourceFile)
                lblMessage.CssClass = "NormalRed"
                hypLink.Visible = False
            End If
            divLog.Controls.Add(LangPackWriter.ProgressLog.GetLogsTable)
            pnlLogs.Visible = True
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL())
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
#End Region

    End Class
End Namespace