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

    Private Sub RepresentationNameTextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            System.IO.Path.GetFullPath(RepresentationNameTextBox.Text)
            SaveButton.IsEnabled = True
        Catch ex As Exception
            SaveButton.IsEnabled = False
            HeaderControl.DescriptionHeader = "Please set a correct folder name, without invalid characters"
        End Try
    End Sub

End Class
