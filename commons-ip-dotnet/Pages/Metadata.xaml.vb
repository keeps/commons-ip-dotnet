Imports System.IO
Imports log4net
Imports org.roda_project.commons_ip.model.MetadataType


Class Metadata
    Inherits ExtendedPage

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(Metadata))

    Public ReadOnly Property SelectedFiles As List(Of FileInfo)
        Get
            Dim result As New List(Of FileInfo)
            For Each item In MetadataFilesGrid.Items
                log.Debug("Selected file in metadata " & CType(item, FileInfo).FullName)
                result.Add(item)
            Next
            Return result
        End Get
    End Property

    Public ReadOnly Property SelectedMetadataType As MetadataTypeEnum.__Enum
        Get
            Dim selected = CType(MetadataTypeCombobox.SelectedItem, String)
            Return EnumsUtils.GetMetadataType(selected)
        End Get
    End Property

    ''' <summary>
    ''' Page load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        LoadCombobox()
    End Sub

    ''' <summary>
    ''' Load all metadata types from CommonsIP to combobox
    ''' </summary>
    Private Sub LoadCombobox()
        Dim metadataTypes = [Enum].GetValues(GetType(MetadataTypeEnum.__Enum))
        For Each type As MetadataTypeEnum.__Enum In metadataTypes
            MetadataTypeCombobox.Items.Add(type.ToString())
        Next
    End Sub

    ''' <summary>
    ''' Fired when files are dropped over the Grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Metadata_Drop(sender As Object, e As DragEventArgs)
        Dim files = ControlsUtils.RetrieveDropFiles(e)
        If files.Count > 0 Then
            ControlsUtils.AddGridItemFromPath(MetadataFilesGrid, files)
            LabelDropFiles.Visibility = Visibility.Hidden
        End If
        CheckIfPageIsValid()
    End Sub

    ''' <summary>
    ''' Fired when metadatatype combobox 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MetadataTypeCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        CheckIfPageIsValid()
    End Sub

    Public Overrides Sub CheckIfPageIsValid()

        If MetadataFilesGrid Is Nothing OrElse MetadataFilesGrid.Items.Count <= 0 OrElse MetadataTypeCombobox.SelectedIndex < 0 Then
            IsValidPage = False
        Else
            IsValidPage = True
        End If
    End Sub

    Protected Overrides Sub UpdateModelObject()
        Throw New NotImplementedException()
    End Sub
End Class

