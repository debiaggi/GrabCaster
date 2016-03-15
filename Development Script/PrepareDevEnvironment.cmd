echo off
echo Build the solution before running the bach.
pause

cd..
rd /s /q Framework\bin\Debug\Root_GrabCaster
cd %~dp0

call DevDefaultBubbligFiles2BubblingDirs.cmd
call DevDefaultConfigurationFile2DebugReleaseDirs.cmd
call DevCopyBatchFiles2DebugReleaseDirs.cmd
call DevCopyEventsDLLDebugversion.cmd
call DevCopyTriggerDLLDebugversion.cmd
call DevCopyEventsDLLReleaseversion.cmd
call DevCopyTriggerDLLReleaseversion.cmd

cd..
xcopy "Batch Files\Create new Clone.cmd" Framework\bin\Debug\*  /y
xcopy "Batch Files\Create new Clone.cmd" Framework\bin\Release\*  /y
copy Framework.Log.EventHubs\bin\Debug\GrabCaster.Framework.Log.EventHubs.dll Framework\bin\Debug\Root_GrabCaster\* /y
copy Framework.Log.EventHubs\bin\Debug\GrabCaster.Framework.Log.EventHubs.pdb Framework\bin\Debug\Root_GrabCaster\* /y
copy Framework.Log.EventHubs\bin\Release\GrabCaster.Framework.Log.EventHubs.dll Framework\bin\Release\Root_GrabCaster\* /y

copy Framework.Log.File\bin\Debug\GrabCaster.Framework.Log.File.dll Framework\bin\Debug\Root_GrabCaster\* /y
copy Framework.Log.File\bin\Debug\GrabCaster.Framework.Log.File.pdb Framework\bin\Debug\Root_GrabCaster\* /y
copy Framework.Log.File\bin\Release\GrabCaster.Framework.Log.File.dll Framework\bin\Release\Root_GrabCaster\* /y

copy Framework.Dcp.Azure\bin\Debug\GrabCaster.Framework.Dcp.Azure.dll Framework\bin\Debug\Root_GrabCaster\* /y
copy Framework.Dcp.Azure\bin\Debug\GrabCaster.Framework.Dcp.Azure.pdb Framework\bin\Debug\Root_GrabCaster\* /y
copy Framework.Dcp.Azure\bin\Release\GrabCaster.Framework.Dcp.Azure.dll Framework\bin\Release\Root_GrabCaster\* /y

copy Framework.Dcp.Redis\bin\Debug\GrabCaster.Framework.Dcp.Redis.dll Framework\bin\Debug\Root_GrabCaster\* /y
copy Framework.Dcp.Redis\bin\Debug\GrabCaster.Framework.Dcp.Redis.pdb Framework\bin\Debug\Root_GrabCaster\* /y
copy Framework.Dcp.Redis\bin\Release\GrabCaster.Framework.Dcp.Redis.dll Framework\bin\Release\Root_GrabCaster\* /y

copy Framework.Dpp.Azure\bin\Debug\GrabCaster.Framework.Dpp.Azure.dll Framework\bin\Debug\Root_GrabCaster\* /y
copy Framework.Dpp.Azure\bin\Debug\GrabCaster.Framework.Dpp.Azure.pdb Framework\bin\Debug\Root_GrabCaster\* /y
copy Framework.Dpp.Azure\bin\Release\GrabCaster.Framework.Dpp.Azure.dll Framework\bin\Release\Root_GrabCaster\* /y

xcopy DefaultFiles\Log Framework\bin\Debug\Log\* /s /y /e
xcopy DefaultFiles\Log Framework\bin\Debug\Log\* /s /y /e

cd %~dp0

