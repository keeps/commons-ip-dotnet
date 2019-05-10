Imports log4net
Imports org.roda_project.commons_ip.model.MetadataType

Class OtherMetadata
    Inherits ExtendedPage

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(OtherMetadata))


    Public OtherMetadataModel As OtherMetadataModel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        OtherMetadataModel = New OtherMetadataModel

        ' Add any initialization after the InitializeComponent() call.
    End Sub


#Region "Events"

    ''' <summary>
    ''' Event fired when files dropped over Label and datagrid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DescriptiveMetadataFiles_Drop(sender As Object, e As DragEventArgs)
        Dim files = ControlsUtils.RetrieveDropFiles(e)
        If files.Count > 0 Then
            ControlsUtils.AddGridItemFromPath(OtherMetadataDataGrid, files)
            DescriptiveMetadataLabel.Visibility = Visibility.Hidden
        End If
        UpdateModelObject()
    End Sub

    ''' <summary>
    ''' When selection item change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MetadataTypeCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        UpdateModelObject()
    End Sub

    ''' <summary>
    ''' If checkbox status change
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBox_CheckChanged(sender As Object, e As RoutedEventArgs)
        If sender IsNot Nothing Then
            CType(CType(sender, CheckBox).DataContext, DataRowFile).IsSelected = DirectCast(e.Source, System.Windows.Controls.Primitives.ToggleButton).IsChecked
            UpdateModelObject()
        End If
    End Sub

#End Region

    ''' <summary>
    ''' Not necessary page
    ''' </summary>
    Public Overrides Sub CheckIfPageIsValid()
        IsValidPage = True
    End Sub

    ''' <summary>
    ''' Update current object model
    ''' </summary>
    Protected Overrides Sub UpdateModelObject()

        If OtherMetadataModel IsNot Nothing AndAlso OtherMetadataDataGrid IsNot Nothing Then
            Me.OtherMetadataModel.OtherMetadataFiles = ControlsUtils.RetrieveFiles(OtherMetadataDataGrid)
            CheckIfPageIsValid()
        End If

    End Sub


End Class
