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
Imports System.Text
Imports DotNetNuke.Entities.Tabs

Namespace DotNetNuke.Common.Utilities

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The LinkClick Page processes links
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class SiteMap

        Inherits Framework.PageBase

        Const SITEMAP_CHANGEFREQ As String = "daily"
        Const SITEMAP_PRIORITY As String = "0.8"
        Const SITEMAP_MAXURLS As Integer = 50000

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try
                Response.ContentType = "text/xml"
                Response.ContentEncoding = Encoding.UTF8
                Response.Write(BuildSiteMap(PortalSettings.PortalId))
            Catch exc As Exception

            End Try

        End Sub

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Builds SiteMap
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function BuildSiteMap(ByVal PortalID As Integer) As String

            Dim sb As New StringBuilder(1024)
            Dim URL As String

            ' build header
            sb.Append("<?xml version=""1.0"" encoding=""UTF-8""?>" & ControlChars.CrLf)
            sb.Append("<urlset xmlns=""http://www.google.com/schemas/sitemap/0.84"">" & ControlChars.CrLf)

            ' add urls
            Dim intURLs As Integer = 0
            Dim objTabs As New TabController
            For Each objTab As TabInfo In objTabs.GetTabs(PortalID)
                If objTab.IsDeleted = False And objTab.DisableLink = False And objTab.TabType = TabType.Normal And ((Null.IsNull(objTab.StartDate) = True Or objTab.StartDate < Now) And (Null.IsNull(objTab.EndDate) = True Or objTab.EndDate > Now)) Then
                    ' the crawler is an anonymous user therefore the site map will only contain publicly accessible pages
                    If PortalSecurity.IsInRoles(objTab.AuthorizedRoles) Then
                        If intURLs < SITEMAP_MAXURLS Then
                            intURLs += 1
                            URL = objTab.FullUrl
                            If URL.IndexOf(Request.Url.Host) = -1 Then
                                URL = AddHTTP(Request.Url.Host) & URL
                            End If
                            sb.Append(BuildURL(URL, 2))
                        End If
                    End If
                End If
            Next

            sb.Append("</urlset>")

            Return sb.ToString

        End Function

        Private Function BuildURL(ByVal URL As String, ByVal Indent As Integer) As String

            Dim sb As New StringBuilder(1024)

            sb.Append(WriteElement("url", Indent))
            sb.Append(WriteElement("loc", URL, Indent + 1))
            sb.Append(WriteElement("lastmod", DateTime.Now.ToString("yyyy-MM-dd"), Indent + 1))
            sb.Append(WriteElement("changefreq", SITEMAP_CHANGEFREQ, Indent + 1))
            sb.Append(WriteElement("priority", SITEMAP_PRIORITY, Indent + 1))
            sb.Append(WriteElement("/url", Indent))

            Return sb.ToString

        End Function

        Private Function WriteElement(ByVal Element As String, ByVal Indent As Integer) As String
            Dim InputLength As Integer = Element.Trim.Length + 20
            Dim sb As New StringBuilder(InputLength)
            sb.Append(ControlChars.CrLf.PadRight(Indent + 2, ControlChars.Tab))
            sb.Append("<").Append(Element).Append(">")
            Return sb.ToString
        End Function

        Private Function WriteElement(ByVal Element As String, ByVal ElementValue As String, ByVal Indent As Integer) As String
            Dim InputLength As Integer = Element.Trim.Length + ElementValue.Trim.Length + 20
            Dim sb As New StringBuilder(InputLength)
            sb.Append(ControlChars.CrLf.PadRight(Indent + 2, ControlChars.Tab))
            sb.Append("<").Append(Element).Append(">")
            sb.Append(ElementValue)
            sb.Append("</").Append(Element).Append(">")
            Return sb.ToString
        End Function

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
