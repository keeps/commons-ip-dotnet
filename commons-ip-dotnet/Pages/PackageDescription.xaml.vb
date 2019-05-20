Imports org.roda_project.commons_ip.utils.METSEnums

Class PackageDescription
    Inherits ExtendedPage

    Private myPackageDescriptionModel As PackageDescriptionModel
    Public Property PackageDescriptionModel As PackageDescriptionModel
        Get
            If myPackageDescriptionModel Is Nothing Then
                myPackageDescriptionModel = New PackageDescriptionModel
            End If
            Return myPackageDescriptionModel
        End Get
        Set(value As PackageDescriptionModel)
            myPackageDescriptionModel = value
        End Set
    End Property

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        BindEvents()

        ' Add any initialization after the InitializeComponent() call.
        LoadCreatorType()
    End Sub

    Private Sub BindEvents()
        AddHandler InputTextPackageIdentification.TextBox.TextChanged, AddressOf TextBox_TextChanged
        AddHandler InputTextPackageDescription.TextBox.TextChanged, AddressOf TextBox_TextChanged
        AddHandler InputTextCreatorName.TextBox.TextChanged, AddressOf TextBox_TextChanged
        AddHandler InputTextCreatorRole.TextBox.TextChanged, AddressOf TextBox_TextChanged
        AddHandler InputComboboxCreatorType.Combobox.SelectionChanged, AddressOf CreatorTypeCombobox_SelectionChanged
    End Sub


    ''' <summary>
    ''' Load create type options to combobox
    ''' </summary>
    Private Sub LoadCreatorType()
        Dim metadataTypes = [Enum].GetValues(GetType(CreatorType.__Enum))
        For Each type As CreatorType.__Enum In metadataTypes
            InputComboboxCreatorType.Combobox.Items.Add(type.ToString())
        Next
    End Sub

    ''' <summary>
    ''' Update object model with changed information from UI
    ''' </summary>
    Protected Overrides Sub UpdateModelObject()
        If Me.PackageDescriptionModel IsNot Nothing Then
            Me.PackageDescriptionModel.SIPID = InputTextPackageIdentification.TextBox.Text
            Me.PackageDescriptionModel.SIPDescription = InputTextPackageDescription.TextBox.Text
            Me.PackageDescriptionModel.CreatorName = InputTextCreatorName.TextBox.Text
            Me.PackageDescriptionModel.CreatorRole = InputTextCreatorRole.TextBox.Text
            Me.PackageDescriptionModel.CreatorType = EnumsUtils.GetCreatorType(InputComboboxCreatorType.Combobox.SelectedItem)
            CheckIfPageIsValid()
        End If
    End Sub

#Region "Events"

    ''' <summary>
    ''' Check if combobox selection change to page validation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CreatorTypeCombobox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        UpdateModelObject()
    End Sub

    ''' <summary>
    ''' Fired when input in textbox are changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        UpdateModelObject()
    End Sub

#End Region

    Public Overrides Sub CheckIfPageIsValid()
        If InputComboboxCreatorType.Combobox IsNot Nothing AndAlso InputComboboxCreatorType.Combobox.SelectedIndex >= 0 Then
            IsValidPage = True
        Else
            IsValidPage = False
        End If
    End Sub

End Class
