{
  "Trigger": {
		"IdComponent": "{7920EE0F-CAC8-4ABB-82C2-1C69351EDD28}",
		"Name": "SQL Server Trigger",
		"Description": "SQL Server Trigger",
	"TriggerProperties": [
	{
		"Name": "SqlQuery",
		"Value": "SELECT *  FROM TableDemo where flag <> 0 for xml auto;update TableDemo set flag=0"
	},
	{
		"Name": "ConnectionString",
		"Value": "Data Source=.;Initial Catalog=Demo;Integrated Security=True"
	}
	]
	},
  "Events": [
    {
		"IdConfiguration": "{035B9C88-C4F4-48B9-9AE2-080EF4754E18}",
		"IdComponent": "{90662D0F-9BBD-4E74-A12D-79BCC0B76BAA}",
		"Name": "Event Notepad",
		"Description": "Event Notepad"
    }
  ]
}