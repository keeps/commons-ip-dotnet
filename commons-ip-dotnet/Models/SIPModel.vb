Imports System.IO

Public Class SIPModel

    Private FileInfo As FileInfo

    Private myFullPath As String
    Public Property FullPath As String
        Get
            Return myFullPath
        End Get
        Set(value As String)
            myFullPath = value
            FileInfo = New FileInfo(FullPath)
        End Set
    End Property

    Public ReadOnly Property Filename As String
        Get
            Return FileInfo.Name
        End Get
    End Property

    Public ReadOnly Property FullDirectoryName As String
        Get
            Return FileInfo.Directory.ToString
        End Get
    End Property

    Public ReadOnly Property FilenameWithoutExtension As String
        Get
            Return FileInfo.Name.Substring(0, FileInfo.Name.LastIndexOf("."))
        End Get
    End Property

    Public ReadOnly Property FileExtension As String
        Get
            Return FileInfo.Extension
        End Get
    End Property


End Class
