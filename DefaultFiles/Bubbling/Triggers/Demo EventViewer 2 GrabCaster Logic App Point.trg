{
  "Trigger": {
		"IdComponent": "{843008B6-F4E1-4A29-8082-BDC111EA0E99}",
		"Name": "Event Viewer Trigger",
		"Description": "Event Viewer Trigger",
	"TriggerProperties": [
	{
		"Name": "EventLog",
		"Value": "Application"
	}
	]
	},
  "Events": [
    {
	"IdConfiguration": "{DF3F9F3F-938F-43F7-AE39-2DA5F9C1BD9E}",
	"IdComponent": "{A31209D7-C989-4E5D-93DA-BD341D843870}",
	"Name": "Send to embedded library",
	"Description": "Send a eventviewer message to embedded library",
		"Channels": [
        {
          "ChannelId": "*",
          "ChannelName": "Channel Name",
          "ChannelDescription": "Channel Description",
          "Points": [
            {
              "PointId": "*",
              "Name": "Point Name",
              "Description": "Point Description"
            }
          ]
        }
	]
    }
  ]
}