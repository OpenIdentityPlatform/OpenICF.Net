/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS HEADER.
 *
 * Copyright (c) 2014 ForgeRock AS. All Rights Reserved
 *
 * The contents of this file are subject to the terms
 * of the Common Development and Distribution License
 * (the License). You may not use this file except in
 * compliance with the License.
 *
 * You can obtain a copy of the License at
 * http://forgerock.org/license/CDDLv1.0.html
 * See the License for the specific language governing
 * permission and limitations under the License.
 *
 * When distributing Covered Code, include this CDDL
 * Header Notice in each file and include the License file
 * at http://forgerock.org/license/CDDLv1.0.html
 * If applicable, add the following below the CDDL Header,
 * with the fields enclosed by brackets [] replaced by
 * your own identifying information:
 * "Portions Copyrighted [year] [name of copyright owner]"
 */
using System;
using Org.IdentityConnectors.Framework.Spi;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Org.IdentityConnectors.Framework.Common.Exceptions;

namespace Org.ForgeRock.OpenICF.Connectors.MsPowerShell
{
    [ConfigurationClassAttribute(true, new[] { "MsPowerShellHost" })]
    public class MsPowerShellConfiguration : AbstractConfiguration, StatefulConfiguration
    {
        private Collection<String> _validScripts;

        public String AuthenticateScriptFileName
        { get; set; }

        public String CreateScriptFileName
        { get; set; }

        public String UpdateScriptFileName
        { get; set; }

        public String DeleteScriptFileName
        { get; set; }

        public String ResolveUsernameScriptFileName
        { get; set; }

        public String ScriptOnConnectorScriptFileName
        { get; set; }

        public String ScriptOnResourceScriptFileName
        { get; set; }

        public String SearchScriptFileName
        { get; set; }

        public String SyncScriptFileName
        { get; set; }

        public String SchemaScriptFileName
        { get; set; }

        public String TestScriptFileName
        { get; set; }

        public String VariablesPrefix
        { get; set;  }

        public String QueryFilterType 
        { get; set; }

        public Boolean SubstituteUidAndNameInQueryFilter
        { get; set; }

        public String UidAttributeName
        { get; set; }

        public String NameAttributeName
        { get; set; }

        public MsPowerShellConfiguration()
        {
            AuthenticateScriptFileName = "";
            CreateScriptFileName = "";
            UpdateScriptFileName = "";
            DeleteScriptFileName = "";
            ResolveUsernameScriptFileName = "";
            ScriptOnConnectorScriptFileName = "";
            ScriptOnResourceScriptFileName = "";
            SearchScriptFileName = "";
            SyncScriptFileName = "";
            SchemaScriptFileName = "";
            TestScriptFileName = "";
            VariablesPrefix = "Connector";
            QueryFilterType = "Map";
            SubstituteUidAndNameInQueryFilter = false;
            UidAttributeName = "__UID__";
            NameAttributeName = "__NAME__";
        }

        public override void Validate()
        {
            var scriptsList = new String[] 
            {
                AuthenticateScriptFileName,
                CreateScriptFileName, 
                UpdateScriptFileName, 
                DeleteScriptFileName,
                TestScriptFileName,
                ResolveUsernameScriptFileName, 
                ScriptOnConnectorScriptFileName,
                ScriptOnResourceScriptFileName, 
                SearchScriptFileName, 
                SyncScriptFileName,
                SchemaScriptFileName
            };

            _validScripts = new Collection<string>();

            Trace.TraceInformation("Entering Validate() configuration"); 
            foreach (var file in scriptsList)
            {
                if ((file != null) && (!file.Equals("")))
                {
                    System.IO.FileStream fs = null;
                    try
                    {
                        fs = System.IO.File.OpenRead(file);
                        _validScripts.Add(file);
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationException(ex);
                    }
                    finally
                    {
                        if (fs != null) fs.Dispose();
                    }
                }
            }

            if ((VariablesPrefix == null) || ("".Equals(VariablesPrefix)))
            {
                throw new ConfigurationException("VariablesPrefix can not be empty or null");
            }

            if (!("Map".Equals(QueryFilterType) || "Ldap".Equals(QueryFilterType) || "Native".Equals(QueryFilterType) ||
                "AdPsModule".Equals(QueryFilterType)))
            {
                throw new ConfigurationException("QueryFilterType must be Native|Map|Ldap|AdPsModule");
            }
        }

        public void Release()
        {
            
        }

        public Collection<String> GetValidScripts()
        {
            return _validScripts;
        }

        private MsPowerShellHost _host = null;

        private MsPowerShellHost MsPowerShellHost
        {
            get
            {
                if (null == _host)
                {
                    lock (this)
                    {
                        if (null == _host)
                        {
                           _host = new MsPowerShellHost();
                        }
                    }
                }
                return _host;
            }
        }

    }
}
