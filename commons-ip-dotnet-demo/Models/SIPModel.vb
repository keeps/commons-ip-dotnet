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

    ''' <summary>
    ''' Return only the filename with extension
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Filename As String
        Get
            Return FileInfo.Name
        End Get
    End Property

    ''' <summary>
    ''' Return the full directory path 
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property FullDirectoryName As String
        Get
            Return FileInfo.Directory.ToString
        End Get
    End Property

    ''' <summary>
    ''' Return the filename without extension
    ''' Replace the extension by empty string to calculate the filename without extension
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property FilenameWithoutExtension As String
        Get
            Return FileInfo.Name.Replace(FileInfo.Extension, String.Empty)
        End Get
    End Property

    ''' <summary>
    ''' Return the file extension
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property FileExtension As String
        Get
            Return FileInfo.Extension
        End Get
    End Property

End Class
