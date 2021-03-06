echo off
cls
echo Build the solution before running the bach.
pause

cd..
rd /s /q Setup\bin\Debug\Deploy
echo Deploy directory removed.
pause
cd %~dp0

call DeployEventsDLLDebugversion.cmd
call DeployTriggerDLLDebugversion.cmd
call DeployEventsDLLReleaseversion.cmd
call DeployTriggerDLLReleaseversion.cmd

cd..

xcopy DefaultFiles\Demo\File2File\TestFile.txt Setup\bin\Debug\Deploy\*  /y
xcopy DefaultFiles\License.txt Setup\bin\Debug\Deploy\*  /y
xcopy "Batch Files\Create new Clone.cmd" Setup\bin\Debug\Deploy\*  /y
xcopy "Documentation\GrabCaster v1.0- Technical Manual.pdf" Setup\bin\Debug\Deploy\Documentation\ /s /y /e
xcopy Laboratory\bin\Debug\Laboratory.exe Setup\bin\Debug\Deploy\Demo\*  /y
xcopy Laboratory\bin\Release\Laboratory.exe Setup\bin\Release\Deploy\Demo\*  /y
xcopy Framework\bin\Debug\*.dll Setup\bin\Debug\Deploy\*  /y
xcopy Framework\bin\Debug\*.pdb Setup\bin\Debug\Deploy\*  /y
copy DefaultFiles\DeployDefault.cfg Setup\bin\Debug\Deploy\GrabCaster.cfg  /y
copy DefaultFiles\license.rtf Setup\bin\Debug\Deploy\license.rtf  /y
copy DefaultFiles\license.rtf Setup\bin\Release\Deploy\license.rtf  /y
xcopy Framework\bin\Debug\*.exe Setup\bin\Debug\Deploy\*  /y

xcopy Framework\bin\Release\*.dll Setup\bin\Release\Deploy\*  /y
copy DefaultFiles\DeployDefault.cfg Setup\bin\Release\Deploy\GrabCaster.cfg  /y
xcopy Framework\bin\Release\*.exe Setup\bin\Release\Deploy\*  /y

xcopy DefaultFiles\BubblingDeploy Setup\bin\Debug\Deploy\Root_GrabCaster\Bubbling\ /s /y /e
xcopy DefaultFiles\BubblingDeploy Setup\bin\Release\Deploy\Root_GrabCaster\Bubbling\ /s /y /e




xcopy SDK Setup\bin\Debug\Deploy\SDK\ /s /y /e
xcopy SDK Setup\bin\Release\Deploy\SDK\ /s /y /e

xcopy DefaultFiles\Log Setup\bin\Debug\Deploy\Log\* /s /y /e
xcopy DefaultFiles\Log Setup\bin\Release\Deploy\Log\* /s /y /e

xcopy DefaultFiles\Demo Setup\bin\Debug\Deploy\Demo\* /s /y /e
xcopy DefaultFiles\Samples Setup\bin\Debug\Deploy\Samples\* /s /y /e
xcopy DefaultFiles\PersistentStorage Setup\bin\Debug\Deploy\PersistentStorage\* /s /y /e

copy Framework.Log.EventHubs\bin\Debug\GrabCaster.Framework.Log.EventHubs.dll Setup\bin\Debug\Deploy\Root_GrabCaster\* /y
copy Framework.Log.EventHubs\bin\Debug\GrabCaster.Framework.Log.EventHubs.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\* /y
copy Framework.Log.EventHubs\bin\Release\GrabCaster.Framework.Log.EventHubs.dll Setup\bin\Release\Deploy\Root_GrabCaster\* /y
copy Framework.Log.File\bin\Debug\GrabCaster.Framework.Log.File.dll Setup\bin\Debug\Deploy\Root_GrabCaster\* /y
copy Framework.Log.File\bin\Debug\GrabCaster.Framework.Log.File.pdb Setup\bin\Debug\Deploy\Root_GrabCaster\* /y
copy Framework.Log.File\bin\Release\GrabCaster.Framework.Log.File.dll Setup\bin\Release\Deploy\Root_GrabCaster\* /y


