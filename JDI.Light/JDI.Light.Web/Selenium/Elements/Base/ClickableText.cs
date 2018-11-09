﻿using System;
using JDI.Core.Interfaces.Common;
using JDI.Web.Selenium.Base;
using OpenQA.Selenium;

namespace JDI.Web.Selenium.Elements.Base
{
    public class ClickableText : Clickable, IText
    {
        protected Func<WebBaseElement, string> GetTextFunc =
            el =>
            {
                var getText = el.WebElement.Text ?? "";
                if (!getText.Equals(""))
                    return getText;
                var getValue = el.WebElement.GetAttribute("value");
                return getValue ?? getText;
            };

        public ClickableText() : this(null)
        {
        }

        public ClickableText(By byLocator = null, IWebElement webElement = null, WebBaseElement element = null)
            : base(byLocator, webElement, element)
        {
        }

        public string Value => Actions.GetValue(GetTextFunc);
        public string GetValue()
        {
            return Value;
        }

        public string GetText => Actions.GetText(GetTextFunc);

        public string WaitText(string text)
        {
            return Actions.WaitText(text, GetTextFunc);
        }

        public string WaitMatchText(string regEx)
        {
            return Actions.WaitMatchText(regEx, GetTextFunc);
        }
    }
}