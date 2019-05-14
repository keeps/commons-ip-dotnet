Imports java.nio.file
Imports java.util
Imports org.roda_project.commons_ip.model
Imports org.roda_project.commons_ip.model.impl.eark
Imports org.roda_project.commons_ip.model.MetadataType
Imports org.roda_project.commons_ip.utils.METSEnums

Module EAKSIP

    Sub Main()
        Console.WriteLine("Create EARKSIP file")
        Dim startTime = DateTime.Now

        createEARKSIP()

        Console.WriteLine("Finish EARKSIP file: " & DateTime.Now.Subtract(startTime).TotalMilliseconds)
        Console.WriteLine("Press any key to continue...")

        Console.WriteLine("Create EARKSIP file2")
        Dim startTime2 = DateTime.Now

        createEARKSIP()

        Console.WriteLine("Finish EARKSIP file2: " & DateTime.Now.Subtract(startTime2).TotalMilliseconds)

        Console.WriteLine("Create EARKSIP file3")
        Dim startTime3 = DateTime.Now

        createEARKSIP()

        Console.WriteLine("Finish EARKSIP file3: " & DateTime.Now.Subtract(startTime3).TotalMilliseconds)
        Console.WriteLine("Press any key to continue...")

        Console.ReadLine()
    End Sub

    Sub createEARKSIP()
        ' 1) instantiate E-ARK SIP object
        Dim sip = New EARKSIP("SIP_1", IPContentType.getMIXED())
        sip.addCreatorSoftwareAgent("RODA Commons IP")

        ' 1.1) set optional human-readable description
        sip.setDescription("A full E-ARK SIP")

        ' 1.2) add descriptive metadata (SIP level)
        Dim metadataDescriptiveDC = New IPDescriptiveMetadata(
          New IPFile(Paths.get("test\resources\eark\metadata_descriptive_dc.xml")),
          New MetadataType(MetadataTypeEnum.DC), Nothing)
        sip.addDescriptiveMetadata(metadataDescriptiveDC)

        ' 1.3) add preservation metadata (SIP level)
        Dim metadataPreservation = New IPMetadata(
          New IPFile(Paths.get("test\resources\eark\metadata_preservation_premis.xml")))
        sip.addPreservationMetadata(metadataPreservation)

        ' 1.4) add other metadata (SIP level)
        Dim metadataOtherFile = New IPFile(Paths.get("test\resources\eark\metadata_other.txt"))
        ' 1.4.1) optionally one may rename file final name
        metadataOtherFile.setRenameTo("metadata_other_renamed.txt")
        Dim metadataOther = New IPMetadata(metadataOtherFile)
        sip.addOtherMetadata(metadataOther)

        ' 1.5) add xml schema (SIP level)
        sip.addSchema(New IPFile(Paths.get("test\resources\eark\schema.xsd")))

        ' 1.6) add documentation (SIP level)
        sip.addDocumentation(New IPFile(Paths.get("test\resources\eark\documentation.pdf")))

        ' 1.7) set optional RODA related information about ancestors
        sip.setAncestors(Arrays.asList("b6f24059-8973-4582-932d-eb0b2cb48f28"))

        ' 1.8) add an agent (SIP level)
        Dim agent = New IPAgent("Agent Name", "OTHER", "OTHER ROLE", CreatorType.INDIVIDUAL, "OTHER TYPE")
        sip.addAgent(agent)

        ' 1.9) add a representation (status will be set to the default value, i.e.,
        ' ORIGINAL)
        Dim representation1 = New IPRepresentation("representation 1")
        sip.addRepresentation(representation1)

        ' 1.9.1) add a file to the representation
        Dim representationFile = New IPFile(Paths.get("test\resources\eark\documentation.pdf"))
        representationFile.setRenameTo("data_.pdf")
        representation1.addFile(representationFile)

        Dim representationFileEnc2 = New IPFile(Paths.get("test\resources\eark\documentation.pdf"))
        representationFileEnc2.setRenameTo("enc2_\u0080\u0081\u0090\u00FF.pdf")
        representation1.addFile(representationFileEnc2)

        Dim representationFileEnc3 = New IPFile(Paths.get("test\resources\eark\documentation.pdf"))
        representation1.addFile(representationFileEnc3)

        Dim representationFileEnc4 = New IPFile(Paths.get("test\resources\eark\documentation.pdf"))
        representation1.addFile(representationFileEnc4)

        ' 1.9.2) add a file to the representation and put it inside a folder
        ' called 'abc' which has a folder inside called 'def'
        Dim representationFile2 = New IPFile(Paths.get("test\resources\eark\documentation.pdf"))
        representationFile2.setRelativeFolders(Arrays.asList("abc", "def"))
        representation1.addFile(representationFile2)

        ' 1.10) add a representation & define its status
        Dim representation2 = New IPRepresentation("representation 2")
        sip.addRepresentation(representation2)

        ' 1.10.1) add a file to the representation
        Dim representationFile3 = New IPFile(Paths.get("test\resources\eark\documentation.pdf"))
        representationFile3.setRenameTo("data3.pdf")
        representation2.addFile(representationFile3)

        ' 2) build SIP, providing an output directory
        Dim zipSIP = sip.build(Paths.get(""))

    End Sub

End Module
