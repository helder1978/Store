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

Imports System.IO
Imports System.Xml
Imports DotNetNuke.Entities.Modules.Definitions
Imports DotNetNuke.Modules.Admin.ResourceInstaller

Namespace DotNetNuke.Modules.Admin.ModuleDefinitions

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The ModuleDefValidator PortalModuleBase is used to validate user PAs before
	''' the are uploaded
	''' </summary>
	''' <returns></returns>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
	'''                       and localisation
	''' </history>
	''' -----------------------------------------------------------------------------
	Partial  Class ModuleDefValidator
        Inherits Entities.Modules.PortalModuleBase

#Region "Controls"


#End Region

#Region "Event Handlers"

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Page_Load runs when the control is loaded.
		''' </summary>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
		'''                       and localisation
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			'Put user code to initialize the page here
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' lnkValidate_Click runs when the Validate button is clicked
		''' </summary>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
		'''                       and localisation
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Sub lnkValidate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkValidate.Click
			If Page.IsPostBack Then
				If cmdBrowse.PostedFile.FileName <> "" Then
					Dim strExtension As String = Path.GetExtension(cmdBrowse.PostedFile.FileName)
					Dim Messages As New ArrayList
					Dim strMessage As String

					Dim postedFile As String = Path.GetFileName(cmdBrowse.PostedFile.FileName)
					If strExtension.ToLower = ".dnn" Then
						Dim xval As New ModuleDefinitionValidator
                        xval.Validate(cmdBrowse.PostedFile.InputStream)
                        If xval.Errors.Count > 0 Then
                            Messages.AddRange(xval.Errors)
                        Else
                            Messages.Add(String.Format(Services.Localization.Localization.GetString("Valid", Me.LocalResourceFile), postedFile))
                        End If
                    Else
						Messages.Add(String.Format(Services.Localization.Localization.GetString("Invalid", Me.LocalResourceFile), postedFile))
					End If
					lstResults.Visible = True
					lstResults.DataSource = Messages
					lstResults.DataBind()
				End If
			End If


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
