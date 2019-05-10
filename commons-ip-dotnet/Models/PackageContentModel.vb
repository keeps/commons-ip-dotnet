Imports org.roda_project.commons_ip.model.RepresentationContentType

Public Class PackageContentModel

    Public Property PackageContent As List(Of DataRowFile)

    Public Property RepresentationContentType As RepresentationContentTypeEnum.__Enum

    Public Sub New()
        PackageContent = New List(Of DataRowFile)
        RepresentationContentType = RepresentationContentTypeEnum.__Enum.MIXED
    End Sub

    ''' <summary>
    ''' Retrieve the dictionary with representation -> List of files
    ''' </summary>
    ''' <returns></returns>
    Public Function RetrieveFilesByRepresentationName() As Dictionary(Of String, List(Of DataRowFile))

        Dim result As New Dictionary(Of String, List(Of DataRowFile))

        If PackageContent IsNot Nothing AndAlso PackageContent.Count > 0 Then
            For Each file In PackageContent
                If result.ContainsKey(file.RepresentationName) Then
                    result(file.RepresentationName).Add(file)
                Else
                    result.Add(file.RepresentationName, New List(Of DataRowFile)({file}))
                End If
            Next
        End If

        Return result

    End Function

End Class
