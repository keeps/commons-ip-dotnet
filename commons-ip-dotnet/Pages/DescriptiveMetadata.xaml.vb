Imports log4net
Imports org.roda_project.commons_ip.model.MetadataType

Class DescriptiveMetadata
    Inherits ExtendedPage

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(DescriptiveMetadata))


    Public DescriptiveMetadataModel As DescriptiveMetadataModel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        DescriptiveMetadataModel = New DescriptiveMetadataModel

        ' Add any initialization after the InitializeComponent() call.
        LoadDescriptionMetadataType()
    End Sub

    ''' <summary>
    ''' Load from CommonsIP DescriptiveMetadataType
    ''' </summary>
    Private Sub LoadDescriptionMetadataType()
        Dim metadataTypes = [Enum].GetValues(GetType(MetadataTypeEnum.__Enum))
        For Each type As MetadataTypeEnum.__Enum In metadataTypes
            MetadataTypeCombobox.Items.Add(type.ToString())
        Next
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
            ControlsUtils.AddGridItemFromPath(DescriptionMetadataDataGrid, files)
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

    Public Overrides Sub CheckIfPageIsValid()
        If MetadataTypeCombobox IsNot Nothing AndAlso DescriptionMetadataDataGrid IsNot Nothing Then
            If MetadataTypeCombobox.SelectedIndex >= 0 AndAlso DescriptionMetadataDataGrid.Items.Count > 0 AndAlso ControlsUtils.RetrieveFiles(DescriptionMetadataDataGrid, True).Count > 0 Then
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

        If DescriptiveMetadataModel IsNot Nothing AndAlso MetadataTypeCombobox IsNot Nothing Then
            Me.DescriptiveMetadataModel.DescriptiveMetadataFile = ControlsUtils.RetrieveFiles(DescriptionMetadataDataGrid, True)
            Me.DescriptiveMetadataModel.DescriptiveMetadataType = EnumsUtils.GetMetadataType(MetadataTypeCombobox.SelectedItem)
            CheckIfPageIsValid()
        End If

    End Sub


End Class
