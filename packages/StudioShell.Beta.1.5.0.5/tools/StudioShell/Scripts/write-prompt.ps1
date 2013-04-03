#
#   Copyright (c) 2011 Code Owls LLC, All Rights Reserved.
#
#   Licensed under the Microsoft Reciprocal License (Ms-RL) (the "License");
#   you may not use this file except in compliance with the License.
#   You may obtain a copy of the License at
#
#     http://www.opensource.org/licenses/ms-rl
#
#   Unless required by applicable law or agreed to in writing, software
#   distributed under the License is distributed on an "AS IS" BASIS,
#   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#   See the License for the specific language governing permissions and
#   limitations under the License.
#


write-host;
prompt | write-host -nonewline;

<#
.SYNOPSIS
Write the prompt string to the host.

.DESCRIPTION
This is a wrapper for forcing the display of a changed prompt.  It exists for use in script commands that change the current directory value, so they can update the prompt appearance in the console.

.INPUTS
None.

.OUTPUTS
None.

.EXAMPLE
$path = "commandBars:/context menus/Project and Solution Context Menus/Project/Mount";
new-item $path -type button -value { 
	$selectedProjects | foreach { 
		pushd "projects:/$($_.PSChildName)"; 
		write-prompt; 
		invoke-item 'dte:/windows/studioshell';
	} 
};

This example shows write-prompt being invoked to update the host prompt display as part of a context menu command script.
#>
