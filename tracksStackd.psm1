# Studioshell solution script which gets loaded when the solution is opened.
#
# There is also a setup.ps1 which will manually setup
import-module psake


$solutionPath = (get-item DTE:\Solution).FullName | split-path -parent
# add PSake to help menu to display the help including available tasks
write-host $solutionPath
$psakeScript = join-path $solutionPath -child build\default.ps1
$psake = (get-item DTE:\Solution).FullName | split-path -parent | join-path -ChildPath build\default.ps1


write-host $psakeScript
new-item dte:\CommandBars\menubar\help\PSake -value {invoke-psake $psake ? | out-outputpane}

new-item "dte:\CommandBars\menubar\build\PSake Test"-value {
  write-host  
  #pop-location
  invoke-psake $psake compile | out-host
  #write-prompt
}

function unregister-tracksStackd
{

  remove-module psake
  remove-item "dte:\commandbars\menubar\build\PSake Test"
  remove-item "dte:\commandbars\menubar\help\PSake"
}