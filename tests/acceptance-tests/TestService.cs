﻿/*
 * Copyright 2014 Splunk, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"): you may
 * not use this file except in compliance with the License. You may obtain
 * a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 */

namespace Splunk.Sdk
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    [TestClass]
    public class TestService
    {
        [TestMethod]
        public void Construct()
        {
            service = new Service(new Context(Scheme.Https, "localhost", 8089), Namespace.Default);
            Assert.AreEqual(service.ToString(), "https://localhost:8089/services");
        }

        [TestMethod]
        public void Login()
        {
            Task task;
            
            task = service.LoginAsync("admin", "changeme");
            task.Wait();

            Assert.AreEqual(task.Status, TaskStatus.RanToCompletion);
            Assert.IsNotNull(service.Context.SessionKey);

            try
            {
                task = service.LoginAsync("admin", "bad-password");
                task.Wait();
            }
            catch (Exception)
            {
                Assert.AreEqual(task.Status, TaskStatus.Faulted);
                Assert.IsInstanceOfType(task.Exception, typeof(AggregateException));

                var aggregateException = (AggregateException)task.Exception;
                Assert.AreEqual(aggregateException.InnerExceptions.Count, 1);
                Assert.IsInstanceOfType(aggregateException.InnerExceptions[0], typeof(RequestException));

                var requestException = (RequestException)(aggregateException.InnerExceptions[0]);
                Assert.AreEqual(requestException.StatusCode, HttpStatusCode.Unauthorized);
                Assert.AreEqual(requestException.Details.Count, 1);
                Assert.AreEqual(requestException.Details[0], new Message(XElement.Parse(@"<msg type=""WARN"">Login failed</msg>")));
            }
        }

        [TestMethod]
        public void Search()
        {
            Task<Job> task;

            task = service.SearchAsync("index=_internal | head 10");
            task.Wait();
        }

        static Service service;
    }
}