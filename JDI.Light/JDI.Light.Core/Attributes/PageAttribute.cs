﻿using System;
using System.Collections.Generic;
using System.Reflection;
using JDI.Core.Selenium.Elements.Composite;
using JDI.Core.Settings;

namespace JDI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class PageAttribute : Attribute
    {
        public CheckPageTypes CheckType = CheckPageTypes.None;
        public string Title = "";
        public CheckPageTypes TitleCheckType = CheckPageTypes.None;
        public string Url = "";
        public CheckPageTypes UrlCheckType = CheckPageTypes.None;
        public Dictionary<string, string> UrlParams;
        public string UrlTemplate = "";

        public static PageAttribute Handler(FieldInfo field)
        {
            return field.GetCustomAttribute<PageAttribute>(false);
        }

        public static PageAttribute Handler(object obj)
        {
            return obj.GetType().GetCustomAttribute<PageAttribute>(false);
        }

        public void FillPage(WebPage page, Type parentClass)
        {
            var url = Url;
            var site = SiteAttribute.Get(parentClass);
            if (!WebSettings.HasDomain && parentClass != null && site != null)
                WebSettings.Domain = site.Domain;
            url = url.Contains("://") || parentClass == null || !WebSettings.HasDomain
                ? url
                : WebPage.GetUrlFromUri(url);
            var title = Title;
            var urlTemplate = UrlTemplate;
            var urlCheckType = UrlCheckType;
            var titleCheckType = TitleCheckType;
            if (!string.IsNullOrEmpty(urlTemplate))
                urlTemplate = urlTemplate.Contains("://") || parentClass == null || !WebSettings.HasDomain ||
                              urlCheckType != CheckPageTypes.Match
                    ? urlTemplate
                    : WebPage.GetMatchFromDomain(urlTemplate);
            page.UpdatePageData(url, title, urlCheckType, titleCheckType, urlTemplate);
        }
    }
}