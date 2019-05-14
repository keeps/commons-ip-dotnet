Imports java.nio.file
Imports log4net
Imports org.roda_project.commons_ip.model
Imports org.roda_project.commons_ip.model.impl.eark
Imports org.roda_project.commons_ip.utils.METSEnums

Public Class SIPBuild
    Implements SIPObserver

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(SIPBuild))

    Public Event TotalItems(sender As Object, total As Integer)
    Public Event CurrentStatus(sender As Object, index As Integer)
    Public Event SIPBuildEnd(sender As Object, ByVal exitStatus As SIPBuildExitStatus)

#Region "Properties"

    Private myPackageDescriptionModel As PackageDescriptionModel
    Public ReadOnly Property PackageDescriptionModel As PackageDescriptionModel
        Get
            Return myPackageDescriptionModel
        End Get
    End Property

    Private myDescriptiveMetadataModel As DescriptiveMetadataModel
    Public ReadOnly Property DescriptiveMetadataModel As DescriptiveMetadataModel
        Get
            Return myDescriptiveMetadataModel
        End Get
    End Property

    Private myOtherMetadataModel As OtherMetadataModel
    Public ReadOnly Property OtherMetadataModel As OtherMetadataModel
        Get
            Return myOtherMetadataModel
        End Get
    End Property

    Private myPackageContentModel As PackageContentModel
    Public ReadOnly Property PackageContentModel As PackageContentModel
        Get
            Return myPackageContentModel
        End Get
    End Property

    Private mySIPModel As SIPModel
    Public ReadOnly Property SIPModel As SIPModel
        Get
            Return mySIPModel
        End Get
    End Property

