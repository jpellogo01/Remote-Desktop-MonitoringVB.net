Public Class DTO
    Public _m_ChildFormNumber As Integer
    Public _FirstName As String
    Public _LastName As String
    Public _Shift As Int16
    Public _AddEditType As String
    Public _EmployeeDataTable As DataTable
    Public _SearchQuery As String
    Public _SearchType As Int16
    Public _TheDate As DateTime
    Public _TimeIn As DateTime
    Public _TimeOut As DateTime
    Public _FromDate As DateTime
    Public _ToDate As DateTime
    Public Property m_ChildFormNumber() As Integer
        Get
            Return _m_ChildFormNumber
        End Get

        Set(ByVal value As Integer)
            _m_ChildFormNumber = value
        End Set
    End Property

    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get

        Set(ByVal value As String)
            _FirstName = value
        End Set
    End Property

    Public Property LastName() As String
        Get
            Return _LastName
        End Get

        Set(ByVal value As String)
            _LastName = value
        End Set
    End Property

    Public Property Shift() As Int16
        Get
            Return _Shift
        End Get

        Set(ByVal value As Int16)
            _Shift = value
        End Set
    End Property

    Public Property AddEditType() As String
        Get
            Return _AddEditType
        End Get

        Set(ByVal value As String)
            _AddEditType = value
        End Set
    End Property

    Public Property EmployeeDataTable() As DataTable
        Get
            Return _EmployeeDataTable
        End Get

        Set(ByVal value As DataTable)
            _EmployeeDataTable = value
        End Set
    End Property
    Public Property SearchQuery() As String
        Get
            Return _SearchQuery
        End Get

        Set(ByVal value As String)
            _SearchQuery = value
        End Set
    End Property
    Public Property SearchType() As Int16
        Get
            Return _SearchType
        End Get

        Set(ByVal value As Int16)
            _SearchType = value
        End Set
    End Property

    Public Property TheDate() As DateTime
        Get
            Return _TheDate
        End Get

        Set(ByVal value As DateTime)
            _TheDate = value
        End Set
    End Property
    Public Property TimeIn() As DateTime
        Get
            Return _TimeIn
        End Get

        Set(ByVal value As DateTime)
            _TimeIn = value
        End Set
    End Property
    Public Property TimeOut() As DateTime
        Get
            Return _TimeOut
        End Get

        Set(ByVal value As DateTime)
            _TimeOut = value
        End Set
    End Property
    Public Property FromDate() As DateTime
        Get
            Return _FromDate
        End Get

        Set(ByVal value As DateTime)
            _FromDate = value
        End Set
    End Property
    Public Property ToDate() As DateTime
        Get
            Return _ToDate
        End Get

        Set(ByVal value As DateTime)
            _ToDate = value
        End Set
    End Property
End Class
