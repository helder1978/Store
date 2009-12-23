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

Imports System.Web.Security

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Services.Mail
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage
Imports DotNetNuke.UI.WebControls

Namespace DotNetNuke.Modules.Admin.Security

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The SendPassword UserModuleBase is used to allow a user to retrieve their password
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	03/21/2006  Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class SendPassword
        Inherits UserModuleBase

#Region "Private Members"

        Private ipAddress As String

#End Region

#Region "Protected Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets whether the Captcha control is used to validate the login
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/21/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property UseCaptcha() As Boolean
            Get
                Dim setting As Object = UserModuleBase.GetSetting(PortalId, "Security_CaptchaLogin")
                Return CType(setting, Boolean)
            End Get
        End Property

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' AddLocalizedModuleMessage adds a localized module message
        ''' </summary>
        ''' <param name="message">The localized message</param>
        ''' <param name="type">The type of message</param>
        ''' <param name="display">A flag that determines whether the message should be displayed</param>
        ''' <history>
        ''' 	[cnurse]	03/21/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub AddLocalizedModuleMessage(ByVal message As String, ByVal type As ModuleMessageType, ByVal display As Boolean)

            If display Then
                UI.Skins.Skin.AddModuleMessage(Me, message, type)
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' AddModuleMessage adds a module message
        ''' </summary>
        ''' <param name="message">The message</param>
        ''' <param name="type">The type of message</param>
        ''' <param name="display">A flag that determines whether the message should be displayed</param>
        ''' <history>
        ''' 	[cnurse]	03/21/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub AddModuleMessage(ByVal message As String, ByVal type As ModuleMessageType, ByVal display As Boolean)

            AddLocalizedModuleMessage(Localization.GetString(message, LocalResourceFile), type, display)

        End Sub

        Private Function GetUser() As UserInfo

            Dim objUser As UserInfo = Nothing
            Dim arrUsers As ArrayList
            Dim noRecords As Integer

            If MembershipProviderConfig.RequiresUniqueEmail AndAlso _
                        Trim(txtEmail.Text) <> "" AndAlso Trim(txtUsername.Text) = "" Then
                arrUsers = UserController.GetUsersByEmail(PortalSettings.PortalId, txtEmail.Text, 0, Int32.MaxValue, noRecords)
                If Not arrUsers Is Nothing AndAlso arrUsers.Count = 1 Then
                    objUser = arrUsers(0)
                End If
            Else
                objUser = UserController.GetUserByName(PortalSettings.PortalId, txtUsername.Text, False)
            End If

            Return objUser

        End Function

#End Region

