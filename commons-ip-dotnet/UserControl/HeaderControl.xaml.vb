Imports System.ComponentModel

Public Class HeaderControl
    Inherits UserControl

    <BindableAttribute(True)>
    Public Property TitleHeader As String
        Get
            Return CType(GetValue(TitleHeaderProperty), String)
        End Get
        Set(value As String)
            SetValue(TitleHeaderProperty, value)
        End Set
    End Property


    <BindableAttribute(True)>
    Public Property DescriptionHeader As String
        Get
            Return CType(GetValue(DescriptionHeaderProperty), String)
        End Get
        Set(value As String)
            SetValue(DescriptionHeaderProperty, value)
        End Set
    End Property

    Public Shared ReadOnly TitleHeaderProperty As DependencyProperty = DependencyProperty.Register("TitleHeader", GetType(String), GetType(HeaderControl), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf TitleHeaderChangedHandler)))
    Public Shared ReadOnly DescriptionHeaderProperty As DependencyProperty = DependencyProperty.Register("DescriptionHeader", GetType(String), GetType(HeaderControl), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf DescriptionHeaderChangedHandler)))

    Private Shared Sub TitleHeaderChangedHandler(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        CType(d, HeaderControl).LabelTitleHeader.Content = e.NewValue.ToString
    End Sub

    Private Shared Sub DescriptionHeaderChangedHandler(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        CType(d, HeaderControl).LabelDescriptionHeader.Content = e.NewValue.ToString
    End Sub

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LabelTitleHeader.Content = TitleHeader
        LabelDescriptionHeader.Content = DescriptionHeader

    End Sub


End Class
