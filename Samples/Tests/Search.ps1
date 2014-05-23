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
    This is a sample Search script        
.DESCRIPTION
	The connector injects the following variables to the script:
	 - <prefix>.Configuration : handler to the connector's configuration object
	 - <prefix>.ObjectClass: a String describing the Object class (__ACCOUNT__ / __GROUP__ / other)
	 - <prefix>.Action: a string describing the action ("SEARCH" here)
	 - <prefix>.Options: a handler to the OperationOptions Map
	 - <prefix>.Query: a handler to the Query Map

	The default Query Dictionary describes the filter used.

	Query = {Operation: "CONTAINS", Left: "attributeName", Right: "value", Not: true/false ]
	Query = {Operation: "ENDSWITH", Left: "attributeName", Right: "value", Not: true/false ]
	Query = {Operation: "STARTSWITH", Left: "attributeName", Right: "value", Not: true/false ]
	Query = {Operation: "EQUALS", Left: "attributeName", Right: "value", Not: true/false ]
	Query = {Operation: "GREATERTHAN", Left: "attributeName", Right: "value", Not: true/false ]
	Query = {Operation: "GREATERTHANOREQUAL", Left: "attributeName", Right: "value", Not: true/false ]
	Query = {Operation: "LESSTHAN", Left: "attributeName", Right: "value", Not: true/false ]
	Query = {Operation: "LESSTHANOREQUAL", Left: "attributeName", Right: "value", Not: true/false ]
	Query = null : then we assume we fetch everything

	AND and OR filter just embed a left/right couple of queries.
	query = [ operation: "AND", left: query1, right: query2 ]
	query = [ operation: "OR", left: query1, right: query2 ]

.RETURNS
	A list of Maps. Each map describing one row.
	!!!! Each Map must contain a '__UID__' and '__NAME__' attribute.
	This is required to build a ConnectorObject.
  
.NOTES  
    File Name      : Search.ps1  
    Author         : Gael Allioux (gael.allioux@forgerock.com)
    Prerequisite   : PowerShell V2
    Copyright 2014 - ForgeRock AS    
.LINK  
    Script posted over:  
    http://openicf.forgerock.org
.EXAMPLE  
    Example 1     
.EXAMPLE    
    Example 2
#>

# Always put code in try/catch statement and make sure exceptions are rethrown to connector
try
{
if ( $Connector.Query )
{
	$fullQuery = $Connector.Query.Left + " " + $Connector.Query.Operation +" "+ $Connector.Query.Right
	
	if ( $Connector.Query.Not )
	{
		$fullQuery = "!(" + $fullQuery +")"
		echo "QUERY" $fullQuery > "C:\PSScripts\dump.txt"
	}
	
	switch ($Connector.ObjectClass)
	{
		"__ACCOUNT__"
		{
			switch ($fullQuery)
			{
				"__UID__ EQUALS 001"
				{
					$Connector.Result.Process(@{__UID__= "001"; __NAME__= "User 1"; sn= "Smith"; mail= "jsmith@example.com"})
					return
				} 
				"sn STARTSWITH smith"
				{
					$Connector.Result.Process(@{__UID__= "001"; __NAME__= "User 1"; sn= "Smith"})
					return
				}
				"!(mail ENDSWITH example.com)"
				{
					$Connector.Result.Process(@{__UID__= "001"; __NAME__= "User 1"; mail= "jsmith@foo.com"})
					return
				}
			}
		}
		"__GROUP__"
		{
			return
		}
		"__TEST__"
		{
			switch ($fullQuery)
			{
				"__UID__ EQUALS 001"
				{
					$Connector.Result.Process(@{__UID__= "001"; __NAME__= "User 1"; sn= "Smith"; mail= "jsmith@example.com"})
					return
				} 
				"sn STARTSWITH smith"
				{
					$Connector.Result.Process(@{__UID__= "001"; __NAME__= "User 1"; sn= "Smith"})
					return
				}
				"!(mail ENDSWITH example.com)"
				{
					$Connector.Result.Process(@{__UID__= "001"; __NAME__= "User 1"; mail= "jsmith@foo.com"})
					return
				}
			}
		}
		default
		{	
			return
		}
	}
}
else 
{ # Query is null, fetch all entries
	switch ($Connector.ObjectClass)
	{
		"__ACCOUNT__"
		{
 			$entries = (@{__UID__= "001"; __NAME__= "User 1"},@{__UID__= "002"; __NAME__= "User 2"})
			foreach($entry in $entries)
			{
				Write-Debug -Message "Processing $entry" 
				if (!$Connector.Result.Process($entry))
				{
					break;
				}
			}
		}
	}
}
}
catch #Rethrow the original exception
{
	throw
}