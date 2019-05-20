Imports System.IO
Imports java.nio.file
Imports log4net
Imports org.roda_project.commons_ip.model
Imports org.roda_project.commons_ip.model.impl.eark
Imports org.roda_project.commons_ip.utils.METSEnums

Class MainWindow
    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(MainWindow))

    'Used to know if the next button restart the aplicacion or continue to the next page
    Private Property RestartWorkflow As Boolean

    Private myPages As List(Of ExtendedPage) = Nothing
    ''' <summary>
    ''' All the pages present in the workflow
    ''' </summary>
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
            log.Debug("Current page index change, value: " & value)
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

        'Navigate to the first page, when aplication start
        Me.MainFrame.NavigationService.Navigate(Pages(CurrentPageIntex))

    End Sub

    ''' <summary>
    ''' Initialize pages
    ''' </summary>
    Private Sub InitializePagesAndCreateCurrentOrder()
        'Reset all the variables
        myPages = Nothing
        RestartWorkflow = False
        'HelpLabel.Content = String.Empty
        'ProgressBarStatus.Value = 0
        'ProgressBarStatus.Visibility = Visibility.Hidden
        ButtonNext.Content = "Next"

        'Create all pages
        Me.presentationPage = New Presentation()
        Me.packageDescriptionPage = New PackageDescription()
        Me.descriptiveMetadataPage = New DescriptiveMetadata()
        Me.otherMetadataPage = New OtherMetadata
        Me.packageContentPage = New PackageContent
        Me.sipPage = New SIP()

        'Add the validPage event to enable/disabel next button
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

    ''' <summary>
    ''' Change IsEnable propertie of next button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="isValid"></param>
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

    ''' <summary>
    ''' Check if page is valid after page index change
    ''' </summary>
    Private Sub UpdateInfoFromSelectedPage()
        Dim page As ExtendedPage = Pages(CurrentPageIntex)
        page.CheckIfPageIsValid()
    End Sub

    ''' <summary>
    ''' Create a SIP file based on CommonsIP
    ''' https://github.com/keeps/commons-ip
    ''' </summary>
    Private Sub CrateSIP()
        ButtonPrevious.IsEnabled = False
        ButtonNext.IsEnabled = False
        'HelpLabel.Content = "Wait!! Creating E-ARK SIP file..."

        ControlsUtils.UpdateUI()
        Dim sipBuild As New SIPBuild(Me.PackageDescriptionModel, Me.DescriptiveMetadataModel, Me.OtherMetadataModel, Me.PackageContentModel, Me.SIPModel)

        AddHandler sipBuild.TotalItems, AddressOf SIPBuild_totalitems
        AddHandler sipBuild.CurrentStatus, AddressOf SIPBuild_CurrentStatus
        AddHandler sipBuild.SIPBuildEnd, AddressOf SIPBuild_Ended

        sipBuild.Build()
    End Sub

    ''' <summary>
    ''' Event fired when SIP build end, receive exit status (SUCCESS OR FAIL)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="sip"></param>
    Private Sub SIPBuild_Ended(sender As Object, sip As SIPBuildExitStatus)
        If sip = SIPBuildExitStatus.SUCCESS Then
            ' HelpLabel.Content = "E-ARK SIP created with success :)"
        Else
            'HelpLabel.Content = "Problem to create E-ARK SIP file, check if all files has correct permissions :("
        End If
        RestartWorkflow = True
        ButtonNext.Content = "Create new SIP"
        ButtonNext.IsEnabled = True
    End Sub

    ''' <summary>
    ''' Receive the current process status
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="index"></param>
    Private Sub SIPBuild_CurrentStatus(sender As Object, index As Integer)
        ControlsUtils.UpdateUI()
        '  ProgressBarStatus.Value = index
    End Sub

    ''' <summary>
    ''' Event fired when process files start, receive the total of files to process
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="total"></param>
    Private Sub SIPBuild_totalitems(sender As Object, total As Integer)
        ' ProgressBarStatus.Visibility = Visibility.Visible
        'ProgressBarStatus.Maximum = total
        ControlsUtils.UpdateUI()
    End Sub

End Class
