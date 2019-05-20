Imports log4net
Imports org.roda_project.commons_ip.model.MetadataType

Class OtherMetadata
    Inherits ExtendedPage

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(OtherMetadata))


    Private myOtherMetadataModel As OtherMetadataModel
    Public Property OtherMetadataModel As OtherMetadataModel
        Get
            If myOtherMetadataModel Is Nothing Then
                myOtherMetadataModel = New OtherMetadataModel
            End If
            Return myOtherMetadataModel
        End Get
        Set(value As OtherMetadataModel)
            myOtherMetadataModel = value
        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        BindEvents
        AddColumnsDataGrid()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub BindEvents()
        AddHandler InputDataGridSelectedFiles.Drop, AddressOf DescriptiveMetadataFiles_Drop
    End Sub

    Private Sub AddColumnsDataGrid()
        'Allow drop
        InputDataGridSelectedFiles.DataGrid.AllowDrop = True
        'Add custom columns
        Dim selectedColumn As FrameworkElementFactory = ControlsUtils.AddTemplateCheckbox(InputDataGridSelectedFiles.DataGrid, "Selected", "IsSelected")
        selectedColumn.AddHandler(CheckBox.CheckedEvent, New RoutedEventHandler(AddressOf CheckBox_CheckChanged))
        selectedColumn.AddHandler(CheckBox.UncheckedEvent, New RoutedEventHandler(AddressOf CheckBox_CheckChanged))
        Dim fileName As New DataGridTextColumn()
        fileName.Header = "Filename"
        fileName.Binding = New Binding("FullName")
        fileName.IsReadOnly = True
        InputDataGridSelectedFiles.DataGrid.Columns.Add(fileName)
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
            ControlsUtils.AddGridItemFromPath(InputDataGridSelectedFiles.DataGrid, files)
            'DescriptiveMetadataLabel.Visibility = Visibility.Hidden
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

        If OtherMetadataModel IsNot Nothing AndAlso InputDataGridSelectedFiles.DataGrid IsNot Nothing Then
            Me.OtherMetadataModel.OtherMetadataFiles = ControlsUtils.RetrieveFiles(InputDataGridSelectedFiles.DataGrid)
            CheckIfPageIsValid()
        End If

    End Sub


End Class
