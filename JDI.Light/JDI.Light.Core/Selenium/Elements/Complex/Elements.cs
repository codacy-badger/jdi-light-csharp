﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JDI.Core.Selenium.Base;
using JDI.Core.Settings;
using JDI.Core.Utils;
using OpenQA.Selenium;

namespace JDI.Core.Selenium.Elements.Complex
{
    public class Elements<T> : WebBaseElement, IList<T> where T : WebBaseElement
    {
        private IList<T> _elements;

        public Elements() : this(null)
        {
        }

        public Elements(By byLocator = null, List<IWebElement> webElements = null)
            : base(byLocator, webElements: webElements)
        {
        }

        private IList<T> ListOfElements
        {
            get
            {
                if (JDISettings.UseCache && _elements != null && _elements.Any())
                    return _elements;
                return _elements = WebAvatar.SearchAll().WebElements
                    .Select(el => ExceptionUtils.ActionWithException(
                        () =>
                        {
                            var element = (T) Activator.CreateInstance(typeof(T), el);
                            element.WebElement = el;
                            element.Parent = this;
                            new WebCascadeInit().InitElements(element, Avatar.DriverName);
                            return element;
                        },
                        ex => "Can't instantiate list element")
                    ).ToList();
            }
        }


        public IEnumerator<T> GetEnumerator()
        {
            return ListOfElements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            // Not applicable
        }

        public void Clear()
        {
            // Not applicable
        }

        public bool Contains(T item)
        {
            return ListOfElements.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ListOfElements.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return false;
            // Not applicable
        }

        public int Count => ListOfElements.Count;
        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            return ListOfElements.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            // Not applicable
        }

        public void RemoveAt(int index)
        {
            // Not applicable
        }

        public T this[int index]
        {
            get => ListOfElements[index];
            set
            {
                /* Not applicable*/
            }
        }
    }
}