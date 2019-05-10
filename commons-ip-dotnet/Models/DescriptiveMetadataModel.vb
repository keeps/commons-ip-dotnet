Imports System.IO
Imports log4net
Imports org.roda_project.commons_ip.model.MetadataType

Public Class DescriptiveMetadataModel

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(DescriptiveMetadataModel))

    Public Property DescriptiveMetadataFile As List(Of DataRowFile)

    Public DescriptiveMetadataType As MetadataTypeEnum.__Enum

    Public Sub New()
        Me.DescriptiveMetadataFile = New List(Of DataRowFile)
        Me.DescriptiveMetadataType = MetadataTypeEnum.__Enum.OTHER
    End Sub

End Class
