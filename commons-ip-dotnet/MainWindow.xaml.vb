Imports System.IO
Imports java.nio.file
Imports log4net
Imports org.roda_project.commons_ip.model
Imports org.roda_project.commons_ip.model.impl.eark
Imports org.roda_project.commons_ip.utils.METSEnums

Class MainWindow
    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(MainWindow))

    Private Property RestartWorkflow As Boolean

    Private myPages As List(Of ExtendedPage) = Nothing
    Private ReadOnly Property Pages As List(Of ExtendedPage)
        Get
            If myPages Is Nothing Then
                myPages = New List(Of ExtendedPage)
            End If
            Return myPages
        End Get
    End Property

    Private myCurrentPageIndex As Integer = 0
    ''' <summary>
    ''' Get current page index to check next and previous
    ''' </summary>
    ''' <returns></returns>
    Private Property CurrentPageIntex As Integer
        Get
            Return myCurrentPageIndex
        End Get
        Set(value As Integer)
            myCurrentPageIndex = value
        End Set
    End Property

    ''' <summary>
    ''' Return total of pages, to prevent out of index
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property TotalPages As Integer
        Get
            Return Pages.Count
        End Get
    End Property

    Private ReadOnly Property PackageDescriptionModel As PackageDescriptionModel
        Get
            Return packageDescriptionPage.PackageDescriptionModel
        End Get
    End Property

    Private ReadOnly Property DescriptiveMetadataModel As DescriptiveMetadataModel
        Get
            Return descriptiveMetadataPage.DescriptiveMetadataModel
        End Get
    End Property

    Private ReadOnly Property OtherMetadataModel As OtherMetadataModel
        Get
            Return otherMetadataPage.OtherMetadataModel
        End Get
    End Property

    Private ReadOnly Property PackageContentModel As PackageContentModel
        Get
            Return packageContentPage.PackageContentModel
        End Get
    End Property

    Private ReadOnly Property SIPModel As SIPModel
        Get
            Return sipPage.SipModel
        End Get
    End Property

    'All the pages are ExtendedPage
    Private presentationPage As Presentation
    Private packageDescriptionPage As PackageDescription
    Private descriptiveMetadataPage As DescriptiveMetadata
    Private otherMetadataPage As OtherMetadata
    Private packageContentPage As PackageContent
    Private sipPage As SIP

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        InitializePagesAndCreateCurrentOrder()

        Me.MainFrame.NavigationService.Navigate(Pages(CurrentPageIntex))

    End Sub

    ''' <summary>
    ''' Initialize pages
    ''' </summary>
    Private Sub InitializePagesAndCreateCurrentOrder()
        myPages = Nothing
        RestartWorkflow = False
        HelpLabel.Content = String.Empty
        ProgressBarStatus.Value = 0
        ProgressBarStatus.Visibility = Visibility.Hidden
        ButtonNext.Content = "Next"

        'Create all pages
        Me.presentationPage = New Presentation()
        Me.packageDescriptionPage = New PackageDescription()
        Me.descriptiveMetadataPage = New DescriptiveMetadata()
        Me.otherMetadataPage = New OtherMetadata
        Me.packageContentPage = New PackageContent
        Me.sipPage = New SIP()

        AddHandler presentationPage.ValidPageChanged, AddressOf ExtendedPage_ValidPageChanged
        AddHandler packageDescriptionPage.ValidPageChanged, AddressOf ExtendedPage_ValidPageChanged
        AddHandler descriptiveMetadataPage.ValidPageChanged, AddressOf ExtendedPage_ValidPageChanged
        AddHandler otherMetadataPage.ValidPageChanged, AddressOf ExtendedPage_ValidPageChanged
        AddHandler packageContentPage.ValidPageChanged, AddressOf ExtendedPage_ValidPageChanged
        AddHandler sipPage.ValidPageChanged, AddressOf ExtendedPage_ValidPageChanged

        'Add pages to list with correct order
        Pages.Add(presentationPage)
        Pages.Add(packageDescriptionPage)
        Pages.Add(descriptiveMetadataPage)
        Pages.Add(otherMetadataPage)
        Pages.Add(packageContentPage)
        Pages.Add(sipPage)
    End Sub

    Private Sub ExtendedPage_ValidPageChanged(sender As Object, isValid As Boolean)
        If isValid Then
            ButtonNext.IsEnabled = True
        Else
            ButtonNext.IsEnabled = False
        End If
    End Sub

    ''' <summary>
    ''' Change to the next page on the MainFrame
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Next_Click(sender As Object, e As RoutedEventArgs)
        If RestartWorkflow Then
            CurrentPageIntex = 0
            InitializePagesAndCreateCurrentOrder()
        End If

        If CurrentPageIntex = TotalPages - 1 Then
                log.Debug("Last PAGE!!!! Create a SIP file")
                CrateSIP()
            Else
                If CurrentPageIntex < TotalPages - 1 Then
                    log.Debug("Change page to the next one")
                    CurrentPageIntex += 1
                    Me.MainFrame.NavigationService.Navigate(Pages(CurrentPageIntex))
                    Me.ButtonPrevious.IsEnabled = True
                Else
                    log.Debug("No more pages available")
                End If
            End If

            UpdateInfoFromSelectedPage()
    End Sub

    ''' <summary>
    ''' Change to the previous page on the MainFrame
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Previous_Click(sender As Object, e As RoutedEventArgs)
        If CurrentPageIntex > 0 Then
            log.Debug("Change page to the previous one")
            CurrentPageIntex -= 1
            Me.MainFrame.NavigationService.Navigate(Pages(CurrentPageIntex))
        Else
            Me.ButtonPrevious.IsEnabled = False
            log.Debug("No more pages available")
        End If
        UpdateInfoFromSelectedPage()
    End Sub


    Private Sub UpdateInfoFromSelectedPage()
        Dim page As ExtendedPage = Pages(CurrentPageIntex)
        page.CheckIfPageIsValid()
        'Me.HelpLabel.Content = page.PageDescription
    End Sub

    ''' <summary>
    ''' Create a SIP file based on CommonsIP
    ''' https://github.com/keeps/commons-ip
    ''' </summary>
    Private Sub CrateSIP()
        ButtonPrevious.IsEnabled = False
        ButtonNext.IsEnabled = False
        HelpLabel.Content = "Wainting, create SIP file..."

        ControlsUtils.UpdateUI()
        Dim sipBuild As New SIPBuild(Me.PackageDescriptionModel, Me.DescriptiveMetadataModel, Me.OtherMetadataModel, Me.PackageContentModel, Me.SIPModel)

        AddHandler sipBuild.TotalItems, AddressOf SIPBuild_totalitems
        AddHandler sipBuild.CurrentStatus, AddressOf SIPBuild_CurrentStatus
        AddHandler sipBuild.SIPBuildEnd, AddressOf SIPBuild_Ended

        sipBuild.Build()
    End Sub

    Private Sub SIPBuild_Ended(sender As Object, sip As SIPBuildExitStatus)
        If sip = SIPBuildExitStatus.SUCCESS Then
            HelpLabel.Content = "SIP created with success :)"
        Else
            HelpLabel.Content = "Problem to create SIP file, check if all files have correct permissions :("
        End If
        RestartWorkflow = True
        ButtonNext.Content = "Restart"
        ButtonNext.IsEnabled = True
    End Sub

    Private Sub SIPBuild_CurrentStatus(sender As Object, index As Integer)
        ControlsUtils.UpdateUI()
        ProgressBarStatus.Value = index
    End Sub


    Private Sub SIPBuild_totalitems(sender As Object, total As Integer)
        ProgressBarStatus.Visibility = Visibility.Visible
        ProgressBarStatus.Maximum = total
        ControlsUtils.UpdateUI()
    End Sub

End Class
