Imports Microsoft.VisualBasic

Namespace nsolutions4u.Modules.DataAvailability

    Public Class DataAvailabilityInfo

        Private _ModuleId As Integer
        Private _ID As Integer
        Private _UserID As Integer
        Private _CountryID As Integer
        Private _CountryDesc As String
        Private _ServiceID As Integer
        Private _Service_Descr As String
        Private _Cycle As String
        Private _Status As String
        Private _PubDate As Date
        Private _PubDateStr As String
        Private _Price As Double

        ' initialization
        Public Sub New()
            MyBase.New()
        End Sub
        ' <summary>
        ' Gets and sets the Module Id
        ' </summary>
        Public Property ModuleId() As Integer
            Get
                Return _ModuleId
            End Get
            Set(ByVal value As Integer)
                _ModuleId = value
            End Set
        End Property
        ' <summary>
        ' Gets and sets the Item ID
        ' </summary>
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property
        ' <summary>
        ' Gets and sets the UserID
        ' </summary>
        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal value As Integer)
                _UserID = value
            End Set
        End Property
        ' <summary>
        ' Gets and sets the CountryID
        ' </summary>
        Public Property CountryID() As Integer
            Get
                Return _CountryID
            End Get
            Set(ByVal value As Integer)
                _CountryID = value
            End Set
        End Property
        ' <summary>
        ' Gets and sets the Country_Descr
        ' </summary>
        Public Property CountryDesc() As String
            Get
                Return _CountryDesc
            End Get
            Set(ByVal value As String)
                _CountryDesc = value
            End Set
        End Property        ' <summary>        
        ' <summary>
        ' Gets and sets the ServiceID
        ' </summary>
        Public Property ServiceID() As Integer
            Get
                Return _ServiceID
            End Get
            Set(ByVal value As Integer)
                _ServiceID = value
            End Set
        End Property
        ' <summary>
        ' Gets and sets the Service_Descr
        ' </summary>
        Public Property Service_Descr() As String
            Get
                Return _Service_Descr
            End Get
            Set(ByVal value As String)
                _Service_Descr = value
            End Set
        End Property        ' <summary>
        ' Gets and sets the Cycle
        ' </summary>
        Public Property Cycle() As String
            Get
                Return _Cycle
            End Get
            Set(ByVal value As String)
                _Cycle = value
            End Set
        End Property
        ' <summary>
        ' Gets and sets the Status
        ' </summary>
        Public Property Status() As String
            Get
                Return _Status
            End Get
            Set(ByVal value As String)
                _Status = value
            End Set
        End Property
        ' <summary>
        ' Gets and sets the PubDateStr
        ' </summary>
        Public Property PubDateStr() As String
            Get
                'Dim ukCulture As CultureInfo = New CultureInfo("en-US")
                'Dim _PubDate As DateTime = DateTime.Parse(PubDateStr(), ukCulture.DateTimeFormat)
                '_PubDateStr = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:dd/MM/yyyy}", _PubDateStr)
                'Format(CType(DataAvailabilityInfo.PubDateStr, Date), "MM/dd/yyyy")
                'Dim ukCulture As CultureInfo = New CultureInfo("en-GB")
                '_PubDateStr = Format(CType(_PubDateStr, Date), "dd/MM/yyyy")

                Try
                    Dim ukCulture As CultureInfo = New CultureInfo("en-GB")
                    _PubDateStr = Format(CType(_PubDateStr, Date), "dd/MM/yyyy")
                Catch ex As Exception

                End Try
                Return _PubDateStr
            End Get
            Set(ByVal value As String)
                _PubDateStr = value
            End Set
        End Property        ' <summary>
        ' <summary>
        ' Gets and sets the PubDate
        ' </summary>
        Public Property PubDate() As Date
            Get
                'Dim ukCulture As CultureInfo = New CultureInfo("en-US")
                'Dim _PubDate As DateTime = DateTime.Parse(PubDateStr(), ukCulture.DateTimeFormat)
                Return _PubDate
            End Get
            Set(ByVal value As Date)
                _PubDate = value
            End Set
        End Property
        ' Gets and sets the Price
        ' </summary>
        Public Property Price() As Double
            Get
                Return _Price
            End Get
            Set(ByVal value As Double)
                _Price = value
            End Set
        End Property

    End Class

End Namespace