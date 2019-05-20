Imports System.IO

Public Class TreeViewPath

    Private Path As FileInfo


    Private myImageFilename As String
    Public Property ImageFilename As String
        Get
            Return myImageFilename
        End Get
        Set(value As String)
            myImageFilename = value
        End Set
    End Property

    Private myOriginPath As String
    Public Property OriginPath As String
        Get
            Return myOriginPath
        End Get
        Set(value As String)
            myOriginPath = value
        End Set
    End Property

    Private myFilename As String
    Public Property Filename As String
        Get
            Return myFilename
        End Get
        Set(value As String)
            myFilename = value
        End Set
    End Property

    Private myFullName As String
    Public Property FullName As String
        Get
            Return myFullName
        End Get
        Set(value As String)
            myFullName = value
        End Set
    End Property

    Private myIsDirectory As Boolean
    Public Property IsDirectory As Boolean
        Get
            Return myIsDirectory
        End Get
        Set(value As Boolean)
            myIsDirectory = value
        End Set
    End Property

    Private myChildren As List(Of TreeViewPath)
    Public Property Children As List(Of TreeViewPath)
        Get
            If myChildren Is Nothing Then
                myChildren = New List(Of TreeViewPath)
            End If
            Return myChildren
        End Get
        Set(value As List(Of TreeViewPath))
            myChildren = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal originPath As String, ByVal filename As String, ByVal fullPath As String, ByVal isDirectory As Boolean)
        myOriginPath = originPath
        myFilename = filename
        myIsDirectory = isDirectory
        myFullName = fullPath
        myImageFilename = If(isDirectory, ControlsUtils.FOLDER_FILENAME, ControlsUtils.FILE_FILENAME)
    End Sub

    Public Function RetrieveRelativePath() As List(Of String)
        Dim result As New List(Of String)
        Dim found As Boolean = False
        Dim pathToAnalyze As String = FullName.Replace(Filename, "")

        If Not OriginPath.Equals(FullName) Then
            If Not IsDirectory Then
                While Not found
                    'Remove last /
                    If pathToAnalyze.EndsWith(System.IO.Path.DirectorySeparatorChar) Then
                        pathToAnalyze = pathToAnalyze.Substring(0, pathToAnalyze.Length - 1)
                    End If

                    If pathToAnalyze.Equals(OriginPath) Then
                        found = True
                    Else
                        result.Add(pathToAnalyze.Substring(pathToAnalyze.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1, pathToAnalyze.Length - pathToAnalyze.LastIndexOf(System.IO.Path.DirectorySeparatorChar) - 1))
                        pathToAnalyze = pathToAnalyze.Substring(0, pathToAnalyze.LastIndexOf(System.IO.Path.DirectorySeparatorChar))
                    End If
                End While
            End If

            result.Reverse()
        End If

        Return result
    End Function


End Class
