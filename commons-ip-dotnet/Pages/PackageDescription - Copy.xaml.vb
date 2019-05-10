Imports org.roda_project.commons_ip.utils.METSEnums

Class PackageDescription
    Inherits ExtendedPage

    Public PackageDescriptionModel As PackageDescriptionModel

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        Me.PackageDescriptionModel = New PackageDescriptionModel

        ' Add any initialization after the InitializeComponent() call.
        LoadCreatorType()
    End Sub


    ''' <summary>
    ''' Load create type options to combobox
    ''' </summary>
    Private Sub LoadCreatorType()
        Dim metadataTypes = [Enum].GetValues(GetType(CreatorType.__Enum))
        For Each type As CreatorType.__Enum In metadataTypes
            CreatorTypeCombobox.Items.Add(type.ToString())
        Next
    End Sub

    ''' <summary>
    ''' Update object model with changed information from UI
    ''' </summary>
    Protected Overrides Sub UpdateModelObject()
        If Me.PackageDescriptionModel IsNot Nothing Then
            Me.PackageDescriptionModel.SIPID = PackageIDTextBox.Text
            Me.PackageDescriptionModel.SIPDescription = PackageDescriptionTextBox.Text
            Me.PackageDescriptionModel.CreatorName = CreatorNameTextBox.Text
            Me.PackageDescriptionModel.CreatorRole = CreatorRoleTextBox.Text
            Me.PackageDescriptionModel.CreatorType = EnumsUtils.GetCreatorType(CreatorTypeCombobox.SelectedItem)
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
        If CreatorTypeCombobox IsNot Nothing AndAlso CreatorTypeCombobox.SelectedIndex >= 0 Then
            IsValidPage = True
        Else
            IsValidPage = False
        End If

    End Sub

End Class
