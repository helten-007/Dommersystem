@Echo Off
REM This script avoids error 38 "It is an error to a section registered as allowDefinition='MachineToApplication'"
REM by deleting output from Publish routine before building debug

if %1==Debug (
	ECHO  CleanupAfterPublish running
	if exist %2obj\release rd /s /q %2obj\release
	if exist %2bin\release rd /s /q %2bin\release
)
