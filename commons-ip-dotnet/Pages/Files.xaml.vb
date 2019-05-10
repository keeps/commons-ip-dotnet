Imports System.IO
Imports log4net
Imports org.roda_project.commons_ip.model
Imports org.roda_project.commons_ip.model.RepresentationStatus

Partial Public Class Files
    Inherits ExtendedPage

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(Files))

    Public ReadOnly Property SelectedFiles As List(Of FileInfo)
        Get
            Dim result As New List(Of FileInfo)
            For Each item In FilesGrid.Items
                log.Debug("Selected file in metadata " & CType(item, FileInfo).FullName)
                result.Add(item)
            Next
            Return result
        End Get
    End Property

    Public ReadOnly Property SelectedRepresentationStatus As RepresentationStatusEnum.__Enum
        Get
            Dim result As RepresentationStatusEnum.__Enum
            Dim selected = CType(ComboboxRepresentationStatus.SelectedItem, String)
            result = CType([Enum].Parse(GetType(RepresentationStatusEnum.__Enum), selected), RepresentationStatusEnum.__Enum)
            Return result
        End Get
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        LoadCombobox()
    End Sub

    Private Sub LoadCombobox()
        Dim representationStatus = [Enum].GetValues(GetType(RepresentationStatusEnum.__Enum))
        For Each status As RepresentationStatusEnum.__Enum In representationStatus
            ComboboxRepresentationStatus.Items.Add(status.ToString())
        Next
    End Sub

    ''' <summary>
    ''' Fired when files are dropped over the GridView
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub File_Drop(sender As Object, e As DragEventArgs)
        Dim files = ControlsUtils.RetrieveDropFiles(e)
        If files.Count > 0 Then
            ControlsUtils.AddGridItemFromPath(FilesGrid, files)
            LabelDropFiles.Visibility = Visibility.Hidden
        End If
        CheckIfPageIsValid()
    End Sub

    ''' <summary>
    ''' When combobox index are changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboboxRepresentationStatus_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        CheckIfPageIsValid()
    End Sub

    Public Overrides Sub CheckIfPageIsValid()

        If FilesGrid Is Nothing OrElse FilesGrid.Items.Count <= 0 OrElse ComboboxRepresentationStatus.SelectedIndex < 0 Then
            IsValidPage = False
        Else
            IsValidPage = True
        End If
    End Sub

    Protected Overrides Sub UpdateModelObject()
        Throw New NotImplementedException()
    End Sub
End Class
