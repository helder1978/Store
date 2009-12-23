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

Namespace DotNetNuke.UI.Containers
	''' -----------------------------------------------------------------------------
	''' Project	 : DotNetNuke
	''' Class	 : DotNetNuke.UI.Containers.Icon
	''' 
	''' -----------------------------------------------------------------------------
	''' <summary>
	''' Contains the attributes of an Icon.  
	''' These are read into the PortalModuleBase collection as attributes for the icons within the module controls.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[sun1]	    2/1/2004	Created
	''' 	[cniknet]	10/15/2004	Replaced public members with properties and removed
	'''                             brackets from property names
	''' </history>
	''' -----------------------------------------------------------------------------
	Partial  Class Icon

        Inherits UI.Skins.SkinObjectBase

		' private members
		Private _borderWidth As String

		' protected controls

#Region "Public Members"
		Public Property BorderWidth() As String
			Get
				Return _borderWidth
			End Get
			Set(ByVal Value As String)
				_borderWidth = Value
			End Set
		End Property
#End Region


#Region " Web Form Designer Generated Code "


		<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

		End Sub

		Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			'CODEGEN: This method call is required by the Web Form Designer
			'Do not modify it using the code editor.
			InitializeComponent()
		End Sub

#End Region

		'*******************************************************
		'
		' The Page_Load server event handler on this page is used
		' to populate the control information
		'
		'*******************************************************

		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try

                ' public attributes
                If BorderWidth <> "" Then
                    imgIcon.BorderWidth = System.Web.UI.WebControls.Unit.Parse(BorderWidth)
                End If

                Me.Visible = False
                Dim objPortalModule As Entities.Modules.PortalModuleBase = Container.GetPortalModuleBase(Me)
                If (Not objPortalModule Is Nothing) AndAlso (Not objPortalModule.ModuleConfiguration Is Nothing) Then
                    If objPortalModule.ModuleConfiguration.IconFile <> "" Then
                        If objPortalModule.ModuleConfiguration.IconFile.StartsWith("~/") Then
                            imgIcon.ImageUrl = objPortalModule.ModuleConfiguration.IconFile
                        Else
                            If IsAdminControl() Then
                                imgIcon.ImageUrl = objPortalModule.TemplateSourceDirectory & "/" & objPortalModule.ModuleConfiguration.IconFile
                            Else
                                If objPortalModule.PortalSettings.ActiveTab.IsAdminTab Then
                                    imgIcon.ImageUrl = "~/images/" & objPortalModule.ModuleConfiguration.IconFile
                                Else
                                    imgIcon.ImageUrl = objPortalModule.PortalSettings.HomeDirectory & objPortalModule.ModuleConfiguration.IconFile
                                End If
                            End If
                        End If
                        imgIcon.AlternateText = objPortalModule.ModuleConfiguration.ModuleTitle
                        Me.Visible = True
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
		End Sub

	End Class

End Namespace
