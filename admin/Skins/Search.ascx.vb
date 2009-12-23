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
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Host

Namespace DotNetNuke.UI.Skins.Controls
	''' -----------------------------------------------------------------------------
	''' <summary></summary>
    ''' <remarks></remarks>
	''' <history>
	''' 	[cniknet]	10/15/2004	Replaced public members with properties and removed
	'''                             brackets from property names
	''' </history>
	''' -----------------------------------------------------------------------------


	Partial  Class Search

		Inherits UI.Skins.SkinObjectBase

		' private members
		Private _submit As String
		Private _cssClass As String
        Private _showSite As Boolean = True
        Private _siteText As String
        Private _siteToolTip As String
        Private _siteURL As String
        Private _showWeb As Boolean = True
        Private _webText As String
        Private _webToolTip As String
        Private _webURL As String

		Const MyFileName As String = "Search.ascx"

		' protected controls

#Region "Public Members"
		Public Property Submit() As String
			Get
				Return _submit
			End Get
			Set(ByVal Value As String)
				_submit = Value
			End Set
		End Property

		Public Property CssClass() As String
			Get
				Return _cssClass
			End Get
			Set(ByVal Value As String)
				_cssClass = Value
			End Set
        End Property

        Public Property ShowSite() As Boolean
            Get
                Return _showSite
            End Get
            Set(ByVal Value As Boolean)
                _showSite = Value
            End Set
        End Property

        Public Property SiteText() As String
            Get
                Return _siteText
            End Get
            Set(ByVal Value As String)
                _siteText = Value
            End Set
        End Property

        Public Property SiteToolTip() As String
            Get
                Return _siteToolTip
            End Get
            Set(ByVal Value As String)
                _siteToolTip = Value
            End Set
        End Property

        Public Property SiteURL() As String
            Get
                Return _siteURL
            End Get
            Set(ByVal Value As String)
                _siteURL = Value
            End Set
        End Property

        Public Property ShowWeb() As Boolean
            Get
                Return _showWeb
            End Get
            Set(ByVal Value As Boolean)
                _showWeb = Value
            End Set
        End Property

        Public Property WebText() As String
            Get
                Return _webText
            End Get
            Set(ByVal Value As String)
                _webText = Value
            End Set
        End Property

        Public Property WebToolTip() As String
            Get
                Return _webToolTip
            End Get
            Set(ByVal Value As String)
                _webToolTip = Value
            End Set
        End Property

        Public Property WebURL() As String
            Get
                Return _webURL
            End Get
            Set(ByVal Value As String)
                _webURL = Value
            End Set
        End Property

#End Region

#Region " Web Form Designer Generated Code "


		<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

		End Sub

		Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			'CODEGEN: This method call is required by the Web Form Designer
			'Do not modify it using the code editor.
			InitializeComponent()
		End Sub

#End Region

		'*******************************************************
		'
		' The Page_Load server event handler on this page is used
		' to populate the role information for the page
		'
		'*******************************************************

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not Page.IsPostBack Then
                If WebText <> "" Then
                    optWeb.Text = WebText
                Else
                    optWeb.Text = Services.Localization.Localization.GetString("Web", Services.Localization.Localization.GetResourceFile(Me, MyFileName))
                End If
                If WebToolTip <> "" Then
                    optWeb.ToolTip = WebToolTip
                Else
                    optWeb.ToolTip = Services.Localization.Localization.GetString("Web.ToolTip", Services.Localization.Localization.GetResourceFile(Me, MyFileName))
                End If
                If SiteText <> "" Then
                    optSite.Text = SiteText
                Else
                    optSite.Text = Services.Localization.Localization.GetString("Site", Services.Localization.Localization.GetResourceFile(Me, MyFileName))
                End If
                If SiteToolTip <> "" Then
                    optSite.ToolTip = SiteToolTip
                Else
                    optSite.ToolTip = Services.Localization.Localization.GetString("Site.ToolTip", Services.Localization.Localization.GetResourceFile(Me, MyFileName))
                End If

                optWeb.Visible = ShowWeb
                optSite.Visible = ShowSite
                If optWeb.Visible Then
                    optWeb.Checked = True
                End If
                If optSite.Visible Then
                    optSite.Checked = True
                End If

                ClientAPI.RegisterKeyCapture(Me.txtSearch, Me.cmdSearch, Asc(vbCr))

                If Not Request.QueryString("Search") Is Nothing Then
                    txtSearch.Text = Request.QueryString("Search").ToString
                End If

                If Submit <> "" Then
                    If Submit.IndexOf("src=") <> -1 Then
                        Submit = Replace(Submit, "src=""", "src=""" & PortalSettings.ActiveTab.SkinPath)
                    End If
                Else
                    Submit = Services.Localization.Localization.GetString("Search", Services.Localization.Localization.GetResourceFile(Me, MyFileName))
                End If
                cmdSearch.Text = Submit

                If CssClass <> "" Then
                    optWeb.CssClass = CssClass
                    optSite.CssClass = CssClass
                    cmdSearch.CssClass = CssClass
                End If
            End If

        End Sub

		Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

            Dim strSearch As String = "S"
            If optWeb.Visible Then
                If optWeb.Checked Then
                    strSearch = "W"
                End If
            End If

            If txtSearch.Text <> "" Then
                Select Case strSearch
                    Case "S" ' site
                        If SiteURL <> "" Then
                            Dim strURL As String = SiteURL
                            If strURL = "" Then
                                strURL = Services.Localization.Localization.GetString("URL", Services.Localization.Localization.GetResourceFile(Me, MyFileName))
                            End If
                            If strURL <> "" Then
                                strURL = strURL.Replace("[TEXT]", Server.UrlEncode(txtSearch.Text))
                                strURL = strURL.Replace("[DOMAIN]", Request.Url.Host)
                                UrlUtils.OpenNewWindow(strURL)
                            End If
                        Else
                            Dim objModules As New ModuleController
                            Dim searchTabId As Integer
                            Dim SearchModule As ModuleInfo = objModules.GetModuleByDefinition(PortalSettings.PortalId, "Search Results")
                            If SearchModule Is Nothing Then
                                Return
                            Else
                                searchTabId = SearchModule.TabID
                            End If
                            If HostSettings.GetHostSetting("UseFriendlyUrls") = "Y" Then
                                Response.Redirect(NavigateURL(searchTabId) & "?Search=" & Server.UrlEncode(txtSearch.Text))
                            Else
                                Response.Redirect(NavigateURL(searchTabId) & "&Search=" & Server.UrlEncode(txtSearch.Text))
                            End If
                        End If
                    Case "W" ' web
                        Dim strURL As String = WebURL
                        If strURL = "" Then
                            strURL = Services.Localization.Localization.GetString("URL", Services.Localization.Localization.GetResourceFile(Me, MyFileName))
                        End If
                        If strURL <> "" Then
                            strURL = strURL.Replace("[TEXT]", Server.UrlEncode(txtSearch.Text))
                            strURL = strURL.Replace("[DOMAIN]", "")
                            UrlUtils.OpenNewWindow(strURL)
                        End If
                End Select
            End If

		End Sub

    End Class

End Namespace
