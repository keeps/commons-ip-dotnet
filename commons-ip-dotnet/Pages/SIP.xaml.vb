Class SIP
    Inherits ExtendedPage

    ''' <summary>
    ''' Retrieve the selected file path
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property SaveFilePath As String
        Get
            Return FilePath.Text.Substring(0, FilePath.Text.LastIndexOf("\"))
        End Get
    End Property

    ''' <summary>
    ''' Open the windows dialog to choose the location and name of ZIP file
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SaveSIP_Click(sender As Object, e As RoutedEventArgs)

        Dim dlg = New Microsoft.Win32.SaveFileDialog
        dlg.Filter = "Zip files (*.zip)|*.zip"

        Dim result = dlg.ShowDialog()


        If result = True Then
            Dim filename As String = dlg.FileName
            FilePath.Text = filename
        End If

    End Sub

    ''' <summary>
    ''' Fired when textbox with path is changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FilePath_TextChanged(sender As Object, e As TextChangedEventArgs)
        CheckIfPageIsValid()
    End Sub


    Public Overrides Sub CheckIfPageIsValid()
        If String.IsNullOrEmpty(FilePath.Text) OrElse Not FilePath.Text.Contains(".zip") Then
            IsValidPage = False
        Else
            IsValidPage = True
        End If
    End Sub

    Protected Overrides Sub UpdateModelObject()
        Throw New NotImplementedException()
    End Sub
End Class
