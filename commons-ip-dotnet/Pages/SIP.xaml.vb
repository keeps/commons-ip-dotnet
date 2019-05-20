Imports System.IO

Class SIP
    Inherits ExtendedPage

    Public Property SipModel As SIPModel

    Public Property ProgressBarTotalItems As Integer
        Get
            Return If(ProgressBarStatus IsNot Nothing, ProgressBarStatus.Maximum, 0)
        End Get
        Set(value As Integer)
            If ProgressBarStatus IsNot Nothing Then
                ProgressBarStatus.Maximum = value
            End If
        End Set
    End Property

    Public Property ProgressBarCurrentStatus As Integer
        Get
            Return If(ProgressBarStatus IsNot Nothing, ProgressBarStatus.Value, 0)
        End Get
        Set(value As Integer)
            If ProgressBarStatus IsNot Nothing Then
                ProgressBarStatus.Value = value
            End If
        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SipModel = New SIPModel()
    End Sub

    Public Overrides Sub CheckIfPageIsValid()
        If String.IsNullOrEmpty(Me.SipModel.FullPath) Then
            IsValidPage = False
        Else
            IsValidPage = True
        End If
    End Sub

    Private Sub Image_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
        Dim dlg = New Microsoft.Win32.SaveFileDialog
        dlg.Filter = "Zip files (*.zip)|*.zip"
        dlg.FileName = "SIP_1"

        Dim result = dlg.ShowDialog()

        If result = True Then
            Dim filename As String = dlg.FileName
            Me.InputTextSaveAs.TextBox.Text = filename
            Me.SipModel.FullPath = filename
        End If

        CheckIfPageIsValid()
    End Sub

    Protected Overrides Sub UpdateModelObject()
        Throw New NotImplementedException()
    End Sub
End Class
