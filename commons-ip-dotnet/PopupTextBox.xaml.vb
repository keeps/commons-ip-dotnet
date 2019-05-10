Public Class PopupTextBox

    Public Property RepresentationName As String = String.Empty

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.RepresentationNameTextBox.Focus()
    End Sub

    Private Sub Save_Click(sender As Object, e As RoutedEventArgs)
        RepresentationName = RepresentationNameTextBox.Text
        Me.Close()
    End Sub

    Private Sub Cancel_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub


End Class
