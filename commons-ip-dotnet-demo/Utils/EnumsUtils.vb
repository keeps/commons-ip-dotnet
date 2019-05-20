Imports org.roda_project.commons_ip.model
Imports org.roda_project.commons_ip.model.MetadataType
Imports org.roda_project.commons_ip.model.RepresentationContentType
Imports org.roda_project.commons_ip.utils.METSEnums

Public Class EnumsUtils

    Public Shared Function GetMetadataType(ByVal value As String) As MetadataTypeEnum.__Enum
        Dim result As MetadataTypeEnum.__Enum

        If value IsNot Nothing Then
            result = CType([Enum].Parse(GetType(MetadataTypeEnum.__Enum), value), MetadataTypeEnum.__Enum)
        End If

        Return result
    End Function

    Public Shared Function GetCreatorType(ByVal value As String) As CreatorType.__Enum
        Dim result As CreatorType.__Enum = CreatorType.__Enum.OTHER

        If value IsNot Nothing Then
            result = CType([Enum].Parse(GetType(CreatorType.__Enum), value), CreatorType.__Enum)
        End If

        Return result
    End Function

    Friend Shared Function GetRepresentationContentType(value As String) As RepresentationContentTypeEnum.__Enum
        Dim result As RepresentationContentTypeEnum.__Enum = RepresentationContentTypeEnum.__Enum.OTHER

        If value IsNot Nothing Then
            result = CType([Enum].Parse(GetType(RepresentationContentTypeEnum.__Enum), value), RepresentationContentTypeEnum.__Enum)
        End If

        Return result
    End Function
End Class
