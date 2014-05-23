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
    This is a sample Create script        
.DESCRIPTION

.INPUT VARIABLES
	The connector sends us the following:
	- <prefix>.Configuration : handler to the connector's configuration object
	- <prefix>.Options: a handler to the Operation Options
	- <prefix>.Action: String correponding to the action ("CREATE" here)
	- <prefix>.ObjectClass: a String describing the Object class (__ACCOUNT__ / __GROUP__ / other)
	- <prefix>.Attributes: A collection of ConnectorAttributes representing the entry attributes
	- <prefix>.Id: Corresponds to the OpenICF __NAME__ atribute if it is provided as part of the attribute set,
	 otherwise null
	
.RETURNS
	Must return the user unique ID (__UID__).
	To do so, set the <prefix>.Result.Uid property

.NOTES  
    File Name      : Create.ps1  
    Author         : Gael Allioux (gael.allioux@forgerock.com)
    Prerequisite   : PowerShell V2
    Copyright 2014 - ForgeRock AS    
.LINK  
    Script posted over:  
    http://openicf.forgerock.org
#>

# Always put code in try/catch statement and make sure exceptions are rethrown to connector
try
{
if ($Connector.Action -eq "CREATE")
{
	switch ($Connector.ObjectClass)
	{
		"__ACCOUNT__" {$Connector.Result.Uid = "123"}
		"__GROUP__" {throw "Unsupported operation"}
		"__ALL__" {Write-Error "ICF Framework MUST REJECT this"; break}
		"__TEST__" {
			switch ($Connector.Id)
			{
				"TEST1" {throw New-Object Org.IdentityConnectors.Framework.Common.Exceptions.AlreadyExistsException}
				"TEST2" {throw New-Object Org.IdentityConnectors.Framework.Common.Exceptions.InvalidAttributeValueException}
				"TEST3" {throw New-Object System.ArgumentException}
				"TEST4" 
				{
					$cause = New-Object Org.IdentityConnectors.Framework.Common.Exceptions.OperationTimeoutException
					throw  [Org.IdentityConnectors.Framework.Common.Exceptions.RetryableException]::Wrap("TIMEOUT",$cause)
				}
				"TEST5" 
				{
					$attrutil = [Org.IdentityConnectors.Framework.Common.Objects.ConnectorAttributeUtil]
					$secutil = [Org.IdentityConnectors.Common.Security.SecurityUtil]
					
					$mail = $attrutil::GetAsStringValue($attrutil::Find("mail",$Connector.Attributes))
					if ($mail -ne "TEST5@example.com") {throw "Wrong email"}
					
					$gstring = $attrutil::GetPasswordValue($Connector.Attributes)
					$password = $secutil::Decrypt($gstring)
					echo "Gstring: " $gstring > "C:\PSScripts\dump.txt"
					echo "Password: " $password >> "C:\PSScripts\dump.txt"
					if ( $password -ne "Passw0rd") {throw "Wrong Password"}
					
					$Connector.Result.Uid = "TEST5"
				}
				"TEST6"
				{
					$Connector.Result.Uid = New-Object Org.IdentityConnectors.Framework.Common.Objects.Uid("TEST6")
				}
				default {throw New-Object Org.IdentityConnectors.Framework.Common.Exceptions.UnknownUidException}
			}
		}
		default {throw "Unsupported operation"}
	}
}
}
catch #Rethrow the original exception
{
	throw
}