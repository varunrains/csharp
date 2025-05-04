using LinkedInRecruiterScraper;
using LinkedInRecruiterScraper.Data;
using LinkedInRecruiterScraper.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


internal class Program
{
    private static void Main(string[] args)
    {
        string chromeDriverPath = @"F:\Softwares\WebDrivers\";
        string pathForDB = @"C:\temp\LinkedInJobs.db";
        var sqlLiteDB = new SQLLiteRepository(pathForDB);

        // Create Chrome options
        ChromeOptions options = new ChromeOptions();

        // Optional: Run in headless mode (uncomment if needed)
       //  options.AddArgument("--headless");

        // Initialize ChromeDriver
        using (IWebDriver driver = new ChromeDriver(chromeDriverPath, options))
        {
            try
            {
                // Navigate to LinkedIn
                driver.Navigate().GoToUrl("https://www.linkedin.com");
                

                // Wait for page to load
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                UIElementXPathHelper.SignInToLinkedIn(wait);

                UIElementXPathHelper.NavigateToJobsSectionAndSearch(wait);

                UIElementXPathHelper.NavigateToConfiguredRecentJobSection(wait);

                int numberOfPagesToNavigate = 30;

                for (int i = 2; i <= numberOfPagesToNavigate; i++)
                {
                    
                    IList<IWebElement> listItems = wait.Until(d => d.FindElements(By.XPath("//li[@data-occludable-job-id]")));
                    foreach (IWebElement listItem in listItems)
                    {
                        try
                        {
                            listItem.Click();
                            Thread.Sleep(2000);

                            var jobElement = UIElementXPathHelper.GetJobPostingElement(wait);
                            var hiringManagerElements = UIElementXPathHelper.GetHiringManagerElement(wait);
                            var hiringManagerLink = UIElementXPathHelper.GetHiringManagerDetails(wait, hiringManagerElements);

                            var jobRecord = new JobRecord()
                            {
                                Title = UIElementXPathHelper.GetJobPostingTitle(wait, jobElement),
                                Company = UIElementXPathHelper.GetCompanyUrl(wait),
                                Location = UIElementXPathHelper.GetLocation(wait),
                                JobDescription = UIElementXPathHelper.GetTheJobPostingContent(wait),
                                HiringManagerLink = hiringManagerLink,
                                JobUrl = UIElementXPathHelper.GetTheJobPostingLink(wait, jobElement),   
                            };
                           
                           sqlLiteDB.InsertJob(jobRecord);

                            var hiringManager = new HiringManager()
                            {
                                HiringManagerLink = hiringManagerLink,
                                ConnectionDegree = UIElementXPathHelper.GetDegreeConnection(wait, hiringManagerElements),
                            };

                            sqlLiteDB.InsertHiringManager(hiringManager);
                        }
                        catch (Exception ex)
                        {
                            // Handle cases where specific elements are not found
                            Console.WriteLine($"Could not extract all details for this job listing :: {ex.Message}");
                        }

                    }
                    Console.WriteLine($"Next page counter {i}");
                    UIElementXPathHelper.NavigateToNextJobPaginatedPosting(wait, i);                  
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Keep browser open to review (optional)
                Console.WriteLine("Press Enter to close the browser...");
                Console.ReadLine();
            }
        }
    }

  
}