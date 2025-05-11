using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V132.IndexedDB;
using OpenQA.Selenium.Support.UI;

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

             return string.Empty;
        }

        public static string GetTheJobPostingContent(WebDriverWait wait)
        {
            var jobRoleDescriptionElement = wait.Until(d => d.FindElement(By.XPath("//article[contains(@class, 'jobs-description__container')]")));
            return jobRoleDescriptionElement?.Text ?? string.Empty;
        }


        public static void NavigateToJobsSectionAndSearch(WebDriverWait wait, string techToSearch, string country)
        {
            IWebElement jobs = wait.Until(d => d.FindElement(By.XPath("//a[@href='https://www.linkedin.com/jobs/?']")));
            jobs.Click();

            wait.Until(d => d.Url.Contains("/jobs/"));

            IWebElement searchTech = wait.Until(d => d.FindElement(By.XPath("//input[contains(@id,'jobs-search-box-keyword-id')]")));
            searchTech.SendKeys(techToSearch);


            IWebElement searchCountry = wait.Until(d => d.FindElement(By.XPath("//input[contains(@id,'jobs-search-box-location-id')]")));
            searchCountry.Clear();
            searchCountry.SendKeys(country+ Keys.Enter);

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

        public static void NavigateToConfiguredRecentJobSection(WebDriverWait wait, string datePosted)
        {
            try
            {
                IWebElement recentJobSection = wait.Until(d => d.FindElement(By.XPath($"//button[@id=\"searchFilter_timePostedRange\"]")));
                recentJobSection.Click();

                Thread.Sleep(1000);
                //Find the configurable past month text in the span and click the show button
                //var recentJobDivPopup = wait.Until(d => d.FindElements(By.XPath($"//input[@name=\"date-posted-filter-value\"]")));
                //The for label value is for past month 
                //To change this we might have to find the values specifically.
                //TODO:: This can be optimized in a better way
                var recentJobDivPopup = wait.Until(d => d.FindElement(By.XPath($"//label[@for=\"{datePosted}\"]")));
                //var locationSpan = wait.Until(d => recentJobDivPopup.FindElements(By.ClassName("date-posted-filter-value")));
                //Change this to show PastMonth, Past week , last 24hrs and anytime
                //  var leb = recentJobDivPopup[1];
                //  locationSpan.ElementAt(1).Click();
                recentJobDivPopup.Click();
                Thread.Sleep(4000);

                IWebElement showResultsButton = wait.Until(d => d.FindElement(By.XPath($"//button[@aria-label='Cancel Date posted filter']/following::button[1]")));
                showResultsButton.Click();
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation failed when navingating to the recent job section which is configured. {ex}");
                throw;
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
