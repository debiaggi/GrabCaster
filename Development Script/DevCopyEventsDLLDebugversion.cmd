echo off
cd..
mkdir Framework\bin\Debug\Root_GrabCaster\Events

REM COPY DLL
copy Events\AzureBlobEvent\bin\Debug\GrabCaster.SDK.AzureBlobEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\AzureQueueEvent\bin\Debug\GrabCaster.SDK.AzureQueueEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\AzureTopicEvent\bin\Debug\GrabCaster.SDK.AzureTopicEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\BULKSQLServerEvent\bin\Debug\GrabCaster.SDK.BulksqlServerEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\CSharpEvent\bin\Debug\GrabCaster.SDK.CSharpEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\DialogBoxEvent\bin\Debug\GrabCaster.SDK.DialogBoxEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\EventHubEvent\bin\Debug\GrabCaster.SDK.EventHubEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\FileEvent\bin\Debug\GrabCaster.SDK.FileEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\MessageBoxEvent\bin\Debug\GrabCaster.SDK.MessageBoxEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\NOPEvent\bin\Debug\GrabCaster.SDK.NopEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\PowershellEvent\bin\Debug\GrabCaster.SDK.PowerShellEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\TwilioEvent\bin\Debug\GrabCaster.SDK.TwilioEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\ChatEvent\bin\Debug\GrabCaster.SDK.ChatEvent.dll Framework\bin\Debug\Root_GrabCaster\Events\* /y

REM COPY PDB
copy Events\AzureBlobEvent\bin\Debug\GrabCaster.SDK.AzureBlobEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\AzureQueueEvent\bin\Debug\GrabCaster.SDK.AzureQueueEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\AzureTopicEvent\bin\Debug\GrabCaster.SDK.AzureTopicEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\BULKSQLServerEvent\bin\Debug\GrabCaster.SDK.BulksqlServerEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\CSharpEvent\bin\Debug\GrabCaster.SDK.CSharpEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\DialogBoxEvent\bin\Debug\GrabCaster.SDK.DialogBoxEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\EventHubEvent\bin\Debug\GrabCaster.SDK.EventHubEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\FileEvent\bin\Debug\GrabCaster.SDK.FileEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\MessageBoxEvent\bin\Debug\GrabCaster.SDK.MessageBoxEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\NOPEvent\bin\Debug\GrabCaster.SDK.NopEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\PowershellEvent\bin\Debug\GrabCaster.SDK.PowerShellEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\TwilioEvent\bin\Debug\GrabCaster.SDK.TwilioEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
copy Events\ChatEvent\bin\Debug\GrabCaster.SDK.ChatEvent.PDB Framework\bin\Debug\Root_GrabCaster\Events\* /y
cd %~dp0