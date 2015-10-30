{
  "Trigger": {
    "IdComponent": "{3C62B951-C353-4899-8670-C6687B6EAEFC}",
    "Name": "FileTrigger Remote To Out",
    "Description": "Get file from disc",
    "TriggerProperties": [{
			"Name": "RegexFilePattern",
			"Value": ".(txt|a)"
		},
		{
			"Name": "DoneExtensionName",
			"Value": "done"
		},
		{
			"Name": "PollingTime",
			"Value": "5000"
		},
		{
			"Name": "InputDirectory",
			"Value": "C:\\Program Files (x86)\\GrabCaster\\Demo\\File2File\\In_LocalRemote"
		}]
	},
  "Events": [
    {
	"IdConfiguration": "{F4CF1B12-4EC4-43FA-8842-575A5E4CDE5A}",
	"IdComponent": "{D438C746-5E75-4D59-B595-8300138FB1EA}",
	"Name": "File2File Demo Event ",
	"Description": "Write file to the GrabCaster Demo OUT remote local folder",
	"Channels": [
        {
          "ChannelId": "{047B6D1E-A991-4CB1-ACAB-E83C3BDC0098}",
          "ChannelName": "Channel Name",
          "ChannelDescription": "Channel Description",
          "Points": [
            {
              "PointId": "{B0A46E60-443C-4E8A-A6ED-7F2CB34CF9E6}",
              "Name": "Point Name",
              "Description": "Point Description"
            }
          ]
        }
      ]
    }
  ]
}