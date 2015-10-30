echo off
cd..
xcopy Events\AzureBlobEvent\bin\Debug\GrabCaster.SDK.AzureBlobEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\AzureQueueEvent\bin\Debug\GrabCaster.SDK.AzureQueueEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\AzureTopicEvent\bin\Debug\GrabCaster.SDK.AzureTopicEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\BULKSQLServerEvent\bin\Debug\GrabCaster.SDK.BulksqlServerEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\CSharpEvent\bin\Debug\GrabCaster.SDK.CSharpEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\DialogBoxEvent\bin\Debug\GrabCaster.SDK.DialogBoxEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\EventHubEvent\bin\Debug\GrabCaster.SDK.EventHubEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\FileEvent\bin\Debug\GrabCaster.SDK.FileEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\MessageBoxEvent\bin\Debug\GrabCaster.SDK.MessageBoxEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\NOPEvent\bin\Debug\GrabCaster.SDK.NOPEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\PowershellEvent\bin\Debug\GrabCaster.SDK.PowerShellEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\RunProcessEvent\bin\Debug\GrabCaster.SDK.RunProcessEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\TwilioEvent\bin\Debug\GrabCaster.SDK.TwilioEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\ChatEvent\bin\Debug\GrabCaster.SDK.ChatEvent.dll Setup\bin\Debug\Deploy\Root_GrabCaster\Events\* /y
cd %~dp0