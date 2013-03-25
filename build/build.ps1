# build functions to assist with the build process

# Configure the formatting of the tasks within the build console log
formatTaskName {
	param($taskName)
	write-host $taskName -foregroundcolor Green
}

function declare-task($taskName){
    "**************"
    "|  $taskName |"
    "**************"
}

function Write-Today()
{
	get-date
}

function Write-Documentation ()
{
	"Each task can be run by using the command: psake"
}

function Create-Folder($folderName)
{
	new-item $folderName -itemtype Directory | out-null
}

function Delete-Directory($folderName)
{
	remove-item -force -recurse $folderName -erroraction SilentlyContinue
}

# Monitors a log file tailing the last addition
function Monitor-Log($path){
	get-content $path -tail 1 -wait
}

function New-BalloonTip {
[CmdletBinding()]
Param(
$TipText='This is the body text.',
$TipTitle='This is the title.',
$TipDuration='10000'
)
	[system.Reflection.Assembly]::LoadWithPartialName('System.Windows.Forms') | Out-Null
	$balloon = New-Object System.Windows.Forms.NotifyIcon
	$path = Get-Process -id $pid | Select-Object -ExpandProperty Path
	$icon = [System.Drawing.Icon]::ExtractAssociatedIcon($path)
	$balloon.Icon = $icon
	$balloon.BalloonTipIcon = 'Info'
	$balloon.BalloonTipText = $TipText
	$balloon.BalloonTipTitle = $TipTitle
	$balloon.Visible = $true
	$balloon.ShowBalloonTip($TipDuration)
}

# Checks if a program exists on the command line
# i.e node, coffee, grunt
# Usage:
# if( -not (program-exists $program)){
#    "You had better go get it"
# }
function Program-Exists($prog){
	try{
	    & $prog --version
	}
	catch [System.Management.Automation.ItemNotFoundException]{
	    return $false;
	}
	catch {
	    return $false;
	}
}

function Get-Version($majorVersion){
	$now = get-date
	$minor = "{0}{1:00}" -f (($now.Year - 12) + $now.Month), $now.Day
	$revision = "{0:00}{1:00}" -f $now.Hour, $now.Minute
	return $majorVersion + "." + $minor
}

function Ensure-Module($module){
    $exists = get-module -ListAvailable $module
    if(-not $exists){
        psget $module
    }
    import-module $module
}

function Ensure-Program($program){
    # leverage chocolatey for program if not exist
    
    $exists = $true
    if(-not $exists){
        chocolatey install $program

        # we might also need to ensure that it's added to path?
    }
    
}