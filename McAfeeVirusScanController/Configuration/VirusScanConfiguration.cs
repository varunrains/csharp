namespace McAfeeVirusScanController.Configuration
{
    public class VirusScanConfiguration
    {
        /// <summary>
        /// The full path to the folder where uploaded files should be saved before they are scanned
        /// </summary>
        public string UploadFolderPath { get; set; }
        
        /// <summary>
        /// The full path to the folder where the virus scan reports should be saved
        /// </summary>
        public string ReportFolderPath { get; set; }

        /// <summary>
        /// The full path to the scanner executable
        /// </summary>
        public string ScannerPath { get; set; }
        
        /// <summary>
        /// The command-line arguments to pass to the scanner executable
        /// </summary>
        public string ScannerArguments { get; set; }

        /// <summary>
        /// The timeout, in seconds, before the scanner process should be terminated
        /// </summary>
        public int ScannerProcessTimeoutSeconds { get; set; }
    }
}
