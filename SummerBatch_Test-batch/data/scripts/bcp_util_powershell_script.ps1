<#
Never use Write-Host as it writes out to Host UI screen. There is no easy way to redirect it, short of writing our own host.
#>

# Enable Verbose messaging...
$VerbosePreference = "Continue"

# Enable Debug messaging...
$DebugPreference = "Continue"

# if you need to trace at statement level...
#Set-PSDebug -Trace 2

# set script file being executed...
$scriptFile = $MyInvocation.MyCommand.Definition

# Let user know script file being executed...
Write-Output "Start Executing Script File: $scriptFile"

Write-Output "On Entry ScriptExitStatus is $global:ScriptExitStatus"

$bcp_path = "C:\Program Files (x86)\Microsoft SQL Server\90\Tools\Binn\bcp.exe"


try
{
	#Make all errors terminating
	$ErrorActionPreference = "Stop"; 
	
	# cd to working directory for this script...
	Set-Location -Path C:\Users\r.saraf\summerbatch_workspace\SummerBatch_Test-solution\SummerBatch_Test-batch\data\scripts
	Write-Output "Working Directory: $pwd"

	#dot-source helper functions
	. C:\Users\r.saraf\summerbatch_workspace\SummerBatch_Test-solution\SummerBatch_Test-batch\data\scripts\HelperFunctions.ps1

	#=> this was passed  via Variables, i.e. List<FileInfo> $filesToCompare
	
	& $bcp_path $table $operation $unloadFileLocation -w -T -t',' -U $user -S $server\SQLEXPR -P $password
	
	# make sure to set ScriptExitStatus in a global scope...
	$global:ScriptExitStatus = [Summer.Batch.Core.ExitStatus]::Completed
	Write-Output "On Exit ScriptExitStatus is $global:ScriptExitStatus"

} catch {
	
	#we are done...let PowerShellTasklet know something failed...
	#$ErrorMessage = $_.Exception.Message
    #$FailedItem = $_.Exception.ItemName
	
	[string] $errString = Format-Error($_)
	Write-Error $errString -ErrorAction Continue
	#Format-List -Property PositionMessage -InputObject $_.InvocationInfo  -Expand Both | Out-String -Width 512 | Write-Error -ErrorAction Continue

	# using built-in ExitStatus...if set will be used by PowerShellTasklet...
	$global:ScriptExitStatus = [Summer.Batch.Core.ExitStatus]::Failed
	Write-Output "On Catch Block Exit ScriptExitStatus is $global:ScriptExitStatus"

	#=> Exit <> 0 will set ExitStatus of the step to Failed
    Exit 1

}finally{

   $ErrorActionPreference = "Continue"; #Reset the error action pref to default

}

#Trace Message...
Write-Output "End Executing Script File: $(([System.IO.FileInfo]$scriptFileInfo).FullName)"

# Write-Error will add to $Error Array an ErrorRecord element
Write-Error  "Test"

# must specify exit or return status...used by PowerShellExitCodeMapper to set ExitStatus...
Exit 0
