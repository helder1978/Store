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
Imports DotNetNuke.Entities.Modules.Definitions
Imports DotNetNuke.UI.Utilities

Namespace DotNetNuke.Modules.Admin.Tabs

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The RecycleBin PortalModuleBase allows Tabs and Modules to be recovered or
	''' prmanentl deleted
	''' </summary>
    ''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cnurse]	9/15/2004	Updated to reflect design changes for Help, 508 support
	'''                       and localisation
	''' </history>
	''' -----------------------------------------------------------------------------
	Partial  Class RecycleBin

		Inherits Entities.Modules.PortalModuleBase

#Region "Controls"


		'Tabs
		Protected dshTabs As UI.UserControls.SectionHeadControl

		'Modules
		Protected dshModules As UI.UserControls.SectionHeadControl

		'tasks

#End Region

#Region "Private Methods"

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Loads deleted tabs and modules into the lists 
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[VMasanas]	18/08/2004	
		'''   [VMasanas]  20/08/2004  Update display information for deleted modules to:
		'''               ModuleFriendlyName: ModuleTitle - Tab: TabName
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub BindData()

			Dim intTab As Integer
			Dim arrDeletedTabs As New ArrayList
            Dim objTabs As New TabController
			Dim objTab As TabInfo

			Dim arrTabs As ArrayList = objTabs.GetTabs(PortalId)
			For intTab = 0 To arrTabs.Count - 1
				objTab = CType(arrTabs(intTab), TabInfo)
				If objTab.IsDeleted = True Then
					arrDeletedTabs.Add(objTab)
				End If
            Next

            Dim objModules As New ModuleController
            Dim objModule As ModuleInfo
            Dim intModule As Integer
            Dim arrDeletedModules As New ArrayList

            Dim arrModules As ArrayList = objModules.GetModules(PortalId)
            For intModule = 0 To arrModules.Count - 1
                objModule = CType(arrModules(intModule), ModuleInfo)
                If objModule.IsDeleted = True Then
                    If objModule.ModuleTitle = "" Then
                        objModule.ModuleTitle = objModule.FriendlyName
                    End If
                    arrDeletedModules.Add(objModule)
                End If
            Next

            lstTabs.DataSource = arrDeletedTabs
            lstTabs.DataBind()

            lstModules.DataSource = arrDeletedModules
            lstModules.DataBind()


            cboTab.DataSource = GetPortalTabs(PortalSettings.DesktopTabs, -1, False, True, False, False, True)
            cboTab.DataBind()

		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Deletes a module
		''' </summary>
		''' <param name="intModuleId">ModuleId of the module to be deleted</param>
		''' <remarks>
		''' Adds a log entry for the action to the EvenLog
		''' </remarks>
		''' <history>
		''' 	[VMasanas]	18/08/2004	Created
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub DeleteModule(ByVal intModuleId As Integer)
			Dim objEventLog As New Services.Log.EventLog.EventLogController

			' delete module
			Dim objModules As New ModuleController
            Dim objModule As ModuleInfo = objModules.GetModule(intModuleId, Null.NullInteger)
            If Not objModule Is Nothing Then
                objModules.DeleteModule(objModule.ModuleID)
				objEventLog.AddLog(objModule, PortalSettings, UserId, "", Services.Log.EventLog.EventLogController.EventLogType.MODULE_DELETED)
            End If

		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Deletes a tab
		''' </summary>
		''' <param name="intTabid">TabId of the tab to be deleted</param>
		''' <remarks>
		''' Adds a log entry for the action to the EventLog
		''' </remarks>
		''' <history>
		''' 	[VMasanas]	18/08/2004	Created
		'''                 19/09/2004  Remove skin deassignment. BLL takes care of this.
        '''                 30/09/2004  Change logic so log is only added when tab is actually deleted
        '''                 28/02/2005  Remove modules when deleting pages
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub DeleteTab(ByVal intTabid As Integer)
			Dim objEventLog As New Services.Log.EventLog.EventLogController

			' delete tab
            Dim objTabs As New TabController
            Dim objModules As New ModuleController

			Dim objTab As TabInfo = objTabs.GetTab(intTabid)
            If Not objTab Is Nothing Then
                'save tab modules before deleting page
                Dim arrTabModules As ArrayList = objModules.GetPortalTabModules(objTab.PortalID, objTab.TabID)

                ' hard delete the tab
                objTabs.DeleteTab(objTab.TabID, objTab.PortalID)

                ' check if it's deleted
                Dim objTabDeleted As TabInfo = objTabs.GetTab(intTabid)
                If objTabDeleted Is Nothing Then
                    'delete modules that do not have other instances
                    For Each objmodule As ModuleInfo In arrTabModules
                        ' check if all modules instances have been deleted
                        Dim objDelModule As ModuleInfo = objModules.GetModule(objmodule.ModuleID, Null.NullInteger)
                        If objDelModule Is Nothing OrElse objDelModule.TabID = Null.NullInteger Then
                            objModules.DeleteModule(objmodule.ModuleID)
                        End If
                    Next
                    objEventLog.AddLog(objTab, PortalSettings, UserId, "", Services.Log.EventLog.EventLogController.EventLogType.TAB_DELETED)
                Else
                    ' should be a parent tab
                    UI.Skins.Skin.AddModuleMessage(Me, String.Format(DotNetNuke.Services.Localization.Localization.GetString("ParentTab.ErrorMessage", Me.LocalResourceFile()), objTab.TabName), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                End If
            End If

		End Sub

#End Region

#Region "Event Handlers"

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Page_Load runs when the control is loaded
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[VMasanas]	18/08/2004	Add confirmation for Empty Recycle Bin button
		''' 	[cnurse]	15/09/2004	Localized Confirm text
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

			Dim ResourceFileRoot As String = Me.TemplateSourceDirectory + "/" + Services.Localization.Localization.LocalResourceDirectory + "/" + Me.ID
			' If this is the first visit to the page
			If (Page.IsPostBack = False) Then

				ClientAPI.AddButtonConfirm(cmdDeleteTab, Services.Localization.Localization.GetString("DeleteTab", ResourceFileRoot))
				ClientAPI.AddButtonConfirm(cmdDeleteModule, Services.Localization.Localization.GetString("DeleteModule", ResourceFileRoot))
				ClientAPI.AddButtonConfirm(cmdEmpty, Services.Localization.Localization.GetString("DeleteAll", ResourceFileRoot))

				BindData()

			End If

		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Restores selected tabs in the listbox
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' Adds a log entry for each restored tab to the EventLog
		''' Redirects to same page after restoring so the menu can be refreshed with restored tabs.
		''' This will not restore deleted modules for selected tabs, only the tabs are restored.
		''' </remarks>
		''' <history>
		''' 	[VMasanas]	18/08/2004	Added support for multiselect listbox
		'''                 30/09/2004  Child tabs cannot be restored until their parent is restored first.
		'''                             Change logic so log is only added when tab is actually restored
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub cmdRestoreTab_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRestoreTab.Click
			Dim item As ListItem
			Dim errors As Boolean = False

			For Each item In lstTabs.Items
				If item.Selected Then
					Dim objEventLog As New Services.Log.EventLog.EventLogController
					Dim objTabs As New TabController

					Dim objTab As TabInfo = objTabs.GetTab(Integer.Parse(item.Value))
					If Not objTab Is Nothing Then
						If Not Null.IsNull(objTab.ParentId) AndAlso Not lstTabs.Items.FindByValue(objTab.ParentId.ToString) Is Nothing Then
							UI.Skins.Skin.AddModuleMessage(Me, String.Format(Services.Localization.Localization.GetString("ChildTab.ErrorMessage", Me.LocalResourceFile()), objTab.TabName), UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
							errors = True
						Else
							objTab.IsDeleted = False
							objTabs.UpdateTab(objTab)
							objEventLog.AddLog(objTab, PortalSettings, UserId, "", Services.Log.EventLog.EventLogController.EventLogType.TAB_RESTORED)

                            Dim objmodules As New ModuleController
                            Dim arrMods As ArrayList = objmodules.GetAllTabsModules(objTab.PortalID, True)

                            For Each objModule As ModuleInfo In arrMods
                                objmodules.CopyModule(objModule.ModuleID, objModule.TabID, objTab.TabID, "", True)
                            Next
                        End If
                    End If
				End If
			Next
			If Not errors Then
				Response.Redirect(NavigateURL())
			Else
				BindData()
			End If

		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Deletes selected tabs in the listbox
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' Parent tabs will not be deleted. To delete a parent tab all child tabs need to be deleted before.
		''' Reloads data to refresh deleted modules and tabs listboxes
		''' </remarks>
		''' <history>
		''' 	[VMasanas]	18/08/2004	Added support for multiselect listbox
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub cmdDeleteTab_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdDeleteTab.Click
			Dim item As ListItem

			For Each item In lstTabs.Items
				If item.Selected Then
					DeleteTab(Integer.Parse(item.Value))
				End If
			Next
			BindData()

		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Restores selected modules in the listbox
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' Adds a log entry for each restored module to the EventLog
		''' </remarks>
		''' <history>
		''' 	[VMasanas]	18/08/2004	Added support for multiselect listbox
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub cmdRestoreModule_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRestoreModule.Click

            Dim objEventLog As New Services.Log.EventLog.EventLogController
            Dim objModules As New ModuleController
            Dim item As ListItem

            If Not cboTab.SelectedItem Is Nothing Then
                For Each item In lstModules.Items
                    If item.Selected Then
                        Dim objModule As ModuleInfo = objModules.GetModule(Integer.Parse(item.Value), Null.NullInteger)
                        If Not objModule Is Nothing Then
                            objModule.IsDeleted = False
                            objModule.TabID = Null.NullInteger
                            objModules.UpdateModule(objModule)

                            ' set defaults
                            objModule.CacheTime = 0
                            objModule.Alignment = ""
                            objModule.Color = ""
                            objModule.Border = ""
                            objModule.IconFile = ""
                            objModule.Visibility = VisibilityState.Maximized
                            objModule.ContainerSrc = ""
                            objModule.DisplayTitle = True
                            objModule.DisplayPrint = True
                            objModule.DisplaySyndicate = False
                            objModule.AllTabs = False

                            ' get default module settings
                            Dim settings As Hashtable = Entities.Portals.PortalSettings.GetSiteSettings(PortalId)
                            If Convert.ToString(settings("defaultmoduleid")) <> "" And Convert.ToString(settings("defaulttabid")) <> "" Then
                                Dim objDefaultModule As ModuleInfo = objModules.GetModule(Integer.Parse(Convert.ToString(settings("defaultmoduleid"))), Integer.Parse(Convert.ToString(settings("defaulttabid"))))
                                If Not objDefaultModule Is Nothing Then
                                    objModule.CacheTime = objDefaultModule.CacheTime
                                    objModule.Alignment = objDefaultModule.Alignment
                                    objModule.Color = objDefaultModule.Color
                                    objModule.Border = objDefaultModule.Border
                                    objModule.IconFile = objDefaultModule.IconFile
                                    objModule.Visibility = objDefaultModule.Visibility
                                    objModule.ContainerSrc = objDefaultModule.ContainerSrc
                                    objModule.DisplayTitle = objDefaultModule.DisplayTitle
                                    objModule.DisplayPrint = objDefaultModule.DisplayPrint
                                    objModule.DisplaySyndicate = objDefaultModule.DisplaySyndicate
                                End If
                            End If

                            ' add tab module
                            objModule.TabID = Integer.Parse(cboTab.SelectedItem.Value)
                            objModule.PaneName = glbDefaultPane
                            objModule.ModuleOrder = -1
                            objModules.AddModule(objModule)

							objEventLog.AddLog(objModule, PortalSettings, UserId, "", Services.Log.EventLog.EventLogController.EventLogType.MODULE_RESTORED)
                        End If
                    End If
                Next
                BindData()
            End If

		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Deletes selected modules in the listbox
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[VMasanas]	18/08/2004	Added support for multiselect listbox
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub cmdDeleteModule_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdDeleteModule.Click
			Dim item As ListItem

			For Each item In lstModules.Items
				If item.Selected Then
					DeleteModule(Integer.Parse(item.Value))
				End If
			Next
			BindData()

		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Permanently removes all deleted tabs and modules
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' Parent tabs will not be deleted. To delete a parent tab all child tabs need to be deleted before.
		''' </remarks>
		''' <history>
		''' 	[VMasanas]	18/08/2004	Created
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub cmdEmpty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEmpty.Click
			Dim item As ListItem

			For Each item In lstTabs.Items
				DeleteTab(Integer.Parse(item.Value))
			Next
			For Each item In lstModules.Items
				DeleteModule(Integer.Parse(item.Value))
			Next
			BindData()

		End Sub

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
