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

Imports DotNetNuke
Imports System.Web
Imports System.Text.RegularExpressions
Imports System.IO

Namespace DotNetNuke.Modules.SearchInput
	Partial Class SearchInput
		Inherits Entities.Modules.PortalModuleBase

#Region "Controls"
#End Region

#Region "Private Methods"

        Private Sub ShowHideImages()
            Dim ShowGoImage As String = CType(Settings("ShowGoImage"), String)
            Dim ShowSearchImage As String = CType(Settings("ShowSearchImage"), String)

            Dim bShowGoImage As Boolean = False
            Dim bShowSearchImage As Boolean = False

            If Not ShowGoImage Is Nothing Then
                bShowGoImage = CType(ShowGoImage, Boolean)
            End If

            If Not ShowSearchImage Is Nothing Then
                bShowSearchImage = CType(ShowSearchImage, Boolean)
            End If

            imgSearch.Visible = bShowSearchImage
            plSearch.Visible = Not bShowSearchImage
            imgGo.Visible = bShowGoImage
            imgGoAdvanced.Visible = bShowGoImage
            cmdGo.Visible = Not bShowGoImage
        End Sub

#End Region

#Region "Public Methods"
        Private Sub SearchExecute()
            Dim ResultsTabid As Integer

            If Not Settings("SearchResultsModule") Is Nothing Then
                ResultsTabid = Integer.Parse(CType(Settings("SearchResultsModule"), String))
            Else
                'Get Default Page
                Dim objModules As New Entities.Modules.ModuleController
                Dim SearchModule As DotNetNuke.Entities.Modules.ModuleInfo = objModules.GetModuleByDefinition(PortalSettings.PortalId, "Search Results")
                If SearchModule Is Nothing Then
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("NoSearchModule", LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning)
                    Return
                Else
                    ResultsTabid = SearchModule.TabID
                End If
            End If
            ' canadean changed: added search section restriction and advanced search options
            Dim separator, searchText As String
            searchText = ""
            If DotNetNuke.Entities.Host.HostSettings.GetHostSetting("UseFriendlyUrls") = "Y" Then
                separator = "?"
            Else
                separator = "&"
            End If
            If txtSearch.Text <> "" Then
                searchText = Server.UrlEncode(txtSearch.Text)
            Else
                If tbAny.Text <> "" Then
                    searchText = Server.UrlEncode(tbAny.Text)
                Else
                    If tbAll.Text <> "" Then
                        Dim a() As String
                        a = tbAll.Text.Split(" ")
                        Dim j As String
                        j = String.Join(" +", a)
                        searchText = Server.UrlEncode(j)
                    Else
                        If tbExact.Text <> "" Then
                            Dim a() As String
                            a = tbExact.Text.Split(" ")
                            Dim j As String
                            j = String.Join(" +", a)
                            searchText = Server.UrlEncode("+" & j)
                        End If
                    End If

                End If

            End If
            Response.Redirect(NavigateURL(ResultsTabid) & separator & "Search=" & searchText & "&section=" & ddSearchSection.SelectedItem.Value)

        End Sub
#End Region

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Dim GoUrl As String = Services.Localization.Localization.GetString("imgGo.ImageUrl", Me.LocalResourceFile)
            Dim SearchUrl As String = Services.Localization.Localization.GetString("imgSearch.ImageUrl", Me.LocalResourceFile)

            If GoUrl.StartsWith("~") Then
                imgGo.ImageUrl = GoUrl
                imgGoAdvanced.ImageUrl = GoUrl
            Else
                imgGo.ImageUrl = Path.Combine(PortalSettings.HomeDirectory, GoUrl)
                imgGoAdvanced.ImageUrl = Path.Combine(PortalSettings.HomeDirectory, GoUrl)
            End If

            If SearchUrl.StartsWith("~") Then
                imgSearch.ImageUrl = SearchUrl
            Else
                imgSearch.ImageUrl = Path.Combine(PortalSettings.HomeDirectory, SearchUrl)
            End If

            plSearch.HelpText = ""
            ShowHideImages()

            cmdGo.Text = Services.Localization.Localization.GetString("cmdGo.Text", Me.LocalResourceFile)

            If Not Request.Params("Advanced") Is Nothing Then
                tableAdvanced.Visible = True
            End If
        End Sub

        Private Sub imgGo_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgGo.Click
            SearchExecute()
        End Sub

        Protected Sub imgGoAdvanced_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgGoAdvanced.Click
            SearchExecute()
        End Sub

        Private Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click
            SearchExecute()
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
