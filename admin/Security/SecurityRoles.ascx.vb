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
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Mail
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.UI.WebControls

Namespace DotNetNuke.Modules.Admin.Security

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The SecurityRoles PortalModuleBase is used to manage the users and roles they
    ''' have
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
    '''                       and localisation
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class SecurityRoles
        Inherits PortalModuleBase
        Implements IActionable

#Region "Private Members"

        Private _ParentModule As PortalModuleBase

        Private RoleId As Integer = -1
        Private Shadows UserId As Integer = -1

#End Region

#Region "Protected Members"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the Return Url for the page
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/14/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property ReturnUrl() As String
            Get
                Return NavigateURL(TabId)
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the control should use a Combo Box or Text Box to display the users
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	05/01/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property UsersControl() As UsersControl
            Get
                Dim setting As Object = UserModuleBase.GetSetting(PortalId, "Security_UsersControl")
                Return CType(setting, UsersControl)
            End Get
        End Property


#End Region

#Region "Public Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the ParentModule (if one exists)
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/10/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property ParentModule() As PortalModuleBase
            Get
                Return _ParentModule
            End Get
            Set(ByVal Value As PortalModuleBase)
                _ParentModule = Value
            End Set
        End Property

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindData loads the controls from the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindData()

            Dim objRoles As New RoleController

            ' bind all portal roles to dropdownlist
            If RoleId = -1 Then
                If cboRoles.Items.Count = 0 Then
                    cboRoles.DataSource = objRoles.GetPortalRoles(PortalId)
                    cboRoles.DataBind()
                End If
            Else
                If Not Page.IsPostBack Then
                    Dim objRole As RoleInfo = objRoles.GetRole(RoleId, PortalId)
                    If Not objRole Is Nothing Then
                        cboRoles.Items.Add(New ListItem(objRole.RoleName, objRole.RoleID.ToString))
                        cboRoles.Items(0).Selected = True
                        lblTitle.Text = String.Format(Localization.GetString("RoleTitle.Text", LocalResourceFile), objRole.RoleName, objRole.RoleID.ToString)
                    End If
                    cmdAdd.Text = String.Format(Localization.GetString("AddUser.Text", LocalResourceFile), objRole.RoleName)
                    cboRoles.Visible = False
                    plRoles.Visible = False
                End If
            End If

            ' bind all portal users to dropdownlist
            If UserId = -1 Then
                If UsersControl = UsersControl.Combo Then
                    If cboUsers.Items.Count = 0 Then
                        cboUsers.DataSource = UserController.GetUsers(PortalId, False)
                        cboUsers.DataBind()
                    End If
                    txtUsers.Visible = False
                    cboUsers.Visible = True
                    cmdValidate.Visible = False
                Else
                    txtUsers.Visible = True
                    cboUsers.Visible = False
                    cmdValidate.Visible = True
                End If
            Else
                Dim objUser As UserInfo = UserController.GetUser(PortalId, UserId, False)
                If Not objUser Is Nothing Then
                    txtUsers.Text = objUser.UserID.ToString
                    lblTitle.Text = String.Format(Localization.GetString("UserTitle.Text", LocalResourceFile), objUser.Username, objUser.UserID.ToString)
                End If
                cmdAdd.Text = String.Format(Localization.GetString("AddRole.Text", LocalResourceFile), objUser.Profile.FullName)
                txtUsers.Visible = False
                cboUsers.Visible = False
                cmdValidate.Visible = False
                plUsers.Visible = False
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' BindGrid loads the data grid from the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub BindGrid()

            Dim objRoleController As New RoleController

            If RoleId <> -1 Then
                Dim RoleName As String = objRoleController.GetRole(RoleId, PortalId).RoleName
                grdUserRoles.DataKeyField = "UserId"
                grdUserRoles.Columns(2).Visible = False
                grdUserRoles.DataSource = objRoleController.GetUserRolesByRoleName(PortalId, RoleName)
                grdUserRoles.DataBind()
            End If
            If UserId <> -1 Then
                Dim objUserInfo As UserInfo = UserController.GetUser(PortalId, UserId, False)
                grdUserRoles.DataKeyField = "RoleId"
                grdUserRoles.Columns(1).Visible = False
                grdUserRoles.DataSource = objRoleController.GetUserRolesByUsername(PortalId, objUserInfo.Username, Null.NullString)
                grdUserRoles.DataBind()
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetDates gets the expiry/effective Dates of a Users Role membership
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="UserId">The Id of the User</param>
        ''' <param name="RoleId">The Id of the Role</param>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        '''     [cnurse]    01/20/2006  Added support for Effective Date
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub GetDates(ByVal UserId As Integer, ByVal RoleId As Integer)

            Dim strExpiryDate As String = ""
            Dim strEffectiveDate As String = ""

            Dim objRoles As New RoleController
            Dim objUserRole As UserRoleInfo = objRoles.GetUserRole(PortalId, UserId, RoleId)
            If Not objUserRole Is Nothing Then
                If Null.IsNull(objUserRole.EffectiveDate) = False Then
                    strEffectiveDate = objUserRole.EffectiveDate.ToShortDateString
                End If
                If Null.IsNull(objUserRole.ExpiryDate) = False Then
                    strExpiryDate = objUserRole.ExpiryDate.ToShortDateString
                End If
            Else    ' new role assignment
                Dim objRole As RoleInfo = objRoles.GetRole(RoleId, PortalId)

                If objRole.BillingPeriod > 0 Then
                    Select Case objRole.BillingFrequency
                        Case "D" : strExpiryDate = DateAdd(DateInterval.Day, objRole.BillingPeriod, Now).ToShortDateString
                        Case "W" : strExpiryDate = DateAdd(DateInterval.Day, (objRole.BillingPeriod * 7), Now).ToShortDateString
                        Case "M" : strExpiryDate = DateAdd(DateInterval.Month, objRole.BillingPeriod, Now).ToShortDateString
                        Case "Y" : strExpiryDate = DateAdd(DateInterval.Year, objRole.BillingPeriod, Now).ToShortDateString
                    End Select
                End If
            End If

            txtEffectiveDate.Text = strEffectiveDate
            txtExpiryDate.Text = strExpiryDate

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' SendNotification sends an email notification to the user of the change in his/her role
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="UserId">The Id of the User</param>
        ''' <param name="RoleId">The Id of the Role</param>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub SendNotification(ByVal UserId As Integer, ByVal RoleId As Integer, ByVal Action As String)

            Dim objUser As UserInfo = UserController.GetUser(PortalId, UserId, False)

            Dim objRoles As New RoleController
            Dim objRole As RoleInfo = objRoles.GetRole(RoleId, PortalId)

            Dim Custom As New ArrayList
            Custom.Add(objRole.RoleName)
            Custom.Add(objRole.Description)

            Select Case Action
                Case "add"
                    Dim objUserRole As UserRoleInfo = objRoles.GetUserRole(PortalId, UserId, RoleId)
                    If Null.IsNull(objUserRole.EffectiveDate) Then
                        Custom.Add(DateTime.Today.ToString())
                    Else
                        Custom.Add(objUserRole.EffectiveDate.ToString())
                    End If
                    If Null.IsNull(objUserRole.ExpiryDate) Then
                        Custom.Add("-")
                    Else
                        Custom.Add(objUserRole.ExpiryDate.ToString())
                    End If
                    Mail.SendMail(PortalSettings.Email, objUser.Email, "", _
                        Services.Localization.Localization.GetSystemMessage(objUser.Profile.PreferredLocale, PortalSettings, "EMAIL_ROLE_ASSIGNMENT_SUBJECT", objUser), _
                        Services.Localization.Localization.GetSystemMessage(objUser.Profile.PreferredLocale, PortalSettings, "EMAIL_ROLE_ASSIGNMENT_BODY", objUser, Services.Localization.Localization.GlobalResourceFile, Custom), _
                        "", "", "", "", "", "")
                Case "remove"
                    Custom.Add("")
                    Mail.SendMail(PortalSettings.Email, objUser.Email, "", _
                        Services.Localization.Localization.GetSystemMessage(objUser.Profile.PreferredLocale, PortalSettings, "EMAIL_ROLE_UNASSIGNMENT_SUBJECT", objUser), _
                        Services.Localization.Localization.GetSystemMessage(objUser.Profile.PreferredLocale, PortalSettings, "EMAIL_ROLE_UNASSIGNMENT_BODY", objUser, Services.Localization.Localization.GlobalResourceFile, Custom), _
                        "", "", "", "", "", "")
            End Select

        End Sub

#End Region

#Region "Public Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' DataBind binds the data to the controls
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/10/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub DataBind()

            Dim objUser As UserInfo = UserController.GetUser(PortalId, UserId, False)

            If (Not (objUser Is Nothing) AndAlso objUser.IsSuperUser) OrElse _
                        PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) = False Then
                Response.Redirect(NavigateURL("Access Denied"), True)
            End If

            MyBase.DataBind()

            'this needs to execute always to the client script code is registred in InvokePopupCal
            cmdEffectiveCalendar.NavigateUrl = Common.Utilities.Calendar.InvokePopupCal(txtEffectiveDate)
            cmdExpiryCalendar.NavigateUrl = Common.Utilities.Calendar.InvokePopupCal(txtExpiryDate)

            Dim localizedCalendarText As String = Localization.GetString("Calendar")
            Dim calendarText As String = "<img src='" + ResolveUrl("~/images/calendar.png") + "' border='0' alt='" + localizedCalendarText + "'>&nbsp;" + localizedCalendarText
            cmdExpiryCalendar.Text = calendarText
            cmdEffectiveCalendar.Text = calendarText

            'Localize Headers
            Localization.LocalizeDataGrid(grdUserRoles, Me.LocalResourceFile)

            'Bind the role data to the datalist
            BindData()

            BindGrid()

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' DeleteButonVisible returns a boolean indicating if the delete button for
        ''' the specified UserID, RoleID pair should be shown
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="UserID">The ID of the user to check delete button visibility for</param>
        ''' <param name="RoleID">The ID of the role to check delete button visibility for</param>
        ''' <history>
        ''' 	[anurse]	01/13/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Function DeleteButtonVisible(ByVal UserID As Integer, ByVal RoleID As Integer) As Boolean
            ' [DNN-4285] Check if the role can be removed
            Return RoleController.CanRemoveUserFromRole(Me.PortalSettings, UserID, RoleID)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' FormatExpiryDate formats the expiry/effective date and filters out nulls
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="DateTime">The Date object to format</param>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Function FormatDate(ByVal DateTime As Date) As String
            If Not Null.IsNull(DateTime) Then
                Return DateTime.ToShortDateString
            Else
                Return ""
            End If
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' FormatExpiryDate formats the expiry/effective date and filters out nulls
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="DateTime">The Date object to format</param>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Function FormatUser(ByVal UserID As Integer, ByVal DisplayName As String) As String
            Return "<a href=""" & Common.LinkClick("userid=" & UserID.ToString, TabId, ModuleId) & """ class=""CommandButton"">" & DisplayName & "</a>"
        End Function
#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Init runs when the control is initialised
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/10/2006  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init

            If Not (Request.QueryString("RoleId") Is Nothing) Then
                RoleId = Int32.Parse(Request.QueryString("RoleId"))
            End If

            If Not (Request.QueryString("UserId") Is Nothing) Then
                UserId = Int32.Parse(Request.QueryString("UserId"))
            End If

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        '''     [VMasanas]  9/28/2004   Changed redirect to Access Denied
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If ParentModule Is Nothing Then
                    DataBind()
                End If
            Catch exc As Threading.ThreadAbortException
                'Do nothing if ThreadAbort as this is caused by a redirect
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cboUsers_SelectedIndexChanged runs when the selected User is changed in the
        ''' Users Drop-Down
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cboUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboUsers.SelectedIndexChanged
            If (Not cboUsers.SelectedItem Is Nothing) And (Not cboRoles.SelectedItem Is Nothing) Then
                GetDates(Int32.Parse(cboUsers.SelectedItem.Value), Int32.Parse(cboRoles.SelectedItem.Value))
            End If
            BindGrid()
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdValidate_Click executes when a user selects the Validate link for a username
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdValidate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdValidate.Click
            If txtUsers.Text <> "" Then
                ' validate username
                Dim objUser As UserInfo = UserController.GetUserByName(PortalId, txtUsers.Text, False)
                If Not objUser Is Nothing Then
                    GetDates(objUser.UserID, Int32.Parse(cboRoles.SelectedItem.Value))
                Else
                    txtUsers.Text = ""
                End If
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cboRoles_SelectedIndexChanged runs when the selected Role is changed in the
        ''' Roles Drop-Down
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cboRoles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoles.SelectedIndexChanged

            GetDates(UserId, Int32.Parse(cboRoles.SelectedItem.Value))
            BindGrid()
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdAdd_Click runs when the Update Button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
            Try
                If Page.IsValid Then
                    Dim objUserController As New UserController
                    Dim objRoleController As New RoleController

                    Dim objUser As UserInfo

                    'Get the User
                    If UserId <> -1 Then
                        objUser = UserController.GetUser(PortalId, UserId, False)
                    ElseIf UsersControl = UsersControl.TextBox AndAlso txtUsers.Text <> "" Then
                        objUser = UserController.GetUserByName(PortalId, txtUsers.Text, False)
                    ElseIf UsersControl = UsersControl.Combo AndAlso (Not cboUsers.SelectedItem Is Nothing) Then
                        objUser = UserController.GetUser(PortalId, Convert.ToInt32(cboUsers.SelectedItem.Value), False)
                    Else
                        objUser = Nothing
                    End If

                    If (Not cboRoles.SelectedItem Is Nothing) AndAlso (Not objUser Is Nothing) Then
                        ' do not modify the portal Administrator account dates
                        If objUser.UserID = PortalSettings.AdministratorId And cboRoles.SelectedItem.Value = PortalSettings.AdministratorRoleId.ToString Then
                            txtEffectiveDate.Text = ""
                            txtExpiryDate.Text = ""
                        End If

                        Dim datEffectiveDate As Date
                        If txtEffectiveDate.Text <> "" Then
                            datEffectiveDate = Date.Parse(txtEffectiveDate.Text)
                        Else
                            datEffectiveDate = Null.NullDate
                        End If
                        Dim datExpiryDate As Date
                        If txtExpiryDate.Text <> "" Then
                            datExpiryDate = Date.Parse(txtExpiryDate.Text)
                        Else
                            datExpiryDate = Null.NullDate
                        End If
                        Dim objEventLog As New Services.Log.EventLog.EventLogController

                        ' update assignment
                        objRoleController.AddUserRole(PortalId, objUser.UserID, Convert.ToInt32(cboRoles.SelectedItem.Value), datEffectiveDate, datExpiryDate)
                        objEventLog.AddLog("Role", cboRoles.SelectedItem.Text, PortalSettings, UserId, Services.Log.EventLog.EventLogController.EventLogType.USER_ROLE_CREATED)

                        ' send notification
                        If chkNotify.Checked Then
                            SendNotification(objUser.UserID, Convert.ToInt32(cboRoles.SelectedItem.Value), "add")
                        End If
                    End If
                End If

                BindGrid()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' grdUserRoles_Delete runs when one of the Delete Buttons in the UserRoles Grid
        ''' is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub grdUserRoles_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
            Try

                Dim objUser As New RoleController
                Dim strMessage As String = ""

                If RoleId <> -1 Then
                    If objUser.DeleteUserRole(PortalId, Integer.Parse(Convert.ToString(grdUserRoles.DataKeys(e.Item.ItemIndex))), RoleId) = False Then
                        strMessage = Services.Localization.Localization.GetString("RoleRemoveError", Me.LocalResourceFile)
                    Else
                        If chkNotify.Checked Then
                            SendNotification(Integer.Parse(Convert.ToString(grdUserRoles.DataKeys(e.Item.ItemIndex))), RoleId, "remove")
                        End If
                    End If
                End If
                If UserId <> -1 Then
                    If objUser.DeleteUserRole(PortalId, UserId, Integer.Parse(Convert.ToString(grdUserRoles.DataKeys(e.Item.ItemIndex)))) = False Then
                        strMessage = Services.Localization.Localization.GetString("RoleRemoveError", Me.LocalResourceFile)
                    Else
                        If chkNotify.Checked Then
                            SendNotification(UserId, Integer.Parse(Convert.ToString(grdUserRoles.DataKeys(e.Item.ItemIndex))), "remove")
                        End If
                    End If
                End If

                grdUserRoles.EditItemIndex = -1
                BindGrid()

                If strMessage <> "" Then
                    UI.Skins.Skin.AddModuleMessage(Me, strMessage, UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' grdUserRoles_ItemCreated runs when an item in the UserRoles Grid is created
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub grdUserRoles_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdUserRoles.ItemCreated
            Try

                Dim cmdDeleteUserRole As Control = e.Item.FindControl("cmdDeleteUserRole")

                If Not cmdDeleteUserRole Is Nothing Then
                    ClientAPI.AddButtonConfirm(CType(cmdDeleteUserRole, ImageButton), Services.Localization.Localization.GetString("DeleteItem"))
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

#Region "Optional Interfaces"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the ModuleActions for this ModuleControl
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	3/01/2006	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property ModuleActions() As ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New ModuleActionCollection

                Actions.Add(GetNextActionID, Localization.GetString("Cancel.Action", LocalResourceFile), ModuleActionType.AddContent, "", "lt.gif", ReturnUrl, False, SecurityAccessLevel.Admin, True, False)
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace
