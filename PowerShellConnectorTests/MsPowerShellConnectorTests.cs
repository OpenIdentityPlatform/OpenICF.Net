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
using System.Management.Automation;
using System.Security;
using NUnit.Framework;
using Org.ForgeRock.OpenICF.Connectors.MsPowerShell;
using Org.IdentityConnectors.Common;
using Org.IdentityConnectors.Common.Security;
using Org.IdentityConnectors.Framework.Api;
using Org.IdentityConnectors.Framework.Common.Exceptions;
using Org.IdentityConnectors.Framework.Common.Objects;
using Org.IdentityConnectors.Framework.Common.Objects.Filters;
using Org.IdentityConnectors.Framework.Common.Serializer;
using Org.IdentityConnectors.Framework.Impl.Api.Local;
using Org.IdentityConnectors.Framework.Spi;
using Org.IdentityConnectors.Test.Common;

namespace MSPowerShellConnectorTests
{
    [TestFixture]
    public class MsPowerShellConnectorTests
    {
        private ConnectorFacade _facade = null;
        private static readonly ObjectClass Test = new ObjectClass("__TEST__");
        private const String Password = "Passw0rd";

        [TestFixtureSetUp]
        public void Init()
        {
            GetFacade();
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            if (null != _facade)
            {
                ((LocalConnectorFacadeImpl) _facade).Dispose();
                _facade = null;
            }
        }

        // =====================================================
        // Test Operation Test
        // =====================================================

        [Test]
        [Category("Test")]
        public void TestTest()
        {
            GetFacade().Test();
        }

        // =====================================================
        // Authenticate Operation Test
        // =====================================================

        [Test]
        [Category("Authenticate")]
        [ExpectedException(typeof(ConnectorSecurityException))]
        public void TestAuthenticate1()
        {
            GetFacade().Authenticate(Test, "TEST1", new GuardedString(GetSecure(Password)), null);
        }

        [Test]
        [Category("Authenticate")]
        [ExpectedException(typeof(InvalidCredentialException))]
        public void TestAuthenticate2()
        {
            GetFacade().Authenticate(Test, "TEST2", new GuardedString(GetSecure(Password)), null);
        }

        [Test]
        [Category("Authenticate")]
        [ExpectedException(typeof(InvalidPasswordException))]
        public void TestAuthenticate3()
        {
            GetFacade().Authenticate(Test, "TEST3", new GuardedString(GetSecure(Password)), null);
        }

        [Test]
        [Category("Authenticate")]
        [ExpectedException(typeof(PermissionDeniedException))]
        public void TestAuthenticate4()
        {
            GetFacade().Authenticate(Test, "TEST4", new GuardedString(GetSecure(Password)), null);
        }

        [Test]
        [Category("Authenticate")]
        [ExpectedException(typeof(PasswordExpiredException))]
        public void TestAuthenticate5()
        {
            GetFacade().Authenticate(Test, "TEST5", new GuardedString(GetSecure(Password)), null);
        }

        [Test]
        [Category("Authenticate")]
        [ExpectedException(typeof(UnknownUidException))]
        public void TestAuthenticate7()
        {
            GetFacade().Authenticate(Test, "TEST7", new GuardedString(GetSecure(Password)), null);
        }

        [Test]
        [Category("Authenticate")]
        public void TestAuthenticateOk()
        {
            Assert.AreEqual("TESTOK1", GetFacade().Authenticate(Test, "TESTOK1", new GuardedString(GetSecure(Password)), null).GetUidValue());
        }

        [Test]
        [Category("Authenticate")]
        public void TestAuthenticateEmpty()
        {
            Assert.AreEqual("TESTOK2", GetFacade().Authenticate(Test, "TESTOK2", new GuardedString(GetSecure("")), null).GetUidValue());
        }


        // =====================================================
        // Create Operation Test
        // =====================================================

        [Test]
        [Category("Create")]
        public void TestCreate()
        {
            Uid uid = GetFacade().Create(ObjectClass.ACCOUNT, GetTestCreateConnectorObject("Foo"), null);
            Assert.NotNull(uid, "The Uid is null");
        }

        [Test]
        [Category("Create")]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void TestCreate1()
        {
            GetFacade().Create(Test, GetTestCreateConnectorObject("TEST1"), null);
        }

        [Test]
        [Category("Create")]
        [ExpectedException(typeof(InvalidAttributeValueException))]
        public void TestCreate2()
        {
            GetFacade().Create(Test, GetTestCreateConnectorObject("TEST2"), null);
        }

