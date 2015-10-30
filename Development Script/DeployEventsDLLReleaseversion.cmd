echo off
cd..
xcopy Events\AzureBlobEvent\bin\Release\GrabCaster.SDK.AzureBlobEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\AzureQueueEvent\bin\Release\GrabCaster.SDK.AzureQueueEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\AzureTopicEvent\bin\Release\GrabCaster.SDK.AzureTopicEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\BULKSQLServerEvent\bin\Release\GrabCaster.SDK.BulksqlServerEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\CSharpEvent\bin\Release\GrabCaster.SDK.CSharpEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\DialogBoxEvent\bin\Release\GrabCaster.SDK.DialogBoxEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\EventHubEvent\bin\Release\GrabCaster.SDK.EventHubEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\FileEvent\bin\Release\GrabCaster.SDK.FileEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\MessageBoxEvent\bin\Release\GrabCaster.SDK.MessageBoxEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\NOPEvent\bin\Release\GrabCaster.SDK.NOPEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\PowershellEvent\bin\Release\GrabCaster.SDK.PowershellEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\RunProcessEvent\bin\Release\GrabCaster.SDK.RunProcessEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\TwilioEvent\bin\Release\GrabCaster.SDK.TwilioEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
xcopy Events\ChatEvent\bin\Release\GrabCaster.SDK.ChatEvent.dll Setup\bin\Release\Deploy\Root_GrabCaster\Events\* /y
cd %~dp0