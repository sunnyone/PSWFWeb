PSWFWeb - PowerShell Workflow WebConsole
=========================================

What's this?
--------------
This is an application to launch PowerShell workflows from web browsers.

First usage
------------
1. Place an .ps1 file to the "workflows" folder.
Define a workflow with the same name of filename. For example, define "Sample" if the filename is "Sample.ps1"

See SampleWorkflow.ps1 for example.

2. Execute PSWFWeb.exe by administrator.
Check the url on the console.

3. Access to the url
Initial password is powershell / powershell.

Proper setup
------
1. Change the port number and password
Configs are defined at config.xml. Generate a password hash with the command:
  PSWFWeb /password YOUR-PASSWORD

2. Permit 'urlacl'
Execute netsh command to permit listing the port.
  > netsh http add urlacl url=http://127.0.0.1:3579/ user=USERNAME

3. Register the service
Register the service with the command that is shown with the 'PSWFWeb.exe /help' command.

4. Start the service
Start PSWFService. In powershell, use this:
  PS> Start-Service PSWFWeb
  
Links
-------
* Nancy - Lightweight Web Framework for .net
  http://nancyfx.org/

* Pure CSS framework
  http://purecss.io/

Contact
--------
Author: Yoichi Imai <sunnyone41@gmail.com>
