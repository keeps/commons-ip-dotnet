# commons-ip-dotnet
Provides an API to manipulate Information Packages

### Requirements

* Visual Studio (>= 2015)
* .net Framework (>= 4.7.2)
* NuGet manager
* IKVM (>= 8)

## IKVM
"IKVM.NET includes the following components: A Java Virtual Machine implemented in .NET. A .NET implementation of the Java class libraries. A tool that translates Java bytecode (JAR files) to .NET IL (DLLs or EXE files)." For more information see [http://www.ikvm.net/](http://www.ikvm.net/)

### Usage
IKVM have a large number of aplications and functionalities, in this project the propose is create a dll file from a jar library. With dll file we can import into a .Net project and use without any complication or major setup.

Download the correct version of IKVM, the IKVM version need to be the same as JDK. The current project has [CommonsIP java implemention](https://github.com/keeps/commons-ip) as base project, and that implementation use JDK 1.8. For that, we need to download IKVM 8 from [http://www.frijters.net/ikvmbin-8.1.5717.0.zip](http://www.frijters.net/ikvmbin-8.1.5717.0.zip)

```bash
	ikvmc.exe #to view help command
``` 

In the previous version of java (< 8), we don't need (-recurse) parameter. Only need a jar with all dependencies inside, for java 8 we need a jar library and a folder with all 3rd party jar files.

```bash
	ikvmc.exe -target:library (PATH-JAVAR-FILE) -recurse:(FOLDER-PATH-WITH-JAR-DEPENDENCIES) -out:(OUTPUT-DLL-FILENAME)
```

The file run.ps1 (Powershell script) download, extract (IKVM 8) and create the commons-ip.dll into target folder. That dll file is used into commons-ip-dotnet project to create a SIP.zip file from a .net Windows application.

## Commons-ip-dotnet

The project include de commons-ip-X.X.X.dll referenced into project, and NuGet download all necessaries references.

### Description

The current application use the commons-ip-X.X.X.dll to create a EARKSIP package, and the .net aplication only create a UI (user interface) to help the user to manipulate information. 
The application works like a wizard setup with 5 steps:
 * Presentation;
 * Package information;
 * Descriptive metadata;
 * Other metadata;
 * Package content;
 * SIP build;

#### Presentation

Only indicates the partners off project and some base information

#### Package information

Start the SIP file configuration, the information like SIP identifier or the creator name can be set in this page.

#### Descriptive metadata

Add the one or more descritive metadata files, the application doesn't apply any validation about the files. Only add the files and set the selected descritive metadata type, present in the top page. This options contains all the types present in the original commns-ip implementation.

#### Other metadata

The current page is optional, this page is used to add other metadata files like ....

#### Package content

Add all files or folder into one or more repsentations. The aplication only load files or files inside a folder. If we drop folders inside folders the content are ignored. The defautl representation name is "Representation". If the content to add is a folder the representation has the same name. The representation name can be changed in the application by two ways. 
1. Edit the cell value;
2. Select multiple rows and click with right button of the mouse and select "Set representation name", this options allows to set the same name to multiple items.

#### SIP build

At this page please select the destination path to save the EARKSIP file and specify the name of package. The output file is a ZIP, when process start the application will show a progress bar and some information about the process, please wait until the finish and start again if you want.