copy Framework.Dcp.Azure\bin\Debug\GrabCaster.Framework.Dcp.Azure.dll Setup\bin\Debug\Deploy\Root_GrabCaster\* /y
copy Framework.Dcp.Azure\bin\Release\GrabCaster.Framework.Dcp.Azure.dll Setup\bin\Release\Deploy\Root_GrabCaster\* /y

copy Framework.Dcp.Redis\bin\Debug\GrabCaster.Framework.Dcp.Redis.dll Setup\bin\Debug\Deploy\Root_GrabCaster\* /y
copy Framework.Dcp.Redis\bin\Release\GrabCaster.Framework.Dcp.Redis.dll Setup\bin\Release\Deploy\Root_GrabCaster\* /y

copy Framework.Dpp.Azure\bin\Debug\GrabCaster.Framework.Dpp.Azure.dll Setup\bin\Debug\Deploy\Root_GrabCaster\* /y
copy Framework.Dpp.Azure\bin\Release\GrabCaster.Framework.Dpp.Azure.dll Setup\bin\Release\Deploy\Root_GrabCaster\* /y

xcopy GrabCasterAdapter\Runtime\bin\Debug\GrabCaster.Framework.BizTalk.Adapter.dll Setup\bin\Debug\Deploy\*  /y
xcopy GrabCasterAdapter\Runtime\bin\Debug\GrabCaster.Framework.BizTalk.Common.dll Setup\bin\Debug\Deploy\*  /y
xcopy GrabCasterAdapter\Runtime\bin\Debug\GrabCaster.Framework.BizTalk.Adapter.Designtime.dll Setup\bin\Debug\Deploy\*  /y
xcopy GrabCasterAdapter\Runtime\bin\Debug\GrabCaster.Framework.BizTalk.Adapter.pdb Setup\bin\Debug\Deploy\*  /y
xcopy GrabCasterAdapter\Runtime\bin\Debug\GrabCaster.Framework.BizTalk.Common.pdb Setup\bin\Debug\Deploy\*  /y
xcopy GrabCasterAdapter\Runtime\bin\Debug\GrabCaster.Framework.BizTalk.Adapter.Designtime.pdb Setup\bin\Debug\Deploy\*  /y

xcopy GrabCasterAdapter\GrabCaster.reg Setup\bin\Debug\Deploy\*  /y
xcopy "GrabCasterAdapter\Register BizTalk Adapter.txt" Setup\bin\Debug\Deploy\*  /y
copy DefaultFiles\BTSNTSvc.cfg Setup\bin\Debug\Deploy\BTSNTSvc.cfg  /y

echo Console area, just replicate the bubbling and copy files.
xcopy DefaultFiles\GCPoints Setup\bin\Debug\Deploy\GCPoints\ /s /y /e
xcopy DefaultFiles\GCPoints Setup\bin\Release\Deploy\GCPoints\ /s /y /e
copy DefaultFiles\DeployDefault.cfg Setup\bin\Debug\Deploy\GrabCasterUI.cfg  /y
copy DefaultFiles\DeployDefault.cfg Setup\bin\Release\Deploy\GrabCasterUI.cfg  /y

xcopy Setup\bin\Debug\Deploy\Root_GrabCaster Setup\bin\Debug\Deploy\Root_GrabCasterUI\ /s /y /e
xcopy Setup\bin\Release\Deploy\Root_GrabCaster Setup\bin\Release\Deploy\Root_GrabCasterUI\ /s /y /e

xcopy GrabCasterUI\bin\Debug\GrabCasterUI.exe Setup\bin\Debug\Deploy\*  /y
xcopy GrabCasterUI\bin\Release\GrabCasterUI.exe Release\bin\Debug\Deploy\*  /y

cd %~dp0
echo Deployment package ready to go.

