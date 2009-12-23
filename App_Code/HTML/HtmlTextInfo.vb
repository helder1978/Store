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

Namespace DotNetNuke.Modules.HTML

    Public Class HtmlTextInfo
        ' local property declarations
        Private _ModuleId As Integer
        Private _DeskTopHTML As String
        Private _DesktopSummary As String
        Private _CreatedByUser As Integer
        Private _CreatedDate As DateTime

        ' initialization
        Public Sub New()
        End Sub

        ' public properties
        Public Property ModuleId() As Integer
            Get
                Return _ModuleId
            End Get
            Set(ByVal Value As Integer)
                _ModuleId = Value
            End Set
        End Property

        Public Property DeskTopHTML() As String
            Get
                Return _DeskTopHTML
            End Get
            Set(ByVal Value As String)
                _DeskTopHTML = Value
            End Set
        End Property

        Public Property DesktopSummary() As String
            Get
                Return _DesktopSummary
            End Get
            Set(ByVal Value As String)
                _DesktopSummary = Value
            End Set
        End Property

        Public Property CreatedByUser() As Integer
            Get
                Return _CreatedByUser
            End Get
            Set(ByVal Value As Integer)
                _CreatedByUser = Value
            End Set
        End Property

        Public Property CreatedDate() As DateTime
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                _CreatedDate = Value
            End Set
        End Property

    End Class

End Namespace

