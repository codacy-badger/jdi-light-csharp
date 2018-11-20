﻿using JDI.Core.Interfaces.Common;
using JDI.Core.Selenium.Elements.Base;
using OpenQA.Selenium;

namespace JDI.Core.Selenium.Elements.Common
{
    public class Button : ClickableText, IButton
    {
        public Button(By byLocator = null, IWebElement webElement = null)
            : base(byLocator, webElement)
        {
        }
    }
}