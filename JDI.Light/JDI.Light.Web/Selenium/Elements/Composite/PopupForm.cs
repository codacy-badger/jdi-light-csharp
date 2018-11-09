﻿using System;
using System.Collections.Generic;
using JDI.Core.Attributes.Functions;
using JDI.Core.Interfaces.Complex;
using JDI.Matchers;

namespace JDI.Web.Selenium.Elements.Composite
{
    public class PopupForm<T> : Form<T>, IPopup
    {
        public Func<PopupForm<T>, string> GetTextAction = pf => pf.WebElement.Text;

        public void Ok()
        {
            GetElementClass.GetButton(Functions.Ok).Click();
        }

        public void Cancel()
        {
            GetElementClass.GetButton(Functions.Cancel).Click();
        }

        public void Close()
        {
            GetElementClass.GetButton(Functions.Close).Click();
        }

        public string GetText => Invoker.DoJActionResult("Get text", pf => ((PopupForm<T>) pf).GetTextAction(this));

        public string WaitText(string text)
        {
            return Invoker.DoJActionResult($"Wait text contains '{text}'",
                pf => Timer.GetResultByCondition(() => GetTextAction(this), t => t.Contains(text)));
        }

        public string WaitMatchText(string regEx)
        {
            return Invoker.DoJActionResult($"Wait text match regex '{regEx}'",
                pf => Timer.GetResultByCondition(() => GetTextAction(this), t => t.Matches(regEx)));
        }

        public new void Submit(Dictionary<string, string> objStrings)
        {
            Fill(objStrings);
            Ok();
        }
    }
}