### UI Automation Utilities - WinUIDriver

I'm just extracting some utility code I found useful when testing a webform application.   It is a simple wrapper over Microsoft's UIAutomation library.


## Inspect.exe (or UIInspect.exe, depending on .NET version)

The UIAutamation code provides functionality for element discovery, the second is action.  Inspect.exe is used for implement discovery.  It installs as part of the Windows .NET SDK.  More info: http://msdn.microsoft.com/en-us/library/dd318521%28v=vs.85%29.aspx

Update:  Window Detective is easier to get, I haven't tried it yet but its probably better overall: http://sourceforge.net/projects/windowdetective

## Useful docs related to automating the Windows user interface

"UI Automation Fundamentals" - http://msdn.microsoft.com/en-us/library/ms753107.aspx

"Obtaining UI Automation Elements" (for element discovery) - http://msdn.microsoft.com/en-us/library/ms752331.aspx

"UI Automation Control Patterns Overview" (for element manipulation) - http://msdn.microsoft.com/en-us/library/ms752362.aspx
