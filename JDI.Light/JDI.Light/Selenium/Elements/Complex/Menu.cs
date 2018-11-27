﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using JDI.Light.Extensions;
using JDI.Light.Interfaces.Complex;
using JDI.Light.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace JDI.Light.Selenium.Elements.Complex
{
    public class Menu : Menu<IConvertible>, IMenu
    {
    }

    public class Menu<TEnum> : Selector<TEnum>, IMenu<TEnum>
        where TEnum : IConvertible
    {
        protected Action<Menu<TEnum>, string[], Action<IWebDriver, IWebElement>> ChooseItemAction =
            (m, names, action) =>
            {
                var nodes = m.SplitToList(names, m.Separator);
                if (m.MenuLevelsLocators.Count == 0 && m.HasLocator)
                    m.MenuLevelsLocators.Add(m.Locator);
                if (m.MenuLevelsLocators.Count < nodes.Count) return;
                for (var i = 0; i < nodes.Count; i++)
                {
                    var value = nodes[i];
                    var elements = new Selector(m.MenuLevelsLocators[i]).Elements;
                    var element = elements.FirstOrDefault(el => el.Text.Equals(value));
                    if (element == null)
                        throw JDISettings.Asserter.Exception("Can't choose element:" + value);
                    if (m.Delay > 0) Thread.Sleep(m.Delay);
                    action(m.WebDriver, element);
                }
            };

        public int Delay = 200;

        public Action<IWebDriver, IWebElement> HoverAction = (driver, el) =>
        {
            var action = new Actions(driver);
            action.MoveToElement(el).ClickAndHold().Build().Perform();
        };

        protected Action<Menu<TEnum>, string[]> HoverAndClickAction = (m, names) =>
        {
            if (names == null || names.Length == 0)
                return;
            var split = m.SplitToList(names, m.Separator);
            if (split.Count == 1)
            {
                if (m.Delay > 0) Thread.Sleep(m.Delay);
                m.Select(m.Locator, split[0]);
                return;
            }

            if (split.Count > m.MenuLevelsLocators.Count)
                throw JDISettings.Asserter.Exception(
                    $"Can't hover and click on element ({m}) by value: {names.FormattedJoin(m.Separator)}. " +
                    $"Amount of locators ({m.MenuLevelsLocators.Count}) less than select path length ({split.Count})");
            m.Hover(split.ListCopy(to: -1).ToArray());
            var lastIndex = split.Count - 1;
            if (m.Delay > 0) Thread.Sleep(m.Delay);
            m.Select(m.MenuLevelsLocators[lastIndex], split[lastIndex]);
        };

        public List<By> MenuLevelsLocators = new List<By>();
        public string Separator = "\\>";

        public Menu()
        {
            SelectNameAction = (m, name) => ChooseItemAction(this, new[] {name}, (w, el) => el.Click());
        }

        public Menu(By optionsNamesLocatorTemplate)
            : base(optionsNamesLocatorTemplate)
        {
            MenuLevelsLocators.Add(optionsNamesLocatorTemplate);
        }

        public Menu(By optionsNamesLocatorTemplate, By allOptionsNamesLocator)
            : base(optionsNamesLocatorTemplate, allOptionsNamesLocator)
        {
            MenuLevelsLocators.Add(optionsNamesLocatorTemplate);
        }

        public Menu(List<By> menuLevelsLocators)
        {
            MenuLevelsLocators = menuLevelsLocators;
        }

        public void Hover(params string[] names)
        {
            if (names == null || names.Length == 0)
                return;
            Actions.Hover(names.FormattedJoin(Separator), (w, n) =>
            {
                var m = (Menu<TEnum>) w;
                m.ChooseItemAction(m, names, HoverAction);
            });
        }

        public void Hover(TEnum name)
        {
            Hover(name.ToString());
        }

        public void Select(params string[] names)
        {
            HoverAndClick(names);
        }

        public void HoverAndClick(params string[] names)
        {
            Actions.Select(names.FormattedJoin(Separator), (m, n) => HoverAndClickAction(this, names));
        }

        public void HoverAndClick(TEnum name)
        {
            HoverAndClick(name.ToString());
        }

        public void HoverAndSelect(params string[] name)
        {
            HoverAndClick(name);
        }

        public void HoverAndSelect(TEnum name)
        {
            HoverAndSelect(name.ToString());
        }

        public Menu<TEnum> UseSeparator(string separator)
        {
            Separator = separator;
            return this;
        }

        private IList<string> SplitToList(string[] str, string separator)
        {
            return (str.Length == 1
                ? Regex.Split(str[0], separator)
                : str).ToList();
        }

        private void Select(By locator, string name)
        {
            new Selector(locator).Select(name);
        }

        public void SetUp(List<By> menuLevelsLocators)
        {
            MenuLevelsLocators = menuLevelsLocators;
        }
    }
}