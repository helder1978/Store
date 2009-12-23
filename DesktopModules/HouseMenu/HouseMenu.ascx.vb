Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Services.Exceptions.Exceptions
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Security

Namespace TimRolands.DNN.Modules.HouseMenu

    Public Class HouseMenu
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase
        Implements Entities.Modules.IActionable
        'Implements Entities.Modules.IPortable
        'Implements Entities.Modules.ISearchable

        Private _Scope As Integer
        Private _ShowHome As Boolean
        Private _ShowParent As Boolean
        Private _ShowHidden As Boolean
        Private _ShowAdmin As Boolean
        Private _ShowPageIcons As Boolean
        Private _HidePageNames As Boolean
        Private _ShowSearchResults As Boolean
        Private _SearchResultsName As String = ""
        Private _IsRecursive As Boolean
        Private _Orientation As String = ""
        Private _ParentTabId As Integer
        Private _Mode As String = ""
        Private _CssElementId As String = ""
        Private _StyleName As String = ""
        Private _MenuPath As String = ""

        Public ReadOnly Property AdminMode() As Boolean
            Get
                Return PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) Or PortalSecurity.IsInRoles(PortalSettings.ActiveTab.AdministratorRoles.ToString)
            End Get
        End Property

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                'Page.SmartNavigation = False
                Me.LoadSettings()
                Me.BindMenu()
                Dim ph As System.Web.UI.WebControls.PlaceHolder
                ph = CType(Page.FindControl("phDNNHead"), PlaceHolder)
                If Not ph Is Nothing Then
                    If Me._StyleName.Length > 0 Then
                        ph.Controls.Add(New System.Web.UI.LiteralControl(Me.GetMenuStyle.ToString))
                    End If
                    If Me._Mode = "D" And Request.Browser.Browser = "IE" Then
                        ph.Controls.Add(New System.Web.UI.LiteralControl(Me.GetIEMenuScript.ToString))
                    End If
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#Region " Private Procedures "

        Private Sub LoadSettings()

            Try
                If Not Settings("Scope") Is Nothing Then
                    Me._Scope = CType(Settings("Scope"), Integer)
                Else
                    Me._Scope = 0
                End If

                If Not Settings("ShowHidden") Is Nothing Then
                    Me._ShowHidden = Boolean.Parse(Settings("ShowHidden").ToString)
                Else
                    Me._ShowHidden = False
                End If

                If Not Settings("ShowHome") Is Nothing Then
                    Me._ShowHome = Boolean.Parse(Settings("ShowHome").ToString)
                Else
                    Me._ShowHome = False
                End If

                If Not Settings("ShowParent") Is Nothing Then
                    Me._ShowParent = Boolean.Parse(Settings("ShowParent").ToString)
                Else
                    Me._ShowParent = False
                End If

                If Not Settings("ShowAdmin") Is Nothing Then
                    Me._ShowAdmin = Boolean.Parse(Settings("ShowAdmin").ToString)
                Else
                    Me._ShowAdmin = False
                End If

                If Not Settings("ShowPageIcons") Is Nothing Then
                    Me._ShowPageIcons = Boolean.Parse(Settings("ShowPageIcons").ToString)
                Else
                    Me._ShowPageIcons = False
                End If

                If Not Settings("HidePageNames") Is Nothing Then
                    Me._HidePageNames = Boolean.Parse(Settings("HidePageNames").ToString)
                Else
                    Me._HidePageNames = False
                End If

                If Not Settings("ShowSearchResults") Is Nothing Then
                    Me._ShowSearchResults = CType(Settings("ShowSearchResults"), Boolean)
                Else
                    Me._ShowSearchResults = False
                End If

                If Not Settings("SearchResultsName") Is Nothing Then
                    Me._SearchResultsName = CType(Settings("SearchResultsName"), String)
                Else
                    Me._SearchResultsName = ""
                End If

                If Not Settings("IsRecursive") Is Nothing Then
                    Me._IsRecursive = Boolean.Parse(Settings("IsRecursive").ToString)
                Else
                    Me._IsRecursive = False
                End If

                If Not Settings("Orientation") Is Nothing Then
                    Me._Orientation = CType(Settings("Orientation"), String)
                Else
                    Me._Orientation = "V"
                End If

                If Not Settings("Mode") Is Nothing Then
                    Me._Mode = CType(Settings("Mode"), String)
                Else
                    Me._Mode = "S"
                End If

                If Not Settings("CssElementId") Is Nothing Then
                    Me._CssElementId = CType(Settings("CssElementId"), String)
                Else
                    Me._CssElementId = ""
                End If

                If Not Settings("StyleName") Is Nothing Then
                    Me._StyleName = CType(Settings("StyleName"), String)
                Else
                    If Me._Orientation = "H" Then
                        Me._StyleName = "ModuleH"
                    Else
                        If Me._Mode = "D" Then
                            Me._StyleName = "ModuleV"
                        Else
                            Me._StyleName = "ModuleVstatic"
                        End If
                    End If
                End If

                Me._MenuPath = DotNetNuke.Common.ApplicationPath & "/DesktopModules/HouseMenu/"

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try

        End Sub

        Private Sub BindMenu()

            Try
                Dim htmMenu As String

                ' Retrieve tabs based on "Scope" property. Default is current tab's 
                ' id. Can be set to any tab id or portal id (all tabs)
                Select Case Me._Scope
                    Case Is > 0  ' Specific selected tab/page
                        Me._ParentTabId = Me._Scope
                        htmMenu = Me.RenderMenu(Me._Scope)
                    Case 0  ' Current tab/page (default)
                        Me._ParentTabId = Me.PortalSettings.ActiveTab.TabID
                        htmMenu = Me.RenderMenu(Me.PortalSettings.ActiveTab.TabID)
                    Case Else  ' All tabs/pages
                        Me._ParentTabId = Me.PortalSettings.HomeTabId
                        Me._ShowParent = False
                        htmMenu = Me.RenderFullMenu
                End Select

                Me.litMenu.Text = htmMenu

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try

        End Sub

        Private Function RenderFullMenu() As String
            Try
                Dim htmMenu As New Text.StringBuilder
                Dim objTabController As TabController = New TabController
                Dim objPortalTabs As ArrayList  ' TabInfo objects

                If Me._ShowAdmin Then
                    If Not PortalSettings.DesktopTabs Is Nothing Then
                        htmMenu.Append("<ul id=""" & Me.GetMenuId & "0"">")
                        If Me.PortalSettings.HomeTabId <> -1 Then
                            Dim objHomeTab As TabInfo = objTabController.GetTab(Me.PortalSettings.HomeTabId)
                            htmMenu.Append(Me.RenderMenuItem(GetPortalDomainName(PortalSettings.PortalAlias.HTTPAlias, Request), objHomeTab.TabName))
                            Me._ShowHome = False
                        End If
                        For Each objTab As TabInfo In PortalSettings.DesktopTabs
                            If objTab.ParentId < 1 And Not objtab.IsDeleted Then
                                htmMenu.Append(Me.RenderMenuItem(objTab.TabId))
                            End If
                        Next
                        htmMenu.Append("</ul>")
                    End If
                Else
                    objPortalTabs = GetPortalTabs(PortalSettings.DesktopTabs, False, Me._ShowHidden, False, True)
                    If Not objPortalTabs Is Nothing Then
                        htmMenu.Append("<ul id=""" & Me.GetMenuId & "0"">")
                        If Me.PortalSettings.HomeTabId <> -1 Then
                            Dim objHomeTab As TabInfo = objTabController.GetTab(Me.PortalSettings.HomeTabId)
                            htmMenu.Append(Me.RenderMenuItem(GetPortalDomainName(PortalSettings.PortalAlias.HTTPAlias, Request), objHomeTab.TabName))
                            Me._ShowHome = False
                        End If
                        For Each objTab As TabInfo In objPortalTabs
                            If objTab.ParentId < 1 And Not objtab.IsDeleted Then
                                htmMenu.Append(Me.RenderMenuItem(objTab.TabId))
                            End If
                        Next
                    End If
                    htmMenu.Append("</ul>")
                End If

                ' Clear any empty submenus
                htmMenu.Replace("<ul></ul>", "")
                htmMenu.Replace("<ul>" & vbCrLf & "</ul>", "")
                htmMenu.Replace("<ul>" & vbCrLf & vbCrLf & "</ul>", "")

                Return htmMenu.ToString

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try

        End Function

        Private Function RenderMenu(ByVal parentId As Integer) As String

            Try
                Dim htmMenu As New Text.StringBuilder
                Dim objTabController As TabController = New TabController
                Dim objTabs As ArrayList  ' TabInfo objects
                Dim parentLevel As Integer

                ' Get passed parent tab/page
                Dim objParent As New TabInfo
                objParent = objTabController.GetTab(parentId)

                ' Set the parent level and get tabs/pages
                parentLevel = objParent.Level
                objTabs = objTabController.GetTabsByParentId(parentId)

                htmMenu.Append("<ul id=""" & Me.GetMenuId & "List" & parentId.ToString & """>")

                ' Render Home, if requested
                If Me._ShowHome Then
                    If Me.PortalSettings.HomeTabId = -1 Then
                        htmMenu.Append(Me.RenderMenuItem(GetPortalDomainName(PortalSettings.PortalAlias.HTTPAlias, Request), "Home"))
                    Else
                        htmMenu.Append(Me.RenderMenuItem(Me.PortalSettings.HomeTabId))
                    End If
                End If
                Me._ShowHome = False

                ' Render parent, if requested
                If Me._ShowParent And objParent.TabID <> 1 Then
                    htmMenu.Append(Me.RenderMenuItem(objParent.TabID))
                End If
                Me._ShowParent = False

                ' Render menu items and submenus
                If Not objTabs Is Nothing Then
                    For Each objTab As TabInfo In objTabs
                        If objTab.IsVisible = True Or Me._ShowHidden = True Then
                            If objTab.Level = parentLevel + 1 Then
                                htmMenu.Append(Me.RenderMenuItem(objTab.TabID))
                            End If
                        End If
                    Next
                End If

                htmMenu.Append("</ul>")

                ' Clear any empty submenus
                htmMenu.Replace("<ul></ul>", "")
                htmMenu.Replace("<ul>" & vbCrLf & "</ul>", "")
                htmMenu.Replace("<ul>" & vbCrLf & vbCrLf & "</ul>", "")
                htmMenu.Replace("<ul id=""" & Me.GetMenuId & "List" & parentId.ToString & """></ul>", "")
                htmMenu.Replace("<ul id=""" & Me.GetMenuId & "List" & parentId.ToString & """>" & vbCrLf & "</ul>", "")
                htmMenu.Replace("<ul id=""" & Me.GetMenuId & "List" & parentId.ToString & """>" & vbCrLf & vbCrLf & "</ul>", "")

                Return htmMenu.ToString

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try

        End Function

        Private Function RenderMenuItem(ByVal itemId As Integer) As String

            Try
                Dim htmMenu As New Text.StringBuilder
                Dim objTabController As TabController = New TabController
                Dim objTab As TabInfo = objTabController.GetTab(itemId)

                'Dim HasChildMenu As Boolean = False
                Dim CssClassHtm As String = ""
                If objTabController.GetTabsByParentId(objTab.TabID).Count > 0 Then CssClassHtm = " class=""submenu"""

                If objTab.IsDeleted = False And (objTab.IsVisible = True Or Me._ShowHidden = True) And (objTab.TabName <> Me._SearchResultsName Or Me._ShowSearchResults = True) Then
                    If ((objTab.StartDate.Year = 1 Or objTab.StartDate < Now.Date) And (objTab.EndDate.Year = 1 Or objTab.EndDate > Now.Date)) Or AdminMode = True Then
                        If PortalSecurity.IsInRoles(objTab.AuthorizedRoles) = True Then
                            If objTab.TabID = Me._ParentTabId Then
                                If Me._ShowParent Then
                                    htmMenu.Append("<li id=""houseMenuParentItem""" & CssClassHtm & ">")
                                    'htmMenu.Append("<a id=""houseMenuParentLink"" href=""" & Page.ResolveUrl("~/default.aspx?tabid=" & objTab.TabID) & """>")
                                    If objTab.FullUrl.IndexOf("javascript:") = -1 Then
                                        htmMenu.Append("<a id=""houseMenuParentLink"" href=""" & objTab.FullUrl & """>")
                                    Else
                                        htmMenu.Append("<a id=""houseMenuParentLink"" href=""#"" onclick=""" & objTab.FullUrl & """>")
                                    End If
                                    Me._ShowParent = False
                                Else
                                    Return ""
                                End If
                            Else
                                If PortalSecurity.IsInRoles(objTab.AuthorizedRoles) Then
                                    If objTab.TabID = PortalSettings.ActiveTab.TabID Then
                                        htmMenu.Append("<li id=""houseMenuCurrentItem""" & CssClassHtm & ">")
                                        If objTab.DisableLink Then
                                            htmMenu.Append("<a id=""houseMenuCurrentLink"" class=""ArrowPointer"">")
                                        Else
                                            'htmMenu.Append("<a id=""houseMenuCurrentLink"" href=""" & Page.ResolveUrl("~/default.aspx?tabid=" & objTab.TabID) & """>")
                                            If objTab.FullUrl.IndexOf("javascript:") = -1 Then
                                                htmMenu.Append("<a id=""houseMenuCurrentLink"" href=""" & objTab.FullUrl & """>")
                                            Else
                                                htmMenu.Append("<a id=""houseMenuCurrentLink"" href=""#"" onclick=""" & objTab.FullUrl & """>")
                                            End If
                                        End If
                                    Else
                                        htmMenu.Append("<li" & CssClassHtm & "aaaa>")
                                        If objTab.DisableLink Then
                                            htmMenu.Append("<a class=""ArrowPointer"">")
                                        Else
                                            'htmMenu.Append("<a href=""" & Page.ResolveUrl("~/default.aspx?tabid=" & objTab.TabID) & """>")
                                            If objTab.FullUrl.IndexOf("javascript:") = -1 Then
                                                htmMenu.Append("<a href=""" & objTab.FullUrl & """>")
                                            Else
                                                htmMenu.Append("<a href=""#"" onclick=""" & objTab.FullUrl & """>")
                                            End If
                                        End If
                                    End If
                                Else
                                    Return ""
                                End If
                            End If
                            If (Me._ShowPageIcons = True And objTab.IconFile.Length > 0) Then
                                If objTab.IsAdminTab Or objTab.IsSuperTab Then
                                    htmMenu.Append("<img align=""absmiddle"" border=""0"" src=""images/" & objTab.IconFile & """ alt=""" & objTab.TabName & """>&nbsp;")
                                Else
                                    htmMenu.Append("<img align=""absmiddle"" border=""0"" src=""" & Me.PortalSettings.HomeDirectory & objTab.IconFile & """ alt=""" & objTab.TabName & """>&nbsp;")
                                End If
                            End If
                            If Not Me._HidePageNames Then htmMenu.Append(objTab.TabName)
                            'htmMenu.Append(objTab.TabName)
                            htmMenu.Append("</a>")
                            If Me._IsRecursive = True And objTab.TabID <> Me._ParentTabId Then
                                If objTabController.GetTabsByParentId(objTab.TabID).Count > 0 Then
                                    Me._ShowHome = False
                                    htmMenu.Append(vbCrLf & Me.RenderMenu(objTab.TabID))
                                End If
                            End If
                            htmMenu.Append("</li>" & vbCrLf)
                        End If
                    End If
                    'Else
                    '    htmMenu.Append("")
                End If

                Return htmMenu.ToString

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try

        End Function

        Private Function RenderMenuItem(ByVal linkUrl As String, ByVal linkName As String, Optional ByVal linkIcon As String = "") As String

            Try
                Dim htmMenu As New Text.StringBuilder

                htmMenu.Append("<li>")
                If linkUrl.IndexOf("javascript:") = -1 Then
                    htmMenu.Append("<a href=""" & linkUrl & """>")
                Else
                    htmMenu.Append("<a href=""#"" onclick=""" & linkUrl & """>")
                End If
                If (Me._ShowPageIcons = True And linkIcon.Length > 0) Then
                    htmMenu.Append("<img align=""absmiddle"" border=""0"" src=""" & Me.PortalSettings.HomeDirectory & linkIcon & """ alt=""" & linkName & """>&nbsp;")
                End If
                'htmMenu.Append(linkName)
                If Not Me._HidePageNames Then htmMenu.Append(linkName)
                htmMenu.Append("</a>")
                htmMenu.Append("</li>" & vbCrLf)

                Return htmMenu.ToString

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try

        End Function

        ' Returns the javascript function necessary to help IE browsers interpret CSS hovers.
        Private Function GetIEMenuScript() As String
            Try
                Dim script As New Text.StringBuilder

                script.Append("<script type=""text/javascript"" src=""" & Me._MenuPath & "module.js""></script>" & vbCrLf)

                script.Append("<script language=""javascript"" type=""text/javascript"">" & vbCrLf)
                script.Append("<!--" & vbCrLf)

                script.Append("    function doHoverFix() {" & vbCrLf)
                script.Append("        ieModuleHoverFix('" & Me.GetMenuId & "','" & Me.AdminMode.ToString & "');" & vbCrLf)
                script.Append("    }" & vbCrLf)
                If Request.Browser.Platform.IndexOf("Mac") <> -1 Then
                    script.Append("    window.onload = doHoverFix;" & vbCrLf)
                Else
                    script.Append("    if (window.attachEvent) {window.attachEvent(""onload"", doHoverFix);}" & vbCrLf)
                End If

                script.Append("//-->" & vbCrLf)
                script.Append("</script>" & vbCrLf)

                ''===external scriptmethod 2===
                ''script.Append("<link id=""HouseMenuStyleLink"" href=""DesktopModules/HouseMenuSkinObject/adxmenu/menuh.css"" type=""text/css"" rel=""stylesheet""></link>" & vbCrLf)
                ''script.Append("<!--[if !IE]> <-->" & vbCrLf)
                ''script.Append("    <script type=""text/javascript"" src=""DesktopModules/HouseMenuSkinObject/adxmenu/ADxMenu.js""></script>" & vbCrLf)
                ''script.Append("<!----> <![endif]-->" & vbCrLf)
                ''script.Append("<!--[if lte IE 6]>" & vbCrLf)
                'script.Append("<link id=""HouseMenuStyleLink"" href=""DesktopModules/HouseMenuSkinObject/adxmenu/menuh.css"" type=""text/css"" rel=""stylesheet""></link>" & vbCrLf)
                'script.Append("    <LINK id=""HouseMenuStyleLinkIE"" href=""DesktopModules/HouseMenuSkinObject/adxmenu/menuh4ie.css"" type=""text/css"" rel=""stylesheet""></link>" & vbCrLf)
                'script.Append("    <STYLE type=""text/css"" media=""screen, tv, projection"">" & vbCrLf)
                'script.Append("        BODY { BEHAVIOR: url(""DesktopModules/HouseMenuSkinObject/adxmenu/ADxMenu.htc""); }" & vbCrLf)
                'script.Append("    </STYLE>" & vbCrLf)
                ''script.Append("<![endif]-->" & vbCrLf)
                ''===

                Return script.ToString

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function GetMenuStyle() As String
            Try
                Dim stylePath As String = Me._MenuPath & "styles/" & Me._StyleName & ".css"

                If System.IO.File.Exists(Server.MapPath(stylePath)) Then
                    Dim script As New Text.StringBuilder
                    script.Append("<link id=""HouseMenuStyleLink" & Me.ModuleId & """ href=""" & stylePath & """ type=""text/css"" rel=""stylesheet""></link>" & vbCrLf)
                    Return script.ToString
                Else
                    Dim objModules As New ModuleController
                    'objModules.UpdateTabModuleSetting(TabModuleId, "StyleName", "")
                    objModules.UpdateModuleSetting(Me.ModuleId, "StyleName", "")
                    Return ""
                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Function

#End Region

#Region " Public Procedures "

        Public Function GetMenuId() As String
            If Me._CssElementId <> "" Then
                Return Me._CssElementId
            Else
                If Me._Orientation = "H" Then
                    Return "houseMenuH"
                Else
                    If Me._Mode = "D" Then
                        Return "houseMenuV"
                    Else
                        Return "houseMenuVstatic"
                    End If
                End If
            End If
        End Function

#End Region

#Region "Optional Interfaces"

        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                'Actions.Add(GetNextActionID, DotNetNuke.Services.Localization.Localization.GetString(Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl(), False, SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property

        'Public Function ExportModule(ByVal ModuleID As Integer) As String Implements Entities.Modules.IPortable.ExportModule
        '    ' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
        'End Function

        'Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements Entities.Modules.IPortable.ImportModule
        '    ' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
        'End Sub

        'Public Function GetSearchItems(ByVal ModInfo As Entities.Modules.ModuleInfo) As Services.Search.SearchItemInfoCollection Implements Entities.Modules.ISearchable.GetSearchItems
        '    ' included as a stub only so that the core knows this module Implements Entities.Modules.ISearchable
        'End Function

#End Region

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub
        Protected WithEvents litMenu As System.Web.UI.WebControls.Literal

        'NOTE: The following placeholder declaration is required by the Web Form Designer.
        'Do not delete or move it.
        Private designerPlaceholderDeclaration As System.Object

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

        End Sub

#End Region

    End Class

End Namespace
