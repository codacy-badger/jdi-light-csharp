﻿using JDI.Light.Attributes;
using JDI.Light.Elements.Common;
using JDI.Light.Elements.Composite;
using JDI.Light.Interfaces.Common;
using JDI.Light.Tests.UIObjects.Forms;

namespace JDI.Light.Tests.UIObjects.Pages
{
    public class ContactPage : WebPage
    {
        [FindBy(Css = "main form")]
        public ContactForm ContactForm;

        [FindBy(Css = "main form")]
        public ContactFormTwoButtons ContactFormTwoButtons;

        [FindBy(XPath = "//*[text()='Submit']")]
        public IButton ContactSubmit;

        [FindBy(Id = "description")]
        public TextArea DescriptionField;

        [FindBy(Id = "last-name")]
        public ITextField LastNameField;

        [FindBy(Id = "name")]
        public TextField NameField;

        [FindBy(Css = ".results")]
        public IText Result;
        
        public void FillAndSubmitForm(string firstName, string secondName, string description)
        {
            FillForm(firstName, secondName, description);
            ContactSubmit.Click();
        }
        
        private void FillForm(string firstName, string secondName, string description)
        {
            NameField.NewInput(firstName);
            LastNameField.NewInput(secondName);
            DescriptionField.NewInput(description);
        }
    }
}