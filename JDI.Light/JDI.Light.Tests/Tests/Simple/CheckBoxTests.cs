﻿using JDI.Light.Tests.DataProviders;
using NUnit.Framework;
using static JDI.Light.Elements.Base.BaseValidation;
using static JDI.Light.Matchers.StringMatchers.ContainsStringMatcher;
using static JDI.Light.Matchers.StringMatchers.EqualToIgnoringCaseMatcher;

namespace JDI.Light.Tests.Tests.Simple
{
    [TestFixture]
    public class CheckBoxTests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            Jdi.Logger.Info("Navigating to Metals and Colors page.");
            TestSite.MetalsColorsPage.Open();
            TestSite.MetalsColorsPage.CheckUrl();
            TestSite.MetalsColorsPage.CheckTitle();
            Jdi.Logger.Info("Setup method finished");
            Jdi.Logger.Info("Start test: " + TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void CheckSingleTest()
        {
            Assert.DoesNotThrow(() => TestSite.MetalsColorsPage.CbWater.Check());
            Jdi.Assert.Contains(TestSite.ActionsLog.Texts[0], "Water: condition changed to true");
        }

        [Test]
        public void UncheckSingleTest()
        {
            TestSite.MetalsColorsPage.CbWater.Click();
            TestSite.MetalsColorsPage.CbWater.Uncheck();
            Jdi.Assert.Contains(TestSite.ActionsLog.Texts[0], "Water: condition changed to false");
        }

        [Test]
        public void IsCheckTest()
        {
            Assert.IsFalse(TestSite.MetalsColorsPage.CbWater.IsChecked);
            TestSite.MetalsColorsPage.CbWater.Click();
            Assert.IsTrue(TestSite.MetalsColorsPage.CbWater.IsChecked);
        }

        [Test]
        public void MultipleUncheckTest()
        {
            TestSite.MetalsColorsPage.CbWater.Click();
            TestSite.MetalsColorsPage.CbWater.Uncheck();
            TestSite.MetalsColorsPage.CbWater.Uncheck();
            Jdi.Assert.Contains(TestSite.ActionsLog.Texts[0], "Water: condition changed to false");
        }

        [Test]
        public void ClickTest()
        {
            TestSite.MetalsColorsPage.CbWater.Click();
            Jdi.Assert.Contains(TestSite.ActionsLog.Texts[0], "Water: condition changed to true");
            TestSite.MetalsColorsPage.CbWater.Click();
            var texts = TestSite.ActionsLog.Texts;
            Jdi.Assert.Contains(texts[0], "Water: condition changed to false");
        }

        [Test]
        [TestCaseSource(typeof(CheckBoxProvider), nameof(CheckBoxProvider.InputData))]
        public void SetValueTest(bool value, bool expected)
        {
            if (!expected)
            {
                TestSite.MetalsColorsPage.CbWater.Click();
            }
            TestSite.MetalsColorsPage.CbWater.Value = value;
            var resultMsg = "Water: condition changed to " + expected.ToString().ToLower();
            Jdi.Assert.Contains(TestSite.ActionsLog.Texts[0], resultMsg);
        }

        [Test]
        public void IsValidationTest()
        {
            TestSite.Html5Page.Open();
            TestSite.Html5Page.AcceptConditions.Is.Selected();
            TestSite.Html5Page.AcceptConditions.Click();
            TestSite.Html5Page.AcceptConditions.Is.Deselected();
            TestSite.Html5Page.AcceptConditions.Is.Enabled();
            TestSite.Html5Page.AcceptConditions.Is.Displayed();
        }

        [Test]
        public void LabelTest()
        {
            TestSite.Html5Page.Open();
            Assert.AreEqual("Accept terms and conditions", TestSite.Html5Page.AcceptConditions.Label().GetText());
            TestSite.Html5Page.AcceptConditions.Label().Is.Text(ContainsString("terms and conditions"));
            TestSite.Html5Page.AcceptConditions.Label().Is.Text(EqualTo("accept terms and conditions"));
        }

        [Test]
        public void AssertValidationTest()
        {
            TestSite.Html5Page.Open();
            TestSite.Html5Page.AcceptConditions.AssertThat.Selected();
        }

        [Test]
        public void BaseValidationTest()
        {
            TestSite.Html5Page.Open();
            BaseElementValidation(TestSite.Html5Page.AcceptConditions);
        }
    }
}
