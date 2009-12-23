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
Imports System.Runtime.Serialization.Formatters
Imports DotNetNuke

Namespace DotNetNuke.Modules.HTML

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Settings ModuleSettingsBase is used to manage the 
    ''' settings for the HTML Module
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[leupold]	    08/12/2007	created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Public Class Settings
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadSettings loads the settings from the Database and displays them
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''		[sleupold]	08/12/2007	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub LoadSettings()
            Try
                If Page.IsPostBack Then
                    'nothing
                ElseIf IsNumeric(CType(ModuleSettings("TEXTHTML_ReplaceTokens"), String)) Then
                    rblstReplaceTokens.SelectedValue = CType(ModuleSettings("TEXTHTML_ReplaceTokens"), Byte)
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' UpdateSettings saves the modified settings to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''		[sleupold]	08/12/2007	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
                Dim objModule As DotNetNuke.Entities.Modules.ModuleInfo = objModules.GetModule(ModuleId, TabId, False)

                'Update Tab Module Settings
                objModules.UpdateModuleSetting(ModuleId, "TEXTHTML_ReplaceTokens", rblstReplaceTokens.SelectedValue)

                'make sure, that for tokenReplace, caching is disabled
                If rblstReplaceTokens.SelectedValue = 2 And objModule.CacheTime <> "0" Then objModule.CacheTime = 0

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

    End Class

End Namespace


