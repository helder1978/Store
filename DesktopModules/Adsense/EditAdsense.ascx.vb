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
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Vendors

Namespace DotNetNuke.Modules.Adsense

    Partial Class EditAdsense

        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack Then

                    If ModuleId = -1 Then
                        ' invoked externally
                        If Not Request.QueryString("adclient") Is Nothing Then
                            txtAdClient.Text = Request.QueryString("adclient")
                            cmdAdClient.Visible = False
                            tblAdsense.Visible = False
                            cmdPreview.Visible = False
                            UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("AdClient.Message", Me.LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                        End If
                    Else
                        ' get module settings
                        Dim settings As Hashtable = Entities.Portals.PortalSettings.GetModuleSettings(ModuleId)
                        If CType(settings("script"), String) <> "" Then
                            txtAdClient.Text = CType(settings("adclient"), String)
                            cboFormat.Items.FindByValue(CType(settings("format"), String)).Selected = True
                            txtChannel.Text = CType(settings("channel"), String)
                            If Not cboBorder.Items.FindByValue(CType(settings("border"), String)) Is Nothing Then
                                cboBorder.Items.FindByValue(CType(settings("border"), String)).Selected = True
                            Else
                                txtBorder.Text = CType(settings("border"), String)
                            End If
                            If Not cboTitle.Items.FindByValue(CType(settings("title"), String)) Is Nothing Then
                                cboTitle.Items.FindByValue(CType(settings("title"), String)).Selected = True
                            Else
                                txtTitle.Text = CType(settings("title"), String)
                            End If
                            If Not cboBackground.Items.FindByValue(CType(settings("background"), String)) Is Nothing Then
                                cboBackground.Items.FindByValue(CType(settings("background"), String)).Selected = True
                            Else
                                txtBackground.Text = CType(settings("background"), String)
                            End If
                            If Not cboText.Items.FindByValue(CType(settings("text"), String)) Is Nothing Then
                                cboText.Items.FindByValue(CType(settings("text"), String)).Selected = True
                            Else
                                txtText.Text = CType(settings("text"), String)
                            End If
                            If Not cboURL.Items.FindByValue(CType(settings("url"), String)) Is Nothing Then
                                cboURL.Items.FindByValue(CType(settings("url"), String)).Selected = True
                            Else
                                txtURL.Text = CType(settings("url"), String)
                            End If
                        Else ' use defaults
                            txtAdClient.Text = Entities.Portals.PortalSettings.GetSiteSetting(PortalId, "AdClient")
                            cboFormat.Items.FindByValue("468x60").Selected = True
                            txtChannel.Text = ""
                            cboBorder.Items.FindByValue("000000").Selected = True
                            cboTitle.Items.FindByValue("0000FF").Selected = True
                            cboBackground.Items.FindByValue("FFFFFF").Selected = True
                            cboText.Items.FindByValue("000000").Selected = True
                            cboURL.Items.FindByValue("008000").Selected = True
                        End If

                        If txtAdClient.Text = "" Then
                            cmdAdClient.Visible = True
                        End If

                        Preview()

                    End If

                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            Try
                If Page.IsValid Then
                    SaveSettings()
                    Response.Redirect(NavigateURL())
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            Response.Redirect(NavigateURL())

        End Sub

        Protected Sub cboDisplayOptions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFormat.SelectedIndexChanged, cboBackground.SelectedIndexChanged, cboBorder.SelectedIndexChanged, cboText.SelectedIndexChanged, cboTitle.SelectedIndexChanged, cboURL.SelectedIndexChanged

            Preview()

        End Sub

        Protected Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click

            Preview()

        End Sub

        Protected Sub cmdAdClient_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdClient.Click

            If Services.Localization.Localization.GetString("cmdAdClient.URL", Me.LocalResourceFile) <> "" Then
                UrlUtils.OpenNewWindow(Services.Localization.Localization.GetString("cmdAdClient.URL", Me.LocalResourceFile))
            End If

        End Sub

#End Region

#Region "Private Methods"

        Private Sub Preview()

            lblPreview.Text = GetScript(True)

        End Sub

        Private Function GetColor(ByVal Color As String) As String

            If Color.StartsWith("#") Then
                Color = Color.Substring(1)
            End If
            Return Color

        End Function

        Private Function GetScript(ByVal Preview As Boolean) As String

            Dim strScript As String = Services.Localization.Localization.GetString("Script", Me.LocalResourceFile)
            If strScript <> "" Then
                If Preview Then
                    strScript = strScript.Replace("[ADCLIENT]", "")
                Else
                    strScript = strScript.Replace("[ADCLIENT]", txtAdClient.Text)
                End If
                Dim strAdHost As String = ""
                If Preview = False And Entities.Portals.PortalSettings.GetSiteSetting(PortalId, "AdClient") = txtAdClient.Text Then
                    If Services.Localization.Localization.GetString("AdHost", Me.LocalResourceFile) <> "" Then
                        strAdHost = vbCrLf & "google_ad_host = """ & Services.Localization.Localization.GetString("AdHost", Me.LocalResourceFile) & """;"
                    End If
                End If
                strScript = strScript.Replace("[HOST]", strAdHost)
                strScript = strScript.Replace("[FORMAT]", cboFormat.SelectedItem.Value & "_as")
                strScript = strScript.Replace("[CHANNEL]", txtChannel.Text)
                strScript = strScript.Replace("[WIDTH]", cboFormat.SelectedItem.Value.Substring(0, cboFormat.SelectedItem.Value.IndexOf("x")))
                strScript = strScript.Replace("[HEIGHT]", cboFormat.SelectedItem.Value.Substring(cboFormat.SelectedItem.Value.IndexOf("x") + 1))
                If txtBorder.Text <> "" Then
                    strScript = strScript.Replace("[BORDER]", GetColor(txtBorder.Text))
                    cboBorder.SelectedIndex = 0
                Else
                    strScript = strScript.Replace("[BORDER]", cboBorder.SelectedItem.Value)
                End If
                If txtBackground.Text <> "" Then
                    strScript = strScript.Replace("[BACKGROUND]", GetColor(txtBackground.Text))
                    cboBackground.SelectedIndex = 0
                Else
                    strScript = strScript.Replace("[BACKGROUND]", cboBackground.SelectedItem.Value)
                End If
                If txtTitle.Text <> "" Then
                    strScript = strScript.Replace("[TITLE]", GetColor(txtTitle.Text))
                    cboTitle.SelectedIndex = 0
                Else
                    strScript = strScript.Replace("[TITLE]", cboTitle.SelectedItem.Value)
                End If
                If txtText.Text <> "" Then
                    strScript = strScript.Replace("[TEXT]", GetColor(txtText.Text))
                    cboText.SelectedIndex = 0
                Else
                    strScript = strScript.Replace("[TEXT]", cboText.SelectedItem.Value)
                End If
                If txtURL.Text <> "" Then
                    strScript = strScript.Replace("[URL]", GetColor(txtURL.Text))
                    cboURL.SelectedIndex = 0
                Else
                    strScript = strScript.Replace("[URL]", cboURL.SelectedItem.Value)
                End If
            End If
            Return strScript

        End Function

        Private Sub SaveSettings()

            If ModuleId = -1 Then
                ' invoked externally
                Entities.Portals.PortalSettings.UpdateSiteSetting(PortalId, "AdClient", txtAdClient.Text)
            Else
                ' save module settings
                Dim objModules As New ModuleController

                objModules.UpdateModuleSetting(ModuleId, "adclient", txtAdClient.Text)
                objModules.UpdateModuleSetting(ModuleId, "format", cboFormat.SelectedItem.Value)
                objModules.UpdateModuleSetting(ModuleId, "channel", txtChannel.Text)
                If txtBorder.Text <> "" Then
                    objModules.UpdateModuleSetting(ModuleId, "border", GetColor(txtBorder.Text))
                Else
                    objModules.UpdateModuleSetting(ModuleId, "border", cboBorder.SelectedItem.Value)
                End If
                If txtTitle.Text <> "" Then
                    objModules.UpdateModuleSetting(ModuleId, "title", GetColor(txtTitle.Text))
                Else
                    objModules.UpdateModuleSetting(ModuleId, "title", cboTitle.SelectedItem.Value)
                End If
                If txtBackground.Text <> "" Then
                    objModules.UpdateModuleSetting(ModuleId, "background", GetColor(txtBackground.Text))
                Else
                    objModules.UpdateModuleSetting(ModuleId, "background", cboBackground.SelectedItem.Value)
                End If
                If txtText.Text <> "" Then
                    objModules.UpdateModuleSetting(ModuleId, "text", GetColor(txtText.Text))
                Else
                    objModules.UpdateModuleSetting(ModuleId, "text", cboText.SelectedItem.Value)
                End If
                If txtURL.Text <> "" Then
                    objModules.UpdateModuleSetting(ModuleId, "url", GetColor(txtURL.Text))
                Else
                    objModules.UpdateModuleSetting(ModuleId, "url", cboURL.SelectedItem.Value)
                End If
                Dim strScript As String = GetScript(False)
                objModules.UpdateModuleSetting(ModuleId, "script", strScript)

                SynchronizeModule()
            End If


        End Sub

#End Region

    End Class

End Namespace
