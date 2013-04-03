# ensure pswatch is in the modules list
if(-not (module | ? Name -eq PsWatch)){
    "Installing PsWatch"
    iex ((new-object net.webclient).DownloadString("http://bit.ly/Install-PsWatch"))
}

Import-Module pswatch

$location = "$pwd\..\tracksStackd\"

"Watching $location for file changes."

watch $location -includeSubdirectories | item |
    where Extension -eq ".cs" |
    foreach {
        "Change to $_.Path, running build."
        invoke-psake .\default.ps1 -nologo
    }

function Ensure-Module($module){

  $moduleList = @{PsWatch = "http://bit.ly/Install-PsWatch"}

  # if psget is installed test for module
  #reference download location
  if(-not (module | ? Name -eq $module)){
    "Installing $module"
    iex ((new-object net.webclient).DownloadString("http://bit.ly/Install-PsWatch"))
  }

  Import-Module pswatch
}