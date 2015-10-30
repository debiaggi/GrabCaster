echo off
cd..
xcopy Triggers\AzureBlobTrigger\bin\Release\GrabCaster.SDK.AzureBlobTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\AzureQueueTrigger\bin\Release\GrabCaster.SDK.AzureQueueTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\AzureTopicTrigger\bin\Release\GrabCaster.SDK.AzureTopicTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\BULKSQLServerTrigger\bin\Release\GrabCaster.SDK.BulksqlServerTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\CSharpTrigger\bin\Release\GrabCaster.SDK.CSharpTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\ETW\bin\Release\GrabCaster.SDK.EtwTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\EventHubsTrigger\bin\Release\GrabCaster.SDK.EventHubsTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\EventViewerTrigger\bin\Release\GrabCaster.SDK.EventViewerTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\FileTrigger\bin\Release\GrabCaster.SDK.FileTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\NOPTrigger\bin\Release\GrabCaster.SDK.NopTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\PowershellTrigger\bin\Release\GrabCaster.SDK.PowerShellTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\RfidTrigger\bin\Release\GrabCaster.SDK.RfidTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\SQLServerTrigger\bin\Release\GrabCaster.SDK.SqlServerTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\ChatTrigger\bin\Release\GrabCaster.SDK.ChatTrigger.dll Setup\bin\Release\Deploy\Root_GrabCaster\Triggers\* /y
cd %~dp0