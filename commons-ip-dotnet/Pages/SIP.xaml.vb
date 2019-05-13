Imports System.IO

Class SIP
    Inherits ExtendedPage

    Public Property SipModel As SIPModel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SipModel = New SIPModel()
    End Sub

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
            Me.SipModel.FullPath = filename
        End If

        CheckIfPageIsValid()

    End Sub

    Public Overrides Sub CheckIfPageIsValid()
        If String.IsNullOrEmpty(Me.SipModel.FullPath) Then
            IsValidPage = False
        Else
            IsValidPage = True
        End If
    End Sub

    Protected Overrides Sub UpdateModelObject()
    End Sub

End Class
