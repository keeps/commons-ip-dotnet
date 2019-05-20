Imports org.roda_project.commons_ip.model
Imports org.roda_project.commons_ip.utils.METSEnums

Class Presentation
    Inherits ExtendedPage

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub ExtendedPage_Loaded(sender As Object, e As RoutedEventArgs)
        CheckIfPageIsValid()
    End Sub

    Public Overrides Sub CheckIfPageIsValid()
        Me.IsValidPage = True
    End Sub

    Protected Overrides Sub UpdateModelObject()
    End Sub


End Class
