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

'Imports FreeTextBoxControls
Imports DotNetNuke

Namespace DotNetNuke.Modules.Html

    ''' -----------------------------------------------------------------------------
    ''' <summary>
	''' The EditHtml PortalModuleBase is used to manage Html
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
    ''' </history>
	''' -----------------------------------------------------------------------------
	Public Partial Class EditHtml
		Inherits Entities.Modules.PortalModuleBase

#Region "Private Members"

        Protected _isNew As Boolean = True

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                ' get HtmlText object
                Dim objHTML As New HtmlTextController
                Dim objText As HtmlTextInfo = objHTML.GetHtmlText(ModuleId)

                ' get the IsNew state from the ViewState
                _isNew = Convert.ToBoolean(ViewState("IsNew"))

                If Page.IsPostBack = False Then
                    If Not objText Is Nothing Then
                        ' initialize control values
                        teContent.Text = objText.DeskTopHTML
                        txtDesktopSummary.Text = Server.HtmlDecode(CType(objText.DesktopSummary, String))
                        _isNew = False
                    Else
                        ' get default content from resource file
                        teContent.Text = Localization.GetString("AddContent", LocalResourceFile)
                        txtDesktopSummary.Text = ""
                        _isNew = True
                    End If
                End If

                ' save the IsNew state to the ViewState
                ViewState("IsNew") = _isNew

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdCancel_Click runs when the cancel button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdPreview_Click runs when the preview button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
            Try
                Dim strDesktopHTML As String

                strDesktopHTML = teContent.Text

                lblPreview.Text = ManageUploadDirectory(Server.HtmlDecode(strDesktopHTML), PortalSettings.HomeDirectory)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdUpdate_Click runs when the update button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            Try
                ' create HTMLText object
                Dim objHTML As New HtmlTextController
                Dim objText As HtmlTextInfo = New HtmlTextInfo

                ' set content values
                objText.ModuleId = ModuleId
                objText.DeskTopHTML = teContent.Text
                objText.DesktopSummary = txtDesktopSummary.Text
                objText.CreatedByUser = Me.UserId

                ' save the content
                If _isNew Then
                    objHTML.AddHtmlText(objText)
                Else
                    objHTML.UpdateHtmlText(objText)
                End If

                ' refresh cache
                SynchronizeModule()

                ' redirect back to portal
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
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
