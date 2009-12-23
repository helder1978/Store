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
Imports System.Security
Imports System.Security.Principal
Imports System.Threading
Imports System.Web.Security
Imports System.IO

Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Services.Upgrade


Namespace DotNetNuke.Common

    ''' -----------------------------------------------------------------------------
    ''' Project	 : DotNetNuke
    ''' Class	 : Global
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[sun1]	1/18/2004	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class [Global]
        Inherits System.Web.HttpApplication

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' CheckVersion determines whether the App is synchronized with the DB
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''     [cnurse]    2/17/2005   created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub CheckVersion()
            Dim Server As HttpServerUtility = HttpContext.Current.Server
            Dim Request As HttpRequest = HttpContext.Current.Request
            Dim Response As HttpResponse = HttpContext.Current.Response

            Dim AutoUpgrade As Boolean
            If Config.GetSetting("AutoUpgrade") Is Nothing Then
                AutoUpgrade = True
            Else
                AutoUpgrade = Boolean.Parse(Config.GetSetting("AutoUpgrade"))
            End If

            Dim UseWizard As Boolean
            If Config.GetSetting("UseInstallWizard") Is Nothing Then
                UseWizard = True
            Else
                UseWizard = Boolean.Parse(Config.GetSetting("UseInstallWizard"))
            End If

            'Determine the Upgrade status and redirect to Install.aspx
            Select Case GetUpgradeStatus()
                Case Globals.UpgradeStatus.Install
                    If AutoUpgrade Then
                        If UseWizard Then
                            Response.Redirect("~/Install/InstallWizard.aspx")
                        Else
                            Response.Redirect("~/Install/Install.aspx?mode=install")
                        End If
                    Else
                        CreateUnderConstructionPage()
                        Response.Redirect("~/Install/UnderConstruction.htm")
                    End If
                Case Globals.UpgradeStatus.Upgrade
                    If AutoUpgrade Then
                        Response.Redirect("~/Install/Install.aspx?mode=upgrade")
                    Else
                        CreateUnderConstructionPage()
                        Response.Redirect("~/Install/UnderConstruction.htm")
                    End If
                Case Globals.UpgradeStatus.Error
                    CreateUnderConstructionPage()
                    Response.Redirect("~/Install/UnderConstruction.htm")
            End Select
        End Sub

        Private Sub CreateUnderConstructionPage()
            ' create an UnderConstruction page if it does not exist already
            If Not File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Install/UnderConstruction.htm")) Then
                If File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Install/UnderConstruction.template.htm")) Then
                    File.Copy(System.Web.HttpContext.Current.Server.MapPath("~/Install/UnderConstruction.template.htm"), System.Web.HttpContext.Current.Server.MapPath("~/Install/UnderConstruction.htm"))
                End If
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LogEnd logs the Application Start Event
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''     [cnurse]    1/28/2005   Moved back to App_End from Logging Module
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub LogEnd()
            Try
                Dim shutdownReason As System.Web.ApplicationShutdownReason = System.Web.Hosting.HostingEnvironment.ShutdownReason
                Dim shutdownDetail As String = ""
                Select Case shutdownReason
                    Case ApplicationShutdownReason.BinDirChangeOrDirectoryRename
                        shutdownDetail = "The AppDomain shut down because of a change to the Bin folder or files contained in it."
                    Case ApplicationShutdownReason.BrowsersDirChangeOrDirectoryRename
                        shutdownDetail = "The AppDomain shut down because of a change to the App_Browsers folder or files contained in it."
                    Case ApplicationShutdownReason.ChangeInGlobalAsax
                        shutdownDetail = "The AppDomain shut down because of a change to Global.asax."
                    Case ApplicationShutdownReason.ChangeInSecurityPolicyFile
                        shutdownDetail = "The AppDomain shut down because of a change in the code access security policy file."
                    Case ApplicationShutdownReason.CodeDirChangeOrDirectoryRename
                        shutdownDetail = "The AppDomain shut down because of a change to the App_Code folder or files contained in it."
                    Case ApplicationShutdownReason.ConfigurationChange
                        shutdownDetail = "The AppDomain shut down because of a change to the application level configuration."
                    Case ApplicationShutdownReason.HostingEnvironment
                        shutdownDetail = "The AppDomain shut down because of the hosting environment."
                    Case ApplicationShutdownReason.HttpRuntimeClose
                        shutdownDetail = "The AppDomain shut down because of a call to Close."
                    Case ApplicationShutdownReason.IdleTimeout
                        shutdownDetail = "The AppDomain shut down because of the maximum allowed idle time limit."
                    Case ApplicationShutdownReason.InitializationError
                        shutdownDetail = "The AppDomain shut down because of an AppDomain initialization error."
                    Case ApplicationShutdownReason.MaxRecompilationsReached
                        shutdownDetail = "The AppDomain shut down because of the maximum number of dynamic recompiles of resources limit."
                    Case ApplicationShutdownReason.PhysicalApplicationPathChanged
                        shutdownDetail = "The AppDomain shut down because of a change to the physical path for the application."
                    Case ApplicationShutdownReason.ResourcesDirChangeOrDirectoryRename
                        shutdownDetail = "The AppDomain shut down because of a change to the App_GlobalResources folder or files contained in it."
                    Case ApplicationShutdownReason.UnloadAppDomainCalled
                        shutdownDetail = "The AppDomain shut down because of a call to UnloadAppDomain."
                    Case Else
                        shutdownDetail = "No shutdown reason provided."
                End Select

                Dim objEv As New EventLogController
                Dim objEventLogInfo As New LogInfo
                objEventLogInfo.BypassBuffering = True
                objEventLogInfo.LogTypeKey = Services.Log.EventLog.EventLogController.EventLogType.APPLICATION_SHUTTING_DOWN.ToString
                objEventLogInfo.AddProperty("Shutdown Details", shutdownDetail)

                objEv.AddLog(objEventLogInfo)
            Catch exc As Exception
                LogException(exc)
            End Try

            ' purge log buffer
            LoggingProvider.Instance.PurgeLogBuffer()
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' CacheMappedDirectory caches the Portal Mapped Directory(s)
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''     [cnurse]    1/27/2005   Moved back to App_Start from Caching Module
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub CacheMappedDirectory()
            'Cache the mapped physical home directory for each portal
            'so the mapped directories are available outside
            'of httpcontext.   This is especially necessary
            'when the /Portals or portal home directory has been 
            'mapped in IIS to another directory or server.
            Dim objFolderController As New Services.FileSystem.FolderController
            Dim objPortalController As New PortalController
            Dim arrPortals As ArrayList = objPortalController.GetPortals()
            Dim i As Integer
            For i = 0 To arrPortals.Count - 1
                Dim objPortalInfo As PortalInfo = CType(arrPortals(i), PortalInfo)
                objFolderController.SetMappedDirectory(objPortalInfo, HttpContext.Current)
            Next

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' StopScheduler stops the Scheduler
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''     [cnurse]    1/28/2005   Moved back to App_End from Scheduling Module
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub StopScheduler()
            ' stop scheduled jobs
            Scheduling.SchedulingProvider.Instance.Halt("Stopped by Application_End")
        End Sub

#End Region

#Region "Public Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LogStart logs the Application Start Event
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''     [cnurse]    1/27/2005   Moved back to App_Start from Logging Module
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Sub LogStart()
            Dim objEv As New EventLogController
            Dim objEventLogInfo As New LogInfo
            objEventLogInfo.BypassBuffering = True
            objEventLogInfo.LogTypeKey = Services.Log.EventLog.EventLogController.EventLogType.APPLICATION_START.ToString
            objEv.AddLog(objEventLogInfo)

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' StartScheduler starts the Scheduler
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''     [cnurse]    1/27/2005   Moved back to App_Start from Scheduling Module
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Sub StartScheduler()
            ' instantiate APPLICATION_START scheduled jobs
            If Services.Scheduling.SchedulingProvider.SchedulerMode = Scheduling.SchedulerMode.TIMER_METHOD Then
                Dim scheduler As Scheduling.SchedulingProvider = Scheduling.SchedulingProvider.Instance()
                scheduler.RunEventSchedule(Scheduling.EventName.APPLICATION_START)
                Dim newThread As New Threading.Thread(AddressOf Scheduling.SchedulingProvider.Instance.Start)
                newThread.IsBackground = True
                newThread.Start()
            End If
        End Sub

#End Region

#Region "Application Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Application_Start
        ''' Executes on the first web request into the portal application, 
        ''' when a new DLL is deployed, or when web.config is modified.
        ''' </summary>
        ''' <param name="Sender"></param>
        ''' <param name="E"></param>
        ''' <remarks>
        ''' - global variable initialization
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Application_Start(ByVal Sender As Object, ByVal E As EventArgs)

            'global variable initialization
            Dim Server As HttpServerUtility = HttpContext.Current.Server
            If Config.GetSetting("ServerName") = "" Then
                ServerName = Server.MachineName
            Else
                ServerName = Config.GetSetting("ServerName")
            End If

            If HttpContext.Current.Request.ApplicationPath = "/" Then
                If Config.GetSetting("InstallationSubfolder") = "" Then
                    ApplicationPath = ""
                Else
                    ApplicationPath = Config.GetSetting("InstallationSubfolder") & "/"
                End If
            Else
                ApplicationPath = HttpContext.Current.Request.ApplicationPath
            End If
            ApplicationMapPath = System.AppDomain.CurrentDomain.BaseDirectory.Substring(0, System.AppDomain.CurrentDomain.BaseDirectory.Length - 1)
            ApplicationMapPath = ApplicationMapPath.Replace("/", "\")

            HostPath = ApplicationPath & "/Portals/_default/"
            HostMapPath = Server.MapPath(HostPath)

            'Don't process some of the AppStart methods if we are installing
            If Not HttpContext.Current.Request.Url.LocalPath.EndsWith("InstallWizard.aspx") Then
                'Check whether the current App Version is the same as the DB Version
                CheckVersion()

                'Cache Mapped Directory(s)
                CacheMappedDirectory()

                'Start Scheduler
                StartScheduler()
            End If

            'Try and Log the App Start and process the EventQueue (may fail if the database does not exist yet)
            Try
                'log APPLICATION_START event
                LogStart()

                'Process any messages in the EventQueue for the Application_Start event
                EventQueue.EventQueueController.ProcessMessages("Application_Start")
            Catch ex As Exception

            End Try

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Application_End
        ''' Executes when the Application times out
        ''' </summary>
        ''' <param name="Sender"></param>
        ''' <param name="E"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Application_End(ByVal Sender As Object, ByVal E As EventArgs)

            ' log APPLICATION_END event
            LogEnd()

            ' stop scheduled jobs
            StopScheduler()

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Application_BeginRequest
        ''' Executes when the request is initiated
        ''' </summary>
        ''' <param name="Sender"></param>
        ''' <param name="E"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)

            'First check if we are upgrading/installing
            If Request.Url.LocalPath.ToLower.EndsWith("install.aspx") OrElse Request.Url.LocalPath.ToLower.EndsWith("installwizard.aspx") Then
                Exit Sub
            End If

            Try

                If Services.Scheduling.SchedulingProvider.SchedulerMode = Scheduling.SchedulerMode.REQUEST_METHOD _
                AndAlso Services.Scheduling.SchedulingProvider.ReadyForPoll Then

                    Dim scheduler As Scheduling.SchedulingProvider = Scheduling.SchedulingProvider.Instance
                    Dim RequestScheduleThread As Threading.Thread
                    RequestScheduleThread = New Threading.Thread(AddressOf scheduler.ExecuteTasks)
                    RequestScheduleThread.IsBackground = True
                    RequestScheduleThread.Start()

                    Services.Scheduling.SchedulingProvider.ScheduleLastPolled = Now

                End If

            Catch exc As Exception
                LogException(exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace
