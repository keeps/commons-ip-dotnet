Imports System.ComponentModel

Public Class InputTreeView

    Private myObject As TreeView
    Public Property TreeView As TreeView
        Get
            Return myObject
        End Get
        Set(value As TreeView)
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

    Public Shared ReadOnly LabelProperty As DependencyProperty = DependencyProperty.Register("Label", GetType(String), GetType(InputTreeView), New FrameworkPropertyMetadata(Nothing, FrameworkPropertyMetadataOptions.AffectsRender, New PropertyChangedCallback(AddressOf LabelHandler)))

    Private Shared Sub LabelHandler(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        CType(d, InputTreeView).InputAbstractLabel = e.NewValue.ToString
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
        myObject = New TreeView
        Me.InputAbstract.AddControl(myObject)
        CreateTreeViewTemplate()
    End Sub

    Private Sub CreateTreeViewTemplate()

        Dim labelFactory = New FrameworkElementFactory(GetType(TextBlock))
        Dim imageFactory = New FrameworkElementFactory(GetType(Image))
        Dim stackPanelFactory = New FrameworkElementFactory(GetType(StackPanel))

        Dim Template = New HierarchicalDataTemplate(GetType(TreeViewPath))
        Template.ItemsSource = New Binding("Children")

        stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal)
        imageFactory.SetValue(Image.WidthProperty, Double.Parse("16"))
        imageFactory.SetValue(Image.HeightProperty, Double.Parse("16"))

        imageFactory.SetBinding(Image.SourceProperty, New Binding("ImageFilename"))
        labelFactory.SetBinding(TextBlock.TextProperty, New Binding("Filename"))

        stackPanelFactory.AppendChild(imageFactory)
        stackPanelFactory.AppendChild(labelFactory)

        Template.VisualTree = stackPanelFactory

        myObject.ItemTemplate = Template
    End Sub






End Class
