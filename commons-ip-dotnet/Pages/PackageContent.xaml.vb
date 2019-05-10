Imports log4net
Imports org.roda_project.commons_ip.model
Imports org.roda_project.commons_ip.model.MetadataType
Imports org.roda_project.commons_ip.model.RepresentationContentType

Class PackageContent
    Inherits ExtendedPage

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(PackageContent))


    Public PackageContentModel As PackageContentModel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        PackageContentModel = New PackageContentModel

        ' Add any initialization after the InitializeComponent() call.
        LoadRepresentationContentType()
    End Sub

    ''' <summary>
    ''' Load from CommonsIP DescriptiveMetadataType
    ''' </summary>
    Private Sub LoadRepresentationContentType()
        Dim representationContentType = [Enum].GetValues(GetType(RepresentationContentTypeEnum.__Enum))
        For Each type As RepresentationContentTypeEnum.__Enum In representationContentType
            RepresentationContentTypeCombobox.Items.Add(type.ToString())
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
            ControlsUtils.AddGridItemFromPath(PackageContentDataGrid, files)
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


    Private Sub MenuItemRemove_Click(sender As Object, e As RoutedEventArgs)
        Dim representation = New IPRepresentation("asd")
    End Sub

    Private Sub MenuItemChangeRepresentationName_Click(sender As Object, e As RoutedEventArgs)
        Dim popupWindow As New PopupTextBox
        popupWindow.ShowDialog()

        If popupWindow IsNot Nothing AndAlso Not String.IsNullOrEmpty(popupWindow.RepresentationName) Then
            For Each item In PackageContentDataGrid.SelectedItems
                CType(item, DataRowFile).RepresentationName = popupWindow.RepresentationName
            Next
        End If

    End Sub

#End Region

    Public Overrides Sub CheckIfPageIsValid()
        If RepresentationContentTypeCombobox IsNot Nothing AndAlso PackageContentDataGrid IsNot Nothing Then
            If RepresentationContentTypeCombobox.SelectedIndex >= 0 AndAlso PackageContentDataGrid.Items.Count > 0 Then
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

        If PackageContentModel IsNot Nothing AndAlso RepresentationContentTypeCombobox IsNot Nothing Then
            Me.PackageContentModel.PackageContent = ControlsUtils.RetrieveFiles(Me.PackageContentDataGrid, True)
            Me.PackageContentModel.RepresentationContentType = EnumsUtils.GetRepresentationContentType(RepresentationContentTypeCombobox.SelectedItem)
            CheckIfPageIsValid()
        End If

    End Sub


End Class
