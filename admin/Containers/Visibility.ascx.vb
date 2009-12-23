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
Imports DotNetNuke.Services.Localization

Namespace DotNetNuke.UI.Containers

    ''' -----------------------------------------------------------------------------
    ''' Project	 : DotNetNuke
    ''' Class	 : Containers.Visibility
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Handles the events for collapsing and expanding modules, 
    ''' Showing or hiding admin controls when preview is checked
    ''' if personalization of the module container and title is allowed for that module.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[sun1]	    2/1/2004	Created
    ''' 	[cniknet]	10/15/2004	Replaced public members with properties and removed
    '''                             brackets from property names
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Visibility

        Inherits UI.Skins.SkinObjectBase

        ' private members
        Private _borderWidth As String
        Private _minIcon As String
        Private _maxIcon As String
        Private _animationFrames As Integer = 5

        Private m_objPortalModule As Entities.Modules.PortalModuleBase
        Private m_pnlModuleContent As Panel

#Region "Public Members"
        Public Property BorderWidth() As String
            Get
                Return _borderWidth
            End Get
            Set(ByVal Value As String)
                _borderWidth = Value
            End Set
        End Property

        Public Property minIcon() As String
            Get
                Return _minIcon
            End Get
            Set(ByVal Value As String)
                _minIcon = Value
            End Set
        End Property

        Public Property MaxIcon() As String
            Get
                Return _maxIcon
            End Get
            Set(ByVal Value As String)
                _maxIcon = Value
            End Set
        End Property

        Public Property AnimationFrames() As Integer
            Get
                Return _animationFrames
            End Get
            Set(ByVal Value As Integer)
                _animationFrames = Value
            End Set
        End Property

#End Region

        Public ReadOnly Property ResourceFile() As String
            Get
                Return Services.Localization.Localization.GetResourceFile(Me, "Visibility.ascx")
            End Get
        End Property

        Private ReadOnly Property PortalModule() As Entities.Modules.PortalModuleBase
            Get
                If m_objPortalModule Is Nothing Then
                    m_objPortalModule = Container.GetPortalModuleBase(Me)
                End If
                Return m_objPortalModule
            End Get
        End Property

        Private ReadOnly Property ModuleContent() As Panel
            Get
                If m_pnlModuleContent Is Nothing Then
                    Dim objCtl As Control = Me.Parent.FindControl("ModuleContent")
                    If Not objCtl Is Nothing Then m_pnlModuleContent = CType(objCtl, Panel)
                End If
                Return m_pnlModuleContent
            End Get
        End Property

        Public Property ContentVisible() As Boolean
            Get
                Select Case PortalModule.ModuleConfiguration.Visibility
                    Case Entities.Modules.VisibilityState.Maximized, Entities.Modules.VisibilityState.Minimized
                        Return DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxContentVisibile(cmdVisibility, PortalModule.ModuleId, PortalModule.ModuleConfiguration.Visibility = Entities.Modules.VisibilityState.Minimized, Utilities.DNNClientAPI.MinMaxPersistanceType.Cookie)
                    Case Else
                        Return True
                End Select
            End Get
            Set(ByVal Value As Boolean)
                DotNetNuke.UI.Utilities.DNNClientAPI.MinMaxContentVisibile(cmdVisibility, PortalModule.ModuleId, PortalModule.ModuleConfiguration.Visibility = Entities.Modules.VisibilityState.Minimized, Utilities.DNNClientAPI.MinMaxPersistanceType.Cookie) = Value
            End Set
        End Property

        Private ReadOnly Property IsAdminControlOrTab() As Boolean
            Get
                Return (IsAdminControl() OrElse PortalModule.PortalSettings.ActiveTab.IsAdminTab)
            End Get
        End Property

        Private ReadOnly Property ModulePath() As String
            Get
                Return PortalModule.ModuleConfiguration.ContainerPath.Substring(0, PortalModule.ModuleConfiguration.ContainerPath.LastIndexOf("/") + 1)
            End Get
        End Property

        Private ReadOnly Property MinIconLoc() As String
            Get
                If minIcon <> "" Then
                    Return ModulePath & minIcon
                Else
                    'Return "~/images/min.gif"
                    Return Common.Globals.ApplicationPath & "/images/min.gif"      'is ~/ the same as ApplicationPath in all cases?
                End If
            End Get
        End Property

        Private ReadOnly Property MaxIconLoc() As String
            Get
                If MaxIcon <> "" Then
                    Return ModulePath & MaxIcon
                Else
                    'Return "~/images/max.gif"
                    Return Common.Globals.ApplicationPath & "/images/max.gif"     'is ~/ the same as ApplicationPath in all cases?
                End If
            End Get
        End Property

        ' protected controls

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
        ' to populate the role information for the page
        '
        '*******************************************************

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack Then

                    ' public attributes
                    If BorderWidth <> "" Then
                        cmdVisibility.BorderWidth = System.Web.UI.WebControls.Unit.Parse(BorderWidth)
                    End If

                    If Not PortalModule.ModuleConfiguration Is Nothing Then
                        ' check if Personalization is allowed
                        If PortalModule.ModuleConfiguration.Visibility = Entities.Modules.VisibilityState.None Then
                            cmdVisibility.Enabled = False
                            cmdVisibility.Visible = False
                        End If

                        If PortalModule.ModuleConfiguration.Visibility = Entities.Modules.VisibilityState.Minimized Then
                            'if visibility is set to minimized, then the client needs to set the cookie for maximized only and delete the cookie for minimized,
                            'instead of the opposite.  We need to notify the client of this
                            DotNetNuke.UI.Utilities.ClientAPI.RegisterClientVariable(Me.Page, "__dnn_" & PortalModule.ModuleId.ToString & ":defminimized", "true", True)
                        End If

                        If IsAdminControlOrTab = False Then
                            If cmdVisibility.Enabled Then
                                If Not ModuleContent Is Nothing Then
                                    'EnableMinMax now done in prerender
                                Else
                                    Me.Visible = False
                                End If
                            End If
                        Else
                            Me.Visible = False
                        End If
                    Else
                        Me.Visible = False
                    End If
                Else
                    'since we disabled viewstate on the cmdVisibility control we need to check to see if we need hide this on postbacks as well
                    If Not PortalModule.ModuleConfiguration Is Nothing Then
                        If PortalModule.ModuleConfiguration.Visibility = Entities.Modules.VisibilityState.None Then
                            cmdVisibility.Enabled = False
                            cmdVisibility.Visible = False
                        End If
                    End If
                End If

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            If Not ModuleContent Is Nothing AndAlso Not PortalModule Is Nothing AndAlso IsAdminControlOrTab = False Then
                Select Case PortalModule.ModuleConfiguration.Visibility
                    Case Entities.Modules.VisibilityState.Maximized, Entities.Modules.VisibilityState.Minimized
                        DotNetNuke.UI.Utilities.DNNClientAPI.EnableMinMax(cmdVisibility, ModuleContent, PortalModule.ModuleId, PortalModule.ModuleConfiguration.Visibility = Entities.Modules.VisibilityState.Minimized, MinIconLoc, MaxIconLoc, Utilities.DNNClientAPI.MinMaxPersistanceType.Cookie, Me.AnimationFrames)
                End Select
            End If

        End Sub

        Private Overloads Sub cmdVisibility_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdVisibility.Click
            Try

                If Not ModuleContent Is Nothing Then
                    If ModuleContent.Visible = True Then
                        Me.ContentVisible = False
                    Else
                        Me.ContentVisible = True
                    End If

                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

    End Class

End Namespace
