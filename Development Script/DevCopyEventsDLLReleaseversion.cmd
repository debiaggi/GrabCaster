echo off
cd..
mkdir Framework\bin\Release\Root_GrabCaster\Events
copy Events\AzureBlobEvent\bin\Release\GrabCaster.SDK.AzureBlobEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\AzureQueueEvent\bin\Release\GrabCaster.SDK.AzureQueueEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\AzureTopicEvent\bin\Release\GrabCaster.SDK.AzureTopicEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\BULKSQLServerEvent\bin\Release\GrabCaster.SDK.BulksqlServerEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\CSharpEvent\bin\Release\GrabCaster.SDK.CSharpEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\DialogBoxEvent\bin\Release\GrabCaster.SDK.DialogBoxEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\EventHubEvent\bin\Release\GrabCaster.SDK.EventHubEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\FileEvent\bin\Release\GrabCaster.SDK.FileEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\MessageBoxEvent\bin\Release\GrabCaster.SDK.MessageBoxEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\NOPEvent\bin\Release\GrabCaster.SDK.NopEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\PowershellEvent\bin\Release\GrabCaster.SDK.PowerShellEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\TwilioEvent\bin\Release\GrabCaster.SDK.TwilioEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
copy Events\ChatEvent\bin\Release\GrabCaster.SDK.ChatEvent.dll Framework\bin\Release\Root_GrabCaster\Events\* /y
cd %~dp0