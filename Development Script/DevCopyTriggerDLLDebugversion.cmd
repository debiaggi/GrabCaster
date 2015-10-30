echo off
cd..
mkdir Framework\bin\Debug\Root_GrabCaster\Triggers

REM COPY DLL
copy Triggers\AzureBlobTrigger\bin\Debug\GrabCaster.SDK.AzureBlobTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureQueueTrigger\bin\Debug\GrabCaster.SDK.AzureQueueTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureTopicTrigger\bin\Debug\GrabCaster.SDK.AzureTopicTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\BULKSQLServerTrigger\bin\Debug\GrabCaster.SDK.BulksqlServerTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\CSharpTrigger\bin\Debug\GrabCaster.SDK.CSharpTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\ETW\bin\Debug\GrabCaster.SDK.EtwTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\EventHubsTrigger\bin\Debug\GrabCaster.SDK.EventHubsTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\EventViewerTrigger\bin\Debug\GrabCaster.SDK.EventViewerTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\FileTrigger\bin\Debug\GrabCaster.SDK.FileTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\NOPTrigger\bin\Debug\GrabCaster.SDK.NopTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\PowershellTrigger\bin\Debug\GrabCaster.SDK.PowerShellTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\RfidTrigger\bin\Debug\GrabCaster.SDK.RfidTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\SQLServerTrigger\bin\Debug\GrabCaster.SDK.SqlServerTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\ChatTrigger\bin\Debug\GrabCaster.SDK.ChatTrigger.dll Framework\bin\Debug\Root_GrabCaster\Triggers\* /y

REM COPY PDB
copy Triggers\AzureBlobTrigger\bin\Debug\GrabCaster.SDK.AzureBlobTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureQueueTrigger\bin\Debug\GrabCaster.SDK.AzureQueueTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\AzureTopicTrigger\bin\Debug\GrabCaster.SDK.AzureTopicTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\BULKSQLServerTrigger\bin\Debug\GrabCaster.SDK.BulksqlServerTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\CSharpTrigger\bin\Debug\GrabCaster.SDK.CSharpTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\ETW\bin\Debug\GrabCaster.SDK.EtwTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\EventHubsTrigger\bin\Debug\GrabCaster.SDK.EventHubsTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\EventViewerTrigger\bin\Debug\GrabCaster.SDK.EventViewerTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\FileTrigger\bin\Debug\GrabCaster.SDK.FileTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\NOPTrigger\bin\Debug\GrabCaster.SDK.NopTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\PowershellTrigger\bin\Debug\GrabCaster.SDK.PowerShellTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\RfidTrigger\bin\Debug\GrabCaster.SDK.RfidTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\SQLServerTrigger\bin\Debug\GrabCaster.SDK.SqlServerTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y
copy Triggers\ChatTrigger\bin\Debug\GrabCaster.SDK.ChatTrigger.pdb Framework\bin\Debug\Root_GrabCaster\Triggers\* /y

cd %~dp0