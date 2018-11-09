﻿using JDI.Core.Interfaces.Common;
using JDI.Web.Attributes;
using JDI.Web.Selenium.Elements.Composite;

namespace JDI.UIWebTests.UIObjects.Sections
{
    public class JdiPaginator : Pagination
    {
        [FindBy(Css = "[class=first] a")] public new IButton First;

        [FindBy(Css = "[class=last]  a")] public new IButton Last;

        [FindBy(Css = "[class=next]  a")] public new IButton Next;

        [FindBy(Css = ".uui-pagination li")] public IButton Page;

        [FindBy(Css = "[class=prev]  a")] public IButton Prev;
    }
}