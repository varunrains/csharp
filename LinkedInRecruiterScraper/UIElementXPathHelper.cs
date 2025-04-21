using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;

namespace LinkedInRecruiterScraper
{
    public static class UIElementXPathHelper
    {
        public static IWebElement GetJobPostingElement(WebDriverWait wait)
        {
            return wait.Until(d => d.FindElement(By.XPath("//div[contains(@class,'job-details-jobs-unified-top-card__job-title')]")));
        }

        public static IList<IWebElement> GetHiringManagerElement(WebDriverWait wait)
        {
            return wait.Until(d => d.FindElements(By.XPath("//div[@class='hirer-card__hirer-information']")));
        }

        public static string GetTheJobPostingLink(WebDriverWait wait, IWebElement jobLinkElement)
        {
            var link = jobLinkElement.FindElement(By.TagName("a")).GetAttribute("href");
            return $"{link?.Split('?')[0]}";
            //return completeLink;
        }

        public static string GetLocation(WebDriverWait wait)
        {
            IWebElement locationElement = wait.Until(d => d.FindElement(By.XPath($"//div[contains(@class,'job-details-jobs-unified-top-card__tertiary-description-container')]")));
            var locationSpan = locationElement.FindElement(By.TagName("span"));

            return locationSpan?.Text ??string.Empty;

        }

        public static string GetCompanyUrl(WebDriverWait wait)
        {
            IWebElement companyElement = wait.Until(d => d.FindElement(By.XPath($"//div[contains(@class,'job-details-jobs-unified-top-card__company-name')]")));
            var link = companyElement.FindElement(By.TagName("a")).GetAttribute("href");

            return link ?? string.Empty;

        }

        public static string GetJobPostingTitle(WebDriverWait wait, IWebElement jobDetailElement)
        {
            var link = jobDetailElement.FindElement(By.TagName("a"));
            return link?.Text ?? string.Empty;
        }

        public static string GetHiringManagerDetails(WebDriverWait wait, IList<IWebElement> hiringElements)
        {
            if (hiringElements.Any())
            {
                var hiringTeam = hiringElements.First();
                var poster = hiringTeam.FindElement(By.TagName("a"));
                var linkedInUrl = poster.GetAttribute("href");

                return linkedInUrl ?? string.Empty;
            }

            return string.Empty;
        }

        public static string GetDegreeConnection(WebDriverWait wait, IList<IWebElement> hiringElements)
        {
            if (hiringElements.Any())
            {
                var connectionDegree = wait.Until(d => d.FindElement(By.XPath("//span[@class='hirer-card__connection-degree']")));
                Console.WriteLine($"Connection Degree {connectionDegree?.Text}");
                return connectionDegree?.Text ?? string.Empty;
            }
            //if (connectionDegree?.Text != "1st")
            //{
            //    return true;
            //}

             return string.Empty;
        }

        public static string GetTheJobPostingContent(WebDriverWait wait)
        {
            var jobRoleDescriptionElement = wait.Until(d => d.FindElement(By.XPath("//article[contains(@class, 'jobs-description__container')]")));
            return jobRoleDescriptionElement?.Text ?? string.Empty;
        }

        //public static void WaitForCssClass(IWebDriver driver, By locator, string cssClass, TimeSpan timeout)
        //{
        //    WebDriverWait wait = new WebDriverWait(driver, timeout);
        //    wait.Until(driver =>
        //    {
        //        IWebElement element = driver.FindElement(locator);
        //        return element.GetAttribute("class")?.Contains(cssClass);
        //    });
        //}

        public static void NavigateToJobsSectionAndSearch(WebDriverWait wait)
        {
            IWebElement jobs = wait.Until(d => d.FindElement(By.XPath("//a[@href='https://www.linkedin.com/jobs/?']")));
            jobs.Click();

            wait.Until(d => d.Url.Contains("/jobs/"));

            IWebElement searchInput = wait.Until(d => d.FindElement(By.XPath("//input[contains(@id,'jobs-search-box-keyword-id')]")));
            searchInput.SendKeys(".net" + Keys.Enter);

            Thread.Sleep(3000);
        }

        public static void NavigateToNextJobPaginatedPosting(WebDriverWait wait, int counter)
        {
            try
            {
                IWebElement nextPage = wait.Until(d => d.FindElement(By.XPath($"//button[@aria-label='Page {counter}']")));
                nextPage.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation failed for {counter} Please check the UI if the page number exists {ex}");
            }
        }

        public static void SignInToLinkedIn(WebDriverWait wait)
        {
            var userDetails = File.ReadAllLines($".env");
            IWebElement signIn = wait.Until(d => d.FindElement(By.LinkText("Sign in")));
            signIn.Click();

            // Find username field and enter username
            IWebElement usernameField = wait.Until(d => d.FindElement(By.Id("username")));
            usernameField.SendKeys(userDetails[0]);

            // Find password field and enter password
            IWebElement passwordField = wait.Until(d => d.FindElement(By.Id("password")));
            passwordField.SendKeys(userDetails[1]);

            // Find and click login button

            IWebElement loginButton = wait.Until(d => d.FindElement(By.XPath("//button[@type='submit']")));
            loginButton.Click();

            Thread.Sleep(1000);
        }
    }
}
