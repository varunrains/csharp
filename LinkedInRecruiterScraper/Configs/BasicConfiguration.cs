namespace LinkedInRecruiterScraper.Configs
{
    public record BasicConfiguration
    {
        public required string SqlDbPath { get; set; }
        public required string ChromeDriverPath { get; set; }
        public required string TechnologyToSearch { get; set; }
        public required string CountryToSearch { get; set; }
        public int DatePosted { get; set; }
        public int NumberOfPagesToNavigate { get; set; }
        public bool RunInHeadlessMode { get; set; } = false;
    }
}
