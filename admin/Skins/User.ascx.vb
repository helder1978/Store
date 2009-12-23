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

Imports System.Web.UI.UserControl
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.UI.Skins.Controls

    ''' -----------------------------------------------------------------------------
    ''' <summary></summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 	[cniknet]	10/15/2004	Replaced public members with properties and removed
    '''                             brackets from property names
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial  Class User
        Inherits UI.Skins.SkinObjectBase

#Region "Controls"


#End Region

#Region "Constants"

        Const MyFileName As String = "User.ascx"

#End Region

#Region "Private Members"

        Private _text As String
        Private _cssClass As String
        Private _url As String

#End Region

#Region "Properties"

        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal Value As String)
                _text = Value
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

        Public Property URL() As String
            Get
                Return _url
            End Get
            Set(ByVal Value As String)
                _url = Value
            End Set
        End Property

#End Region

#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                ' public attributes
                If CssClass <> "" Then
                    cmdRegister.CssClass = CssClass
                End If

                If Request.IsAuthenticated = False Then
                    If PortalSettings.UserRegistration <> PortalRegistrationType.NoRegistration Then
                        If Text <> "" Then
                            If Text.IndexOf("src=") <> -1 Then
                                Text = Replace(Text, "src=""", "src=""" & PortalSettings.ActiveTab.SkinPath)
                            End If
                            cmdRegister.Text = Text
                        Else
                            cmdRegister.Text = Localization.GetString("Register", Localization.GetResourceFile(Me, MyFileName))
                        End If
                        If PortalSettings.Users < PortalSettings.UserQuota Or PortalSettings.UserQuota = 0 Then
                            cmdRegister.Visible = True
                        Else
                            cmdRegister.Visible = False
                        End If
                    Else
                        cmdRegister.Visible = False
                    End If
                Else
                    Dim objUserInfo As UserInfo = UserController.GetCurrentUserInfo
                    If objUserInfo.UserID <> -1 Then
                        cmdRegister.Text = objUserInfo.DisplayName
                        cmdRegister.ToolTip = Localization.GetString("ToolTip", Localization.GetResourceFile(Me, MyFileName))
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdRegister_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRegister.Click
            Try
                If Request.IsAuthenticated = False Then
                    If PortalSettings.UserRegistration <> PortalRegistrationType.NoRegistration Then
                        If URL <> "" Then
                            Response.Redirect(URL, True)
                        Else
                            Dim ReturnUrl As String = HttpContext.Current.Request.RawUrl
                            If ReturnUrl.IndexOf("?returnurl=") <> -1 Then
                                ReturnUrl = ReturnUrl.Substring(0, ReturnUrl.IndexOf("?returnurl="))
                            End If
                            ReturnUrl = HttpUtility.UrlEncode(ReturnUrl)

                            If PortalSettings.UserTabId <> -1 Then
                                ' user defined tab
                                Response.Redirect(NavigateURL(PortalSettings.UserTabId, "", "returnurl=" & ReturnUrl), True)
                            Else
                                ' portal tab
                                If PortalSettings.HomeTabId <> -1 Then
                                    Response.Redirect(NavigateURL(PortalSettings.HomeTabId, "Register", "returnurl=" & ReturnUrl), True)
                                Else
                                    Response.Redirect(NavigateURL("Register", "returnurl=" & ReturnUrl), True)
                                End If
                            End If
                        End If
                    End If
                Else
                    Dim objUserInfo As UserInfo = UserController.GetCurrentUserInfo
                    If objUserInfo.UserID <> -1 Then
                        If PortalSettings.UserTabId <> -1 Then
                            Response.Redirect(NavigateURL(PortalSettings.UserTabId), True)
                        Else
                            Response.Redirect(NavigateURL(PortalSettings.ActiveTab.TabID, "Profile", "UserID=" + objUserInfo.UserID.ToString), True)
                        End If
                    End If
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

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

    End Class

End Namespace
