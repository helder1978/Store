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
Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Services.Localization

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Manages the suported locales file
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[vmasanas]	10/04/2004  Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Languages
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable


#Region "Controls"
#End Region

#Region "Private Members"
        Dim xmlLocales As New XmlDocument
        Dim bXmlLoaded As Boolean = False
#End Region

#Region "Event Handlers"
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Loads defined locales
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If File.Exists(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml")) Then
                    Try
                        xmlLocales.Load(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml"))
                        bXmlLoaded = True
                    Catch
                    End Try
                End If

                If Not Page.IsPostBack Then
                    'only host can add and delete, this should also only be visible on the host menu
                    If UserInfo.IsSuperUser And (PortalSettings.ActiveTab.ParentId <> PortalSettings.AdminTabId) Then
                        chkEnableBrowser.Text = Localization.GetString("EnableBrowserHost", LocalResourceFile)
                        chkEnableLanguageInUrl.Text = Localization.GetString("chkEnableLanguageInUrlHost", LocalResourceFile)
                    Else
                        chkEnableBrowser.Text = Localization.GetString("EnableBrowserPortal", LocalResourceFile)
                        chkEnableLanguageInUrl.Text = Localization.GetString("chkEnableLanguageInUrlPortal", LocalResourceFile)
                        pnlAdd.Visible = False
                        chkDeleteFiles.Visible = False
                    End If
                    'Localize Grid
                    Localization.LocalizeDataGrid(dgLocales, Me.LocalResourceFile)
                    BindGrid()

                    LoadLocales()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Adds a new locale to the locales xml file
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' Only one definition for a given locale key can be defined
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

            Try
                Select Case (New Localization).AddLocale(cboLocales.SelectedValue, cboLocales.SelectedItem.Text)
                    Case "Duplicate.ErrorMessage"
                        UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Duplicate.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    Case "NewLocale.ErrorMessage"
                        UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("NewLocale.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess)
                        BindGrid()
                    Case "Save.ErrorMessage"
                        UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    Case ""
                        BindGrid()
                End Select

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub dgLocales_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgLocales.ItemCreated
            Try
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    Dim ctldelete As LinkButton = CType(e.Item.FindControl("cmdDelete"), LinkButton)
                    Dim ctldisable As LinkButton = CType(e.Item.FindControl("cmdDisable"), LinkButton)

                    If PortalSettings.ActiveTab.ParentId = PortalSettings.AdminTabId Then
                        ' we are on the Admin menu, hide delete button, since this is only for host
                        dgLocales.Columns(4).Visible = False
                    Else
                        ' we are on the host menu, hide enable button and status, since this is only for admins
                        dgLocales.Columns(2).Visible = False
                        dgLocales.Columns(3).Visible = False
                    End If
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub dgLocales_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgLocales.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    Dim ctl As LinkButton
                    ctl = CType(e.Item.FindControl("cmdDelete"), LinkButton)
                    ClientAPI.AddButtonConfirm(ctl, Services.Localization.Localization.GetString("DeleteItem"))

                    Dim lbl As Label
                    Dim ctlStatus As LinkButton
                    lbl = CType(e.Item.FindControl("lblStatus"), Label)
                    ctlStatus = CType(e.Item.FindControl("cmdDisable"), LinkButton)

                    If Not bXmlLoaded OrElse xmlLocales.SelectSingleNode("//locales/inactive/locale[.='" + CType(dgLocales.DataKeys(e.Item.ItemIndex), String) + "']") Is Nothing Then
                        lbl.Text = Localization.GetString("Enabled", LocalResourceFile)
                        ctlStatus.Text = Localization.GetString("Disable", LocalResourceFile)
                    Else
                        lbl.Text = Localization.GetString("Disabled", LocalResourceFile)
                        ctlStatus.Text = Localization.GetString("Enable", LocalResourceFile)
                    End If
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub dgLocales_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgLocales.ItemCommand

            Try
                Select Case e.CommandName
                    Case "Disable"
                        If Not bXmlLoaded Then
                            Try
                                ' First access to file, create using template
                                File.Copy(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal.xml.config"), Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml"))
                                xmlLocales.Load(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml"))
                                bXmlLoaded = True
                            Catch
                                UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                            End Try
                        End If

                        If bXmlLoaded Then
                            Dim inactive As XmlNode
                            Dim current As XmlNode
                            inactive = xmlLocales.SelectSingleNode("//locales/inactive")
                            current = inactive.SelectSingleNode("locale[.='" + CType(dgLocales.DataKeys(e.Item.ItemIndex), String) + "']")
                            If current Is Nothing Then 'disable
                                'can only disable if not last one enabled
                                Dim found As Integer
                                For Each l As DataGridItem In dgLocales.Items
                                    Dim lbl As Label
                                    lbl = CType(l.FindControl("lblStatus"), Label)
                                    If lbl.Text = Localization.GetString("Enabled", LocalResourceFile) Then
                                        found += 1
                                    End If
                                Next
                                If found > 1 Then
                                    ' current portal locale cannot be disabled
                                    If PortalSettings.DefaultLanguage <> CType(dgLocales.DataKeys(e.Item.ItemIndex), String) Then
                                        Dim newnode As XmlNode = xmlLocales.CreateElement("locale")
                                        newnode.InnerText = CType(dgLocales.DataKeys(e.Item.ItemIndex), String)
                                        inactive.AppendChild(newnode)
                                    Else
                                        UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("DisableCurrent.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                                        Exit Sub
                                    End If
                                Else
                                    UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Disable.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                                    Exit Sub
                                End If
                            Else ' enable
                                inactive.RemoveChild(current)
                            End If
                            xmlLocales.Save(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml"))
                            BindGrid()
                        End If
                    Case "Delete"
                        Dim key As String
                        Dim node As XmlNode
                        Dim resDoc As New XmlDocument

                        key = e.Item.Cells(1).Text
                        If key = Services.Localization.Localization.SystemLocale Then
                            UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Delete.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                        Else
                            'can only delete if not last one enabled
                            Dim found As Integer
                            For Each l As DataGridItem In dgLocales.Items
                                Dim lbl As Label
                                lbl = CType(l.FindControl("lblStatus"), Label)
                                If lbl.Text = Localization.GetString("Enabled", LocalResourceFile) Then
                                    found += 1
                                End If
                            Next
                            If found > 1 Then
                                ' current portal locale cannot be disabled
                                If PortalSettings.DefaultLanguage <> CType(dgLocales.DataKeys(e.Item.ItemIndex), String) Then
                                    resDoc.Load(Server.MapPath(Localization.SupportedLocalesFile))
                                    node = resDoc.SelectSingleNode("//root/language[@key='" + key + "']")
                                    node.ParentNode.RemoveChild(node)

                                    Try
                                        resDoc.Save(Server.MapPath(Localization.SupportedLocalesFile))
                                        BindGrid()
                                        ' check if files needs to be deleted.
                                        If chkDeleteFiles.Checked Then
                                            DeleteLocalizedFiles(key)
                                            chkDeleteFiles.Checked = False
                                        End If
                                    Catch
                                        UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                                    End Try
                                Else
                                    UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("DisableCurrent.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                                    Exit Sub
                                End If
                            Else
                                UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Disable.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                                Exit Sub
                            End If
                        End If
                End Select

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub rbDisplay_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDisplay.SelectedIndexChanged
            LoadLocales()
        End Sub


        Protected Sub chkEnableBrowser_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEnableBrowser.CheckedChanged
            Dim browserLanguage As XmlNode

            If PortalSettings.ActiveTab.ParentId = PortalSettings.AdminTabId Then
                If Not bXmlLoaded Then
                    Try
                        ' First access to file, create using template
                        File.Copy(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal.xml.config"), Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml"))
                        xmlLocales.Load(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml"))
                        bXmlLoaded = True
                    Catch
                        UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    End Try
                End If
                If bXmlLoaded Then
                    browserLanguage = xmlLocales.SelectSingleNode("//locales/browserDetection")
                    If browserLanguage Is Nothing Then
                        Dim attr As XmlNode = xmlLocales.CreateNode(XmlNodeType.Attribute, "enabled", "")

                        browserLanguage = xmlLocales.CreateElement("browserDetection")
                        browserLanguage.Attributes.Append(attr)
                        xmlLocales.SelectSingleNode("//locales").AppendChild(browserLanguage)
                    End If
                    browserLanguage.Attributes("enabled").Value = chkEnableBrowser.Checked.ToString().ToLower()
                    xmlLocales.Save(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml"))
                End If
            Else
                Dim xmldoc As New XmlDocument
                xmldoc.Load(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.xml"))
                browserLanguage = xmldoc.SelectSingleNode("//root/browserDetection")
                If browserLanguage Is Nothing Then
                    Dim attr As XmlNode = xmldoc.CreateNode(XmlNodeType.Attribute, "enabled", "")

                    browserLanguage = xmldoc.CreateElement("browserDetection")
                    browserLanguage.Attributes.Append(attr)
                    xmldoc.SelectSingleNode("//root").AppendChild(browserLanguage)
                End If
                browserLanguage.Attributes("enabled").Value = chkEnableBrowser.Checked.ToString().ToLower()
                xmldoc.Save(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.xml"))
            End If

        End Sub

        Protected Sub chkEnableLanguageInUrl_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEnableLanguageInUrl.CheckedChanged
            Dim languageInUrl As XmlNode

            If PortalSettings.ActiveTab.ParentId = PortalSettings.AdminTabId Then
                If Not bXmlLoaded Then
                    Try
                        ' First access to file, create using template
                        File.Copy(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal.xml.config"), Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml"))
                        xmlLocales.Load(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml"))
                        bXmlLoaded = True
                    Catch
                        UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    End Try
                End If
                If bXmlLoaded Then
                    languageInUrl = xmlLocales.SelectSingleNode("//locales/languageInUrl")
                    If languageInUrl Is Nothing Then
                        Dim attr As XmlNode = xmlLocales.CreateNode(XmlNodeType.Attribute, "enabled", "")

                        languageInUrl = xmlLocales.CreateElement("languageInUrl")
                        languageInUrl.Attributes.Append(attr)
                        xmlLocales.SelectSingleNode("//locales").AppendChild(languageInUrl)
                    End If
                    languageInUrl.Attributes("enabled").Value = chkEnableLanguageInUrl.Checked.ToString().ToLower()
                    xmlLocales.Save(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.Portal-" + PortalId.ToString + ".xml"))
                End If
            Else
                Dim xmldoc As New XmlDocument
                xmldoc.Load(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.xml"))
                languageInUrl = xmldoc.SelectSingleNode("//root/languageInUrl")
                If languageInUrl Is Nothing Then
                    Dim attr As XmlNode = xmldoc.CreateNode(XmlNodeType.Attribute, "enabled", "")

                    languageInUrl = xmldoc.CreateElement("languageInUrl")
                    languageInUrl.Attributes.Append(attr)
                    xmldoc.SelectSingleNode("//root").AppendChild(languageInUrl)
                End If
                languageInUrl.Attributes("enabled").Value = chkEnableLanguageInUrl.Checked.ToString().ToLower()
                xmldoc.Save(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.xml"))
            End If
        End Sub

#End Region

#Region "Private Methods"
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Reads XML file and binds to the datagrid
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindGrid()
            Dim ds As New DataSet
            Dim dv As DataView

            ds.ReadXml(Server.MapPath(Localization.SupportedLocalesFile))
            dv = ds.Tables(0).DefaultView
            dv.Sort = "name ASC"

            dgLocales.DataSource = dv
            dgLocales.DataKeyField = "key"
            dgLocales.DataBind()
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Removes all localized files for a given locale
        ''' </summary>
        ''' <param name="locale">Locale to delete</param>
        ''' <remarks>
        ''' LocalResources files are only found in \admin, \controls, \DesktopModules
        ''' Global and shared resource files are in \Resources
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub DeleteLocalizedFiles(ByVal locale As String)
            Dim fil As String

            ' Delete LocalResources from following folders
            DeleteLocalizedFiles(Server.MapPath("~"), locale, True)

            ' Delete Global/Shared resources
            For Each fil In Directory.GetFiles(Server.MapPath(Localization.ApplicationResourceDirectory))
                ' find the locale substring, ex: .nl-NL.
                If Path.GetFileName(fil).ToLower.IndexOf("." + locale.ToLower + ".") > -1 Then
                    Try
                        File.Delete(fil)
                    Catch
                        UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    End Try

                End If
            Next
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Recursively deletes files on a given folder
        ''' </summary>
        ''' <param name="folder">Initial folder</param>
        ''' <param name="locale">Locale files to be deleted</param>
        ''' <param name="recurse">Delete recursively or not</param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[vmasanas]	04/10/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub DeleteLocalizedFiles(ByVal folder As String, ByVal locale As String, ByVal recurse As Boolean)
            Dim fol As String
            Dim fil As String
            locale = locale.ToLower

            For Each fol In Directory.GetDirectories(folder)
                If Path.GetFileName(fol) = Services.Localization.Localization.LocalResourceDirectory Then
                    ' Found LocalResources folder
                    For Each fil In Directory.GetFiles(fol)
                        ' find the locale substring, ex: .nl-NL.
                        If Path.GetFileName(fil).ToLower.IndexOf("." + locale + ".resx") > -1 Then
                            Try
                                File.Delete(fil)
                            Catch
                                UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("Save.ErrorMessage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                            End Try

                        End If
                    Next
                Else
                    If recurse Then
                        'recurse
                        DeleteLocalizedFiles(fol, locale, recurse)
                    End If
                End If
            Next

        End Sub

        Private Sub LoadLocales()
            Dim localeKey As String
            Dim localeName As String

            cboLocales.Items.Clear()
            Dim cultures As CultureInfo() = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            Array.Sort(cultures, New CultureInfoComparer(rbDisplay.SelectedValue))
            For Each cinfo As CultureInfo In cultures
                localeKey = Convert.ToString(cinfo.Name)
                If rbDisplay.SelectedValue = "Native" Then
                    localeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cinfo.NativeName)
                Else
                    localeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cinfo.EnglishName)
                End If
                cboLocales.Items.Add(New ListItem(localeName, localeKey))
            Next cinfo

            chkEnableBrowser.Checked = True
            Try
                Dim browserLanguage As XmlNode
                If PortalSettings.ActiveTab.ParentId = PortalSettings.AdminTabId Then
                    If bXmlLoaded Then
                        browserLanguage = xmlLocales.SelectSingleNode("//locales/browserDetection")
                        If Not browserLanguage Is Nothing Then
                            chkEnableBrowser.Checked = Boolean.Parse(browserLanguage.Attributes("enabled").InnerText)
                        End If
                    End If
                Else
                    Dim xmldoc As New XmlDocument
                    xmldoc.Load(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.xml"))
                    browserLanguage = xmldoc.SelectSingleNode("//root/browserDetection")
                    If Not browserLanguage Is Nothing Then
                        chkEnableBrowser.Checked = Boolean.Parse(browserLanguage.Attributes("enabled").InnerText)
                    End If
                End If
            Catch
            End Try

            chkEnableLanguageInUrl.Checked = True
            Try
                Dim languageInUrl As XmlNode
                If PortalSettings.ActiveTab.ParentId = PortalSettings.AdminTabId Then
                    If bXmlLoaded Then
                        languageInUrl = xmlLocales.SelectSingleNode("//locales/languageInUrl")
                        If Not languageInUrl Is Nothing Then
                            chkEnableLanguageInUrl.Checked = Boolean.Parse(languageInUrl.Attributes("enabled").InnerText)
                        End If
                    End If
                Else
                    Dim xmldoc As New XmlDocument
                    xmldoc.Load(Server.MapPath(Localization.ApplicationResourceDirectory + "/Locales.xml"))
                    languageInUrl = xmldoc.SelectSingleNode("//root/languageInUrl")
                    If Not languageInUrl Is Nothing Then
                        chkEnableLanguageInUrl.Checked = Boolean.Parse(languageInUrl.Attributes("enabled").InnerText)
                    End If
                End If
            Catch
            End Try
        End Sub
#End Region

#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim FileManagerModule As ModuleInfo = (New ModuleController).GetModuleByDefinition(Null.NullInteger, "File Manager")
                Dim params(2) As String

                params(0) = "mid=" & FileManagerModule.ModuleID
                params(1) = "ftype=" & UploadType.LanguagePack.ToString
                params(2) = "rtab=" & Me.TabId

                Dim Actions As New ModuleActionCollection
                Actions.Add(GetNextActionID, Services.Localization.Localization.GetString("Languages.Action", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl(ControlKey:="language"), False, SecurityAccessLevel.Admin, True, False)
                Actions.Add(GetNextActionID, Services.Localization.Localization.GetString("TimeZones.Action", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl(ControlKey:="timezone"), False, SecurityAccessLevel.Host, True, False)
                Actions.Add(GetNextActionID, Services.Localization.Localization.GetString("Verify.Action", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl(ControlKey:="verify"), False, SecurityAccessLevel.Host, True, False)
                Actions.Add(GetNextActionID, Services.Localization.Localization.GetString("PackageGenerate.Action", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl(ControlKey:="package"), False, SecurityAccessLevel.Host, True, False)
                Actions.Add(GetNextActionID, Services.Localization.Localization.GetString("PackageImport.Action", LocalResourceFile), ModuleActionType.AddContent, "", "", NavigateURL(FileManagerModule.TabID, "Edit", params), False, SecurityAccessLevel.Host, True, False)
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
