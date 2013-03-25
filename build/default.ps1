include ".\build.ps1"

Framework "4.0"

properties {
	$applicationName = "tracksStackd"
	$buildConfig = "Debug"
}

#Messages
properties {
  $testMessage = 'Executed Test!'
  $compileMessage = 'Executed Compile!'
  $cleanMessage = 'Executed Clean!'
}

# Files
properties { 
	$executingDir = new-object System.IO.DirectoryInfo $pwd
	$baseDir = resolve-path .
	$sourceDir = "."
	$solutionFile = "$applicationName.sln"
	$buildOutputDir = "..\Deploy"
    $testAssemblies = (Get-ChildItem (Resolve-Path $buildOutputDir) -Recurse -Include *Tests.dll)
}

# default task - called with 'psake' command
task default -depends Test

# Run Tests
task Test -depends Compile, Clean -description "Run the project test cases"{ 
  declare-task $testMessage

  foreach ($test in $testAssemblies)
  {
      exec {nunit-console $test /nologo /nodots /labels}
  }
}

task integration-tests -depends compile -description "Runs the set of integration tests against an actual store" {
    declare-task "Integration tests"

    foreach ($test in $testAssemblies)
  {
      exec {nunit-console $test /include:Integration /nologo /nodots /labels}
  }
}

# Compile source code
task Compile -depends Clean -description "Compile the project source code"{ 
  declare-task "Compiling"
  msbuild /p:Configuration=$buildConfig /p:OutDir=$buildOutputDir /verbosity:minimal /consoleLoggerparameters:ErrorsOnly /nologo /m "..\$applicationName.sln"
  new-balloontip -tiptext "Build" -tiptitle "Compiling"
  #$compileMessage
}

# Compile Source code
task CompileDebug -depends Clean { 
  
  msbuild /p:Configuration="Debug" /p:OutDir=$buildOutputDir /verbosity:minimal /consoleLoggerparameters:ErrorsOnly /nologo /m "..\$applicationName.sln"
  
  $compileMessage
}

# 
task Clean  -description "Clean the project"{ 
  declare-task $cleanMessage
  remove-item "$buildOutputDir\*.*"
  #Write-today
}

# Deploy the project
task Deploy -depends Test  -Description "Create a project deployment"{
	msbuild /p:Configuration="Release" /p:OutDir=$buildOutputDir /verbosity:minimal /consoleLoggerparameters:ErrorsOnly /nologo /m "$applicationName.sln"
	"Deploying"
}

task Run -depends Compile -description "Runs the tracks rest shell" {
    & "$buildOutputDir\tracks.shell.exe"
}

task Client -description "build the client-side javascript." {

    if((program-exist grunt)){
        grunt    
    }
}

task Watch -description "Starts project monitoring" {
  .\Watch-Autobuild.ps1
}

task install-test -description "Self-install components in order to do testing" {
    ensure-program "nunit.runners" 
}

# Documentation
task ? -Description "Helper to display task info" {
	Write-Documentation
	"psake compile - compile source code"
	"psake compiledebug - compile source code with"
	"psake clean"
	"psake deploy"
    "psake client"
    "psake watch"
    "psake run"
}