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
    This is a sample Schema script        
.DESCRIPTION

.INPUT VARIABLES
	The connector sends us the following:
	- <prefix>.Configuration : handler to the connector's configuration object
	- <prefix>.Action: String correponding to the action ("SCHEMA" here)
	- <prefix>.SchemaBuilder: an instance of org.identityconnectors.framework.common.objects.SchemaBuilder 
	that must be used to define the schema.
	
.RETURNS 
	Nothing. Connector will finalize the schema build.
	
.NOTES  
    File Name      : Schema.ps1  
    Author         : Gael Allioux (gael.allioux@forgerock.com)
    Prerequisite   : PowerShell V2
    Copyright 2014 - ForgeRock AS    
.LINK  
    Script posted over:  
    http://openicf.forgerock.org

.EXAMPLE  
#>

# Always put code in try/catch statement and make sure exceptions are rethrown to connector
try
{

$AttributeInfoBuilder = [Org.IdentityConnectors.Framework.Common.Objects.ConnectorAttributeInfoBuilder]

 if ($Connector.Action -eq "SCHEMA")
 {
 	###########################
 	# __ACCOUNT__ object class
	###########################
	$ocib = New-Object Org.IdentityConnectors.Framework.Common.Objects.ObjectClassInfoBuilder
	$ocib.ObjectType = "__ACCOUNT__"

	$uid = New-Object Org.IdentityConnectors.Framework.Common.Objects.ConnectorAttributeInfoBuilder("uid");
	$uid.Required = $TRUE
	$ocib.AddAttributeInfo($uid.Build())
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("firstname"))
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("lastname"))
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("email"))
	
	$Connector.SchemaBuilder.DefineObjectClass($ocib.Build())
	
	###########################
 	# __GROUP__ object class
	###########################
	$ocib = New-Object Org.IdentityConnectors.Framework.Common.Objects.ObjectClassInfoBuilder
	$ocib.ObjectType = "__GROUP__"

	$cn = New-Object Org.IdentityConnectors.Framework.Common.Objects.ConnectorAttributeInfoBuilder("cn");
	$cn.Required = $TRUE
	$ocib.AddAttributeInfo($cn.Build())
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("description"))
	$members = New-Object Org.IdentityConnectors.Framework.Common.Objects.ConnectorAttributeInfoBuilder("member");
	$members.Required = $TRUE
	$members.MultiValued = $TRUE
	
	$Connector.SchemaBuilder.DefineObjectClass($ocib.Build())
	
	###########################
 	# __TEST__ object class
	###########################
	$ocib = New-Object Org.IdentityConnectors.Framework.Common.Objects.ObjectClassInfoBuilder
	$ocib.ObjectType = "__TEST__"
	
	# All supported attribute types				
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeString",[string]))
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeLong",[long]))
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeChar",[char]))
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeFloat",[float]))
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeInt",[int]))
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeBool",[bool]))
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeByte",[byte]))
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeByteArray",[byte[]]))
	
	# We need to instantiate an object to be able to get the type
	# [type]::gettype does not work here
	
	$foo = New-Object Org.IdentityConnectors.Common.Security.GuardedByteArray 
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeGuardedByteArray",$foo.GetType()))
	
	$foo = New-Object Org.IdentityConnectors.Common.Security.GuardedString
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeGuardedString",$foo.GetType()))
	
	$foo = New-Object Org.IdentityConnectors.Framework.Common.Objects.BigInteger("1")
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeBigInteger",$foo.GetType()))
	
	$foo2 = New-Object Org.IdentityConnectors.Framework.Common.Objects.BigDecimal($foo,1)
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeBigDecimal",$foo2.GetType()))
	
	$foo = New-Object 'System.Collections.Generic.Dictionary[object, object]'
	$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeDictionary",$foo.GetType()))
	
	# Hashtable is not supported. 
	#$ocib.AddAttributeInfo($AttributeInfoBuilder::Build("attributeHashTable",[hashtable]))
	
	$Connector.SchemaBuilder.DefineObjectClass($ocib.Build())
 }
 }
 catch #Rethrow the original exception
 {
 	throw
 }