        [Test]
        [Category("Create")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreate3()
        {
            GetFacade().Create(Test, GetTestCreateConnectorObject("TEST3"), null);
        }

        [Test]
        [Category("Create")]
        [ExpectedException(typeof(RetryableException))]
        public void TestCreate4()
        {
            GetFacade().Create(Test, GetTestCreateConnectorObject("TEST4"), null);
        }

        [Test]
        [Category("Create")]
        public void TestCreate5()
        {
            Assert.AreEqual("TEST5", GetFacade().Create(Test, GetTestCreateConnectorObject("TEST5"), null).GetUidValue());
        }

        [Test]
        [Category("Create")]
        public void TestCreate6()
        {
            Assert.AreEqual(new Uid("TEST6", "1"), GetFacade().Create(Test, GetTestCreateConnectorObject("TEST6"), null));
        }

        [Test]
        [Category("Create")]
        [ExpectedException(typeof(UnknownUidException))]
        public void TestCreate7()
        {
            GetFacade().Create(Test, GetTestCreateConnectorObject("TEST7"), null);
        }

        // =====================================================
        // Update Operation Test
        // =====================================================

        [Test]
        [Category("Update")]
        public void TestUpdate()
        {
            Uid uid = GetFacade().Update(ObjectClass.ACCOUNT, new Uid("Foo"), GetTestCreateConnectorObject("Foo"), null);
            Assert.NotNull(uid, "The Uid is null");
        }

        [Test]
        [Category("Update")]
        [ExpectedException(typeof(ConnectorException))]
        public void TestUpdate1()
        {
            GetFacade().Update(Test, new Uid("TEST1"), GetTestUpdateConnectorObject("TEST1"), null);
        }

        [Test]
        [Category("Update")]
        [ExpectedException(typeof(InvalidAttributeValueException))]
        public void TestUpdate2()
        {
            GetFacade().Update(Test, new Uid("TEST2"), GetTestUpdateConnectorObject("TEST2"), null);
        }

        [Test]
        [Category("Update")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUpdate3()
        {
            GetFacade().Update(Test, new Uid("TEST3"), GetTestUpdateConnectorObject("TEST3"), null);
        }

        [Test]
        [Category("Update")]
        [ExpectedException(typeof(RetryableException))]
        public void TestUpdate4()
        {
            GetFacade().Update(Test, new Uid("TEST4"), GetTestUpdateConnectorObject("TEST4"), null);
        }

        [Test]
        [Category("Update")]
        public void TestUpdate5()
        {
            Assert.AreEqual("TEST5", GetFacade().Update(Test, new Uid("TEST5"), GetTestUpdateConnectorObject("TEST5"), null).GetUidValue());
        }

        [Test]
        [Category("Update")]
        public void TestUpdate6()
        {
            Assert.AreEqual("TEST6", GetFacade().Update(Test, new Uid("TEST6"), GetTestUpdateConnectorObject("TEST6"), null).GetUidValue());
        }

        [Test]
        [Category("Update")]
        [ExpectedException(typeof(RuntimeException))]
        public void TestUpdate7()
        {
            GetFacade().Update(ObjectClass.GROUP, new Uid("Group1"), GetTestUpdateConnectorObject("Group1"), null);
        }

        // =====================================================
        // Delete Operation Test
        // =====================================================

        [Test]
        [Category("Delete")]
        public void TestDelete1()
        {
            // Will succeed
            GetFacade().Delete(ObjectClass.ACCOUNT, new Uid("smith"), null);
        }


        [Test]
        [Category("Delete")]
        [ExpectedException(typeof(UnknownUidException))]
        public void TestDelete2()
        {
            // Will fail
            GetFacade().Delete(ObjectClass.ACCOUNT, new Uid("doe"), null);
        }

        // =====================================================
        // ResolveUsername Operation Test
        // =====================================================

        [Test]
        [Category("ResolveUsername")]
        public void TestResolveUsername1()
        {
            Assert.AreEqual("123", GetFacade().ResolveUsername(ObjectClass.ACCOUNT, "TESTOK1", null).GetUidValue());
        }

        [Test]
        [Category("ResolveUsername")]
        [ExpectedException(typeof(UnknownUidException))]
        public void TestResolveUsername2()
        {
            GetFacade().ResolveUsername(ObjectClass.ACCOUNT, "NON_EXIST", null);
        }

        [Test]
        [Category("ResolveUsername")]
        [ExpectedException(typeof(RuntimeException))]
        public void TestResolveUsername3()
        {
            GetFacade().ResolveUsername(Test, "NON_EXIST", null);
        }


        // =====================================================
        // Schema Operation Test
        // =====================================================

        [Test]
        [Category("Schema")]
        public void TestSchema1()
        {
            Schema schema = GetFacade().Schema();
            Assert.NotNull(schema.FindObjectClassInfo("__TEST__"));
            Assert.NotNull(schema.FindObjectClassInfo("__ACCOUNT__"));
            Assert.NotNull(schema.FindObjectClassInfo("__GROUP__"));
            Console.WriteLine(SerializerUtil.SerializeXmlObject(schema, true));
        }


        // =====================================================
        // Search Operation Test
        // =====================================================

        [Test]
        [Category("Search")]
        public void TestGetObject()
        {
            var co = GetFacade().GetObject(Test, new Uid("001"), null);
            Assert.IsNotNull(co);
        }

        /*FilterBuilder.EqualTo(ConnectorAttributeBuilder.Build(Name.NAME, "Foo"))*/
        [Test]
        [Category("Search")]
        public void TestNullQuery()
        {
            var result = new List<ConnectorObject>();
            GetFacade().Search(ObjectClass.ACCOUNT, null, new ResultsHandler()
            {
                Handle = connectorObject => { result.Add(connectorObject); return true; }
            }, null);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        [Category("Search")]
        public void TestEqualsQuery()
        {
            var result = new List<ConnectorObject>();
            GetFacade().Search(ObjectClass.ACCOUNT, FilterBuilder.EqualTo(ConnectorAttributeBuilder.Build(Uid.NAME, "001")), new ResultsHandler()
            {
                Handle = connectorObject => { result.Add(connectorObject); return true; }
            }, null);

            Assert.AreEqual(1, result.Count);
            var co = result[0];
            Assert.IsTrue("User 1".Equals(co.Name.GetNameValue()));
        }

        [Test]
        [Category("Search")]
        public void TestStartsWithQuery()
        {
            var result = new List<ConnectorObject>();
            GetFacade().Search(ObjectClass.ACCOUNT, FilterBuilder.StartsWith(ConnectorAttributeBuilder.Build("sn", "SMITH")), new ResultsHandler()
            {
                Handle = connectorObject => { result.Add(connectorObject); return true; }
            }, null);

            Assert.AreEqual(1, result.Count);
            var co = result[0];
            Assert.IsTrue("Smith".Equals(ConnectorAttributeUtil.GetAsStringValue(co.GetAttributeByName("sn"))));
        }

        // =====================================================
        // Sync Operation Test
        // =====================================================

        [Test]
        [Category("Sync")]
        public void TestSyncToken()
        {
            Assert.AreEqual(17, GetFacade().GetLatestSyncToken(ObjectClass.ACCOUNT).Value);
            Assert.AreEqual(16, GetFacade().GetLatestSyncToken(ObjectClass.GROUP).Value);
            Assert.AreEqual(17, GetFacade().GetLatestSyncToken(ObjectClass.ALL).Value);
            Assert.IsInstanceOf(typeof(string), GetFacade().GetLatestSyncToken(Test).Value);
        }

        [Test]
        [Category("Sync")]
        public void TestSyncAccount()
        {
            var result = new List<SyncDelta>();

            SyncToken lastToken = GetFacade().Sync(ObjectClass.ACCOUNT, new SyncToken(0), new SyncResultsHandler()
                {
                    Handle = delta =>
                    {
                        result.Add(delta);
                        return true;
                    }
                }, null);
            Assert.AreEqual(1, lastToken.Value);
            Assert.AreEqual(1, result.Count);
            SyncDelta sdelta = result[0];
            result.RemoveAt(0);
            Assert.AreEqual(SyncDeltaType.CREATE, sdelta.DeltaType);
            Assert.AreEqual(4, sdelta.Object.GetAttributes().Count);

            lastToken = GetFacade().Sync(ObjectClass.ACCOUNT, lastToken, new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
            Assert.AreEqual(2, lastToken.Value);
            Assert.AreEqual(1, result.Count);
            sdelta = result[0];
            result.RemoveAt(0);
            Assert.AreEqual(SyncDeltaType.UPDATE, sdelta.DeltaType);
            Assert.AreEqual(4, sdelta.Object.GetAttributes().Count);

            lastToken = GetFacade().Sync(ObjectClass.ACCOUNT, lastToken, new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
            Assert.AreEqual(3, lastToken.Value);
            Assert.AreEqual(1, result.Count);
            sdelta = result[0];
            result.RemoveAt(0);
            Assert.AreEqual(SyncDeltaType.CREATE_OR_UPDATE, sdelta.DeltaType);
            Assert.AreEqual(4, sdelta.Object.GetAttributes().Count);

            lastToken = GetFacade().Sync(ObjectClass.ACCOUNT, lastToken, new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
            Assert.AreEqual(4, lastToken.Value);
            Assert.AreEqual(1, result.Count);
            sdelta = result[0];
            result.RemoveAt(0);
            Assert.AreEqual(SyncDeltaType.UPDATE, sdelta.DeltaType);
            Assert.AreEqual(4, sdelta.Object.GetAttributes().Count);
            Assert.AreEqual("001", sdelta.PreviousUid.GetUidValue());

            lastToken = GetFacade().Sync(ObjectClass.ACCOUNT, lastToken, new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
            Assert.AreEqual(5, lastToken.Value);
            Assert.AreEqual(1, result.Count);
            sdelta = result[0];
            result.RemoveAt(0);
            Assert.AreEqual(SyncDeltaType.DELETE, sdelta.DeltaType);

            lastToken = GetFacade().Sync(ObjectClass.ACCOUNT, lastToken, new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
            Assert.AreEqual(10, lastToken.Value);
            Assert.IsEmpty(result);

            lastToken = GetFacade().Sync(ObjectClass.ACCOUNT, lastToken, new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
            Assert.AreEqual(13, lastToken.Value);
            Assert.AreEqual(3, result.Count);

        }

        [Test]
        [Category("Sync")]
        public void TestSyncGroup()
        {
            var result = new List<SyncDelta>();

            GetFacade().Sync(ObjectClass.GROUP, new SyncToken(0), new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
            Assert.AreEqual(1, result.Count);
            SyncDelta sdelta = result[0];
            result.RemoveAt(0);
            Assert.AreEqual(SyncDeltaType.CREATE, sdelta.DeltaType);
            Assert.AreEqual(4, sdelta.Object.GetAttributes().Count);
        }

        [Test]
        [Category("Sync")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSyncTest0()
        {
            var result = new List<SyncDelta>();

            GetFacade().Sync(Test, new SyncToken(0), new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
        }

        [Test]
        [Category("Sync")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSyncTest1()
        {
            var result = new List<SyncDelta>();

            GetFacade().Sync(Test, new SyncToken(1), new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
        }

        [Test]
        [Category("Sync")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSyncTest2()
        {
            var result = new List<SyncDelta>();

            GetFacade().Sync(Test, new SyncToken(2), new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
        }

        [Test]
        [Category("Sync")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSyncTest3()
        {
            var result = new List<SyncDelta>();

            GetFacade().Sync(Test, new SyncToken(3), new SyncResultsHandler()
            {
                Handle = delta =>
                {
                    result.Add(delta);
                    return true;
                }
            }, null);
        }

        protected ConnectorFacade GetFacade()
        {
            var f = CreateConnectorFacade(SafeType<Connector>.ForRawType(typeof(MsPowerShellConnector)));
            Assert.NotNull(f, "The Facade Creation fails");
            return f;
        }

        public ConnectorFacade CreateConnectorFacade(SafeType<Connector> clazz)
        {
            if (null == _facade)
            {
                PropertyBag propertyBag = TestHelpers.GetProperties(clazz.RawType);

                APIConfiguration impl = TestHelpers.CreateTestConfiguration(clazz, propertyBag, "configuration");
                impl.ProducerBufferSize = 0;
                impl.ResultsHandlerConfiguration.EnableAttributesToGetSearchResultsHandler = false;
                impl.ResultsHandlerConfiguration.EnableCaseInsensitiveFilter = false;
                impl.ResultsHandlerConfiguration.EnableFilteredResultsHandler = false;
                impl.ResultsHandlerConfiguration.EnableNormalizingResultsHandler = false;
                _facade = ConnectorFacadeFactory.GetInstance().NewInstance(impl);
            }
            return _facade;
        }

        private List<ConnectorAttribute> GetTestCreateConnectorObject(String name)
        {
            var attrs = new List<ConnectorAttribute>
            {
                ConnectorAttributeBuilder.Build("sn", name.ToUpper()),
                ConnectorAttributeBuilder.Build("mail", name + "@example.com"),
                ConnectorAttributeBuilder.Build(Name.NAME, name),
                ConnectorAttributeBuilder.BuildPassword(new GuardedString(GetSecure(Password))),
                ConnectorAttributeBuilder.BuildEnabled(true)
            };
            return attrs;
        }

        private List<ConnectorAttribute> GetTestUpdateConnectorObject(String name)
        {
            var attrs = new List<ConnectorAttribute>
            {
                ConnectorAttributeBuilder.Build("mail", name + "@example2.com"),
                ConnectorAttributeBuilder.BuildPassword(new GuardedString(GetSecure(Password+"2"))),
                ConnectorAttributeBuilder.BuildEnabled(false)
            };
            return attrs;
        }
        private SecureString GetSecure(String password)
        {
            var secure = new SecureString();
            foreach (char c in password)
            {
                secure.AppendChar(c);
            }
            return secure;
        }
    }
}
