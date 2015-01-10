==============================================================================
DANSrv customizable OPC Server                       Simple Sample Application
==============================================================================
Copyright 2004 Advosol Inc.  All rights reserved.


This C# Customization Plug-in shows a simple server implementation.
At startup a few items are statically defined. No custom item properties are supported.
The RefreshThread simulates signal changes and writes the incremented values into the server cache.
Item values written by a client are written into the local buffer and the RefreshThread
increments the values from this new item value.


Files in this sample:
- ServerAdapt.cs
    The AppPlugin class inherits GenericServer and contains the methods that are called by 
    the generic server and need to be implemented application specific.
- IGeneric.cs
    Defines the generic server interface. DON'T CHANGE THIS FILE.
    It contains definitions, callback methods and default implementations of the methods
    call by the generic server.
- AssemblyInfo.cs
    Standard .NET assembly definitions.

- DANSrv.exe
    This is the generic OPC DA V2/V3 server
- RegServer.bat
    Batch file that register the server
- DANSrv.EXE.CONFIG
    This is a sample of an optional configuration file.
    See the sample method ReadAppConfiguation() for a sample how definitions are read.



Post Build Steps
After a successful compilation the following steps are executed in the post build event:
1.  copy the files  DANSrv.exe
                    RegServer.bat
    from the project directory into the bin\debug directory
2.  Execute RegServer.bat in the bin\debug directory
					
The server is now registered and can be accessed by OPC DA V2/V3 clients.

Debugging
To debug the plug-in assembly you need to:
1.  Open project properties and select  Configuration Properties  -  Debugging
    Select 'Start external program' and browse to DANSrv.exe in bin\debug
2.  Set Breakpoints
3.  Start the program execution

The generic server is started and the plug-in method GetServerRegistryDef is called.
The server execution is fully started when the first client connects. Then the plug-in 
methods GetServerParameters and CreateServerItems are called. 
The further activity depends on the plug-in and he client access.


