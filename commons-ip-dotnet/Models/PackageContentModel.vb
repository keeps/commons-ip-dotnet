Imports org.roda_project.commons_ip.model.RepresentationContentType

Public Class PackageContentModel

    Public Property RepresentationName As String

    Public Property PackageContent As List(Of TreeViewPath)

    Public Property RepresentationContentType As RepresentationContentTypeEnum.__Enum

    Public Sub New()
        PackageContent = New List(Of TreeViewPath)
        RepresentationContentType = RepresentationContentTypeEnum.__Enum.MIXED
        RepresentationName = String.Empty
    End Sub


End Class
