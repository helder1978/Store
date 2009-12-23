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
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Modules.Admin.PortalManagement

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The SiteSettings PortalModuleBase is used to edit the main settings for a 
    ''' portal.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	9/8/2004	Updated to reflect design changes for Help, 508 support
    '''                       and localisation
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial  Class SiteSettings
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

#Region "Private Members"

        Dim intPortalId As Integer = -1

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadStyleSheet loads the stylesheet
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/8/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub LoadStyleSheet()

            Dim strUploadDirectory As String = ""

            Dim objPortalController As New PortalController
            Dim objPortal As PortalInfo = objPortalController.GetPortal(intPortalId)
            If Not objPortal Is Nothing Then
                strUploadDirectory = objPortal.HomeDirectoryMapPath
            End If

            ' read CSS file
            If System.IO.File.Exists(strUploadDirectory & "portal.css") Then
                Dim objStreamReader As StreamReader
                objStreamReader = File.OpenText(strUploadDirectory & "portal.css")
                txtStyleSheet.Text = objStreamReader.ReadToEnd
                objStreamReader.Close()
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' SkinChanged changes the skin
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/19/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function SkinChanged(ByVal SkinRoot As String, ByVal PortalId As Integer, ByVal SkinType As DotNetNuke.ui.Skins.SkinType, ByVal PostedSkinSrc As String) As Boolean
            Dim objSkinInfo As UI.Skins.SkinInfo
            Dim strSkinSrc As String = ""
            objSkinInfo = SkinController.GetSkin(SkinRoot, PortalId, SkinType.Admin)
            If Not objSkinInfo Is Nothing Then strSkinSrc = objSkinInfo.SkinSrc
            If strSkinSrc Is Nothing Then strSkinSrc = ""
            Return strSkinSrc <> PostedSkinSrc
        End Function

#End Region

