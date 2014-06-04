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
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;

namespace Org.ForgeRock.OpenICF.Connectors.MsPowerShell
{
    class MsPowerShellHost : IDisposable
    {
        private readonly PowerShell _ps;
        private readonly Runspace _space;
        private InitialSessionState _initial;

        public MsPowerShellHost()
        {
            _space = RunspaceFactory.CreateRunspace();
            _space.Open();
            _ps = PowerShell.Create();
            _ps.Runspace = _space;
        }

        public MsPowerShellHost(string[] modules)
        {
            // check if re-loading modules is more efficient
            //_initial.ImportPSModule(new string[] { "C:\\OpenICF\\fr-branches\\openicf-powershell-connector-1.4.0.x\\Samples\\Tests\\TestModule.psm1" });
            _initial = InitialSessionState.CreateDefault();
            _initial.ImportPSModule(modules);
            _space = RunspaceFactory.CreateRunspace(_initial);
            _space.Open();
            _ps = PowerShell.Create();
            _ps.Runspace = _space;
        }

        public void ValidateScript(String script)
        {
            Collection<PSParseError> errors;
            PSParser.Tokenize(script, out errors);
            if (errors.Count != 0)
            {
                throw new ParseException(errors[0].Message);
            }
        }


        public Collection<Object> ExecuteScript(String script, IDictionary<String, Object> arguments)
        {
            var result = new Collection<object>();
            foreach (var entry in arguments)
            {
                if (entry.Value != null)
                {
                    _space.SessionStateProxy.SetVariable(entry.Key, new PSObject(entry.Value));
                }
            }
            //TODO: check this
            _ps.Commands.Clear(); 
            //ps.AddScript(loadScript(fileName),true);
            _ps.AddScript(script);
            _ps.Streams.Debug.DataAdded += new EventHandler<DataAddedEventArgs>(Debug_DataAdded);
            try
            {
                var psResult = _ps.Invoke();
                //TODO: test this!!!
                foreach (var debug in _ps.Streams.Debug.ReadAll())
                {
                    Trace.TraceInformation(debug.Message);
                }
                foreach (var res in psResult)
                {
                    result.Add(res.BaseObject);
                }
                return result;
            }
            finally
            {
                _ps.Stop();
            }
        }

        void Debug_DataAdded(object sender, DataAddedEventArgs e)
        {
            Trace.TraceInformation("eeee");
        }

        public void Dispose() 
        {
            if (_ps != null) _ps.Dispose();
            if (_space != null)
            {
                _space.Close();
                _space.Dispose();
            }
        }
    }
}
