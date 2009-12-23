'
' DotNetNuke - http://www.dotnetnuke.com
' Copyright (c) 2002-2005
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
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

Imports System
Imports System.Configuration
Imports System.Data
Imports DotNetNuke.Services.Search
Imports DotNetNuke
Imports System.XML
Imports System.Web

Namespace DotNetNuke.Modules.HTML

    ''' -----------------------------------------------------------------------------
    ''' Namespace:  DotNetNuke.Modules.Html
    ''' Project:    DotNetNuke
    ''' Class:      HtmlTextController
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The HtmlTextController is the Controller class for the HtmlText Module
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    '''		[cnurse]	11/15/2004	documented
    '''     [cnurse]    11/16/2004  Add/UpdateHtmlText separated into two methods,
    '''                             GetSearchItems modified to use decoded content
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class HtmlTextController
        Implements Entities.Modules.ISearchable
        Implements Entities.Modules.IPortable

        Private Const MAX_DESCRIPTION_LENGTH As Integer = 100

#Region "Public Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' AddHtmlText adds a HtmlTextInfo object to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="objText">The HtmlTextInfo object</param>
        ''' <history>
        '''		[cnurse]	11/15/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub AddHtmlText(ByVal objText As HtmlTextInfo)

            DataProvider.Instance().AddHtmlText(objText.ModuleId, objText.DeskTopHTML, objText.DesktopSummary, objText.CreatedByUser)

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetHtmlText gets the HtmlTextInfo object from the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="moduleId">The Id of the module</param>
        ''' <history>
        '''		[cnurse]	11/15/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetHtmlText(ByVal moduleId As Integer) As HtmlTextInfo

            Return CType(CBO.FillObject(DataProvider.Instance().GetHtmlText(moduleId), GetType(HtmlTextInfo)), HtmlTextInfo)

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' UpdateHtmlText saves the HtmlTextInfo object to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="objText">The HtmlTextInfo object</param>
        ''' <history>
        '''		[cnurse]	11/15/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub UpdateHtmlText(ByVal objText As HtmlTextInfo)

            DataProvider.Instance().UpdateHtmlText(objText.ModuleId, objText.DeskTopHTML, objText.DesktopSummary, objText.CreatedByUser)

        End Sub

#End Region

#Region "Optional Interfaces"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetSearchItems implements the ISearchable Interface
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        ''' <history>
        '''		[cnurse]	11/15/2004	documented
        '''     [skamphuis] 03/11/2006  FIX: HTM-2632. Enabled Search Summary.
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetSearchItems(ByVal ModInfo As Entities.Modules.ModuleInfo) As Services.Search.SearchItemInfoCollection Implements Entities.Modules.ISearchable.GetSearchItems

            Dim SearchItemCollection As New SearchItemInfoCollection

            Dim HtmlText As HtmlTextInfo = GetHtmlText(ModInfo.ModuleID)

            If Not HtmlText Is Nothing Then
                'DesktopHTML is encoded in the Database so Decode before Indexing
                Dim strDesktopHtml As String = HttpUtility.HtmlDecode(HtmlText.DeskTopHTML)

                'Get the description string
                Dim strDescription As String = HtmlUtils.Shorten(HtmlUtils.Clean(strDesktopHtml, False), MAX_DESCRIPTION_LENGTH, "...")

                Dim SearchItem As SearchItemInfo = New SearchItemInfo(ModInfo.ModuleTitle, strDescription, HtmlText.CreatedByUser, HtmlText.CreatedDate, ModInfo.ModuleID, "", HtmlText.DesktopSummary & " " & strDesktopHtml, "", Null.NullInteger)
                SearchItemCollection.Add(SearchItem)
            End If
            Return SearchItemCollection

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' ExportModule implements the IPortable ExportModule Interface
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModuleID">The Id of the module to be exported</param>
        ''' <history>
        '''		[cnurse]	11/15/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements Entities.Modules.IPortable.ExportModule

            Dim strXML As String = ""

            Dim objHtmlText As HtmlTextInfo = GetHtmlText(ModuleID)
            If Not objHtmlText Is Nothing Then
                strXML += "<htmltext>"
                strXML += "<desktophtml>" & Common.Utilities.XmlUtils.XMLEncode(objHtmlText.DeskTopHTML) & "</desktophtml>"
                strXML += "<desktopsummary>" & Common.Utilities.XmlUtils.XMLEncode(objHtmlText.DesktopSummary) & "</desktopsummary>"
                strXML += "</htmltext>"
            End If

            Return strXML

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' ImportModule implements the IPortable ImportModule Interface
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="ModuleID">The Id of the module to be imported</param>
        ''' <history>
        '''		[cnurse]	11/15/2004	documented
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements Entities.Modules.IPortable.ImportModule

            Dim xmlHtmlText As XmlNode = GetContent(Content, "htmltext")

            Dim objText As HtmlTextInfo = New HtmlTextInfo

            objText.ModuleId = ModuleID
            objText.CreatedByUser = UserId

            'Get the original item
            Dim objTextOld As HtmlTextInfo = Me.GetHtmlText(ModuleID)

            'See if there was an item already
            If objTextOld Is Nothing Then
                'Need to insert the imported item
                objText.DeskTopHTML = xmlHtmlText.SelectSingleNode("desktophtml").InnerText
                objText.DesktopSummary = xmlHtmlText.SelectSingleNode("desktopsummary").InnerText
                AddHtmlText(objText)
            Else
                'Need to appende the imported item to the existing item
                objText.DeskTopHTML = objTextOld.DeskTopHTML & xmlHtmlText.SelectSingleNode("desktophtml").InnerText
                objText.DesktopSummary = objTextOld.DesktopSummary & xmlHtmlText.SelectSingleNode("desktopsummary").InnerText
                UpdateHtmlText(objText)
            End If

        End Sub

#End Region

    End Class
End Namespace

