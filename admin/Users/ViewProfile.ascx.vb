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
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Modules.Admin.Security
Imports DotNetNuke.Security.Profile
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Mail
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.UI.WebControls

Namespace DotNetNuke.Modules.Admin.Users

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The ViewProfile UserModuleBase is used to view a Users Profile
    ''' </summary>
    ''' <history>
    ''' 	[cnurse]	05/02/2006   Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class ViewProfile
        Inherits UserModuleBase

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Init runs when the control is initialised
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/01/2006
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            ' get userid from encrypted ticket
            UserId = Null.NullInteger
            If Not Context.Request.QueryString("userticket") Is Nothing Then
                UserId = Int32.Parse(UrlUtils.DecryptParameter(Context.Request.QueryString("userticket")))
            End If

            ctlProfile.ID = "Profile"
            ctlProfile.UserId = UserId

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/01/2006
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                'Before we bind the Profile to the editor we need to "update" the visibility data
                Dim properties As ProfilePropertyDefinitionCollection = ctlProfile.UserProfile.ProfileProperties

                For Each profProperty As ProfilePropertyDefinition In properties
                    If profProperty.Visible Then
                        'Check Visibility
                        If profProperty.Visibility = UserVisibilityMode.AdminOnly Then
                            'Only Visible if Admin (or self)
                            profProperty.Visible = (IsAdmin Or IsUser)
                        ElseIf profProperty.Visibility = UserVisibilityMode.MembersOnly Then
                            'Only Visible if Is a Member (ie Authenticated)
                            profProperty.Visible = Request.IsAuthenticated
                        End If
                    End If
                Next

                'Bind the profile information to the control
                ctlProfile.DataBind()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

    End Class

End Namespace