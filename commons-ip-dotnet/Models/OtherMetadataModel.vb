Imports log4net

Public Class OtherMetadataModel

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(OtherMetadataModel))

    ''' <summary>
    ''' All the files selected in OtherMetadata page
    ''' </summary>
    ''' <returns></returns>
    Public Property OtherMetadataFiles As List(Of DataRowFile)

    Public Sub New()
        Me.OtherMetadataFiles = New List(Of DataRowFile)
    End Sub

End Class
