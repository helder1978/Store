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
Imports DotNetNuke
Imports System.Web.UI
Imports System.Text.RegularExpressions

Namespace DotNetNuke.Modules.Html

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The HtmlModule Class provides the UI for displaying the Html
    ''' </summary>
       ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Partial Class HtmlModule
		Inherits Entities.Modules.PortalModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Methods"

        'Local tag cache to avoid multiple generations of the same tag
        Private tagCache As Hashtable

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Replaces all smart Tags in content with their appropriate values.
        ''' </summary>
        ''' <param name="content"></param>
        ''' <returns>Content with replaced Tags</returns>
        ''' <remarks>
        ''' deprecated, replaced by extended Tokenreplace
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------

        Private Function ProcessTags(ByVal content As String) As String
            Dim sb As New System.Text.StringBuilder
            Dim retVal As String = Null.NullString

            Dim position As Integer = 0

            'Find all tags used in text
            'The regular expression matches the tag including the square brackets:
            'in "aaa[bbb[ccc]ddd]eee" it matches only "[ccc]"
            If Not content Is Nothing Then
                For Each _match As Match In Regex.Matches(content, "\[([^\[]*?)\]")
                    'Append the text before the match to the result
                    sb.Append(content.Substring(position, _match.Index - position))

                    'Process the tag and append the output to the result
                    sb.Append(ProcessTag(_match.Value))

                    'Set the starting point for the next match
                    position = _match.Index + _match.Value.Length
                Next

                'Append the rest of the text to the result
                sb.Append(content.Substring(position))

                retVal = sb.ToString
            End If

            Return retVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Processes all occurrences of the given tag by the given value in given content
        ''' </summary>
        ''' <param name="tag"></param>
        ''' <returns>Content with replaced Tag</returns>
        ''' <remarks>
        ''' A hashtable is used to make sure each tag will only be processed once each load.
        ''' deprecated, replaced by extended Tokenreplace
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function ProcessTag(ByVal tag As String) As String

            'Store the tag in the result to keep the text if its not a smarttag
            Dim retval As String = tag
            Dim tagitems As String() = tag.Substring(1, tag.Length - 2).Trim().Split(" ".ToCharArray())
            'Ensure case independency
            tagitems(0) = tagitems(0).ToUpper()
            'Initialize cache if not created
            If tagCache Is Nothing Then tagCache = New Hashtable
            'If we have tag in cache, simply return the cached content
            If tagCache.ContainsKey(tag) Then
                retval = tagCache(tag).ToString()
            Else
                'Build tag content value if its a known tag
                Select Case tagitems(0)
                    Case "PORTAL.NAME"
                        retval = PortalSettings.PortalName
                        tagCache(tag) = retval
                    Case "DATE"
                        If tagitems.Length = 2 Then
                            Try
                                retval = DateTime.Now.ToString(tagitems(1))
                            Catch
                                retval = DateTime.Now.ToShortDateString()
                            End Try
                        Else
                            retval = DateTime.Now.ToShortDateString()
                        End If
                        tagCache(tag) = retval
                    Case "TIME"
                        If tagitems.Length = 2 Then
                            Try
                                retval = DateTime.Now.ToString(tagitems(1))
                            Catch
                                retval = DateTime.Now.ToShortTimeString()
                            End Try
                        Else
                            retval = DateTime.Now.ToShortTimeString()
                        End If
                        tagCache(tag) = retval
                End Select
            End If

            Return retval
        End Function



#End Region

