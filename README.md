TAS Request Form
=============

SUNY Cortland Test Administration Services Software

Server Requirements
=============
Windows Server 2012 (Can probably use older versions)
IIS 8 (Again, can probably use previous versions)
.NET 4.5 installed
ODBC System DSN to connect to Oracle (the application queries Banner and also it’s own tables in Oracle)
CAS Single Sign-On

Install
=============

1.) Run all SQL files for Oracle installation.

2.) Open project in Visual Studio and download all Nuget dependencies.
			
3.) Web.Config
		
    Add Connection String
    Add CasHost key
    Add TestEmailAddress key - This is used when debug mode is turned on
		
4.) Run the following SQL command to initialize an administrator - replace ADMIN_USER_PIDM with the pidm of any admin accounts:
    
    INSERT INTO tas_administrators (seq_tas_administrators_id.nextval, ADMIN_USER_PIDM)
    
5.) Run locally or set up application in IIS.

Automated Scripts
=============
The API built into the software requires a shared key between automated scripts and the actual web API.

To setup a shared key, make the following changes:

1.) Edit TASRequestForm.Web/Attributes/WebApiAuthorizeAttribute.cs line 27
    
    private const string KEY = "YOUR_KEY";
    
    Change the string to whatever shared key you wish to use.
    
2.) Edit scripts/TASFunctions.ps1

    $sUsername variable will need to be set to an administrator's pidm.
    $sPassword is the shared key you set above.
    $sUri is the URI where the software is running.
    
3.) Run the function tas_run_automated_tasks on a 5 minute schedule.
