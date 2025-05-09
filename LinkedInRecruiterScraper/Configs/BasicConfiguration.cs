namespace LinkedInRecruiterScraper.Configs
{
    public class BasicConfiguration
    {
        public required string SqlDbPath { get; set; }

        public required string ChromeDriverPath { get; set; }

        public int DatePosted { get; set; }

        public int NumberOfPagesToNavigate { get; set; }

        public bool RunInHeadlessMode { get; set; } = false;
    }
}
