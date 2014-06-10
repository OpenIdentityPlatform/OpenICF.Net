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
	- <prefix>.Operation: an OperationType corresponding to the action ("CREATE" here)
	- <prefix>.ObjectClass: an ObjectClass describing the Object class (__ACCOUNT__ / __GROUP__ / other)
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
if ($Connector.Operation -eq "CREATE")
{
	switch ($Connector.ObjectClass.Type)
	{
		"__ACCOUNT__" 	
		{
			$cobld = New-Object Org.IdentityConnectors.Framework.Common.Objects.ConnectorObjectBuilder
			$cobld.setUid($Connector.Id)
			$cobld.setName($Connector.Id)
			$cobld.ObjectClass = $Connector.ObjectClass
			$cobld.AddAttributes($Connector.Attributes)
			
			$Connector.Result.Uid = New-ConnectorObjectCache $cobld.Build()
		}
		"__GROUP__" 	{}
		"__ALL__" 		{throw "ICF Framework must reject this"}
		"__TEST__" 		
		{
			$uid = New-Object Org.IdentityConnectors.Framework.Common.Objects.Uid($Connector.Id, "0")
			$Connector.Result.Uid = Exception-Test -Operation $Connector.Operation -ObjectClass $Connector.ObjectClass -Uid $uid -Options $Connector.Options
		}
		"__SAMPLE__" 	{throw New-Object System.NotSupportedException("$($Connector.Operation) operation of type:$($Connector.objectClass.Type)")}
		default 		{throw New-Object System.NotSupportedException("$($Connector.Operation) operation of type:$($Connector.objectClass.Type)")}			
	} 
}
}
catch #Rethrow the original exception
{
	throw
}