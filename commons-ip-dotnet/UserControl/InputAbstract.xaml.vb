Public Class InputAbstract

    Public Const ELEMENT_NAME As String = "element1"

    Public Property LabelContent As String
        Get
            Return Label.Content
        End Get
        Set(value As String)
            Label.Content = value
        End Set
    End Property


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    ''' <summary>
    ''' Add control to elemnt
    ''' </summary>
    ''' <param name="control"></param>
    ''' <returns></returns>
    Public Function AddControl(ByVal control As UIElement) As UIElement

        Me.DockPanel.Children.Add(control)
        Me.DockPanel.RegisterName(ELEMENT_NAME, control)

        Return control

    End Function

End Class
