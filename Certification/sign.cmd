makecert -sv grabcaster.pvk -n "CN=GrabCaster Ltd" grabcaster.cer -r -sr LocalMachine -ss Root
cert2spc.exe grabcaster.cer grabcaster.spc

Queste sotto sono le righe

Creare il cer
makecert.exe -sv grabcaster.pvk -n "CN=GrabCaster Ltd" grabcaster.cer

Creare il pfx per visual studio
pvk2pfx -pvk grabcaster.pvk  -pi 4r@bc@st3r -spc grabcaster.cer -pfx grabcaster.pfx -po 4r@bc@st3r

Sign the msi or exe
signTool sign /f grabcaster.pfx /p password /v grabcaster.msi