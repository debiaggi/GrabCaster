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
copy Framework.Log.EventHubs\bin\Release\GrabCaster.Framework.Log.EventHubs.dll Framework\bin\Release\Root_GrabCaster\* /y
cd %~dp0

