﻿using NUnit.Framework;
using Unicorn.UI.Web;
using Unicorn.UI.Web.Driver;
using Unicorn.UnitTests.Gui.Web;
using Uv = Unicorn.Taf.Core.Verification;
using Ui = Unicorn.UI.Core.Matchers.UI;

namespace Unicorn.UnitTests.UI
{
    [TestFixture]
    public class UiMatchersWeb
    {
        [OneTimeSetUp]
        public static void Setup() =>
            WebDriver.Instance = new DesktopWebDriver(BrowserType.Chrome, true);

        [OneTimeTearDown]
        public static void TearDown() =>
            WebDriver.Close();

        public JqueryDataGridPage OpenGridPage()
        {
            JqueryDataGridPage gridPage = new JqueryDataGridPage(WebDriver.Instance.SeleniumDriver);
            WebDriver.Instance.Get(gridPage.Url);
            gridPage.WaitForLoading();

            return gridPage;
        }

        public JqueryDialogPage OpenDialogPage()
        {
            JqueryDialogPage page = new JqueryDialogPage(WebDriver.Instance.SeleniumDriver);
            WebDriver.Instance.Get(page.Url);
            page.WaitForLoading();

            return page;
        }

        public JquerySelectPage OpenSelectPage()
        {
            JquerySelectPage page = new JquerySelectPage(WebDriver.Instance.SeleniumDriver);
            WebDriver.Instance.Get(page.Url);
            page.WaitForLoading();

            return page;
        }

        public JqueryCheckboxRadioPage OpenCheckboxRadioPage()
        {
            JqueryCheckboxRadioPage page = new JqueryCheckboxRadioPage(WebDriver.Instance.SeleniumDriver);
            WebDriver.Instance.Get(page.Url);
            page.WaitForLoading();

            return page;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check WindowHasTitleMatcher")]
        public void TestWindowHasTitleMatcherMatcher()
        {
            JqueryDialogPage dialogPage = OpenDialogPage();
            Uv.Assert.That(dialogPage.Dialog, Ui.Window.HasTitle("Empty the recycle bin?"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check WindowHasTitleMatcher Negative")]
        public void TestWindowHasTitleMatcherMatcherNegative()
        {
            JqueryDialogPage dialogPage = OpenDialogPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(dialogPage.Dialog, Ui.Window.HasTitle("weeee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check ModalWindowHasTextMatcher")]
        public void TestModalWindowHasTextMatcher()
        {
            JqueryDialogPage dialogPage = OpenDialogPage();
            Uv.Assert.That(dialogPage.Dialog, Ui.Window.HasText(
                "These items will be permanently deleted and cannot be recovered. Are you sure?"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check ModalWindowHasTextMatcher Negative")]
        public void TestModalWindowHasTextMatcherNegative()
        {
            JqueryDialogPage dialogPage = OpenDialogPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(dialogPage.Dialog, Ui.Window.HasText("weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasRowsCountMatcher")]
        public void TestDataGridHasRowsCountMatcher()
        {
            JqueryDataGridPage gridPage = OpenGridPage();
            Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasRowsCount(20));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasRowsCountMatcher Negative")]
        public void TestDataGridHasRowsCountMatcherNegative()
        {
            JqueryDataGridPage gridPage = OpenGridPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasRowsCount(10));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasRowMatcher")]
        public void TestDataGridHasRowMatcher()
        {
            JqueryDataGridPage gridPage = OpenGridPage();
            Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasRow("Continent", "Asia"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasRowMatcher Negative")]
        public void TestDataGridHasRowMatcherNegative()
        {
            JqueryDataGridPage gridPage = OpenGridPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasRow("Continent", "weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasColumnMatcher")]
        public void TestDataGridHasColumnMatcher()
        {
            JqueryDataGridPage gridPage = OpenGridPage();
            Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasColumn("Name"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasColumnMatcher Negative")]
        public void TestDataGridHasColumnMatcherNegative()
        {
            JqueryDataGridPage gridPage = OpenGridPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasColumn("weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasCellWithTextMatcher1")]
        public void TestDataGridHasCellWithTextMatcher1()
        {
            JqueryDataGridPage gridPage = OpenGridPage();
            Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasCellWithText("Continent", "Europe", "Name", "Albania"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasCellWithTextMatcher1 Negative")]
        public void TestDataGridHasCellWithTextMatcher1Negative()
        {
            JqueryDataGridPage gridPage = OpenGridPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasCellWithText("Continent", "Europe", "Name", "weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasCellWithTextMatcher2")]
        public void TestDataGridHasCellWithTextMatcher2()
        {
            JqueryDataGridPage gridPage = OpenGridPage();
            Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasCellWithText(0, 1, "Aruba"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasCellWithTextMatcher2 Negative")]
        public void TestDataGridHasCellWithTextMatcher2Negative()
        {
            JqueryDataGridPage gridPage = OpenGridPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasCellWithText(0, 1, "weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DropdownHasSelectedValueMatcher")]
        public void TestDropdownHasSelectedValueMatcher()
        {
            JquerySelectPage selectPage = OpenSelectPage();
            Uv.Assert.That(selectPage.Dropdown, Ui.Dropdown.HasSelectedValue("Medium"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DropdownHasSelectedValueMatcher Negative")]
        public void TestDropdownHasSelectedValueMatcherNegative()
        {
            JquerySelectPage selectPage = OpenSelectPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(selectPage.Dropdown, Ui.Dropdown.HasSelectedValue("weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check CheckboxCheckedMatcher")]
        public void TestCheckboxCheckedMatcher()
        {
            JqueryCheckboxRadioPage cboxPage = OpenCheckboxRadioPage();
            cboxPage.JqCheckbox.JsClick();
            Uv.Assert.That(cboxPage.JqCheckbox, Ui.Checkbox.Checked());
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check CheckboxCheckedMatcher Negative")]
        public void TestCheckboxCheckedMatcherNegative()
        {
            JqueryCheckboxRadioPage cboxPage = OpenCheckboxRadioPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(cboxPage.JqCheckbox, Ui.Checkbox.Checked());
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check CheckboxHasCheckStateMatcher")]
        public void TestCheckboxHasCheckStateMatcher()
        {
            JqueryCheckboxRadioPage cboxPage = OpenCheckboxRadioPage();
            Uv.Assert.That(cboxPage.JqCheckbox, Ui.Checkbox.HasCheckState(false));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check CheckboxHasCheckStateMatcher Negative")]
        public void TestCheckboxHasCheckStateMatcherNegative()
        {
            JqueryCheckboxRadioPage cboxPage = OpenCheckboxRadioPage();
            cboxPage.JqCheckbox.JsClick();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(cboxPage.JqCheckbox, Ui.Checkbox.HasCheckState(false));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check SelectedMatcher")]
        public void TestSelectedMatcher()
        {
            JqueryCheckboxRadioPage cboxPage = OpenCheckboxRadioPage();
            cboxPage.JqRadio.JsClick();
            Uv.Assert.That(cboxPage.JqRadio, Ui.Control.Selected());
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check SelectedMatcher Negative")]
        public void TestSelectedMatcherNegative()
        {
            JqueryCheckboxRadioPage cboxPage = OpenCheckboxRadioPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(cboxPage.JqRadio, Ui.Control.Selected());
            });
        }
    }
}