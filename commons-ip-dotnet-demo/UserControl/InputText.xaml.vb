Imports System.ComponentModel

Public Class InputText

    Private myObject As TextBox
    Public Property TextBox As TextBox
        Get
            Return myObject
        End Get
        Set(value As TextBox)
            myObject = value
        End Set
    End Property

    <BindableAttribute(True)>
    Public Property Label As String
        Get
            Return CType(GetValue(LabelProperty), String)
        End Get
        Set(value As String)
            SetValue(LabelProperty, value)
        End Set
    End Property

    Public Shared ReadOnly LabelProperty As DependencyProperty = DependencyProperty.Register("Label", GetType(String), GetType(InputText), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf LabelHandler)))

    Private Shared Sub LabelHandler(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        CType(d, InputText).InputAbstractLabel = e.NewValue.ToString
    End Sub

    Public Property InputAbstractLabel As String
        Get
            Return InputAbstract.LabelContent
        End Get
        Set(value As String)
            InputAbstract.LabelContent = value
        End Set
    End Property

    <BindableAttribute(True)>
    Public Property TextBoxText As String
        Get
            Return CType(GetValue(TextBoxProperty), String)
        End Get
        Set(value As String)
            SetValue(TextBoxProperty, value)
        End Set
    End Property

    Public Shared ReadOnly TextBoxProperty As DependencyProperty = DependencyProperty.Register("TextBoxText", GetType(String), GetType(InputText), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf TextBoxHandler)))

    Private Shared Sub TextBoxHandler(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        CType(d, InputText).InputTextContent = e.NewValue.ToString
    End Sub

    Public Property InputTextContent As String
        Get
            Return TextBox.Text
        End Get
        Set(value As String)
            TextBox.Text = value
        End Set
    End Property


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        myObject = New TextBox
        Me.InputAbstract.AddControl(myObject)
    End Sub


End Class
