/*
 * Copyright 2008 Sun Microsystems, Inc. All rights reserved.
 * 
 * U.S. Government Rights - Commercial software. Government users 
 * are subject to the Sun Microsystems, Inc. standard license agreement
 * and applicable provisions of the FAR and its supplements.
 * 
 * Use is subject to license terms.
 * 
 * This distribution may include materials developed by third parties.
 * Sun, Sun Microsystems, the Sun logo, Java and Project Identity 
 * Connectors are trademarks or registered trademarks of Sun 
 * Microsystems, Inc. or its subsidiaries in the U.S. and other
 * countries.
 * 
 * UNIX is a registered trademark in the U.S. and other countries,
 * exclusively licensed through X/Open Company, Ltd. 
 * 
 * -----------
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS HEADER.
 * 
 * Copyright 2008 Sun Microsystems, Inc. All rights reserved. 
 * 
 * The contents of this file are subject to the terms of the Common Development
 * and Distribution License(CDDL) (the License).  You may not use this file
 * except in  compliance with the License. 
 * 
 * You can obtain a copy of the License at
 * http://identityconnectors.dev.java.net/CDDLv1.0.html
 * See the License for the specific language governing permissions and 
 * limitations under the License.  
 * 
 * When distributing the Covered Code, include this CDDL Header Notice in each
 * file and include the License file at identityconnectors/legal/license.txt.
 * If applicable, add the following below this CDDL Header, with the fields 
 * enclosed by brackets [] replaced by your own identifying information: 
 * "Portions Copyrighted [year] [name of copyright owner]"
 * -----------
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Org.IdentityConnectors.Framework.Spi;
using Org.IdentityConnectors.Framework.Common.Exceptions;

namespace Org.IdentityConnectors.ActiveDirectory
{
    public class ActiveDirectoryConfiguration : Org.IdentityConnectors.Framework.Spi.AbstractConfiguration
    {
        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display_SyncGlobalCatalogServer", HelpMessageKey = "help_SyncGlobalCatalogServer")]
        public String SyncGlobalCatalogServer
        { get; set; }
            
        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display_SyncDomainController", HelpMessageKey = "help_SyncDomainController")]
        public String SyncDomainController
        { get; set; }

        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display_SyncSearchContext", HelpMessageKey = "help_SyncSearchContext")]
        public String SyncSearchContext
        { get; set; }

        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display_domainName", HelpMessageKey = "help_domainName")]
        public String DomainName
        { get; set; }

        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display.DirectoryAdminName", HelpMessageKey = "help.DirectoryAdminName")]
        public String DirectoryAdminName
        { get; set; }

        [ConfigurationProperty(Confidential = true, DisplayMessageKey = "display.DirectoryAdminPassword", HelpMessageKey = "help.DirectoryAdminPassword")]
        public String DirectoryAdminPassword
        { get; set; }

        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display.ObjectClass", HelpMessageKey = "help.ObjectClass")]
        public String ObjectClass
        { get; set; }

        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display.UserProvidesPasswordOnChange", HelpMessageKey = "help.UserProvidesPasswordOnChange")]
        public bool UserProvidesPasswordOnChange{get;set;}

        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display.CreateHomeDirectory", HelpMessageKey = "help.CreateHomeDirectory")]
        public bool CreateHomeDirectory { get; set; }

        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display.SearchContext", HelpMessageKey = "help.SearchContext")]
        public String SearchContext
        { get; set; }

        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display.SearchChildDomains", HelpMessageKey = "help.SearchChildDomains")]
        public bool SearchChildDomains {get;set;}

        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display.EncryptionType", HelpMessageKey = "help.EncryptionType")]
        public String EncryptionType
        { get; set; }

        [ConfigurationProperty(Confidential = false, DisplayMessageKey = "display.LDAPHostName", HelpMessageKey = "help.LDAPHostName")]
        public String LDAPHostName
        { get; set; }

        public ActiveDirectoryConfiguration()
        {
            DomainName = "";
            SearchContext = "";
            DirectoryAdminName = "cn=DirectoryAdmin";
            ObjectClass = "User";
            UserProvidesPasswordOnChange = false;
            CreateHomeDirectory = false;
            SearchChildDomains = false;
            EncryptionType = "NONE";
            LDAPHostName = "";
        }

        public override void Validate()
        {
            String message = "Configuration errors:  ";
            Boolean foundError = false;

            // can't lookup the schema without the domain name
            if (DomainName.Length == 0)
            {
                message += "->Domain name not supplied  ";
                foundError = true;
            }

            if (DirectoryAdminName.Length == 0)
            {
                message += "->Directory administrator name not supplied  ";
                foundError = true;
            }

            if (ObjectClass.Length == 0)
            {
                message += "->ObjectClass was not supplied  ";
                foundError = true;
            }

            if (foundError)
            {
                throw new ConfigurationException(message);
            }
        }
    }
}