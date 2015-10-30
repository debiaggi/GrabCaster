echo off
cd..
mkdir Framework\bin\Release\Root_GrabCaster\Triggers
copy Triggers\AzureBlobTrigger\bin\Release\GrabCaster.SDK.AzureBlobTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureQueueTrigger\bin\Release\GrabCaster.SDK.AzureQueueTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureTopicTrigger\bin\Release\GrabCaster.SDK.AzureTopicTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\BULKSQLServerTrigger\bin\Release\GrabCaster.SDK.BulksqlServerTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\CSharpTrigger\bin\Release\GrabCaster.SDK.CSharpTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\ETW\bin\Release\GrabCaster.SDK.EtwTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\EventHubsTrigger\bin\Release\GrabCaster.SDK.EventHubsTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\EventViewerTrigger\bin\Release\GrabCaster.SDK.EventViewerTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\FileTrigger\bin\Release\GrabCaster.SDK.FileTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\NOPTrigger\bin\Release\GrabCaster.SDK.NopTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\PowershellTrigger\bin\Release\GrabCaster.SDK.PowershellTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\RfidTrigger\bin\Release\GrabCaster.SDK.RfidTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\SQLServerTrigger\bin\Release\GrabCaster.SDK.SqlServerTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
copy Triggers\ChatTrigger\bin\Release\GrabCaster.SDK.ChatTrigger.dll Framework\bin\Release\Root_GrabCaster\Triggers\* /y
cd %~dp0