#Region "Event Handlers"
        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)
            'If DotNetNuke.Entities.Host.HostSettings.GetHostSetting("UseFriendlyUrls") <> "Y" Then
            '    'allow for relative urls
            '    lblContent.UrlFormat = UI.WebControls.UrlFormatType.Relative
            'End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' [sleupold] 08/20/2007   Use of TokenReplace added
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                lblContent.EditEnabled = Me.IsEditable

                ' get HtmlText object
                Dim objHTML As New HtmlTextController
                Dim objText As HtmlTextInfo = objHTML.GetHtmlText(ModuleId)

                ' get default content from resource file
                Dim strContent As String = Localization.GetString("AddContentFromToolBar.Text", LocalResourceFile)
                If Entities.Portals.PortalSettings.GetSiteSetting(PortalId, "InlineEditorEnabled") = "False" Then
                    lblContent.EditEnabled = False
                    strContent = Localization.GetString("AddContentFromActionMenu.Text", LocalResourceFile)
                End If

                ' get html
                If Not objText Is Nothing Then
                    strContent = Server.HtmlDecode(CType(objText.DeskTopHTML, String))
                End If

                ' handle Smart Tags that might have been used
                Dim bytReplaceTokenType As Byte
                If IsNumeric(CType(Settings("TEXTHTML_ReplaceTokens"), String)) Then
                    bytReplaceTokenType = CType(Settings("TEXTHTML_ReplaceTokens"), Byte)
                End If
                Select Case bytReplaceTokenType
                    Case 1 'old replace // deprecated!
                        strContent = ProcessTags(strContent)
                        lblContent.EditEnabled = False
                    Case 2 'extended replace
                        Dim tr As New DotNetNuke.Services.Tokens.TokenReplace()
                        tr.AccessingUser = CType(HttpContext.Current.Items("UserInfo"), UserInfo)
                        tr.DebugMessages = Not IsTabPreview()
                        strContent = tr.ReplaceEnvironmentTokens(strContent)
                        lblContent.EditEnabled = False
                End Select

                If lblContent.EditEnabled Then
                    'localize toolbar
                    For Each objButton As DotNetNuke.UI.WebControls.DNNToolBarButton In Me.tbEIPHTML.Buttons
                        objButton.ToolTip = Services.Localization.Localization.GetString("cmd" & objButton.ToolTip, LocalResourceFile)
                    Next
                Else
                    Me.tbEIPHTML.Visible = False
                End If

                'add content to module
                lblContent.Controls.Add(New LiteralControl(DotNetNuke.Common.Globals.ManageUploadDirectory(strContent, PortalSettings.HomeDirectory)))

                ' menu action handler
                Dim ParentSkin As UI.Skins.Skin = UI.Skins.Skin.GetParentSkin(Me)
                'We should always have a ParentSkin, but need to make sure
                If Not ParentSkin Is Nothing Then
                    'Register our EventHandler as a listener on the ParentSkin so that it may tell us when a menu has been clicked.
                    ParentSkin.RegisterModuleActionEvent(Me.ModuleId, AddressOf ModuleAction_Click)
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' lblContent_UpdateLabel allows for inline editing of content
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub lblContent_UpdateLabel(ByVal source As Object, ByVal e As UI.WebControls.DNNLabelEditEventArgs) Handles lblContent.UpdateLabel

            ' get HtmlText object
            Dim objHTML As HtmlTextController = New HtmlTextController
            Dim objText As HtmlTextInfo = objHTML.GetHtmlText(ModuleId)

            ' check if this is a new module instance
            Dim blnIsNew As Boolean = False
            If objText Is Nothing Then
                objText = New HtmlTextInfo
                blnIsNew = True
            End If

            ' set content values
            objText.ModuleId = ModuleId
            objText.DeskTopHTML = e.Text
            objText.CreatedByUser = Me.UserId

            ' save the content
            If blnIsNew Then
                objHTML.AddHtmlText(objText)
            Else
                objHTML.UpdateHtmlText(objText)
            End If

            ' refresh cache
            SynchronizeModule()

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' ModuleAction_Click handles all ModuleAction events raised from the skin
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub ModuleAction_Click(ByVal sender As Object, ByVal e As Entities.Modules.Actions.ActionEventArgs)

            If e.Action.Url.Length > 0 Then
                Response.Redirect(e.Action.Url, True)
            End If

        End Sub

#End Region

#Region "Optional Interfaces"

        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Localization.GetString(Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl(), False, Security.SecurityAccessLevel.Edit, True, False)
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

