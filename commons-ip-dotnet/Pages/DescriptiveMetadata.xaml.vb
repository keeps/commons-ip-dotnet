Imports log4net
Imports org.roda_project.commons_ip.model.MetadataType

Class DescriptiveMetadata
    Inherits ExtendedPage

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(DescriptiveMetadata))

    Private myDescriptiveMetadataModel As DescriptiveMetadataModel
    Public Property DescriptiveMetadataModel As DescriptiveMetadataModel
        Get
            If myDescriptiveMetadataModel Is Nothing Then
                myDescriptiveMetadataModel = New DescriptiveMetadataModel
            End If
            Return myDescriptiveMetadataModel
        End Get
        Set(value As DescriptiveMetadataModel)
            myDescriptiveMetadataModel = value
        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        BindEvents()
        AddColumnsDataGrid()

        ' Add any initialization after the InitializeComponent() call.
        LoadDescriptionMetadataType()
    End Sub

    ''' <summary>
    ''' Load from CommonsIP DescriptiveMetadataType
    ''' </summary>
    Private Sub LoadDescriptionMetadataType()
        Dim metadataTypes = [Enum].GetValues(GetType(MetadataTypeEnum.__Enum))
        For Each type As MetadataTypeEnum.__Enum In metadataTypes
            InputComboboxMetadatType.Combobox.Items.Add(type.ToString())
        Next
    End Sub

    ''' <summary>
    ''' This method add custom columns to current grid
    ''' </summary>
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

    Private Sub BindEvents()
        AddHandler InputComboboxMetadatType.Combobox.SelectionChanged, AddressOf MetadataTypeCombobox_SelectionChanged
        AddHandler InputDataGridSelectedFiles.DataGrid.Drop, AddressOf DescriptiveMetadataFiles_Drop
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

            ControlsUtils.AddGridItemFromPath(InputDataGridSelectedFiles.DataGrid, files, True, True)
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

    Public Overrides Sub CheckIfPageIsValid()
        If InputComboboxMetadatType.Combobox IsNot Nothing AndAlso InputDataGridSelectedFiles.DataGrid IsNot Nothing Then
            If InputComboboxMetadatType.Combobox.SelectedIndex >= 0 AndAlso InputDataGridSelectedFiles.DataGrid.Items.Count > 0 AndAlso ControlsUtils.RetrieveFiles(InputDataGridSelectedFiles.DataGrid, True).Count = 1 Then
                IsValidPage = True
            Else
                IsValidPage = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' Update current object model
    ''' </summary>
    Protected Overrides Sub UpdateModelObject()

        If DescriptiveMetadataModel IsNot Nothing AndAlso InputComboboxMetadatType.Combobox IsNot Nothing Then
            Me.DescriptiveMetadataModel.DescriptiveMetadataFile = ControlsUtils.RetrieveFiles(InputDataGridSelectedFiles.DataGrid, True)
            Me.DescriptiveMetadataModel.DescriptiveMetadataType = EnumsUtils.GetMetadataType(InputComboboxMetadatType.Combobox.SelectedItem)
            CheckIfPageIsValid()
        End If

    End Sub


End Class
