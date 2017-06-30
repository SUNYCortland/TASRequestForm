function tas_run_automated_tasks
{
	[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12
	
	tas_remind_instructors
	
	tas_remind_instructors_of_approvals
	
	tas_remind_students
}

function tas_remind_instructors
{
	$sProtocol = "https://"
	$sUri = $sProtocol + "webapp.cortland.edu/TASRequestForm/api/Automation/RemindInstructors.xml"
	$sUsername = "" # Your pidm
	$sPassword = "YOUR_KEY" # Shared Key
	$sAuthorizationToken = "Basic "+[System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($sUsername + ":" + $sPassword))
	
	# Call RemindInstructors API method
	Invoke-WebRequest -Uri $sUri -Headers @{"Authorization" = $sAuthorizationToken} -Method Get
}

function tas_remind_instructors_of_approvals
{
	$sProtocol = "https://"
	$sUri = $sProtocol + "webapp.cortland.edu/TASRequestForm/api/Automation/RemindInstructorsOfApprovals.xml"
	$sUsername = "" # Your pidm
	$sPassword = "YOUR_KEY" # Shared Key
	$sAuthorizationToken = "Basic "+[System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($sUsername + ":" + $sPassword))
	
	# Call RemindInstructorsOfApprovals API method
	Invoke-WebRequest -Uri $sUri -Headers @{"Authorization" = $sAuthorizationToken} -Method Get
}

function tas_remind_students
{
	$sProtocol = "https://"
	$sUri = $sProtocol + "webapp.cortland.edu/TASRequestForm/api/Automation/RemindStudents.xml"
	$sUsername = "" # Your pidm
	$sPassword = "YOUR_KEY" # Shared Key
	$sAuthorizationToken = "Basic "+[System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($sUsername + ":" + $sPassword))
	
	# Call RemindStudents API method
	Invoke-WebRequest -Uri $sUri -Headers @{"Authorization" = $sAuthorizationToken} -Method Get
}