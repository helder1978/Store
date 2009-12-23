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
Imports DotNetNuke.Entities.Tabs

Namespace DotNetNuke.UI.Skins.Controls

    ''' -----------------------------------------------------------------------------
    ''' <summary></summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 	[cniknet]	10/15/2004	Replaced public members with properties and removed
    '''                             brackets from property names
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Links
        Inherits UI.Skins.SkinObjectBase

        ' private members
        Private _separator As String
        Private _cssClass As String
        Private _level As String
        Private _alignment As String

#Region "Public Members"
        Public Property Separator() As String
            Get
                Return _separator
            End Get
            Set(ByVal Value As String)
                _separator = Value
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

        Public Property Level() As String
            Get
                Return _level
            End Get
            Set(ByVal Value As String)
                _level = Value
            End Set
        End Property

        Public Property Alignment() As String
            Get
                Return _alignment
            End Get
            Set(ByVal Value As String)
                _alignment = Value
            End Set
        End Property

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            ' public attributes
            Dim strCssClass As String
            If CssClass <> "" Then
                strCssClass = CssClass
            Else
                strCssClass = "SkinObject"
            End If

            Dim strSeparator As String
            If Separator <> "" Then
                If Separator.IndexOf("src=") <> -1 Then
                    strSeparator = Replace(Separator, "src=", "src=" & PortalSettings.ActiveTab.SkinPath)
                Else
                    strSeparator = "<span class=""" & strCssClass & """>" & Replace(Separator, " ", "&nbsp;") & "</span>"
                End If
            Else
                strSeparator = "  "
            End If

            ' build links
            Dim strLinks As String = ""

            strLinks = BuildLinks(Level, Alignment, strSeparator, strCssClass)

            If strLinks = "" Then
                strLinks = BuildLinks("", Alignment, strSeparator, strCssClass)
            End If

            lblLinks.Text = strLinks

        End Sub

        Private Function BuildLinks(ByVal Level As String, ByVal Alignment As String, ByVal strSeparator As String, ByVal strCssClass As String) As String

            Dim strLinks As String = ""
            Dim strLoop As String
            Dim intIndex As Integer

            For intIndex = 0 To PortalSettings.DesktopTabs.Count - 1

                Dim objTab As TabInfo = CType(PortalSettings.DesktopTabs(intIndex), TabInfo)

                If objTab.IsVisible = True And objTab.IsDeleted = False And objTab.DisableLink = False Then
                    If (objTab.StartDate < Now And objTab.EndDate > Now) Or AdminMode = True Then
                        If PortalSecurity.IsInRoles(objTab.AuthorizedRoles) = True Then
                            strLoop = ""
                            If Alignment = "Vertical" Then
                                If strLinks <> "" Then
                                    strLoop = "<br />" & strSeparator
                                Else
                                    strLoop = strSeparator
                                End If
                            Else
                                If strLinks <> "" Then
                                    strLoop = strSeparator
                                End If
                            End If

                            Select Case Level
                                Case "Same", ""
                                    If objTab.ParentId = PortalSettings.ActiveTab.ParentId Then
                                        strLinks += strLoop & AddLink(objTab.TabName, objTab.FullUrl, strCssClass)
                                    End If
                                Case "Child"
                                    If objTab.ParentId = PortalSettings.ActiveTab.TabID Then
                                        strLinks += strLoop & AddLink(objTab.TabName, objTab.FullUrl, strCssClass)
                                    End If
                                Case "Parent"
                                    If objTab.TabID = PortalSettings.ActiveTab.ParentId Then
                                        strLinks += strLoop & AddLink(objTab.TabName, objTab.FullUrl, strCssClass)
                                    End If
                                Case "Root"
                                    If objTab.Level = 0 Then
                                        strLinks += strLoop & AddLink(objTab.TabName, objTab.FullUrl, strCssClass)
                                    End If
                            End Select
                        End If
                    End If
                End If

            Next intIndex

            Return strLinks

        End Function

        Private Function AddLink(ByVal strTabName As String, ByVal strURL As String, ByVal strCssClass As String) As String

            Return "<a class=""" & strCssClass & """ href=""" & strURL & """>" & strTabName & "</a>"

        End Function

    End Class

End Namespace