#Region "Public Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' FormatCurrency formats the currency.
        ''' control.
        ''' </summary>
        ''' <returns>A formatted string</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/8/2004	Modified
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function FormatCurrency() As String

            Dim retValue As String = ""
            Try
                retValue = Convert.ToString(Common.Globals.HostSettings("HostCurrency")) & " / " & Services.Localization.Localization.GetString("Month")
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

            Return retValue
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' FormatFee formats the fee.
        ''' control.
        ''' </summary>
        ''' <returns>A formatted string</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/8/2004	Modified
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function FormatFee(ByVal objHostFee As Object) As String
            Dim retValue As String = ""

            Try
                'TODO - this needs to be localised
                If Not IsDBNull(objHostFee) Then
                    retValue = Format(objHostFee, "#,##0.00")
                Else
                    retValue = "0"
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

            Return retValue
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' IsSubscribed determines whether the portal has subscribed to the premium 
        ''' control.
        ''' </summary>
        ''' <returns>True if Subscribed, False if not</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/8/2004	Modified
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function IsSubscribed(ByVal PortalModuleDefinitionId As Integer) As Boolean
            Try
                Return Null.IsNull(PortalModuleDefinitionId) = False

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' IsSuperUser determines whether the cuurent user is a SuperUser
        ''' control.
        ''' </summary>
        ''' <returns>True if SuperUser, False if not</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	10/4/2004	Added
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function IsSuperUser() As Boolean

            Return Me.UserInfo.IsSuperUser

        End Function

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/8/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                If Not (Request.QueryString("pid") Is Nothing) And (PortalSettings.ActiveTab.ParentId = PortalSettings.SuperTabId Or UserInfo.IsSuperUser) Then
                    intPortalId = Int32.Parse(Request.QueryString("pid"))
                    ctlLogo.ShowUpLoad = False
                    ctlBackground.ShowUpLoad = False
                Else
                    intPortalId = PortalId
                    ctlLogo.ShowUpLoad = True
                    ctlBackground.ShowUpLoad = True
                End If

                'this needs to execute always to the client script code is registred in InvokePopupCal
                cmdExpiryCalendar.NavigateUrl = Common.Utilities.Calendar.InvokePopupCal(txtExpiryDate)
                ClientAPI.AddButtonConfirm(cmdRestore, Services.Localization.Localization.GetString("RestoreCCSMessage", Me.LocalResourceFile))

                ' If this is the first visit to the page, populate the site data
                If Page.IsPostBack = False Then

                    ClientAPI.AddButtonConfirm(cmdDelete, Services.Localization.Localization.GetString("DeleteMessage", Me.LocalResourceFile))

                    Dim objPortalController As New PortalController
                    Dim objModules As New ModuleController
                    Dim objUsers As New UserController
                    Dim ctlList As New Common.Lists.ListController
                    Dim colProcessor As Common.Lists.ListEntryInfoCollection = ctlList.GetListEntryInfoCollection("Processor")

                    Dim settings As Hashtable = Entities.Portals.PortalSettings.GetSiteSettings(intPortalId)

                    cboProcessor.DataSource = colProcessor
                    cboProcessor.DataBind()
                    cboProcessor.Items.Insert(0, New ListItem("<" + Services.Localization.Localization.GetString("None_Specified") + ">", ""))

                    Dim objPortal As PortalInfo = objPortalController.GetPortal(intPortalId)

                    txtPortalName.Text = objPortal.PortalName
                    ctlLogo.Url = objPortal.LogoFile
                    ctlLogo.FileFilter = glbImageFileTypes
                    txtDescription.Text = objPortal.Description
                    txtKeyWords.Text = objPortal.KeyWords
                    lblGUID.Text = objPortal.GUID.ToString.ToUpper
                    ctlBackground.Url = objPortal.BackgroundFile
                    ctlBackground.FileFilter = glbImageFileTypes
                    txtFooterText.Text = objPortal.FooterText
                    optUserRegistration.SelectedIndex = objPortal.UserRegistration

                    txtSiteMap.Text = AddHTTP(GetDomainName(Request)) & "/SiteMap.aspx"
                    lblAdvertising.Text = Services.Localization.Localization.GetString("Advertising", Me.LocalResourceFile)
                    optBanners.SelectedIndex = objPortal.BannerAdvertising

                    cboSplashTabId.DataSource = GetPortalTabs(intPortalId, True, True, False, False, False)
                    cboSplashTabId.DataBind()
                    If Not cboSplashTabId.Items.FindByValue(objPortal.SplashTabId.ToString) Is Nothing Then
                        cboSplashTabId.Items.FindByValue(objPortal.SplashTabId.ToString).Selected = True
                    End If
                    cboHomeTabId.DataSource = GetPortalTabs(intPortalId, True, True, False, False, False)
                    cboHomeTabId.DataBind()
                    If Not cboHomeTabId.Items.FindByValue(objPortal.HomeTabId.ToString) Is Nothing Then
                        cboHomeTabId.Items.FindByValue(objPortal.HomeTabId.ToString).Selected = True
                    End If
                    cboLoginTabId.DataSource = GetPortalTabs(intPortalId, True, True, False, False, False)
                    cboLoginTabId.DataBind()
                    If Not cboLoginTabId.Items.FindByValue(objPortal.LoginTabId.ToString) Is Nothing Then
                        cboLoginTabId.Items.FindByValue(objPortal.LoginTabId.ToString).Selected = True
                    End If
                    cboUserTabId.DataSource = GetPortalTabs(intPortalId, True, True, False, False, False)
                    cboUserTabId.DataBind()
                    If Not cboUserTabId.Items.FindByValue(objPortal.UserTabId.ToString) Is Nothing Then
                        cboUserTabId.Items.FindByValue(objPortal.UserTabId.ToString).Selected = True
                    End If

                    Dim colList As Common.Lists.ListEntryInfoCollection = ctlList.GetListEntryInfoCollection("Currency")

                    cboCurrency.DataSource = colList
                    cboCurrency.DataBind()
                    If Null.IsNull(objPortal.Currency) Or cboCurrency.Items.FindByValue(objPortal.Currency) Is Nothing Then
                        cboCurrency.Items.FindByValue("USD").Selected = True
                    Else
                        cboCurrency.Items.FindByValue(objPortal.Currency).Selected = True
                    End If
                    Dim objRoleController As New DotNetNuke.Security.Roles.RoleController

                    Dim Arr As ArrayList = objRoleController.GetUserRolesByRoleName(intPortalId, objPortal.AdministratorRoleName)
                    Dim i As Integer
                    For i = 0 To Arr.Count - 1
                        Dim objUser As UserRoleInfo = CType(Arr(i), UserRoleInfo)
                        cboAdministratorId.Items.Add(New ListItem(objUser.FullName, objUser.UserID.ToString))
                    Next
                    If Not cboAdministratorId.Items.FindByValue(objPortal.AdministratorId.ToString) Is Nothing Then
                        cboAdministratorId.Items.FindByValue(objPortal.AdministratorId.ToString).Selected = True
                    End If

                    If Not Null.IsNull(objPortal.ExpiryDate) Then
                        txtExpiryDate.Text = objPortal.ExpiryDate.ToShortDateString
                    End If
                    txtHostFee.Text = objPortal.HostFee.ToString
                    txtHostSpace.Text = objPortal.HostSpace.ToString
                    txtPageQuota.Text = objPortal.PageQuota.ToString
                    txtUserQuota.Text = objPortal.UserQuota.ToString
                    If Not IsDBNull(objPortal.SiteLogHistory) Then
                        txtSiteLogHistory.Text = objPortal.SiteLogHistory.ToString
                    End If

                    Dim objDesktopModules As New DesktopModuleController
                    Dim arrDesktopModules As ArrayList = objDesktopModules.GetDesktopModules

                    Dim arrPremiumModules As New ArrayList
                    Dim objDesktopModule As DesktopModuleInfo
                    For Each objDesktopModule In arrDesktopModules
                        If objDesktopModule.IsPremium Then
                            arrPremiumModules.Add(objDesktopModule)
                        End If
                    Next

                    Dim arrPortalDesktopModules As ArrayList = objDesktopModules.GetPortalDesktopModules(intPortalId, Null.NullInteger)
                    Dim objPortalDesktopModule As PortalDesktopModuleInfo
                    For Each objPortalDesktopModule In arrPortalDesktopModules
                        For Each objDesktopModule In arrPremiumModules
                            If objDesktopModule.DesktopModuleID = objPortalDesktopModule.DesktopModuleID Then
                                arrPremiumModules.Remove(objDesktopModule)
                                Exit For
                            End If
                        Next
                    Next

                    ctlDesktopModules.Available = arrPremiumModules
                    ctlDesktopModules.Assigned = arrPortalDesktopModules

                    If objPortal.PaymentProcessor <> "" Then
                        If Not cboProcessor.Items.FindByText(objPortal.PaymentProcessor) Is Nothing Then
                            cboProcessor.Items.FindByText(objPortal.PaymentProcessor).Selected = True
                        Else       ' default
                            If Not cboProcessor.Items.FindByText("PayPal") Is Nothing Then
                                cboProcessor.Items.FindByText("PayPal").Selected = True
                            End If
                        End If
                    Else
                        cboProcessor.Items.FindByValue("").Selected = True
                    End If
                    txtUserId.Text = objPortal.ProcessorUserId
                    txtPassword.Attributes.Add("value", objPortal.ProcessorPassword)

                    ' usability settings
                    If Convert.ToString(settings("InlineEditorEnabled")) <> "False" Then
                        chkInlineEditor.Checked = True
                    End If
                    If Convert.ToString(settings("ControlPanelMode")) <> "VIEW" Then
                        optControlPanelMode.Items.FindByValue("EDIT").Selected = True
                    Else
                        optControlPanelMode.Items.FindByValue("VIEW").Selected = True
                    End If
                    If Convert.ToString(settings("ControlPanelVisibility")) <> "MIN" Then
                        optControlPanelVisibility.Items.FindByValue("MAX").Selected = True
                    Else
                        optControlPanelVisibility.Items.FindByValue("MIN").Selected = True
                    End If
                    If Convert.ToString(settings("ControlPanelSecurity")) <> "TAB" Then
                        optControlPanelSecurity.Items.FindByValue("MODULE").Selected = True
                    Else
                        optControlPanelSecurity.Items.FindByValue("TAB").Selected = True
                    End If
                    If Convert.ToString(settings("SSLEnabled")) = "True" Then
                        chkSSLEnabled.Checked = True
                    End If
                    If Convert.ToString(settings("SSLEnforced")) = "True" Then
                        chkSSLEnforced.Checked = True
                    End If
                    txtSSLURL.Text = Convert.ToString(settings("SSLURL"))
                    txtSTDURL.Text = Convert.ToString(settings("STDURL"))

                    txtHomeDirectory.Text = objPortal.HomeDirectory

                    'Populate the default language combobox
                    Services.Localization.Localization.LoadCultureDropDownList(cboDefaultLanguage, CultureDropDownTypes.NativeName, objPortal.DefaultLanguage)

                    'Populate the timezone combobox (look up timezone translations based on currently set culture)
                    Services.Localization.Localization.LoadTimeZoneDropDownList(cboTimeZone, CType(Page, PageBase).PageCulture.Name, Convert.ToString(objPortal.TimeZoneOffset))

                    Dim objSkin As UI.Skins.SkinInfo

                    ctlPortalSkin.Width = "275px"
                    ctlPortalSkin.SkinRoot = SkinInfo.RootSkin
                    objSkin = SkinController.GetSkin(SkinInfo.RootSkin, intPortalId, SkinType.Portal)
                    If Not objSkin Is Nothing Then
                        If objSkin.PortalId = intPortalId Then
                            ctlPortalSkin.SkinSrc = objSkin.SkinSrc
                        End If
                    End If
                    ctlPortalContainer.Width = "275px"
                    ctlPortalContainer.SkinRoot = SkinInfo.RootContainer
                    objSkin = SkinController.GetSkin(SkinInfo.RootContainer, intPortalId, SkinType.Portal)
                    If Not objSkin Is Nothing Then
                        If objSkin.PortalId = intPortalId Then
                            ctlPortalContainer.SkinSrc = objSkin.SkinSrc
                        End If
                    End If

                    ctlAdminSkin.Width = "275px"
                    ctlAdminSkin.SkinRoot = SkinInfo.RootSkin
                    objSkin = SkinController.GetSkin(SkinInfo.RootSkin, intPortalId, SkinType.Admin)
                    If Not objSkin Is Nothing Then
                        If objSkin.PortalId = intPortalId Then
                            ctlAdminSkin.SkinSrc = objSkin.SkinSrc
                        End If
                    End If
                    ctlAdminContainer.Width = "275px"
                    ctlAdminContainer.SkinRoot = SkinInfo.RootContainer
                    objSkin = SkinController.GetSkin(SkinInfo.RootContainer, intPortalId, SkinType.Admin)
                    If Not objSkin Is Nothing Then
                        If objSkin.PortalId = intPortalId Then
                            ctlAdminContainer.SkinSrc = objSkin.SkinSrc
                        End If
                    End If

                    LoadStyleSheet()

                    If Convert.ToString(PortalSettings.HostSettings("SkinUpload")) = "G" And Not UserInfo.IsSuperUser Then
                        lnkUploadSkin.Visible = False
                        lnkUploadContainer.Visible = False
                    Else
                        Dim FileManagerModule As ModuleInfo = (New ModuleController).GetModuleByDefinition(intPortalId, "File Manager")
                        Dim params(2) As String
                        params(0) = "mid=" & FileManagerModule.ModuleID
                        params(1) = "ftype=" & UploadType.Skin.ToString
                        params(2) = "rtab=" & Me.TabId
                        lnkUploadSkin.NavigateUrl = NavigateURL(FileManagerModule.TabID, "Edit", params)

                        params(1) = "ftype=" & UploadType.Container.ToString
                        lnkUploadContainer.NavigateUrl = NavigateURL(FileManagerModule.TabID, "Edit", params)
                    End If

                    If Not Request.UrlReferrer Is Nothing Then
                        If Request.UrlReferrer.AbsoluteUri = Request.Url.AbsoluteUri Then
                            ViewState("UrlReferrer") = ""
                        Else
                            'Make sure we have not been referred from the PortalAliases module
                            Dim objModuleController As New ModuleController
                            Dim objModule As ModuleInfo = objModuleController.GetModuleByDefinition(PortalId, "Portal Aliases")

                            If Not (Request.UrlReferrer.AbsoluteUri.Contains("mid") And Request.UrlReferrer.AbsoluteUri.Contains(objModule.ModuleID.ToString)) Then
                                ViewState("UrlReferrer") = Convert.ToString(Request.UrlReferrer)
                            End If
                        End If
                    Else
                        ViewState("UrlReferrer") = ""
                    End If

                End If

                If UserInfo.IsSuperUser Then
                    dshHost.Visible = True
                    tblHost.Visible = True
                    dshSSL.Visible = True
                    tblSSL.Visible = True
                    cmdDelete.Visible = (intPortalId <> PortalId)
                    If Convert.ToString(ViewState("UrlReferrer")) = "" Then
                        cmdCancel.Visible = False
                    Else
                        cmdCancel.Visible = True
                    End If
                Else
                    dshHost.Visible = False
                    tblHost.Visible = False
                    dshSSL.Visible = False
                    tblSSL.Visible = False
                    cmdDelete.Visible = False
                    cmdCancel.Visible = False
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdCancel_Click runs when the Cancel LinkButton is clicked.  It returns the user
        ''' to the referring page
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/9/2004	Modified
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(Convert.ToString(ViewState("UrlReferrer")), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdDelete_Click runs when the Delete LinkButton is clicked.
        ''' It deletes the current portal form the Database.  It can only run in Host
        ''' (SuperUser) mode
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/9/2004	Modified
        '''     [VMasanas]  9/12/2004   Move skin deassignment to DeletePortalInfo.
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
            Try

                Dim objPortalController As New PortalController
                Dim objPortalInfo As PortalInfo = objPortalController.GetPortal(intPortalId)

                If Not objPortalInfo Is Nothing Then
                    Dim strMessage As String = PortalController.DeletePortal(objPortalInfo, GetAbsoluteServerPath(Request))

                    If String.IsNullOrEmpty(strMessage) Then
                        Dim objEventLog As New Services.Log.EventLog.EventLogController
                        objEventLog.AddLog("PortalName", objPortalInfo.PortalName, PortalSettings, UserId, Services.Log.EventLog.EventLogController.EventLogType.PORTAL_DELETED)

                        ' Redirect to another site
                        If intPortalId = PortalId Then
                            If PortalSettings.HostSettings("HostURL").ToString <> "" Then
                                Response.Redirect(AddHTTP(PortalSettings.HostSettings("HostURL").ToString))
                            Else
                                Response.End()
                            End If
                        Else
                            Response.Redirect(Convert.ToString(ViewState("UrlReferrer")), True)
                        End If
                    Else
                        UI.Skins.Skin.AddModuleMessage(Me, strMessage, ModuleMessageType.RedError)
                    End If
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdProcessor_Click runs when the Processor Website Linkbutton is clicked. It
        ''' redirects the user to the selected processor's website.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/9/2004	Modified
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdProcessor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdProcessor.Click
            Try
                Response.Redirect(AddHTTP(cboProcessor.SelectedItem.Value), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdRestore_Click runs when the Restore Default Stylesheet Linkbutton is clicked. 
        ''' It reloads the default stylesheet (copies from _default Portal to current Portal)
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/9/2004	Modified
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdRestore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRestore.Click
            Try
                Dim strServerPath As String = Request.MapPath(Common.Globals.ApplicationPath)

                Dim objPortalController As New PortalController
                Dim objPortal As PortalInfo = objPortalController.GetPortal(intPortalId)
                If Not objPortal Is Nothing Then
                    If System.IO.File.Exists(objPortal.HomeDirectoryMapPath + "portal.css") Then
                        ' delete existing style sheet
                        System.IO.File.Delete(objPortal.HomeDirectoryMapPath + "portal.css")
                    End If
                    ' copy the default style sheet to the upload directory
                    System.IO.File.Copy(Common.Globals.HostMapPath + "portal.css", objPortal.HomeDirectoryMapPath + "portal.css")
                End If

                LoadStyleSheet()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdSave_Click runs when the Save Stylesheet Linkbutton is clicked.  It saves
        ''' the edited Stylesheet
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/9/2004	Modified
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            Try
                Dim strUploadDirectory As String = ""

                Dim objPortalController As New PortalController
                Dim objPortal As PortalInfo = objPortalController.GetPortal(intPortalId)
                If Not objPortal Is Nothing Then
                    strUploadDirectory = objPortal.HomeDirectoryMapPath
                End If

                ' reset attributes
                If File.Exists(strUploadDirectory & "portal.css") Then
                    File.SetAttributes(strUploadDirectory & "portal.css", FileAttributes.Normal)
                End If

                ' write CSS file
                Dim objStream As StreamWriter
                objStream = File.CreateText(strUploadDirectory & "portal.css")
                objStream.WriteLine(txtStyleSheet.Text)
                objStream.Close()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdUpdate_Click runs when the Update LinkButton is clicked.
        ''' It saves the current Site Settings
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/9/2004	Modified
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            If Page.IsValid Then

                Try
                    Dim strLogo As String
                    Dim strBackground As String
                    Dim refreshPage As Boolean = Null.NullBoolean

                    Dim objPortalController As New PortalController
                    Dim objPortal As PortalInfo = objPortalController.GetPortal(intPortalId)

                    strLogo = ctlLogo.Url
                    strBackground = ctlBackground.Url

                    'Refresh if Background or Logo file have changed
                    refreshPage = (strBackground = objPortal.BackgroundFile Or strLogo = objPortal.LogoFile)

                    Dim dblHostFee As Double = 0
                    If txtHostFee.Text <> "" Then
                        dblHostFee = Double.Parse(txtHostFee.Text)
                    End If

                    Dim dblHostSpace As Double = 0
                    If txtHostSpace.Text <> "" Then
                        dblHostSpace = Double.Parse(txtHostSpace.Text)
                    End If

                    Dim intPageQuota As Integer = 0
                    If txtPageQuota.Text <> "" Then
                        intPageQuota = Integer.Parse(txtPageQuota.Text)
                    End If

                    Dim intUserQuota As Double = 0
                    If txtUserQuota.Text <> "" Then
                        intUserQuota = Integer.Parse(txtUserQuota.Text)
                    End If

                    Dim intSiteLogHistory As Integer = -1
                    If txtSiteLogHistory.Text <> "" Then
                        intSiteLogHistory = Integer.Parse(txtSiteLogHistory.Text)
                    End If

                    Dim datExpiryDate As Date = Null.NullDate
                    If txtExpiryDate.Text <> "" Then
                        datExpiryDate = Convert.ToDateTime(txtExpiryDate.Text)
                    End If

                    Dim intSplashTabId As Integer = Null.NullInteger
                    If Not cboSplashTabId.SelectedItem Is Nothing Then
                        intSplashTabId = Integer.Parse(cboSplashTabId.SelectedItem.Value)
                    End If

                    Dim intHomeTabId As Integer = Null.NullInteger
                    If Not cboHomeTabId.SelectedItem Is Nothing Then
                        intHomeTabId = Integer.Parse(cboHomeTabId.SelectedItem.Value)
                    End If

                    Dim intLoginTabId As Integer = Null.NullInteger
                    If Not cboLoginTabId.SelectedItem Is Nothing Then
                        intLoginTabId = Integer.Parse(cboLoginTabId.SelectedItem.Value)
                    End If

                    Dim intUserTabId As Integer = Null.NullInteger
                    If Not cboUserTabId.SelectedItem Is Nothing Then
                        intUserTabId = Integer.Parse(cboUserTabId.SelectedItem.Value)
                    End If

                    If Not txtPassword.Attributes.Item("value") Is Nothing Then
                        txtPassword.Attributes.Item("value") = txtPassword.Text
                    End If

                    'check only relevant fields altered
                    If Not UserInfo.IsSuperUser Then
                        Dim HostChanged As Boolean = False
                        If dblHostFee <> objPortal.HostFee Then HostChanged = True
                        If dblHostSpace <> objPortal.HostSpace Then HostChanged = True
                        If intPageQuota <> objPortal.PageQuota Then HostChanged = True
                        If intUserQuota <> objPortal.UserQuota Then HostChanged = True
                        If intSiteLogHistory <> objPortal.SiteLogHistory Then HostChanged = True
                        If datExpiryDate <> objPortal.ExpiryDate Then HostChanged = True
                        If HostChanged = True Then
                            Throw New System.Exception
                        End If
                    End If

                    objPortalController.UpdatePortalInfo(intPortalId, txtPortalName.Text, strLogo, _
                        txtFooterText.Text, datExpiryDate, optUserRegistration.SelectedIndex, _
                        optBanners.SelectedIndex, cboCurrency.SelectedItem.Value, _
                        Convert.ToInt32(cboAdministratorId.SelectedItem.Value), dblHostFee, _
                        dblHostSpace, intPageQuota, intUserQuota, _
                        IIf(cboProcessor.SelectedValue = "", "", cboProcessor.SelectedItem.Text).ToString, _
                        txtUserId.Text, txtPassword.Text, txtDescription.Text, txtKeyWords.Text, _
                        strBackground, intSiteLogHistory, intSplashTabId, intHomeTabId, intLoginTabId, _
                        intUserTabId, cboDefaultLanguage.SelectedValue, Convert.ToInt32(cboTimeZone.SelectedValue), _
                        txtHomeDirectory.Text)

                    If Not refreshPage Then
                        refreshPage = SkinChanged(SkinInfo.RootSkin, intPortalId, SkinType.Admin, ctlAdminSkin.SkinSrc) OrElse SkinChanged(SkinInfo.RootContainer, intPortalId, SkinType.Admin, ctlAdminContainer.SkinSrc)
                    End If

                    SkinController.SetSkin(SkinInfo.RootSkin, intPortalId, SkinType.Portal, ctlPortalSkin.SkinSrc)
                    SkinController.SetSkin(SkinInfo.RootContainer, intPortalId, SkinType.Portal, ctlPortalContainer.SkinSrc)
                    SkinController.SetSkin(SkinInfo.RootSkin, intPortalId, SkinType.Admin, ctlAdminSkin.SkinSrc)
                    SkinController.SetSkin(SkinInfo.RootContainer, intPortalId, SkinType.Admin, ctlAdminContainer.SkinSrc)

                    Entities.Portals.PortalSettings.UpdateSiteSetting(intPortalId, "InlineEditorEnabled", chkInlineEditor.Checked.ToString)
                    Entities.Portals.PortalSettings.UpdateSiteSetting(intPortalId, "ControlPanelMode", optControlPanelMode.SelectedItem.Value)
                    Entities.Portals.PortalSettings.UpdateSiteSetting(intPortalId, "ControlPanelVisibility", optControlPanelVisibility.SelectedItem.Value)
                    Entities.Portals.PortalSettings.UpdateSiteSetting(intPortalId, "ControlPanelSecurity", optControlPanelSecurity.SelectedItem.Value)

                    If IsSuperUser() Then
                        Entities.Portals.PortalSettings.UpdateSiteSetting(intPortalId, "SSLEnabled", chkSSLEnabled.Checked.ToString)
                        Entities.Portals.PortalSettings.UpdateSiteSetting(intPortalId, "SSLEnforced", chkSSLEnforced.Checked.ToString)
                        Entities.Portals.PortalSettings.UpdateSiteSetting(intPortalId, "SSLURL", AddPortalAlias(txtSSLURL.Text, intPortalId))
                        Entities.Portals.PortalSettings.UpdateSiteSetting(intPortalId, "STDURL", AddPortalAlias(txtSTDURL.Text, intPortalId))
                    End If

                    If UserInfo.IsSuperUser Then
                        ' delete old portal module assignments
                        Dim objDesktopModules As New DesktopModuleController
                        objDesktopModules.DeletePortalDesktopModules(intPortalId, Null.NullInteger)
                        ' add new portal module assignments
                        Dim objListItem As ListItem
                        For Each objListItem In ctlDesktopModules.Assigned
                            objDesktopModules.AddPortalDesktopModule(intPortalId, Integer.Parse(objListItem.Value))
                        Next
                    End If

                    ' Redirect to this site to refresh only if admin skin changed or either of the images have changed
                    If refreshPage Then Response.Redirect(Request.RawUrl, True)

                Catch exc As Exception    'Module failed to load
                    ProcessModuleLoadException(Me, exc)
                End Try
            End If
        End Sub

        Private Function AddPortalAlias(ByVal PortalAlias As String, ByVal PortalID As Integer) As String
            If PortalAlias <> "" Then
                If PortalAlias.IndexOf("://") <> -1 Then
                    PortalAlias = PortalAlias.Remove(0, PortalAlias.IndexOf("://") + 3)
                End If
                Dim objPortalAliasController As New PortalAliasController
                Dim objPortalAlias As PortalAliasInfo = objPortalAliasController.GetPortalAlias(PortalAlias, PortalID)
                If objPortalAlias Is Nothing Then
                    objPortalAlias = New PortalAliasInfo
                    objPortalAlias.PortalID = PortalID
                    objPortalAlias.HTTPAlias = PortalAlias
                    objPortalAliasController.AddPortalAlias(objPortalAlias)
                End If
            End If
            Return PortalAlias
        End Function

        Protected Sub cmdSearchEngine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearchEngine.Click
            Try
                If Not cboSearchEngine.SelectedItem Is Nothing Then
                    Dim strURL As String = ""
                    Select Case cboSearchEngine.SelectedItem.Text
                        Case "Google"
                            strURL += "http://www.google.com/addurl?q=" & HTTPPOSTEncode(AddHTTP(GetDomainName(Request)))
                            strURL += "&dq="
                            If txtPortalName.Text <> "" Then
                                strURL += HTTPPOSTEncode(txtPortalName.Text)
                            End If
                            If txtDescription.Text <> "" Then
                                strURL += HTTPPOSTEncode(txtDescription.Text)
                            End If
                            If txtKeyWords.Text <> "" Then
                                strURL += HTTPPOSTEncode(txtKeyWords.Text)
                            End If
                            strURL += "&submit=Add+URL"
                        Case "Yahoo"
                            strURL = "http://siteexplorer.search.yahoo.com/submit"
                        Case "Microsoft"
                            strURL = "http://search.msn.com.sg/docs/submit.aspx"
                    End Select

                    UrlUtils.OpenNewWindow(strURL)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Sub cmdSiteMap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSiteMap.Click
            UrlUtils.OpenNewWindow("http://www.google.com/webmasters/sitemaps/")
        End Sub

        Protected Sub cmdVerification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdVerification.Click
            If txtVerification.Text <> "" AndAlso txtVerification.Text.EndsWith(".html") Then
                If Not File.Exists(ApplicationMapPath & "\" & txtVerification.Text) Then
                    ' write SiteMap verification file
                    Dim objStream As StreamWriter
                    objStream = File.CreateText(ApplicationMapPath & "\" & txtVerification.Text)
                    objStream.WriteLine("Google SiteMap Verification File")
                    objStream.WriteLine(" - " & txtSiteMap.Text)
                    objStream.WriteLine(" - " & UserInfo.DisplayName)
                    objStream.Close()
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace

