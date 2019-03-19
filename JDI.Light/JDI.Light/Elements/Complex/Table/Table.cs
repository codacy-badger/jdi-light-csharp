﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JDI.Light.Asserts;
using JDI.Light.Elements.Base;
using JDI.Light.Factories;
using OpenQA.Selenium;

namespace JDI.Light.Elements.Complex.Table
{
    public class Table : UIElement
    {
        public Table(By locator) : base(locator)
        {
            TableHeadersLocator = By.XPath(".//tr/th");
            TableBodyLocator = By.XPath(".//tbody");
            TableFooterLocator = By.XPath(".//tfoot");
            TableRowsLocator = By.XPath(".//tr");
            TableCellsLocator = By.XPath(".//td");
        }

        public By TableHeadersLocator { get; set; }
        public By TableBodyLocator { get; set; }
        public By TableFooterLocator { get; set; }
        public By TableRowsLocator { get; set; }
        public By TableCellsLocator { get; set; }

        public List<UIElement> Headers => Body.FindElements(TableHeadersLocator)
            .Select(e => UIElementFactory.CreateInstance<UIElement>(TableHeadersLocator, Body, e)).ToList();
        public UIElement Body => UIElementFactory.CreateInstance<UIElement>(TableBodyLocator, this);
        public UIElement Footer => UIElementFactory.CreateInstance<UIElement>(TableFooterLocator, this);
        public UIElement[] Rows => Body.FindElements(TableRowsLocator)
            .Select(e => UIElementFactory.CreateInstance<UIElement>(TableRowsLocator, Body, e)).ToArray();

        public UIElement[][] Cells => Rows.Select(r => r.FindElements(TableCellsLocator)
            .Select(e => UIElementFactory.CreateInstance<UIElement>(TableCellsLocator, r, e)).ToArray()).ToArray();

        public Line Row(params TableMatcher[] matchers)
        {
            var lines = TableMatcher.Table_Matcher.Invoke(this, matchers);
            if (lines == null || lines.Count.Equals(0))
            {
                return null;
            }
            var result = new List<string>();
            for (var i = 0; i < Headers.Count; i++)
            {
                result.Add(lines.ElementAt(i).Text);
            }
            return Line.InitLine(result, Headers.Select(h => h.Text).ToList());
        }

        public TableAssert Is()
        {
            return new TableAssert(this);
        }

        public TableAssert AssertThat()
        {
            return Is();
        }
    }
}