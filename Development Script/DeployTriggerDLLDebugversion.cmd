echo off
cd..
xcopy Triggers\AzureBlobTrigger\bin\Debug\GrabCaster.SDK.AzureBlobTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\AzureQueueTrigger\bin\Debug\GrabCaster.SDK.AzureQueueTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\AzureTopicTrigger\bin\Debug\GrabCaster.SDK.AzureTopicTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\BULKSQLServerTrigger\bin\Debug\GrabCaster.SDK.BulksqlServerTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\CSharpTrigger\bin\Debug\GrabCaster.SDK.CSharpTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\ETW\bin\Debug\GrabCaster.SDK.EtwTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\EventHubsTrigger\bin\Debug\GrabCaster.SDK.EventHubsTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\EventViewerTrigger\bin\Debug\GrabCaster.SDK.EventViewerTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\FileTrigger\bin\Debug\GrabCaster.SDK.FileTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\NOPTrigger\bin\Debug\GrabCaster.SDK.NopTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\PowershellTrigger\bin\Debug\GrabCaster.SDK.PowerShellTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\RfidTrigger\bin\Debug\GrabCaster.SDK.RfidTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\SQLServerTrigger\bin\Debug\GrabCaster.SDK.SqlServerTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
xcopy Triggers\ChatTrigger\bin\Debug\GrabCaster.SDK.ChatTrigger.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Triggers\* /y
cd %~dp0