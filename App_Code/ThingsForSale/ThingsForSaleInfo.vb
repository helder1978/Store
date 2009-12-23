Namespace YourCompany.Modules.ThingsForSale

    Public Class ThingsForSaleInfo

        Private _ModuleId As Integer
        Private _ID As Integer
        Private _UserID As Integer
        Private _Category As String
        Private _Description As String
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
        ' Gets and sets the Category
        ' </summary>
        Public Property Category() As String
            Get
                Return _Category
            End Get
            Set(ByVal value As String)
                _Category = value
            End Set
        End Property
        ' <summary>
        ' Gets and sets the Description
        ' </summary>
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property
        ' <summary>
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