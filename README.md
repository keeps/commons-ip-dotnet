# commons-ip-dotnet
Provides an API to manipulate Information Packages in .net framework project.

### Requirements

* Visual Studio (>= 2015)
* .net Framework (>= 4.7.2)
* NuGet manager
* IKVM (>= 8)

## Commons-IP (Java)

The base of this project is the implementation of [E-ARK IP manipulation java library](https://github.com/keeps/commons-ip). The current application use somes strategies to use a wrapper of that project in order to use apply it a .net project.

## IKVM
"IKVM.NET includes the following components: A Java Virtual Machine implemented in .NET. A .NET implementation of the Java class libraries. A tool that translates Java bytecode (JAR files) to .NET IL (DLLs or EXE files)." For more information see [http://www.ikvm.net/](http://www.ikvm.net/)

### Usage
IKVM have a large number of aplications and functionalities, in this project the propose is create a dll file from a jar library. With dll file we can import into a .Net project and use without any complication or major setup.

Download the correct version of IKVM, the IKVM version need to be the same as JDK. The current base project use JDK 1.8, for that, we need to download IKVM 8 from [http://www.frijters.net/ikvmbin-8.1.5717.0.zip](http://www.frijters.net/ikvmbin-8.1.5717.0.zip)

```bash
ikvmc.exe #to view help command
``` 

In the previous version of java (< 8), we don't need (-recurse) parameter. Only need a jar with all dependencies inside, for java 8 we need a jar library and a folder with all 3rd party jar files.

```bash
ikvmc.exe -target:library (PATH-JAR-FILE) -recurse:(FOLDER-PATH-WITH-JAR-DEPENDENCIES) -out:(OUTPUT-DLL-FILENAME)
```

 * (PATH-JAR-FILE) - The location of jar file (ex. "C:\Users\keep\Desktop\CommonsIP\commons-ip-1.0.3.jar");
 * (FOLDER-PATH-WITH-JAR-DEPENDENCIES) - The folder with all previous jar dependencies (ex. "C:\Users\keep\Desktop\CommonsIP\Dependencies\");
 * (OUTPUT-DLL-FILENAME) - Set the filename of .dll output (ex. commons-ip-1.0.3.dll);

To see all commons-ip jar dependencies please see the [pom.xml](https://github.com/keeps/commons-ip/blob/master/pom.xml) file and find all dependencies/versions.

**Note:** Don't worry about the **warnings** when execute the previous command, check only if an error are occurred.

<!--
The file run.ps1 (Powershell script) download, extract (IKVM 8) and create the commons-ip.dll into target folder. That dll file is used into commons-ip-dotnet project to create a SIP.zip file from a .net Windows application.-->

## Commons-ip-dotnet

The project include de commons-ip-X.X.X.dll, the file is present inside target folder and referenced into project, the .net package manager [Nuget](https://www.nuget.org/) download all necessaries references to compile the project.

### Description

The current application use the commons-ip-X.X.X.dll to create a EARKSIP package, and the .net aplication only create a UI (user interface) to help the user to manipulate information. 

The application works like a wizard with 5 steps:
 * Presentation;
 * Package information;
 * Descriptive metadata;
 * Other metadata;
 * Package content;
 * SIP build;

#### Presentation

Only indicates the partners off project and some base information about the stakeholders.

#### Package information

Start the EARKSIP file configuration, the information like SIP identifier or the creator name can be set in this page.

#### Descriptive metadata

Add one or more descritive metadata files, the application **doesn't apply any validation** about the files. Only add the files and set the selected descritive metadata type, present in the top page. This options contains all the types present in the original commns-ip implementation (ex. EAD,EAC-CPF.etc).

#### Other metadata

The current page is optional, this page is used to add other metadata files like information about how file were created and stored, intellectual property rights,etc.

#### Package content

Add all files or folders into one or more representations. The aplication only load files or files inside a folder. If we drop folders inside folders the content are ignored. The default representation name is "Representation". If the content to add is a folder the representation has the same name. The representation name can be changed in the application by two ways. 
1. Edit the cell value;
2. Select multiple rows and click with right button of the mouse and select "Set representation name", this options allows to set the same name to multiple items.

#### SIP build

At this page please select the destination path to save the EARKSIP file and specify the name of package. The output file is a ZIP, when process start the application will show a progress bar and some information about the process, please wait until the finish and start again if you want.

### Performance

The biggest problem of this approach is the performance, to measure this value we test in the same machine the implementation in java and the wrapper in .net project. The test create a SIP file with the following instructions in java and visual basic. 


``` vb
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
```

The next table show the values for 3 tests in every implementation, the java implementation is obviously more quickly around 800 milliseconds. The .net test follow 2 approaches, the first one create the EARKSIP and load all IKVM dll at the same time, the second create the EARKSIP but all IKVM dll already loaded in memory.


| Test number        | Java (ms)          | .net (ms)  | .net 2 (ms)
|:-------------:|:-------------:|:-----:|:-----:
| 1      | 758 | 3994 | 1629
| 2      | 815 | 4015 | 1601
| 3      | 870 | 3884 | 1617


## Creadits
 * Paulo Lima (KEEP SOLUTIONS)

## License
?????