#End Region

    ''' <summary>
    ''' Create a object with all page models to create a EARKSip
    ''' </summary>
    ''' <param name="packageDescriptionModel">The model contains the description package data</param>
    ''' <param name="descriptiveMetadataModel">The model contains the descriptive metadata data</param>
    ''' <param name="otherMetadataModel">The model contains the other metadata data</param>
    ''' <param name="packageContentModel">The model contains the package content data</param>
    ''' <param name="sIPModel">The model contains the path and location to save de EARKSIP</param>
    Public Sub New(packageDescriptionModel As PackageDescriptionModel, descriptiveMetadataModel As DescriptiveMetadataModel, otherMetadataModel As OtherMetadataModel, packageContentModel As PackageContentModel, sIPModel As SIPModel)
        myPackageDescriptionModel = packageDescriptionModel
        myDescriptiveMetadataModel = descriptiveMetadataModel
        myOtherMetadataModel = otherMetadataModel
        myPackageContentModel = packageContentModel
        mySIPModel = sIPModel
    End Sub

    ''' <summary>
    ''' Use all data to build the EARKSIP file
    ''' </summary>
    Public Sub Build()
        log.Debug("Buid sip start")
        ' 1) instantiate E-ARK SIP object
        Dim sip = New EARKSIP(PackageDescriptionModel.SIPID, IPContentType.getMIXED)
        sip.addObserver(Me)

        log.Debug("Set description to sip: " & PackageDescriptionModel.SIPDescription)
        ' 1.1) set optional human-readable description
        sip.setDescription(PackageDescriptionModel.SIPDescription)

        ' 1.2) add descriptive metadata (SIP level)
        For Each file In DescriptiveMetadataModel.DescriptiveMetadataFile
            log.Debug("Add descriptive metadata files: " & file.FullName & " with type: " & DescriptiveMetadataModel.DescriptiveMetadataType.ToString)
            Dim metadataDescriptiveDC = New IPDescriptiveMetadata(
            New IPFile(Paths.get(file.FullName)),
            New MetadataType(DescriptiveMetadataModel.DescriptiveMetadataType), Nothing)
            sip.addDescriptiveMetadata(metadataDescriptiveDC)
        Next

        For Each file In OtherMetadataModel.OtherMetadataFiles
            log.Debug("Add other metadata files: " & file.FullName)
            Dim metadataOtherFile = New IPFile(Paths.get(file.FullName))
            Dim metadataOther = New IPMetadata(metadataOtherFile)
            sip.addOtherMetadata(metadataOther)
        Next

        log.Debug("Add agent information")
        Dim agent = New IPAgent(PackageDescriptionModel.CreatorName, PackageDescriptionModel.CreatorType.ToString, "", CreatorType.valueOf(PackageDescriptionModel.CreatorType.ToString()), "")

        For Each keyValue In PackageContentModel.RetrieveFilesByRepresentationName
            Dim representation = New IPRepresentation(keyValue.Key)
            For Each file In keyValue.Value
                log.Debug("Add file: " & file.FullName & " to representation: " & keyValue.Key)
                representation.addFile(New IPFile(Paths.get(file.FullName)))
            Next
            sip.addRepresentation(representation)
        Next

        Try
            ' 2) build SIP, providing an output directory
            Dim zipSIP = sip.build(Paths.get(SIPModel.FullDirectoryName), SIPModel.FilenameWithoutExtension)

        Catch ex As Exception
            log.Error("Problem to create ZIP file")
            log.Error("Current error", ex)
            RaiseEvent SIPBuildEnd(Me, SIPBuildExitStatus.FAIL)
        End Try
    End Sub

    Public Sub sipBuildRepresentationsProcessingStarted(i As Integer) Implements SIPObserver.sipBuildRepresentationsProcessingStarted
        log.Debug("Build status: sipBuildRepresentationsProcessingStarted " & i)
    End Sub

    Public Sub sipBuildRepresentationProcessingStarted(i As Integer) Implements SIPObserver.sipBuildRepresentationProcessingStarted
        log.Debug("Build status: sipBuildRepresentationProcessingStarted " & i)
    End Sub

    Public Sub sipBuildRepresentationProcessingCurrentStatus(i As Integer) Implements SIPObserver.sipBuildRepresentationProcessingCurrentStatus
        log.Debug("Build status: sipBuildRepresentationProcessingCurrentStatus " & i)
    End Sub

    Public Sub sipBuildRepresentationProcessingEnded() Implements SIPObserver.sipBuildRepresentationProcessingEnded
        log.Debug("Build status: sipBuildRepresentationProcessingEnded ")
    End Sub

    Public Sub sipBuildRepresentationsProcessingEnded() Implements SIPObserver.sipBuildRepresentationsProcessingEnded
        log.Debug("Build status: sipBuildRepresentationsProcessingEnded ")
    End Sub

    ''' <summary>
    ''' Used to start the build and set the total of files to be proccess
    ''' </summary>
    ''' <param name="i"></param>
    Public Sub sipBuildPackagingStarted(i As Integer) Implements SIPObserver.sipBuildPackagingStarted
        log.Debug("Build status: sipBuildPackagingStarted " & i)
        RaiseEvent TotalItems(Me, i)
    End Sub

    ''' <summary>
    ''' Used to fired the current status
    ''' </summary>
    ''' <param name="i"></param>
    Public Sub sipBuildPackagingCurrentStatus(i As Integer) Implements SIPObserver.sipBuildPackagingCurrentStatus
        log.Debug("Build status: sipBuildPackagingCurrentStatus " & i)
        RaiseEvent CurrentStatus(Me, i)
    End Sub

    ''' <summary>
    ''' Used to fired event SIP build end
    ''' </summary>
    Public Sub sipBuildPackagingEnded() Implements SIPObserver.sipBuildPackagingEnded
        log.Debug("Build status: sipBuildPackagingEnded")
        RaiseEvent SIPBuildEnd(Me, SIPBuildExitStatus.SUCCESS)
    End Sub
End Class

Public Enum SIPBuildExitStatus
    SUCCESS
    FAIL
End Enum
