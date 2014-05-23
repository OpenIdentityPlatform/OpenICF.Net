# DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS HEADER.
#
# Copyright (c) 2014 ForgeRock AS. All Rights Reserved
#
# The contents of this file are subject to the terms
# of the Common Development and Distribution License
# (the License). You may not use this file except in
# compliance with the License.
#
# You can obtain a copy of the License at
# http://forgerock.org/license/CDDLv1.0.html
# See the License for the specific language governing
# permission and limitations under the License.
#
# When distributing Covered Code, include this CDDL
# Header Notice in each file and include the License file
# at http://forgerock.org/license/CDDLv1.0.html
# If applicable, add the following below the CDDL Header,
# with the fields enclosed by brackets [] replaced by
# your own identifying information:
# " Portions Copyrighted [year] [name of copyright owner]"
#
# @author Gael Allioux <gael.allioux@forgerock.com>
#
#REQUIRES -Version 2.0

<#  
.SYNOPSIS  
    This is a sample ResolveUsername script        
.DESCRIPTION
	Parameters:
	The connector sends us the following:
	- <prefix>.Configuration : handler to the connector's configuration object
	- <prefix>.Options: a handler to the Operation Options
	- <prefix>.Action: String correponding to the action ("RESOLVE_USERNAME" here)
	- <prefix>.ObjectClass: a String describing the Object class (__ACCOUNT__ / __GROUP__ / other)
	- <prefix>.Username: Usename String

.RETURNS 
	Must return the user unique ID (__UID__).
	To do so, set the <prefix>.Result.Uid property
	
.NOTES  
    File Name      : Delete.ps1  
    Author         : Gael Allioux (gael.allioux@forgerock.com)
    Prerequisite   : PowerShell V2
    Copyright 2014 - ForgeRock AS    
.LINK  
    Script posted over:  
    http://openicf.forgerock.org
.EXAMPLE  
    Example 1     
#>

# Always put code in try/catch statement and make sure exceptions are rethrown to connector
try
{

if ($Connector.Action -eq "RESOLVE_USERNAME")
{
	if ($Connector.ObjectClass -eq "__ACCOUNT__")
	{
		if ($Connector.Username -eq "TEST1")
		{
		$Connector.Result.Uid = "123"
		} else
		{
			throw New-Object Org.IdentityConnectors.Framework.Common.Exceptions.UnknownUidException
		}
	}
}
}
catch #Rethrow the original exception
{
	throw
}