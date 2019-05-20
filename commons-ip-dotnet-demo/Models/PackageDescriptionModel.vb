Imports org.roda_project.commons_ip.utils.METSEnums

Public Class PackageDescriptionModel

    Public Property SIPID As String
    Public Property SIPDescription As String
    Public Property CreatorName As String
    Public Property CreatorRole As String
    Public Property CreatorType As CreatorType.__Enum

    Public Sub New()
        Me.SIPID = String.Empty
        Me.SIPDescription = String.Empty
        Me.CreatorName = String.Empty
        Me.CreatorRole = String.Empty
        Me.CreatorType = org.roda_project.commons_ip.utils.METSEnums.CreatorType.__Enum.OTHER
    End Sub

End Class
