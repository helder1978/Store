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
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Modules.Admin.Tabs

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The Tabs PortalModuleBase is used to manage the Tabs/Pages for a 
	''' portal.
	''' </summary>
    ''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cnurse]	9/9/2004	Updated to reflect design changes for Help, 508 support
	'''                       and localisation
	''' </history>
	''' -----------------------------------------------------------------------------
	Partial  Class Tabs

		Inherits Entities.Modules.PortalModuleBase
		Implements Entities.Modules.IActionable


#Region "Controls"

		'Pages Area

#End Region

#Region "Private Members"

		Protected arrPortalTabs As ArrayList

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' DeleteTab deletes the selected tab
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/27/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub DeleteTab()
            If lstTabs.SelectedIndex <> -1 Then

                Dim objTab As TabInfo = CType(arrPortalTabs(lstTabs.SelectedIndex), TabInfo)
                Dim tabs As ArrayList = GetPortalTabs(PortalSettings.DesktopTabs, objTab.TabID, False, False, False, False, False)

                If tabs.Count > 0 AndAlso objTab.TabID <> PortalSettings.AdminTabId AndAlso objTab.TabID <> PortalSettings.SplashTabId AndAlso objTab.TabID <> PortalSettings.HomeTabId AndAlso objTab.TabID <> PortalSettings.LoginTabId AndAlso objTab.TabID <> PortalSettings.UserTabId Then
                    Dim objTabs As New TabController

                    objTabs.DeleteTab(objTab.TabID, objTab.PortalID)

                    ' Redirect to this site to refresh
                    Response.Redirect(NavigateURL(TabId), True)
                Else
                    UI.Skins.Skin.AddModuleMessage(Me, Services.Localization.Localization.GetString("DeleteSpecialPage", Me.LocalResourceFile), UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                End If

            End If

        End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' EditTab redirects to the Edit Tab Page for the currently selected tab/page
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/9/2004	Created
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub EditTab()
            ' Redirect to edit page of currently selected tab
			If lstTabs.SelectedIndex <> -1 Then
                ' Redirect to module settings page
                Dim objTab As TabInfo = CType(arrPortalTabs(lstTabs.SelectedIndex), TabInfo)
                Response.Redirect(NavigateURL(objTab.TabID, "Tab", "action=edit", "returntabid=" & TabId.ToString), True)
            End If
        End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' ViewTab redirects to the Tab/Page for the currently selected tab/page
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/9/2004	Created
		''' </history>
		''' -----------------------------------------------------------------------------
        Private Sub ViewTab()
            If lstTabs.SelectedIndex <> -1 Then
                Dim objTabs As New TabController
                Dim objTab As TabInfo = objTabs.GetTab(CType(arrPortalTabs(lstTabs.SelectedIndex), TabInfo).TabID, PortalId, False)
                If Not objTab Is Nothing Then
                    Response.Redirect(NavigateURL(objTab.TabID), True)
                End If
            End If
        End Sub

#End Region

#Region "Event Handlers"

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Page_Load runs when the control is loaded
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/9/2004	Updated to reflect design changes for Help, 508 support
		'''                       and localisation
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try

                arrPortalTabs = GetPortalTabs(PortalSettings.DesktopTabs, False, True, False, True)

				' If this is the first visit to the page, bind the tab data to the page listbox
				If Page.IsPostBack = False Then

					lstTabs.DataSource = arrPortalTabs
                    lstTabs.DataBind()

                    ' select the tab ( if specified )
                    If Not Request.QueryString("selecttabid") Is Nothing Then
                        If Not lstTabs.Items.FindByValue(Request.QueryString("selecttabid")) Is Nothing Then
                            lstTabs.Items.FindByValue(Request.QueryString("selecttabid")).Selected = True
                        End If
                    End If

                End If

                ClientAPI.AddButtonConfirm(cmdDelete, Services.Localization.Localization.GetString("DeleteItem"))

            Catch exc As Exception    'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' RightLeft_Click runs when either the cmdLeft or cmdRight buttons is clicked
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/9/2004	Updated to reflect design changes for Help, 508 support
		'''                       and localisation
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub RightLeft_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles cmdLeft.Click, cmdRight.Click
			Try

				If lstTabs.SelectedIndex <> -1 Then

                    Dim objTab As TabInfo = CType(arrPortalTabs(lstTabs.SelectedIndex), TabInfo)

					Dim objTabs As New TabController

					Select Case CType(sender, ImageButton).CommandName
						Case "left"
							objTabs.UpdatePortalTabOrder(PortalId, objTab.TabId, objTab.ParentId, -1, 0, True)
						Case "right"
							objTabs.UpdatePortalTabOrder(PortalId, objTab.TabId, objTab.ParentId, 1, 0, True)
					End Select

					' Redirect to this site to refresh
                    Response.Redirect(NavigateURL(TabId, "", "selecttabid", objTab.TabID.ToString), True)

				End If

			Catch exc As Exception			 'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' UpDown_Click runs when either the cmdUp or cmdDown buttons is clicked
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/9/2004	Updated to reflect design changes for Help, 508 support
		'''                       and localisation
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub UpDown_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles cmdDown.Click, cmdUp.Click
			Try
				If lstTabs.SelectedIndex <> -1 Then

                    Dim objTab As TabInfo = CType(arrPortalTabs(lstTabs.SelectedIndex), TabInfo)
                    Dim objTabs As New TabController

					Select Case CType(sender, ImageButton).CommandName
						Case "up"
							objTabs.UpdatePortalTabOrder(PortalId, objTab.TabId, objTab.ParentId, 0, -1, True)
						Case "down"
							objTabs.UpdatePortalTabOrder(PortalId, objTab.TabId, objTab.ParentId, 0, 1, True)
					End Select

					' Redirect to this site to refresh
                    Response.Redirect(NavigateURL(TabId, "", "selecttabid", objTab.TabID.ToString), True)

				End If

			Catch exc As Exception			 'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdDelete_Click runs when the cmdDelete button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/27/2007	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdDelete.Click
            Try
                DeleteTab()
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
        ''' cmdEdit_Click runs when the cmdEdit button is clicked
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/9/2004	Updated to reflect design changes for Help, 508 support
		'''                       and localisation
		''' </history>
		''' -----------------------------------------------------------------------------
        Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdEdit.Click
            Try
                EditTab()
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
        ''' cmdView_Click runs when the cmdView button is clicked
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/9/2004	Updated to reflect design changes for Help, 508 support
		'''                       and localisation
		''' </history>
		''' -----------------------------------------------------------------------------
        Private Sub cmdView_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdView.Click
            Try
                ViewTab()
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

#Region "Optional Interfaces"
        Public ReadOnly Property ModuleActions() As ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New ModuleActionCollection
                Actions.Add(GetNextActionID, Services.Localization.Localization.GetString(ModuleActionType.AddContent, LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("returntabid", TabId.ToString), False, SecurityAccessLevel.Admin, True, False)
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
