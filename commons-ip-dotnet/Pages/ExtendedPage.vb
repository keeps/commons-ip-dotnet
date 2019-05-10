Public MustInherit Class ExtendedPage
    Inherits System.Windows.Controls.Page

    Public Event ValidPageChanged(ByVal sender As Object, ByVal IsValid As Boolean)

    Private myIsValidPage As Boolean
    Protected Property IsValidPage As Boolean
        Get
            Return myIsValidPage
        End Get
        Set(value As Boolean)
            myIsValidPage = value
            RaiseEvent ValidPageChanged(Me, value)
        End Set
    End Property

    Private myPageDescription As String
    Public Property PageDescription As String
        Get
            Return myPageDescription
        End Get
        Set(value As String)
            myPageDescription = value
        End Set
    End Property

    ''' <summary>
    ''' This method calculate if current page are valid, and set the value into Property IsValidPage
    ''' </summary>
    Public MustOverride Sub CheckIfPageIsValid()

    Protected MustOverride Sub UpdateModelObject()

End Class
