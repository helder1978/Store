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
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Framework
	''' -----------------------------------------------------------------------------
	''' Project	 : DotNetNuke
	''' Class	 : CDefault
	''' 
	''' -----------------------------------------------------------------------------
	''' <summary>
	''' 
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[sun1]	1/19/2004	Created
	''' </history>
	''' -----------------------------------------------------------------------------
    Partial Class DefaultPage

        Inherits DotNetNuke.Framework.CDefault : Implements IClientAPICallbackEventHandler

#Region "Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Property to allow the programmatic assigning of ScrollTop position
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[Jon Henning]	3/23/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property PageScrollTop() As Integer
            Get
                If ScrollTop.Value.Length > 0 AndAlso IsNumeric(ScrollTop.Value) Then
                    Return CInt(ScrollTop.Value)
                End If
            End Get
            Set(ByVal Value As Integer)
                ScrollTop.Value = Value.ToString
            End Set
        End Property

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks>
        ''' - Obtain PortalSettings from Current Context
        ''' - redirect to a specific tab based on name
        ''' - if first time loading this page then reload to avoid caching
        ''' - set page title and stylesheet
        ''' - check to see if we should show the Assembly Version in Page Title 
        ''' - set the background image if there is one selected
        ''' - set META tags, copyright, keywords and description
        ''' </remarks>
        ''' <history>
        ''' 	[sun1]	1/19/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub InitializePage()

            Dim objTabs As New TabController
            Dim objTab As TabInfo

            ' redirect to a specific tab based on name
            If Request.QueryString("tabname") <> "" Then
                Dim strURL As String = ""

                objTab = objTabs.GetTabByName(Request.QueryString("TabName"), CType(HttpContext.Current.Items("PortalSettings"), PortalSettings).PortalId)
                If Not objTab Is Nothing Then

                    Dim actualParamCount As Integer = 0
                    Dim params(Request.QueryString.Count - 1) As String 'maximum number of elements
                    For intParam As Integer = 0 To Request.QueryString.Count - 1
                        Select Case Request.QueryString.Keys(intParam).ToLower()
                            Case "tabid", "tabname"
                            Case Else
                                params(actualParamCount) = Request.QueryString.Keys(intParam) + "=" + Request.QueryString(intParam)
                                actualParamCount = actualParamCount + 1
                        End Select
                    Next
                    ReDim Preserve params(actualParamCount - 1) 'redim to remove blank elements

                    Response.Redirect(NavigateURL(objTab.TabID, Null.NullString, params), True)

                End If
            End If

            If Request.IsAuthenticated = True Then
                ' set client side page caching for authenticated users
                If Convert.ToString(PortalSettings.HostSettings("AuthenticatedCacheability")) <> "" Then
                    Select Case Convert.ToString(PortalSettings.HostSettings("AuthenticatedCacheability"))
                        Case "0" : Response.Cache.SetCacheability(HttpCacheability.NoCache)
                        Case "1" : Response.Cache.SetCacheability(HttpCacheability.Private)
                        Case "2" : Response.Cache.SetCacheability(HttpCacheability.Public)
                        Case "3" : Response.Cache.SetCacheability(HttpCacheability.Server)
                        Case "4" : Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache)
                        Case "5" : Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate)
                    End Select
                Else
                    Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache)
                End If
            End If


            ' page comment
            If GetHashValue(Common.Globals.HostSettings("Copyright"), "Y") = "Y" Then

                Comment += vbCrLf & _
                            "<!--**********************************************************************************-->" & vbCrLf & _
                            "<!-- DotNetNuke® - http://www.dotnetnuke.com                                          -->" & vbCrLf & _
                            "<!-- Copyright (c) 2002-2007                                                          -->" & vbCrLf & _
                            "<!-- by DotNetNuke Corporation                                                        -->" & vbCrLf & _
                            "<!--**********************************************************************************-->" & vbCrLf
            End If
            Page.Header.Controls.AddAt(0, New LiteralControl(Comment))

            If PortalSettings.ActiveTab.PageHeadText <> Null.NullString Then
                Page.Header.Controls.Add(New LiteralControl(PortalSettings.ActiveTab.PageHeadText))
            End If

            ' set page title
            Dim strTitle As String = PortalSettings.PortalName
            For Each objTab In PortalSettings.ActiveTab.BreadCrumbs
                strTitle += " > " & objTab.TabName
            Next
            ' tab title override
            If PortalSettings.ActiveTab.Title <> "" Then
                strTitle = PortalSettings.ActiveTab.Title
            End If
            Title = strTitle

            'set the background image if there is one selected
            If Not Me.FindControl("Body") Is Nothing Then
                If PortalSettings.BackgroundFile <> "" Then
                    CType(Me.FindControl("Body"), HtmlGenericControl).Attributes("background") = PortalSettings.HomeDirectory & PortalSettings.BackgroundFile
                End If
            End If

            ' META Refresh
            If PortalSettings.ActiveTab.RefreshInterval > 0 _
                    AndAlso Request.QueryString("ctl") Is Nothing Then
                MetaRefresh.Content = PortalSettings.ActiveTab.RefreshInterval.ToString
            Else
                MetaRefresh.Visible = False
            End If

            ' META description
            If PortalSettings.ActiveTab.Description <> "" Then
                Description = PortalSettings.ActiveTab.Description
            Else
                Description = PortalSettings.Description
            End If

            ' META keywords
            If PortalSettings.ActiveTab.KeyWords <> "" Then
                KeyWords = PortalSettings.ActiveTab.KeyWords
            Else
                KeyWords = PortalSettings.KeyWords
            End If
            If GetHashValue(Common.Globals.HostSettings("Copyright"), "Y") = "Y" Then
                KeyWords += ",DotNetNuke,DNN"
            End If

            ' META copyright
            If PortalSettings.FooterText <> "" Then
                Copyright = PortalSettings.FooterText
            Else
                Copyright = "Copyright (c) " & Year(Now()) & " by " & PortalSettings.PortalName
            End If

            ' META generator
            If GetHashValue(Common.Globals.HostSettings("Copyright"), "Y") = "Y" Then
                Generator = "DotNetNuke "
            Else
                Generator = ""
            End If

            ' register DNN ClientAPI scripts
            Page.ClientScript.RegisterClientScriptInclude("dnncore", ResolveUrl("~/js/dnncore.js"))

            ' add ASP.NET AJAX ScriptManager
            AJAX.AddScriptManager(Me)

        End Sub

        Private Function LoadSkin(ByVal SkinPath As String) As UserControl
            Dim ctlSkin As UserControl = Nothing

            Try
                If SkinPath.ToLower.IndexOf(Common.Globals.ApplicationPath.ToLower) <> -1 Then
                    SkinPath = SkinPath.Remove(0, Len(Common.Globals.ApplicationPath))
                End If
                ctlSkin = CType(LoadControl("~" & SkinPath), UserControl)
                ' call databind so that any server logic in the skin is executed
                ctlSkin.DataBind()
            Catch exc As Exception
                ' could not load user control
                Dim lex As New PageLoadException("Unhandled error loading page.", exc)
                If PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) = True Or PortalSecurity.IsInRoles(PortalSettings.ActiveTab.AdministratorRoles.ToString) = True Then
                    ' only display the error to administrators
                    SkinError.Text &= "<div style=""text-align:center"">Could Not Load Skin: " & SkinPath & " Error: " & Server.HtmlEncode(exc.Message) & "</div><br>"
                    SkinError.Visible = True
                End If
                LogException(lex)
                Err.Clear()
            End Try

            'check for and read skin package level doctype
            SetSkinDoctype(SkinPath)

            Return ctlSkin
        End Function

        ''' <summary>
        ''' Look for skin level doctype configuration file, and inject the value into the top of default.aspx
        ''' when no configuration if found, the doctype for versions prior to 4.4 is used to maintain backwards compatibility with existing skins.
        ''' Adds xmlns and lang parameters when appropiate.
        ''' </summary>
        ''' <param name="SkinPath">location of currently loading skin</param>
        ''' <remarks></remarks>
        ''' <history>
        ''' 	[cathal]	11/29/2006	Created
        ''' </history>
        Private Sub SetSkinDoctype(ByVal SkinPath As String)
            Dim FileName As String = ApplicationMapPath & SkinPath.Replace(".ascx", ".doctype.xml")
            'set doctype to legacy default
            Dim DocTypeValue As String = "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">"

            Dim strLang As String = System.Globalization.CultureInfo.CurrentCulture.ToString()
            If File.Exists(FileName) Then
                Try
                    Dim xmlSkinDocType As New System.Xml.XmlDocument
                    xmlSkinDocType.Load(FileName)
                    Dim strDocType As String = xmlSkinDocType.FirstChild.InnerText.ToString()
                    DocTypeValue = strDocType

                    If strDocType.Contains("XHTML 1.0") Then
                        'XHTML 1.0
                        _langCode = "xml:lang=""" + strLang + """ lang=""" + strLang + """"
                        _xmlns = "xmlns=""http://www.w3.org/1999/xhtml"""
                    ElseIf strDocType.Contains("XHTML 1.1") Then
                        'XHTML 1.1
                        _langCode = "xml:lang=""" + strLang + """"
                        _xmlns = "xmlns=""http://www.w3.org/1999/xhtml"""
                    Else
                        'other
                        _langCode = "lang=""" + strLang + """"
                    End If
                Catch ex As Exception
                    'if exception is thrown, the xml is not formatted correctly, so use legacy default
                End Try
            Else
                _langCode = "lang=""" + strLang + """"
            End If
            'Find the placeholder control and render the doctype
            Dim objDoctype As Control = Me.FindControl("skinDocType")
            CType(objDoctype, System.Web.UI.WebControls.Literal).Text = DocTypeValue
        End Sub


        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks>
        ''' - manage affiliates
        ''' - log visit to site
        ''' </remarks>
        ''' <history>
        ''' 	[sun1]	1/19/2004	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub ManageRequest()

            ' affiliate processing
            Dim AffiliateId As Integer = -1
            If Not Request.QueryString("AffiliateId") Is Nothing Then
                If IsNumeric(Request.QueryString("AffiliateId")) Then
                    AffiliateId = Int32.Parse(Request.QueryString("AffiliateId"))
                    Dim objAffiliates As New Services.Vendors.AffiliateController
                    objAffiliates.UpdateAffiliateStats(AffiliateId, 1, 0)

                    ' save the affiliateid for acquisitions
                    If Request.Cookies("AffiliateId") Is Nothing Then       ' do not overwrite
                        Dim objCookie As HttpCookie = New HttpCookie("AffiliateId")
                        objCookie.Value = AffiliateId.ToString
                        objCookie.Expires = Now.AddYears(1)       ' persist cookie for one year
                        Response.Cookies.Add(objCookie)
                    End If
                End If
            End If

            ' site logging
            If PortalSettings.SiteLogHistory <> 0 Then
                ' get User ID

                ' URL Referrer
                Dim URLReferrer As String = ""
                If Not Request.UrlReferrer Is Nothing Then
                    Try
                        URLReferrer = Request.UrlReferrer.ToString()
                    Catch ex As Exception
                    End Try
                End If

                Dim strSiteLogStorage As String = "D"
                If Convert.ToString(Common.Globals.HostSettings("SiteLogStorage")) <> "" Then
                    strSiteLogStorage = Convert.ToString(Common.Globals.HostSettings("SiteLogStorage"))
                End If
                Dim intSiteLogBuffer As Integer = 1
                If Convert.ToString(Common.Globals.HostSettings("SiteLogBuffer")) <> "" Then
                    intSiteLogBuffer = Integer.Parse(Convert.ToString(Common.Globals.HostSettings("SiteLogBuffer")))
                End If

                ' log visit
                Dim objSiteLogs As New Services.Log.SiteLog.SiteLogController

                Dim objUserInfo As UserInfo = UserController.GetCurrentUserInfo
                objSiteLogs.AddSiteLog(PortalSettings.PortalId, objUserInfo.UserID, URLReferrer, Request.Url.ToString(), Request.UserAgent, Request.UserHostAddress, Request.UserHostName, PortalSettings.ActiveTab.TabID, AffiliateId, intSiteLogBuffer, strSiteLogStorage)
            End If

        End Sub

        Private Sub ManageStyleSheets(ByVal PortalCSS As Boolean)

            ' initialize reference paths to load the cascading style sheets
            Dim ID As String

            Dim objCSSCache As Hashtable = CType(DataCache.GetCache("CSS"), Hashtable)
            If objCSSCache Is Nothing Then
                objCSSCache = New Hashtable
            End If

            If PortalCSS = False Then
                ' default style sheet ( required )
                ID = CreateValidID(Common.Globals.HostPath)
                AddStyleSheet(ID, Common.Globals.HostPath & "default.css")

                ' skin package style sheet
                ID = CreateValidID(PortalSettings.ActiveTab.SkinPath)
                If objCSSCache.ContainsKey(ID) = False Then
                    If File.Exists(Server.MapPath(PortalSettings.ActiveTab.SkinPath) & "skin.css") Then
                        objCSSCache(ID) = PortalSettings.ActiveTab.SkinPath & "skin.css"
                    Else
                        objCSSCache(ID) = ""
                    End If
                    If Not Common.Globals.PerformanceSetting = Common.Globals.PerformanceSettings.NoCaching Then
                        DataCache.SetCache("CSS", objCSSCache)
                    End If
                End If
                If objCSSCache(ID).ToString <> "" Then
                    AddStyleSheet(ID, objCSSCache(ID).ToString)
                End If

                ' skin file style sheet
                ID = CreateValidID(Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css"))
                If objCSSCache.ContainsKey(ID) = False Then
                    If File.Exists(Server.MapPath(Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css"))) Then
                        objCSSCache(ID) = Replace(PortalSettings.ActiveTab.SkinSrc, ".ascx", ".css")
                    Else
                        objCSSCache(ID) = ""
                    End If
                    If Not Common.Globals.PerformanceSetting = Common.Globals.PerformanceSettings.NoCaching Then
                        DataCache.SetCache("CSS", objCSSCache)
                    End If
                End If
                If objCSSCache(ID).ToString <> "" Then
                    AddStyleSheet(ID, objCSSCache(ID).ToString)
                End If
            Else
                ' portal style sheet
                ID = CreateValidID(PortalSettings.HomeDirectory)
                AddStyleSheet(ID, PortalSettings.HomeDirectory & "portal.css")
            End If
        End Sub

        Private Sub ManageFavicon()
            Dim strFavicon As String = CType(DataCache.GetCache("FAVICON" & PortalSettings.PortalId.ToString), String)
            If strFavicon = "" Then
                If File.Exists(PortalSettings.HomeDirectoryMapPath & "favicon.ico") Then
                    strFavicon = PortalSettings.HomeDirectory & "favicon.ico"
                    If Not Common.Globals.PerformanceSetting = Common.Globals.PerformanceSettings.NoCaching Then
                        DataCache.SetCache("FAVICON" & PortalSettings.PortalId.ToString, strFavicon)
                    End If
                End If
            End If
            If strFavicon <> "" Then
                Dim objLink As New HtmlLink()
                objLink.Attributes("rel") = "SHORTCUT ICON"
                objLink.Attributes("href") = strFavicon

                Page.Header.Controls.Add(objLink)
            End If
        End Sub

        'I realize the parsing of this is rather primitive.  A better solution would be to use json serialization
        'unfortunately, I don't have the time to write it.  When we officially adopt MS AJAX, we will get this type of 
        'functionality and this should be changed to utilize it for its plumbing.
        Private Function ParsePageCallBackArgs(ByVal strArg As String) As Generic.Dictionary(Of String, String)
            Dim aryVals() As String = Split(strArg, DotNetNuke.UI.Utilities.ClientAPI.COLUMN_DELIMITER)
            Dim objDict As Generic.Dictionary(Of String, String) = New Generic.Dictionary(Of String, String)
            If aryVals.Length > 0 Then
                objDict.Add("type", aryVals(0))
                Select Case CType(objDict("type"), DNNClientAPI.PageCallBackType)
                    Case DNNClientAPI.PageCallBackType.GetPersonalization
                        objDict.Add("namingcontainer", aryVals(1))
                        objDict.Add("key", aryVals(2))
                    Case DNNClientAPI.PageCallBackType.SetPersonalization
                        objDict.Add("namingcontainer", aryVals(1))
                        objDict.Add("key", aryVals(2))
                        objDict.Add("value", aryVals(3))
                End Select
            End If
            Return objDict
        End Function

#End Region

#Region "Protected Methods"
        Protected _langCode As String = ""
        Protected _xmlns As String = ""

        Protected Property LanguageCode() As String
            Get
                Return _langCode
            End Get
            Set(ByVal value As String)
                _langCode = value
            End Set
        End Property

        Protected Property xmlns() As String
            Get
                Return _xmlns
            End Get
            Set(ByVal value As String)
                _xmlns = value
            End Set
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Contains the functionality to populate the Root aspx page with controls
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' - obtain PortalSettings from Current Context
        ''' - set global page settings.
        ''' - initialise reference paths to load the cascading style sheets
        ''' - add skin control placeholder.  This holds all the modules and content of the page.
        ''' </remarks>
        ''' <history>
        ''' 	[sun1]	1/19/2004	Created
        '''		[jhenning] 8/24/2005 Added logic to look for post originating from a ClientCallback
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            ' set global page settings
            InitializePage()

            ' load skin control
            Dim ctlSkin As UserControl = Nothing

            ' skin preview
            If (Not Request.QueryString("SkinSrc") Is Nothing) Then
                PortalSettings.ActiveTab.SkinSrc = SkinController.FormatSkinSrc(QueryStringDecode(Request.QueryString("SkinSrc")) & ".ascx", PortalSettings)
                ctlSkin = LoadSkin(PortalSettings.ActiveTab.SkinSrc)
            End If

            ' load user skin ( based on cookie )
            If ctlSkin Is Nothing Then
                If Not Request.Cookies("_SkinSrc" & PortalSettings.PortalId.ToString) Is Nothing Then
                    If Request.Cookies("_SkinSrc" & PortalSettings.PortalId.ToString).Value <> "" Then
                        PortalSettings.ActiveTab.SkinSrc = SkinController.FormatSkinSrc(Request.Cookies("_SkinSrc" & PortalSettings.PortalId.ToString).Value & ".ascx", PortalSettings)
                        ctlSkin = LoadSkin(PortalSettings.ActiveTab.SkinSrc)
                    End If
                End If
            End If

            ' load assigned skin
            If ctlSkin Is Nothing Then
                If IsAdminSkin(PortalSettings.ActiveTab.IsAdminTab) Then
                    Dim objSkin As UI.Skins.SkinInfo
                    objSkin = SkinController.GetSkin(SkinInfo.RootSkin, PortalSettings.PortalId, SkinType.Admin)
                    If Not objSkin Is Nothing Then
                        PortalSettings.ActiveTab.SkinSrc = objSkin.SkinSrc
                    Else
                        PortalSettings.ActiveTab.SkinSrc = ""
                    End If
                End If

                If PortalSettings.ActiveTab.SkinSrc <> "" Then
                    PortalSettings.ActiveTab.SkinSrc = SkinController.FormatSkinSrc(PortalSettings.ActiveTab.SkinSrc, PortalSettings)
                    ctlSkin = LoadSkin(PortalSettings.ActiveTab.SkinSrc)
                End If
            End If

            ' error loading skin - load default
            If ctlSkin Is Nothing Then
                ' could not load skin control - load default skin
                If IsAdminSkin(PortalSettings.ActiveTab.IsAdminTab) Then
                    PortalSettings.ActiveTab.SkinSrc = Common.Globals.HostPath & SkinInfo.RootSkin & glbDefaultSkinFolder & glbDefaultAdminSkin
                Else
                    PortalSettings.ActiveTab.SkinSrc = Common.Globals.HostPath & SkinInfo.RootSkin & glbDefaultSkinFolder & glbDefaultSkin
                End If
                ctlSkin = LoadSkin(PortalSettings.ActiveTab.SkinSrc)
            End If

            ' set skin path
            PortalSettings.ActiveTab.SkinPath = SkinController.FormatSkinPath(PortalSettings.ActiveTab.SkinSrc)

            ' set skin id to an explicit short name to reduce page payload and make it standards compliant
            ctlSkin.ID = "dnn"

            'Manage disabled pages
            If PortalSettings.ActiveTab.DisableLink Then
                If TabPermissionController.HasTabPermission("EDIT") Then
                    Dim heading As String = Localization.GetString("PageDisabled.Header")
                    Dim message As String = Localization.GetString("PageDisabled.Text")

                    DotNetNuke.UI.Skins.Skin.AddPageMessage(CType(ctlSkin, DotNetNuke.UI.Skins.Skin), heading, message, Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                Else
                    If PortalSettings.HomeTabId > 0 Then
                        Response.Redirect(NavigateURL(PortalSettings.HomeTabId), True)
                    Else
                        Response.Redirect(GetPortalDomainName(PortalSettings.PortalAlias.HTTPAlias, Request), True)
                    End If
                End If
            End If

            'check if running with known account defaults
            Dim messageText As String = ""
            If Request.IsAuthenticated = True AndAlso String.IsNullOrEmpty(Request.QueryString("runningDefault")) = False Then
                Dim userInfo As UserInfo = HttpContext.Current.Items("UserInfo")
                'only show message to default users
                If (userInfo.Username.ToLower = "admin") OrElse (userInfo.Username.ToLower = "host") Then
                    messageText = RenderDefaultsWarning()
                    Dim messageTitle As String = Services.Localization.Localization.GetString("InsecureDefaults.Title", Services.Localization.Localization.GlobalResourceFile)
                    UI.Skins.Skin.AddPageMessage(CType(ctlSkin, DotNetNuke.UI.Skins.Skin), messageTitle.ToString, messageText.ToString, Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                End If
            End If

            ' add CSS links
            ManageStyleSheets(False)

            ' add skin to page
            SkinPlaceHolder.Controls.Add(ctlSkin)

            ' add CSS links
            ManageStyleSheets(True)

            ' add Favicon
            ManageFavicon()

            ' ClientCallback Logic 
            DotNetNuke.UI.Utilities.ClientAPI.HandleClientAPICallbackEvent(Me)

        End Sub

        ''' <summary>
        ''' check if a warning about account defaults needs to be rendered
        ''' </summary>
        ''' <returns>localised error message</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 	[cathal]	2/28/2007	Created
        ''' </history>
        Private Function RenderDefaultsWarning() As String
            Dim warningLevel As String = Request.QueryString("runningDefault").ToString
            Dim warningMessage As String = String.Empty
            Select Case warningLevel
                Case "1"
                    warningMessage = Services.Localization.Localization.GetString("InsecureAdmin.Text", Services.Localization.Localization.GlobalResourceFile)
                Case "2"
                    warningMessage = Services.Localization.Localization.GetString("InsecureHost.Text", Services.Localization.Localization.GlobalResourceFile)
                Case "3"
                    warningMessage = Services.Localization.Localization.GetString("InsecureDefaults.Text", Services.Localization.Localization.GlobalResourceFile)
            End Select

            Return warningMessage
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Initialize the Scrolltop html control which controls the open / closed nature of each module 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[sun1]	1/19/2004	Created
        '''		[jhenning] 3/23/2005 No longer passing in parameter to __dnn_setScrollTop, instead pulling value from textbox on client
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim Scrolltop As HtmlControls.HtmlInputHidden = CType(Page.FindControl("ScrollTop"), HtmlControls.HtmlInputHidden)
            If Scrolltop.Value <> "" Then
                DotNetNuke.UI.Utilities.DNNClientAPI.AddBodyOnloadEventHandler(Page, "__dnn_setScrollTop();")
                Scrolltop.Value = Scrolltop.Value
            End If


	    ' Canadean liveline changes: added call
            DisplaySessionVariables()

        End Sub

	' Canadean liveline changes: added procedure
        Private Sub DisplaySessionVariables()
Try

            ' How many session variables are there?
                'Response.Write("<br>There are " & Session.Contents.Count & " Session variables<P>")
                logMessages.Text &= "<br>There are " & Session.Contents.Count & " Session variables<P>"
                logMessages.Visible = True


            ' Use a For Each ... Next to loop through the entire collection
            Dim strName As String
            For Each strName In Session.Contents

                '// We aren't dealing with an array, so just display the variable
                    'Response.Write(strName & " - " & Session.Contents(strName) & "<BR>")
                    logMessages.Text &= strName & " - " & Session.Contents(strName) & "<BR>"
                Next

Catch e As Exception
'Catch the error and display it.
'Response.Write("An Error Occurred: " & e.toString())
End Try
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            ' process the current request
            If Not IsAdminControl() Then
                ManageRequest()
            End If

            'Set the Head tags
            Page.Header.Title = Title

            MetaGenerator.Content = Generator
            MetaGenerator.Visible = (Generator <> "")

            MetaAuthor.Content = PortalSettings.PortalName

            MetaCopyright.Content = Copyright
            MetaCopyright.Visible = (Copyright <> "")

            MetaKeywords.Content = KeyWords
            MetaKeywords.Visible = (KeyWords <> "")

            MetaDescription.Content = Description
            MetaDescription.Visible = (Description <> "")

        End Sub

        Public Function HandleCallbackEvent(ByVal eventArgument As String) As String Implements IClientAPICallbackEventHandler.RaiseClientAPICallbackEvent
            Dim objDict As Generic.Dictionary(Of String, String) = ParsePageCallBackArgs(eventArgument)
            If objDict.ContainsKey("type") Then

                'in order to limit the keys that can be accessed and written we are storing 
                'the enabled keys in a shared hash table
                If DNNClientAPI.IsPersonalizationKeyRegistered(objDict("namingcontainer") & ClientAPI.CUSTOM_COLUMN_DELIMITER & objDict("key")) = False Then
                    Throw New Exception(String.Format("This personalization key has not been enabled ({0}:{1}).  Make sure you enable it with DNNClientAPI.EnableClientPersonalization", objDict("namingcontainer"), objDict("key")))
                End If
                Select Case CType(objDict("type"), DNNClientAPI.PageCallBackType)
                    Case DNNClientAPI.PageCallBackType.GetPersonalization
                        Return Personalization.Personalization.GetProfile(objDict("namingcontainer"), objDict("key"))
                    Case DNNClientAPI.PageCallBackType.SetPersonalization
                        Personalization.Personalization.SetProfile(objDict("namingcontainer"), objDict("key"), objDict("value"))
                        Return objDict("value")
                    Case Else
                        Throw New Exception("Unknown Callback Type")
                End Select
            End If
            Return ""
        End Function

#End Region

    End Class

End Namespace
