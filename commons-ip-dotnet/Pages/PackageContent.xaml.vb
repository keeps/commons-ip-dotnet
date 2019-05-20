Imports log4net
Imports org.roda_project.commons_ip.model
Imports org.roda_project.commons_ip.model.MetadataType
Imports org.roda_project.commons_ip.model.RepresentationContentType

Class PackageContent
    Inherits ExtendedPage

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(PackageContent))

    Private myPackageContentModel As PackageContentModel
    Public Property PackageContentModel As PackageContentModel
        Get
            If myPackageContentModel Is Nothing Then
                myPackageContentModel = New PackageContentModel
            End If
            Return myPackageContentModel
        End Get
        Set(value As PackageContentModel)
            myPackageContentModel = value
        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        BindEvents()

        ' Add any initialization after the InitializeComponent() call.
        LoadRepresentationContentType()

    End Sub

    Private Sub BindEvents()
        InputTreeViewPaths.TreeView.AllowDrop = True

        AddHandler InputComboboxType.Combobox.SelectionChanged, AddressOf MetadataTypeCombobox_SelectionChanged
        AddHandler InputTextRepresentationName.TextBox.TextChanged, AddressOf InputTextRepresentationName_TextChanged
        AddHandler InputTreeViewPaths.TreeView.DragOver, AddressOf InputTreeViewPaths_DragOver
        AddHandler InputTreeViewPaths.TreeView.Drop, AddressOf InputTreeViewPaths_Drop
    End Sub


    Private Sub InputTreeViewPaths_Drop(sender As Object, e As DragEventArgs)
        Dim filesPaths() As String = e.Data.GetData("FileDrop", False)
        If filesPaths IsNot Nothing Then
            Dim list As New List(Of TreeViewPath)
            For Each filePath As String In filesPaths
                list.AddRange(ControlsUtils.RetrieveTreeViewPath(filePath, filePath, Nothing))
            Next
            InputTreeViewPaths.TreeView.ItemsSource = list

            UpdateModelObject()
        End If
    End Sub

    Private Sub InputTreeViewPaths_DragOver(sender As Object, e As DragEventArgs)
        If (e.Data.GetDataPresent(DataFormats.FileDrop)) Then
            Dim filesPaths() As String = e.Data.GetData("FileDrop", False)
            If filesPaths.Count > 1 Then
                e.Effects = DragDropEffects.None
            Else
                e.Effects = DragDropEffects.Move
            End If
        End If
    End Sub

    ''' <summary>
    ''' Load from CommonsIP DescriptiveMetadataType
    ''' </summary>
    Private Sub LoadRepresentationContentType()
        Dim representationContentType = [Enum].GetValues(GetType(RepresentationContentTypeEnum.__Enum))
        For Each type As RepresentationContentTypeEnum.__Enum In representationContentType
            InputComboboxType.Combobox.Items.Add(type.ToString())
        Next
    End Sub

#Region "Events"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub InputTextRepresentationName_TextChanged(sender As Object, e As TextChangedEventArgs)
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
        If InputComboboxType.Combobox IsNot Nothing AndAlso InputTreeViewPaths.TreeView IsNot Nothing Then
            Dim items = CType(InputTreeViewPaths.TreeView.ItemsSource, List(Of TreeViewPath))
            If InputComboboxType.Combobox.SelectedIndex >= 0 AndAlso items.Count > 0 Then
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

        If PackageContentModel IsNot Nothing AndAlso InputComboboxType.Combobox IsNot Nothing AndAlso InputTreeViewPaths IsNot Nothing AndAlso InputTreeViewPaths.TreeView.ItemsSource IsNot Nothing Then
            Dim paths = CType(InputTreeViewPaths.TreeView.ItemsSource, List(Of TreeViewPath))
            Me.PackageContentModel.PackageContent = paths
            Me.PackageContentModel.RepresentationContentType = EnumsUtils.GetRepresentationContentType(InputComboboxType.Combobox.SelectedItem)
            Me.PackageContentModel.RepresentationName = InputTextRepresentationName.TextBox.Text
            CheckIfPageIsValid()
        End If

    End Sub


End Class
