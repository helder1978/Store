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

Imports System.Collections.Specialized
Imports System.Threading
Imports System.Web.Security
Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Common.Controls

    Partial Class Logoff
        Inherits Framework.PageBase

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the Redirect URL after the user logs out
        ''' </summary>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property RedirectURL() As String
            Get
                Dim _RedirectURL As String = ""

                ' check if anonymous users have access to the current page
                If PortalSettings.ActiveTab.AuthorizedRoles.IndexOf(";" & glbRoleAllUsersName & ";") <> -1 Then
                    ' redirect to current page
                    _RedirectURL = NavigateURL(PortalSettings.ActiveTab.TabID)
                Else ' redirect to a different page
                    Dim setting As Object = UserModuleBase.GetSetting(PortalSettings.PortalId, "Redirect_AfterLogout")

                    If CType(setting, Integer) = Null.NullInteger Then
                        If PortalSettings.HomeTabId <> -1 Then
                            ' redirect to portal home page specified
                            _RedirectURL = NavigateURL(PortalSettings.HomeTabId)
                        Else ' redirect to default portal root
                            _RedirectURL = GetPortalDomainName(PortalSettings.PortalAlias.HTTPAlias, Request) & "/" & glbDefaultPage
                        End If
                    Else ' redirect to after logout page
                        _RedirectURL = NavigateURL(CType(setting, Integer))
                    End If
                End If

                '_RedirectURL = "/" ' canadean changed
                Return _RedirectURL

            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/23/2006  Documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                'Remove user from cache
                If Me.User IsNot Nothing Then
                    DataCache.ClearUserCache(Me.PortalSettings.PortalId, Context.User.Identity.Name)
                End If

                Dim objPortalSecurity As New PortalSecurity
                objPortalSecurity.SignOut()

                ' canadean changed: clear session 
                Session.Clear()

                ' Redirect browser back to portal 
                Response.Redirect(RedirectURL, True)

            Catch exc As Exception    'Page failed to load
                ProcessPageLoadException(exc)
            End Try
        End Sub

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