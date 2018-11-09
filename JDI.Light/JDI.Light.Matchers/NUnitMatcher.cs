﻿using System;
using JDI.Core;
using JDI.Core.Settings;
using NUnit.Framework;

namespace JDI.Matchers
{
    public class NUnitMatcher : IAssert
    {
        public Exception Exception(string message, Exception ex)
        {
            JDISettings.Logger.Exception(ex);
            return ex;
        }

        public Exception Exception(string message)
        {
            JDISettings.Logger.Error(message);
            Assert.Fail(message);
            return new Exception(message);
        }

        public void IsTrue(bool actual)
        {
            Assert.IsTrue(actual);
        }

        public void AreEqual<T>(T actual, T expected)
        {
            Assert.AreEqual(actual, expected);
        }

        public void Matches(string actual, string regEx)
        {
            IsTrue(actual.Matches(regEx));
        }

        public void Contains(string actual, string expected)
        {
            IsTrue(actual.Contains(expected));
        }

        public void IsTrue(Func<bool> actual)
        {
            Assert.IsTrue(actual.ForceDone());
        }
    }
}