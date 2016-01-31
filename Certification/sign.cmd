makecert -sv grabcaster.pvk -n "CN=GrabCaster Ltd" grabcaster.cer -r -sr LocalMachine -ss Root
cert2spc.exe grabcaster.cer grabcaster.spc
pvk2pfx -pvk grabcaster.pvk  -pi password -spc grabcaster.cer -pfx grabcaster.pfx -po password
signTool sign /f grabcaster.pfx /p password /v grabcaster.msi