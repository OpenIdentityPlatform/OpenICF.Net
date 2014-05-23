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
using System.Collections;
using System.Collections.ObjectModel;
using Org.IdentityConnectors.Framework.Common;
using Org.IdentityConnectors.Framework.Common.Exceptions;
using Org.IdentityConnectors.Framework.Common.Objects;
using Org.IdentityConnectors.Framework.Spi;

namespace Org.ForgeRock.OpenICF.Connectors.MsPowerShell
{
    class MsPowerShellSearchResults
    {
        private readonly ObjectClass _objectClass;
        private readonly ResultsHandler _handler;

        public MsPowerShellSearchResults(ObjectClass objectClass, ResultsHandler handler)
        {
            _objectClass = objectClass;
            _handler = handler;
        }

        public void Complete(String searchResult)
        {
                ((SearchResultsHandler)_handler).HandleResult(new SearchResult((string)searchResult, -1));
        }

        public void Complete(SearchResult searchResult)
        {
                ((SearchResultsHandler)_handler).HandleResult((SearchResult)searchResult);
        }

        public Boolean Process(ConnectorObject result)
        {
            return _handler.Handle(result);
        }

        public Boolean Process(Hashtable result)
        {
            var cobld = new ConnectorObjectBuilder();
            foreach (String key in result.Keys)
            {
                var attrName = key;
                var attrValue = result[key];
                if ("__UID__".Equals(attrName))
                {
                    if (attrValue == null)
                    {
                        throw new ConnectorException("Uid can not be null");
                    }
                    cobld.SetUid(attrValue.ToString());
                }
                else if ("__NAME__".Equals(attrName))
                {
                    if (attrValue == null)
                    {
                        throw new ConnectorException("Name can not be null");
                    }
                    cobld.SetName(attrValue.ToString());
                }
                else
                {
                    if (attrValue == null)
                    {
                        cobld.AddAttribute(ConnectorAttributeBuilder.Build(attrName));
                    }
                    else if (attrValue.GetType() == typeof(Object[]))
                    {
                        var list = new Collection<object>();
                        foreach (var val in (ICollection)attrValue)
                        {
                            list.Add(FrameworkUtil.IsSupportedAttributeType(val.GetType()) ? val : val.ToString());
                        }
                        cobld.AddAttribute(ConnectorAttributeBuilder.Build(attrName, list));
                    }
                    else
                    {
                        cobld.AddAttribute(FrameworkUtil.IsSupportedAttributeType(attrValue.GetType())
                            ? ConnectorAttributeBuilder.Build(attrName, attrValue)
                            : ConnectorAttributeBuilder.Build(attrName, attrValue.ToString()));
                    }
                }
            }
            cobld.ObjectClass = _objectClass;
            return _handler.Handle(cobld.Build());
        }
    }
}
