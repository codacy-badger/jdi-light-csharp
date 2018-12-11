﻿using System;
using System.Collections.Generic;
using System.Linq;
using JDI.Light.Extensions;
using JDI.Light.Interfaces;

namespace JDI.Light.Utils
{
    public class BaseAsserter : IAssert
    {
        private ILogger _logger;

        public ILogger Logger
        {
            get => _logger ?? (_logger = JDI.Logger);
            set => _logger = value;
        }

        private readonly string _checkMessage;

        public BaseAsserter(string checkMessage) : this()
        {
            _checkMessage = GetCheckMessage(checkMessage);
        }

        public BaseAsserter()
        {
        }

        public virtual void ThrowFail(string message)
        {
        }

        public void ThrowFail(string message, Exception ex)
        {
            throw new Exception(message, ex);
        }

        public Exception Exception(string message)
        {
            throw new Exception(message);
        }

        private string GetCheckMessage(string checkMessage)
        {
            if (string.IsNullOrEmpty(checkMessage)) return string.Empty;
            var firstWord = checkMessage.Split(' ')[0];
            if (firstWord.Contains("check", StringComparison.OrdinalIgnoreCase) ||
                firstWord.Contains("verify", StringComparison.OrdinalIgnoreCase))
                return checkMessage;
            return "Check that " + checkMessage;
        }

        public void Contains(string actual, string expected)
        {
            Contains(actual, expected, false);
        }

        public void Contains(string actual, string expected, bool logOnlyFail)
        {
            Contains(actual, expected, logOnlyFail, null);
        }

        public void Contains(string actual, string expected, bool logOnlyFail, string failMessage)
        {
            var result = actual.Contains(expected);
            AssertAction($"Check that '{actual}' contains '{expected}'", result, logOnlyFail);
        }

        private void AssertAction(string message, bool result, bool logOnlyFail = false, string failMessage = null)
        {
            if (!logOnlyFail) Logger.Info(GetBeforeMessage(message));
            // TODO: Take screenshot
            //TakeScreenshot();
            if (!result) AssertException(failMessage ?? message + " failed");
        }

        private string GetBeforeMessage(string message)
        {
            return !string.IsNullOrEmpty(_checkMessage) ? _checkMessage : message;
        }

        private void AssertException(string failMessage, params object[] args)
        {
            var failMsg = args.Length > 0 ? string.Format(failMessage, args) : failMessage;
            Logger.Error(failMsg);
            ThrowFail(failMsg);
        }

        public void IsTrue(bool condition, string message = "")
        {
            var msg = string.IsNullOrEmpty(message) ? "" : $": {message}";
            AssertAction($"Check that condition is true{msg}", condition);
        }

        public void IsFalse(bool condition, string message = "")
        {
            var msg = string.IsNullOrEmpty(message) ? "" : $": {message}";
            AssertAction($"Check that condition is false{msg}", !condition);
        }

        public void AreEquals<T>(T actual, T expected, bool logOnlyFail = false)
        {
            var result = typeof(T) == typeof(string) ? actual.ToString().Equals(expected.ToString()) : actual.Equals(expected);
            AssertAction($"Check that '{actual}' equals to '{expected}'", result, logOnlyFail);
        }

        public void CollectionEquals<T>(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            var first = actual as T[] ?? actual.ToArray();
            var second = expected as T[] ?? expected.ToArray();
            var result = first.OrderBy(i => i).SequenceEqual(second.OrderBy(i => i));
            AssertAction($"Check that collection '{string.Join(", ", first)}' equals to '{string.Join(", ", second)}'",
                result);
        }
        
        public void HasNoException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                AssertException($"Action throws exception: {e.GetType()} - {e.Message}");
            }
        }
    }
}