#Region "Public Methods"

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Init runs when the control is initialised
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/21/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/21/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If Not Request.UserHostAddress Is Nothing Then
                ipAddress = Request.UserHostAddress
            End If

            rowEmailLabel.Visible = MembershipProviderConfig.RequiresUniqueEmail
            rowEmailText.Visible = MembershipProviderConfig.RequiresUniqueEmail

            trCaptcha1.Visible = UseCaptcha
            trCaptcha2.Visible = UseCaptcha

            If UseCaptcha Then
                ctlCaptcha.ErrorMessage = Localization.GetString("InvalidCaptcha", Me.LocalResourceFile)
                ctlCaptcha.Text = Localization.GetString("CaptchaText", Me.LocalResourceFile)
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdSendPassword_Click runs when the Password Reminder button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/21/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdSendPassword_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSendPassword.Click

            Dim strMessage As String = Null.NullString
            Dim strLogMessage As String = Null.NullString
            Dim canSend As Boolean = True
            Dim objUser As UserInfo = Nothing

            If MembershipProviderConfig.RequiresQuestionAndAnswer And txtAnswer.Text = "" Then
                objUser = GetUser()
                If Not objUser Is Nothing Then
                    lblQuestion.Text = objUser.Membership.PasswordQuestion
                End If
                tblQA.Visible = True
                Exit Sub
            End If

            If (UseCaptcha And ctlCaptcha.IsValid) OrElse (Not UseCaptcha) Then

                If Trim(txtUsername.Text) = "" Then
                    'No UserName provided
                    If MembershipProviderConfig.RequiresUniqueEmail Then
                        If Trim(txtEmail.Text) = "" Then
                            'No email address either (cannot retrieve password)
                            canSend = False
                            strMessage = Services.Localization.Localization.GetString("EnterUsernameEmail", Me.LocalResourceFile)
                        End If
                    Else
                        'Cannot retreeve password
                        canSend = False
                        strMessage = Services.Localization.Localization.GetString("EnterUsername", Me.LocalResourceFile)
                    End If
                End If

                If canSend Then
                    Dim objSecurity As New PortalSecurity
                    objUser = GetUser()

                    If Not objUser Is Nothing Then
                        If MembershipProviderConfig.PasswordRetrievalEnabled Then
                            Try
                                objUser.Membership.Password = UserController.GetPassword(objUser, txtAnswer.Text)
                            Catch ex As Exception
                                canSend = False
                                strMessage = Localization.GetString("PasswordRetrievalError", Me.LocalResourceFile)
                            End Try
                        Else
                            canSend = False
                            strMessage = Localization.GetString("PasswordRetrievalDisabled", Me.LocalResourceFile)
                        End If
                        If canSend Then
                            Try
                                If Mail.SendMail(objUser, MessageType.PasswordReminder, PortalSettings) <> String.Empty Then
                                    strMessage = Localization.GetString("SendMailError", Me.LocalResourceFile)
                                    canSend = False
                                Else
                                    strMessage = Localization.GetString("PasswordSent", Me.LocalResourceFile)
                                End If
                            Catch ex As Exception
                                canSend = False
                            End Try
                        End If
                    Else
                        If MembershipProviderConfig.RequiresUniqueEmail AndAlso _
                                    Trim(txtEmail.Text) <> "" AndAlso Trim(txtUsername.Text) = "" Then
                            strMessage = Localization.GetString("EmailError", Me.LocalResourceFile)
                        Else
                            strMessage = Localization.GetString("UsernameError", Me.LocalResourceFile)
                        End If

                        canSend = False
                        End If

                        If canSend Then
                            Dim objEventLog As New Services.Log.EventLog.EventLogController
                            Dim objEventLogInfo As New Services.Log.EventLog.LogInfo
                            objEventLogInfo.AddProperty("IP", ipAddress)
                            objEventLogInfo.LogPortalID = PortalSettings.PortalId
                            objEventLogInfo.LogPortalName = PortalSettings.PortalName
                            objEventLogInfo.LogUserID = UserId
                            objEventLogInfo.LogUserName = objSecurity.InputFilter(txtUsername.Text, PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoAngleBrackets Or PortalSecurity.FilterFlag.NoMarkup)
                            objEventLogInfo.LogTypeKey = "PASSWORD_SENT_SUCCESS"
                            objEventLog.AddLog(objEventLogInfo)

                            UI.Skins.Skin.AddModuleMessage(Me, strMessage, UI.Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess)
                        Else
                            Dim objEventLog As New Services.Log.EventLog.EventLogController
                            Dim objEventLogInfo As New Services.Log.EventLog.LogInfo
                            objEventLogInfo.AddProperty("IP", ipAddress)
                            objEventLogInfo.LogPortalID = PortalSettings.PortalId
                            objEventLogInfo.LogPortalName = PortalSettings.PortalName
                            objEventLogInfo.LogUserID = UserId
                            objEventLogInfo.LogUserName = objSecurity.InputFilter(txtUsername.Text, PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoAngleBrackets Or PortalSecurity.FilterFlag.NoMarkup)
                            objEventLogInfo.LogTypeKey = "PASSWORD_SENT_FAILURE"
                            objEventLogInfo.LogProperties.Add(New LogDetailInfo("Cause", strMessage))
                            objEventLog.AddLog(objEventLogInfo)

                            UI.Skins.Skin.AddModuleMessage(Me, strMessage, UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                        End If
                Else
                        UI.Skins.Skin.AddModuleMessage(Me, strMessage, UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                End If
            End If
        End Sub

#End Region

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

#End Region

    End Class

End Namespace
