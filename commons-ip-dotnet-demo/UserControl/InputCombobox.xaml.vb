﻿Imports System.ComponentModel

Public Class InputCombobox

    Private myObject As ComboBox
    Public Property Combobox As ComboBox
        Get
            Return myObject
        End Get
        Set(value As ComboBox)
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

    Public Shared ReadOnly LabelProperty As DependencyProperty = DependencyProperty.Register("Label", GetType(String), GetType(InputCombobox), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf LabelHandler)))

    Private Shared Sub LabelHandler(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        CType(d, InputCombobox).InputAbstractLabel = e.NewValue.ToString
    End Sub

    Public Property InputAbstractLabel As String
        Get
            Return InputAbstract.LabelContent
        End Get
        Set(value As String)
            InputAbstract.LabelContent = value
        End Set
    End Property


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        myObject = New ComboBox
        Me.InputAbstract.AddControl(myObject)
    End Sub




End Class
