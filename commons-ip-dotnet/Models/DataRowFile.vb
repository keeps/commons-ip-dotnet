Imports System.ComponentModel
Imports System.IO

Public Class DataRowFile
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property IsSelected As Boolean

    Public ReadOnly Property FullName As String
        Get
            Return File.FullName
        End Get
    End Property

    Public ReadOnly Property FileName As String
        Get
            Return File.Name
        End Get
    End Property

    Private myRepresentationName As String
    Public Property RepresentationName As String
        Get
            Return myRepresentationName
        End Get
        Set(value As String)
            myRepresentationName = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RepresentationName"))
        End Set
    End Property

    Public Property File As FileInfo


    Public Sub New(ByVal path As String)
        Me.File = New FileInfo(path)
        Me.RepresentationName = "Representation"
        Me.IsSelected = True
    End Sub


    Public Sub New(ByVal path As String, ByVal isSelected As Boolean)
        Me.New(path)

        Me.IsSelected = isSelected
    End Sub

    Public Sub New(ByVal path As String, ByVal isSelected As Boolean, ByVal representatioName As String)
        Me.New(path)
        Me.IsSelected = isSelected
        Me.RepresentationName = representatioName
    End Sub

End Class
