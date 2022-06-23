﻿using OpenQA.Selenium;
using System;
using Unicorn.Taf.Core.Utility;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Core.Synchronization.Conditions;
using Unicorn.UI.Web.Controls.Dynamic;
using Unicorn.UI.Web.PageObject;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Unicorn.UnitTests.UI.Gui.Web
{
    [PageInfo("https://jqueryui.com/dialog/#modal-confirmation")]
    public class JqueryDialogPage : WebPage
    {
        public JqueryDialogPage(IWebDriver driver) : base(driver)
        {
        }

        [Find(Using.WebCss, "[aria-describedby = 'dialog-confirm']")]
        [DefineDialog(DialogElement.Title, Using.WebCss, ".ui-dialog-title")]
        [DefineDialog(DialogElement.Content, Using.Id, "dialog-confirm")]
        [DefineDialog(DialogElement.Accept, Using.WebXpath, ".//button[. = 'Delete all items']")]
        [DefineDialog(DialogElement.Decline, Using.WebXpath, ".//button[. = 'Cancel']")]
        [DefineDialog(DialogElement.Close, Using.WebCss, ".ui-dialog-titlebar-close")]
        public DynamicDialog Dialog { get; set; }

        [Find(Using.WebCss, ".demo-frame")]
        public DynamicDialog Frame { get; set; }
        
        public void WaitForLoading()
        {
            Frame.Wait(Until.Visible, TimeSpan.FromSeconds(10));
            (SearchContext as IWebDriver).SwitchTo().Frame(Frame.Instance);
            Dialog.Wait(Until.Visible);
        }
    }
}