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

Imports ICSharpCode.SharpZipLib.Zip

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Packages
Imports DotNetNuke.Modules.Admin.ResourceInstaller
Imports DotNetNuke.UI.Skins

Namespace DotNetNuke.Modules.Admin.Packages

    ''' -----------------------------------------------------------------------------
    ''' Project	 : DotNetNuke
    ''' Class	 : UnInstall
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Supplies the functionality for uninstalling packages from the Portal
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    '''     [cnurse]   07/26/2007    Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class UnInstall
        Inherits PackageModuleBase

#Region "Members"

#End Region

#Region "Public Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the Return Url
        ''' </summary>
        ''' <history>
        '''     [cnurse]   07/31/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property ReturnURL() As String
            Get
                Dim TabID As Integer = PortalSettings.HomeTabId

                If Not Request.Params("rtab") Is Nothing Then
                    TabID = Integer.Parse(Request.Params("rtab"))
                End If
                Return NavigateURL(TabID)
            End Get
        End Property

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' This routine checks the Access Security
        ''' </summary>
        ''' <history>
        '''     [cnurse]   07/26/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub CheckSecurity()
            If Not UserInfo.IsSuperUser Then
                Response.Redirect(NavigateURL("Access Denied"), True)
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' This routine uninstalls the package
        ''' </summary>
        ''' <history>
        '''     [cnurse]   07/31/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub UnInstallPackage()
            phPaLogs.Visible = True
            Dim installer As New Installer(Package, Request.MapPath("."))
            installer.UnInstall()
            phPaLogs.Controls.Add(installer.InstallerInfo.Log.GetLogsTable)
        End Sub

#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The Page_Load runs when the page loads
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''     [cnurse]   07/26/2007    Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                CheckSecurity()

                ctlPackage.EditMode = UI.WebControls.PropertyEditorMode.View
                ctlPackage.DataSource = Package
                ctlPackage.DataBind()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The cmdUninstall_Click runs when the Uninstall Button is clicked
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''   [cnurse] 07/31/2007  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdUninstall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUninstall.Click
            Try
                UnInstallPackage()

                If phPaLogs.Controls.Count > 0 Then
                    tblLogs.Visible = True
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The cmdReturn_Click runs when the Return Button is clicked
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''   [cnurse] 16/9/2004  Updated for localization, Help and 508
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReturn1.Click, cmdReturn2.Click
            Response.Redirect(ReturnURL(), True)
        End Sub

#End Region

    End Class

End Namespace
