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
Imports DotNetNuke.Entities.Modules.Actions

Namespace DotNetNuke.UI.Containers
	''' -----------------------------------------------------------------------------
	''' Project	 : DotNetNuke
	''' Class	 : Containers.Icon
	''' 
	''' -----------------------------------------------------------------------------
	''' <summary>
	''' Contains the attributes of an Icon.  
	''' These are read into the PortalModuleBase collection as attributes for the icons within the module controls.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[sun1]	        2/1/2004	Created
	''' 	[Nik Kalyani]	10/15/2004	Replaced public members with properties and removed
	'''                                 brackets from property names
	''' </history>
	''' -----------------------------------------------------------------------------
	Partial  Class PrintModule
        Inherits ActionBase

        ' private members
        Private _printIcon As String

#Region "Public Members"
        Public Property PrintIcon() As String
            Get
                Return _printIcon
            End Get
            Set(ByVal Value As String)
                _printIcon = Value
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

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                For Each action As ModuleAction In Me.MenuActions
                    If action.CommandName = ModuleActionType.PrintModule Then
                        If action.Visible = True And PortalSecurity.HasNecessaryPermission(action.Secure, PortalSettings, ModuleConfiguration) = True Then
                            If (PortalSettings.UserMode = PortalSettings.Mode.Edit) Or (action.Secure = SecurityAccessLevel.Anonymous Or action.Secure = SecurityAccessLevel.View) Then

                                If PortalModule.ModuleConfiguration.DisplayPrint Then
                                    Dim ModuleActionIcon As New ImageButton
                                    If PrintIcon <> "" Then
                                        ModuleActionIcon.ImageUrl = PortalModule.ModuleConfiguration.ContainerPath.Substring(0, ModuleConfiguration.ContainerPath.LastIndexOf("/") + 1) & PrintIcon
                                    Else
                                        ModuleActionIcon.ImageUrl = "~/images/" & action.Icon
                                    End If
                                    ModuleActionIcon.ToolTip = action.Title
                                    ModuleActionIcon.ID = "ico" & action.ID.ToString
                                    ModuleActionIcon.CausesValidation = False

                                    AddHandler ModuleActionIcon.Click, AddressOf IconAction_Click

                                    Me.Controls.Add(ModuleActionIcon)
                                End If

                            End If
                        End If
                    End If
                Next

                ' set visibility
                If Me.Controls.Count > 0 Then
                    Me.Visible = True
                Else
                    Me.Visible = False
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub IconAction_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
            Try
                ProcessAction(DirectCast(sender, ImageButton).ID.Substring(3))
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

    End Class

End Namespace
