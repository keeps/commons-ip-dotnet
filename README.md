# commons-ip-dotnet
Provides an API to manipulate Information Packages

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

The file run.ps1 (Powershell script) download, extract (IKVM 8) and create the commons-ip.dll into target folder. That dll file is used into commons-ip-dotnet project to create a SIP.zip file from a Windows application.

## Commons-ip-dotnet

### Requirements

* Visual Studio (>= 2015)
* NuGet manager

The project include de commons-ip.dll referenced into project, and NuGet download all necessaries references. 
